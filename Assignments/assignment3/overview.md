# Assignment 3

**Exercises (PLC):** 3.5 · 3.6 · 3.7 · 4.1 · 4.2 · 4.3 · 4.4
**Folders from the repository:** `Expr/` (3.5–3.7) · `Fun/` (4.1–4.4)

> [!important] Rules
> - **Don't** follow the book's build instructions. Follow the `README.md` in each folder.
> - The book uses old fslex/fsyacc code. Use the tools as shown at the lecture.
> - Solve the exercises in your group.

| Exercise | Folder | What you do | Files you change |
|---|---|---|---|
| [3.5](#exercise-35) | `Expr/` | Build the lexer and parser, then try example expressions | — |
| [3.6](#exercise-36) | `Expr/` | Write `compString : string -> sinstr list` | a new function |
| [3.7](#exercise-37) | `Expr/` | Add `If(e1, e2, e3)` | `Absyn.fs`, `ExprLex.fsl`, `ExprPar.fsy` |
| [4.1](#exercise-41) | `Fun/` | Build micro-ML and run the examples | — |
| [4.2](#exercise-42) | `Fun/` | Write four example programs | — |
| [4.3](#exercise-43) | `Fun/` | Let functions take several arguments | `Absyn.fs`, `Fun.fs` |
| [4.4](#exercise-44) | `Fun/` | Parse multi-argument functions | `FunPar.fsy` |

> [!tip] Building
> Build from **PowerShell** with `dotnet build parse.fsproj`. WSL only has .NET SDK 8, and these projects need SDK 10.
> Run `dotnet fsi -r bin/Debug/net10.0/FsLexYacc.Runtime.dll …` from **inside** the folder, because that path is relative.

---

## Expr — exercises 3.5 to 3.7

Exercise 3.6 completes the small Expr compiler from the `Lec02/Expr` folder.

### Exercise 3.5

Generate the lexer and the parser for expressions, then load the abstract syntax, the lexer and parser modules, and the interpreter and compilers into `fsi`.

The book's commands are outdated. Use the ones from `Expr/README.md`:

```powershell
dotnet build parse.fsproj
dotnet fsi -r bin/Debug/net10.0/FsLexYacc.Runtime.dll Absyn.fs ExprPar.fs ExprLex.fs Parse.fs Expr.fs
```

<details>
<summary>The book's original commands (don't use them)</summary>

```
fslex --unicode ExprLex.fsl
fsyacc --module ExprPar ExprPar.fsy
fsi -r FSharp.PowerPack.dll Absyn.fs ExprPar.fs ExprLex.fs Parse.fs
```
</details>

Try the parser on well-formed and ill-formed expressions, including these and some of your own:

```fsharp
open Parse;;
fromString "1 + 2 * 3";;
fromString "1 - 2 - 3";;
fromString "1 + -2";;
fromString "x++";;
fromString "1 + 1.2";;
fromString "1 + ";;
fromString "let z = (17) in z + 2 * 3 end";;
fromString "let z = 17) in z + 2 * 3 end";;
fromString "let in = (17) in z + 2 * 3 end";;
fromString "1 + let x=5 in let y=7+x in y+y end + x end";;
```

### Exercise 3.6

Use the parser from `Parse.fs`, and the compiler `scomp` (expressions to stack machine instructions) with its datatypes from `Expr.fs`, to define

```fsharp
compString : string -> sinstr list
```

It parses a string as an expression and compiles it to stack machine code.

### Exercise 3.7

Extend the expression language with **conditional expressions**. The abstract syntax is `If(e1, e2, e3)`, so change `Absyn.fs`, `ExprLex.fsl` and `ExprPar.fsy`.

The concrete syntax can be either of these:

| Style | Syntax |
|---|---|
| F#/ML, with keywords | `if e1 then e2 else e3` |
| C/C++/Java/C#, lighter | `e1 ? e2 : e3` |

There's fslex and fsyacc documentation in chapter 3 and in *Expert F#* [17].

---

## Fun (micro-ML) — exercises 4.1 to 4.4

### Exercise 4.1

The `Fun/` folder has the lexer and parser specifications and the interpreter for a small first-order functional language. Build them as described in `Fun/README.md`, then parse and run some example programs with `ParseAndRun.fs`.

### Exercise 4.2

Write more example programs in the functional language, and test them the same way as in 4.1:

1. **Sum 1000 down to 1.** Define a function `sum n` that computes n + (n − 1) + ··· + 2 + 1. Use plain summation, no clever tricks.
2. **3⁸** (3 to the power 8). Again, use a recursive function.
3. **3⁰ + 3¹ + ··· + 3¹⁰ + 3¹¹**, using one recursive function, or two if you prefer.
4. **1⁸ + 2⁸ + ··· + 10⁸**, again using one or two recursive functions.

### Exercise 4.3

At the moment every function must take **exactly one** argument, which seriously limits the programs you can write without excessive cleverness.

Change the language so functions can take **one or more** arguments:

1. **`Absyn.fs`**: allow a **list of parameter names** in `Letfun` and a **list of argument expressions** in `Call`.
2. **`Fun.fs`**: update `eval` for the new abstract syntax.
   - Change the closure representation so it holds a list of parameters.
   - Change the `Letfun` and `Call` cases.
   - You need to pair a list of names with a list of values to get an association-list environment. `List.zip` may help.

### Exercise 4.4

Continuing from 4.3, change the **parser specification** so functions can take any non-zero number of arguments. The parser should accept declarations like these:

```fsharp
let pow x n = if n=0 then 1 else x * pow x (n-1) in pow 3 8 end
```

```fsharp
let max2 a b = if a<b then b else a
in let max3 a b c = max2 a (max2 b c)
   in max3 25 6 62 end
end
```

Hints from the book:

- **Parameter and argument lists:** define non-empty lists the same way as the `Names1` nonterminal in `Usql/UsqlPar.fsy`, but **without commas** between the parameters.
- **`f a b` already parses,** but it produces `Call(Call(Var "f", Var "a"), Var "b")`, which `Fun.eval` doesn't understand.
  - Change the `AppExpr` nonterminal and its semantic action so that it produces `Call(Var "f", [Var "a"; Var "b"])` instead.
