¢J
^D:\work\bikewaleweb\Bikewale.ExpiringListingReminder\ExpiringListingSellerDetailsRepository.cs
	namespace

 	
Bikewale


 
.

 #
ExpiringListingReminder

 *
{ 
public 

class 2
&ExpiringListingSellerDetailsRepository 7
{ 
public #
SellerDetailsListEntity &
getExpiringListings' :
(: ;
); <
{ 	#
SellerDetailsListEntity #'
objSellerDetailsListsEntity$ ?
=@ A
nullB F
;F G
try 
{ 
using 
( 
	DbCommand  
cmd! $
=% &
	DbFactory' 0
.0 1
GetDBCommand1 =
(= >
)> ?
)? @
{ 
cmd 
. 
CommandType #
=$ %
CommandType& 1
.1 2
StoredProcedure2 A
;A B
cmd 
. 
CommandText #
=$ %
$str& O
;O P
using 
( 
IDataReader &
dr' )
=* +
MySqlDatabase, 9
.9 :
SelectQuery: E
(E F
cmdF I
,I J
ConnectionTypeK Y
.Y Z
MasterDatabaseZ h
)h i
)i j
{ 
if 
( 
dr 
!= !
null" &
)& '
{ '
objSellerDetailsListsEntity   7
=  8 9
new  : =#
SellerDetailsListEntity  > U
(  U V
)  V W
;  W X
ICollection!! '
<!!' (
SellerDetailsEntity!!( ;
>!!; <
objListSevenDays!!= M
=!!N O
new!!P S

Collection!!T ^
<!!^ _
SellerDetailsEntity!!_ r
>!!r s
(!!s t
)!!t u
;!!u v
while## !
(##" #
dr### %
.##% &
Read##& *
(##* +
)##+ ,
)##, -
{$$ 
objListSevenDays&&  0
.&&0 1
Add&&1 4
(&&4 5
new&&5 8
SellerDetailsEntity&&9 L
(&&L M
)&&M N
{''  !
	inquiryId(($ -
=((. /
SqlReaderConvertor((0 B
.((B C
ToUInt16((C K
(((K L
dr((L N
[((N O
$str((O Z
]((Z [
)(([ \
,((\ ]
makeName))$ ,
=))- .
Convert))/ 6
.))6 7
ToString))7 ?
())? @
dr))@ B
[))B C
$str))C M
]))M N
)))N O
,))O P
	modelName**$ -
=**. /
Convert**0 7
.**7 8
ToString**8 @
(**@ A
dr**A C
[**C D
$str**D O
]**O P
)**P Q
,**Q R

sellerName++$ .
=++/ 0
Convert++1 8
.++8 9
ToString++9 A
(++A B
dr++B D
[++D E
$str++E S
]++S T
)++T U
,++U V
sellerMobileNumber,,$ 6
=,,7 8
Convert,,9 @
.,,@ A
ToString,,A I
(,,I J
dr,,J L
[,,L M
$str,,M ]
],,] ^
),,^ _
,,,_ `
sellerEmail--$ /
=--0 1
Convert--2 9
.--9 :
ToString--: B
(--B C
dr--C E
[--E F
$str--F U
]--U V
)--V W
,--W X
ModelId..$ +
=..+ ,
SqlReaderConvertor.., >
...> ?
ToUInt32..? G
(..G H
dr..H J
[..J K
$str..K T
]..T U
)..U V
,..V W
MakeYear//$ ,
=//, -
SqlReaderConvertor//- ?
.//? @

ToDateTime//@ J
(//J K
dr//K M
[//M N
$str//N X
]//X Y
)//Y Z
,//Z [
Owner00$ )
=00) *
SqlReaderConvertor00* <
.00< =
ToUInt1600= E
(00E F
dr00F H
[00H I
$str00I P
]00P Q
)00Q R
,00R S
RideDistance11$ 0
=111 2
Convert113 :
.11: ;
ToString11; C
(11C D
dr11D F
[11F G
$str11G S
]11S T
)11T U
,11U V
City22$ (
=22( )
Convert22) 0
.220 1
ToString221 9
(229 :
dr22: <
[22< =
$str22= C
]22C D
)22D E
,22E F
HostUrl33$ +
=33+ ,
Convert33- 4
.334 5
ToString335 =
(33= >
dr33> @
[33@ A
$str33A J
]33J K
)33K L
,33L M
OriginalImagePath44$ 5
=446 7
Convert448 ?
.44? @
ToString44@ H
(44H I
dr44I K
[44K L
$str44L _
]44_ `
)44` a
}55  !
)55! "
;55" #
}66 '
objSellerDetailsListsEntity88 7
.887 8+
sellerDetailsSevenDaysRemaining888 W
=88X Y
objListSevenDays88Z j
;88j k
if:: 
(::  
dr::  "
.::" #

NextResult::# -
(::- .
)::. /
)::/ 0
{;; 
ICollection<<  +
<<<+ ,
SellerDetailsEntity<<, ?
><<? @
objListOneDays<<A O
=<<P Q
new<<R U

Collection<<V `
<<<` a
SellerDetailsEntity<<a t
><<t u
(<<u v
)<<v w
;<<w x
while>>  %
(>>& '
dr>>' )
.>>) *
Read>>* .
(>>. /
)>>/ 0
)>>0 1
{??  !
objListOneDays@@$ 2
.@@2 3
Add@@3 6
(@@6 7
new@@7 :
SellerDetailsEntity@@; N
(@@N O
)@@O P
{AA$ %
	inquiryIdBB( 1
=BB2 3
SqlReaderConvertorBB4 F
.BBF G
ToUInt16BBG O
(BBO P
drBBP R
[BBR S
$strBBS ^
]BB^ _
)BB_ `
,BB` a
makeNameCC( 0
=CC1 2
ConvertCC3 :
.CC: ;
ToStringCC; C
(CCC D
drCCD F
[CCF G
$strCCG Q
]CCQ R
)CCR S
,CCS T
	modelNameDD( 1
=DD2 3
ConvertDD4 ;
.DD; <
ToStringDD< D
(DDD E
drDDE G
[DDG H
$strDDH S
]DDS T
)DDT U
,DDU V

sellerNameEE( 2
=EE3 4
ConvertEE5 <
.EE< =
ToStringEE= E
(EEE F
drEEF H
[EEH I
$strEEI W
]EEW X
)EEX Y
,EEY Z
sellerMobileNumberFF( :
=FF; <
ConvertFF= D
.FFD E
ToStringFFE M
(FFM N
drFFN P
[FFP Q
$strFFQ a
]FFa b
)FFb c
,FFc d
sellerEmailGG( 3
=GG4 5
ConvertGG6 =
.GG= >
ToStringGG> F
(GGF G
drGGG I
[GGI J
$strGGJ Y
]GGY Z
)GGZ [
,GG[ \
ModelIdHH( /
=HH0 1
SqlReaderConvertorHH2 D
.HHD E
ToUInt32HHE M
(HHM N
drHHN P
[HHP Q
$strHHQ Z
]HHZ [
)HH[ \
,HH\ ]
MakeYearII( 0
=II1 2
SqlReaderConvertorII3 E
.IIE F

ToDateTimeIIF P
(IIP Q
drIIQ S
[IIS T
$strIIT ^
]II^ _
)II_ `
,II` a
OwnerJJ( -
=JJ. /
SqlReaderConvertorJJ0 B
.JJB C
ToUInt16JJC K
(JJK L
drJJL N
[JJN O
$strJJO V
]JJV W
)JJW X
,JJX Y
RideDistanceKK( 4
=KK5 6
ConvertKK7 >
.KK> ?
ToStringKK? G
(KKG H
drKKH J
[KKJ K
$strKKK W
]KKW X
)KKX Y
,KKY Z
CityLL( ,
=LL- .
ConvertLL/ 6
.LL6 7
ToStringLL7 ?
(LL? @
drLL@ B
[LLB C
$strLLC I
]LLI J
)LLJ K
,LLK L
HostUrlMM( /
=MM0 1
ConvertMM2 9
.MM9 :
ToStringMM: B
(MMB C
drMMC E
[MME F
$strMMF O
]MMO P
)MMP Q
,MMQ R
OriginalImagePathNN( 9
=NN: ;
ConvertNN< C
.NNC D
ToStringNND L
(NNL M
drNNM O
[NNO P
$strNNP c
]NNc d
)NNd e
}OO$ %
)OO% &
;OO& '
}PP  !'
objSellerDetailsListsEntityRR  ;
.RR; <(
sellerDetailsOneDayRemainingRR< X
=RRY Z
objListOneDaysRR[ i
;RRi j
}SS 
}TT 
}UU 
}VV 
}WW 
catchXX 
(XX 
	ExceptionXX 
exXX 
)XX  
{YY 
LogsZZ 
.ZZ 
WriteErrorLogZZ "
(ZZ" #
$strZZ# H
+ZZI J
exZZK M
.ZZM N
MessageZZN U
)ZZU V
;ZZV W
SendMail[[ 
.[[ 
HandleException[[ (
([[( )
ex[[) +
,[[+ ,
$str[[- i
)[[i j
;[[j k
}\\ 
return^^ '
objSellerDetailsListsEntity^^ .
;^^. /
}__ 	
}`` 
}aa ˜Ü
QD:\work\bikewaleweb\Bikewale.ExpiringListingReminder\NotifySellerListingExpiry.cs
	namespace

 	
Bikewale


 
.

 #
ExpiringListingReminder

 *
{ 
public 

class %
NotifySellerListingExpiry *
{ 
private #
SellerDetailsListEntity ''
objSellerDetailsListsEntity( C
;C D
private 
readonly 
Utility  
.  !
UrlShortner! ,
url- 0
=1 2
new3 6
Utility7 >
.> ?
UrlShortner? J
(J K
)K L
;L M
private 
readonly 2
&ExpiringListingSellerDetailsRepository ?
objSellerDetails@ P
=Q R
newS V2
&ExpiringListingSellerDetailsRepositoryW }
(} ~
)~ 
;	 Ä
private 
readonly 
SMSTypes !
newSms" (
=) *
new+ .
SMSTypes/ 7
(7 8
)8 9
;9 :
private 
readonly 
string 

_repostUrl  *
=+ ,
$str- K
;K L
private 
readonly 
string 

_removeUrl  *
=+ ,
$str- K
;K L
private 
readonly 
string 
_emailSubjectOneDay  3
=4 5
$str6 t
;t u
private 
readonly 
string !
_emailSubjectSevenDay  5
=6 7
$str8 t
;t u
private 
readonly 
string 
_emailHeadingOneDay  3
=4 5
$str6 _
;_ `
private 
readonly 
string !
_emailHeadingSevenDay  5
=6 7
$str8 _
;_ `
private!! 
readonly!! 
string!! 
_messageOneDay!!  .
=!!/ 0
$str	!!1 Â
;
!!Â Ê
private"" 
readonly"" 
string"" 
_messageSevenDay""  0
=""1 2
$str	""3 Â
;
""Â Ê
private$$ 
readonly$$ 
string$$ 
_hostUrl$$  (
=$$) *
Bikewale$$+ 3
.$$3 4
Utility$$4 ;
.$$; <
BWConfiguration$$< K
.$$K L
Instance$$L T
.$$T U
	BwHostUrl$$U ^
;$$^ _
public** 
void** *
NotifySellerAboutListingExpiry** 2
(**2 3
)**3 4
{++ 	
try,, 
{-- '
objSellerDetailsListsEntity// +
=//, -
objSellerDetails//. >
.//> ?
getExpiringListings//? R
(//R S
)//S T
;//T U
Logs11 
.11 
WriteInfoLog11 !
(11! "
$str11" ]
)11] ^
;11^ _2
&SendExpiringListingReminderSMSAndEmail22 6
(226 7
)227 8
;228 9
Logs33 
.33 
WriteInfoLog33 !
(33! "
$str33" [
)33[ \
;33\ ]
}55 
catch66 
(66 
	Exception66 
ex66 
)66  
{77 
Logs88 
.88 
WriteErrorLog88 "
(88" #
$str88# S
+88T U
ex88V X
.88X Y
Message88Y `
)88` a
;88a b
SendMail99 
.99 
HandleException99 (
(99( )
ex99) +
,99+ ,
$str99- M
)99M N
;99N O
}:: 
};; 	
privateAA 
voidAA 2
&SendExpiringListingReminderSMSAndEmailAA ;
(AA; <
)AA< =
{BB 	
tryCC 
{DD 
ifEE 
(EE '
objSellerDetailsListsEntityEE /
!=EE0 2
nullEE3 7
)EE7 8
{FF 
foreachGG 
(GG 
varGG  
sellerGG! '
inGG( *'
objSellerDetailsListsEntityGG+ F
.GGF G(
sellerDetailsOneDayRemainingGGG c
)GGc d
{HH 
SendSMSII 
(II  
sellerII  &
,II& '
EnumSMSServiceTypeII( :
.II: ;.
"BikeListingExpiryOneDaySMSToSellerII; ]
)II] ^
;II^ _
	SendEmailJJ !
(JJ! "
sellerJJ" (
,JJ( )
EnumSMSServiceTypeJJ* <
.JJ< =.
"BikeListingExpiryOneDaySMSToSellerJJ= _
)JJ_ `
;JJ` a
}KK 
foreachMM 
(MM 
varMM  
sellerMM! '
inMM( *'
objSellerDetailsListsEntityMM+ F
.MMF G+
sellerDetailsSevenDaysRemainingMMG f
)MMf g
{NN 
SendSMSOO 
(OO  
sellerOO  &
,OO& '
EnumSMSServiceTypeOO( :
.OO: ;0
$BikeListingExpirySevenDaySMSToSellerOO; _
)OO_ `
;OO` a
	SendEmailPP !
(PP! "
sellerPP" (
,PP( )
EnumSMSServiceTypePP* <
.PP< =0
$BikeListingExpirySevenDaySMSToSellerPP= a
)PPa b
;PPb c
}QQ 
}RR 
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
(VV" #
$strVV# [
+VV\ ]
exVV^ `
.VV` a
MessageVVa h
)VVh i
;VVi j
SendMailWW 
.WW 
HandleExceptionWW (
(WW( )
exWW) +
,WW+ ,
$strWW- U
)WWU V
;WWV W
}XX 
}YY 	
private__ 
void__ 
SendSMS__ 
(__ 
SellerDetailsEntity__ 0
seller__1 7
,__7 8
EnumSMSServiceType__9 K
dayRemaining__L X
)__X Y
{`` 	
tryaa 
{bb 
stringcc 
	repostUrlcc  
=cc! "
stringcc# )
.cc) *
Formatcc* 0
(cc0 1

_repostUrlcc1 ;
,cc; <
_hostUrlcc= E
,ccE F
sellerccG M
.ccM N
	inquiryIdccN W
)ccW X
;ccX Y
stringdd 
	removeUrldd  
=dd! "
stringdd# )
.dd) *
Formatdd* 0
(dd0 1

_removeUrldd1 ;
,dd; <
_hostUrldd= E
,ddE F
sellerddG M
.ddM N
	inquiryIdddN W
)ddW X
;ddX Y
stringee 
messageee 
;ee 
UrlShortnerResponsegg #
shortRepostUrlgg$ 2
=gg3 4
urlgg5 8
.gg8 9
GetShortUrlgg9 D
(ggD E
	repostUrlggE N
)ggN O
;ggO P
UrlShortnerResponsehh #
shortRemoveUrlhh$ 2
=hh3 4
urlhh5 8
.hh8 9
GetShortUrlhh9 D
(hhD E
	removeUrlhhE N
)hhN O
;hhO P
ifjj 
(jj 
shortRepostUrljj "
!=jj# %
nulljj& *
)jj* +
	repostUrlkk 
=kk 
shortRepostUrlkk  .
.kk. /
ShortUrlkk/ 7
;kk7 8
ifmm 
(mm 
shortRemoveUrlmm "
!=mm# %
nullmm& *
)mm* +
	removeUrlnn 
=nn 
shortRemoveUrlnn  .
.nn. /
ShortUrlnn/ 7
;nn7 8
ifpp 
(pp 
EnumSMSServiceTypepp &
.pp& '.
"BikeListingExpiryOneDaySMSToSellerpp' I
.ppI J
EqualsppJ P
(ppP Q
dayRemainingppQ ]
)pp] ^
)pp^ _
{qq 
messagerr 
=rr 
stringrr $
.rr$ %
Formatrr% +
(rr+ ,
_messageOneDayrr, :
,rr: ;
sellerrr< B
.rrB C
makeNamerrC K
,rrK L
sellerrrM S
.rrS T
	modelNamerrT ]
,rr] ^
	removeUrlrr_ h
,rrh i
	repostUrlrrj s
)rrs t
;rrt u
newSmsss 
.ss &
ExpiringListingReminderSMSss 5
(ss5 6
sellerss6 <
.ss< =
sellerMobileNumberss= O
,ssO P
$strssQ s
,sss t
dayRemaining	ssu Å
,
ssÅ Ç
message
ssÉ ä
)
ssä ã
;
ssã å
Logstt 
.tt 
WriteInfoLogtt %
(tt% &
$strtt& T
+ttU V
sellerttW ]
.tt] ^
	inquiryIdtt^ g
)ttg h
;tth i
}uu 
elsevv 
{ww 
messagexx 
=xx 
stringxx $
.xx$ %
Formatxx% +
(xx+ ,
_messageSevenDayxx, <
,xx< =
sellerxx> D
.xxD E
makeNamexxE M
,xxM N
sellerxxO U
.xxU V
	modelNamexxV _
,xx_ `
	removeUrlxxa j
,xxj k
	repostUrlxxl u
)xxu v
;xxv w
newSmsyy 
.yy &
ExpiringListingReminderSMSyy 5
(yy5 6
selleryy6 <
.yy< =
sellerMobileNumberyy= O
,yyO P
$stryyQ s
,yys t
dayRemaining	yyu Å
,
yyÅ Ç
message
yyÉ ä
)
yyä ã
;
yyã å
Logszz 
.zz 
WriteInfoLogzz %
(zz% &
$strzz& V
+zzW X
sellerzzY _
.zz_ `
	inquiryIdzz` i
)zzi j
;zzj k
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
string# )
.) *
Format* 0
(0 1
$str1 L
,L M
sellerN T
,T U
dayRemainingV b
)b c
+d e
exf h
.h i
Messagei p
)p q
;q r
SendMail
ÄÄ 
.
ÄÄ 
HandleException
ÄÄ (
(
ÄÄ( )
ex
ÄÄ) +
,
ÄÄ+ ,
string
ÄÄ- 3
.
ÄÄ3 4
Format
ÄÄ4 :
(
ÄÄ: ;
$str
ÄÄ; V
,
ÄÄV W
seller
ÄÄX ^
,
ÄÄ^ _
dayRemaining
ÄÄ` l
)
ÄÄl m
)
ÄÄm n
;
ÄÄn o
}
ÅÅ 
}
ÉÉ 	
private
ãã 
void
ãã 
	SendEmail
ãã 
(
ãã !
SellerDetailsEntity
ãã 2
seller
ãã3 9
,
ãã9 : 
EnumSMSServiceType
ãã; M
dayRemaining
ããN Z
)
ããZ [
{
åå 	
try
çç 
{
éé 
string
èè 
	repostUrl
èè  
=
èè! "
string
èè# )
.
èè) *
Format
èè* 0
(
èè0 1

_repostUrl
èè1 ;
,
èè; <
_hostUrl
èè= E
,
èèE F
seller
èèG M
.
èèM N
	inquiryId
èèN W
)
èèW X
;
èèX Y!
UrlShortnerResponse
êê #
shortRepostUrl
êê$ 2
;
êê2 3
shortRepostUrl
ëë 
=
ëë  
url
ëë! $
.
ëë$ %
GetShortUrl
ëë% 0
(
ëë0 1
	repostUrl
ëë1 :
)
ëë: ;
;
ëë; <
if
ìì 
(
ìì 
shortRepostUrl
ìì "
!=
ìì# %
null
ìì& *
)
ìì* +
	repostUrl
îî 
=
îî 
shortRepostUrl
îî  .
.
îî. /
ShortUrl
îî/ 7
;
îî7 8
string
ïï 
qEncoded
ïï 
=
ïï  !
Utils
ïï" '
.
ïï' (
Utils
ïï( -
.
ïï- .
EncryptTripleDES
ïï. >
(
ïï> ?
string
ïï? E
.
ïïE F
Format
ïïF L
(
ïïL M
$str
ïïM [
,
ïï[ \
(
ïï] ^
int
ïï^ a
)
ïïa b
Bikewale
ïïb j
.
ïïj k
Entities
ïïk s
.
ïïs t
UserReviews
ïït 
.ïï Ä(
UserReviewPageSourceEnumïïÄ ò
.ïïò ô
UsedBikes_Emailïïô ®
)ïï® ©
)ïï© ™
;ïï™ ´
if
ññ 
(
ññ  
EnumSMSServiceType
ññ &
.
ññ& '0
"BikeListingExpiryOneDaySMSToSeller
ññ' I
.
ññI J
Equals
ññJ P
(
ññP Q
dayRemaining
ññQ ]
)
ññ] ^
)
ññ^ _
{
óó 
ComposeEmailBase
òò $
objEmail
òò% -
=
òò. /
new
òò0 3*
ExpiringListingReminderEmail
òò4 P
(
òòP Q
seller
òòQ W
.
òòW X

sellerName
òòX b
,
òòb c
seller
ôô 
.
ôô 
makeName
ôô '
,
ôô' (
seller
öö 
.
öö 
	modelName
öö (
,
öö( ) 
EnumSMSServiceType
õõ *
.
õõ* +0
"BikeListingExpiryOneDaySMSToSeller
õõ+ M
,
õõM N
	repostUrl
úú !
,
úú! "!
_emailHeadingOneDay
ùù +
,
ùù+ ,
Image
ûû 
.
ûû !
GetPathToShowImages
ûû 1
(
ûû1 2
seller
ûû2 8
.
ûû8 9
OriginalImagePath
ûû9 J
,
ûûJ K
seller
ûûL R
.
ûûR S
HostUrl
ûûS Z
,
ûûZ [
	ImageSize
ûû\ e
.
ûûe f
_110x61
ûûf m
)
ûûm n
,
ûûn o
Format
üü 
.
üü 
FormatNumeric
üü ,
(
üü, -
seller
üü- 3
.
üü3 4
RideDistance
üü4 @
)
üü@ A
,
üüA B
qEncoded
††  
,
††  !
BWConfiguration
°° '
.
°°' (
Instance
°°( 0
.
°°0 1
	BwHostUrl
°°1 :
,
°°: ;
seller
¢¢ 
.
¢¢ 
ModelId
¢¢ &
)
¢¢& '
;
¢¢' (
objEmail
££ 
.
££ 
Send
££ !
(
££! "
seller
££" (
.
££( )
sellerEmail
££) 4
,
££4 5!
_emailSubjectOneDay
££6 I
)
££I J
;
££J K
Logs
§§ 
.
§§ 
WriteInfoLog
§§ %
(
§§% &
$str
§§& R
+
§§S T
seller
§§U [
.
§§[ \
	inquiryId
§§\ e
)
§§e f
;
§§f g
}
•• 
else
¶¶ 
{
ßß 
ComposeEmailBase
®® $
objEmail
®®% -
=
®®. /
new
®®0 3*
ExpiringListingReminderEmail
®®4 P
(
®®P Q
seller
®®Q W
.
®®W X

sellerName
®®X b
,
®®b c
seller
©© 
.
©© 
makeName
©© '
,
©©' (
seller
™™ 
.
™™ 
	modelName
™™ (
,
™™( ) 
EnumSMSServiceType
´´ *
.
´´* +2
$BikeListingExpirySevenDaySMSToSeller
´´+ O
,
´´O P
	repostUrl
¨¨ !
,
¨¨! "#
_emailHeadingSevenDay
≠≠ -
,
≠≠- .
Image
ÆÆ 
.
ÆÆ !
GetPathToShowImages
ÆÆ 1
(
ÆÆ1 2
seller
ÆÆ2 8
.
ÆÆ8 9
OriginalImagePath
ÆÆ9 J
,
ÆÆJ K
seller
ÆÆL R
.
ÆÆR S
HostUrl
ÆÆS Z
,
ÆÆZ [
	ImageSize
ÆÆ\ e
.
ÆÆe f
_110x61
ÆÆf m
)
ÆÆm n
,
ÆÆn o
Format
ØØ 
.
ØØ 
FormatNumeric
ØØ ,
(
ØØ, -
seller
ØØ- 3
.
ØØ3 4
RideDistance
ØØ4 @
)
ØØ@ A
,
ØØA B
qEncoded
∞∞  
,
∞∞  !
BWConfiguration
±± '
.
±±' (
Instance
±±( 0
.
±±0 1
	BwHostUrl
±±1 :
,
±±: ;
seller
≤≤ 
.
≤≤ 
ModelId
≤≤ &
)
≤≤& '
;
≤≤' (
objEmail
≥≥ 
.
≥≥ 
Send
≥≥ !
(
≥≥! "
seller
≥≥" (
.
≥≥( )
sellerEmail
≥≥) 4
,
≥≥4 5#
_emailSubjectSevenDay
≥≥6 K
)
≥≥K L
;
≥≥L M
Logs
¥¥ 
.
¥¥ 
WriteInfoLog
¥¥ %
(
¥¥% &
$str
¥¥& T
+
¥¥U V
seller
¥¥W ]
.
¥¥] ^
	inquiryId
¥¥^ g
)
¥¥g h
;
¥¥h i
}
µµ 
}
∂∂ 
catch
∑∑ 
(
∑∑ 
	Exception
∑∑ 
ex
∑∑ 
)
∑∑  
{
∏∏ 
Logs
ππ 
.
ππ 
WriteErrorLog
ππ "
(
ππ" #
string
ππ# )
.
ππ) *
Format
ππ* 0
(
ππ0 1
$str
ππ1 M
,
ππM N
seller
ππO U
,
ππU V
dayRemaining
ππW c
)
ππc d
+
ππe f
ex
ππg i
.
ππi j
Message
ππj q
)
ππq r
;
ππr s
SendMail
∫∫ 
.
∫∫ 
HandleException
∫∫ (
(
∫∫( )
ex
∫∫) +
,
∫∫+ ,
string
∫∫- 3
.
∫∫3 4
Format
∫∫4 :
(
∫∫: ;
$str
∫∫; W
,
∫∫W X
seller
∫∫Y _
,
∫∫_ `
dayRemaining
∫∫a m
)
∫∫m n
)
∫∫n o
;
∫∫o p
}
ªª 
}
ºº 	
}
ææ 
}øø ˇ
?D:\work\bikewaleweb\Bikewale.ExpiringListingReminder\Program.cs
	namespace 	
Bikewale
 
. #
ExpiringListingReminder *
{ 
class 	
Program
 
{ 
static		 
void		 
Main		 
(		 
string		 
[		  
]		  !
args		" &
)		& '
{

 	
Logs 
. 
WriteInfoLog 
( 
$str I
)I J
;J K
try 
{ 
Logs 
. 
WriteInfoLog !
(! "
$str" W
)W X
;X Y
( 
new %
NotifySellerListingExpiry .
(. /
)/ 0
)0 1
.1 2*
NotifySellerAboutListingExpiry2 P
(P Q
)Q R
;R S
Logs 
. 
WriteInfoLog !
(! "
$str" U
)U V
;V W
} 
catch 
( 
	Exception 
ex 
)  
{ 
Logs 
. 
WriteErrorLog "
(" #
$str# 9
+: ;
ex< >
.> ?
Message? F
)F G
;G H
SendMail 
. 
HandleException (
(( )
ex) +
,+ ,
$str- V
)V W
;W X
} 
Logs 
. 
WriteInfoLog 
( 
$str G
)G H
;H I
} 	
} 
} ˙
OD:\work\bikewaleweb\Bikewale.ExpiringListingReminder\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str ;
); <
]< =
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
$str =
)= >
]> ?
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
]$$) *™
KD:\work\bikewaleweb\Bikewale.ExpiringListingReminder\SellerDetailsEntity.cs
	namespace 	
Bikewale
 
. #
ExpiringListingReminder *
{ 
public 

class 
SellerDetailsEntity $
{		 
public

 
int

 
	inquiryId

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

) *
public 
string 
makeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
	modelName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 

sellerName  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
sellerMobileNumber (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
string 
sellerEmail !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
DateTime 
MakeYear  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
ushort 
Owner 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
RideDistance "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
City 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
ModelId 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
HostUrl 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
OriginalImagePath '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
} 
} †
PD:\work\bikewaleweb\Bikewale.ExpiringListingReminder\SellerDetailsListsEntity.cs
	namespace 	
Bikewale
 
. #
ExpiringListingReminder *
{ 
public		 

class		 #
SellerDetailsListEntity		 (
{

 
public 
IEnumerable 
< 
SellerDetailsEntity .
>. /+
sellerDetailsSevenDaysRemaining0 O
{P Q
getR U
;U V
setW Z
;Z [
}\ ]
public 
IEnumerable 
< 
SellerDetailsEntity .
>. /(
sellerDetailsOneDayRemaining0 L
{M N
getO R
;R S
setT W
;W X
}Y Z
} 
} 