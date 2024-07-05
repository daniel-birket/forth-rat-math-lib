\ Notation:
\ num : single-precision signed integer numerator
\ den : single-precision signed integer denominator
\ "name" : name from input stream (not stack)
\ a-addr : address, perhaps from a variable

: GCF ( n1 n2 -- n ) \ rational "Greatest Common Factor"
    \ Return the positive signed integer greatest common factor
    \ of 2 signed non-0 ints
    begin
	?dup while
	    tuck mod  \ mod naturally swaps n1 n2 when n1 < n2
    repeat
    abs ;

: LCM ( n1 n2 -- n ) \ rational "Least Common Multiple"
    \ Return the positive signed integer least common multiple
    \ of two signed non-zero integers.
    2dup GCF
    dup if  \ if GCF <> 0
	*/ abs
    else
	drop 2drop 0  \ error: cleanup, return 0
    endif ;

: CTZ ( w -- u ) \ rational "Count Trailing Zeros"
    \  Count trailing zero bits of an single integer, returning int
    dup negate and  \ isolate trailing 1-bit
    32 swap  \ initialize count to 32
    dup if swap 1- swap endif
    dup $0000FFFF and if swap 16 - swap endif
    dup $00FF00FF and if swap 8 - swap endif
    dup $0F0F0F0F and if swap 4 - swap endif
    dup $33333333 and if swap 2 - swap endif
    dup $55555555 and if swap 1- swap endif
    drop ;  \ drop the input

: ratConstant ( num den "name" -- ) \ rational "rat-constant"
    \ Allocate and initialize a rational constant word
    create 2,
  does> ( -- num den )
    2@ ;

0 1 ratConstant ratZero  \ Zero or Positive Zero
0 -1 ratConstant ratNegZero  \ Negative Zero
1 0 ratConstant ratPosInf  \ Positive Infinity
-1 0 ratConstant ratNegInf  \ Negative Infinity
0 0 ratConstant ratNaN  \ Not-a-Number
1 1 ratConstant ratPosOne  \ +1
-1 1 ratConstant ratNegOne  \ -1
2147483647 1 ratConstant ratPosMax  \ Positive Max
-2147483647 1 ratConstant ratNegMax  \ Negative Max
1 2147483647 ratConstant ratPosMin  \ Positive Min
-1 2147483647 ratConstant ratNegMin  \ Negative Min
355 113 ratConstant ratPi6  \ pi to 6 decimal places
104348 33215 ratConstant ratPi9  \ pi to 9 decimal places
1264 465 ratConstant ratE6  \ e to 6 decimal places
23225 8544 ratConstant ratE9  \ e to 9 decimal places

: ratVariable ( “name” -- a-addr ) \ rational "rat-variable"
  \ Allocate and initialize a rational variable to zero.
  create RatZero 2, ;  \ Initialize to rational 0/1

\ : rat@ ( -- num den ) \ rational "rat-fetch"
\ rat@ is identical to 2@
' 2@ Alias rat@

\ : rat! ( num den -- ) \ rational "rat-store"
\ rat! is identical to 2!
' 2! Alias rat!

: n>rat ( num -- num 1 ) \ rational "int to rational"
    \ Convert a single signed integer to rational
    1 ;

: 1/n>rat ( n -- 1 den ) \ rational "inverse int to rational"
    \ Invert a single signed integer, converting to rational
    1 swap ;

: ratFixSign ( num1 den1 -- num2 den2 ) \ rational “Rational Fix Sign”
    \ Ensure that the denominator is not negative when the numerator is non-zero.
    dup 0< if \ denom < 0 ?
	over if \ numer <> 0 ?
	    swap negate swap  \ negate numer
	    negate  \ negate denom
	endif
    endif ;

