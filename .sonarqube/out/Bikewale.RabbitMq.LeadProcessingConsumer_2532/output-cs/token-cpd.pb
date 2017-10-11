·-
VD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\BajajFinanceLeadEntity.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
public 

class "
BajajFinanceLeadEntity '
{ 
public		 
string		 
	FirstName		 
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
 
LastName

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

+ ,
public 
string 
MobileNo 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
EmailID 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
ProductMake  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
Model 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
PinCode 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
City 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
State 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
	LanNumber 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
PreApprovedAmount '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
string 
DateOfBirth !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
LikelyPurchaseDate (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
string 
address1 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
address2 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 

DealerCode 
{  !
get" %
{& '
return( .
$str/ 5
;5 6
}7 8
}9 :
public 
string 
PrefLanguage "
{# $
get% (
{) *
return+ 1
$str2 ;
;; <
}= >
}? @
public 
string 
source 
{ 
get "
{# $
return% +
$str, 7
;7 8
}9 :
}; <
public 
string 
PurchaseType "
{# $
get% (
{) *
return+ 1
$str2 ;
;; <
}= >
}? @
public 
string 
assignto 
{  
get! $
{% &
return' -
$str. ;
;; <
}= >
}? @
public   
string   

subchannel    
{  ! "
get  # &
{  ' (
return  ) /
$str  0 9
;  9 :
}  ; <
}  = >
public!! 
string!! 
caseSourceFrom!! $
{!!% &
get!!' *
{!!+ ,
return!!- 3
$str!!4 9
;!!9 :
}!!; <
}!!= >
public"" 
string"" 

Ext_UserID""  
{""! "
get""# &
{""' (
return"") /
$str""0 :
;"": ;
}""< =
}""> ?
public## 
string## 
	Ext_sysID## 
{##  !
get##" %
{##& '
return##( .
$str##/ 4
;##4 5
}##6 7
}##8 9
public$$ 
string$$ 
icrm_user_id$$ "
{$$# $
get$$% (
{$$) *
return$$+ 1
$str$$2 8
;$$8 9
}$$: ;
}$$< =
public%% 
string%% 
AgentComment%% "
{%%# $
get%%% (
{%%) *
return%%+ 1
$str%%2 8
;%%8 9
}%%: ;
}%%< =
public&& 
string&& 
ProductType&& !
{&&" #
get&&$ '
{&&( )
return&&* 0
$str&&1 5
;&&5 6
}&&7 8
}&&9 :
}'' 
public** 

class** !
BajajFinanceLeadInput** &
{++ 
public,, 
string,, 
batch_id,, 
{,,  
get,,! $
{,,% &
return,,' -
$str,,. 1
;,,1 2
},,3 4
},,5 6
public-- 
string-- 
AuthKey-- 
{-- 
get--  #
{--$ %
return--& , 
ConfigurationManager--- A
.--A B
AppSettings--B M
[--M N
$str--N \
]--\ ]
;--] ^
}--_ `
}--a b
public.. 
IEnumerable.. 
<.. "
BajajFinanceLeadEntity.. 1
>..1 2
leadData..3 ;
{..< =
get..> A
;..A B
set..C F
;..F G
}..H I
}// 
}11 ±B
WD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\BajajFinanceLeadHandler.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
internal 
class #
BajajFinanceLeadHandler *
:+ ,#
ManufacturerLeadHandler- D
{ 
public #
BajajFinanceLeadHandler &
(& '
uint' +
manufacturerId, :
,: ;
string< B
urlAPIC I
,I J
boolK O
isAPIEnabledP \
)\ ]
:^ _
base` d
(d e
manufacturerIde s
,s t
urlAPIu {
,{ |
isAPIEnabled	} â
)
â ä
{ 	
} 	
public 
override 
bool 
Process $
($ %&
ManufacturerLeadEntityBase% ?

leadEntity@ J
)J K
{ 	
return   
base   
.   
Process   
(    

leadEntity    *
)  * +
;  + ,
}!! 	
	protected)) 
override)) 
string)) !"
PushLeadToManufacturer))" 8
())8 9&
ManufacturerLeadEntityBase))9 S

leadEntity))T ^
)))^ _
{** 	
string++ 
response++ 
=++ 
string++ $
.++$ %
Empty++% *
;++* +
try,, 
{-- "
BajajFinanceLeadEntity// &
bikeMappingInfo//' 6
=//7 8
base//9 =
.//= >
LeadRepostiory//> L
.//L M*
GetBajajFinanceBikeMappingInfo//M k
(//k l

leadEntity//l v
.//v w
	VersionId	//w Ä
,
//Ä Å

leadEntity
//Ç å
.
//å ç
	PinCodeId
//ç ñ
)
//ñ ó
;
//ó ò
if00 
(00 
bikeMappingInfo00 #
!=00$ &
null00' +
&&00, .
!00/ 0
string000 6
.006 7
IsNullOrEmpty007 D
(00D E
bikeMappingInfo00E T
.00T U
Model00U Z
)00Z [
&&00\ ^
!00_ `
string00` f
.00f g
IsNullOrEmpty00g t
(00t u
bikeMappingInfo	00u Ñ
.
00Ñ Ö
City
00Ö â
)
00â ä
)
00ä ã
{11 
if22 
(22 
!22 
string22 
.22  
IsNullOrEmpty22  -
(22- .

leadEntity22. 8
.228 9
CustomerName229 E
)22E F
)22F G
{33 

leadEntity44 "
.44" #
CustomerName44# /
=440 1

leadEntity442 <
.44< =
CustomerName44= I
.44I J
Trim44J N
(44N O
)44O P
;44P Q
int55 

spaceIndex55 &
=55' (

leadEntity55) 3
.553 4
CustomerName554 @
.55@ A
IndexOf55A H
(55H I
$str55I L
)55L M
;55M N
if66 
(66 

spaceIndex66 &
>66' (
$num66) *
)66* +
{77 
bikeMappingInfo88 +
.88+ ,
	FirstName88, 5
=886 7

leadEntity888 B
.88B C
CustomerName88C O
.88O P
	Substring88P Y
(88Y Z
$num88Z [
,88[ \

spaceIndex88] g
)88g h
;88h i
bikeMappingInfo99 +
.99+ ,
LastName99, 4
=995 6

leadEntity997 A
.99A B
CustomerName99B N
.99N O
	Substring99O X
(99X Y

spaceIndex99Y c
+99d e
$num99f g
)99g h
;99h i
}:: 
else;; 
{<< 
bikeMappingInfo== +
.==+ ,
	FirstName==, 5
===6 7

leadEntity==8 B
.==B C
CustomerName==C O
;==O P
}>> 
}?? 
bikeMappingInfoAA #
.AA# $
MobileNoAA$ ,
=AA- .

leadEntityAA/ 9
.AA9 :
CustomerMobileAA: H
;AAH I
bikeMappingInfoBB #
.BB# $
EmailIDBB$ +
=BB, -

leadEntityBB. 8
.BB8 9
CustomerEmailBB9 F
;BBF G!
BajajFinanceLeadInputDD )
bajajLeadInputDD* 8
=DD9 :
newDD; >!
BajajFinanceLeadInputDD? T
(DDT U
)DDU V
{EE 
leadDataFF  
=FF! "
newFF# &
ListFF' +
<FF+ ,"
BajajFinanceLeadEntityFF, B
>FFB C
(FFC D
)FFD E
{FFF G
bikeMappingInfoFFH W
}FFX Y
}GG 
;GG 
usingII 
(II 

HttpClientII %
_httpClientII& 1
=II2 3
newII4 7

HttpClientII8 B
(IIB C
)IIC D
)IID E
{JJ 
stringKK 

jsonStringKK )
=KK* +

NewtonsoftKK, 6
.KK6 7
JsonKK7 ;
.KK; <
JsonConvertKK< G
.KKG H
SerializeObjectKKH W
(KKW X
bajajLeadInputKKX f
)KKf g
;KKg h
HttpContentLL #
httpContentLL$ /
=LL0 1
newLL2 5
StringContentLL6 C
(LLC D

jsonStringLLD N
)LLN O
;LLO P
LogsNN 
.NN 
WriteInfoLogNN )
(NN) *
StringNN* 0
.NN0 1
FormatNN1 7
(NN7 8
$strNN8 Y
,NNY Z

jsonStringNN[ e
)NNe f
)NNf g
;NNg h
usingPP 
(PP 
HttpResponseMessagePP 2
	_responsePP3 <
=PP= >
_httpClientPP? J
.PPJ K
	PostAsyncPPK T
(PPT U
basePPU Y
.PPY Z
APIUrlPPZ `
,PP` a
httpContentPPb m
)PPm n
.PPn o
ResultPPo u
)PPu v
{QQ 
ifRR 
(RR  
	_responseRR  )
.RR) *
IsSuccessStatusCodeRR* =
)RR= >
{SS 
ifTT  "
(TT# $
	_responseTT$ -
.TT- .

StatusCodeTT. 8
==TT9 ;
SystemTT< B
.TTB C
NetTTC F
.TTF G
HttpStatusCodeTTG U
.TTU V
OKTTV X
)TTX Y
{UU  !
responseVV$ ,
=VV- .
	_responseVV/ 8
.VV8 9
ContentVV9 @
.VV@ A
ReadAsStringAsyncVVA R
(VVR S
)VVS T
.VVT U
ResultVVU [
;VV[ \
	_responseWW$ -
.WW- .
ContentWW. 5
.WW5 6
DisposeWW6 =
(WW= >
)WW> ?
;WW? @
	_responseXX$ -
.XX- .
ContentXX. 5
=XX6 7
nullXX8 <
;XX< =
}YY  !
}ZZ 
}[[ 
if\\ 
(\\ 
string\\ "
.\\" #
IsNullOrEmpty\\# 0
(\\0 1
response\\1 9
)\\9 :
)\\: ;
{]] 
response^^ $
=^^% &
$str^^' W
;^^W X
}__ 
Logs`` 
.`` 
WriteInfoLog`` )
(``) *
String``* 0
.``0 1
Format``1 7
(``7 8
$str``8 Z
,``Z [
response``\ d
)``d e
)``e f
;``f g
}aa 
}bb 
elsecc 
{dd 
Logsee 
.ee 
WriteInfoLogee %
(ee% &
Stringee& ,
.ee, -
Formatee- 3
(ee3 4
$stree4 c
,eec d

Newtonsofteee o
.eeo p
Jsoneep t
.eet u
JsonConvert	eeu Ä
.
eeÄ Å
SerializeObject
eeÅ ê
(
eeê ë
bikeMappingInfo
eeë †
)
ee† °
)
ee° ¢
)
ee¢ £
;
ee£ §
}ff 
}gg 
catchhh 
(hh 
	Exceptionhh 
exhh 
)hh  
{ii 
Logsjj 
.jj 
WriteErrorLogjj "
(jj" #
Stringjj# )
.jj) *
Formatjj* 0
(jj0 1
$strjj1 O
,jjO P
exjjQ S
.jjS T
MessagejjT [
)jj[ \
)jj\ ]
;jj] ^
}kk 
returnll 
responsell 
;ll 
}mm 	
}nn 
}oo ¯*
SD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\BikeQuotationEntity.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
public 

