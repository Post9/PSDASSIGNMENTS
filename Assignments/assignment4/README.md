# Group "xXNillermafiaXx" Assignment 04
PLC 4.5, 5.7

## Exercise 4.5
Extend the (untyped) functional language with infix operator "&&"
meaning sequential logical "and" and infix operator "||" meaning sequential logical
"or", as in C, C++, Java, C#, or F#. Note that e1 && e2 can be encoded as
if e1 then e2 else false and that e1 || e2 can be encoded as if e1
then true else e2. Hence you need only change the lexer and parser specifications,
and make the new rules in the parser specification generate the appropriate
abstract syntax. You need not change Absyn.fs or Fun.fs.

Only `FunLex.fsl` and `FunPar.fsy` were changed. The lexer gets two new tokens:

```fsharp
  | "&&"            { SEQAND }
  | "||"            { SEQOR }
```

The parser turns them straight into `If`, so there is no new abstract syntax:

```fsharp
  | Expr SEQAND Expr                    { If($1, $3, CstB false) }
  | Expr SEQOR  Expr                    { If($1, CstB true, $3)  }
```

Because it's an `If`, the right side is only evaluated when it's needed, so `false && 1/0 = 0` never divides.

Precedence is the same as in C and F#: `||` binds weaker than `&&`, and both bind weaker than the comparisons. That's why they sit between `ELSE` and `EQ NE`:

```fsharp
%left ELSE              /* lowest precedence  */
%left SEQOR
%left SEQAND
%left EQ NE
```

So `a < b && c = d || e` is read as `((a < b) && (c = d)) || e`.

## Exercise 5.7
Extend the monomorphic type checker to deal with lists. Use the following
extra kinds of types:

```fsharp
type typ =
  | ...
  | TypL of typ                            (* list, element type is typ *)
  | ...
```

The exercise doesn't say what the list expressions look like, so we used the ones from exercise 4.8:

```fsharp
  | CstN of typ                         (* [] with element type        *)
  | ConC of tyexpr * tyexpr             (* e1 :: e2                    *)
  | Match of tyexpr * tyexpr * (string * string * tyexpr)
          (* match e0 with [] -> e1 | h :: t -> e2                     *)
```

`CstN` takes a type, unlike in 4.8. The checker is monomorphic and has no type inference, so `[]` on its own has no type. It's the same idea as `Letfun` having explicit types on the parameter and result.

The new cases in `typ`:

```fsharp
    | CstN t -> TypL t
    | ConC(e1, e2) ->
      let t1 = typ e1 env
      match typ e2 env with
      | TypL t2 when t1 = t2 -> TypL t1
      | _ -> failwith "ConC: tail is not a list of the head's type"
    | Match(e0, e1, (h, t, e2)) ->
      match typ e0 env with
      | TypL tElem ->
        let t1 = typ e1 env
        let t2 = typ e2 ((h, tElem) :: (t, TypL tElem) :: env)
        if t1 = t2 then t1
        else failwith "Match: branch types differ"
      | _ -> failwith "Match: not a list"
```

For `ConC` the tail has to be a list of the head's type, which is how all elements end up with the same type. For `Match` the `[]` branch is checked in the normal environment. The `h :: t` branch gets `h` bound to the element type and `t` to the list type. Both branches have to give the same type, like the two branches of `If`.

`eval` wasn't changed since the exercise is only about the type checker, so F# gives an incomplete match warning for it.

Examples are at the bottom of `TypedFun.fs`. `exL1` is `[1; 2]`, `exL2` is a recursive `sum` over an `int list` and `exL3` is a function that returns a `bool list`. `exLErr1` to `exLErr3` are a list with mixed element types, a `Match` on an int and a `Match` where the branches have different types.

```
dotnet fsi TypedFun.fs
```

```fsharp
open TypedFun;;
typesL;;             // [TypL TypI; TypI; TypL TypB]
typeCheck exLErr1;;  // ConC: tail is not a list of the head's type
typeCheck exLErr2;;  // Match: not a list
typeCheck exLErr3;;  // Match: branch types differ
```

6.1 
 open ParseAndRunHigher;;
 from the examples
> run (fromString @"let twice f = let g x = f(f(x)) in g end
-                   in let mul3 z = z*3 in twice mul3 2 end end");;
val it: HigherFun.value = Int 18

First 


 run (fromString "let add x = let f y = x+y in f end
- in add 2 5 end");;
val it: HigherFun.value = Int 7

second 

run (fromString "let add x = let f y = x+y in f end
- in let addtwo = add 2
- in addtwo 5 end
- end");;
val it: HigherFun.value = Int 7


third

a function's body sees the variables from where it was declared, not from where it's called therefore, the function uses static scope or the definition of eval uses fDeclEnv.





> run (fromString "let add x = let f y = x+y in f end
-   in let addtwo = add 2
-   in let x = 77 in addtwo 5 end
-   end
-   end");;
val it: HigherFun.value = Int 7


This one Gives a weird return thing

forth 

> run (fromString "let add x = let f y = x+y in f end
-   in add 2 end");;
val it: HigherFun.value =
  Closure
    ("f", "y", Prim ("+", Var "x", Var "y"),
     [("x", Int 2);
      ("add",
       Closure
         ("add", "x", Letfun ("f", "y", Prim ("+", Var "x", Var "y"), Var "f"),
          []))])
This returns a function that asks for the valye for y, and not a result. proper f# things

so we have to eval it, with micro-ml using eval and then adding addtwoC as env?


> eval (fromString "addtwoC 5") [("addtwoC", addtwoC)];;
val it: HigherFun.value = Int 7


6.2

Added clos, a simpler closure for anonymous function in call in Eval. 




6.3

Added Arrow and fun to lexer

> open ParseAndRunHigher;;
> run (fromString "fun x -> 2*x");;
val it: HigherFun.value = Clos ("x", Prim ("*", CstI 2, Var "x"), [])
now Works with anonymous functions.
