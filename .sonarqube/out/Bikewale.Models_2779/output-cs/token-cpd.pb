»	
ED:\work\bikewaleweb\Bikewale.Models\BestBikes\BestBikeByCategoryVM.cs
	namespace 	
Bikewale
 
. 
Models 
{		 
public

 

class

  
BestBikeByCategoryVM

 %
{ 
public 
IEnumerable 
< 
BestBikeEntityBase -
>- .
objBestScootersList/ B
{C D
getE H
;H I
setJ M
;M N
}O P
public 
IEnumerable 
< 
BestBikeEntityBase -
>- .#
objBestMileageBikesList/ F
{G H
getI L
;L M
setN Q
;Q R
}S T
public 
IEnumerable 
< 
BestBikeEntityBase -
>- .!
objBestSportsBikeList/ D
{E F
getG J
;J K
setL O
;O P
}Q R
public 
IEnumerable 
< 
BestBikeEntityBase -
>- .#
objBestCruiserBikesList/ F
{G H
getI L
;L M
setN Q
;Q R
}S T
} 
} ˜
DD:\work\bikewaleweb\Bikewale.Models\BikeCare\BikeCareDetailPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

  
BikeCareDetailPageVM

 %
:

& '
	ModelBase

( 1
{ 
public 
ArticlePageDetails !
ArticleDetails" 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
BikeModelEntityBase "
Model# (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
PopularBodyStyleVM !
PopularBodyStyle" 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public #
MostPopularBikeWidgetVM &
MostPopularBikes' 7
{8 9
get: =
;= >
set? B
;B C
}D E
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public !
EditCMSPhotoGalleryVM $
PhotoGallery% 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
} 
} Â”
=D:\work\bikewaleweb\Bikewale.Models\BikeModels\ModelPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 

BikeModels $
{ 
public 

class 
ModelPageVM 
: 
	ModelBase (
{ 
public 
BikeModelPageEntity "
ModelPageEntity# 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
PQOnRoadPrice 

PriceQuote '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
BikeVersionMinSpecs "
SelectedVersion# 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
Bikewale 
. 
Entities  
.  !

PriceQuote! +
.+ ,
v2, .
.. /)
DetailedDealerQuotationEntity/ L
DetailedDealerM [
{\ ]
get^ a
;a b
setc f
;f g
}h i
public 
LeadCaptureEntity  
LeadCapture! ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
OtherBestBikesVM 
OtherBestBikes  .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public !
UpcomingBikesWidgetVM $
objUpcomingBikes% 5
{6 7
get8 ;
;; <
set= @
;@ A
}B C
public 
EMI 

EMIDetails 
{ 
get  #
;# $
set% (
;( )
}* +
public 
uint 
	VersionId 
{ 
get  #
;# $
set% (
;( )
}* +
public   
uint   
DealerId   
{   
get   "
;  " #
set  $ '
;  ' (
}  ) *
public!! 
uint!! 
PQId!! 
{!! 
get!! 
;!! 
set!!  #
;!!# $
}!!% &
public"" 
uint"" 
ModelId"" 
{"" 
get"" !
;""! "
set""# &
;""& '
}""( )
public## 
uint## 
CityId## 
{## 
get##  
;##  !
set##" %
;##% &
}##' (
public$$ 
uint$$ 
AreaId$$ 
{$$ 
get$$  
;$$  !
set$$" %
;$$% &
}$$' (
public%% 
uint%% 
	BikePrice%% 
{%% 
get%%  #
;%%# $
set%%% (
;%%( )
}%%* +
public&& 
string&& 
VersionName&& !
{&&" #
get&&$ '
;&&' (
set&&) ,
;&&, -
}&&. /
public'' 
CityEntityBase'' 
City'' "
{''# $
get''% (
;''( )
set''* -
;''- .
}''/ 0
public)) 
string)) 
BikeName)) 
{))  
get))! $
;))$ %
set))& )
;))) *
}))+ ,
public** 
bool** "
IsOnRoadPriceAvailable** *
{**+ ,
get**- 0
;**0 1
set**2 5
;**5 6
}**7 8
public++  
GlobalCityAreaEntity++ #
LocationCookie++$ 2
{++3 4
get++5 8
;++8 9
set++: =
;++= >
}++? @
public,, 
bool,, 
IsAreaSelected,, "
{,,# $
get,,% (
;,,( )
set,,* -
;,,- .
},,/ 0
public-- 
bool-- 
IsLocationSelected-- &
{--' (
get--) ,
{--- .
return--/ 5
(--6 7
this--7 ;
.--; <
City--< @
!=--A C
null--D H
&&--I K
this--L P
.--P Q
City--Q U
.--U V
CityId--V \
>--] ^
$num--_ `
)--` a
;--a b
}--c d
}--e f
public.. 
string.. 
Location.. 
{..  
get..! $
{..% &
return..' -
(... /
this../ 3
...3 4
IsAreaSelected..4 B
?..C D
(..E F
string..F L
...L M
IsNullOrEmpty..M Z
(..Z [
LocationCookie..[ i
...i j
Area..j n
)..n o
?..p q
LocationCookie	..r Ä
.
..Ä Å
City
..Å Ö
:
..Ü á
string
..à é
.
..é è
Format
..è ï
(
..ï ñ
$str
..ñ †
,
..† °
LocationCookie
..¢ ∞
.
..∞ ±
Area
..± µ
,
..µ ∂
LocationCookie
..∑ ≈
.
..≈ ∆
City
..∆  
)
..  À
)
..À Ã
:
..Õ Œ
(
..œ –
this
..– ‘
.
..‘ ’ 
IsLocationSelected
..’ Á
?
..Ë È
LocationCookie
..Í ¯
.
..¯ ˘
City
..˘ ˝
:
..˛ ˇ
$str
..Ä à
)
..à â
)
..â ä
;
..ä ã
}
..å ç
}
..é è
public// 
string// 
LeadBtnLongText// %
{//& '
get//( +
{//, -
return//. 4
$str//5 M
;//M N
}//O P
}//Q R
public00 
string00 
LeadBtnShortText00 &
{00' (
get00) ,
{00- .
return00/ 5
$str006 B
;00B C
}00D E
}00F G
public22 
bool22 
IsModelDetails22 "
{22# $
get22% (
{22) *
return22+ 1
(222 3
this223 7
.227 8
ModelPageEntity228 G
!=22H J
null22K O
&&22P R
this22S W
.22W X
ModelPageEntity22X g
.22g h
ModelDetails22h t
!=22u w
null22x |
)22| }
;22} ~
}	22 Ä
}
22Å Ç
public44 
bool44 
	IsNewBike44 
{44 
get44  #
{44$ %
return44& ,
(44- .
IsModelDetails44. <
&&44= ?
this44@ D
.44D E
ModelPageEntity44E T
.44T U
ModelDetails44U a
.44a b
New44b e
)44e f
;44f g
}44h i
}44j k
public55 
bool55 
IsUpcomingBike55 "
{55# $
get55% (
{55) *
return55+ 1
(552 3
IsModelDetails553 A
&&55B D
this55E I
.55I J
ModelPageEntity55J Y
.55Y Z
ModelDetails55Z f
.55f g

Futuristic55g q
)55q r
;55r s
}55t u
}55v w
public66 
bool66 
IsDiscontinuedBike66 &
{66' (
get66) ,
{66- .
return66/ 5
(666 7
IsModelDetails667 E
&&66F H
!66I J
	IsNewBike66J S
&&66T V
!66W X
IsUpcomingBike66X f
)66f g
;66g h
}66i j
}66k l
public77 
bool77 
IsDPQAvailable77 "
{77# $
get77% (
;77( )
set77* -
;77- .
}77/ 0
public88 
bool88 
IsBPQAvailable88 "
{88# $
get88% (
;88( )
set88* -
;88- .
}88/ 0
public99 
bool99 

IsGstPrice99 
{99  
get99! $
;99$ %
set99& )
;99) *
}99+ ,
public:: 
bool:: 
IsPrimaryDealer:: #
{::$ %
get::& )
{::* +
return::, 2
(::3 4
this::4 8
.::8 9
DetailedDealer::9 G
!=::H J
null::K O
&&::P R
this::S W
.::W X
DetailedDealer::X f
.::f g
PrimaryDealer::g t
!=::u w
null::x |
)::| }
;::} ~
}	:: Ä
}
::Å Ç
public;; 
bool;; !
IsDealerDetailsExists;; )
{;;* +
get;;, /
{;;0 1
return;;2 8
(;;9 :
this;;: >
.;;> ?
IsPrimaryDealer;;? N
&&;;O Q
this;;R V
.;;V W
DetailedDealer;;W e
.;;e f
PrimaryDealer;;f s
.;;s t
DealerDetails	;;t Å
!=
;;Ç Ñ
null
;;Ö â
)
;;â ä
;
;;ä ã
}
;;å ç
}
;;é è
public<< 
bool<< 
IsPremiumDealer<< #
{<<$ %
get<<& )
{<<* +
return<<, 2
(<<3 4
IsPrimaryDealer<<4 C
&&<<D F
this<<G K
.<<K L
DetailedDealer<<L Z
.<<Z [
PrimaryDealer<<[ h
.<<h i
IsPremiumDealer<<i x
)<<x y
;<<y z
}<<{ |
}<<} ~
public== 
NewBikeDealers== 
DealerDetails== +
{==, -
get==. 1
{==2 3
return==4 :
this==; ?
.==? @
DetailedDealer==@ N
.==N O
PrimaryDealer==O \
.==\ ]
DealerDetails==] j
;==j k
}==l m
}==n o
public>> 
string>> 
	MPQString>> 
{>>  !
get>>" %
;>>% &
set>>' *
;>>* +
}>>, -
public?? 
string?? 