class 
BikeQuotationEntity $
{ 
public 
ulong 
PriceQuoteId !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
ManufacturerName &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
MaskingNumber #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
ulong 
ExShowroomPrice $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
uint 
RTO 
{ 
get 
; 
set "
;" #
}$ %
public 
uint 
	Insurance 
{ 
get  #
;# $
set% (
;( )
}* +
public 
ulong 
OnRoadPrice  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public   
string   
MakeMaskingName   %
{  & '
get  ( +
;  + ,
set  - 0
;  0 1
}  2 3
public!! 
string!! 
	ModelName!! 
{!!  !
get!!" %
;!!% &
set!!' *
;!!* +
}!!, -
public"" 
string"" 
ModelMaskingName"" &
{""' (
get"") ,
;"", -
set"". 1
;""1 2
}""3 4
public## 
string## 
VersionName## !
{##" #
get##$ '
;##' (
set##) ,
;##, -
}##. /
public$$ 
uint$$ 
CityId$$ 
{$$ 
get$$  
;$$  !
set$$" %
;$$% &
}$$' (
public%% 
string%% 
CityMaskingName%% %
{%%& '
get%%( +
;%%+ ,
set%%- 0
;%%0 1
}%%2 3
public&& 
string&& 
City&& 
{&& 
get&&  
;&&  !
set&&" %
;&&% &
}&&' (
public'' 
string'' 
Area'' 
{'' 
get''  
;''  !
set''" %
;''% &
}''' (
public(( 
bool(( 
HasArea(( 
{(( 
get(( !
;((! "
set((# &
;((& '
}((( )
public)) 
uint)) 
	VersionId)) 
{)) 
get))  #
;))# $
set))% (
;))( )
}))* +
public++ 
uint++ 

CampaignId++ 
{++  
get++! $
;++$ %
set++& )
;++) *
}+++ ,
public-- 
uint-- 
ManufacturerId-- "
{--# $
get--% (
;--( )
set--* -
;--- .
}--/ 0
public// 
IEnumerable// 
<// "
OtherVersionInfoEntity// 1
>//1 2
Varients//3 ;
{//< =
get//> A
;//A B
set//C F
;//F G
}//H I
public11 
string11 
OriginalImage11 #
{11$ %
get11& )
;11) *
set11+ .
;11. /
}110 1
public22 
string22 
HostUrl22 
{22 
get22  #
;22# $
set22% (
;22( )
}22* +
public44 
uint44 
MakeId44 
{44 
get44  
;44  !
set44" %
;44% &
}44' (
public55 
bool55 

IsModelNew55 
{55  
get55! $
;55$ %
set55& )
;55) *
}55+ ,
public66 
bool66 
IsVersionNew66  
{66! "
get66# &
;66& '
set66( +
;66+ ,
}66- .
public88 
string88 
State88 
{88 
get88 !
;88! "
set88# &
;88& '
}88( )
public:: 
string:: 
ManufacturerAd:: $
{::% &
get::' *
;::* +
set::, /
;::/ 0
}::1 2
public;; 
string;; #
LeadCapturePopupHeading;; -
{;;. /
get;;0 3
;;;3 4
set;;5 8
;;;8 9
};;: ;
public<< 
string<< '
LeadCapturePopupDescription<< 1
{<<2 3
get<<4 7
;<<7 8
set<<9 <
;<<< =
}<<> ?
public== 
string== #
LeadCapturePopupMessage== -
{==. /
get==0 3
;==3 4
set==5 8
;==8 9
}==: ;
public>> 
bool>> 
PinCodeRequired>> #
{>>$ %
get>>& )
;>>) *
set>>+ .
;>>. /
}>>0 1
}?? 
}@@ ∂
OD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\BWConfiguration.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{		 
public

 

sealed

 
class

 
BWConfiguration

 '
{ 
private 
static 
BWConfiguration &
	_instance' 0
=1 2
null3 7
;7 8
private 
static 
readonly 
object  &
padlock' .
=/ 0
new1 4
object5 ;
(; <
)< =
;= >
private 
readonly 
string 
_royalEnFiedToken  1
=2 3
String4 :
.: ;
Empty; @
;@ A
public 
string 
RoyalEnFiedToken &
{) *
get+ .
{/ 0
return1 7
_royalEnFiedToken8 I
;I J
}K L
}M N
private 
BWConfiguration 
(  
)  !
{ 	
_royalEnFiedToken 
=  
ConfigurationManager  4
.4 5
AppSettings5 @
[@ A
$strA T
]T U
;U V
} 	
public 
static 
BWConfiguration %
Instance& .
{ 	
get 
{ 
if 
( 
	_instance 
==  
null! %
)% &
{ 
lock 
( 
padlock !
)! "
{ 
if 
( 
	_instance %
==& (
null) -
)- .
{ 
	_instance   %
=  & '
new  ( +
BWConfiguration  , ;
(  ; <
)  < =
;  = >
}!! 
}"" 
}## 
return$$ 
	_instance$$  
;$$  !
}%% 
}&& 	
}'' 
}(( ≈	
^D:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\DefaultManufacturerLeadHandler.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
internal		 
class		 *
DefaultManufacturerLeadHandler		 1
:		2 3#
ManufacturerLeadHandler		4 K
{

 
public *
DefaultManufacturerLeadHandler -
(- .
uint. 2
manufacturerId3 A
,A B
stringC I
urlAPIJ P
,P Q
boolR V
isAPIEnabledW c
)c d
:e f
baseg k
(k l
manufacturerIdl z
,z {
urlAPI	| Ç
,
Ç É
isAPIEnabled
Ñ ê
)
ê ë
{ 	
} 	
	protected 
override 
string !"
PushLeadToManufacturer" 8
(8 9&
ManufacturerLeadEntityBase9 S

leadEntityT ^
)^ _
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} Ë2
\D:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\HondaManufacturerLeadHandler.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
internal 
class (
HondaManufacturerLeadHandler /
:0 1#
ManufacturerLeadHandler2 I
{ 
public (
HondaManufacturerLeadHandler +
(+ ,
uint, 0
manufacturerId1 ?
,? @
stringA G
urlAPIH N
,N O
boolP T
isAPIEnabledU a
)a b
:c d
basee i
(i j
manufacturerIdj x
,x y
urlAPI	z Ä
,
Ä Å
isAPIEnabled
Ç é
)
é è
{ 	
} 	
public 
override 
bool 
Process $
($ %&
ManufacturerLeadEntityBase% ?

leadEntity@ J
)J K
{ 	
return 
base 
. 
Process 
(  

leadEntity  *
)* +
;+ ,
}   	
	protected(( 
override(( 
string(( !"
PushLeadToManufacturer((" 8
(((8 9&
ManufacturerLeadEntityBase((9 S

leadEntity((T ^
)((^ _
{)) 	
string** 
leadURL** 
=** 
string** #
.**# $
Empty**$ )
;**) *
string++ 
response++ 
=++ 
string++ $
.++$ %
Empty++% *
;++* +
try,, 
{-- 
BikeQuotationEntity// #
	quotation//$ -
=//. /
base//0 4
.//4 5
LeadRepostiory//5 C
.//C D
GetPriceQuoteById//D U
(//U V

leadEntity//V `
.//` a
PQId//a e
)//e f
;//f g
GaadiLeadEntity11 
	gaadiLead11  )
=11* +
new11, /
GaadiLeadEntity110 ?
(11? @
)11@ A
{22 
City33 
=33 
	quotation33 $
.33$ %
City33% )
,33) *
Email44 
=44 

leadEntity44 &
.44& '
CustomerEmail44' 4
,444 5
Make55 
=55 
	quotation55 $
.55$ %
MakeName55% -
,55- .
Mobile66 
=66 

leadEntity66 '
.66' (
CustomerMobile66( 6
,666 7
Model77 
=77 
	quotation77 %
.77% &
	ModelName77& /
,77/ 0
Name88 
=88 

leadEntity88 %
.88% &
CustomerName88& 2
,882 3
State99 
=99 
	quotation99 %
.99% &
State99& +
}:: 
;:: 
using<< 
(<< 

HttpClient<< !
_httpClient<<" -
=<<. /
new<<0 3

HttpClient<<4 >
(<<> ?
)<<? @
)<<@ A
{== 
string>> 

jsonString>> %
=>>& '

Newtonsoft>>( 2
.>>2 3
Json>>3 7
.>>7 8
JsonConvert>>8 C
.>>C D
SerializeObject>>D S
(>>S T
	gaadiLead>>T ]
)>>] ^
;>>^ _
byte?? 
[?? 
]?? 
toEncodeAsBytes?? *
=??+ ,
System??- 3
.??3 4
Text??4 8
.??8 9
ASCIIEncoding??9 F
.??F G
ASCII??G L
.??L M
GetBytes??M U
(??U V

jsonString??V `
)??` a
;??a b
leadURL@@ 
=@@ 
String@@ $
.@@$ %
Format@@% +
(@@+ ,
$str@@, 4
,@@4 5
base@@6 :
.@@: ;
APIUrl@@; A
,@@A B
System@@C I
.@@I J
Convert@@J Q
.@@Q R
ToBase64String@@R `
(@@` a
toEncodeAsBytes@@a p
)@@p q
)@@q r
;@@r s
LogsAA 
.AA 
WriteInfoLogAA %
(AA% &
StringAA& ,
.AA, -
FormatAA- 3
(AA3 4
$strAA4 I
,AAI J
leadURLAAK R
)AAR S
)AAS T
;AAT U
usingBB 
(BB 
HttpResponseMessageBB .
	_responseBB/ 8
=BB9 :
_httpClientBB; F
.BBF G
GetAsyncBBG O
(BBO P
leadURLBBP W
)BBW X
.BBX Y
ResultBBY _
)BB_ `
{CC 
ifDD 
(DD 
	_responseDD %
.DD% &
IsSuccessStatusCodeDD& 9
&&DD: <
	_responseDD= F
.DDF G

StatusCodeDDG Q
==DDR T
SystemDDU [
.DD[ \
NetDD\ _
.DD_ `
HttpStatusCodeDD` n
.DDn o
OKDDo q
)DDq r
{EE 
responseGG $
=GG% &
	_responseGG' 0
.GG0 1
ContentGG1 8
.GG8 9
ReadAsStringAsyncGG9 J
(GGJ K
)GGK L
.GGL M
ResultGGM S
;GGS T
	_responseHH %
.HH% &
ContentHH& -
.HH- .
DisposeHH. 5
(HH5 6
)HH6 7
;HH7 8
	_responseII %
.II% &
ContentII& -
=II. /
nullII0 4
;II4 5
}JJ 
}KK 
}LL 
ifNN 
(NN 
stringNN 
.NN 
IsNullOrEmptyNN (
(NN( )
responseNN) 1
)NN1 2
)NN2 3
{OO 
responsePP 
=PP 
$strPP P
;PPP Q
}QQ 
LogsRR 
.RR 
WriteInfoLogRR !
(RR! "
StringRR" (
.RR( )
FormatRR) /
(RR/ 0
$strRR0 F
,RRF G
responseRRH P
)RRP Q
)RRQ R
;RRR S
}SS 
catchTT 
(TT 
	ExceptionTT 
exTT 
)TT  
{UU 
LogsVV 
.VV 
WriteErrorLogVV "
(VV" #
StringVV# )
.VV) *
FormatVV* 0
(VV0 1
$strVV1 H
,VVH I
exVVJ L
.VVL M
MessageVVM T
)VVT U
)VVU V
;VVV W
}WW 
returnXX 
responseXX 
;XX 
}YY 	
}ZZ 
}[[ ¸
XD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\IManufacturerLeadHandler.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
internal 
	interface $
IManufacturerLeadHandler /
{ 
bool		 
Process		 
(		 &
ManufacturerLeadEntityBase		 /

leadEntity		0 :
)		: ;
;		; <
}

 
} ‚
ZD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\ManufacturerLeadEntityBase.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
internal 
class &
ManufacturerLeadEntityBase -
{ 
public		 
string		 
CustomerName		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		/ 0
public

 
string

 
CustomerEmail

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

0 1
public 
string 
CustomerMobile $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
ushort 
RetryAttempt "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
uint 
PQId 
{ 
get 
; 
set  #
;# $
}% &
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
uint 
LeadId 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 

CampaignId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
InquiryJSON !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
uint 
	VersionId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
	PinCodeId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
uint  
ManufacturerDealerId (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
} 
} ÷ñ
XD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\LeadProcessingRepository.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{		 
internal 
class $
LeadProcessingRepository +
{ 
public 
bool 

PushedToAB 
( 
uint #
pqId$ (
,( )
uint* .
abInquiryId/ :
,: ;
UInt16< B

retryCountC M
)M N
{ 	
bool 
	isSuccess 
= 
false "
;" #
try 
{ 
using 
( 
	DbCommand  
cmd! $
=% &
	DbFactory' 0
.0 1
GetDBCommand1 =
(= >
)> ?
)? @
{ 
cmd 
. 
CommandText #
=$ %
$str& =
;= >
cmd 
. 
CommandType #
=$ %
CommandType& 1
.1 2
StoredProcedure2 A
;A B
cmd!! 
.!! 

Parameters!! "
.!!" #
Add!!# &
(!!& '
	DbFactory!!' 0
.!!0 1

GetDbParam!!1 ;
(!!; <
$str!!< F
,!!F G
DbType!!H N
.!!N O
Int64!!O T
,!!T U
pqId!!V Z
)!!Z [
)!![ \
;!!\ ]
cmd"" 
."" 

Parameters"" "
.""" #
Add""# &
(""& '
	DbFactory""' 0
.""0 1

GetDbParam""1 ;
(""; <
$str""< M
,""M N
DbType""O U
.""U V
Int64""V [
,""[ \
abInquiryId""] h
)""h i
)""i j
;""j k
cmd## 
.## 

Parameters## "
.##" #
Add### &
(##& '
	DbFactory##' 0
.##0 1

GetDbParam##1 ;
(##; <
$str##< Q
,##Q R
DbType##S Y
.##Y Z
Int16##Z _
,##_ `

retryCount##a k
)##k l
)##l m
;##m n
if$$ 
($$ 
Convert$$ 
.$$  
	ToBoolean$$  )
($$) *
MySqlDatabase$$* 7
.$$7 8
UpdateQuery$$8 C
($$C D
cmd$$D G
,$$G H
ConnectionType$$I W
.$$W X
MasterDatabase$$X f
)$$f g
)$$g h
)$$h i
	isSuccess%% !
=%%" #
true%%$ (
;%%( )
}&& 
}'' 
catch(( 
((( 
	Exception(( 
ex(( 
)((  
{)) 
Logs** 
.** 
WriteErrorLog** "
(**" #
String**# )
.**) *
Format*** 0
(**0 1
$str**1 [
,**[ \
pqId**] a
,**a b
abInquiryId**c n
,**n o
ex**p r
.**r s
Message**s z
)**z {
)**{ |
;**| }
	isSuccess++ 
=++ 
false++ !
;++! "
},, 
return.. 
	isSuccess.. 
;.. 
}// 	
public77 
bool77 &
UpdateDealerDailyLeadCount77 .
(77. /
uint77/ 3

campaignId774 >
,77> ?
uint77@ D
abInquiryId77E P
)77P Q
{88 	
bool99 
isUpdateDealerCount99 $
=99% &
false99' ,
;99, -
try:: 
{;; 
using<< 
(<< 
	DbCommand<<  
cmd<<! $
=<<% &
	DbFactory<<' 0
.<<0 1
GetDBCommand<<1 =
(<<= >
)<<> ?
)<<? @
{== 
cmd>> 
.>> 
CommandText>> #
=>>$ %
$str>>& B
;>>B C
cmd?? 
.?? 
CommandType?? #
=??$ %
CommandType??& 1
.??1 2
StoredProcedure??2 A
;??A B
cmdAA 
.AA 

ParametersAA "
.AA" #
AddAA# &
(AA& '
	DbFactoryAA' 0
.AA0 1

GetDbParamAA1 ;
(AA; <
$strAA< L
,AAL M
DbTypeAAN T
.AAT U
Int32AAU Z
,AAZ [

campaignIdAA\ f
)AAf g
)AAg h
;AAh i
cmdBB 
.BB 

ParametersBB "
.BB" #
AddBB# &
(BB& '
	DbFactoryBB' 0
.BB0 1

GetDbParamBB1 ;
(BB; <
$strBB< M
,BBM N
DbTypeBBO U
.BBU V
Int32BBV [
,BB[ \
abInquiryIdBB] h
)BBh i
)BBi j
;BBj k
cmdCC 
.CC 

ParametersCC "
.CC" #
AddCC# &
(CC& '
	DbFactoryCC' 0
.CC0 1

GetDbParamCC1 ;
(CC; <
$strCC< U
,CCU V
DbTypeCCW ]
.CC] ^
BooleanCC^ e
,CCe f
ParameterDirectionCCg y
.CCy z
Output	CCz Ä
)
CCÄ Å
)
CCÅ Ç
;
CCÇ É
MySqlDatabaseEE !
.EE! "
ExecuteNonQueryEE" 1
(EE1 2
cmdEE2 5
,EE5 6
ConnectionTypeEE7 E
.EEE F
MasterDatabaseEEF T
)EET U
;EEU V
isUpdateDealerCountGG '
=GG( )
ConvertGG* 1
.GG1 2
	ToBooleanGG2 ;
(GG; <
!GG< =
ConvertGG= D
.GGD E
IsDBNullGGE M
(GGM N
cmdGGN Q
.GGQ R

ParametersGGR \
[GG\ ]
$strGG] v
]GGv w
.GGw x
ValueGGx }
)GG} ~
?	GG Ä
cmd
GGÅ Ñ
.
GGÑ Ö

Parameters
GGÖ è
[
GGè ê
$str
GGê ©
]
GG© ™
.
GG™ ´
Value
GG´ ∞
:
GG± ≤
false
GG≥ ∏
)
GG∏ π
;
GGπ ∫
}II 
}JJ 
catchKK 
(KK 
	ExceptionKK 
exKK 
)KK  
{LL 
LogsMM 
.MM 
WriteErrorLogMM "
(MM" #
StringMM# )
.MM) *
FormatMM* 0
(MM0 1
$strMM1 k
,MMk l

campaignIdMMm w
,MMw x
abInquiryId	MMy Ñ
,
MMÑ Ö
ex
MMÜ à
.
MMà â
Message
MMâ ê
)
MMê ë
)
MMë í
;
MMí ì
}NN 
returnOO 
isUpdateDealerCountOO &
;OO& '
}PP 	
public]] &
PriceQuoteParametersEntity]] )&
FetchPriceQuoteDetailsById]]* D
(]]D E
ulong]]E J
pqId]]K O
)]]O P
{^^ 	&
PriceQuoteParametersEntity__ &
objQuotation__' 3
=__4 5
null__6 :
;__: ;
try`` 
{aa 
usingbb 
(bb 
	DbCommandbb  
cmdbb! $
=bb% &
	DbFactorybb' 0
.bb0 1
GetDBCommandbb1 =
(bb= >
)bb> ?
)bb? @
{cc 
cmddd 
.dd 
CommandTextdd #
=dd$ %
$strdd& =
;dd= >
cmdee 
.ee 
CommandTypeee #
=ee$ %
CommandTypeee& 1
.ee1 2
StoredProcedureee2 A
;eeA B
cmdff 
.ff 

Parametersff "
.ff" #
Addff# &
(ff& '
	DbFactoryff' 0
.ff0 1

GetDbParamff1 ;
(ff; <
$strff< I
,ffI J
DbTypeffK Q
.ffQ R
Int32ffR W
,ffW X
pqIdffY ]
)ff] ^
)ff^ _
;ff_ `
usinggg 
(gg 
IDataReadergg &
drgg' )
=gg* +
MySqlDatabasegg, 9
.gg9 :
SelectQuerygg: E
(ggE F
cmdggF I
,ggI J
ConnectionTypeggK Y
.ggY Z
MasterDatabaseggZ h
)ggh i
)ggi j
{hh 
objQuotationii $
=ii% &
newii' *&
PriceQuoteParametersEntityii+ E
(iiE F
)iiF G
;iiG H
ifjj 
(jj 
drjj 
!=jj !
nulljj" &
)jj& '
{kk 
whilell !
(ll" #
drll# %
.ll% &
Readll& *
(ll* +
)ll+ ,
)ll, -
{mm 
objQuotationnn  ,
.nn, -
AreaIdnn- 3
=nn4 5
SqlReaderConvertornn6 H
.nnH I
ToUInt32nnI Q
(nnQ R
drnnR T
[nnT U
$strnnU ]
]nn] ^
)nn^ _
;nn_ `
objQuotationoo  ,
.oo, -
CityIdoo- 3
=oo4 5
SqlReaderConvertoroo6 H
.ooH I
ToUInt32ooI Q
(ooQ R
drooR T
[ooT U
$strooU ]
]oo] ^
)oo^ _
;oo_ `
objQuotationpp  ,
.pp, -
	VersionIdpp- 6
=pp7 8
SqlReaderConvertorpp9 K
.ppK L
ToUInt32ppL T
(ppT U
drppU W
[ppW X
$strppX g
]ppg h
)pph i
;ppi j
objQuotationqq  ,
.qq, -
DealerIdqq- 5
=qq6 7
SqlReaderConvertorqq8 J
.qqJ K
ToUInt32qqK S
(qqS T
drqqT V
[qqV W
$strqqW a
]qqa b
)qqb c
;qqc d
objQuotationrr  ,
.rr, -

CampaignIdrr- 7
=rr8 9
SqlReaderConvertorrr: L
.rrL M
ToUInt32rrM U
(rrU V
drrrV X
[rrX Y
$strrrY e
]rre f
)rrf g
;rrg h
objQuotationss  ,
.ss, -
CustomerMobiless- ;
=ss< =
Convertss> E
.ssE F
ToStringssF N
(ssN O
drssO Q
[ssQ R
$strssR b
]ssb c
)ssc d
;ssd e
objQuotationtt  ,
.tt, -
CustomerEmailtt- :
=tt; <
Converttt= D
.ttD E
ToStringttE M
(ttM N
drttN P
[ttP Q
$strttQ `
]tt` a
)tta b
;ttb c
objQuotationuu  ,
.uu, -
CustomerNameuu- 9
=uu: ;
Convertuu< C
.uuC D
ToStringuuD L
(uuL M
druuM O
[uuO P
$struuP ^
]uu^ _
)uu_ `
;uu` a
}vv 
drww 
.ww 
Closeww $
(ww$ %
)ww% &
;ww& '
}xx 
}yy 
}zz 
}{{ 
catch|| 
(|| 
	Exception|| 
ex|| 
)||  
{}} 
Logs~~ 
.~~ 
WriteErrorLog~~ "
(~~" #
String~~# )
.~~) *
Format~~* 0
(~~0 1
$str~~1 g
,~~g h
pqId~~i m
,~~m n
ex~~o q
.~~q r
Message~~r y
)~~y z
)~~z {
;~~{ |
} 
return
ÅÅ 
objQuotation
ÅÅ 
;
ÅÅ  
}
ÇÇ 	
public
çç 
bool
çç $
UpdateManufacturerLead
çç *
(
çç* +
uint
çç+ /
pqId
çç0 4
,
çç4 5
string
çç6 <
response
çç= E
,
ççE F
uint
ççG K
leadId
ççL R
)
ççR S
{
éé 	
bool
èè 
status
èè 
=
èè 
false
èè 
;
èè  
try
êê 
{
ëë 
if
íí 
(
íí 
leadId
íí 
>
íí 
$num
íí 
)
íí 
{
ìì 
using
îî 
(
îî 
	DbCommand
îî $
cmd
îî% (
=
îî) *
	DbFactory
îî+ 4
.
îî4 5
GetDBCommand
îî5 A
(
îîA B
)
îîB C
)
îîC D
{
ïï 
cmd
ññ 
.
ññ 
CommandType
ññ '
=
ññ( )
CommandType
ññ* 5
.
ññ5 6
StoredProcedure
ññ6 E
;
ññE F
cmd
óó 
.
óó 
CommandText
óó '
=
óó( )
$str
óó* K
;
óóK L
cmd
òò 
.
òò 

Parameters
òò &
.
òò& '
Add
òò' *
(
òò* +
	DbFactory
òò+ 4
.
òò4 5

GetDbParam
òò5 ?
(
òò? @
$str
òò@ L
,
òòL M
DbType
òòN T
.
òòT U
Int64
òòU Z
,
òòZ [
leadId
òò\ b
)
òòb c
)
òòc d
;
òòd e
cmd
ôô 
.
ôô 

Parameters
ôô &
.
ôô& '
Add
ôô' *
(
ôô* +
	DbFactory
ôô+ 4
.
ôô4 5

GetDbParam
ôô5 ?
(
ôô? @
$str
ôô@ N
,
ôôN O
DbType
ôôP V
.
ôôV W
String
ôôW ]
,
ôô] ^
$num
ôô_ b
,
ôôb c
response
ôôd l
)
ôôl m
)
ôôm n
;
ôôn o
if
öö 
(
öö 
MySqlDatabase
öö )
.
öö) *
ExecuteNonQuery
öö* 9
(
öö9 :
cmd
öö: =
,
öö= >
ConnectionType
öö? M
.
ööM N
MasterDatabase
ööN \
)
öö\ ]
>
öö^ _
$num
öö` a
)
ööa b
status
õõ "
=
õõ# $
true
õõ% )
;
õõ) *
}
úú 
}
ùù 
}
ûû 
catch
üü 
(
üü 
	Exception
üü 
ex
üü 
)
üü  
{
†† 
Logs
°° 
.
°° 
WriteErrorLog
°° "
(
°°" #
String
°°# )
.
°°) *
Format
°°* 0
(
°°0 1
$str
°°1 c
,
°°c d
pqId
°°e i
,
°°i j
ex
°°k m
.
°°m n
Message
°°n u
)
°°u v
)
°°v w
;
°°w x
}
¢¢ 
return
§§ 
status
§§ 
;
§§ 
}
•• 	
public
ÆÆ !
BikeQuotationEntity
ÆÆ "
GetPriceQuoteById
ÆÆ# 4
(
ÆÆ4 5
uint
ÆÆ5 9
pqId
ÆÆ: >
)
ÆÆ> ?
{
ØØ 	!
BikeQuotationEntity
∞∞ 
objQuotation
∞∞  ,
=
∞∞- .
null
∞∞/ 3
;
∞∞3 4
try
±± 
{
≤≤ 
objQuotation
≥≥ 
=
≥≥ 
new
≥≥ "!
BikeQuotationEntity
≥≥# 6
(
≥≥6 7
)
≥≥7 8
;
≥≥8 9
using
¥¥ 
(
¥¥ 
	DbCommand
¥¥  
cmd
¥¥! $
=
¥¥% &
	DbFactory
¥¥' 0
.
¥¥0 1
GetDBCommand
¥¥1 =
(
¥¥= >
)
¥¥> ?
)
¥¥? @
{
µµ 
cmd
∂∂ 
.
∂∂ 
CommandText
∂∂ #
=
∂∂$ %
$str
∂∂& B
;
∂∂B C
cmd
∑∑ 
.
∑∑ 
CommandType
∑∑ #
=
∑∑$ %
CommandType
∑∑& 1
.
∑∑1 2
StoredProcedure
∑∑2 A
;
∑∑A B
cmd
ππ 
.
ππ 

Parameters
ππ "
.
ππ" #
Add
ππ# &
(
ππ& '
	DbFactory
ππ' 0
.
ππ0 1

GetDbParam
ππ1 ;
(
ππ; <
$str
ππ< I
,
ππI J
DbType
ππK Q
.
ππQ R
UInt32
ππR X
,
ππX Y
pqId
ππZ ^
)
ππ^ _
)
ππ_ `
;
ππ` a
using
∫∫ 
(
∫∫ 
IDataReader
∫∫ &
dr
∫∫' )
=
∫∫* +
MySqlDatabase
∫∫, 9
.
∫∫9 :
SelectQuery
∫∫: E
(
∫∫E F
cmd
∫∫F I
,
∫∫I J
ConnectionType
∫∫K Y
.
∫∫Y Z
MasterDatabase
∫∫Z h
)
∫∫h i
)
∫∫i j
{
ªª 
if
ºº 
(
ºº 
dr
ºº 
!=
ºº !
null
ºº" &
&&
ºº' )
dr
ºº* ,
.
ºº, -
Read
ºº- 1
(
ºº1 2
)
ºº2 3
)
ºº3 4
{
ΩΩ 
objQuotation
ææ (
.
ææ( )
ExShowroomPrice
ææ) 8
=
ææ9 : 
SqlReaderConvertor
ææ; M
.
ææM N
ToUInt64
ææN V
(
ææV W
dr
ææW Y
[
ææY Z
$str
ææZ f
]
ææf g
)
ææg h
;
ææh i
objQuotation
øø (
.
øø( )
RTO
øø) ,
=
øø- . 
SqlReaderConvertor
øø/ A
.
øøA B
ToUInt32
øøB J
(
øøJ K
dr
øøK M
[
øøM N
$str
øøN S
]
øøS T
)
øøT U
;
øøU V
objQuotation
¿¿ (
.
¿¿( )
	Insurance
¿¿) 2
=
¿¿3 4 
SqlReaderConvertor
¿¿5 G
.
¿¿G H
ToUInt32
¿¿H P
(
¿¿P Q
dr
¿¿Q S
[
¿¿S T
$str
¿¿T _
]
¿¿_ `
)
¿¿` a
;
¿¿a b
objQuotation
¡¡ (
.
¡¡( )
OnRoadPrice
¡¡) 4
=
¡¡5 6 
SqlReaderConvertor
¡¡7 I
.
¡¡I J
ToUInt64
¡¡J R
(
¡¡R S
dr
¡¡S U
[
¡¡U V
$str
¡¡V ^
]
¡¡^ _
)
¡¡_ `
;
¡¡` a
objQuotation
¬¬ (
.
¬¬( )
MakeName
¬¬) 1
=
¬¬2 3
Convert
¬¬4 ;
.
¬¬; <
ToString
¬¬< D
(
¬¬D E
dr
¬¬E G
[
¬¬G H
$str
¬¬H N
]
¬¬N O
)
¬¬O P
;
¬¬P Q
objQuotation
√√ (
.
√√( )
	ModelName
√√) 2
=
√√3 4
Convert
√√5 <
.
√√< =
ToString
√√= E
(
√√E F
dr
√√F H
[
√√H I
$str
√√I P
]
√√P Q
)
√√Q R
;
√√R S
objQuotation
ƒƒ (
.
ƒƒ( )
VersionName
ƒƒ) 4
=
ƒƒ5 6
Convert
ƒƒ7 >
.
ƒƒ> ?
ToString
ƒƒ? G
(
ƒƒG H
dr
ƒƒH J
[
ƒƒJ K
$str
ƒƒK T
]
ƒƒT U
)
ƒƒU V
;
ƒƒV W
objQuotation
≈≈ (
.
≈≈( )
City
≈≈) -
=
≈≈. /
Convert
≈≈0 7
.
≈≈7 8
ToString
≈≈8 @
(
≈≈@ A
dr
≈≈A C
[
≈≈C D
$str
≈≈D N
]
≈≈N O
)
≈≈O P
;
≈≈P Q
objQuotation
∆∆ (
.
∆∆( )
	VersionId
∆∆) 2
=
∆∆3 4 
SqlReaderConvertor
∆∆5 G
.
∆∆G H
ToUInt32
∆∆H P
(
∆∆P Q
dr
∆∆Q S
[
∆∆S T
$str
∆∆T _
]
∆∆_ `
)
∆∆` a
;
∆∆a b
objQuotation
«« (
.
««( )

CampaignId
««) 3
=
««4 5 
SqlReaderConvertor
««6 H
.
««H I
ToUInt32
««I Q
(
««Q R
dr
««R T
[
««T U
$str
««U a
]
««a b
)
««b c
;
««c d
objQuotation
»» (
.
»»( )
ManufacturerId
»») 7
=
»»8 9 
SqlReaderConvertor
»»: L
.
»»L M
ToUInt32
»»M U
(
»»U V
dr
»»V X
[
»»X Y
$str
»»Y i
]
»»i j
)
»»j k
;
»»k l
objQuotation
…… (
.
……( )
State
……) .
=
……/ 0
Convert
……1 8
.
……8 9
ToString
……9 A
(
……A B
dr
……B D
[
……D E
$str
……E P
]
……P Q
)
……Q R
;
……R S
objQuotation
ÀÀ (
.
ÀÀ( )
PriceQuoteId
ÀÀ) 5
=
ÀÀ6 7
pqId
ÀÀ8 <
;
ÀÀ< =
}
ÃÃ 
}
ÕÕ 
}
ŒŒ 
}
œœ 
catch
–– 
(
–– 
	Exception
–– 
ex
–– 
)
––  
{
—— 
Logs
““ 
.
““ 
WriteErrorLog
““ "
(
““" #
String
““# )
.
““) *
Format
““* 0
(
““0 1
$str
““1 ^
,
““^ _
pqId
““` d
,
““d e
ex
““f h
.
““h i
Message
““i p
)
““p q
)
““q r
;
““r s
}
”” 
return
’’ 
objQuotation
’’ 
;
’’  
}
÷÷ 	
public
·· 
bool
·· "
SaveManufacturerLead
·· (
(
··( )$
ManufacturerLeadEntity
··) ?
objLead
··@ G
)
··G H
{
‚‚ 	
bool
„„ 
status
„„ 
=
„„ 
false
„„ 
;
„„  
try
‰‰ 
{
ÂÂ 
if
ÊÊ 
(
ÊÊ 
objLead
ÊÊ 
!=
ÊÊ 
null
ÊÊ #
&&
ÊÊ$ &
objLead
ÊÊ' .
.
ÊÊ. /
PQId
ÊÊ/ 3
>
ÊÊ4 5
$num
ÊÊ6 7
&&
ÊÊ8 :
objLead
ÊÊ; B
.
ÊÊB C
DealerId
ÊÊC K
>
ÊÊL M
$num
ÊÊN O
)
ÊÊO P
{
ÁÁ 
using
ËË 
(
ËË 
	DbCommand
ËË $
cmd
ËË% (
=
ËË) *
	DbFactory
ËË+ 4
.
ËË4 5
GetDBCommand
ËË5 A
(
ËËA B
)
ËËB C
)
ËËC D
{
ÈÈ 
cmd
ÍÍ 
.
ÍÍ 
CommandType
ÍÍ '
=
ÍÍ( )
CommandType
ÍÍ* 5
.
ÍÍ5 6
StoredProcedure
ÍÍ6 E
;
ÍÍE F
cmd
ÎÎ 
.
ÎÎ 
CommandText
ÎÎ '
=
ÎÎ( )
$str
ÎÎ* I
;
ÎÎI J
cmd
ÌÌ 
.
ÌÌ 

Parameters
ÌÌ &
.
ÌÌ& '
Add
ÌÌ' *
(
ÌÌ* +
	DbFactory
ÌÌ+ 4
.
ÌÌ4 5

GetDbParam
ÌÌ5 ?
(
ÌÌ? @
$str
ÌÌ@ J
,
ÌÌJ K
DbType
ÌÌL R
.
ÌÌR S
String
ÌÌS Y
,
ÌÌY Z
$num
ÌÌ[ ]
,
ÌÌ] ^
objLead
ÌÌ_ f
.
ÌÌf g
Name
ÌÌg k
)
ÌÌk l
)
ÌÌl m
;
ÌÌm n
cmd
ÓÓ 
.
ÓÓ 

Parameters
ÓÓ &
.
ÓÓ& '
Add
ÓÓ' *
(
ÓÓ* +
	DbFactory
ÓÓ+ 4
.
ÓÓ4 5

GetDbParam
ÓÓ5 ?
(
ÓÓ? @
$str
ÓÓ@ K
,
ÓÓK L
DbType
ÓÓM S
.
ÓÓS T
String
ÓÓT Z
,
ÓÓZ [
$num
ÓÓ\ _
,
ÓÓ_ `
objLead
ÓÓa h
.
ÓÓh i
Email
ÓÓi n
)
ÓÓn o
)
ÓÓo p
;
ÓÓp q
cmd
ÔÔ 
.
ÔÔ 

Parameters
ÔÔ &
.
ÔÔ& '
Add
ÔÔ' *
(
ÔÔ* +
	DbFactory
ÔÔ+ 4
.
ÔÔ4 5

GetDbParam
ÔÔ5 ?
(
ÔÔ? @
$str
ÔÔ@ L
,
ÔÔL M
DbType
ÔÔN T
.
ÔÔT U
String
ÔÔU [
,
ÔÔ[ \
$num
ÔÔ] _
,
ÔÔ_ `
objLead
ÔÔa h
.
ÔÔh i
Mobile
ÔÔi o
)
ÔÔo p
)
ÔÔp q
;
ÔÔq r
cmd
 
.
 

Parameters
 &
.
& '
Add
' *
(
* +
	DbFactory
+ 4
.
4 5

GetDbParam
5 ?
(
? @
$str
@ J
,
J K
DbType
L R
.
R S
Int64
S X
,
X Y
objLead
Z a
.
a b
PQId
b f
)
f g
)
g h
;
h i
cmd
ÒÒ 
.
ÒÒ 

Parameters
ÒÒ &
.
ÒÒ& '
Add
ÒÒ' *
(
ÒÒ* +
	DbFactory
ÒÒ+ 4
.
ÒÒ4 5

GetDbParam
ÒÒ5 ?
(
ÒÒ? @
$str
ÒÒ@ R
,
ÒÒR S
DbType
ÒÒT Z
.
ÒÒZ [
Int16
ÒÒ[ `
,
ÒÒ` a
objLead
ÒÒb i
.
ÒÒi j
LeadSourceId
ÒÒj v
)
ÒÒv w
)
ÒÒw x
;
ÒÒx y
cmd
ÚÚ 
.
ÚÚ 

Parameters
ÚÚ &
.
ÚÚ& '
Add
ÚÚ' *
(
ÚÚ* +
	DbFactory
ÚÚ+ 4
.
ÚÚ4 5

GetDbParam
ÚÚ5 ?
(
ÚÚ? @
$str
ÚÚ@ M
,
ÚÚM N
DbType
ÚÚO U
.
ÚÚU V
String
ÚÚV \
,
ÚÚ\ ]
objLead
ÚÚ^ e
.
ÚÚe f
	PinCodeId
ÚÚf o
)
ÚÚo p
)
ÚÚp q
;
ÚÚq r
cmd
ÛÛ 
.
ÛÛ 

Parameters
ÛÛ &
.
ÛÛ& '
Add
ÛÛ' *
(
ÛÛ* +
	DbFactory
ÛÛ+ 4
.
ÛÛ4 5

GetDbParam
ÛÛ5 ?
(
ÛÛ? @
$str
ÛÛ@ N
,
ÛÛN O
DbType
ÛÛP V
.
ÛÛV W
Int64
ÛÛW \
,
ÛÛ\ ]
objLead
ÛÛ^ e
.
ÛÛe f
DealerId
ÛÛf n
)
ÛÛn o
)
ÛÛo p
;
ÛÛp q
cmd
ÙÙ 
.
ÙÙ 

Parameters
ÙÙ &
.
ÙÙ& '
Add
ÙÙ' *
(
ÙÙ* +
	DbFactory
ÙÙ+ 4
.
ÙÙ4 5

GetDbParam
ÙÙ5 ?
(
ÙÙ? @
$str
ÙÙ@ Z
,
ÙÙZ [
DbType
ÙÙ\ b
.
ÙÙb c
String
ÙÙc i
,
ÙÙi j
$num
ÙÙk m
,
ÙÙm n
objLead
ÙÙo v
.
ÙÙv w#
ManufacturerDealerIdÙÙw ã
)ÙÙã å
)ÙÙå ç
;ÙÙç é
cmd
ıı 
.
ıı 

Parameters
ıı &
.
ıı& '
Add
ıı' *
(
ıı* +
	DbFactory
ıı+ 4
.
ıı4 5

GetDbParam
ıı5 ?
(
ıı? @
$str
ıı@ L
,
ııL M
DbType
ııN T
.
ııT U
Int32
ııU Z
,
ııZ [
objLead
ıı\ c
.
ııc d
LeadId
ııd j
)
ııj k
)
ıık l
;
ııl m
status
ˆˆ 
=
ˆˆ   
SqlReaderConvertor
ˆˆ! 3
.
ˆˆ3 4
	ToBoolean
ˆˆ4 =
(
ˆˆ= >
MySqlDatabase
ˆˆ> K
.
ˆˆK L
ExecuteNonQuery
ˆˆL [
(
ˆˆ[ \
cmd
ˆˆ\ _
,
ˆˆ_ `
ConnectionType
ˆˆa o
.
ˆˆo p
MasterDatabase
ˆˆp ~
)
ˆˆ~ 
)ˆˆ Ä
;ˆˆÄ Å
}
˜˜ 
}
¯¯ 
}
˘˘ 
catch
˚˚ 
(
˚˚ 
	Exception
˚˚ 
ex
˚˚ 
)
˚˚  
{
¸¸ 
Logs
˝˝ 
.
˝˝ 
WriteErrorLog
˝˝ "
(
˝˝" #
String
˝˝# )
.
˝˝) *
Format
˝˝* 0
(
˝˝0 1
$str
˝˝1 a
,
˝˝a b
ex
˝˝c e
.
˝˝e f
Message
˝˝f m
)
˝˝m n
)
˝˝n o
;
˝˝o p
}
˛˛ 
return
ÄÄ 
status
ÄÄ 
;
ÄÄ 
}
ÇÇ 	
public
ãã $
BajajFinanceLeadEntity
ãã %,
GetBajajFinanceBikeMappingInfo
ãã& D
(
ããD E
uint
ããE I
	versionid
ããJ S
,
ããS T
uint
ããU Y
	pincodeId
ããZ c
)
ããc d
{
åå 	$
BajajFinanceLeadEntity
çç "
objQuotation
çç# /
=
çç0 1
null
çç2 6
;
çç6 7
try
éé 
{
èè 
objQuotation
êê 
=
êê 
new
êê "$
BajajFinanceLeadEntity
êê# 9
(
êê9 :
)
êê: ;
;
êê; <
using
ëë 
(
ëë 
	DbCommand
ëë  
cmd
ëë! $
=
ëë% &
	DbFactory
ëë' 0
.
ëë0 1
GetDBCommand
ëë1 =
(
ëë= >
)
ëë> ?
)
ëë? @
{
íí 
cmd
ìì 
.
ìì 
CommandText
ìì #
=
ìì$ %
$str
ìì& B
;
ììB C
cmd
îî 
.
îî 
CommandType
îî #
=
îî$ %
CommandType
îî& 1
.
îî1 2
StoredProcedure
îî2 A
;
îîA B
cmd
ññ 
.
ññ 

Parameters
ññ "
.
ññ" #
Add
ññ# &
(
ññ& '
	DbFactory
ññ' 0
.
ññ0 1

GetDbParam
ññ1 ;
(
ññ; <
$str
ññ< K
,
ññK L
DbType
ññM S
.
ññS T
UInt32
ññT Z
,
ññZ [
	versionid
ññ\ e
)
ññe f
)
ññf g
;
ññg h
cmd
óó 
.
óó 

Parameters
óó "
.
óó" #
Add
óó# &
(
óó& '
	DbFactory
óó' 0
.
óó0 1

GetDbParam
óó1 ;
(
óó; <
$str
óó< K
,
óóK L
DbType
óóM S
.
óóS T
UInt32
óóT Z
,
óóZ [
	pincodeId
óó\ e
)
óóe f
)
óóf g
;
óóg h
using
òò 
(
òò 
IDataReader
òò &
dr
òò' )
=
òò* +
MySqlDatabase
òò, 9
.
òò9 :
SelectQuery
òò: E
(
òòE F
cmd
òòF I
,
òòI J
ConnectionType
òòK Y
.
òòY Z
MasterDatabase
òòZ h
)
òòh i
)
òòi j
{
ôô 
if
öö 
(
öö 
dr
öö 
!=
öö !
null
öö" &
&&
öö' )
dr
öö* ,
.
öö, -
Read
öö- 1
(
öö1 2
)
öö2 3
)
öö3 4
{
õõ 
objQuotation
úú (
.
úú( )
ProductMake
úú) 4
=
úú5 6
Convert
úú7 >
.
úú> ?
ToString
úú? G
(
úúG H
dr
úúH J
[
úúJ K
$str
úúK X
]
úúX Y
)
úúY Z
;
úúZ [
objQuotation
ùù (
.
ùù( )
Model
ùù) .
=
ùù/ 0
Convert
ùù1 8
.
ùù8 9
ToString
ùù9 A
(
ùùA B
dr
ùùB D
[
ùùD E
$str
ùùE T
]
ùùT U
)
ùùU V
;
ùùV W
objQuotation
ûû (
.
ûû( )
City
ûû) -
=
ûû. /
Convert
ûû0 7
.
ûû7 8
ToString
ûû8 @
(
ûû@ A
dr
ûûA C
[
ûûC D
$str
ûûD O
]
ûûO P
)
ûûP Q
;
ûûQ R
objQuotation
üü (
.
üü( )
State
üü) .
=
üü/ 0
Convert
üü1 8
.
üü8 9
ToString
üü9 A
(
üüA B
dr
üüB D
[
üüD E
$str
üüE T
]
üüT U
)
üüU V
;
üüV W
objQuotation
†† (
.
††( )
PinCode
††) 0
=
††1 2
Convert
††3 :
.
††: ;
ToString
††; C
(
††C D
dr
††D F
[
††F G
$str
††G P
]
††P Q
)
††Q R
;
††R S
}
°° 
}
¢¢ 
}
££ 
}
§§ 
catch
•• 
(
•• 
	Exception
•• 
ex
•• 
)
••  
{
¶¶ 
Logs
ßß 
.
ßß 
WriteErrorLog
ßß "
(
ßß" #
String
ßß# )
.
ßß) *
Format
ßß* 0
(
ßß0 1
$str
ßß1 h
,
ßßh i
ex
ßßj l
.
ßßl m
Message
ßßm t
)
ßßt u
)
ßßu v
;
ßßv w
}
®® 
return
™™ 
objQuotation
™™ 
;
™™  
}
´´ 	
public
≥≥ 
bool
≥≥ +
IsDealerDailyLeadLimitExceeds
≥≥ 1
(
≥≥1 2
uint
≥≥2 6

campaignId
≥≥7 A
)
≥≥A B
{
¥¥ 	
bool
µµ 
islimitexceeds
µµ 
=
µµ  !
false
µµ" '
;
µµ' (
try
∂∂ 
{
∑∑ 
using
ππ 
(
ππ 
	DbCommand
ππ  
cmd
ππ! $
=
ππ% &
	DbFactory
ππ' 0
.
ππ0 1
GetDBCommand
ππ1 =
(
ππ= >
)
ππ> ?
)
ππ? @
{
∫∫ 
cmd
ªª 
.
ªª 
CommandText
ªª #
=
ªª$ %
$str
ªª& E
;
ªªE F
cmd
ºº 
.
ºº 
CommandType
ºº #
=
ºº$ %
CommandType
ºº& 1
.
ºº1 2
StoredProcedure
ºº2 A
;
ººA B
cmd
ææ 
.
ææ 

Parameters
ææ "
.
ææ" #
Add
ææ# &
(
ææ& '
	DbFactory
ææ' 0
.
ææ0 1

GetDbParam
ææ1 ;
(
ææ; <
$str
ææ< L
,
ææL M
DbType
ææN T
.
ææT U
Int32
ææU Z
,
ææZ [

campaignId
ææ\ f
)
ææf g
)
ææg h
;
ææh i
cmd
øø 
.
øø 

Parameters
øø "
.
øø" #
Add
øø# &
(
øø& '
	DbFactory
øø' 0
.
øø0 1

GetDbParam
øø1 ;
(
øø; <
$str
øø< P
,
øøP Q
DbType
øøR X
.
øøX Y
Boolean
øøY `
,
øø` a 
ParameterDirection
øøb t
.
øøt u
Output
øøu {
)
øø{ |
)
øø| }
;
øø} ~
MySqlDatabase
¡¡ !
.
¡¡! "
ExecuteNonQuery
¡¡" 1
(
¡¡1 2
cmd
¡¡2 5
,
¡¡5 6
ConnectionType
¡¡7 E
.
¡¡E F
MasterDatabase
¡¡F T
)
¡¡T U
;
¡¡U V
islimitexceeds
√√ "
=
√√# $ 
SqlReaderConvertor
√√% 7
.
√√7 8
	ToBoolean
√√8 A
(
√√A B
cmd
√√B E
.
√√E F

Parameters
√√F P
[
√√P Q
$str
√√Q e
]
√√e f
.
√√f g
Value
√√g l
)
√√l m
;
√√m n
}
≈≈ 
}
∆∆ 
catch
«« 
(
«« 
	Exception
«« 
ex
«« 
)
««  
{
»» 
Logs
…… 
.
…… 
WriteErrorLog
…… "
(
……" #
String
……# )
.
……) *
Format
……* 0
(
……0 1
$str
……1 j
,
……j k

campaignId
……l v
,
……v w
ex
……x z
.
……z {
Message……{ Ç
)……Ç É
)……É Ñ
;……Ñ Ö
}
   
return
ÃÃ 
islimitexceeds
ÃÃ !
;
ÃÃ! "
}
ŒŒ 	
public
÷÷ 
bool
÷÷ 
IsLeadExists
÷÷  
(
÷÷  !
uint
÷÷! %
dealerId
÷÷& .
,
÷÷. /
string
÷÷0 6
mobile
÷÷7 =
)
÷÷= >
{
◊◊ 	
bool
ÿÿ 
IsLeadExists
ÿÿ 
=
ÿÿ 
false
ÿÿ  %
;
ÿÿ% &
try
ŸŸ 
{
⁄⁄ 
using
€€ 
(
€€ 
	DbCommand
€€  
cmd
€€! $
=
€€% &
	DbFactory
€€' 0
.
€€0 1
GetDBCommand
€€1 =
(
€€= >
)
€€> ?
)
€€? @
{
‹‹ 
cmd
›› 
.
›› 
CommandText
›› #
=
››$ %
$str
››& 9
;
››9 :
cmd
ﬁﬁ 
.
ﬁﬁ 
CommandType
ﬁﬁ #
=
ﬁﬁ$ %
CommandType
ﬁﬁ& 1
.
ﬁﬁ1 2
StoredProcedure
ﬁﬁ2 A
;
ﬁﬁA B
cmd
‡‡ 
.
‡‡ 

Parameters
‡‡ "
.
‡‡" #
Add
‡‡# &
(
‡‡& '
	DbFactory
‡‡' 0
.
‡‡0 1

GetDbParam
‡‡1 ;
(
‡‡; <
$str
‡‡< J
,
‡‡J K
DbType
‡‡L R
.
‡‡R S
Int32
‡‡S X
,
‡‡X Y
dealerId
‡‡Z b
)
‡‡b c
)
‡‡c d
;
‡‡d e
cmd
·· 
.
·· 

Parameters
·· "
.
··" #
Add
··# &
(
··& '
	DbFactory
··' 0
.
··0 1

GetDbParam
··1 ;
(
··; <
$str
··< H
,
··H I
DbType
··J P
.
··P Q
String
··Q W
,
··W X
mobile
··Y _
)
··_ `
)
··` a
;
··a b
MySqlDatabase
„„ !
.
„„! "
ExecuteNonQuery
„„" 1
(
„„1 2
cmd
„„2 5
,
„„5 6
ConnectionType
„„7 E
.
„„E F
MasterDatabase
„„F T
)
„„T U
;
„„U V
string
ÂÂ !
IsLeadExistsAlready
ÂÂ .
=
ÂÂ/ 0
MySqlDatabase
ÂÂ1 >
.
ÂÂ> ?
ExecuteScalar
ÂÂ? L
(
ÂÂL M
cmd
ÂÂM P
,
ÂÂP Q
ConnectionType
ÂÂR `
.
ÂÂ` a
MasterDatabase
ÂÂa o
)
ÂÂo p
;
ÂÂp q
if
ÊÊ 
(
ÊÊ 
!
ÊÊ 
string
ÊÊ 
.
ÊÊ  
IsNullOrEmpty
ÊÊ  -
(
ÊÊ- .!
IsLeadExistsAlready
ÊÊ. A
)
ÊÊA B
)
ÊÊB C
IsLeadExists
ÁÁ $
=
ÁÁ% &
true
ÁÁ' +
;
ÁÁ+ ,
}
ËË 
}
ÈÈ 
catch
ÍÍ 
(
ÍÍ 
	Exception
ÍÍ 
ex
ÍÍ 
)
ÍÍ  
{
ÎÎ 
Logs
ÏÏ 
.
ÏÏ 
WriteErrorLog
ÏÏ "
(
ÏÏ" #
String
ÏÏ# )
.
ÏÏ) *
Format
ÏÏ* 0
(
ÏÏ0 1
$str
ÏÏ1 }
,
ÏÏ} ~
dealerIdÏÏ á
,ÏÏá à
mobileÏÏâ è
,ÏÏè ê
exÏÏë ì
.ÏÏì î
MessageÏÏî õ
)ÏÏõ ú
)ÏÏú ù
;ÏÏù û
}
ÌÌ 
return
ÓÓ 
IsLeadExists
ÓÓ 
;
ÓÓ  
}
ÔÔ 	
public
¯¯  
RoyalEnfieldDealer
¯¯ !'
GetRoyalEnfieldDealerById
¯¯" ;
(
¯¯; <
uint
¯¯< @
re_dealerId
¯¯A L
)
¯¯L M
{
˘˘ 	 
RoyalEnfieldDealer
˙˙ 
objDealerData
˙˙ ,
=
˙˙- .
null
˙˙/ 3
;
˙˙3 4
try
˚˚ 
{
¸¸ 
objDealerData
˝˝ 
=
˝˝ 
new
˝˝  # 
RoyalEnfieldDealer
˝˝$ 6
(
˝˝6 7
)
˝˝7 8
;
˝˝8 9
using
˛˛ 
(
˛˛ 
	DbCommand
˛˛  
cmd
˛˛! $
=
˛˛% &
	DbFactory
˛˛' 0
.
˛˛0 1
GetDBCommand
˛˛1 =
(
˛˛= >
)
˛˛> ?
)
˛˛? @
{
ˇˇ 
cmd
ÄÄ 
.
ÄÄ 
CommandText
ÄÄ #
=
ÄÄ$ %
$str
ÄÄ& >
;
ÄÄ> ?
cmd
ÅÅ 
.
ÅÅ 
CommandType
ÅÅ #
=
ÅÅ$ %
CommandType
ÅÅ& 1
.
ÅÅ1 2
StoredProcedure
ÅÅ2 A
;
ÅÅA B
cmd
ÉÉ 
.
ÉÉ 

Parameters
ÉÉ "
.
ÉÉ" #
Add
ÉÉ# &
(
ÉÉ& '
	DbFactory
ÉÉ' 0
.
ÉÉ0 1

GetDbParam
ÉÉ1 ;
(
ÉÉ; <
$str
ÉÉ< J
,
ÉÉJ K
DbType
ÉÉL R
.
ÉÉR S
UInt32
ÉÉS Y
,
ÉÉY Z
re_dealerId
ÉÉ[ f
)
ÉÉf g
)
ÉÉg h
;
ÉÉh i
using
ÑÑ 
(
ÑÑ 
IDataReader
ÑÑ &
dr
ÑÑ' )
=
ÑÑ* +
MySqlDatabase
ÑÑ, 9
.
ÑÑ9 :
SelectQuery
ÑÑ: E
(
ÑÑE F
cmd
ÑÑF I
,
ÑÑI J
ConnectionType
ÑÑK Y
.
ÑÑY Z
MasterDatabase
ÑÑZ h
)
ÑÑh i
)
ÑÑi j
{
ÖÖ 
if
ÜÜ 
(
ÜÜ 
dr
ÜÜ 
!=
ÜÜ !
null
ÜÜ" &
&&
ÜÜ' )
dr
ÜÜ* ,
.
ÜÜ, -
Read
ÜÜ- 1
(
ÜÜ1 2
)
ÜÜ2 3
)
ÜÜ3 4
{
áá 
objDealerData
àà )
.
àà) *

DealerCode
àà* 4
=
àà5 6
Convert
àà7 >
.
àà> ?
ToString
àà? G
(
ààG H
dr
ààH J
[
ààJ K
$str
ààK W
]
ààW X
)
ààX Y
;
ààY Z
objDealerData
ââ )
.
ââ) *

DealerName
ââ* 4
=
ââ5 6
Convert
ââ7 >
.
ââ> ?
ToString
ââ? G
(
ââG H
dr
ââH J
[
ââJ K
$str
ââK W
]
ââW X
)
ââX Y
;
ââY Z
objDealerData
ää )
.
ää) *

DealerCity
ää* 4
=
ää5 6
Convert
ää7 >
.
ää> ?
ToString
ää? G
(
ääG H
dr
ääH J
[
ääJ K
$str
ääK W
]
ääW X
)
ääX Y
;
ääY Z
objDealerData
ãã )
.
ãã) *
DealerState
ãã* 5
=
ãã6 7
Convert
ãã8 ?
.
ãã? @
ToString
ãã@ H
(
ããH I
dr
ããI K
[
ããK L
$str
ããL Y
]
ããY Z
)
ããZ [
;
ãã[ \
}
åå 
}
çç 
}
éé 
}
èè 
catch
êê 
(
êê 
	Exception
êê 
ex
êê 
)
êê  
{
ëë 
Logs
íí 
.
íí 
WriteErrorLog
íí "
(
íí" #
String
íí# )
.
íí) *
Format
íí* 0
(
íí0 1
$str
íí1 f
,
ííf g
re_dealerId
ííh s
,
íís t
ex
ííu w
.
ííw x
Message
ííx 
)íí Ä
)ííÄ Å
;ííÅ Ç
}
ìì 
return
ïï 
objDealerData
ïï  
;
ïï  !
}
ññ 	
public
ûû 
string
ûû $
GetTataCapitalByCityId
ûû ,
(
ûû, -
uint
ûû- 1
cityId
ûû2 8
)
ûû8 9
{
üü 	
string
†† 
tataCapitalCityId
†† $
=
††% &
string
††' -
.
††- .
Empty
††. 3
;
††3 4
try
°° 
{
¢¢ 
using
§§ 
(
§§ 
	DbCommand
§§  
cmd
§§! $
=
§§% &
	DbFactory
§§' 0
.
§§0 1
GetDBCommand
§§1 =
(
§§= >
)
§§> ?
)
§§? @
{
•• 
cmd
¶¶ 
.
¶¶ 
CommandText
¶¶ #
=
¶¶$ %
$str
¶¶& ;
;
¶¶; <
cmd
ßß 
.
ßß 
CommandType
ßß #
=
ßß$ %
CommandType
ßß& 1
.
ßß1 2
StoredProcedure
ßß2 A
;
ßßA B
cmd
®® 
.
®® 

Parameters
®® "
.
®®" #
Add
®®# &
(
®®& '
	DbFactory
®®' 0
.
®®0 1

GetDbParam
®®1 ;
(
®®; <
$str
®®< H
,
®®H I
DbType
®®J P
.
®®P Q
Int32
®®Q V
,
®®V W
cityId
®®X ^
)
®®^ _
)
®®_ `
;
®®` a
using
©© 
(
©© 
IDataReader
©© &
dr
©©' )
=
©©* +
MySqlDatabase
©©, 9
.
©©9 :
SelectQuery
©©: E
(
©©E F
cmd
©©F I
,
©©I J
ConnectionType
©©K Y
.
©©Y Z
MasterDatabase
©©Z h
)
©©h i
)
©©i j
{
™™ 
if
´´ 
(
´´ 
dr
´´ 
!=
´´ !
null
´´" &
&&
´´' )
dr
´´* ,
.
´´, -
Read
´´- 1
(
´´1 2
)
´´2 3
)
´´3 4
{
¨¨ 
tataCapitalCityId
≠≠ -
=
≠≠. /
Convert
≠≠0 7
.
≠≠7 8
ToString
≠≠8 @
(
≠≠@ A
dr
≠≠A C
[
≠≠C D
$str
≠≠D J
]
≠≠J K
)
≠≠K L
;
≠≠L M
}
ÆÆ 
}
ØØ 
}
∞∞ 
}
±± 
catch
≤≤ 
(
≤≤ 
	Exception
≤≤ 
ex
≤≤ 
)
≤≤  
{
≥≥ 
Logs
¥¥ 
.
¥¥ 
WriteErrorLog
¥¥ "
(
¥¥" #
String
¥¥# )
.
¥¥) *
Format
¥¥* 0
(
¥¥0 1
$str
¥¥1 c
,
¥¥c d
cityId
¥¥e k
,
¥¥k l
ex
¥¥m o
.
¥¥o p
Message
¥¥p w
)
¥¥w x
)
¥¥x y
;
¥¥y z
}
µµ 
return
∂∂ 
tataCapitalCityId
∂∂ $
;
∂∂$ %
}
∑∑ 	
public
øø 
bool
øø +
IsManufacturerLeadLimitExceed
øø 1
(
øø1 2
uint
øø2 6

campaignId
øø7 A
)
øøA B
{
¿¿ 	
bool
¡¡ 
islimitexceeds
¡¡ 
=
¡¡  !
false
¡¡" '
;
¡¡' (
try
¬¬ 
{
√√ 
using
≈≈ 
(
≈≈ 
	DbCommand
≈≈  
cmd
≈≈! $
=
≈≈% &
	DbFactory
≈≈' 0
.
≈≈0 1
GetDBCommand
≈≈1 =
(
≈≈= >
)
≈≈> ?
)
≈≈? @
{
∆∆ 
cmd
«« 
.
«« 
CommandText
«« #
=
««$ %
$str
««& N
;
««N O
cmd
»» 
.
»» 
CommandType
»» #
=
»»$ %
CommandType
»»& 1
.
»»1 2
StoredProcedure
»»2 A
;
»»A B
cmd
   
.
   

Parameters
   "
.
  " #
Add
  # &
(
  & '
	DbFactory
  ' 0
.
  0 1

GetDbParam
  1 ;
(
  ; <
$str
  < L
,
  L M
DbType
  N T
.
  T U
Int32
  U Z
,
  Z [

campaignId
  \ f
)
  f g
)
  g h
;
  h i
cmd
ÀÀ 
.
ÀÀ 

Parameters
ÀÀ "
.
ÀÀ" #
Add
ÀÀ# &
(
ÀÀ& '
	DbFactory
ÀÀ' 0
.
ÀÀ0 1

GetDbParam
ÀÀ1 ;
(
ÀÀ; <
$str
ÀÀ< O
,
ÀÀO P
DbType
ÀÀQ W
.
ÀÀW X
Boolean
ÀÀX _
,
ÀÀ_ ` 
ParameterDirection
ÀÀa s
.
ÀÀs t
Output
ÀÀt z
)
ÀÀz {
)
ÀÀ{ |
;
ÀÀ| }
MySqlDatabase
ÕÕ !
.
ÕÕ! "
ExecuteNonQuery
ÕÕ" 1
(
ÕÕ1 2
cmd
ÕÕ2 5
,
ÕÕ5 6
ConnectionType
ÕÕ7 E
.
ÕÕE F
MasterDatabase
ÕÕF T
)
ÕÕT U
;
ÕÕU V
islimitexceeds
œœ "
=
œœ# $ 
SqlReaderConvertor
œœ% 7
.
œœ7 8
	ToBoolean
œœ8 A
(
œœA B
cmd
œœB E
.
œœE F

Parameters
œœF P
[
œœP Q
$str
œœQ d
]
œœd e
.
œœe f
Value
œœf k
)
œœk l
;
œœl m
}
—— 
}
““ 
catch
”” 
(
”” 
	Exception
”” 
ex
”” 
)
””  
{
‘‘ 
Logs
’’ 
.
’’ 
WriteErrorLog
’’ "
(
’’" #
String
’’# )
.
’’) *
Format
’’* 0
(
’’0 1
$str
’’1 j
,
’’j k

campaignId
’’l v
,
’’v w
ex
’’x z
.
’’z {
Message’’{ Ç
)’’Ç É
)’’É Ñ
;’’Ñ Ö
}
÷÷ 
return
◊◊ 
islimitexceeds
◊◊ !
;
◊◊! "
}
ÿÿ 	
public
‡‡ 
bool
‡‡ .
 UpdateManufacturerDailyLeadCount
‡‡ 4
(
‡‡4 5
uint
‡‡5 9

campaignId
‡‡: D
,
‡‡D E
uint
‡‡F J
abInquiryId
‡‡K V
)
‡‡V W
{
·· 	
bool
‚‚ !
isUpdateDealerCount
‚‚ $
=
‚‚% &
false
‚‚' ,
;
‚‚, -
try
„„ 
{
‰‰ 
using
ÂÂ 
(
ÂÂ 
	DbCommand
ÂÂ  
cmd
ÂÂ! $
=
ÂÂ% &
	DbFactory
ÂÂ' 0
.
ÂÂ0 1
GetDBCommand
ÂÂ1 =
(
ÂÂ= >
)
ÂÂ> ?
)
ÂÂ? @
{
ÊÊ 
cmd
ÁÁ 
.
ÁÁ 
CommandText
ÁÁ #
=
ÁÁ$ %
$str
ÁÁ& K
;
ÁÁK L
cmd
ËË 
.
ËË 
CommandType
ËË #
=
ËË$ %
CommandType
ËË& 1
.
ËË1 2
StoredProcedure
ËË2 A
;
ËËA B
cmd
ÍÍ 
.
ÍÍ 

Parameters
ÍÍ "
.
ÍÍ" #
Add
ÍÍ# &
(
ÍÍ& '
	DbFactory
ÍÍ' 0
.
ÍÍ0 1

GetDbParam
ÍÍ1 ;
(
ÍÍ; <
$str
ÍÍ< L
,
ÍÍL M
DbType
ÍÍN T
.
ÍÍT U
Int32
ÍÍU Z
,
ÍÍZ [

campaignId
ÍÍ\ f
)
ÍÍf g
)
ÍÍg h
;
ÍÍh i
cmd
ÎÎ 
.
ÎÎ 

Parameters
ÎÎ "
.
ÎÎ" #
Add
ÎÎ# &
(
ÎÎ& '
	DbFactory
ÎÎ' 0
.
ÎÎ0 1

GetDbParam
ÎÎ1 ;
(
ÎÎ; <
$str
ÎÎ< M
,
ÎÎM N
DbType
ÎÎO U
.
ÎÎU V
Int32
ÎÎV [
,
ÎÎ[ \
abInquiryId
ÎÎ] h
)
ÎÎh i
)
ÎÎi j
;
ÎÎj k
MySqlDatabase
ÏÏ !
.
ÏÏ! "
ExecuteNonQuery
ÏÏ" 1
(
ÏÏ1 2
cmd
ÏÏ2 5
,
ÏÏ5 6
ConnectionType
ÏÏ7 E
.
ÏÏE F
MasterDatabase
ÏÏF T
)
ÏÏT U
;
ÏÏU V!
isUpdateDealerCount
ÌÌ '
=
ÌÌ( )
true
ÌÌ* .
;
ÌÌ. /
}
ÓÓ 
}
ÔÔ 
catch
 
(
 
	Exception
 
ex
 
)
  
{
ÒÒ 
Logs
ÚÚ 
.
ÚÚ 
WriteErrorLog
ÚÚ "
(
ÚÚ" #
String
ÚÚ# )
.
ÚÚ) *
Format
ÚÚ* 0
(
ÚÚ0 1
$str
ÚÚ1 q
,
ÚÚq r

campaignId
ÚÚs }
,
ÚÚ} ~
abInquiryIdÚÚ ä
,ÚÚä ã
exÚÚå é
.ÚÚé è
MessageÚÚè ñ
)ÚÚñ ó
)ÚÚó ò
;ÚÚò ô
}
ÛÛ 
return
ÙÙ !
isUpdateDealerCount
ÙÙ &
;
ÙÙ& '
}
ıı 	
public
ÄÄ 
bool
ÄÄ )
UpdateManufacturerABInquiry
ÄÄ /
(
ÄÄ/ 0
uint
ÄÄ0 4
leadId
ÄÄ5 ;
,
ÄÄ; <
uint
ÄÄ= A
abInquiryId
ÄÄB M
,
ÄÄM N
UInt16
ÄÄO U

retryCount
ÄÄV `
)
ÄÄ` a
{
ÅÅ 	
bool
ÇÇ !
isUpdateDealerCount
ÇÇ $
=
ÇÇ% &
false
ÇÇ' ,
;
ÇÇ, -
try
ÉÉ 
{
ÑÑ 
using
ÖÖ 
(
ÖÖ 
	DbCommand
ÖÖ  
cmd
ÖÖ! $
=
ÖÖ% &
	DbFactory
ÖÖ' 0
.
ÖÖ0 1
GetDBCommand
ÖÖ1 =
(
ÖÖ= >
)
ÖÖ> ?
)
ÖÖ? @
{
ÜÜ 
cmd
áá 
.
áá 
CommandText
áá #
=
áá$ %
$str
áá& C
;
ááC D
cmd
àà 
.
àà 
CommandType
àà #
=
àà$ %
CommandType
àà& 1
.
àà1 2
StoredProcedure
àà2 A
;
ààA B
cmd
ää 
.
ää 

Parameters
ää "
.
ää" #
Add
ää# &
(
ää& '
	DbFactory
ää' 0
.
ää0 1

GetDbParam
ää1 ;
(
ää; <
$str
ää< D
,
ääD E
DbType
ääF L
.
ääL M
Int32
ääM R
,
ääR S
leadId
ääT Z
)
ääZ [
)
ää[ \
;
ää\ ]
cmd
ãã 
.
ãã 

Parameters
ãã "
.
ãã" #
Add
ãã# &
(
ãã& '
	DbFactory
ãã' 0
.
ãã0 1

GetDbParam
ãã1 ;
(
ãã; <
$str
ãã< M
,
ããM N
DbType
ããO U
.
ããU V
Int32
ããV [
,
ãã[ \
abInquiryId
ãã] h
)
ããh i
)
ããi j
;
ããj k
cmd
åå 
.
åå 

Parameters
åå "
.
åå" #
Add
åå# &
(
åå& '
	DbFactory
åå' 0
.
åå0 1

GetDbParam
åå1 ;
(
åå; <
$str
åå< Q
,
ååQ R
DbType
ååS Y
.
ååY Z
Int16
ååZ _
,
åå_ `

retryCount
ååa k
)
ååk l
)
åål m
;
ååm n
MySqlDatabase
çç !
.
çç! "
ExecuteNonQuery
çç" 1
(
çç1 2
cmd
çç2 5
,
çç5 6
ConnectionType
çç7 E
.
ççE F
MasterDatabase
ççF T
)
ççT U
;
ççU V!
isUpdateDealerCount
éé '
=
éé( )
true
éé* .
;
éé. /
}
èè 
}
êê 
catch
ëë 
(
ëë 
	Exception
ëë 
ex
ëë 
)
ëë  
{
íí 
Logs
ìì 
.
ìì 
WriteErrorLog
ìì "
(
ìì" #
String
ìì# )
.
ìì) *
Format
ìì* 0
(
ìì0 1
$str
ìì1 l
,
ììl m
leadId
ììn t
,
ììt u
abInquiryIdììv Å
,ììÅ Ç
exììÉ Ö
.ììÖ Ü
MessageììÜ ç
)ììç é
)ììé è
;ììè ê
}
îî 
return
ïï !
isUpdateDealerCount
ïï &
;
ïï& '
}
ññ 	
}
óó 
}òò “Ù
MD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\LeadProcessor.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
internal 
class 
LeadConsumer 
{ 
private 
readonly 
string 
_applicationName  0
,0 1
_retryCount2 =
,= >
_RabbitMsgTTL? L
;L M
private 
readonly 
LeadProcessor &
_leadProcessor' 5
;5 6
private 
readonly $
LeadProcessingRepository 1
_repository2 =
=> ?
null@ D
;D E
private 
IConnection 
_connetionRabbitMq .
;. /
private 
IModel 
_model 
; 
private !
QueueingBasicConsumer %
consumer& .
;. /
private 
NameValueCollection #
nvc$ '
=( )
new* -
NameValueCollection. A
(A B
)B C
;C D
private 
string 

_queueName !
,! "
	_hostName# ,
;, -
public   
LeadConsumer   
(   
)   
{!! 	
try"" 
{## 

_queueName$$ 
=$$ 
String$$ #
.$$# $
Format$$$ *
($$* +
$str$$+ ?
,$$? @ 
ConfigurationManager$$A U
.$$U V
AppSettings$$V a
[$$a b
$str$$b m
]$$m n
.$$n o
ToUpper$$o v
($$v w
)$$w x
)$$x y
;$$y z
	_hostName%% 
=%% 
CreateConnection%% ,
.%%, -
nodes%%- 2
[%%2 3
(%%3 4
new%%4 7
Random%%8 >
(%%> ?
)%%? @
)%%@ A
.%%A B
Next%%B F
(%%F G
CreateConnection%%G W
.%%W X
nodes%%X ]
.%%] ^
Count%%^ c
)%%c d
]%%d e
;%%e f
SendMail'' 
.'' 
APPLICATION'' $
=''% &
_applicationName''' 7
=''8 9
Convert'': A
.''A B
ToString''B J
(''J K 
ConfigurationManager''K _
.''_ `
AppSettings''` k
[''k l
$str''l z
]''z {
)''{ |
;''| }
_retryCount(( 
=((  
ConfigurationManager(( 2
.((2 3
AppSettings((3 >
[((> ?
$str((? K
]((K L
;((L M
_RabbitMsgTTL)) 
=))  
ConfigurationManager))  4
.))4 5
AppSettings))5 @
[))@ A
$str))A O
]))O P
;))P Q
InitConsumer** 
(** 
)** 
;** 
_leadProcessor++ 
=++  
new++! $
LeadProcessor++% 2
(++2 3
)++3 4
;++4 5
_repository,, 
=,, 
new,, !$
LeadProcessingRepository,," :
(,,: ;
),,; <
;,,< =
}-- 
catch.. 
(.. 
	Exception.. 
ex.. 
)..  
{// 
SendMail00 
.00 
HandleException00 (
(00( )
ex00) +
,00+ ,
String00- 3
.003 4
Format004 :
(00: ;
$str00; f
,00f g
_applicationName00h x
)00x y
)00y z
;00z {
}11 
}22 	
private44 
void44 
InitConsumer44 !
(44! "
)44" #
{55 	
try66 
{77 
CreateConnection99  
.99  !
Connect99! (
(99( )

_queueName99) 3
,993 4
	_hostName995 >
)99> ?
;99? @
_connetionRabbitMq:: "
=::# $
CreateConnection::% 5
.::5 6

Connection::6 @
;::@ A
if;; 
(;; 
_connetionRabbitMq;; &
!=;;' )
null;;* .
);;. /
{<< 
CreateConnection== $
.==$ %
CreateChannel==% 2
(==2 3
)==3 4
;==4 5
_model>> 
=>> 
CreateConnection>> -
.>>- .
Model>>. 3
;>>3 4
if?? 
(?? 
_model?? 
==?? !
null??" &
)??& '
{@@ 
SendMailAA  
.AA  !
HandleExceptionAA! 0
(AA0 1
newAA1 4
	ExceptionAA5 >
(AA> ?
$strAA? O
)AAO P
,AAP Q
StringAAR X
.AAX Y
FormatAAY _
(AA_ `
$strAA` w
,AAw x
_applicationName	AAy â
)
AAâ ä
)
AAä ã
;
AAã å
}BB 
elseCC 
{DD 
consumerEE  
=EE! "
CreateConnectionEE# 3
.EE3 4
CreateConsumerEE4 B
(EEB C
)EEC D
;EED E
ifFF 
(FF 
consumerFF $
==FF% '
nullFF( ,
)FF, -
{GG 
SendMailHH $
.HH$ %
HandleExceptionHH% 4
(HH4 5
newHH5 8
	ExceptionHH9 B
(HHB C
$strHHC p
)HHp q
,HHq r
StringHHs y
.HHy z
Format	HHz Ä
(
HHÄ Å
$str
HHÅ ò
,
HHò ô
_applicationName
HHö ™
)
HH™ ´
)
HH´ ¨
;
HH¨ ≠
}II 
elseJJ 
{KK 
consumerLL $
.LL$ %
ConsumerCancelledLL% 6
+=LL7 9&
consumer_ConsumerCancelledLL: T
;LLT U
}MM 
}NN 
}OO 
elsePP 
{QQ 
SendMailRR 
.RR 
HandleExceptionRR ,
(RR, -
newRR- 0
	ExceptionRR1 :
(RR: ;
$strRR; W
)RRW X
,RRX Y
StringRRZ `
.RR` a
FormatRRa g
(RRg h
$strRRh 
,	RR Ä
_applicationName
RRÅ ë
)
RRë í
)
RRí ì
;
RRì î
}SS 
}TT 
catchUU 
(UU 
	ExceptionUU 
exUU 
)UU  
{VV 
SendMailWW 
.WW 
HandleExceptionWW (
(WW( )
exWW) +
,WW+ ,
StringWW- 3
.WW3 4
FormatWW4 :
(WW: ;
$strWW; a
,WWa b
_applicationNameWWc s
)WWs t
)WWt u
;WWu v
}XX 
}YY 	
public[[ 
void[[ 
ProcessMessages[[ #
([[# $
)[[$ %
{\\ 	
try]] 
{^^ 
while__ 
(__ 
true__ 
&&__ 
_leadProcessor__ -
!=__. 0
null__1 5
)__5 6
{`` 
Logsaa 
.aa 
WriteInfoLogaa %
(aa% &
$straa& K
)aaK L
;aaL M
RabbitMQbb 
.bb 
Clientbb #
.bb# $
Eventsbb$ *
.bb* +!
BasicDeliverEventArgsbb+ @
argbbA D
=bbE F
(bbG H
RabbitMQbbH P
.bbP Q
ClientbbQ W
.bbW X
EventsbbX ^
.bb^ _!
BasicDeliverEventArgsbb_ t
)bbt u
consumerbbu }
.bb} ~
Queue	bb~ É
.
bbÉ Ñ
Dequeue
bbÑ ã
(
bbã å
)
bbå ç
;
bbç é
trycc 
{dd 
nvcee 
=ee 
ByteArrayToObjectee /
(ee/ 0
argee0 3
.ee3 4
Bodyee4 8
)ee8 9
;ee9 :
uintff 
pqIdff !
,ff! "
dealerIdff# +
,ff+ ,
	pincodeIdff- 6
,ff6 7
leadSourceIdff8 D
,ffD E
	versionIdffF O
,ffO P
cityIdffQ W
,ffW X 
manufacturerDealerIdffY m
,ffm n
manufacturerLeadId	ffo Å
;
ffÅ Ç
	LeadTypesgg !
leadTypegg" *
=gg+ ,
defaultgg- 4
(gg4 5
	LeadTypesgg5 >
)gg> ?
;gg? @
ifhh 
(hh 
nvchh 
!=hh  "
nullhh# '
&&ii 
nvcii "
.ii" #
HasKeysii# *
(ii* +
)ii+ ,
&&jj 
UInt32jj %
.jj% &
TryParsejj& .
(jj. /
nvcjj/ 2
[jj2 3
$strjj3 9
]jj9 :
,jj: ;
outjj< ?
pqIdjj@ D
)jjD E
&&jjF H
pqIdjjI M
>jjN O
$numjjP Q
&&kk 
UInt32kk %
.kk% &
TryParsekk& .
(kk. /
nvckk/ 2
[kk2 3
$strkk3 =
]kk= >
,kk> ?
outkk@ C
dealerIdkkD L
)kkL M
&&kkN P
dealerIdkkQ Y
>kkZ [
$numkk\ ]
&&ll 
UInt32ll %
.ll% &
TryParsell& .
(ll. /
nvcll/ 2
[ll2 3
$strll3 >
]ll> ?
,ll? @
outllA D
	versionIdllE N
)llN O
&&llP R
	versionIdllS \
>ll] ^
$numll_ `
&&mm 
UInt32mm %
.mm% &
TryParsemm& .
(mm. /
nvcmm/ 2
[mm2 3
$strmm3 ;
]mm; <
,mm< =
outmm> A
cityIdmmB H
)mmH I
&&mmJ L
cityIdmmM S
>mmT U
$nummmV W
)nn 
{oo 
Logspp  
.pp  !
WriteInfoLogpp! -
(pp- .
Stringpp. 4
.pp4 5
Formatpp5 ;
(pp; <
$strpp< y
,ppy z
pqIdpp{ 
,	pp Ä
dealerId
ppÅ â
)
ppâ ä
)
ppä ã
;
ppã å
Enumrr  
.rr  !
TryParserr! )
(rr) *
nvcrr* -
[rr- .
$strrr. 8
]rr8 9
,rr9 :
outrr; >
leadTyperr? G
)rrG H
;rrH I
UInt32ss "
.ss" #
TryParsess# +
(ss+ ,
nvcss, /
[ss/ 0
$strss0 ;
]ss; <
,ss< =
outss> A
	pincodeIdssB K
)ssK L
;ssL M
UInt32tt "
.tt" #
TryParsett# +
(tt+ ,
nvctt, /
[tt/ 0
$strtt0 >
]tt> ?
,tt? @
outttA D
leadSourceIdttE Q
)ttQ R
;ttR S
UInt32uu "
.uu" #
TryParseuu# +
(uu+ ,
nvcuu, /
[uu/ 0
$struu0 F
]uuF G
,uuG H
outuuI L 
manufacturerDealerIduuM a
)uua b
;uub c
UInt32vv "
.vv" #
TryParsevv# +
(vv+ ,
nvcvv, /
[vv/ 0
$strvv0 D
]vvD E
,vvE F
outvvG J
manufacturerLeadIdvvK ]
)vv] ^
;vv^ _
varww 

priceQuoteww  *
=ww+ ,
_leadProcessorww- ;
.ww; < 
GetPriceQuoteDetailsww< P
(wwP Q
pqIdwwQ U
)wwU V
;wwV W
ifzz 
(zz  

priceQuotezz  *
!=zz+ -
nullzz. 2
)zz2 3
{{{ 
if||  "
(||# $
nvc||$ '
[||' (
$str||( 3
]||3 4
==||5 7
_retryCount||8 C
)||C D
{}}  !
_model~~$ *
.~~* +
BasicReject~~+ 6
(~~6 7
arg~~7 :
.~~: ;
DeliveryTag~~; F
,~~F G
false~~H M
)~~M N
;~~N O
Logs$ (
.( )
WriteInfoLog) 5
(5 6
String6 <
.< =
Format= C
(C D
$strD y
,y z

Newtonsoft	{ Ö
.
Ö Ü
Json
Ü ä
.
ä ã
JsonConvert
ã ñ
.
ñ ó
SerializeObject
ó ¶
(
¶ ß
nvc
ß ™
)
™ ´
,
´ ¨
nvc
≠ ∞
[
∞ ±
$str
± º
]
º Ω
)
Ω æ
)
æ ø
;
ø ¿
continue
ÄÄ$ ,
;
ÄÄ, -
}
ÅÅ  !
UInt16
ÉÉ  &
	iteration
ÉÉ' 0
=
ÉÉ1 2
(
ÉÉ3 4
UInt16
ÉÉ4 :
)
ÉÉ: ;
(
ÉÉ; <
nvc
ÉÉ< ?
[
ÉÉ? @
$str
ÉÉ@ K
]
ÉÉK L
==
ÉÉM O
null
ÉÉP T
?
ÉÉU V
$num
ÉÉW X
:
ÉÉY Z
(
ÉÉ[ \
UInt16
ÉÉ\ b
.
ÉÉb c
Parse
ÉÉc h
(
ÉÉh i
nvc
ÉÉi l
[
ÉÉl m
$str
ÉÉm x
]
ÉÉx y
)
ÉÉy z
+
ÉÉ{ |
$num
ÉÉ} ~
)
ÉÉ~ 
)ÉÉ Ä
;ÉÉÄ Å
bool
ÖÖ  $
success
ÖÖ% ,
=
ÖÖ- .
false
ÖÖ/ 4
;
ÖÖ4 5
switch
ÜÜ  &
(
ÜÜ' (
leadType
ÜÜ( 0
)
ÜÜ0 1
{
áá  !
case
àà$ (
	LeadTypes
àà) 2
.
àà2 3
Dealer
àà3 9
:
àà9 :
success
ââ( /
=
ââ0 1
PushDealerLead
ââ2 @
(
ââ@ A

priceQuote
ââA K
,
ââK L
pqId
ââM Q
,
ââQ R
	iteration
ââS \
)
ââ\ ]
;
ââ] ^
break
ää( -
;
ää- .
case
ãã$ (
	LeadTypes
ãã) 2
.
ãã2 3
Manufacturer
ãã3 ?
:
ãã? @
success
åå( /
=
åå0 1"
PushManufacturerLead
åå2 F
(
ååF G

priceQuote
ååG Q
,
ååQ R
pqId
ååS W
,
ååW X
	pincodeId
ååY b
,
ååb c
leadSourceId
ååd p
,
ååp q
	iteration
åår {
,
åå{ |#
manufacturerDealerIdåå} ë
,ååë í"
manufacturerLeadIdååì •
)åå• ¶
;åå¶ ß
break
çç( -
;
çç- .
default
éé$ +
:
éé+ ,
success
èè( /
=
èè0 1
PushDealerLead
èè2 @
(
èè@ A

priceQuote
èèA K
,
èèK L
pqId
èèM Q
,
èèQ R
	iteration
èèS \
)
èè\ ]
;
èè] ^
break
êê( -
;
êê- .
}
ëë  !
if
ìì  "
(
ìì# $
success
ìì$ +
)
ìì+ ,
{
îî  !
Logs
ïï$ (
.
ïï( )
WriteInfoLog
ïï) 5
(
ïï5 6
String
ïï6 <
.
ïï< =
Format
ïï= C
(
ïïC D
$str
ïïD ~
,
ïï~ 
pqIdïïÄ Ñ
,ïïÑ Ö
dealerIdïïÜ é
)ïïé è
)ïïè ê
;ïïê ë
_model
ññ$ *
.
ññ* +
BasicAck
ññ+ 3
(
ññ3 4
arg
ññ4 7
.
ññ7 8
DeliveryTag
ññ8 C
,
ññC D
false
ññE J
)
ññJ K
;
ññK L
}
óó  !
else
òò  $
{
ôô  !
Logs
öö$ (
.
öö( )
WriteInfoLog
öö) 5
(
öö5 6
String
öö6 <
.
öö< =
Format
öö= C
(
ööC D
$strööD â
,ööâ ä
pqIdööã è
,ööè ê
dealerIdööë ô
)ööô ö
)ööö õ
;ööõ ú
DeadLetterPublish
õõ$ 5
(
õõ5 6
nvc
õõ6 9
,
õõ9 :"
ConfigurationManager
õõ; O
.
õõO P
AppSettings
õõP [
[
õõ[ \
$str
õõ\ g
]
õõg h
.
õõh i
ToUpper
õõi p
(
õõp q
)
õõq r
)
õõr s
;
õõs t
_model
úú$ *
.
úú* +
BasicReject
úú+ 6
(
úú6 7
arg
úú7 :
.
úú: ;
DeliveryTag
úú; F
,
úúF G
false
úúH M
)
úúM N
;
úúN O
}
ùù  !
}
ûû 
else
üü  
{
†† 
_model
°°  &
.
°°& '
BasicReject
°°' 2
(
°°2 3
arg
°°3 6
.
°°6 7
DeliveryTag
°°7 B
,
°°B C
false
°°D I
)
°°I J
;
°°J K
Logs
¢¢  $
.
¢¢$ %
WriteInfoLog
¢¢% 1
(
¢¢1 2
String
¢¢2 8
.
¢¢8 9
Format
¢¢9 ?
(
¢¢? @
$str
¢¢@ u
,
¢¢u v
pqId
¢¢w {
,
¢¢{ |
dealerId¢¢} Ö
)¢¢Ö Ü
)¢¢Ü á
;¢¢á à
}
££ 
}
§§ 
else
•• 
{
¶¶ 
_model
ßß "
.
ßß" #
BasicReject
ßß# .
(
ßß. /
arg
ßß/ 2
.
ßß2 3
DeliveryTag
ßß3 >
,
ßß> ?
false
ßß@ E
)
ßßE F
;
ßßF G
Logs
®®  
.
®®  !
WriteInfoLog
®®! -
(
®®- .
String
®®. 4
.
®®4 5
Format
®®5 ;
(
®®; <
$str
®®< j
,
®®j k
nvc
®®l o
[
®®o p
$str
®®p v
]
®®v w
,
®®w x
nvc
®®y |
[
®®| }
$str®®} á
]®®á à
)®®à â
)®®â ä
;®®ä ã
}
©© 
}
™™ 
catch
´´ 
(
´´ 
	Exception
´´ $
ex
´´% '
)
´´' (
{
¨¨ 
_model
≠≠ 
.
≠≠ 
BasicReject
≠≠ *
(
≠≠* +
arg
≠≠+ .
.
≠≠. /
DeliveryTag
≠≠/ :
,
≠≠: ;
false
≠≠< A
)
≠≠A B
;
≠≠B C
Logs
ÆÆ 
.
ÆÆ 
WriteInfoLog
ÆÆ )
(
ÆÆ) *
String
ÆÆ* 0
.
ÆÆ0 1
Format
ÆÆ1 7
(
ÆÆ7 8
$strÆÆ8 ä
,ÆÆä ã
nvcÆÆå è
[ÆÆè ê
$strÆÆê ñ
]ÆÆñ ó
,ÆÆó ò
nvcÆÆô ú
[ÆÆú ù
$strÆÆù ß
]ÆÆß ®
,ÆÆ® ©
exÆÆ™ ¨
.ÆÆ¨ ≠
MessageÆÆ≠ ¥
)ÆÆ¥ µ
)ÆÆµ ∂
;ÆÆ∂ ∑
}
ØØ 
}
∞∞ 
}
±± 
catch
≤≤ 
(
≤≤ 
	Exception
≤≤ 
ex
≤≤ 
)
≤≤  
{
≥≥ 
Logs
¥¥ 
.
¥¥ 
WriteErrorLog
¥¥ "
(
¥¥" #
$str
¥¥# 6
+
¥¥7 8
ex
¥¥9 ;
.
¥¥; <
Message
¥¥< C
)
¥¥C D
;
¥¥D E
SendMail
µµ 
.
µµ 
HandleException
µµ (
(
µµ( )
ex
µµ) +
,
µµ+ ,
String
µµ- 3
.
µµ3 4
Format
µµ4 :
(
µµ: ;
$str
µµ; d
,
µµd e
_applicationName
µµf v
)
µµv w
)
µµw x
;
µµx y
}
∂∂ 
}
∑∑ 	
private
≈≈ 
bool
≈≈ "
PushManufacturerLead
≈≈ )
(
≈≈) *(
PriceQuoteParametersEntity
≈≈* D

priceQuote
≈≈E O
,
≈≈O P
uint
≈≈Q U
pqId
≈≈V Z
,
≈≈Z [
uint
≈≈\ `
	pincodeId
≈≈a j
,
≈≈j k
uint
≈≈l p
leadSourceId
≈≈q }
,
≈≈} ~
ushort≈≈ Ö
	iteration≈≈Ü è
,≈≈è ê
uint≈≈ë ï$
manufacturerDealerId≈≈ñ ™
,≈≈™ ´
uint≈≈¨ ∞"
manufacturerLeadId≈≈± √
)≈≈√ ƒ
{
∆∆ 	
bool
«« 
	isSuccess
«« 
=
«« 
false
«« "
;
««" #
try
»» 
{
…… 
if
   
(
   

priceQuote
   
!=
   !
null
  " &
)
  & '
{
ÀÀ 
Logs
ÃÃ 
.
ÃÃ 
WriteInfoLog
ÃÃ %
(
ÃÃ% &
String
ÃÃ& ,
.
ÃÃ, -
Format
ÃÃ- 3
(
ÃÃ3 4
$str
ÃÃ4 [
)
ÃÃ[ \
)
ÃÃ\ ]
;
ÃÃ] ^
string
ÕÕ  
jsonInquiryDetails
ÕÕ -
=
ÕÕ. /
String
ÕÕ0 6
.
ÕÕ6 7
Format
ÕÕ7 =
(
ÕÕ= >
$strÕÕ> °
,ÕÕ° ¢

priceQuoteÕÕ£ ≠
.ÕÕ≠ Æ
CustomerNameÕÕÆ ∫
,ÕÕ∫ ª

priceQuoteÕÕº ∆
.ÕÕ∆ «
CustomerMobileÕÕ« ’
,ÕÕ’ ÷

priceQuoteÕÕ◊ ·
.ÕÕ· ‚
CustomerEmailÕÕ‚ Ô
,ÕÕÔ 

priceQuoteÕÕÒ ˚
.ÕÕ˚ ¸
	VersionIdÕÕ¸ Ö
,ÕÕÖ Ü

priceQuoteÕÕá ë
.ÕÕë í
CityIdÕÕí ò
,ÕÕò ô

priceQuoteÕÕö §
.ÕÕ§ •

CampaignIdÕÕ• Ø
)ÕÕØ ∞
;ÕÕ∞ ±$
ManufacturerLeadEntity
ŒŒ *

leadEntity
ŒŒ+ 5
=
ŒŒ6 7
new
ŒŒ8 ;$
ManufacturerLeadEntity
ŒŒ< R
(
ŒŒR S
)
ŒŒS T
{
œœ 
PQId
–– 
=
–– 
pqId
–– #
,
––# $
Mobile
—— 
=
——  

priceQuote
——! +
.
——+ ,
CustomerMobile
——, :
,
——: ;
Email
““ 
=
““ 

priceQuote
““  *
.
““* +
CustomerEmail
““+ 8
,
““8 9
Name
”” 
=
”” 

priceQuote
”” )
.
””) *
CustomerName
””* 6
,
””6 7
DealerId
‘‘  
=
‘‘! "

priceQuote
‘‘# -
.
‘‘- .
DealerId
‘‘. 6
,
‘‘6 7
	PinCodeId
’’ !
=
’’" #
	pincodeId
’’$ -
,
’’- .
LeadSourceId
÷÷ $
=
÷÷% &
leadSourceId
÷÷' 3
,
÷÷3 4"
ManufacturerDealerId
◊◊ ,
=
◊◊- ."
manufacturerDealerId
◊◊/ C
,
◊◊C D
LeadId
ÿÿ 
=
ÿÿ   
manufacturerLeadId
ÿÿ! 3
}
⁄⁄ 
;
⁄⁄ 
if
‹‹ 
(
‹‹ 
_leadProcessor
‹‹ &
.
‹‹& '"
SaveManufacturerLead
‹‹' ;
(
‹‹; <

leadEntity
‹‹< F
)
‹‹F G
)
‹‹G H
{
›› (
ManufacturerLeadEntityBase
ﬁﬁ 2
lead
ﬁﬁ3 7
=
ﬁﬁ8 9
new
ﬁﬁ: =(
ManufacturerLeadEntityBase
ﬁﬁ> X
(
ﬁﬁX Y
)
ﬁﬁY Z
{
ﬂﬂ 

CampaignId
‡‡ &
=
‡‡' (
(
‡‡) *
uint
‡‡* .
)
‡‡. /

priceQuote
‡‡/ 9
.
‡‡9 :

CampaignId
‡‡: D
,
‡‡D E
CityId
·· "
=
··# $

priceQuote
··% /
.
··/ 0
CityId
··0 6
,
··6 7
CustomerEmail
‚‚ )
=
‚‚* +

priceQuote
‚‚, 6
.
‚‚6 7
CustomerEmail
‚‚7 D
,
‚‚D E
CustomerMobile
„„ *
=
„„+ ,

priceQuote
„„- 7
.
„„7 8
CustomerMobile
„„8 F
,
„„F G
CustomerName
‰‰ (
=
‰‰) *

priceQuote
‰‰+ 5
.
‰‰5 6
CustomerName
‰‰6 B
,
‰‰B C
DealerId
ÂÂ $
=
ÂÂ% &

priceQuote
ÂÂ' 1
.
ÂÂ1 2
DealerId
ÂÂ2 :
,
ÂÂ: ;
InquiryJSON
ÊÊ '
=
ÊÊ( ) 
jsonInquiryDetails
ÊÊ* <
,
ÊÊ< =
LeadId
ÁÁ "
=
ÁÁ# $ 
manufacturerLeadId
ÁÁ% 7
,
ÁÁ7 8
	PinCodeId
ËË %
=
ËË& '
	pincodeId
ËË( 1
,
ËË1 2
PQId
ÈÈ  
=
ÈÈ! "
pqId
ÈÈ# '
,
ÈÈ' (
RetryAttempt
ÍÍ (
=
ÍÍ) *
	iteration
ÍÍ+ 4
,
ÍÍ4 5
	VersionId
ÎÎ %
=
ÎÎ& '

priceQuote
ÎÎ( 2
.
ÎÎ2 3
	VersionId
ÎÎ3 <
,
ÎÎ< ="
ManufacturerDealerId
ÏÏ 0
=
ÏÏ1 2"
manufacturerDealerId
ÏÏ3 G
}
ÌÌ 
;
ÌÌ 
	isSuccess
ÔÔ !
=
ÔÔ" #
_leadProcessor
ÔÔ$ 2
.
ÔÔ2 3%
ProcessManufacturerLead
ÔÔ3 J
(
ÔÔJ K
lead
ÔÔK O
)
ÔÔO P
;
ÔÔP Q
if
 
(
 
	isSuccess
 %
)
% &
Logs
ÒÒ  
.
ÒÒ  !
WriteInfoLog
ÒÒ! -
(
ÒÒ- .
String
ÒÒ. 4
.
ÒÒ4 5
Format
ÒÒ5 ;
(
ÒÒ; <
$str
ÒÒ< d
,
ÒÒd e 
manufacturerLeadId
ÒÒf x
)
ÒÒx y
)
ÒÒy z
;
ÒÒz {
else
ÚÚ 
Logs
ÛÛ  
.
ÛÛ  !
WriteInfoLog
ÛÛ! -
(
ÛÛ- .
String
ÛÛ. 4
.
ÛÛ4 5
Format
ÛÛ5 ;
(
ÛÛ; <
$str
ÛÛ< i
,
ÛÛi j 
manufacturerLeadId
ÛÛk }
)
ÛÛ} ~
)
ÛÛ~ 
;ÛÛ Ä
}
ÙÙ 
else
ıı 
{
ˆˆ 
Logs
˜˜ 
.
˜˜ 
WriteInfoLog
˜˜ )
(
˜˜) *
String
˜˜* 0
.
˜˜0 1
Format
˜˜1 7
(
˜˜7 8
$str
˜˜8 b
,
˜˜b c 
manufacturerLeadId
˜˜d v
)
˜˜v w
)
˜˜w x
;
˜˜x y
}
¯¯ 
}
˘˘ 
}
˙˙ 
catch
˚˚ 
(
˚˚ 
	Exception
˚˚ 
ex
˚˚ 
)
˚˚  
{
¸¸ 
Logs
˝˝ 
.
˝˝ 
WriteInfoLog
˝˝ !
(
˝˝! "
String
˝˝" (
.
˝˝( )
Format
˝˝) /
(
˝˝/ 0
$str
˝˝0 [
,
˝˝[ \ 
manufacturerLeadId
˝˝] o
,
˝˝o p
ex
˝˝q s
.
˝˝s t
Message
˝˝t {
)
˝˝{ |
)
˝˝| }
;
˝˝} ~
}
˛˛ 
return
ˇˇ 
	isSuccess
ˇˇ 
;
ˇˇ 
}
ÄÄ 	
private
ÇÇ 
bool
ÇÇ 
PushDealerLead
ÇÇ #
(
ÇÇ# $(
PriceQuoteParametersEntity
ÇÇ$ >

priceQuote
ÇÇ? I
,
ÇÇI J
uint
ÇÇK O
pqId
ÇÇP T
,
ÇÇT U
ushort
ÇÇV \
	iteration
ÇÇ] f
)
ÇÇf g
{
ÉÉ 	
try
ÑÑ 
{
ÖÖ 
if
ÜÜ 
(
ÜÜ 

priceQuote
ÜÜ 
!=
ÜÜ !
null
ÜÜ" &
)
ÜÜ& '
{
áá 
string
àà  
jsonInquiryDetails
àà -
=
àà. /
String
àà0 6
.
àà6 7
Format
àà7 =
(
àà= >
$stràà> °
,àà° ¢

priceQuoteàà£ ≠
.àà≠ Æ
CustomerNameààÆ ∫
,àà∫ ª

priceQuoteààº ∆
.àà∆ «
CustomerMobileàà« ’
,àà’ ÷

priceQuoteàà◊ ·
.àà· ‚
CustomerEmailàà‚ Ô
,ààÔ 

priceQuoteààÒ ˚
.àà˚ ¸
	VersionIdàà¸ Ö
,ààÖ Ü

priceQuoteààá ë
.ààë í
CityIdààí ò
,ààò ô

priceQuoteààö §
.àà§ •

CampaignIdàà• Ø
)ààØ ∞
;àà∞ ±
Logs
ââ 
.
ââ 
WriteInfoLog
ââ %
(
ââ% &
String
ââ& ,
.
ââ, -
Format
ââ- 3
(
ââ3 4
$str
ââ4 T
,
ââT U

priceQuote
ââV `
.
ââ` a

CampaignId
ââa k
)
ââk l
)
ââl m
;
ââm n
return
ãã 
(
ãã 
_leadProcessor
ãã *
.
ãã* +
PushLeadToAutoBiz
ãã+ <
(
ãã< =
pqId
ãã= A
,
ããA B

priceQuote
ããC M
.
ããM N
DealerId
ããN V
,
ããV W
(
ããX Y
uint
ããY ]
)
ãã] ^

priceQuote
ãã^ h
.
ããh i

CampaignId
ããi s
,
ããs t!
jsonInquiryDetailsããu á
,ããá à
	iterationããâ í
,ããí ì
	LeadTypesããî ù
.ããù û
Dealerããû §
,ãã§ •
$numãã¶ ß
)ããß ®
)ãã® ©
;ãã© ™
}
çç 
}
éé 
catch
èè 
(
èè 
	Exception
èè 
ex
èè 
)
èè  
{
êê 
Logs
ëë 
.
ëë 
WriteInfoLog
ëë !
(
ëë! "
String
ëë" (
.
ëë( )
Format
ëë) /
(
ëë/ 0
$str
ëë0 P
,
ëëP Q
pqId
ëëR V
,
ëëV W
ex
ëëX Z
.
ëëZ [
Message
ëë[ b
)
ëëb c
)
ëëc d
;
ëëd e
}
íí 
return
îî 
false
îî 
;
îî 
}
ïï 	
private
óó 
void
óó (
consumer_ConsumerCancelled
óó /
(
óó/ 0
object
óó0 6
sender
óó7 =
,
óó= >
RabbitMQ
óó? G
.
óóG H
Client
óóH N
.
óóN O
Events
óóO U
.
óóU V
ConsumerEventArgs
óóV g
args
óóh l
)
óól m
{
òò 	
Logs
ôô 
.
ôô 
WriteInfoLog
ôô 
(
ôô 
$str
ôô ?
)
ôô? @
;
ôô@ A
CreateConnection
öö 
.
öö 
CloseConnections
öö -
(
öö- .
)
öö. /
;
öö/ 0
CreateConnection
õõ 
.
õõ 
RefreshNodes
õõ )
(
õõ) *
)
õõ* +
;
õõ+ ,
CreateConnection
úú 
.
úú 
GetNextNode
úú (
(
úú( )
)
úú) *
;
úú* +

_queueName
ùù 
=
ùù 
CreateConnection
ùù )
.
ùù) *
	QueueName
ùù* 3
;
ùù3 4
	_hostName
ûû 
=
ûû 
CreateConnection
ûû (
.
ûû( )
serverIp
ûû) 1
;
ûû1 2
InitConsumer
üü 
(
üü 
)
üü 
;
üü 
ProcessMessages
†† 
(
†† 
)
†† 
;
†† 
}
°° 	
private
®® 
void
®® 
DeadLetterPublish
®® &
(
®®& '!
NameValueCollection
®®' :
nvc
®®; >
,
®®> ?
string
®®@ F
	queueName
®®G P
)
®®P Q
{
©© 	
RabbitMqPublish
´´ 
publish
´´ #
=
´´$ %
new
´´& )
RabbitMqPublish
´´* 9
(
´´9 :
)
´´: ;
;
´´; <
publish
ÆÆ 
.
ÆÆ #
UseDeadLetterExchange
ÆÆ )
=
ÆÆ* +
true
ÆÆ, 0
;
ÆÆ0 1
publish
ØØ 
.
ØØ 

MessageTTL
ØØ 
=
ØØ  
int
ØØ! $
.
ØØ$ %
Parse
ØØ% *
(
ØØ* +
_RabbitMsgTTL
ØØ+ 8
)
ØØ8 9
;
ØØ9 :
int
±± 
	iteration
±± 
=
±± 
nvc
±± 
[
±±  
$str
±±  +
]
±±+ ,
==
±±- /
null
±±0 4
?
±±5 6
$num
±±7 8
:
±±9 :
Int16
±±; @
.
±±@ A
Parse
±±A F
(
±±F G
nvc
±±G J
[
±±J K
$str
±±K V
]
±±V W
)
±±W X
+
±±Y Z
$num
±±[ \
;
±±\ ]
nvc
¥¥ 
.
¥¥ 
Set
¥¥ 
(
¥¥ 
$str
¥¥ 
,
¥¥  
	iteration
¥¥! *
.
¥¥* +
ToString
¥¥+ 3
(
¥¥3 4
)
¥¥4 5
)
¥¥5 6
;
¥¥6 7
publish
∑∑ 
.
∑∑ 
PublishToQueue
∑∑ "
(
∑∑" #
	queueName
∑∑# ,
,
∑∑, -
nvc
∑∑. 1
)
∑∑1 2
;
∑∑2 3
}
∏∏ 	
private
øø !
NameValueCollection
øø #
ByteArrayToObject
øø$ 5
(
øø5 6
byte
øø6 :
[
øø: ;
]
øø; <
	byteArray
øø= F
)
øøF G
{
¿¿ 	
MemoryStream
¬¬ 
memstr
¬¬ 
=
¬¬  !
new
¬¬" %
MemoryStream
¬¬& 2
(
¬¬2 3
	byteArray
¬¬3 <
)
¬¬< =
;
¬¬= >
BinaryFormatter
≈≈ 
bf
≈≈ 
=
≈≈  
new
≈≈! $
BinaryFormatter
≈≈% 4
(
≈≈4 5
)
≈≈5 6
;
≈≈6 7
bf
»» 
.
»» 
AssemblyFormat
»» 
=
»» 
System
»»  &
.
»»& '
Runtime
»»' .
.
»». /
Serialization
»»/ <
.
»»< =

Formatters
»»= G
.
»»G H$
FormatterAssemblyStyle
»»H ^
.
»»^ _
Simple
»»_ e
;
»»e f
memstr
ÀÀ 
.
ÀÀ 
Position
ÀÀ 
=
ÀÀ 
$num
ÀÀ 
;
ÀÀ  !
NameValueCollection
ŒŒ 
obj
ŒŒ  #
=
ŒŒ$ %
(
ŒŒ& '!
NameValueCollection
ŒŒ' :
)
ŒŒ: ;
bf
ŒŒ; =
.
ŒŒ= >
Deserialize
ŒŒ> I
(
ŒŒI J
memstr
ŒŒJ P
)
ŒŒP Q
;
ŒŒQ R
return
–– 
obj
–– 
;
–– 
}
—— 	
}
““ 
internal
‘‘ 
class
‘‘ 
LeadProcessor
‘‘  
{
’’ 
private
÷÷ 
readonly
÷÷ &
LeadProcessingRepository
÷÷ 1
_repository
÷÷2 =
=
÷÷> ?
null
÷÷@ D
;
÷÷D E
private
◊◊ 
readonly
◊◊ 
string
◊◊ 
_hondaGaddiAPIUrl
◊◊  1
,
◊◊1 2!
_bajajFinanceAPIUrl
◊◊3 F
,
◊◊F G 
_tataCapitalAPIUrl
◊◊H Z
;
◊◊Z [
private
ÿÿ 
readonly
ÿÿ 
uint
ÿÿ 
_hondaGaddiId
ÿÿ +
,
ÿÿ+ ,
_bajajFinanceId
ÿÿ- <
,
ÿÿ< =
_RoyalEnfieldId
ÿÿ> M
,
ÿÿM N
_TataCapitalId
ÿÿO ]
;
ÿÿ] ^
private
ŸŸ 
readonly
ŸŸ 
bool
ŸŸ &
_isTataCapitalAPIStarted
ŸŸ 6
=
ŸŸ7 8
false
ŸŸ9 >
;
ŸŸ> ?
private
⁄⁄ 
readonly
⁄⁄ 
IDictionary
⁄⁄ $
<
⁄⁄$ %
uint
⁄⁄% )
,
⁄⁄) *&
IManufacturerLeadHandler
⁄⁄+ C
>
⁄⁄C D
handlers
⁄⁄E M
;
⁄⁄M N
public
‡‡ 
LeadProcessor
‡‡ 
(
‡‡ 
)
‡‡ 
{
·· 	
_repository
‚‚ 
=
‚‚ 
new
‚‚ &
LeadProcessingRepository
‚‚ 6
(
‚‚6 7
)
‚‚7 8
;
‚‚8 9
_hondaGaddiAPIUrl
„„ 
=
„„ "
ConfigurationManager
„„  4
.
„„4 5
AppSettings
„„5 @
[
„„@ A
$str
„„A S
]
„„S T
;
„„T U!
_bajajFinanceAPIUrl
‰‰ 
=
‰‰  !"
ConfigurationManager
‰‰" 6
.
‰‰6 7
AppSettings
‰‰7 B
[
‰‰B C
$str
‰‰C W
]
‰‰W X
;
‰‰X Y 
_tataCapitalAPIUrl
ÂÂ 
=
ÂÂ  "
ConfigurationManager
ÂÂ! 5
.
ÂÂ5 6
AppSettings
ÂÂ6 A
[
ÂÂA B
$str
ÂÂB U
]
ÂÂU V
;
ÂÂV W
UInt32
ÁÁ 
.
ÁÁ 
TryParse
ÁÁ 
(
ÁÁ "
ConfigurationManager
ÁÁ 0
.
ÁÁ0 1
AppSettings
ÁÁ1 <
[
ÁÁ< =
$str
ÁÁ= K
]
ÁÁK L
,
ÁÁL M
out
ÁÁN Q
_hondaGaddiId
ÁÁR _
)
ÁÁ_ `
;
ÁÁ` a
UInt32
ËË 
.
ËË 
TryParse
ËË 
(
ËË "
ConfigurationManager
ËË 0
.
ËË0 1
AppSettings
ËË1 <
[
ËË< =
$str
ËË= M
]
ËËM N
,
ËËN O
out
ËËP S
_bajajFinanceId
ËËT c
)
ËËc d
;
ËËd e
UInt32
ÈÈ 
.
ÈÈ 
TryParse
ÈÈ 
(
ÈÈ "
ConfigurationManager
ÈÈ 0
.
ÈÈ0 1
AppSettings
ÈÈ1 <
[
ÈÈ< =
$str
ÈÈ= M
]
ÈÈM N
,
ÈÈN O
out
ÈÈP S
_RoyalEnfieldId
ÈÈT c
)
ÈÈc d
;
ÈÈd e
UInt32
ÍÍ 
.
ÍÍ 
TryParse
ÍÍ 
(
ÍÍ "
ConfigurationManager
ÍÍ 0
.
ÍÍ0 1
AppSettings
ÍÍ1 <
[
ÍÍ< =
$str
ÍÍ= L
]
ÍÍL M
,
ÍÍM N
out
ÍÍO R
_TataCapitalId
ÍÍS a
)
ÍÍa b
;
ÍÍb c
Boolean
ÎÎ 
.
ÎÎ 
TryParse
ÎÎ 
(
ÎÎ "
ConfigurationManager
ÎÎ 1
.
ÎÎ1 2
AppSettings
ÎÎ2 =
[
ÎÎ= >
$str
ÎÎ> W
]
ÎÎW X
,
ÎÎX Y
out
ÎÎZ ]&
_isTataCapitalAPIStarted
ÎÎ^ v
)
ÎÎv w
;
ÎÎw x
handlers
ÌÌ 
=
ÌÌ 
new
ÌÌ 

