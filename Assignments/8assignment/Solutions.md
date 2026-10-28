# Assignment 8

10.1, 10.2 and 10.3

## Exercise 10.1

    Ints on the stack are tagged, Tag(v) = (v<<1)|1, so the last bit is always 1.
    Heap pointers are word aligned so the last bit is always 0.
    That's how the machine and the gc can tell the difference between an int and a pointer.

### (i)

#### ADD
    s[sp-1] = Tag(Untag(s[sp-1]) + Untag(s[sp])); sp--;

    Untags the two top values on the stack, adds them and tags the result again.
    The result is stored in s[sp-1] and sp is decremented, so the two values are replaced by the sum.

#### CSTI i
    s[sp+1] = Tag(p[pc++]); sp++;

    i is stored untagged in the program right after the instruction.
    The machine reads it, moves pc past it, tags it and pushes it on the stack.

#### NIL
    s[sp+1] = NILVALUE; sp++;

    Pushes 0 on the stack. It is not tagged, so it is a null reference.

    The difference from CSTI 0 is that CSTI 0 pushes Tag(0) which is 1, so it's an int.
    NIL pushes 0 which ends with a 0 bit, so it looks like a pointer.
    In the program both act like zero/false, but the gc needs to know the difference.
    NIL counts as a reference, so the gc has to check for it and not follow it, since it doesn't point to anything in the heap.

#### IFZERO
    word v = s[sp--];
    pc = (IsInt(v) ? Untag(v) == 0 : v == 0) ? p[pc] : pc+1;

    Pops the top value. If it is an int it checks if the untagged value is 0, if it is a reference it checks if it is nil.
    If the check is true it jumps to the label stored at p[pc], otherwise it skips the label and continues at pc+1.

#### CONS
    Calls allocate(CONSTAG, 2, s, sp) to get a block with 2 words plus the header.
    A gc can happen inside allocate, but the two values are still on the stack so they are roots and don't get collected.
    Then s[sp-1] is put in p[1] (car) and s[sp] in p[2] (cdr).
    The two values are replaced by the pointer to the new cons cell and sp is decremented.

#### CAR
    Takes the pointer on top of the stack. If it is null the machine prints "Cannot take car of null" and stops.
    Otherwise it replaces the pointer with p[1], the first field of the cons cell (p[0] is the header).

#### SETCAR
    word v = s[sp--];
    word* p = (word*)s[sp];
    p[1] = v;

    Pops the value v and sets the car field of the cons cell under it to v.
    The pointer to the cons cell stays on the stack.
    This is how the program can change the heap after it is allocated, which is also how you can get shared data and cycles.

### (ii)
    In microvm the header is 64 bits: 8 bit tag, 54 bit length and 2 bit color.
    ttttttttnnnn...nnnngg
    The book uses 32 bits with a 22 bit length, but it works the same way.

    Length(hdr) = (hdr>>2) & 0x003FFFFFFFFFFFFF
    Shifts the two color bits out and masks away the tag, so we get the length n.
    That's the number of words in the block, not counting the header.

    Color(hdr) = hdr & 3
    Gives the last two bits gg, so 0 = White, 1 = Grey, 2 = Black and 3 = Blue.

    Paint(hdr, color) = (hdr & 0xFFFFFFFFFFFFFFFC) | color
    Sets the two color bits to 0 and then ORs in the new color.
    The result is the same header with the same tag and length, but with the new color.

    Example: a new cons cell gets the header mkheader(0, 2, White) = 8 = 1000 in binary.
    Length(8) = 2, Color(8) = 0 = White and Paint(8, Black) = 10 = 1010 in binary.

    The comment at the top of microvm.c says black is 11, but in the code Black is defined as 2, which is 10.
    Only Blue is 11, so the comment is wrong.

