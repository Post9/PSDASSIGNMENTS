// 11.4

// 0 fix
let rec prodc2 xs k =
    match xs with
    | [] -> k 1
    | 0 :: _ -> 0
    | x :: xr -> prodc2 xr (fun v -> k (x * v))

let id = fun v -> v

printfn "Prodc2 tests:"
printfn "Normal: %d" (prodc2 [2; 5; 7] id)

printfn "With 0: %d" (prodc2 [2; 5; 0; 7; 9] id)

printfn "Times 2: %d" (prodc2 [2; 5; 7] (fun v -> 2 * v))

printfn "Times 2 with 0: %d" (prodc2 [2; 5; 0; 7; 9] (fun v -> 2 * v))



let rec prodi xs acc =
    match xs with
    | [] -> acc
    | 0 :: _ -> 0
    | x :: xr -> prodi xr (x * acc)
    

printfn "ProdI tests:"
printfn "Normal: %d" (prodi [2; 5; 7] 1)

printfn "With 0: %d" (prodi [2; 5; 0; 7; 9] 1)