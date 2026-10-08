# Assignment 6

## 7.4
                      // 7.4 Eval just like access, but increment/decrement. by +1/-1¨
                        // VARIABLE9000 = v 
    |PreDec acc -> let (loc, store1) = access acc locEnv gloEnv store
                       let VARIABLE9000 =  getSto store1 loc - 1
                       (VARIABLE9000, setSto store1 loc, store1)           
    |PreInc acc -> let (loc, store1) = access acc locEnv gloEnv store
                       let VARIABLE9000 =  getSto store1 loc + 1
                       (VARIABLE9000, setSto store1 loc, store1)


## 7.5

  | "++"            { DOUBLEPLUS }   
  | "--"            { DOUBLEMINUS }   


%token DOUBLEPLUS DOUBLEMINUS


AtExprNotAccess:
    Const                               { CstI $1             }
  | LPAR ExprNotAccess RPAR             { $2                  } 
  | AMP Access                          { Addr $2             }
  | DOUBLEPLUS Access                   { PreInc $2           }
  | DOUBLEMINUS Access                  { PreDec $2           }
  ;



## 8.1

### (i)



### (ii)



## 8.3



## 8.4



## 8.5