### (iii)
    allocate is only called from two instructions:
    - CONS
    - HEAPALLOC, which is used for closures and objects in micro-SML and micro-Java

    Other interaction between the mutator and the gc:
    - s and sp are passed to allocate and then to collect, so the gc can use the stack as roots.
    - The tagging is an agreement between them. The gc trusts that ints end in 1 and pointers end in 0, so it never mistakes an int for a pointer. This makes the gc precise and not conservative.
    - HEAPALLOC fills the new block with Tag(0), so if a gc happens before the fields are set it doesn't find random values that look like pointers.
    - SETCAR, SETCDR etc. write directly into the heap without telling the gc. That works because the gc only runs inside allocate, and the program is stopped while it runs.

### (iv)
    collect is called from allocate when it has gone through the whole freelist without finding a block that is big enough.
    After the gc it tries one more time, and if there still is no space it prints "Out of memory" and exits.

    This can also happen when there is enough free memory in total, if it is split into a lot of small blocks (fragmentation).
    That is what 10.3 tries to fix.

    In the code we got, markPhase and sweepPhase are empty, so a program that allocates a lot just prints GC[M,BS] and then Out of memory.

## Exercise 10.2

    The code is in MicroVM/microvm.c, search for "Exercise 10.2".

    We made a small helper isHeapRef(v) which checks that v is not an int, not nil and actually points into the heap.

    markPhase goes through the whole stack s[0..sp] and calls mark on everything that is a heap reference.

    void mark(word* block) {
      if (Color(block[0]) != White)
        return;
      block[0] = Paint(block[0], Black);
      word n = Length(block[0]);
      for (word i = 1; i <= n; i++) {
        if (isHeapRef(block[i]))
          mark((word*)block[i]);
      }
    }

    mark paints the block black and then calls itself on all fields that are heap references.
    If the block is not white it is already marked, so we return.
    This is needed so it doesn't loop forever on cycles (ex36) or mark shared data twice (ex35).

    sweepPhase goes through the whole heap one block at a time. The next block is at p + Length + 1.
    - White: the block is dead, so we paint it blue and put it in front of the freelist (p[1] is the next pointer).
    - Black: the block is alive, so we paint it white again for the next gc.
    - Blue: already on the freelist or an orphan, so we skip it.

    We don't use grey, since mark is recursive and finishes a block right away.

    The heap is 20000 words and a cons cell is 3 words, so without a gc you run out after around 6600 cons cells.

    Tests:
    - ex30 with 1000000: prints 1000000 down to 1 with 150 gc's and no Out of memory.
    - ex35: prints 33 33 44 44. Also tested with makegarbage(100000) so we are sure the gc actually runs.
    - ex36: prints 1 1. Also tested with makegarbage(100000), so the cycle doesn't make the gc loop forever.
    - Our own test: a list of 6500 elements is kept alive while 50000 garbage cons cells are made, then we sum the list.
      It prints 21128250 = 6500*6501/2, so the live list survives around 300 gc's.
    - ex55.out (micro-Java queens, uses HEAPALLOC) still gives 4 solutions for n = 6.

## Exercise 10.3

    In sweepPhase, when we find a white block at p with length n, we look at the next block at p + n + 1.
    If that block is also white we join them into one block.
    The new length is n + 1 + Length(next). The +1 is because the header of the second block just becomes a normal word in the new block.
    The joined block is then put on the freelist like in 10.2.

    word* next = p + n + 1;
    if (next < afterHeap && Color(next[0]) == White) {
      n = n + 1 + Length(next[0]);
    }

    Since the loop continues from p + n + 1 with the new n, the second block is skipped and never ends up on the freelist by itself.

    Output from heapStatistics after a gc when running ex30 with 20000:

    10.2: Heap: 6667 blocks (13333 words); of which 6666 free (13331 words, largest 2 words)
    10.3: Heap: 3335 blocks (16665 words); of which 3334 free (16663 words, largest 5 words)

    So there are about half as many blocks, and the largest free block is 5 words (2 + 1 + 2) instead of 2.
    It only joins two blocks at a time. Changing the if to a while would join all of them, which is 10.4.

    ex35 and ex36 still print 33 33 44 44 and 1 1.
