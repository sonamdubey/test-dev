�
0D:\work\bikewaleweb\BikewaleOpr.BAL\BikeMakes.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
{		 
public 

class 
	BikeMakes 
: 

IBikeMakes &
{ 
private 
readonly  
IBikeMakesRepository - 
_bikeMakesRepository. B
;B C
public 
	BikeMakes 
(  
IBikeMakesRepository -
bikeMakesRepository. A
)A B
{ 	 
_bikeMakesRepository  
=! "
bikeMakesRepository# 6
;6 7
} 	
public 
IEnumerable 
< 
BikeModelEntityBase .
>. /
GetModelsByMake0 ?
(? @
EnumBikeType@ L
requestTypeM X
,X Y
uintZ ^
makeId_ e
)e f
{ 	
IEnumerable 
< 
BikeModelEntityBase +
>+ ,&
objBikeModelEntityBaseList- G
=H I
nullJ N
;N O
try   
{!! 
if"" 
("" 
makeId"" 
>"" 
$num"" 
)"" 
{## &
objBikeModelEntityBaseList$$ .
=$$/ 0 
_bikeMakesRepository$$1 E
.$$E F
GetModelsByMake$$F U
($$U V
requestType$$V a
,$$a b
makeId$$c i
)$$i j
;$$j k
}%% 
}&& 
catch'' 
('' 
	Exception'' 
ex'' 
)''  
{(( 

ErrorClass)) 
objErr))  
=))! "
new))# &

