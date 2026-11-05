Solutions: 

11.1:
Code is in Ex1.fsx file

(i)
Answer is 3
Trace of lenc [2; 5; 7] id:

lenc [2; 5; 7] id
==> lenc [5;7] (fun v -> id(1 + v))
==> lenc [7] (fun w -> (fun v -> id(1 + v)) (1 + w))
==> lenc [] (fun u -> (fun w -> (fun v -> id(1 + v)) (1 + w)) (1 + u))
==> (fun u -> (fun w -> (fun v -> id(1 + v)) (1 + w)) (1 + u)) 0
==> (fun w -> (fun v -> id(1 + v)) (1 + w)) (1 + 0)
==> (fun w -> (fun v -> id(1 + v)) (1 + w)) 1
==> (fun v -> id(1 + v)) (1 + 1)
==> (fun v -> id(1 + v)) 2
==> id(1 + 2)
==> id 3
==> 3


(ii)
The Answer will be 6

fsi:
val it : 6


(iii)
Brug for svar



11.2
Code is in Ex2.fsx

(i)
The code in Ex2.fsx gives [7; 5; 2]

Trace of revc [2; 5; 7] id:

revc [2;5;7] id
==> revc [5;7] (fun v -> id(v @ [2]))
==> revc [7] (fun w -> (fun v -> id(v @ [2])) (w @ [5]))
==> revc [] (fun u -> (fun w -> (fun v -> id(v @ [2])) (w @ [5])) (u @ [7]))
==> (fun u -> (fun w -> (fun v -> id(v @ [2])) (w @ [5])) (u @ [7])) []
==> (fun w -> (fun v -> id(v @ [2])) (w @ [5])) ([] @ [7])
==> (fun w -> (fun v -> id(v @ [2])) (w @ [5])) [7]
==> (fun v -> id(v @ [2])) ([7] @ [5])
==> (fun v -> id(v @ [2])) [7; 5]
==> id([7; 5] @ [2])
==> id [7; 5; 2]
==> [7; 5; 2]


(ii)
The code in Ex2.fsx gives [7; 5; 2; 7; 5; 2]


(iii)
Brug for svar



11.3
Code is in Ex3.fsx

Trace of prodc [2; 5; 7] id:

prodc [2;5;7] id
==> prodc [5;7] (fun v -> id(2 * v))
==> prodc [7] (fun w -> (fun v -> id(2 * v)) (5 * w))
==> prodc [] (fun u -> (fun w -> (fun v -> id(2 * v)) (5 * w)) (7 * u))
==> (fun u -> (fun w -> (fun v -> id(2 * v)) (5 * w)) (7 * u)) 1
==> (fun w -> (fun v -> id(2 * v)) (5 * w)) (7 * 1)
==> (fun w -> (fun v -> id(2 * v)) (5 * w)) 7
==> (fun v -> id(2 * v)) (5 * 7)
==> (fun v -> id(2 * v)) 35
==> id(2 * 35)
==> id 70
==> 70



11.4
Brug for svar?

