// Exercise 11.1

// (i)
let rec lenc xs k =
    match xs with
    | [] -> k 0
    | x :: xr -> lenc xr (fun v -> k (1 + v))

let id = fun v -> v

lenc [2; 5; 7] id

lenc [2; 5; 7] (printf "The answer is '%d'\n")


// (ii)
lenc [2; 5; 7] (fun v -> 2 * v)

// (iii)
let rec leni xs acc =
    match xs with
    | [] -> acc
    | x :: xr -> leni xr (acc + 1)

leni [2; 5; 7] 0