Dictionary
ÌÌ %
<
ÌÌ% &
uint
ÌÌ& *
,
ÌÌ* +&
IManufacturerLeadHandler
ÌÌ, D
>
ÌÌD E
(
ÌÌE F
)
ÌÌF G
;
ÌÌG H
handlers
 
.
 
Add
 
(
 
$num
 
,
 
new
 ,
DefaultManufacturerLeadHandler
  >
(
> ?
$num
? @
,
@ A
$str
B D
,
D E
false
F K
)
K L
)
L M
;
M N
handlers
ÚÚ 
.
ÚÚ 
Add
ÚÚ 
(
ÚÚ 
_hondaGaddiId
ÚÚ &
,
ÚÚ& '
new
ÚÚ( +*
HondaManufacturerLeadHandler
ÚÚ, H
(
ÚÚH I
_hondaGaddiId
ÚÚI V
,
ÚÚV W
_hondaGaddiAPIUrl
ÚÚX i
,
ÚÚi j
true
ÚÚk o
)
ÚÚo p
)
ÚÚp q
;
ÚÚq r
handlers
ÛÛ 
.
ÛÛ 
Add
ÛÛ 
(
ÛÛ 
_bajajFinanceId
ÛÛ (
,
ÛÛ( )
new
ÛÛ* -%
BajajFinanceLeadHandler
ÛÛ. E
(
ÛÛE F
_bajajFinanceId
ÛÛF U
,
ÛÛU V!
_bajajFinanceAPIUrl
ÛÛW j
,
ÛÛj k
true
ÛÛl p
)
ÛÛp q
)
ÛÛq r
;
ÛÛr s
handlers
ÙÙ 
.
ÙÙ 
Add
ÙÙ 
(
ÙÙ 
_RoyalEnfieldId
ÙÙ (
,
ÙÙ( )
new
ÙÙ* -%
RoyalEnfieldLeadHandler
ÙÙ. E
(
ÙÙE F
_RoyalEnfieldId
ÙÙF U
,
ÙÙU V
$str
ÙÙW Y
,
ÙÙY Z
true
ÙÙ[ _
,
ÙÙ_ `
false
ÙÙa f
)
ÙÙf g
)
ÙÙg h
;
ÙÙh i
handlers
ıı 
.
ıı 
Add
ıı 
(
ıı 
_TataCapitalId
ıı '
,
ıı' (
new
ıı) ,$
TataCapitalLeadHandler
ıı- C
(
ııC D
_TataCapitalId
ııD R
,
ııR S 
_tataCapitalAPIUrl
ııT f
,
ııf g'
_isTataCapitalAPIStartedııh Ä
)ııÄ Å
)ııÅ Ç
;ııÇ É
}
˜˜ 	
public
ÑÑ 
bool
ÑÑ 
PushLeadToAutoBiz
ÑÑ %
(
ÑÑ% &
uint
ÑÑ& *
pqId
ÑÑ+ /
,
ÑÑ/ 0
uint
ÑÑ1 5
dealerId
ÑÑ6 >
,
ÑÑ> ?
uint
ÑÑ@ D

campaignId
ÑÑE O
,
ÑÑO P
string
ÑÑQ W
inquiryJson
ÑÑX c
,
ÑÑc d
UInt16
ÑÑe k
retryAttempt
ÑÑl x
,
ÑÑx y
	LeadTypesÑÑz É
leadTypeÑÑÑ å
,ÑÑå ç
uintÑÑé í
leadIdÑÑì ô
)ÑÑô ö
{
ÖÖ 	
bool
ÜÜ 
	isSuccess
ÜÜ 
=
ÜÜ 
false
ÜÜ "
;
ÜÜ" #
string
áá 
abInquiryId
áá 
=
áá  
string
áá! '
.
áá' (
Empty
áá( -
;
áá- .
uint
àà 
abInqId
àà 
=
àà 
$num
àà 
;
àà 
try
ââ 
{
ää 
Logs
ãã 
.
ãã 
WriteInfoLog
ãã !
(
ãã! "
String
ãã" (
.
ãã( )
Format
ãã) /
(
ãã/ 0
$str
ãã0 J
,
ããJ K
retryAttempt
ããL X
)
ããX Y
)
ããY Z
;
ããZ [
using
çç 
(
çç 
TCApi_Inquiry
çç $
_inquiryAPI
çç% 0
=
çç1 2
new
çç3 6
TCApi_Inquiry
çç7 D
(
ççD E
)
ççE F
)
ççF G
{
éé 
abInquiryId
èè 
=
èè  !
_inquiryAPI
èè" -
.
èè- .
AddNewCarInquiry
èè. >
(
èè> ?
dealerId
èè? G
.
èèG H
ToString
èèH P
(
èèP Q
)
èèQ R
,
èèR S
inquiryJson
èèT _
)
èè_ `
;
èè` a
}
êê 
Logs
íí 
.
íí 
WriteInfoLog
íí !
(
íí! "
String
íí" (
.
íí( )
Format
íí) /
(
íí/ 0
$str
íí0 M
,
ííM N
abInquiryId
ííO Z
)
ííZ [
)
íí[ \
;
íí\ ]
if
ìì 
(
ìì 
UInt32
ìì 
.
ìì 
TryParse
ìì #
(
ìì# $
abInquiryId
ìì$ /
,
ìì/ 0
out
ìì1 4
abInqId
ìì5 <
)
ìì< =
&&
ìì> @
abInqId
ììA H
>
ììI J
$num
ììK L
)
ììL M
{
îî 
Logs
ïï 
.
ïï 
WriteInfoLog
ïï %
(
ïï% &
$str
ïï& :
)
ïï: ;
;
ïï; <
if
ññ 
(
ññ 

campaignId
ññ "
>
ññ# $
$num
ññ% &
)
ññ& '
{
óó 
	isSuccess
òò !
=
òò" #
_repository
òò$ /
.
òò/ 0+
IsDealerDailyLeadLimitExceeds
òò0 M
(
òòM N

campaignId
òòN X
)
òòX Y
;
òòY Z
	isSuccess
ôô !
=
ôô" #
_repository
ôô$ /
.
ôô/ 0(
UpdateDealerDailyLeadCount
ôô0 J
(
ôôJ K

campaignId
ôôK U
,
ôôU V
abInqId
ôôW ^
)
ôô^ _
;
ôô_ `
	isSuccess
öö !
=
öö" #
_repository
öö$ /
.
öö/ 0

PushedToAB
öö0 :
(
öö: ;
pqId
öö; ?
,
öö? @
abInqId
ööA H
,
ööH I
retryAttempt
ööJ V
)
ööV W
;
ööW X
}
õõ 
Logs
úú 
.
úú 
WriteInfoLog
úú %
(
úú% &
$str
úú& :
)
úú: ;
;
úú; <
}
ùù 
}
ûû 
catch
üü 
(
üü 
	Exception
üü 
ex
üü 
)
üü  
{
†† 
Logs
°° 
.
°° 
WriteErrorLog
°° "
(
°°" #
string
°°# )
.
°°) *
Format
°°* 0
(
°°0 1
$str°°1 å
,°°å ç
inquiryJson°°é ô
,°°ô ö
ex°°õ ù
.°°ù û
Message°°û •
,°°• ¶
pqId°°ß ´
,°°´ ¨
dealerId°°≠ µ
,°°µ ∂

campaignId°°∑ ¡
)°°¡ ¬
)°°¬ √
;°°√ ƒ
}
¢¢ 
return
££ 
	isSuccess
££ 
;
££ 
}
§§ 	
internal
¨¨ 
bool
¨¨ %
ProcessManufacturerLead
¨¨ -
(
¨¨- .(
ManufacturerLeadEntityBase
¨¨. H

leadEntity
¨¨I S
)
¨¨S T
{
≠≠ 	
bool
ÆÆ 
leadProcessed
ÆÆ 
=
ÆÆ  
false
ÆÆ! &
;
ÆÆ& '
try
ØØ 
{
∞∞ 
if
±± 
(
±± 
handlers
±± 
!=
±± 
null
±±  $
&&
±±% '
handlers
±±( 0
.
±±0 1
Count
±±1 6
>
±±7 8
$num
±±9 :
)
±±: ;
{
≤≤ &
IManufacturerLeadHandler
≥≥ ,
handler
≥≥- 4
=
≥≥5 6
null
≥≥7 ;
;
≥≥; <
if
µµ 
(
µµ 
!
µµ 
handlers
µµ !
.
µµ! "
TryGetValue
µµ" -
(
µµ- .

leadEntity
µµ. 8
.
µµ8 9
DealerId
µµ9 A
,
µµA B
out
µµC F
handler
µµG N
)
µµN O
)
µµO P
{
∂∂ 
if
∏∏ 
(
∏∏ 
handler
∏∏ #
==
∏∏$ &
null
∏∏' +
)
∏∏+ ,
{
ππ 
handler
∫∫ #
=
∫∫$ %
handlers
∫∫& .
[
∫∫. /
$num
∫∫/ 0
]
∫∫0 1
;
∫∫1 2
}
ªª 
}
ºº 
if
ææ 
(
ææ 
handler
ææ 
!=
ææ  "
null
ææ# '
)
ææ' (
{
øø 
leadProcessed
¡¡ %
=
¡¡& '
handler
¡¡( /
.
¡¡/ 0
Process
¡¡0 7
(
¡¡7 8

leadEntity
¡¡8 B
)
¡¡B C
;
¡¡C D
if
√√ 
(
√√ 
!
√√ 
leadProcessed
√√ *
)
√√* +
{
ƒƒ 
Logs
≈≈  
.
≈≈  !
WriteInfoLog
≈≈! -
(
≈≈- .
String
≈≈. 4
.
≈≈4 5
Format
≈≈5 ;
(
≈≈; <
$str
≈≈< V
,
≈≈V W

Newtonsoft
≈≈X b
.
≈≈b c
Json
≈≈c g
.
≈≈g h
JsonConvert
≈≈h s
.
≈≈s t
SerializeObject≈≈t É
(≈≈É Ñ

leadEntity≈≈Ñ é
)≈≈é è
)≈≈è ê
)≈≈ê ë
;≈≈ë í
}
∆∆ 
else
«« 
{
»» 
Logs
……  
.
……  !
WriteInfoLog
……! -
(
……- .
String
……. 4
.
……4 5
Format
……5 ;
(
……; <
$str
……< i
,
……i j

leadEntity
……k u
.
……u v
LeadId
……v |
)
……| }
)
……} ~
;
……~ 
}
   
}
ÀÀ 
else
ÃÃ 
{
ÕÕ 
Logs
ŒŒ 
.
ŒŒ 
WriteInfoLog
ŒŒ )
(
ŒŒ) *
String
ŒŒ* 0
.
ŒŒ0 1
Format
ŒŒ1 7
(
ŒŒ7 8
$str
ŒŒ8 Z
,
ŒŒZ [

Newtonsoft
ŒŒ\ f
.
ŒŒf g
Json
ŒŒg k
.
ŒŒk l
JsonConvert
ŒŒl w
.
ŒŒw x
SerializeObjectŒŒx á
(ŒŒá à

leadEntityŒŒà í
)ŒŒí ì
)ŒŒì î
)ŒŒî ï
;ŒŒï ñ
}
œœ 
}
–– 
}
—— 
catch
““ 
(
““ 
	Exception
““ 
ex
““ 
)
““  
{
”” 
Logs
‘‘ 
.
‘‘ 
WriteErrorLog
‘‘ "
(
‘‘" #
String
‘‘# )
.
‘‘) *
Format
‘‘* 0
(
‘‘0 1
$str
‘‘1 g
,
‘‘g h

Newtonsoft
‘‘i s
.
‘‘s t
Json
‘‘t x
.
‘‘x y
JsonConvert‘‘y Ñ
.‘‘Ñ Ö
SerializeObject‘‘Ö î
(‘‘î ï

leadEntity‘‘ï ü
)‘‘ü †
,‘‘† °
ex‘‘¢ §
.‘‘§ •
Message‘‘• ¨
)‘‘¨ ≠
)‘‘≠ Æ
;‘‘Æ Ø
}
’’ 
return
÷÷ 
leadProcessed
÷÷  
;
÷÷  !
}
◊◊ 	
internal
ŸŸ (
PriceQuoteParametersEntity
ŸŸ +"
GetPriceQuoteDetails
ŸŸ, @
(
ŸŸ@ A
uint
ŸŸA E
pqId
ŸŸF J
)
ŸŸJ K
{
⁄⁄ 	
return
€€ 
_repository
€€ 
.
€€ (
FetchPriceQuoteDetailsById
€€ 9
(
€€9 :
pqId
€€: >
)
€€> ?
;
€€? @
}
‹‹ 	
internal
ﬁﬁ 
bool
ﬁﬁ "
SaveManufacturerLead
ﬁﬁ *
(
ﬁﬁ* +$
ManufacturerLeadEntity
ﬁﬁ+ A

leadEntity
ﬁﬁB L
)
ﬁﬁL M
{
ﬂﬂ 	
return
‡‡ 
_repository
‡‡ 
.
‡‡ "
SaveManufacturerLead
‡‡ 3
(
‡‡3 4

leadEntity
‡‡4 >
)
‡‡> ?
;
‡‡? @
}
·· 	
}
„„ 
}‰‰ á
ID:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\LeadTypes.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
public 