ErrorClass))' 1
())1 2
ex))2 4
,))4 5
$str))6 a
)))a b
;))b c
}** 
return++ &
objBikeModelEntityBaseList++ -
;++- .
},, 	
public.. 
IEnumerable.. 
<.. 
BikeMakeEntityBase.. -
>..- .
GetMakes../ 7
(..7 8
ushort..8 >
requestType..? J
)..J K
{// 	
IEnumerable00 
<00 
BikeMakeEntityBase00 *
>00* +
objMakes00, 4
=005 6
null007 ;
;00; <
try11 
{22 
objMakes33 
=33  
_bikeMakesRepository33 /
.33/ 0
GetMakes330 8
(338 9
requestType339 D
)33D E
;33E F
}44 
catch55 
(55 
	Exception55 
ex55 
)55  
{66 

ErrorClass77 
objErr77 !
=77" #
new77$ '

ErrorClass77( 2
(772 3
ex773 5
,775 6
$str777 [
)77[ \
;77\ ]
}88 
return99 
objMakes99 
;99 
}:: 	
};; 
}<< �1
1D:\work\bikewaleweb\BikewaleOpr.BAL\BikeModels.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
{ 
public 

class 

BikeModels 
: 
IBikeModels )
{ 
private 
readonly !
IBikeModelsRepository .
_IBikeModel/ :
;: ;
public 

BikeModels 
( !
IBikeModelsRepository /
	bikeModel0 9
)9 :
{ 	
_IBikeModel 
= 
	bikeModel #
;# $
} 	
public 0
$UsedBikeImagesByMakeNotificationData 30
$GetPendingUsedBikesWithoutModelImage4 X
(X Y
)Y Z
{ 	0
$UsedBikeImagesByMakeNotificationData 0)
objBikeByMakeNotificationData1 N
=O P
newQ T0
$UsedBikeImagesByMakeNotificationDataU y
(y z
)z {
;{ |
IEnumerable 
< 
UsedModelsByMake (
>( )
objBikesByMake* 8
=9 :
null; ?
;? @
try 
{   *
UsedBikeImagesNotificationData!! .(
usedBikeNotificationDataList!!/ K
=!!L M
_IBikeModel!!N Y
.!!Y Z0
$GetPendingUsedBikesWithoutModelImage!!Z ~
(!!~ 
)	!! �
;
!!� �
objBikesByMake"" 
=""  
new""! $
List""% )
<"") *
UsedModelsByMake""* :
>"": ;
(""; <
)""< =
;""= >
if## 
(## (
usedBikeNotificationDataList## 0
!=##1 3
null##4 8
&&##9 ;(
usedBikeNotificationDataList##< X
.##X Y
Bikes##Y ^
!=##_ a
null##b f
)##f g
{$$ 
var%% 
grpMakes%%  
=%%! "(
usedBikeNotificationDataList%%# ?
.%%? @
Bikes%%@ E
.&& 
GroupBy&&  
(&&  !
m&&! "
=>&&# %
m&&& '
.&&' (
MakeId&&( .
)&&. /
;&&/ 0
if(( 
((( 
grpMakes((  
!=((! #
null(($ (
)((( )
{)) 
objBikesByMake** &
=**' (
grpMakes**) 1
.++( )
Select++) /
(++/ 0
m++0 1
=>++2 4
new++5 8
UsedModelsByMake++9 I
(++I J
)++J K
{,,( )
MakeId--, 2
=--3 4
m--5 6
.--6 7
FirstOrDefault--7 E
(--E F
)--F G
.--G H
MakeId--H N
,--N O
MakeName.., 4
=..5 6
m..7 8
...8 9
FirstOrDefault..9 G
(..G H
)..H I
...I J
MakeName..J R
,..R S
	ModelList//, 5
=//6 7
m//8 9
.//9 :
Select//: @
(//@ A
x//A B
=>//C E
x//F G
.//G H
	ModelName//H Q
)//Q R
}00( )
)00) *
;00* +
}11 )
objBikeByMakeNotificationData33 1
.331 2
BikesByMake332 =
=33> ?
objBikesByMake33@ N
;33N O)
objBikeByMakeNotificationData44 1
.441 2
IsNotify442 :
=44; <(
usedBikeNotificationDataList44= Y
.44Y Z
IsNotify44Z b
;44b c
}55 
}66 
catch77 
(77 
	Exception77 
ex77 
)77  
{88 
Bikewale99 
.99 
Notifications99 &
.99& '

ErrorClass99' 1
objErr992 8
=999 :
new99; >
Bikewale99? G
.99G H
Notifications99H U
.99U V

ErrorClass99V `
(99` a
ex99a c
,99c d
$str	99e �
)
99� �
;
99� �
}:: 
return;; )
objBikeByMakeNotificationData;; 0
;;;0 1
}<< 	
publicCC 
IEnumerableCC 
<CC 
BikeModelsByMakeCC +
>CC+ ,*
GetModelsWithMissingColorImageCC- K
(CCK L
)CCL M
{DD 	
IEnumerableEE 
<EE 
BikeModelsByMakeEE (
>EE( )#
objBikeModelsByMakeListEE* A
=EEB C
nullEED H
;EEH I
IEnumerableFF 
<FF 
BikeMakeModelDataFF )
>FF) *
objBikeDataListFF+ :
=FF; <
nullFF= A
;FFA B
tryHH 
{II 
objBikeDataListJJ 
=JJ  !
_IBikeModelJJ" -
.JJ- .*
GetModelsWithMissingColorImageJJ. L
(JJL M
)JJM N
;JJN O
ifLL 
(LL 
objBikeDataListLL "
!=LL# %
nullLL& *
)LL* +
{MM #
objBikeModelsByMakeListOO +
=OO, -
objBikeDataListOO. =
.PP 
GroupByPP  
(PP  !
mPP! "
=>PP# %
mPP& '
.PP' (
BikeMakePP( 0
.PP0 1
MakeIdPP1 7
)PP7 8
.QQ 
SelectQQ 
(QQ  
grpQQ! $
=>QQ% '
newQQ( +
BikeModelsByMakeQQ, <
{RR 
BikeMakeEntitySS *
=SS+ ,
grpSS- 0
.SS0 1
FirstSS1 6
(SS6 7
)SS7 8
.SS8 9
BikeMakeSS9 A
,SSA B
BikeModelEntityTT +
=TT, -
grpTT. 1
.TT1 2
SelectTT2 8
(TT8 9
tTT9 :
=>TT; =
tTT> ?
.TT? @
	BikeModelTT@ I
)TTI J
}UU 
)UU 
;UU 
}VV 
}WW 
catchXX 
(XX 
	ExceptionXX 
exXX 
)XX  
{YY 
BikewaleZZ 
.ZZ 
NotificationsZZ &
.ZZ& '

ErrorClassZZ' 1
objErrZZ2 8
=ZZ9 :
newZZ; >
BikewaleZZ? G
.ZZG H
NotificationsZZH U
.ZZU V

ErrorClassZZV `
(ZZ` a
exZZa c
,ZZc d
$str	ZZe �
)
ZZ� �
;
ZZ� �
}[[ 
return\\ #
objBikeModelsByMakeList\\ *
;\\* +
}]] 	
}^^ 
}__ �F
OD:\work\bikewaleweb\BikewaleOpr.BAL\ContractCampaign\ContractCampaignMapping.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
. 
ContractCampaign *
{ 
public 

class 
ContractCampaign !
:" #
IContractCampaign$ 5
{ 
public 
IEnumerable 
< 
MaskingNumber (
>( ) 
GetAllMaskingNumbers* >
(> ?
uint? C
dealerIdD L
)L M
{ 	
IEnumerable 
< 
MaskingNumber %
>% &
_maskingNumbers' 6
=7 8
null9 =
;= >
try 
{ 
string 
api 
= 
string #
.# $
Format$ *
(* +
$str+ U
,U V
dealerIdW _
)_ `
;` a
using 
( 
Bikewale 
.  
Utility  '
.' (
BWHttpClient( 4
	objClient5 >
=? @
newA D
BikewaleE M
.M N
UtilityN U
.U V
BWHttpClientV b
(b c
)c d
)d e
{ 
_maskingNumbers #
=$ %
	objClient& /
./ 0
GetApiResponseSync0 B
<B C
IEnumerableC N
<N O
MaskingNumberO \
>\ ]
>] ^
(^ _
Bikewale_ g
.g h
Utilityh o
.o p
APIHostp w
.w x
CWSx {
,{ |
Bikewale	} �
.
� �
Utility
� �
.
� �
BWConfiguration
� �
.
� �
Instance
� �
.
� � 
APIRequestTypeJSON
� �
,
� �
api
� �
,
� �
_maskingNumbers
� �
)
� �
;
� �
} 
}   
catch!! 
(!! 
System!! 
.!! 
	Exception!! #
ex!!$ &
)!!& '
{"" 

ErrorClass## 
objErr## !
=##" #
new##$ '

ErrorClass##( 2
(##2 3
ex##3 5
,##5 6
$str##7 k
)##k l
;##l m
objErr$$ 
.$$ 
SendMail$$ 
($$  
)$$  !
;$$! "
}%% 
return&& 
_maskingNumbers&& "
;&&" #
}'' 	
public22 
bool22 

IsCCMapped22 
(22 
uint22 #
dealerId22$ ,
,22, -
uint22. 2

contractId223 =
,22= >
uint22? C

campaignId22D N
)22N O
{33 	
bool44 
_isCCMapped44 
=44 
false44 $
;44$ %
try55 
{66 
string77 
_apiUrl77 
=77  
string77! '
.77' (
Format77( .
(77. /
$str	77/ �
,
77� �
dealerId
77� �
,
77� �

contractId
77� �
,
77� �

campaignId
77� �
)
77� �
;
77� �
using99 
(99 
Bikewale99 
.99  
Utility99  '
.99' (
BWHttpClient99( 4
	objClient995 >
=99? @
new99A D
Bikewale99E M
.99M N
Utility99N U
.99U V
BWHttpClient99V b
(99b c
)99c d
)99d e
{:: 
_isCCMapped;; 
=;;  !
	objClient;;" +
.;;+ ,
PostSync;;, 4
<;;4 5
bool;;5 9
,;;9 :
bool;;; ?
>;;? @
(;;@ A
Bikewale;;A I
.;;I J
Utility;;J Q
.;;Q R
APIHost;;R Y
.;;Y Z
CWS;;Z ]
,;;] ^
Bikewale;;_ g
.;;g h
Utility;;h o
.;;o p
BWConfiguration;;p 
.	;; �
Instance
;;� �
.
;;� � 
APIRequestTypeJSON
;;� �
,
;;� �
_apiUrl
;;� �
,
;;� �
_isCCMapped
;;� �
)
;;� �
;
;;� �
}<< 
}== 
catch>> 
(>> 
System>> 
.>> 
	Exception>> #
ex>>$ &
)>>& '
{?? 

ErrorClass@@ 
objErr@@ !
=@@" #
new@@$ '

ErrorClass@@( 2
(@@2 3
ex@@3 5
,@@5 6
$str@@7 a
)@@a b
;@@b c
objErrAA 
.AA 
SendMailAA 
(AA  
)AA  !
;AA! "
}BB 
returnCC 
_isCCMappedCC 
;CC 
}DD 	
publicPP 
boolPP  
RelaseMaskingNumbersPP (
(PP( )
uintPP) -
dealerIdPP. 6
,PP6 7
intPP8 ;
userIdPP< B
,PPB C
stringPPD J
maskingNumbersPPK Y
)PPY Z
{QQ 	
boolRR 
_areNumbersReleasedRR $
=RR% &
falseRR' ,
;RR, -
trySS 
{TT 
stringUU 
_apiUrlUU 
=UU  
stringUU! '
.UU' (
FormatUU( .
(UU. /
$strUU/ e
,UUe f
dealerIdUUg o
,UUo p
userIdUUq w
)UUw x
;UUx y
usingWW 
(WW 
BikewaleWW 
.WW  
UtilityWW  '
.WW' (
BWHttpClientWW( 4
	objClientWW5 >
=WW? @
newWWA D
BikewaleWWE M
.WWM N
UtilityWWN U
.WWU V
BWHttpClientWWV b
(WWb c
)WWc d
)WWd e
{XX 
_areNumbersReleasedYY '
=YY( )
	objClientYY* 3
.YY3 4
PostSyncYY4 <
<YY< =
stringYY= C
,YYC D
boolYYE I
>YYI J
(YYJ K
BikewaleYYK S
.YYS T
UtilityYYT [
.YY[ \
APIHostYY\ c
.YYc d
CWSYYd g
,YYg h
BikewaleYYi q
.YYq r
UtilityYYr y
.YYy z
BWConfiguration	YYz �
.
YY� �
Instance
YY� �
.
YY� � 
APIRequestTypeJSON
YY� �
,
YY� �
_apiUrl
YY� �
,
YY� �
maskingNumbers
YY� �
)
YY� �
;
YY� �
}ZZ 
}[[ 
catch\\ 
(\\ 
System\\ 
.\\ 
	Exception\\ #
ex\\$ &
)\\& '
{]] 

ErrorClass^^ 
objErr^^ !
=^^" #
new^^$ '

ErrorClass^^( 2
(^^2 3
ex^^3 5
,^^5 6
$str^^7 k
)^^k l
;^^l m
objErr__ 
.__ 
SendMail__ 
(__  
)__  !
;__! "
}`` 
returnaa 
_areNumbersReleasedaa &
;aa& '
}bb 	
publickk 
boolkk #
AddCampaignContractDatakk +
(kk+ ,'
ContractCampaignInputEntitykk, G
	_ccInputskkH Q
)kkQ R
{ll 	
boolmm #
isCampaignContractAddedmm (
=mm) *
falsemm+ 0
;mm0 1
trynn 
{oo 
stringpp 
_apiUrlpp 
=pp  
$strpp! ;
;pp; <
usingrr 
(rr 
Bikewalerr 
.rr  
Utilityrr  '
.rr' (
BWHttpClientrr( 4
	objClientrr5 >
=rr? @
newrrA D
BikewalerrE M
.rrM N
UtilityrrN U
.rrU V
BWHttpClientrrV b
(rrb c
)rrc d
)rrd e
{ss #
isCampaignContractAddedtt +
=tt, -
	objClienttt. 7
.tt7 8
PostSynctt8 @
<tt@ A'
ContractCampaignInputEntityttA \
,tt\ ]
booltt^ b
>ttb c
(ttc d
Bikewalettd l
.ttl m
Utilityttm t
.ttt u
APIHostttu |
.tt| }
CWS	tt} �
,
tt� �
Bikewale
tt� �
.
tt� �
Utility
tt� �
.
tt� �
BWConfiguration
tt� �
.
tt� �
Instance
tt� �
.
tt� � 
APIRequestTypeJSON
tt� �
,
tt� �
_apiUrl
tt� �
,
tt� �
	_ccInputs
tt� �
)
tt� �
;
tt� �
}uu 
}vv 
catchww 
(ww 
Systemww 
.ww 
	Exceptionww #
exww$ &
)ww& '
{xx 

ErrorClassyy 
objErryy !
=yy" #
newyy$ '

ErrorClassyy( 2
(yy2 3
exyy3 5
,yy5 6
$stryy7 n
)yyn o
;yyo p
objErrzz 
.zz 
SendMailzz 
(zz  
)zz  !
;zz! "
}{{ 
return|| #
isCampaignContractAdded|| *
;||* +
}}} 	
}~~ 
} ��
>D:\work\bikewaleweb\BikewaleOpr.BAL\BikePricing\DealerPrice.cs
	namespace

 	
BikewaleOpr


 
.

 
BAL

 
.

 
BikePricing

 %
{ 
public 

class 
DealerPrice 
: 
IDealerPrice +
{ 
private 
readonly "
IDealerPriceRepository /"
_dealerPriceRepository0 F
=G H
nullI M
;M N
private 
readonly 
IDealerPriceQuote *'
_dealerPriceQuoteRepository+ F
=G H
nullI M
;M N
public 
DealerPrice 
( "
IDealerPriceRepository 1'
dealerPriceRepositoryObject2 M
,M N
IDealerPriceQuoteO `-
 dealerPriceQuoteRepositoryObject	a �
)
� �
{ 	"
_dealerPriceRepository "
=# $'
dealerPriceRepositoryObject% @
;@ A'
_dealerPriceQuoteRepository '
=( ),
 dealerPriceQuoteRepositoryObject* J
;J K
} 	
private 
IList 
< $
DealerVersionPriceEntity .
>. /
Convert0 7
(7 8
IEnumerable8 C
<C D
DealerVersionEntityD W
>W X

objDealersY c
)c d
{   	
Mapper!! 
.!! 
	CreateMap!! 
<!! 
DealerVersionEntity!! 0
,!!0 1$
DealerVersionPriceEntity!!2 J
>!!J K
(!!K L
)!!L M
;!!M N
return"" 
Mapper"" 
."" 
Map"" 
<"" 
IEnumerable"" )
<"") *
DealerVersionEntity""* =
>""= >
,""> ?
IList""@ E
<""E F$
DealerVersionPriceEntity""F ^
>""^ _
>""_ `
(""` a

objDealers""a k
)""k l
;""l m
}## 	
public-- 
IEnumerable-- 
<-- $
DealerVersionPriceEntity-- 3
>--3 4 
GetDealerPriceQuotes--5 I
(--I J
uint--J N
cityId--O U
,--U V
uint--W [
makeId--\ b
,--b c
uint--d h
dealerId--i q
)--q r
{.. 	
IList// 
<// $
DealerVersionPriceEntity// *
>//* +
dealerVersionPrices//, ?
=//@ A
null//B F
;//F G!
DealerPriceBaseEntity11 !
dealerPriceBase11" 1
=112 3
null114 8
;118 9
ICollection22 
<22 
VersionPriceEntity22 *
>22* +
nullCategories22, :
=22; <
new22= @
List22A E
<22E F
VersionPriceEntity22F X
>22X Y
(22Y Z
)22Z [
;22[ \
nullCategories33 
.33 
Add33 
(33 
new33 "
VersionPriceEntity33# 5
{44 
	VersionId55 
=55 
$num55 
,55 
ItemCategoryId66 
=66  
$num66! "
,66" #
ItemName77 
=77 
$str77 (
,77( )
	ItemValue88 
=88 
$num88 
,88 
}99 
):: 
;:: 
nullCategories;; 
.;; 
Add;; 
(;; 
new;; "
VersionPriceEntity;;# 5
{<< 
	VersionId== 
=== 
$num== 
,== 
ItemCategoryId>> 
=>>  
$num>>! "
,>>" #
ItemName?? 
=?? 
$str??  
,??  !
	ItemValue@@ 
=@@ 
$num@@ 
,@@ 
}AA 
)BB 
;BB 
tryDD 
{EE 
dealerPriceBaseFF 
=FF  !"
_dealerPriceRepositoryFF" 8
.FF8 9
GetDealerPricesFF9 H
(FFH I
cityIdFFI O
,FFO P
makeIdFFQ W
,FFW X
dealerIdFFY a
)FFa b
;FFb c
ifHH 
(HH 
dealerPriceBaseHH #
!=HH$ &
nullHH' +
&&HH, .
dealerPriceBaseHH/ >
.HH> ?
DealerVersionsHH? M
!=HHN P
nullHHQ U
&&HHV X
dealerPriceBaseHHY h
.HHh i
VersionPricesHHi v
.HHv w
CountHHw |
(HH| }
)HH} ~
>	HH �
$num
HH� �
)
HH� �
{II 
ICollectionJJ 
<JJ  
VersionPriceEntityJJ  2
>JJ2 3!
partialNullCategoriesJJ4 I
=JJJ K
newJJL O
ListJJP T
<JJT U
VersionPriceEntityJJU g
>JJg h
(JJh i
)JJi j
;JJj k
varLL 
tempLL 
=LL 
dealerPriceBaseLL .
.LL. /
VersionPricesLL/ <
.LL< =
SelectLL= C
(LLC D
oLLD E
=>LLF H
newLLI L
{LLM N
oLLO P
.LLP Q
ItemCategoryIdLLQ _
,LL_ `
oLLa b
.LLb c
ItemNameLLc k
}LLl m
)LLm n
.LLn o
DistinctLLo w
(LLw x
)LLx y
;LLy z
foreachNN 
(NN 
varNN  
categoryNN! )
inNN* ,
tempNN- 1
)NN1 2
{OO !
partialNullCategoriesPP -
.PP- .
AddPP. 1
(PP1 2
newPP2 5
VersionPriceEntityPP6 H
{QQ 
ItemCategoryIdRR *
=RR+ ,
categoryRR- 5
.RR5 6
ItemCategoryIdRR6 D
,RRD E
ItemNameSS $
=SS% &
categorySS' /
.SS/ 0
ItemNameSS0 8
}TT 
)UU 
;UU 
}VV 
dealerVersionPricesXX '
=XX( )
dealerPriceBaseXX* 9
.XX9 :
DealerVersionsXX: H
.XXH I
	GroupJoinXXI R
(XXR S
dealerPriceBaseXXS b
.XXb c
VersionPricesXXc p
,XXp q
modelYY 
=>YY  
modelYY! &
.YY& '
	VersionIdYY' 0
,YY0 1
categoryZZ  
=>ZZ! #
categoryZZ$ ,
.ZZ, -
	VersionIdZZ- 6
,ZZ6 7
([[ 
model[[ 
,[[ 

categories[[  *
)[[* +
=>[[, .
new[[/ 2$
DealerVersionPriceEntity[[3 K
{\\ 
MakeName]] $
=]]% &
model]]' ,
.]], -
MakeName]]- 5
,]]5 6
VersionName^^ '
=^^( )
model^^* /
.^^/ 0
VersionName^^0 ;
,^^; <
	ModelName__ %
=__& '
model__( -
.__- .
	ModelName__. 7
,__7 8
	VersionId`` %
=``& '
model``( -
.``- .
	VersionId``. 7
,``7 8

Categoriesaa &
=aa' (

categoriesaa) 3
!=aa4 6
nullaa7 ;
&&aa< >

categoriesaa? I
.aaI J
CountaaJ O
(aaO P
)aaP Q
>aaR S
$numaaT U
?aaV W

categoriesaaX b
:aac d!
partialNullCategoriesaae z
,aaz {
NumberOfDaysbb (
=bb) *
modelbb+ 0
.bb0 1
NumberOfDaysbb1 =
,bb= >
BikeModelIdcc '
=cc( )
modelcc* /
.cc/ 0
BikeModelIdcc0 ;
}dd 
)ee 
.ee 
ToListee 
(ee 
)ee 
;ee 
vargg 
comparegg 
=gg  !
newgg" %&
VersionPriceEntityComparergg& @
(gg@ A
)ggA B
;ggB C
forhh 
(hh 
inthh 
ihh 
=hh  
$numhh! "
;hh" #
ihh$ %
<hh& '
dealerVersionPriceshh( ;
.hh; <
Counthh< A
(hhA B
)hhB C
;hhC D
ihhE F
++hhF H
)hhH I
{ii 
ifjj 
(jj 
dealerVersionPricesjj /
[jj/ 0
ijj0 1
]jj1 2
.jj2 3

Categoriesjj3 =
.jj= >
Countjj> C
(jjC D
)jjD E
!=jjF H!
partialNullCategoriesjjI ^
.jj^ _
Countjj_ d
(jjd e
)jje f
)jjf g
{kk 
dealerVersionPricesll /
[ll/ 0
ill0 1
]ll1 2
.ll2 3

Categoriesll3 =
=ll> ?
dealerVersionPricesll@ S
[llS T
illT U
]llU V
.llV W

CategoriesllW a
.lla b
Unionllb g
(llg h!
partialNullCategoriesllh }
,ll} ~
compare	ll �
)
ll� �
.
ll� �
OrderBy
ll� �
(
ll� �
category
ll� �
=>
ll� �
category
ll� �
.
ll� �
ItemCategoryId
ll� �
)
ll� �
;
ll� �
}mm 
}nn 
}pp 
elseqq 
ifqq 
(qq 
dealerPriceBaseqq (
!=qq) +
nullqq, 0
&&qq1 3
dealerPriceBaseqq4 C
.qqC D
DealerVersionsqqD R
!=qqS U
nullqqV Z
||qq[ ]
dealerPriceBaseqq^ m
.qqm n
VersionPricesqqn {
.qq{ |
Count	qq| �
(
qq� �
)
qq� �
==
qq� �
$num
qq� �
)
qq� �
{rr 
dealerVersionPricesss '
=ss( )
Convertss* 1
(ss1 2
dealerPriceBasess2 A
.ssA B
DealerVersionsssB P
)ssP Q
;ssQ R
foreachtt 
(tt $
DealerVersionPriceEntitytt 5
dealerVersionEntitytt6 I
inttJ L
dealerVersionPricesttM `
)tt` a
{uu 
dealerVersionEntityvv +
.vv+ ,

Categoriesvv, 6
=vv7 8
nullCategoriesvv9 G
;vvG H
}ww 
}xx 
}yy 
catchzz 
(zz 
	Exceptionzz 
exzz 
)zz  
{{{ 

ErrorClass|| 
objErr|| !
=||" #
new||$ '

ErrorClass||( 2
(||2 3
ex||3 5
,||5 6
string||7 =
.||= >
Format||> D
(||D E
$str}} M
,}}M N
cityId}}O U
,}}U V
makeId}}W ]
,}}] ^
dealerId}}_ g
)}}g h
)}}h i
;}}i j
}~~ 
return
�� !
dealerVersionPrices
�� &
;
��& '
}
�� 	
private
�� 
class
�� (
VersionPriceEntityComparer
�� 0
:
��1 2
IEqualityComparer
��3 D
<
��D E 
VersionPriceEntity
��E W
>
��W X
{
�� 	
public
�� 
bool
�� 
Equals
�� 
(
��  
VersionPriceEntity
�� 1
b1
��2 4
,
��4 5 
VersionPriceEntity
��6 H
b2
��I K
)
��K L
{
�� 
if
�� 
(
�� 
b2
�� 
==
�� 
null
�� 
&&
�� !
b1
��" $
==
��% '
null
��( ,
)
��, -
return
�� 
true
�� 
;
��  
else
�� 
if
�� 
(
�� 
b1
�� 
==
�� 
null
�� #
|
��$ %
b2
��& (
==
��) +
null
��, 0
)
��0 1
return
�� 
false
��  
;
��  !
else
�� 
if
�� 
(
�� 
b1
�� 
.
�� 
ItemCategoryId
�� *
==
��+ -
b2
��. 0
.
��0 1
ItemCategoryId
��1 ?
&&
��@ B
b1
��C E
.
��E F
ItemName
��F N
==
��O Q
b2
��R T
.
��T U
ItemName
��U ]
)
��] ^
return
�� 
true
�� 
;
��  
else
�� 
return
�� 
false
��  
;
��  !
}
�� 
public
�� 
int
�� 
GetHashCode
�� "
(
��" # 
VersionPriceEntity
��# 5
bx
��6 8
)
��8 9
{
�� 
int
�� 
hCode
�� 
=
�� 
(
�� 
int
��  
)
��  !
bx
��! #
.
��# $
ItemCategoryId
��$ 2
;
��2 3
return
�� 
hCode
�� 
.
�� 
GetHashCode
�� (
(
��( )
)
��) *
;
��* +
}
�� 
}
�� 	
public
�� 
bool
�� &
DeleteVersionPriceQuotes
�� ,
(
��, -
uint
��- 1
dealerId
��2 :
,
��: ;
uint
��< @
cityId
��A G
,
��G H
IEnumerable
��I T
<
��T U
uint
��U Y
>
��Y Z

versionIds
��[ e
)
��e f
{
�� 	
bool
�� 
	isDeleted
�� 
=
�� 
false
�� "
;
��" #
string
�� 
versionIdsString
�� #
=
��$ %
null
��& *
;
��* +
try
�� 
{
�� 
versionIdsString
��  
=
��! "
string
��# )
.
��) *
Join
��* .
<
��. /
uint
��/ 3
>
��3 4
(
��4 5
$str
��5 8
,
��8 9

versionIds
��: D
)
��D E
;
��E F
	isDeleted
�� 
=
�� $
_dealerPriceRepository
�� 2
.
��2 3!
DeleteVersionPrices
��3 F
(
��F G
dealerId
��G O
,
��O P
cityId
��Q W
,
��W X
versionIdsString
��Y i
)
��i j
;
��j k
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
string
��7 =
.
��= >
Format
��> D
(
��D E
$str
�� [
,
��[ \
dealerId
��] e
,
��e f
cityId
��g m
,
��m n
versionIdsString
��o 
)�� �
)��� �
;��� �
}
�� 
return
�� 
	isDeleted
�� 
;
�� 
}
�� 	
public
�� 
bool
�� $
SaveVersionPriceQuotes
�� *
(
��* +
IEnumerable
��+ 6
<
��6 7
uint
��7 ;
>
��; <
	dealerIds
��= F
,
��F G
IEnumerable
��H S
<
��S T
uint
��T X
>
��X Y
cityIds
��Z a
,
��a b
IEnumerable
��c n
<
��n o
uint
��o s
>
��s t

versionIds
��u 
,�� �
IEnumerable
�� 
<
�� 
uint
�� 
>
�� 
itemIds
�� &
,
��& '
IEnumerable
��( 3
<
��3 4
uint
��4 8
>
��8 9

itemValues
��: D
,
��D E
uint
��F J
	enteredBy
��K T
)
��T U
{
�� 	
bool
�� 
isSaved
�� 
=
�� 
false
��  
;
��  !
string
�� 
versionIdsString
�� #
=
��$ %
null
��& *
;
��* +
string
�� 
itemIdsString
��  
=
��! "
null
��# '
;
��' (
string
�� 
itemValuesString
�� #
=
��$ %
null
��& *
;
��* +
string
�� 
dealerIdsString
�� "
=
��# $
null
��% )
;
��) *
string
�� 
cityIdsString
��  
=
��! "
null
��# '
;
��' (
try
�� 
{
�� 
if
�� 
(
�� 
itemIds
�� 
!=
�� 
null
�� #
&&
��$ &

itemValues
��' 1
!=
��2 4
null
��5 9
&&
��: <

itemValues
��= G
.
��G H
Count
��H M
(
��M N
)
��N O
>
��P Q
$num
��R S
&&
��T V
itemIds
��W ^
.
��^ _
Count
��_ d
(
��d e
)
��e f
>
��g h
$num
��i j
)
��j k
{
�� 
itemIdsString
�� !
=
��" #
string
��$ *
.
��* +
Join
��+ /
<
��/ 0
uint
��0 4
>
��4 5
(
��5 6
$str
��6 9
,
��9 :
itemIds
��; B
)
��B C
;
��C D
itemValuesString
�� $
=
��% &
string
��' -
.
��- .
Join
��. 2
<
��2 3
uint
��3 7
>
��7 8
(
��8 9
$str
��9 <
,
��< =

itemValues
��> H
)
��H I
;
��I J
versionIdsString
�� $
=
��% &
string
��' -
.
��- .
Join
��. 2
<
��2 3
uint
��3 7
>
��7 8
(
��8 9
$str
��9 <
,
��< =

versionIds
��> H
)
��H I
;
��I J
dealerIdsString
�� #
=
��$ %
string
��& ,
.
��, -
Join
��- 1
<
��1 2
uint
��2 6
>
��6 7
(
��7 8
$str
��8 ;
,
��; <
	dealerIds
��= F
)
��F G
;
��G H
cityIdsString
�� !
=
��" #
string
��$ *
.
��* +
Join
��+ /
<
��/ 0
uint
��0 4
>
��4 5
(
��5 6
$str
��6 9
,
��9 :
cityIds
��; B
)
��B C
;
��C D
isSaved
�� 
=
�� $
_dealerPriceRepository
�� 4
.
��4 5
SaveDealerPrices
��5 E
(
��E F
dealerIdsString
��F U
,
��U V
cityIdsString
��W d
,
��d e
versionIdsString
��f v
,
��v w
itemIdsString��x �
,��� � 
itemValuesString��� �
,��� �
	enteredBy��� �
)��� �
;��� �
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
string
��7 =
.
��= >
Format
��> D
(
��D E
$str�� �
,��� �
	dealerIds
�� 
,
�� 
cityIds
�� &
,
��& '
versionIdsString
��( 8
,
��8 9
itemIdsString
��: G
,
��G H
itemValuesString
��I Y
,
��Y Z
	enteredBy
��[ d
)
��d e
)
��e f
;
��f g
}
�� 
return
�� 
isSaved
�� 
;
�� 
}
�� 	
public
�� .
 UpdatePricingRulesResponseEntity
�� /$
SaveVersionPriceQuotes
��0 F
(
��F G
IEnumerable
��G R
<
��R S
uint
��S W
>
��W X
	dealerIds
��Y b
,
��b c
IEnumerable
��d o
<
��o p
uint
��p t
>
��t u
cityIds
��v }
,
��} ~
IEnumerable�� �
<��� �
uint��� �
>��� �

versionIds��� �
,��� �
IEnumerable
�� 
<
�� 
uint
�� 
>
�� 
itemIds
�� &
,
��& '
IEnumerable
��( 3
<
��3 4
uint
��4 8
>
��8 9

itemValues
��: D
,
��D E
IEnumerable
��F Q
<
��Q R
uint
��R V
>
��V W
bikeModelIds
��X d
,
��d e
IEnumerable
��f q
<
��q r
string
��r x
>
��x y
bikeModelNames��z �
,��� �
uint��� �
	enteredBy��� �
,��� �
uint��� �
makeId��� �
)��� �
{
�� 	.
 UpdatePricingRulesResponseEntity
�� ,
response
��- 5
=
��6 7
new
��8 ;.
 UpdatePricingRulesResponseEntity
��< \
(
��\ ]
)
��] ^
;
��^ _
response
�� 
.
�� 
IsPriceSaved
�� !
=
��" #
false
��$ )
;
��) *
string
�� 
versionIdsString
�� #
=
��$ %
null
��& *
;
��* +
string
�� 
itemIdsString
��  
=
��! "
null
��# '
;
��' (
string
�� 
itemValuesString
�� #
=
��$ %
null
��& *
;
��* +
string
�� 
dealerIdsString
�� "
=
��# $
null
��% )
;
��) *
string
�� 
cityIdsString
��  
=
��! "
null
��# '
;
��' (
string
��  
modelIdNamesString
�� %
=
��& '
null
��( ,
;
��, -
try
�� 
{
�� 
if
�� 
(
�� 
itemIds
�� 
!=
�� 
null
�� #
&&
��$ &

itemValues
��' 1
!=
��2 4
null
��5 9
&&
��: <

itemValues
��= G
.
��G H
Count
��H M
(
��M N
)
��N O
>
��P Q
$num
��R S
&&
��T V
itemIds
��W ^
.
��^ _
Count
��_ d
(
��d e
)
��e f
>
��g h
$num
��i j
)
��j k
{
�� 
itemIdsString
�� !
=
��" #
string
��$ *
.
��* +
Join
��+ /
<
��/ 0
uint
��0 4
>
��4 5
(
��5 6
$str
��6 9
,
��9 :
itemIds
��; B
)
��B C
;
��C D
itemValuesString
�� $
=
��% &
string
��' -
.
��- .
Join
��. 2
<
��2 3
uint
��3 7
>
��7 8
(
��8 9
$str
��9 <
,
��< =

itemValues
��> H
)
��H I
;
��I J
versionIdsString
�� $
=
��% &
string
��' -
.
��- .
Join
��. 2
<
��2 3
uint
��3 7
>
��7 8
(
��8 9
$str
��9 <
,
��< =

versionIds
��> H
)
��H I
;
��I J
dealerIdsString
�� #
=
��$ %
string
��& ,
.
��, -
Join
��- 1
<
��1 2
uint
��2 6
>
��6 7
(
��7 8
$str
��8 ;
,
��; <
	dealerIds
��= F
)
��F G
;
��G H
cityIdsString
�� !
=
��" #
string
��$ *
.
��* +
Join
��+ /
<
��/ 0
uint
��0 4
>
��4 5
(
��5 6
$str
��6 9
,
��9 :
cityIds
��; B
)
��B C
;
��C D 
modelIdNamesString
�� &
=
��' (
string
��) /
.
��/ 0
Join
��0 4
<
��4 5
string
��5 ;
>
��; <
(
��< =
$str
��= @
,
��@ A
bikeModelIds
��B N
.
��N O
Zip
��O R
(
��R S
bikeModelNames
��S a
,
��a b
(
��c d
modelId
��d k
,
��k l
	modelName
��m v
)
��v w
=>
��x z
string��{ �
.��� �
Format��� �
(��� �
$str��� �
,��� �
modelId��� �
,��� �
	modelName��� �
)��� �
)��� �
)��� �
;��� �
response
�� 
.
�� 
IsPriceSaved
�� )
=
��* +$
_dealerPriceRepository
��, B
.
��B C
SaveDealerPrices
��C S
(
��S T
dealerIdsString
��T c
,
��c d
cityIdsString
��e r
,
��r s
versionIdsString��t �
,��� �
itemIdsString��� �
,��� � 
itemValuesString��� �
,��� �
	enteredBy��� �
)��� �
;��� �
}
�� 
if
�� 
(
�� 
	dealerIds
�� 
.
�� 
Count
�� #
(
��# $
)
��$ %
==
��& (
$num
��) *
)
��* +
response
�� 
.
�� $
RulesUpdatedModelNames
�� 3
=
��4 5)
_dealerPriceQuoteRepository
��6 Q
.
��Q R%
AddRulesOnPriceUpdation
��R i
(
��i j 
modelIdNamesString
��j |
,
��| }
	dealerIds��~ �
.��� �
First��� �
(��� �
)��� �
,��� �
makeId��� �
,��� �
	enteredBy��� �
)��� �
;��� �
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
string
��7 =
.
��= >
Format
��> D
(
��D E
$str�� �
,��� �
	dealerIds
�� 
,
�� 
cityIds
�� &
,
��& '
versionIdsString
��( 8
,
��8 9
itemIdsString
��: G
,
��G H
itemValuesString
��I Y
,
��Y Z
	enteredBy
��[ d
)
��d e
)
��e f
;
��f g
}
�� 
return
�� 
response
�� 
;
�� 
}
�� 	
}
�� 
}�� �
.D:\work\bikewaleweb\BikewaleOpr.BAL\Dealers.cs
	namespace

 	
BikewaleOpr


 
.

 
BAL

 
{ 
public 

class 
Dealers 
: 
IDealer "
{ 
IDealerPriceQuote 
_dealerPQRepository -
=. /
null0 4
;4 5
public 
Dealers 
( 
IDealerPriceQuote (
dealerPQRepository) ;
); <
{ 	
_dealerPQRepository 
=  !
dealerPQRepository" 4
;4 5
} 	
public 
IEnumerable 
< 
uint 
>  !
GetAllAvailableDealer! 6
(6 7
uint7 ;
	versionId< E
,E F
uintG K
areaIdL R
)R S
{ 	
IEnumerable 
< 
uint 
> 
objDealerList +
=, -
null. 2
;2 3
double!! 
areaLattitude!!  
=!!! "
$num!!# $
,!!$ %
areaLongitude!!& 3
=!!4 5
$num!!6 7
;!!7 8
try## 
{$$ 
_dealerPQRepository%% #
.%%# $
GetAreaLatLong%%$ 2
(%%2 3
areaId%%3 9
,%%9 :
out%%; >
areaLattitude%%? L
,%%L M
out%%N Q
areaLongitude%%R _
)%%_ `
;%%` a
List'' 
<'' 
DealerLatLong'' "
>''" #
objDealersList''$ 2
=''3 4
_dealerPQRepository''5 H
.''H I
GetDealersLatLong''I Z
(''Z [
	versionId''[ d
,''d e
areaId''f l
)''l m
;''m n
if)) 
()) 
objDealersList)) "
!=))# %
null))& *
&&))+ -
objDealersList)). <
.))< =
Count))= B
>))C D
$num))E F
)))F G
{** 
var++ 
dealerDistList++ &
=++' (
new++) ,
List++- 1
<++1 2%
DealerCustDistanceMapping++2 K
>++K L
(++L M
)++M N
;++N O
foreach-- 
(-- 
var--  
dealer--! '
in--( *
objDealersList--+ 9
)--9 :
{.. 
dealerDistList// &
.//& '
Add//' *
(//* +
new//+ .%
DealerCustDistanceMapping/// H
{00 
dealer11 "
=11# $
dealer11% +
,11+ ,
distance22 $
=22% &*
GetDistanceBetweenTwoLocations22' E
(22E F
areaLattitude22F S
,22S T
areaLongitude22U b
,22b c
dealer22d j
.22j k
	Lattitude22k t
,22t u
dealer22v |
.22| }
	Longitude	22} �
)
22� �
}33 
)33 
;33 
}44 
objDealerList66 !
=66" #
dealerDistList66$ 2
.662 3
FindAll663 :
(66: ;
s66; <
=>66= ?
(66@ A
s66A B
.66B C
dealer66C I
.66I J
ServingDistance66J Y
==66Z \
$num66] ^
||66_ a
s66b c
.66c d
dealer66d j
.66j k
ServingDistance66k z
>=66{ }
s66~ 
.	66 �
distance
66� �
)
66� �
)
66� �
.
66� �
OrderBy
66� �
(
66� �
s
66� �
=>
66� �
s
66� �
.
66� �
distance
66� �
)
66� �
.
66� �
Select
66� �
(
66� �
s
66� �
=>
66� �
s
66� �
.
66� �
dealer
66� �
.
66� �
DealerId
66� �
)
66� �
;
66� �
}77 
}88 
catch99 
(99 
	Exception99 
ex99 
)99  
{:: 

ErrorClass;; 
objErr;; !
=;;" #
new;;$ '

ErrorClass;;( 2
(;;2 3
ex;;3 5
,;;5 6
$str;;7 h
);;h i
;;;i j
objErr<< 
.<< 
SendMail<< 
(<<  
)<<  !
;<<! "
}== 
return>> 
objDealerList>>  
;>>  !
}?? 	
publicJJ 
IEnumerableJJ 
<JJ $
DealerPriceQuoteDetailedJJ 3
>JJ3 4%
GetDealerPriceQuoteDetailJJ5 N
(JJN O
uintJJO S
	versionIdJJT ]
