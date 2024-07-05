\ ----------------------------------------------------------------
\ Test Suite for rat-math-lib.fs
: confirm if ." Pass " else ." Fail " bye endif ;  \ immediate assert( )

.s  \ <0> =
0 0 GCF .s 0= confirm  \ input error
1 0 GCF .s 1 = confirm  \ exit case
0 1 GCF .s 1 = confirm  \ rev exit
2 1 GCF .s 1 = confirm  \ n1 > n2
1 2 GCF .s 1 = confirm  \ n1 < n2
1 1 GCF .s 1 = confirm
-1 1 GCF .s 1 = confirm
1 -1 GCF .s 1 = confirm
-1 -1 GCF .s 1 = confirm
2 2 GCF .s 2 = confirm  \ n1 = n2
-2 2 GCF .s 2 = confirm  \ -n1 = n2
2 -2 GCF .s 2 = confirm  \ n1 = -n2
2 4 GCF .s 2 = confirm
4 2 GCF .s 2 = confirm
-2 4 GCF .s 2 = confirm
4 -2 GCF .s 2 = confirm
2 -4 GCF .s 2 = confirm
-4 2 GCF .s 2 = confirm
65536 256 GCF .s 256 = confirm
65537 257 GCF .s 1 = confirm
2147483645 6289 GCF .s 19 = confirm

.s  \ <0> =
0 0 LCM .s 0= confirm  \ input error
1 0 LCM .s 0= confirm  \ GCD exit case
0 1 LCM .s 0= confirm  \ GCD rev exit case
2 1 LCM .s 2 = confirm
1 2 LCM .s 2 = confirm
1 1 LCM .s 1 = confirm
1 -1 LCM .s 1 = confirm
-1 1 LCM .s 1 = confirm
65537 257 LCM .s 16843009 = confirm

.s  \ <0> =
0 CTZ .s 32 = confirm
$80000000 CTZ .s 31 = confirm
$40000000 CTZ .s 30 = confirm
$60000000 CTZ .s 29 = confirm
$70000000 CTZ .s 28 = confirm
1 CTZ .s 0= confirm
2 CTZ .s 1 = confirm
12 CTZ .s 2 = confirm

.s  \ <0> =
ratZero .s 2drop  \ <2> 0 1 =
ratNegZero .s 2drop  \ <2> 0 -1 =
ratPosInf .s 2drop  \ <2> 1 0 =
ratNegInf .s 2drop  \ <2> -1 0 =
ratNaN .s 2drop  \ <2> 0 0 =
ratPosOne .s 2drop  \ <2> 1 1 =
ratNegOne .s 2drop  \ <2> -1 1 =
ratPosMax .s 2drop  \ <2> 2147483647 1 =
ratNegMax .s 2drop  \ <2> -2147483648 1 =
ratPosMin .s 2drop  \ <2> 1 2147483647 =
ratNegMin .s 2drop  \ <2> -1 2147483647 =
ratPi6 .s 2drop  \ <2> 355 113 =
ratPi9 .s 2drop  \ <2> 104348 33215 =
ratE6 .s 2drop  \ <2> 1264 465 =
ratE9 .s 2drop  \ <2> 23225 8544 =

.s  \ <0> =
ratVariable rx .s
rx .s
rat@ .s 0 1 d= confirm
-2 3 rx rat! .s
rx rat@ .s -2 3 d= confirm
-2147483648 2147483647 rx rat! .s
rx rat@ .s -2147483648 2147483647 d= confirm

.s  \ <0> =
0 n>rat .s 0 1 d= confirm
1 n>rat .s 1 1 d= confirm
-1 n>rat .s -1 1 d= confirm
2147483647 n>rat .s 2147483647 1 d= confirm
-2147483647 n>rat .s -2147483647 1 d= confirm

.s  \ <0> =
0 1/n>rat .s 1 0 d= confirm
1 1/n>rat .s 1 1 d= confirm
-1 1/n>rat .s 1 -1 d= confirm
2147483647 1/n>rat .s 1 2147483647 d= confirm
-2147483647 1/n>rat .s 1 -2147483647 d= confirm

.s  \ <0> =
RatZero ratFixSign .s 0 1 d= confirm
RatNegZero ratFixSign .s 0 -1 d= confirm
RatPosInf ratFixSign .s 1 0 d= confirm
RatNegInf ratFixSign .s -1 0 d= confirm
RatNaN ratFixSign .s 0 0 d= confirm
RatPosOne ratFixSign .s 1 1 d= confirm
RatNegOne ratFixSign .s -1 1 d= confirm
-1 -1 ratFixSign .s RatPosOne d= confirm
1 -1 ratFixSign .s RatNegOne d= confirm
RatPosMax ratFixSign .s 2147483647 1 d= confirm
RatNegMax ratFixSign .s -2147483647 1 d= confirm
-2147483647 -1 ratFixSign .s RatPosMax d= confirm
2147483647 -1 ratFixSign .s RatNegMax d= confirm
RatPosMin ratFixSign .s 1 2147483647 d= confirm
RatNegMin ratFixSign .s -1 2147483647 d= confirm
-1 -2147483647 ratFixSign .s RatPosMin d= confirm
1 -2147483647  ratFixSign .s RatNegMin d= confirm
2 3 ratFixSign .s 2 3 d= confirm
-2 3 ratFixSign .s -2 3 d= confirm
-2 -3 ratFixSign .s 2 3 d= confirm
2 -3 ratFixSign .s -2 3 d= confirm