: rat>n { num den -- n } \ rational "Rational to integer"
  num 0< den 0< xor if \ if the signs of numerator and denominator are opposite
      num den s>d 2 sm/rem swap drop -   \ bias the numerator down den/2...
  else
      num den s>d 2 sm/rem swap drop + \ bias the numerator up den/2...
  endif
  s>d den sm/rem swap drop ; \ then divide, dropping remainder

: rat>f ( num den -- ) ( F: -- f ) \ rational "Rational to Float"
    \ Convert a rational number to floating point
    2dup 0< swap 0= and if  \ is 0/neg aka -0
	2drop -0.0e0  \ handle -0e0 case
    else
	swap s>f s>f f/  \ s>f moves ints to float stack, reversing order.
    endif ;

: 2rshift ( n1 n2 u -- n3 n4 ) \ rational "Rational logical right shift"
    dup -rot ( n1 u n2 u )
    2swap rshift ( n2 u n3 )
    -rot rshift ; ( n3 n4 )

: ratTZshift ( num1 den1 -- num2 den2 ) \ rational "Rational trailing-zero shift"
    over CTZ ( num1 den1 ntz )
    over CTZ ( num1 den1 ntz dtz )
    min ( num1 den1 mintz )
    2rshift ; ( num2 den2 )

: ex8f23>rat { ex8 f23 -- num den }
    \ Convert a normal IEEE float exponent and fraction to rational.
    \ ex8: 8 bit unsigned 0-255 IEEE float exponent, right justified
    \ fr23 : 23-bit unsigned float fraction (sans implied 1 in bit 23)
    \ Does not handle Zero, -Zero, +/- infinity or NaN.
    \ Returns RatPosMax if exponent too high
    \ Returns RatPosMin if exponent too low
    f23 $00800000 or  \ base 24-bit unsigned numerator
    dup CTZ  \ trailing zeros is 0 to 23
    $00800000  \ base 24-bit unsigned denominator
    ex8 127 -  \ 2^exp exponent
    { n24 tz d24 exp }
    exp if  \ non-zero exponent
	exp 0< if  \ is negative ?
	    \ shift denominator left <= 7 and numerator right <= tz
	    exp -7 tz - >= if  \ exp in (-7-tz)..-1 range?
		exp negate tz <= if \ -exp in 1..tz range?
		    n24 exp negate rshift d24  \ rshift n24 by -exp
		else
		    n24 tz rshift  \ rshift n24 by full tz, if any
		    d24 exp negate tz - lshift  \ and lshift d24 the rest
		endif
	    else
		RatPosMin
	    endif
	else \ is positive
	    \ shift numerator left <= 7 and denominator right <= 23
	    exp 30 <= if  \ exp in 1..30 range?
		exp 23 <= if \ exp in 1..23 range?
		    n24 d24 exp rshift  \ rshift d24 by exp
		else
		    n24 exp 23 - lshift  \ lshift n24 by exp-23
		    d24 23 rshift  \ and rshift d24 by 23
		endif
	    else
		RatPosMax
	    endif
	endif
    else \ exp is zero. Use base n24 d24
	n24 d24
    endif ( num den )
    ratTZshift ;  \ eliminate any common trailing zeros

variable f32ieee  \ Entire 32-bit ieee float buffer
: f>rat ( f -- num den)
    \ Convert a floating point number to a rational number.
    f32ieee sf!  \ store the given float as 32 bits
    f32ieee @ $007FFFFF and  \ 23-bit fraction part of float, with bit 23 set
    f32ieee @ $7F800000 and 23 rshift  \ exponent of float, shifted 
    f32ieee @ $80000000 and  \ negative sign of float
    { f32frac f32expo f32sign }
    f32expo $FF = if  \ expo = special 255 ?
	f32frac if  \ frac <> 0 ?
	    ratNan  \ non-a-number
	else  \ +/- inf
	    f32sign if  \ negative ?
		ratNegInf
	    else
		ratPosInf
	    endif
	endif
    else
	f32expo 0= f32frac 0= and if  \ expo = 0 and frac = 0
	    f32sign if  \ negative ?
		ratNegZero
	    else
		ratZero
	    endif
	else
	    f32expo f32frac ex8f23>rat  \ Convert  exponent and fraction to rational
	    f32sign if  \ is negative?
		swap negate swap
	    endif  \ negate the numerator
	endif
    endif ;