,JJ] ^
uintJJ_ c
cityIdJJd j
,JJj k
stringJJl r
	dealerIdsJJs |
)JJ| }
{KK 	
IListLL 
<LL $
DealerPriceQuoteDetailedLL *
>LL* +
	objDealerLL, 5
=LL6 7
nullLL8 <
;LL< =
IListMM 
<MM #
BikeAvailabilityByColorMM )
>MM) *
objColorAvailMM+ 8
=MM9 :
nullMM; ?
;MM? @"
DealerPriceQuoteEntityNN "
objDealerEntityNN# 2
=NN3 4
nullNN5 9
;NN9 :
stringPP 
[PP 
]PP 
dealersPP 
=PP 
nullPP #
;PP# $
tryQQ 
{RR 
ifSS 
(SS 
!SS 
StringSS 
.SS 
IsNullOrEmptySS )
(SS) *
	dealerIdsSS* 3
)SS3 4
)SS4 5
dealersTT 
=TT 
	dealerIdsTT '
.TT' (
SplitTT( -
(TT- .
$charTT. 1
)TT1 2
;TT2 3
objDealerEntityUU 
=UU  !
_dealerPQRepositoryUU" 5
.UU5 6%
GetPriceQuoteForAllDealerUU6 O
(UUO P
	versionIdUUP Y
,UUY Z
cityIdUU[ a
,UUa b
	dealerIdsUUc l
)UUl m
;UUm n
ifWW 
(WW 
dealersWW 
!=WW 
nullWW #
&&WW$ &
dealersWW' .
.WW. /
LengthWW/ 5
>WW6 7
$numWW8 9
&&WW: <
objDealerEntityWW= L
!=WWM O
nullWWP T
)WWT U
{XX 
	objDealerYY 
=YY 
newYY  #
ListYY$ (
<YY( )$
DealerPriceQuoteDetailedYY) A
>YYA B
(YYB C
)YYC D
;YYD E
foreachZZ 
(ZZ 
stringZZ #
dealerZZ$ *
inZZ+ -
dealersZZ. 5
)ZZ5 6
{[[ $
DealerPriceQuoteDetailed\\ 0#
dealerPriceQuoteDetails\\1 H
=\\I J
new\\K N$
DealerPriceQuoteDetailed\\O g
(\\g h
)\\h i
;\\i j#
dealerPriceQuoteDetails^^ /
.^^/ 0
	OfferList^^0 9
=^^: ;
from^^< @
offer^^A F
in^^G I
objDealerEntity^^J Y
.^^Y Z
	OfferList^^Z c
where__< A
offer__B G
.__G H
DealerId__H P
==__Q S
Convert__T [
.__[ \
ToUInt32__\ d
(__d e
dealer__e k
)__k l
select``< B
offer``C H
;``H I#
dealerPriceQuoteDetailsbb /
.bb/ 0
	PriceListbb0 9
=bb: ;
frombb< @
pricebbA F
inbbG I
objDealerEntitybbJ Y
.bbY Z
	PriceListbbZ c
wherecc< A
priceccB G
.ccG H
DealerIdccH P
==ccQ S
ConvertccT [
.cc[ \
ToUInt32cc\ d
(ccd e
dealercce k
)cck l
selectdd< B
priceddC H
;ddH I
varff 
ColorListForDealerff .
=ff/ 0
fromff1 5
colorff6 ;
inff< >
objDealerEntityff? N
.ffN O#
BikeAvailabilityByColorffO f
wheregg1 6
colorgg7 <
.gg< =
DealerIdgg= E
==ggF H
$numggI J
||ggK M
colorggN S
.ggS T
DealerIdggT \
==gg] _
Convertgg` g
.ggg h
ToUInt32ggh p
(ggp q
dealerggq w
)ggw x
grouphh1 6
colorhh7 <
byhh= ?
colorhh@ E
.hhE F
ColorIdhhF M
intohhN R
newgrouphhS [
orderbyii1 8
newgroupii9 A
.iiA B
KeyiiB E
selectjj1 7
newgroupjj8 @
;jj@ A
objColorAvailmm %
=mm& '
newmm( +
Listmm, 0
<mm0 1#
BikeAvailabilityByColormm1 H
>mmH I
(mmI J
)mmJ K
;mmK L
foreachnn 
(nn  !
varnn! $
colornn% *
innn+ -
ColorListForDealernn. @
)nn@ A
{oo #
BikeAvailabilityByColorpp 3
objAvailpp4 <
=pp= >
newpp? B#
BikeAvailabilityByColorppC Z
(ppZ [
)pp[ \
;pp\ ]
objAvailrr $
.rr$ %
ColorIdrr% ,
=rr- .
colorrr/ 4
.rr4 5
Keyrr5 8
;rr8 9
objAvailss $
.ss$ %
DealerIdss% -
=ss. /
Convertss0 7
.ss7 8
ToUInt32ss8 @
(ss@ A
dealerssA G
)ssG H
;ssH I
IListuu !
<uu! "
stringuu" (
>uu( )
HexCodeListuu* 5
=uu6 7
newuu8 ;
Listuu< @
<uu@ A
stringuuA G
>uuG H
(uuH I
)uuI J
;uuJ K
foreachvv #
(vv$ %
varvv% (
	colorListvv) 2
invv3 5
colorvv6 ;
)vv; <
{ww 
objAvailxx  (
.xx( )
	ColorNamexx) 2
=xx3 4
	colorListxx5 >
.xx> ?
	ColorNamexx? H
;xxH I
objAvailyy  (
.yy( )
NoOfDaysyy) 1
=yy2 3
	colorListyy4 =
.yy= >
NoOfDaysyy> F
;yyF G
objAvailzz  (
.zz( )
	VersionIdzz) 2
=zz3 4
	colorListzz5 >
.zz> ?
	VersionIdzz? H
;zzH I
HexCodeList||  +
.||+ ,
Add||, /
(||/ 0
	colorList||0 9
.||9 :
HexCode||: A
)||A B
;||B C
}}} 
objAvail~~ $
.~~$ %
HexCode~~% ,
=~~- .
HexCodeList~~/ :
;~~: ;
objColorAvail
�� )
.
��) *
Add
��* -
(
��- .
objAvail
��. 6
)
��6 7
;
��7 8
}
�� %
dealerPriceQuoteDetails
�� /
.
��/ 0!
AvailabilityByColor
��0 C
=
��D E
objColorAvail
��F S
;
��S T%
dealerPriceQuoteDetails
�� /
.
��/ 0
DealerDetails
��0 =
=
��> ?
objDealerEntity
��@ O
.
��O P
DealerDetails
��P ]
.
��] ^
Where
��^ c
(
��c d
ss
��d f
=>
��g i
ss
��j l
.
��l m
DealerId
��m u
==
��v x
Convert��y �
.��� �
ToUInt32��� �
(��� �
dealer��� �
)��� �
)��� �
.��� �
Select��� �
(��� �
ss��� �
=>��� �
ss��� �
.��� �
Dealer��� �
)��� �
.��� �
FirstOrDefault��� �
(��� �
)��� �
;��� �%
dealerPriceQuoteDetails
�� /
.
��/ 0
Availability
��0 <
=
��= >
objDealerEntity
��? N
.
��N O
DealerDetails
��O \
.
��\ ]
Where
��] b
(
��b c
ss
��c e
=>
��f h
ss
��i k
.
��k l
DealerId
��l t
==
��u w
Convert
��x 
.�� �
ToUInt32��� �
(��� �
dealer��� �
)��� �
)��� �
.��� �
Select��� �
(��� �
ss��� �
=>��� �
ss��� �
.��� �
Availability��� �
)��� �
.��� �
FirstOrDefault��� �
(��� �
)��� �
;��� �%
dealerPriceQuoteDetails
�� /
.
��/ 0
BookingAmount
��0 =
=
��> ?
objDealerEntity
��@ O
.
��O P
DealerDetails
��P ]
.
��] ^
Where
��^ c
(
��c d
ss
��d f
=>
��g i
ss
��j l
.
��l m
DealerId
��m u
==
��v x
Convert��y �
.��� �
ToUInt32��� �
(��� �
dealer��� �
)��� �
)��� �
.��� �
Select��� �
(��� �
ss��� �
=>��� �
ss��� �
.��� �
BookingAmount��� �
)��� �
.��� �
FirstOrDefault��� �
(��� �
)��� �
;��� �
	objDealer
�� !
.
��! "
Add
��" %
(
��% &%
dealerPriceQuoteDetails
��& =
)
��= >
;
��> ?
}
�� 
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
$str
��7 l
)
��l m
;
��m n
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
return
�� 
	objDealer
�� 
;
�� 
}
�� 	
public
�� 
double
�� ,
GetDistanceBetweenTwoLocations
�� 4
(
��4 5
double
��5 ;

lattitude1
��< F
,
��F G
double
��H N

longitude1
��O Y
,
��Y Z
double
��[ a

lattitude2
��b l
,
��l m
double
��n t

longitude2
��u 
)�� �
{
�� 	
var
�� 
radEarth
�� 
=
�� 
$num
�� 
;
��  
double
�� 
lat1
�� 
,
�� 
lon1
�� 
,
�� 
lat2
�� #
,
��# $
lon2
��% )
,
��) *
dlat
��+ /
,
��/ 0
dlon
��1 5
,
��5 6
a
��7 8
,
��8 9
gtCirDistance
��: G
,
��G H
distance
��I Q
;
��Q R
lat1
�� 
=
�� 
DegToRad
�� 
(
�� 

lattitude1
�� &
)
��& '
;
��' (
lon1
�� 
=
�� 
DegToRad
�� 
(
�� 

longitude1
�� &
)
��& '
;
��' (
lat2
�� 
=
�� 
DegToRad
�� 
(
�� 

lattitude2
�� &
)
��& '
;
��' (
lon2
�� 
=
�� 
DegToRad
�� 
(
�� 

longitude2
�� &
)
��& '
;
��' (
dlon
�� 
=
�� 
lon2
�� 
-
�� 
lon1
�� 
;
�� 
dlat
�� 
=
�� 
lat2
�� 
-
�� 
lat1
�� 
;
�� 
a
�� 
=
�� 
Math
�� 
.
�� 
Pow
�� 
(
�� 
Math
�� 
.
�� 
Sin
�� !
(
��! "
dlat
��" &
/
��' (
$num
��) *
)
��* +
,
��+ ,
$num
��- .
)
��. /
+
��0 1
Math
��2 6
.
��6 7
Cos
��7 :
(
��: ;
lat1
��; ?
)
��? @
*
��A B
Math
��C G
.
��G H
Cos
��H K
(
��K L
lat2
��L P
)
��P Q
*
��R S
Math
��T X
.
��X Y
Pow
��Y \
(
��\ ]
Math
��] a
.
��a b
Sin
��b e
(
��e f
dlon
��f j
/
��k l
$num
��m n
)
��n o
,
��o p
$num
��q r
)
��r s
;
��s t
gtCirDistance
�� 
=
�� 
$num
�� 
*
�� 
Math
��  $
.
��$ %
Atan2
��% *
(
��* +
Math
��+ /
.
��/ 0
Sqrt
��0 4
(
��4 5
a
��5 6
)
��6 7
,
��7 8
Math
��9 =
.
��= >
Sqrt
��> B
(
��B C
$num
��C D
-
��E F
a
��G H
)
��H I
)
��I J
;
��J K
distance
�� 
=
�� 
radEarth
�� 
*
��  !
gtCirDistance
��" /
;
��/ 0
return
�� 
distance
�� 
;
�� 
}
�� 	
private
�� 
double
�� 
DegToRad
�� 
(
��  
double
��  &
deg
��' *
)
��* +
{
�� 	
double
�� 
rad
�� 
=
�� 
deg
�� 
*
�� 
Math
�� #
.
��# $
PI
��$ &
/
��' (
$num
��) ,
;
��, -
return
�� 
rad
�� 
;
�� 
}
�� 	
}
�� 
}�� �"
FD:\work\bikewaleweb\BikewaleOpr.BAL\BikePricing\VersionAvailability.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
. 
BikePricing %
{ 
public 

class 
VersionAvailability $
:% & 
IVersionAvailability' ;
{ 
private 
readonly 
IDealers !
_dealersRepository" 4
;4 5
public 
VersionAvailability "
(" #
IDealers# +#
dealersRepositoryObject, C
)C D
{ 	
_dealersRepository 
=  #
dealersRepositoryObject! 8
;8 9
} 	
public 
bool #
SaveVersionAvailability +
(+ ,
uint, 0
dealerId1 9
,9 :
IEnumerable; F
<F G
uintG K
>K L
bikeVersionIdsM [
,[ \
IEnumerable] h
<h i
uinti m
>m n
numberOfDayso {
){ |
{ 	
bool 
isSaved 
= 
false  
;  !
string  
bikeVersionIdStrings '
=( )
null* .
;. /
string   
numberOfDaysStrings   &
=  ' (
null  ) -
;  - .
try"" 
{##  
bikeVersionIdStrings$$ $
=$$% &
string$$' -
.$$- .
Join$$. 2
<$$2 3
uint$$3 7
>$$7 8
($$8 9
$str$$9 <
,$$< =
bikeVersionIds$$> L
)$$L M
;$$M N
numberOfDaysStrings%% #
=%%$ %
string%%& ,
.%%, -
Join%%- 1
<%%1 2
uint%%2 6
>%%6 7
(%%7 8
$str%%8 ;
,%%; <
numberOfDays%%= I
)%%I J
;%%J K
isSaved'' 
='' 
_dealersRepository'' ,
.'', -#
SaveVersionAvailability''- D
(''D E
dealerId''E M
,''M N 
bikeVersionIdStrings''O c
,''c d
numberOfDaysStrings''e x
)''x y
;''y z
}(( 
catch)) 
()) 
	Exception)) 
ex)) 
)))  
{** 

ErrorClass++ 
objErr++ !
=++" #
new++$ '

ErrorClass++( 2
(++2 3
ex++3 5
,++5 6
string++7 =
.++= >
Format++> D
(++D E
$str,, Z
,,,Z [
dealerId,,\ d
,,,d e 
bikeVersionIdStrings,,f z
,,,z { 
numberOfDaysStrings	,,| �
)
,,� �
)
,,� �
;
,,� �
}-- 
return// 
isSaved// 
;// 
}00 	
public88 
bool88 %
DeleteVersionAvailability88 -
(88- .
uint88. 2
dealerId883 ;
,88; <
IEnumerable88= H
<88H I
uint88I M
>88M N
bikeVersionIds88O ]
)88] ^
{99 	
bool:: 
	isDeleted:: 
=:: 
false:: "
;::" #
string;;  
bikeVersionIdStrings;; '
=;;( )
null;;* .
;;;. /
try== 
{>>  
bikeVersionIdStrings?? $
=??% &
string??' -
.??- .
Join??. 2
<??2 3
uint??3 7
>??7 8
(??8 9
$str??9 <
,??< =
bikeVersionIds??> L
)??L M
;??M N
	isDeletedAA 
=AA 
_dealersRepositoryAA .
.AA. /%
DeleteVersionAvailabilityAA/ H
(AAH I
dealerIdAAI Q
,AAQ R 
bikeVersionIdStringsAAS g
)AAg h
;AAh i
}BB 
catchCC 
(CC 
	ExceptionCC 
exCC 
)CC  
{DD 

ErrorClassEE 
objErrEE !
=EE" #
newEE$ '

ErrorClassEE( 2
(EE2 3
exEE3 5
,EE5 6
stringEE7 =
.EE= >
FormatEE> D
(EED E
$strFF N
,FFN O
dealerIdFFP X
,FFX Y 
bikeVersionIdStringsFFZ n
)FFn o
)FFo p
;FFp q
}GG 
returnII 
	isDeletedII 
;II 
}JJ 	
}KK 
}LL �)
/D:\work\bikewaleweb\BikewaleOpr.BAL\HomePage.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
{		 
public

 

class

 
HomePage

 
:

 
	IHomePage

 %
{ 
private 
readonly !
IBikeModelsRepository .
_IBikeModelRepo/ >
;> ?
private 
readonly 

IUsedBikes #
_IUsedBikes$ /
;/ 0
private 
readonly 
IBikeModels $
_IBikeModel% 0
;0 1
public 
HomePage 
( !
IBikeModelsRepository -
bikeModelRepos. <
,< =

IUsedBikes> H
	usedBikesI R
,R S
IBikeModelsT _
	bikeModel` i
)i j
{ 	
_IBikeModelRepo 
= 
bikeModelRepos ,
;, -
_IUsedBikes 
= 
	usedBikes #
;# $
_IBikeModel 
= 
	bikeModel #
;# $
} 	
public## 
HomePageData## 
GetHomePageData## +
(##+ ,
string##, 2
id##3 5
,##5 6
string##7 =
userName##> F
)##F G
{$$ 	
HomePageData%% 
objHomePageData%% (
=%%) *
null%%+ /
;%%/ 0
try&& 
{'' 
string** 
[** 
]** 
allowedUsers** %
=**& '
Bikewale**( 0
.**0 1
Utility**1 8
.**8 9
BWOprConfiguration**9 K
.**K L
Instance**L T
.**T U#
NotificationOtherUserId**U l
.**l m
Split**m r
(**r s
$char**s v
)**v w
;**w x
if++ 
(++ 
allowedUsers++  
!=++! #
null++$ (
&&++) +
allowedUsers++, 8
.++8 9
Length++9 ?
>++@ A
$num++B C
&&++D F
allowedUsers++G S
.++S T
Contains++T \
(++\ ]
id++] _
)++_ `
)++` a
{,, 
objHomePageData-- #
=--$ %
new--& )
HomePageData--* 6
(--6 7
)--7 8
;--8 9
if00 
(00 
DateTime00  
.00  !
Now00! $
.00$ %
Day00% (
>00) *
Bikewale00+ 3
.003 4
Utility004 ;
.00; <
BWOprConfiguration00< N
.00N O
Instance00O W
.00W X'
UnitSoldDataNotificationDay00X s
)00s t
{11 
objHomePageData22 '
.22' (
SoldUnitsData22( 5
=226 7
_IBikeModelRepo228 G
.22G H
GetLastSoldUnitData22H [
(22[ \
)22\ ]
;22] ^
if33 
(33 
objHomePageData33 +
.33+ ,
SoldUnitsData33, 9
!=33: <
null33= A
)33A B
{44 
objHomePageData55 +
.55+ ,%
IsSoldBikeDataUpatedShown55, E
=55F G
_IUsedBikes55H S
.55S T
SendUnitSoldEmail55T e
(55e f
objHomePageData55f u
.55u v
SoldUnitsData	55v �
,
55� �
userName
55� �
)
55� �
;
55� �
}66 
}77 
objHomePageData:: #
.::# $
UsedModelsData::$ 2
=::3 4
_IBikeModel::5 @
.::@ A0
$GetPendingUsedBikesWithoutModelImage::A e
(::e f
)::f g
;::g h
if<< 
(<< 
objHomePageData<< '
.<<' (
UsedModelsData<<( 6
!=<<7 9
null<<: >
)<<> ?
{== 
objHomePageData>> '
.>>' (%
IsUsedBikeModelsAvailable>>( A
=>>B C
true>>D H
;>>H I
if?? 
(?? 
objHomePageData?? +
.??+ ,
UsedModelsData??, :
.??: ;
IsNotify??; C
)??C D
_IUsedBikes@@ '
.@@' ()
SendUploadUsedModelImageEmail@@( E
(@@E F
)@@F G
;@@G H
}AA 
objHomePageDataEE #
.EE# $
BikeModelByMakeListEE$ 7
=EE8 9
_IBikeModelEE: E
.EEE F*
GetModelsWithMissingColorImageEEF d
(EEd e
)EEe f
;EEf g
}FF 
}GG 
catchHH 
(HH 
	ExceptionHH 
exHH 
)HH  
{II 
BikewaleJJ 
.JJ 
NotificationsJJ &
.JJ& '

ErrorClassJJ' 1
objErrJJ2 8
=JJ9 :
newJJ; >
BikewaleJJ? G
.JJG H
NotificationsJJH U
.JJU V

ErrorClassJJV `
(JJ` a
exJJa c
,JJc d
stringJJe k
.JJk l
FormatJJl r
(JJr s
$str	JJs �
,
JJ� �
id
JJ� �
,
JJ� �
userName
JJ� �
)
JJ� �
)
JJ� �
;
JJ� �
}KK 
returnLL 
objHomePageDataLL "
;LL" #
}MM 	
}NN 
}OO �E
5D:\work\bikewaleweb\BikewaleOpr.BAL\Images\ImageBL.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
. 
Images  
{ 
public 

class 
ImageBL 
: 
IImage !
{ 
private 
readonly 
string 
_environment  ,
=- .
Bikewale/ 7
.7 8
Utility8 ?
.? @
BWOprConfiguration@ R
.R S
InstanceS [
.[ \
AWSEnvironment\ j
;j k
private 
readonly 
string 
_queue  &
=' (
Bikewale) 1
.1 2
Utility2 9
.9 :
BWOprConfiguration: L
.L M
InstanceM U
.U V
AWSImageQueueNameV g
;g h
private 
readonly 
IImageRepository )
_objDAL* 1
=2 3
null4 8
;8 9
private 
readonly 
	ISecurity "
	_security# ,
=- .
null/ 3
;3 4
public 
ImageBL 
( 
IImageRepository '
objDAL( .
,. /
	ISecurity0 9
security: B
)B C
{ 	
_objDAL 
= 
objDAL 
; 
	_security 
= 
security  
;  !
} 	
public%% 

ImageToken%% $
GenerateImageUploadToken%% 2
(%%2 3
Image%%3 8
objImage%%9 A
)%%A B
{&& 	

ImageToken'' 
token'' 
='' 
new'' "

ImageToken''# -
(''- .
)''. /
;''/ 0
Token(( 
awsToken(( 
=(( 
null(( !
;((! "
try)) 
{** 
_objDAL++ 
.++ 
Add++ 
(++ 
objImage++ $
)++$ %
;++% &
if,, 
(,, 
objImage,, 
.,, 
Id,, 
.,,  
HasValue,,  (
&&,,) +
objImage,,, 4
.,,4 5
Id,,5 7
.,,7 8
Value,,8 =
>,,> ?
$num,,@ A
),,A B
{-- 
token.. 
... 
Id.. 
=.. 
(..  
uint..  $
)..$ %
objImage..% -
...- .
Id... 0
...0 1
Value..1 6
;..6 7
string// 
hash// 
=//  !
	_security//" +
.//+ ,
GenerateHash//, 8
(//8 9
(//9 :
uint//: >
)//> ?
objImage//? G
.//G H
Id//H J
.//J K
Value//K P
)//P Q
;//Q R
awsToken00 
=00 
	_security00 (
.00( )
GetToken00) 1
(001 2
)002 3
;003 4
token11 
.11 
Key11 
=11 
String11  &
.11& '
Format11' -
(11- .
$str11. C
,11C D
_environment11E Q
,11Q R
hash11S W
,11W X
objImage11Y a
.11a b
Id11b d
,11d e
(11f g
!11g h
String11h n
.11n o
IsNullOrEmpty11o |
(11| }
objImage	11} �
.
11� �
	Extension
11� �
)
11� �
?
11� �
objImage
11� �
.
11� �
	Extension
11� �
:
11� �
$str
11� �
)
11� �
)
11� �
;
11� �
token22 
.22 
OriginalImagePath22 +
=22, -
IfNull22. 4
(224 5
objImage225 =
.22= >
OriginalPath22> J
,22J K
token22L Q
.22Q R
Key22R U
)22U V
;22V W
token33 
.33 
Policy33  
=33! "
awsToken33# +
.33+ ,
Policy33, 2
;332 3
token44 
.44 
	Signature44 #
=44$ %
awsToken44& .
.44. /
	Signature44/ 8
;448 9
token55 
.55 
AccessKeyId55 %
=55& '
awsToken55( 0
.550 1
AccessKeyId551 <
;55< =
token66 
.66 
URI66 
=66 
awsToken66  (
.66( )
URI66) ,
;66, -
token77 
.77 
Status77  
=77! "
true77# '
;77' (
token88 
.88 
DatetTmeISO88 %
=88& '
awsToken88( 0
.880 1
DatetTmeISO881 <
;88< =
token99 
.99 
DateTimeISOLong99 )
=99* +
awsToken99, 4
.994 5
DateTimeISOLong995 D
;99D E
}:: 
};; 
catch<< 
(<< 
	Exception<< 
ex<< 
)<<  
{== 

ErrorClass>> 
objErr>> !
=>>" #
new>>$ '

ErrorClass>>( 2
(>>2 3
ex>>3 5
,>>5 6
$str>>7 Q
)>>Q R
;>>R S
}?? 
return@@ 
token@@ 
;@@ 
}AA 	
privateHH 
StringHH 
IfNullHH 
(HH 
paramsHH $
stringHH% +
[HH+ ,
]HH, -
arrHH. 1
)HH1 2
{II 	
returnJJ 
arrJJ 
.JJ 
FirstOrDefaultJJ %
(JJ% &
mJJ& '
=>JJ( *
!JJ+ ,
StringJJ, 2
.JJ2 3
IsNullOrEmptyJJ3 @
(JJ@ A
mJJA B
)JJB C
)JJC D
;JJD E
}KK 	
publicUU 

ImageTokenUU 
ProcessImageUploadUU ,
(UU, -

ImageTokenUU- 7
tokenUU8 =
)UU= >
{VV 	
tryWW 
{XX 
varYY 
hashIdYY 
=YY 
tokenYY "
.YY" #
KeyYY# &
.YY& '
	SubstringYY' 0
(YY0 1
$numYY1 2
+YY3 4
_environmentYY5 A
.YYA B
LengthYYB H
,YYH I
BikewaleYYJ R
.YYR S
UtilityYYS Z
.YYZ [
BWOprConfigurationYY[ m
.YYm n
InstanceYYn v
.YYv w
SecurityHashLength	YYw �
)
YY� �
;
YY� �
if[[ 
([[ 
	_security[[ 
.[[ 

VerifyHash[[ (
([[( )
hashId[[) /
,[[/ 0
Convert[[1 8
.[[8 9
ToUInt32[[9 A
([[A B
token[[B G
.[[G H
Id[[H J
)[[J K
)[[K L
)[[L M
{\\ 
NameValueCollection]] '
objNVC]]( .
=]]/ 0
new]]1 4
NameValueCollection]]5 H
(]]H I
)]]I J
;]]J K
objNVC^^ 
.^^ 
Add^^ 
(^^ 
$str^^ #
,^^# $
token^^% *
.^^* +
Id^^+ -
.^^- .
ToString^^. 6
(^^6 7
)^^7 8
)^^8 9
;^^9 :
objNVC__ 
.__ 
Add__ 
(__ 
$str__ -
,__- .
$str__/ 2
+__3 4
IfNull__5 ;
(__; <
token__< A
.__A B
OriginalImagePath__B S
,__S T
token__U Z
.__Z [
Key__[ ^
)__^ _
)___ `
;__` a
objNVC`` 
.`` 
Add`` 
(`` 
$str`` (
,``( )
token``* /
.``/ 0
PhotoId``0 7
.``7 8
ToString``8 @
(``@ A
)``A B
)``B C
;``C D
RabbitMqPublishbb #
objRMQPublishbb$ 1
=bb2 3
newbb4 7
RabbitMqPublishbb8 G
(bbG H
)bbH I
;bbI J
tokencc 
.cc 
Statuscc  
=cc! "
objRMQPublishcc# 0
.cc0 1
PublishToQueuecc1 ?
(cc? @
_queuecc@ F
,ccF G
objNVCccH N
)ccN O
;ccO P
}dd 
}ee 
catchff 
(ff 
	Exceptionff 
exff 
)ff  
{gg 
tokenhh 
.hh 
Statushh 
=hh 
falsehh $
;hh$ %

ErrorClassii 
objErrii !
=ii" #
newii$ '

ErrorClassii( 2
(ii2 3
exii3 5
,ii5 6
$strii7 M
)iiM N
;iiN O
objErrjj 
.jj 
SendMailjj 
(jj  
)jj  !
;jj! "
}kk 
returnll 
tokenll 
;ll 
}mm 	
}nn 
}oo �*
>D:\work\bikewaleweb\BikewaleOpr.BAL\ManageBookingAmountPage.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
{		 
public 

class #
ManageBookingAmountPage (
:( )$
IManageBookingAmountPage* B
{ 
private 
readonly  
IBikeMakesRepository -
_bikeMakesRepo. <
== >
null? C
;C D
private 
readonly 
IDealers !
_dealers" *
=+ ,
null- 1
;1 2
public #
ManageBookingAmountPage &
(& ' 
IBikeMakesRepository' ;
bikeMakesRepo< I
,I J
IDealersK S
dealersT [
)[ \
{ 	
_bikeMakesRepo 
= 
bikeMakesRepo *
;* +
_dealers 
= 
dealers 
; 
} 	
public #
ManageBookingAmountData &&
GetManageBookingAmountData' A
(A B
UInt32B H
dealerIdI Q
)Q R
{   	#
ManageBookingAmountData!! #&
objManageBookingAmountData!!$ >
=!!? @
null!!A E
;!!E F
try"" 
{## 
if$$ 
($$ 
dealerId$$ 
>$$ 
$num$$ 
)$$  
{%% &
objManageBookingAmountData&& .
=&&/ 0
new&&1 4#
ManageBookingAmountData&&5 L
(&&L M
)&&M N
;&&N O&
objManageBookingAmountData'' .
.''. /
BikeMakeList''/ ;
=''< =
_bikeMakesRepo''> L
.''L M
GetMakes''M U
(''U V
$num''V W
)''W X
;''X Y&
objManageBookingAmountData(( .
.((. /
BookingAmountList((/ @
=((A B
_dealers((C K
.((K L 
GetBikeBookingAmount((L `
(((` a
dealerId((a i
)((i j
;((j k
})) 
}** 
catch++ 
(++ 
	Exception++ 
ex++ 
)++  
{,, 

ErrorClass-- 
objErr-- !
=--" #
new--$ '

ErrorClass--( 2
(--2 3
ex--3 5
,--5 6
string--7 =
.--= >
Format--> D
(--D E
$str--E |
,--| }
dealerId	--~ �
)
--� �
)
--� �
;
--� �
}.. 
return// &
objManageBookingAmountData// -
;//- .
}00 	
public88 
bool88 
AddBookingAmount88 $
(88$ %
BookingAmountEntity88% 8"
objBookingAmountEntity889 O
,88O P
string88Q W
	updatedBy88X a
)88a b
{99 	
bool:: 
	isUpdated:: 
=:: 
false:: "
;::" #
try;; 
{<< 
if== 
(== "
objBookingAmountEntity== )
!===* ,
null==- 1
&&==2 4"
objBookingAmountEntity==5 K
.==K L
BookingAmountBase==L ]
!===^ `
null==a e
)==e f
{>> "
objBookingAmountEntity?? *
.??* +
	UpdatedOn??+ 4
=??5 6
DateTime??7 ?
.??? @
Now??@ C
;??C D
UInt32@@ 
updatedById@@ &
=@@' (
$num@@) *
;@@* +
UInt32AA 
.AA 
TryParseAA #
(AA# $
	updatedByAA$ -
,AA- .
outAA/ 2
updatedByIdAA3 >
)AA> ?
;AA? @
ifBB 
(BB "
objBookingAmountEntityBB .
.BB. /
	BikeModelBB/ 8
==BB9 ;
nullBB< @
)BB@ A
{CC "
objBookingAmountEntityDD .
.DD. /
	BikeModelDD/ 8
=DD9 :
newDD; >
BikeModelEntityBaseDD? R
(DDR S
)DDS T
{EE 
ModelIdFF #
=FF$ %
$numFF& '
}GG 
;GG "
objBookingAmountEntityHH .
.HH. /
BikeVersionHH/ :
=HH; <
newHH= @!
BikeVersionEntityBaseHHA V
(HHV W
)HHW X
{II 
	VersionIdJJ %
=JJ& '
$numJJ( )
}KK 
;KK 
}LL 
ifMM 
(MM "
objBookingAmountEntityMM .
.MM. /
DealerIdMM/ 7
>MM8 9
$numMM: ;
&&MM< >"
objBookingAmountEntityMM? U
.MMU V
BookingAmountBaseMMV g
.MMg h
AmountMMh n
>=MMo q
$numMMr s
&&MMt v
updatedById	MMw �
>
MM� �
$num
MM� �
)
MM� �
{NN 
	isUpdatedOO !
=OO" #
_dealersOO$ ,
.OO, -
SaveBookingAmountOO- >
(OO> ?"
objBookingAmountEntityOO? U
,OOU V
updatedByIdOOW b
)OOb c
;OOc d
}PP 
}QQ 
}RR 
catchSS 
(SS 
	ExceptionSS 
exSS 
)SS  
{TT 

ErrorClassUU 
objErrUU !
=UU" #
newUU$ '

ErrorClassUU( 2
(UU2 3
exUU3 5
,UU5 6
$strUU7 q
)UUq r
;UUr s
}VV 
returnWW 
	isUpdatedWW 
;WW 
}XX 	
}YY 
}ZZ �
GD:\work\bikewaleweb\BikewaleOpr.BAL\ManufacturerReleaseMaskingNumber.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
{ 
public		 

class		 ,
 ManufacturerReleaseMaskingNumber		 1
:		2 3-
!IManufacturerReleaseMaskingNumber		4 U
{

 
public 
bool 
ReleaseNumber !
(! "
uint" &
dealerId' /
,/ 0
int1 4

campaignId5 ?
,? @
stringA G
maskingNumberH U
,U V
intW Z
userId[ a
)a b
{ 	
bool 
	isSuccess 
= 
false "
;" #
try 
{ 
using 
( 
IUnityContainer &
	container' 0
=1 2
new3 6
UnityContainer7 E
(E F
)F G
)G H
{ 
	container 
. 
RegisterType *
<* ++
IManufacturerCampaignRepository+ J
,J K
BikewaleOprL W
.W X
DALsX \
.\ ]
ManufactureCampaign] p
.p q!
ManufacturerCampaign	q �
>
� �
(
� �
)
� �
;
� �
	container 
. 
RegisterType *
<* +
IContractCampaign+ <
,< =
BikewaleOpr> I
.I J
BALJ M
.M N
ContractCampaignN ^
.^ _
ContractCampaign_ o
>o p
(p q
)q r
;r s+
IManufacturerCampaignRepository 3
objMfgCampaign4 B
=C D
	containerE N
.N O
ResolveO V
<V W+
IManufacturerCampaignRepositoryW v
>v w
(w x
)x y
;y z
IContractCampaign % 
_objContractCampaign& :
=; <
	container= F
.F G
ResolveG N
<N O
IContractCampaignO `
>` a
(a b
)b c
;c d
	isSuccess 
=  
_objContractCampaign  4
.4 5 
RelaseMaskingNumbers5 I
(I J
dealerIdJ R
,R S
userIdT Z
,Z [
maskingNumber\ i
)i j
;j k
if 
( 
	isSuccess !
)! "
{ 
	isSuccess !
=" #
objMfgCampaign$ 2
.2 3(
ReleaseCampaignMaskingNumber3 O
(O P

campaignIdP Z
)Z [
;[ \
} 
} 
} 
catch   
(   
	Exception   
ex   
)    
{!! 

ErrorClass"" 
objErr"" !
=""" #
new""$ '

ErrorClass""( 2
(""2 3
ex""3 5
,""5 6
$str""7 w
)""w x
;""x y
objErr## 
.## 
SendMail## 
(##  
)##  !
;##! "
}$$ 
return%% 
	isSuccess%% 
;%% 
}&& 	
}'' 
}(( �

4D:\work\bikewaleweb\BikewaleOpr.BAL\MemCachedUtil.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
{ 
public 

static 
class 
MemCachedUtil %
{ 
static 
MemcachedClient 
_mc "
=# $
null% )
;) *
public 
static 
void 
Remove !
(! "
string" (
key) ,
), -
{ 	
try 
{ 
if 
( 
_mc 
== 
null 
)  
{ 
_mc 
= 
new 
MemcachedClient -
(- .
$str. 9
)9 :
;: ;
} 
_mc 
. 
Remove 
( 
key 
) 
;  
} 
catch 
( 
	Exception 
ex 
) 
{ 

ErrorClass 
objErr !
=" #
new$ '

ErrorClass( 2
(2 3
ex3 5
,5 6
$str7 V
)V W
;W X
objErr 
. 
SendMail 
(  
)  !
;! "
} 
} 	
} 
}   �
:D:\work\bikewaleweb\BikewaleOpr.BAL\PageMetas\PageMetas.cs
	namespace		 	
BikewaleOpr		
 
.		 
BAL		 
{

 
public 

class 
	PageMetas 
: 

IPageMetas '
{ 
private 
readonly  
IPageMetasRepository -

_pageMetas. 8
=9 :
null; ?
;? @
public 
	PageMetas 
(  
IPageMetasRepository -
	PageMetas. 7
)7 8
{ 	

_pageMetas 
= 
	PageMetas "
;" #
} 	
public 
bool  
UpdatePageMetaStatus (
(( )
uint) -
id. 0
,0 1
ushort2 8
status9 ?
)? @
{ 	
return 

_pageMetas 
.  
UpdatePageMetaStatus 2
(2 3
id3 5
,5 6
status7 =
)= >
;> ?
} 	
} 
} �
>D:\work\bikewaleweb\BikewaleOpr.BAL\Properties\AssemblyInfo.cs
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
]$$) *�`
:D:\work\bikewaleweb\BikewaleOpr.BAL\Security\SecurityBL.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
. 
Security "
{		 
public 

class 

SecurityBL 
: 
	ISecurity '
{ 
	protected 
string 
	signature "
;" #
	protected 
string 
policyJSONBase64 )
;) *
private 
readonly 
string 
awsAccessKeyId  .
=/ 0
Bikewale1 9
.9 :
Utility: A
.A B
BWOprConfigurationB T
.T U
InstanceU ]
.] ^
AWSAccessKey^ j
;j k
private 
readonly 
string 

bucketName  *
=+ ,
Bikewale- 5
.5 6
Utility6 =
.= >
BWOprConfiguration> P
.P Q
InstanceQ Y
.Y Z
AWSBucketNameZ g
;g h
private 
readonly 
string 
	secretKey  )
=* +
Bikewale, 4
.4 5
Utility5 <
.< =
BWOprConfiguration= O
.O P
InstanceP X
.X Y
AWSSecretKeyY e
;e f
private 
const 
string 
SEED !
=" #
$str$ J
;J K
private 
const 
int 
	baseCount #
=$ %
$num& (
;( )
private 
readonly 
int 

HashLength '
=( )
Bikewale* 2
.2 3
Utility3 :
.: ;
BWOprConfiguration; M
.M N
InstanceN V
.V W
SecurityHashLengthW i
;i j
private 
readonly 
string 
s3region  (
=) *
Bikewale+ 3
.3 4
Utility4 ;
.; <
BWConfiguration< K
.K L
InstanceL T
.T U
AWSS3RegionU `
;` a
public 
Token 
GetToken 
( 
) 
{   	
Token!! 
objAwsToken!! 
=!! 
null!!  $
;!!$ %
try"" 
{## 
string$$ 
[$$ 
]$$ 
tokens$$ 
=$$  !
new$$" %
string$$& ,
[$$, -
$num$$- .
]$$. /
;$$/ 0
string%% 
	signature%%  
=%%! "
$str%%# %
,%%% &
policyJSONBase64%%' 7
=%%8 9
$str%%: <
,%%< =
awsURI%%> D
=%%E F
$str%%G I
;%%I J
DateTime&& 
now&& 
=&& 
DateTime&& '
.&&' (
Now&&( +
;&&+ ,
string(( 
datetimeiso(( "
=((# $
now((% (
.((( )
ToString(() 1
(((1 2
$str((2 <
)((< =
;((= >
string)) 
datetimeisolong)) &
=))' (
now))) ,
.)), -
ToString))- 5
())5 6
$str))6 H
)))H I
;))I J
awsURI** 
=** 
String** 
.**  
Format**  &
(**& '
$str**' F
,**F G

bucketName**H R
)**R S
;**S T
string++ 

jsonPolicy++ !
=++" #
String++$ *
.++* +
Format+++ 1
(++1 2
$str	++2 �
,
++� �
DateTime
++� �
.
++� �
Now
++� �
.
++� �
AddHours
++� �
(
++� �
$num
++� �
)
++� �
.
++� �
ToString
++� �
(
++� �
$str
++� �
)
++� �
,
++� �

bucketName
++� �
,
++� �
awsAccessKeyId
++� �
,
++� �
datetimeiso
++� �
,
++� �
s3region
++� �
,
++� �
datetimeisolong
++� �
)
++� �
;
++� �
policyJSONBase64--  
=--! "
Convert--# *
.--* +
ToBase64String--+ 9
(--9 :
Encoding--: B
.--B C
UTF8--C G
.--G H
GetBytes--H P
(--P Q

jsonPolicy--Q [
)--[ \
)--\ ]
;--] ^
string// 
secret// 
=// 
	secretKey//  )
;//) *
byte00 
[00 
]00 
kSecret00 
=00  
Encoding00! )
.00) *
UTF800* .
.00. /
GetBytes00/ 7
(007 8
(008 9
$str009 ?
+00@ A
secret00B H
)00H I
.00I J
ToCharArray00J U
(00U V
)00V W
)00W X
;00X Y
byte11 
[11 
]11 
kDate11 
=11 

HmacSHA25611 )
(11) *
datetimeiso11* 5
,115 6
kSecret117 >
)11> ?
;11? @
byte22 
[22 
]22 
kRegion22 
=22  

HmacSHA25622! +
(22+ ,
s3region22, 4
,224 5
kDate226 ;
)22; <
;22< =
byte33 
[33 
]33 
kService33 
=33  !

HmacSHA25633" ,
(33, -
$str33- 1
,331 2
kRegion333 :
)33: ;
;33; <
byte44 
[44 
]44 
kSigning44 
=44  !

HmacSHA25644" ,
(44, -
$str44- ;
,44; <
kService44= E
)44E F
;44F G
byte55 
[55 
]55 
signatureBytes55 %
=55& '

HmacSHA25655( 2
(552 3
policyJSONBase64553 C
,55C D
kSigning55E M
)55M N
;55N O
	signature66 
=66 
BitConverter66 (
.66( )
ToString66) 1
(661 2
signatureBytes662 @
)66@ A
.66A B
Replace66B I
(66I J
$str66J M
,66M N
$str66O Q
)66Q R
.66R S
ToLower66S Z
(66Z [
)66[ \
;66\ ]
objAwsToken88 
=88 
new88 !
Token88" '
(88' (
)88( )
{99 
Policy:: 
=:: 
policyJSONBase64:: -
,::- .
	Signature;; 
=;; 
	signature;;  )
,;;) *
AccessKeyId<< 
=<<  !
awsAccessKeyId<<" 0
,<<0 1
URI== 
=== 
awsURI==  
,==  !
DatetTmeISO>> 
=>>  !
datetimeiso>>" -
,>>- .
DateTimeISOLong?? #
=??$ %
datetimeisolong??& 5
}@@ 
;@@ 
}AA 
catchBB 
(BB 
	ExceptionBB 
exBB 
)BB  
{CC 

ErrorClassDD 
objErrDD !
=DD" #
newDD$ '

ErrorClassDD( 2
(DD2 3
exDD3 5
,DD5 6
$strDD7 L
)DDL M
;DDM N
}EE 
returnFF 
objAwsTokenFF 
;FF 
}GG 	
staticII 
byteII 
[II 
]II 

HmacSHA256II  
(II  !
StringII! '
dataII( ,
,II, -
byteII. 2
[II2 3
]II3 4
keyII5 8
)II8 9
{JJ 	
varKK 
hashKK 
=KK 
newKK 

HMACSHA256KK %
(KK% &
keyKK& )
)KK) *
;KK* +
returnLL 
hashLL 
.LL 
ComputeHashLL #
(LL# $
EncodingLL$ ,
.LL, -
UTF8LL- 1
.LL1 2
GetBytesLL2 :
(LL: ;
dataLL; ?
)LL? @
)LL@ A
;LLA B
}MM 	
publicUU 
stringUU 
GenerateHashUU "
(UU" #
uintUU# '
uniqueIdUU( 0
)UU0 1
{VV 	
stringWW 
baseHashWW 
=WW 
$strWW  
;WW  !
charXX 
[XX 
]XX 
hashPaddingXX 
=XX  
nullXX! %
;XX% &
tryYY 
{ZZ 
while[[ 
([[ 
uniqueId[[ 
!=[[  "
$num[[# $
)[[$ %
{\\ 
baseHash]] 
+=]] 
SEED]]  $
[]]$ %
(]]% &
int]]& )
)]]) *
(]]* +
uniqueId]]+ 3
%]]4 5
	baseCount]]6 ?
)]]? @
]]]@ A
;]]A B
uniqueId^^ 
/=^^ 
	baseCount^^  )
;^^) *
}__ 
intaa 

hashLengthaa 
=aa  
baseHashaa! )
.aa) *
Lengthaa* 0
;aa0 1
hashPaddingbb 
=bb 
newbb !
charbb" &
[bb& '

HashLengthbb' 1
-bb2 3

hashLengthbb4 >
]bb> ?
;bb? @
Randomdd 
objRanddd 
=dd  
newdd! $
Randomdd% +
(dd+ ,
)dd, -
;dd- .
foree 
(ee 
intee 
iee 
=ee 
$numee 
;ee 
iee  !
<ee" #

HashLengthee$ .
-ee/ 0

hashLengthee1 ;
;ee; <
iee= >
++ee> @
)ee@ A
{ff 
hashPaddinggg 
[gg  
igg  !
]gg! "
=gg# $
SEEDgg% )
[gg) *
objRandgg* 1
.gg1 2
Nextgg2 6
(gg6 7
SEEDgg7 ;
.gg; <
Lengthgg< B
)ggB C
]ggC D
;ggD E
}hh 
}ii 
catchjj 
(jj 
	Exceptionjj 
exjj 
)jj  
{kk 

ErrorClassll 
objErrll !
=ll" #
newll$ '

ErrorClassll( 2
(ll2 3
exll3 5
,ll5 6
$strll7 S
+llT U
uniqueIdllV ^
)ll^ _
;ll_ `
}mm 
returnnn 
(nn 
newnn 
stringnn 
(nn 
hashPaddingnn *
)nn* +
+nn, -
baseHashnn. 6
)nn6 7
;nn7 8
}oo 	
publicxx 
boolxx 

VerifyHashxx 
(xx 
stringxx %
	hashValuexx& /
,xx/ 0
uintxx1 5
uniqueIdxx6 >
)xx> ?
{yy 	
boolzz 
isEqualzz 
=zz 
falsezz  
;zz  !
string{{ 
newHash{{ 
={{ 
$str{{ 
;{{  
try}} 
{~~ 
while 
( 
uniqueId 
!=  "
$num# $
)$ %
{
�� 
newHash
�� 
+=
�� 
SEED
�� #
[
��# $
(
��$ %
int
��% (
)
��( )
(
��) *
uniqueId
��* 2
%
��3 4
	baseCount
��5 >
)
��> ?
]
��? @
;
��@ A
uniqueId
�� 
/=
�� 
	baseCount
��  )
;
��) *
}
�� 
if
�� 
(
�� 
newHash
�� 
==
�� 
	hashValue
�� (
.
��( )
	Substring
��) 2
(
��2 3
	hashValue
��3 <
.
��< =
Length
��= C
-
��D E
newHash
��F M
.
��M N
Length
��N T
)
��T U
)
��U V
isEqual
�� 
=
�� 
true
�� "
;
��" #
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
$str
��7 F
+
��G H
	hashValue
��I R
)
��R S
;
��S T
}
�� 
return
�� 
isEqual
�� 
;
�� 
}
�� 	
}
�� 
}�� �i
BD:\work\bikewaleweb\BikewaleOpr.BAL\ServiceCenter\ServiceCenter.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
. 
ServiceCenter '
{ 
public 

class 
ServiceCenter 
:  
IServiceCenter! /
{ 
private 

IBikeMakes 

_IBikeMake %
;% &
private $
IServiceCenterRepository (
_IServiceCenter) 8
;8 9
public 
ServiceCenter 
( 

IBikeMakes '
bikeMake( 0
,0 1$
IServiceCenterRepository2 J
serviceCenterK X
)X Y
{ 	

_IBikeMake 
= 
bikeMake !
;! "
_IServiceCenter 
= 
serviceCenter +
;+ ,
} 	
public@@ 
IEnumerable@@ 
<@@ 
CityEntityBase@@ )
>@@) *"
GetServiceCenterCities@@+ A
(@@A B
uint@@B F
makeId@@G M
)@@M N
{AA 	
IEnumerableBB 
<BB 
CityEntityBaseBB &
>BB& '
objCityListBB( 3
=BB4 5
nullBB6 :
;BB: ;
tryCC 
{DD 
ifEE 
(EE 
_IServiceCenterEE #
!=EE$ &
nullEE' +
&&EE, .
makeIdEE/ 5
>EE6 7
$numEE8 9
)EE9 :
{FF 
objCityListGG 
=GG  !
_IServiceCenterGG" 1
.GG1 2"
GetServiceCenterCitiesGG2 H
(GGH I
makeIdGGI O
)GGO P
;GGP Q
}HH 
}II 
catchJJ 
(JJ 
	ExceptionJJ 
exJJ 
)JJ  
{KK 
BikewaleLL 
.LL 
NotificationsLL &
.LL& '

ErrorClassLL' 1
objErrLL2 8
=LL9 :
newLL; >
BikewaleLL? G
.LLG H
NotificationsLLH U
.LLU V

ErrorClassLLV `
(LL` a
exLLa c
,LLc d
$str	LLe �
)
LL� �
;
LL� �
}NN 
returnOO 
objCityListOO 
;OO 
}PP 	
publicZZ 
ServiceCenterDataZZ  '
GetServiceCentersByCityMakeZZ! <
(ZZ< =
uintZZ= A
cityIdZZB H
,ZZH I
uintZZJ N
makeIdZZO U
,ZZU V
sbyteZZW \
activeStatusZZ] i
)ZZi j
{[[ 	
ServiceCenterData\\  
objServiceCenterData\\ 2
=\\3 4
null\\5 9
;\\9 :
try]] 
{^^ 
if__ 
(__ 
_IServiceCenter__ #
!=__$ &
null__' +
&&__, .
cityId__/ 5
>__6 7
$num__8 9
&&__: <
makeId__= C
>__D E
$num__F G
)__G H
{``  
objServiceCenterDataaa (
=aa) *
_IServiceCenteraa+ :
.aa: ;'
GetServiceCentersByCityMakeaa; V
(aaV W
cityIdaaW ]
,aa] ^
makeIdaa_ e
,aaf g
activeStatusaah t
)aat u
;aau v
}bb 
}cc 
catchdd 
(dd 
	Exceptiondd 
exdd 
)dd  
{ee 
Bikewaleff 
.ff 
Notificationsff &
.ff& '

ErrorClassff' 1
objErrff2 8
=ff9 :
newff; >
Bikewaleff? G
.ffG H
NotificationsffH U
.ffU V

ErrorClassffV `
(ff` a
exffa c
,ffc d
$str	ffe �
)
ff� �
;
ff� �
}hh 
returnii  
objServiceCenterDataii '
;ii' (
}jj 	
publicqq 
boolqq "
AddUpdateServiceCenterqq *
(qq* +%
ServiceCenterCompleteDataqq+ D 
serviceCenterDetailsqqE Y
,qqY Z
stringqq[ a
	updatedByqqb k
)qqk l
{rr 	
boolss 
statusss 
=ss 
falsess 
;ss  
tryuu 
{vv 
ifww 
(ww 
_IServiceCenterww "
!=ww# %
nullww& *
&&ww+ - 
serviceCenterDetailsww. B
!=wwC E
nullwwE I
)wwI J
{xx 
statusyy 
=yy 
_IServiceCenteryy +
.yy+ ,"
AddUpdateServiceCenteryy, B
(yyB C 
serviceCenterDetailsyyC W
,yyW X
	updatedByyyY b
)yyb c
;yyc d
ifzz 
(zz 
statuszz 
)zz 
{{{ 
MemCachedUtil|| %
.||% &
Remove||& ,
(||, -
string||- 3
.||3 4
Format||4 :
(||: ;
$str||; Z
,||Z [ 
serviceCenterDetails||\ p
.||p q
MakeId||q w
)||w x
)||x y
;||y z
MemCachedUtil}} %
.}}% &
Remove}}& ,
(}}, -
string}}- 3
.}}3 4
Format}}4 :
(}}: ;
$str}}; X
,}}X Y 
serviceCenterDetails}}Z n
.}}n o
MakeId}}o u
)}}u v
)}}v w
;}}w x
MemCachedUtil~~ %
.~~% &
Remove~~& ,
(~~, -
string~~- 3
.~~3 4
Format~~4 :
(~~: ;
$str~~; c
,~~c d 
serviceCenterDetails~~e y
.~~y z
Location	~~z �
.
~~� �
CityId
~~� �
,
~~� �"
serviceCenterDetails
~~� �
.
~~� �
MakeId
~~� �
)
~~� �
)
~~� �
;
~~� �
}
�� 
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Bikewale
�� 
.
�� 
Notifications
�� &
.
��& '

ErrorClass
��' 1
objErr
��2 8
=
��9 :
new
��; >
Bikewale
��? G
.
��G H
Notifications
��H U
.
��U V

ErrorClass
��V `
(
��` a
ex
��a c
,
��c d
$str��e �
)��� �
;��� �
}
�� 
return
�� 
status
�� 
;
�� 
}
�� 	
public
�� 
bool
�� '
UpdateServiceCenterStatus
�� -
(
��- .
uint
��. 2
cityId
��3 9
,
��9 :
uint
��; ?
makeId
��@ F
,
��F G
uint
��G K
serviceCenterId
��L [
,
��[ \
string
��] c
currentUserId
��d q
)
��q r
{
�� 	
bool
�� 
status
�� 
=
�� 
false
�� 
;
��  
try
�� 
{
�� 
if
�� 
(
�� 
_IServiceCenter
�� "
!=
��# %
null
��& *
&&
��+ -
serviceCenterId
��. =
>
��> ?
$num
��@ A
)
��A B
{
�� 
status
�� 
=
�� 
_IServiceCenter
�� ,
.
��, -'
UpdateServiceCenterStatus
��- F
(
��F G
serviceCenterId
��G V
,
��V W
currentUserId
��X e
)
��e f
;
��f g
if
�� 
(
�� 
status
�� 
)
�� 
{
�� 
MemCachedUtil
�� %
.
��% &
Remove
��& ,
(
��, -
string
��- 3
.
��3 4
Format
��4 :
(
��: ;
$str
��; U
,
��U V
serviceCenterId
��W f
)
��f g
)
��g h
;
��h i
MemCachedUtil
�� %
.
��% &
Remove
��& ,
(
��, -
string
��- 3
.
��3 4
Format
��4 :
(
��: ;
$str
��; Z
,
��Z [
makeId
��\ b
)
��b c
)
��c d
;
��d e
MemCachedUtil
�� %
.
��% &
Remove
��& ,
(
��, -
string
��- 3
.
��3 4
Format
��4 :
(
��: ;
$str
��; X
,
��X Y
makeId
��Z `
)
��` a
)
��a b
;
��b c
MemCachedUtil
�� %
.
��% &
Remove
��& ,
(
��, -
string
��- 3
.
��3 4
Format
��4 :
(
��: ;
$str
��; c
,
��c d
cityId
��e k
,
��k l
makeId
��m s
)
��s t
)
��t u
;
��u v
}
�� 
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Bikewale
�� 
.
�� 
Notifications
�� &
.
��& '

ErrorClass
��' 1
objErr
��2 8
=
��9 :
new
��; >
Bikewale
��? G
.
��G H
Notifications
��H U
.
��U V

ErrorClass
��V `
(
��` a
ex
��a c
,
��c d
$str��e �
)��� �
;��� �
}
�� 
return
�� 
status
�� 
;
�� 
}
�� 	
public
�� 
IEnumerable
�� 
<
�� 
CityEntityBase
�� )
>
��) *
GetAllCities
��+ 7
(
��7 8
)
��8 9
{
�� 	
IEnumerable
�� 
<
�� 
CityEntityBase
�� &
>
��& '
objCityList
��( 3
=
��4 5
null
��6 :
;
��: ;
try
�� 
{
�� 
if
�� 
(
�� 
_IServiceCenter
�� #
!=
��$ &
null
��' +
)
��, -
{
�� 
objCityList
�� 
=
��  !
_IServiceCenter
��" 1
.
��1 2
GetAllCities
��2 >
(
��> ?
)
��? @
;
��@ A
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Bikewale
�� 
.
�� 
Notifications
�� &
.
��& '

ErrorClass
��' 1
objErr
��2 8
=
��9 :
new
��; >
Bikewale
��? G
.
��G H
Notifications
��H U
.
��U V

ErrorClass
��V `
(
��` a
ex
��a c
,
��c d
$str��e �
)��� �
;��� �
}
�� 
return
�� 
objCityList
�� 
;
�� 
}
�� 	
public
�� '
ServiceCenterCompleteData
�� ()
GetServiceCenterDetailsbyId
��) D
(
��D E
uint
��E I
serviceCenterId
��J Y
)
��Y Z
{
�� 	'
ServiceCenterCompleteData
�� %
objData
��& -
=
��. /
new
��0 3'
ServiceCenterCompleteData
��4 M
(
��M N
)
��N O
;
��O P
try
�� 
{
�� 
if
�� 
(
�� 
_IServiceCenter
�� #
!=
��$ &
null
��' +
)
��+ ,
{
�� 
objData
�� 
=
�� 
_IServiceCenter
�� -
.
��- .
GetDataById
��. 9
(
��9 :
serviceCenterId
��: I
)
��I J
;
��J K
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Bikewale
�� 
.
�� 
Notifications
�� &
.
��& '

ErrorClass
��' 1
objErr
��2 8
=
��9 :
new
��; >
Bikewale
��? G
.
��G H
Notifications
��H U
.
��U V

ErrorClass
��V `
(
��` a
ex
��a c
,
��c d
$str��e �
)��� �
;��� �
}
�� 
return
�� 
objData
�� 
;
�� 
}
�� 	
public
�� 
StateCityEntity
�� #
GetStateDetailsByCity
�� 4
(
��4 5
uint
��5 9
cityId
��: @
)
��@ A
{
�� 	
StateCityEntity
�� 
objData
�� #
=
��$ %
new
��& )
StateCityEntity
��* 9
(
��9 :
)
��: ;
;
��; <
try
�� 
{
�� 
if
�� 
(
�� 
_IServiceCenter
�� "
!=
��# %
null
��& *
&&
��+ -
cityId
��. 4
>
��4 5
$num
��5 6
)
��6 7
{
�� 
objData
�� 
=
�� 
_IServiceCenter
�� -
.
��- .
GetStateDetails
��. =
(
��= >
cityId
��> D
)
��D E
;
��E F
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Bikewale
�� 
.
�� 
Notifications
�� &
.
��& '

ErrorClass
��' 1
objErr
��2 8
=
��9 :
new
��; >
Bikewale
��? G
.
��G H
Notifications
��H U
.
��U V

ErrorClass
��V `
(
��` a
ex
��a c
,
��c d
$str��e �
)��� �
;��� �
}
�� 
return
�� 
objData
�� 
;
�� 
}
�� 	
}
�� 
}�� �6
5D:\work\bikewaleweb\BikewaleOpr.BAL\Used\SellBikes.cs
	namespace		 	
BikewaleOpr		
 
.		 
BAL		 
.		 
Used		 
{

 
public 

class 
	SellBikes 
: 

ISellBikes '
{ 
private 
ISellerRepository !
_sellerRepo" -
;- .
public 
	SellBikes 
( 
ISellerRepository *
sellerRepository+ ;
); <
{ 	
_sellerRepo 
= 
sellerRepository *
;* +
} 	
public 
IEnumerable 
< 

SellBikeAd %
>% &)
GetClassifiedPendingInquiries' D
(D E
)E F
{ 	
return 
_sellerRepo 
. )
GetClassifiedPendingInquiries <
(< =
)= >
;> ?
} 	
public$$ 
bool$$ 
SaveEditedInquiry$$ %
($$% &
uint$$& *
	inquiryId$$+ 4
,$$4 5
short$$6 ;

isApproved$$< F
,$$F G
int$$H K

approvedBy$$L V
,$$V W
string$$X ^
	profileId$$_ h
,$$h i
string$$j p
bikeName$$q y
,$$y z
uint$${ 
modelId
$$� �
)
$$� �
{%% 	
bool&& 
	isSuccess&& 
=&& 
_sellerRepo&& (
.&&( )
SaveEditedInquiry&&) :
(&&: ;
	inquiryId&&; D
,&&D E

isApproved&&F P
,&&P Q

approvedBy&&R \
)&&\ ]
;&&] ^
if'' 
('' 
	isSuccess'' 
)'' 
{(( 
MemCachedUtil)) 
.)) 
Remove)) $
())$ %
string))% +
.))+ ,
Format)), 2
())2 3
$str))3 J
,))J K
	inquiryId))L U
)))U V
)))V W
;))W X"
UsedBikeProfileDetails** &
seller**' -
=**. /
_sellerRepo**0 ;
.**; <$
GetUsedBikeSellerDetails**< T
(**T U
(**U V
int**V Y
)**Y Z
	inquiryId**Z c
,**c d
false**e j
)**j k
;**k l
if++ 
(++ 
seller++ 
!=++ 
null++ "
)++" #
{,, 
SMSTypes-- 
newSms-- #
=--$ %
new--& )
SMSTypes--* 2
(--2 3
)--3 4
;--4 5
string.. 

modelImage.. %
=..& '
Bikewale..( 0
...0 1
Utility..1 8
...8 9
Image..9 >
...> ?
GetPathToShowImages..? R
(..R S
seller..S Y
...Y Z
OriginalImagePath..Z k
,..k l
seller..m s
...s t
HostUrl..t {
,..{ |
Bikewale	..} �
.
..� �
Utility
..� �
.
..� �
	ImageSize
..� �
.
..� �
_110x61
..� �
)
..� �
;
..� �
if// 
(// 

isApproved// "
==//# %
$num//& '
)//' (
{00 (
SendEmailSMSToDealerCustomer11 4
.114 50
$UsedBikeEditedRejectionEmailToSeller115 Y
(11Y Z
seller11Z `
.11` a
SellerDetails11a n
,11n o
	profileId11p y
,11y z
bikeName	11{ �
,
11� �

modelImage
11� �
,
11� �
Format
11� �
.
11� �
FormatNumeric
11� �
(
11� �
seller
11� �
.
11� �
RideDistance
11� �
)
11� �
)
11� �
;
11� �
newSms22 
.22 -
!RejectionEditedUsedSellListingSMS22 @
(22@ A
EnumSMSServiceType33 .
.33. /2
&RejectionEditedUsedBikeListingToSeller33/ U
,33U V
seller44 "
.44" #
SellerDetails44# 0
.440 1
CustomerMobile441 ?
,44? @
	profileId55 %
,55% &
seller66 "
.66" #
SellerDetails66# 0
.660 1
CustomerName661 =
,66= >
HttpContext77 '
.77' (
Current77( /
.77/ 0
Request770 7
.777 8
ServerVariables778 G
[77G H
$str77H M
]77M N
)88 
;88 
}99 
else:: 
{;; 
string<< 
qEncoded<< '
=<<( )
Utils<<* /
.<</ 0
Utils<<0 5
.<<5 6
EncryptTripleDES<<6 F
(<<F G
string<<G M
.<<M N
Format<<N T
(<<T U
$str<<U c
,<<c d
(<<e f
int<<f i
)<<i j
Bikewale<<j r
.<<r s
Entities<<s {
.<<{ |
UserReviews	<<| �
.
<<� �&
UserReviewPageSourceEnum
<<� �
.
<<� �
UsedBikes_Email
<<� �
)
<<� �
)
<<� �
;
<<� �
string>> 
writeReview>> *
=>>+ ,
string>>- 3
.>>3 4
Format>>4 :
(>>: ;
$str>>; Z
,>>Z [
Bikewale>>\ d
.>>d e
Utility>>e l
.>>l m
BWOprConfiguration>>m 
.	>> �
Instance
>>� �
.
>>� �
	BwHostUrl
>>� �
,
>>� �
modelId
>>� �
,
>>� �
qEncoded
>>� �
)
>>� �
;
>>� �(
SendEmailSMSToDealerCustomer?? 4
.??4 5/
#UsedBikeEditedApprovalEmailToSeller??5 X
(??X Y
seller??Y _
.??_ `
SellerDetails??` m
,??m n
	profileId??o x
,??x y
bikeName	??z �
,
??� �

modelImage
??� �
,
??� �
Format
??� �
.
??� �
FormatNumeric
??� �
(
??� �
seller
??� �
.
??� �
RideDistance
??� �
)
??� �
,
??� �
writeReview
??� �
)
??� �
;
??� �
newSms@@ 
.@@ ,
 ApprovalEditedUsedSellListingSMS@@ ?
(@@? @
EnumSMSServiceTypeAA .
.AA. /1
%ApprovalEditedUsedBikeListingToSellerAA/ T
,AAT U
sellerBB "
.BB" #
SellerDetailsBB# 0
.BB0 1
CustomerMobileBB1 ?
,BB? @
	profileIdCC %
,CC% &
sellerDD "
.DD" #
SellerDetailsDD# 0
.DD0 1
CustomerNameDD1 =
,DD= >
HttpContextEE '
.EE' (
CurrentEE( /
.EE/ 0
RequestEE0 7
.EE7 8
ServerVariablesEE8 G
[EEG H
$strEEH M
]EEM N
)FF 
;FF 
}GG 
}HH 
}II 
returnJJ 
	isSuccessJJ 
;JJ 
}KK 	
}LL 
}MM �
5D:\work\bikewaleweb\BikewaleOpr.BAL\Used\UsedBikes.cs
	namespace 	
BikewaleOpr
 
. 
BAL 
. 
Used 
{ 
public 

class 
	UsedBikes 
: 

IUsedBikes '
{ 
public 
bool 
SendUnitSoldEmail %
(% &
SoldUnitData& 2

dataObject3 =
,= >
string? E
currentUserNameF U
)U V
{ 	
bool 
isShownNotification $
=% &
false' ,
;, -
if 
( 

dataObject 
. 
IsEmailToSend (
)( )
{ 
string 
ccList 
= 
Bikewale  (
.( )
Utility) 0
.0 1
BWOprConfiguration1 C
.C D
InstanceD L
.L M$
NotificationCCUserMailIdM e
;e f
string 
[ 
] 
cc 
= 
ccList $
.$ %
Split% *
(* +
$char+ .
). /
;/ 0
ComposeEmailBase  
objEmail! )
=* +
new, /%
ModelSoldUnitMailTemplate0 I
(I J
currentUserNameJ Y
,Y Z

dataObject[ e
.e f
LastUpdateDatef t
)t u
;u v
objEmail 
. 
Send 
( 
Bikewale &
.& '
Utility' .
.. /
BWOprConfiguration/ A
.A B
InstanceB J
.J K$
NotificationToUserMailIdK c
,c d
$str	e �
,
� �
$str
� �
,
� �
cc
� �
,
� �
null
� �
)
� �
;
� �
} 
if 
( 

dataObject 
. 
LastUpdateDate )
.) *
Month* /
!=0 2
(3 4
DateTime4 <
.< =
Now= @
.@ A
MonthA F
-G H
$numI J
)J K
)K L
{ 
isShownNotification #
=$ %
true& *
;* +
} 
return 
isShownNotification &
;& '
} 	
public"" 
void"" )
SendUploadUsedModelImageEmail"" 1
(""1 2
)""2 3
{## 	
string$$ 
[$$ 
]$$ 
ccRecievers$$  
=$$! "
Bikewale$$# +
.$$+ ,
Utility$$, 3
.$$3 4
BWOprConfiguration$$4 F
.$$F G
Instance$$G O
.$$O P$
NotificationCCUserMailId$$P h
.$$h i
Split$$i n
($$n o
$char$$o r
)$$r s
;$$s t
ComposeEmailBase%% 
objEmail%% %
=%%& '
new%%( +,
 UsedBikesModelImagesMailTemplate%%, L
(%%L M
)%%M N
;%%N O
objEmail&& 
.&& 
Send&& 
(&& 
Bikewale&& "
.&&" #
Utility&&# *
.&&* +
BWOprConfiguration&&+ =
.&&= >
Instance&&> F
.&&F G$
NotificationToUserMailId&&G _
,&&_ `
$str	&&a �
,
&&� �
$str
&&� �
,
&&� �
ccRecievers
&&� �
,
&&� �
null
&&� �
)
&&� �
;
&&� �
}'' 	
}(( 
})) 