DealerArea??  
{??! "
get??# &
{??' (
return??) /
(??0 1!
IsDealerDetailsExists??1 F
&&??G I
DealerDetails??J W
.??W X
objArea??X _
!=??` b
null??c g
???h i
DealerDetails??j w
.??w x
objArea??x 
.	?? Ä
AreaName
??Ä à
:
??â ä
LocationCookie
??ã ô
.
??ô ö
Area
??ö û
)
??û ü
;
??ü †
}
??° ¢
}
??£ §
public@@ 
string@@ 
BestBikeHeading@@ %
{@@& '
get@@( +
;@@+ ,
set@@- 0
;@@0 1
}@@2 3
publicAA 
stringAA 
ColourImageUrlAA $
{AA% &
getAA' *
;AA* +
setAA, /
;AA/ 0
}AA1 2
publicCC 
stringCC 
ClientIPCC 
{CC  
getCC! $
;CC$ %
setCC& )
;CC) *
}CC+ ,
publicDD 
stringDD 
PageUrlDD 
{DD 
getDD  #
;DD# $
setDD% (
;DD( )
}DD* +
publicEE 
intEE 
PQSourcePageEE 
{EE  !
getEE" %
{EE& '
returnEE( .
(EE/ 0
intEE0 3
)EE3 4
PQSourceEnumEE4 @
.EE@ A
Desktop_ModelPageEEA R
;EER S
}EET U
}EEV W
publicFF 
intFF 
PQLeadSourceFF 
{FF  !
getFF" %
{FF& '
returnFF( .
$numFF/ 1
;FF1 2
}FF3 4
}FF5 6
publicGG 
stringGG #
VersionPriceListSummaryGG -
{GG. /
getGG0 3
;GG3 4
setGG5 8
;GG8 9
}GG: ;
publicJJ 
RecentNewsVMJJ 
NewsJJ  
{JJ! "
getJJ# &
;JJ& '
setJJ( +
;JJ+ ,
}JJ- .
publicKK !
RecentExpertReviewsVMKK $
ExpertReviewsKK% 2
{KK3 4
getKK5 8
;KK8 9
setKK: =
;KK= >
}KK? @
publicLL 
RecentVideosVMLL 
VideosLL $
{LL% &
getLL' *
;LL* +
setLL, /
;LL/ 0
}LL1 2
publicMM  
SimilarBikesWidgetVMMM #
SimilarBikesMM$ 0
{MM1 2
getMM3 6
;MM6 7
setMM8 ;
;MM; <
}MM= >
publicNN 
DealerCardVMNN 
OtherDealersNN (
{NN) *
getNN+ .
;NN. /
setNN0 3
;NN3 4
}NN5 6
publicOO 
ServiceCentersOO 
.OO (
ServiceCenterDetailsWidgetVMOO :
ServiceCentersOO; I
{OOJ K
getOOL O
;OOO P
setOOQ T
;OOT U
}OOV W
publicPP .
"DealersServiceCentersIndiaWidgetVMPP 1 
DealersServiceCenterPP2 F
{PPG H
getPPI L
;PPL M
setPPN Q
;PPQ R
}PPS T
publicQQ 
PriceInCityQQ 
.QQ $
PriceInTopCitiesWidgetVMQQ 3
PriceInTopCitiesQQ4 D
{QQE F
getQQG J
;QQJ K
setQQL O
;QQO P
}QQQ R
publicRR 
SystemRR 
.RR 
CollectionsRR !
.RR! "
GenericRR" )
.RR) *
ICollectionRR* 5
<RR5 6$
SimilarCompareBikeEntityRR6 N
>RRN O
PopularComparisionsRRP c
{RRd e
getRRf i
;RRi j
setRRk n
;RRn o
}RRp q
publicSS !
UsedBikeByModelCityVMSS $

UsedModelsSS% /
{SS0 1
getSS2 5
;SS5 6
setSS7 :
;SS: ;
}SS< =
publicTT 
UserReviewsSearchVMTT "
UserReviewsTT# .
{TT/ 0
getTT1 4
;TT4 5
setTT6 9
;TT9 :
}TT; <
publicVV 
boolVV #
AreModelPhotosAvailableVV +
{VV, -
getVV. 1
{VV2 3
returnVV4 :
(VV; <
thisVV< @
.VV@ A
ModelPageEntityVVA P
!=VVQ S
nullVVT X
&&VVY [
ModelPageEntityVV\ k
.VVk l
	AllPhotosVVl u
!=VVv x
nullVVy }
&&	VV~ Ä
this
VVÅ Ö
.
VVÖ Ü
ModelPageEntity
VVÜ ï
.
VVï ñ
	AllPhotos
VVñ ü
.
VVü †
Count
VV† •
(
VV• ¶
)
VV¶ ß
>
VV® ©
$num
VV™ ´
)
VV´ ¨
;
VV¨ ≠
}
VVÆ Ø
}
VV∞ ±
publicWW 
boolWW 
IsNewsAvailableWW #
{WW$ %
getWW& )
{WW* +
returnWW, 2
(WW3 4
NewsWW4 8
!=WW9 ;
nullWW< @
&&WWA C
NewsWWD H
.WWH I
FetchedCountWWI U
>WWV W
$numWWX Y
)WWY Z
;WWZ [
}WW\ ]
}WW^ _
publicXX 
boolXX 
IsReviewsAvailableXX &
{XX' (
getXX) ,
{XX- .
returnXX/ 5
(XX6 7
ExpertReviewsXX7 D
!=XXE G
nullXXH L
&&XXM O
ExpertReviewsXXP ]
.XX] ^
FetchedCountXX^ j
>XXk l
$numXXm n
)XXn o
;XXo p
}XXq r
}XXs t
publicYY 
boolYY 
IsVideosAvailableYY %
{YY& '
getYY( +
{YY, -
returnYY. 4
(YY5 6
VideosYY6 <
!=YY= ?
nullYY@ D
&&YYE G
VideosYYH N
.YYN O

VideosListYYO Y
!=YYZ \
nullYY] a
&&YYb d
VideosYYe k
.YYk l
FetchedCountYYl x
>YYy z
$numYY{ |
&&YY} 
Videos
YYÄ Ü
.
YYÜ á

VideosList
YYá ë
.
YYë í
Count
YYí ó
(
YYó ò
)
YYò ô
>
YYö õ
$num
YYú ù
)
YYù û
;
YYû ü
}
YY† °
}
YY¢ £
publicZZ 
boolZZ #
IsSimilarBikesAvailableZZ +
{ZZ, -
getZZ. 1
{ZZ2 3
returnZZ4 :
(ZZ; <
SimilarBikesZZ< H
!=ZZI K
nullZZL P
&&ZZQ S
SimilarBikesZZT `
.ZZ` a
BikesZZa f
!=ZZg i
nullZZj n
&&ZZo q
SimilarBikesZZr ~
.ZZ~ 
Bikes	ZZ Ñ
.
ZZÑ Ö
Count
ZZÖ ä
(
ZZä ã
)
ZZã å
>
ZZç é
$num
ZZè ê
)
ZZê ë
;
ZZë í
}
ZZì î
}
ZZï ñ
public[[ 
bool[[ #
IsOtherDealersAvailable[[ +
{[[, -
get[[. 1
{[[2 3
return[[4 :
([[; <
OtherDealers[[< H
!=[[I K
null[[L P
&&[[Q S
OtherDealers[[T `
.[[` a
Dealers[[a h
!=[[i k
null[[l p
&&[[q s
OtherDealers	[[t Ä
.
[[Ä Å
Dealers
[[Å à
.
[[à â
Count
[[â é
(
[[é è
)
[[è ê
>
[[ë í
$num
[[ì î
)
[[î ï
;
[[ï ñ
}
[[ó ò
}
[[ô ö
public\\ 
bool\\ %
IsServiceCentersAvailable\\ -
{\\. /
get\\0 3
{\\4 5
return\\6 <
(\\= >
ServiceCenters\\> L
!=\\M O
null\\P T
&&\\U W
ServiceCenters\\X f
.\\f g
ServiceCentersList\\g y
!=\\z |
null	\\} Å
&&
\\Ç Ñ
ServiceCenters
\\Ö ì
.
\\ì î 
ServiceCentersList
\\î ¶
.
\\¶ ß
Count
\\ß ¨
(
\\¨ ≠
)
\\≠ Æ
>
\\Ø ∞
$num
\\± ≤
)
\\≤ ≥
;
\\≥ ¥
}
\\µ ∂
}
\\∑ ∏
public]] 
bool]] *
IsPopularComparisionsAvailable]] 2
{]]3 4
get]]5 8
{]]9 :
return]]; A
(]]B C
PopularComparisions]]C V
!=]]W Y
null]]Z ^
&&]]_ a
PopularComparisions]]b u
.]]u v
Count]]v {
(]]{ |
)]]| }
>]]~ 
$num
]]Ä Å
)
]]Å Ç
;
]]Ç É
}
]]Ñ Ö
}
]]Ü á
public^^ 
bool^^ '
IsPriceInTopCitiesAvailable^^ /
{^^0 1
get^^2 5
{^^6 7
return^^8 >
(^^? @
PriceInTopCities^^@ P
!=^^Q S
null^^T X
&&^^Y [
PriceInTopCities^^\ l
.^^l m
PriceQuoteList^^m {
!=^^| ~
null	^^ É
&&
^^Ñ Ü
PriceInTopCities
^^á ó
.
^^ó ò
PriceQuoteList
^^ò ¶
.
^^¶ ß
Count
^^ß ¨
(
^^¨ ≠
)
^^≠ Æ
>
^^Ø ∞
$num
^^± ≤
)
^^≤ ≥
;
^^≥ ¥
}
^^µ ∂
}
^^∑ ∏
public__ 
bool__ +
IsDealersServiceCenterAvailable__ 3
{__4 5
get__6 9
{__: ;
return__< B
(__C D 
DealersServiceCenter__D X
!=__Y [
null__\ `
&&__a c 
DealersServiceCenter__d x
.__x y!
DealerServiceCenters	__y ç
!=
__é ê
null
__ë ï
&&
__ñ ò
(
__ô ö"
DealersServiceCenter
__ö Æ
.
__Æ Ø"
DealerServiceCenters
__Ø √
.
__√ ƒ
TotalDealerCount
__ƒ ‘
>
__’ ÷
$num
__◊ ÿ
||
__Ÿ €"
DealersServiceCenter
__‹ 
.
__ Ò"
DealerServiceCenters
__Ò Ö
.
__Ö Ü%
TotalServiceCenterCount
__Ü ù
>
__û ü
$num
__† °
)
__° ¢
)
__¢ £
;
__£ §
}
__• ¶
}
__ß ®
public`` 
bool`` #
IsVersionSpecsAvailable`` +
{``, -
get``. 1
{``2 3
return``4 :
(``; <
ModelPageEntity``< K
!=``L N
null``O S
&&``T V
ModelPageEntity``W f
.``f g
ModelVersionSpecs``g x
!=``y {
null	``| Ä
)
``Ä Å
;
``Å Ç
}
``É Ñ
}
``Ö Ü
publicaa 
boolaa '
IsModelDescriptionAvailableaa /
{aa0 1
getaa2 5
{aa6 7
returnaa8 >
(aa? @
thisaa@ D
.aaD E#
IsVersionSpecsAvailableaaE \
||aa] _
(aa` a
thisaaa e
.aae f
ModelPageEntityaaf u
.aau v
	ModelDescaav 
!=
aaÄ Ç
null
aaÉ á
&&
aaà ä
!
aaã å
string
aaå í
.
aaí ì
IsNullOrEmpty
aaì †
(
aa† °
this
aa° •
.
aa• ¶
ModelPageEntity
aa¶ µ
.
aaµ ∂
	ModelDesc
aa∂ ø
.
aaø ¿
SmallDescription
aa¿ –
)
aa– —
)
aa— “
)
aa“ ”
;
aa” ‘
}
aa’ ÷
}
aa◊ ÿ
publicbb 
boolbb "
IsModelColorsAvailablebb *
{bb+ ,
getbb- 0
{bb1 2
returnbb3 9
(bb: ;
thisbb; ?
.bb? @
ModelPageEntitybb@ O
!=bbP R
nullbbS W
&&bbX Z
thisbb[ _
.bb_ `
ModelPageEntitybb` o
.bbo p
ModelColorsbbp {
!=bb| ~
null	bb É
&&
bbÑ Ü
this
bbá ã
.
bbã å
ModelPageEntity
bbå õ
.
bbõ ú
ModelColors
bbú ß
.
bbß ®
Count
bb® ≠
(
bb≠ Æ
)
bbÆ Ø
>
bb∞ ±
$num
bb≤ ≥
)
bb≥ ¥
;
bb¥ µ
}
bb∂ ∑
}
bb∏ π
publiccc 
boolcc  
IsUsedBikesAvailablecc (
{cc) *
getcc+ .
{cc/ 0
returncc1 7
(cc8 9

UsedModelscc9 C
!=ccD F
nullccG K
&&ccL N

UsedModelsccO Y
.ccY Z
RecentUsedBikesListccZ m
!=ccn p
nullccq u
&&ccv x

UsedModels	ccy É
.
ccÉ Ñ!
RecentUsedBikesList
ccÑ ó
.
ccó ò
Count
ccò ù
(
ccù û
)
ccû ü
>
cc† °
$num
cc¢ £
)
cc£ §
;
cc§ •
}
cc¶ ß
}
cc® ©
publicdd 
booldd 
IsShowPriceTabdd "
{dd# $
getdd% (
;dd( )
setdd* -
;dd- .
}dd/ 0
publicee 
boolee "
IsUserReviewsAvailableee *
{ee+ ,
getee- 0
{ee1 2
returnee3 9
UserReviewsee: E
!=eeF H
nulleeI M
&&eeN P
UserReviewseeQ \
.ee\ ]
UserReviewsee] h
!=eei k
nulleel p
&&eeq s
UserReviewseet 
.	ee Ä
ReviewsInfo
eeÄ ã
!=
eeå é
null
eeè ì
&&
eeî ñ
UserReviews
eeó ¢
.
ee¢ £
ReviewsInfo
ee£ Æ
.
eeÆ Ø
TotalReviews
eeØ ª
>
eeº Ω
$num
eeæ ø
;
eeø ¿
}
ee¡ ¬
}
ee√ ƒ
publicff 
IEnumerableff 
<ff  
ColorImageBaseEntityff /
>ff/ 0
ColorImagesff1 <
{ff= >
getff? B
{ffC D
returnffE K#
AreModelPhotosAvailableffL c
?ffd e
ModelPageEntityfff u
.ffu v
	AllPhotosffv 
.	ff Ä
Where
ffÄ Ö
(
ffÖ Ü
x
ffÜ á
=>
ffà ä
x
ffã å
.
ffå ç
ColorId
ffç î
>
ffï ñ
$num
ffó ò
)
ffò ô
:
ffö õ
null
ffú †
;
ff† °
}
ff¢ £
}
ff§ •
privatejj 
StringBuilderjj 
colorStrjj &
=jj' (
newjj) ,
StringBuilderjj- :
(jj: ;
)jj; <
;jj< =
publickk '
BikeRankingPropertiesEntitykk *
BikeRankingkk+ 6
{kk7 8
getkk9 <
;kk< =
setkk> A
;kkA B
}kkC D
publicll 
stringll 
ModelSummaryll "
{ll# $
getll% (
;ll( )
setll* -
;ll- .
}ll/ 0
publicmm 
uintmm 

CampaignIdmm 
{mm  
getmm! $
;mm$ %
setmm& )
;mm) *
}mm+ ,
publicoo 
intoo !
ModelColorPhotosCountoo (
{oo) *
getoo+ .
;oo. /
setoo0 3
;oo3 4
}oo5 6
publicqq 
boolqq 
ShowOnRoadButtonqq $
{qq% &
getqq' *
;qq* +
setqq, /
;qq/ 0
}qq1 2
publicrr 
stringrr 
	ReturnUrlrr 
{rr  !
getrr" %
;rr% &
setrr' *
;rr* +
}rr, -
publicss 
boolss 
HasCityPricingss "
{ss# $
getss% (
;ss( )
setss* -
;ss- .
}ss/ 0
publictt )
ManufactureCampaignLeadEntitytt ,
LeadCampaigntt- 9
{tt: ;
gettt< ?
;tt? @
setttA D
;ttD E
}ttF G
publicuu 
booluu %
IsManufacturerLeadAdShownuu -
{uu. /
getuu0 3
;uu3 4
setuu5 8
;uu8 9
}uu: ;
publicvv 
boolvv (
IsManufacturerTopLeadAdShownvv 0
{vv1 2
getvv3 6
;vv6 7
setvv8 ;
;vv; <
}vv= >
publicww (
ManufactureCampaignEMIEntityww +
EMICampaignww, 7
{ww8 9
getww: =
;ww= >
setww? B
;wwB C
}wwD E
publicxx 
boolxx $
IsManufacturerEMIAdShownxx ,
{xx- .
getxx/ 2
;xx2 3
setxx4 7
;xx7 8
}xx9 :
publiczz 
EnumBikeBodyStyleszz !
	BodyStylezz" +
{zz, -
getzz. 1
;zz1 2
setzz3 6
;zz6 7
}zz8 9
public|| 
string|| 
BodyStyleText|| #
{||$ %
get||& )
;||) *
set||+ .
;||. /
}||0 1
public~~ 
string~~ !
BodyStyleTextSingular~~ +
{~~, -
get~~. 1
;~~1 2
set~~3 6
;~~6 7
}~~8 9
public 
PopularBodyStyleVM !
PopularBodyStyle" 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public
ÄÄ 
bool
ÄÄ )
IsPopularBodyStyleAvailable
ÄÄ /
{
ÄÄ0 1
get
ÄÄ2 5
{
ÄÄ6 7
return
ÄÄ8 >
(
ÄÄ? @
PopularBodyStyle
ÄÄ@ P
!=
ÄÄQ S
null
ÄÄT X
&&
ÄÄY [
PopularBodyStyle
ÄÄ\ l
.
ÄÄl m
PopularBikes
ÄÄm y
!=
ÄÄz |
nullÄÄ} Å
&&ÄÄÇ Ñ 
PopularBodyStyleÄÄÖ ï
.ÄÄï ñ
PopularBikesÄÄñ ¢
.ÄÄ¢ £
CountÄÄ£ ®
(ÄÄ® ©
)ÄÄ© ™
>ÄÄ´ ¨
$numÄÄ≠ Æ
)ÄÄÆ Ø
;ÄÄØ ∞
}ÄÄ± ≤
}ÄÄ≥ ¥
}
ÅÅ 
}ÉÉ ñ

CD:\work\bikewaleweb\Bikewale.Models\BikeCare\BikeCareIndexPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public		 

class		 
BikeCareIndexPageVM		 $
:		% &
	ModelBase		' 0
{

 
public 

CMSContent 
Articles "
{# $
get% (
;( )
set* -
;- .
}/ 0
public #
MostPopularBikeWidgetVM &
MostPopularBikes' 7
{8 9
get: =
;= >
set? B
;B C
}D E
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
int 

StartIndex 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
EndIndex 
{ 
get !
;! "
set# &
;& '
}( )
public 
PagerEntity 
PagerEntity &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} Ê
BD:\work\bikewaleweb\Bikewale.Models\BikeModels\OtherBestBikesVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 

BikeModels $
{ 
public

 

class

 
OtherBestBikesVM

 !
{ 
public 
bool !
IsMakePresentInConfig )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
string !
OtherBestBikesHeading +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
IEnumerable 
< 
BestBikeEntityBase -
>- .
	BestBikes/ 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
} 
} Ã
HD:\work\bikewaleweb\Bikewale.Models\CompareBikes\PopularComparisonsVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class  
PopularComparisonsVM %
{ 
public 
IEnumerable 
< $
SimilarCompareBikeEntity 3
>3 4
CompareBikes5 A
{B C
getD G
;G H
setI L
;L M
}N O
public 
CompareSources 
CompareSource +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
bool 
IsDataAvailable #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} 
} Æ
?D:\work\bikewaleweb\Bikewale.Models\Compare\CompareDetailsVM.cs
	namespace 	
Bikewale
 
. 
Models 
{		 
public 

class 
CompareDetailsVM !
:" #
	ModelBase$ -
{ 
public 
string 
KnowMoreLinkUrl %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
BikeCompareEntity  
Compare! (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
bool 
isSponsoredBike #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
Int64 
sponsoredVersionId '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
string 
comparisionText %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
bool 
isUsedBikePresent %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
targetModels "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
compareSummaryText (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
IEnumerable 
< $
SimilarCompareBikeEntity 3
>3 4
topBikeCompares5 D
{E F
getG J
;J K
setL O
;O P
}Q R
public 
string  
templateSummaryTitle *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
IEnumerable 
< 
ArticleSummary )
>) *
ArticlesList+ 7
{8 9
get: =
;= >
set? B
;B C
}D E
public 
PQSourceEnum 

PQSourceId &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
Bikewale 
. 

Comparison "
." #
Entities# +
.+ ,&
SponsoredVersionEntityBase, F
SponsoredBikeG T
{U V
getW Z
;Z [
set\ _
;_ `
}a b
public 
string 
KnowMoreLinkText &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} À
8D:\work\bikewaleweb\Bikewale.Models\Compare\CompareVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
	CompareVM 
: 
	ModelBase &
{ 
public 
ComparisonWidgetVM !
CompareBikes" .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
IEnumerable 
< 
ArticleSummary )
>) *
ArticlesList+ 7
{8 9
get: =
;= >
set? B
;B C
}D E
public 
string 
CityName 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} ÷	
AD:\work\bikewaleweb\Bikewale.Models\Compare\ComparisonWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 
ComparisonWidgetVM

 #
{ 
public 
IEnumerable 
< $
SimilarCompareBikeEntity 3
>3 4
topBikeCompares5 D
{E F
getG J
;J K
setL O
;O P
}Q R
public 
IEnumerable 
< $
SimilarCompareBikeEntity 3
>3 4#
topBikeComparesScooters5 L
{M N
getO R
;R S
setT W
;W X
}Y Z
public 
IEnumerable 
< $
SimilarCompareBikeEntity 3
>3 4#
topBikeComparesCruisers5 L
{M N
getO R
;R S
setT W
;W X
}Y Z
public 
IEnumerable 
< $
SimilarCompareBikeEntity 3
>3 4!
topBikeComparesSports5 J
{K L
getM P
;P Q
setR U
;U V
}W X
} 
} ﬁ
PD:\work\bikewaleweb\Bikewale.Models\ComparisonTests\ComparisonTestIndexPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class &
ComparisonTestsIndexPageVM +
:, -
	ModelBase. 7
{ 
public 

CMSContent 
Articles "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
int 

StartIndex 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
EndIndex 
{ 
get !
;! "
set# &
;& '
}( )
public 
PagerEntity 
PagerEntity &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
PageH1 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
PageH2 
{ 
get "
;" #
set$ '
;' (
}) *
public #
MostPopularBikeWidgetVM &
MostPopularBikes' 7
{8 9
get: =
;= >
set? B
;B C
}D E
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
} 
} ∂

