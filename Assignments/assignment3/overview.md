PLC:  3.5, 3.6, 3.7, 4.1, 4.2, 4.3, 4.4

Folder from repository: Expr, Fun

Do not follow build instructions in book - follow the README in the repository.

You should solve the assignments in your groups.

The book uses old code for fsLex and fsYacc. Use the tools as explained at lecture.

Assignment 3.6 completes our small Expr compiler in Lec02/Expr folder.

Exercise 3.5 Get expr.zip from the book homepage and unpack it. Using a com-
mand prompt, generate (1) the lexer and (2) the parser for expressions by running
fslex and fsyacc; then (3) load the expression abstract syntax, the lexer and
parser modules, and the expression interpreter and compilers, into an interactive F#
session (fsi):
fslex --unicode ExprLex.fsl
fsyacc --module ExprPar ExprPar.fsy
fsi -r FSharp.PowerPack.dll Absyn.fs ExprPar.fs ExprLex.fs ^
Parse.fs
Now try the parser on several example expressions, both well-formed and ill-formed
ones, such as these, and some of your own invention:
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


## Exercise 3.6 
Use the expression parser from Parse.fs and the compiler scomp
(from expressions to stack machine instructions) and the associated datatypes from
Expr.fs, to define a function compString : string -> sinstr list
that parses a string as an expression and compiles it to stack machine code


## Exercise 3.7

Extend the expression language abstract syntax and the lexer and parser specifications
with conditional expressions. The abstract syntax should be `If(e1, e2, e3)`, so modify
file `Absyn.fs` as well as `ExprLex.fsl` and file `ExprPar.fsy`. The concrete syntax may
be the keyword-laden F#/ML-style:

```
if e1 then e2 else e3
```

or the more light-weight C/C++/Java/C#-style:

```
e1 ? e2 : e3
```

Some documentation for fslex and fsyacc is found in this chapter and in Expert F# [17].

## Exercise 4.1

 Get archive fun.zip from the homepage and unpack to directory
Fun. It contains lexer and parser specifications and interpreter for a small first-
order functional language. Generate and compile the lexer and parser as described in
README.TXT; parse and run some example programs with ParseAndRun.fs.

## Exercise 4.2

 Write more example programs in the functional language, and test
them in the same way as in Exercise 4.1:
• Compute the sum of the numbers from 1000 down to 1. Do this by defining a
function sum n that computes the sum n + (n − 1) + ··· + 2 + 1. (Use straight-
forward summation, no clever tricks.)
• Compute the number 38, that is, 3 raised to the power 8. Again, use a recursive
function.
• Compute 30 + 31 + ··· + 310 + 311, using a recursive function (or two, if you
prefer).
• Compute 18 + 28 + ··· + 108, again using a recursive function (or two).
Exercise 4.3 For simplicity, the current implementation of the functional language
requires all functions to take exactly one argument. This seriously limits the pro-
grams that can be written in the language (at least it limits what that can be written
without excessive cleverness and complications).
Modify the language to allow functions to take one or more arguments. Start by
modifying the abstract syntax in Absyn.fs to permit a list of parameter names in
Letfun and a list of argument expressions in Call.
Then modify the eval interpreter in file Fun.fs to work for the new abstract
syntax. You must modify the closure representation to accommodate a list of pa-
rameters. Also, modify the Letfun and Call clauses of the interpreter. You will
need a way to zip together a list of variable names and a list of variable values, to
get an environment in the form of an association list; so function List.zip might
be useful.
Exercise 4.4 In continuation of Exercise 4.3, modify the parser specification to
accept a language where functions may take any (non-zero) number of arguments.
The resulting parser should permit function declarations such as these:
let pow x n = if n=0 then 1 else x * pow x (n-1) in pow 3 8 end
let max2 a b = if a<b then b else a
in let max3 a b c = max2 a (max2 b c)
endin max3 25 6 62 end
74
4
A First-Order Functional Language
You may want to define non-empty parameter lists and argument lists in analogy
with the Names1 nonterminal from Usql/UsqlPar.fsy, except that the param-
eters should not be separated by commas. Note that multi-argument applications
such as f a b are already permitted by the existing grammar, but they would
produce abstract syntax of the form Call(Call(Var "f", Var "a"), Var
"b") which the Fun.eval function does not understand. You need to modify the
AppExpr nonterminal and its semantic action to produce Call(Var "f", [Var
"a"; Var "b"]) instead.
