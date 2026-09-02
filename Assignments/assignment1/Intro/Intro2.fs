(* Programming language concepts for software developers, 2010-08-28 *)

(* Evaluating simple expressions with variables *)

module Intro2

(* Association lists map object language variables to their values *)

let env = [("a", 3); ("c", 78); ("baf", 666); ("b", 111)];;

let emptyenv = [](* the empty environment *)

let rec lookup env x =
    match env with 
    | []        -> failwith (x + " not found")
    | (y, v)::r -> if x=y then v else lookup r x;;

let cvalue = lookup env "c";;


(* Object language expressions with variables *)

type expr = 
    | CstI of int
    | Var of string
    | Prim of string * expr * expr   
    | If of expr * expr * expr // 1.4  String or no String for op? op is always if



let e1 = CstI 17;;

let e2 = Prim("+", CstI 3, Var "a");;

let e3 = Prim("+", Prim("*", Var "b", CstI 9), Var "a");;


(* Evaluation within an environment *)
// Missing type check no f# 

let rec eval e (env : (string * int) list) : int =
    match e with
    | CstI i            -> i
    | Var x             -> lookup env x 
    (* Moved out because of 1.3
    | Prim("+", e1, e2) -> eval e1 env + eval e2 env
    | Prim("*", e1, e2) -> eval e1 env * eval e2 env
    | Prim("-", e1, e2) -> eval e1 env - eval e2 env


        // 1.1.1
        // max min and equals
    | Prim("max", e1, e2) -> 
        if eval e1 env >= eval e2 env then
            eval e1 env   
        else 
            eval e2 env 
    | Prim("min", e1, e2) ->
        if eval e1 env <= eval e2 env then
            eval e1 env else
            eval e2 env
    | Prim("==", e1, e2) -> 
    
        if eval e1 env = eval e2 env then 1 else 0
    *)
        // 1.1.3
    | Prim(ope, e1, e2) ->
        let i1 = eval e1 env
        let i2 = eval e2 env
        match ope with
            | "+" -> i1 + i2
            | "-" -> i1 - i2
            | "*" -> i1 * i2
            | "max" -> if i1 >= i2 then i1 else i2
            | "min" -> if i1 <= i2 then i1 else i2
            | "==" -> if i1 = i2 then 1 else 0
            | _ -> failwith "unknown operator"

        // 1.1.4
    |If (e1,e2,e3) ->

        let i1 = eval e1 env  
        
        // check zero since anything else than 0 is true
        
        if i1 = 0 then eval e3 env else eval e2 env
        
        
        // This is for when true stricly 1 and false strictly 0
        // if i1 = 1 then eval e2 env elif i1 = 0 then  eval e3 env else failwith "condition not 0 or 1"


    
// 1.1.2 
let ex1 = Prim("max", Prim("max", CstI 10, CstI 9), CstI 11);;  // max(max(10, 9), 11) = 11

// WE make a longer one to test for Stack overflow. 
let ex2 = Prim("==", Prim("min", CstI 1, CstI 13), CstI 11);; // equals(min(1, 13), 11) = 0 .. False

// this fails due, to what?
let ex3 = Prim("max", Prim("max", Prim("max", Prim("max", Prim("max", Prim("max", CstI 10, CstI 9), CstI 9), CstI 9), CstI 9), CstI 9), CstI 11);; 
// stackoverflow? im not doing it 50000 times?

// 1.1.5 test
// If(Var "a", CstI 11, CstI 22)
let ex4 = If(Var "a", CstI 11, CstI 22)

let ex5 = If(Var "b", CstI 11, CstI 22)


let ex1v = eval ex1 env;;  // expected 11// doesnt need env? since there is not vars defined in them -> wrong we need them still to but they can be empty
let ex2v = eval ex2 env ;;  // expexted 0 
let ex3v = eval ex3 env;; // expcted 11
// still gives the right answer not long enough? 
// we dont get too see what the values are since they are in the vars

let ex4v = eval ex4 [("a", 1)];; // expected 11

let ex5v = eval ex5 [("b", 0)];; // expected 22




let e1v  = eval e1 env;;
let e2v1 = eval e2 env;;
let e2v2 = eval e2 [("a", 314)];;
let e3v  = eval e3 env;;

//1.2
//1.2.1

    type aexpr = 
        | ACstI of int                    
        | AVar  of string               
        | Add  of aexpr * aexpr   // expr? instead aexpr?       
        | Sub  of aexpr * aexpr        
        | Mul  of aexpr * aexpr    
    
    let rec evalA e (env : (string * int) list) : int =
        match e with
        | ACstI e -> e
        | AVar e -> lookup env e
        | Add (e1,e2)  -> evalA e1 env + evalA e2 env
        | Sub (e1,e2)  -> evalA e1 env - evalA e2 env
        | Mul  (e1,e2) -> evalA e1 env * evalA e2 env 

// Write the representation of the expressions v − (w + z) and 2 ∗ (v − (w + z))
//     and x + y + z +v.

//1.2.2
let ex7a = Sub(AVar "v", Add(AVar "w", AVar "z")) // v − (w + z)
let ex7b = Mul(ACstI 2, Sub(AVar "v", Add(AVar "w", AVar "z"))) // 2 ∗ (v − (w + z))
let ex7c = Add(Add(Add(AVar "x", AVar "y"), AVar "z"), AVar "v") //x + y + z +v

let ex7av = evalA ex7a  [("v", 5);("w", 6); ("z", 2)] // 5 - (6 + 2) = -3
let ex7bv = evalA ex7b  [("v", 8);("w", 1); ("z", 9)] // 2 * (8 - (1 + 9)) = 2 * (8 - 10) = 2 * -2 = -4
let ex7cv = evalA ex7c [("x", 5);("y", 6); ("z", 2);("v",10)] // 5 + 6 + 2 + 10 = 23

(*
Questions, 
should we make expr ane axepr mutally recursive? they are very similar and they could be interchanged for eachother?
This expression was expected to have type
    'expr'    
but here has type
    'aexpr'F# Compiler1

    getting this error when doing this. So different approach


Should we make an second eval, Aeval for arithmetic expressions?  yeah we should. it will be too complicated to make one? 




*)



    // f(x) = 2*x^2 + x42let ex19a = Sub(Var "v", Add(Var "w", Var "z"))
// let ex19b = Mul(CstI 2, Sub(Var "v", Add(Var "w", Var "z")))
// let ex19c = Add(Add(Add(Var "x", Var "y"), Var "z"), Var "v")