BD:\work\bikewaleweb\Bikewale.Models\DealerShowroom\DealerCardVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
[ 
Serializable 
] 
public 

class 
DealerCardVM 
{ 
public 
IEnumerable 
< 
DealersList &
>& '
Dealers( /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
public 
UInt16 

TotalCount  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
CityName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
CityMaskingName %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
MakeMaskingName %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
} 
} ≤
XD:\work\bikewaleweb\Bikewale.Models\DealerShowroom\DealersServiceCentersIndiaWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public		 

class		 .
"DealersServiceCentersIndiaWidgetVM		 3
{

 
public &
PopularDealerServiceCenter ) 
DealerServiceCenters* >
{? @
getA D
;D E
setF I
;I J
}K L
public 
string 
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
MakeMaskingName %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
} 
} ≥	
=D:\work\bikewaleweb\Bikewale.Models\DealerShowroom\IndexVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
IndexVM 
: 
	ModelBase $
{ 
public		 
NewLaunchedWidgetVM		 "
NewLaunchedBikes		# 3
{		4 5
get		6 9
;		9 :
set		; >
;		> ?
}		@ A
public

 !
UpcomingBikesWidgetVM

 $
objUpcomingBikes

% 5
{

6 7
get

8 ;
;

; <
set

= @
;

@ A
}

B C
public 
BestBikeWidgetVM 
	BestBikes  )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
	MakesList/ 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public 
BrandWidgetVM 
Brands #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} 
} ﬂ
OD:\work\bikewaleweb\Bikewale.Models\ExpertReviews\ScooterExpertReviewsPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class &
ScooterExpertReviewsPageVM +
:+ ,
	ModelBase- 6
{ 
public 

CMSContent 
Articles "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
int 

StartIndex 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
EndIndex 
{ 
get !
;! "
set# &
;& '
}( )
public 
PagerEntity 
PagerEntity &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
PageH1 
{ 
get "
;" #
set$ '
;' (
}) *
public #
MostPopularBikeWidgetVM &
MostPopularScooters' :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .%
PopularScooterMakesWidget/ H
{I J
getK N
;N O
setP S
;S T
}U V
public 
string -
!PopularScooterBrandsWidgetHeading 7
{8 9
get: =
;= >
set? B
;B C
}D E
} 
} Ô
?D:\work\bikewaleweb\Bikewale.Models\Features\DetailFeatureVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
DetailFeatureVM  
:! "
	ModelBase# ,
{ 
public 
ArticlePageDetails !

objFeature" ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public		 
BikeMakeEntityBase		 !
objMake		" )
{		* +
get		, /
;		/ 0
set		1 4
;		4 5
}		6 7
public

 
BikeModelEntityBase

 "
objModel

# +
{

, -
get

. 1
;

1 2
set

3 6
;

6 7
}

8 9
public 
PopularBodyStyleVM !
PopularBodyStyle" 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public #
MostPopularBikeWidgetVM &
MostPopularBikes' 7
{8 9
get: =
;= >
set? B
;B C
}D E
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public !
EditCMSPhotoGalleryVM $
PhotoGallery% 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
} 
} §
>D:\work\bikewaleweb\Bikewale.Models\Features\IndexFeatureVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
IndexFeatureVM 
:  !
	ModelBase" +
{ 
public		 
IList		 
<		 
ArticleSummary		 #
>		# $
ArticlesList		% 1
{		2 3
get		4 7
;		7 8
set		9 <
;		< =
}		> ?
public

 
PagerEntity

 
PagerEntity

 &
{

' (
get

) ,
;

, -
set

. 1
;

1 2
}

3 4
public 
string 
prevPageUrl !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
nextPageUrl !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
uint 

StartIndex 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
uint 
EndIndex 
{ 
get "
;" #
set$ '
;' (
}) *
public 
uint 
TotalArticles !
{" #
get$ '
;' (
set) ,
;, -
}. /
public #
MostPopularBikeWidgetVM &
MostPopularBikes' 7
{8 9
get: =
;= >
set? B
;B C
}D E
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
} 
} ª
ND:\work\bikewaleweb\Bikewale.Models\ExpertReviews\ExpertReviewsDetailPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class %
ExpertReviewsDetailPageVM *
:+ ,
	ModelBase- 6
{ 
public 
ArticlePageDetails !
ArticleDetails" 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public !
EditCMSPhotoGalleryVM $
PhotoGallery% 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
public 
PopularBodyStyleVM !
PopularBodyStyle" 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public #
MostPopularBikeWidgetVM &
MostPopularBikes' 7
{8 9
get: =
;= >
set? B
;B C
}D E
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 

BikeInfoVM 
BikeInfo "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
BikeModelEntityBase "
Model# (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
StringBuilder 

BikeTested '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .%
PopularScooterMakesWidget/ H
{I J
getK N
;N O
setP S
;S T
}U V
} 
} ß
HD:\work\bikewaleweb\Bikewale.Models\GenericBestBikes\IndexBestBikesVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 
IndexBestBikesVM

 !
:

" #
	ModelBase

$ -
{ 
public 
IEnumerable 
< 
BestBikeEntityBase ,
>, -
objBestBikesList. >
{? @
getA D
;D E
setF I
;I J
}K L
public 
string 
PageMaskingName %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
PageName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
Content 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
bannerImagePos $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
PQSourceEnum 
pqSource $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
string 
bannerImage !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
BestBikeWidgetVM 
	BestBikes  )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
BrandWidgetVM 
Brands #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} 
} ˚
<D:\work\bikewaleweb\Bikewale.Models\Make\BrandCityPopupVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
BrandCityPopupVM !
{		 
public

 
EnumBikeType

 
PageType

 $
{

% &
get

' *
;

* +
set

, /
;

/ 0
}

1 2
public 
bool 
IsOperaBrowser "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
MakeId 
{ 
get  
;  !
set" %
;% &
}' (
} 
} „
GD:\work\bikewaleweb\Bikewale.Models\Make\DealerServiceCenterWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class '
DealerServiceCenterWidgetVM ,
{ 
public 
string 
Title 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
MakeMaskingName %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
ICollection 
< #
PopularCityDealerEntity 2
>2 3
DealerDetails4 A
{B C
getD G
;G H
setI L
;L M
}N O
public (
ServiceCenterDetailsWidgetVM +
ServiceCenters, :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
uint 
TotalDealerCount $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
uint #
TotalServiceCenterCount +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
} 
} á
<D:\work\bikewaleweb\Bikewale.Models\Make\MakeModelPopupVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Make 
{ 
public

 

class

 
UserReviewPopupVM

 "
{ 
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
Makes/ 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
} 
} Ø+
6D:\work\bikewaleweb\Bikewale.Models\Make\MakePageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 

MakePageVM 
: 
	ModelBase '
{ 
public 
string 
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
MakeMaskingName %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
LocationMasking %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
Location 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
DealerServiceTitle (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
IEnumerable 
<  
MostPopularBikesBase /
>/ 0
Bikes1 6
{7 8
get9 <
;< =
set> A
;A B
}C D
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public  
PopularComparisonsVM #
CompareSimilarBikes$ 7
{8 9
get: =
;= >
set? B
;B C
}D E
public !
BikeDescriptionEntity $
BikeDescription% 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public "
UsedBikeModelsWidgetVM %

UsedModels& 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public   
RecentNewsVM   
News    
{  ! "
get  # &
;  & '
set  ( +
;  + ,
}  - .
public!! !
RecentExpertReviewsVM!! $
ExpertReviews!!% 2
{!!3 4
get!!5 8
;!!8 9
set!!: =
;!!= >
}!!? @
public"" 
RecentVideosVM"" 
Videos"" $
{""% &
get""' *
;""* +
set"", /
;""/ 0
}""1 2
public## 
IEnumerable## 
<## 
BikeVersionEntity## ,
>##, -
DiscontinuedBikes##. ?
{##@ A
get##B E
;##E F
set##G J
;##J K
}##L M
public%% 
bool%% $
IsUpComingBikesAvailable%% ,
{%%- .
get%%/ 2
;%%2 3
set%%4 7
;%%7 8
}%%9 :
public&& 
bool&& #
IsCompareBikesAvailable&& +
{&&, -
get&&. 1
;&&1 2
set&&3 6
;&&6 7
}&&8 9
public'' 
bool'' 
IsNewsAvailable'' #
{''$ %
get''& )
;'') *
set''+ .
;''. /
}''0 1
public(( 
bool(( $
IsExpertReviewsAvailable(( ,
{((- .
get((/ 2
;((2 3
set((4 7
;((7 8
}((9 :
public)) 
bool)) 
IsVideosAvailable)) %
{))& '
get))( +
;))+ ,
set))- 0
;))0 1
}))2 3
public** 
bool** '
IsDiscontinuedBikeAvailable** /
{**0 1
get**2 5
;**5 6
set**7 :
;**: ;
}**< =
public++ 
bool++ %
IsUsedModelsBikeAvailable++ -
{++. /
get++0 3
;++3 4
set++5 8
;++8 9
}++: ;
public-- 
bool-- /
#IsDealerServiceDataInIndiaAvailable-- 7
{--8 9
get--: =
;--= >
set--? B
;--B C
}--D E
public.. 
bool.. (
IsDealerServiceDataAvailable.. 0
{..1 2
get..3 6
;..6 7
set..8 ;
;..; <
}..= >
public// 
bool// 
IsDealerAvailable// %
{//& '
get//( +
;//+ ,
set//- 0
;//0 1
}//2 3
public00 
bool00 "
IsServiceDataAvailable00 *
{00+ ,
get00- 0
;000 1
set002 5
;005 6
}007 8
public11 
bool11 #
IsMakeTabsDataAvailable11 +
{11, -
get11. 1
;111 2
set113 6
;116 7
}118 9
public33 .
"DealersServiceCentersIndiaWidgetVM33 1 
DealersServiceCenter332 F
{33G H
get33I L
;33L M
set33N Q
;33Q R
}33S T
public44 (
ServiceCenterDetailsWidgetVM44 +
ServiceCenters44, :
{44; <
get44= @
;44@ A
set44B E
;44E F
}44G H
public55 
DealerCardVM55 
Dealers55 #
{55$ %
get55& )
;55) *
set55+ .
;55. /
}550 1
public77 
IEnumerable77 
<77 
BikeMakeEntityBase77 -
>77- .

OtherMakes77/ 9
{77: ;
get77< ?
;77? @
set77A D
;77D E
}77F G
}99 
}:: â	
@D:\work\bikewaleweb\Bikewale.Models\BikeCare\RecentBikeCareVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
BikeCare "
{ 
public

 

class

 
RecentBikeCareVM

 !
{ 
public 
IEnumerable 
< 
ArticleSummary )
>) *
ArticlesList+ 7
{8 9
get: =
;= >
set? B
;B C
}D E
public 
string 
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
	ModelName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
MakeMasking !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
ModelMasking "
{# $
get% (
;( )
set* -
;- .
}/ 0
} 
} Ö
AD:\work\bikewaleweb\Bikewale.Models\BikeModels\BikeModelRankVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public		 

class		 
BikeModelRankVM		  
{

 
public 
BikeRankingEntity  
Rank! %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
	StyleName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
BikeType 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
RankText 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} ≠
ND:\work\bikewaleweb\Bikewale.Models\DealerShowroom\DealerShowroomCityPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{		 
public 

class $
DealerShowroomCityPageVM )
:* +
	ModelBase, 5
{ 
public 
NearByCityDealer 
DealerCountCity  /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
public 
DealersEntity 
DealersList (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public "
UsedBikeModelsWidgetVM %
UsedBikeModel& 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public 
CityEntityBase 
CityDetails )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
uint 
TotalDealers  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
LeadCaptureEntity  
LeadCapture! ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
BrandCityPopupVM  
BrandCityPopupWidget  4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public #
MostPopularBikeWidgetVM &
PopularBikes' 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public (
ServiceCenterDetailsWidgetVM + 
ServiceCenterDetails, @
{A B
getC F
;F G
setH K
;K L
}M N
public 
BrandCityPopupVM 
BrandCityPopUp  .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
} 
} ô
SD:\work\bikewaleweb\Bikewale.Models\DealerShowroom\DealerShowroomDealerDetailsVM.cs
	namespace 	
Bikewale
 
. 
Models 
{		 
public 

class )
DealerShowroomDealerDetailsVM .
:/ 0
	ModelBase1 :
{ 
public 
DealerCardVM 
DealersList '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
LeadCaptureEntity  
LeadCapture! ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
CityEntityBase 
CityDetails )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
DealerBikesEntity  
DealerDetails! .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public #
MostPopularBikeWidgetVM &
PopularBikes' 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public (
ServiceCenterDetailsWidgetVM + 
ServiceCenterDetails, @
{A B
getC F
;F G
setH K
;K L
}M N
public 
uint 
PQCityId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
uint 
PQAreaID 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 

