�-
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
}11 �B
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
isAPIEnabled	} �
)
� �
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
	VersionId	//w �
,
//� �

leadEntity
//� �
.
//� �
	PinCodeId
//� �
)
//� �
;
//� �
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
bikeMappingInfo	00u �
.
00� �
City
00� �
)
00� �
)
00� �
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
JsonConvert	eeu �
.
ee� �
SerializeObject
ee� �
(
ee� �
bikeMappingInfo
ee� �
)
ee� �
)
ee� �
)
ee� �
;
ee� �
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
}oo �*
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
}@@ �
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
}(( �	
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
urlAPI	| �
,
� �
isAPIEnabled
� �
)
� �
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
} �2
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
urlAPI	z �
,
� �
isAPIEnabled
� �
)
� �
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
}[[ �
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
} �
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
} ֖
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
Output	CCz �
)
CC� �
)
CC� �
;
CC� �
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
?	GG �
cmd
GG� �
.
GG� �

Parameters
GG� �
[
GG� �
$str
GG� �
]
GG� �
.
GG� �
Value
GG� �
:
GG� �
false
GG� �
)
GG� �
;
GG� �
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
abInquiryId	MMy �
,
MM� �
ex
MM� �
.
MM� �
Message
MM� �
)
MM� �
)
MM� �
;
MM� �
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
�� 
objQuotation
�� 
;
��  
}
�� 	
public
�� 
bool
�� $
UpdateManufacturerLead
�� *
(
��* +
uint
��+ /
pqId
��0 4
,
��4 5
string
��6 <
response
��= E
,
��E F
uint
��G K
leadId
��L R
)
��R S
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
�� 
leadId
�� 
>
�� 
$num
�� 
)
�� 
{
�� 
using
�� 
(
�� 
	DbCommand
�� $
cmd
��% (
=
��) *
	DbFactory
��+ 4
.
��4 5
GetDBCommand
��5 A
(
��A B
)
��B C
)
��C D
{
�� 
cmd
�� 
.
�� 
CommandType
�� '
=
��( )
CommandType
��* 5
.
��5 6
StoredProcedure
��6 E
;
��E F
cmd
�� 
.
�� 
CommandText
�� '
=
��( )
$str
��* K
;
��K L
cmd
�� 
.
�� 

Parameters
�� &
.
��& '
Add
��' *
(
��* +
	DbFactory
��+ 4
.
��4 5

GetDbParam
��5 ?
(
��? @
$str
��@ L
,
��L M
DbType
��N T
.
��T U
Int64
��U Z
,
��Z [
leadId
��\ b
)
��b c
)
��c d
;
��d e
cmd
�� 
.
�� 

Parameters
�� &
.
��& '
Add
��' *
(
��* +
	DbFactory
��+ 4
.
��4 5

GetDbParam
��5 ?
(
��? @
$str
��@ N
,
��N O
DbType
��P V
.
��V W
String
��W ]
,
��] ^
$num
��_ b
,
��b c
response
��d l
)
��l m
)
��m n
;
��n o
if
�� 
(
�� 
MySqlDatabase
�� )
.
��) *
ExecuteNonQuery
��* 9
(
��9 :
cmd
��: =
,
��= >
ConnectionType
��? M
.
��M N
MasterDatabase
��N \
)
��\ ]
>
��^ _
$num
��` a
)
��a b
status
�� "
=
��# $
true
��% )
;
��) *
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
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 c
,
��c d
pqId
��e i
,
��i j
ex
��k m
.
��m n
Message
��n u
)
��u v
)
��v w
;
��w x
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
�� !
BikeQuotationEntity
�� "
GetPriceQuoteById
��# 4
(
��4 5
uint
��5 9
pqId
��: >
)
��> ?
{
�� 	!
BikeQuotationEntity
�� 
objQuotation
��  ,
=
��- .
null
��/ 3
;
��3 4
try
�� 
{
�� 
objQuotation
�� 
=
�� 
new
�� "!
BikeQuotationEntity
��# 6
(
��6 7
)
��7 8
;
��8 9
using
�� 
(
�� 
	DbCommand
��  
cmd
��! $
=
��% &
	DbFactory
��' 0
.
��0 1
GetDBCommand
��1 =
(
��= >
)
��> ?
)
��? @
{
�� 
cmd
�� 
.
�� 
CommandText
�� #
=
��$ %
$str
��& B
;
��B C
cmd
�� 
.
�� 
CommandType
�� #
=
��$ %
CommandType
��& 1
.
��1 2
StoredProcedure
��2 A
;
��A B
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< I
,
��I J
DbType
��K Q
.
��Q R
UInt32
��R X
,
��X Y
pqId
��Z ^
)
��^ _
)
��_ `
;
��` a
using
�� 
(
�� 
IDataReader
�� &
dr
��' )
=
��* +
MySqlDatabase
��, 9
.
��9 :
SelectQuery
��: E
(
��E F
cmd
��F I
,
��I J
ConnectionType
��K Y
.
��Y Z
MasterDatabase
��Z h
)
��h i
)
��i j
{
�� 
if
�� 
(
�� 
dr
�� 
!=
�� !
null
��" &
&&
��' )
dr
��* ,
.
��, -
Read
��- 1
(
��1 2
)
��2 3
)
��3 4
{
�� 
objQuotation
�� (
.
��( )
ExShowroomPrice
��) 8
=
��9 : 
SqlReaderConvertor
��; M
.
��M N
ToUInt64
��N V
(
��V W
dr
��W Y
[
��Y Z
$str
��Z f
]
��f g
)
��g h
;
��h i
objQuotation
�� (
.
��( )
RTO
��) ,
=
��- . 
SqlReaderConvertor
��/ A
.
��A B
ToUInt32
��B J
(
��J K
dr
��K M
[
��M N
$str
��N S
]
��S T
)
��T U
;
��U V
objQuotation
�� (
.
��( )
	Insurance
��) 2
=
��3 4 
SqlReaderConvertor
��5 G
.
��G H
ToUInt32
��H P
(
��P Q
dr
��Q S
[
��S T
$str
��T _
]
��_ `
)
��` a
;
��a b
objQuotation
�� (
.
��( )
OnRoadPrice
��) 4
=
��5 6 
SqlReaderConvertor
��7 I
.
��I J
ToUInt64
��J R
(
��R S
dr
��S U
[
��U V
$str
��V ^
]
��^ _
)
��_ `
;
��` a
objQuotation
�� (
.
��( )
MakeName
��) 1
=
��2 3
Convert
��4 ;
.
��; <
ToString
��< D
(
��D E
dr
��E G
[
��G H
$str
��H N
]
��N O
)
��O P
;
��P Q
objQuotation
�� (
.
��( )
	ModelName
��) 2
=
��3 4
Convert
��5 <
.
��< =
ToString
��= E
(
��E F
dr
��F H
[
��H I
$str
��I P
]
��P Q
)
��Q R
;
��R S
objQuotation
�� (
.
��( )
VersionName
��) 4
=
��5 6
Convert
��7 >
.
��> ?
ToString
��? G
(
��G H
dr
��H J
[
��J K
$str
��K T
]
��T U
)
��U V
;
��V W
objQuotation
�� (
.
��( )
City
��) -
=
��. /
Convert
��0 7
.
��7 8
ToString
��8 @
(
��@ A
dr
��A C
[
��C D
$str
��D N
]
��N O
)
��O P
;
��P Q
objQuotation
�� (
.
��( )
	VersionId
��) 2
=
��3 4 
SqlReaderConvertor
��5 G
.
��G H
ToUInt32
��H P
(
��P Q
dr
��Q S
[
��S T
$str
��T _
]
��_ `
)
��` a
;
��a b
objQuotation
�� (
.
��( )

CampaignId
��) 3
=
��4 5 
SqlReaderConvertor
��6 H
.
��H I
ToUInt32
��I Q
(
��Q R
dr
��R T
[
��T U
$str
��U a
]
��a b
)
��b c
;
��c d
objQuotation
�� (
.
��( )
ManufacturerId
��) 7
=
��8 9 
SqlReaderConvertor
��: L
.
��L M
ToUInt32
��M U
(
��U V
dr
��V X
[
��X Y
$str
��Y i
]
��i j
)
��j k
;
��k l
objQuotation
�� (
.
��( )
State
��) .
=
��/ 0
Convert
��1 8
.
��8 9
ToString
��9 A
(
��A B
dr
��B D
[
��D E
$str
��E P
]
��P Q
)
��Q R
;
��R S
objQuotation
�� (
.
��( )
PriceQuoteId
��) 5
=
��6 7
pqId
��8 <
;
��< =
}
�� 
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
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 ^
,
��^ _
pqId
��` d
,
��d e
ex
��f h
.
��h i
Message
��i p
)
��p q
)
��q r
;
��r s
}
�� 
return
�� 
objQuotation
�� 
;
��  
}
�� 	
public
�� 
bool
�� "
SaveManufacturerLead
�� (
(
��( )$
ManufacturerLeadEntity
��) ?
objLead
��@ G
)
��G H
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
�� 
objLead
�� 
!=
�� 
null
�� #
&&
��$ &
objLead
��' .
.
��. /
PQId
��/ 3
>
��4 5
$num
��6 7
&&
��8 :
objLead
��; B
.
��B C
DealerId
��C K
>
��L M
$num
��N O
)
��O P
{
�� 
using
�� 
(
�� 
	DbCommand
�� $
cmd
��% (
=
��) *
	DbFactory
��+ 4
.
��4 5
GetDBCommand
��5 A
(
��A B
)
��B C
)
��C D
{
�� 
cmd
�� 
.
�� 
CommandType
�� '
=
��( )
CommandType
��* 5
.
��5 6
StoredProcedure
��6 E
;
��E F
cmd
�� 
.
�� 
CommandText
�� '
=
��( )
$str
��* I
;
��I J
cmd
�� 
.
�� 

Parameters
�� &
.
��& '
Add
��' *
(
��* +
	DbFactory
��+ 4
.
��4 5

GetDbParam
��5 ?
(
��? @
$str
��@ J
,
��J K
DbType
��L R
.
��R S
String
��S Y
,
��Y Z
$num
��[ ]
,
��] ^
objLead
��_ f
.
��f g
Name
��g k
)
��k l
)
��l m
;
��m n
cmd
�� 
.
�� 

Parameters
�� &
.
��& '
Add
��' *
(
��* +
	DbFactory
��+ 4
.
��4 5

GetDbParam
��5 ?
(
��? @
$str
��@ K
,
��K L
DbType
��M S
.
��S T
String
��T Z
,
��Z [
$num
��\ _
,
��_ `
objLead
��a h
.
��h i
Email
��i n
)
��n o
)
��o p
;
��p q
cmd
�� 
.
�� 

Parameters
�� &
.
��& '
Add
��' *
(
��* +
	DbFactory
��+ 4
.
��4 5

GetDbParam
��5 ?
(
��? @
$str
��@ L
,
��L M
DbType
��N T
.
��T U
String
��U [
,
��[ \
$num
��] _
,
��_ `
objLead
��a h
.
��h i
Mobile
��i o
)
��o p
)
��p q
;
��q r
cmd
�� 
.
�� 

Parameters
�� &
.
��& '
Add
��' *
(
��* +
	DbFactory
��+ 4
.
��4 5

GetDbParam
��5 ?
(
��? @
$str
��@ J
,
��J K
DbType
��L R
.
��R S
Int64
��S X
,
��X Y
objLead
��Z a
.
��a b
PQId
��b f
)
��f g
)
��g h
;
��h i
cmd
�� 
.
�� 

Parameters
�� &
.
��& '
Add
��' *
(
��* +
	DbFactory
��+ 4
.
��4 5

GetDbParam
��5 ?
(
��? @
$str
��@ R
,
��R S
DbType
��T Z
.
��Z [
Int16
��[ `
,
��` a
objLead
��b i
.
��i j
LeadSourceId
��j v
)
��v w
)
��w x
;
��x y
cmd
�� 
.
�� 

Parameters
�� &
.
��& '
Add
��' *
(
��* +
	DbFactory
��+ 4
.
��4 5

GetDbParam
��5 ?
(
��? @
$str
��@ M
,
��M N
DbType
��O U
.
��U V
String
��V \
,
��\ ]
objLead
��^ e
.
��e f
	PinCodeId
��f o
)
��o p
)
��p q
;
��q r
cmd
�� 
.
�� 

Parameters
�� &
.
��& '
Add
��' *
(
��* +
	DbFactory
��+ 4
.
��4 5

GetDbParam
��5 ?
(
��? @
$str
��@ N
,
��N O
DbType
��P V
.
��V W
Int64
��W \
,
��\ ]
objLead
��^ e
.
��e f
DealerId
��f n
)
��n o
)
��o p
;
��p q
cmd
�� 
.
�� 

Parameters
�� &
.
��& '
Add
��' *
(
��* +
	DbFactory
��+ 4
.
��4 5

GetDbParam
��5 ?
(
��? @
$str
��@ Z
,
��Z [
DbType
��\ b
.
��b c
String
��c i
,
��i j
$num
��k m
,
��m n
objLead
��o v
.
��v w#
ManufacturerDealerId��w �
)��� �
)��� �
;��� �
cmd
�� 
.
�� 

Parameters
�� &
.
��& '
Add
��' *
(
��* +
	DbFactory
��+ 4
.
��4 5

GetDbParam
��5 ?
(
��? @
$str
��@ L
,
��L M
DbType
��N T
.
��T U
Int32
��U Z
,
��Z [
objLead
��\ c
.
��c d
LeadId
��d j
)
��j k
)
��k l
;
��l m
status
�� 
=
��   
SqlReaderConvertor
��! 3
.
��3 4
	ToBoolean
��4 =
(
��= >
MySqlDatabase
��> K
.
��K L
ExecuteNonQuery
��L [
(
��[ \
cmd
��\ _
,
��_ `
ConnectionType
��a o
.
��o p
MasterDatabase
��p ~
)
��~ 
)�� �
;��� �
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
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 a
,
��a b
ex
��c e
.
��e f
Message
��f m
)
��m n
)
��n o
;
��o p
}
�� 
return
�� 
status
�� 
;
�� 
}
�� 	
public
�� $
BajajFinanceLeadEntity
�� %,
GetBajajFinanceBikeMappingInfo
��& D
(
��D E
uint
��E I
	versionid
��J S
,
��S T
uint
��U Y
	pincodeId
��Z c
)
��c d
{
�� 	$
BajajFinanceLeadEntity
�� "
objQuotation
��# /
=
��0 1
null
��2 6
;
��6 7
try
�� 
{
�� 
objQuotation
�� 
=
�� 
new
�� "$
BajajFinanceLeadEntity
��# 9
(
��9 :
)
��: ;
;
��; <
using
�� 
(
�� 
	DbCommand
��  
cmd
��! $
=
��% &
	DbFactory
��' 0
.
��0 1
GetDBCommand
��1 =
(
��= >
)
��> ?
)
��? @
{
�� 
cmd
�� 
.
�� 
CommandText
�� #
=
��$ %
$str
��& B
;
��B C
cmd
�� 
.
�� 
CommandType
�� #
=
��$ %
CommandType
��& 1
.
��1 2
StoredProcedure
��2 A
;
��A B
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< K
,
��K L
DbType
��M S
.
��S T
UInt32
��T Z
,
��Z [
	versionid
��\ e
)
��e f
)
��f g
;
��g h
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< K
,
��K L
DbType
��M S
.
��S T
UInt32
��T Z
,
��Z [
	pincodeId
��\ e
)
��e f
)
��f g
;
��g h
using
�� 
(
�� 
IDataReader
�� &
dr
��' )
=
��* +
MySqlDatabase
��, 9
.
��9 :
SelectQuery
��: E
(
��E F
cmd
��F I
,
��I J
ConnectionType
��K Y
.
��Y Z
MasterDatabase
��Z h
)
��h i
)
��i j
{
�� 
if
�� 
(
�� 
dr
�� 
!=
�� !
null
��" &
&&
��' )
dr
��* ,
.
��, -
Read
��- 1
(
��1 2
)
��2 3
)
��3 4
{
�� 
objQuotation
�� (
.
��( )
ProductMake
��) 4
=
��5 6
Convert
��7 >
.
��> ?
ToString
��? G
(
��G H
dr
��H J
[
��J K
$str
��K X
]
��X Y
)
��Y Z
;
��Z [
objQuotation
�� (
.
��( )
Model
��) .
=
��/ 0
Convert
��1 8
.
��8 9
ToString
��9 A
(
��A B
dr
��B D
[
��D E
$str
��E T
]
��T U
)
��U V
;
��V W
objQuotation
�� (
.
��( )
City
��) -
=
��. /
Convert
��0 7
.
��7 8
ToString
��8 @
(
��@ A
dr
��A C
[
��C D
$str
��D O
]
��O P
)
��P Q
;
��Q R
objQuotation
�� (
.
��( )
State
��) .
=
��/ 0
Convert
��1 8
.
��8 9
ToString
��9 A
(
��A B
dr
��B D
[
��D E
$str
��E T
]
��T U
)
��U V
;
��V W
objQuotation
�� (
.
��( )
PinCode
��) 0
=
��1 2
Convert
��3 :
.
��: ;
ToString
��; C
(
��C D
dr
��D F
[
��F G
$str
��G P
]
��P Q
)
��Q R
;
��R S
}
�� 
}
�� 
}
�� 
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
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 h
,
��h i
ex
��j l
.
��l m
Message
��m t
)
��t u
)
��u v
;
��v w
}
�� 
return
�� 
objQuotation
�� 
;
��  
}
�� 	
public
�� 
bool
�� +
IsDealerDailyLeadLimitExceeds
�� 1
(
��1 2
uint
��2 6

campaignId
��7 A
)
��A B
{
�� 	
bool
�� 
islimitexceeds
�� 
=
��  !
false
��" '
;
��' (
try
�� 
{
�� 
using
�� 
(
�� 
	DbCommand
��  
cmd
��! $
=
��% &
	DbFactory
��' 0
.
��0 1
GetDBCommand
��1 =
(
��= >
)
��> ?
)
��? @
{
�� 
cmd
�� 
.
�� 
CommandText
�� #
=
��$ %
$str
��& E
;
��E F
cmd
�� 
.
�� 
CommandType
�� #
=
��$ %
CommandType
��& 1
.
��1 2
StoredProcedure
��2 A
;
��A B
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< L
,
��L M
DbType
��N T
.
��T U
Int32
��U Z
,
��Z [

campaignId
��\ f
)
��f g
)
��g h
;
��h i
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< P
,
��P Q
DbType
��R X
.
��X Y
Boolean
��Y `
,
��` a 
ParameterDirection
��b t
.
��t u
Output
��u {
)
��{ |
)
��| }
;
��} ~
MySqlDatabase
�� !
.
��! "
ExecuteNonQuery
��" 1
(
��1 2
cmd
��2 5
,
��5 6
ConnectionType
��7 E
.
��E F
MasterDatabase
��F T
)
��T U
;
��U V
islimitexceeds
�� "
=
��# $ 
SqlReaderConvertor
��% 7
.
��7 8
	ToBoolean
��8 A
(
��A B
cmd
��B E
.
��E F

Parameters
��F P
[
��P Q
$str
��Q e
]
��e f
.
��f g
Value
��g l
)
��l m
;
��m n
}
�� 
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
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 j
,
��j k

campaignId
��l v
,
��v w
ex
��x z
.
��z {
Message��{ �
)��� �
)��� �
;��� �
}
�� 
return
�� 
islimitexceeds
�� !
;
��! "
}
�� 	
public
�� 
bool
�� 
IsLeadExists
��  
(
��  !
uint
��! %
dealerId
��& .
,
��. /
string
��0 6
mobile
��7 =
)
��= >
{
�� 	
bool
�� 
IsLeadExists
�� 
=
�� 
false
��  %
;
��% &
try
�� 
{
�� 
using
�� 
(
�� 
	DbCommand
��  
cmd
��! $
=
��% &
	DbFactory
��' 0
.
��0 1
GetDBCommand
��1 =
(
��= >
)
��> ?
)
��? @
{
�� 
cmd
�� 
.
�� 
CommandText
�� #
=
��$ %
$str
��& 9
;
��9 :
cmd
�� 
.
�� 
CommandType
�� #
=
��$ %
CommandType
��& 1
.
��1 2
StoredProcedure
��2 A
;
��A B
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< J
,
��J K
DbType
��L R
.
��R S
Int32
��S X
,
��X Y
dealerId
��Z b
)
��b c
)
��c d
;
��d e
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< H
,
��H I
DbType
��J P
.
��P Q
String
��Q W
,
��W X
mobile
��Y _
)
��_ `
)
��` a
;
��a b
MySqlDatabase
�� !
.
��! "
ExecuteNonQuery
��" 1
(
��1 2
cmd
��2 5
,
��5 6
ConnectionType
��7 E
.
��E F
MasterDatabase
��F T
)
��T U
;
��U V
string
�� !
IsLeadExistsAlready
�� .
=
��/ 0
MySqlDatabase
��1 >
.
��> ?
ExecuteScalar
��? L
(
��L M
cmd
��M P
,
��P Q
ConnectionType
��R `
.
��` a
MasterDatabase
��a o
)
��o p
;
��p q
if
�� 
(
�� 
!
�� 
string
�� 
.
��  
IsNullOrEmpty
��  -
(
��- .!
IsLeadExistsAlready
��. A
)
��A B
)
��B C
IsLeadExists
�� $
=
��% &
true
��' +
;
��+ ,
}
�� 
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
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 }
,
��} ~
dealerId�� �
,��� �
mobile��� �
,��� �
ex��� �
.��� �
Message��� �
)��� �
)��� �
;��� �
}
�� 
return
�� 
IsLeadExists
�� 
;
��  
}
�� 	
public
��  
RoyalEnfieldDealer
�� !'
GetRoyalEnfieldDealerById
��" ;
(
��; <
uint
��< @
re_dealerId
��A L
)
��L M
{
�� 	 
RoyalEnfieldDealer
�� 
objDealerData
�� ,
=
��- .
null
��/ 3
;
��3 4
try
�� 
{
�� 
objDealerData
�� 
=
�� 
new
��  # 
RoyalEnfieldDealer
��$ 6
(
��6 7
)
��7 8
;
��8 9
using
�� 
(
�� 
	DbCommand
��  
cmd
��! $
=
��% &
	DbFactory
��' 0
.
��0 1
GetDBCommand
��1 =
(
��= >
)
��> ?
)
��? @
{
�� 
cmd
�� 
.
�� 
CommandText
�� #
=
��$ %
$str
��& >
;
��> ?
cmd
�� 
.
�� 
CommandType
�� #
=
��$ %
CommandType
��& 1
.
��1 2
StoredProcedure
��2 A
;
��A B
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< J
,
��J K
DbType
��L R
.
��R S
UInt32
��S Y
,
��Y Z
re_dealerId
��[ f
)
��f g
)
��g h
;
��h i
using
�� 
(
�� 
IDataReader
�� &
dr
��' )
=
��* +
MySqlDatabase
��, 9
.
��9 :
SelectQuery
��: E
(
��E F
cmd
��F I
,
��I J
ConnectionType
��K Y
.
��Y Z
MasterDatabase
��Z h
)
��h i
)
��i j
{
�� 
if
�� 
(
�� 
dr
�� 
!=
�� !
null
��" &
&&
��' )
dr
��* ,
.
��, -
Read
��- 1
(
��1 2
)
��2 3
)
��3 4
{
�� 
objDealerData
�� )
.
��) *

DealerCode
��* 4
=
��5 6
Convert
��7 >
.
��> ?
ToString
��? G
(
��G H
dr
��H J
[
��J K
$str
��K W
]
��W X
)
��X Y
;
��Y Z
objDealerData
�� )
.
��) *

DealerName
��* 4
=
��5 6
Convert
��7 >
.
��> ?
ToString
��? G
(
��G H
dr
��H J
[
��J K
$str
��K W
]
��W X
)
��X Y
;
��Y Z
objDealerData
�� )
.
��) *

DealerCity
��* 4
=
��5 6
Convert
��7 >
.
��> ?
ToString
��? G
(
��G H
dr
��H J
[
��J K
$str
��K W
]
��W X
)
��X Y
;
��Y Z
objDealerData
�� )
.
��) *
DealerState
��* 5
=
��6 7
Convert
��8 ?
.
��? @
ToString
��@ H
(
��H I
dr
��I K
[
��K L
$str
��L Y
]
��Y Z
)
��Z [
;
��[ \
}
�� 
}
�� 
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 f
,
��f g
re_dealerId
��h s
,
��s t
ex
��u w
.
��w x
Message
��x 
)�� �
)��� �
;��� �
}
�� 
return
�� 
objDealerData
��  
;
��  !
}
�� 	
public
�� 
string
�� $
GetTataCapitalByCityId
�� ,
(
��, -
uint
��- 1
cityId
��2 8
)
��8 9
{
�� 	
string
�� 
tataCapitalCityId
�� $
=
��% &
string
��' -
.
��- .
Empty
��. 3
;
��3 4
try
�� 
{
�� 
using
�� 
(
�� 
	DbCommand
��  
cmd
��! $
=
��% &
	DbFactory
��' 0
.
��0 1
GetDBCommand
��1 =
(
��= >
)
��> ?
)
��? @
{
�� 
cmd
�� 
.
�� 
CommandText
�� #
=
��$ %
$str
��& ;
;
��; <
cmd
�� 
.
�� 
CommandType
�� #
=
��$ %
CommandType
��& 1
.
��1 2
StoredProcedure
��2 A
;
��A B
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< H
,
��H I
DbType
��J P
.
��P Q
Int32
��Q V
,
��V W
cityId
��X ^
)
��^ _
)
��_ `
;
��` a
using
�� 
(
�� 
IDataReader
�� &
dr
��' )
=
��* +
MySqlDatabase
��, 9
.
��9 :
SelectQuery
��: E
(
��E F
cmd
��F I
,
��I J
ConnectionType
��K Y
.
��Y Z
MasterDatabase
��Z h
)
��h i
)
��i j
{
�� 
if
�� 
(
�� 
dr
�� 
!=
�� !
null
��" &
&&
��' )
dr
��* ,
.
��, -
Read
��- 1
(
��1 2
)
��2 3
)
��3 4
{
�� 
tataCapitalCityId
�� -
=
��. /
Convert
��0 7
.
��7 8
ToString
��8 @
(
��@ A
dr
��A C
[
��C D
$str
��D J
]
��J K
)
��K L
;
��L M
}
�� 
}
�� 
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 c
,
��c d
cityId
��e k
,
��k l
ex
��m o
.
��o p
Message
��p w
)
��w x
)
��x y
;
��y z
}
�� 
return
�� 
tataCapitalCityId
�� $
;
��$ %
}
�� 	
public
�� 
bool
�� +
IsManufacturerLeadLimitExceed
�� 1
(
��1 2
uint
��2 6

campaignId
��7 A
)
��A B
{
�� 	
bool
�� 
islimitexceeds
�� 
=
��  !
false
��" '
;
��' (
try
�� 
{
�� 
using
�� 
(
�� 
	DbCommand
��  
cmd
��! $
=
��% &
	DbFactory
��' 0
.
��0 1
GetDBCommand
��1 =
(
��= >
)
��> ?
)
��? @
{
�� 
cmd
�� 
.
�� 
CommandText
�� #
=
��$ %
$str
��& N
;
��N O
cmd
�� 
.
�� 
CommandType
�� #
=
��$ %
CommandType
��& 1
.
��1 2
StoredProcedure
��2 A
;
��A B
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< L
,
��L M
DbType
��N T
.
��T U
Int32
��U Z
,
��Z [

campaignId
��\ f
)
��f g
)
��g h
;
��h i
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< O
,
��O P
DbType
��Q W
.
��W X
Boolean
��X _
,
��_ ` 
ParameterDirection
��a s
.
��s t
Output
��t z
)
��z {
)
��{ |
;
��| }
MySqlDatabase
�� !
.
��! "
ExecuteNonQuery
��" 1
(
��1 2
cmd
��2 5
,
��5 6
ConnectionType
��7 E
.
��E F
MasterDatabase
��F T
)
��T U
;
��U V
islimitexceeds
�� "
=
��# $ 
SqlReaderConvertor
��% 7
.
��7 8
	ToBoolean
��8 A
(
��A B
cmd
��B E
.
��E F

Parameters
��F P
[
��P Q
$str
��Q d
]
��d e
.
��e f
Value
��f k
)
��k l
;
��l m
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 j
,
��j k

campaignId
��l v
,
��v w
ex
��x z
.
��z {
Message��{ �
)��� �
)��� �
;��� �
}
�� 
return
�� 
islimitexceeds
�� !
;
��! "
}
�� 	
public
�� 
bool
�� .
 UpdateManufacturerDailyLeadCount
�� 4
(
��4 5
uint
��5 9

campaignId
��: D
,
��D E
uint
��F J
abInquiryId
��K V
)
��V W
{
�� 	
bool
�� !
isUpdateDealerCount
�� $
=
��% &
false
��' ,
;
��, -
try
�� 
{
�� 
using
�� 
(
�� 
	DbCommand
��  
cmd
��! $
=
��% &
	DbFactory
��' 0
.
��0 1
GetDBCommand
��1 =
(
��= >
)
��> ?
)
��? @
{
�� 
cmd
�� 
.
�� 
CommandText
�� #
=
��$ %
$str
��& K
;
��K L
cmd
�� 
.
�� 
CommandType
�� #
=
��$ %
CommandType
��& 1
.
��1 2
StoredProcedure
��2 A
;
��A B
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< L
,
��L M
DbType
��N T
.
��T U
Int32
��U Z
,
��Z [

campaignId
��\ f
)
��f g
)
��g h
;
��h i
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< M
,
��M N
DbType
��O U
.
��U V
Int32
��V [
,
��[ \
abInquiryId
��] h
)
��h i
)
��i j
;
��j k
MySqlDatabase
�� !
.
��! "
ExecuteNonQuery
��" 1
(
��1 2
cmd
��2 5
,
��5 6
ConnectionType
��7 E
.
��E F
MasterDatabase
��F T
)
��T U
;
��U V!
isUpdateDealerCount
�� '
=
��( )
true
��* .
;
��. /
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 q
,
��q r

campaignId
��s }
,
��} ~
abInquiryId�� �
,��� �
ex��� �
.��� �
Message��� �
)��� �
)��� �
;��� �
}
�� 
return
�� !
isUpdateDealerCount
�� &
;
��& '
}
�� 	
public
�� 
bool
�� )
UpdateManufacturerABInquiry
�� /
(
��/ 0
uint
��0 4
leadId
��5 ;
,
��; <
uint
��= A
abInquiryId
��B M
,
��M N
UInt16
��O U

retryCount
��V `
)
��` a
{
�� 	
bool
�� !
isUpdateDealerCount
�� $
=
��% &
false
��' ,
;
��, -
try
�� 
{
�� 
using
�� 
(
�� 
	DbCommand
��  
cmd
��! $
=
��% &
	DbFactory
��' 0
.
��0 1
GetDBCommand
��1 =
(
��= >
)
��> ?
)
��? @
{
�� 
cmd
�� 
.
�� 
CommandText
�� #
=
��$ %
$str
��& C
;
��C D
cmd
�� 
.
�� 
CommandType
�� #
=
��$ %
CommandType
��& 1
.
��1 2
StoredProcedure
��2 A
;
��A B
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< D
,
��D E
DbType
��F L
.
��L M
Int32
��M R
,
��R S
leadId
��T Z
)
��Z [
)
��[ \
;
��\ ]
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< M
,
��M N
DbType
��O U
.
��U V
Int32
��V [
,
��[ \
abInquiryId
��] h
)
��h i
)
��i j
;
��j k
cmd
�� 
.
�� 

Parameters
�� "
.
��" #
Add
��# &
(
��& '
	DbFactory
��' 0
.
��0 1

GetDbParam
��1 ;
(
��; <
$str
��< Q
,
��Q R
DbType
��S Y
.
��Y Z
Int16
��Z _
,
��_ `

retryCount
��a k
)
��k l
)
��l m
;
��m n
MySqlDatabase
�� !
.
��! "
ExecuteNonQuery
��" 1
(
��1 2
cmd
��2 5
,
��5 6
ConnectionType
��7 E
.
��E F
MasterDatabase
��F T
)
��T U
;
��U V!
isUpdateDealerCount
�� '
=
��( )
true
��* .
;
��. /
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 l
,
��l m
leadId
��n t
,
��t u
abInquiryId��v �
,��� �
ex��� �
.��� �
Message��� �
)��� �
)��� �
;��� �
}
�� 
return
�� !
isUpdateDealerCount
�� &
;
��& '
}
�� 	
}
�� 
}�� ��
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
_applicationName	AAy �
)
AA� �
)
AA� �
;
AA� �
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
Format	HHz �
(
HH� �
$str
HH� �
,
HH� �
_applicationName
HH� �
)
HH� �
)
HH� �
;
HH� �
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
,	RR �
_applicationName
RR� �
)
RR� �
)
RR� �
;
RR� �
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
Queue	bb~ �
.
bb� �
Dequeue
bb� �
(
bb� �
)
bb� �
;
bb� �
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
manufacturerLeadId	ffo �
;
ff� �
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
,	pp �
dealerId
pp� �
)
pp� �
)
pp� �
;
pp� �
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

Newtonsoft	{ �
.
� �
Json
� �
.
� �
JsonConvert
� �
.
� �
SerializeObject
� �
(
� �
nvc
� �
)
� �
,
� �
nvc
� �
[
� �
$str
� �
]
� �
)
� �
)
� �
;
� �
continue
��$ ,
;
��, -
}
��  !
UInt16
��  &
	iteration
��' 0
=
��1 2
(
��3 4
UInt16
��4 :
)
��: ;
(
��; <
nvc
��< ?
[
��? @
$str
��@ K
]
��K L
==
��M O
null
��P T
?
��U V
$num
��W X
:
��Y Z
(
��[ \
UInt16
��\ b
.
��b c
Parse
��c h
(
��h i
nvc
��i l
[
��l m
$str
��m x
]
��x y
)
��y z
+
��{ |
$num
��} ~
)
��~ 
)�� �
;��� �
bool
��  $
success
��% ,
=
��- .
false
��/ 4
;
��4 5
switch
��  &
(
��' (
leadType
��( 0
)
��0 1
{
��  !
case
��$ (
	LeadTypes
��) 2
.
��2 3
Dealer
��3 9
:
��9 :
success
��( /
=
��0 1
PushDealerLead
��2 @
(
��@ A

priceQuote
��A K
,
��K L
pqId
��M Q
,
��Q R
	iteration
��S \
)
��\ ]
;
��] ^
break
��( -
;
��- .
case
��$ (
	LeadTypes
��) 2
.
��2 3
Manufacturer
��3 ?
:
��? @
success
��( /
=
��0 1"
PushManufacturerLead
��2 F
(
��F G

priceQuote
��G Q
,
��Q R
pqId
��S W
,
��W X
	pincodeId
��Y b
,
��b c
leadSourceId
��d p
,
��p q
	iteration
��r {
,
��{ |#
manufacturerDealerId��} �
,��� �"
manufacturerLeadId��� �
)��� �
;��� �
break
��( -
;
��- .
default
��$ +
:
��+ ,
success
��( /
=
��0 1
PushDealerLead
��2 @
(
��@ A

priceQuote
��A K
,
��K L
pqId
��M Q
,
��Q R
	iteration
��S \
)
��\ ]
;
��] ^
break
��( -
;
��- .
}
��  !
if
��  "
(
��# $
success
��$ +
)
��+ ,
{
��  !
Logs
��$ (
.
��( )
WriteInfoLog
��) 5
(
��5 6
String
��6 <
.
��< =
Format
��= C
(
��C D
$str
��D ~
,
��~ 
pqId��� �
,��� �
dealerId��� �
)��� �
)��� �
;��� �
_model
��$ *
.
��* +
BasicAck
��+ 3
(
��3 4
arg
��4 7
.
��7 8
DeliveryTag
��8 C
,
��C D
false
��E J
)
��J K
;
��K L
}
��  !
else
��  $
{
��  !
Logs
��$ (
.
��( )
WriteInfoLog
��) 5
(
��5 6
String
��6 <
.
��< =
Format
��= C
(
��C D
$str��D �
,��� �
pqId��� �
,��� �
dealerId��� �
)��� �
)��� �
;��� �
DeadLetterPublish
��$ 5
(
��5 6
nvc
��6 9
,
��9 :"
ConfigurationManager
��; O
.
��O P
AppSettings
��P [
[
��[ \
$str
��\ g
]
��g h
.
��h i
ToUpper
��i p
(
��p q
)
��q r
)
��r s
;
��s t
_model
��$ *
.
��* +
BasicReject
��+ 6
(
��6 7
arg
��7 :
.
��: ;
DeliveryTag
��; F
,
��F G
false
��H M
)
��M N
;
��N O
}
��  !
}
�� 
else
��  
{
�� 
_model
��  &
.
��& '
BasicReject
��' 2
(
��2 3
arg
��3 6
.
��6 7
DeliveryTag
��7 B
,
��B C
false
��D I
)
��I J
;
��J K
Logs
��  $
.
��$ %
WriteInfoLog
��% 1
(
��1 2
String
��2 8
.
��8 9
Format
��9 ?
(
��? @
$str
��@ u
,
��u v
pqId
��w {
,
��{ |
dealerId��} �
)��� �
)��� �
;��� �
}
�� 
}
�� 
else
�� 
{
�� 
_model
�� "
.
��" #
BasicReject
��# .
(
��. /
arg
��/ 2
.
��2 3
DeliveryTag
��3 >
,
��> ?
false
��@ E
)
��E F
;
��F G
Logs
��  
.
��  !
WriteInfoLog
��! -
(
��- .
String
��. 4
.
��4 5
Format
��5 ;
(
��; <
$str
��< j
,
��j k
nvc
��l o
[
��o p
$str
��p v
]
��v w
,
��w x
nvc
��y |
[
��| }
$str��} �
]��� �
)��� �
)��� �
;��� �
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� $
ex
��% '
)
��' (
{
�� 
_model
�� 
.
�� 
BasicReject
�� *
(
��* +
arg
��+ .
.
��. /
DeliveryTag
��/ :
,
��: ;
false
��< A
)
��A B
;
��B C
Logs
�� 
.
�� 
WriteInfoLog
�� )
(
��) *
String
��* 0
.
��0 1
Format
��1 7
(
��7 8
$str��8 �
,��� �
nvc��� �
[��� �
$str��� �
]��� �
,��� �
nvc��� �
[��� �
$str��� �
]��� �
,��� �
ex��� �
.��� �
Message��� �
)��� �
)��� �
;��� �
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
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
$str
��# 6
+
��7 8
ex
��9 ;
.
��; <
Message
��< C
)
��C D
;
��D E
SendMail
�� 
.
�� 
HandleException
�� (
(
��( )
ex
��) +
,
��+ ,
String
��- 3
.
��3 4
Format
��4 :
(
��: ;
$str
��; d
,
��d e
_applicationName
��f v
)
��v w
)
��w x
;
��x y
}
�� 
}
�� 	
private
�� 
bool
�� "
PushManufacturerLead
�� )
(
��) *(
PriceQuoteParametersEntity
��* D

priceQuote
��E O
,
��O P
uint
��Q U
pqId
��V Z
,
��Z [
uint
��\ `
	pincodeId
��a j
,
��j k
uint
��l p
leadSourceId
��q }
,
��} ~
ushort�� �
	iteration��� �
,��� �
uint��� �$
manufacturerDealerId��� �
,��� �
uint��� �"
manufacturerLeadId��� �
)��� �
{
�� 	
bool
�� 
	isSuccess
�� 
=
�� 
false
�� "
;
��" #
try
�� 
{
�� 
if
�� 
(
�� 

priceQuote
�� 
!=
�� !
null
��" &
)
��& '
{
�� 
Logs
�� 
.
�� 
WriteInfoLog
�� %
(
��% &
String
��& ,
.
��, -
Format
��- 3
(
��3 4
$str
��4 [
)
��[ \
)
��\ ]
;
��] ^
string
��  
jsonInquiryDetails
�� -
=
��. /
String
��0 6
.
��6 7
Format
��7 =
(
��= >
$str��> �
,��� �

priceQuote��� �
.��� �
CustomerName��� �
,��� �

priceQuote��� �
.��� �
CustomerMobile��� �
,��� �

priceQuote��� �
.��� �
CustomerEmail��� �
,��� �

priceQuote��� �
.��� �
	VersionId��� �
,��� �

priceQuote��� �
.��� �
CityId��� �
,��� �

priceQuote��� �
.��� �

CampaignId��� �
)��� �
;��� �$
ManufacturerLeadEntity
�� *

leadEntity
��+ 5
=
��6 7
new
��8 ;$
ManufacturerLeadEntity
��< R
(
��R S
)
��S T
{
�� 
PQId
�� 
=
�� 
pqId
�� #
,
��# $
Mobile
�� 
=
��  

priceQuote
��! +
.
��+ ,
CustomerMobile
��, :
,
��: ;
Email
�� 
=
�� 

priceQuote
��  *
.
��* +
CustomerEmail
��+ 8
,
��8 9
Name
�� 
=
�� 

priceQuote
�� )
.
��) *
CustomerName
��* 6
,
��6 7
DealerId
��  
=
��! "

priceQuote
��# -
.
��- .
DealerId
��. 6
,
��6 7
	PinCodeId
�� !
=
��" #
	pincodeId
��$ -
,
��- .
LeadSourceId
�� $
=
��% &
leadSourceId
��' 3
,
��3 4"
ManufacturerDealerId
�� ,
=
��- ."
manufacturerDealerId
��/ C
,
��C D
LeadId
�� 
=
��   
manufacturerLeadId
��! 3
}
�� 
;
�� 
if
�� 
(
�� 
_leadProcessor
�� &
.
��& '"
SaveManufacturerLead
��' ;
(
��; <

leadEntity
��< F
)
��F G
)
��G H
{
�� (
ManufacturerLeadEntityBase
�� 2
lead
��3 7
=
��8 9
new
��: =(
ManufacturerLeadEntityBase
��> X
(
��X Y
)
��Y Z
{
�� 

CampaignId
�� &
=
��' (
(
��) *
uint
��* .
)
��. /

priceQuote
��/ 9
.
��9 :

CampaignId
��: D
,
��D E
CityId
�� "
=
��# $

priceQuote
��% /
.
��/ 0
CityId
��0 6
,
��6 7
CustomerEmail
�� )
=
��* +

priceQuote
��, 6
.
��6 7
CustomerEmail
��7 D
,
��D E
CustomerMobile
�� *
=
��+ ,

priceQuote
��- 7
.
��7 8
CustomerMobile
��8 F
,
��F G
CustomerName
�� (
=
��) *

priceQuote
��+ 5
.
��5 6
CustomerName
��6 B
,
��B C
DealerId
�� $
=
��% &

priceQuote
��' 1
.
��1 2
DealerId
��2 :
,
��: ;
InquiryJSON
�� '
=
��( ) 
jsonInquiryDetails
��* <
,
��< =
LeadId
�� "
=
��# $ 
manufacturerLeadId
��% 7
,
��7 8
	PinCodeId
�� %
=
��& '
	pincodeId
��( 1
,
��1 2
PQId
��  
=
��! "
pqId
��# '
,
��' (
RetryAttempt
�� (
=
��) *
	iteration
��+ 4
,
��4 5
	VersionId
�� %
=
��& '

priceQuote
��( 2
.
��2 3
	VersionId
��3 <
,
��< ="
ManufacturerDealerId
�� 0
=
��1 2"
manufacturerDealerId
��3 G
}
�� 
;
�� 
	isSuccess
�� !
=
��" #
_leadProcessor
��$ 2
.
��2 3%
ProcessManufacturerLead
��3 J
(
��J K
lead
��K O
)
��O P
;
��P Q
if
�� 
(
�� 
	isSuccess
�� %
)
��% &
Logs
��  
.
��  !
WriteInfoLog
��! -
(
��- .
String
��. 4
.
��4 5
Format
��5 ;
(
��; <
$str
��< d
,
��d e 
manufacturerLeadId
��f x
)
��x y
)
��y z
;
��z {
else
�� 
Logs
��  
.
��  !
WriteInfoLog
��! -
(
��- .
String
��. 4
.
��4 5
Format
��5 ;
(
��; <
$str
��< i
,
��i j 
manufacturerLeadId
��k }
)
��} ~
)
��~ 
;�� �
}
�� 
else
�� 
{
�� 
Logs
�� 
.
�� 
WriteInfoLog
�� )
(
��) *
String
��* 0
.
��0 1
Format
��1 7
(
��7 8
$str
��8 b
,
��b c 
manufacturerLeadId
��d v
)
��v w
)
��w x
;
��x y
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
�� 
Logs
�� 
.
�� 
WriteInfoLog
�� !
(
��! "
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str
��0 [
,
��[ \ 
manufacturerLeadId
��] o
,
��o p
ex
��q s
.
��s t
Message
��t {
)
��{ |
)
��| }
;
��} ~
}
�� 
return
�� 
	isSuccess
�� 
;
�� 
}
�� 	
private
�� 
bool
�� 
PushDealerLead
�� #
(
��# $(
PriceQuoteParametersEntity
��$ >

priceQuote
��? I
,
��I J
uint
��K O
pqId
��P T
,
��T U
ushort
��V \
	iteration
��] f
)
��f g
{
�� 	
try
�� 
{
�� 
if
�� 
(
�� 

priceQuote
�� 
!=
�� !
null
��" &
)
��& '
{
�� 
string
��  
jsonInquiryDetails
�� -
=
��. /
String
��0 6
.
��6 7
Format
��7 =
(
��= >
$str��> �
,��� �

priceQuote��� �
.��� �
CustomerName��� �
,��� �

priceQuote��� �
.��� �
CustomerMobile��� �
,��� �

priceQuote��� �
.��� �
CustomerEmail��� �
,��� �

priceQuote��� �
.��� �
	VersionId��� �
,��� �

priceQuote��� �
.��� �
CityId��� �
,��� �

priceQuote��� �
.��� �

CampaignId��� �
)��� �
;��� �
Logs
�� 
.
�� 
WriteInfoLog
�� %
(
��% &
String
��& ,
.
��, -
Format
��- 3
(
��3 4
$str
��4 T
,
��T U

priceQuote
��V `
.
��` a

CampaignId
��a k
)
��k l
)
��l m
;
��m n
return
�� 
(
�� 
_leadProcessor
�� *
.
��* +
PushLeadToAutoBiz
��+ <
(
��< =
pqId
��= A
,
��A B

priceQuote
��C M
.
��M N
DealerId
��N V
,
��V W
(
��X Y
uint
��Y ]
)
��] ^

priceQuote
��^ h
.
��h i

CampaignId
��i s
,
��s t!
jsonInquiryDetails��u �
,��� �
	iteration��� �
,��� �
	LeadTypes��� �
.��� �
Dealer��� �
,��� �
$num��� �
)��� �
)��� �
;��� �
}
�� 
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
�� 
Logs
�� 
.
�� 
WriteInfoLog
�� !
(
��! "
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str
��0 P
,
��P Q
pqId
��R V
,
��V W
ex
��X Z
.
��Z [
Message
��[ b
)
��b c
)
��c d
;
��d e
}
�� 
return
�� 
false
�� 
;
�� 
}
�� 	
private
�� 
void
�� (
consumer_ConsumerCancelled
�� /
(
��/ 0
object
��0 6
sender
��7 =
,
��= >
RabbitMQ
��? G
.
��G H
Client
��H N
.
��N O
Events
��O U
.
��U V
ConsumerEventArgs
��V g
args
��h l
)
��l m
{
�� 	
Logs
�� 
.
�� 
WriteInfoLog
�� 
(
�� 
$str
�� ?
)
��? @
;
��@ A
CreateConnection
�� 
.
�� 
CloseConnections
�� -
(
��- .
)
��. /
;
��/ 0
CreateConnection
�� 
.
�� 
RefreshNodes
�� )
(
��) *
)
��* +
;
��+ ,
CreateConnection
�� 
.
�� 
GetNextNode
�� (
(
��( )
)
��) *
;
��* +

