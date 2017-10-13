¡
MD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.BAL\ManufacturerCampaign.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
BAL( +
{		 
public

 

class

  
ManufacturerCampaign

 %
:

& '!
IManufacturerCampaign

( =
{ 
private 
readonly 
	Interface "
." #+
IManufacturerCampaignRepository# B
_repoC H
=I J
nullK O
;O P
public  
ManufacturerCampaign #
(# $
	Interface$ -
.- .+
IManufacturerCampaignRepository. M
repoN R
)R S
{ 	
_repo 
= 
repo 
; 
} 	
public &
ManufacturerCampaignEntity )
GetCampaigns* 6
(6 7
uint7 ;
modelId< C
,C D
uintE I
cityIdJ P
,P Q,
 ManufacturerCampaignServingPagesR r
pageIds y
)y z
{ 	
return 
_repo 
. 
GetCampaigns %
(% &
modelId& -
,- .
cityId/ 5
,5 6
pageId7 =
)= >
;> ?
} 	
public 
bool +
SaveManufacturerIdInPricequotes 3
(3 4
uint4 8
pqId9 =
,= >
uint? C
dealerIdD L
)L M
{ 	
bool 
	isSuccess 
= 
false "
;" #
try   
{!! 
if"" 
("" 
pqId"" 
>"" 
$num"" 
&&"" 
dealerId""  (
>"") *
$num""+ ,
)"", -
{## 
NameValueCollection%% '
objNVC%%( .
=%%/ 0
new%%1 4
NameValueCollection%%5 H
(%%H I
)%%I J
;%%J K
objNVC&& 
.&& 
Add&& 
(&& 
$str&& )
,&&) *
pqId&&+ /
.&&/ 0
ToString&&0 8
(&&8 9
)&&9 :
)&&: ;
;&&; <
objNVC'' 
.'' 
Add'' 
('' 
$str'' -
,''- .
dealerId''/ 7
.''7 8
ToString''8 @
(''@ A
)''A B
)''B C
;''C D

SyncBWData(( 
.(( 
PushToQueue(( *
(((* +
$str((+ H
,((H I
DataBaseName((J V
.((V W
BW((W Y
,((Y Z
objNVC(([ a
)((a b
;((b c
	isSuccess** 
=** 
true**  $
;**$ %
}++ 
},, 
catch-- 
(-- 
	Exception-- 
ex-- 
)--  
{.. 

ErrorClass// 
objErr// !
=//" #
new//$ '

ErrorClass//( 2
(//2 3
ex//3 5
,//5 6
$str//7 r
)//r s
;//s t
}00 
return11 
	isSuccess11 
;11 
}22 	
}33 
}44 û
PD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.BAL\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str <
)< =
]= >
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
$str >
)> ?
]? @
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