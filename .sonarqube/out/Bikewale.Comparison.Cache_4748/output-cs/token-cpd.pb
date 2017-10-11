Û
HD:\work\bikewaleweb\Bikewale.Comparison.Cache\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str 4
)4 5
]5 6
[		 
assembly		 	
:			 

AssemblyDescription		 
(		 
$str		 !
)		! "
]		" #
[

 
assembly

 	
:

	 
!
AssemblyConfiguration

  
(

  !
$str

! #
)

# $
]

$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 
) 
] 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str 6
)6 7
]7 8
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
[## 
assembly## 	
:##	 

AssemblyVersion## 
(## 
$str## $
)##$ %
]##% &
[$$ 
assembly$$ 	
:$$	 

AssemblyFileVersion$$ 
($$ 
$str$$ (
)$$( )
]$$) *≤
SD:\work\bikewaleweb\Bikewale.Comparison.Cache\SponsoredComparisonCacheRepository.cs
	namespace 	
Bikewale
 
. 

Comparison 
. 
Cache #
{		 
public 

class .
"SponsoredComparisonCacheRepository 3
:4 5/
#ISponsoredComparisonCacheRepository6 Y
{ 
private 
readonly *
ISponsoredComparisonRepository 7
_repository8 C
=D E
nullF J
;J K
private 
readonly 
ICacheManager &
_cache' -
=. /
null0 4
;4 5
public .
"SponsoredComparisonCacheRepository 1
(1 2*
ISponsoredComparisonRepository2 P

repositoryQ [
,[ \
ICacheManager] j
cachek p
)p q
{ 	
_repository 
= 

repository $
;$ %
_cache 
= 
cache 
; 
} 	
public 
IEnumerable 
< &
SponsoredVersionEntityBase 5
>5 6)
GetActiveSponsoredComparisons7 T
(T U
)U V
{ 	
string 
key 
= 
$str 2
;2 3
IEnumerable   
<   &
SponsoredVersionEntityBase   2
>  2 3
activeComparisons  4 E
=  F G
null  H L
;  L M
try!! 
{"" 
activeComparisons## !
=##" #
_cache##$ *
.##* +
GetFromCache##+ 7
<##7 8
IEnumerable##8 C
<##C D&
SponsoredVersionEntityBase##D ^
>##^ _
>##_ `
(##` a
key##a d
,##d e
new##f i
TimeSpan##j r
(##r s
$num##s t
,##t u
$num##v x
,##x y
$num##z {
)##{ |
,##| }
(##~ 
)	## Ä
=>
##Å É
_repository
##Ñ è
.
##è ê+
GetActiveSponsoredComparisons
##ê ≠
(
##≠ Æ
)
##Æ Ø
)
##Ø ∞
;
##∞ ±
}%% 
catch&& 
(&& 
	Exception&& 
ex&& 
)&&  
{'' 

ErrorClass(( 
objErr(( !
=((" #
new(($ '

ErrorClass((( 2
(((2 3
ex((3 5
,((5 6
$str((7 y
)((y z
;((z {
})) 
return** 
activeComparisons** $
;**$ %
}++ 	
public11 
void11 -
!RefreshSpsonsoredComparisonsCache11 5
(115 6
)116 7
{22 	
string33 
key33 
=33 
$str33 2
;332 3
try44 
{55 
_cache66 
.66 
RefreshCache66 #
(66# $
key66$ '
)66' (
;66( )
}77 
catch88 
(88 
	Exception88 
ex88 
)88  
{99 

ErrorClass:: 
objErr:: !
=::" #
new::$ '

ErrorClass::( 2
(::2 3
ex::3 5
,::5 6
$str::7 }
)::} ~
;::~ 
};; 
}<< 	
}== 
}>> 