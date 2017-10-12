ÖM
4D:\work\bikewaleweb\Bikewale.BookingSMS\CustomSMS.cs
	namespace 	
Bikewale
 
. 

BookingSMS 
{		 
public 

enum 
EnumSMSServiceType "
{ /
#UsedPurchaseInquiryIndividualSeller +
=, -
$num. /
,/ 0+
UsedPurchaseInquiryDealerSeller '
=( )
$num* +
,+ ,.
"UsedPurchaseInquiryIndividualBuyer *
=+ ,
$num- .
,. /*
UsedPurchaseInquiryDealerBuyer &
=' (
$num) *
,* +
NewBikeSellOpr 
= 
$num 
, 
NewBikeBuyOpr 
= 
$num 
, 
AccountDetails 
= 
$num 
,  
CustomerRegistration 
= 
$num  
,  ! 
DealerAddressRequest 
= 
$num  
,  !
SellInquiryReminder 
= 
$num  
,  !
PromotionalOffer 
= 
$num 
, 
NewBikeQuote 
= 
$num 
, 
	CustomSMS 
= 
$num 
, 
CustomerPassword 
= 
$num 
, 
MobileVerification 
= 
$num 
,  
PaidRenewalAlert 
= 
$num 
, 
PaidRenewal 
= 
$num 
, 
MyGarageSMS   
=   
$num   
,   
MyGarageAlerts!! 
=!! 
$num!! 
,!! 
InsuranceRenewal"" 
="" 
$num"" 
,"" (
NewBikePriceQuoteSMSToDealer## $
=##% &
$num##' )
,##) **
NewBikePriceQuoteSMSToCustomer$$ &
=$$' (
$num$$) +
,$$+ ,#
BikeBookedSMSToCustomer%% 
=%%  !
$num%%" $
,%%$ %!
BikeBookedSMSToDealer&& 
=&& 
$num&&  "
,&&" #
RSAFreeHelmetSMS'' 
='' 
$num'' 
}(( 
public// 

class// 
	CustomSMS// 
{00 
	protected11 
RabbitMqPublish11 !
publish11" )
=11* +
new11, /
RabbitMqPublish110 ?
(11? @
)11@ A
;11A B
public55 
void55 
SendSMS55 
(55 
)55 
{66 	
IEnumerable77 
<77 
CustomSMSEntity77 '
>77' (
lstSMS77) /
=770 1
null772 6
;776 7
CustomSMSDAL88 
objDal88 
=88  !
null88" &
;88& '
try99 
{:: 
objDal;; 
=;; 
new;; 
CustomSMSDAL;; )
(;;) *
);;* +
;;;+ ,
lstSMS<< 
=<< 
objDal<< 
.<<  
FetchSMSData<<  ,
(<<, -
)<<- .
;<<. /
if== 
(== 
lstSMS== 
!=== 
null== "
)==" #
{>> 
foreach?? 
(?? 
CustomSMSEntity?? ,
	smsEntity??- 6
in??7 9
lstSMS??: @
)??@ A
{@@ 
	SendSMSExAA !
(AA! "
	smsEntityAA" +
)AA+ ,
;AA, -
}BB 
}CC 
}DD 
catchEE 
(EE 
	ExceptionEE 
exEE 
)EE  
{FF 
LogsGG 
.GG 
WriteErrorLogGG "
(GG" #
$strGG# <
+GG= >
exGG? A
.GGA B
MessageGGB I
)GGI J
;GGJ K
}HH 
}II 	
privateRR 
stringRR 
	FormatSMSRR  
(RR  !
CustomSMSEntityRR! 0
smsRR1 4
)RR4 5
{SS 	
stringTT 

smsMessageTT 
=TT 
StringTT  &
.TT& '
FormatTT' -
(TT- .
$str	TT. ª
,
TTª º
sms
TTΩ ¿
.
TT¿ ¡
BikeName
TT¡ …
,
TT…  
sms
TTÀ Œ
.
TTŒ œ

DealerName
TTœ Ÿ
,
TTŸ ⁄
sms
TT€ ﬁ
.
TTﬁ ﬂ
DealerContact
TTﬂ Ï
,
TTÏ Ì"
ConfigurationManager
TTÓ Ç
.
TTÇ É
AppSettings
TTÉ é
[
TTé è
$str
TTè ®
]
TT® ©
)
TT© ™
;
TT™ ´
returnUU 

smsMessageUU 
;UU 
}VV 	
private\\ 
void\\ 
	SendSMSEx\\ 
(\\ 
CustomSMSEntity\\ .
sms\\/ 2
)\\2 3
{]] 	
int^^ 
smsId^^ 
=^^ 
$num^^ 
;^^ 
string__ 
message__ 
=__ 
String__ #
.__# $
Empty__$ )
;__) *
string`` 
customerContact`` "
=``# $
String``% +
.``+ ,
Empty``, 1
;``1 2
stringaa 
appNameaa 
=aa 
Stringaa #
.aa# $
Emptyaa$ )
;aa) *
NameValueCollectionbb 
nvcSMSbb  &
=bb' (
nullbb) -
;bb- .
trycc 
{dd 
appNameee 
=ee  
ConfigurationManageree .
.ee. /
AppSettingsee/ :
[ee: ;
$stree; L
]eeL M
;eeM N
messageff 
=ff 
	FormatSMSff #
(ff# $
smsff$ '
)ff' (
;ff( )
customerContactgg 
=gg  !
Stringgg" (
.gg( )
Formatgg) /
(gg/ 0
$strgg0 7
,gg7 8
smsgg9 <
.gg< =
CustomerContactgg= L
)ggL M
;ggM N
smsIdhh 
=hh 
SaveSMSToDbhh #
(hh# $
smshh$ '
.hh' (
CustomerContacthh( 7
,hh7 8
messagehh9 @
,hh@ A
EnumSMSServiceTypehhB T
.hhT U
RSAFreeHelmetSMShhU e
,hhe f
Stringhhg m
.hhm n
Emptyhhn s
)hhs t
;hht u
Logsii 
.ii 
WriteInfoLogii !
(ii! "
Stringii" (
.ii( )
Formatii) /
(ii/ 0
$strii0 E
,iiE F
smsIdiiG L
)iiL M
)iiM N
;iiN O
nvcSMSjj 
=jj 
newjj 
NameValueCollectionjj 0
(jj0 1
)jj1 2
;jj2 3
nvcSMSkk 
.kk 
Addkk 
(kk 
$strkk 
,kk  
Convertkk! (
.kk( )
ToStringkk) 1
(kk1 2
smsIdkk2 7
)kk7 8
)kk8 9
;kk9 :
nvcSMSll 
.ll 
Addll 
(ll 
$strll $
,ll$ %
messagell& -
)ll- .
;ll. /
nvcSMSmm 
.mm 
Addmm 
(mm 
$strmm %
,mm% &
customerContactmm' 6
)mm6 7
;mm7 8
nvcSMSnn 
.nn 
Addnn 
(nn 
$strnn #
,nn# $
$strnn% )
)nn) *
;nn* +
nvcSMSoo 
.oo 
Addoo 
(oo 
$stroo %
,oo% &
$stroo' )
)oo) *
;oo* +
publishqq 
.qq 
PublishToQueueqq &
(qq& '
appNameqq' .
,qq. /
nvcSMSqq0 6
)qq6 7
;qq7 8
Logsrr 
.rr 
WriteInfoLogrr !
(rr! "
Stringrr" (
.rr( )
Formatrr) /
(rr/ 0
$strrr0 H
,rrH I
smsIdrrJ O
)rrO P
)rrP Q
;rrQ R
}ss 
catchtt 
(tt 
	Exceptiontt 
extt 
)tt  
{uu 
Logsvv 
.vv 
WriteErrorLogvv "
(vv" #
$strvv# >
+vv? @
exvvA C
.vvC D
MessagevvD K
)vvK L
;vvL M
}ww 
}yy 	
private
ÉÉ 
int
ÉÉ 
SaveSMSToDb
ÉÉ 
(
ÉÉ  
string
ÉÉ  &
number
ÉÉ' -
,
ÉÉ- .
string
ÉÉ/ 5
message
ÉÉ6 =
,
ÉÉ= > 
EnumSMSServiceType
ÉÉ? Q
smsType
ÉÉR Y
,
ÉÉY Z
string
ÉÉ[ a
retMsg
ÉÉb h
)
ÉÉh i
{
ÑÑ 	
int
ÖÖ 
smsId
ÖÖ 
=
ÖÖ 
$num
ÖÖ 
;
ÖÖ 
try
ÜÜ 
{
áá 
CustomSMSDAL
àà 
objDal
àà #
=
àà$ %
new
àà& )
CustomSMSDAL
àà* 6
(
àà6 7
)
àà7 8
;
àà8 9
smsId
ââ 
=
ââ 
Convert
ââ 
.
ââ  
ToInt32
ââ  '
(
ââ' (
objDal
ââ( .
.
ââ. /
	InsertSMS
ââ/ 8
(
ââ8 9
number
ââ9 ?
,
ââ? @
message
ââA H
,
ââH I
smsType
ââJ Q
,
ââQ R
retMsg
ââS Y
,
ââY Z
false
ââ[ `
)
ââ` a
)
ââa b
;
ââb c
}
ää 
catch
ãã 
(
ãã 
	Exception
ãã 
ex
ãã 
)
ãã  
{
åå 
Logs
çç 
.
çç 
WriteErrorLog
çç "
(
çç" #
$str
çç# @
+
ççA B
ex
ççC E
.
ççE F
Message
ççF M
)
ççM N
;
ççN O
}
éé 
return
èè 
smsId
èè 
;
èè 
}
êê 	
}
ëë 
}íí ™
7D:\work\bikewaleweb\Bikewale.BookingSMS\CustomSMSDAL.cs
	namespace 	
Bikewale
 
. 

BookingSMS 
{		 
public 

class 
CustomSMSDAL 
{ 
	protected 
string 
connectionString )
=* +
String, 2
.2 3
Empty3 8
;8 9
public 
CustomSMSDAL 
( 
) 
{ 	
this 
. 
connectionString !
=" # 
ConfigurationManager$ 8
.8 9
AppSettings9 D
[D E
$strE ^
]^ _
;_ `
} 	
public%% 
string%% 
	InsertSMS%% 
(%%  
string%%  &
number%%' -
,%%- .
string%%/ 5
message%%6 =
,%%= >
EnumSMSServiceType%%? Q
smsType%%R Y
,%%Y Z
string%%[ a
retMsg%%b h
,%%h i
bool%%j n
status%%o u
)%%u v
{&& 	
throw(( 
new(( 
	Exception(( 
(((  
$str	((  ù
)
((ù û
;
((û ü
}RR 	
publicXX 
IEnumerableXX 
<XX 
CustomSMSEntityXX *
>XX* +
FetchSMSDataXX, 8
(XX8 9
)XX9 :
{YY 	
throwZZ 
newZZ 
	ExceptionZZ 
(ZZ  
$strZZ  J
)ZZJ K
;ZZK L
}
ïï 	
}
ññ 
}óó à
:D:\work\bikewaleweb\Bikewale.BookingSMS\CustomSMSEntity.cs
	namespace 	
Bikewale
 
. 

BookingSMS 
{ 
public 

class 
CustomSMSEntity  
{ 
public 
string 
CustomerContact %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 

DealerName  
{! "
get# &
;& '
set( +
;+ ,
}- .
public		 
string		 
DealerContact		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		0 1
}

 
} ü
2D:\work\bikewaleweb\Bikewale.BookingSMS\Program.cs
	namespace 	
Bikewale
 
. 

BookingSMS 
{ 
class		 	
Program		
 
{

 
static 
void 
Main 
( 
string 
[  
]  !
args" &
)& '
{ 	
Logs 
. 
WriteInfoLog 
( 
$str A
)A B
;B C
bool 
isOfferAvailable !
=" #
false$ )
;) *
	CustomSMS 
obj 
= 
new 
	CustomSMS  )
() *
)* +
;+ ,
try 
{ 
isOfferAvailable  
=! "
Convert# *
.* +
	ToBoolean+ 4
(4 5 
ConfigurationManager5 I
.I J
AppSettingsJ U
[U V
$strV h
]h i
)i j
;j k
if 
( 
isOfferAvailable $
)$ %
obj 
. 
SendSMS 
(  
)  !
;! "
} 
catch 
( 
	Exception 
ex 
)  
{ 
Logs 
. 
WriteErrorLog "
(" #
$str# 9
+: ;
ex< >
.> ?
Message? F
)F G
;G H
} 
Logs 
. 
WriteInfoLog 
( 
$str ?
)? @
;@ A
} 	
} 
} Ì
BD:\work\bikewaleweb\Bikewale.BookingSMS\Properties\AssemblyInfo.cs
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
]$$) *