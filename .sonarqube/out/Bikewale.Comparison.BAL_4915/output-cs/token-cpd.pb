˘
BD:\work\bikewaleweb\Bikewale.Comparison.BAL\SponsoredComparison.cs
	namespace 	
Bikewale
 
. 

Comparison 
. 
BAL !
{ 
public 

class 
SponsoredComparison $
:% & 
ISponsoredComparison' ;
{ 
private 
readonly /
#ISponsoredComparisonCacheRepository <
_cache= C
=D E
nullF J
;J K
public 
SponsoredComparison "
(" #/
#ISponsoredComparisonCacheRepository# F
cacheG L
)L M
{ 	
_cache 
= 
cache 
; 
} 	
public &
SponsoredVersionEntityBase )
GetSponsoredVersion* =
(= >
string> D
targetVersionIdsE U
)U V
{ 	&
SponsoredVersionEntityBase &
sponsoredVersion' 7
=8 9
null: >
;> ?
try 
{ 
var   
versions   
=   
_cache   %
.  % &)
GetActiveSponsoredComparisons  & C
(  C D
)  D E
;  E F
if!! 
(!! 
versions!! 
!=!! 
null!!  $
&&!!% '
!!!( )
String!!) /
.!!/ 0
IsNullOrEmpty!!0 =
(!!= >
targetVersionIds!!> N
)!!N O
)!!O P
{"" 
IEnumerable## 
<##  
uint##  $
>##$ %
targets##& -
=##. /
targetVersionIds##0 @
.##@ A
Split##A F
(##F G
$char##G J
)##J K
.##K L
Select##L R
(##R S
v##S T
=>##U W
uint##X \
.##\ ]
Parse##] b
(##b c
v##c d
)##d e
)##e f
;##f g
sponsoredVersion$$ $
=$$% &
versions$$' /
.$$/ 0
FirstOrDefault$$0 >
($$> ?
m$$? @
=>$$A C
targets$$D K
.$$K L
Contains$$L T
($$T U
m$$U V
.$$V W
TargetVersionId$$W f
)$$f g
&&$$h j
!$$k l
targets$$l s
.$$s t
Contains$$t |
($$| }
m$$} ~
.$$~ 
SponsoredVersionId	$$ ë
)
$$ë í
)
$$í ì
;
$$ì î
}%% 
}&& 
catch'' 
('' 
	Exception'' 
ex'' 
)''  
{(( 

ErrorClass)) 
err)) 
=))  
new))! $

ErrorClass))% /
())/ 0
ex))0 2
,))2 3
String))4 :
.)): ;
Format)); A
())A B
$str	))B à
,
))à â
targetVersionIds
))ä ö
)
))ö õ
)
))õ ú
;
))ú ù
}** 
return++ 
sponsoredVersion++ #
;++# $
},, 	
}-- 
}.. Ò
FD:\work\bikewaleweb\Bikewale.Comparison.BAL\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str 2
)2 3
]3 4
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
$str 4
)4 5
]5 6
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