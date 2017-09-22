Ê	
ED:\work\bikewaleweb\BikewaleOpr.Interface\Banner\IBannerRepository.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
Banner  &
{ 
public 

	interface 
IBannerRepository &
{		 
BannerVM

 
GetBannerDetails

 !
(

! "
uint

" &
bannerId

' /
)

/ 0
;

0 1
uint "
SaveBannerBasicDetails #
(# $
BannerVM$ ,
	objBanner- 6
)6 7
;7 8
bool  
SaveBannerProperties !
(! "
BannerDetails" /
	objBanner0 9
,9 :
uint; ?

platformId@ J
,J K
uintL P

campaignIdQ [
)[ \
;\ ]
IEnumerable 
< 
BannerProperty "
>" #

GetBanners$ .
(. /
uint/ 3
bannerStatus4 @
)@ A
;A B
bool 
ChangeBannerStatus 
(  
uint  $
bannerId% -
,- .
UInt16/ 5
bannerStatus6 B
)B C
;C D
} 
} ç
WD:\work\bikewaleweb\BikewaleOpr.Interface\BikeColorImages\IColorImagesBikeRepository.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
BikeColorImages  /
{ 
public		 

	interface		 &
IColorImagesBikeRepository		 /
{

 
uint 
FetchPhotoId 
( #
ColorImagesBikeEntities 1
objBikeColorDetails2 E
)E F
;F G
bool "
DeleteBikeColorDetails #
(# $
uint$ (
modelid) 0
)0 1
;1 2
} 
} ≈
JD:\work\bikewaleweb\BikewaleOpr.Interface\BikeData\IBikeMakesRepository.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
BikeData  (
{ 
public 

	interface  
IBikeMakesRepository )
{ 
IEnumerable 
< 
BikeMakeEntityBase &
>& '
GetMakes( 0
(0 1
string1 7
requestType8 C
)C D
;D E
IEnumerable 
< 
BikeMakeEntityBase &
>& '
GetMakes( 0
(0 1
ushort1 7
RequestType8 C
)C D
;D E
IEnumerable 
< 
BikeMakeEntity "
>" #
GetMakesList$ 0
(0 1
)1 2
;2 3
void 
AddMake 
( 
BikeMakeEntity #
make$ (
,( )
ref* -
short. 3
isMakeExist4 ?
,? @
refA D
intE H
makeIdI O
)O P
;P Q
void 

UpdateMake 
( 
BikeMakeEntity &
make' +
)+ ,
;, -
void 

DeleteMake 
( 
int 
makeId "
," #
int$ '
	updatedBy( 1
)1 2
;2 3
SynopsisData 
Getsynopsis  
(  !
int! $
makeId% +
)+ ,
;, -
void 
UpdateSynopsis 
( 
int 
makeId  &
,& '
int( +
	updatedBy, 5
,5 6
SynopsisData7 C
objSynopsisD O
)O P
;P Q
IEnumerable 
< 
BikeModelEntityBase '
>' (
GetModelsByMake) 8
(8 9
EnumBikeType9 E
requestTypeF Q
,Q R
uintS W
makeIdX ^
)^ _
;_ `
} 
} ˜
AD:\work\bikewaleweb\BikewaleOpr.Interface\BikeData\IBikeModels.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
BikeData  (
{ 
public 

	interface 
IBikeModels  
{ 0
$UsedBikeImagesByMakeNotificationData ,0
$GetPendingUsedBikesWithoutModelImage- Q
(Q R
)R S
;S T
IEnumerable 
< 
BikeModelsByMake $
>$ %*
GetModelsWithMissingColorImage& D
(D E
)E F
;F G
} 
} ƒ
KD:\work\bikewaleweb\BikewaleOpr.Interface\BikeData\IBikeModelsRepository.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
BikeData  (
{ 
public 

	interface !
IBikeModelsRepository *
{ 
IEnumerable 
< 
BikeModelEntityBase '
>' (
	GetModels) 2
(2 3
uint3 7
makeId8 >
,> ?
string@ F
requestTypeG R
)R S
;S T
IEnumerable 
< 
BikeModelEntityBase '
>' (
	GetModels) 2
(2 3
uint3 7
makeId8 >
,> ?
ushort@ F
requestTypeG R
)R S
;S T
void 
SaveModelUnitSold 
( 
string %
list& *
,* +
DateTime, 4
date5 9
)9 :
;: ;
SoldUnitData 
GetLastSoldUnitData (
(( )
)) *
;* +
uint 
FetchPhotoId 
( $
UsedBikeModelImageEntity 2
objModelImageEntity3 F
)F G
;G H
bool $
DeleteUsedBikeModelImage %
(% &
uint& *
modelId+ 2
)2 3
;3 4
IEnumerable 
< "
UsedBikeModelImageData *
>* +'
GetUsedBikeModelImageByMake, G
(G H
uintH L
makeIdM S
)S T
;T U*
UsedBikeImagesNotificationData &0
$GetPendingUsedBikesWithoutModelImage' K
(K L
)L M
;M N
IEnumerable 
< 
BikeModelMailEntity '
>' (
GetModelsByMake) 8
(8 9
uint9 =
makeId> D
,D E
stringF L
hostUrlM T
,T U
stringV \
oldMakeMasking] k
,k l
stringm s
newMakeMasking	t Ç
)
Ç É
;
É Ñ
IEnumerable 
< 
BikeMakeModelData %
>% &*
GetModelsWithMissingColorImage' E
(E F
)F G
;G H
bool   
UpdateInquiryAsSold    
(    !
uint  ! %
	inquiryId  & /
)  / 0
;  0 1
IEnumerable!! 
<!! !
BikeVersionEntityBase!! )
>!!) *
GetVersionsByModel!!+ =
(!!= >
EnumBikeType!!> J
requestType!!K V
,!!V W
uint!!X \
modelId!!] d
)!!d e
;!!e f
}"" 
}## ø
CD:\work\bikewaleweb\BikewaleOpr.Interface\BikeData\IBikeVersions.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
BikeData  (
{ 
public

 

	interface

 
IBikeVersions

 "
{ 
IEnumerable 
< !
BikeVersionEntityBase )
>) *
GetVersions+ 6
(6 7
uint7 ;
modelId< C
,C D
stringE K
requestTypeL W
)W X
;X Y
} 
} ∫
KD:\work\bikewaleweb\BikewaleOpr.Interface\Dealers\IDealerPriceRepository.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
Dealers  '
{ 
public 

	interface "
IDealerPriceRepository +
{ 
ICollection 
< 
PriceCategoryEntity '
>' (!
GetAllPriceCategories) >
(> ?
)? @
;@ A
bool 
SaveBikeCategory 
( 
string $
categoryName% 1
)1 2
;2 3!
DealerPriceBaseEntity 
GetDealerPrices -
(- .
uint. 2
cityId3 9
,9 :
uint; ?
makeId@ F
,F G
uintH L
dealerIdM U
)U V
;V W
bool 
DeleteVersionPrices  
(  !
uint! %
dealerId& .
,. /
uint0 4
cityId5 ;
,; <
string= C
versionIdListD Q
)Q R
;R S
bool 
SaveDealerPrices 
( 
string $
dealerIdList% 1
,1 2
string3 9

cityIdList: D
,D E
stringF L
versionIdListM Z
,Z [
string\ b

itemIdListc m
,m n
stringo u
itemvValueList	v Ñ
,
Ñ Ö
uint
Ü ä
	enteredBy
ã î
)
î ï
;
ï ñ
} 
} á
@D:\work\bikewaleweb\BikewaleOpr.Interface\BikeData\IBikeMakes.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
BikeData  (
{ 
public 

	interface 

IBikeMakes 
{ 
IEnumerable 
< 
BikeModelEntityBase '
>' (
GetModelsByMake) 8
(8 9
EnumBikeType9 E
requestTypeF Q
,Q R
uintS W
makeIdX ^
)^ _
;_ `
IEnumerable 
< 
BikeMakeEntityBase &
>& '
GetMakes( 0
(0 1
ushort1 7
requestType8 C
)C D
;D E
} 
} œ
RD:\work\bikewaleweb\BikewaleOpr.Interface\BikePricing\IShowroomPricesRepository.cs
	namespace

 	
BikewaleOpr


 
.

 
	Interface

 
.

  
Dealers

  '
{ 
public 

	interface %
IShowroomPricesRepository .
{ 
IEnumerable 
< 
	BikePrice 
> 
GetBikePrices ,
(, -
uint- 1
makeId2 8
,8 9
uint: >
cityId? E
)E F
;F G
bool 
SaveBikePrices 
( 
string "
versionPriceList# 3
,3 4
string5 ;

citiesList< F
,F G
intH K
	updatedByL U
)U V
;V W!
PriceMonitoringEntity %
GetPriceMonitoringDetails 7
(7 8
uint8 <
makeId= C
,C D
uintE I
modelIdJ Q
,Q R
uintS W
stateIdX _
)_ `
;` a
} 
} í
LD:\work\bikewaleweb\BikewaleOpr.Interface\Comparison\ISponsoredComparison.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  

Comparison  *
{		 
public

 

	interface

  
ISponsoredComparison

 )
{ 
SponsoredComparison "
GetSponsoredComparison 2
(2 3
)3 4
;4 5
IEnumerable 
< 
SponsoredComparison '
>' (#
GetSponsoredComparisons) @
(@ A%
SponsoredComparisonStatusA Z
status[ a
)a b
;b c
bool #
SaveSponsoredComparison $
($ %
SponsoredComparison% 8
campaign9 A
)A B
;B C
bool ,
 SaveSponsoredComparisonBikeRules -
(- .
). /
;/ 0"
TargetSponsoredMapping 0
$GetSponsoredComparisonVersionMapping C
(C D
uintD H
camparisonIdI U
,U V
uintW [
sponsoredModelId\ l
)l m
;m n
dynamic /
#GetSponsoredComparisonSponsoredBike 3
(3 4
uint4 8
camparisonId9 E
)E F
;F G
bool 1
%DeleteSponsoredComparisonBikeAllRules 2
(2 3
uint3 7
camparisonId8 D
)D E
;E F
bool <
0DeleteSponsoredComparisonBikeSponsoredModelRules =
(= >
uint> B
camparisonIdC O
,O P
uintQ U
SponsoredmodelIdV f
)f g
;g h
bool >
2DeleteSponsoredComparisonBikeSponsoredVersionRules ?
(? @
uint@ D
camparisonIdE Q
,Q R
uintS W
SponsoredversionIdX j
)j k
;k l
bool ;
/DeleteSponsoredComparisonBikeTargetVersionRules <
(< =
uint= A
camparisonIdB N
,N O
uintP T
targetversionIdU d
)d e
;e f
} 
} û	
]D:\work\bikewaleweb\BikewaleOpr.Interface\ConfigurePageMetas\IConfigurePageMetasRepository.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
ConfigurePageMetas  2
{		 
public 

	interface  
IPageMetasRepository )
{ 
IEnumerable 
< 

PageEntity 
> 
GetPagesList  ,
(, -
)- .
;. /
bool 
SavePageMetas 
( 
PageMetasEntity *
objMetas+ 3
)3 4
;4 5
PageMetasEntity 
GetPageMetasById (
(( )
uint) -

pageMetaId. 8
)8 9
;9 :
bool  
UpdatePageMetaStatus !
(! "
uint" &
id' )
,) *
ushort+ 1
status2 8
)8 9
;9 :
IEnumerable 
< 
PageMetaEntity "
>" #
GetPageMetas$ 0
(0 1
uint1 5
pageMetaStatus6 D
)D E
;E F
} 
} Æ	
OD:\work\bikewaleweb\BikewaleOpr.Interface\ContractCampaign\IContractCampaign.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
ContractCampaign  0
{ 
public

 

	interface

 
IContractCampaign

 &
{ 
IEnumerable 
< 
MaskingNumber !
>! " 
GetAllMaskingNumbers# 7
(7 8
uint8 <
dealerId= E
)E F
;F G
bool 

IsCCMapped 
( 
uint 
dealerId %
,% &
uint' +

contractId, 6
,6 7
uint8 <

campaignId= G
)G H
;H I
bool  
RelaseMaskingNumbers !
(! "
uint" &
dealerId' /
,/ 0
int1 4
userId5 ;
,; <
string= C
maskingNumbersD R
)R S
;S T
bool #
AddCampaignContractData $
($ %'
ContractCampaignInputEntity% @
	_ccInputsA J
)J K
;K L
} 
} ¶%
WD:\work\bikewaleweb\BikewaleOpr.Interface\ContractCampaign\IDealerCampaignRepository.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
ContractCampaign  0
{ 
public 

	interface %
IDealerCampaignRepository .
{  
DealerCampaignEntity !
FetchBWDealerCampaign 2
(2 3
uint3 7

campaignId8 B
)B C
;C D
ICollection 
< "
CallToActionEntityBase *
>* +$
FetchDealerCallToActions, D
(D E
)E F
;F G
bool "
UpdateBWDealerCampaign #
(# $
bool$ (
isActive) 1
,1 2
int3 6

campaignId7 A
,A B
intC F
userIdG M
,M N
intO R
dealerIdS [
,[ \
int] `

contractIda k
,k l
stringm s
maskingNumber	t Å
,
Å Ç
string
É â

dealerName
ä î
,
î ï
string
ñ ú
dealerEmailId
ù ™
,
™ ´
int
¨ Ø
dailyleadlimit
∞ æ
,
æ ø
ushort
¿ ∆
callToAction
« ”
,
” ‘
bool
’ Ÿ 
isBookingAvailable
⁄ Ï
=
Ì Ó
false
Ô Ù
)
Ù ı
;
ı ˆ
int "
InsertBWDealerCampaign "
(" #
bool# '
isActive( 0
,0 1
int2 5
userId6 <
,< =
int> A
dealerIdB J
,J K
intL O

contractIdP Z
,Z [
string\ b
maskingNumberc p
,p q
stringr x

dealerName	y É
,
É Ñ
string
Ö ã
dealerEmailId
å ô
,
ô ö
int
õ û
dailyleadlimit
ü ≠
,
≠ Æ
ushort
Ø µ
callToAction
∂ ¬
,
¬ √
bool
ƒ » 
isBookingAvailable
… €
=
‹ ›
false
ﬁ „
)
„ ‰
;
‰ Â
ICollection 
< 
BikeMakeEntityBase &
>& '
MakesByDealerCity( 9
(9 :
uint: >
cityId? E
)E F
;F G
ICollection 
< 
DealerEntityBase $
>$ %
DealersByMakeCity& 7
(7 8
uint8 <
cityId= C
,C D
uintE I
makeIdJ P
,P Q
boolR V
activecontractW e
)e f
;f g
ICollection 
< '
DealerCampaignDetailsEntity /
>/ 0
DealerCampaigns1 @
(@ A
uintA E
dealerIdF N
,N O
uintP T
cityIdU [
,[ \
uint] a
makeIdb h
,h i
boolj n
activecontracto }
)} ~
;~ 
DealerCampaignArea (
GetMappedDealerCampaignAreas 7
(7 8
uint8 <
dealerId= E
)E F
;F G
void )
SaveDealerCampaignAreaMapping *
(* +
uint+ /
dealerId0 8
,8 9
uint: >

campaignid? I
,I J
ushortK Q!
campaignServingStatusR g
,g h
ushorti o
servingRadiusp }
,} ~
string	 Ö

cityIdList
Ü ê
,
ê ë
string
í ò
stateIdList
ô §
)
§ •
;
• ¶
DealerAreaDistance $
GetDealerToAreasDistance 3
(3 4
uint4 8
dealerId9 A
,A B
ushortC I!
campaignServingStatusJ _
,_ `
ushorta g
servingRadiush u
,u v
stringw }

cityIdList	~ à
,
à â
string
ä ê
stateIdList
ë ú
)
ú ù
;
ù û
DealerAreaDistance %
GetDealerAreasWithLatLong 4
(4 5
uint5 9
dealerId: B
,B C
stringD J
	areasListK T
)T U
;U V
void &
SaveAdditionalAreasMapping '
(' (
uint( ,
dealerId- 5
,5 6
string7 =
	areasList> G
)G H
;H I
void '
DeleteAdditionalMappedAreas (
(( )
uint) -
dealerId. 6
,6 7
string8 >
areadIdList? J
)J K
;K L
}   
}!! ¸
=D:\work\bikewaleweb\BikewaleOpr.Interface\ICommuteDistance.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
{		 
public 

	interface 
ICommuteDistance %
{ 
bool 
SaveCampaignAreas 
( 
uint #
dealerId$ ,
,, -
uint. 2

campaignid3 =
,= >
ushort? E!
campaignServingStatusF [
,[ \
ushort] c
servingRadiusd q
,q r
strings y

cityIdList	z Ñ
,
Ñ Ö
string
Ü å
stateIdList
ç ò
)
ò ô
;
ô ö
bool '
SaveAdditionalCampaignAreas (
(( )
uint) -
dealerId. 6
,6 7
string8 >

areaIdList? I
)I J
;J K
bool !
UpdateCommuteDistance "
(" #
uint# '
dealerId( 0
,0 1
DealerAreaDistance2 D
objDealerAreaDistE V
)V W
;W X
} 
} Á
AD:\work\bikewaleweb\BikewaleOpr.Interface\Dealers\IDealerPrice.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
Dealers  '
{ 
public

 

	interface

 
IDealerPrice

 !
{ 
IEnumerable 
< $
DealerVersionPriceEntity ,
>, - 
GetDealerPriceQuotes. B
(B C
uintC G
cityIdH N
,N O
uintP T
makeIdU [
,[ \
uint] a
dealerIdb j
)j k
;k l
bool $
DeleteVersionPriceQuotes %
(% &
uint& *
dealerId+ 3
,3 4
uint5 9
cityId: @
,@ A
IEnumerableB M
<M N
uintN R
>R S

versionIdsT ^
)^ _
;_ `
bool "
SaveVersionPriceQuotes #
(# $
IEnumerable$ /
</ 0
uint0 4
>4 5
	dealerIds6 ?
,? @
IEnumerableA L
<L M
uintM Q
>Q R
cityIdsS Z
,Z [
IEnumerable\ g
<g h
uinth l
>l m

versionIdsn x
,x y
IEnumerable 
< 
uint 
> 
itemIds &
,& '
IEnumerable( 3
<3 4
uint4 8
>8 9

itemValues: D
,D E
uintF J
	enteredByK T
)T U
;U V,
 UpdatePricingRulesResponseEntity ("
SaveVersionPriceQuotes) ?
(? @
IEnumerable@ K
<K L
uintL P
>P Q
	dealerIdsR [
,[ \
IEnumerable] h
<h i
uinti m
>m n
cityIdso v
,v w
IEnumerable	x É
<
É Ñ
uint
Ñ à
>
à â

versionIds
ä î
,
î ï
IEnumerable 
< 
uint 
> 
itemIds &
,& '
IEnumerable( 3
<3 4
uint4 8
>8 9

itemValues: D
,D E
IEnumerableF Q
<Q R
uintR V
>V W
bikeModelIdsX d
,d e
IEnumerablef q
<q r
stringr x
>x y
bikeModelNames	z à
,
à â
uint
ä é
	enteredBy
è ò
,
ò ô
uint
ö û
makeId
ü •
)
• ¶
;
¶ ß
} 
} ª
>D:\work\bikewaleweb\BikewaleOpr.Interface\IDealerPriceQuote.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
{ 
public 

	interface 
IDealerPriceQuote &
{ 
bool 
DeleteVersionPrices  
(  !
uint! %
dealerId& .
,. /
uint0 4
cityId5 ;
,; <
string= C
versionIdListD Q
)Q R
;R S
List 
< 
PQ_Price 
>  
GetBikeCategoryItems +
(+ ,
string, 2
catgoryList3 >
)> ?
;? @
bool 
SaveDealerPrice 
( 
uint !
dealerId" *
,* +
uint, 0
	versionId1 :
,: ;
uint< @
cityIdA G
,G H
UInt16I O
itemIdP V
,V W
UInt32X ^
	itemValue_ h
)h i
;i j
bool 
SaveDealerPrice 
( 
	DataTable &
dt' )
)) *
;* +
DataSet 
GetDealerPrices 
(  
uint  $
cityId% +
,+ ,
uint- 1
makelId2 9
,9 :
uint; ?
dealerId@ H
)H I
;I J
bool 
MapDealerWithArea 
( 
uint #
dealerId$ ,
,, -
string. 4

areaIdList5 ?
)? @
;@ A
bool 
UnmapDealer 
( 
uint 
dealerId &
,& '
string( .

areaIdList/ 9
)9 :
;: ;
List 
< 
DealerAreaDetails 
>  
GetDealerAreaDetails  4
(4 5
uint5 9
cityId: @
)@ A
;A B
void 
GetAreaLatLong 
( 
uint  
areaId! '
,' (
out) ,
double- 3
	lattitude4 =
,= >
out? B
doubleC I
	longitudeJ S
)S T
;T U
List 
< 
DealerLatLong 
> 
GetDealersLatLong -
(- .
uint. 2
	versionId3 <
,< =
uint> B
areaIdC I
)I J
;J K"
DealerPriceQuoteEntity %
GetPriceQuoteForAllDealer 8
(8 9
uint9 =
	versionId> G
,G H
uintI M
cityIdN T
,T U
stringV \
	dealerIds] f
)f g
;g h
string #
AddRulesOnPriceUpdation &
(& '
string' -
	modelList. 7
,7 8
uint9 =
dealerId> F
,F G
uintH L
makeIdM S
,S T
uintU Y
	updatedByZ c
)c d
;d e
} 
} ÷D
5D:\work\bikewaleweb\BikewaleOpr.Interface\IDealers.cs
	namespace		 	
BikewaleOpr		
 
.		 
	Interface		 
{

 
public 

	interface 
IDealers 
{ 
IEnumerable 
< 
FacilityEntity "
>" #
GetDealerFacilities$ 7
(7 8
uint8 <
dealerId= E
)E F
;F G
IEnumerable 
< 
DealerMakeEntity $
>$ %
GetDealersByCity& 6
(6 7
UInt327 =
cityId> D
)D E
;E F
UInt16 
SaveDealerFacility !
(! "
FacilityEntity" 0
objData1 8
)8 9
;9 :
bool  
UpdateDealerFacility !
(! "
FacilityEntity" 0
objData1 8
)8 9
;9 :
	DataTable 
GetOfferTypes 
(  
)  !
;! "
EMI  
GetDealerLoanAmounts  
(  !
uint! %
dealerId& .
). /
;/ 0
List 
< 
OfferEntity 
> 
GetDealerOffers )
() *
int* -
dealerId. 6
)6 7
;7 8
bool 
SaveDealerOffer 
( 
int  
dealerId! )
,) *
uint+ /
userId0 6
,6 7
int8 ;
cityId< B
,B C
stringD J
modelIdK R
,R S
intT W
offercategoryIdX g
,g h
stringi o
	offerTextp y
,y z
int{ ~
?~ 

offerValue
Ä ä
,
ä ã
DateTime
å î
offervalidTill
ï £
,
£ §
bool
• ©
isPriceImpact
™ ∑
,
∑ ∏
string
π ø
terms
¿ ≈
)
≈ ∆
;
∆ «
bool 
DeleteDealerOffer 
( 
string %
offerId& -
)- .
;. /
bool  
SaveBikeAvailability !
(! "
uint" &
dealerId' /
,/ 0
uint1 5
bikemodelId6 A
,A B
uintC G
?G H
bikeversionIdI V
,V W
ushortX ^
	numOfDays_ h
)h i
;i j
List 
< 
OfferEntity 
> 
GetBikeAvailability -
(- .
uint. 2
dealerId3 ;
); <
;< =
bool  
EditAvailabilityDays !
(! "
int" %
availabilityId& 4
,4 5
int6 9
days: >
)> ?
;? @
uint 
GetAvailabilityDays  
(  !
uint! %
dealerId& .
,. /
uint0 4
	versionId5 >
)> ?
;? @
void  
SaveDealerDisclaimer !
(! "
uint" &
dealerId' /
,/ 0
uint1 5
makeId6 <
,< =
uint> B
?B C
modelIdD K
,K L
uintM Q
?Q R
	versionIdS \
,\ ]
string^ d

disclaimere o
)o p
;p q
bool "
DeleteDealerDisclaimer #
(# $
uint$ (
disclaimerId) 5
)5 6
;6 7
List   
<   "
DealerDisclaimerEntity   #
>  # $
GetDealerDisclaimer  % 8
(  8 9
uint  9 =
dealerId  > F
)  F G
;  G H
bool!! 
EditDisclaimer!! 
(!! 
uint!!  
disclaimerId!!! -
,!!- .
string!!/ 5
newDisclaimerText!!6 G
)!!G H
;!!H I
IEnumerable## 
<## 
BookingAmountEntity## '
>##' ( 
GetBikeBookingAmount##) =
(##= >
uint##> B
dealerId##C K
)##K L
;##L M
bool$$ 
UpdateBookingAmount$$  
($$  !#
BookingAmountEntityBase$$! 8
objbookingAmtBase$$9 J
)$$J K
;$$K L
bool%% 
SaveBookingAmount%% 
(%% 
BookingAmountEntity%% 2
objBookingAmt%%3 @
,%%@ A
UInt32%%B H
updatedById%%I T
)%%T U
;%%U V
bool'' 
DeleteBookingAmount''  
(''  !
uint''! %
	bookingId''& /
)''/ 0
;''0 1
bool(( "
UpdateDealerBikeOffers(( #
(((# $
DealerOffersEntity(($ 6
dealerOffers((7 C
)((C D
;((D E
bool)) #
SaveVersionAvailability)) $
())$ %
uint))% )
dealerId))* 2
,))2 3
string))4 :
bikeVersionIds)); I
,))I J
string))K Q
numberOfDays))R ^
)))^ _
;))_ `
bool** %
DeleteVersionAvailability** &
(**& '
uint**' +
dealerId**, 4
,**4 5
string**6 <
bikeVersionId**= J
)**J K
;**K L
bool++ 
CopyOffersToCities++ 
(++  
uint++  $
dealerId++% -
,++- .
string++/ 5
lstOfferIds++6 A
,++A B
string++C I
	lstCityId++J S
)++S T
;++T U
IEnumerable,, 
<,, 
DealerBenefitEntity,, '
>,,' (
GetDealerBenefits,,) :
(,,: ;
uint,,; ?
dealerId,,@ H
),,H I
;,,I J
bool--  
DeleteDealerBenefits-- !
(--! "
string--" (

benefitIds--) 3
)--3 4
;--4 5
bool.. 
SaveDealerBenefit.. 
(.. 
uint.. #
dealerId..$ ,
,.., -
uint... 2
cityId..3 9
,..9 :
uint..; ?
CatId..@ E
,..E F
string..G M
BenefitText..N Y
,..Y Z
uint..[ _
UserId..` f
,..f g
uint..h l
	BenefitId..m v
)..v w
;..w x
bool// 
SaveDealerEMI// 
(// 
uint// 
dealerId//  (
,//( )
float//* /
?/// 0
MinDownPayment//1 ?
,//? @
float//A F
?//F G
MaxDownPayment//H V
,//V W
UInt16//X ^
?//^ _
	MinTenure//` i
,//i j
UInt16//k q
?//q r
	MaxTenure//s |
,//| }
float	//~ É
?
//É Ñ
MinRateOfInterest
//Ö ñ
,
//ñ ó
float
//ò ù
?
//ù û
MaxRateOfInterest
//ü ∞
,
//∞ ±
float
//≤ ∑
?
//∑ ∏
MinLtv
//π ø
,
//ø ¿
float
//¡ ∆
?
//∆ «
MaxLtv
//» Œ
,
//Œ œ
string
//– ÷
loanProvider
//◊ „
,
//„ ‰
float
//Â Í
?
//Í Î
ProcessingFee
//Ï ˘
,
//˘ ˙
uint
//˚ ˇ
?
//ˇ Ä
id
//Å É
,
//É Ñ
UInt32
//Ö ã
UserID
//å í
)
//í ì
;
//ì î
bool00 
DeleteDealerEMI00 
(00 
uint00 !
id00" $
)00$ %
;00% &
IEnumerable11 
<11 
BikewaleOpr11 
.11  
Entities11  (
.11( )
BikeData11) 1
.111 2
BikeMakeEntityBase112 D
>11D E 
GetDealerMakesByCity11F Z
(11Z [
int11[ ^
cityId11_ e
)11e f
;11f g
IEnumerable22 
<22 
DealerEntityBase22 $
>22$ %
GetDealersByMake22& 6
(226 7
uint227 ;
makeId22< B
,22B C
uint22D H
cityId22I O
)22O P
;22P Q
}33 
public55 

	interface55 
IDealer55 
{66 
IEnumerable77 
<77 
uint77 
>77 !
GetAllAvailableDealer77 /
(77/ 0
uint770 4
	versionId775 >
,77> ?
uint77@ D
areaId77E K
)77K L
;77L M
IEnumerable88 
<88 $
DealerPriceQuoteDetailed88 ,
>88, -%
GetDealerPriceQuoteDetail88. G
(88G H
uint88H L
	versionId88M V
,88V W
uint88X \
cityId88] c
,88c d
string88e k
	dealerIds88l u
)88u v
;88v w
}:: 
};; «
6D:\work\bikewaleweb\BikewaleOpr.Interface\IHomePage.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
{ 
public		 

	interface		 
	IHomePage		 
{

 
HomePageData 
GetHomePageData $
($ %
string% +
id, .
,. /
string0 6
userName7 ?
)? @
;@ A
} 
} Œ
:D:\work\bikewaleweb\BikewaleOpr.Interface\Images\IImage.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
Images  &
{		 
public 

	interface 
IImage 
{ 

ImageToken $
GenerateImageUploadToken +
(+ ,
Image, 1
objImage2 :
): ;
;; <

ImageToken 
ProcessImageUpload %
(% &

ImageToken& 0
token1 6
)6 7
;7 8
} 
} µ
DD:\work\bikewaleweb\BikewaleOpr.Interface\Images\IImageRepository.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
Images  &
{ 
public 

	interface 
IImageRepository %
{ 
ulong 
Add 
( 
Image 
image 
) 
; 
} 
}		 º
ID:\work\bikewaleweb\BikewaleOpr.Interface\Dealers\IVersionAvailability.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
Dealers  '
{ 
public 

	interface  
IVersionAvailability )
{ 
bool #
SaveVersionAvailability $
($ %
uint% )
dealerId* 2
,2 3
IEnumerable4 ?
<? @
uint@ D
>D E
bikeVersionIdsF T
,T U
IEnumerableV a
<a b
uintb f
>f g
numberOfDaysh t
)t u
;u v
bool %
DeleteVersionAvailability &
(& '
uint' +
dealerId, 4
,4 5
IEnumerable6 A
<A B
uintB F
>F G
bikeVersionIdsH V
)V W
;W X
} 
} §
ED:\work\bikewaleweb\BikewaleOpr.Interface\IManageBookingAmountPage.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
{ 
public 

	interface $
IManageBookingAmountPage -
{ #
ManageBookingAmountData &
GetManageBookingAmountData  :
(: ;
UInt32; A
dealerIdB J
)J K
;K L
bool 
AddBookingAmount 
( 
BookingAmountEntity 1"
objBookingAmountEntity2 H
,H I
stringJ P
	updatedByQ Z
)Z [
;[ \
} 
} »
?D:\work\bikewaleweb\BikewaleOpr.Interface\Location\ILocation.cs
	namespace		 	
BikewaleOpr		
 
.		 
	Interface		 
.		  
Location		  (
{

 
public 

	interface 
	ILocation 
{ 
IEnumerable 
< 
StateEntityBase #
># $
	GetStates% .
(. /
)/ 0
;0 1
IEnumerable 
< 
CityNameEntity "
>" #
GetDealerCities$ 3
(3 4
)4 5
;5 6
IEnumerable 
< 
CityNameEntity "
>" #
GetCitiesByState$ 4
(4 5
uint5 9
stateId: A
)A B
;B C
IEnumerable 
< 
CityNameEntity "
>" #
GetAllCities$ 0
(0 1
)1 2
;2 3
} 
} Ã
AD:\work\bikewaleweb\BikewaleOpr.Interface\PageMetas\IPageMetas.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
{ 
public 

	interface 

IPageMetas 
{ 
bool  
UpdatePageMetaStatus !
(! "
uint" &
id' )
,) *
ushort+ 1
status2 8
)9 :
;: ;
} 
} õ
ID:\work\bikewaleweb\BikewaleOpr.Interface\ServiceCenter\IServiceCenter.cs
	namespace		 	
BikewaleOpr		
 
.		 
	Interface		 
.		  
ServiceCenter		  -
{

 
public 

	interface 
IServiceCenter #
{ 
IEnumerable 
< 
CityEntityBase !
>! ""
GetServiceCenterCities# 9
(9 :
uint: >
makeId? E
)E F
;F G
IEnumerable 
< 
CityEntityBase !
>! "
GetAllCities# /
(/ 0
)0 1
;1 2
ServiceCenterData '
GetServiceCentersByCityMake 5
(5 6
uint6 :
cityId; A
,A B
uintC G
makeIdH N
,N O
sbyteP U
activeStatusV b
)b c
;c d%
ServiceCenterCompleteData !'
GetServiceCenterDetailsbyId" =
(= >
uint> B
serviceCenterIdC R
)R S
;S T
StateCityEntity !
GetStateDetailsByCity -
(- .
uint. 2
cityId3 9
)9 :
;: ;
bool "
AddUpdateServiceCenter #
(# $%
ServiceCenterCompleteData$ = 
serviceCenterDetails> R
,R S
stringT Z

_updatedBy[ e
)e f
;f g
bool %
UpdateServiceCenterStatus &
(& '
uint' +
cityId, 2
,2 3
uint4 8
makeId9 ?
,? @
uintA E
serviceCenterIdF U
,U V
stringW ]
currentUserId^ k
)k l
;l m
} 
} ™
SD:\work\bikewaleweb\BikewaleOpr.Interface\ServiceCenter\IServiceCenterRepository.cs
	namespace		 	
BikewaleOpr		
 
.		 
	Interface		 
.		  
ServiceCenter		  -
{

 
public 	
	interface
 $
IServiceCenterRepository ,
{ 
IEnumerable 
< 
CityEntityBase !
>! ""
GetServiceCenterCities# 9
(9 :
uint: >
makeId? E
)E F
;F G
IEnumerable 
< 
CityEntityBase !
>! "
GetAllCities# /
(/ 0
)0 1
;1 2
ServiceCenterData '
GetServiceCentersByCityMake 4
(4 5
uint5 9
cityId: @
,@ A
uintB F
makeIdG M
,M N
sbyteO T
activeStatusU a
)a b
;b c
StateCityEntity 
GetStateDetails &
(& '
uint' +
cityId, 2
)2 3
;3 4%
ServiceCenterCompleteData  
GetDataById! ,
(, -
uint- 1
serviceCenterId2 A
)A B
;B C
bool "
AddUpdateServiceCenter "
(" #%
ServiceCenterCompleteData# < 
serviceCenterDetails= Q
,Q R
stringS Y
	updatedByZ c
)c d
;d e
bool %
UpdateServiceCenterStatus %
(% &
uint& *
serviceCenterId+ :
,: ;
string< B
	updatedByC L
)L M
;M N
} 
} Ù
JD:\work\bikewaleweb\BikewaleOpr.Interface\UserReviews\IUserReviewsCache.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
UserReviews  +
{		 
public 

	interface 
IUserReviewsCache &
{ 
IEnumerable 
< 
DiscardReasons "
>" #(
GetUserReviewsDiscardReasons$ @
(@ A
)A B
;B C
} 
} ë
OD:\work\bikewaleweb\BikewaleOpr.Interface\UserReviews\IUserReviewsRepository.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
UserReviews  +
{ 
public 

	interface "
IUserReviewsRepository +
{ 
IEnumerable 
< 

ReviewBase 
> 
GetReviewsList  .
(. /
ReviewsInputFilters/ B
filterC I
)I J
;J K
IEnumerable 
< 
DiscardReasons "
>" #(
GetUserReviewsDiscardReasons$ @
(@ A
)A B
;B C
uint #
UpdateUserReviewsStatus $
($ %
uint% )
reviewId* 2
,2 3
ReviewsStatus4 A
reviewStatusB N
,N O
uintP T
moderatorIdU `
,` a
ushortb h
disapprovalReasonIdi |
,| }
string	~ Ñ
review
Ö ã
,
ã å
string
ç ì
reviewTitle
î ü
,
ü †
string
° ß

reviewTips
® ≤
,
≤ ≥
bool
¥ ∏
iShortListed
π ≈
)
≈ ∆
;
∆ «
UserReviewSummary  
GetUserReviewSummary .
(. /
uint/ 3
reviewId4 <
)< =
;= >

ReviewBase ,
 GetUserReviewWithEmailIdReviewId 3
(3 4
uint4 8
reviewId9 A
,A B
stringC I
emailIdJ Q
)Q R
;R S
IEnumerable 
< 

ReviewBase 
> 
GetRatingsList  .
(. /
)/ 0
;0 1
IEnumerable 
< #
BikeRatingApproveEntity +
>+ , 
GetUserReviewDetails- A
(A B
stringB H
	reviewIdsI R
)R S
;S T
bool )
UpdateUserReviewRatingsStatus *
(* +
string+ 1
	reviewIds2 ;
,; <
ReviewsStatus= J
reviewStatusK W
,W X
uintY ]
moderatorId^ i
,i j
ushortk q 
disapprovalReasonId	r Ö
)
Ö Ü
;
Ü á
bool  
SaveUserReviewWinner !
(! "
uint" &
reviewId' /
,/ 0
uint1 5
moderatorId6 A
)A B
;B C
}   
}!! –$
WD:\work\bikewaleweb\BikewaleOpr.Interface\ManufacturerCampaign\IManufacturerCampaign.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.   
ManufacturerCampaign  4
{ 
public 

	interface +
IManufacturerCampaignRepository 4
{ 
bool  
UpdateCampaignStatus !
(! "
uint" &
id' )
,) *
bool+ /
isactive0 8
)8 9
;9 :
bool  
UpdateCampaignStatus !
(! "
uint" &
id' )
,) *
uint+ /
isactive0 8
)8 9
;9 :
IEnumerable 
< 
ManufacturerEntity &
>& ' 
GetManufacturersList( <
(< =
)= >
;> ?
IEnumerable 
< %
ManufactureDealerCampaign -
>- .&
SearchManufactureCampaigns/ I
(I J
uintJ N
dealeridO W
)W X
;X Y
IEnumerable 
< 
MfgCityEntity !
>! "!
GetManufacturerCities# 8
(8 9
)9 :
;: ;
IEnumerable 
< "
MfgCampaignRulesEntity *
>* +*
FetchManufacturerCampaignRules, J
(J K
intK N

campaignIdO Y
)Y Z
;Z [
int )
SaveManufacturerCampaignRules )
() *
MfgNewRulesEntity* ;
MgfRules< D
)D E
;E F
bool +
DeleteManufacturerCampaignRules ,
(, -
int- 0
userId1 7
,7 8
string9 ?
ruleIds@ G
)G H
;H I
int "
InsertBWDealerCampaign "
(" #
string# )
description* 5
,5 6
int7 :
isActive; C
,C D
stringE K
maskingNumberL Y
,Y Z
int[ ^
dealerId_ g
,g h
inti l
userIdm s
)s t
;t u
bool "
UpdateBWDealerCampaign #
(# $
string$ *
description+ 6
,6 7
int8 ;
isActive< D
,D E
stringF L
maskingNumberM Z
,Z [
int\ _
dealerId` h
,h i
intj m
userIdn t
,t u
intv y

campaignId	z Ñ
,
Ñ Ö
List
Ü ä
<
ä ã&
ManuCamEntityForTemplate
ã £
>
£ §
objList
• ¨
,
¨ ≠
string
Æ ¥%
LeadCapturePopupMessage
µ Ã
,
Ã Õ
string
Œ ‘)
LeadCapturePopupDescription
’ 
,
 Ò
string
Ú ¯%
LeadCapturePopupHeading
˘ ê
,
ê ë
bool
í ñ
pinCodeRequired
ó ¶
,
¶ ß
bool
ß ´
emailIdRequired
¨ ª
)
ª º
;
º Ω
bool ,
 SaveManufacturerCampaignTemplate -
(- .
List. 2
<2 3$
ManuCamEntityForTemplate3 K
>K L
objListM T
,T U
intV Y
userIdZ `
,` a
intb e

campaignIdf p
,p q
stringr x$
LeadCapturePopupMessage	y ê
,
ê ë
string
í ò)
LeadCapturePopupDescription
ô ¥
,
¥ µ
string
∂ º%
LeadCapturePopupHeading
Ω ‘
,
‘ ’
int
÷ Ÿ
dealerId
⁄ ‚
,
‚ „
bool
‰ Ë
pinCodeRequired
È ¯
,
¯ ˘
bool
˘ ˝
emailIdRequired
˛ ç
)
ç é
;
é è
List 
< 
BikewaleOpr 
. 
Entities !
.! " 
ManufacturerCampaign" 6
.6 7&
ManufacturerCampaignEntity7 Q
>Q R 
FetchCampaignDetailsS g
(g h
inth k

campaignIdl v
)v w
;w x
bool (
ReleaseCampaignMaskingNumber )
() *
int* -

campaignId. 8
)8 9
;9 :
IEnumerable 
< +
ManufacturerCampaignDetailsList 3
>3 4#
GetManufactureCampaigns5 L
(L M
uintM Q
dealeridR Z
,Z [
uint[ _
allActiveCampaign` q
)q r
;r s
}   
}!! ®
cD:\work\bikewaleweb\BikewaleOpr.Interface\ManufacturerCampaign\IManufacturerReleaseMaskingNumber.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.   
ManufacturerCampaign  4
{ 
public 	
	interface -
!IManufacturerReleaseMaskingNumber 6
{ 
bool 
ReleaseNumber 
( 
uint 
dealerId  (
,( )
int* -

campaignId. 8
,8 9
string: @
maskingNumberA N
,N O
intP S
userIdT Z
)Z [
;[ \
} 
} ¶

ZD:\work\bikewaleweb\BikewaleOpr.Interface\Popular Comparisions\IPopularBikeComparisions.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
PopularComparisions  3
{ 
public 

	interface $
IPopularBikeComparisions -
{ 
IEnumerable 
< "
PopularBikeComparision *
>* +
GetBikeComparisions, ?
(? @
)@ A
;A B
bool 
SaveBikeComparision  
(  !
ushort! '
	compareId( 1
,1 2
uint3 7

versionId18 B
,B C
uintD H

versionId2I S
,S T
boolU Y
isActiveZ b
,b c
boold h
isSponsoredi t
,t u
DateTimev ~
sponsoredStartDate	 ë
,
ë í
DateTime
ì õ
sponsoredEndDate
ú ¨
)
¨ ≠
;
≠ Æ
bool 
UpdatePriorities 
( 
string $
prioritiesList% 3
)3 4
;4 5
bool 
DeleteCompareBike 
( 
uint #
deleteId$ ,
), -
;- .
} 
} Ô
DD:\work\bikewaleweb\BikewaleOpr.Interface\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str 0
)0 1
]1 2
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
$str 2
)2 3
]3 4
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
]$$) *¢
?D:\work\bikewaleweb\BikewaleOpr.Interface\Security\ISecurity.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
Security  (
{ 
public 

	interface 
	ISecurity 
{		 
BikewaleOpr

 
.

 
Entities

 
.

 
AWS

  
.

  !
Token

! &
GetToken

' /
(

/ 0
)

0 1
;

1 2
string 
GenerateHash 
( 
uint  
id! #
)# $
;$ %
bool 

VerifyHash 
( 
string 
hash #
,# $
uint% )
id* ,
), -
;- .
} 
} ¸
<D:\work\bikewaleweb\BikewaleOpr.Interface\Used\ISellBikes.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
Used  $
{ 
public

 

	interface

 

ISellBikes

 
{ 
IEnumerable 
< 

SellBikeAd 
> )
GetClassifiedPendingInquiries  =
(= >
)> ?
;? @
bool 
SaveEditedInquiry 
( 
uint #
	inquiryId$ -
,- .
short/ 4

isApproved5 ?
,? @
intA D

approvedByE O
,O P
stringQ W
	profileIdX a
,a b
stringc i
bikeNamej r
,r s
uintt x
modelId	y Ä
)
Ä Å
;
Å Ç
} 
} ¸
CD:\work\bikewaleweb\BikewaleOpr.Interface\Used\ISellerRepository.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
Used  $
{		 
public 

	interface 
ISellerRepository &
{ 
UsedBikeSellerBase 
GetSellerDetails +
(+ ,
int, /
	inquiryId0 9
,9 :
bool; ?
isDealer@ H
)H I
;I J
IEnumerable 
< 

SellBikeAd 
> )
GetClassifiedPendingInquiries  =
(= >
)> ?
;? @
bool 
SaveEditedInquiry 
( 
uint #
	inquiryId$ -
,- .
short/ 4

isApproved5 ?
,? @
intA D

approvedByE O
)O P
;P Q"
UsedBikeProfileDetails $
GetUsedBikeSellerDetails 7
(7 8
int8 ;
	inquiryId< E
,E F
boolG K
isDealerL T
)T U
;U V
} 
} Ë
<D:\work\bikewaleweb\BikewaleOpr.Interface\Used\IUsedBikes.cs
	namespace 	
BikewaleOpr
 
. 
	Interface 
.  
Used  $
{ 
public		 

	interface		 

IUsedBikes		 
{

 
bool 
SendUnitSoldEmail 
( 
SoldUnitData +

dataObject, 6
,6 7
string8 >
currentUserName? N
)N O
;O P
void )
SendUploadUsedModelImageEmail *
(* +
)+ ,
;, -
} 
} 