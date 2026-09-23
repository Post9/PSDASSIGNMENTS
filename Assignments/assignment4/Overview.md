Get-Clipboard | Add-Content README.md -Encoding utf8
Exercise 6.1 Download and unpack fun1.zip and fun2.zip and build the
micro-ML higher-order evaluator as described in file README.TXT point E.
Then run the evaluator on the following four programs. Is the result of the third
one as expected? Explain the result of the last one:

let add x = let f y = x+y in f end
in add 2 5 end
let add x = let f y = x+y in f end
in let addtwo = add 2
in addtwo 5 end
end
let add x = let f y = x+y in f end
in let addtwo = add 2
in let x = 77 in addtwo 5 end
end
end
let add x = let f y = x+y in f end
in add 2 end
Exercise 6.2 Add anonymous functions, similar to F#’s fun x -> ..., to the
micro-ML higher-order functional language abstract syntax:
type expr =
...
| Fun of string * expr
| ...
For instance, these two expressions in concrete syntax:
fun x -> 2*x
let y = 22 in fun z -> z+y end
should parse to these two expressions in abstract syntax:
Fun("x", Prim("*", CstI 2, Var "x"))
Let("y", CstI 22, Fun("z", Prim("+", Var "z", Var "y")))
Evaluation of a Fun(...) should produce a non-recursive closure of the form
type value =
| ...
| Clos of string * expr * value env
(* (x,body,declEnv) *)
In the empty environment the two expressions shown above should evaluate to these
two closure values:
Clos("x", Prim("*", CstI 2, Var "x"), [])
Clos("z", Prim("+", Var "z", Var "y"), [(y,22)])
Extend the evaluator eval in file HigherFun.fs to interpret such anonymous
functions.
6.8
Exercises
111
Exercise 6.3 Extend the micro-ML lexer and parser specifications in FunLex.fsl
and FunPar.fsy to permit anonymous functions. The concrete syntax may be as
in F#: fun x -> expr or as in Standard ML: fn x => expr , where x is a
variable. The micro-ML examples from Exercise 6.1 can now be written in these
two alternative ways:
let add x = fun y -> x+y
in add 2 5 end
let add = fun x -> fun y -> x+y
in add 2 5 end
