(* Lexing and parsing of micro-ML programs using fslex and fsyacc *)

module Parse

open System
open System.IO
open System.Text
open FSharp.Text.Lexing
open Absyn

(* Plain parsing from a string, with poor error reporting *)

let fromString (str : string) : expr =
    let lexbuf = LexBuffer<char>.FromString(str)
    try 
      FunPar.Main FunLex.Token lexbuf
    with 
      | exn -> let pos = lexbuf.EndPos 
               failwithf "%s near line %d, column %d\n" 
                  (exn.Message) (pos.Line+1) pos.Column
             
(* Parsing from a file *)

let fromFile (filename : string) =
    use reader = new StreamReader(filename)
    let lexbuf = LexBuffer<char>.FromTextReader reader
    try 
      FunPar.Main FunLex.Token lexbuf
    with 
      | exn -> let pos = lexbuf.EndPos 
               failwithf "%s in file %s near line %d, column %d\n" 
                  (exn.Message) filename (pos.Line+1) pos.Column


(* Exercise 4.2*)

let e1 = fromString "5+7";;
let e2 = fromString "let f x = x + 7 in f 2 end";;



let e3 = fromString "let f x = if x = 1 then x else x + f(x - 1) in f 1000 end"

// what do they mean

(* Examples in concrete syntax *)

let e4 = fromString "let pow n = if n = 0 then 1 else 3 * pow (n - 1) in pow 8 end"

// e5: 3^0 + 3^1 + ... + 3^11. pow n gives 3^n, g k adds pow k to g (k-1) until k = 0. 
let e5 = fromString "let pow n = if n = 0 then 1 else 3 * pow (n - 1) in let g k = if k = 0 then 1 else pow k + g (k - 1) in g 11 end end"

// e6: 1^8 + 2^8 + ... + 10^8. p x gives x^8 (functions only take one argument, so no pow x 8),
// g k adds p k to g (k-1) until k = 0. Result 
let e6 = fromString "let p x = x * x * x * x * x * x * x * x in let g k = if k = 0 then 0 else p k + g (k - 1) in g 10 end end"

let ex1 = fromString 
            @"let f1 x = x + 1 in f1 12 end";;

(* Example: factorial *)

let ex2 = fromString 
            @"let fac x = if x=0 then 1 else x * fac(x - 1)
              in fac n end";;

(* Example: deep recursion to check for constant-space tail recursion *)

let ex3 = fromString 
            @"let deep x = if x=0 then 1 else deep(x-1) 
              in deep count end";;
    
(* Example: static scope (result 14) or dynamic scope (result 25) *)

let ex4 = fromString 
            @"let y = 11
              in let f x = x + y
                 in let y = 22 in f 3 end 
                 end
              end";;

(* Example: two function definitions: a comparison and Fibonacci *)

let ex5 = fromString
            @"let ge2 x = 1 < x
              in let fib n = if ge2(n) then fib(n-1) + fib(n-2) else 1
                 in fib 25 
                 end
              end";;
