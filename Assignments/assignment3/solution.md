3.4



ran the shii.

the first three work.
 open Parse;;

> fromString "1 + 2 \* 3";;

val it: Absyn.expr = Prim ("+", CstI 1, Prim ("\*", CstI 2, CstI 3))



> fromString "1 - 2 - 3";;

val it: Absyn.expr = Prim ("-", Prim ("-", CstI 1, CstI 2), CstI 3)



> fromString "1 + -2";;

val it: Absyn.expr = Prim ("+", CstI 1, CstI -2)









exception due to ++ not being a lexer thing?



Nope, its because there is not rule that matches EXPR PLUS PLUS. which would be a x = x + 1 sorta deal





fromString "x++";;

System.Exception: parse error near line 1, column 3



&#x20;  at Microsoft.FSharp.Core.PrintfModule.PrintFormatToStringThenFail@1448.Invoke(String message)

&#x20;  at FSI\_0002.Parse.fromString(String str) in /mnt/c/Users/niels/UniFiles/5\_semester/Programsdata/Assignments/Assignments/assignment3/Expr/Parse.fs:line 20

&#x20;  at <StartupCode$FSI\_0007>.$FSI\_0007.main@() in /mnt/c/Users/niels/UniFiles/5\_semester/Programsdata/Assignments/Assignments/assignment3/Expr/stdin:line 5

&#x20;  at System.RuntimeMethodHandle.InvokeMethod(Object target, Void\*\* arguments, Signature sig, Boolean isConstructor)

&#x20;  at System.Reflection.MethodBaseInvoker.InvokeWithNoArgs(Object obj, BindingFlags invokeAttr)

Stopped due to error





no floats exception



Lexer dosent have regex for .



fromString "1 + 1.2";;

System.Exception: Lexer error: illegal symbol near line 1, column 6



&#x20;  at Microsoft.FSharp.Core.PrintfModule.PrintFormatToStringThenFail@1448.Invoke(String message)

&#x20;  at FSI\_0002.Parse.fromString(String str) in /mnt/c/Users/niels/UniFiles/5\_semester/Programsdata/Assignments/Assignments/assignment3/Expr/Parse.fs:line 20

&#x20;  at <StartupCode$FSI\_0008>.$FSI\_0008.main@() in /mnt/c/Users/niels/UniFiles/5\_semester/Programsdata/Assignments/Assignments/assignment3/Expr/stdin:line 6

&#x20;  at System.RuntimeMethodHandle.InvokeMethod(Object target, Void\*\* arguments, Signature sig, Boolean isConstructor)

&#x20;  at System.Reflection.MethodBaseInvoker.InvokeWithNoArgs(Object obj, BindingFlags invokeAttr)

Stopped due to error





Works



> fromString "let z = (17) in z + 2 \* 3 end";;

val it: Absyn.expr =

&#x20; Let ("z", CstI 17, Prim ("+", Var "z", Prim ("\*", CstI 2, CstI 3)))



fails missing parenthesis





> fromString "let z = 17) in z + 2 \* 3 end";;

System.Exception: parse error near line 1, column 11



&#x20;  at Microsoft.FSharp.Core.PrintfModule.PrintFormatToStringThenFail@1448.Invoke(String message)

&#x20;  at FSI\_0002.Parse.fromString(String str) in /mnt/c/Users/niels/UniFiles/5\_semester/Programsdata/Assignments/Assignments/assignment3/Expr/Parse.fs:line 20

&#x20;  at <StartupCode$FSI\_0011>.$FSI\_0011.main@() in /mnt/c/Users/niels/UniFiles/5\_semester/Programsdata/Assignments/Assignments/assignment3/Expr/stdin:line 9

&#x20;  at System.RuntimeMethodHandle.InvokeMethod(Object target, Void\*\* arguments, Signature sig, Boolean isConstructor)

&#x20;  at System.Reflection.MethodBaseInvoker.InvokeWithNoArgs(Object obj, BindingFlags invokeAttr)

Stopped due to error





&#x20;fromString "1 + let x=5 in let y=7+x in y+y end + x end";;

val it: Absyn.expr =

&#x20; Prim

&#x20;   ("+", CstI 1,

&#x20;    Let

&#x20;      ("x", CstI 5,

&#x20;       Prim

&#x20;         ("+",

&#x20;          Let

&#x20;            ("y", Prim ("+", CstI 7, Var "x"), Prim ("+", Var "y", Var "y")),

&#x20;          Var "x")))

>

