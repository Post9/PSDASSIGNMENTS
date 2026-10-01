// Exercise 7.2, using the for loops from Exercise 7.3 



// (1) 
void arrsum(int n, int arr[], int *sump) {
  int i;
  *sump = 0;
  for (i = 0; i < n; i = i + 1)
    *sump = *sump + arr[i];
}

// (2) 
void squares(int n, int arr[]) {
  int i;
  for (i = 0; i < n; i = i + 1)
    arr[i] = i * i;
}

// (3) 
void histogram(int n, int ns[], int max, int freq[]) {
  int i;
  for (i = 0; i <= max; i = i + 1)
    freq[i] = 0;
  for (i = 0; i < n; i = i + 1)
    freq[ns[i]] = freq[ns[i]] + 1;
}

void main(int n) {
  int sum;
  int i;

  
  int arr[4];
  arr[0] = 7;
  arr[1] = 13;
  arr[2] = 9;
  arr[3] = 8;
  arrsum(4, arr, &sum);
  print sum;
  println;

  
  int sq[20];
  squares(n, sq);
  arrsum(n, sq, &sum);
  print sum;
  println;

  
  int ns[7];
  int freq[4];
  ns[0] = 1;
  ns[1] = 2;
  ns[2] = 1;
  ns[3] = 1;
  ns[4] = 1;
  ns[5] = 2;
  ns[6] = 0;
  histogram(7, ns, 3, freq);
  for (i = 0; i <= 3; i = i + 1)
    print freq[i];
  println;
}
