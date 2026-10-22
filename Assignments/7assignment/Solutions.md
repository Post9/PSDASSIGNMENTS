8.5

updated parser and lexer and Comp.fs with the logic
, if statement reused.
important that Questionmark and Colon is low assosiativity ( cant spell fs). %right

8.6 

updated lexer parser absyn and comp, for switch and case. 





9.1
The bytecode is in each of these files:
(i) Virtual/Selsort/Selsort.il/Selsort.il
(ii) Virtual/Selsort/Selsort.jvmbytecode

Each has the local variable mapping at the top of each file.

9.3
The Problem:
    The cause of the SentinelLockQueue crash is the field 'dummy', which keep a permanent reference to the first sentinal node.
    Because everything is linked to 'dummy' the garbage collecter can help since it only frees objects that are unreachable from the roots.
    Therefore, while 'dummy' might be "dead data" it cant be collected since it is still linked.

Our fix removes the 'dummy' field and creates the sentinel in the constructor instead.

private Node head, tail;

public SentinelLockQueue() {
head = tail = new Node(-444, null);
} (line 92 - 96)

// To run 

Once 'head' moves past a Node it becomes unreachable and can be collected.