: ratReduce ( num1 den1 -- num2 den2 )
    \ if both terms are non-zero and the GCF is > 1, reduce to lowest terms.
    dup if  \ numer <> 0 ?
	over if  \ denom <> 0 ?
	    2dup GCF  \ find GCF (which is positive)
	    dup 1- if   \ GCF > 1 ? ( -- num1 den1 GCF )
		dup -rot ( -- num1 GCF den1 GCF )
		2swap / -rot  \ numer=numer/GCF ( -- num2 den1 GCF )
		/  \ denom=denom/GCF ( -- num2 den2 )
	    else
		drop  \ drop GCF of 1
	    endif
	endif
    endif ;

: ratNorm ( num1 den1 -- num2 den2 ) \ rational "Rational Normalize"
    ratFixSign  \ Ensure non-negative den1)
    dup if  \ "? 0<>" den non-zero
	over if  \ "0<> 0<>" normal
	    ratReduce  \ Reduce fraction by factoring GCF
	else  \ "0= 0<>" some zero
	    dup 0< if  \ "0= 0<" neg zero
		ratNegZero
	    else  \ "0= 0>" normal zero
		ratZero
	    endif
	endif
    else  \ "? 0=" infinity or NaN
	over if  \ "0<> 0=" +/- Infinity
	    over 0< if  \ "0< 0=" -Infinity
		ratNegInf
	    else  \ "0> 0=" +Infinity
		ratPosInf
	    endif
	else  \ "0= 0=" NaN
	    ratNaN
	endif
    endif ;

: ratMediant { numA denA numB denB -- num den } \ rational "rat-mediantt"
    \ Compute the mediant of two ratios
;



: rat* { numA denA numB denB -- num den } \ Multiply 2 rational numbers
    numA numB *
    denA denB * ;

' swap Alias rat1/

: rat/ ( numA denA numB denB -- num den ) \ divide two rational numbers
    rat1/ rat* ;

: ratComDen { numA denA numB denB -- numC denC numD denD } \ Convert two ratios to use a common denominator
    denA denB GCF { cf } \ remember the GCF of the denominators
    numA denB cf */  \ get numC
    denA denB cf */  \ get denC
    numB denA cf */  \ get numD
    over ;  \ get denD

: rat+ ( numA denA numB denB -- numC denC ) \ rational "Rat Plus"
    ratComDen { numC denC numD denD } \ make denominators common
    numC numD + denC ;  \ Add numerators and use either denominator

: ratNegate ( num1 den1 -- num2 den2 ) \ negate a rational number
    swap negate swap ;  \ Negate the numerator and replace it.

: rat- ( numA denA numB denB -- numC den C ) \ subtract ratioA - ratioB
    ratNegate rat+ ;  \ Subtract by negating ratioB and adding.

: rat1+ ( num1 den1 -- num2 den2 ) \ add one to a rational
    tuck + swap ;  \ Add den1 to num1 and replace it

: rat1- ( num1 den1 -- num2 den2 )  \ subtract 1 from a rational
    tuck - swap ;  \ Subtract den1 from num1 and replace it.

: ratAbs ( num1 den1 -- num2 den2 ) \ absolute value
    dup 0< if \ numer < 0 ?
	negate
    endif
    swap
    dup 0< if \ denom < 0 ?
	negate
    endif
    swap ;

: rat= ( num1 den1 num2 den2 -- f ) \ rational "rat-equals"
    \ compare two rational numbers for equality
    \ normalizes both rational, then tests for equality with d=
    2swap ratNormal 2swap ratNormal
    d= ;