_queueName
�� 
=
�� 
CreateConnection
�� )
.
��) *
	QueueName
��* 3
;
��3 4
	_hostName
�� 
=
�� 
CreateConnection
�� (
.
��( )
serverIp
��) 1
;
��1 2
InitConsumer
�� 
(
�� 
)
�� 
;
�� 
ProcessMessages
�� 
(
�� 
)
�� 
;
�� 
}
�� 	
private
�� 
void
�� 
DeadLetterPublish
�� &
(
��& '!
NameValueCollection
��' :
nvc
��; >
,
��> ?
string
��@ F
	queueName
��G P
)
��P Q
{
�� 	
RabbitMqPublish
�� 
publish
�� #
=
��$ %
new
��& )
RabbitMqPublish
��* 9
(
��9 :
)
��: ;
;
��; <
publish
�� 
.
�� #
UseDeadLetterExchange
�� )
=
��* +
true
��, 0
;
��0 1
publish
�� 
.
�� 

MessageTTL
�� 
=
��  
int
��! $
.
��$ %
Parse
��% *
(
��* +
_RabbitMsgTTL
��+ 8
)
��8 9
;
��9 :
int
�� 
	iteration
�� 
=
�� 
nvc
�� 
[
��  
$str
��  +
]
��+ ,
==
��- /
null
��0 4
?
��5 6
$num
��7 8
:
��9 :
Int16
��; @
.
��@ A
Parse
��A F
(
��F G
nvc
��G J
[
��J K
$str
��K V
]
��V W
)
��W X
+
��Y Z
$num
��[ \
;
��\ ]
nvc
�� 
.
�� 
Set
�� 
(
�� 
$str
�� 
,
��  
	iteration
��! *
.
��* +
ToString
��+ 3
(
��3 4
)
��4 5
)
��5 6
;
��6 7
publish
�� 
.
�� 
PublishToQueue
�� "
(
��" #
	queueName
��# ,
,
��, -
nvc
��. 1
)
��1 2
;
��2 3
}
�� 	
private
�� !
NameValueCollection
�� #
ByteArrayToObject
��$ 5
(
��5 6
byte
��6 :
[
��: ;
]
��; <
	byteArray
��= F
)
��F G
{
�� 	
MemoryStream
�� 
memstr
�� 
=
��  !
new
��" %
MemoryStream
��& 2
(
��2 3
	byteArray
��3 <
)
��< =
;
��= >
BinaryFormatter
�� 
bf
�� 
=
��  
new
��! $
BinaryFormatter
��% 4
(
��4 5
)
��5 6
;
��6 7
bf
�� 
.
�� 
AssemblyFormat
�� 
=
�� 
System
��  &
.
��& '
Runtime
��' .
.
��. /
Serialization
��/ <
.
��< =

Formatters
��= G
.
��G H$
FormatterAssemblyStyle
��H ^
.
��^ _
Simple
��_ e
;
��e f
memstr
�� 
.
�� 
Position
�� 
=
�� 
$num
�� 
;
��  !
NameValueCollection
�� 
obj
��  #
=
��$ %
(
��& '!
NameValueCollection
��' :
)
��: ;
bf
��; =
.
��= >
Deserialize
��> I
(
��I J
memstr
��J P
)
��P Q
;
��Q R
return
�� 
obj
�� 
;
�� 
}
�� 	
}
�� 
internal
�� 
class
�� 
LeadProcessor
��  
{
�� 
private
�� 
readonly
�� &
LeadProcessingRepository
�� 1
_repository
��2 =
=
��> ?
null
��@ D
;
��D E
private
�� 
readonly
�� 
string
�� 
_hondaGaddiAPIUrl
��  1
,
��1 2!
_bajajFinanceAPIUrl
��3 F
,
��F G 
_tataCapitalAPIUrl
��H Z
;
��Z [
private
�� 
readonly
�� 
uint
�� 
_hondaGaddiId
�� +
,
��+ ,
_bajajFinanceId
��- <
,
��< =
_RoyalEnfieldId
��> M
,
��M N
_TataCapitalId
��O ]
;
��] ^
private
�� 
readonly
�� 
bool
�� &
_isTataCapitalAPIStarted
�� 6
=
��7 8
false
��9 >
;
��> ?
private
�� 
readonly
�� 
IDictionary
�� $
<
��$ %
uint
��% )
,
��) *&
IManufacturerLeadHandler
��+ C
>
��C D
handlers
��E M
;
��M N
public
�� 
LeadProcessor
�� 
(
�� 
)
�� 
{
�� 	
_repository
�� 
=
�� 
new
�� &
LeadProcessingRepository
�� 6
(
��6 7
)
��7 8
;
��8 9
_hondaGaddiAPIUrl
�� 
=
�� "
ConfigurationManager
��  4
.
��4 5
AppSettings
��5 @
[
��@ A
$str
��A S
]
��S T
;
��T U!
_bajajFinanceAPIUrl
�� 
=
��  !"
ConfigurationManager
��" 6
.
��6 7
AppSettings
��7 B
[
��B C
$str
��C W
]
��W X
;
��X Y 
_tataCapitalAPIUrl
�� 
=
��  "
ConfigurationManager
��! 5
.
��5 6
AppSettings
��6 A
[
��A B
$str
��B U
]
��U V
;
��V W
UInt32
�� 
.
�� 
TryParse
�� 
(
�� "
ConfigurationManager
�� 0
.
��0 1
AppSettings
��1 <
[
��< =
$str
��= K
]
��K L
,
��L M
out
��N Q
_hondaGaddiId
��R _
)
��_ `
;
��` a
UInt32
�� 
.
�� 
TryParse
�� 
(
�� "
ConfigurationManager
�� 0
.
��0 1
AppSettings
��1 <
[
��< =
$str
��= M
]
��M N
,
��N O
out
��P S
_bajajFinanceId
��T c
)
��c d
;
��d e
UInt32
�� 
.
�� 
TryParse
�� 
(
�� "
ConfigurationManager
�� 0
.
��0 1
AppSettings
��1 <
[
��< =
$str
��= M
]
��M N
,
��N O
out
��P S
_RoyalEnfieldId
��T c
)
��c d
;
��d e
UInt32
�� 
.
�� 
TryParse
�� 
(
�� "
ConfigurationManager
�� 0
.
��0 1
AppSettings
��1 <
[
��< =
$str
��= L
]
��L M
,
��M N
out
��O R
_TataCapitalId
��S a
)
��a b
;
��b c
Boolean
�� 
.
�� 
TryParse
�� 
(
�� "
ConfigurationManager
�� 1
.
��1 2
AppSettings
��2 =
[
��= >
$str
��> W
]
��W X
,
��X Y
out
��Z ]&
_isTataCapitalAPIStarted
��^ v
)
��v w
;
��w x
handlers
�� 
=
�� 
new
�� 

