# Assignment 7 Exercises

Assigned exercises: **8.5, 8.6, 9.1 and 9.3** (see `QueueWithMistake.java`).

| Exercise | Topic | Where the work happens | Done |
|---|---|---|---|
| 8.5 | Conditional expression `e1 ? e2 : e3` | `MicroC/` | ☐ |
| 8.6 | `switch` statement | `MicroC/` | ☐ |
| 9.1 | Selection sort bytecode, .NET + JVM | `Virtual/Selsort/` | ☐ |
| 9.3 | (see note below) | `QueueWithMistake.java` | ☐ |

---

## Exercise 8.5 — Conditional expressions

Extend micro-C with conditional expressions of the form `(e1 ? e2 : e3)`.

**Requirement:** `e2` is evaluated only when `e1` is true, and `e3` only when `e1`
is false. The compilation scheme is the same as for `if (e1) e2 else e3`, with one
difference: whichever branch runs must leave its value on the **stack top**, so the
whole expression leaves exactly one value there.

Four files to touch:

| File | Change |
|---|---|
| `MicroC/Absyn.fs` | New `expr` case, e.g. `Cond of expr * expr * expr` |
| `MicroC/CLex.fsl` | Tokens for `?` and `:` |
| `MicroC/CPar.fsy` | Grammar rule + precedence (`?:` binds loosely, right-associative) |
| `MicroC/Comp.fs` | New case in `cExpr` |

Note that `:` may already be in use in the lexer/parser — check before adding.

The existing `cStmt` case for `If` is the model to copy: two labels, a `IFZERO`
jump to the else-branch, and a `GOTO` past it. The only change is that both arms
push a value instead of leaving the stack unchanged.

## Exercise 8.6 — switch statements

Implement `switch` statements like:

```c
switch (month) {
  case 1: { days = 31; }
  case 2: { days = 28; if (y%4==0) days = 29; }
  case 3: { days = 31; }
}
```

**Constraints** (simplifications vs real C):

- **No fall-through** — after the last statement of a case, jump to the end of the switch
- The parentheses after `switch` must contain an expression
- A `case` value must be an **integer constant**
- A `case` must be followed by a **statement block**
- Do **not** implement `break` or `default`

**Compilation hint from the book:** a switch with *n* cases compiles using *n* labels,
the last of which sits at the very end of the switch.

Same four files as 8.5 — but this is a *statement*, so the abstract syntax case goes
on `stmt` and the compiler case goes in `cStmt` rather than `cExpr`.

## Exercise 9.1 — Selection sort bytecode

Compile and disassemble `SelectionSort` in both C# and Java, then annotate the
bytecode with comments explaining each instruction's purpose. Also map the bytecode
locals (local 0, 1, …) back to the source variables.

Source files are already present in `Virtual/Selsort/`: `Selsort.cs` and `Selsort.java`.

**Deliverable:** the two edited bytecode files, with your comments. Strip everything
except the `SelectionSort` method body.

### (ii) Java — works out of the box

JDK 24 is installed, so:

```bash
cd Virtual/Selsort
javac Selsort.java
javap -verbose -c Selsort > Selsort.jvmbytecode
```

Reference for JVM instruction semantics: the JVM Specification, chapter 6.

### (i) C# — toolchain gap, needs a decision

The book says to use a Visual Studio Command Prompt:

```bash
csc /o Selsort.cs
ildasm /text Selsort.exe > Selsort.il
```

**Neither `csc` nor `ildasm` is installed on this machine** (checked 2026-10-21) —
both ship with the .NET Framework SDK / Visual Studio, not the .NET SDK. Options:

1. Build with the SDK, which `Selsort.csproj` already supports. Use Release so it
   corresponds to the book's `/o` optimize flag:
   ```bash
   cd Virtual/Selsort
   dotnet build Selsort.csproj -c Release
   ```
   Then disassemble the resulting DLL with a tool you install yourself — e.g.
   `dotnet tool install -g ilspycmd`. Neither is installed yet, so expect some
   trial and error.
2. Install the .NET Framework developer pack / Visual Studio Build Tools to get the
   exact `csc` + `ildasm` the book expects.

Worth asking the lecturer which they accept before investing time — the IL from a
modern SDK build will not be instruction-for-instruction identical to the book's.

Reference for CLI instruction semantics: Ecma-335, Partition III (search for the
instruction name, e.g. `ldc`).

## Exercise 9.3

Exercise text not yet recorded here. The original overview note pointed at
`QueueWithMistake.java`, which is in this folder.

From that file's own header: it implements `SentinelLockQueue`, a lock-based
concurrent FIFO queue built as a linked list of nodes, with an extra sentinel node so
`head` and `tail` always have something to point at (meaning the first item is
`head.next.item`, not `head.item`). It contains a deliberate **memory-management**
mistake that makes the test program run out of memory. Per the comments, the bug is
unrelated to concurrency or threads.

**TODO:** paste the actual 9.3 text in here.

---

## Build notes

- MicroC has two projects: `MicroC/microcc.fsproj` (compiler) and `MicroC/parse.fsproj`
  (parser only). The lexer/parser are generated from `CLex.fsl` and `CPar.fsy` by
  fslex/fsyacc at build time, so a grammar change requires a rebuild, not just an edit.
- `MicroC/grammar.txt` documents the existing grammar — update it if you change `CPar.fsy`.
- `MicroC/README.md` has the book's own build and run instructions.
- Generated bytecode runs on the Java machine: `java Machine prog.out <args>`, or
  `java Machinetrace prog.out <args>` to trace the stack.