PQAreaName  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
CustomerAreaName &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
GALabel 
{ 
get  #
;# $
set% (
;( )
}* +
} 
} Ú	
OD:\work\bikewaleweb\Bikewale.Models\DealerShowroom\DealerShowroomIndiaPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{		 
public

 

class

 %
DealerShowroomIndiaPageVM

 *
:

+ ,
	ModelBase

- 6
{ 
public 
IEnumerable 
< 
DealerBrandEntity ,
>, -

AllDealers. 8
;8 9
public !
UpcomingBikesWidgetVM $
objUpcomingBikes% 5
{6 7
get8 ;
;; <
set= @
;@ A
}B C
public "
UsedBikeModelsWidgetVM %
UsedBikeModel& 3
;3 4
public 
BikeMakeEntityBase !
Make" &
;& '
public 
CityEntityBase 
CityDetails )
;) *
public 
uint 
DealerCount 
;  
public 
uint 
CitiesCount 
;  
public 
DealerLocatorList  
States! '
;' (
public 
NewLaunchedWidgetVM "
NewLaunchedBikes# 3
;3 4
} 
} ë
CD:\work\bikewaleweb\Bikewale.Models\BestBikes\PopularBodyStyleVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
PopularBodyStyleVM #
{ 
public 
IEnumerable 
<  
MostPopularBikesBase /
>/ 0
PopularBikes1 =
{> ?
get@ C
;C D
setE H
;H I
}J K
public 
string 
WidgetHeading #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
WidgetLinkTitle %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 

WidgetHref  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
BodyStyleText #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
BodyStyleLinkTitle (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
EnumBikeBodyStyles !
	BodyStyle" +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
bool 
ShowCheckOnRoadCTA &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
PQSourceEnum 

PQSourceId &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} ö
<D:\work\bikewaleweb\Bikewale.Models\BikeModels\BikeInfoVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 

BikeInfoVM

 
{ 
public 
GenericBikeInfo 
BikeInfo '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
BikeUrl 
{ 
get  #
;# $
set% (
;( )
}* +
public 
CityEntityBase 
CityDetails )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
bool 

IsUpcoming 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
bool 
IsDiscontinued "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
bool 
IsSmallSlug 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 

ReviewLink  
{! "
get" %
;% &
set& )
;) *
}* +
public 
uint 
ReviewCount 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} ´
AD:\work\bikewaleweb\Bikewale.Models\BestBikes\BestBikeWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
BestBikeWidgetVM !
{ 
public 
string 
Title 
{ 
get !
;! "
set# &
;& '
}( )
public 
EnumBikeBodyStyles !
?! "
CurrentPage# .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
string 
objBestBikesList &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
objBestScootersList )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
string !
objBestSportsBikeList +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
string #
objBestCruiserBikesList -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
string #
objBestMileageBikesList -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
} 
} ⁄
9D:\work\bikewaleweb\Bikewale.Models\Make\BrandWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
BrandWidgetVM 
{ 
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
	TopBrands/ 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
OtherBrands/ :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
bool 
HasOtherBrands "
{# $
get% (
{) *
return+ 1
(2 3
OtherBrands3 >
!=? A
nullB F
&&G I
OtherBrandsJ U
.U V
CountV [
([ \
)\ ]
>^ _
$num` a
)a b
;b c
}d e
}f g
} 
} ﬂ
ID:\work\bikewaleweb\Bikewale.Models\CompareBikes\ComparisonMinWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 !
ComparisonMinWidgetVM

 &
{ 
public 
TopBikeCompareBase !
TopComparisonRecord" 5
{6 7
get8 ;
;; <
set= @
;@ A
}B C
public 
IEnumerable 
< 
TopBikeCompareBase -
>- . 
RemainingCompareList/ C
{D E
getF I
;I J
setK N
;N O
}P Q
public 
bool  
ShowComparisonButton (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
} 
} √
MD:\work\bikewaleweb\Bikewale.Models\ExpertReviews\ExpertReviewsIndexPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class $
ExpertReviewsIndexPageVM )
:* +
	ModelBase, 5
{ 
public 

CMSContent 
Articles "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
BikeModelEntityBase "
Model# (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
int 

StartIndex 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
EndIndex 
{ 
get !
;! "
set# &
;& '
}( )
public 
PagerEntity 
PagerEntity &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
PageH1 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
PageH2 
{ 
get "
;" #
set$ '
;' (
}) *
public 
PopularBodyStyleVM !
PopularBodyStyle" 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public #
MostPopularBikeWidgetVM &
MostPopularBikes' 7
{8 9
get: =
;= >
set? B
;B C
}D E
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .%
PopularScooterMakesWidget/ H
{I J
getK N
;N O
setP S
;S T
}U V
} 
} â
JD:\work\bikewaleweb\Bikewale.Models\ExpertReviews\RecentExpertReviewsVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 !
RecentExpertReviewsVM

 &
{ 
public 
string 
Title 
{ 
get !
;! "
set# &
;& '
}( )
public 
IEnumerable 
< 
ArticleSummary )
>) *
ArticlesList+ 7
{8 9
get: =
;= >
set? B
;B C
}D E
public 
string 
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
	ModelName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
MakeMasking !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
ModelMasking "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
	LinkTitle 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
MoreExpertReviewUrl )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
int 
FetchedCount 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} ˇ
ID:\work\bikewaleweb\Bikewale.Models\BikeModels\MostPopularBikeWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class #
MostPopularBikeWidgetVM (
{ 
public 
uint 
	PageCatId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
PQSourceEnum 

PQSourceId &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
ShowCheckOnRoadCTA &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
ShowPriceInCityCTA &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
IEnumerable 
<  
MostPopularBikesBase /
>/ 0
Bikes1 6
{7 8
get9 <
;< =
set> A
;A B
}C D
public 
string 
WidgetHeading #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 

WidgetHref  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
WidgetLinkTitle %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
CtaText 
{ 
get  #
;# $
set% (
;( )
}* +
} 
} ÿ
FD:\work\bikewaleweb\Bikewale.Models\BikeModels\SimilarBikesWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class  
SimilarBikesWidgetVM %
{ 
public 
PQSourceEnum 

PQSourceId &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
ShowCheckOnRoadCTA &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
ShowPriceInCityCTA &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
IEnumerable 
< 
SimilarBikeEntity ,
>, -
Bikes. 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public 
BikeModelEntityBase "
Model# (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
uint 
	VersionId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
EnumBikeBodyStyles !
	BodyStyle" +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
GAPages 
Page 
{ 
get !
;! "
set# &
;& '
}( )
public 
bool 
IsNew 
{ 
get 
;  
set! $
;$ %
}& '
public 
bool 

IsUpcoming 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
bool 
IsDiscontinued "
{# $
get% (
;( )
set* -
;- .
}/ 0
} 
} û$
:D:\work\bikewaleweb\Bikewale.Models\HomePage\HomePageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 

HomePageVM 
: 
	ModelBase '
{ 
public 
string 
LocationMasking %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
Location 
{  
get! $
;$ %
set& )
;) *
}+ ,
public  
HomePageBannerEntity #
Banner$ *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
BrandWidgetVM 
Brands #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
NewLaunchedWidgetVM "
NewLaunchedBikes# 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public #
MostPopularBikeWidgetVM &
PopularBikes' 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public  
BestBikeByCategoryVM #
	BestBikes$ -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public "
UsedBikeCitiesWidgetVM %
UsedBikeCities& 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public "
UsedBikeModelsWidgetVM %

UsedModels& 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public 
RecentNewsVM 
News  
{! "
get# &
;& '
set( +
;+ ,
}- .
public !
RecentExpertReviewsVM $
ExpertReviews% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
RecentVideosVM 
Videos $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public  
PopularComparisonsVM #
ComparePopularBikes$ 7
{8 9
get: =
;= >
set? B
;B C
}D E
public   
bool   '
IsPopularBikesDataAvailable   /
{  0 1
get  2 5
;  5 6
set  7 :
;  : ;
}  < =
public!! 
bool!! &
IsNewLaunchedDataAvailable!! .
{!!/ 0
get!!1 4
;!!4 5
set!!6 9
;!!9 :
}!!; <
public"" 
bool"" %
IsUsedBikeCitiesAvailable"" -
{"". /
get""0 3
;""3 4
set""5 8
;""8 9
}"": ;
public## 
bool## #
IsUpcomingBikeAvailable## +
{##, -
get##. 1
;##1 2
set##3 6
;##6 7
}##8 9
public$$ 
bool$$ !
IsUsedModelsAvailable$$ )
{$$* +
get$$, /
;$$/ 0
set$$1 4
;$$4 5
}$$6 7
public%% 
bool%% *
IsComparePopularBikesAvailable%% 2
{%%3 4
get%%5 8
;%%8 9
set%%: =
;%%= >
}%%? @
public&& 
uint&& 
TabCount&& 
=&& 
$num&&  
;&&  !
public'' 
bool'' 
IsNewsActive''  
{''! "
get''# &
;''& '
set''( +
;''+ ,
}''- .
public(( 
bool((  
IsExpertReviewActive(( (
{(() *
get((+ .
;((. /
set((0 3
;((3 4
}((5 6
public)) 
bool)) 
IsVideoActive)) !
=))" #
false))$ )
;))) *
public** 
IEnumerable** 
<** 
RecentReviewsWidget** .
>**. /!
RecentUserReviewsList**0 E
{**F G
get**H K
;**K L
set**M P
;**P Q
}**R S
public,, 
string,, 
Source,, 
{,, 
get,, "
;,," #
set,,$ '
;,,' (
},,) *
}-- 
}// ò
ED:\work\bikewaleweb\Bikewale.Models\Location\ChangeLocationPopupVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public		 

class		 !
ChangeLocationPopupVM		 &
{

 
public 
bool 
IsLocationChanged %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public  
GlobalCityAreaEntity #
CurrentLocation$ 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public  
GlobalCityAreaEntity #
UrlLocation$ /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
} 
} ›
8D:\work\bikewaleweb\Bikewale.Models\Make\OtherMakesVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
OtherMakesVM 
{ 
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
Makes/ 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
} 
} ë	
JD:\work\bikewaleweb\Bikewale.Models\Mobile\NewLaunches\NewLaunchedBikes.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Mobile  
.  !
NewLaunches! ,
{ 
public 

class 
NewLaunchedBikes !
{ 
public 
PageMetaTags 
	PageMetas %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public		 
BikeMakeEntityBase		 !
Make		" &
{		' (
get		) ,
;		, -
set		. 1
;		1 2
}		3 4
public

 
BikeModelEntityBase

 "
Model

# (
{

) *
get

+ .
;

. /
set

0 3
;

3 4
}

5 6
public 
string 
PageHeading !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
PageDescription %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
} 
} »
AD:\work\bikewaleweb\Bikewale.Models\Mobile\Videos\VideoDetails.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Mobile  
.  !
Videos! '
{ 
public 

class 
GenericBikeInfoCard $
{ 
public 
uint 
ModelId 
{ 
get !
;! "
set# &
;& '
}( )
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public		 
BikeInfoTabType		 
PageId		 %
{		& '
get		( +
;		+ ,
set		- 0
;		0 1
}		2 3
public

 
bool

 
IsSmallSlug

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

, -
} 
} ¡
8D:\work\bikewaleweb\Bikewale.Models\NewPage\NewPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
	NewPageVM 
: 
	ModelBase &
{ 
public 
string 
LocationMasking %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
Location 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
BrandWidgetVM 
Brands #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
NewLaunchedWidgetVM "
NewLaunchedBikes# 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public #
MostPopularBikeWidgetVM &
PopularBikes' 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public  
BestBikeByCategoryVM #
	BestBikes$ -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
RecentNewsVM 
News  
{! "
get# &
;& '
set( +
;+ ,
}- .
public !
RecentExpertReviewsVM $
ExpertReviews% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
RecentVideosVM 
Videos $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public  
PopularComparisonsVM #
ComparePopularBikes$ 7
{8 9
get: =
;= >
set? B
;B C
}D E
public 
bool '
IsPopularBikesDataAvailable /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
public 
bool &
IsNewLaunchedDataAvailable .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
bool *
IsComparePopularBikesAvailable 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public   
bool   #
IsUpcomingBikeAvailable   +
{  , -
get  . 1
;  1 2
set  3 6
;  6 7
}  8 9
public"" 
uint"" 
TabCount"" 
="" 
$num""  
;""  !
public## 
bool## 
IsNewsActive##  
{##! "
get### &
;##& '
set##( +
;##+ ,
}##- .
public$$ 
bool$$  
IsExpertReviewActive$$ (
{$$) *
get$$+ .
;$$. /
set$$0 3
;$$3 4
}$$5 6
public%% 
bool%% 
IsVideoActive%% !
=%%" #
false%%$ )
;%%) *
public&& 
IEnumerable&& 
<&& 
RecentReviewsWidget&& .
>&&. /!
RecentUserReviewsList&&0 E
{&&F G
get&&H K
;&&K L
set&&M P
;&&P Q
}&&R S
public(( 
string(( 
Source(( 
{(( 
get(( "
;((" #
set(($ '
;((' (
}(() *
})) 
}** ”
<D:\work\bikewaleweb\Bikewale.Models\News\NewsDetailPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
NewsDetailPageVM !
:" #
	ModelBase$ -
{ 
public 
ArticleDetails 
ArticleDetails ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
PopularBodyStyleVM !
PopularBodyStyle" 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public #
MostPopularBikeWidgetVM &
MostPopularBikes' 7
{8 9
get: =
;= >
set? B
;B C
}D E
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 

BikeInfoVM 
BikeInfo "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
BikeModelEntityBase "
Model# (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
string 
BaseUrl 
{ 
get  #
;# $
set% (
;( )
}* +
public 
PwaReduxStore 

ReduxStore '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
IHtmlString 
ServerRouterWrapper .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
string 
WindowState !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .%
PopularScooterMakesWidget/ H
{I J
getK N
;N O
setP S
;S T
}U V
} 
} ‹
ED:\work\bikewaleweb\Bikewale.Models\NewLaunches\NewLaunchedIndexVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
NewLaunchedIndexVM #
:$ %
	ModelBase& /
{		 
public

 
NewLaunchesBikesVM

 !
NewLaunched

" -
{

. /
get

0 3
;

3 4
set

5 8
;

8 9
}

: ;
public !
UpcomingBikesWidgetVM $
Upcoming% -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
BrandWidgetVM 
Brands #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public 
bool 
HasUpcoming 
{  !
get" %
{& '
return( .
Upcoming/ 7
!=8 :
null; ?
&&@ B
UpcomingC K
.K L
UpcomingBikesL Y
!=Z \
null] a
&&b d
Upcominge m
.m n
UpcomingBikesn {
.{ |
Count	| Å
(
Å Ç
)
Ç É
>
Ñ Ö
$num
Ü á
;
á à
}
â ä
}
ã å
public 
bool 
	HasBrands 
{ 
get  #
{$ %
return& ,
Brands- 3
!=4 6
null7 ;
&&< >
Brands? E
.E F
	TopBrandsF O
!=P R
nullS W
&&X Z
Brands[ a
.a b
	TopBrandsb k
.k l
Countl q
(q r
)r s
>t u
$numv w
;w x
}y z
}{ |
} 
} ˛
DD:\work\bikewaleweb\Bikewale.Models\NewLaunches\NewLaunchedMakeVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public		 

class		 
NewLaunchedMakeVM		 "
:		# $
	ModelBase		% .
{

 
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
NewLaunchesBikesVM !
NewLaunched" -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public !
UpcomingBikesWidgetVM $
Upcoming% -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
BrandWidgetVM 
Brands #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public 
bool 
HasUpcoming 
{  !
get" %
{& '
return( .
Upcoming/ 7
!=8 :
null; ?
&&@ B
UpcomingC K
.K L
UpcomingBikesL Y
!=Z \
null] a
&&b d
Upcominge m
.m n
UpcomingBikesn {
.{ |
Count	| Å
(
Å Ç
)
Ç É
>
Ñ Ö
$num
Ü á
;
á à
}
â ä
}
ã å
public 
bool 
	HasBrands 
{ 
get  #
{$ %
return& ,
Brands- 3
!=4 6
null7 ;
&&< >
Brands? E
.E F
	TopBrandsF O
!=P R
nullS W
&&X Z
Brands[ a
.a b
	TopBrandsb k
.k l
Countl q
(q r
)r s
>t u
$numv w
;w x
}y z
}{ |
} 
} ‘
ED:\work\bikewaleweb\Bikewale.Models\NewLaunches\NewLaunchesBikesVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
NewLaunchesBikesVM #
{ 
public !
NewLaunchedBikeResult $
Bikes% *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
IEnumerable 
< &
BikesCountByMakeEntityBase 5
>5 6
Makes7 <
{= >
get? B
;B C
setD G
;G H
}I J
public 
PagerEntity 
Pager  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
bool 
HasBikes 
{ 
get "
{# $
return% +
(, -
Bikes- 2
!=3 5
null6 :
&&; =
Bikes> C
.C D

TotalCountD N
>O P
$numQ R
)R S
;S T
}U V
}W X
public 
bool 
HasMakes 
{ 
get "
{# $
return% +
(, -
Makes- 2
!=3 5
null6 :
&&; =
Makes> C
.C D
CountD I
(I J
)J K
>L M
$numN O
)O P
;P Q
}R S
}T U
public 
bool 
HasPages 
{ 
get "
{# $
return% +
(, -
Pager- 2
!=3 5
null6 :
&&; =
Pager> C
.C D
TotalResultsD P
>Q R
$numS T
)T U
;U V
}W X
}Y Z
public 
string 
Page_H2 
{ 
get  #
;# $
set% (
;( )
}* +
public 
PQSourceEnum 
PqSource $
{% &
get' *
;* +
set, /
;/ 0
}1 2
} 
} ò
;D:\work\bikewaleweb\Bikewale.Models\News\NewsIndexPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{		 
public 

class 
NewsIndexPageVM  
:! "
	ModelBase# ,
{ 
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
BikeModelEntityBase "
Model# (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
int 

StartIndex 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
EndIndex 
{ 
get !
;! "
set# &
;& '
}( )
public 

CMSContent 
Articles "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
PagerEntity 
PagerEntity &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
PageH1 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
PageH2 
{ 
get "
;" #
set$ '
;' (
}) *
public 
PopularBodyStyleVM !
PopularBodyStyle" 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public #
MostPopularBikeWidgetVM &
MostPopularBikes' 7
{8 9
get: =
;= >
set? B
;B C
}D E
public !
UpcomingBikesWidgetVM $
UpcomingBikes% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
PwaReduxStore 

ReduxStore '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
IHtmlString 
ServerRouterWrapper .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
string 
WindowState !
{" #
get$ '
;' (
set) ,
;, -
}. /
public   
IEnumerable   
<   
BikeMakeEntityBase   -
>  - .%
PopularScooterMakesWidget  / H
{  I J
get  K N
;  N O
set  P S
;  S T
}  U V
}!! 
}"" ∞
FD:\work\bikewaleweb\Bikewale.Models\NewLaunches\NewLaunchedWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 
NewLaunchedWidgetVM

 $
{ 
public 
IEnumerable 
< %
NewLaunchedBikeEntityBase 4
>4 5
Bikes6 ;
{< =
get> A
;A B
setC F
;F G
}H I
public 
uint 

PQSourceId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
uint 
	PageCatId 
{ 
get  #
;# $
set% (
;( )
}* +
} 
} ‘
>D:\work\bikewaleweb\Bikewale.Models\News\NewsScootersPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
News 
{ 
public 
class	 
NewsScootersPageVM !
:" #
	ModelBase$ -
{ 
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
BikeModelEntityBase "
Model# (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
int 

StartIndex 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
EndIndex 
{ 
get !
;! "
set# &
;& '
}( )
public 

CMSContent 
Articles "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
PagerEntity 
PagerEntity &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
PageH1 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
PageH2 
{ 
get "
;" #
set$ '
;' (
}) *
public 
PopularBodyStyleVM !
PopularBodyStyle" 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public #
MostPopularBikeWidgetVM &
MostPopularBikes' 7
{8 9
get: =
;= >
set? B
;B C
}D E
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .%
PopularScooterMakesWidget/ H
{I J
getK N
;N O
setP S
;S T
}U V
public 
string -
!PopularScooterBrandsWidgetHeading 7
{8 9
get: =
;= >
set? B
;B C
}D E
}!! 
}"" ÓQ
CD:\work\bikewaleweb\Bikewale.Models\Price\DealerPriceQuotePageVM.cs
	namespace		 	
Bikewale		
 
.		 
Models		 
.		 
Price		 
{

 
public 

class "
DealerPriceQuotePageVM '
:( )
	ModelBase* 3
{ 
public 
BikeVersionEntity  
SelectedVersion! 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public 
IEnumerable 
< "
BikeVersionsListEntity 1
>1 2
VersionsList3 ?
{@ A
getB E
;E F
setG J
;J K
}L M
public 
Bikewale 
. 
Entities  
.  !

PriceQuote! +
.+ ,
v2, .
.. /)
DetailedDealerQuotationEntity/ L
DetailedDealerM [
{\ ]
get^ a
;a b
setc f
;f g
}h i
public 
BikeQuotationEntity "
	Quotation# ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
LeadCaptureEntity  
LeadCapture! ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
DealerPackageTypes !

DealerType" ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
string 
MinSpecsHtml "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
Location 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
MPQQueryString $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
uint 
PQId 
{ 
get 
; 
set  #
;# $
}% &
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
CiyName 
{ 
get  #
;# $
set% (
;( )
}* +
public 
uint 
AreaId 
{ 
get  
;  !
set" %
;% &
}' (
public   
uint   

TotalPrice   
{    
get  ! $
;  $ %
set  & )
;  ) *
}  + ,
public!! 
DealerCardVM!! 
OtherDealers!! (
{!!) *
get!!+ .
;!!. /
set!!0 3
;!!3 4
}!!5 6
public"" 
uint"" 
ModelId"" 
{"" 
get"" !
{""" #
return""$ *
(""+ ,
this"", 0
.""0 1
SelectedVersion""1 @
!=""A C
null""D H
&&""I K
this""L P
.""P Q
SelectedVersion""Q `
.""` a
	ModelBase""a j
!=""k m
null""n r
)""r s
?""t u
(""v w
uint""w {
)""{ |
this	""| Ä
.
""Ä Å
SelectedVersion
""Å ê
.
""ê ë
	ModelBase
""ë ö
.
""ö õ
ModelId
""õ ¢
:
""£ §
$num
""• ¶
;
""¶ ß
}
""® ©
}
""™ ´
public## 
uint## 
	VersionId## 
{## 
get##  #
{##$ %
return##& ,
(##- .
this##. 2
.##2 3
SelectedVersion##3 B
!=##C E
null##F J
)##J K
?##L M
(##N O
uint##O S
)##S T
this##T X
.##X Y
SelectedVersion##Y h
.##h i
	VersionId##i r
:##s t
$num##u v
;##v w
}##x y
}##z {
public%% 
bool%% $
IsPrimaryDealerAvailable%% ,
{%%- .
get%%/ 2
{%%3 4
return%%5 ;
(%%< =
this%%= A
.%%A B
DetailedDealer%%B P
!=%%Q S
null%%T X
&&%%Y [
this%%\ `
.%%` a
DetailedDealer%%a o
.%%o p
PrimaryDealer%%p }
!=	%%~ Ä
null
%%Å Ö
&&
%%Ü à
this
%%â ç
.
%%ç é
DetailedDealer
%%é ú
.
%%ú ù
PrimaryDealer
%%ù ™
.
%%™ ´
DealerDetails
%%´ ∏
!=
%%π ª
null
%%º ¿
)
%%¿ ¡
;
%%¡ ¬
}
%%√ ƒ
}
%%≈ ∆
public'' 
bool'' 
IsDealerPriceQuote'' &
{''' (
get'') ,
{''- .
return''/ 5
(''6 7
this''7 ;
.''; <
DetailedDealer''< J
!=''K M
null''N R
&&''S U
this''V Z
.''Z [
DetailedDealer''[ i
.''i j
PrimaryDealer''j w
!=''x z
null''{ 
&&
''Ä Ç
this
''É á
.
''á à
DetailedDealer
''à ñ
.
''ñ ó
PrimaryDealer
''ó §
.
''§ •
	PriceList
''• Æ
!=
''Ø ±
null
''≤ ∂
&&
''∑ π
this
''∫ æ
.
''æ ø
DetailedDealer
''ø Õ
.
''Õ Œ
PrimaryDealer
''Œ €
.
''€ ‹
	PriceList
''‹ Â
.
''Â Ê
Count
''Ê Î
(
''Î Ï
)
''Ï Ì
>
''Ó Ô
$num
'' Ò
)
''Ò Ú
;
''Ú Û
}
''Ù ı
}
''ˆ ˜
public)) 
bool)) 
ShowOtherDealers)) $
{))% &
get))' *
{))+ ,
return))- 3
())4 5
this))5 9
.))9 :
DealerId)): B
==))C E
$num))F G
&&))H J
this))K O
.))O P
DetailedDealer))P ^
!=))_ a
null))b f
&&))g i
this))j n
.))n o
DetailedDealer))o }
.))} ~!
SecondaryDealerCount	))~ í
==
))ì ï
$num
))ñ ó
&&
))ò ö
this
))õ ü
.
))ü †
	Quotation
))† ©
!=
))™ ¨
null
))≠ ±
&&
))≤ ¥
string
))µ ª
.
))ª º
IsNullOrEmpty
))º …
(
))…  
this
))  Œ
.
))Œ œ
	Quotation
))œ ÿ
.
))ÿ Ÿ
ManufacturerAd
))Ÿ Á
)
))Á Ë
)
))Ë È
;
))È Í
}
))Î Ï
}
))Ì Ó
public++ 
bool++ $
AreOtherDealersAvailable++ ,
{++- .
get++/ 2
{++3 4
return++5 ;
(++< =
this++= A
.++A B
OtherDealers++B N
!=++O Q
null++R V
&&++W Y
this++Z ^
.++^ _
OtherDealers++_ k
.++k l
Dealers++l s
!=++t v
null++w {
&&++| ~
this	++ É
.
++É Ñ
OtherDealers
++Ñ ê
.
++ê ë
Dealers
++ë ò
.
++ò ô
Count
++ô û
(
++û ü
)
++ü †
>
++° ¢
$num
++£ §
)
++§ •
;
++• ¶
}
++ß ®
}
++© ™
public-- 
string-- 
LeadBtnLongText-- %
{--& '
get--( +
{--, -
return--. 4
$str--5 M
;--M N
}--O P
}--Q R
public.. 
string.. 
LeadBtnShortText.. &
{..' (
get..) ,
{..- .
return../ 5
$str..6 B
;..B C
}..D E
}..F G
public00 
string00 
ClientIP00 
{00  
get00! $
;00$ %
set00& )
;00) *
}00+ ,
public11 
string11 
PageUrl11 
{11 
get11  #
;11# $
set11% (
;11( )
}11* +
public22 
int22 
PQSourcePage22 
{22  !
get22" %
{22& '
return22( .
(22/ 0
int220 3
)223 4
Bikewale224 <
.22< =
Entities22= E
.22E F

PriceQuote22F P
.22P Q
PQSourceEnum22Q ]
.22] ^!
Desktop_DPQ_Quotation22^ s
;22s t
}22u v
}22w x
public33 
int33 
PQLeadSource33 
{33  !
get33" %
{33& '
return33( .
$num33/ 1
;331 2
}333 4
}335 6
public55  
SimilarBikesWidgetVM55 #
SimilarBikesVM55$ 2
{553 4
get555 8
;558 9
set55: =
;55= >
}55? @
public66 )
ManufactureCampaignLeadEntity66 ,
LeadCampaign66- 9
{66: ;
get66< ?
;66? @
set66A D
;66D E
}66F G
public77 
bool77 %
IsManufacturerLeadAdShown77 -
{77. /
get770 3
;773 4
set775 8
;778 9
}77: ;
public88 (
ManufactureCampaignEMIEntity88 +
EMICampaign88, 7
{888 9
get88: =
;88= >
set88? B
;88B C
}88D E
public99 
bool99 $
IsManufacturerEMIAdShown99 ,
{99- .
get99/ 2
;992 3
set994 7
;997 8
}999 :
public:: 
string:: 
BhriguTrackingLabel:: )
{::* +
get::, /
;::/ 0
set::1 4
;::4 5
}::6 7
public;; 
EnumBikeBodyStyles;; !
	BodyStyle;;" +
{;;, -
get;;. 1
;;;1 2
set;;3 6
;;;6 7
};;8 9
public<< 
string<< 
BodyStyleText<< #
{<<$ %
get<<& )
;<<) *
set<<+ .
;<<. /
}<<0 1
}== 
}>> é
8D:\work\bikewaleweb\Bikewale.Models\News\RecentNewsVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 
RecentNewsVM

 
{ 
public 
string 
Title 
{ 
get !
;! "
set# &
;& '
}( )
public 
IEnumerable 
< 
ArticleSummary )
>) *
ArticlesList+ 7
{8 9
get: =
;= >
set? B
;B C
}D E
public 
string 
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
	ModelName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
MakeMasking !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
ModelMasking "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
int 
FetchedCount 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} óÇ
DD:\work\bikewaleweb\Bikewale.Models\PriceInCity\PriceInCityPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
PriceInCityPageVM "
:# $
	ModelBase% .
{ 
public 
IEnumerable 
< !
PriceQuoteOfTopCities 0
>0 1 
PriceInNearestCities2 F
{G H
getI L
;L M
setN Q
;Q R
}S T
public $
PriceInTopCitiesWidgetVM '
PriceTopCities( 6
{7 8
get9 <
;< =
set> A
;A B
}C D
public 
DealerCardVM 
Dealers #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public  
SimilarBikesWidgetVM #
AlternateBikes$ 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public (
ServiceCenterDetailsWidgetVM +
ServiceCenters, :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
uint 
ServiceCenterCount &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public $
PriceInTopCitiesWidgetVM '
PriceInTopCities( 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public 
uint 
ServiceCentersCount '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 

BikeInfoVM 
BikeInfo "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
Entities 
. 
BikeData  
.  !
BikeMakeEntityBase! 3
Make4 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public 
Entities 
. 
BikeData  
.  !
BikeModelEntityBase! 4
	BikeModel5 >
{? @
getA D
;D E
setF I
;I J
}K L
public   
IEnumerable   
<   
BikeVersionMinSpecs   .
>  . /
VersionSpecs  0 <
{  = >
get  ? B
;  B C
set  D G
;  G H
}  I J
public!! 
IEnumerable!! 
<!! 
BikeQuotationEntity!! .
>!!. /
BikeVersionPrices!!0 A
{!!B C
get!!D G
;!!G H
set!!I L
;!!L M
}!!N O
public"" 
CityEntityBase"" 

CityEntity"" (
{"") *
get""+ .
;"". /
set""0 3
;""3 4
}""5 6
public## 
CityEntityBase## 
CookieCityEntity## .
{##/ 0
get##1 4
;##4 5
set##6 9
;##9 :
}##; <
public$$ 
BikeModelRankVM$$ 
BikeRank$$ '
{$$( )
get$$* -
;$$- .
set$$/ 2
;$$2 3
}$$4 5
public%% 
string%% 

ModelImage%%  
{%%! "
get%%# &
;%%& '
set%%( +
;%%+ ,
}%%- .
public&& 
uint&& 
	VersionId&& 
{&& 
get&&  #
;&&# $
set&&% (
;&&( )
}&&* +
public'' 
String'' 
PageDescription'' %
{''& '
get''( +
;''+ ,
set''- 0
;''0 1
}''2 3
public(( 
bool(( 
IsNew(( 
{(( 
get(( 
;((  
set((! $
;(($ %
}((& '
public)) 
bool)) 
IsAreaSelected)) "
{))# $
get))% (
;))( )
set))* -
;))- .
}))/ 0
public** 
bool** 
IsAreaAvailable** #
{**$ %
get**& )
;**) *
set**+ .
;**. /
}**0 1
public++ 
string++ 
CookieCityArea++ $
{++% &
get++' *
;++* +
set++, /
;++/ 0
}++1 2
public,, $
PriceInTopCitiesWidgetVM,, '
NearestPriceCities,,( :
{,,; <
get,,= @
;,,@ A
set,,B E
;,,E F
},,G H
public-- 
String-- 
BikeName-- 
{--  
get--! $
{--% &
return--' -
(--. /
String--/ 5
.--5 6
Format--6 <
(--< =
$str--= F
,--F G
Make--H L
.--L M
MakeName--M U
,--U V
	BikeModel--W `
.--` a
	ModelName--a j
)--j k
)--k l
;--l m
}--n o
}--p q
public// 
String// 
DealersWidget_H2// &
{//' (
get//) ,
{//- .
return/// 5
(//6 7

HasDealers//7 A
?//B C
String//D J
.//J K
Format//K Q
(//Q R
$str//R h
,//h i
Make//j n
.//n o
MakeName//o w
,//w x

CityEntity	//y É
.
//É Ñ
CityName
//Ñ å
)
//å ç
:
//é è
$str
//ê í
)
//í ì
;
//ì î
}
//ï ñ
}
//ó ò
public00 
String00 '
DealersWidget_ViewAll_Title00 1
{002 3
get004 7
{008 9
return00: @
String00A G
.00G H
Format00H N
(00N O
$str00O e
,00e f
Make00g k
.00k l
MakeName00l t
,00t u

CityEntity	00v Ä
.
00Ä Å
CityName
00Å â
)
00â ä
;
00ä ã
}
00å ç
}
00é è
public11 
String11 &
DealersWidget_ViewAll_Href11 0
{111 2
get113 6
{117 8
return119 ?
String11@ F
.11F G
Format11G M
(11M N
$str11N m
,11m n
Make11o s
.11s t
MaskingName11t 
,	11 Ä

CityEntity
11Å ã
.
11ã å
CityMaskingName
11å õ
)
11õ ú
;
11ú ù
}
11û ü
}
11† °
public44 
String44 "
ServiceCenterWidget_H244 ,
{44- .
get44/ 2
{443 4
return445 ;
(44< =
HasServiceCenters44= N
?44O P
String44Q W
.44W X
Format44X ^
(44^ _
$str44_ {
,44{ |
Make	44} Å
.
44Å Ç
MakeName
44Ç ä
,
44ä ã

CityEntity
44å ñ
.
44ñ ó
CityName
44ó ü
)
44ü †
:
44° ¢
$str
44£ •
)
44• ¶
;
44¶ ß
}
44® ©
}
44™ ´
public55 
String55 -
!ServiceCenterWidget_ViewAll_Title55 7
{558 9
get55: =
{55> ?
return55@ F
String55G M
.55M N
Format55N T
(55T U
$str55U q
,55q r
Make55s w
.55w x
MakeName	55x Ä
,
55Ä Å

CityEntity
55Ç å
.
55å ç
CityName
55ç ï
)
55ï ñ
;
55ñ ó
}
55ò ô
}
55ö õ
public66 
String66 ,
 ServiceCenterWidget_ViewAll_Href66 6
{667 8
get669 <
{66= >
return66? E
String66F L
.66L M
Format66M S
(66S T
$str66T q
,66q r
Make66s w
.66w x
MaskingName	66x É
,
66É Ñ

CityEntity
66Ö è
.
66è ê
CityMaskingName
66ê ü
)
66ü †
;
66† °
}
66¢ £
}
66§ •
public88 
String88 '
NearestPriceCitiesWidget_H288 1
{882 3
get884 7
{888 9
return88: @
(88A B!
HasNearestPriceCities88B W
?88X Y
String88Z `
.88` a
Format88a g
(88g h
$str	88h Ü
,
88Ü á
	BikeModel
88à ë
.
88ë í
	ModelName
88í õ
,
88õ ú

CityEntity
88ù ß
.
88ß ®
CityName
88® ∞
)
88∞ ±
:
88≤ ≥
$str
88¥ ∂
)
88∂ ∑
;
88∑ ∏
}
88π ∫
}
88ª º
public:: 
BikeQuotationEntity:: "
FirstVersion::# /
{::0 1
get::2 5
{::6 7
return::8 >
(::? @
BikeVersionPrices::@ Q
!=::R T
null::U Y
&&::Z \
BikeVersionPrices::] n
.::n o
Count::o t
(::t u
)::u v
>::w x
$num::y z
?::{ |
BikeVersionPrices	::} é
.
::é è
FirstOrDefault
::è ù
(
::ù û
)
::û ü
:
::† °
null
::¢ ¶
)
::¶ ß
;
::ß ®
}
::© ™
}
::´ ¨
public;; 
bool;; 
IsDiscontinued;; "
{;;# $
get;;% (
{;;) *
return;;+ 1
!;;2 3
IsNew;;3 8
;;;8 9
};;: ;
};;< =
public<< 
bool<< !
HasNearestPriceCities<< )
{<<* +
get<<, /
{<<0 1
return<<2 8
(<<9 :
NearestPriceCities<<: L
!=<<M O
null<<P T
&&<<U W
NearestPriceCities<<X j
.<<j k
PriceQuoteList<<k y
!=<<z |
null	<<} Å
&&
<<Ç Ñ 
NearestPriceCities
<<Ö ó
.
<<ó ò
PriceQuoteList
<<ò ¶
.
<<¶ ß
Count
<<ß ¨
(
<<¨ ≠
)
<<≠ Æ
>
<<Ø ∞
$num
<<± ≤
)
<<≤ ≥
;
<<≥ ¥
}
<<µ ∂
}
<<∑ ∏
public== 
bool== 
HasPriceInTopCities== '
{==( )
get==* -
{==. /
return==0 6
PriceInTopCities==7 G
!===H J
null==K O
&&==P R
PriceInTopCities==S c
.==c d
PriceQuoteList==d r
!===s u
null==v z
&&=={ }
PriceInTopCities	==~ é
.
==é è
PriceQuoteList
==è ù
.
==ù û
Count
==û £
(
==£ §
)
==§ •
>
==¶ ß
$num
==® ©
;
==© ™
}
==´ ¨
}
==≠ Æ
public>> 
bool>> 

HasDealers>> 
{>>  
get>>! $
{>>% &
return>>' -
(>>. /
Dealers>>/ 6
!=>>7 9
null>>: >
&&>>? A
Dealers>>B I
.>>I J
Dealers>>J Q
!=>>R T
null>>U Y
&&>>Z \
Dealers>>] d
.>>d e
Dealers>>e l
.>>l m
Count>>m r
(>>r s
)>>s t
>>>u v
$num>>w x
)>>x y
;>>y z
}>>{ |
}>>} ~
public?? 
bool?? 
HasAlternateBikes?? %
{??& '
get??( +
{??, -
return??. 4
(??5 6
AlternateBikes??6 D
!=??E G
null??H L
&&??M O
AlternateBikes??P ^
.??^ _
Bikes??_ d
!=??e g
null??h l
&&??m o
AlternateBikes??p ~
.??~ 
Bikes	?? Ñ
.
??Ñ Ö
Count
??Ö ä
(
??ä ã
)
??ã å
>
??ç é
$num
??è ê
)
??ê ë
;
??ë í
}
??ì î
}
??ï ñ
public@@ 
bool@@ 
HasTopCities@@  
{@@! "
get@@# &
{@@' (
return@@) /
(@@0 1
PriceTopCities@@1 ?
!=@@@ B
null@@C G
&&@@H J
PriceTopCities@@K Y
.@@Y Z
PriceQuoteList@@Z h
!=@@i k
null@@l p
&&@@q s
PriceTopCities	@@t Ç
.
@@Ç É
PriceQuoteList
@@É ë
.
@@ë í
Count
@@í ó
(
@@ó ò
)
@@ò ô
>
@@ö õ
$num
@@ú ù
)
@@ù û
;
@@û ü
}
@@† °
}
@@¢ £
publicAA 
stringAA 
JSONBikeVersionsAA &
{AA' (
getAA) ,
;AA, -
setAA. 1
;AA1 2
}AA3 4
publicCC 
boolCC 
HasServiceCentersCC %
{CC& '
getCC( +
{CC, -
returnCC. 4
(CC5 6
ServiceCentersCountCC6 I
>CCJ K
$numCCL M
)CCM N
;CCN O
}CCP Q
}CCR S
publicDD 
boolDD 
HasCampaignDealerDD %
{DD& '
getDD( +
{DD, -
returnDD. 4
(DD5 6
DetailedDealerDD6 D
!=DDE G
nullDDH L
&&DDM O
DetailedDealerDDP ^
.DD^ _
PrimaryDealerDD_ l
!=DDm o
nullDDp t
&&DDu w
DetailedDealer	DDx Ü
.
DDÜ á
PrimaryDealer
DDá î
.
DDî ï
DealerDetails
DDï ¢
!=
DD£ •
null
DD¶ ™
)
DD™ ´
;
DD´ ¨
}
DD≠ Æ
}
DDØ ∞
publicFF 
BikewaleFF 
.FF 
EntitiesFF  
.FF  !

PriceQuoteFF! +
.FF+ ,
v2FF, .
.FF. /)
DetailedDealerQuotationEntityFF/ L
DetailedDealerFFM [
{FF\ ]
getFF^ a
;FFa b
setFFc f
;FFf g
}FFh i
publicGG 
stringGG 
	MPQStringGG 
{GG  !
getGG" %
;GG% &
setGG' *
;GG* +
}GG, -
publicHH 
stringHH 
MinSpecsHtmlHH "
{HH# $
getHH% (
;HH( )
setHH* -
;HH- .
}HH/ 0
publicII 
stringII 

GABikeNameII  
{II! "
getII# &
{II' (
returnII) /
stringII0 6
.II6 7
FormatII7 =
(II= >
$strII> G
,IIG H
MakeIII M
.IIM N
MakeNameIIN V
,IIV W
	BikeModelIIX a
.IIa b
	ModelNameIIb k
)IIk l
;IIl m
}IIn o
}IIp q
publicJJ 
LeadCaptureEntityJJ  
LeadCaptureJJ! ,
{JJ- .
getJJ/ 2
;JJ2 3
setJJ4 7
;JJ7 8
}JJ9 :
publicKK 
ulongKK 
PQIdKK 
{KK 
getKK 
;KK  
setKK! $
;KK$ %
}KK& '
publicLL )
ManufactureCampaignLeadEntityLL ,
LeadCampaignLL- 9
{LL: ;
getLL< ?
;LL? @
setLLA D
;LLD E
}LLF G
publicMM 
boolMM %
IsManufacturerLeadAdShownMM -
{MM. /
getMM0 3
;MM3 4
setMM5 8
;MM8 9
}MM: ;
publicNN (
ManufactureCampaignEMIEntityNN +
EMICampaignNN, 7
{NN8 9
getNN: =
;NN= >
setNN? B
;NNB C
}NND E
publicOO 
boolOO $
IsManufacturerEMIAdShownOO ,
{OO- .
getOO/ 2
;OO2 3
setOO4 7
;OO7 8
}OO9 :
publicQQ 
EnumBikeBodyStylesQQ !
	BodyStyleQQ" +
{QQ, -
getQQ. 1
;QQ1 2
setQQ3 6
;QQ6 7
}QQ8 9
publicRR 
stringRR 
BodyStyleTextRR #
{RR$ %
getRR& )
;RR) *
setRR+ .
;RR. /
}RR0 1
publicSS 
PopularBodyStyleVMSS !
PopularBodyStyleSS" 2
{SS3 4
getSS5 8
;SS8 9
setSS: =
;SS= >
}SS? @
publicTT 
boolTT '
IsPopularBodyStyleAvailableTT /
{TT0 1
getTT2 5
{TT6 7
returnTT8 >
(TT? @
PopularBodyStyleTT@ P
!=TTQ S
nullTTT X
&&TTY [
PopularBodyStyleTT\ l
.TTl m
PopularBikesTTm y
!=TTz |
null	TT} Å
&&
TTÇ Ñ
PopularBodyStyle
TTÖ ï
.
TTï ñ
PopularBikes
TTñ ¢
.
TT¢ £
Count
TT£ ®
(
TT® ©
)
TT© ™
>
TT´ ¨
$num
TT≠ Æ
)
TTÆ Ø
;
TTØ ∞
}
TT± ≤
}
TT≥ ¥
}UU 
}WW Ã
KD:\work\bikewaleweb\Bikewale.Models\PriceInCity\PriceInTopCitiesWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
PriceInCity %
{ 
public

 

class

 $
PriceInTopCitiesWidgetVM

 )
{ 
public 
IEnumerable 
< !
PriceQuoteOfTopCities 0
>0 1
PriceQuoteList2 @
{A B
getC F
;F G
setH K
;K L
}M N
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} È
>D:\work\bikewaleweb\Bikewale.Models\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str *
)* +
]+ ,
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
$str ,
), -
]- .
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
]$$) *ç
AD:\work\bikewaleweb\Bikewale.Models\Scooters\ScooterComparesVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
ScooterComparesVM "
{ 
public 
ICollection 
< $
SimilarCompareBikeEntity 3
>3 4
Bikes5 :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
string 
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} õ 
CD:\work\bikewaleweb\Bikewale.Models\Scooters\ScootersIndexPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
ScootersIndexPageVM $
:% &
	ModelBase' 0
{ 
public 
BrandWidgetVM 
Brands #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public #
MostPopularBikeWidgetVM &
PopularBikes' 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public 
NewLaunchedWidgetVM "
NewLaunches# .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public !
UpcomingBikesWidgetVM $
Upcoming% -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public  
PopularComparisonsVM #"
ComparePopularScooters$ :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
RecentNewsVM 
News  
{! "
get# &
;& '
set( +
;+ ,
}- .
public !
RecentExpertReviewsVM $
ExpertReviews% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
RecentVideosVM 
Videos $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
bool 
HasPopularBikes #
{$ %
get& )
{* +
return, 2
(3 4
PopularBikes4 @
!=A C
nullD H
&&I K
PopularBikesL X
.X Y
BikesY ^
!=_ a
nullb f
&&g i
PopularBikesj v
.v w
Bikesw |
.| }
Count	} Ç
(
Ç É
)
É Ñ
>
Ö Ü
$num
á à
)
à â
;
â ä
}
ã å
}
ç é
public 
bool 
HasNewLaunches "
{# $
get% (
{) *
return+ 1
(2 3
NewLaunches3 >
!=? A
nullB F
&&G I
NewLaunchesJ U
.U V
BikesV [
!=\ ^
null_ c
&&d f
NewLaunchesg r
.r s
Bikess x
.x y
County ~
(~ 
)	 Ä
>
Å Ç
$num
É Ñ
)
Ñ Ö
;
Ö Ü
}
á à
}
â ä
public 
bool 
HasUpcoming 
{  !
get" %
{& '
return( .
(/ 0
Upcoming0 8
!=9 ;
null< @
&&A C
UpcomingD L
.L M
UpcomingBikesM Z
!=[ ]
null^ b
&&c e
Upcomingf n
.n o
UpcomingBikeso |
.| }
Count	} Ç
(
Ç É
)
É Ñ
>
Ö Ü
$num
á à
)
à â
;
â ä
}
ã å
}
ç é
public 
bool 
HasComparison !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
uint 
TabCount 
= 
$num  
;  !
public 
bool 
IsNewsActive  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
bool  
IsExpertReviewActive (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
bool 
IsVideoActive !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} ß(
BD:\work\bikewaleweb\Bikewale.Models\Scooters\ScootersMakePageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
ScootersMakePageVM #
:$ %
	ModelBase& /
{ 
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public !
BikeDescriptionEntity $
Description% 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public 
IEnumerable 
<  
MostPopularBikesBase /
>/ 0
Scooters1 9
{: ;
get< ?
;? @
setA D
;D E
}F G
public !
UpcomingBikesWidgetVM $
UpcomingScooters% 5
{6 7
get8 ;
;; <
set= @
;@ A
}B C
public  
PopularComparisonsVM #"
SimilarCompareScooters$ :
{; <
get= @
;@ A
setB E
;E F
}G H
public .
"DealersServiceCentersIndiaWidgetVM 1 
DealersServiceCenter2 F
{G H
getI L
;L M
setN Q
;Q R
}S T
public (
ServiceCenterDetailsWidgetVM +
ServiceCenters, :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
DealerCardVM 
Dealers #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
OtherBrands/ :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
RecentNewsVM 
News  
{! "
get# &
;& '
set( +
;+ ,
}- .
public !
RecentExpertReviewsVM $
ExpertReviews% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public   
RecentVideosVM   
Videos   $
{  % &
get  ' *
;  * +
set  , /
;  / 0
}  1 2
public!! 
string!! 
DealerServiceTitle!! (
{!!) *
get!!+ .
;!!. /
set!!0 3
;!!3 4
}!!5 6
public"" 
string"" 
LocationMasking"" %
{""& '
get""( +
;""+ ,
set""- 0
;""0 1
}""2 3
public## 
string## 
Location## 
{##  
get##! $
;##$ %
set##& )
;##) *
}##+ ,
public$$ 
ushort$$ 
	PageCatId$$ 
{$$  !
get$$" %
;$$% &
set$$' *
;$$* +
}$$, -
public%% 
bool%% "
IsScooterDataAvailable%% *
{%%+ ,
get%%- 0
;%%0 1
set%%2 5
;%%5 6
}%%7 8
public&& 
bool&& "
IsCompareDataAvailable&& *
{&&+ ,
get&&- 0
;&&0 1
set&&2 5
;&&5 6
}&&7 8
public'' 
bool'' $
IsUpComingBikesAvailable'' ,
{''- .
get''/ 2
;''2 3
set''4 7
;''7 8
}''9 :
public(( 
bool(( 
IsNewsAvailable(( #
{(($ %
get((& )
;(() *
set((+ .
;((. /
}((0 1
public)) 
bool)) $
IsExpertReviewsAvailable)) ,
{))- .
get))/ 2
;))2 3
set))4 7
;))7 8
}))9 :
public** 
bool** 
IsVideosAvailable** %
{**& '
get**( +
;**+ ,
set**- 0
;**0 1
}**2 3
public++ 
bool++ /
#IsDealerServiceDataInIndiaAvailable++ 7
{++8 9
get++: =
;++= >
set++? B
;++B C
}++D E
public,, 
bool,, (
IsDealerServiceDataAvailable,, 0
{,,1 2
get,,3 6
;,,6 7
set,,8 ;
;,,; <
},,= >
public-- 
bool-- 
IsDealerAvailable-- %
{--& '
get--( +
;--+ ,
set--- 0
;--0 1
}--2 3
public.. 
bool.. "
IsServiceDataAvailable.. *
{..+ ,
get..- 0
;..0 1
set..2 5
;..5 6
}..7 8
public// 
bool// #
IsMakeTabsDataAvailable// +
{//, -
get//. 1
;//1 2
set//3 6
;//6 7
}//8 9
public11 
string11 
ScooterNewsUrl11 $
{11% &
get11' *
;11* +
set11, /
;11/ 0
}111 2
}44 
}55 ˘
MD:\work\bikewaleweb\Bikewale.Models\ServiceCenters\ServiceCenterCityPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
ServiceCenters (
{ 
public 

class #
ServiceCenterCityPageVM (
:) *
	ModelBase+ 4
{ 
public		 
ServiceCenterData		  $
ServiceCentersListObject		! 9
{		: ;
get		< ?
;		? @
set		A D
;		D E
}		F G
public

 
BikeMakeEntityBase

 !
Make

" &
{

' (
get

) ,
;

, -
set

. 1
;

1 2
}

3 4
public 
CityEntityBase 
City "
{# $
get% (
;( )
set* -
;- .
}/ 0
public ,
 ServiceCentersNearByCityWidgetVM / 
NearByCityWidgetData0 D
{E F
getG J
;J K
setL O
;O P
}Q R
public #
MostPopularBikeWidgetVM &
PopularWidgetData' 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public "
UsedBikeModelsWidgetVM %
UsedBikesByMakeList& 9
{: ;
get< ?
;? @
setA D
;D E
}F G
public 
DealerCardVM 
DealersWidgetData -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
BrandCityPopupVM  
BrandCityPopupWidget  4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
} 
} º
PD:\work\bikewaleweb\Bikewale.Models\ServiceCenters\ServiceCenterDetailsPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
ServiceCenters (
{ 
public 

class &
ServiceCenterDetailsPageVM +
:, -
	ModelBase. 7
{		 
public

 "
UsedBikeModelsWidgetVM

 %
UsedBikesByMakeList

& 9
{

: ;
get

< ?
;

? @
set

A D
;

D E
}

F G
public 
DealerCardVM 
DealersWidgetData -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public #
MostPopularBikeWidgetVM &
PopularWidgetData' 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public %
ServiceCenterCompleteData (
ServiceCenterData) :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
CityEntityBase 
City "
{# $
get% (
;( )
set* -
;- .
}/ 0
public (
ServiceCenterDetailsWidgetVM +)
OtherServiceCentersWidgetData, I
{J K
getL O
;O P
setQ T
;T U
}V W
public 
IEnumerable 
<  
ModelServiceSchedule /
>/ 0
BikeScheduleList1 A
{B C
getD G
;G H
setI L
;L M
}N O
} 
} á
RD:\work\bikewaleweb\Bikewale.Models\ServiceCenters\ServiceCenterDetailsWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
ServiceCenters (
{ 
public

 

class

 (
ServiceCenterDetailsWidgetVM

 -
{ 
public 
IEnumerable 
<  
ServiceCenterDetails /
>/ 0
ServiceCentersList1 C
{D E
getF I
;I J
setK N
;N O
}P Q
public 
string 
MakeMaskingName %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
CityMaskingName %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
} 
} ®

ND:\work\bikewaleweb\Bikewale.Models\ServiceCenters\ServiceCenterIndiaPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
ServiceCenters (
{ 
public 

class $
ServiceCenterIndiaPageVM )
:* +
	ModelBase, 5
{ 
public 
IEnumerable 
< 
BrandServiceCenters .
>. /#
ServiceCenterBrandsList0 G
{H I
getJ M
;M N
setO R
;R S
}T U
public "
UsedBikeModelsWidgetVM %
UsedBikesByMakeList& 9
{: ;
get< ?
;? @
setA D
;D E
}F G
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public $
ServiceCenterLocatorList '"
ServiceCentersCityList( >
{? @
getA D
;D E
setF I
;I J
}K L
public 
RecentBikeCareVM 
BikeCareWidgetVM  0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
} 
} ∫
PD:\work\bikewaleweb\Bikewale.Models\ServiceCenters\ServiceCenterLandingPageVM.cs
	namespace 	
Bikewale
 
. 
ServiceCenters !
{ 
public		 

class		 &
ServiceCenterLandingPageVM		 +
:		, -
	ModelBase		. 7
{

 
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
	MakesList/ 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public 
BrandWidgetVM 
Brands #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
RecentBikeCareVM 
BikeCareWidgetVM  0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public 
NewLaunchedWidgetVM "
NewLaunchWidgetData# 6
{7 8
get9 <
;< =
set> A
;A B
}C D
public !
UpcomingBikesWidgetVM $
UpcomingWidgetData% 7
{8 9
get: =
;= >
set? B
;B C
}D E
public #
MostPopularBikeWidgetVM &
PopularWidgetData' 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public "
UsedBikeModelsWidgetVM %$
UsedBikesModelWidgetData& >
{? @
getA D
;D E
setF I
;I J
}K L
public "
UsedBikeCitiesWidgetVM %#
UsedBikesCityWidgetData& =
{> ?
get@ C
;C D
setE H
;H I
}J K
public 
CityEntityBase 
City "
{# $
get% (
;( )
set* -
;- .
}/ 0
} 
} ¯
VD:\work\bikewaleweb\Bikewale.Models\ServiceCenters\ServiceCentersNearByCityWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
ServiceCenters (
{ 
public 

class ,
 ServiceCentersNearByCityWidgetVM 1
{ 
public 
IEnumerable 
< #
CityBrandServiceCenters 2
>2 3&
ServiceCentersNearbyCities4 N
{O P
getQ T
;T U
setV Y
;Y Z
}[ \
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} ì	
6D:\work\bikewaleweb\Bikewale.Models\Shared\BikeInfo.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Shared  
{ 
public 

class 
BikeInfo 
{ 
public 
string 
Url 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
Bike 
{ 
get  
;  !
set" %
;% &
}' (
public 
int 
PQSource 
{ 
get !
;! "
set# &
;& '
}( )
public		 
uint		 
ModelId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
public

 
Bikewale

 
.

 
Entities

  
.

  !
GenericBikes

! -
.

- .
GenericBikeInfo

. =
Info

> B
{

C D
get

E H
;

H I
set

J M
;

M N
}

O P
} 
} ˆ
4D:\work\bikewaleweb\Bikewale.Models\Shared\Brands.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Shared  
{ 
public 

class 
BrandWidget 
{ 
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
	TopBrands/ 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
OtherBrands/ :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
bool 
HasOtherBrands "
{# $
get% (
{) *
return+ 1
(2 3
OtherBrands3 >
!=? A
nullB F
&&G I
OtherBrandsJ U
.U V
CountV [
([ \
)\ ]
>^ _
$num` a
)a b
;b c
}d e
}f g
} 
} °
CD:\work\bikewaleweb\Bikewale.Models\Shared\EditCMSPhotoGalleryVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 !
EditCMSPhotoGalleryVM

 &
{ 
public 
IEnumerable 
< 

ModelImage %
>% &
Images' -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
int 

ImageCount 
{ 
get "
;" #
set# &
;& '
}' (
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} ñ
3D:\work\bikewaleweb\Bikewale.Models\Shared\Pager.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Shared  
{ 
public 

class 
Pager 
{ 
public 
int 

TotalPages 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
CurrentPageNo  
{! "
get# &
;& '
set( +
;+ ,
}- .
public		 
string		 
MakeId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
public

 
uint

 
CityId

 
{

 
get

  
;

  !
set

" %
;

% &
}

' (
public 
string 
ModelId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 

StartIndex 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
EndIndex 
{ 
get !
;! "
set# &
;& '
}( )
public 
PagerOutputEntity  
PagerOutput! ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
bool 
ShowHash 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} —
AD:\work\bikewaleweb\Bikewale.Models\Shared\PopularScootersList.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Shared  
{		 
public 

class 
PopularScootersList $
{ 
public 
int 
	PageCatId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 

PQSourceId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
IEnumerable 
<  
MostPopularBikesBase /
>/ 0
PopularScooters1 @
{A B
getC F
;F G
setH K
;K L
}M N
} 
} œ
7D:\work\bikewaleweb\Bikewale.Models\Shared\Showrooms.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Shared  
{ 
public

 

class

 
ShowroomsVM

 
{ 
public 
string 
Title 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
MakeMaskingName %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
CityName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
CityMaskingName %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
DealersEntity 
dealers $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public &
PopularDealerServiceCenter )
dealerServiceCenter* =
{> ?
get@ C
;C D
setE H
;H I
}J K
public 
bool 
IsDataAvailable #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} 
} »
;D:\work\bikewaleweb\Bikewale.Models\Survey\BajajSurveyVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Survey  
{ 
public		 

class		 
BajajSurveyVM		 
:		  
	ModelBase		! *
{

 
public 
string 
CurrentBike !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
BikeToPurchase $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
string  
RecentBikeCommercial *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
string 

SeenThisAd  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 

viewscount  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
	AllMedium 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
Age 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
Handset 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
City 
{ 
get  
;  !
set" %
;% &
}' (
public 
bool 
IsSubmitted 
{  !
get" %
;% &
set' *
;* +
}, -
public 
bool 
IsMobile 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
Source 
{ 
get "
;" #
set$ '
;' (
}) *
public 
List 
< 
string 
> 
MultipleModel )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
List 
< 
string 
> 
AdMedium $
{% &
get' *
;* +
set, /
;/ 0
}1 2
} 
} ›
ED:\work\bikewaleweb\Bikewale.Models\Upcoming\UpcomingBikesWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 !
UpcomingBikesWidgetVM

 &
{ 
public 
IEnumerable 
< 
UpcomingBikeEntity -
>- .
UpcomingBikes/ <
{= >
get? B
;B C
setD G
;G H
}I J
public 
string 
WidgetHeading #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 

WidgetHref  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
WidgetLinkTitle %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
} 
} œ
>D:\work\bikewaleweb\Bikewale.Models\Upcoming\UpcomingPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
UpcomingPageVM 
:  !
	ModelBase" +
{ 
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
BrandWidgetVM 
Brands #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
NewLaunchedWidgetVM "
NewLaunches# .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public #
MostPopularBikeWidgetVM &
PopularBikes' 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public 
IEnumerable 
< 
UpcomingBikeEntity -
>- .
UpcomingBikeModels/ A
{B C
getD G
;G H
setI L
;L M
}N O
public 
OtherMakesVM 

OtherMakes &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
HasBikes 
{ 
get "
;" #
set$ '
;' (
}) *
public 
uint 

