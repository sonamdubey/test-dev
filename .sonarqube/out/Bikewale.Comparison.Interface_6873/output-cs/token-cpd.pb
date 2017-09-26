
ID:\work\bikewaleweb\Bikewale.Comparison.Interface\ISponsoredComparison.cs
	namespace 	
Bikewale
 
. 

Comparison 
. 
	Interface '
{ 
public		 

	interface		  
ISponsoredComparison		 )
{

 &
SponsoredVersionEntityBase "
GetSponsoredVersion# 6
(6 7
string7 =
targetVersionIds> N
)N O
;O P
} 
} À
SD:\work\bikewaleweb\Bikewale.Comparison.Interface\ISponsoredComparisonRepository.cs
	namespace 	
Bikewale
 
. 

Comparison 
. 
	Interface '
{ 
public 

	interface *
ISponsoredComparisonRepository 3
{ 
SponsoredComparison "
GetSponsoredComparison 2
(2 3
)3 4
;4 5
IEnumerable 
< 
SponsoredComparison '
>' (#
GetSponsoredComparisons) @
(@ A
stringA G
statusesH P
)P Q
;Q R
uint #
SaveSponsoredComparison $
($ %
SponsoredComparison% 8
campaign9 A
)A B
;B C
bool ,
 SaveSponsoredComparisonBikeRules -
(- . 
VersionTargetMapping. B
rulesC H
)H I
;I J"
TargetSponsoredMapping 0
$GetSponsoredComparisonVersionMapping C
(C D
uintD H
comparisonidI U
,U V
uintW [
targetModelId\ i
,i j
uintk o
sponsoredModelId	p Ä
)
Ä Å
;
Å Ç
IEnumerable 
< 
SponsoredVersion $
>$ %/
#GetSponsoredComparisonSponsoredBike& I
(I J
uintJ N
comparisonidO [
)[ \
;\ ]
bool 1
%DeleteSponsoredComparisonBikeAllRules 2
(2 3
uint3 7
comparisonid8 D
)D E
;E F
bool <
0DeleteSponsoredComparisonBikeSponsoredModelRules =
(= >
uint> B
comparisonidC O
,O P
uintQ U
SponsoredmodelIdV f
)f g
;g h
bool >
2DeleteSponsoredComparisonBikeSponsoredVersionRules ?
(? @
uint@ D
comparisonidE Q
,Q R
uintS W
SponsoredversionIdX j
)j k
;k l
bool ;
/DeleteSponsoredComparisonBikeTargetVersionRules <
(< =
uint= A
comparisonidB N
,N O
uintP T
targetversionIdU d
)d e
;e f
bool +
ChangeSponsoredComparisonStatus ,
(, -
uint- 1
comparisonid2 >
,> ?
ushort@ F
statusG M
)M N
;N O
IEnumerable 
< &
SponsoredVersionEntityBase .
>. /)
GetActiveSponsoredComparisons0 M
(M N
)N O
;O P
} 
} ã
XD:\work\bikewaleweb\Bikewale.Comparison.Interface\ISponsoredComparisonCacheRepository.cs
	namespace 	
Bikewale
 
. 

Comparison 
. 
	Interface '
{ 
public

 

	interface

 /
#ISponsoredComparisonCacheRepository

 8
{ 
IEnumerable 
< &
SponsoredVersionEntityBase .
>. /)
GetActiveSponsoredComparisons0 M
(M N
)N O
;O P
void -
!RefreshSpsonsoredComparisonsCache .
(. /
)/ 0
;0 1
} 
} ˜
LD:\work\bikewaleweb\Bikewale.Comparison.Interface\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str 8
)8 9
]9 :
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
$str :
): ;
]; <
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
]$$) *