Solutions: 

11.1:
Code is in Ex1.fsx file

(i)
Answer is 3
Trace of lenc [2; 5; 7] id:

lenc [2; 5; 7] id
==> lenc [5;7] (fun v -> id(1 + v))
==> lenc [7] (fun w -> (fun v -> id(1 + v)) (1 + w))
==> lenc [] (fun u -> (fun w -> (fun v -> id(1 + v)) (1 + w)) (1 + u))
==> (fun u -> (fun w -> (fun v -> id(1 + v)) (1 + w)) (1 + u)) 0
==> (fun w -> (fun v -> id(1 + v)) (1 + w)) (1 + 0)
==> (fun w -> (fun v -> id(1 + v)) (1 + w)) 1
==> (fun v -> id(1 + v)) (1 + 1)
==> (fun v -> id(1 + v)) 2
==> id(1 + 2)
==> id 3
==> 3


(ii)
The Answer will be 6

fsi:
val it : 6


(iii)
leni is in Ex1.fsx, leni [2; 5; 7] 0 gives 3.

The accumulator in leni does the same job as the continuation in lenc.
In lenc the continuation remembers "add 1 for each element" as a chain of functions,
in leni we just count it in an int right away.
So lenc xs k = k (leni xs 0), and lenc xs id = leni xs 0.
Since the continuation only ever adds 1, it can be replaced by a number, which is the accumulator.



11.2
Code is in Ex2.fsx

(i)
The code in Ex2.fsx gives [7; 5; 2]

Trace of revc [2; 5; 7] id:

revc [2;5;7] id
==> revc [5;7] (fun v -> id(v @ [2]))
==> revc [7] (fun w -> (fun v -> id(v @ [2])) (w @ [5]))
==> revc [] (fun u -> (fun w -> (fun v -> id(v @ [2])) (w @ [5])) (u @ [7]))
==> (fun u -> (fun w -> (fun v -> id(v @ [2])) (w @ [5])) (u @ [7])) []
==> (fun w -> (fun v -> id(v @ [2])) (w @ [5])) ([] @ [7])
==> (fun w -> (fun v -> id(v @ [2])) (w @ [5])) [7]
==> (fun v -> id(v @ [2])) ([7] @ [5])
==> (fun v -> id(v @ [2])) [7; 5]
==> id([7; 5] @ [2])
==> id [7; 5; 2]
==> [7; 5; 2]


(ii)
The code in Ex2.fsx gives [7; 5; 2; 7; 5; 2]


(iii)
revi is in Ex2.fsx, revi [2; 5; 7] [] gives [7; 5; 2].

Each element is put in front of the accumulator with x :: acc, so the first element ends up last.
It is tail recursive and uses :: instead of @, so it runs in linear time instead of quadratic.



11.3
Code is in Ex3.fsx

Trace of prodc [2; 5; 7] id:

prodc [2;5;7] id
==> prodc [5;7] (fun v -> id(2 * v))
==> prodc [7] (fun w -> (fun v -> id(2 * v)) (5 * w))
==> prodc [] (fun u -> (fun w -> (fun v -> id(2 * v)) (5 * w)) (7 * u))
==> (fun u -> (fun w -> (fun v -> id(2 * v)) (5 * w)) (7 * u)) 1
==> (fun w -> (fun v -> id(2 * v)) (5 * w)) (7 * 1)
==> (fun w -> (fun v -> id(2 * v)) (5 * w)) 7
==> (fun v -> id(2 * v)) (5 * 7)
==> (fun v -> id(2 * v)) 35
==> id(2 * 35)
==> id 70
==> 70



11.4
Code is in Ex4.fsx. The optimized CPS version is called prodc2.

prodc2 has an extra case 0 :: _ -> 0 which returns 0 right away without calling the continuation k.
That way all the multiplications saved up in k are skipped.
This is only correct because the continuation just multiplies the result by some factor, and 0 times anything is 0.

prodc2 [2; 5; 0; 7; 9] id gives 0
prodc2 [2; 5; 0; 7; 9] (fun v -> 2 * v) gives 0

prodi is the tail recursive version with an accumulator, called as prodi xs 1.
It also stops at the first 0 and returns 0 instead of the accumulator.
prodi [2; 5; 7] 1 gives 70 and prodi [2; 5; 0; 7; 9] 1 gives 0.

11.8
1.
3579 - >

 1 + 2 * (1 to 4)

 run (Every(Write(Prim("+", CstI 1, Prim("*", CstI 2, FromTo(1, 4))))))

21 22 31 32 41 42
// do we care about the spaces? ... oh hell nah

10 * (1 to 4) + (1 to 2)

run (Every(Write(Prim("+", Prim("*", CstI 10, FromTo(2, 4)), FromTo(1, 2)))))


2. 




7 * (1 to 10) > 50 ish will write everynumber until 50?

50 < (7 * (1 to 10) )

run (Write(Prim("<", CstI 50, Prim("*", CstI 7, FromTo(1, 10)))))



3.
Added Prim1 of string * expr to expr in Cont/Icon.fs, and a Prim1 case in eval.
It evaluates the argument and then matches on the name, like Prim but with one argument.

sqr(3 to 6) -> 9 16 25 36

run (Every(Write(Prim1("sqr", FromTo(3, 6)))))

even(1 to 7) -> 2 4 6
// odd numbers call the failure continuation, so it backtracks to (1 to 7) for the next one

run (Every(Write(Prim1("even", FromTo(1, 7)))))



4.
multiples is a loop like FromTo, just without an upper limit.
The failure continuation is "give me the next multiple", so it never fails.

multiples(3) -> 3 6 9 12 ... forever

run (Every(Write(Prim1("multiples", CstI 3))))

// to stop it, take the first multiple of 3 above 20 (no every)
20 < multiples(3) -> 21

run (Write(Prim("<", CstI 20, Prim1("multiples", CstI 3))))

multiples(3 to 4) only gives multiples of 3.
multiples never calls its failure continuation, so it never backtracks to (3 to 4) to get 4.