.s  \ <0> =
 RatZero rat>n .s 0= confirm
 RatNegZero rat>n .s 0= confirm
 RatPosOne rat>n .s 1 = confirm
 RatNegOne rat>n .s -1 = confirm
 ratPosMax rat>n .s 2147483647 = confirm
 ratNegMax rat>n .s -2147483647 = confirm
 ratPosMin rat>n .s 0= confirm
 ratNegMin rat>n .s 0= confirm
 1 2 rat>n .s 1 = confirm
 -1 2 rat>n .s -1 = confirm
-1 -2 rat>n .s 1 = confirm
 1 -2 rat>n .s -1 = confirm
 1 3 rat>n .s 0= confirm
 -1 3 rat>n .s 0= confirm
 -1 -3 rat>n .s 0= confirm
 1 -3 rat>n .s 0= confirm

.s ." F: " f.s  \ <0> F: <0> =
ratZero rat>f .s ." F: " f.s 0.0e0 f= confirm
ratNegZero rat>f .s ." F: " f.s f>rat .s ." F: " f.s ratNegZero d= confirm
ratPosInf rat>f .s ." F: " f.s f>rat .s ." F: " f.s ratPosInf d= confirm
ratNegInf rat>f .s ." F: " f.s f>rat .s ." F: " f.s ratNegInf d= confirm
ratNaN rat>f .s ." F: " f.s f>rat .s ." F: " f.s ratNaN d= confirm
ratPosOne rat>f .s ." F: " f.s 1.0e0 f= confirm
ratNegOne rat>f .s ." F: " f.s -1.0e0 f= confirm
ratPosMax rat>f .s ." F: " f.s 2147483647.E0 f= confirm
ratNegMax rat>f .s ." F: " f.s -2147483647.E0 f= confirm
ratPosMin rat>f .s ." F: " f.s 4.6566128752E-10 f- fabs 1.e-11 f< confirm
ratNegMin rat>f .s ." F: " f.s -4.6566128752E-10 f- fabs 1.e-11 f< confirm
1 2 rat>f .s ." F: " f.s 0.5e0 f= confirm
-1 2 rat>f .s ." F: " f.s -0.5e0 f= confirm
-1 -2 rat>f .s ." F: " f.s 0.5e0 f= confirm
1 -2 rat>f .s ." F: " f.s -0.5e0 f= confirm
1 3 rat>f .s ." F: " f.s 0.333333333333e0 f- fabs 1.e-12 f< confirm
-1 3 rat>f .s ." F: " f.s -0.333333333333e0 f- fabs 1.e-12 f< confirm
-1 -3 rat>f .s ." F: " f.s 0.333333333333e0 f- fabs 1.e-12 f< confirm
1 -3 rat>f .s ." F: " f.s -0.333333333333e0 f- fabs 1.e-12 f< confirm
2 3 rat>f .s ." F: " f.s 0.666666666666e0 f- fabs 1.e-12 f< confirm
-2 3 rat>f .s ." F: " f.s -0.666666666666e0 f- fabs 1.e-12 f< confirm
-2 -3 rat>f .s ." F: " f.s 0.666666666666e0 f- fabs 1.e-12 f< confirm
2 -3 rat>f .s ." F: " f.s -0.666666666666e0 f- fabs 1.e-12 f< confirm
3 2 rat>f .s ." F: " f.s 1.5e0 f= confirm
-3 2 rat>f .s ." F: " f.s -1.5e0 f= confirm
-3 -2 rat>f .s ." F: " f.s 1.5e0 f= confirm
3 -2 rat>f .s ." F: " f.s -1.5e0 f= confirm
3 3 rat>f .s ." F: " f.s 1.e0 f= confirm
-3 3 rat>f .s ." F: " f.s -1.e0 f= confirm
-3 -3 rat>f .s ." F: " f.s 1.e0 f= confirm
3 -3 rat>f .s ." F: " f.s -1.e0 f= confirm

6 14 1 2rshift .s 3 7 d= confirm
12 28 2 2rshift .s 3 7 d= confirm
24 56 3 2rshift .s 3 7 d= confirm