TotalBikes 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
IEnumerable 
< 
int 
> 
	YearsList  )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
	MakesList/ 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public 
PagerEntity 
Pager  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
bool 
HasPages 
{ 
get "
{# $
return% +
(, -
Pager- 2
!=3 5
null6 :
&&; =
Pager> C
.C D
TotalResultsD P
>Q R
$numS T
)T U
;U V
}W X
}Y Z
} 
} ¶
QD:\work\bikewaleweb\Bikewale.Models\UserReviews\UserReviewSimilarBikesWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
UserReviews %
{ 
public		 

class		 *
UserReviewSimilarBikesWidgetVM		 /
{

 
public 
IEnumerable 
< 
Bikewale #
.# $
Entities$ ,
., -!
SimilarBikeUserReview- B
>B C
SimilarBikesD P
{Q R
getS V
;V W
setX [
;[ \
}] ^
public 
string 
GlobalCityName $
{% &
get' *
;* +
set, /
;/ 0
}1 2
} 
} ˚
FD:\work\bikewaleweb\Bikewale.Models\UserReviews\UserReviewListingVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
UserReviews %
{ 
public 

class 
UserReviewListingVM $
:% &
	ModelBase' 0
{ 
public 
uint 
ModelId 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public "
BikeRatingsReviewsInfo %
RatingReviewData& 6
{7 8
get9 <
;< =
set> A
;A B
}C D
public 
BikeRatingsInfo 
RatingsInfo *
{+ ,
get- 0
{1 2
return3 9
(: ;
RatingReviewData; K
!=L N
nullO S
?T U
RatingReviewDataV f
.f g
RatingDetailsg t
:u v
nullw {
){ |
;| }
}~ 
}
Ä Å
public 
BikeReviewsInfo 
ReviewsInfo *
{+ ,
get- 0
{1 2
return3 9
(: ;
RatingReviewData; K
!=L N
nullO S
?T U
RatingReviewDataV f
.f g
ReviewDetailsg t
:u v
nullw {
){ |
;| }
}~ 
}
Ä Å
public 
bool 
IsRatingsAvailable &
{' (
get) ,
{- .
return/ 5
RatingsInfo6 A
!=B D
nullE I
;I J
}K L
}M N
public 
bool 
IsReviewsAvailable &
{' (
get) ,
{- .
return/ 5
ReviewsInfo6 A
!=B D
nullE I
&&J L
ReviewsInfoM X
.X Y
TotalReviewsY e
>f g
$numh i
;i j
}k l
}m n
public 
UserReviewsSearchVM "
UserReviews# .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public !
RecentExpertReviewsVM $
ExpertReviews% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public *
UserReviewSimilarBikesWidgetVM -
SimilarBikesWidget. @
{A B
getC F
;F G
setH K
;K L
}M N
public 
string 
PageUrl 
{ 
get  #
;# $
set% (
;( )
}* +
} 
} ˘
FD:\work\bikewaleweb\Bikewale.Models\UserReviews\UserReviewDetailsVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
UserReviewDetailsVM $
:% &
	ModelBase' 0
{ 
public 
UserReviewSummary   
UserReviewDetailsObj! 5
{6 7
get8 ;
;; <
set= @
;@ A
}B C
public 
ICollection 
< 
UserReviewQuestion -
>- .
RatingQuestions/ >
{? @
getA D
;D E
setF I
;I J
}K L
public 
ICollection 
< 
UserReviewQuestion -
>- .
ReviewQuestions/ >
{? @
getA D
;D E
setF I
;I J
}K L
public 
uint 
RatingQuestionCount '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 

BikeInfoVM !
GenericBikeWidgetData /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
public !
RecentExpertReviewsVM $
ExpertReviews% 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public *
UserReviewSimilarBikesWidgetVM -
SimilarBikesWidget. @
{A B
getC F
;F G
setH K
;K L
}M N
public 
uint 
ReviewId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
UserReviewsSearchVM "
UserReviews# .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
string 
PageUrl 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
	ReviewAge 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} €
PD:\work\bikewaleweb\Bikewale.Models\UserReviews\UserReviewsOtherDetailsPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
UserReviews %
{ 
public 

class )
UserReviewsOtherDetailsPageVM .
:/ 0
	ModelBase1 :
{ 
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
BikeModelEntityBase "
Model# (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public #
UserReviewOverallRating &
Rating' -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
uint 
ReviewId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
IEnumerable 
< 
UserReviewQuestion -
>- .
QuestionsList/ <
{= >
get? B
;B C
setD G
;G H
}I J
public 
uint 
PageSourceId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 

ContestSrc 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
EmailId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
UserName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
ulong 

CustomerId 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
PreviousPageUrl %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
EncodedWriteUrl %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
	ReturnUrl 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} €
FD:\work\bikewaleweb\Bikewale.Models\UserReviews\UserReviewsSearchVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
UserReviews %
{ 
public 

class 
UserReviewsSearchVM $
{ 
public		 
BikeReviewsInfo		 
ReviewsInfo		 *
{		+ ,
get		- 0
;		0 1
set		2 5
;		5 6
}		7 8
public

 
uint

 
ModelId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

( )
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
SearchResult 
UserReviews '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
uint 
PageSize 
{ 
get "
;" #
set$ '
;' (
}) *
public 
PagerEntity 
Pager  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
FilterBy  
ActiveReviewCategory ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
string 
WriteReviewLink %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
private 
bool 
_IsPagerNeeded #
=$ %
true& *
;* +
public 
bool 
IsPagerNeeded !
{ 	
get 
{ 
return 
_IsPagerNeeded '
;' (
}) *
set 
{ 
_IsPagerNeeded  
=! "
value# (
;( )
}* +
} 	
public 
string 
WidgetHeading #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public '
QuestionsRatingValueByModel *
ObjQuestionValue+ ;
{< =
get> A
;A B
setC F
;F G
}H I
} 
} —
FD:\work\bikewaleweb\Bikewale.Models\UserReviews\UserReviewSummaryVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 
UserReviewSummaryVM

 $
