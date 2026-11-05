// 11.3

let rec prodc xs k =
    match xs with
    | [] -> k 1
    | x :: xr -> prodc xr (fun v -> k (x * v))

let id = fun v -> v

printfn "%d" (prodc [2; 5; 7] id)

printfn "%d" (prodc [2; 5; 7] (fun v -> 2 * v))