enum 
	LeadTypes 
{ 
Dealer 
= 
$num 
, 
Manufacturer 
= 
$num 
, 
Finance 
= 
$num 
}		 
}

 ‘)
VD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\ManufacturerLeadEntity.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
public 

class "
ManufacturerLeadEntity '
{ 
[ 	
JsonProperty	 
( 
$str 
) 
] 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
[ 	
JsonProperty	 
( 
$str 
) 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
JsonProperty	 
( 
$str 
) 
]  
public 
string 
Mobile 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
JsonProperty	 
( 
$str !
)! "
]" #
public 
uint 
	VersionId 
{ 
get  #
;# $
set% (
;( )
}* +
[ 	
JsonProperty	 
( 
$str 
) 
]  
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
[ 	
JsonProperty	 
( 
$str  
)  !
]! "
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
JsonProperty	 
( 
$str 
) 
] 
public   
uint   
PQId   
{   
get   
;   
set    #
;  # $
}  % &
["" 	
JsonProperty""	 
("" 
$str""  
)""  !
]""! "
public## 
string## 
DeviceId## 
{##  
get##! $
;##$ %
set##& )
;##) *
}##+ ,
[%% 	
JsonProperty%%	 
(%% 
$str%% $
)%%$ %
]%%% &
public&& 
uint&& 
LeadSourceId&&  
{&&! "
get&&# &
;&&& '
set&&( +
;&&+ ,
}&&- .
public(( 
uint(( 
	PinCodeId(( 
{(( 
get((  #
;((# $
set((% (
;((( )
}((* +
public** 
uint**  
ManufacturerDealerId** (
{**) *
get**+ .
;**. /
set**0 3
;**3 4
}**5 6
public,, 
uint,, 
LeadId,, 
{,, 
get,,  
;,,  !
set,," %
;,,% &
},,' (
}-- 
public33 

class33 
GaadiLeadEntity33  
{44 
[55 	
JsonProperty55	 
(55 
$str55 
)55 
]55 
public66 
string66 
Name66 
{66 
get66  
;66  !
set66" %
;66% &
}66' (
[77 	
JsonProperty77	 
(77 
$str77 
)77 
]77 
public88 
string88 
Email88 
{88 
get88 !
;88! "
set88# &
;88& '
}88( )
[99 	
JsonProperty99	 
(99 
$str99 
)99 
]99  
public:: 
string:: 
Mobile:: 
{:: 
get:: "
;::" #
set::$ '
;::' (
}::) *
[;; 	
JsonProperty;;	 
(;; 
$str;; 
);; 
];; 
public<< 
string<< 
City<< 
{<< 
get<<  
;<<  !
set<<" %
;<<% &
}<<' (
[== 	
JsonProperty==	 
(== 
$str== 
)== 
]== 
public>> 
string>> 
State>> 
{>> 
get>> !
;>>! "
set>># &
;>>& '
}>>( )
[?? 	
JsonProperty??	 
(?? 
$str?? 
)?? 
]?? 
public@@ 
string@@ 
Make@@ 
{@@ 
get@@  
;@@  !
set@@" %
;@@% &
}@@' (
[AA 	
JsonPropertyAA	 
(AA 
$strAA 
)AA 
]AA 
publicBB 
stringBB 
ModelBB 
{BB 
getBB !
;BB! "
setBB# &
;BB& '
}BB( )
[CC 	
JsonPropertyCC	 
(CC 
$strCC "
)CC" #
]CC# $
publicDD 
stringDD 
SourceDD 
{DD 
getDD "
{DD# $
returnDD% +
$strDD, 6
;DD6 7
}DD8 9
}DD: ;
}EE 
}FF ÜM
YD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\ManufacturerLeadProcessor.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
internal 
abstract 
class #
ManufacturerLeadHandler 3
:4 5$
IManufacturerLeadHandler6 N
{ 
	protected $
LeadProcessingRepository *
LeadRepostiory+ 9
{: ;
get< ?
{@ A
returnB H
_repositoryI T
;T U
}V W
}X Y
	protected 
string 
APIUrl 
{  !
get" %
{& '
return( .
_APIUrl/ 6
;6 7
}8 9
}: ;
	protected 
uint 
ManufacturerId %
{& '
get( +
{, -
return. 4
_manufacturerId5 D
;D E
}F G
}H I
private 
readonly $
LeadProcessingRepository 1
_repository2 =
=> ?
null@ D
;D E
private 
readonly 
string 
_APIUrl  '
=( )
$str* ,
;, -
private 
readonly 
uint 
_manufacturerId -
;- .
private 
readonly 
bool 
_isAPIEnabled +
=, -
false. 3
;3 4
private 
readonly 
bool  
_submitDuplicateLead 2
=3 4
true5 9
;9 :
public #
ManufacturerLeadHandler &
(& '
uint' +
manufacturerId, :
): ;
{ 	
_manufacturerId 
= 
manufacturerId ,
;, -
_repository 
= 
new $
LeadProcessingRepository 6
(6 7
)7 8
;8 9
} 	
public&& #
ManufacturerLeadHandler&& &
(&&& '
uint&&' +
manufacturerId&&, :
,&&: ;
string&&< B
urlAPI&&C I
,&&I J
bool&&K O
isAPIEnabled&&P \
)&&\ ]
:&&^ _
this&&` d
(&&d e
manufacturerId&&e s
)&&s t
{'' 	
_APIUrl(( 
=(( 
urlAPI(( 
;(( 
_isAPIEnabled)) 
=)) 
isAPIEnabled)) (
;))( )
}** 	
public33 #
ManufacturerLeadHandler33 &
(33& '
uint33' +
manufacturerId33, :
,33: ;
string33< B
urlAPI33C I
,33I J
bool33K O
isAPIEnabled33P \
,33\ ]
bool33^ b
submitDuplicateLead33c v
)33v w
:33x y
this33z ~
(33~ 
manufacturerId	33 ç
,
33ç é
urlAPI
33è ï
,
33ï ñ
isAPIEnabled
33ó £
)
33£ §
{44 	 
_submitDuplicateLead55  
=55! "
submitDuplicateLead55# 6
;556 7
}66 	
public>> 
virtual>> 
bool>> 
Process>> #
(>># $&
ManufacturerLeadEntityBase>>$ >

leadEntity>>? I
)>>I J
{?? 	
bool@@ 
	isSuccess@@ 
=@@ 
false@@ "
;@@" #
uintAA 
abInqIdAA 
=AA 
$numAA 
;AA 
tryBB 
{CC 
ifEE 
(EE 
!EE 
_repositoryEE  
.EE  !)
IsManufacturerLeadLimitExceedEE! >
(EE> ?

leadEntityEE? I
.EEI J

CampaignIdEEJ T
)EET U
)EEU V
{FF 
abInqIdHH 
=HH 
PushLeadToAutoBizHH /
(HH/ 0

leadEntityHH0 :
.HH: ;
RetryAttemptHH; G
,HHG H

leadEntityHHI S
.HHS T
DealerIdHHT \
,HH\ ]

leadEntityHH^ h
.HHh i
InquiryJSONHHi t
)HHt u
;HHu v
ifII 
(II 
abInqIdII 
>II  !
$numII" #
)II# $
{JJ 
	isSuccessLL !
=LL" #
_repositoryLL$ /
.LL/ 0,
 UpdateManufacturerDailyLeadCountLL0 P
(LLP Q

leadEntityLLQ [
.LL[ \

CampaignIdLL\ f
,LLf g
abInqIdLLh o
)LLo p
;LLp q
	isSuccessNN !
=NN" #
UpdateABInquiryIdNN$ 5
(NN5 6

leadEntityNN6 @
.NN@ A
LeadIdNNA G
,NNG H
abInqIdNNI P
,NNP Q

leadEntityNNR \
.NN\ ]
RetryAttemptNN] i
)NNi j
;NNj k
ifPP 
(PP 
	isSuccessPP %
&&PP& (
_isAPIEnabledPP) 6
)PP6 7
{QQ 
ifRR 
(RR  
IsDuplicateLeadRR  /
(RR/ 0

leadEntityRR0 :
)RR: ;
)RR; <
{SS 
stringTT  &
responseTT' /
=TT0 1"
PushLeadToManufacturerTT2 H
(TTH I

leadEntityTTI S
)TTS T
;TTT U
ifUU  "
(UU# $
!UU$ %
StringUU% +
.UU+ ,
IsNullOrEmptyUU, 9
(UU9 :
responseUU: B
)UUB C
)UUC D
{VV  !
	isSuccessWW$ -
=WW. /
_repositoryWW0 ;
.WW; <"
UpdateManufacturerLeadWW< R
(WWR S

leadEntityWWS ]
.WW] ^
PQIdWW^ b
,WWb c
responseWWd l
,WWl m

leadEntityWWn x
.WWx y
LeadIdWWy 
)	WW Ä
;
WWÄ Å
}XX  !
}YY 
}ZZ 
}[[ 
}\\ 
else]] 
{^^ 
	isSuccess`` 
=`` 
true``  $
;``$ %
}aa 
}bb 
catchcc 
(cc 
	Exceptioncc 
excc 
)cc  
{dd 
Logsee 
.ee 
WriteErrorLogee "
(ee" #
Stringee# )
.ee) *
Formatee* 0
(ee0 1
$stree1 l
,eel m
exeen p
.eep q
Messageeeq x
,eex y

Newtonsoft	eez Ñ
.
eeÑ Ö
Json
eeÖ â
.
eeâ ä
JsonConvert
eeä ï
.
eeï ñ
SerializeObject
eeñ •
(
ee• ¶

leadEntity
ee¶ ∞
)
ee∞ ±
)
ee± ≤
)
ee≤ ≥
;
ee≥ ¥
}ff 
returngg 
	isSuccessgg 
;gg 
}hh 	
	protectedjj 
virtualjj 
booljj 
IsDuplicateLeadjj .
(jj. /&
ManufacturerLeadEntityBasejj/ I

leadEntityjjJ T
)jjT U
{kk 	
returnll  
_submitDuplicateLeadll '
;ll' (
}mm 	
	protectedoo 
abstractoo 
stringoo !"
PushLeadToManufactureroo" 8
(oo8 9&
ManufacturerLeadEntityBaseoo9 S

leadEntityooT ^
)oo^ _
;oo_ `
privateqq 
UInt32qq 
PushLeadToAutoBizqq (
(qq( )
ushortqq) /
retryAttemptqq0 <
,qq< =
uintqq> B
dealerIdqqC K
,qqK L
stringqqM S
inquiryJSONqqT _
)qq_ `
{rr 	
uintss 
abInqIdss 
=ss 
$numss 
;ss 
stringtt 
abInquiryIdtt 
=tt  
stringtt! '
.tt' (
Emptytt( -
;tt- .
tryvv 
{ww 
usingxx 
(xx 
TCApi_Inquiryxx $
_inquiryAPIxx% 0
=xx1 2
newxx3 6
TCApi_Inquiryxx7 D
(xxD E
)xxE F
)xxF G
{yy 
abInquiryIdzz 
=zz  !
_inquiryAPIzz" -
.zz- .
AddNewCarInquiryzz. >
(zz> ?
dealerIdzz? G
.zzG H
ToStringzzH P
(zzP Q
)zzQ R
,zzR S
inquiryJSONzzT _
)zz_ `
;zz` a
}{{ 
}|| 
catch}} 
(}} 
	Exception}} 
ex}} 
)}}  
{~~ 
Logs 
. 
WriteErrorLog "
(" #
String# )
.) *
Format* 0
(0 1
$str1 s
,s t
exu w
.w x
Messagex 
,	 Ä
retryAttempt
Å ç
,
ç é
dealerId
è ó
,
ó ò
inquiryJSON
ô §
)
§ •
)
• ¶
;
¶ ß
}
ÄÄ 
return
ÅÅ 
(
ÅÅ 
UInt32
ÅÅ 
.
ÅÅ 
TryParse
ÅÅ #
(
ÅÅ# $
abInquiryId
ÅÅ$ /
,
ÅÅ/ 0
out
ÅÅ1 4
abInqId
ÅÅ5 <
)
ÅÅ< =
&&
ÅÅ> @
abInqId
ÅÅA H
>
ÅÅI J
$num
ÅÅK L
)
ÅÅL M
?
ÅÅN O
abInqId
ÅÅP W
:
ÅÅX Y
$num
ÅÅZ [
;
ÅÅ[ \
}
ÉÉ 	
private
ÖÖ 
bool
ÖÖ 
UpdateABInquiryId
ÖÖ &
(
ÖÖ& '
uint
ÖÖ' +
leadId
ÖÖ, 2
,
ÖÖ2 3
uint
ÖÖ4 8
abInqId
ÖÖ9 @
,
ÖÖ@ A
ushort
ÖÖB H
retryAttempt
ÖÖI U
)
ÖÖU V
{
ÜÜ 	
return
áá 
_repository
áá 
.
áá )
UpdateManufacturerABInquiry
áá :
(
áá: ;
leadId
áá; A
,
ááA B
abInqId
ááC J
,
ááJ K
retryAttempt
ááL X
)
ááX Y
;
ááY Z
}
àà 	
}
ââ 
}ää É

VD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\OtherVersionInfoEntity.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
public 

class "
OtherVersionInfoEntity '
{ 
public 
uint 
	VersionId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
VersionName !
{" #
get$ '
;' (
set) ,
;, -
}. /
public		 
ulong		 
OnRoadPrice		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		- .
public

 
UInt32

 
Price

 
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
UInt32 
RTO 
{ 
get 
;  
set! $
;$ %
}& '
public 
UInt32 
	Insurance 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} ‡
ZD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\PriceQuoteParametersEntity.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
public

 

class

 &
PriceQuoteParametersEntity

 +
{ 
public 
uint 
	VersionId 
{ 
get  #
;# $
set% (
;( )
}* +
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
uint 
AreaId 
{ 
get  
;  !
set" %
;% &
}' (
public 
ulong 

CustomerId 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
CustomerName "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
CustomerEmail #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
CustomerMobile $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
string 
ClientIP 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
UInt16 
SourceId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
uint 
ModelId 
{ 
get !
;! "
set# &
;& '
}( )
public 
uint 
ColorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
uint 
? 

CampaignId 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} µ
GD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\Program.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
class 	
Program
 
{ 
static 
void 
Main 
( 
string 
[  
]  !
args" &
)& '
{		 	
log4net

 
.

 
Config

 
.

 
XmlConfigurator

 *
.

* +
	Configure

+ 4
(

4 5
)

5 6
;

6 7
try 
{ 
Logs 
. 
WriteInfoLog !
(! "
$str" 1
+2 3
DateTime4 <
.< =
Now= @
)@ A
;A B
LeadConsumer 
consumer %
=& '
new( +
LeadConsumer, 8
(8 9
)9 :
;: ;
consumer 
. 
ProcessMessages (
(( )
)) *
;* +
} 
catch 
( 
	Exception 
ex 
)  
{ 
Logs 
. 
WriteErrorLog "
(" #
$str# 1
+2 3
ex4 6
.6 7
Message7 >
)> ?
;? @
} 
finally 
{ 
Logs 
. 
WriteInfoLog !
(! "
$str" -
+. /
DateTime0 8
.8 9
Now9 <
)< =
;= >
} 
} 	
} 
} Ç
WD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str C
)C D
]D E
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
$str E
)E F
]F G
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
]$$) *œ
RD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\RoyalEnfieldDealer.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
public 

class 
RoyalEnfieldDealer #
{ 
public 
string 

DealerCode  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 

DealerName  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 

DealerCity  
{! "
get# &
;& '
set( +
;+ ,
}- .
public		 
string		 
DealerState		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
}

 
} ®1
WD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\RoyalEnfieldLeadHandler.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
internal 
class #
RoyalEnfieldLeadHandler *
:+ ,#
ManufacturerLeadHandler- D
{ 
private 
readonly 
string 
_token  &
;& '
public #
RoyalEnfieldLeadHandler &
(& '
uint' +
manufacturerId, :
,: ;
string< B
urlAPIC I
,I J
boolK O
isAPIEnabledP \
,\ ]
bool^ b
submitDuplicateLeadc v
)v w
:x y
basez ~
(~ 
manufacturerId	 ç
,
ç é
urlAPI
è ï
,
ï ñ
isAPIEnabled
ó £
,
£ §!
submitDuplicateLead
• ∏
)
∏ π
{ 	
_token 
=  
ConfigurationManager )
.) *
AppSettings* 5
[5 6
$str6 I
]I J
;J K
} 	
public!! 
override!! 
bool!! 
Process!! $
(!!$ %&
ManufacturerLeadEntityBase!!% ?

leadEntity!!@ J
)!!J K
{"" 	
return## 
base## 
.## 
Process## 
(##  

leadEntity##  *
)##* +
;##+ ,
}$$ 	
	protected,, 
override,, 
bool,, 
IsDuplicateLead,,  /
(,,/ 0&
ManufacturerLeadEntityBase,,0 J

leadEntity,,K U
),,U V
{-- 	
if// 
(// 
!// 
base// 
.// 
LeadRepostiory// $
.//$ %
IsLeadExists//% 1
(//1 2

leadEntity//2 <
.//< =
DealerId//= E
,//E F

leadEntity//G Q
.//Q R
CustomerMobile//R `
)//` a
)//a b
{00 
return11 
true11 
;11 
}22 
else33 
{44 
base66 
.66 
LeadRepostiory66 #
.66# $"
UpdateManufacturerLead66$ :
(66: ;

leadEntity66; E
.66E F
PQId66F J
,66J K
$str66L {
,66{ |

leadEntity	66} á
.
66á à
LeadId
66à é
)
66é è
;
66è ê
return77 
false77 
;77 
}88 
}99 	
	protectedAA 
overrideAA 
stringAA !"
PushLeadToManufacturerAA" 8
(AA8 9&
ManufacturerLeadEntityBaseAA9 S

leadEntityAAT ^
)AA^ _
{BB 	
stringCC 
responseCC 
=CC 
stringCC $
.CC$ %
EmptyCC% *
;CC* +
tryDD 
{EE 
BikeQuotationEntityFF #
	quotationFF$ -
=FF. /
baseFF0 4
.FF4 5
LeadRepostioryFF5 C
.FFC D
GetPriceQuoteByIdFFD U
(FFU V

leadEntityFFV `
.FF` a
PQIdFFa e
)FFe f
;FFf g
RoyalEnfieldDealerGG "
dealerGG# )
=GG* +
baseGG, 0
.GG0 1
LeadRepostioryGG1 ?
.GG? @%
GetRoyalEnfieldDealerByIdGG@ Y
(GGY Z

leadEntityGGZ d
.GGd e 
ManufacturerDealerIdGGe y
)GGy z
;GGz {
LogsHH 
.HH 
WriteInfoLogHH !
(HH! "
StringHH" (
.HH( )
FormatHH) /
(HH/ 0
$strHH0 M
,HHM N

NewtonsoftHHO Y
.HHY Z
JsonHHZ ^
.HH^ _
JsonConvertHH_ j
.HHj k
SerializeObjectHHk z
(HHz {

leadEntity	HH{ Ö
)
HHÖ Ü
)
HHÜ á
)
HHá à
;
HHà â
usingJJ 
(JJ 
RoyalEnfieldWebAPIJJ )
.JJ) *
ServiceJJ* 1
serviceJJ2 9
=JJ: ;
newJJ< ?
RoyalEnfieldWebAPIJJ@ R
.JJR S
ServiceJJS Z
(JJZ [
)JJ[ \
)JJ\ ]
{KK 
responseLL 
=LL 
serviceLL &
.LL& '
OrganicLL' .
(LL. /

leadEntityLL/ 9
.LL9 :
CustomerNameLL: F
,LLF G

leadEntityLLH R
.LLR S
CustomerMobileLLS a
,LLa b
$strLLc j
,LLj k
dealerLLl r
.LLr s
DealerStateLLs ~
,LL~ 
dealerMM  &
.MM& '

DealerCityMM' 1
,MM1 2

leadEntityMM3 =
.MM= >
CustomerEmailMM> K
,MMK L
	quotationMMM V
.MMV W
	ModelNameMMW `
,MM` a
dealerMMb h
.MMh i

DealerNameMMi s
,MMs t
$strMMu w
,MMw x
$strNN  :
,NN: ;
_tokenNN< B
,NNB C
$strNND N
,NNN O
dealerNNP V
.NNV W

DealerCodeNNW a
)NNa b
;NNb c
}OO 
ifQQ 
(QQ 
stringQQ 
.QQ 
IsNullOrEmptyQQ (
(QQ( )
responseQQ) 1
)QQ1 2
)QQ2 3
{RR 
responseSS 
=SS 
$strSS O
;SSO P
}TT 
LogsUU 
.UU 
WriteInfoLogUU !
(UU! "
StringUU" (
.UU( )
FormatUU) /
(UU/ 0
$strUU0 N
,UUN O
responseUUP X
)UUX Y
)UUY Z
;UUZ [
}VV 
catchWW 
(WW 
	ExceptionWW 
exWW 
)WW  
{XX 
LogsYY 
.YY 
WriteErrorLogYY "
(YY" #
StringYY# )
.YY) *
FormatYY* 0
(YY0 1
$strYY1 j
,YYj k

leadEntityYYl v
.YYv w
LeadIdYYw }
,YY} ~
ex	YY Å
.
YYÅ Ç
Message
YYÇ â
)
YYâ ä
)
YYä ã
;
YYã å
}ZZ 
return[[ 
response[[ 
;[[ 
}\\ 	
}^^ 
}__ ô
VD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\TataCapitalInputEntity.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
public 

class "
TataCapitalInputEntity '
{ 
public		 
string		 
source		 
{		 
get		 "
{		# $
return		% +
$str		, 6
;		6 7
}		8 9
}		: ;
public

 
string

 
password

 
{

  
get

! $
{

% &
return

' -
$str

. <
;

< =
}

> ?
}

@ A
public 
string 
fname 
{ 
get !
;! "
set# &
;& '
}( )
=* +
$str, .
;. /
public 
string 
lname 
{ 
get !
;! "
set# &
;& '
}( )
=* +
$str, .
;. /
public 
string 

resEmailId  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
$str1 3
;3 4
public 
string 
resMobNo 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
$str/ 1
;1 2
public 
string 
resCity 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
$str. 0
;0 1
public 
string 
sageProduct !
{" #
get$ '
{( )
return* 0
$str1 ?
;? @
}A B
}C D
public 
string 
sagechannel !
{" #
get$ '
{( )
return* 0
$str1 ;
;; <
}= >
}? @
public 
string 
leadType 
{  
get! $
{% &
return' -
$str. :
;: ;
}< =
}> ?
public 
string 
leadTag 
{ 
get  #
{$ %
return& ,
$str- 7
;7 8
}9 :
}; <
public 
string 
	leadStage 
{  !
get" %
{& '
return( .
$str/ 8
;8 9
}: ;
}< =
} 
} ¬<
VD:\work\bikewaleweb\Bikewale.RabbitMq.LeadProcessingConsumer\TataCapitalLeadHandler.cs
	namespace 	
Bikewale
 
. 
RabbitMq 
. "
LeadProcessingConsumer 2
{ 
internal 
class "
TataCapitalLeadHandler )
:* +#
ManufacturerLeadHandler, C
{ 
public "
TataCapitalLeadHandler %
(% &
uint& *
manufacturerId+ 9
,9 :
string; A
urlAPIB H
,H I
boolJ N
isAPIEnabledO [
)[ \
:] ^
base_ c
(c d
manufacturerIdd r
,r s
urlAPIt z
,z {
isAPIEnabled	| à
)
à â
{ 	
} 	
public!! 
override!! 
bool!! 
Process!! $
(!!$ %&
ManufacturerLeadEntityBase!!% ?

leadEntity!!@ J
)!!J K
{"" 	
return## 
base## 
.## 
Process## 
(##  

leadEntity##  *
)##* +
;##+ ,
}$$ 	
	protected,, 
override,, 
string,, !"
PushLeadToManufacturer,," 8
(,,8 9&
ManufacturerLeadEntityBase,,9 S

leadEntity,,T ^
),,^ _
{-- 	
string.. 
response.. 
=.. 
string.. $
...$ %
Empty..% *
;..* +"
TataCapitalInputEntity// "
tataLeadInput//# 0
=//1 2
null//3 7
;//7 8
try00 
{11 
string55 
fullName55 
=55  !

leadEntity55" ,
.55, -
CustomerName55- 9
.559 :
Trim55: >
(55> ?
)55? @
;55@ A
fullName66 
=66 
Regex66  
.66  !
Replace66! (
(66( )
fullName66) 1
,661 2
$str663 A
,66A B
string66C I
.66I J
Empty66J O
)66O P
;66P Q
string77 
	firstName77  
=77! "
string77# )
.77) *
Empty77* /
,77/ 0
lastName771 9
=77: ;
string77< B
.77B C
Empty77C H
;77H I
if88 
(88 
fullName88 
.88 
Contains88 %
(88% &
$str88& )
)88) *
)88* +
{99 
int:: 

spaceStart:: "
=::# $
fullName::% -
.::- .
IndexOf::. 5
(::5 6
$char::6 9
)::9 :
;::: ;
	firstName;; 
=;; 
fullName;;  (
.;;( )
	Substring;;) 2
(;;2 3
$num;;3 4
,;;4 5

spaceStart;;6 @
);;@ A
;;;A B
lastName<< 
=<< 
fullName<< '
.<<' (
	Substring<<( 1
(<<1 2

spaceStart<<2 <
+<<= >
$num<<? @
)<<@ A
;<<A B
}== 
else>> 
{?? 
	firstName@@ 
=@@ 
fullName@@  (
;@@( )
lastNameAA 
=AA 
fullNameAA '
;AA' (
}BB 
stringGG 
tataCapitalCityIdGG (
=GG) *
baseGG+ /
.GG/ 0
LeadRepostioryGG0 >
.GG> ?"
GetTataCapitalByCityIdGG? U
(GGU V

leadEntityGGV `
.GG` a
CityIdGGa g
)GGg h
;GGh i
tataLeadInputHH 
=HH 
newHH  #"
TataCapitalInputEntityHH$ :
(HH: ;
)HH; <
{II 
fnameJJ 
=JJ 
	firstNameJJ %
.JJ% &
TrimJJ& *
(JJ* +
)JJ+ ,
,JJ, -
lnameKK 
=KK 
lastNameKK $
.KK$ %
TrimKK% )
(KK) *
)KK* +
,KK+ ,

resEmailIdLL 
=LL  

leadEntityLL! +
.LL+ ,
CustomerEmailLL, 9
,LL9 :
resMobNoMM 
=MM 

leadEntityMM )
.MM) *
CustomerMobileMM* 8
,MM8 9
resCityNN 
=NN 
tataCapitalCityIdNN /
}OO 
;OO 
usingTT 
(TT 

HttpClientTT !
_httpClientTT" -
=TT. /
newTT0 3

HttpClientTT4 >
(TT> ?
)TT? @
)TT@ A
{UU 
stringVV 

jsonStringVV %
=VV& '

NewtonsoftVV( 2
.VV2 3
JsonVV3 7
.VV7 8
JsonConvertVV8 C
.VVC D
SerializeObjectVVD S
(VVS T
tataLeadInputVVT a
)VVa b
;VVb c
HttpContentYY 
httpContentYY  +
=YY, -
newYY. 1
StringContentYY2 ?
(YY? @

jsonStringYY@ J
)YYJ K
;YYK L
httpContentZZ 
.ZZ  
HeadersZZ  '
.ZZ' (
ContentTypeZZ( 3
=ZZ4 5
newZZ6 9 
MediaTypeHeaderValueZZ: N
(ZZN O
$strZZO a
)ZZa b
;ZZb c
Logs\\ 
.\\ 
WriteInfoLog\\ %
(\\% &
String\\& ,
.\\, -
Format\\- 3
(\\3 4
$str\\4 P
,\\P Q

jsonString\\R \
)\\\ ]
)\\] ^
;\\^ _
using^^ 
(^^ 
HttpResponseMessage^^ .
	_response^^/ 8
=^^9 :
_httpClient^^; F
.^^F G
	PostAsync^^G P
(^^P Q
base^^Q U
.^^U V
APIUrl^^V \
,^^\ ]
httpContent^^^ i
)^^i j
.^^j k
Result^^k q
)^^q r
{__ 
if`` 
(`` 
	_response`` %
.``% &
IsSuccessStatusCode``& 9
)``9 :
{aa 
ifbb 
(bb  
	_responsebb  )
.bb) *

StatusCodebb* 4
==bb5 7
Systembb8 >
.bb> ?
Netbb? B
.bbB C
HttpStatusCodebbC Q
.bbQ R
OKbbR T
)bbT U
{cc 
responsedd  (
=dd) *
	_responsedd+ 4
.dd4 5
Contentdd5 <
.dd< =
ReadAsStringAsyncdd= N
(ddN O
)ddO P
.ddP Q
ResultddQ W
;ddW X
	_responseee  )
.ee) *
Contentee* 1
.ee1 2
Disposeee2 9
(ee9 :
)ee: ;
;ee; <
	_responseff  )
.ff) *
Contentff* 1
=ff2 3
nullff4 8
;ff8 9
}gg 
}hh 
}ii 
ifjj 
(jj 
Stringjj 
.jj 
IsNullOrEmptyjj ,
(jj, -
responsejj- 5
)jj5 6
)jj6 7
{kk 
responsell  
=ll! "
$strll# M
;llM N
}mm 
Logsnn 
.nn 
WriteInfoLognn %
(nn% &
Stringnn& ,
.nn, -
Formatnn- 3
(nn3 4
$strnn4 Q
,nnQ R
responsennS [
)nn[ \
)nn\ ]
;nn] ^
}oo 
}qq 
catchrr 
(rr 
	Exceptionrr 
exrr 
)rr  
{ss 
Logstt 
.tt 
WriteErrorLogtt "
(tt" #
Stringtt# )
.tt) *
Formattt* 0
(tt0 1
$strtt1 Y
,ttY Z
extt[ ]
.tt] ^
Messagett^ e
)tte f
)ttf g
;ttg h
}uu 
returnvv 
responsevv 
;vv 
}ww 	
}xx 
}yy 