Dictionary
�� %
<
��% &
uint
��& *
,
��* +&
IManufacturerLeadHandler
��, D
>
��D E
(
��E F
)
��F G
;
��G H
handlers
�� 
.
�� 
Add
�� 
(
�� 
$num
�� 
,
�� 
new
�� ,
DefaultManufacturerLeadHandler
��  >
(
��> ?
$num
��? @
,
��@ A
$str
��B D
,
��D E
false
��F K
)
��K L
)
��L M
;
��M N
handlers
�� 
.
�� 
Add
�� 
(
�� 
_hondaGaddiId
�� &
,
��& '
new
��( +*
HondaManufacturerLeadHandler
��, H
(
��H I
_hondaGaddiId
��I V
,
��V W
_hondaGaddiAPIUrl
��X i
,
��i j
true
��k o
)
��o p
)
��p q
;
��q r
handlers
�� 
.
�� 
Add
�� 
(
�� 
_bajajFinanceId
�� (
,
��( )
new
��* -%
BajajFinanceLeadHandler
��. E
(
��E F
_bajajFinanceId
��F U
,
��U V!
_bajajFinanceAPIUrl
��W j
,
��j k
true
��l p
)
��p q
)
��q r
;
��r s
handlers
�� 
.
�� 
Add
�� 
(
�� 
_RoyalEnfieldId
�� (
,
��( )
new
��* -%
RoyalEnfieldLeadHandler
��. E
(
��E F
_RoyalEnfieldId
��F U
,
��U V
$str
��W Y
,
��Y Z
true
��[ _
,
��_ `
false
��a f
)
��f g
)
��g h
;
��h i
handlers
�� 
.
�� 
Add
�� 
(
�� 
_TataCapitalId
�� '
,
��' (
new
��) ,$
TataCapitalLeadHandler
��- C
(
��C D
_TataCapitalId
��D R
,
��R S 
_tataCapitalAPIUrl
��T f
,
��f g'
_isTataCapitalAPIStarted��h �
)��� �
)��� �
;��� �
}
�� 	
public
�� 
bool
�� 
PushLeadToAutoBiz
�� %
(
��% &
uint
��& *
pqId
��+ /
,
��/ 0
uint
��1 5
dealerId
��6 >
,
��> ?
uint
��@ D

campaignId
��E O
,
��O P
string
��Q W
inquiryJson
��X c
,
��c d
UInt16
��e k
retryAttempt
��l x
,
��x y
	LeadTypes��z �
leadType��� �
,��� �
uint��� �
leadId��� �
)��� �
{
�� 	
bool
�� 
	isSuccess
�� 
=
�� 
false
�� "
;
��" #
string
�� 
abInquiryId
�� 
=
��  
string
��! '
.
��' (
Empty
��( -
;
��- .
uint
�� 
abInqId
�� 
=
�� 
$num
�� 
;
�� 
try
�� 
{
�� 
Logs
�� 
.
�� 
WriteInfoLog
�� !
(
��! "
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str
��0 J
,
��J K
retryAttempt
��L X
)
��X Y
)
��Y Z
;
��Z [
using
�� 
(
�� 
TCApi_Inquiry
�� $
_inquiryAPI
��% 0
=
��1 2
new
��3 6
TCApi_Inquiry
��7 D
(
��D E
)
��E F
)
��F G
{
�� 
abInquiryId
�� 
=
��  !
_inquiryAPI
��" -
.
��- .
AddNewCarInquiry
��. >
(
��> ?
dealerId
��? G
.
��G H
ToString
��H P
(
��P Q
)
��Q R
,
��R S
inquiryJson
��T _
)
��_ `
;
��` a
}
�� 
Logs
�� 
.
�� 
WriteInfoLog
�� !
(
��! "
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str
��0 M
,
��M N
abInquiryId
��O Z
)
��Z [
)
��[ \
;
��\ ]
if
�� 
(
�� 
UInt32
�� 
.
�� 
TryParse
�� #
(
��# $
abInquiryId
��$ /
,
��/ 0
out
��1 4
abInqId
��5 <
)
��< =
&&
��> @
abInqId
��A H
>
��I J
$num
��K L
)
��L M
{
�� 
Logs
�� 
.
�� 
WriteInfoLog
�� %
(
��% &
$str
��& :
)
��: ;
;
��; <
if
�� 
(
�� 

campaignId
�� "
>
��# $
$num
��% &
)
��& '
{
�� 
	isSuccess
�� !
=
��" #
_repository
��$ /
.
��/ 0+
IsDealerDailyLeadLimitExceeds
��0 M
(
��M N

campaignId
��N X
)
��X Y
;
��Y Z
	isSuccess
�� !
=
��" #
_repository
��$ /
.
��/ 0(
UpdateDealerDailyLeadCount
��0 J
(
��J K

campaignId
��K U
,
��U V
abInqId
��W ^
)
��^ _
;
��_ `
	isSuccess
�� !
=
��" #
_repository
��$ /
.
��/ 0

PushedToAB
��0 :
(
��: ;
pqId
��; ?
,
��? @
abInqId
��A H
,
��H I
retryAttempt
��J V
)
��V W
;
��W X
}
�� 
Logs
�� 
.
�� 
WriteInfoLog
�� %
(
��% &
$str
��& :
)
��: ;
;
��; <
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
string
��# )
.
��) *
Format
��* 0
(
��0 1
$str��1 �
,��� �
inquiryJson��� �
,��� �
ex��� �
.��� �
Message��� �
,��� �
pqId��� �
,��� �
dealerId��� �
,��� �

campaignId��� �
)��� �
)��� �
;��� �
}
�� 
return
�� 
	isSuccess
�� 
;
�� 
}
�� 	
internal
�� 
bool
�� %
ProcessManufacturerLead
�� -
(
��- .(
ManufacturerLeadEntityBase
��. H

leadEntity
��I S
)
��S T
{
�� 	
bool
�� 
leadProcessed
�� 
=
��  
false
��! &
;
��& '
try
�� 
{
�� 
if
�� 
(
�� 
handlers
�� 
!=
�� 
null
��  $
&&
��% '
handlers
��( 0
.
��0 1
Count
��1 6
>
��7 8
$num
��9 :
)
��: ;
{
�� &
IManufacturerLeadHandler
�� ,
handler
��- 4
=
��5 6
null
��7 ;
;
��; <
if
�� 
(
�� 
!
�� 
handlers
�� !
.
��! "
TryGetValue
��" -
(
��- .

leadEntity
��. 8
.
��8 9
DealerId
��9 A
,
��A B
out
��C F
handler
��G N
)
��N O
)
��O P
{
�� 
if
�� 
(
�� 
handler
�� #
==
��$ &
null
��' +
)
��+ ,
{
�� 
handler
�� #
=
��$ %
handlers
��& .
[
��. /
$num
��/ 0
]
��0 1
;
��1 2
}
�� 
}
�� 
if
�� 
(
�� 
handler
�� 
!=
��  "
null
��# '
)
��' (
{
�� 
leadProcessed
�� %
=
��& '
handler
��( /
.
��/ 0
Process
��0 7
(
��7 8

leadEntity
��8 B
)
��B C
;
��C D
if
�� 
(
�� 
!
�� 
leadProcessed
�� *
)
��* +
{
�� 
Logs
��  
.
��  !
WriteInfoLog
��! -
(
��- .
String
��. 4
.
��4 5
Format
��5 ;
(
��; <
$str
��< V
,
��V W

Newtonsoft
��X b
.
��b c
Json
��c g
.
��g h
JsonConvert
��h s
.
��s t
SerializeObject��t �
(��� �

leadEntity��� �
)��� �
)��� �
)��� �
;��� �
}
�� 
else
�� 
{
�� 
Logs
��  
.
��  !
WriteInfoLog
��! -
(
��- .
String
��. 4
.
��4 5
Format
��5 ;
(
��; <
$str
��< i
,
��i j

leadEntity
��k u
.
��u v
LeadId
��v |
)
��| }
)
��} ~
;
��~ 
}
�� 
}
�� 
else
�� 
{
�� 
Logs
�� 
.
�� 
WriteInfoLog
�� )
(
��) *
String
��* 0
.
��0 1
Format
��1 7
(
��7 8
$str
��8 Z
,
��Z [

Newtonsoft
��\ f
.
��f g
Json
��g k
.
��k l
JsonConvert
��l w
.
��w x
SerializeObject��x �
(��� �

leadEntity��� �
)��� �
)��� �
)��� �
;��� �
}
�� 
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
Logs
�� 
.
�� 
WriteErrorLog
�� "
(
��" #
String
��# )
.
��) *
Format
��* 0
(
��0 1
$str
��1 g
,
��g h

Newtonsoft
��i s
.
��s t
Json
��t x
.
��x y
JsonConvert��y �
.��� �
SerializeObject��� �
(��� �

leadEntity��� �
)��� �
,��� �
ex��� �
.��� �
Message��� �
)��� �
)��� �
;��� �
}
�� 
return
�� 
leadProcessed
��  
;
��  !
}
�� 	
internal
�� (
PriceQuoteParametersEntity
�� +"
GetPriceQuoteDetails
��, @
(
��@ A
uint
��A E
pqId
��F J
)
��J K
{
�� 	
return
�� 
_repository
�� 
.
�� (
FetchPriceQuoteDetailsById
�� 9
(
��9 :
pqId
��: >
)
��> ?
;
��? @
}
�� 	
internal
�� 
bool
�� "
SaveManufacturerLead
�� *
(
��* +$
ManufacturerLeadEntity
��+ A

leadEntity
��B L
)
��L M
{
�� 	
return
�� 
_repository
�� 
.
�� "
SaveManufacturerLead
�� 3
(
��3 4

leadEntity
��4 >
)
��> ?
;
��? @
}
�� 	
}
�� 
}�� �
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
 �)
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
}FF �M
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
manufacturerId	33 �
,
33� �
urlAPI
33� �
,
33� �
isAPIEnabled
33� �
)
33� �
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
)	WW �
;
WW� �
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