:

% &
	ModelBase

' 0
{ 
public 
UserReviewSummary  
Summary! (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
string 
WriteReviewLink %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
IEnumerable 
< 
UserReviewQuestion -
>- .
RatingQuestions/ >
{? @
getA D
;D E
setF I
;I J
}K L
public 
IEnumerable 
< 
UserReviewQuestion -
>- .
ReviewQuestions/ >
{? @
getA D
;D E
setF I
;I J
}K L
} 
} œ
ED:\work\bikewaleweb\Bikewale.Models\UserReviews\WriteReviewContest.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class  
WriteReviewContestVM %
:& '
	ModelBase( 1
{ 
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
Makes/ 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public 
string 
QueryString !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
IEnumerable 
< 
RecentReviewsWidget .
>. /
UserReviewsWinners0 B
{C D
getE H
;H I
setJ M
;M N
}O P
public 
UserReviewPopupVM  
UserReviewPopup! 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
} 
} ≤
PD:\work\bikewaleweb\Bikewale.Models\UserReviews\WriteReviewPageSubmitResponse.cs
	namespace 	
Bikewale
 
. 
Models 
. 
UserReviews %
{ 
public 

class )
WriteReviewPageSubmitResponse .
{		 
public

 
string

 
TitleErrorText

 $
{

% &
get

' *
;

* +
set

, /
;

/ 0
}

1 2
public 
string 
ReviewErrorText %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
bool 
	IsSuccess 
{ 
get  #
;# $
set% (
;( )
}* +
} 
} Ú
CD:\work\bikewaleweb\Bikewale.Models\UserReview\WriteReviewPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
WriteReviewPageVM "
:# $
	ModelBase% .
{ 
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
BikeModelEntityBase "
Model# (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public #
UserReviewOverallRating &
Rating' -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
uint 
ReviewId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
IEnumerable 
< 
UserReviewQuestion -
>- .
QuestionsList/ <
{= >
get? B
;B C
setD G
;G H
}I J
public 
string 
Tips 
{ 
get  
;  !
set" %
;% &
}' (
public 
bool 
	HasReview 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
JsonQuestionList &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
HostUrl 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
OriginalImagePath '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
ulong 

CustomerId 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
PreviousPageUrl %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
EncodedWriteUrl %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
EmailId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
UserName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public )
WriteReviewPageSubmitResponse ,
SubmitResponse- ;
{< =
get> A
;A B
setC F
;F G
}H I
public   
string   
JsonReviewSummary   '
{  ( )
get  * -
;  - .
set  / 2
;  2 3
}  4 5
public!! 
uint!! 
PageSourceId!!  
{!!! "
get!!# &
;!!& '
set!!( +
;!!+ ,
}!!- .
public"" 
int"" 

ContestSrc"" 
{"" 
get""  #
;""# $
set""% (
;""( )
}""* +
}## 
}$$ ú
ED:\work\bikewaleweb\Bikewale.Models\UserReviews\UserReviewRatingVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
UserReviewRatingVM #
:$ %
	ModelBase& /
{ 
public 
BikeModelEntity 
objModelEntity -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
string 
OverAllRatingText '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
string 
RatingQuestion $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
string 
ErrorMessage "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
uint 
PriceRangeId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string  
ReviewsOverAllrating *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
string 
CustomerName "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
CustomerEmail #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
uint 
ReviewId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
bool 
IsFake 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
SelectedRating "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
	ReturnUrl 
{  !
get" %
;% &
set' *
;* +
}, -
public 
ushort 
SourceId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
int 

ContestSrc 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
UtmzCookieValue %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
} 
} Ã
>D:\work\bikewaleweb\Bikewale.Models\Videos\MakeVideosPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
MakeVideosPageVM !
:" #
	ModelBase$ -
{ 
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
IEnumerable 
<  
BikeVideoModelEntity /
>/ 0
Videos1 7
{8 9
get: =
;= >
set? B
;B C
}D E
} 
} ≥
BD:\work\bikewaleweb\Bikewale.Models\Videos\ModelWiseVideoPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Videos  
{ 
public 

class  
ModelWiseVideoPageVM %
:& '
	ModelBase( 1
{ 
public		 
BikeMakeEntityBase		 !
Make		" &
{		' (
get		) ,
;		, -
set		. 1
;		1 2
}		3 4
public

 
BikeModelEntityBase

 "
Model

# (
{

) *
get

+ .
;

. /
set

0 3
;

3 4
}

5 6
public 
IEnumerable 
< 
BikeVideoEntity *
>* +

VideosList, 6
{7 8
get9 <
;< =
set> A
;A B
}C D
public 

BikeInfoVM 
BikeInfoWidgetData ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
IEnumerable 
<  
SimilarBikeWithVideo /
>/ 0 
SimilarBikeVideoList1 E
{F G
getH K
;K L
setM P
;P Q
}R S
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
} 
} »
=D:\work\bikewaleweb\Bikewale.Models\Videos\ScooterVideosVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Videos  
{		 
public 

class 
ScooterVideosVM  
:! "
	ModelBase# ,
{ 
public 
IEnumerable 
< 
BikeVideoEntity *
>* +

VideosList, 6
{7 8
get9 <
;< =
set> A
;A B
}C D
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
} 
} ®
BD:\work\bikewaleweb\Bikewale.Models\Videos\SimilarVideoModelsVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

  
SimilarVideoModelsVM

 %
{ 
public 
uint 
ModelId 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
	ModelName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
IEnumerable 
< 
BikeVideoEntity *
>* +
Videos, 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
string 
ViewAllLinkText %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
ViewAllLinkUrl $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
string 
ViewAllLinkTitle &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} ◊
@D:\work\bikewaleweb\Bikewale.Models\Videos\VideoDetailsPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public		 

