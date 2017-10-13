¡
TD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Interface\IManufacturerCampaign.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
	Interface( 1
{ 
public		 

	interface		 !
IManufacturerCampaign		 *
{

 &
ManufacturerCampaignEntity "
GetCampaigns# /
(/ 0
uint0 4
modelId5 <
,< =
uint> B
cityIdC I
,I J,
 ManufacturerCampaignServingPagesK k
pageIdl r
)r s
;s t
bool +
SaveManufacturerIdInPricequotes ,
(, -
uint- 1
pqId2 6
,6 7
uint7 ;
dealerId< D
)D E
;E F
} 
} ®$
^D:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Interface\IManufacturerCampaignRepository.cs
	namespace

 	
Bikewale


 
.

  
ManufacturerCampaign

 '
.

' (
	Interface

( 1
{ 
public 

	interface +
IManufacturerCampaignRepository 4
{ #
ConfigureCampaignEntity #
GetManufacturerCampaign  7
(7 8
uint8 <
dealerId= E
,E F
uintG K

campaignIdL V
)V W
;W X"
CampaignPropertyEntity -
!GetManufacturerCampaignProperties @
(@ A
uintA E

campaignIdF P
)P Q
;Q R
uint $
saveManufacturerCampaign %
(% &!
ConfigureCampaignSave& ;
objCampaign< G
)G H
;H I
IEnumerable 
< 
ManufacturerEntity &
>& ' 
GetManufacturersList( <
(< =
)= >
;> ?
void )
saveManufacturerCampaignPopup *
(* +%
ManufacturerCampaignPopup+ D
objDataE L
)L M
;M N%
ManufacturerCampaignPopup !(
getManufacturerCampaignPopup" >
(> ?
uint? C

campaignIdD N
)N O
;O P
IEnumerable 
< 
BikeMakeEntity "
>" #
GetBikeMakes$ 0
(0 1
)1 2
;2 3
IEnumerable 
< 
BikeModelEntity #
># $
GetBikeModels% 2
(2 3
uint3 7
makeId8 >
)> ?
;? @
IEnumerable 
< 
StateEntity 
>  
	GetStates! *
(* +
)+ ,
;, -
IEnumerable 
< 

CityEntity 
> 
GetCitiesByState  0
(0 1
uint1 5
stateId6 =
)= >
;> ?,
 ManufacturerCampaignRulesWrapper ((
GetManufacturerCampaignRules) E
(E F
uintF J

campaignIdK U
)U V
;V W
bool )
SaveManufacturerCampaignRules *
(* +
uint+ /

campaignId0 :
,: ;
string< B
modelIdsC K
,K L
stringM S
stateIdsT \
,\ ]
string^ d
cityIdse l
,l m
booln r

isAllIndias }
,} ~
uint	 É
userId
Ñ ä
)
ä ã
;
ã å
bool +
DeleteManufacturerCampaignRules ,
(, -
uint- 1

campaignId2 <
,< =
uint> B
modelIdC J
,J K
uintL P
stateIdQ X
,X Y
uintZ ^
cityId_ e
,e f
uintg k
userIdl r
,r s
boolt x

isAllIndia	y É
)
É Ñ
;
Ñ Ö
bool .
"SaveManufacturerCampaignProperties /
(/ 0 
CampaignPropertiesVM0 D
objCampaignE P
)P Q
;Q R
Entities 
. &
ManufacturerCampaignEntity +
GetCampaigns, 8
(8 9
uint9 =
modelId> E
,E F
uintG K
cityIdL R
,R S,
 ManufacturerCampaignServingPagesT t
pageIdu {
){ |
;| }
uint (
SaveManufacturerCampaignLead )
() *
uint* .
dealerid/ 7
,7 8
uint9 =
pqId> B
,B C
UInt64D J

customerIdK U
,U V
stringW ]
customerName^ j
,j k
stringl r
customerEmail	s Ä
,
Ä Å
string
Ç à
customerMobile
â ó
,
ó ò
uint
ô ù
leadSourceId
û ™
,
™ ´
string
¨ ≤
utma
≥ ∑
,
∑ ∏
string
π ø
utmz
¿ ƒ
,
ƒ ≈
string
∆ Ã
deviceId
Õ ’
,
’ ÷
uint
◊ €

campaignId
‹ Ê
,
Ê Á
uint
Ë Ï
leadId
Ì Û
)
Û Ù
;
Ù ı
bool #
ResetTotalLeadDelivered $
($ %
uint% )

campaignId* 4
,4 5
uint6 :
userId; A
)A B
;B C
} 
}   Å
VD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Interface\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str B
)B C
]C D
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
$str D
)D E
]E F
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