6 14 ratTZshift .s 3 7 d= confirm
12 28 ratTZshift .s 3 7 d= confirm
24 56 ratTZshift .s 3 7 d= confirm

.s ." F: " f.s  \ <0> F: <0> =
0.0e0 f>rat .s RatZero d= confirm
-0.0e0 f>rat .s RatNegZero d= confirm
1.0e0 0.0e0  f/ f>rat .s RatPosInf d= confirm
-1.0e0 0.0e0 f/ f>rat .s RatNegInf d= confirm
0.0e0 0.0e0  f/ f>rat .s RatNan d= confirm
1.0e0 f>rat .s RatPosOne d= confirm
-1.0e0 f>rat .s RatNegOne d= confirm
2.0e10 f>rat .s RatPosMax d= confirm
-2.0e10 f>rat .s RatNegMax d= confirm
2.0e-11 f>rat .s RatPosMin d= confirm
-2.0e-11 f>rat .s RatNegMin d= confirm
1.5e0 f>rat .s 3 2 d= confirm
-1.5e0 f>rat .s -3 2 d= confirm
1.75e0 f>rat .s 7 4 d= confirm
-1.75e0 f>rat .s -7 4 d= confirm
1.1e0 f>rat .s 9227469 8388608 d= confirm
-1.1e0 f>rat .s -9227469 8388608 d= confirm
2147483647.0e0 f>rat .s 2147483647 1 d= confirm
-2147483647.0e0 f>rat .s -2147483647 1 d= confirm
1.0e0 2147483647.0e0 f/ f>rat .s 1 2147483647 d= confirm
-1.0e0 2147483647.0e0 f/ f>rat .s -1 2147483647 d= confirm
2.0e0 f>rat .s 2 1 d= confirm
4.0e0 f>rat .s 4 1 d= confirm
8.0e0 f>rat .s 8 1 d= confirm
16.0e0 f>rat .s 16 1 d= confirm
32.0e0 f>rat .s 32 1 d= confirm
64.0e0 f>rat .s 64 1 d= confirm
128.0e0 f>rat .s 128 1 d= confirm
256.0e0 f>rat .s 256 1 d= confirm
512.0e0 f>rat .s 512 1 d= confirm
1024.0e0 f>rat .s 1024 1 d= confirm
2048.0e0 f>rat .s 2048 1 d= confirm
4096.0e0 f>rat .s 4096 1 d= confirm
8192.0e0 f>rat .s 8192 1 d= confirm
16384.0e0 f>rat .s 16384 1 d= confirm
32768.0e0 f>rat .s 32768 1 d= confirm
65536.0e0 f>rat .s 65536 1 d= confirm
131072.0e0 f>rat .s 131072 1 d= confirm
262144.0e0 f>rat .s 262144 1 d= confirm
524288.0e0 f>rat .s 524288 1 d= confirm
1048576.0e0 f>rat .s 1048576 1 d= confirm
2097152.0e0 f>rat .s 2097152 1 d= confirm
4194304.0e0 f>rat .s 4194304 1 d= confirm
8388608.0e0 f>rat .s 8388608 1 d= confirm
16777216.0e0 f>rat .s 16777216 1 d= confirm
33554432.0e0 f>rat .s 33554432 1 d= confirm
67108864.0e0 f>rat .s 67108864 1 d= confirm
134217728.0e0 f>rat .s 134217728 1 d= confirm
268435456.0e0 f>rat .s 268435456 1 d= confirm
536870912.0e0 f>rat .s 536870912 1 d= confirm
1073741824.0e0 f>rat .s 1073741824 1 d= confirm
2147483648.0e0 f>rat .s RatPosMax d= confirm
4294967296.0e0 f>rat .s RatPosMax d= confirm

ratZero ratReduce .s ratZero d= confirm
ratNegZero ratReduce .s ratNegZero d= confirm
ratPosInf ratReduce .s ratPosInf d= confirm
ratNegInf ratReduce .s ratNegInf d= confirm
ratNaN ratReduce .s ratNaN d= confirm
ratPosOne ratReduce .s ratPosOne d= confirm
ratNegOne ratReduce .s ratNegOne d= confirm
ratPosMax ratReduce .s ratPosMax d= confirm
ratNegMax ratReduce .s ratNegMax d= confirm
ratPosMin ratReduce .s ratPosMin d= confirm
ratNegMin ratReduce .s ratNegMin d= confirm
123 123 ratReduce .s 1 1 d= confirm
18 123 ratReduce .s 6 41 d= confirm











0 1 2 3 ratComDen .s 2 3 d= -rot 0 3 d= and confirm
1 2 3 4 ratComDen .s 3 4 d= -rot 2 4 d= and confirm
2 3 4 5 ratComDen .s 12 15 d= -rot 10 15 d= and confirm
3 4 5 6 ratComDen .s 10 12 d= -rot 9 12 d= and confirm


