class		 
VideoDetailsPageVM		 #
:		$ %
	ModelBase		& /
{

 
public 
BikeVideoEntity 
Video $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
DisplayDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
bool 
IsBikeTagged  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
TaggedBikeName $
{% &
get' *
{+ ,
return- 3
IsBikeTagged4 @
?A B
stringC I
.I J
FormatJ P
(P Q
$strQ Z
,Z [
Video\ a
.a b
MakeNameb j
,j k
Videol q
.q r
	ModelNamer {
){ |
:} ~
$str	 Å
;
Å Ç
}
É Ñ
}
Ö Ü
public 
uint 
VideoId 
{ 
get !
;! "
set# &
;& '
}( )
public 
uint 
TaggedModelId !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
SmallDescription &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
IsSmallDescription &
{' (
get) ,
{- .
return/ 5
Video6 ;
.; <
Description< G
.G H
LengthH N
>O P
$numQ T
;T U
}V W
}X Y
} 
} Ñ	
DD:\work\bikewaleweb\Bikewale.Models\Videos\VideosByCategoryPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Videos  
{ 
public 

class "
VideosByCategoryPageVM '
:( )
	ModelBase* 3
{ 
public  
BikeVideosListEntity #
Videos$ *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
string 
CanonicalTitle $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public		 
string		 
	TitleName		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		, -
public

 
string

 
PageHeading

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

. /
public 
string 

CategoryId  
{! "
get# &
;& '
set( +
;+ ,
}- .
} 
} ˘
CD:\work\bikewaleweb\Bikewale.Models\Videos\VideosBySubcategoryVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Videos  
{ 
public		 

class		 !
VideosBySubcategoryVM		 &
{

 
public  
BikeVideosListEntity #
	VideoList$ -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
string 
SectionTitle "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
CategoryIdList $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
EnumVideosCategory !

CategoryId" ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
string "
SectionBackgroundClass ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
} 
} ü
BD:\work\bikewaleweb\Bikewale.Models\Used\UsedBikeCitiesWidgetVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 "
UsedBikeCitiesWidgetVM

 '
{ 
public 
string 
Title 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
Href 
{ 
get  
;  !
set" %
;% &
}' (
public 
IEnumerable 
< 
UsedBikeCities )
>) *
Cities+ 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
} 
} É	
AD:\work\bikewaleweb\Bikewale.Models\Used\UsedBikeByModelCityVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class !
UsedBikeByModelCityVM &
{ 
public 
IEnumerable 
< 
MostRecentBikes *
>* +
RecentUsedBikesList, ?
{@ A
getB E
;E F
setG J
;J K
}L M
public 
string 
LinkHeading !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
BikeMakeEntityBase !
Make" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
BikeModelEntityBase "
Model# (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
CityEntityBase 
City "
{# $
get% (
;( )
set* -
;- .
}/ 0
} 
} ¯
=D:\work\bikewaleweb\Bikewale.Models\Used\UsedBikeByModelVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class 
UsedBikeModelsVM !
{ 
public		 
IEnumerable		 
<		 
MostRecentBikes		 *
>		* +
UsedBikeModelList		, =
;		= >
public

 
CityEntityBase

 
CityDetails

 )
