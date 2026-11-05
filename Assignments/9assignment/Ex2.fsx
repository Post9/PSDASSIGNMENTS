// Exercise 11.2

// (i)
let rec revc xs k =
    match xs with
    | [] -> k []
    | x :: xr -> revc xr (fun v -> k (v @ [x]))
    
let id = fun v -> v

printfn "i %A" (revc [2; 5; 7] id)


// (ii)
printfn "ii %A" (revc [2; 5; 7] (fun v -> v @ v))

// (iii)
let rec revi xs acc =
    match xs with
    | [] -> acc
    | x :: xr -> revi xr (x :: acc)

printfn "iii %A" (revi [2; 5; 7] [])
    