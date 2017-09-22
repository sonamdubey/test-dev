Œ
FD:\work\bikewaleweb\Bikewale.Interfaces\AppDeepLinking\IDeepLinking.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
AppDeepLinking ,
{		 
public 

	interface 
IDeepLinking !
{ 
DeepLinkingEntity 
GetParameters %
(% &
string& ,
url- 0
)0 1
;1 2
} 
} 
:D:\work\bikewaleweb\Bikewale.Interfaces\App\IAppVersion.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
App !
{		 
public 

	interface 
IAppVersion  
{ 

AppVersion 
CheckVersionStatus %
(% &
uint& *

appVersion+ 5
,5 6
uint7 ;
sourceId< D
)D E
;E F
} 
} ˙
?D:\work\bikewaleweb\Bikewale.Interfaces\App\IAppVersionCache.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
App !
{ 
public		 

	interface		 
IAppVersionCache		 %
{

 

AppVersion 
CheckVersionStatus %
(% &
uint& *

appVersion+ 5
,5 6
uint7 ;
sourceId< D
)D E
;E F
} 
} Û
<D:\work\bikewaleweb\Bikewale.Interfaces\App\ISplashScreen.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
App !
{ 
public

 

	interface

 (
ISplashScreenCacheRepository

 1
{ 
IEnumerable 
< 
SplashScreenEntity &
>& '
GetAppSplashScreen( :
(: ;
); <
;< =
} 
public 

	interface #
ISplashScreenRepository ,
{ 
IEnumerable 
< 
SplashScreenEntity &
>& '
GetAppSplashScreen( :
(: ;
); <
;< =
} 
} †
AD:\work\bikewaleweb\Bikewale.Interfaces\App\ISplashScreenCache.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
App !
{ 
public		 

	interface		 
ISplashScreen		 "
{

 
SplashScreenEntity 
GetAppSplashScreen -
(- .
). /
;/ 0
} 
} ¥
KD:\work\bikewaleweb\Bikewale.Interfaces\AutoBiz\DealerPriceQuote\IDealer.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
AutoBiz %
{ 
public 

	interface 
IDealer 
{ 

DealerInfo !
GetSubscriptionDealer (
(( )
uint) -
modelId. 5
,5 6
uint7 ;
cityId< B
,B C
uintD H
areaIdI O
)O P
;P Q
} 
} ¨
UD:\work\bikewaleweb\Bikewale.Interfaces\AutoBiz\DealerPriceQuote\IDealerPriceQuote.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
AutoBiz %
{		 
public 

	interface 
IDealerPriceQuote &
{ 
PQ_QuotationEntity 
GetDealerPriceQuote .
(. /
PQParameterEntity/ @
	objParamsA J
)J K
;K L
List 
< 
CityEntityBase 
>  
GetBikeBookingCities 1
(1 2
uint2 6
?6 7
modelId8 ?
)? @
;@ A
List 
< 
BikeMakeEntityBase 
>  
GetBikeMakesInCity! 3
(3 4
uint4 8
cityId9 ?
)? @
;@ A
OfferHtmlEntity 
GetOfferTerms %
(% &
string& ,
offerMaskingName- =
,= >
int? B
?B C
offerIdD K
)K L
;L M"
DealerPriceQuoteEntity %
GetPriceQuoteForAllDealer 8
(8 9
uint9 =
	versionId> G
,G H
uintI M
cityIdN T
,T U
stringV \
	dealerIds] f
)f g
;g h)
DetailedDealerQuotationEntity %(
GetDealerPriceQuoteByPackage& B
(B C
PQParameterEntityC T
	objParamsU ^
)^ _
;_ `
Bikewale 
. 
Entities 
. 

PriceQuote $
.$ %
v2% '
.' ()
DetailedDealerQuotationEntity( E*
GetDealerPriceQuoteByPackageV2F d
(d e
PQParameterEntitye v
	objParams	w Ä
)
Ä Å
;
Å Ç

DealerInfo 
GetNearestDealer #
(# $
uint$ (
modelId) 0
,0 1
uint2 6
cityId7 =
)= >
;> ?

DealerInfo 
GetNearestDealer #
(# $
uint$ (
modelId) 0
,0 1
uint2 6
cityId7 =
,= >
uint? C
areaIdD J
)J K
;K L
} 
} Õ
CD:\work\bikewaleweb\Bikewale.Interfaces\AutoBiz\Dealers\IDealers.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
AutoBiz %
{ 
public 

	interface 
IDealers 
{ !
PQ_DealerDetailEntity 
GetDealerDetailsPQ 0
(0 1
PQParameterEntity1 B
	objParamsC L
)L M
;M N
	DataTable 
GetDealerCities !
(! "
)" #
;# $
uint 
GetAvailabilityDays  
(  !
uint! %
dealerId& .
,. /
uint0 4
	versionId5 >
)> ?
;? @
BookingAmountEntity "
GetDealerBookingAmount 2
(2 3
uint3 7
	versionId8 A
,A B
uintC G
dealerIdH P
)P Q
;Q R
} 
} À
DD:\work\bikewaleweb\Bikewale.Interfaces\AutoComplete\IAutoSuggest.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
AutoComplete *
{ 
public 

	interface 
IAutoSuggest !
{ 
IEnumerable 
< 
Nest 
. 
SuggestOption &
<& '
T' (
>( )
>) * 
GetAutoSuggestResult+ ?
<? @
T@ A
>A B
(B C
stringC I
	inputTextJ S
,S T
intU X
noOfRecordsY d
,d e
AutoSuggestEnumf u
sourcev |
)| }
where	~ É
T
Ñ Ö
:
Ü á
class
à ç
;
ç é
} 
} ã
CD:\work\bikewaleweb\Bikewale.Interfaces\BikeBooking\IBikeBooking.cs
	namespace		 	
Bikewale		
 
.		 

Interfaces		 
.		 
BikeBooking		 )
{

 
public 

	interface 
IBikeBooking !
{ 
BookingResults 
	DoBooking  
(  !
TransactionDetails! 3
entity4 :
,: ;
string< B

sourceTypeC M
)M N
;N O
} 
} π

KD:\work\bikewaleweb\Bikewale.Interfaces\BikeBooking\IBookingCancellation.cs
	namespace		 	
Bikewale		
 
.		 

Interfaces		 
.		 
BikeBooking		 )
{

 
public 

	interface  
IBookingCancellation )
{ /
#ValidBikeCancellationResponseEntity +
IsValidCancellation, ?
(? @
string@ F
bwidG K
,K L
stringM S
mobileT Z
)Z [
;[ \
uint 
SaveCancellationOTP  
(  !
string! '
bwId( ,
,, -
string. 4
mobile5 ;
,; <
string= C
otpD G
)G H
;H I!
CancelledBikeCustomer !
VerifyCancellationOTP 3
(3 4
string4 :
BwId; ?
,? @
stringA G
MobileH N
,N O
stringP V
OTPW Z
)Z [
;[ \!
CancelledBikeCustomer "
GetCancellationDetails 4
(4 5
uint5 9
pqId: >
)> ?
;? @
bool 
ConfirmCancellation  
(  !
uint! %
pqId& *
)* +
;+ ,
} 
} €
FD:\work\bikewaleweb\Bikewale.Interfaces\BikeBooking\IBookingListing.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeBooking )
{ 
public 

	interface 
IBookingListing $
{ 
IEnumerable 
< $
BikeBookingListingEntity ,
>, -
FetchBookingList. >
(> ?
int? B
cityIdC I
,I J
uintK O
areaIdP V
,V W
EntitiesX `
.` a
BikeBookinga l
.l m'
BookingListingFilterEntity	m á
filter
à é
,
é è
out
ê ì
int
î ó

totalCount
ò ¢
,
¢ £
out
§ ß
int
® ´
fetchedCount
¨ ∏
,
∏ π
out
∫ Ω
	PagingUrl
æ «
pageUrl
» œ
)
œ –
;
– —
} 
} €"
HD:\work\bikewaleweb\Bikewale.Interfaces\BikeBooking\IDealerPriceQuote.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeBooking )
{ 
public 

	interface 
IDealerPriceQuote &
{ 
bool 
SaveCustomerDetail 
(  
DPQ_SaveEntity  .
entity/ 5
)5 6
;6 7
bool "
UpdateIsMobileVerified #
(# $
uint$ (
pqId) -
)- .
;. /
bool 
UpdateMobileNumber 
(  
uint  $
pqId% )
,) *
string+ 1
mobileNo2 :
): ;
;; <
bool 

PushedToAB 
( 
uint 
pqId !
,! "
uint# '
abInquiryId( 3
)3 4
;4 5
PQCustomerDetail 
GetCustomerDetails +
(+ ,
uint, 0
pqId1 5
)5 6
;6 7
bool 
IsNewBikePQExists 
( 
uint #
pqId$ (
)( )
;) *
List 
< !
BikeVersionEntityBase "
>" #
GetVersionList$ 2
(2 3
uint3 7
	versionId8 A
,A B
uintC G
dealerIdH P
,P Q
uintR V
cityIdW ]
)] ^
;^ _
bool 
SaveRSAOfferClaim 
( 
RSAOfferClaimEntity 2
objOffer3 ;
,; <
string= C
bikeNameD L
)L M
;M N
bool   
UpdatePQBikeColor   
(   
uint   #
colorId  $ +
,  + ,
uint  - 1
pqId  2 6
)  6 7
;  7 8
bool"" '
UpdatePQTransactionalDetail"" (
(""( )
uint"") -
pqId"". 2
,""2 3
uint""4 8
transId""9 @
,""@ A
bool""B F
isTransComplete""G V
,""V W
string""X ^
bookingReferenceNo""_ q
)""q r
;""r s
bool## 
IsDealerNotified## 
(## 
uint## "
dealerId### +
,##+ ,
string##- 3
customerMobile##4 B
,##B C
ulong##D I

customerId##J T
)##T U
;##U V
bool$$ "
IsDealerPriceAvailable$$ #
($$# $
uint$$$ (
	versionId$$) 2
,$$2 3
uint$$4 8
cityId$$9 ?
)$$? @
;$$@ A
uint%% '
GetDefaultPriceQuoteVersion%% (
(%%( )
uint%%) -
modelId%%. 5
,%%5 6
uint%%7 ;
cityId%%< B
)%%B C
;%%C D
uint&& '
GetDefaultPriceQuoteVersion&& (
(&&( )
uint&&) -
modelId&&. 5
,&&5 6
uint&&7 ;
cityId&&< B
,&&B C
uint&&D H
areaId&&I O
)&&O P
;&&P Q
List'' 
<'' 
Bikewale'' 
.'' 
Entities'' 
.'' 
Location'' '
.''' (
AreaEntityBase''( 6
>''6 7
GetAreaList''8 C
(''C D
uint''D H
modelId''I P
,''P Q
uint''R V
cityId''W ]
)''] ^
;''^ _
PQOutputEntity(( 
	ProcessPQ((  
(((  !&
PriceQuoteParametersEntity((! ;
PQParams((< D
)((D E
;((E F
PQOutputEntity)) 
ProcessPQV2)) "
())" #&
PriceQuoteParametersEntity))# =
PQParams))> F
)))F G
;))G H$
BookingPageDetailsEntity**  #
FetchBookingPageDetails**! 8
(**8 9
uint**9 =
cityId**> D
,**D E
uint**F J
	versionId**K T
,**T U
uint**V Z
dealerId**[ c
)**c d
;**d e
bool++ &
UpdateDealerDailyLeadCount++ '
(++' (
uint++( ,

campaignId++- 7
,++7 8
uint++9 =
abInquiryId++> I
)++I J
;++J K
bool,, )
IsDealerDailyLeadLimitExceeds,, *
(,,* +
uint,,+ /

campaignId,,0 :
),,: ;
;,,; <
}-- 
}.. Ú
=D:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IBikeInfo.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
{ 
public		 

	interface		 
	IBikeInfo		 
{

 
BikeInfo 
GetBikeInfo 
( 
uint !
modelId" )
)) *
;* +
GenericBikeInfo 
GetBikeInfo #
(# $
uint$ (
modelId) 0
,0 1
uint2 6
cityId7 =
)= >
;> ?
} 
} ö
>D:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IBikeMakes.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
{ 
public 

	interface 

IBikeMakes 
<  
T  !
,! "
U# $
>$ %
:& '
IRepository( 3
<3 4
T4 5
,5 6
U7 8
>8 9
{ 
List 
< 
BikeMakeEntityBase 
>  
GetMakesByType! /
(/ 0
EnumBikeType0 <
requestType= H
)H I
;I J!
BikeDescriptionEntity 
GetMakeDescription 0
(0 1
U1 2
makeId3 9
)9 :
;: ;
BikeMakeEntityBase 
GetMakeDetails )
() *
uint* .
makeId/ 5
)5 6
;6 7
IEnumerable 
< 
BikeMakeEntityBase &
>& '
UpcomingBikeMakes( 9
(9 :
): ;
;; <
IEnumerable 
< 
BikeVersionEntity %
>% &+
GetDiscontinuedBikeModelsByMake' F
(F G
uintG K
makeIdL R
)R S
;S T
IEnumerable 
< 
BikeMakeModelBase %
>% &
GetAllMakeModels' 7
(7 8
)8 9
;9 :
	Hashtable 
GetOldMaskingNames $
($ %
)% &
;& '!
BikeDescriptionEntity %
GetScooterMakeDescription 7
(7 8
uint8 <
makeId= C
)C D
;D E
IEnumerable 
< 
BikeMakeEntityBase &
>& '
GetScooterMakes( 7
(7 8
)8 9
;9 :
}   
}!! √
MD:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IBikeMakesCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
{ 
public 

	interface %
IBikeMakesCacheRepository .
<. /
U/ 0
>0 1
{ 
IEnumerable 
< 
BikeMakeEntityBase &
>& '
GetMakesByType( 6
(6 7
EnumBikeType7 C
makeTypeD L
)L M
;M N
IEnumerable 
< 
BikeVersionEntity %
>% &+
GetDiscontinuedBikeModelsByMake' F
(F G
uintG K
makeIdL R
)R S
;S T!
BikeDescriptionEntity 
GetMakeDescription 0
(0 1
U1 2
makeId3 9
)9 :
;: ;
BikeMakeEntityBase 
GetMakeDetails )
() *
uint* .
makeId/ 5
)5 6
;6 7
IEnumerable 
< 
BikeMakeModelBase %
>% &
GetAllMakeModels' 7
(7 8
)8 9
;9 :
MakeMaskingResponse "
GetMakeMaskingResponse 2
(2 3
string3 9
maskingName: E
)E F
;F G
IEnumerable 
< 
BikeMakeEntityBase &
>& '
GetScooterMakes( 7
(7 8
)8 9
;9 :!
BikeDescriptionEntity %
GetScooterMakeDescription 7
(7 8
uint8 <
makeId= C
)C D
;D E
} 
} ›
TD:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IBikeModelMaskingCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
{ 
public 

	interface '
IBikeMaskingCacheRepository 0
<0 1
T1 2
,2 3
U4 5
>5 6
{  
ModelMaskingResponse #
GetModelMaskingResponse 4
(4 5
string5 ;
maskingName< G
)G H
;H I#
BikeSpecificationEntity 
MVSpecsFeatures  /
(/ 0
int0 3
	versionId4 =
)= >
;> ?
IEnumerable 
< "
SimilarBikesWithPhotos *
>* +$
GetSimilarBikeWithPhotos, D
(D E
UE F
modelIdG N
,N O
ushortP V
totalRecordsW c
,c d
uinte i
cityIdj p
)p q
;q r
ReviewDetailsEntity 
GetDetailsByModel -
(- .
U. /
modelId0 7
,7 8
uint9 =
cityId> D
)D E
;E F
ReviewDetailsEntity   
GetDetailsByVersion   /
(  / 0
U  0 1
	versionId  2 ;
,  ; <
uint  = A
cityId  B H
)  H I
;  I J
ReviewDetailsEntity!! 

GetDetails!! &
(!!& '
string!!' -
reviewId!!. 6
,!!6 7
bool!!8 <
isAlreadyViewed!!= L
)!!L M
;!!M N
IEnumerable"" 
<"" 
BikeMakeEntityBase"" &
>""& '
GetMakeIfVideo""( 6
(""6 7
)""7 8
;""8 9
IEnumerable## 
<##  
SimilarBikeWithVideo## (
>##( )!
GetSimilarBikesVideos##* ?
(##? @
uint##@ D
modelId##E L
,##L M
uint##N R

totalcount##S ]
)##] ^
;##^ _
IEnumerable$$ 
<$$ !
SimilarBikeUserReview$$ )
>$$) *&
GetSimilarBikesUserReviews$$+ E
($$E F
uint$$F J
modelId$$K R
,$$R S
uint$$T X
cityId$$Y _
,$$_ `
uint$$a e
totalRecords$$f r
)$$r s
;$$s t
T%% 	
GetById%%
 
(%% 
U%% 
id%% 
)%% 
;%% 
}&& 
}'' †-
?D:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IBikeModels.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
{ 
public 

	interface 
IBikeModels  
<  !
T! "
," #
U$ %
>% &
:' (
IRepository) 4
<4 5
T5 6
,6 7
U8 9
>9 :
{ 
List 
< 
BikeModelEntityBase  
>  !
GetModelsByType" 1
(1 2
EnumBikeType2 >
requestType? J
,J K
intL O
makeIdP V
)V W
;W X
List   
<   "
BikeVersionsListEntity   #
>  # $
GetVersionsList  % 4
(  4 5
U  5 6
modelId  7 >
,  > ?
bool  @ D
isNew  E J
)  J K
;  K L!
BikeDescriptionEntity!! 
GetModelSynopsis!! .
(!!. /
U!!/ 0
modelId!!1 8
)!!8 9
;!!9 :
UpcomingBikeEntity## "
GetUpcomingBikeDetails## 1
(##1 2
U##2 3
modelId##4 ;
)##; <
;##< =
List$$ 
<$$ 
UpcomingBikeEntity$$ 
>$$   
GetUpcomingBikesList$$! 5
($$5 6(
UpcomingBikesListInputEntity$$6 R
inputParams$$S ^
,$$^ _#
EnumUpcomingBikesFilter$$` w
sortBy$$x ~
,$$~ 
out
$$Ä É
int
$$Ñ á
recordCount
$$à ì
)
$$ì î
;
$$î ï
IEnumerable%% 
<%% 
UpcomingBikeEntity%% &
>%%& ' 
GetUpcomingBikesList%%( <
(%%< =#
EnumUpcomingBikesFilter%%= T
sortBy%%U [
,%%[ \
int%%] `
pageSize%%a i
,%%i j
int%%k n
?%%n o
makeId%%p v
=%%w x
null%%y }
,%%} ~
int	%% Ç
?
%%Ç É
modelId
%%Ñ ã
=
%%å ç
null
%%é í
,
%%í ì
int
%%î ó
?
%%ó ò
	curPageNo
%%ô ¢
=
%%£ §
null
%%• ©
)
%%© ™
;
%%™ ´ 
NewLaunchedBikesBase&& #
GetNewLaunchedBikesList&& 4
(&&4 5
int&&5 8

startIndex&&9 C
,&&C D
int&&E H
endIndex&&I Q
,&&Q R
int&&S V
?&&V W
makeid&&X ^
=&&_ `
null&&a e
)&&e f
;&&f g
BikeModelPageEntity'' 
GetModelPageDetails'' /
(''/ 0
U''0 1
modelId''2 9
)''9 :
;'': ;
IEnumerable(( 
<(( 
NewBikeModelColor(( %
>((% &
GetModelColor((' 4
(((4 5
U((5 6
modelId((7 >
)((> ?
;((? @
IEnumerable)) 
<)) 

ModelImage)) 
>)) $
GetBikeModelPhotoGallery))  8
())8 9
U))9 :
modelId)); B
)))B C
;))C D
IEnumerable** 
<** 

ModelImage** 
>** -
!GetModelPhotoGalleryWithMainImage**  A
(**A B
U**B C
modelId**D K
)**K L
;**L M
BikeModelContent++ "
GetRecentModelArticles++ /
(++/ 0
U++0 1
modelId++2 9
)++9 :
;++: ;
Bikewale,, 
.,, 
Entities,, 
.,, 
BikeData,, "
.,," #
v2,,# %
.,,% &
BikeModelContent,,& 6$
GetRecentModelArticlesv2,,7 O
(,,O P
U,,P Q
modelId,,R Y
),,Y Z
;,,Z [
IEnumerable-- 
<--  
MostPopularBikesBase-- (
>--( ))
GetMostPopularBikesbyMakeCity--* G
(--G H
uint--H L
topCount--M U
,--U V
uint--W [
makeId--\ b
,--b c
uint--d h
cityId--i o
)--o p
;--p q
IEnumerable.. 
<..  
BikeUserReviewRating.. (
>..( )$
GetUserReviewSimilarBike..* B
(..B C
uint..C G
modelId..H O
,..O P
uint..Q U
topCount..V ^
)..^ _
;.._ `#
ModelPhotoGalleryEntity// 
GetPhotoGalleryData//  3
(//3 4
U//4 5
modelId//6 =
)//= >
;//> ?
IEnumerable00 
<00  
ColorImageBaseEntity00 (
>00( )
CreateAllPhotoList00* <
(00< =
U00= >
modelId00? F
)00F G
;00G H
BikeModelPageEntity11 
GetModelPageDetails11 /
(11/ 0
U110 1
modelId112 9
,119 :
int11; >
	versionId11? H
)11H I
;11I J
IEnumerable22 
<22  
MostPopularBikesBase22 (
>22( )"
GetMostPopularScooters22* @
(22@ A
uint22A E
makeId22F L
)22L M
;22M N
IEnumerable33 
<33  
MostPopularBikesBase33 (
>33( )"
GetMostPopularScooters33* @
(33@ A
uint33A E
topCount33F N
,33N O
uint33P T
?33T U
cityId33V \
)33\ ]
;33] ^
IEnumerable44 
<44  
MostPopularBikesBase44 (
>44( )
GetMostPopularBikes44* =
(44= >
EnumBikeType44> J
requestType44K V
,44V W
uint44X \
topCount44] e
,44e f
uint44g k
makeId44l r
,44r s
uint44t x
cityId44y 
)	44 Ä
;
44Ä Å
}55 
}66 ã7
ND:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IBikeModelsCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
{ 
public// 

	interface// &
IBikeModelsCacheRepository// /
</// 0
U//0 1
>//1 2
{00 
BikeModelPageEntity11 
GetModelPageDetails11 /
(11/ 0
U110 1
modelId112 9
)119 :
;11: ;
BikeModelPageEntity22 
GetModelPageDetails22 /
(22/ 0
U220 1
modelId222 9
,229 :
int22; >
	versionId22? H
)22H I
;22I J
IEnumerable33 
<33 
UpcomingBikeEntity33 &
>33& ' 
GetUpcomingBikesList33( <
(33< =#
EnumUpcomingBikesFilter33= T
sortBy33U [
,33[ \
int33] `
pageSize33a i
,33i j
int33k n
?33n o
makeId33p v
=33w x
null33y }
,33} ~
int	33 Ç
?
33Ç É
modelId
33Ñ ã
=
33å ç
null
33é í
,
33í ì
int
33î ó
?
33ó ò
	curPageNo
33ô ¢
=
33£ §
null
33• ©
)
33© ™
;
33™ ´
IEnumerable44 
<44  
MostPopularBikesBase44 (
>44( )%
GetMostPopularBikesByMake44* C
(44C D
int44D G
makeId44H N
)44N O
;44O P
IEnumerable55 
<55  
MostPopularBikesBase55 (
>55( )
GetMostPopularBikes55* =
(55= >
int55> A
?55A B
topCount55C K
=55L M
null55N R
,55R S
int55T W
?55W X
makeId55Y _
=55` a
null55b f
)55f g
;55g h 
NewLaunchedBikesBase66 #
GetNewLaunchedBikesList66 4
(664 5
int665 8

startIndex669 C
,66C D
int66E H
endIndex66I Q
,66Q R
int66S V
?66V W
makeId66X ^
=66_ `
null66a e
)66e f
;66f g 
NewLaunchedBikesBase77 )
GetNewLaunchedBikesListByMake77 :
(77: ;
int77; >

startIndex77? I
,77I J
int77K N
endIndex77O W
,77W X
int77Y \
?77\ ]
makeId77^ d
=77e f
null77g k
)77k l
;77l m!
BikeDescriptionEntity88 
GetModelSynopsis88 .
(88. /
U88/ 0
modelId881 8
)888 9
;889 :
IEnumerable99 
<99  
MostPopularBikesBase99 (
>99( ))
GetMostPopularBikesbyMakeCity99* G
(99G H
uint99H L
topCount99M U
,99U V
uint99W [
makeId99\ b
,99b c
uint99d h
cityId99i o
)99o p
;99p q
IEnumerable:: 
<:: 
NewBikeModelColor:: %
>::% &
GetModelColor::' 4
(::4 5
U::5 6
modelId::7 >
)::> ?
;::? @
IEnumerable;; 
<;;  
BikeUserReviewRating;; (
>;;( )$
GetUserReviewSimilarBike;;* B
(;;B C
uint;;C G
modelId;;H O
,;;O P
uint;;Q U
topCount;;V ^
);;^ _
;;;_ `
EnumBikeBodyStyles<< 
GetBikeBodyType<< *
(<<* +
uint<<+ /
modelId<<0 7
)<<7 8
;<<8 9
IEnumerable== 
<==  
MostPopularBikesBase== (
>==( )&
GetPopularBikesByBodyStyle==* D
(==D E
ushort==E K
bodyStyleId==L W
,==W X
uint==Y ]
topCount==^ f
,==f g
uint==h l
cityId==m s
)==s t
;==t u
ICollection>> 
<>>  
MostPopularBikesBase>> (
>>>( )/
#GetMostPopularBikesByModelBodyStyle>>* M
(>>M N
int>>N Q
modelId>>R Y
,>>Y Z
int>>[ ^
topCount>>_ g
,>>g h
uint>>i m
cityId>>n t
)>>t u
;>>u v
GenericBikeInfo?? 
GetBikeInfo?? #
(??# $
uint??$ (
modelId??) 0
,??0 1
uint??2 6
cityId??7 =
)??= >
;??> ?
GenericBikeInfo@@ 
GetBikeInfo@@ #
(@@# $
uint@@$ (
modelId@@) 0
)@@0 1
;@@1 2
BikeRankingEntityAA $
GetBikeRankingByCategoryAA 2
(AA2 3
uintAA3 7
modelIdAA8 ?
)AA? @
;AA@ A
ICollectionBB 
<BB 
BestBikeEntityBaseBB &
>BB& '"
GetBestBikesByCategoryBB( >
(BB> ?
EnumBikeBodyStylesBB? Q
	bodyStyleBBR [
,BB[ \
uintBB] a
?BBa b
cityIdBBc i
=BBj k
nullBBl p
)BBp q
;BBq r
ICollectionCC 
<CC 
BestBikeEntityBaseCC &
>CC& '%
GetBestBikesByModelInMakeCC( A
(CCA B
uintCCB F
modelIdCCG N
,CCN O
uintCCP T
?CCT U
cityIdCCV \
=CC] ^
nullCC_ c
)CCc d
;CCd e
IEnumerableDD 
<DD %
NewLaunchedBikeEntityBaseDD -
>DD- .#
GetNewLaunchedBikesListDD/ F
(DDF G
)DDG H
;DDH I
IEnumerableEE 
<EE %
NewLaunchedBikeEntityBaseEE -
>EE- .#
GetNewLaunchedBikesListEE/ F
(EEF G
uintEEG K
cityIdEEL R
)EER S
;EES T
IEnumerableFF 
<FF 
ModelColorImageFF #
>FF# $
GetModelColorPhotosFF% 8
(FF8 9
UFF9 :
modelIdFF; B
)FFB C
;FFC D
BikewaleGG 
.GG 
EntitiesGG 
.GG 
CMSGG 
.GG 
PhotosGG $
.GG$ %
ModelHostImagePathGG% 7
GetModelPhotoInfoGG8 I
(GGI J
UGGJ K
modelIdGGL S
)GGS T
;GGT U
IEnumerableHH 
<HH  
MostPopularBikesBaseHH (
>HH( )"
GetMostPopularScootersHH* @
(HH@ A
uintHHA E
topCountHHF N
,HHN O
uintHHP T
?HHT U
cityIdHHV \
)HH\ ]
;HH] ^
IEnumerableII 
<II  
MostPopularBikesBaseII (
>II( )"
GetMostPopularScootersII* @
(II@ A
uintIIA E
makeIdIIF L
)IIL M
;IIM N
IEnumerableJJ 
<JJ  
MostPopularBikesBaseJJ (
>JJ( )"
GetMostPopularScootersJJ* @
(JJ@ A
uintJJA E
topCountJJF N
,JJN O
uintJJP T
makeIdJJU [
,JJ[ \
uintJJ] a
cityIdJJb h
)JJh i
;JJi j
}MM 
}NN ∫U
ID:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IBikeModelsRepository.cs
	namespace

 	
Bikewale


 
.

 

Interfaces

 
.

 
BikeData

 &
{ 
public44 

	interface44 !
IBikeModelsRepository44 *
<44* +
T44+ ,
,44, -
U44. /
>44/ 0
:441 2
IRepository443 >
<44> ?
T44? @
,44@ A
U44B C
>44C D
{55 
List66 
<66 
BikeModelEntityBase66  
>66  !
GetModelsByType66" 1
(661 2
EnumBikeType662 >
requestType66? J
,66J K
int66L O
makeId66P V
)66V W
;66W X
List77 
<77 "
BikeVersionsListEntity77 #
>77# $
GetVersionsList77% 4
(774 5
U775 6
modelId777 >
,77> ?
bool77@ D
isNew77E J
)77J K
;77K L!
BikeDescriptionEntity88 
GetModelSynopsis88 .
(88. /
U88/ 0
modelId881 8
)888 9
;889 :
UpcomingBikeEntity99 "
GetUpcomingBikeDetails99 1
(991 2
U992 3
modelId994 ;
)99; <
;99< =
List:: 
<:: 
UpcomingBikeEntity:: 
>::   
GetUpcomingBikesList::! 5
(::5 6(
UpcomingBikesListInputEntity::6 R
inputParams::S ^
,::^ _#
EnumUpcomingBikesFilter::` w
sortBy::x ~
,::~ 
out
::Ä É
int
::Ñ á
recordCount
::à ì
)
::ì î
;
::î ï 
NewLaunchedBikesBase;; #
GetNewLaunchedBikesList;; 4
(;;4 5
int;;5 8

startIndex;;9 C
,;;C D
int;;E H
endIndex;;I Q
);;Q R
;;;R S 
NewLaunchedBikesBase<< )
GetNewLaunchedBikesListByMake<< :
(<<: ;
int<<; >

startIndex<<? I
,<<I J
int<<K N
endIndex<<O W
,<<W X
int<<Y \
?<<\ ]
makeid<<^ d
=<<e f
null<<g k
)<<k l
;<<l m
BikeModelPageEntity== 
GetModelPage== (
(==( )
U==) *
modelId==+ 2
)==2 3
;==3 4
IEnumerable>> 
<>> 
NewBikeModelColor>> %
>>>% &
GetModelColor>>' 4
(>>4 5
U>>5 6
modelId>>7 >
)>>> ?
;>>? @#
BikeSpecificationEntity?? 
MVSpecsFeatures??  /
(??/ 0
int??0 3
	versionId??4 =
)??= >
;??> ?
IEnumerable@@ 
<@@ #
BikeSpecificationEntity@@ +
>@@+ ,"
GetModelSpecifications@@- C
(@@C D
U@@D E
modelId@@F M
)@@M N
;@@N O
IEnumerableAA 
<AA  
MostPopularBikesBaseAA (
>AA( ))
GetMostPopularBikesbyMakeCityAA* G
(AAG H
uintAAH L
topCountAAM U
,AAU V
uintAAW [
makeIdAA\ b
,AAb c
uintAAd h
cityIdAAi o
)AAo p
;AAp q
IEnumerableBB 
<BB  
BikeUserReviewRatingBB (
>BB( )$
GetUserReviewSimilarBikeBB* B
(BBB C
uintBBC G
modelIdBBH O
,BBO P
uintBBQ U
topCountBBV ^
)BB^ _
;BB_ `
ListII 
<II  
MostPopularBikesBaseII !
>II! "
GetMostPopularBikesII# 6
(II6 7
intII7 :
?II: ;
topCountII< D
=IIE F
nullIIG K
,IIK L
intIIM P
?IIP Q
makeIdIIR X
=IIY Z
nullII[ _
)II_ `
;II` a
ListOO 
<OO  
MostPopularBikesBaseOO !
>OO! "%
GetMostPopularBikesByMakeOO# <
(OO< =
intOO= @
makeIdOOA G
)OOG H
;OOH I
	HashtableQQ 
GetMaskingNamesQQ !
(QQ! "
)QQ" #
;QQ# $
	HashtableRR 
GetOldMaskingNamesRR $
(RR$ %
)RR% &
;RR& '
ListTT 
<TT 
FeaturedBikeEntityTT 
>TT  
GetFeaturedBikesTT! 1
(TT1 2
uintTT2 6

topRecordsTT7 A
)TTA B
;TTB C
IEnumerableUU 
<UU 
BikeMakeModelEntityUU '
>UU' (
GetAllModelsUU) 5
(UU5 6
EnumBikeTypeUU6 B
requestTypeUUC N
)UUN O
;UUO P
ListVV 
<VV 
BikeVersionMinSpecsVV  
>VV  !
GetVersionMinSpecsVV" 4
(VV4 5
UVV5 6
modelIdVV7 >
,VV> ?
boolVV@ D
isNewVVE J
)VVJ K
;VVK L
BikeModelContentWW "
GetRecentModelArticlesWW /
(WW/ 0
UWW0 1
modelIdWW2 9
)WW9 :
;WW: ;
ModelHostImagePathXX 
GetModelPhotoInfoXX ,
(XX, -
UXX- .
modelIdXX/ 6
)XX6 7
;XX7 8
ReviewDetailsEntityZZ 
GetDetailsByModelZZ -
(ZZ- .
UZZ. /
modelIdZZ0 7
,ZZ7 8
uintZZ9 =
cityIdZZ> D
)ZZD E
;ZZE F
ReviewDetailsEntity[[ 
GetDetailsByVersion[[ /
([[/ 0
U[[0 1
	versionId[[2 ;
,[[; <
uint[[= A
cityId[[B H
)[[H I
;[[I J
ReviewDetailsEntity\\ 

GetDetails\\ &
(\\& '
string\\' -
reviewId\\. 6
,\\6 7
bool\\8 <
isAlreadyViewed\\= L
)\\L M
;\\M N
IEnumerable]] 
<]] 
ModelColorImage]] #
>]]# $
GetModelColorPhotos]]% 8
(]]8 9
U]]9 :
modelId]]; B
)]]B C
;]]C D
EnumBikeBodyStyles^^ 
GetBikeBodyType^^ *
(^^* +
uint^^+ /
modelId^^0 7
)^^7 8
;^^8 9
IEnumerable`` 
<``  
MostPopularBikesBase`` (
>``( )&
GetPopularBikesByBodyStyle``* D
(``D E
ushort``E K
bodyStyleId``L W
,``W X
uint``Y ]
topCount``^ f
,``f g
uint``h l
cityId``m s
)``s t
;``t u
ICollectionaa 
<aa  
MostPopularBikesBaseaa (
>aa( )+
GetPopularBikesByModelBodyStyleaa* I
(aaI J
intaaJ M
modelIdaaN U
,aaU V
intaaW Z
topCountaa[ c
,aac d
uintaae i
cityIdaaj p
)aap q
;aaq r
GenericBikeInfobb 
GetBikeInfobb #
(bb# $
uintbb$ (
modelIdbb) 0
,bb0 1
uintbb2 6
cityIdbb7 =
)bb= >
;bb> ?
GenericBikeInfocc 
GetBikeInfocc #
(cc# $
uintcc$ (
modelIdcc) 0
)cc0 1
;cc1 2
BikeRankingEntitydd $
GetBikeRankingByCategorydd 2
(dd2 3
uintdd3 7
modelIddd8 ?
)dd? @
;dd@ A
IEnumerableee 
<ee 
BikeMakeEntityBaseee &
>ee& '
GetMakeIfVideoee( 6
(ee6 7
)ee7 8
;ee8 9
IEnumerableff 
<ff  
SimilarBikeWithVideoff (
>ff( )!
GetSimilarBikesVideosff* ?
(ff? @
uintff@ D
modelIdffE L
,ffL M
uintffN R
totalRecordsffS _
)ff_ `
;ff` a
ICollectiongg 
<gg 
BestBikeEntityBasegg &
>gg& '"
GetBestBikesByCategorygg( >
(gg> ?
EnumBikeBodyStylesgg? Q
	bodyStyleggR [
,gg[ \
uintgg] a
?gga b
cityIdggc i
=ggj k
nullggl p
)ggp q
;ggq r
ICollectionhh 
<hh 
BestBikeEntityBasehh &
>hh& '%
GetBestBikesByModelInMakehh( A
(hhA B
uinthhB F
modelIdhhG N
)hhN O
;hhO P
ICollectionii 
<ii 
BestBikeEntityBaseii &
>ii& '%
GetBestBikesByModelInMakeii( A
(iiA B
uintiiB F
modelIdiiG N
,iiN O
uintiiP T
cityIdiiU [
)ii[ \
;ii\ ]
IEnumerablejj 
<jj %
NewLaunchedBikeEntityBasejj -
>jj- .#
GetNewLaunchedBikesListjj/ F
(jjF G
)jjG H
;jjH I
IEnumerablekk 
<kk %
NewLaunchedBikeEntityBasekk -
>kk- .#
GetNewLaunchedBikesListkk/ F
(kkF G
uintkkG K
cityIdkkL R
)kkR S
;kkS T
IEnumerablell 
<ll  
MostPopularBikesBasell (
>ll( )"
GetMostPopularScootersll* @
(ll@ A
uintllA E
makeIdllF L
)llL M
;llM N
IEnumerablemm 
<mm  
MostPopularBikesBasemm (
>mm( )"
GetMostPopularScootersmm* @
(mm@ A
uintmmA E
topCountmmF N
,mmN O
uintmmP T
?mmT U
cityIdmmV \
)mm\ ]
;mm] ^
IEnumerablenn 
<nn  
MostPopularBikesBasenn (
>nn( )"
GetMostPopularScootersnn* @
(nn@ A
uintnnA E
topCountnnF N
,nnN O
uintnnP T
makeIdnnU [
,nn[ \
uintnn] a
cityIdnnb h
)nnh i
;nni j
IEnumerableoo 
<oo !
SimilarBikeUserReviewoo )
>oo) */
#GetSimilarBikesUserReviewsWithPriceoo+ N
(ooN O
uintooO S
modelIdooT [
,oo[ \
uintoo] a
totalRecordsoob n
)oon o
;ooo p
IEnumerablepp 
<pp !
SimilarBikeUserReviewpp )
>pp) *5
)GetSimilarBikesUserReviewsWithPriceInCitypp+ T
(ppT U
uintppU Y
modelIdppZ a
,ppa b
uintppc g
cityIdpph n
,ppn o
uintppp t
totalRecords	ppu Å
)
ppÅ Ç
;
ppÇ É
IEnumerablerr 
<rr "
SimilarBikesWithPhotosrr *
>rr* +)
GetAlternativeBikesWithPhotosrr, I
(rrI J
UrrJ K
modelIdrrL S
,rrS T
ushortrrU [
totalRecordsrr\ h
)rrh i
;rri j
IEnumerabless 
<ss "
SimilarBikesWithPhotosss *
>ss* +/
#GetAlternativeBikesWithPhotosInCityss, O
(ssO P
UssP Q
modelIdssR Y
,ssY Z
ushortss[ a
totalRecordsssb n
,ssn o
uintssp t
cityIdssu {
)ss{ |
;ss| }
}tt 
}uu ã
?D:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IBikeSeries.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
{		 
public

 

	interface

 
IBikeSeries

  
<

  !
T

! "
,

" #
U

# $
>

$ %
:

& '
IRepository

( 3
<

3 4
T

4 5
,

5 6
U

6 7
>

7 8
{ 
List 
< 
BikeModelEntity 
> 
GetModelsList +
(+ ,
U, -
seriesId. 6
)6 7
;7 8!
BikeDescriptionEntity  
GetSeriesDescription 2
(2 3
U3 4
seriesId5 =
)= >
;> ?
List 
< 
BikeModelEntityBase  
>  !#
GetModelsListBySeriesId" 9
(9 :
U: ;
seriesId< D
)D E
;E F
} 
} Å
OD:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IBikeVersionCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
{ 
public 

	interface '
IBikeVersionCacheRepository 0
<0 1
T1 2
,2 3
U4 5
>5 6
{ 
IEnumerable 
< 
SimilarBikeEntity %
>% &
GetSimilarBikesList' :
(: ;
U; <
	versionId= F
,F G
uintH L
topCountM U
,U V
uintW [
cityid\ b
)b c
;c d
List 
< "
BikeVersionsListEntity #
># $
GetVersionsByType% 6
(6 7
EnumBikeType7 C
requestTypeD O
,O P
intQ T
modelIdU \
,\ ]
int^ a
?a b
cityIdc i
=j k
nulll p
)p q
;q r
T 	
GetById
 
( 
U 
	versionId 
) 
; 
List 
< 
BikeVersionMinSpecs  
>  !
GetVersionMinSpecs" 4
(4 5
uint5 9
modelId: A
,A B
boolC G
isNewH M
)M N
;N O
IEnumerable 
< 
BikeColorsbyVersion '
>' ( 
GetColorsbyVersionId) =
(= >
uint> B
	versionIdC L
)L M
;M N
} 
} Ç
AD:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IBikeVersions.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
{ 
public 

	interface 
IBikeVersions "
<" #
T# $
,$ %
U& '
>' (
:) *
IRepository+ 6
<6 7
T7 8
,8 9
U: ;
>; <
{ 
List 
< "
BikeVersionsListEntity #
># $
GetVersionsByType% 6
(6 7
EnumBikeType7 C
requestTypeD O
,O P
intQ T
modelIdU \
,\ ]
int^ a
?a b
cityIdc i
=j k
nulll p
)p q
;q r#
BikeSpecificationEntity 
GetSpecifications  1
(1 2
U2 3
	versionId4 =
)= >
;> ?
List 
< 
BikeVersionMinSpecs  
>  !
GetVersionMinSpecs" 4
(4 5
uint5 9
modelId: A
,A B
boolC G
isNewH M
)M N
;N O
IEnumerable 
< 
SimilarBikeEntity %
>% &
GetSimilarBikesList' :
(: ;
U; <
	versionId= F
,F G
uintH L
topCountM U
,U V
uintW [
cityId\ b
)b c
;c d
List 
< 
VersionColor 
> 
GetColorByVersion ,
(, -
U- .
	versionId/ 8
)8 9
;9 :
IEnumerable 
< 
BikeColorsbyVersion '
>' ( 
GetColorsbyVersionId) =
(= >
uint> B
	versionIdC L
)L M
;M N
IEnumerable 
< 
BikeVersionsSegment '
>' (
GetModelVersionsDAL) <
(< =
)= >
;> ?
IEnumerable 
< $
BikeModelVersionsDetails ,
>, -
GetModelVersions. >
(> ?
)? @
;@ A
} 
}   ƒ
AD:\work\bikewaleweb\Bikewale.Interfaces\BikeBooking\IFeedBacks.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Cancellation *
{		 
public 

	interface 

IFeedBacks 
{ 
bool 
SaveFeedBack 
( 
FeedBackEntity (
feedback) 1
)1 2
;2 3
} 
} ˛
@D:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IModelsCache.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
.& '
UpComing' /
{ 
public

 

	interface

 
IModelsCache

 !
{ 
IEnumerable 
< 
UpcomingBikeEntity &
>& '
GetUpcomingModels( 9
(9 :
): ;
;; <
} 
} à
ED:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IModelsRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
.& '
UpComing' /
{ 
public

 

	interface

 
IModelsRepository

 &
{ 
IEnumerable 
< 
UpcomingBikeEntity &
>& '
GetUpcomingModels( 9
(9 :
): ;
;; <
} 
} †
RD:\work\bikewaleweb\Bikewale.Interfaces\BikeData\NewLaunched\INewBikeLaunchesBL.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
.& '
NewLaunched' 2
{ 
public		 

	interface		 
INewBikeLaunchesBL		 '
{

 
IEnumerable 
< &
BikesCountByMakeEntityBase .
>. /
GetMakeList0 ;
(; <
)< =
;= >
IEnumerable 
< &
BikesCountByMakeEntityBase .
>. /
GetMakeList0 ;
(; <
uint< @

skipMakeIdA K
)K L
;L M
IEnumerable 
< &
BikesCountByYearEntityBase .
>. /
YearList0 8
(8 9
)9 :
;: ;!
NewLaunchedBikeResult 
GetBikes &
(& '
InputFilter' 2
filters3 :
): ;
;; <
} 
} œ
=D:\work\bikewaleweb\Bikewale.Interfaces\BikeData\IUpcoming.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
BikeData &
.& '
UpComing' /
{ 
public 

	interface 
	IUpcoming 
{ 
IEnumerable 
< 
UpcomingBikeEntity &
>& '
	GetModels( 1
(1 2(
UpcomingBikesListInputEntity2 N
inputParamsO Z
,Z [#
EnumUpcomingBikesFilter\ s
sortByt z
)z {
;{ |
BrandWidgetVM 
BindUpcomingMakes '
(' (
uint( ,
topCount- 5
)5 6
;6 7
IEnumerable 
< 
BikeMakeEntityBase &
>& '

OtherMakes( 2
(2 3
uint3 7
makeId8 >
,> ?
int@ C
topCountD L
)L M
;M N
IEnumerable 
< 
int 
> 
GetYearList $
($ %
)% &
;& '
IEnumerable 
< 
int 
> 
GetYearList $
($ %
uint% )
makeId* 0
)0 1
;1 2
IEnumerable 
< 
BikeMakeEntityBase &
>& '
GetMakeList( 3
(3 4
)4 5
;5 6
UpcomingBikeResult 
GetBikes #
(# $(
UpcomingBikesListInputEntity$ @
inputParamsA L
,L M#
EnumUpcomingBikesFilterN e
sortByf l
)l m
;m n
} 
} ≥
3D:\work\bikewaleweb\Bikewale.Interfaces\CMS\ICMS.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
CMS !
{ 
public		 

	interface		 
ICMS		 
{

 
string !
GetArticleDetailsPage $
($ %
uint% )
basicId* 1
)1 2
;2 3
string "
GetArticleDetailsPages %
(% &
uint& *
basicId+ 2
)2 3
;3 4
} 
} Ÿ
BD:\work\bikewaleweb\Bikewale.Interfaces\CMS\ICMSCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
CMS !
{ 
public 

	interface 
ICMSCacheContent %
{ 
ArticleDetails 
GetNewsDetails %
(% &
uint& *
basicId+ 2
)2 3
;3 4
ArticlePageDetails 
GetArticlesDetails -
(- .
uint. 2
basicId3 :
): ;
;; <
IEnumerable 
< 

ModelImage 
> 
GetArticlePhotos  0
(0 1
int1 4
basicId5 <
)< =
;= >
IEnumerable 
< 
ArticleSummary "
>" #)
GetMostRecentArticlesByIdList$ A
(A B
stringB H
categoryIdListI W
,W X
uintY ]
totalRecords^ j
,j k
uintl p
makeIdq w
,w x
uinty }
modelId	~ Ö
)
Ö Ü
;
Ü á
IEnumerable 
< 
ArticleSummary "
>" #)
GetMostRecentArticlesByIdList$ A
(A B
stringB H
categoryIdListI W
,W X
uintY ]
totalRecords^ j
,j k
stringk q
bodyStyleIdr }
,} ~
uint	 É
makeId
Ñ ä
,
ä ã
uint
å ê
modelId
ë ò
)
ò ô
;
ô ö

CMSContent %
GetArticlesByCategoryList ,
(, -
string- 3
categoryIdList4 B
,B C
intD G

startIndexH R
,R S
intT W
endIndexX `
,` a
intb e
makeIdf l
,l m
intn q
modelIdr y
)y z
;z {

CMSContent -
!GetTrackDayArticlesByCategoryList 4
(4 5
string5 ;
categoryIdList< J
,J K
intL O

startIndexP Z
,Z [
int\ _
endIndex` h
,h i
intj m
makeIdn t
,t u
intv y
modelId	z Å
)
Å Ç
;
Ç É

CMSContent %
GetArticlesByCategoryList ,
(, -
string- 3
categoryIdList4 B
,B C
intD G

startIndexH R
,R S
intT W
endIndexX `
,` a
stringb h
bodyStyleIdi t
,t u
intv y
makeId	z Ä
)
Ä Å
;
Å Ç
} 
} À
DD:\work\bikewaleweb\Bikewale.Interfaces\CMS\ICMSContentRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
CMS !
{		 
public 

	interface !
ICMSContentRepository *
<* +
T+ ,
,, -
V- .
>. /
{ 
IList 
< 
T 
> 
GetContentList 
(  
int  #

startIndex$ .
,. /
int0 3
endIndex4 <
,< =
out> A
intB E
recordCountF Q
,Q R
ContentFilterS `
filtersa h
)h i
;i j
V 	
GetContentDetails
 
( 
int 
	contentId  )
,) *
int+ .
pageId/ 5
)5 6
;6 7
void 
UpdateViews 
( 
int 
	contentId &
)& '
;' (
List 
< %
CMSFeaturedArticlesEntity &
>& '
GetFeaturedArticles( ;
(; <
List< @
<@ A
EnumCMSContentTypeA S
>S T
contentTypesU a
,a b
ushortc i
totalRecordsj v
)v w
;w x
List 
< %
CMSFeaturedArticlesEntity &
>& '!
GetMostRecentArticles( =
(= >
List> B
<B C
EnumCMSContentTypeC U
>U V
contentTypesW c
,c d
ushorte k
totalRecordsl x
)x y
;y z
} 
} ¨
?D:\work\bikewaleweb\Bikewale.Interfaces\Compare\IBikeCompare.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Compare %
{ 
public 

	interface 
IBikeCompare !
{ 
Entities 
. 
Compare 
. 
BikeCompareEntity *
	DoCompare+ 4
(4 5
string5 ;
versions< D
)D E
;E F
Entities 
. 
Compare 
. 
BikeCompareEntity *
	DoCompare+ 4
(4 5
string5 ;
versions< D
,D E
uintF J
cityIdK Q
)Q R
;R S
IEnumerable 
< 
TopBikeCompareBase &
>& '
CompareList( 3
(3 4
uint4 8
topCount9 A
)A B
;B C
ICollection 
< $
SimilarCompareBikeEntity ,
>, -"
GetSimilarCompareBikes. D
(D E
stringE K
versionListL W
,W X
ushortY _
topCount` h
,h i
intj m
cityidn t
)t u
;u v
ICollection 
< $
SimilarCompareBikeEntity ,
>, -*
GetSimilarCompareBikeSponsored. L
(L M
stringM S
versionListT _
,_ `
ushorta g
topCounth p
,p q
intr u
cityidv |
,| }
uint	~ Ç 
sponsoredVersionId
É ï
)
ï ñ
;
ñ ó
IEnumerable 
< 
TopBikeCompareBase &
>& '
ScooterCompareList( :
(: ;
uint; ?
topCount@ H
)H I
;I J
IEnumerable   
<   $
SimilarCompareBikeEntity   ,
>  , -!
GetPopularCompareList  . C
(  C D
uint  D H
cityId  I O
)  O P
;  P Q
IEnumerable!! 
<!! $
SimilarCompareBikeEntity!! ,
>!!, -!
GetScooterCompareList!!. C
(!!C D
uint!!D H
cityId!!I O
)!!O P
;!!P Q
}"" 
}## ∏
ND:\work\bikewaleweb\Bikewale.Interfaces\Compare\IBikeCompareCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Compare %
{ 
public 

	interface '
IBikeCompareCacheRepository 0
{ 
IEnumerable 
< 
TopBikeCompareBase &
>& '
CompareList( 3
(3 4
uint4 8
topCount9 A
)A B
;B C
BikeCompareEntity 
	DoCompare #
(# $
string$ *
versions+ 3
)3 4
;4 5
BikeCompareEntity 
	DoCompare #
(# $
string$ *
versions+ 3
,3 4
uint5 9
cityId: @
)@ A
;A B
ICollection 
< $
SimilarCompareBikeEntity ,
>, -"
GetSimilarCompareBikes. D
(D E
stringE K
versionListL W
,W X
ushortY _
topCount` h
,h i
intj m
cityidn t
)t u
;u v
ICollection 
< $
SimilarCompareBikeEntity ,
>, -*
GetSimilarCompareBikeSponsored. L
(L M
stringM S
versionListT _
,_ `
ushorta g
topCounth p
,p q
intr u
cityidv |
,| }
uint	~ Ç 
sponsoredVersionId
É ï
)
ï ñ
;
ñ ó
IEnumerable 
< 
TopBikeCompareBase &
>& '
ScooterCompareList( :
(: ;
uint; ?
topCount@ H
)H I
;I J
IEnumerable 
< $
SimilarCompareBikeEntity ,
>, -!
GetPopularCompareList. C
(C D
uintD H
cityIdI O
)O P
;P Q
IEnumerable   
<   $
SimilarCompareBikeEntity   ,
>  , -!
GetScooterCompareList  . C
(  C D
uint  D H
cityId  I O
)  O P
;  P Q
}!! 
}"" â
@D:\work\bikewaleweb\Bikewale.Interfaces\Content\IFeatureCache.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Content %
{ 
public 

	interface 
IFeatureCache "
{ 
ArticlePageDetails $
GetFeatureDetailsViaGrpc 3
(3 4
int4 7
basicId8 ?
)? @
;@ A
IEnumerable 
< 

ModelImage 
> 

BindPhotos  *
(* +
int+ .
basicId/ 6
)6 7
;7 8
} 
} •
=D:\work\bikewaleweb\Bikewale.Interfaces\Customer\ICustomer.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Customer &
{		 
public 

	interface 
	ICustomer 
< 
T  
,  !
U! "
>" #
:$ %
IRepository& 1
<1 2
T2 3
,3 4
U4 5
>5 6
{ 
T 	

GetByEmail
 
( 
string 
emailId #
)# $
;$ %
T 	
GetByEmailMobile
 
( 
string !
emailId" )
,) *
string+ 1
mobile2 8
)8 9
;9 :
} 
} ¿
KD:\work\bikewaleweb\Bikewale.Interfaces\Customer\ICustomerAuthentication.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Customer &
{		 
public 

	interface #
ICustomerAuthentication ,
<, -
T- .
,. /
U/ 0
>0 1
{ 
bool 
IsRegisteredUser 
( 
string $
email% *
)* +
;+ ,
bool 
IsRegisteredUser 
( 
string $
email% *
,* +
string+ 1
mobile2 8
)8 9
;9 :
T 	
AuthenticateUser
 
( 
string !
email" '
,' (
string) /
password0 8
,8 9
bool: >
?> ?
createAuthTicket@ P
=Q R
nullS W
)W X
;X Y
T 	
AuthenticateUser
 
( 
string !
email" '
)' (
;( )
void &
UpdateCustomerMobileNumber '
(' (
string( .
mobile/ 5
,5 6
string7 =
email> C
,C D
stringE K
nameL P
=Q R
nullS W
)W X
;X Y
void "
UpdatePasswordSaltHash #
(# $
U$ %

customerId& 0
,0 1
string2 8
passwordSalt9 E
,E F
stringG M
passwordHashN Z
)Z [
;[ \
void %
SavePasswordRecoveryToken &
(& '
U' (

customerId) 3
,3 4
string5 ;
token< A
)A B
;B C
bool (
IsValidPasswordRecoveryToken )
() *
U* +

customerId, 6
,6 7
string8 >
token? D
)D E
;E F
void +
DeactivatePasswordRecoveryToken ,
(, -
U- .

customerId/ 9
)9 :
;: ;
string '
GenerateAuthenticationToken *
(* +
string+ 1
custId2 8
,8 9
string: @
custNameA I
,I J
stringK Q
	custEmailR [
)[ \
;\ ]
} 
} ˙
GD:\work\bikewaleweb\Bikewale.Interfaces\Customer\ICustomerRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Customer &
{ 
public 

	interface 
ICustomerRepository (
<( )
T) *
,* +
U, -
>- .
:/ 0
IRepository1 <
<< =
T= >
,> ?
U@ A
>A B
{ 
T 	

GetByEmail
 
( 
string 
emailId #
)# $
;$ %
T 	
GetByEmailMobile
 
( 
string !
emailId" )
,) *
string+ 1
mobile2 8
)8 9
;9 :
void &
UpdateCustomerMobileNumber '
(' (
string( .
mobile/ 5
,5 6
string7 =
email> C
,C D
stringE K
nameL P
=Q R
nullS W
)W X
;X Y
void "
UpdatePasswordSaltHash #
(# $
U$ %

customerId& 0
,0 1
string2 8
passwordSalt9 E
,E F
stringG M
passwordHashN Z
)Z [
;[ \
void %
SavePasswordRecoveryToken &
(& '
U' (

customerId) 3
,3 4
string5 ;
token< A
)A B
;B C
bool (
IsValidPasswordRecoveryToken )
() *
U* +

customerId, 6
,6 7
string8 >
token? D
)D E
;E F
void +
DeactivatePasswordRecoveryToken ,
(, -
U- .

customerId/ 9
)9 :
;: ;
bool 
IsFakeCustomer 
( 
ulong !

customerId" ,
), -
;- .
} 
} €
9D:\work\bikewaleweb\Bikewale.Interfaces\Dealer\IDealer.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Dealer $
{ 
public 

	interface 
IDealer 
{ 
List 
< $
NewBikeDealersMakeEntity %
>% &
GetDealersMakesList' :
(: ;
); <
;< =$
NewBikeDealersListEntity  (
GetDealersCitiesListByMakeId! =
(= >
uint> B
makeIdC I
)I J
;J K
IEnumerable 
< 
CityEntityBase "
>" ##
FetchDealerCitiesByMake$ ;
(; <
uint< @
makeIdA G
)G H
;H I
List   
<   
NewBikeDealerEntity    
>    !
GetDealersList  " 0
(  0 1
uint  1 5
makeId  6 <
,  < =
uint  > B
cityId  C I
)  I J
;  J K
List!! 
<!! 
BikeMakeEntityBase!! 
>!!  &
GetDealersMakeListByCityId!!! ;
(!!; <
uint!!< @
cityId!!A G
)!!G H
;!!H I
List"" 
<"" 
CityEntityBase"" 
>""  
GetDealersCitiesList"" 1
(""1 2
)""2 3
;""3 4
List## 
<## 
CityEntityBase## 
>## '
GetDealersBookingCitiesList## 8
(##8 9
)##9 :
;##: ;
IEnumerable%% 
<%% #
NewBikeDealerEntityBase%% +
>%%+ ,!
GetNewBikeDealersList%%- B
(%%B C
int%%C F
makeId%%G M
,%%M N
int%%O R
cityId%%S Y
,%%Y Z#
EnumNewBikeDealerClient%%[ r
?%%r s
clientId%%t |
=%%} ~
null	%% É
)
%%É Ñ
;
%%Ñ Ö
bool&&  
SaveManufacturerLead&& !
(&&! ""
ManufacturerLeadEntity&&" 8
customer&&9 A
)&&A B
;&&B C
DealersEntity'' 
GetDealerByMakeCity'' )
('') *
uint''* .
cityId''/ 5
,''5 6
uint''7 ;
makeId''< B
,''B C
uint''D H
modelid''I P
=''Q R
$num''S T
)''T U
;''U V
DealerBikesEntity(( $
GetDealerDetailsAndBikes(( 2
(((2 3
uint((3 7
dealerId((8 @
,((@ A
uint((B F

campaignId((G Q
)((Q R
;((R S
DealerBikesEntity)) 3
'GetDealerDetailsAndBikesByDealerAndMake)) A
())A B
uint))B F
dealerId))G O
,))O P
int))Q T
makeId))U [
)))[ \
;))\ ]&
PopularDealerServiceCenter** " 
GetPopularCityDealer**# 7
(**7 8
uint**8 <
makeId**= C
,**C D
uint**E I
topCount**J R
)**R S
;**S T
bool++ !
UpdateManufaturerLead++ "
(++" #
uint++# '
pqId++( ,
,++, -
string++. 4
	custEmail++5 >
,++> ?
string++@ F
mobile++G M
,++M N
string++O U
response++V ^
)++^ _
;++_ `
IEnumerable,, 
<,, 
DealerBrandEntity,, %
>,,% & 
GetDealerByBrandList,,' ;
(,,; <
),,< =
;,,= >
IEnumerable-- 
<-- '
NearByCityDealerCountEntity-- /
>--/ 0'
FetchNearByCityDealersCount--1 L
(--L M
uint--M Q
makeId--R X
,--X Y
uint--Z ^
cityId--_ e
)--e f
;--f g
}.. 
}// ë
HD:\work\bikewaleweb\Bikewale.Interfaces\Dealer\IDealerCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Dealer $
{ 
public 

	interface "
IDealerCacheRepository +
{ 
DealersEntity 
GetDealerByMakeCity )
() *
uint* .
cityId/ 5
,5 6
uint7 ;
makeId< B
,B C
uintD H
modelidI P
=Q R
$numS T
)T U
;U V
DealerBikesEntity $
GetDealerDetailsAndBikes 2
(2 3
uint3 7
dealerId8 @
,@ A
uintB F

campaignIdG Q
)Q R
;R S
DealerBikesEntity 3
'GetDealerDetailsAndBikesByDealerAndMake A
(A B
uintB F
dealerIdG O
,O P
intQ T
makeIdU [
)[ \
;\ ]&
PopularDealerServiceCenter " 
GetPopularCityDealer# 7
(7 8
uint8 <
makeId= C
,C D
uintE I
topCountJ R
)R S
;S T
IEnumerable 
< $
NewBikeDealersMakeEntity ,
>, -
GetDealersMakesList. A
(A B
)B C
;C D
IEnumerable 
< 
DealerBrandEntity %
>% & 
GetDealerByBrandList' ;
(; <
)< =
;= >
IEnumerable 
< '
NearByCityDealerCountEntity /
>/ 0'
FetchNearByCityDealersCount1 L
(L M
uintM Q
makeIdR X
,X Y
uintZ ^
cityId_ e
)e f
;f g
IEnumerable 
< 
CityEntityBase "
>" ##
FetchDealerCitiesByMake$ ;
(; <
uint< @
makeIdA G
)G H
;H I
}   
}!! Ô
CD:\work\bikewaleweb\Bikewale.Interfaces\Dealer\IDealerRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Dealer $
{ 
public 

	interface 
IDealerRepository &
{ 
List 
< $
NewBikeDealersMakeEntity %
>% &
GetDealersMakesList' :
(: ;
); <
;< =$
NewBikeDealersListEntity  (
GetDealersCitiesListByMakeId! =
(= >
uint> B
makeIdC I
)I J
;J K
IEnumerable 
< 
CityEntityBase "
>" ##
FetchDealerCitiesByMake$ ;
(; <
uint< @
makeIdA G
)G H
;H I
List   
<   
NewBikeDealerEntity    
>    !
GetDealersList  " 0
(  0 1
uint  1 5
makeId  6 <
,  < =
uint  > B
cityId  C I
)  I J
;  J K
List!! 
<!! 
BikeMakeEntityBase!! 
>!!  &
GetDealersMakeListByCityId!!! ;
(!!; <
uint!!< @
cityId!!A G
)!!G H
;!!H I
List"" 
<"" 
CityEntityBase"" 
>""  
GetDealersCitiesList"" 1
(""1 2
)""2 3
;""3 4
List## 
<## 
CityEntityBase## 
>## '
GetDealersBookingCitiesList## 8
(##8 9
)##9 :
;##: ;
IEnumerable%% 
<%% #
NewBikeDealerEntityBase%% +
>%%+ ,!
GetNewBikeDealersList%%- B
(%%B C
int%%C F
makeId%%G M
,%%M N
int%%O R
cityId%%S Y
,%%Y Z#
EnumNewBikeDealerClient%%[ r
?%%r s
clientId%%t |
=%%} ~
null	%% É
)
%%É Ñ
;
%%Ñ Ö
bool&&  
SaveManufacturerLead&& !
(&&! ""
ManufacturerLeadEntity&&" 8
customer&&9 A
)&&A B
;&&B C
DealersEntity'' 
GetDealerByMakeCity'' )
('') *
uint''* .
cityId''/ 5
,''5 6
uint''7 ;
makeId''< B
,''B C
uint''D H
modelid''I P
=''Q R
$num''S T
)''T U
;''U V
DealerBikesEntity(( $
GetDealerDetailsAndBikes(( 2
(((2 3
uint((3 7
dealerId((8 @
,((@ A
uint((B F

campaignId((G Q
)((Q R
;((R S
DealerBikesEntity)) 3
'GetDealerDetailsAndBikesByDealerAndMake)) A
())A B
uint))B F
dealerId))G O
,))O P
int))Q T
makeId))U [
)))[ \
;))\ ]&
PopularDealerServiceCenter** " 
GetPopularCityDealer**# 7
(**7 8
uint**8 <
makeId**= C
,**C D
uint**E I
topCount**J R
)**R S
;**S T
bool++ !
UpdateManufaturerLead++ "
(++" #
uint++# '
pqId++( ,
,++, -
string++. 4
	custEmail++5 >
,++> ?
string++@ F
mobile++G M
,++M N
string++O U
response++V ^
)++^ _
;++_ `
IEnumerable,, 
<,, 
DealerBrandEntity,, %
>,,% & 
GetDealerByBrandList,,' ;
(,,; <
),,< =
;,,= >
IEnumerable-- 
<-- '
NearByCityDealerCountEntity-- /
>--/ 0'
FetchNearByCityDealersCount--1 L
(--L M
uint--M Q
makeId--R X
,--X Y
uint--Z ^
cityId--_ e
)--e f
;--f g
}.. 
}// ï
8D:\work\bikewaleweb\Bikewale.Interfaces\Dealer\IOffer.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Dealer $
{		 
public 

	interface 
IOffer 
{ 
List 
< 
Offer 
> 
GetOffersByDealerId '
(' (
uint( ,
dealerId- 5
,5 6
uint7 ;
modelId< C
)C D
;D E
} 
} “
<D:\work\bikewaleweb\Bikewale.Interfaces\EditCMS\IArticles.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
EditCMS %
{ 
public 

	interface 
	IArticles 
{ 
ArticleDetails 
GetNewsDetails %
(% &
uint& *
basicId+ 2
)2 3
;3 4
ArticlePageDetails 
GetArticleDetails ,
(, -
uint- 1
basicId2 9
)9 :
;: ;
IEnumerable 
< 

ModelImage 
> 
GetArticlePhotos  0
(0 1
int1 4
basicId5 <
)< =
;= >
IEnumerable 
< 
ArticleSummary "
>" #)
GetMostRecentArticlesByIdList$ A
(A B
stringB H
categoryIdListI W
,W X
uintY ]
totalRecords^ j
,j k
uintl p
makeIdq w
,w x
uinty }
modelId	~ Ö
)
Ö Ü
;
Ü á

CMSContent %
GetArticlesByCategoryList ,
(, -
string- 3
categoryIdList4 B
,B C
intD G

startIndexH R
,R S
intT W
endIndexX `
,` a
intb e
makeIdf l
,l m
intn q
modelIdr y
)y z
;z {
void 
UpdateViewCount 
( 
uint !
basicId" )
)) *
;* +
IEnumerable 
< 
ArticleSummary "
>" #)
GetMostRecentArticlesByIdList$ A
(A B
stringB H
categoryIdListI W
,W X
uintY ]
totalRecords^ j
,j k
stringk q
bodyStyleIdr }
,} ~
uint	 É
makeId
Ñ ä
,
ä ã
uint
å ê
modelId
ë ò
)
ò ô
;
ô ö

CMSContent %
GetArticlesByCategoryList ,
(, -
string- 3
categoryIdList4 B
,B C
intD G

startIndexH R
,R S
intT W
endIndexX `
,` a
stringb h
bodyStyleIdi t
,t u
intv y
makeId	z Ä
)
Ä Å
;
Å Ç
} 
} Ú
=D:\work\bikewaleweb\Bikewale.Interfaces\Feedback\IFeedback.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Feedback &
{ 
public 

	interface 
	IFeedback 
{ 
bool  
SaveCustomerFeedback !
(! "
string" (
feedbackComment) 8
,8 9
ushort: @
feedbackTypeA M
,M N
ushortO U

platformIdV `
,` a
stringb h
pageUrli p
)p q
;q r
} 
} ˙
QD:\work\bikewaleweb\Bikewale.Interfaces\GenericBikes\IBestBikesCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
GenericBikes *
{ 
public 

	interface %
IBestBikesCacheRepository .
{ 
SearchOutputEntity 
BestBikesByType *
(* +
EnumBikeBodyStyles+ =
	bodyStyle> G
,G H
FilterInputI T
filterInputsU a
,a b
InputBaseEntityc r
inputs x
)x y
;y z
} 
} Ú
RD:\work\bikewaleweb\Bikewale.Interfaces\HomePage\IHomePageBannerCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
HomePage &
{ 
public		 

	interface		 *
IHomePageBannerCacheRepository		 3
{

  
HomePageBannerEntity 
GetHomePageBanner .
(. /
uint/ 3

platformId4 >
)> ?
;? @
} 
} Ë
MD:\work\bikewaleweb\Bikewale.Interfaces\HomePage\IHomePageBannerRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
HomePage &
{ 
public		 

	interface		 %
IHomePageBannerRepository		 .
{

  
HomePageBannerEntity 
GetHomePageBanner .
(. /
uint/ 3

platformId4 >
)> ?
;? @
} 
}  
8D:\work\bikewaleweb\Bikewale.Interfaces\Images\IImage.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Images $
{ 
public		 

	interface		 
IImage		 
{

 

ImageToken $
GenerateImageUploadToken +
(+ ,
Image, 1
objImage2 :
): ;
;; <

ImageToken 
ProcessImageUpload %
(% &

ImageToken& 0
token1 6
)6 7
;7 8
} 
} ˘
BD:\work\bikewaleweb\Bikewale.Interfaces\Images\IImageRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Images $
{ 
public 

	interface 
IImageRepository %
<% &
T& '
,' (
U) *
>* +
:, -
IRepository. 9
<9 :
T: ;
,; <
U= >
>> ?
{ 
new 
UInt64 
Add 
( 
T 
t 
) 
; 
} 
} ¨
ED:\work\bikewaleweb\Bikewale.Interfaces\Insurance\IInsuranceDetail.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
	Insurance '
{ 
	interface		 
IInsuranceDetail		 
{

 
} 
} ˇ
ID:\work\bikewaleweb\Bikewale.Interfaces\Insurance\IInsuranceRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
	Insurance '
{ 
	interface		  
IInsuranceRepository		 "
<		" #
T		# $
,		$ %
U		& '
>		' (
{

 
} 
} è
6D:\work\bikewaleweb\Bikewale.Interfaces\IRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
{ 
public 

	interface 
IRepository  
<  !
T! "
," #
U$ %
>% &
{ 
U 	
Add
 
( 
T 
t 
) 
; 
bool 
Update 
( 
T 
t 
) 
; 
bool 
Delete 
( 
U 
id 
) 
; 
List 
< 
T 
> 
GetAll 
( 
) 
; 
T 	
GetById
 
( 
U 
id 
) 
; 
} 
} D
BD:\work\bikewaleweb\Bikewale.Interfaces\Location\DealersInIndia.cs£
9D:\work\bikewaleweb\Bikewale.Interfaces\Location\IArea.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Location &
{		 
public

 

	interface

 
IArea

 
{ 
List 
< 
AreaEntityBase 
> 
GetAreas %
(% &
string& ,
cityId- 3
)3 4
;4 5
IEnumerable 
< 
AreaEntityBase "
>" #
GetAreasByCity$ 2
(2 3
UInt163 9
cityId: @
)@ A
;A B
} 
} ™
HD:\work\bikewaleweb\Bikewale.Interfaces\Location\IAreaCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Location &
{ 
public 

	interface  
IAreaCacheRepository )
{ 
IEnumerable 
< 
Bikewale 
. 
Entities %
.% &
Location& .
.. /
AreaEntityBase/ =
>= >
GetAreaList? J
(J K
uintK O
modelIdP W
,W X
uintY ]
cityId^ d
)d e
;e f
} 
} ≥
9D:\work\bikewaleweb\Bikewale.Interfaces\Location\ICity.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Location &
{ 
public 

	interface 
ICity 
{ 
List 
< 
CityEntityBase 
> 
GetPriceQuoteCities 0
(0 1
uint1 5
modelId6 =
)= >
;> ?
List 
< 
CityEntityBase 
> 
GetAllCities )
() *
EnumBikeType* 6
requestType7 B
)B C
;C D
List 
< 
CityEntityBase 
> 
	GetCities &
(& '
string' -
stateId. 5
,5 6
EnumBikeType7 C
requestTypeD O
)O P
;P Q
	Hashtable 
GetMaskingNames !
(! "
)" #
;# $
	Hashtable 
GetOldMaskingNames $
($ %
)% &
;& '
DealerStateCities  
GetDealerStateCities .
(. /
uint/ 3
makeId4 :
,: ;
uint< @
stateIdA H
)H I
;I J
IEnumerable 
< 
UsedBikeCities "
>" #&
GetUsedBikeByCityWithCount$ >
(> ?
)? @
;@ A
IEnumerable 
< 
UsedBikeCities "
>" #*
GetUsedBikeByMakeCityWithCount$ B
(B C
uintC G
makeidH N
)N O
;O P
} 
} ±
HD:\work\bikewaleweb\Bikewale.Interfaces\Location\ICityCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Location &
{ 
public 

	interface  
ICityCacheRepository )
{ 
IEnumerable 
< 
CityEntityBase "
>" #
GetPriceQuoteCities$ 7
(7 8
uint8 <
modelId= D
)D E
;E F
IEnumerable 
< 
CityEntityBase "
>" #
GetAllCities$ 0
(0 1
EnumBikeType1 =
requestType> I
)I J
;J K
DealerStateCities  
GetDealerStateCities .
(. /
uint/ 3
makeId4 :
,: ;
uint< @
stateIdA H
)H I
;I J
IEnumerable 
< 
UsedBikeCities "
>" #&
GetUsedBikeByCityWithCount$ >
(> ?
)? @
;@ A
IEnumerable 
< 
UsedBikeCities "
>" #*
GetUsedBikeByMakeCityWithCount$ B
(B C
uintC G
makeidH N
)N O
;O P
CityEntityBase 
GetCityDetails %
(% &
string& ,
cityMasking- 8
)8 9
;9 :
} 
} Û
OD:\work\bikewaleweb\Bikewale.Interfaces\Location\ICityMaskingCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Location &
{		 
public

 

	interface

 '
ICityMaskingCacheRepository

 0
{ 
CityMaskingResponse "
GetCityMaskingResponse 2
(2 3
string3 9
maskingName: E
)E F
;F G
} 
} ˜
:D:\work\bikewaleweb\Bikewale.Interfaces\Location\IState.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Location &
{ 
public 

	interface 
IState 
{ 
List 
< 
StateEntityBase 
> 
	GetStates '
(' (
)( )
;) *
IEnumerable 
< 
DealerStateEntity %
>% &
GetDealerStates' 6
(6 7
uint7 ;
makeId< B
)B C
;C D
DealerLocatorList !
GetDealerStatesCities /
(/ 0
uint0 4
makeId5 ;
); <
;< =
	Hashtable 
GetMaskingNames !
(! "
)" #
;# $
} 
} ƒ
ID:\work\bikewaleweb\Bikewale.Interfaces\Location\IStateCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Location &
{ 
public

 

	interface

 !
IStateCacheRepository

 *
{ 
IEnumerable 
< 
DealerStateEntity %
>% &
GetDealerStates' 6
(6 7
uint7 ;
makeId< B
)B C
;C D
DealerLocatorList !
GetDealerStatesCities /
(/ 0
uint0 4
makeId5 ;
); <
;< = 
StateMaskingResponse #
GetStateMaskingResponse 4
(4 5
string5 ;
maskingName< G
)G H
;H I
} 
} ˘
ID:\work\bikewaleweb\Bikewale.Interfaces\MobileAppAlert\IMobileAppAlert.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
MobileAppAlert ,
{ 
public 

	interface 
IMobileAppAlert $
{		 
bool

 
SendFCMNotification

  
(

  !&
MobilePushNotificationData

! ;
payload

< C
)

C D
;

D E
bool 
SubscribeFCMUser 
( 
AppFCMInput )
input* /
)/ 0
;0 1
bool 
UnSubscribeFCMUser 
(  
AppFCMInput  +
input, 1
)1 2
;2 3
} 
} Ú
SD:\work\bikewaleweb\Bikewale.Interfaces\MobileAppAlert\IMobileAppAlertRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
MobileAppAlert ,
{ 
public 

	interface %
IMobileAppAlertRepository .
{ 
bool '
CompleteNotificationProcess (
(( )&
MobilePushNotificationData) C
payloadD K
,K L 
NotificationResponseM a
resultb h
)h i
;i j
bool 
SaveIMEIFCMData 
( 
string #
imei$ (
,( )
string* 0
gcmId1 6
,6 7
string8 >
osType? E
,E F
stringG M
subsMasterIdN Z
)Z [
;[ \
} 
} ∞
QD:\work\bikewaleweb\Bikewale.Interfaces\MobileVerification\IMobileVerification.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
MobileVerification 0
{		 
public 

	interface 
IMobileVerification (
{ $
MobileVerificationEntity  %
ProcessMobileVerification! :
(: ;
string; A
emailB G
,G H
stringI O
mobileP V
)V W
;W X
} 
} ˘
VD:\work\bikewaleweb\Bikewale.Interfaces\MobileVerification\IMobileVerificationCache.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
MobileVerification 0
{ 
public		 

	interface		 $
IMobileVerificationCache		 -
{

 
IEnumerable 
< 
string 
> 
GetBlockedNumbers -
(- .
). /
;/ 0
} 
} ¡
[D:\work\bikewaleweb\Bikewale.Interfaces\MobileVerification\IMobileVerificationRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
MobileVerification 0
{ 
public 

	interface )
IMobileVerificationRepository 2
{ 
bool 
IsMobileVerified 
( 
string $
mobileNo% -
,- .
string/ 5
emailId6 =
)= >
;> ?
sbyte 
OTPAttemptsMade 
( 
string $
mobileNo% -
,- .
string/ 5
emailId6 =
)= >
;> ?
ulong $
AddMobileNoToPendingList &
(& '
string' -
mobileNo. 6
,6 7
string8 >
emailId? F
,F G
stringH N
cwiCodeO V
,V W
stringX ^
cuiCode_ f
)f g
;g h
bool (
VerifyMobileVerificationCode )
() *
string* 0
mobileNo1 9
,9 :
string; A
cwiCodeB I
,I J
stringK Q
cuiCodeR Y
)Y Z
;Z [
IEnumerable 
< 
string 
> "
GetBlockedPhoneNumbers 2
(2 3
)3 4
;4 5
} 
} Ÿ
GD:\work\bikewaleweb\Bikewale.Interfaces\NewBikeSearch\IProcessFilter.cs
	namespace		 	
Bikewale		
 
.		 

Interfaces		 
.		 
NewBikeSearch		 +
{

 
public 

	interface 
IProcessFilter #
{ 
FilterInput 
ProcessFilters "
(" #
InputBaseEntity# 2
objInput3 ;
); <
;< =
} 
} ã
ED:\work\bikewaleweb\Bikewale.Interfaces\NewBikeSearch\ISearchQuery.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
NewBikeSearch +
{		 
public

 

	interface

 
ISearchQuery

 !
{ 
string 
GetSelectClause 
( 
)  
;  !
string 
GetFromClause 
( 
) 
; 
string 
GetWhereClause 
( 
) 
;  
string 
GetOrderByClause 
(  
)  !
;! "
string 
GetRecordCountQry  
(  !
)! "
;" #
string  
GetSearchResultQuery #
(# $
)$ %
;% &
void 
InitSearchCriteria 
(  
FilterInput  +
filterInputs, 8
)8 9
;9 :
} 
} û
FD:\work\bikewaleweb\Bikewale.Interfaces\NewBikeSearch\ISearchResult.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
NewBikeSearch +
{		 
public

 

	interface

 
ISearchResult

 "
{ 
SearchOutputEntity 
GetSearchResult *
(* +
FilterInput+ 6
filterInputs7 C
,C D
InputBaseEntityE T
inputU Z
)Z [
;[ \
} 
} ´
7D:\work\bikewaleweb\Bikewale.Interfaces\Pager\IPager.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Pager #
{ 
public 

	interface 
IPager 
{		 
T

 	
GetPager


 
<

 
T

 
>

 
(

 
PagerEntity

 !
pagerDetails

" .
)

. /
where

0 5
T

6 7
:

8 9
PagerOutputEntity

: K
,

K L
new

M P
(

P Q
)

Q R
;

R S
T 	
GetUsedBikePager
 
< 
T 
> 
( 
PagerEntity )
pagerDetails* 6
)6 7
where8 =
T> ?
:@ A
PagerOutputEntityB S
,S T
newU X
(X Y
)Y Z
;Z [
void 
GetStartEndIndex 
( 
int !
pageSize" *
,* +
int, /
currentPageNo0 =
,= >
out? B
intC F

startIndexG Q
,Q R
outS V
intW Z
endIndex[ c
)c d
;d e
int 
GetTotalPages 
( 
int 
totalRecords *
,* +
int, /
pageSize0 8
)8 9
;9 :
Bikewale 
. 
Models 
. 
Shared 
. 
Pager $
GetPagerControl% 4
(4 5
PagerEntity5 @
objPageA H
)H I
;I J
} 
} ß
MD:\work\bikewaleweb\Bikewale.Interfaces\PriceQuote\IDealerPriceQuoteDetail.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 

PriceQuote (
{ 
public 

	interface #
IDealerPriceQuoteDetail ,
{ )
DetailedDealerQuotationEntity %
GetDealerQuotation& 8
(8 9
UInt329 ?
cityId@ F
,F G
UInt32H N
	versionIDO X
,X Y
UInt32Z `
dealerIda i
)i j
;j k
Bikewale 
. 
Entities 
. 

PriceQuote $
.$ %
v2% '
.' ()
DetailedDealerQuotationEntity( E 
GetDealerQuotationV2F Z
(Z [
UInt32[ a
cityIdb h
,h i
UInt32j p
	versionIDq z
,z {
UInt32	| Ç
dealerId
É ã
,
ã å
uint
ç ë
areaId
í ò
)
ò ô
;
ô ö
PQ_QuotationEntity 
	Quotation $
($ %
uint% )
cityId* 0
,0 1
UInt162 8

sourceType9 C
,C D
stringE K
deviceIdL T
,T U
uintV Z
dealerId[ c
,c d
uinte i
modelIdj q
,q r
refs v
ulongw |
pqId	} Å
,
Å Ç
bool
É á
isPQRegistered
à ñ
,
ñ ó
uint
ò ú
?
ú ù
areaId
û §
=
• ¶
null
ß ´
,
´ ¨
uint
≠ ±
?
± ≤
	versionId
≥ º
=
Ω æ
null
ø √
)
√ ƒ
;
ƒ ≈
} 
} Ÿ
GD:\work\bikewaleweb\Bikewale.Interfaces\PriceQuote\ILeadNofitication.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 

PriceQuote (
{ 
public 

	interface 
ILeadNofitication &
{ 
void 
NotifyCustomer 
( 
uint  
pqId! %
,% &
string' -
bikeName. 6
,6 7
string8 >
	bikeImage? H
,H I
stringJ P

dealerNameQ [
,[ \
string] c
dealerEmaild o
,o p
stringq w
dealerMobileNo	x Ü
,
Ü á
string
à é
organization
è õ
,
õ ú
string
ù £
address
§ ´
,
´ ¨
string
≠ ≥
customerName
¥ ¿
,
¿ ¡
string
¬ »
customerEmail
… ÷
,
÷ ◊
List
ÿ ‹
<
‹ ›
PQ_Price
› Â
>
Â Ê
	priceList
Á 
,
 Ò
List
Ú ˆ
<
ˆ ˜
OfferEntity
˜ Ç
>
Ç É
	offerList
Ñ ç
,
ç é
string
è ï
pinCode
ñ ù
,
ù û
string
ü •
	stateName
¶ Ø
,
Ø ∞
string
± ∑
cityName
∏ ¿
,
¿ ¡
uint
¬ ∆

totalPrice
« —
,
— “
DPQSmsEntity
” ﬂ
objDPQSmsEntity
‡ Ô
,
Ô 
string
Ò ˜

requestUrl
¯ Ç
,
Ç É
uint
Ñ à
?
à â
leadSourceId
ä ñ
,
ñ ó
string
ò û
versionName
ü ™
,
™ ´
double
¨ ≤
	dealerLat
≥ º
,
º Ω
double
æ ƒ

dealerLong
≈ œ
,
œ –
string
— ◊
workingHours
ÿ ‰
,
‰ Â
string
Ê Ï

platformId
Ì ˜
=
¯ ˘
$str
˙ ¸
)
¸ ˝
;
˝ ˛
void 
NotifyDealer 
( 
uint 
pqId #
,# $
string% +
makeName, 4
,4 5
string6 <
	modelName= F
,F G
stringH N
versionNameO Z
,Z [
string\ b

dealerNamec m
,m n
stringo u
dealerEmail	v Å
,
Å Ç
string
É â
customerName
ä ñ
,
ñ ó
string
ò û
customerEmail
ü ¨
,
¨ ≠
string
Æ ¥
customerMobile
µ √
,
√ ƒ
string
≈ À
areaName
Ã ‘
,
‘ ’
string
÷ ‹
cityName
› Â
,
Â Ê
List
Á Î
<
Î Ï
PQ_Price
Ï Ù
>
Ù ı
	priceList
ˆ ˇ
,
ˇ Ä
int
Å Ñ

totalPrice
Ö è
,
è ê
List
ë ï
<
ï ñ
OfferEntity
ñ °
>
° ¢
	offerList
£ ¨
,
¨ ≠
string
Æ ¥
	imagePath
µ æ
,
æ ø
string
¿ ∆
dealerMobile
« ”
,
” ‘
string
’ €
bikeName
‹ ‰
,
‰ Â
string
Ê Ï

dealerArea
Ì ˜
)
˜ ¯
;
¯ ˘
void 
PushtoAB 
( 
string 
dealerId %
,% &
uint' +
pqId, 0
,0 1
string2 8
customerName9 E
,E F
stringG M
customerMobileN \
,\ ]
string^ d
customerEmaile r
,r s
stringt z
	versionId	{ Ñ
,
Ñ Ö
string
Ü å
cityId
ç ì
)
ì î
;
î ï
bool 
PushLeadToGaadi 
( "
ManufacturerLeadEntity 3

leadEntity4 >
)> ?
;? @
} 
} Ñ
AD:\work\bikewaleweb\Bikewale.Interfaces\PriceQuote\IPriceQuote.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 

PriceQuote (
{ 
public 

	interface 
IPriceQuote  
{ 
ulong 
RegisterPriceQuote  
(  !&
PriceQuoteParametersEntity! ;
pqParams< D
)D E
;E F
BikeQuotationEntity 
GetPriceQuoteById -
(- .
ulong. 3
pqId4 8
)8 9
;9 :
BikeQuotationEntity 
GetPriceQuote )
() *&
PriceQuoteParametersEntity* D
pqParamsE M
)M N
;N O
List 
< "
OtherVersionInfoEntity #
># $"
GetOtherVersionsPrices% ;
(; <
ulong< A
pqIdB F
)F G
;G H
IEnumerable 
< "
OtherVersionInfoEntity *
>* +"
GetOtherVersionsPrices, B
(B C
uintC G
modelIdH O
,O P
uintQ U
cityIdV \
)\ ]
;] ^
bool 
UpdatePriceQuote 
( 
UInt32 $
pqId% )
,) *&
PriceQuoteParametersEntity+ E
pqParamsF N
)N O
;O P
bool 
SaveBookingState 
( 
UInt32 $
pqId% )
,) *
PriceQuoteStates+ ;
state< A
)A B
;B C&
PriceQuoteParametersEntity "&
FetchPriceQuoteDetailsById# =
(= >
UInt64> D
pqIdE I
)I J
;J K
IEnumerable 
< !
PriceQuoteOfTopCities )
>) *&
FetchPriceQuoteOfTopCities+ E
(E F
uintF J
modelIdK R
,R S
uintT X
topCountY a
)a b
;b c
IEnumerable 
< !
PriceQuoteOfTopCities )
>) *(
GetModelPriceInNearestCities+ G
(G H
uintH L
modelIdM T
,T U
uintV Z
cityId[ a
,a b
ushortc i
topCountj r
)r s
;s t
IEnumerable 
< 
BikeQuotationEntity '
>' (%
GetVersionPricesByModelId) B
(B C
uintC G
modelIdH O
,O P
uintQ U
cityIdV \
,\ ]
out^ a
boolb f
HasAreag n
)n o
;o p
BikeQuotationEntity 
GetPriceQuoteById -
(- .
ulong. 3
p4 5
,5 6
Entities7 ?
.? @
BikeBooking@ K
.K L
LeadSourceEnumL Z
leadSourceEnum[ i
)i j
;j k
IEnumerable 
< 
ManufacturerDealer &
>& '"
GetManufacturerDealers( >
(> ?
)? @
;@ A
}   
}!! ß
FD:\work\bikewaleweb\Bikewale.Interfaces\PriceQuote\IPriceQuoteCache.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 

PriceQuote (
{ 
public 

	interface 
IPriceQuoteCache %
{ 
IEnumerable 
< !
PriceQuoteOfTopCities )
>) *+
FetchPriceQuoteOfTopCitiesCache+ J
(J K
uintK O
modelIdP W
,W X
uintY ]
topCount^ f
)f g
;g h
IEnumerable 
< !
PriceQuoteOfTopCities )
>) *(
GetModelPriceInNearestCities+ G
(G H
uintH L
modelIdM T
,T U
uintV Z
cityId[ a
,a b
ushortc i
topCountj r
)r s
;s t
IEnumerable 
< "
OtherVersionInfoEntity *
>* +"
GetOtherVersionsPrices, B
(B C
uintC G
modelIdH O
,O P
uintQ U
cityIdV \
)\ ]
;] ^
IEnumerable 
< 
ManufacturerDealer &
>& '"
GetManufacturerDealers( >
(> ?
uint? C
cityIdD J
,J K
uintL P
dealerIdQ Y
)Y Z
;Z [
} 
} Ì
BD:\work\bikewaleweb\Bikewale.Interfaces\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str .
). /
]/ 0
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
$str 0
)0 1
]1 2
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
]$$) *Ä
KD:\work\bikewaleweb\Bikewale.Interfaces\PWA\CMS\IPWACMSContentRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
PWA !
.! "
CMS" %
{ 
public

 

	interface

 $
IPWACMSContentRepository

 -
{ 
IHtmlString 
GetNewsListDetails &
(& '%
PwaNewsArticleListReducer' @
reducerA H
,H I
stringJ P
urlQ T
,T U
stringV \
containerId] h
,h i
stringj p
componentNameq ~
)~ 
;	 Ä
IHtmlString 
GetNewsDetails "
(" # 
PwaNewsDetailReducer# 7
reducer8 ?
,? @
stringA G
urlH K
,K L
stringM S
containerIdT _
,_ `
stringa g
componentNameh u
)u v
;v w
} 
} Ì
ID:\work\bikewaleweb\Bikewale.Interfaces\PWA\CMS\IPWACMSCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
PWA !
.! "
CMS" %
{ 
public 

	interface "
IPWACMSCacheRepository +
{ 
IHtmlString 
GetNewsListDetails &
(& '
string' -
key. 1
,1 2%
PwaNewsArticleListReducer3 L
reducerM T
,T U
stringU [
url\ _
,_ `
stringa g
containerIdh s
,s t
stringt z
componentName	{ à
)
à â
;
â ä
IHtmlString 
GetNewsDetails "
(" #
string# )
key* -
,- . 
PwaNewsDetailReducer/ C
reducerD K
,K L
stringM S
urlT W
,W X
stringY _
containerId` k
,k l
stringm s
componentName	t Å
)
Å Ç
;
Ç É
} 
} Ú

GD:\work\bikewaleweb\Bikewale.Interfaces\ServiceCenter\IServiceCenter.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
ServiceCenter +
{ 
public 

	interface 
IServiceCenter #
{ 
IEnumerable 
< 
CityEntityBase "
>" #"
GetServiceCenterCities$ :
(: ;
uint; ?
makeId@ F
)F G
;G H
ServiceCenterData #
GetServiceCentersByCity 1
(1 2
uint2 6
cityId7 =
,= >
int? B
makeIdC I
)I J
;J K
IEnumerable 
<  
ModelServiceSchedule (
>( )$
GetServiceScheduleByMake* B
(B C
uintC G
makeIdH N
)N O
;O P%
ServiceCenterCompleteData !$
GetServiceCenterDataById" :
(: ;
uint; ?
serviceCenterId@ O
)O P
;P Q&
EnumServiceCenterSMSStatus "#
GetServiceCenterSMSData# :
(: ;
uint; ?
serviceCenterId@ O
,O P
stringQ W
mobileNumberX d
,d e
stringf l
pageUrlm t
)t u
;u v
} 
} õ
=D:\work\bikewaleweb\Bikewale.Interfaces\Security\ISecurity.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Security &
{ 
public 

	interface 
	ISecurity 
{		 
Bikewale

 
.

 
Entities

 
.

 
AWS

 
.

 
Token

 #
GetToken

$ ,
(

, -
)

- .
;

. /
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
} Ö
VD:\work\bikewaleweb\Bikewale.Interfaces\ServiceCenter\IServiceCenterCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
ServiceCenter +
{ 
public 

	interface )
IServiceCenterCacheRepository 2
{ $
ServiceCenterLocatorList   
GetServiceCenterList! 5
(5 6
uint6 :
makeid; A
)A B
;B C
IEnumerable 
< 
CityEntityBase "
>" #"
GetServiceCenterCities$ :
(: ;
uint; ?
makeid@ F
)F G
;G H
ServiceCenterData #
GetServiceCentersByCity 1
(1 2
uint2 6
cityId7 =
,= >
int? B
makeIdC I
)I J
;J K
IEnumerable 
<  
ModelServiceSchedule (
>( )$
GetServiceScheduleByMake* B
(B C
uintC G
makeIdH N
)N O
;O P%
ServiceCenterCompleteData !$
GetServiceCenterDataById" :
(: ;
uint; ?
serviceCenterId@ O
)O P
;P Q
IEnumerable 
< 
BrandServiceCenters '
>' ('
GetAllServiceCentersByBrand) D
(D E
)E F
;F G
IEnumerable 
< #
CityBrandServiceCenters +
>+ ,0
$GetServiceCentersNearbyCitiesByBrand- Q
(Q R
intR U
cityIdV \
,\ ]
int^ a
makeIdb h
,h i
intj m
topCountn v
)v w
;w x
}   
}!! ô
QD:\work\bikewaleweb\Bikewale.Interfaces\ServiceCenter\IServiceCenterRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
ServiceCenter +
{ 
public 

	interface $
IServiceCenterRepository -
<- .
T. /
,/ 0
U1 2
>2 3
:4 5
IRepository6 A
<A B
TB C
,C D
UE F
>F G
{ $
ServiceCenterLocatorList   
GetServiceCenterList! 5
(5 6
uint6 :
makeId; A
)A B
;B C
IEnumerable 
< 
CityEntityBase "
>" #"
GetServiceCenterCities$ :
(: ;
uint; ?
makeId@ F
)F G
;G H
ServiceCenterData #
GetServiceCentersByCity 1
(1 2
uint2 6
cityId7 =
,= >
int? B
makeIdC I
)I J
;J K
IEnumerable 
<  
ModelServiceSchedule (
>( )$
GetServiceScheduleByMake* B
(B C
uintC G
makeIdH N
)N O
;O P%
ServiceCenterCompleteData !$
GetServiceCenterDataById" :
(: ;
uint; ?
serviceCenterId@ O
)O P
;P Q 
ServiceCenterSMSData #
GetServiceCenterSMSData 4
(4 5
uint5 9
serviceCenterId: I
,I J
stringK Q
mobileNumberR ^
)^ _
;_ `
IEnumerable 
< 
BrandServiceCenters '
>' ('
GetAllServiceCentersByBrand) D
(D E
)E F
;F G
IEnumerable 
< #
CityBrandServiceCenters +
>+ ,0
$GetServiceCentersNearbyCitiesByBrand- Q
(Q R
intR U
cityIdV \
,\ ]
int] `
makeIda g
,g h
inth k
topCountl t
)t u
;u v
} 
} ≥
CD:\work\bikewaleweb\Bikewale.Interfaces\Survey\ISurveyRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
{		 
public 

	interface 
ISurveyRepository &
{ 
void %
InsertBajajSurveyResponse &
(& '
BajajSurveyVM' 4
surveryResponse5 D
)D E
;E F
} 
public 

	interface 
ISurvey 
{ 
void %
InsertBajajSurveyResponse &
(& '
BajajSurveyVM' 4
surveryResponse5 D
)D E
;E F
} 
} Û
>D:\work\bikewaleweb\Bikewale.Interfaces\Used\ISearchFilters.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
." #
Search# )
{ 
public 

	interface 
ISearchFilters #
{		 !
ProcessedInputFilters

 
ProcessFilters

 ,
(

, -
InputFilters

- 9

objFilters

: D
)

D E
;

E F
} 
} ‡
7D:\work\bikewaleweb\Bikewale.Interfaces\Used\ISearch.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
." #
Search# )
{		 
public 

	interface 
ISearch 
{ 
SearchResult 
GetUsedBikesList %
(% &
InputFilters& 2
inputFilters3 ?
)? @
;@ A
} 
} Ë
<D:\work\bikewaleweb\Bikewale.Interfaces\Used\ISearchQuery.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
." #
Search# )
{		 
public 

	interface 
ISearchQuery !
{ 
string  
GetSearchResultQuery #
(# $
InputFilters$ 0
inputFilters1 =
)= >
;> ?
} 
} ﬂ
?D:\work\bikewaleweb\Bikewale.Interfaces\UsedBikes\IUsedBikes.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
	UsedBikes '
{ 
public 

	interface 

IUsedBikes 
{ 
IEnumerable 
< 
MostRecentBikes #
># $
GetPopularUsedBikes% 8
(8 9
uint9 =
makeId> D
,D E
uintF J
modelIdK R
,R S
uintT X
cityIdY _
,_ `
uinta e

totalCountf p
)p q
;q r
InquiryDetails (
GetInquiryDetailsByProfileId 3
(3 4
string4 :
	profileId; D
,D E
stringF L

customerIdM W
,W X
stringY _

platformId` j
)j k
;k l
IEnumerable 
< 
UsedBikeMakeEntity &
>& '%
GetUsedBikeMakesWithCount( A
(A B
)B C
;C D
} 
} Œ
DD:\work\bikewaleweb\Bikewale.Interfaces\UsedBikes\IUsedBikesCache.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
	UsedBikes '
{ 
public 

	interface 
IUsedBikesCache $
{ 
IEnumerable 
< 
MostRecentBikes #
># $
GetUsedBikes% 1
(1 2
uint2 6
makeId7 =
,= >
uint? C
modelIdD K
,K L
uintM Q
cityIdR X
,X Y
uintZ ^

totalCount_ i
)i j
;j k
IEnumerable 
< 
UsedBikeMakeEntity &
>& '%
GetUsedBikeMakesWithCount( A
(A B
)B C
;C D
} 
} û
ID:\work\bikewaleweb\Bikewale.Interfaces\UsedBikes\IUsedBikesRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
	UsedBikes '
{ 
public 

	interface  
IUsedBikesRepository )
{ 
IEnumerable 
< "
PopularUsedBikesEntity *
>* +
GetPopularUsedBikes, ?
(? @
uint@ D

totalCountE O
,O P
intQ T
?T U
cityV Z
=[ \
null] a
)a b
;b c
IEnumerable 
< 
MostRecentBikes #
># $
GetUsedBikesbyMake% 7
(7 8
uint8 <
makeId= C
,C D
uintE I

totalCountJ T
)T U
;U V
IEnumerable 
< 
MostRecentBikes #
># $
GetUsedBikesbyModel% 8
(8 9
uint9 =
modelId> E
,E F
uintG K

totalCountL V
)V W
;W X
IEnumerable 
< 
MostRecentBikes #
># $#
GetUsedBikesbyModelCity% <
(< =
uint= A
modelIdB I
,I J
uintK O
cityIdP V
,V W
uintX \

totalCount] g
)g h
;h i
IEnumerable 
< 
MostRecentBikes #
># $"
GetUsedBikesbyMakeCity% ;
(; <
uint< @
makeIdA G
,G H
uintI M
cityIdN T
,T U
uintV Z

totalCount[ e
)e f
;f g
IEnumerable 
< 
UsedBikeMakeEntity &
>& '%
GetUsedBikeMakesWithCount( A
(A B
)B C
;C D
} 
} Ù
UD:\work\bikewaleweb\Bikewale.Interfaces\UsedBikes\IPopularUsedBikesCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
	UsedBikes '
{		 
public

 

	interface

 ,
 IPopularUsedBikesCacheRepository

 5
{ 
IEnumerable 
< "
PopularUsedBikesEntity *
>* +
GetPopularUsedBikes, ?
(? @
uint@ D
topCountE M
,M N
intO R
?R S
cityIdT Z
)Z [
;[ \
} 
} Ì
AD:\work\bikewaleweb\Bikewale.Interfaces\Used\ISearchRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
." #
Search# )
{		 
public 

	interface 
ISearchRepository &
{ 
SearchResult 
GetUsedBikesList %
(% &
string& ,
searchQuery- 8
)8 9
;9 :
} 
} 
:D:\work\bikewaleweb\Bikewale.Interfaces\Used\ISellBikes.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
{ 
public 

	interface 

ISellBikes 
{ '
SellBikeInquiryResultEntity #
SaveSellBikeAd$ 2
(2 3

SellBikeAd3 =
ad> @
)@ A
;A B
bool "
UpdateOtherInformation #
(# $&
SellBikeAdOtherInformation$ >
adInformation? L
,L M
intN Q
	inquiryAdR [
,[ \
ulong] b

customerIdc m
)m n
;n o

SellBikeAd 
GetById 
( 
int 
	inquiryId (
,( )
ulong* /

customerId0 :
): ;
;; <
bool 
VerifyMobile 
( 
SellerEntity &
seller' -
)- .
;. /
void 
SendNotification 
( 

SellBikeAd (
ad) +
)+ ,
;, -
bool 
IsFakeCustomer 
( 
ulong !

customerId" ,
), -
;- .
bool 
RemoveBikePhotos 
( 
ulong #

customerId$ .
,. /
string0 6
	profileId7 @
,@ A
stringB H
photoIdI P
)P Q
;Q R+
SellBikeImageUploadResultEntity '
UploadBikeImage( 7
(7 8
bool8 <
isMain= C
,C D
ulongE J

customerIdK U
,U V
stringW ]
	profileId^ g
,g h
stringi o
fileExtensionp }
,} ~
string	 Ö
description
Ü ë
)
ë í
;
í ì
bool 
MakeMainImage 
( 
uint 
photoId  '
,' (
ulong) .

customerId/ 9
,9 :
string; A
	profileIdB K
)K L
;L M
void 
ChangeInquiryStatus  
(  !
uint! %
	inquiryId& /
,/ 0
SellAdStatus1 =
status> D
)D E
;E F
} 
} ù
DD:\work\bikewaleweb\Bikewale.Interfaces\Used\ISellBikesRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
{ 
public 

	interface  
ISellBikesRepository )
<) *
T* +
,+ ,
U- .
>. /
:0 1
IRepository2 =
<= >
T> ?
,? @
UA B
>B C
{ 
new 
int 
Add 
( 
T 
ad 
) 
; 
T 	
GetById
 
( 
U 
	inquiryId 
, 
UInt64 %

customerId& 0
)0 1
;1 2
bool "
UpdateOtherInformation #
(# $&
SellBikeAdOtherInformation$ >
	otherInfo? H
,H I
UJ K
	inquiryIdL U
,U V
UInt64W ]

customerId^ h
)h i
;i j
string 
SaveBikePhotos 
( 
bool "
isMain# )
,) *
bool+ /
isDealer0 8
,8 9
U: ;
	inquiryId< E
,E F
stringG M
originalImageNameN _
,_ `
stringa g
descriptionh s
)s t
;t u
string '
UploadImageToCommonDatabase *
(* +
string+ 1
photoId2 9
,9 :
string; A
	imageNameB K
,K L
ImageCategoriesM \
imgC] a
,a b
stringc i
directoryPathj w
)w x
;x y
IEnumerable 
< 
	BikePhoto 
> 
GetBikePhotos ,
(, -
U- .
	inquiryId/ 8
,8 9
bool: >

isApproved? I
)I J
;J K
bool 
MarkMainImage 
( 
U 
	inquiryId &
,& '
uint( ,
photoId- 4
,4 5
bool6 :
isDealer; C
)C D
;D E
void 
ChangeInquiryStatus  
(  !
uint! %
	inquiryId& /
,/ 0
SellAdStatus1 =
status> D
)D E
;E F
} 
} Ø
>D:\work\bikewaleweb\Bikewale.Interfaces\Used\IUsedBikeBuyer.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
{ 
public

 

	interface

 
IUsedBikeBuyer

 #
{ 
bool 
UploadPhotosRequest  
(  !
PhotoRequest! -
request. 5
)5 6
;6 7
BikeInterestDetails 
ShowInterestInBike .
(. /
CustomerEntityBase/ A
buyerB G
,G H
stringI O
	profileIdP Y
,Y Z
bool[ _
isDealer` h
)h i
;i j'
PurchaseInquiryResultEntity #!
SubmitPurchaseInquiry$ 9
(9 :
CustomerEntityBase: L
buyerM R
,R S
stringT Z
	profileId[ d
,d e
stringf l
pageUrlm t
,t u
ushortv |
sourceId	} Ö
)
Ö Ü
;
Ü á
} 
} á

HD:\work\bikewaleweb\Bikewale.Interfaces\Used\IUsedBikeBuyerRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
{ 
public		 

	interface		 $
IUsedBikeBuyerRepository		 -
{

 
bool 
IsBuyerEligible 
( 
string #
mobile$ *
)* +
;+ ,
bool 
UploadPhotosRequest  
(  !
string! '
sellInquiryId( 5
,5 6
UInt647 =
buyerId> E
,E F
byteG K
consumerTypeL X
,X Y
stringZ `
buyerMessagea m
)m n
;n o
bool &
HasShownInterestInUsedBike '
(' (
bool( ,
isDealer- 5
,5 6
string7 =
	inquiryId> G
,G H
UInt64I O
buyerIdP W
)W X
;X Y
bool 
IsPhotoRequestDone 
(  
string  &
sellInquiryId' 4
,4 5
UInt646 <
buyerId= D
,D E
boolF J
isDealerK S
)S T
;T U
} 
} À
@D:\work\bikewaleweb\Bikewale.Interfaces\Used\IUsedBikeDetails.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
{ 
public 

	interface 
IUsedBikeDetails %
{ $
ClassifiedInquiryDetails  
GetProfileDetails! 2
(2 3
uint3 7
	inquiryId8 A
)A B
;B C
IEnumerable 
< 
BikeDetailsMin "
>" #
GetSimilarBikes$ 3
(3 4
uint4 8
	inquiryId9 B
,B C
uintD H
cityIdI O
,O P
uintQ U
modelIdV ]
,] ^
ushort_ e
topCountf n
)n o
;o p
IEnumerable 
<  
OtherUsedBikeDetails (
>( )!
GetOtherBikesByCityId* ?
(? @
uint@ D
	inquiryIdE N
,N O
uintP T
cityIdU [
,[ \
ushort] c
topCountd l
)l m
;m n
InquiryDetails   (
GetInquiryDetailsByProfileId   3
(  3 4
string  4 :
	profileId  ; D
,  D E
string  F L

customerId  M W
)  W X
;  X Y
IEnumerable!! 
<!!  
OtherUsedBikeDetails!! (
>!!( )%
GetRecentUsedBikesInIndia!!* C
(!!C D
ushort!!D J
topCount!!K S
)!!S T
;!!T U
IEnumerable"" 
<"" 
	BikePhoto"" 
>"" 
GetBikePhotos"" ,
("", -
uint""- 1
	inquiryId""2 ;
,""; <
bool""= A

isApproved""B L
)""L M
;""M N
IEnumerable## 
<## 
MostRecentBikes## #
>### $)
GetUsedBikeByModelCountInCity##% B
(##B C
uint##C G
makeid##H N
,##N O
uint##P T
cityid##U [
,##[ \
uint##] a
topcount##b j
)##j k
;##k l
IEnumerable$$ 
<$$ 
MostRecentBikes$$ #
>$$# $"
GetUsedBikeCountInCity$$% ;
($$; <
uint$$< @
cityid$$A G
,$$G H
uint$$I M
topcount$$N V
)$$V W
;$$W X
IEnumerable%% 
<%%  
UsedBikesCountInCity%% (
>%%( ))
GetUsedBikeInCityCountByModel%%* G
(%%G H
uint%%H L
modelId%%M T
,%%T U
ushort%%V \
topCount%%] e
)%%e f
;%%f g
IEnumerable&& 
<&&  
UsedBikesCountInCity&& (
>&&( )(
GetUsedBikeInCityCountByMake&&* F
(&&F G
uint&&G K
makeId&&L R
,&&R S
ushort&&T Z
topCount&&[ c
)&&c d
;&&d e
IEnumerable'' 
<'' 
MostRecentBikes'' #
>''# $&
GetPopularUsedModelsByMake''% ?
(''? @
uint''@ D
makeid''E K
,''K L
uint''M Q
topcount''R Z
)''Z [
;''[ \
IEnumerable(( 
<(( 
MostRecentBikes(( #
>((# $
GetUsedBike((% 0
(((0 1
uint((1 5
topcount((6 >
)((> ?
;((? @
})) 
}** Ò
OD:\work\bikewaleweb\Bikewale.Interfaces\Used\IUsedBikeDetailsCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
{ 
public 

	interface +
IUsedBikeDetailsCacheRepository 4
{ $
ClassifiedInquiryDetails  
GetProfileDetails! 2
(2 3
uint3 7
	inquiryId8 A
)A B
;B C
IEnumerable 
< 
BikeDetailsMin "
>" #
GetSimilarBikes$ 3
(3 4
uint4 8
	inquiryId9 B
,B C
uintD H
cityIdI O
,O P
uintQ U
modelIdV ]
,] ^
ushort_ e
topCountf n
)n o
;o p
IEnumerable 
<  
OtherUsedBikeDetails (
>( )!
GetOtherBikesByCityId* ?
(? @
uint@ D
	inquiryIdE N
,N O
uintP T
cityIdU [
,[ \
ushort] c
topCountd l
)l m
;m n
InquiryDetails (
GetInquiryDetailsByProfileId 3
(3 4
string4 :
	profileId; D
,D E
stringF L

customerIdM W
)W X
;X Y
IEnumerable 
<  
OtherUsedBikeDetails (
>( )%
GetRecentUsedBikesInIndia* C
(C D
ushortD J
topCountK S
)S T
;T U
IEnumerable 
< 
MostRecentBikes #
># $)
GetUsedBikeByModelCountInCity% B
(B C
uintC G
makeidH N
,N O
uintP T
cityidU [
,[ \
uint] a
topcountb j
)j k
;k l
IEnumerable 
< 
MostRecentBikes #
># $"
GetUsedBikeCountInCity% ;
(; <
uint< @
cityidA G
,G H
uintI M
topcountN V
)V W
;W X
IEnumerable   
<    
UsedBikesCountInCity   (
>  ( ))
GetUsedBikeInCityCountByModel  * G
(  G H
uint  H L
modelId  M T
,  T U
ushort  V \
topCount  ] e
)  e f
;  f g
IEnumerable!! 
<!!  
UsedBikesCountInCity!! (
>!!( )(
GetUsedBikeInCityCountByMake!!* F
(!!F G
uint!!G K
makeId!!L R
,!!R S
ushort!!T Z
topCount!![ c
)!!c d
;!!d e
IEnumerable"" 
<"" 
MostRecentBikes"" #
>""# $&
GetPopularUsedModelsByMake""% ?
(""? @
uint""@ D
makeid""E K
,""K L
uint""M Q
topcount""R Z
)""Z [
;""[ \
IEnumerable## 
<## 
MostRecentBikes## #
>### $
GetUsedBike##% 0
(##0 1
uint##1 5
topcount##6 >
)##> ?
;##? @
}$$ 
}%% Û
?D:\work\bikewaleweb\Bikewale.Interfaces\Used\IUsedBikeSeller.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
{ 
public 

	interface 
IUsedBikeSeller $
{		 
bool

 
RepostSellBikeAd

 
(

 
int

 !
	inquiryId

" +
,

+ ,
ulong

- 2

customerId

3 =
)

= >
;

> ?
} 
} À
ID:\work\bikewaleweb\Bikewale.Interfaces\Used\IUsedBikeSellerRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Used "
{ 
public 

	interface %
IUsedBikeSellerRepository .
{ 
UsedBikeSellerBase 
GetSellerDetails +
(+ ,
string, 2
	inquiryId3 <
,< =
bool> B
isDealerC K
)K L
;L M
int 
SaveCustomerInquiry 
(  
string  &
	inquiryId' 0
,0 1
ulong2 7

customerId8 B
,B C
UInt16D J
sourceIdK S
,S T
outU X
boolY ]
isNew^ c
)c d
;d e'
ClassifiedInquiryDetailsMin #
GetInquiryDetails$ 5
(5 6
string6 <
	inquiryId= F
)F G
;G H
bool 
RemoveBikePhotos 
( 
int !
	inquiryId" +
,+ ,
string- 3
photoId4 ;
); <
;< =
bool 
RepostSellBikeAd 
( 
int !
	inquiryId" +
,+ ,
ulong- 2

customerId3 =
)= >
;> ?
IEnumerable 
< "
CustomerListingDetails *
>* +%
GetCustomerListingDetails, E
(E F
uintF J
	cutomerIdK T
)T U
;U V
} 
} ÿ
ED:\work\bikewaleweb\Bikewale.Interfaces\UserReviews\ISearchFilters.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
UserReviews )
.) *
Search* 0
{ 
public 

	interface 
IUserReviewsSearch '
{ 
SearchResult 
GetUserReviewsList '
(' (
InputFilters( 4
inputFilters5 A
)A B
;B C
SearchResult 
GetUserReviewsList '
(' (
InputFilters( 4
inputFilters5 A
,A B
uintC G
skipTopCountH T
)T U
;U V
} 
} ﬂ
CD:\work\bikewaleweb\Bikewale.Interfaces\UserReviews\IUserReviews.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
UserReviews )
{ 
public 

	interface 
IUserReviews !
{ 
Entities 
. 
UserReviews 
. 
UserReviewsData ,
GetUserReviewsData- ?
(? @
)@ A
;A B
UserReviewSummary  
GetUserReviewSummary .
(. /
uint/ 3
reviewId4 <
)< =
;= >
IEnumerable 
< 
UserReviewQuestion &
>& '"
GetUserReviewQuestions( >
(> ?"
UserReviewsInputEntity? U
inputParamsV a
)a b
;b c
IEnumerable 
< 
UserReviewQuestion &
>& '"
GetUserReviewQuestions( >
(> ?"
UserReviewsInputEntity? U
inputParamsV a
,a b
Entitiesc k
.k l
UserReviewsl w
.w x
UserReviewsData	x á$
objUserReviewQuestions
à û
)
û ü
;
ü †"
UserReviewRatingObject 
SaveUserRatings .
(. /!
InputRatingSaveEntity/ D
objInputRatingE S
)S T
;T U)
WriteReviewPageSubmitResponse %
SaveUserReviews& 5
(5 6
ReviewSubmitData6 F
objReviewDataG T
)T U
;U V
ReviewListBase 
GetUserReviews %
(% &
uint& *

startIndex+ 5
,5 6
uint7 ;
endIndex< D
,D E
uintF J
modelIdK R
,R S
uintT X
	versionIdY b
,b c
FilterByd l
filterm s
)s t
;t u 
UserReviewRatingData 
GetRateBikeData ,
(, -
RateBikeInput- :
objRateBike; F
)F G
;G H
} 
} î+
MD:\work\bikewaleweb\Bikewale.Interfaces\UserReviews\IUserReviewsRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
UserReviews )
{ 
public 

	interface "
IUserReviewsRepository +
{ 
List 
< "
ReviewTaggedBikeEntity #
># $$
GetMostReviewedBikesList% =
(= >
ushort> D
totalRecordsE Q
)Q R
;R S
List 
< "
ReviewTaggedBikeEntity #
># $ 
GetReviewedBikesList% 9
(9 :
): ;
;; <
List 
< 
ReviewsListEntity 
> 
GetMostReadReviews  2
(2 3
ushort3 9
totalRecords: F
)F G
;G H
List 
< 
ReviewsListEntity 
> !
GetMostHelpfulReviews  5
(5 6
ushort6 <
totalRecords= I
)I J
;J K
List 
< 
ReviewsListEntity 
>  
GetMostRecentReviews  4
(4 5
ushort5 ;
totalRecords< H
)H I
;I J
List 
< 
ReviewsListEntity 
> 
GetMostRatedReviews  3
(3 4
ushort4 :
totalRecords; G
)G H
;H I
ReviewRatingEntity 
GetBikeRatings )
() *
uint* .
modelId/ 6
)6 7
;7 8
ReviewListBase 
GetBikeReviewsList )
() *
uint* .

startIndex/ 9
,9 :
uint; ?
endIndex@ H
,H I
uintJ N
modelIdO V
,V W
uintX \
	versionId] f
,f g
FilterByh p
filterq w
)w x
;x y
ReviewDetailsEntity 
GetReviewDetails ,
(, -
uint- 1
reviewId2 :
): ;
;; <
bool 
AbuseReview 
( 
uint 
reviewId &
,& '
string( .
comment/ 6
,6 7
string8 >
userId? E
)E F
;F G
bool 
UpdateViews 
( 
uint 
reviewId &
)& '
;' (
bool 
UpdateReviewUseful 
(  
uint  $
reviewId% -
,- .
bool/ 3
	isHelpful4 =
)= >
;> ?
UserReviewsData 
GetUserReviewsData *
(* +
)+ ,
;, -
uint   !
SaveUserReviewRatings   "
(  " #!
InputRatingSaveEntity  # 8
inputSaveEntity  9 H
,  H I
uint  J N

customerId  O Y
,  Y Z
uint  [ _
reviewId  ` h
)  h i
;  i j
bool!! 
SaveUserReviews!! 
(!! 
uint!! !
reviewId!!" *
,!!* +
string!!, 2
tipsnAdvices!!3 ?
,!!? @
string!!A G
comment!!H O
,!!O P
string!!Q W
commentTitle!!X d
,!!d e
string!!f l
reviewsQuestionAns!!m 
,	!! Ä
uint
!!Å Ö
mileage
!!Ü ç
)
!!ç é
;
!!é è
UserReviewSummary""  
GetUserReviewSummary"" .
("". /
uint""/ 3
reviewId""4 <
)""< =
;""= >
bool## 
IsUserVerified## 
(## 
uint##  
reviewId##! )
,##) *
ulong##+ 0

customerId##1 ;
)##; <
;##< =
ReviewListBase$$ 
GetUserReviews$$ %
($$% &
)$$& '
;$$' (
UserReviewSummary%% *
GetUserReviewSummaryWithRating%% 8
(%%8 9
uint%%9 =
reviewId%%> F
)%%F G
;%%G H
BikeReviewsInfo&& 
GetBikeReviewsInfo&& *
(&&* +
uint&&+ /
modelId&&0 7
,&&7 8
uint&&9 =
?&&= >
skipReviewId&&? K
)&&K L
;&&L M"
BikeRatingsReviewsInfo'' %
GetBikeRatingsReviewsInfo'' 8
(''8 9
uint''9 =
modelId''> E
)''E F
;''F G
	Hashtable(( #
GetUserReviewsIdMapping(( )
((() *
)((* +
;((+ ,
IEnumerable** 
<** 
UserReviewSummary** %
>**% &$
GetUserReviewSummaryList**' ?
(**? @
string**@ F
reviewIdList**G S
)**S T
;**T U&
BikeReviewIdListByCategory,, "#
GetReviewsIdListByModel,,# :
(,,: ;
uint,,; ?
modelId,,@ G
),,G H
;,,H I'
QuestionsRatingValueByModel-- #*
GetReviewQuestionValuesByModel--$ B
(--B C
uint--C G
modelId--H O
)--O P
;--P Q
IEnumerable// 
<// 
RecentReviewsWidget// '
>//' (
GetRecentReviews//) 9
(//9 :
)//: ;
;//; <
IEnumerable00 
<00 
RecentReviewsWidget00 '
>00' (!
GetUserReviewsWinners00) >
(00> ?
)00? @
;00@ A
}11 
}22 ‡
HD:\work\bikewaleweb\Bikewale.Interfaces\UserReviews\IUserReviewsCache.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
UserReviews )
{ 
public 

	interface 
IUserReviewsCache &
{ 
ReviewListBase 
GetBikeReviewsList )
() *
uint* .

startIndex/ 9
,9 :
uint; ?
endIndex@ H
,H I
uintJ N
modelIdO V
,V W
uintX \
	versionId] f
,f g
FilterByh p
filterq w
)w x
;x y
UserReviewsData 
GetUserReviewsData *
(* +
)+ ,
;, -
ReviewListBase 
GetUserReviews %
(% &
)& '
;' (
UserReviewSummary *
GetUserReviewSummaryWithRating 8
(8 9
uint9 =
reviewId> F
)F G
;G H"
BikeRatingsReviewsInfo %
GetBikeRatingsReviewsInfo 8
(8 9
uint9 =
modelId> E
)E F
;F G
BikeReviewsInfo 
GetBikeReviewsInfo *
(* +
uint+ /
modelId0 7
)7 8
;8 9
	Hashtable #
GetUserReviewsIdMapping )
() *
)* +
;+ ,
IEnumerable 
< 
UserReviewSummary %
>% &$
GetUserReviewSummaryList' ?
(? @
IEnumerable@ K
<K L
uintL P
>P Q
reviewIdListR ^
)^ _
;_ `&
BikeReviewIdListByCategory "#
GetReviewsIdListByModel# :
(: ;
uint; ?
modelId@ G
)G H
;H I'
QuestionsRatingValueByModel #*
GetReviewQuestionValuesByModel$ B
(B C
uintC G
modelIdH O
)O P
;P Q
IEnumerable   
<   
RecentReviewsWidget   '
>  ' (
GetRecentReviews  ) 9
(  9 :
)  : ;
;  ; <
IEnumerable!! 
<!! 
RecentReviewsWidget!! '
>!!' (!
GetUserReviewsWinners!!) >
(!!> ?
)!!? @
;!!@ A
}"" 
}## ê
MD:\work\bikewaleweb\Bikewale.Interfaces\UsersTestimonial\IUsersTestimonial.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
UsersTestimonial .
{		 
public 

	interface 
IUsersTestimonial &
{ 
IEnumerable 
< 
Entities 
. 
UsersTestimonial -
.- .
UsersTestimonial. >
>> ?!
FetchUsersTestimonial@ U
(U V
uintV Z
topCount[ c
=d e
$numf g
)g h
;h i
} 
} Ç
BD:\work\bikewaleweb\Bikewale.Interfaces\Videos\IVideoRepository.cs
	namespace		 	
Bikewale		
 
.		 

Interfaces		 
.		 
Videos		 $
{

 
public 

	interface 
IVideoRepository %
{ 
ICollection 
<  
BikeVideoModelEntity (
>( )
GetModelVideos* 8
(8 9
uint9 =
makeId> D
)D E
;E F
} 
} Ô
9D:\work\bikewaleweb\Bikewale.Interfaces\Videos\IVideos.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Videos $
{ 
public 

	interface 
IVideos 
{ 
IEnumerable 
< 
BikeVideoEntity #
># $
GetVideosByCategory% 8
(8 9
EnumVideosCategory9 K

categoryIdL V
,V W
ushortX ^

totalCount_ i
)i j
;j k
IEnumerable 
< 
BikeVideoEntity #
># $ 
GetVideosByMakeModel% 9
(9 :
ushort: @
pageNoA G
,G H
ushortI O
pageSizeP X
,X Y
uintZ ^
makeId_ e
,e f
uintg k
?k l
modelIdm t
=u v
nullw {
){ |
;| }
IEnumerable 
< 
BikeVideoEntity #
># $ 
GetVideosByMakeModel% 9
(9 :
ushort: @
pageNoA G
,G H
ushortI O
pageSizeP X
,X Y
stringZ `
bodyStyleIda l
,l m
uintn r
makeIds y
,y z
uint{ 
?	 Ä
modelId
Å à
=
â ä
null
ã è
)
è ê
;
ê ë
IEnumerable 
< 
BikeVideoEntity #
># $
GetSimilarVideos% 5
(5 6
uint6 :
videoBasicId; G
,G H
ushortI O

totalCountP Z
)Z [
;[ \
BikeVideoEntity 
GetVideoDetails '
(' (
uint( ,
videoBasicId- 9
)9 :
;: ; 
BikeVideosListEntity "
GetVideosBySubCategory 3
(3 4
string4 :
categoryIdList; I
,I J
ushortK Q
pageNoR X
,X Y
ushortZ `
pageSizea i
,i j
VideosSortOrderk z
?z {
	sortOrder	| Ö
=
Ü á
null
à å
)
å ç
;
ç é
IEnumerable 
< 
BikeVideoEntity #
># $"
GetSimilarModelsVideos% ;
(; <
uint< @
videoIdA H
,H I
uintJ N
ModelIdO V
,V W
ushortX ^

totalCount_ i
)i j
;j k
IEnumerable 
< 
BikeVideoEntity #
># $
GetVideosByModelId% 7
(7 8
uint8 <
ModelId= D
)D E
;E F
} 
} Í
HD:\work\bikewaleweb\Bikewale.Interfaces\Videos\IVideosCacheRepository.cs
	namespace 	
Bikewale
 
. 

Interfaces 
. 
Videos $
{ 
public 

	interface "
IVideosCacheRepository +
{ 
IEnumerable 
< 
BikeVideoEntity #
># $
GetVideosByCategory% 8
(8 9
EnumVideosCategory9 K

categoryIdL V
,V W
ushortX ^

totalCount_ i
)i j
;j k
IEnumerable 
< 
BikeVideoEntity #
># $ 
GetVideosByMakeModel% 9
(9 :
ushort: @
pageNoA G
,G H
ushortI O
pageSizeP X
,X Y
uintZ ^
makeId_ e
,e f
uintg k
?k l
modelIdm t
=u v
nullw {
){ |
;| }
IEnumerable 
< 
BikeVideoEntity #
># $
GetSimilarVideos% 5
(5 6
uint6 :
videoBasicId; G
,G H
ushortI O

totalCountP Z
)Z [
;[ \
BikeVideoEntity 
GetVideoDetails '
(' (
uint( ,
videoBasicId- 9
)9 :
;: ; 
BikeVideosListEntity "
GetVideosBySubCategory 3
(3 4
string4 :
categoryIdList; I
,I J
ushortK Q
pageNoR X
,X Y
ushortZ `
pageSizea i
,i j
VideosSortOrderk z
?z {
	sortOrder	| Ö
=
Ü á
null
à å
)
å ç
;
ç é
IEnumerable 
<  
BikeVideoModelEntity (
>( )
GetModelVideos* 8
(8 9
uint9 =
makeId> D
)D E
;E F
} 
} 