Newtonsoft	eez �
.
ee� �
Json
ee� �
.
ee� �
JsonConvert
ee� �
.
ee� �
SerializeObject
ee� �
(
ee� �

leadEntity
ee� �
)
ee� �
)
ee� �
)
ee� �
;
ee� �
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
,	 �
retryAttempt
� �
,
� �
dealerId
� �
,
� �
inquiryJSON
� �
)
� �
)
� �
;
� �
}
�� 
return
�� 
(
�� 
UInt32
�� 
.
�� 
TryParse
�� #
(
��# $
abInquiryId
��$ /
,
��/ 0
out
��1 4
abInqId
��5 <
)
��< =
&&
��> @
abInqId
��A H
>
��I J
$num
��K L
)
��L M
?
��N O
abInqId
��P W
:
��X Y
$num
��Z [
;
��[ \
}
�� 	
private
�� 
bool
�� 
UpdateABInquiryId
�� &
(
��& '
uint
��' +
leadId
��, 2
,
��2 3
uint
��4 8
abInqId
��9 @
,
��@ A
ushort
��B H
retryAttempt
��I U
)
��U V
{
�� 	
return
�� 
_repository
�� 
.
�� )
UpdateManufacturerABInquiry
�� :
(
��: ;
leadId
��; A
,
��A B
abInqId
��C J
,
��J K
retryAttempt
��L X
)
��X Y
;
��Y Z
}
�� 	
}
�� 
}�� �

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
} �
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
} �
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
} �
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
]$$) *�
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
} �1
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
manufacturerId	 �
,
� �
urlAPI
� �
,
� �
isAPIEnabled
� �
,
� �!
submitDuplicateLead
� �
)
� �
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

leadEntity	66} �
.
66� �
LeadId
66� �
)
66� �
;
66� �
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

leadEntity	HH{ �
)
HH� �
)
HH� �
)
HH� �
;
HH� �
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
ex	YY �
.
YY� �
Message
YY� �
)
YY� �
)
YY� �
;
YY� �
}ZZ 
return[[ 
response[[ 
;[[ 
}\\ 	
}^^ 
}__ �
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
} �<
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
isAPIEnabled	| �
)
� �
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