;

) *
} 
} ˘
8D:\work\bikewaleweb\Bikewale.Models\Used\UsedModelsVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public 

class "
UsedBikeModelsWidgetVM '
{ 
public 
IEnumerable 
< 
MostRecentBikes *
>* +
UsedBikeModelList, =
;= >
public 
CityEntityBase 
CityDetails )
;) *
} 
} ”
<D:\work\bikewaleweb\Bikewale.Models\Videos\RecentVideosVM.cs
	namespace 	
Bikewale
 
. 
Models 
{ 
public

 

class

 
RecentVideosVM

 
{ 
public 
IEnumerable 
< 
BikeVideoEntity *
>* +

VideosList, 6
{7 8
get9 <
;< =
set> A
;A B
}C D
public 
string 
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
	ModelName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
MakeMasking !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
ModelMasking "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
	LinkTitle 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
MoreVideoUrl "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
int 
FetchedCount 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} ß
AD:\work\bikewaleweb\Bikewale.Models\Videos\VideosLandingPageVM.cs
	namespace 	
Bikewale
 
. 
Models 
. 
Videos  
{ 
public		 

class		 
VideosLandingPageVM		 $
:		% &
	ModelBase		' 0
{

 
public 
BikeVideoEntity !
LandingFirstVideoData 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public 
IEnumerable 
< 
BikeVideoEntity *
>* +"
LandingOtherVideosData, B
{C D
getE H
;H I
setJ M
;M N
}O P
public 
BrandWidgetVM 
Brands #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public !
VideosBySubcategoryVM $#
ExpertReviewsWidgetData% <
{= >
get? B
;B C
setD G
;G H
}I J
public !
VideosBySubcategoryVM $
FirstRideWidgetData% 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public !
VideosBySubcategoryVM $!
LaunchAlertWidgetData% :
{; <
get= @
;@ A
setB E
;E F
}G H
public !
VideosBySubcategoryVM $
FirstLookWidgetData% 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public !
VideosBySubcategoryVM $+
PowerDriftBlockbusterWidgetData% D
{E F
getG J
;J K
setL O
;O P
}Q R
public !
VideosBySubcategoryVM $!
MotorSportsWidgetData% :
{; <
get= @
;@ A
setB E
;E F
}G H
public !
VideosBySubcategoryVM $(
PowerDriftSpecialsWidgetData% A
{B C
getD G
;G H
setI L
;L M
}N O
public !
VideosBySubcategoryVM $(
PowerDriftTopMusicWidgetData% A
{B C
getD G
;G H
setI L
;L M
}N O
public !
VideosBySubcategoryVM $#
MiscellaneousWidgetData% <
{= >
get? B
;B C
setD G
;G H
}I J
} 
} 