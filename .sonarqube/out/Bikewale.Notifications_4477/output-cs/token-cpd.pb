è
>D:\work\bikewaleweb\Bikewale.Notifications\ComposeEmailBase.cs
	namespace 	
Bikewale
 
. 
Notifications  
{ 
public 

abstract 
class 
ComposeEmailBase *
{ 
public 
abstract 
string 
ComposeBody *
(* +
)+ ,
;, -
public 
void 
Send 
( 
string 
sendTo  &
,& '
string( .
subject/ 6
)6 7
{ 	
	SendMails 
mail 
= 
new  
	SendMails! *
(* +
)+ ,
;, -
mail 
. 
SendMail 
( 
sendTo  
,  !
subject" )
,) *
ComposeBody+ 6
(6 7
)7 8
)8 9
;9 :
} 	
public)) 
void)) 
Send)) 
()) 
string)) 
sendTo))  &
,))& '
string))( .
subject))/ 6
,))6 7
string))8 >
replyTo))? F
)))F G
{** 	
	SendMails++ 
mail++ 
=++ 
new++  
	SendMails++! *
(++* +
)+++ ,
;++, -
mail,, 
.,, 
SendMail,, 
(,, 
sendTo,,  
,,,  !
subject,," )
,,,) *
ComposeBody,,+ 6
(,,6 7
),,7 8
,,,8 9
replyTo,,: A
),,A B
;,,B C
}-- 	
public99 
void99 
Send99 
(99 
string99 
sendTo99  &
,99& '
string99( .
subject99/ 6
,996 7
string998 >
replyTo99? F
,99F G
string99H N
[99N O
]99O P
cc99Q S
,99S T
string99U [
[99[ \
]99\ ]
bcc99^ a
)99a b
{:: 	
	SendMails;; 
mail;; 
=;; 
new;;  
	SendMails;;! *
(;;* +
);;+ ,
;;;, -
mail<< 
.<< 
SendMail<< 
(<< 
sendTo<<  
,<<  !
subject<<" )
,<<) *
ComposeBody<<+ 6
(<<6 7
)<<7 8
,<<8 9
replyTo<<: A
,<<A B
cc<<C E
,<<E F
bcc<<G J
)<<J K
;<<K L
}== 	
}?? 
}@@ ƒ
RD:\work\bikewaleweb\Bikewale.Notifications\Configuration\ErrorMailConfiguration.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
Configuration! .
{ 
class

 	"
ErrorMailConfiguration


  
{ 
public 
static 
string 
ERRORMAILTO (
{) *
get+ .
{/ 0
return1 7 
ConfigurationManager8 L
.L M
AppSettingsM X
[X Y
$strY f
]f g
;g h
}i j
}k l
public 
static 
string 
APPLICATIONNAME ,
{- .
get/ 2
{3 4
return5 ; 
ConfigurationManager< P
.P Q
AppSettingsQ \
[\ ]
$str] n
]n o
;o p
}q r
}s t
} 
} ™
MD:\work\bikewaleweb\Bikewale.Notifications\Configuration\MailConfiguration.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
Configuration! .
{ 
class

 	
MailConfiguration


 
{ 
public 
static 
string 

SMTPSERVER '
{( )
get* -
{. /
return0 6 
ConfigurationManager7 K
.K L
AppSettingsL W
[W X
$strX d
]d e
;e f
}g h
}i j
public 
static 
string 
	LOCALMAIL &
{' (
get) ,
{- .
return/ 5 
ConfigurationManager6 J
.J K
AppSettingsK V
[V W
$strW b
]b c
;c d
}e f
}g h
public 
static 
string 
MAILFROM %
{& '
get( +
{, -
return. 4 
ConfigurationManager5 I
.I J
AppSettingsJ U
[U V
$strV `
]` a
;a b
}c d
}e f
public 
static 
string 
REPLYTO $
{% &
get' *
{+ ,
return- 3 
ConfigurationManager4 H
.H I
AppSettingsI T
[T U
$strU ^
]^ _
;_ `
}a b
}c d
} 
} Û:
8D:\work\bikewaleweb\Bikewale.Notifications\ErrorClass.cs
	namespace 	
Bikewale
 
. 
Notifications  
{ 
public		 

class		 

ErrorClass		 
{

 
	protected 
static 
readonly !
ILog" &
log' *
=+ ,

LogManager- 7
.7 8
	GetLogger8 A
(A B
typeofB H
(H I

ErrorClassI S
)S T
)T U
;U V
private 
HttpContext 
objTrace $
=% &
HttpContext' 2
.2 3
Current3 :
;: ;
private 
	Exception 
_err 
; 
public 
	Exception 
Error 
{ 	
get 
{ 
return 
_err 
; 
}  
set 
{ 
_err 
= 
value 
; 
}  !
} 	
private 
string 
_pageUrl 
;  
public 
string 
PageUrl 
{ 	
get 
{ 
return 
_pageUrl !
;! "
}# $
set   
{   
_pageUrl   
=   
value   "
;  " #
}  $ %
}!! 	
public(( 

ErrorClass(( 
((( 
	Exception(( #
ex(($ &
,((& '
string((( .
pageUrl((/ 6
)((6 7
{)) 	
Error++ 
=++ 
ex++ 
;++ 
PageUrl,, 
=,, 
pageUrl,, 
;,, $
LogCurrentHttpParameters-- $
(--$ %
)--% &
;--& '
log.. 
... 
Error.. 
(.. 
pageUrl.. 
,.. 
ex.. !
)..! "
;.." #
}00 	
public77 

ErrorClass77 
(77 
SqlException77 &
ex77' )
,77) *
string77+ 1
pageUrl772 9
)779 :
{88 	
Error99 
=99 
(99 
	Exception99 
)99 
ex99 !
;99! "
PageUrl:: 
=:: 
pageUrl:: 
;:: $
LogCurrentHttpParameters;; $
(;;$ %
);;% &
;;;& '
log<< 
.<< 
Error<< 
(<< 
pageUrl<< 
,<< 
ex<< !
)<<! "
;<<" #
}== 	
publicDD 

ErrorClassDD 
(DD 
OleDbExceptionDD (
exDD) +
,DD+ ,
stringDD- 3
pageUrlDD4 ;
)DD; <
{EE 	
ErrorFF 
=FF 
(FF 
	ExceptionFF 
)FF 
exFF !
;FF! "
PageUrlGG 
=GG 
pageUrlGG 
;GG $
LogCurrentHttpParametersHH $
(HH$ %
)HH% &
;HH& '
logII 
.II 
ErrorII 
(II 
pageUrlII 
,II 
exII !
)II! "
;II" #
}JJ 	
privatePP 
voidPP $
LogCurrentHttpParametersPP -
(PP- .
)PP. /
{QQ 	
ifRR 
(RR 
objTraceRR 
!=RR 
nullRR  
&&RR! #
objTraceRR$ ,
.RR, -
RequestRR- 4
!=RR5 7
nullRR8 <
)RR< =
{SS 
log4netTT 
.TT 
ThreadContextTT %
.TT% &

PropertiesTT& 0
[TT0 1
$strTT1 ;
]TT; <
=TT= >
ConvertTT? F
.TTF G
ToStringTTG O
(TTO P
objTraceTTP X
.TTX Y
RequestTTY `
.TT` a
ServerVariablesTTa p
[TTp q
$str	TTq Å
]
TTÅ Ç
)
TTÇ É
;
TTÉ Ñ
log4netUU 
.UU 
ThreadContextUU %
.UU% &

PropertiesUU& 0
[UU0 1
$strUU1 :
]UU: ;
=UU< =
objTraceUU> F
.UUF G
RequestUUG N
.UUN O
BrowserUUO V
.UUV W
TypeUUW [
;UU[ \
log4netVV 
.VV 
ThreadContextVV %
.VV% &

PropertiesVV& 0
[VV0 1
$strVV1 ;
]VV; <
=VV= >
objTraceVV? G
.VVG H
RequestVVH O
.VVO P
UrlReferrerVVP [
;VV[ \
log4netWW 
.WW 
ThreadContextWW %
.WW% &

PropertiesWW& 0
[WW0 1
$strWW1 <
]WW< =
=WW> ?
objTraceWW@ H
.WWH I
RequestWWI P
.WWP Q
	UserAgentWWQ Z
;WWZ [
log4netXX 
.XX 
ThreadContextXX %
.XX% &

PropertiesXX& 0
[XX0 1
$strXX1 ?
]XX? @
=XXA B
objTraceXXC K
.XXK L
RequestXXL S
.XXS T
PhysicalPathXXT `
;XX` a
log4netYY 
.YY 
ThreadContextYY %
.YY% &

PropertiesYY& 0
[YY0 1
$strYY1 7
]YY7 8
=YY9 :
objTraceYY; C
.YYC D
RequestYYD K
.YYK L
UrlYYL O
.YYO P
HostYYP T
;YYT U
log4netZZ 
.ZZ 
ThreadContextZZ %
.ZZ% &

PropertiesZZ& 0
[ZZ0 1
$strZZ1 6
]ZZ6 7
=ZZ8 9
objTraceZZ: B
.ZZB C
RequestZZC J
.ZZJ K
UrlZZK N
;ZZN O
log4net[[ 
.[[ 
ThreadContext[[ %
.[[% &

Properties[[& 0
[[[0 1
$str[[1 >
][[> ?
=[[@ A
Convert[[B I
.[[I J
ToString[[J R
([[R S
objTrace[[S [
.[[[ \
Request[[\ c
.[[c d
QueryString[[d o
)[[o p
;[[p q
var\\ 
Cookies\\ 
=\\ 
objTrace\\ &
.\\& '
Request\\' .
.\\. /
Cookies\\/ 6
;\\6 7
if]] 
(]] 
Cookies]] 
!=]] 
null]] #
)]]# $
{^^ 
log4net__ 
.__ 
ThreadContext__ )
.__) *

Properties__* 4
[__4 5
$str__5 :
]__: ;
=__< =
(__> ?
Cookies__? F
[__F G
$str__G L
]__L M
!=__N P
null__Q U
?__V W
Cookies__X _
[___ `
$str__` e
]__e f
.__f g
Value__g l
:__m n
$str__o u
)__u v
;__v w
log4net`` 
.`` 
ThreadContext`` )
.``) *

Properties``* 4
[``4 5
$str``5 ?
]``? @
=``A B
(``C D
Cookies``D K
[``K L
$str``L V
]``V W
!=``X Z
null``[ _
?``` a
Cookies``b i
[``i j
$str``j t
]``t u
.``u v
Value``v {
:``| }
$str	``~ Ñ
)
``Ñ Ö
;
``Ö Ü
}aa 
}bb 
}cc 	
publicvv 
voidvv 
SendMailvv 
(vv 
)vv 
{ww 	
}
ÇÇ 	
}
ÑÑ 
}ÖÖ ã
AD:\work\bikewaleweb\Bikewale.Notifications\JSExceptionTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
{ 
public 

class 
JSExceptionTemplate $
:% &
ComposeEmailBase' 7
{ 
private 
readonly 
JSExceptionEntity *
_error+ 1
=2 3
null4 8
;8 9
public 
JSExceptionTemplate "
(" #
JSExceptionEntity# 4
ex5 7
)7 8
{ 	
_error 
= 
ex 
; 
} 	
private 
readonly 
string 
ErrorMsgBody  ,
=- .
$str	/ ‡
;
‡ ·
public"" 
override"" 
string"" 
ComposeBody"" *
(""* +
)""+ ,
{## 	
string$$ 
_mailBodyText$$  
=$$! "
string$$# )
.$$) *
Empty$$* /
;$$/ 0
try%% 
{&& 
var'' 
	ServerVar'' 
='' 
HttpContext''  +
.''+ ,
Current'', 3
.''3 4
Request''4 ;
.''; <
ServerVariables''< K
;''K L
_mailBodyText(( 
=(( 
string((  &
.((& '
Format((' -
(((- .
ErrorMsgBody((. :
,((: ;
	ServerVar((< E
[((E F
$str((F Q
]((Q R
,((R S
	ServerVar((T ]
[((] ^
$str((^ c
]((c d
,((d e
	ServerVar((f o
[((o p
$str	((p Ñ
]
((Ñ Ö
,
((Ö Ü
	ServerVar
((á ê
[
((ê ë
$str
((ë ü
]
((ü †
,
((† °
_error
((¢ ®
.
((® ©
	ErrorType
((© ≤
,
((≤ ≥
_error
((¥ ∫
.
((∫ ª
Message
((ª ¬
,
((¬ √
	ServerVar
((ƒ Õ
[
((Õ Œ
$str
((Œ ‹
]
((‹ ›
,
((› ﬁ
_error
((ﬂ Â
.
((Â Ê

SourceFile
((Ê 
,
(( Ò
_error
((Ú ¯
.
((¯ ˘
LineNo
((˘ ˇ
,
((ˇ Ä
_error
((Å á
.
((á à
Trace
((à ç
)
((ç é
;
((é è
})) 
catch** 
(** 
	Exception** 
e** 
)** 
{++ 
HttpContext,, 
.,, 
Current,, #
.,,# $
Trace,,$ )
.,,) *
Warn,,* .
(,,. /
$str,,/ a
+,,b c
e,,d e
.,,e f
Message,,f m
),,m n
;,,n o
}-- 
return.. 
_mailBodyText..  
;..  !
}// 	
}11 
}22 ¿.
WD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\BookingCancellationTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public		 

class		 '
BookingCancellationTemplate		 ,
:		- .
ComposeEmailBase		/ ?
{

 
public 
string 
BWId 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
TransactionId !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
CustomerName "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
CustomerEmail #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
CustomerMobile $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
string 
BookingDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 

DealerName  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
BikeName 
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
public '
BookingCancellationTemplate *
(* +
string+ 1
bwId2 6
,6 7
uint8 <
transactionId= J
,J K
stringL R
customerNameS _
,_ `
stringa g
customerEmailh u
,u v
stringw }
customerMobile	~ å
,
å ç
string
é î
bookingDate
ï †
,
† °
string 

dealerName 
, 
string %
bikeName& .
,. /
string/ 5
cityName6 >
)> ?
{ 	
BWId 
= 
bwId 
; 
TransactionId 
= 
transactionId )
;) *
CustomerEmail 
= 
customerEmail )
;) *
CustomerMobile 
= 
customerMobile +
;+ ,
CustomerName 
= 
customerName '
;' (
BookingDate 
= 
bookingDate %
;% &

DealerName 
= 

dealerName #
;# $
BikeName 
= 
bikeName 
;  
CityName   
=   
cityName   
;    
}!! 	
public## 
override## 
string## 
ComposeBody## *
(##* +
)##+ ,
{$$ 	
StringBuilder%% 
sb%% 
=%% 
new%% "
StringBuilder%%# 0
(%%0 1
)%%1 2
;%%2 3
try&& 
{'' 
sb(( 
.(( 
AppendFormat(( 
(((  
$str((  '
+)) 
$str)) 
+** 
$str** "
+++ 
$str++ $
+,,  !
$str,," P
+--  !
$str--" 0
+.. 
$str.. %
+// 
$str// $
+00  !
$str00" B
+11  !
$str11" 0
+22 
$str22 %
+33 
$str33 $
+44  !
$str44" @
+55  !
$str55" 0
+66 
$str66 %
+77 
$str77 $
+88  !
$str88" A
+99  !
$str99" 0
+:: 
$str:: %
+;; 
$str;; $
+<<  !
$str<<" B
+==  !
$str==" 0
+>> 
$str>> %
+?? 
$str?? $
+@@  !
$str@@" C
+AA  !
$strAA" 0
+BB 
$strBB %
+CC 
$strCC $
+DD  !
$strDD" =
+EE  !
$strEE" 0
+FF 
$strFF %
+GG 
$strGG $
+HH  !
$strHH" ?
+II  !
$strII" 0
+JJ 
$strJJ %
+KK 
$strKK %
+LL  !
$strLL" ?
+MM  !
$strMM" 0
+NN 
$strNN %
+OO 
$strOO $
+PP 
$strPP  
+QQ 
$strQQ 
,QQ 
BWIdQQ  
,QQ  !
TransactionIdQQ" /
.QQ/ 0
ToStringQQ0 8
(QQ8 9
)QQ9 :
,QQ: ;
BookingDateQQ< G
,QQG H
CustomerNameQQI U
,QQU V
CustomerEmailQQW d
,QQd e
CustomerMobileQQf t
,QQt u
BikeNameQQv ~
,QQ~ 

DealerName
QQÄ ä
,
QQä ã
CityName
QQã ì
)
QQì î
;
QQî ï
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
$strUU7 a
)UUa b
;UUb c
objErrVV 
.VV 
SendMailVV 
(VV  
)VV  !
;VV! "
}WW 
returnXX 
sbXX 
.XX 
ToStringXX 
(XX 
)XX  
;XX  !
}YY 	
}[[ 
}\\ Â
XD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\CancellationFeedbackTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public 

class (
CancellationFeedbackTemplate -
:- .
ComposeEmailBase/ ?
{ 
	protected 
string 
_bwId 
=  
string! '
.' (
Empty( -
;- .
	protected 
string 
_feedbackText &
=' (
string) /
./ 0
Empty0 5
;5 6
public (
CancellationFeedbackTemplate +
(+ ,
string, 2
bwId3 7
,7 8
string9 ?
feedbackText@ L
)L M
{ 	
_bwId 
= 
bwId 
; 
_feedbackText 
= 
feedbackText (
;( )
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
try 
{ 
StringBuilder 
sb  
=! "
null# '
;' (
sb 
= 
new 
StringBuilder &
(& '
)' (
;( )
sb 
. 
Append 
( 
string  
.  !
Format! '
(' (
$str	( é
,
é è
_bwId
ê ï
,
ï ñ
_feedbackText
ó §
)
§ •
)
• ¶
;
¶ ß
return 
sb 
. 
ToString "
(" #
)# $
;$ %
} 
catch 
{   
return!! 
string!! 
.!! 
Empty!! #
;!!# $
}"" 
}## 	
}$$ 
}%% ¢
\D:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\CustomerRegistrationMailTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{		 
public 

class ,
 CustomerRegistrationMailTemplate 1
:2 3
ComposeEmailBase4 D
{ 
private 
string 

customerId !
,! "
customerName 
, 
password 
; 
public ,
 CustomerRegistrationMailTemplate /
(/ 0
string0 6

customerId7 A
,A B
stringC I
customerNameJ V
,V W
stringX ^
password_ g
)g h
{ 	
this 
. 

customerId 
= 

customerId (
;( )
this 
. 
customerName 
= 
customerName  ,
;, -
this 
. 
password 
= 
password $
;$ %
} 	
public 
override 
string !
ComposeBody" -
(- .
). /
{ 
StringBuilder 
message %
=& '
new( +
StringBuilder, 9
(9 :
): ;
;; <
message 
. 
AppendFormat $
($ %
$str	% è
)
è ê
;
ê ë
message 
. 
AppendFormat $
($ %
$str% 9
,9 :
customerName; G
)G H
;H I
message!! 
.!! 
AppendFormat!! $
(!!$ %
$str!!% F
)!!F G
;!!G H
message## 
.## 
AppendFormat## $
(##$ %
$str##% N
)##N O
;##O P
message$$ 
.$$ 
AppendFormat$$ $
($$$ %
$str$$% u
)$$u v
;$$v w
message%% 
.%% 
AppendFormat%% $
(%%$ %
$str%%% c
)%%c d
;%%d e
message&& 
.&& 
AppendFormat&& $
(&&$ %
$str&&% I
)&&I J
;&&J K
message'' 
.'' 
AppendFormat'' $
(''$ %
$str	''% ä
)
''ä ã
;
''ã å
message(( 
.(( 
AppendFormat(( $
((($ %
$str((% q
)((q r
;((r s
message** 
.** 
AppendFormat** $
(**$ %
$str**% 9
,**9 :

customerId**; E
)**E F
;**F G
message++ 
.++ 
AppendFormat++ $
(++$ %
$str++% 9
,++9 :
password++; C
)++C D
;++D E
message-- 
.-- 
AppendFormat-- $
(--$ %
$str	--% ≠
)
--≠ Æ
;
--Æ Ø
message// 
.// 
AppendFormat// $
(//$ %
$str//% @
)//@ A
;//A B
message00 
.00 
AppendFormat00 $
(00$ %
$str00% H
)00H I
;00I J
return22 
message22 
.22 
ToString22 '
(22' (
)22( )
;22) *
}33 
}77 
}88 ¨
MD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\ErrorMailTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public 

class 
ErrorMailTemplate "
:# $
ComposeEmailBase% 5
{ 
public 
	Exception 
err 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
PageUrl 
{ 
get  #
;# $
set% (
;( )
}* +
public 
ErrorMailTemplate  
(  !
	Exception! *
ex+ -
,- .
string/ 5
pageUrl6 =
)= >
{ 	
err 
= 
ex 
; 
PageUrl 
= 
pageUrl 
; 
} 	
private 
static 
readonly 
string  &
ErrorMessageBody' 7
=8 9
$str	: ﬂ
;
ﬂ ‡
public"" 
override"" 
string"" 
ComposeBody"" *
(""* +
)""+ ,
{## 	
string%% 
	retString%% 
=%% 
string%% %
.%%% &
Empty%%& +
;%%+ ,
try&& 
{'' 
var(( 
	ServerVar(( 
=(( 
HttpContext((  +
.((+ ,
Current((, 3
.((3 4
Request((4 ;
.((; <
ServerVariables((< K
;((K L
	retString)) 
=)) 
string)) "
.))" #
Format))# )
())) *
ErrorMessageBody))* :
,)): ;
	ServerVar))< E
[))E F
$str))F Q
]))Q R
,))R S
	ServerVar))T ]
[))] ^
$str))^ c
]))c d
,))d e
	ServerVar))f o
[))o p
$str	))p Ñ
]
))Ñ Ö
,
))Ö Ü
	ServerVar
))á ê
[
))ê ë
$str
))ë ü
]
))ü †
,
))† °
	ServerVar
))¢ ´
[
))´ ¨
$str
))¨ º
]
))º Ω
,
))Ω æ
	ServerVar** 
[** 
$str** )
]**) *
,*** +
PageUrl**, 3
,**3 4
err**5 8
.**8 9
Message**9 @
,**@ A
err**B E
.**E F
InnerException**F T
,**T U
err**V Y
.**Y Z

StackTrace**Z d
)**d e
;**e f
}++ 
catch,, 
(,, 
	Exception,, 
ex,, 
),,  
{-- 
HttpContext.. 
... 
Current.. #
...# $
Trace..$ )
...) *
Warn..* .
(... /
$str../ Z
+..[ \
ex..] _
..._ `
Message..` g
)..g h
;..h i
}// 
return00 
	retString00 
;00 
}11 	
}33 
}44 ó
JD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\FeedbackMailer.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{		 
public

 

class

 
FeedbackMailer

 
:

  !
ComposeEmailBase

" 2
{ 
public 
string 
PageUrl 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
Feedback 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
FeedbackMailer 
( 
string $
pageUrl% ,
,, -
string- 3
feedback4 <
)< =
{ 	
PageUrl 
= 
pageUrl 
; 
Feedback 
= 
feedback 
;  
} 	
static 
readonly 
string 
	_MailBody (
=) *
$str	+ •
;
• ¶
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
return
 
string 
. 
Format 
( 
	_MailBody (
,( )
DateTime* 2
.2 3
Now3 6
.6 7
ToString7 ?
(? @
$str@ N
)N O
,O P
PageUrlQ X
,X Y
FeedbackZ b
)b c
;c d
}// 	
}00 
}11 ù
VD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\MakeMaskingNameChangedMail.cs
	namespace

 	
Bikewale


 
.

 
Notifications

  
.

  !
MailTemplates

! .
{ 
public 

class &
MakeMaskingNameChangedMail +
:, -
ComposeEmailBase. >
{ 
private 
IEnumerable 
< 
BikeModelMailEntity /
>/ 0
models1 7
;7 8
private 
string 
makeName 
;  
public &
MakeMaskingNameChangedMail )
() *
string* 0
makeName1 9
,9 :
IEnumerable; F
<F G
BikeModelMailEntityG Z
>Z [
models\ b
)b c
{ 	
this 
. 
makeName 
= 
makeName $
;$ %
this 
. 
models 
= 
models  
;  !
} 	
public   
override   
string   
ComposeBody   *
(  * +
)  + ,
{!! 	
StringBuilder"" 
sb"" 
="" 
new"" "
StringBuilder""# 0
(""0 1
)""1 2
;""2 3
sb## 
.## 
AppendFormat## 
(## 
$str	## ß
)
##ß ®
;
##® ©
int%% 
i%% 
=%% 
$num%% 
;%% 
if&& 
(&& 
models&& 
!=&& 
null&& 
)&& 
sb// 
.// 
AppendFormat// 
(// 
$str// .
)//. /
;/// 0
return00 
sb00 
.00 
ToString00 
(00 
)00  
;00  !
}11 	
}22 
}33 ›
WD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\ModelMaskingNameChangedMail.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public 

class '
ModelMaskingNameChangedMail ,
:- .
ComposeEmailBase/ ?
{ 
private	 
string 
makeName 
, 
	modelName 
, 
oldUrl 
, 
newUrl 
; 
public	 '
ModelMaskingNameChangedMail +
(+ ,
string, 2
makeName3 ;
,; <
string= C
	modelNameD M
,M N
stringO U
oldUrlV \
,\ ]
string^ d
newUrle k
)k l
{ 	
this 
. 
makeName 
= 
makeName $
;$ %
this 
. 
	modelName 
= 
	modelName &
;& '
this 
. 
oldUrl 
= 
oldUrl  
;  !
this 
. 
newUrl 
= 
newUrl  
;  !
} 	
public"" 
override"" 
string"" 
ComposeBody"" *
(""* +
)""+ ,
{## 	
StringBuilder$$ 
sb$$ 
=$$ 
new$$ "
StringBuilder$$# 0
($$0 1
)$$1 2
;$$2 3
sb%% 
.%% 
AppendFormat%% 
(%% 
$str%% S
,%%S T
makeName%%U ]
,%%] ^
	modelName%%^ g
,%%g h
oldUrl%%h n
,%%n o
newUrl%%o u
)%%u v
;%%v w
return&& 
sb&& 
.&& 
ToString&& 
(&& 
)&&  
;&&  !
}'' 	
}(( 
})) ¥
QD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\ModelDiscontinuedMail.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public 

class !
ModelDiscontinuedMail &
:' (
ComposeEmailBase) 9
{ 
private 
string 
makeName 
, 
	modelName 
, 
date 
; 
public !
ModelDiscontinuedMail $
($ %
string% +
makeName, 4
,4 5
string6 <
	modelName= F
,F G
DateTimeG O
dateP T
)T U
{ 	
this 
. 
makeName 
= 
makeName $
;$ %
this 
. 
	modelName 
= 
	modelName &
;& '
this 
. 
date 
= 
date 
. 
ToString %
(% &
$str& 3
)3 4
;4 5
} 	
public   
override   
string   
ComposeBody   *
(  * +
)  + ,
{!! 	
StringBuilder"" 
sb"" 
="" 
new"" "
StringBuilder""# 0
(""0 1
)""1 2
;""2 3
sb## 
.## 
AppendFormat## 
(## 
$str## U
,##U V
makeName##W _
,##_ `
	modelName##` i
,##i j
date##j n
)##n o
;##o p
return$$ 
sb$$ 
.$$ 
ToString$$ 
($$ 
)$$  
;$$  !
}%% 	
}&& 
}'' Œ
UD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\ModelSoldUnitMailTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public

 

class

 %
ModelSoldUnitMailTemplate

 *
:

+ ,
ComposeEmailBase

- =
{ 
private 
string !
ModelSoldUnitMailHtml ,
=- .
null/ 3
;3 4
public %
ModelSoldUnitMailTemplate (
(( )
string) /
customerName0 <
,< =
DateTime> F
dateG K
)K L
{ 	
try 
{ 
StringBuilder 
message %
=& '
new( +
StringBuilder, 9
(9 :
): ;
;; <
message 
. 
Append 
( 
$str *
++ ,
customerName- 9
+: ;
$str< D
)D E
;E F
message 
. 
Append 
( 
$str M
+N O
dateP T
+U V
$strW ]
)] ^
;^ _
message 
. 
AppendFormat $
($ %
$str% z
,z {
Bikewale	| Ñ
.
Ñ Ö
Utility
Ö å
.
å ç 
BWOprConfiguration
ç ü
.
ü †
Instance
† ®
.
® ©
BwOprHostUrlForJs
© ∫
)
∫ ª
;
ª º!
ModelSoldUnitMailHtml %
=& '
message( /
./ 0
ToString0 8
(8 9
)9 :
;: ;
} 
catch 
( 
	Exception 
err  
)  !
{ 
Bikewale 
. 
Notifications &
.& '

ErrorClass' 1
objErr2 8
=9 :
new; >
Bikewale? G
.G H
NotificationsH U
.U V

ErrorClassV `
(` a
erra d
,d e
$str	f õ
)
õ ú
;
ú ù
} 
} 	
public!! 
override!! 
string!! 
ComposeBody!! *
(!!* +
)!!+ ,
{"" 	
return## !
ModelSoldUnitMailHtml## (
;##( )
}$$ 	
}%% 
}&& ⁄:
aD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\NewBikePriceQuoteMailToDealerTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public 

class 1
%NewBikePriceQuoteMailToDealerTemplate 6
:7 8
ComposeEmailBase9 I
{ 
private 
string 
MailHTML 
=  !
null" &
;& '
public!! 1
%NewBikePriceQuoteMailToDealerTemplate!! 4
(!!4 5
string"" 
	makeModel"" 
,"" 
string"" $
versionName""% 0
,""0 1
string""2 8

dealerName""9 C
,""C D
string""E K
customerName""L X
,""X Y
string## 
customerEmail##  
,##  !
string##" (
customerMobile##) 7
,##7 8
string##9 ?
areaName##@ H
,##H I
string##J P
cityName##Q Y
,##Y Z
List$$ 
<$$ 
Entities$$ 
.$$ 
BikeBooking$$ %
.$$% &
PQ_Price$$& .
>$$. /
	priceList$$0 9
,$$9 :
int$$; >

totalPrice$$? I
,$$I J
List$$K O
<$$O P
Entities$$P X
.$$X Y
BikeBooking$$Y d
.$$d e
OfferEntity$$e p
>$$p q
	offerList$$r {
,$${ |
string	$$} É
	imagePath
$$Ñ ç
)
$$ç é
{%% 	
StringBuilder&& 
mail&& 
=&&  
new&&! $
StringBuilder&&% 2
(&&2 3
)&&3 4
;&&4 5
areaName'' 
='' 
string'' 
.'' 
IsNullOrEmpty'' +
(''+ ,
areaName'', 4
)''4 5
?''6 7
cityName''8 @
:''A B
areaName''C K
+''L M
$str''N R
+''S T
cityName''U ]
;''] ^
try(( 
{)) 
mail** 
.** 
AppendFormat** !
(**! "
$str	**" ¥
,++ 
DateTime++ 
.++ 
Now++ "
.++" #
ToString++# +
(+++ ,
$str++, :
)++: ;
,,, 

dealerName,,  
,-- 
	makeModel-- 
,.. 
versionName.. !
,// 
customerName// "
,00 
(00 
!00 
String00 
.00 
IsNullOrEmpty00 +
(00+ ,
customerEmail00, 9
)009 :
?00: ;
String00; A
.00A B
Format00B H
(00H I
$str	00I ß
,
00ß ®
customerEmail
00© ∂
)
00∂ ∑
:
00∏ π
$str
00∫ º
)
00º Ω
,11 
customerMobile11 $
,22 
areaName22 
,33 
	imagePath33 
)44 
;44 
if66 
(66 
	priceList66 
!=66  
null66! %
&&66& (
	priceList66) 2
.662 3
Count663 8
>669 :
$num66; <
)66< =
{77 
foreach88 
(88 
var88  
list88! %
in88& (
	priceList88) 2
)882 3
{99 
mail:: 
.:: 
AppendFormat:: )
(::) *
$str	::* §
,;; 
list;; "
.;;" #
CategoryName;;# /
,<< 
Utility<< %
.<<% &
Format<<& ,
.<<, -
FormatPrice<<- 8
(<<8 9
Convert<<9 @
.<<@ A
ToString<<A I
(<<I J
list<<J N
.<<N O
Price<<O T
)<<T U
)<<U V
)== 
;== 
}>> 
if?? 
(?? 
	offerList?? !
!=??" $
null??% )
&&??* ,
	offerList??- 6
.??6 7
Count??7 <
>??= >
$num??? @
)??@ A
{@@ 
ListAA 
<AA 
PQ_PriceAA %
>AA% &
discountListAA' 3
=AA4 5
OfferHelperAA6 A
.AAA B#
ReturnDiscountPriceListAAB Y
(AAY Z
	offerListAAZ c
,AAc d
	priceListAAe n
)AAn o
;AAo p
ifBB 
(BB 
discountListBB (
!=BB) +
nullBB, 0
&&BB1 3
discountListBB4 @
.BB@ A
CountBBA F
>BBG H
$numBBI J
)BBJ K
{CC 
foreachDD #
(DD$ %
varDD% (
listDD) -
inDD. 0
discountListDD1 =
)DD= >
{EE 
mailFF  $
.FF$ %
AppendFormatFF% 1
(FF1 2
$str	FF2 ¨
,GG$ %
listGG& *
.GG* +
CategoryNameGG+ 7
,HH$ %
UtilityHH& -
.HH- .
FormatHH. 4
.HH4 5
FormatPriceHH5 @
(HH@ A
ConvertHHA H
.HHH I
ToStringHHI Q
(HHQ R
listHHR V
.HHV W
PriceHHW \
)HH\ ]
)HH] ^
)II$ %
;II% &
}JJ 
}KK 
}LL 
mailMM 
.MM 
AppendFormatMM %
(MM% &
$str	MM& °
,NN$ %
FormatNN& ,
.NN, -
FormatPriceNN- 8
(NN8 9
ConvertNN9 @
.NN@ A
ToStringNNA I
(NNI J

totalPriceNNJ T
)NNT U
)NNU V
)OO$ %
;OO% &
ifPP 
(PP 
	offerListPP !
!=PP" $
nullPP% )
&&PP* ,
	offerListPP- 6
.PP6 7
CountPP7 <
>PP= >
$numPP? @
)PP@ A
{QQ 
mailRR 
.RR 
AppendRR #
(RR# $
$str	RR$ ç
)
RRç é
;
RRé è
foreachSS 
(SS  !
varSS! $
listSS% )
inSS* ,
	offerListSS- 6
)SS6 7
{TT 
mailUU  
.UU  !
AppendFormatUU! -
(UU- .
$str	UU. Ï
,VV  !
listVV" &
.VV& '
OfferCategoryIdVV' 6
,WW  !
listWW" &
.WW& '
	OfferTextWW' 0
)WW0 1
;WW1 2
}XX 
}YY 
}ZZ 
mail\\ 
.\\ 
AppendFormat\\ !
(\\! "
$str	\\" ∂
)
\\∂ ∑
;
\\∑ ∏
}]] 
catch^^ 
(^^ 
	Exception^^ 
ex^^ 
)^^  
{__ 
Bikewale`` 
.`` 
Notifications`` &
.``& '

ErrorClass``' 1
objErr``2 8
=``9 :
new``; >
Bikewale``? G
.``G H
Notifications``H U
.``U V

ErrorClass``V `
(``` a
ex``a c
,``c d
$str	``e Æ
)
``Æ Ø
;
``Ø ∞
objErraa 
.aa 
SendMailaa 
(aa  
)aa  !
;aa! "
}bb 
MailHTMLcc 
=cc 
mailcc 
.cc 
ToStringcc $
(cc$ %
)cc% &
;cc& '
}dd 	
publickk 
overridekk 
stringkk 
ComposeBodykk *
(kk* +
)kk+ ,
{ll 	
returnmm 
MailHTMLmm 
;mm 
}nn 	
}pp 
}qq Ä]
eD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\NewBikePriceQuoteMailToDealerTemplate_old.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public 

class 5
)NewBikePriceQuoteMailToDealerTemplate_old :
:; <
ComposeEmailBase= M
{ 
private 
string 
MailHTML 
=  !
null" &
;& '
public 5
)NewBikePriceQuoteMailToDealerTemplate_old 8
(8 9
string9 ?
makeName@ H
,H I
stringJ P
	modelNameQ Z
,Z [
string\ b

dealerNamec m
,m n
stringo u
customerName	v Ç
,
Ç É
string
Ñ ä
customerEmail
ã ò
,
ò ô
string
ö †
customerMobile
° Ø
,
Ø ∞
string 
areaName 
, 
string #
cityName$ ,
,, -
List. 2
<2 3
PQ_Price3 ;
>; <
	priceList= F
,F G
intH K

totalPriceL V
,V W
ListX \
<\ ]
OfferEntity] h
>h i
	offerListj s
,s t
DateTimeu }
date	~ Ç
,
Ç É
string
Ñ ä
	imagePath
ã î
,
î ï
uint
ñ ö
insuranceAmount
õ ™
=
´ ¨
$num
≠ Æ
)
Æ Ø
{ 	
List 
< 
PQ_Price 
> 
discountList '
=( )
OfferHelper* 5
.5 6#
ReturnDiscountPriceList6 M
(M N
	offerListN W
,W X
	priceListY b
)b c
;c d
StringBuilder 
sb 
= 
null #
;# $
try 
{ 
sb 
= 
new 
StringBuilder &
(& '
)' (
;( )
sb 
. 
AppendFormat 
(  
$str	 ”
+ 
$str R
+ 
$str L
+  !
$str	" ‡
+   
$str   &
+!! 
$str!! b
+"" 
$str"" &
+## 
$str## A
+$$ 
$str$$ "
+&& 
$str&& O
+'' 
$str'' i
+(( 
$str(( b
+)) 
$str)) r
+** 
$str** J
+++ 
$str++ "
+-- 
$str	-- ã
+.. 
$str	.. ´
+// 
$str// ;
+00  !
$str00" `
+11$ %
$str11& S
+22$ %
$str22& 9
+33  !
$str33" *
+44  !
$str44" X
+55$ %
$str55& W
+66$ %
$str66& 9
+77  !
$str77" *
+88  !
$str88" `
+99$ %
$str99& Y
+::$ %
$str::& 9
+;;  !
$str;;" *
+<<  !
$str<<" i
+==$ %
$str==& W
+>>$ %
$str>>& 9
+??  !
$str??" *
+@@  !
$str@@" E
+AA 
$strAA &
+BB 
$strBB "
+CC 
$strCC P
+DD 
$str	DD û
+EE 
$strEE u
+FF  !
$str	FF" Ç
+GG$ %
$strGG& f
+HH( )
$strHH* t
+II$ %
$strII& .
+JJ  !
$strJJ" *
+KK  !
$str	KK" ñ
,LL 
dateLL 
.LL 
ToStringLL #
(LL# $
$strLL$ 2
)LL2 3
,LL3 4

dealerNameLL5 ?
.LL? @
TrimLL@ D
(LLD E
)LLE F
,LLF G
makeNameLLH P
+LLQ R
$strLLS V
+LLW X
	modelNameLLY b
,LLb c
customerNameLLd p
,LLp q
customerEmailLLr 
,	LL Ä
customerMobile
LLÅ è
,
LLè ê
areaName
LLë ô
+
LLö õ
$str
LLú †
+
LL° ¢
cityName
LL£ ´
,
LL´ ¨
	imagePath
LL≠ ∂
)
LL∂ ∑
;
LL∑ ∏
ifNN 
(NN 
	priceListNN 
!=NN  
nullNN! %
&&NN& (
	priceListNN) 2
.NN2 3
CountNN3 8
>NN9 :
$numNN; <
)NN< =
{OO 
foreachPP 
(PP 
varPP  
listPP! %
inPP& (
	priceListPP) 2
)PP2 3
{QQ 
sbRR 
.RR 
AppendFormatRR '
(RR' (
$strSS B
+TT  !
$strTT" b
+UU  !
$str	UU" å
+VV  !
$strVV" E
+WW 
$strWW &
,XX 
listXX "
.XX" #
CategoryNameXX# /
,XX/ 0
FormatXX1 7
.XX7 8
FormatPriceXX8 C
(XXC D
listXXD H
.XXH I
PriceXXI N
.XXN O
ToStringXXO W
(XXW X
)XXX Y
)XXY Z
)XXZ [
;XX[ \
}YY 
ifZZ 
(ZZ 
discountListZZ $
!=ZZ% '
nullZZ( ,
&&ZZ- /
discountListZZ0 <
.ZZ< =
CountZZ= B
>ZZC D
$numZZE F
)ZZF G
{[[ 
sb\\ 
.\\ 
AppendFormat\\ '
(\\' (
$str]] [
+^^  !
$str^^" r
+__  !
$str	__" ¡
+``  !
$str``" E
+aa 
$straa &
,bb 
Formatbb $
.bb$ %
FormatPricebb% 0
(bb0 1

totalPricebb1 ;
.bb; <
ToStringbb< D
(bbD E
)bbE F
)bbF G
)cc 
;cc 
foreachdd 
(dd  !
vardd! $
listdd% )
indd* ,
discountListdd- 9
)dd9 :
{ee 
sbff 
.ff 
AppendFormatff +
(ff+ ,
$strgg  F
+hh$ %
$strhh& f
+ii$ %
$str	ii& ê
+jj$ %
$strjj& I
+kk  !
$strkk" *
,ll 
listll "
.ll" #
CategoryNamell# /
,ll/ 0
Formatll1 7
.ll7 8
FormatPricell8 C
(llC D
listllD H
.llH I
PricellI N
.llN O
ToStringllO W
(llW X
)llX Y
)llY Z
)llZ [
;ll[ \
}mm 
}nn 
sboo 
.oo 
AppendFormatoo #
(oo# $
$strpp >
+qq 
$strqq q
+rr 
$str	rr à
+ss 
$strss A
+tt 
$strtt "
,uu 
Formatuu  
.uu  !
FormatPriceuu! ,
(uu, -
Convertuu- 4
.uu4 5
ToStringuu5 =
(uu= >

totalPriceuu> H
-uuI J 
TotalDiscountedPriceuuK _
(uu_ `
discountListuu` l
)uul m
)uum n
)uun o
)vv 
;vv 
}ww 
sbyy 
.yy 
AppendFormatyy 
(yy  
$strzz  (
+{{ 
$str{{ F
+|| 
$str|| "
)}} 
;}} 
if
ÄÄ 
(
ÄÄ 
	offerList
ÄÄ 
!=
ÄÄ  
null
ÄÄ! %
&&
ÄÄ& (
	offerList
ÄÄ) 2
.
ÄÄ2 3
Count
ÄÄ3 8
>
ÄÄ9 :
$num
ÄÄ; <
)
ÄÄ< =
{
ÅÅ 
sb
ÇÇ 
.
ÇÇ 
AppendFormat
ÇÇ #
(
ÇÇ# $
$str
ÉÉ }
+
ÑÑ 
$strÑÑ æ
+
ÖÖ 
$str
ÖÖ K
)
ÜÜ 
;
ÜÜ 
foreach
áá 
(
áá 
var
áá  
offer
áá! &
in
áá' )
	offerList
áá* 3
)
áá3 4
{
àà 
sb
ââ 
.
ââ 
AppendFormat
ââ '
(
ââ' (
$str
ââ( U
,
ââU V
offer
ââW \
.
ââ\ ]
	OfferText
ââ] f
)
ââf g
;
ââg h
}
ää 
sb
ãã 
.
ãã 
AppendFormat
ãã #
(
ãã# $
$str
åå #
+
çç 
$str
çç ?
)
éé 
;
éé 
}
èè 
sb
êê 
.
êê 
AppendFormat
êê 
(
êê  
$strëë Ä
+
íí 
$stríí ß
+
ìì 
$str
ìì v
+
îî  !
$strîî" Ö
+
ïï$ %
$str
ïï& K
+
ññ( )
$strññ* ∏
+
óó$ %
$str
óó& .
+
òò$ %
$str
òò& e
+
ôô  !
$str
ôô" *
+
öö  !
$ströö" Ö
+
õõ$ %
$str
õõ& K
+
úú' (
$strúú) Æ
+
ùù$ %
$str
ùù& .
+
ûû$ %
$str
ûû& `
+
üü  !
$str
üü" *
+
††  
$str††! Ö
+
°°$ %
$str
°°& K
+
¢¢( )
$str¢¢* ∏
+
££$ %
$str
££& .
+
§§$ %
$str
§§& e
+
••  !
$str
••" *
+
¶¶ 
$str
¶¶ &
+
ßß 
$str
ßß A
+
©© 
$str©© Ä
+
™™ 
$str™™ É
+
´´ 
$str
´´ d
+
¨¨ 
$str
¨¨ "
+
≠≠ 
$str
≠≠ a
+
ÆÆ 
$strÆÆ ®
+
ØØ 
$strØØ £
+
∞∞ 
$str
∞∞ "
+
±± 
$str
±± 
)
≤≤ 
;
≤≤ 
}
≥≥ 
catch
¥¥ 
(
¥¥ 
	Exception
¥¥ 
ex
¥¥ 
)
¥¥  
{
µµ 
Bikewale
∂∂ 
.
∂∂ 
Notifications
∂∂ &
.
∂∂& '

ErrorClass
∂∂' 1
objErr
∂∂2 8
=
∂∂9 :
new
∂∂; >
Bikewale
∂∂? G
.
∂∂G H
Notifications
∂∂H U
.
∂∂U V

ErrorClass
∂∂V `
(
∂∂` a
ex
∂∂a c
,
∂∂c d
$str∂∂e Æ
)∂∂Æ Ø
;∂∂Ø ∞
objErr
∑∑ 
.
∑∑ 
SendMail
∑∑ 
(
∑∑  
)
∑∑  !
;
∑∑! "
}
∏∏ 
MailHTML
ππ 
=
ππ 
sb
ππ 
.
ππ 
ToString
ππ "
(
ππ" #
)
ππ# $
;
ππ$ %
}
∫∫ 	
public
ºº 
override
ºº 
string
ºº 
ComposeBody
ºº *
(
ºº* +
)
ºº+ ,
{
ΩΩ 	
return
ææ 
MailHTML
ææ 
;
ææ 
}
øø 	
	protected
∆∆ 
UInt32
∆∆ "
TotalDiscountedPrice
∆∆ -
(
∆∆- .
List
∆∆. 2
<
∆∆2 3
PQ_Price
∆∆3 ;
>
∆∆; <
discountList
∆∆= I
)
∆∆I J
{
«« 	
UInt32
»» 

totalPrice
»» 
=
»» 
$num
»»  !
;
»»! "
if
…… 
(
…… 
discountList
…… 
!=
…… 
null
……  $
&&
……% '
discountList
……( 4
.
……4 5
Count
……5 :
>
……; <
$num
……= >
)
……> ?
{
   
foreach
ÀÀ 
(
ÀÀ 
var
ÀÀ 
priceListObj
ÀÀ )
in
ÀÀ* ,
discountList
ÀÀ- 9
)
ÀÀ9 :
{
ÃÃ 

totalPrice
ÕÕ 
+=
ÕÕ !
priceListObj
ÕÕ" .
.
ÕÕ. /
Price
ÕÕ/ 4
;
ÕÕ4 5
}
ŒŒ 
}
œœ 
return
–– 

totalPrice
–– 
;
–– 
}
—— 	
}
““ 
}”” Ñ8
_D:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\NewBikePriceQuoteToCustomerTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{		 
public 

class /
#NewBikePriceQuoteToCustomerTemplate 4
:5 6
ComposeEmailBase7 G
{ 
private 
StringBuilder 
mail "
=# $
null% )
;) *
public /
#NewBikePriceQuoteToCustomerTemplate 2
(2 3
string3 9
bikeName: B
,B C
stringD J
versionNameK V
,V W
stringX ^
	bikeImage_ h
,h i
stringj p
dealerEmailIdq ~
,~ 
string
Ä Ü
dealerMobileNo
á ï
,
ï ñ
string 
organization 
,  
string! '
address( /
,/ 0
string1 7
customerName8 D
,D E
ListF J
<J K
PQ_PriceK S
>S T
	priceListU ^
,^ _
List` d
<d e
OfferEntitye p
>p q
	offerListr {
,{ |
string 
pinCode 
, 
string "
	stateName# ,
,, -
string. 4
cityName5 =
,= >
uint? C

totalPriceD N
, 
double 
	dealerLat 
, 
double  &

dealerLong' 1
,1 2
string3 9
workingHours: F
)F G
{ 	
List 
< 
PQ_Price 
> 
discountList '
=( )
OfferHelper* 5
.5 6#
ReturnDiscountPriceList6 M
(M N
	offerListN W
,W X
	priceListY b
)b c
;c d
mail 
= 
new 
StringBuilder $
($ %
)% &
;& '
mail 
. 
AppendFormat 
( 
$str	 ˛
, 
DateTime 
. 
Now 
. 
ToString '
(' (
$str( 6
)6 7
,7 8
customerName9 E
,E F
bikeNameG O
,O P
	bikeImageQ Z
,Z [
versionName\ g
)g h
;h i
if 
( 
	priceList 
!= 
null !
&&" $
	priceList% .
.. /
Count/ 4
>5 6
$num7 8
)8 9
{ 
mail 
. 
Append 
( 
$str R
)R S
;S T
foreach   
(   
var   
plist   "
in  # %
	priceList  & /
)  / 0
{!! 
mail"" 
."" 
AppendFormat"" %
(""% &
$str	""& û
,## 
plist## 
.##  
CategoryName##  ,
,##, -
Format##. 4
.##4 5
FormatPrice##5 @
(##@ A
Convert##A H
.##H I
ToString##I Q
(##Q R
plist##R W
.##W X
Price##X ]
)##] ^
)##^ _
)##_ `
;##` a
}$$ 
if%% 
(%% 
discountList%%  
!=%%! #
null%%$ (
&&%%) +
discountList%%, 8
.%%8 9
Count%%9 >
>%%? @
$num%%A B
)%%B C
{&& 
mail'' 
.'' 
Append'' 
(''  
$str''  V
)''V W
;''W X
foreach(( 
((( 
var((  
dlist((! &
in((' )
discountList((* 6
)((6 7
{)) 
mail** 
.** 
AppendFormat** )
(**) *
$str	*** ¢
,++ 
dlist++ #
.++# $
CategoryName++$ 0
,++0 1
Format++2 8
.++8 9
FormatPrice++9 D
(++D E
Convert++E L
.++L M
ToString++M U
(++U V
dlist++V [
.++[ \
Price++\ a
)++a b
)++b c
)++c d
;++d e
},, 
}-- 
mail.. 
... 
AppendFormat.. !
(..! "
$str	.." Ë
,// 
Format// 
.// 
FormatPrice// (
(//( )
Convert//) 0
.//0 1
ToString//1 9
(//9 :

totalPrice//: D
)//D E
)//E F
)//F G
;//G H
}00 
mail33 
.33 
AppendFormat33 
(33 
$str	33 ª
,44 
organization44 
,44 
address44  '
,44' (
cityName44) 1
,441 2
	stateName443 <
,44< =
pinCode44> E
,44E F
dealerMobileNo44G U
,44U V
dealerEmailId44W d
)44d e
;44e f
if66 
(66 
!66 
string66 
.66 
IsNullOrEmpty66 %
(66% &
workingHours66& 2
)662 3
)663 4
{77 
mail88 
.88 
AppendFormat88 !
(88! "
$str88" j
,88j k
workingHours88l x
)88x y
;88y z
}99 
mail;; 
.;; 
AppendFormat;; 
(;; 
$str	;; ı
,<< 
	dealerLat<< 
,<< 

dealerLong<< '
)<<' (
;<<( )
if>> 
(>> 
	offerList>> 
!=>> 
null>> !
&&>>" $
	offerList>>% .
.>>. /
Count>>/ 4
(>>4 5
)>>5 6
>>>7 8
$num>>9 :
)>>: ;
{?? 
mail@@ 
.@@ 
Append@@ 
(@@ 
$str	@@ —
)
@@— “
;
@@“ ”
foreachAA 
(AA 
varAA 
offerAA "
inAA# %
	offerListAA& /
)AA/ 0
{BB 
mailCC 
.CC 
AppendFormatCC %
(CC% &
$str	CC& ¢
,
CC¢ £
offerDD 
.DD 
	OfferTextDD '
,DD' (
offerDD) .
.DD. /
OfferCategoryIdDD/ >
)DD> ?
;DD? @
}EE 
mailFF 
.FF 
AppendFF 
(FF 
$strFF +
)FF+ ,
;FF, -
}GG 
mailHH 
.HH 
AppendHH 
(HH 
$str	HH â
)
HHâ ä
;
HHä ã
}II 	
publicKK 
overrideKK 
stringKK 
ComposeBodyKK *
(KK* +
)KK+ ,
{LL 	
returnMM 
ConvertMM 
.MM 
ToStringMM #
(MM# $
mailMM$ (
)MM( )
;MM) *
}NN 	
}OO 
}PP ﬁ+
WD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\OfferClaimAlertNotification.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public 

class '
OfferClaimAlertNotification ,
:- .
ComposeEmailBase/ ?
{ 
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
Helmet 
{ 
get "
;" #
set$ '
;' (
}) *
public 
RSAOfferClaimEntity "
OfferEntity# .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public '
OfferClaimAlertNotification *
(* +
RSAOfferClaimEntity+ >
offerEntity? J
,J K
stringL R
bikeNameS [
,[ \
string] c
helmetd j
)j k
{ 	
this 
. 
OfferEntity 
= 
offerEntity *
;* +
this 
. 
BikeName 
= 
bikeName $
;$ %
this 
. 
Helmet 
= 
helmet  
;  !
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
StringBuilder 
sb 
= 
new "
StringBuilder# 0
(0 1
)1 2
;2 3
string 

otherOffer 
= 
string  &
.& '
Empty' ,
;, -
sb 
. 
Append 
( 
$str "
)" #
;# $
sb 
. 
Append 
( 
$str y
)y z
;z {
sb 
. 
Append 
( 
$str .
). /
;/ 0
sb 
. 

AppendLine 
( 
$str  
)  !
;! "
sb   
.   
AppendFormat   
(   
$str   *
,  * +
$str  , G
)  G H
;  H I
if!! 
(!! 
!!! 
string!! 
.!! 
IsNullOrWhiteSpace!! *
(!!* +
Helmet!!+ 1
)!!1 2
)!!2 3
sb"" 
."" 
AppendFormat"" 
(""  
$str""  .
,"". /
Helmet""0 6
)""6 7
;""7 8
sb## 
.## 
Append## 
(## 
$str## 
)## 
;## 
sb$$ 
.$$ 
Append$$ 
($$ 
$str$$ :
)$$: ;
;$$; <
sb%% 
.%% 
AppendFormat%% 
(%% 
$str%% 5
,%%5 6
OfferEntity%%7 B
.%%B C
CustomerName%%C O
)%%O P
;%%P Q
sb&& 
.&& 
AppendFormat&& 
(&& 
$str&& 8
,&&8 9
OfferEntity&&: E
.&&E F
CustomerMobile&&F T
)&&T U
;&&U V
sb'' 
.'' 
AppendFormat'' 
('' 
$str'' 7
,''7 8
OfferEntity''9 D
.''D E
CustomerAddress''E T
)''T U
;''U V
sb(( 
.(( 
AppendFormat(( 
((( 
$str(( 6
,((6 7
BikeName((8 @
)((@ A
;((A B
sb)) 
.)) 
AppendFormat)) 
()) 
$str)) ;
,)); <
OfferEntity))= H
.))H I
BikeRegistrationNo))I [
)))[ \
;))\ ]
sb** 
.** 
AppendFormat** 
(** 
$str** <
,**< =
OfferEntity**> I
.**I J
DealerAddress**J W
)**W X
;**X Y
sb++ 
.++ 
AppendFormat++ 
(++ 
$str++ 5
,++5 6
OfferEntity++7 B
.++B C
DeliveryDate++C O
.++O P
ToShortDateString++P a
(++a b
)++b c
)++c d
;++d e
sb,, 
.,, 
AppendFormat,, 
(,, 
$str,, 4
,,,4 5
OfferEntity,,6 A
.,,A B
Comments,,B J
),,J K
;,,K L
sb-- 
.-- 
Append-- 
(-- 
$str-- 0
)--0 1
;--1 2
sb.. 
... 
AppendFormat.. 
(.. 
$str.. 3
,..3 4
OfferEntity..5 @
...@ A

DealerName..A K
)..K L
;..L M
sb// 
.// 
AppendFormat// 
(// 
$str// 6
,//6 7
OfferEntity//8 C
.//C D
DealerAddress//D Q
)//Q R
;//R S
sb00 
.00 
Append00 
(00 
$str00 *
)00* +
;00+ ,
sb11 
.11 
Append11 
(11 
$str11 *
)11* +
;11+ ,
return22 
sb22 
.22 
ToString22 
(22 
)22  
;22  !
}33 	
}44 
}55 ·'
SD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\PageMetasChangeTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{		 
public 

class #
PageMetasChangeTemplate (
:) *
ComposeEmailBase+ ;
{ 
private 
string 
PageMetasChangeHtml *
=+ ,
null- 1
;1 2
public #
PageMetasChangeTemplate &
(& '
PageMetasEntity' 6
objMetas7 ?
)? @
{ 	
try 
{ 
StringBuilder 
message %
=& '
new( +
StringBuilder, 9
(9 :
): ;
;; <
message 
. 
Append 
( 
$str 3
)3 4
;4 5
if 
( 
objMetas 
!= 
null  $
)$ %
{ 
if 
( 
! 
string 
.  
IsNullOrEmpty  -
(- .
objMetas. 6
.6 7
	ModelName7 @
)@ A
)A B
{ 
message 
.  
Append  &
(& '
$str' >
+? @
objMetasA I
.I J
PageNameJ R
+S T
$strU n
+o p
objMetasq y
.y z
MakeName	z Ç
+
É Ñ
$str
Ö ì
+
î ï
objMetas
ñ û
.
û ü
	ModelName
ü ®
+
© ™
$str
´ ≤
)
≤ ≥
;
≥ ¥
}   
else!! 
{"" 
message## 
.##  
Append##  &
(##& '
$str##' >
+##? @
objMetas##A I
.##I J
PageName##J R
+##S T
$str##U n
+##o p
objMetas##q y
.##y z
MakeName	##z Ç
+
##É Ñ
$str
##Ö å
)
##å ç
;
##ç é
}$$ 
if&& 
(&& 
!&& 
string&& 
.&&  
IsNullOrEmpty&&  -
(&&- .
objMetas&&. 6
.&&6 7
Title&&7 <
)&&< =
)&&= >
message'' 
.''  
Append''  &
(''& '
$str''' 8
+''9 :
objMetas''; C
.''C D
Title''D I
+''J K
$str''L S
)''S T
;''T U
if)) 
()) 
!)) 
string)) 
.))  
IsNullOrEmpty))  -
())- .
objMetas)). 6
.))6 7
Description))7 B
)))B C
)))C D
message** 
.**  
Append**  &
(**& '
$str**' >
+**? @
objMetas**A I
.**I J
Description**J U
+**V W
$str**X _
)**_ `
;**` a
if,, 
(,, 
!,, 
string,, 
.,,  
IsNullOrEmpty,,  -
(,,- .
objMetas,,. 6
.,,6 7
Keywords,,7 ?
),,? @
),,@ A
message-- 
.--  
Append--  &
(--& '
$str--' ;
+--< =
objMetas--> F
.--F G
Keywords--G O
+--P Q
$str--R Y
)--Y Z
;--Z [
if// 
(// 
!// 
string// 
.//  
IsNullOrEmpty//  -
(//- .
objMetas//. 6
.//6 7
Heading//7 >
)//> ?
)//? @
message00 
.00  
Append00  &
(00& '
$str00' :
+00; <
objMetas00= E
.00E F
Heading00F M
+00N O
$str00P W
)00W X
;00X Y
if22 
(22 
!22 
string22 
.22  
IsNullOrEmpty22  -
(22- .
objMetas22. 6
.226 7
Summary227 >
)22> ?
)22? @
message33 
.33  
Append33  &
(33& '
$str33' :
+33; <
objMetas33= E
.33E F
Summary33F M
+33N O
$str33P W
)33W X
;33X Y
PageMetasChangeHtml55 '
=55( )
message55* 1
.551 2
ToString552 :
(55: ;
)55; <
;55< =
}66 
}88 
catch99 
(99 
	Exception99 
ex99 
)99 
{:: 

ErrorClass;; 
objErr;; !
=;;" #
new;;$ '

ErrorClass;;( 2
(;;2 3
ex;;3 5
,;;5 6
$str;;7 ~
);;~ 
;	;; Ä
}<< 
}>> 	
public@@ 
override@@ 
string@@ 
ComposeBody@@ *
(@@* +
)@@+ ,
{AA 	
returnBB 
PageMetasChangeHtmlBB &
;BB& '
}CC 	
}DD 
}EE °
PD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\PasswordRecoveryMail.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public 

class  
PasswordRecoveryMail %
:& '
ComposeEmailBase( 8
{ 
private 
string 
RecoveryMailHtml '
=( )
null* .
;. /
public  
PasswordRecoveryMail #
(# $
string$ *
customerName+ 7
,7 8
string9 ?
email@ E
,E F
stringG M
tokenN S
)S T
{ 	
try 
{ 
StringBuilder 
message %
=& '
new( +
StringBuilder, 9
(9 :
): ;
;; <
message 
. 
Append 
( 
$str	 â
)
â ä
;
ä ã
message 
. 
Append 
( 
$str *
++ ,
customerName- 9
+: ;
$str< D
)D E
;E F
message 
. 
Append 
( 
$str @
)@ A
;A B
message 
. 
Append 
( 
$str D
)D E
;E F
message 
. 
Append 
( 
$str t
)t u
;u v
message 
. 
Append 
( 
$str v
)v w
;w x
message 
. 
Append 
( 
$str X
)X Y
;Y Z
message   
.   
Append   
(   
$str   +
+  , -
email  . 3
+  4 5
$str  6 <
)  < =
;  = >
message"" 
."" 
Append"" 
("" 
$str"" X
)""X Y
;""Y Z
message## 
.## 
Append## 
(## 
$str## _
+##` a
token##b g
)##g h
;##h i
message$$ 
.$$ 
Append$$ 
($$ 
$str$$ O
)$$O P
;$$P Q
message&& 
.&& 
Append&& 
(&& 
$str&& |
)&&| }
;&&} ~
message(( 
.(( 
Append(( 
((( 
$str(( p
)((p q
;((q r
message** 
.** 
Append** 
(** 
$str** :
)**: ;
;**; <
message++ 
.++ 
Append++ 
(++ 
$str++ B
)++B C
;++C D
RecoveryMailHtml..  
=..! "
message..# *
...* +
ToString..+ 3
(..3 4
)..4 5
;..5 6
}// 
catch00 
(00 
	Exception00 
err00  
)00  !
{11 
Bikewale22 
.22 
Notifications22 &
.22& '

ErrorClass22' 1
objErr222 8
=229 :
new22; >
Bikewale22? G
.22G H
Notifications22H U
.22U V

ErrorClass22V `
(22` a
err22a d
,22d e
$str	22f û
)
22û ü
;
22ü †
objErr33 
.33 
SendMail33 
(33  
)33  !
;33! "
}44 
}55 	
public77 
override77 
string77 
ComposeBody77 *
(77* +
)77+ ,
{88 	
return99 
RecoveryMailHtml99 #
;99# $
}:: 	
};; 
}<< €\
^D:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\PreBookingConfirmationMailToDealer.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public 

class .
"PreBookingConfirmationMailToDealer 3
:4 5
ComposeEmailBase6 F
{ 
private 
string 
MailHTML 
=  !
null" &
;& '
public .
"PreBookingConfirmationMailToDealer 1
(1 2
string2 8
customerName9 E
,E F
stringG M
customerMobileN \
,\ ]
string^ d
customerAreae q
,q r
strings y
customerEmail	z á
,
á à
uint
â ç

totalPrice
é ò
,
ò ô
uint
ö û
bookingAmount
ü ¨
,
¨ ≠
uint 
balanceAmount 
, 
List  $
<$ %
PQ_Price% -
>- .
	priceList/ 8
,8 9
string: @
bookingReferenceNoA S
,S T
stringU [
bikeName\ d
,d e
stringf l
	bikeColorm v
,v w
stringx ~

dealerName	 â
,
â ä
List
ã è
<
è ê
OfferEntity
ê õ
>
õ ú
	offerList
ù ¶
,
¶ ß
string
® Æ
	imagePath
Ø ∏
,
∏ π
string
∫ ¿
versionName
¡ Ã
,
Ã Õ
uint
Œ “
insuranceAmount
” ‚
=
„ ‰
$num
Â Ê
)
Ê Á
{ 	
List 
< 
PQ_Price 
> 
discountList '
=( )
OfferHelper* 5
.5 6#
ReturnDiscountPriceList6 M
(M N
	offerListN W
,W X
	priceListY b
)b c
;c d
StringBuilder 
sb 
= 
null #
;# $
try 
{ 
sb 
= 
new 
StringBuilder &
(& '
)' (
;( )
sb 
. 
AppendFormat 
(  
$str	  ﬂ
+ 
$str	 ◊
+ 
$str =
+ 
$str 5
+   
$str   X
+!! 
$str!! i
+"" 
$str	"" „
+## 
$str## &
+$$ 
$str$$ q
+%% 
$str%% #
+&& 
$str&& &
+'' 
$str'' @
+(( 
$str(( "
+)) 
$str)) =
+** 
$str** {
+++ 
$str++ u
+,, 
$str,, &
+-- 
$str-- "
+.. 
$str.. #
,..# $
DateTime..% -
...- .
Now... 1
...1 2
ToString..2 :
(..: ;
$str..; I
)..I J
)..J K
;..K L
sb11 
.11 
AppendFormat11 
(11  
$str11  @
+22 
$str	22 ú
+33 
$str33 h
+44 
$str44 g
+55 
$str55 m
+66 
$str	66 ¢
+77 
$str	77 â
+88 
$str88 M
+99 
$str99 *
+:: 
$str:: &
+;; 
$str	;; â
+<< 
$str<< i
+== 
$str== `
+>> 
$str	>> ˇ
+?? 
$str?? *
+@@ 
$str@@ Z
+AA 
$strAA g
+BB 
$str	BB ˇ
+CC 
$strCC *
+DD 
$strDD "
+EE 
$strEE "
,EE" #

dealerNameGG &
,GG& '
customerNameHH (
,HH( )
bikeNameII $
,II$ %
FormatJJ "
.JJ" #
FormatPriceJJ# .
(JJ. /
bookingAmountJJ/ <
.JJ< =
ToStringJJ= E
(JJE F
)JJF G
)JJG H
,JJH I
bookingReferenceNoKK .
.KK. /
TrimKK/ 3
(KK3 4
)KK4 5
,KK5 6
FormatLL "
.LL" #
FormatPriceLL# .
(LL. /
(LL/ 0
balanceAmountLL0 =
-LL> ? 
TotalDiscountedPriceLL@ T
(LLT U
discountListLLU a
)LLa b
)LLb c
.LLc d
ToStringLLd l
(LLl m
)LLm n
)LLn o
)MM 
;MM 
sbQQ 
.QQ 
AppendFormatQQ 
(QQ  
$str	RR §
+SS 
$strSS p
+TT 
$str	TT é
+UU 
$str	UU ˛
+VV 
$str	VV ﬂ
+WW 
$str	WW ›
+XX 
$strXX A
+YY 
$strYY 
,ZZ 
customerNameZZ "
,[[ 
customerEmail[[ #
,\\ 
customerMobile\\ $
,]] 
customerArea]] "
)^^ 
;^^ 
sbbb 
.bb 
AppendFormatbb 
(bb  
$strcc P
+dd 
$str	dd ó
+ee 
$stree J
+ff 
$strff x
+gg 
$strgg 
+hh 
$strhh s
+ii 
$str	ii £
+jj 
$str	jj ê
+kk 
$strkk 9
,ll 
bikeNamell 
,mm 
versionNamemm 
,nn 
	bikeColornn 
,oo 
	imagePathoo 
)pp 
;pp 
sbtt 
.tt 
AppendFormattt 
(tt  
$struu r
+vv 
$strvv j
+ww 
$str	ww ç
+xx 
$strxx C
+yy 
$stryy #
,zz 
Formatzz 
.zz 
FormatPricezz *
(zz* +
Convertzz+ 2
.zz2 3
ToStringzz3 ;
(zz; <

totalPricezz< F
-zzG H 
TotalDiscountedPricezzI ]
(zz] ^
discountListzz^ j
)zzj k
)zzk l
)zzl m
){{ 
;{{ 
sb}} 
.}} 
AppendFormat}} 
(}}  
$str~~ ?
+ 
$str g
+
ÄÄ 
$strÄÄ â
+
ÅÅ 
$str
ÅÅ ?
+
ÇÇ 
$str
ÇÇ !
,
ÉÉ 
Format
ÉÉ 
.
ÉÉ 
FormatPrice
ÉÉ (
(
ÉÉ( )
bookingAmount
ÉÉ) 6
.
ÉÉ6 7
ToString
ÉÉ7 ?
(
ÉÉ? @
)
ÉÉ@ A
)
ÉÉA B
)
ÑÑ 
;
ÑÑ 
sb
ÜÜ 
.
ÜÜ 
AppendFormat
ÜÜ 
(
ÜÜ  
$str
ÜÜ  F
+
áá 
$str
áá u
+
àà 
$stràà ì
+
ââ 
$str
ââ I
+
ää 
$str
ää 7
,
ãã 
Format
ãã 
.
ãã 
FormatPrice
ãã $
(
ãã$ %
(
ãã% &
balanceAmount
ãã& 3
-
ãã4 5"
TotalDiscountedPrice
ãã6 J
(
ããJ K
discountList
ããK W
)
ããW X
)
ããX Y
.
ããY Z
ToString
ããZ b
(
ããb c
)
ããc d
)
ããd e
)
åå 
;
åå 
if
ïï 
(
ïï 
	offerList
ïï 
!=
ïï  
null
ïï! %
&&
ïï& (
	offerList
ïï) 2
.
ïï2 3
Count
ïï3 8
>
ïï9 :
$num
ïï; <
)
ïï< =
{
ññ 
sb
óó 
.
óó 
AppendFormat
óó #
(
óó# $
$str
òò Z
+
ôô 
$strôô ≈
+
öö 
$str
öö e
)
ööe f
;
ööf g
foreach
õõ 
(
õõ 
var
õõ  
offer
õõ! &
in
õõ' )
	offerList
õõ* 3
)
õõ3 4
{
úú 
sb
ùù 
.
ùù 
AppendFormat
ùù '
(
ùù' (
$str
ûû |
+
üü  !
$strüü" °
+
††  !
$str††" ∏
+
°°  !
$str
°°" E
+
¢¢ 
$str
¢¢ "
,
££ 
offer
££ 
.
££  
OfferCategoryId
££  /
,
§§ 
offer
§§ 
.
§§  
	OfferText
§§  )
)
•• 
;
•• 
}
¶¶ 
sb
ßß 
.
ßß 
AppendFormat
ßß #
(
ßß# $
$str
ßß$ 2
)
ßß2 3
;
ßß3 4
}
®® 
sb
¨¨ 
.
¨¨ 
AppendFormat
¨¨ 
(
¨¨  
$str
¨¨  |
+
≠≠ 
$str≠≠ Ë
+
ÆÆ 
$strÆÆ ≈
+
ØØ 
$str
ØØ s
+
∞∞ 
$str
∞∞ 
+
±± 
$str
±± "
+
≤≤ 
$str
≤≤ 
+
≥≥ 
$str≥≥ ‘
+
¥¥ 
$str
¥¥ q
+
µµ 
$strµµ Ã
+
∂∂ 
$str
∂∂ ~
+
∑∑ 
$str∑∑ à
+
∏∏ 
$str
∏∏ *
+
ππ 
$str
ππ &
+
∫∫ 
$str
∫∫ h
+
ªª 
$str
ªª %
+
ºº 
$strºº ˇ
+
ΩΩ 
$str
ΩΩ *
+
ææ 
$str
ææ &
+
øø 
$str
øø B
+
¿¿ 
$str
¿¿ "
+
¡¡ 
$str
¡¡ 
+
¬¬ 
$str
¬¬ g
+
√√ 
$str
√√ 
+
ƒƒ 
$str
ƒƒ 
+
≈≈ 
$str
≈≈ 
)
≈≈ 
;
≈≈ 
}
∆∆ 
catch
«« 
(
«« 
	Exception
«« 
ex
«« 
)
««  
{
»» 
Bikewale
…… 
.
…… 
Notifications
…… &
.
……& '

ErrorClass
……' 1
objErr
……2 8
=
……9 :
new
……; >
Bikewale
……? G
.
……G H
Notifications
……H U
.
……U V

ErrorClass
……V `
(
……` a
ex
……a c
,
……c d
$str……e ´
)……´ ¨
;……¨ ≠
objErr
   
.
   
SendMail
   
(
    
)
    !
;
  ! "
}
ÀÀ 
MailHTML
ÃÃ 
=
ÃÃ 
sb
ÃÃ 
.
ÃÃ 
ToString
ÃÃ "
(
ÃÃ" #
)
ÃÃ# $
;
ÃÃ$ %
}
ÕÕ 	
public
œœ 
override
œœ 
string
œœ 
ComposeBody
œœ *
(
œœ* +
)
œœ+ ,
{
–– 	
return
—— 
MailHTML
—— 
;
—— 
}
““ 	
	protected
ŸŸ 
UInt32
ŸŸ "
TotalDiscountedPrice
ŸŸ -
(
ŸŸ- .
List
ŸŸ. 2
<
ŸŸ2 3
PQ_Price
ŸŸ3 ;
>
ŸŸ; <
discountList
ŸŸ= I
)
ŸŸI J
{
⁄⁄ 	
UInt32
€€ 

totalPrice
€€ 
=
€€ 
$num
€€  !
;
€€! "
if
›› 
(
›› 
discountList
›› 
!=
›› 
null
››  $
&&
››% '
discountList
››( 4
.
››4 5
Count
››5 :
>
››; <
$num
››= >
)
››> ?
{
ﬁﬁ 
foreach
ﬂﬂ 
(
ﬂﬂ 
var
ﬂﬂ 
priceListObj
ﬂﬂ )
in
ﬂﬂ* ,
discountList
ﬂﬂ- 9
)
ﬂﬂ9 :
{
‡‡ 

totalPrice
·· 
+=
·· !
priceListObj
··" .
.
··. /
Price
··/ 4
;
··4 5
}
‚‚ 
}
„„ 
return
ÂÂ 

totalPrice
ÂÂ 
;
ÂÂ 
}
ÊÊ 	
}
ÁÁ 
}ËË ßï
bD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\PreBookingConfirmationMailToDealer_old.cs
	namespace

 	
Bikewale


 
.

 
Notifications

  
.

  !
MailTemplates

! .
{ 
public 

class 2
&PreBookingConfirmationMailToDealer_old 7
:8 9
ComposeEmailBase: J
{ 
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
string 
CustomerMobile $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
string 
CustomerArea "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
CustomerEmail #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
uint 

TotalPrice 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
uint 
BookingAmount !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
uint 
BalanceAmount !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
List 
< 
PQ_Price 
> 
	PriceList '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
string 
BookingReferenceNo (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
string 
BikeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
	BikeColor 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 

DealerName  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
List 
< 
OfferEntity 
>  
	OfferList! *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
uint 
InsuranceAmount #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public   
List   
<   
PQ_Price   
>   
DiscountList   *
{  + ,
get  - 0
;  0 1
set  2 5
;  5 6
}  7 8
public!! 2
&PreBookingConfirmationMailToDealer_old!! 5
(!!5 6
string!!6 <
customerName!!= I
,!!I J
string!!K Q
customerMobile!!R `
,!!` a
string!!b h
customerArea!!i u
,!!u v
string!!w }
customerEmail	!!~ ã
,
!!ã å
uint
!!ç ë

totalPrice
!!í ú
,
!!ú ù
uint
!!û ¢
bookingAmount
!!£ ∞
,
!!∞ ±
uint"" 
balanceAmount"" 
,"" 
List""  $
<""$ %
PQ_Price""% -
>""- .
	priceList""/ 8
,""8 9
string"": @
bookingReferenceNo""A S
,""S T
string""U [
bikeName""\ d
,""d e
string""f l
	bikeColor""m v
,""v w
string""x ~

dealerName	"" â
,
""â ä
List
""ã è
<
""è ê
OfferEntity
""ê õ
>
""õ ú
	offerList
""ù ¶
,
""¶ ß
uint
""® ¨
insuranceAmount
""≠ º
=
""Ω æ
$num
""ø ¿
)
""¿ ¡
{## 	
CustomerName$$ 
=$$ 
customerName$$ '
;$$' (
CustomerMobile%% 
=%% 
customerMobile%% +
;%%+ ,
CustomerEmail&& 
=&& 
customerEmail&& )
;&&) *
CustomerArea'' 
='' 
customerArea'' '
;''' (

TotalPrice(( 
=(( 

totalPrice(( #
;((# $
BookingAmount)) 
=)) 
bookingAmount)) )
;))) *
BalanceAmount** 
=** 
balanceAmount** )
;**) *
	PriceList++ 
=++ 
	priceList++ !
;++! "
BookingReferenceNo,, 
=,,  
bookingReferenceNo,,! 3
;,,3 4
BikeName-- 
=-- 
bikeName-- 
;--  
	BikeColor.. 
=.. 
	bikeColor.. !
;..! "

DealerName// 
=// 

dealerName// #
;//# $
	OfferList00 
=00 
	offerList00 !
;00! "
InsuranceAmount11 
=11 
insuranceAmount11 -
;11- .
DiscountList22 
=22 
OfferHelper22 &
.22& '#
ReturnDiscountPriceList22' >
(22> ?
	offerList22? H
,22H I
	priceList22J S
)22S T
;22T U
}33 	
public55 
override55 
string55 
ComposeBody55 *
(55* +
)55+ ,
{66 	
StringBuilder77 
sb77 
=77 
null77 #
;77# $
try99 
{:: 
sb;; 
=;; 
new;; 
StringBuilder;; &
(;;& '
);;' (
;;;( )
sb== 
.== 
Append== 
(== 
$str	== Ù
)
==Ù ı
;
==ı ˆ
sb>> 
.>> 
Append>> 
(>> 
$str	>> µ
)
>>µ ∂
;
>>∂ ∑
sb?? 
.?? 
Append?? 
(?? 
$str	?? ﬂ
)
??ﬂ ‡
;
??‡ ·
sb@@ 
.@@ 
Append@@ 
(@@ 
$str	@@ Ä
+
@@Å Ç
BikeName
@@É ã
+
@@å ç
(
@@é è
!
@@è ê
String
@@ê ñ
.
@@ñ ó
IsNullOrEmpty
@@ó §
(
@@§ •
	BikeColor
@@• Æ
)
@@Æ Ø
?
@@∞ ±
$str
@@≤ ∂
+
@@∑ ∏
	BikeColor
@@π ¬
+
@@√ ƒ
$str
@@≈ »
:
@@…  
$str
@@À Õ
)
@@Õ Œ
+
@@œ –
$str
@@— ë
+
@@í ì
DateTime
@@î ú
.
@@ú ù
Now
@@ù †
.
@@† °
ToString
@@° ©
(
@@© ™
$str
@@™ ∏
)
@@∏ π
+
@@∫ ª
$str
@@º ƒ
)
@@ƒ ≈
;
@@≈ ∆
sbAA 
.AA 
AppendAA 
(AA 
$str	AA ¢
)
AA¢ £
;
AA£ §
sbBB 
.BB 
AppendBB 
(BB 
$strBB i
+BBj k

DealerNameBBl v
+BBw x
$str	BBy Ç
)
BBÇ É
;
BBÉ Ñ
sbCC 
.CC 
AppendCC 
(CC 
$strCC s
+CCt u
CustomerName	CCv Ç
+
CCÉ Ñ
$str
CCÖ ‘
)
CC‘ ’
;
CC’ ÷
sbDD 
.DD 
AppendDD 
(DD 
$strDD >
+DD? @
FormatDDA G
.DDG H
FormatPriceDDH S
(DDS T
BookingAmountDDT a
.DDa b
ToStringDDb j
(DDj k
)DDk l
)DDl m
+DDn o
$str	DDp ®
+
DD© ™
BikeName
DD´ ≥
+
DD¥ µ
$str
DD∂ ›
+
DDﬁ ﬂ 
BookingReferenceNo
DD‡ Ú
+
DDÛ Ù
$str
DDı ù
)
DDù û
;
DDû ü
sbEE 
.EE 
AppendEE 
(EE 
$str	EE Â
)
EEÂ Ê
;
EEÊ Á
sbFF 
.FF 
AppendFF 
(FF 
$str	FF Ø
)
FFØ ∞
;
FF∞ ±
sbGG 
.GG 
AppendGG 
(GG 
$str	GG ™
+
GG´ ¨
CustomerName
GG≠ π
+
GG∫ ª
$str
GGº »
)
GG» …
;
GG…  
sbHH 
.HH 
AppendHH 
(HH 
$str	HH ñ
+
HHó ò
CustomerMobile
HHô ß
+
HH® ©
$str
HH™ ∂
)
HH∂ ∑
;
HH∑ ∏
sbII 
.II 
AppendII 
(II 
$str	II ñ
+
IIó ò
CustomerEmail
IIô ¶
+
IIß ®
$str
II© µ
)
IIµ ∂
;
II∂ ∑
sbJJ 
.JJ 
AppendJJ 
(JJ 
$str	JJ ô
+
JJö õ
CustomerArea
JJú ®
+
JJ© ™
$str
JJ´ «
)
JJ« »
;
JJ» …
sbKK 
.KK 
AppendKK 
(KK 
$str	KK ô
+
KKö õ
BikeName
KKú §
+
KK• ¶
$str
KKß ´
+
KK¨ ≠
	BikeColor
KKÆ ∑
+
KK∏ π
$str
KK∫ √
)
KK√ ƒ
;
KKƒ ≈
sbLL 
.LL 
AppendLL 
(LL 
$strLL E
)LLE F
;LLF G
ifNN 
(NN 
	PriceListNN 
!=NN  
nullNN! %
&&NN& (
	PriceListNN) 2
.NN2 3
CountNN3 8
>NN9 :
$numNN; <
)NN< =
{OO 
sbPP 
.PP 
AppendPP 
(PP 
$str	PP ø
)
PPø ¿
;
PP¿ ¡
sbQQ 
.QQ 
AppendQQ 
(QQ 
$str	QQ ¿
)
QQ¿ ¡
;
QQ¡ ¬
foreachSS 
(SS 
varSS  
itemsSS! &
inSS' )
	PriceListSS* 3
)SS3 4
{TT 
sbUU 
.UU 
AppendUU !
(UU! "
$strUU" U
+UUV W
itemsUUX ]
.UU] ^
CategoryNameUU^ j
+UUk l
$strUUm t
)UUt u
;UUu v
ifVV 
(VV 
itemsVV !
.VV! "
CategoryNameVV" .
.VV. /
ToUpperVV/ 6
(VV6 7
)VV7 8
.VV8 9
ContainsVV9 A
(VVA B
$strVVB M
)VVM N
&&VVO Q
(VVR S
InsuranceAmountVVS b
>VVc d
$numVVe f
)VVf g
)VVg h
{WW 
sbXX 
.XX 
AppendXX %
(XX% &
$str	XX& Å
+
XXÇ É
Format
XXÑ ä
.
XXä ã
FormatPrice
XXã ñ
(
XXñ ó
items
XXó ú
.
XXú ù
Price
XXù ¢
.
XX¢ £
ToString
XX£ ´
(
XX´ ¨
)
XX¨ ≠
)
XX≠ Æ
+
XXØ ∞
$str
XX± ƒ
)
XXƒ ≈
;
XX≈ ∆
}YY 
elseZZ 
{[[ 
sb\\ 
.\\ 
Append\\ %
(\\% &
$str	\\& Å
+
\\Ç É
Format
\\Ñ ä
.
\\ä ã
FormatPrice
\\ã ñ
(
\\ñ ó
items
\\ó ú
.
\\ú ù
Price
\\ù ¢
.
\\¢ £
ToString
\\£ ´
(
\\´ ¨
)
\\¨ ≠
)
\\≠ Æ
+
\\Ø ∞
$str
\\± ƒ
)
\\ƒ ≈
;
\\≈ ∆
}]] 
}^^ 
if`` 
(`` 
DiscountList`` $
!=``% '
null``( ,
&&``- /
DiscountList``0 <
.``< =
Count``= B
>``C D
$num``E F
)``F G
{aa 
sbbb 
.bb 
Appendbb !
(bb! "
$str	bb" Ö
)
bbÖ Ü
;
bbÜ á
sbcc 
.cc 
Appendcc !
(cc! "
$str	cc" ˇ
+
ccÄ Å
Format
ccÇ à
.
ccà â
FormatPrice
ccâ î
(
ccî ï

TotalPrice
ccï ü
.
ccü †
ToString
cc† ®
(
cc® ©
)
cc© ™
)
cc™ ´
+
cc¨ ≠
$str
ccÆ ∫
)
cc∫ ª
;
ccª º
foreachdd 
(dd  !
vardd! $
listdd% )
indd* ,
DiscountListdd- 9
)dd9 :
{ee 
sbff 
.ff 
Appendff %
(ff% &
$strff& b
+ffc d
listffe i
.ffi j
CategoryNameffj v
+ffw x
$str	ffy 
+
ffÒ Ú
Format
ffÛ ˘
.
ff˘ ˙
FormatPrice
ff˙ Ö
(
ffÖ Ü
list
ffÜ ä
.
ffä ã
Price
ffã ê
.
ffê ë
ToString
ffë ô
(
ffô ö
)
ffö õ
)
ffõ ú
+
ffù û
$str
ffü ´
)
ff´ ¨
;
ff¨ ≠
}gg 
sbii 
.ii 
Appendii !
(ii! "
$str	ii" Ö
)
iiÖ Ü
;
iiÜ á
sbjj 
.jj 
Appendjj !
(jj! "
$str	jj" 
+
jjÒ Ú
Format
jjÛ ˘
.
jj˘ ˙
FormatPrice
jj˙ Ö
(
jjÖ Ü
(
jjÜ á

TotalPrice
jjá ë
-
jjí ì"
TotalDiscountedPrice
jjî ®
(
jj® ©
)
jj© ™
)
jj™ ´
.
jj´ ¨
ToString
jj¨ ¥
(
jj¥ µ
)
jjµ ∂
)
jj∂ ∑
+
jj∏ π
$str
jj∫ ∆
)
jj∆ «
;
jj« »
}kk 
elsell 
{mm 
sbnn 
.nn 
Appendnn !
(nn! "
$str	nn" Ö
)
nnÖ Ü
;
nnÜ á
sboo 
.oo 
Appendoo !
(oo! "
$str	oo" ‹
+
oo› ﬁ
Format
ooﬂ Â
.
ooÂ Ê
FormatPrice
ooÊ Ò
(
ooÒ Ú

TotalPrice
ooÚ ¸
.
oo¸ ˝
ToString
oo˝ Ö
(
ooÖ Ü
)
ooÜ á
)
ooá à
+
ooâ ä
$str
ooã ó
)
ooó ò
;
ooò ô
}pp 
sbqq 
.qq 
Appendqq 
(qq 
$strqq 6
)qq6 7
;qq7 8
}rr 
sbss 
.ss 
Appendss 
(ss 
$str	ss Ó
)
ssÓ Ô
;
ssÔ 
sbtt 
.tt 
Appendtt 
(tt 
$str	tt ñ
+
ttó ò
Format
ttô ü
.
ttü †
FormatPrice
tt† ´
(
tt´ ¨
BookingAmount
tt¨ π
.
ttπ ∫
ToString
tt∫ ¬
(
tt¬ √
)
tt√ ƒ
)
ttƒ ≈
+
tt∆ «
$str
tt» ÷
)
tt÷ ◊
;
tt◊ ÿ
sbuu 
.uu 
Appenduu 
(uu 
$str	uu ö
)
uuö õ
;
uuõ ú
sbvv 
.vv 
Appendvv 
(vv 
$str	vv ô
+
vvö õ
Format
vvú ¢
.
vv¢ £
FormatPrice
vv£ Æ
(
vvÆ Ø
(
vvØ ∞
BalanceAmount
vv∞ Ω
-
vvæ ø"
TotalDiscountedPrice
vv¿ ‘
(
vv‘ ’
)
vv’ ÷
)
vv÷ ◊
.
vv◊ ÿ
ToString
vvÿ ‡
(
vv‡ ·
)
vv· ‚
)
vv‚ „
+
vv‰ Â
$str
vvÊ Ù
)
vvÙ ı
;
vvı ˆ
sbww 
.ww 
Appendww 
(ww 
$str	ww  
)
ww  À
;
wwÀ Ã
ifyy 
(yy 
	OfferListyy 
!=yy  
nullyy! %
&&yy& (
	OfferListyy) 2
.yy2 3
Countyy3 8
>yy9 :
$numyy; <
)yy< =
{zz 
sb{{ 
.{{ 
Append{{ 
({{ 
$str	{{ „
)
{{„ ‰
;
{{‰ Â
if}} 
(}} 
InsuranceAmount}} '
==}}( *
$num}}+ ,
)}}, -
{~~ 
foreach 
(  !
var! $
items% *
in+ -
	OfferList. 7
)7 8
{
ÄÄ 
sb
ÅÅ 
.
ÅÅ 
Append
ÅÅ %
(
ÅÅ% &
$strÅÅ& Å
+ÅÅÇ É
itemsÅÅÑ â
.ÅÅâ ä
	OfferTextÅÅä ì
+ÅÅî ï
$strÅÅñ ù
)ÅÅù û
;ÅÅû ü
}
ÇÇ 
}
ÉÉ 
else
ÑÑ 
{
ÖÖ 
sb
ÜÜ 
.
ÜÜ 
AppendFormat
ÜÜ '
(
ÜÜ' (
$strÜÜ( ¡
,ÜÜ¡ ¬
InsuranceAmountÜÜ√ “
)ÜÜ“ ”
;ÜÜ” ‘
}
áá 
sb
àà 
.
àà 
Append
àà 
(
àà 
$stràà »
)àà» …
;àà…  
}
ââ 
sb
ää 
.
ää 
Append
ää 
(
ää 
$strää Â
)ääÂ Ê
;ääÊ Á
sb
ãã 
.
ãã 
Append
ãã 
(
ãã 
$strãã ñ
)ããñ ó
;ããó ò
sb
åå 
.
åå 
Append
åå 
(
åå 
$stråå ∏
)åå∏ π
;ååπ ∫
sb
çç 
.
çç 
Append
çç 
(
çç 
$strçç ê
)ççê ë
;ççë í
sb
éé 
.
éé 
Append
éé 
(
éé 
$stréé ·
)éé· ‚
;éé‚ „
}
èè 
catch
êê 
(
êê 
	Exception
êê 
ex
êê 
)
êê  
{
ëë 
Bikewale
íí 
.
íí 
Notifications
íí &
.
íí& '

ErrorClass
íí' 1
objErr
íí2 8
=
íí9 :
new
íí; >
Bikewale
íí? G
.
ííG H
Notifications
ííH U
.
ííU V

ErrorClass
ííV `
(
íí` a
ex
íía c
,
ííc d
$strííe ´
)íí´ ¨
;íí¨ ≠
objErr
ìì 
.
ìì 
SendMail
ìì 
(
ìì  
)
ìì  !
;
ìì! "
}
îî 
return
ïï 
sb
ïï 
.
ïï 
ToString
ïï 
(
ïï 
)
ïï  
;
ïï  !
}
ññ 	
	protected
ùù 
UInt32
ùù "
TotalDiscountedPrice
ùù -
(
ùù- .
)
ùù. /
{
ûû 	
UInt32
üü 

totalPrice
üü 
=
üü 
$num
üü  !
;
üü! "
if
°° 
(
°° 
DiscountList
°° 
!=
°° 
null
°°  $
&&
°°% '
DiscountList
°°( 4
.
°°4 5
Count
°°5 :
>
°°; <
$num
°°= >
)
°°> ?
{
¢¢ 
foreach
££ 
(
££ 
var
££ 
priceListObj
££ )
in
££* ,
DiscountList
££- 9
)
££9 :
{
§§ 

totalPrice
•• 
+=
•• !
priceListObj
••" .
.
••. /
Price
••/ 4
;
••4 5
}
¶¶ 
}
ßß 
return
©© 

totalPrice
©© 
;
©© 
}
™™ 	
}
´´ 
}
¨¨ Ÿ7
\D:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\PreBookingConfirmationToCustomer.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{		 
public 

class ,
 PreBookingConfirmationToCustomer 1
:2 3
ComposeEmailBase4 D
{ 
private 
string 
MailHTML 
=  !
string" (
.( )
Empty) .
;. /
public ,
 PreBookingConfirmationToCustomer /
(/ 0
string0 6
customerName7 C
,C D
List 
< 
PQ_Price 
> 
	priceList $
,$ %
List& *
<* +
OfferEntity+ 6
>6 7
	offerList8 A
,A B
string 
bookingReferenceNo %
,% &
uint' +
totalAmount, 7
,7 8
uint 
preBookingAmount !
,! "
string# )
makeModelName* 7
,7 8
string9 ?
versionName@ K
,K L
stringM S
colorT Y
,Y Z
string[ a
imgb e
,e f
string 

dealerName 
, 
string %
dealerAddress& 3
,3 4
string5 ;
dealerMobile< H
,H I
stringJ P
dealerEmailIdQ ^
,^ _
string` f
dealerWorkingTimeg x
,x y
double	z Ä
dealerLatitude
Å è
,
è ê
double
ë ó
dealerLongitude
ò ß
)
ß ®
{ 	
StringBuilder 
mail 
=  
new! $
StringBuilder% 2
(2 3
)3 4
;4 5
try 
{ 
mail 
. 
AppendFormat !
(! "
$str	" ·!
,
·! ‚!
DateTime   
.   
Now    
.    !
ToString  ! )
(  ) *
$str  * 8
)  8 9
,  9 :
customerName!!  
,!!  !
makeModelName"" !
,""! "
Utility## 
.## 
Format## "
.##" #
FormatPrice### .
(##. /
Convert##/ 6
.##6 7
ToString##7 ?
(##? @
preBookingAmount##@ P
)##P Q
)##Q R
,##R S
bookingReferenceNo$$ &
,$$& '
Utility%% 
.%% 
Format%% "
.%%" #
FormatPrice%%# .
(%%. /
Convert%%/ 6
.%%6 7
ToString%%7 ?
(%%? @
totalAmount%%@ K
-%%L M
preBookingAmount%%N ^
)%%^ _
)%%_ `
,%%` a
makeModelName&& !
,&&! "
versionName'' 
,''  
color(( 
,(( 
img)) 
)** 
;** 
if++ 
(++ 
	priceList++ 
!=++  
null++! %
&&++& (
	priceList++) 2
.++2 3
Count++3 8
>++9 :
$num++; <
)++< =
{,, 
mail-- 
.-- 
Append-- 
(--  
$str--  v
)--v w
;--w x
foreach.. 
(.. 
var..  
list..! %
in..& (
	priceList..) 2
)..2 3
{// 
mail00 
.00 
AppendFormat00 )
(00) *
$str	00* •
,11 
list11 "
.11" #
CategoryName11# /
,11/ 0
Utility111 8
.118 9
Format119 ?
.11? @
FormatPrice11@ K
(11K L
Convert11L S
.11S T
ToString11T \
(11\ ]
list11] a
.11a b
Price11b g
)11g h
)11h i
)11i j
;11j k
}22 
}33 
mail44 
.44 
AppendFormat44 !
(44! "
$str	44" Ó	
,55 
Format55 
.55 
FormatPrice55 (
(55( )
Convert55) 0
.550 1
ToString551 9
(559 :
totalAmount55: E
)55E F
)55F G
,66 
Format66 
.66 
FormatPrice66 (
(66( )
Convert66) 0
.660 1
ToString661 9
(669 :
preBookingAmount66: J
)66J K
)66K L
,77 
Format77 
.77 
FormatPrice77 (
(77( )
Convert77) 0
.770 1
ToString771 9
(779 :
totalAmount77: E
-77F G
preBookingAmount77H X
)77X Y
)77Y Z
)88 
;88 
mail:: 
.:: 
AppendFormat:: !
(::! "
$str	::" ¥	
,;; 

dealerName;;  
,;;  !
dealerAddress;;" /
,;;/ 0
dealerMobile;;1 =
,;;= >
dealerEmailId;;? L
,;;L M
dealerWorkingTime;;N _
,;;_ `
dealerLatitude;;a o
,;;o p
dealerLongitude	;;q Ä
)
;;Ä Å
;
;;Å Ç
if== 
(== 
	offerList== 
!===  
null==! %
&&==& (
	offerList==) 2
.==2 3
Count==3 8
>==9 :
$num==; <
)==< =
{>> 
mail?? 
.?? 
AppendFormat?? %
(??% &
$str	??& è
)
??è ê
;
??ê ë
foreach@@ 
(@@ 
var@@  
list@@! %
in@@& (
	offerList@@) 2
)@@2 3
{AA 
mailBB 
.BB 
AppendFormatBB )
(BB) *
$str	BB* Ë
,
BBË È
listCC  
.CC  !
OfferCategoryIdCC! 0
,CC0 1
listCC2 6
.CC6 7
	OfferTextCC7 @
)CC@ A
;CCA B
}DD 
mailEE 
.EE 
AppendEE 
(EE  
$strEE  .
)EE. /
;EE/ 0
}FF 
mailGG 
.GG 
AppendGG 
(GG 
$str	GG Â
)
GGÂ Ê
;
GGÊ Á
MailHTMLHH 
=HH 
ConvertHH "
.HH" #
ToStringHH# +
(HH+ ,
mailHH, 0
)HH0 1
;HH1 2
}II 
catchJJ 
(JJ 
	ExceptionJJ 
exJJ 
)JJ  
{KK 
HttpContextLL 
.LL 
CurrentLL #
.LL# $
TraceLL$ )
.LL) *
WarnLL* .
(LL. /
$strLL/ Z
+LL[ \
exLL] _
.LL_ `
MessageLL` g
)LLg h
;LLh i
}MM 
}NN 	
publicUU 
overrideUU 
stringUU 
ComposeBodyUU *
(UU* +
)UU+ ,
{VV 	
returnWW 
MailHTMLWW 
;WW 
}XX 	
}YY 
}ZZ ˜
\D:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikesModelImagesMailTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
{ 
public

 

class

 ,
 UsedBikesModelImagesMailTemplate

 1
:

2 3
ComposeEmailBase

4 D
{ 
private 
string !
ModelSoldUnitMailHtml ,
=- .
null/ 3
;3 4
public ,
 UsedBikesModelImagesMailTemplate /
(/ 0
)0 1
{ 	
try 
{ 
StringBuilder 
message %
=& '
new( +
StringBuilder, 9
(9 :
): ;
;; <
message 
. 
Append 
( 
$str 4
)4 5
;5 6
message 
. 
Append 
( 
$str	 ò
)
ò ô
;
ô ö
message 
. 
AppendFormat $
($ %
$str% |
,| }
Bikewale	~ Ü
.
Ü á
Utility
á é
.
é è 
BWOprConfiguration
è °
.
° ¢
Instance
¢ ™
.
™ ´
BwOprHostUrlForJs
´ º
)
º Ω
;
Ω æ!
ModelSoldUnitMailHtml %
=& '
message( /
./ 0
ToString0 8
(8 9
)9 :
;: ;
} 
catch 
( 
	Exception 
err  
)  !
{ 
Bikewale 
. 
Notifications &
.& '

ErrorClass' 1
objErr2 8
=9 :
new; >
Bikewale? G
.G H
NotificationsH U
.U V

ErrorClassV `
(` a
erra d
,d e
$str	f ™
)
™ ´
;
´ ¨
} 
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
return !
ModelSoldUnitMailHtml (
;( )
}   	
}!! 
}"" Î
hD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\EditedListingApprovalEmailToSeller.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public

 

class

 .
"EditedListingApprovalEmailToSeller

 3
:

4 5
ComposeEmailBase

6 F
{ 
private 
string 

sellerName !
,! "
	profileNo 
, 
bikeName 
, 

modelImage 
, 
kms 
, 
writeReviewLink 
; 
public .
"EditedListingApprovalEmailToSeller 1
(1 2
string2 8

sellerName9 C
,C D
stringE K
	profileNoL U
,U V
stringW ]
bikeName^ f
,f g
stringh n

modelImageo y
,y z
string	{ Å
kms
Ç Ö
,
Ö Ü
string
á ç
writeReviewLink
é ù
)
ù û
{ 	
this 
. 

sellerName 
= 

sellerName (
;( )
this   
.   
	profileNo   
=   
	profileNo   &
;  & '
this!! 
.!! 
bikeName!! 
=!! 
bikeName!! $
;!!$ %
this"" 
."" 

modelImage"" 
="" 

modelImage"" (
;""( )
this## 
.## 
kms## 
=## 
kms## 
;## 
this$$ 
.$$ 
writeReviewLink$$  
=$$! "
writeReviewLink$$# 2
;$$2 3
}%% 	
public++ 
override++ 
string++ 
ComposeBody++ *
(++* +
)+++ ,
{,, 	
StringBuilder-- 
sb-- 
=-- 
new-- "
StringBuilder--# 0
(--0 1
)--1 2
;--2 3
sb.. 
... 
AppendFormat.. 
(.. 
string.. "
..." #
Format..# )
(..) *
$str	..* ˚	
,
..˚	 ¸	
DateTime
..˝	 Ö

.
..Ö
 Ü

Now
..Ü
 â

.
..â
 ä

ToString
..ä
 í

(
..í
 ì

$str
..ì
 °

)
..°
 ¢

,
..¢
 £

$str
..§
 –

)
..–
 —

)
..—
 “

;
..“
 ”

sb// 
.// 
AppendFormat// 
(// 
$str// l
,//l m

sellerName//n x
)//x y
;//y z
sb00 
.00 
AppendFormat00 
(00 
$str	00 å
,
00å ç
bikeName
00é ñ
,
00ñ ó
	profileNo
00ò °
)
00° ¢
;
00¢ £
sb11 
.11 
AppendFormat11 
(11 
$str	11 À
)
11À Ã
;
11Ã Õ
sb22 
.22 
AppendFormat22 
(22 
string22 "
.22" #
Format22# )
(22) *
$str	22* ¸
,
22¸ ˝
bikeName
22˛ Ü
,
22Ü á
kms
22à ã
,
22ã å
writeReviewLink
22ç ú
,
22ú ù

modelImage
22û ®
)
22® ©
)
22© ™
;
22™ ´
return33 
sb33 
.33 
ToString33 
(33 
)33  
;33  !
}44 	
}55 
}66 ›
iD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\EditedListingRejectionEmailToSeller.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public

 

class

 /
#EditedListingRejectionEmailToSeller

 4
:

5 6
ComposeEmailBase

7 G
{ 
private 
string 

sellerName !
,! "
	profileNo 
, 
bikeName 
, 

modelImage 
, 
kms 
, 
writeReviewLink 
; 
public /
#EditedListingRejectionEmailToSeller 2
(2 3
string3 9

sellerName: D
,D E
stringF L
	profileNoM V
,V W
stringX ^
bikeName_ g
,g h
stringi o

modelImagep z
,z {
string	| Ç
kms
É Ü
)
Ü á
{ 	
this 
. 

sellerName 
= 

sellerName (
;( )
this   
.   
	profileNo   
=   
	profileNo   &
.  & '
ToUpper  ' .
(  . /
)  / 0
;  0 1
this!! 
.!! 
bikeName!! 
=!! 
bikeName!! $
;!!$ %
this"" 
."" 

modelImage"" 
="" 

modelImage"" (
;""( )
this## 
.## 
kms## 
=## 
kms## 
;## 
}$$ 	
public** 
override** 
string** 
ComposeBody** *
(*** +
)**+ ,
{++ 	
StringBuilder,, 
sb,, 
=,, 
new,, "
StringBuilder,,# 0
(,,0 1
),,1 2
;,,2 3
sb-- 
.-- 
AppendFormat-- 
(-- 
string-- "
.--" #
Format--# )
(--) *
$str	--* ˚	
,
--˚	 ¸	
DateTime
--˝	 Ö

.
--Ö
 Ü

Now
--Ü
 â

.
--â
 ä

ToString
--ä
 í

(
--í
 ì

$str
--ì
 °

)
--°
 ¢

,
--¢
 £

$str
--§
 —

)
--—
 “

)
--“
 ”

;
--”
 ‘

sb.. 
... 
AppendFormat.. 
(.. 
$str.. l
,..l m

sellerName..n x
)..x y
;..y z
sb// 
.// 
AppendFormat// 
(// 
$str// |
,//| }
bikeName	//~ Ü
,
//Ü á
	profileNo
//à ë
)
//ë í
;
//í ì
sb00 
.00 
AppendFormat00 
(00 
$str00 j
)00j k
;00k l
sb11 
.11 
AppendFormat11 
(11 
$str	11 á
)
11á à
;
11à â
sb22 
.22 
AppendFormat22 
(22 
$str22 g
)22g h
;22h i
sb33 
.33 
AppendFormat33 
(33 
string33 "
.33" #
Format33# )
(33) *
$str	33* π
,
33π ∫
bikeName
33ª √
,
33√ ƒ
kms
33≈ »
,
33» …
writeReviewLink
33  Ÿ
,
33Ÿ ⁄

modelImage
33€ Â
)
33Â Ê
)
33Ê Á
;
33Á Ë
return44 
sb44 
.44 
ToString44 
(44 
)44  
;44  !
}55 	
}66 
}77 ¿(
bD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\ExpiringListingReminderEmail.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public 

class (
ExpiringListingReminderEmail -
:. /
ComposeEmailBase0 @
{ 
private 
string 
_sellerName "
," #
	_makeName$ -
,- .

_modelName/ 9
,9 :

_repostUrl; E
,E F
_remainingTimeG U
,U V
_emailSubjectW d
,d e
	_qEncoded 
, 
	_distance  
,  !

_bwHostUrl" ,
,, -
_imgPath. 6
;6 7
private 
EnumSMSServiceType "
_remainingDays# 1
;1 2
private 
uint 
_modelId 
; 
public (
ExpiringListingReminderEmail +
(+ ,
string, 2

sellerName3 =
,= >
string? E
makeNameF N
,N O
stringP V
	modelNameW `
,` a
EnumSMSServiceTypeb t
remainingDays	u Ç
,
Ç É
string
Ñ ä
	repostUrl
ã î
,
î ï
string
ñ ú
emailSubject
ù ©
,
© ™
string
™ ∞
imgPath
± ∏
,
∏ π
string
∫ ¿
distance
¡ …
,
…  
string
À —
qEncoded
“ ⁄
,
⁄ €
string
‹ ‚
	bwHostUrl
„ Ï
,
Ï Ì
uint
Ó Ú
modelId
Û ˙
)
˙ ˚
{ 	
_sellerName 
= 

sellerName $
;$ %
	_makeName 
= 
makeName  
;  !

_modelName 
= 
	modelName "
;" #
_remainingDays 
= 
remainingDays *
;* +

_repostUrl 
= 
	repostUrl "
;" #
_emailSubject 
= 
emailSubject (
;( )

_bwHostUrl 
= 
	bwHostUrl "
;" #
	_distance 
= 
distance  
;  !
_imgPath 
= 
imgPath 
; 
_modelId 
= 
modelId 
; 
	_qEncoded 
= 
qEncoded  
;  !
if   
(   
remainingDays   
==    
EnumSMSServiceType  ! 3
.  3 4.
"BikeListingExpiryOneDaySMSToSeller  4 V
)  V W
_remainingTime!! 
=!!  
$str!!! +
;!!+ ,
else"" 
_remainingTime## 
=##  
$str##! )
;##) *
}$$ 	
public++ 
override++ 
string++ 
ComposeBody++ *
(++* +
)+++ ,
{,, 	
StringBuilder-- 
sb-- 
=-- 
new-- "
StringBuilder--# 0
(--0 1
)--1 2
;--2 3
sb// 
.// 
AppendFormat// 
(// 
$str	// ≥
,
//≥ ¥
DateTime
//µ Ω
.
//Ω æ
Now
//æ ¡
.
//¡ ¬
ToString
//¬  
(
//  À
$str
//À ÿ
)
//ÿ Ÿ
,
//Ÿ ⁄
_emailSubject
//⁄ Á
)
//Á Ë
;
//Ë È
sb22 
.22 
AppendFormat22 
(22 
$str	22 œ
,
22œ –
_sellerName
22— ‹
,
22‹ ›
	_makeName
22ﬁ Á
,
22Á Ë

_modelName
22È Û
,
22Û Ù
_remainingTime
22ı É
,
22É Ñ

_repostUrl
22Ö è
)
22è ê
;
22ê ë
if33 
(33 
EnumSMSServiceType33 "
.33" #.
"BikeListingExpiryOneDaySMSToSeller33# E
.33E F
Equals33F L
(33L M
_remainingDays33M [
)33[ \
)33\ ]
sb44 
.44 
AppendFormat44 
(44  
$str	44  ∏
)
44∏ π
;
44π ∫
sb55 
.55 
AppendFormat55 
(55 
$str	55 Ú
,
55Ú Û
_imgPath
55Ù ¸
,
55¸ ˝
	_makeName
55˛ á
,
55á à

_modelName
55â ì
,
55ì î
	_distance
55ï û
,
55û ü

_bwHostUrl
55† ™
,
55™ ´
_modelId
55¨ ¥
,
55¥ µ
	_qEncoded
55∂ ø
)
55ø ¿
;
55¿ ¡
sb77 
.77 
AppendFormat77 
(77 
$str	77 Ã
)
77Ã Õ
;
77Õ Œ
sb88 
.88 
AppendFormat88 
(88 
$str	88 À	
)
88À	 Ã	
;
88Ã	 Õ	
return:: 
sb:: 
.:: 
ToString:: 
(:: 
)::  
;::  !
};; 	
}<< 
}== ˚(
bD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\ListingApprovalEmailToSeller.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public 

class (
ListingApprovalEmailToSeller -
:. /
ComposeEmailBase0 @
{ 
private 
string 

sellerName !
,! "
	profileNo 
, 
bikeName 
, 
city 
, 
owner 
, 
imgPath 
, 
distance 
, 
	bwHostUrl 
, 
qEncoded 
; 
private 
int 
	inquiryId 
; 
private 
uint 
modelId 
; 
private 
DateTime 
makeYear !
;! "
public%% (
ListingApprovalEmailToSeller%% +
(%%+ ,
string%%, 2

sellerName%%3 =
,%%= >
string%%? E
	profileNo%%F O
,%%O P
string%%Q W
bikeName%%X `
,%%` a
DateTime%%b j
makeYear%%k s
,%%s t
string%%u {
owner	%%| Å
,
%%Å Ç
string
%%É â
distance
%%ä í
,
%%í ì
string
%%î ö
city
%%õ ü
,
%%ü †
string
%%° ß
imgPath
%%® Ø
,
%%Ø ∞
int
%%∞ ≥
	inquiryId
%%¥ Ω
,
%%Ω æ
string
%%æ ƒ
	bwHostUrl
%%≈ Œ
,
%%Œ œ
uint
%%œ ”
modelId
%%‘ €
,
%%€ ‹
string
%%‹ ‚
qEncoded
%%„ Î
)
%%Î Ï
{&& 	
this'' 
.'' 

sellerName'' 
='' 

sellerName'' (
;''( )
this(( 
.(( 
	profileNo(( 
=(( 
	profileNo(( &
.((& '
ToUpper((' .
(((. /
)((/ 0
;((0 1
this)) 
.)) 
bikeName)) 
=)) 
bikeName)) $
;))$ %
this** 
.** 
makeYear** 
=** 
makeYear** $
;**$ %
this++ 
.++ 
owner++ 
=++ 
owner++ 
;++ 
this,, 
.,, 
distance,, 
=,, 
distance,, $
;,,$ %
this-- 
.-- 
city-- 
=-- 
city-- 
;-- 
this.. 
... 
imgPath.. 
=.. 
imgPath.. "
;.." #
this// 
.// 
	inquiryId// 
=// 
	inquiryId// &
;//& '
this00 
.00 
	bwHostUrl00 
=00 
	bwHostUrl00 &
;00& '
this11 
.11 
modelId11 
=11 
modelId11 "
;11" #
this22 
.22 
qEncoded22 
=22 
qEncoded22 $
;22$ %
}33 	
public99 
override99 
string99 
ComposeBody99 *
(99* +
)99+ ,
{:: 	
StringBuilder<< 
sb<< 
=<< 
new<< "
StringBuilder<<# 0
(<<0 1
)<<1 2
;<<2 3
sb?? 
.?? 
AppendFormat?? 
(?? 
$str	?? —
,
??— “
DateTime
??” €
.
??€ ‹
Now
??‹ ﬂ
.
??ﬂ ‡
ToString
??‡ Ë
(
??Ë È
$str
??È ˆ
)
??ˆ ˜
)
??˜ ¯
;
??¯ ˘
sbBB 
.BB 
AppendFormatBB 
(BB 
$str	BB ˆ
,
BBˆ ˜

sellerName
BB˜ Å
,
BBÅ Ç
bikeName
BBÇ ä
,
BBä ã
	profileNo
BBå ï
)
BBï ñ
;
BBñ ó
sbCC 
.CC 
AppendFormatCC 
(CC 
$str	CC ô
,
CCô ö
bikeName
CCõ £
,
CC£ §
city
CC§ ®
,
CC® ©
makeYear
CC© ±
.
CC± ≤
Year
CC≤ ∂
,
CC∂ ∑
owner
CC∑ º
,
CCº Ω
distance
CCΩ ≈
,
CC≈ ∆
	inquiryId
CC∆ œ
,
CCœ –
	bwHostUrl
CC– Ÿ
)
CCŸ ⁄
;
CC⁄ €
sbDD 
.DD 
AppendFormatDD 
(DD 
$str	DD Í
,
DDÍ Î
imgPath
DDÎ Ú
,
DDÚ Û
bikeName
DDÛ ˚
,
DD˚ ¸
distance
DD¸ Ñ
,
DDÑ Ö
	bwHostUrl
DDÖ é
,
DDé è
modelId
DDè ñ
,
DDñ ó
qEncoded
DDó ü
)
DDü †
;
DD† °
sbGG 
.GG 
AppendFormatGG 
(GG 
$str	GG Ò
)
GGÒ Ú
;
GGÚ Û
sbHH 
.HH 
AppendFormatHH 
(HH 
$str	HH À	
)
HHÀ	 Ã	
;
HHÃ	 Õ	
returnKK 
sbKK 
.KK 
ToStringKK 
(KK 
)KK  
;KK  !
}LL 	
}MM 
}NN Ñ!
fD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\ListingEmailtoIndividualTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public

 

class

 ,
 ListingEmailtoIndividualTemplate

 1
:

2 3
ComposeEmailBase

4 D
{ 
private 
string 
sellerEmail "
," #

sellerName 
, 
	profileNo 
, 
bikeName 
, 
	bikePrice 
, 
modelImageUrl $
,$ %
kms& )
,) *

reviewLink+ 5
;5 6
public ,
 ListingEmailtoIndividualTemplate /
(/ 0
string0 6
sellerEmail7 B
,B C
stringD J

sellerNameK U
,U V
stringW ]
	profileNo^ g
,g h
stringi o
bikeNamep x
,x y
string 
	bikePrice 
, 
string $
modelImageUrl% 2
,2 3
string4 :
kms; >
,> ?
string@ F

reviewLinkG Q
)Q R
{ 	
this 
. 
sellerEmail 
= 
sellerEmail *
;* +
this   
.   

sellerName   
=   

sellerName   (
;  ( )
this!! 
.!! 
	profileNo!! 
=!! 
	profileNo!! &
.!!& '
ToUpper!!' .
(!!. /
)!!/ 0
;!!0 1
this"" 
."" 
bikeName"" 
="" 
bikeName"" $
;""$ %
this## 
.## 
	bikePrice## 
=## 
	bikePrice## &
;##& '
this$$ 
.$$ 
modelImageUrl$$ 
=$$  
modelImageUrl$$! .
;$$. /
this%% 
.%% 
kms%% 
=%% 
kms%% 
;%% 
this&& 
.&& 

reviewLink&& 
=&& 

reviewLink&& (
;&&( )
}'' 	
public-- 
override-- 
string-- 
ComposeBody-- *
(--* +
)--+ ,
{.. 	
StringBuilder// 
sb// 
=// 
new// "
StringBuilder//# 0
(//0 1
)//1 2
;//2 3
sb00 
.00 
AppendFormat00 
(00 
string00 "
.00" #
Format00# )
(00) *
$str	00* ´

,
00´
 ¨

DateTime
00≠
 µ

.
00µ
 ∂

Now
00∂
 π

.
00π
 ∫

ToString
00∫
 ¬

(
00¬
 √

$str
00√
 —

)
00—
 “

)
00“
 ”

)
00”
 ‘

;
00‘
 ’

sb11 
.11 
AppendFormat11 
(11 
$str11 l
,11l m

sellerName11n x
)11x y
;11y z
sb22 
.22 
AppendFormat22 
(22 
$str	22 á
,
22á à
bikeName
22â ë
,
22ë í
	profileNo
22ì ú
)
22ú ù
;
22ù û
sb33 
.33 
AppendFormat33 
(33 
$str33 s
)33s t
;33t u
sb44 
.44 
AppendFormat44 
(44 
$str	44 ä
)
44ä ã
;
44ã å
sb55 
.55 
AppendFormat55 
(55 
$str	55 À
)
55À Ã
;
55Ã Õ
sb66 
.66 
AppendFormat66 
(66 
string66 "
.66" #
Format66# )
(66) *
$str	66* ¸
,
66¸ ˝
bikeName
66˛ Ü
,
66Ü á
kms
66à ã
,
66ã å

reviewLink
66ç ó
,
66ó ò
modelImageUrl
66ô ¶
)
66¶ ß
)
66ß ®
;
66® ©
return77 
sb77 
.77 
ToString77 
(77 
)77  
;77  !
}88 	
}99 
}:: ∂
cD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\ListingRejectionEmailToSeller.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public 

class )
ListingRejectionEmailToSeller .
:/ 0
ComposeEmailBase1 A
{ 
private 
string 

sellerName !
,! "
	profileNo 
, 
bikeName 
; 
public )
ListingRejectionEmailToSeller ,
(, -
string- 3

sellerName4 >
,> ?
string@ F
	profileNoG P
,P Q
stringR X
bikeNameY a
)a b
{ 	
this 
. 

sellerName 
= 

sellerName (
;( )
this 
. 
	profileNo 
= 
	profileNo &
.& '
ToUpper' .
(. /
)/ 0
;0 1
this   
.   
bikeName   
=   
bikeName   $
;  $ %
}!! 	
public'' 
override'' 
string'' 
ComposeBody'' *
(''* +
)''+ ,
{(( 	
StringBuilder)) 
sb)) 
=)) 
new)) "
StringBuilder))# 0
())0 1
)))1 2
;))2 3
sb++ 
.++ 
AppendFormat++ 
(++ 
$str	++ ﬁ
,
++ﬁ ﬂ
DateTime
++‡ Ë
.
++Ë È
Now
++È Ï
.
++Ï Ì
ToString
++Ì ı
(
++ı ˆ
$str
++ˆ É	
)
++É	 Ñ	
)
++Ñ	 Ö	
;
++Ö	 Ü	
sb-- 
.-- 
AppendFormat-- 
(-- 
$str	-- â
,
--â ä

sellerName
--ã ï
,
--ï ñ
bikeName
--ó ü
,
--ü †
	profileNo
--° ™
)
--™ ´
;
--´ ¨
sb.. 
... 
AppendFormat.. 
(.. 
$str	.. ´
)
..´ ¨
;
..¨ ≠
sb00 
.00 
AppendFormat00 
(00 
$str	00 Ã
)
00Ã Õ
;
00Õ Œ
sb11 
.11 
AppendFormat11 
(11 
$str	11 À	
)
11À	 Ã	
;
11Ã	 Õ	
return22 
sb22 
.22 
ToString22 
(22 
)22  
;22  !
}33 	
}44 
}55 ú
mD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\PhotoRequestEmailToDealerSellerTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public		 

class		 3
'PhotoRequestEmailToDealerSellerTemplate		 8
:		9 :
ComposeEmailBase		; K
{

 
private 
string 

sellerName !
,! "
	buyerName# ,
,, -
buyerContact. :
,: ;
bikeName< D
,D E
	profileIdF O
;O P
public 3
'PhotoRequestEmailToDealerSellerTemplate 6
(6 7
string7 =

sellerName> H
,H I
stringJ P
	buyerNameQ Z
,Z [
string\ b
buyerContactc o
,o p
stringq w
bikeName	x Ä
,
Ä Å
string
Ç à
	profileId
â í
)
í ì
{ 	
this 
. 

sellerName 
= 

sellerName (
;( )
this 
. 
	buyerName 
= 
	buyerName &
;& '
this 
. 
buyerContact 
= 
buyerContact  ,
;, -
this 
. 
bikeName 
= 
bikeName $
;$ %
this 
. 
	profileId 
= 
	profileId &
;& '
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
StringBuilder 
sb 
= 
new "
StringBuilder# 0
(0 1
)1 2
;2 3
if 
( 

sellerName 
!= 
$str  
&&! #
	buyerName$ -
!=. 0
$str1 3
&&4 6
buyerContact7 C
!=D F
$strG I
&&J L
bikeNameM U
!=V X
$strY [
)[ \
{ 
sb 
. 
AppendFormat 
(  
$str  +
,+ ,

sellerName- 7
)7 8
;8 9
sb 
. 
AppendFormat 
(  
$str	  «
,
« »
	buyerName
… “
,
“ ”
buyerContact
‘ ‡
,
‡ ·
bikeName
‚ Í
,
Í Î
	profileId
Ï ı
)
ı ˆ
;
ˆ ˜
sb 
. 
Append 
( 
$str	 §
)
§ •
;
• ¶
sb 
. 
Append 
( 
$str ^
)^ _
;_ `
sb 
. 
Append 
( 
$str 0
)0 1
;1 2
sb 
. 
Append 
( 
$str 0
)0 1
;1 2
}   
return"" 
sb"" 
."" 
ToString"" 
("" 
)""  
;""  !
}## 	
}$$ 
}%% Î
qD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\PhotoRequestEmailToIndividualSellerTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public		 

class		 7
+PhotoRequestEmailToIndividualSellerTemplate		 <
:		= >
ComposeEmailBase		? O
{

 
private 
string 

sellerName !
,! "
	buyerName# ,
,, -
buyerContact. :
,: ;
bikeName< D
,D E

listingUrlF P
;P Q
public 7
+PhotoRequestEmailToIndividualSellerTemplate :
(: ;
string; A

sellerNameB L
,L M
stringN T
	buyerNameU ^
,^ _
string` f
buyerContactg s
,s t
stringu {
bikeName	| Ñ
,
Ñ Ö
string
Ü å

listingUrl
ç ó
)
ó ò
{ 	
this 
. 

sellerName 
= 

sellerName (
;( )
this 
. 
	buyerName 
= 
	buyerName &
;& '
this 
. 
buyerContact 
= 
buyerContact  ,
;, -
this 
. 
bikeName 
= 
bikeName $
;$ %
this 
. 

listingUrl 
= 

listingUrl (
;( )
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
StringBuilder 
sb 
= 
new "
StringBuilder# 0
(0 1
)1 2
;2 3
if 
( 

sellerName 
!= 
$str  
&&! #
	buyerName$ -
!=. 0
$str1 3
&&4 6
buyerContact7 C
!=D F
$strG I
&&J L
bikeNameM U
!=V X
$strY [
&&\ ^

listingUrl_ i
!=j l
$strm o
)o p
{ 
sb 
. 
AppendFormat 
(  
$str  +
,+ ,

sellerName- 7
)7 8
;8 9
sb 
. 
AppendFormat 
(  
$str	  ´
,
´ ¨
	buyerName
≠ ∂
,
∂ ∑
buyerContact
∏ ƒ
,
ƒ ≈
bikeName
∆ Œ
)
Œ œ
;
œ –
sb 
. 
AppendFormat 
(  
$str  N
,N O

listingUrlP Z
)Z [
;[ \
sb 
. 
Append 
( 
$str	 §
)
§ •
;
• ¶
sb 
. 
Append 
( 
$str ^
)^ _
;_ `
sb 
. 
Append 
( 
$str 0
)0 1
;1 2
sb   
.   
Append   
(   
$str   0
)  0 1
;  1 2
}!! 
return## 
sb## 
.## 
ToString## 
(## 
)##  
;##  !
}$$ 	
}%% 
}&& ß
hD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\PhotoRequestToCustomerForSevenDays.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public		 

class		 .
"PhotoRequestToCustomerForSevenDays		 3
:		4 5
ComposeEmailBase		6 F
{

 
private 
string 
_customerName $
,$ %
_make& +
,+ ,
_model- 3
,3 4

_profileId5 ?
;? @
public .
"PhotoRequestToCustomerForSevenDays 1
(1 2
string2 8
customerName9 E
,E F
stringG M
makeN R
,R S
stringT Z
model[ `
,` a
stringb h
	profileIdi r
)r s
{ 	
_customerName 
= 
customerName (
;( )
_make 
= 
make 
; 
_model 
= 
model 
; 

_profileId 
= 
	profileId "
;" #
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
StringBuilder 
sb 
= 
new "
StringBuilder# 0
(0 1
)1 2
;2 3
if 
( 
! 
string 
. 
IsNullOrEmpty %
(% &
_customerName& 3
)3 4
&&5 7
!8 9
string9 ?
.? @
IsNullOrEmpty@ M
(M N
_makeN S
)S T
&&U W
!X Y
stringY _
._ `
IsNullOrEmpty` m
(m n
_modeln t
)t u
&&v x
!y z
string	z Ä
.
Ä Å
IsNullOrEmpty
Å é
(
é è

_profileId
è ô
)
ô ö
)
ö õ
{ 
sb 
. 
AppendFormat 
(  
$str	   é
,
  é è
_make
  ê ï
,
  ï ñ
_model
  ó ù
,
  ù û

_profileId
  ü ©
,
  © ™
_customerName
  ´ ∏
,
  ∏ π
Bikewale
  ∫ ¬
.
  ¬ √
Utility
  √  
.
    À
BWConfiguration
  À ⁄
.
  ⁄ €
Instance
  € „
.
  „ ‰
	BwHostUrl
  ‰ Ì
)
  Ì Ó
;
  Ó Ô
}!! 
return## 
sb## 
.## 
ToString## 
(## 
)##  
;##  !
}$$ 	
}&& 
}'' ã
hD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\PhotoRequestToCustomerForThreeDays.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public		 

class		 .
"PhotoRequestToCustomerForThreeDays		 3
:		4 5
ComposeEmailBase		6 F
{

 
private 
string 
_customerName $
,$ %
_make& +
,+ ,
_model- 3
,3 4

_profileId5 ?
;? @
public .
"PhotoRequestToCustomerForThreeDays 1
(1 2
string2 8
customerName9 E
,E F
stringG M
makeN R
,R S
stringT Z
model[ `
,` a
stringb h
	profileIdi r
)r s
{ 	
_customerName 
= 
customerName (
;( )
_make 
= 
make 
; 
_model 
= 
model 
; 

_profileId 
= 
	profileId "
;" #
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
StringBuilder 
sb 
= 
new "
StringBuilder# 0
(0 1
)1 2
;2 3
if 
( 
! 
string 
. 
IsNullOrEmpty %
(% &
_customerName& 3
)3 4
&&5 7
!8 9
string9 ?
.? @
IsNullOrEmpty@ M
(M N
_makeN S
)S T
&&U W
!X Y
stringY _
._ `
IsNullOrEmpty` m
(m n
_modeln t
)t u
&&v x
!y z
string	z Ä
.
Ä Å
IsNullOrEmpty
Å é
(
é è

_profileId
è ô
)
ô ö
)
ö õ
{ 
sb 
. 
AppendFormat 
(  
$str!  7
,!!7 8
_make!!9 >
,!!> ?
_model!!@ F
,!!F G

_profileId!!H R
,!!R S
_customerName!!T a
,!!a b
Bikewale!!c k
.!!k l
Utility!!l s
.!!s t
BWConfiguration	!!t É
.
!!É Ñ
Instance
!!Ñ å
.
!!å ç
	BwHostUrl
!!ç ñ
)
!!ñ ó
;
!!ó ò
}"" 
return## 
sb## 
.## 
ToString## 
(## 
)##  
;##  !
}$$ 	
}%% 
}&& á'
iD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\PurchaseInquiryEmailToBuyerTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public 

class /
#PurchaseInquiryEmailToBuyerTemplate 4
:5 6
ComposeEmailBase7 G
{ 
private 
string 
sellerEmail "
," #

sellerName$ .
,. /
sellerContactNo0 ?
,? @
sellerAddressA N
,N O
	profileNoP Y
,Y Z
buyerId[ b
,b c
bikeNamed l
,l m

kilometersn x
,x y
bikeYear	z Ç
,
Ç É
	bikePrice
Ñ ç
,
ç é
	buyerName
è ò
,
ò ô

listingUrl
ö §
;
§ •
public /
#PurchaseInquiryEmailToBuyerTemplate 2
(2 3
string3 9
sellerEmail: E
,E F
stringG M

sellerNameN X
,X Y
string4 :
sellerContactNo; J
,J K
stringL R
sellerAddressS `
,` a
string4 :
	profileNo; D
,D E
stringF L
buyerIdM T
,T U
string4 :
bikeName; C
,C D
stringE K

kilometersL V
,V W
string  4 :
bikeYear  ; C
,  C D
string  E K
	bikePrice  L U
,  U V
string  W ]
	buyerName  ^ g
,  g h
string  i o

listingUrl  p z
)  z {
{!! 	
this"" 
."" 
sellerEmail"" 
="" 
sellerEmail"" *
;""* +
this## 
.## 

sellerName## 
=## 

sellerName## (
;##( )
this$$ 
.$$ 
sellerContactNo$$  
=$$! "
sellerContactNo$$# 2
;$$2 3
this%% 
.%% 
sellerAddress%% 
=%%  
sellerAddress%%! .
;%%. /
this&& 
.&& 
	profileNo&& 
=&& 
	profileNo&& &
;&&& '
this'' 
.'' 
buyerId'' 
='' 
buyerId'' "
;''" #
this(( 
.(( 
bikeName(( 
=(( 
bikeName(( $
;(($ %
this)) 
.)) 

kilometers)) 
=)) 

kilometers)) (
;))( )
this** 
.** 
bikeYear** 
=** 
bikeYear** $
;**$ %
this++ 
.++ 
	bikePrice++ 
=++ 
	bikePrice++ &
;++& '
this,, 
.,, 
	buyerName,, 
=,, 
	buyerName,, &
;,,& '
this-- 
.-- 

listingUrl-- 
=-- 

listingUrl-- (
;--( )
}.. 	
public55 
override55 
string55 
ComposeBody55 *
(55* +
)55+ ,
{66 	
StringBuilder77 
sb77 
=77 
new77 "
StringBuilder77# 0
(770 1
)771 2
;772 3
sb88 
.88 
AppendFormat88 
(88 
$str88 '
,88' (
	buyerName88) 2
)882 3
;883 4
sb99 
.99 
AppendFormat99 
(99 
$str99 U
)99U V
;99V W
sb:: 
.:: 
AppendFormat:: 
(:: 
$str:: z
,::z {
bikeName	::| Ñ
,
::Ñ Ö
	profileNo
::Ü è
,
::è ê

kilometers
::ë õ
,
::õ ú
	bikePrice
::ù ¶
)
::¶ ß
;
::ß ®
sb;; 
.;; 
AppendFormat;; 
(;; 
$str;; p
,;;p q

listingUrl;;r |
);;| }
;;;} ~
sb<< 
.<< 
AppendFormat<< 
(<< 
$str<< [
)<<[ \
;<<\ ]
sb== 
.== 
AppendFormat== 
(== 
$str== R
,==R S

sellerName==T ^
,==^ _
sellerContactNo==` o
,==o p
sellerAddress==q ~
)==~ 
;	== Ä
sb>> 
.>> 
AppendFormat>> 
(>> 
$str>> S
)>>S T
;>>T U
sb?? 
.?? 
AppendFormat?? 
(?? 
$str?? 7
)??7 8
;??8 9
sb@@ 
.@@ 
AppendFormat@@ 
(@@ 
$str@@ +
)@@+ ,
;@@, -
returnAA 
sbAA 
.AA 
ToStringAA 
(AA 
)AA  
;AA  !
}BB 	
}CC 
}DD –'
tD:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UsedBikes\PurchaseInquiryEmailToIndividualSellerTemplate.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
	UsedBikes/ 8
{ 
public		 

class		 :
.PurchaseInquiryEmailToIndividualSellerTemplate		 ?
:		@ A
ComposeEmailBase		B R
{

 
private 
string 
sellerEmail "
," #

sellerName 
, 
	buyerName 
, 

buyerEmail 
, 
buyerContactNo 
, 
	profileNo 
, 
bikeName 
, 
	bikePrice 
; 
public :
.PurchaseInquiryEmailToIndividualSellerTemplate =
(= >
string> D
sellerEmailE P
,P Q
stringR X

sellerNameY c
,c d
stringe k
	buyerNamel u
,u v
stringw }

buyerEmail	~ à
,
à â
string 
buyerContactNo !
,! "
string# )
	profileNo* 3
,3 4
string5 ;
bikeName< D
,D E
stringF L
	bikePriceM V
)V W
{   	
this!! 
.!! 
sellerEmail!! 
=!! 
sellerEmail!! *
;!!* +
this"" 
."" 

sellerName"" 
="" 

sellerName"" (
;""( )
this## 
.## 
	buyerName## 
=## 
	buyerName## &
;##& '
this$$ 
.$$ 

buyerEmail$$ 
=$$ 

buyerEmail$$ (
;$$( )
this%% 
.%% 
buyerContactNo%% 
=%%  !
buyerContactNo%%" 0
;%%0 1
this&& 
.&& 
	profileNo&& 
=&& 
	profileNo&& &
;&&& '
this'' 
.'' 
bikeName'' 
='' 
bikeName'' $
;''$ %
this(( 
.(( 
	bikePrice(( 
=(( 
	bikePrice(( &
;((& '
})) 	
public11 
override11 
string11 
ComposeBody11 *
(11* +
)11+ ,
{22 	
StringBuilder33 
sb33 
=33 
new33 "
StringBuilder33# 0
(330 1
)331 2
;332 3
sb44 
.44 
AppendFormat44 
(44 
$str44 .
,44. /

sellerName440 :
)44: ;
;44; <
sb55 
.55 
AppendFormat55 
(55 
$str55 5
)555 6
;556 7
sb66 
.66 
AppendFormat66 
(66 
$str	66 ã
,
66ã å
	buyerName
66ç ñ
,
66ñ ó
bikeName
66ò †
,
66† °
	profileNo
66¢ ´
,
66´ ¨
	bikePrice
66≠ ∂
)
66∂ ∑
;
66∑ ∏
sb77 
.77 
AppendFormat77 
(77 
$str77 P
,77P Q
	buyerName77R [
)77[ \
;77\ ]
sb88 
.88 
AppendFormat88 
(88 
$str88 V
,88V W
	buyerName88X a
,88a b

buyerEmail88c m
,88m n
buyerContactNo88o }
)88} ~
;88~ 
sb99 
.99 
AppendFormat99 
(99 
$str99 X
)99X Y
;99Y Z
sb:: 
.:: 
AppendFormat:: 
(:: 
$str:: j
)::j k
;::k l
sb;; 
.;; 
AppendFormat;; 
(;; 
$str;; u
);;u v
;;;v w
sb<< 
.<< 
AppendFormat<< 
(<< 
$str<< T
)<<T U
;<<U V
sb== 
.== 
AppendFormat== 
(== 
$str== x
)==x y
;==y z
sb>> 
.>> 
AppendFormat>> 
(>> 
$str>> p
)>>p q
;>>q r
sb?? 
.?? 
AppendFormat?? 
(?? 
$str?? x
,??x y
bikeName	??z Ç
)
??Ç É
;
??É Ñ
sb@@ 
.@@ 
AppendFormat@@ 
(@@ 
$str	@@ ¨
)
@@¨ ≠
;
@@≠ Æ
sbAA 
.AA 
AppendFormatAA 
(AA 
$strAA b
)AAb c
;AAc d
sbBB 
.BB 
AppendFormatBB 
(BB 
$strBB 7
)BB7 8
;BB8 9
sbCC 
.CC 
AppendFormatCC 
(CC 
$strCC +
)CC+ ,
;CC, -
returnDD 
sbDD 
.DD 
ToStringDD 
(DD 
)DD  
;DD  !
}EE 	
}FF 
}GG ˘
[D:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UserReviews\ReviewApprovalEmail.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
UserReviews/ :
{ 
public		 

class		 
ReviewApprovalEmail		 $
:		% &
ComposeEmailBase		' 7
{

 
private 
string 
userName 
,  
	modelName 
, 

reviewLink 
; 
public 
ReviewApprovalEmail "
(" #
string# )
userName* 2
,2 3
string4 :

reviewLink; E
,E F
stringG M
	modelNameN W
)W X
{ 	
this 
. 
userName 
= 
userName $
;$ %
this 
. 

reviewLink 
= 

reviewLink (
;( )
this 
. 
	modelName 
= 
	modelName &
;& '
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
StringBuilder 
sb 
= 
new "
StringBuilder# 0
(0 1
)1 2
;2 3
sb 
. 
AppendFormat 
( 
$str .
,. /
userName0 8
)8 9
;9 :
sb 
. 
AppendFormat 
( 
$str s
,s t
	modelNamet }
)} ~
;~ 
sb   
.   
AppendFormat   
(   
$str   b
)  b c
;  c d
sb!! 
.!! 
AppendFormat!! 
(!! 
$str!! a
)!!a b
;!!b c
sb"" 
."" 
AppendFormat"" 
("" 
$str"" D
,""D E

reviewLink""E O
)""O P
;""P Q
sb## 
.## 
AppendFormat## 
(## 
$str## +
)##+ ,
;##, -
sb$$ 
.$$ 
AppendFormat$$ 
($$ 
$str$$ 2
)$$2 3
;$$3 4
return%% 
sb%% 
.%% 
ToString%% 
(%% 
)%%  
;%%  !
}&& 	
}'' 
}(( Ò
\D:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UserReviews\ReviewRejectionEmail.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
UserReviews/ :
{ 
public

 

class

  
ReviewRejectionEmail

 %
:

& '
ComposeEmailBase

( 8
{ 
private 
string 
userName 
,  
	modelName 
;  
public  
ReviewRejectionEmail #
(# $
string$ *
userName+ 3
,3 4
string5 ;
	modelName< E
)E F
{ 	
this 
. 
userName 
= 
userName $
;$ %
this 
. 
	modelName 
= 
	modelName &
;& '
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
StringBuilder 
sb 
= 
new "
StringBuilder# 0
(0 1
)1 2
;2 3
sb 
. 
AppendFormat 
( 
$str .
,. /
userName/ 7
)7 8
;8 9
sb 
. 
AppendFormat 
( 
$str m
,m n
	modelNamen w
)w x
;x y
sb 
. 
AppendFormat 
( 
$str V
)V W
;W X
sb   
.   
AppendFormat   
(   
$str   U
)  U V
;  V W
sb!! 
.!! 
AppendFormat!! 
(!! 
$str!! d
)!!d e
;!!e f
sb"" 
."" 
AppendFormat"" 
("" 
$str"" z
)""z {
;""{ |
sb## 
.## 
AppendFormat## 
(## 
$str## W
)##W X
;##X Y
sb$$ 
.$$ 
AppendFormat$$ 
($$ 
$str	$$ ä
)
$$ä ã
;
$$ã å
sb%% 
.%% 
AppendFormat%% 
(%% 
$str%% +
)%%+ ,
;%%, -
sb&& 
.&& 
AppendFormat&& 
(&& 
$str&& 2
)&&2 3
;&&3 4
return'' 
sb'' 
.'' 
ToString'' 
('' 
)''  
;''  !
}(( 	
})) 
}** ä
]D:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UserReviews\RatingSubmissionEmail.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
UserReviews/ :
{ 
public

 

class

 !
RatingSubmissionEmail

 &
:

' (
ComposeEmailBase

) 9
{ 
private 
string 
userName 
,  
makeName 
, 
	modelName 
, 

reviewLink 
; 
public !
RatingSubmissionEmail $
($ %
string% +
userName, 4
,4 5
string6 <
makeName= E
,E F
stringG M
	modelNameN W
,W X
stringX ^

reviewLink_ i
)i j
{ 	
this 
. 
userName 
= 
userName $
;$ %
this 
. 
makeName 
= 
makeName $
;$ %
this 
. 
	modelName 
= 
	modelName &
;& '
this 
. 

reviewLink 
= 

reviewLink (
;( )
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
StringBuilder   
sb   
=   
new   "
StringBuilder  # 0
(  0 1
)  1 2
;  2 3
sb!! 
.!! 
AppendFormat!! 
(!! 
$str!! .
,!!. /
userName!!/ 7
)!!7 8
;!!8 9
sb"" 
."" 
AppendFormat"" 
("" 
$str"" Z
,""Z [
makeName""\ d
,""d e
	modelName""f o
)""o p
;""p q
sb## 
.## 
AppendFormat## 
(## 
$str## r
)##r s
;##s t
sb$$ 
.$$ 
AppendFormat$$ 
($$ 
$str$$ j
)$$j k
;$$k l
sb%% 
.%% 
AppendFormat%% 
(%% 
$str%% n
)%%n o
;%%o p
sb&& 
.&& 
AppendFormat&& 
(&& 
$str&& `
,&&` a
makeName&&a i
,&&i j
	modelName&&j s
)&&s t
;&&t u
sb'' 
.'' 
AppendFormat'' 
('' 
$str'' {
)''{ |
;''| }
sb(( 
.(( 
AppendFormat(( 
((( 
$str(( ^
,((^ _
makeName((` h
,((h i
	modelName((j s
,((s t

reviewLink((u 
)	(( Ä
;
((Ä Å
sb)) 
.)) 
AppendFormat)) 
()) 
$str)) s
,))s t
	modelName))u ~
)))~ 
;	)) Ä
sb** 
.** 
AppendFormat** 
(** 
$str	** è
)
**è ê
;
**ê ë
sb++ 
.++ 
AppendFormat++ 
(++ 
$str++ +
)+++ ,
;++, -
sb,, 
.,, 
AppendFormat,, 
(,, 
$str,, 2
),,2 3
;,,3 4
return-- 
sb-- 
.-- 
ToString-- 
(-- 
)--  
;--  !
}.. 	
}// 
}00 Ô
[D:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UserReviews\ReviewReminderEmail.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
UserReviews/ :
{ 
public		 

class		 
ReviewReminderEmail		 $
:		% &
ComposeEmailBase		' 7
{

 
private 
string 
userName 
,  
makeName 
, 
	modelName 
, 

reviewLink 
, 

ratingDate 
; 
public 
ReviewReminderEmail "
(" #
string# )
userName* 2
,2 3
string4 :

reviewLink; E
,E F
stringG M

ratingDateN X
,X Y
stringZ `
makeNamea i
,i j
stringk q
	modelNamer {
){ |
{ 	
this 
. 
userName 
= 
userName $
;$ %
this 
. 
makeName 
= 
makeName $
;$ %
this 
. 
	modelName 
= 
	modelName &
;& '
this 
. 

reviewLink 
= 

reviewLink (
;( )
this 
. 

ratingDate 
= 

ratingDate (
;( )
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{   	
StringBuilder!! 
sb!! 
=!! 
new!! "
StringBuilder!!# 0
(!!0 1
)!!1 2
;!!2 3
sb"" 
."" 
AppendFormat"" 
("" 
$str"" .
,"". /
userName""0 8
)""8 9
;""9 :
sb## 
.## 
AppendFormat## 
(## 
$str## {
,##{ |
	modelName	##} Ü
,
##Ü á

ratingDate
##à í
)
##í ì
;
##ì î
sb$$ 
.$$ 
AppendFormat$$ 
($$ 
$str$$ q
)$$q r
;$$r s
sb%% 
.%% 
AppendFormat%% 
(%% 
$str%% a
,%%a b
makeName%%c k
,%%k l
	modelName%%m v
)%%v w
;%%w x
sb&& 
.&& 
AppendFormat&& 
(&& 
$str&& z
)&&z {
;&&{ |
sb'' 
.'' 
AppendFormat'' 
('' 
$str'' X
,''X Y
makeName''Z b
,''b c
	modelName''d m
,''m n

reviewLink''o y
)''y z
;''z {
sb(( 
.(( 
AppendFormat(( 
((( 
$str(( T
,((T U
	modelName((V _
)((_ `
;((` a
sb)) 
.)) 
AppendFormat)) 
()) 
$str)) +
)))+ ,
;)), -
sb** 
.** 
AppendFormat** 
(** 
$str** 2
)**2 3
;**3 4
return++ 
sb++ 
.++ 
ToString++ 
(++ 
)++  
;++  !
},, 	
}-- 
}.. Î
]D:\work\bikewaleweb\Bikewale.Notifications\MailTemplates\UserReviews\ReviewSubmissionEmail.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MailTemplates! .
.. /
UserReviews/ :
{ 
public

 

class

 !
ReviewSubmissionEmail

 &
:

' (
ComposeEmailBase

) 9
{ 
private 
string 
userName 
,  
makeName 
, 
	modelName 
; 
public !
ReviewSubmissionEmail $
($ %
string% +
userName, 4
,4 5
string6 <
makeName= E
,E F
stringG M
	modelNameN W
)W X
{ 	
this 
. 
userName 
= 
userName $
;$ %
this 
. 
makeName 
= 
makeName $
;$ %
this 
. 
	modelName 
= 
	modelName &
;& '
} 	
public 
override 
string 
ComposeBody *
(* +
)+ ,
{ 	
StringBuilder 
sb 
= 
new "
StringBuilder# 0
(0 1
)1 2
;2 3
sb 
. 
AppendFormat 
( 
$str .
,. /
userName/ 7
)7 8
;8 9
sb   
.   
AppendFormat   
(   
$str   a
,  a b
makeName  c k
,  k l
	modelName  m v
)  v w
;  w x
sb!! 
.!! 
AppendFormat!! 
(!! 
$str	!! Ç
)
!!Ç É
;
!!É Ñ
sb"" 
."" 
AppendFormat"" 
("" 
$str"" _
)""_ `
;""` a
sb## 
.## 
AppendFormat## 
(## 
$str## r
)##r s
;##s t
sb$$ 
.$$ 
AppendFormat$$ 
($$ 
$str$$ ]
)$$] ^
;$$^ _
sb%% 
.%% 
AppendFormat%% 
(%% 
$str%% ~
)%%~ 
;	%% Ä
sb&& 
.&& 
AppendFormat&& 
(&& 
$str&& +
)&&+ ,
;&&, -
sb'' 
.'' 
AppendFormat'' 
('' 
$str'' 2
)''2 3
;''3 4
return(( 
sb(( 
.(( 
ToString(( 
((( 
)((  
;((  !
})) 	
}** 
}++ Ñ
>D:\work\bikewaleweb\Bikewale.Notifications\MySqlDbUtilities.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
MySqlUtility! -
{ 
public 

class 
MySqlDbUtilities !
{ 
public'' 
string'' 
GetInClauseValue'' &
(''& '
string''' -
input''. 3
,''3 4
string''5 ;
	fieldName''< E
,''E F
	DbCommand''G P
cmd''Q T
)''T U
{(( 	
string)) 
[)) 
])) 
inputArr)) 
=)) 
input))  %
.))% &
Split))& +
())+ ,
$char)), /
)))/ 0
;))0 1
string** 
[** 
]** 

parameters** 
=**  !
new**" %
string**& ,
[**, -
inputArr**- 5
.**5 6
Length**6 <
]**< =
;**= >
try++ 
{,, 
for-- 
(-- 
int-- 
i-- 
=-- 
$num-- 
;-- 
i--  !
<--" #
inputArr--$ ,
.--, -
Length--- 3
;--3 4
i--5 6
++--6 8
)--8 9
{.. 
cmd// 
.// 

Parameters// "
.//" #
Add//# &
(//& '
	DbFactory//' 0
.//0 1

GetDbParam//1 ;
(//; <
$str//< ?
+//@ A
	fieldName//B K
+//L M
i//N O
,//O P
DbType//Q W
.//W X
String//X ^
,//^ _
inputArr//` h
[//h i
i//i j
]//j k
.//k l
Length//l r
,//r s
inputArr//t |
[//| }
i//} ~
]//~ 
.	// Ä
ToString
//Ä à
(
//à â
)
//â ä
)
//ä ã
)
//ã å
;
//å ç

parameters00 
[00 
i00  
]00  !
=00" #
$str00$ '
+00( )
	fieldName00* 3
+004 5
i006 7
;007 8
}11 
}22 
catch33 
(33 
	Exception33 
err33  
)33  !
{44 
HttpContext66 
.66 
Current66 #
.66# $
Trace66$ )
.66) *
Warn66* .
(66. /
$str66/ B
+66C D
err66E H
.66H I
Message66I P
+66Q R
err66S V
.66V W
Source66W ]
+66^ _
$str66` r
)66r s
;66s t

ErrorClass77 
objErr77 !
=77" #
new77$ '

ErrorClass77( 2
(772 3
err773 6
,776 7
HttpContext778 C
.77C D
Current77D K
.77K L
Request77L S
.77S T
ServerVariables77T c
[77c d
$str77d i
]77i j
)77j k
;77k l
objErr88 
.88 
SendMail88 
(88  
)88  !
;88! "
}99 
return:: 
string:: 
.:: 
Join:: 
(:: 
$str:: "
,::" #

parameters::$ .
)::. /
;::/ 0
};; 	
}YY 
}ZZ €m
PD:\work\bikewaleweb\Bikewale.Notifications\NotificationDAL\SavePQNotification.cs
	namespace 	
Bikewale
 
. 
Notifications  
.  !
NotificationDAL! 0
{ 
internal		 
class		 
SavePQNotification		 %
{

 
internal 
void #
SaveDealerPQSMSTemplate -
(- .
uint. 2
pqId3 7
,7 8
string9 ?
message@ G
,G H
intI L
smsTypeM T
,T U
stringV \
dealerMobileNo] k
,k l
stringm s
pageUrlt {
){ |
{ 	
try 
{ 
using 
( 
	DbCommand  
cmd! $
=% &
	DbFactory' 0
.0 1
GetDBCommand1 =
(= >
)> ?
)? @
{ 
if 
( 
pqId 
> 
$num  
)  !
{ 
cmd 
. 
CommandText '
=( )
$str* A
;A B
cmd 
. 
CommandType '
=( )
CommandType* 5
.5 6
StoredProcedure6 E
;E F
cmd%% 
.%% 

Parameters%% &
.%%& '
Add%%' *
(%%* +
	DbFactory%%+ 4
.%%4 5

GetDbParam%%5 ?
(%%? @
$str%%@ J
,%%J K
DbType%%L R
.%%R S
Int64%%S X
,%%X Y
pqId%%Z ^
)%%^ _
)%%_ `
;%%` a
cmd&& 
.&& 

Parameters&& &
.&&& '
Add&&' *
(&&* +
	DbFactory&&+ 4
.&&4 5

GetDbParam&&5 ?
(&&? @
$str&&@ X
,&&X Y
DbType&&Z `
.&&` a
String&&a g
,&&g h
message&&i p
)&&p q
)&&q r
;&&r s
cmd'' 
.'' 

Parameters'' &
.''& '
Add''' *
(''* +
	DbFactory''+ 4
.''4 5

GetDbParam''5 ?
(''? @
$str''@ X
,''X Y
DbType''Z `
.''` a
String''a g
,''g h
$num''i l
,''l m
dealerMobileNo''n |
)''| }
)''} ~
;''~ 
cmd(( 
.(( 

Parameters(( &
.((& '
Add((' *
(((* +
	DbFactory((+ 4
.((4 5

GetDbParam((5 ?
(((? @
$str((@ \
,((\ ]
DbType((^ d
.((d e
Byte((e i
,((i j
smsType((k r
)((r s
)((s t
;((t u
cmd)) 
.)) 

Parameters)) &
.))& '
Add))' *
())* +
	DbFactory))+ 4
.))4 5

GetDbParam))5 ?
())? @
$str))@ X
,))X Y
DbType))Z `
.))` a
String))a g
,))g h
$num))i l
,))l m
smsType))n u
)))u v
)))v w
;))w x
MySqlDatabase,, %
.,,% &
ExecuteNonQuery,,& 5
(,,5 6
cmd,,6 9
,,,9 :
ConnectionType,,; I
.,,I J
MasterDatabase,,J X
),,X Y
;,,Y Z
}-- 
}.. 
}// 
catch00 
(00 
	Exception00 
ex00 
)00  
{11 

ErrorClass22 
objErr22 !
=22" #
new22$ '

ErrorClass22( 2
(222 3
ex223 5
,225 6
$str	227 á
)
22á à
;
22à â
objErr33 
.33 
SendMail33 
(33  
)33  !
;33! "
}44 
}55 	
internal@@ 
void@@ %
SaveCustomerPQSMSTemplate@@ /
(@@/ 0
uint@@0 4
pqId@@5 9
,@@9 :
string@@; A
message@@B I
,@@I J
int@@K N
smsType@@O V
,@@V W
string@@X ^
customerMobile@@_ m
,@@m n
string@@o u
pageUrl@@v }
)@@} ~
{AA 	
tryBB 
{CC 
usingDD 
(DD 
	DbCommandDD  
cmdDD! $
=DD% &
	DbFactoryDD' 0
.DD0 1
GetDBCommandDD1 =
(DD= >
)DD> ?
)DD? @
{EE 
ifFF 
(FF 
pqIdFF 
>FF 
$numFF  
)FF  !
{GG 
cmdHH 
.HH 
CommandTextHH '
=HH( )
$strHH* C
;HHC D
cmdII 
.II 
CommandTypeII '
=II( )
CommandTypeII* 5
.II5 6
StoredProcedureII6 E
;IIE F
cmdQQ 
.QQ 

ParametersQQ &
.QQ& '
AddQQ' *
(QQ* +
	DbFactoryQQ+ 4
.QQ4 5

GetDbParamQQ5 ?
(QQ? @
$strQQ@ J
,QQJ K
DbTypeQQL R
.QQR S
Int64QQS X
,QQX Y
pqIdQQZ ^
)QQ^ _
)QQ_ `
;QQ` a
cmdRR 
.RR 

ParametersRR &
.RR& '
AddRR' *
(RR* +
	DbFactoryRR+ 4
.RR4 5

GetDbParamRR5 ?
(RR? @
$strRR@ Z
,RRZ [
DbTypeRR\ b
.RRb c
StringRRc i
,RRi j
messageRRk r
)RRr s
)RRs t
;RRt u
cmdSS 
.SS 

ParametersSS &
.SS& '
AddSS' *
(SS* +
	DbFactorySS+ 4
.SS4 5

GetDbParamSS5 ?
(SS? @
$strSS@ Z
,SSZ [
DbTypeSS\ b
.SSb c
StringSSc i
,SSi j
$numSSk n
,SSn o
customerMobileSSp ~
)SS~ 
)	SS Ä
;
SSÄ Å
cmdTT 
.TT 

ParametersTT &
.TT& '
AddTT' *
(TT* +
	DbFactoryTT+ 4
.TT4 5

GetDbParamTT5 ?
(TT? @
$strTT@ ^
,TT^ _
DbTypeTT` f
.TTf g
ByteTTg k
,TTk l
smsTypeTTm t
)TTt u
)TTu v
;TTv w
cmdUU 
.UU 

ParametersUU &
.UU& '
AddUU' *
(UU* +
	DbFactoryUU+ 4
.UU4 5

GetDbParamUU5 ?
(UU? @
$strUU@ Z
,UUZ [
DbTypeUU\ b
.UUb c
StringUUc i
,UUi j
$numUUk n
,UUn o
smsTypeUUp w
)UUw x
)UUx y
;UUy z
MySqlDatabaseWW %
.WW% &
ExecuteNonQueryWW& 5
(WW5 6
cmdWW6 9
,WW9 :
ConnectionTypeWW; I
.WWI J
MasterDatabaseWWJ X
)WWX Y
;WWY Z
}XX 
}YY 
}ZZ 
catch[[ 
([[ 
	Exception[[ 
ex[[ 
)[[  
{\\ 

ErrorClass]] 
objErr]] !
=]]" #
new]]$ '

ErrorClass]]( 2
(]]2 3
ex]]3 5
,]]5 6
$str	]]7 â
)
]]â ä
;
]]ä ã
objErr^^ 
.^^ 
SendMail^^ 
(^^  
)^^  !
;^^! "
}__ 
}`` 	
internaljj 
voidjj %
SaveDealerPQEmailTemplatejj /
(jj/ 0
uintjj0 4
pqIdjj5 9
,jj9 :
stringjj; A
	emailBodyjjB K
,jjK L
stringjjM S
emailsubjectjjT `
,jj` a
stringjjb h
dealerEmailjji t
)jjt u
{kk 	
tryll 
{mm 
usingnn 
(nn 
	DbCommandnn  
cmdnn! $
=nn% &
	DbFactorynn' 0
.nn0 1
GetDBCommandnn1 =
(nn= >
)nn> ?
)nn? @
{oo 
ifpp 
(pp 
pqIdpp 
>pp 
$numpp  
)pp  !
{qq 
cmdrr 
.rr 
CommandTextrr '
=rr( )
$strrr* C
;rrC D
cmdss 
.ss 
CommandTypess '
=ss( )
CommandTypess* 5
.ss5 6
StoredProceduress6 E
;ssE F
cmdtt 
.tt 

Parameterstt &
.tt& '
Addtt' *
(tt* +
	DbFactorytt+ 4
.tt4 5

GetDbParamtt5 ?
(tt? @
$strtt@ J
,ttJ K
DbTypettL R
.ttR S
Int64ttS X
,ttX Y
pqIdttZ ^
)tt^ _
)tt_ `
;tt` a
cmduu 
.uu 

Parametersuu &
.uu& '
Adduu' *
(uu* +
	DbFactoryuu+ 4
.uu4 5

GetDbParamuu5 ?
(uu? @
$struu@ ^
,uu^ _
DbTypeuu` f
.uuf g
Stringuug m
,uum n
$numuuo t
,uut u
	emailBodyuuv 
)	uu Ä
)
uuÄ Å
;
uuÅ Ç
cmdvv 
.vv 

Parametersvv &
.vv& '
Addvv' *
(vv* +
	DbFactoryvv+ 4
.vv4 5

GetDbParamvv5 ?
(vv? @
$strvv@ Z
,vvZ [
DbTypevv\ b
.vvb c
Stringvvc i
,vvi j
$numvvk n
,vvn o
emailsubjectvvp |
)vv| }
)vv} ~
;vv~ 
cmdww 
.ww 

Parametersww &
.ww& '
Addww' *
(ww* +
	DbFactoryww+ 4
.ww4 5

GetDbParamww5 ?
(ww? @
$strww@ Z
,wwZ [
DbTypeww\ b
.wwb c
Stringwwc i
,wwi j
$numwwk n
,wwn o
dealerEmailwwp {
)ww{ |
)ww| }
;ww} ~
MySqlDatabaseyy %
.yy% &
ExecuteNonQueryyy& 5
(yy5 6
cmdyy6 9
,yy9 :
ConnectionTypeyy; I
.yyI J
MasterDatabaseyyJ X
)yyX Y
;yyY Z
}zz 
}{{ 
}|| 
catch}} 
(}} 
	Exception}} 
ex}} 
)}}  
{~~ 

ErrorClass 
objErr !
=" #
new$ '

ErrorClass( 2
(2 3
ex3 5
,5 6
$str	7 å
)
å ç
;
ç é
objErr
ÄÄ 
.
ÄÄ 
SendMail
ÄÄ 
(
ÄÄ  
)
ÄÄ  !
;
ÄÄ! "
}
ÅÅ 
}
ÇÇ 	
internal
ÜÜ 
void
ÜÜ )
SaveCustomerPQEmailTemplate
ÜÜ 1
(
ÜÜ1 2
uint
ÜÜ2 6
pqId
ÜÜ7 ;
,
ÜÜ; <
string
ÜÜ= C
	emailBody
ÜÜD M
,
ÜÜM N
string
ÜÜO U
emailSubject
ÜÜV b
,
ÜÜb c
string
ÜÜd j
customerEmail
ÜÜk x
)
ÜÜx y
{
áá 	
try
àà 
{
ââ 
using
ää 
(
ää 
	DbCommand
ää  
cmd
ää! $
=
ää% &
	DbFactory
ää' 0
.
ää0 1
GetDBCommand
ää1 =
(
ää= >
)
ää> ?
)
ää? @
{
ãã 
if
åå 
(
åå 
pqId
åå 
>
åå 
$num
åå  
)
åå  !
{
çç 
cmd
éé 
.
éé 
CommandText
éé '
=
éé( )
$str
éé* E
;
ééE F
cmd
èè 
.
èè 
CommandType
èè '
=
èè( )
CommandType
èè* 5
.
èè5 6
StoredProcedure
èè6 E
;
èèE F
cmd
ññ 
.
ññ 

Parameters
ññ &
.
ññ& '
Add
ññ' *
(
ññ* +
	DbFactory
ññ+ 4
.
ññ4 5

GetDbParam
ññ5 ?
(
ññ? @
$str
ññ@ J
,
ññJ K
DbType
ññL R
.
ññR S
Int64
ññS X
,
ññX Y
pqId
ññZ ^
)
ññ^ _
)
ññ_ `
;
ññ` a
cmd
óó 
.
óó 

Parameters
óó &
.
óó& '
Add
óó' *
(
óó* +
	DbFactory
óó+ 4
.
óó4 5

GetDbParam
óó5 ?
(
óó? @
$str
óó@ `
,
óó` a
DbType
óób h
.
óóh i
String
óói o
,
óóo p
	emailBody
óóq z
)
óóz {
)
óó{ |
;
óó| }
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
òò@ \
,
òò\ ]
DbType
òò^ d
.
òòd e
String
òòe k
,
òòk l
$num
òòm p
,
òòp q
emailSubject
òòr ~
)
òò~ 
)òò Ä
;òòÄ Å
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
ôô@ \
,
ôô\ ]
DbType
ôô^ d
.
ôôd e
String
ôôe k
,
ôôk l
$num
ôôm p
,
ôôp q
customerEmail
ôôr 
)ôô Ä
)ôôÄ Å
;ôôÅ Ç
MySqlDatabase
úú %
.
úú% &
ExecuteNonQuery
úú& 5
(
úú5 6
cmd
úú6 9
,
úú9 :
ConnectionType
úú; I
.
úúI J
MasterDatabase
úúJ X
)
úúX Y
;
úúY Z
}
ùù 
}
ûû 
}
üü 
catch
†† 
(
†† 
	Exception
†† 
ex
†† 
)
††  
{
°° 

ErrorClass
¢¢ 
objErr
¢¢ !
=
¢¢" #
new
¢¢$ '

ErrorClass
¢¢( 2
(
¢¢2 3
ex
¢¢3 5
,
¢¢5 6
$str¢¢7 å
)¢¢å ç
;¢¢ç é
objErr
££ 
.
££ 
SendMail
££ 
(
££  
)
££  !
;
££! "
}
§§ 
}
•• 	
}
¶¶ 
}ßß 
ED:\work\bikewaleweb\Bikewale.Notifications\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str 1
)1 2
]2 3
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
$str 3
)3 4
]4 5
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
]$$) *ï¨
JD:\work\bikewaleweb\Bikewale.Notifications\SendEmailSMSToDealerCustomer.cs
	namespace 	
Bikewale
 
. 
Notifications  
{ 
public 

class (
SendEmailSMSToDealerCustomer -
{ 
public$$ 
static$$ 
void$$ 
SendEmailToDealer$$ ,
($$, -
string$$- 3
makeName$$4 <
,$$< =
string$$> D
	modelName$$E N
,$$N O
string$$P V
versionName$$W b
,$$b c
string$$d j

dealerName$$k u
,$$u v
string$$w }
dealerEmail	$$~ â
,
$$â ä
string
$$ã ë
customerName
$$í û
,
$$û ü
string
$$† ¶
customerEmail
$$ß ¥
,
$$¥ µ
string
$$∂ º
customerMobile
$$Ω À
,
$$À Ã
string
$$Õ ”
areaName
$$‘ ‹
,
$$‹ ›
string
$$ﬁ ‰
cityName
$$Â Ì
,
$$Ì Ó
List
$$Ô Û
<
$$Û Ù
PQ_Price
$$Ù ¸
>
$$¸ ˝
	priceList
$$˛ á
,
$$á à
int
$$â å

totalPrice
$$ç ó
,
$$ó ò
List
$$ô ù
<
$$ù û
OfferEntity
$$û ©
>
$$© ™
	offerList
$$´ ¥
,
$$¥ µ
string%% 
	imagePath%% 
)%% 
{&& 	
if'' 
('' 
!'' 
String'' 
.'' 
IsNullOrEmpty'' %
(''% &
dealerEmail''& 1
)''1 2
)''2 3
{(( 
string)) 
[)) 
])) 
arrDealerEmail)) '
=))( )
dealerEmail))* 5
.))5 6
Split))6 ;
()); <
$char))< ?
)))? @
;))@ A
foreach++ 
(++ 
string++ 
email++  %
in++& (
arrDealerEmail++) 7
)++7 8
{,, 
ComposeEmailBase-- $
objEmail--% -
=--. /
new--0 31
%NewBikePriceQuoteMailToDealerTemplate--4 Y
(--Y Z
makeName--Z b
+--c d
$str--e h
+--i j
	modelName--k t
,--t u
versionName	--v Å
,
--Å Ç

dealerName
--É ç
,
--ç é
customerName
--è õ
,
--õ ú
customerEmail.. %
,..% &
customerMobile..' 5
,..5 6
areaName..7 ?
,..? @
cityName..A I
,..I J
	priceList// !
,//! "

totalPrice//# -
,//- .
	offerList/// 8
,//8 9
	imagePath//: C
)//C D
;//D E
objEmail00 
.00 
Send00 !
(00! "
email00" '
,00' (
$str00) G
+00H I
makeName00J R
+00S T
$str00U X
+00Y Z
	modelName00[ d
+00e f
$str00g j
+00k l
versionName00m x
,00x y
customerEmail	00z á
)
00á à
;
00à â
}11 
}22 
}33 	
publicLL 
staticLL 
voidLL 
SendEmailToCustomerLL .
(LL. /
stringLL/ 5
bikeNameLL6 >
,LL> ?
stringLL@ F
	bikeImageLLG P
,LLP Q
stringLLR X

dealerNameLLY c
,LLc d
stringLLe k
dealerEmailLLl w
,LLw x
stringLLy 
dealerMobileNo
LLÄ é
,
LLé è
stringMM 
organizationMM 
,MM  
stringMM! '
addressMM( /
,MM/ 0
stringMM1 7
customerNameMM8 D
,MMD E
stringMMF L
customerEmailMMM Z
,MMZ [
ListMM\ `
<MM` a
PQ_PriceMMa i
>MMi j
	priceListMMk t
,MMt u
ListMMv z
<MMz {
OfferEntity	MM{ Ü
>
MMÜ á
	offerList
MMà ë
,
MMë í
stringNN 
pinCodeNN 
,NN 
stringNN "
	stateNameNN# ,
,NN, -
stringNN. 4
cityNameNN5 =
,NN= >
uintNN? C

totalPriceNND N
,NNN O
stringOO 
versionNameOO 
,OO 
doubleOO  &
	dealerLatOO' 0
,OO0 1
doubleOO2 8

dealerLongOO9 C
,OOC D
stringOOE K
workingHoursOOL X
)OOX Y
{PP 	
ComposeEmailBaseQQ 
objEmailQQ %
=QQ& '
newQQ( +/
#NewBikePriceQuoteToCustomerTemplateQQ, O
(QQO P
bikeNameQQP X
,QQX Y
versionNameQQZ e
,QQe f
	bikeImageQQg p
,QQp q
dealerEmailQQr }
,QQ} ~
dealerMobileNo	QQ ç
,
QQç é
organizationRR 
,RR 
addressRR %
,RR% &
customerNameRR' 3
,RR3 4
	priceListRR5 >
,RR> ?
	offerListRR@ I
,RRI J
pinCodeRRK R
,RRR S
	stateNameRRT ]
,RR] ^
cityNameRR_ g
,RRg h

totalPriceRRi s
,RRs t
	dealerLatRRu ~
,RR~ 

dealerLong
RRÄ ä
,
RRä ã
workingHours
RRå ò
)
RRò ô
;
RRô ö
objEmailSS 
.SS 
SendSS 
(SS 
customerEmailSS '
,SS' (
$strSS) K
+SSL M
bikeNameSSN V
,SSV W
dealerEmailSSX c
)SSc d
;SSd e
}TT 	
publicaa 
staticaa 
voidaa 
SMSToDealeraa &
(aa& '
stringaa' -
dealerMobileaa. :
,aa: ;
stringaa< B
customerNameaaC O
,aaO P
stringaaQ W
customerMobileaaX f
,aaf g
stringaah n
bikeNameaao w
,aaw x
stringaay 
areaName
aaÄ à
,
aaà â
string
aaä ê
cityName
aaë ô
,
aaô ö
string
aaõ °

dealerArea
aa¢ ¨
)
aa¨ ≠
{bb 	
Bikewalecc 
.cc 
Notificationscc "
.cc" #
SMSTypescc# +
objcc, /
=cc0 1
newcc2 5
Bikewalecc6 >
.cc> ?
Notificationscc? L
.ccL M
SMSTypesccM U
(ccU V
)ccV W
;ccW X
objdd 
.dd (
NewBikePriceQuoteSMSToDealerdd ,
(dd, -
dealerMobiledd- 9
,dd9 :
customerNamedd; G
,ddG H
customerMobileddI W
,ddW X
bikeNameddY a
,dda b
areaNameddc k
,ddk l
cityNameddm u
,ddu v
HttpContext	ddw Ç
.
ddÇ É
Current
ddÉ ä
.
ddä ã
Request
ddã í
.
ddí ì
ServerVariables
ddì ¢
[
dd¢ £
$str
dd£ ®
]
dd® ©
.
dd© ™
ToString
dd™ ≤
(
dd≤ ≥
)
dd≥ ¥
,
dd¥ µ

dealerArea
dd∂ ¿
)
dd¿ ¡
;
dd¡ ¬
}ee 	
publicgg 
staticgg 
voidgg 
SMSToCustomergg (
(gg( )!
PQ_DealerDetailEntitygg) >
dealerEntitygg? K
,ggK L
stringggM S
customerMobileggT b
,ggb c
stringggd j
customerNameggk w
,ggw x
stringggy 
BikeName
ggÄ à
,
ggà â
string
ggä ê

dealerName
ggë õ
,
ggõ ú
string
ggù £
dealerContactNo
gg§ ≥
,
gg≥ ¥
string
ggµ ª
dealerAddress
ggº …
,
gg…  
uint
ggÀ œ
bookingAmount
gg– ›
,
gg› ﬁ
uint
ggﬂ „
insuranceAmount
gg‰ Û
=
ggÙ ı
$num
ggˆ ˜
,
gg˜ ¯
bool
gg˘ ˝"
hasBumperDealerOffer
gg˛ í
=
ggì î
false
ggï ö
)
ggö õ
{hh 	
Bikewaleii 
.ii 
Notificationsii "
.ii" #
SMSTypesii# +
objii, /
=ii0 1
newii2 5
Bikewaleii6 >
.ii> ?
Notificationsii? L
.iiL M
SMSTypesiiM U
(iiU V
)iiV W
;iiW X
objjj 
.jj *
NewBikePriceQuoteSMSToCustomerjj .
(jj. /
dealerEntityjj/ ;
,jj; <
customerMobilejj= K
,jjK L
customerNamejjM Y
,jjY Z
BikeNamejj[ c
,jjc d

dealerNamejje o
,jjo p
dealerContactNo	jjq Ä
,
jjÄ Å
dealerAddress
jjÇ è
,
jjè ê
HttpContext
jjë ú
.
jjú ù
Current
jjù §
.
jj§ •
Request
jj• ¨
.
jj¨ ≠
ServerVariables
jj≠ º
[
jjº Ω
$str
jjΩ ¬
]
jj¬ √
.
jj√ ƒ
ToString
jjƒ Ã
(
jjÃ Õ
)
jjÕ Œ
,
jjŒ œ
bookingAmount
jj– ›
,
jj› ﬁ
insuranceAmount
jjﬂ Ó
,
jjÓ Ô"
hasBumperDealerOffer
jj Ñ
)
jjÑ Ö
;
jjÖ Ü
}kk 	
publicmm 
staticmm 
voidmm  
BookingSMSToCustomermm /
(mm/ 0
stringmm0 6
customerMobilemm7 E
,mmE F
stringmmG M
customerNamemmN Z
,mmZ [
stringmm\ b
BikeNamemmc k
,mmk l
stringmmm s

dealerNamemmt ~
,mm~ 
string
mmÄ Ü
dealerContactNo
mmá ñ
,
mmñ ó
string
mmò û
dealerAddress
mmü ¨
,
mm¨ ≠
string
mmÆ ¥
bookingRefNum
mmµ ¬
,
mm¬ √
uint
mmƒ »
insuranceAmount
mm… ÿ
=
mmŸ ⁄
$num
mm€ ‹
)
mm‹ ›
{nn 	
Bikewaleoo 
.oo 
Notificationsoo "
.oo" #
SMSTypesoo# +
objoo, /
=oo0 1
newoo2 5
Bikewaleoo6 >
.oo> ?
Notificationsoo? L
.ooL M
SMSTypesooM U
(ooU V
)ooV W
;ooW X
objpp 
.pp $
BikeBookingSMSToCustomerpp (
(pp( )
customerMobilepp) 7
,pp7 8
customerNamepp9 E
,ppE F
BikeNameppG O
,ppO P

dealerNameppQ [
,pp[ \
dealerContactNopp] l
,ppl m
dealerAddressppn {
,pp{ |
HttpContext	pp} à
.
ppà â
Current
ppâ ê
.
ppê ë
Request
ppë ò
.
ppò ô
ServerVariables
ppô ®
[
pp® ©
$str
pp© Æ
]
ppÆ Ø
.
ppØ ∞
ToString
pp∞ ∏
(
pp∏ π
)
ppπ ∫
,
pp∫ ª
bookingRefNum
ppº …
,
pp…  
insuranceAmount
ppÀ ⁄
)
pp⁄ €
;
pp€ ‹
}qq 	
publicss 
staticss 
voidss 
BookingSMSToDealerss -
(ss- .
stringss. 4
customerMobiless5 C
,ssC D
stringssE K
customerNamessL X
,ssX Y
stringssZ `
BikeNamessa i
,ssi j
stringssk q

dealerNamessr |
,ss| }
string	ss~ Ñ
dealerContactNo
ssÖ î
,
ssî ï
string
ssñ ú
dealerAddress
ssù ™
,
ss™ ´
string
ss¨ ≤
bookingRefNum
ss≥ ¿
,
ss¿ ¡
UInt32
ss¬ »

bookingAmt
ss… ”
,
ss” ‘
uint
ss’ Ÿ
insuranceAmount
ss⁄ È
=
ssÍ Î
$num
ssÏ Ì
)
ssÌ Ó
{tt 	
Bikewaleuu 
.uu 
Notificationsuu "
.uu" #
SMSTypesuu# +
objuu, /
=uu0 1
newuu2 5
Bikewaleuu6 >
.uu> ?
Notificationsuu? L
.uuL M
SMSTypesuuM U
(uuU V
)uuV W
;uuW X
objvv 
.vv "
BikeBookingSMSToDealervv &
(vv& '
dealerContactNovv' 6
,vv6 7
customerNamevv8 D
,vvD E

dealerNamevvF P
,vvP Q
customerMobilevvR `
,vv` a
BikeNamevvb j
,vvj k
HttpContextvvl w
.vvw x
Currentvvx 
.	vv Ä
Request
vvÄ á
.
vvá à
ServerVariables
vvà ó
[
vvó ò
$str
vvò ù
]
vvù û
.
vvû ü
ToString
vvü ß
(
vvß ®
)
vv® ©
,
vv© ™

bookingAmt
vv´ µ
,
vvµ ∂
bookingRefNum
vv∑ ƒ
,
vvƒ ≈
insuranceAmount
vv∆ ’
)
vv’ ÷
;
vv÷ ◊
}ww 	
public}} 
static}} 
void}} #
SMSNoPhotoUploadTwoDays}} 2
(}}2 3
string}}3 9
customerName}}: F
,}}F G
string}}H N
customerMobile}}O ]
,}}] ^
string}}_ e
make}}f j
,}}j k
string}}l r
model}}s x
,}}x y
string	}}z Ä
	profileId
}}Å ä
,
}}ä ã
string
}}å í
editUrl
}}ì ö
)
}}ö õ
{~~ 	
Bikewale 
. 
Notifications "
." #
SMSTypes# +
obj, /
=0 1
new2 5
Bikewale6 >
.> ?
Notifications? L
.L M
SMSTypesM U
(U V
)V W
;W X
obj
ÄÄ 
.
ÄÄ &
SMSForPhotoUploadTwoDays
ÄÄ (
(
ÄÄ( )
customerName
ÄÄ) 5
,
ÄÄ5 6
customerMobile
ÄÄ7 E
,
ÄÄE F
make
ÄÄG K
,
ÄÄK L
model
ÄÄM R
,
ÄÄR S
	profileId
ÄÄT ]
,
ÄÄ] ^
editUrl
ÄÄ_ f
)
ÄÄf g
;
ÄÄg h
}
ÅÅ 	
public
ìì 
static
ìì 
void
ìì $
BookingEmailToCustomer
ìì 1
(
ìì1 2
string
ìì2 8
customerEmail
ìì9 F
,
ììF G
string
ììH N
customerName
ììO [
,
ìì[ \
List
ìì] a
<
ììa b
PQ_Price
ììb j
>
ììj k
	priceList
ììl u
,
ììu v
List
ììw {
<
ìì{ |
OfferEntityìì| á
>ììá à
	offerListììâ í
,ììí ì
stringììî ö"
bookingReferenceNoììõ ≠
,ìì≠ Æ
uintììØ ≥
totalAmountìì¥ ø
,ììø ¿
uintìì¡ ≈ 
preBookingAmountìì∆ ÷
,ìì÷ ◊
stringììÿ ﬁ
makeModelNameììﬂ Ï
,ììÏ Ì
stringììÓ Ù
versionììı ¸
,ìì¸ ˝
stringìì˛ Ñ
colorììÖ ä
,ììä ã
stringììå í
imgììì ñ
,ììñ ó
stringììò û

dealerNameììü ©
,ìì© ™
stringìì´ ±
dealerAddressìì≤ ø
,ììø ¿
stringìì¡ «
dealerMobileìì» ‘
,ìì‘ ’
stringìì÷ ‹
dealerEmailIdìì› Í
,ììÍ Î
stringììÏ Ú!
dealerWorkingTimeììÛ Ñ
,ììÑ Ö
doubleììÜ å
dealerLatitudeììç õ
,ììõ ú
doubleììù £
dealerLongitudeìì§ ≥
)ìì≥ ¥
{
îî 	
ComposeEmailBase
ïï 
objEmail
ïï %
=
ïï& '
new
ïï( +.
 PreBookingConfirmationToCustomer
ïï, L
(
ïïL M
customerName
ïïM Y
,
ïïY Z
	priceList
ïï[ d
,
ïïd e
	offerList
ïïf o
,
ïïo p!
bookingReferenceNoïïq É
,ïïÉ Ñ
totalAmountïïÖ ê
,ïïê ë 
preBookingAmountïïí ¢
,ïï¢ £
makeModelNameïï§ ±
,ïï± ≤
versionïï≥ ∫
,ïï∫ ª
colorïïº ¡
,ïï¡ ¬
imgïï√ ∆
,ïï∆ «

dealerNameïï» “
,ïï“ ”
dealerAddressïï‘ ·
,ïï· ‚
dealerMobileïï„ Ô
,ïïÔ 
dealerEmailIdïïÒ ˛
,ïï˛ ˇ!
dealerWorkingTimeïïÄ ë
,ïïë í
dealerLatitudeïïì °
,ïï° ¢
dealerLongitudeïï£ ≤
)ïï≤ ≥
;ïï≥ ¥
objEmail
ññ 
.
ññ 
Send
ññ 
(
ññ 
customerEmail
ññ '
,
ññ' (
$str
ññ) N
+
ññO P
makeModelName
ññQ ^
,
ññ^ _
$str
ññ` b
)
ññb c
;
ññc d
}
óó 	
public
ÆÆ 
static
ÆÆ 
void
ÆÆ "
BookingEmailToDealer
ÆÆ /
(
ÆÆ/ 0
string
ÆÆ0 6
dealerEmail
ÆÆ7 B
,
ÆÆB C
string
ÆÆD J
customerName
ÆÆK W
,
ÆÆW X
string
ÆÆY _
customerMobile
ÆÆ` n
,
ÆÆn o
string
ÆÆp v
customerAreaÆÆw É
,ÆÆÉ Ñ
stringÆÆÖ ã
customerEmailÆÆå ô
,ÆÆô ö
uintÆÆõ ü

totalPriceÆÆ† ™
,ÆÆ™ ´
uintÆÆ¨ ∞
bookingAmountÆÆ± æ
,ÆÆæ ø
uintÆÆ¿ ƒ
balanceAmountÆÆ≈ “
,ÆÆ“ ”
ListÆÆ‘ ÿ
<ÆÆÿ Ÿ
PQ_PriceÆÆŸ ·
>ÆÆ· ‚
	priceListÆÆ„ Ï
,ÆÆÏ Ì
stringÆÆÓ Ù"
bookingReferenceNoÆÆı á
,ÆÆá à
stringÆÆâ è
bikeNameÆÆê ò
,ÆÆò ô
stringÆÆö †
	bikeColorÆÆ° ™
,ÆÆ™ ´
stringÆÆ¨ ≤

dealerNameÆÆ≥ Ω
,ÆÆΩ æ
stringÆÆø ≈
	imagePathÆÆ∆ œ
,ÆÆœ –
ListÆÆ— ’
<ÆÆ’ ÷
OfferEntityÆÆ÷ ·
>ÆÆ· ‚
	offerListÆÆ„ Ï
,ÆÆÏ Ì
stringÆÆÓ Ù
versionNameÆÆı Ä
)ÆÆÄ Å
{
ØØ 	
if
∞∞ 
(
∞∞ 
!
∞∞ 
String
∞∞ 
.
∞∞ 
IsNullOrEmpty
∞∞ %
(
∞∞% &
dealerEmail
∞∞& 1
)
∞∞1 2
)
∞∞2 3
{
±± 
string
≤≤ 
[
≤≤ 
]
≤≤ 
arrDealerEmail
≤≤ '
=
≤≤( )
dealerEmail
≤≤* 5
.
≤≤5 6
Split
≤≤6 ;
(
≤≤; <
$char
≤≤< ?
)
≤≤? @
;
≤≤@ A
foreach
≥≥ 
(
≥≥ 
string
≥≥ 
email
≥≥  %
in
≥≥& (
arrDealerEmail
≥≥) 7
)
≥≥7 8
{
¥¥ 
ComposeEmailBase
µµ $
objEmail
µµ% -
=
µµ. /
new
µµ0 30
"PreBookingConfirmationMailToDealer
µµ4 V
(
µµV W
customerName
µµW c
,
µµc d
customerMobile
µµe s
,
µµs t
customerAreaµµu Å
,µµÅ Ç
customerEmailµµÉ ê
,µµê ë

totalPriceµµí ú
,µµú ù
bookingAmountµµû ´
,µµ´ ¨
balanceAmountµµ≠ ∫
,µµ∫ ª
	priceListµµº ≈
,µµ≈ ∆"
bookingReferenceNoµµ« Ÿ
,µµŸ ⁄
bikeNameµµ€ „
,µµ„ ‰
	bikeColorµµÂ Ó
,µµÓ Ô

dealerNameµµ ˙
,µµ˙ ˚
	offerListµµ¸ Ö
,µµÖ Ü
	imagePathµµá ê
,µµê ë
versionNameµµí ù
)µµù û
;µµû ü
objEmail
∂∂ 
.
∂∂ 
Send
∂∂ !
(
∂∂! "
email
∂∂" '
,
∂∂' (
$str
∂∂) ;
+
∂∂< =
customerName
∂∂> J
+
∂∂K L
$str
∂∂M Y
+
∂∂Z [
bookingAmount
∂∂\ i
+
∂∂j k
$str
∂∂l s
+
∂∂t u
bikeName
∂∂v ~
+∂∂ Ä
$str∂∂Å Ü
+∂∂á à
	bikeColor∂∂â í
,∂∂í ì
$str∂∂î ñ
)∂∂ñ ó
;∂∂ó ò
}
∑∑ 
}
∏∏ 
}
ππ 	
public
““ 
static
““ 
void
““ "
BookingEmailToDealer
““ /
(
““/ 0
string
““0 6
dealerEmail
““7 B
,
““B C
string
““D J
bikewaleEmail
““K X
,
““X Y
string
““Z `
customerName
““a m
,
““m n
string
““o u
customerMobile““v Ñ
,““Ñ Ö
string““Ü å
customerArea““ç ô
,““ô ö
string““õ °
customerEmail““¢ Ø
,““Ø ∞
uint““± µ

totalPrice““∂ ¿
,““¿ ¡
uint““¬ ∆
bookingAmount““« ‘
,““‘ ’
uint““÷ ⁄
balanceAmount““€ Ë
,““Ë È
List““Í Ó
<““Ó Ô
PQ_Price““Ô ˜
>““˜ ¯
	priceList““˘ Ç
,““Ç É
string““Ñ ä"
bookingReferenceNo““ã ù
,““ù û
string““ü •
bikeName““¶ Æ
,““Æ Ø
string““∞ ∂
	bikeColor““∑ ¿
,““¿ ¡
string““¬ »

dealerName““… ”
,““” ‘
List““’ Ÿ
<““Ÿ ⁄
OfferEntity““⁄ Â
>““Â Ê
	offerList““Á 
,““ Ò
string““Ú ¯
	imagePath““˘ Ç
,““Ç É
string““Ñ ä
versionName““ã ñ
,““ñ ó
uint““ò ú
insuranceAmount““ù ¨
=““≠ Æ
$num““Ø ∞
)““∞ ±
{
”” 	
string
‘‘ 
[
‘‘ 
]
‘‘ 
arrBikeWaleEmail
‘‘ %
=
‘‘& '
null
‘‘( ,
;
‘‘, -
if
’’ 
(
’’ 
!
’’ 
String
’’ 
.
’’ 
IsNullOrEmpty
’’ %
(
’’% &
dealerEmail
’’& 1
)
’’1 2
)
’’2 3
{
÷÷ 
string
◊◊ 
[
◊◊ 
]
◊◊ 
arrDealerEmail
◊◊ '
=
◊◊( )
dealerEmail
◊◊* 5
.
◊◊5 6
Split
◊◊6 ;
(
◊◊; <
$char
◊◊< ?
)
◊◊? @
;
◊◊@ A
arrBikeWaleEmail
ÿÿ  
=
ÿÿ! "
bikewaleEmail
ÿÿ# 0
.
ÿÿ0 1
Split
ÿÿ1 6
(
ÿÿ6 7
$char
ÿÿ7 :
)
ÿÿ: ;
;
ÿÿ; <
foreach
ŸŸ 
(
ŸŸ 
string
ŸŸ 
email
ŸŸ  %
in
ŸŸ& (
arrDealerEmail
ŸŸ) 7
)
ŸŸ7 8
{
⁄⁄ 
ComposeEmailBase
€€ $
objEmail
€€% -
=
€€. /
new
€€0 30
"PreBookingConfirmationMailToDealer
€€4 V
(
€€V W
customerName
€€W c
,
€€c d
customerMobile
€€e s
,
€€s t
customerArea€€u Å
,€€Å Ç
customerEmail€€É ê
,€€ê ë

totalPrice€€í ú
,€€ú ù
bookingAmount€€û ´
,€€´ ¨
balanceAmount€€≠ ∫
,€€∫ ª
	priceList€€º ≈
,€€≈ ∆"
bookingReferenceNo€€« Ÿ
,€€Ÿ ⁄
bikeName€€€ „
,€€„ ‰
	bikeColor€€Â Ó
,€€Ó Ô

dealerName€€ ˙
,€€˙ ˚
	offerList€€¸ Ö
,€€Ö Ü
	imagePath€€á ê
,€€ê ë
versionName€€í ù
,€€ù û
insuranceAmount€€ü Æ
)€€Æ Ø
;€€Ø ∞
objEmail
‹‹ 
.
‹‹ 
Send
‹‹ !
(
‹‹! "
email
‹‹" '
,
‹‹' (
$str
‹‹) ;
+
‹‹< =
customerName
‹‹> J
+
‹‹K L
$str
‹‹M Y
+
‹‹Z [
bookingAmount
‹‹\ i
+
‹‹j k
$str
‹‹l s
+
‹‹t u
bikeName
‹‹v ~
+‹‹ Ä
$str‹‹Å Ü
+‹‹á à
	bikeColor‹‹â í
,‹‹í ì
$str‹‹î ñ
,‹‹ñ ó
null‹‹ò ú
,‹‹ú ù 
arrBikeWaleEmail‹‹û Æ
)‹‹Æ Ø
;‹‹Ø ∞
}
›› 
}
ﬁﬁ 
}
ﬂﬂ 	
public
‰‰ 
static
‰‰ 
void
‰‰ !
SaveEmailToCustomer
‰‰ .
(
‰‰. /
uint
‰‰/ 3
pqId
‰‰4 8
,
‰‰8 9
string
‰‰: @
bikeName
‰‰A I
,
‰‰I J
string
‰‰K Q
	bikeImage
‰‰R [
,
‰‰[ \
string
‰‰] c

dealerName
‰‰d n
,
‰‰n o
string
‰‰p v
dealerEmail‰‰w Ç
,‰‰Ç É
string‰‰Ñ ä
dealerMobileNo‰‰ã ô
,‰‰ô ö
string‰‰õ °
organization‰‰¢ Æ
,‰‰Æ Ø
string‰‰∞ ∂
address‰‰∑ æ
,‰‰æ ø
string‰‰¿ ∆
customerName‰‰« ”
,‰‰” ‘
string‰‰’ €
customerEmail‰‰‹ È
,‰‰È Í
List‰‰Î Ô
<‰‰Ô 
PQ_Price‰‰ ¯
>‰‰¯ ˘
	priceList‰‰˙ É
,‰‰É Ñ
List‰‰Ö â
<‰‰â ä
OfferEntity‰‰ä ï
>‰‰ï ñ
	offerList‰‰ó †
,‰‰† °
string‰‰¢ ®
pinCode‰‰© ∞
,‰‰∞ ±
string‰‰≤ ∏
	stateName‰‰π ¬
,‰‰¬ √
string‰‰ƒ  
cityName‰‰À ”
,‰‰” ‘
uint‰‰’ Ÿ

totalPrice‰‰⁄ ‰
,‰‰‰ Â
string
ÂÂ 
versionName
ÂÂ 
,
ÂÂ 
double
ÂÂ  &
	dealerLat
ÂÂ' 0
,
ÂÂ0 1
double
ÂÂ2 8

dealerLong
ÂÂ9 C
,
ÂÂC D
string
ÂÂE K
workingHours
ÂÂL X
)
ÂÂX Y
{
ÊÊ 	
ComposeEmailBase
ÁÁ 
objEmail
ÁÁ %
=
ÁÁ& '
new
ÁÁ( +1
#NewBikePriceQuoteToCustomerTemplate
ÁÁ, O
(
ÁÁO P
bikeName
ÁÁP X
,
ÁÁX Y
versionName
ÁÁZ e
,
ÁÁe f
	bikeImage
ÁÁg p
,
ÁÁp q
dealerEmail
ÁÁr }
,
ÁÁ} ~
dealerMobileNoÁÁ ç
,ÁÁç é
organization
ËË 
,
ËË 
address
ËË %
,
ËË% &
customerName
ËË' 3
,
ËË3 4
	priceList
ËË5 >
,
ËË> ?
	offerList
ËË@ I
,
ËËI J
pinCode
ËËK R
,
ËËR S
	stateName
ËËT ]
,
ËË] ^
cityName
ËË_ g
,
ËËg h

totalPrice
ËËi s
,
ËËs t
	dealerLat
ËËu ~
,
ËË~ 

dealerLongËËÄ ä
,ËËä ã
workingHoursËËå ò
)ËËò ô
;ËËô ö
string
ÏÏ 
	emailBody
ÏÏ 
=
ÏÏ 
objEmail
ÏÏ '
.
ÏÏ' (
ComposeBody
ÏÏ( 3
(
ÏÏ3 4
)
ÏÏ4 5
;
ÏÏ5 6 
SavePQNotification
ÓÓ 
obj
ÓÓ "
=
ÓÓ# $
new
ÓÓ% ( 
SavePQNotification
ÓÓ) ;
(
ÓÓ; <
)
ÓÓ< =
;
ÓÓ= >
obj
ÔÔ 
.
ÔÔ )
SaveCustomerPQEmailTemplate
ÔÔ +
(
ÔÔ+ ,
pqId
ÔÔ, 0
,
ÔÔ0 1
	emailBody
ÔÔ2 ;
,
ÔÔ; <
$str
ÔÔ= _
+
ÔÔ` a
bikeName
ÔÔb j
,
ÔÔj k
dealerEmail
ÔÔl w
.
ÔÔw x
Split
ÔÔx }
(
ÔÔ} ~
$charÔÔ~ Å
)ÔÔÅ Ç
[ÔÔÇ É
$numÔÔÉ Ñ
]ÔÔÑ Ö
)ÔÔÖ Ü
;ÔÔÜ á
}
ÚÚ 	
public
ÄÄ 
static
ÄÄ 
void
ÄÄ 
SaveSMSToDealer
ÄÄ *
(
ÄÄ* +
uint
ÄÄ+ /
pqId
ÄÄ0 4
,
ÄÄ4 5
string
ÄÄ6 <
dealerMobile
ÄÄ= I
,
ÄÄI J
string
ÄÄK Q
customerName
ÄÄR ^
,
ÄÄ^ _
string
ÄÄ` f
customerMobile
ÄÄg u
,
ÄÄu v
string
ÄÄw }
bikeNameÄÄ~ Ü
,ÄÄÜ á
stringÄÄà é
areaNameÄÄè ó
,ÄÄó ò
stringÄÄô ü
cityNameÄÄ† ®
,ÄÄ® ©
stringÄÄ™ ∞

dealerAreaÄÄ± ª
)ÄÄª º
{
ÅÅ 	
Bikewale
ÇÇ 
.
ÇÇ 
Notifications
ÇÇ "
.
ÇÇ" #
SMSTypes
ÇÇ# +
obj
ÇÇ, /
=
ÇÇ0 1
new
ÇÇ2 5
Bikewale
ÇÇ6 >
.
ÇÇ> ?
Notifications
ÇÇ? L
.
ÇÇL M
SMSTypes
ÇÇM U
(
ÇÇU V
)
ÇÇV W
;
ÇÇW X
obj
ÉÉ 
.
ÉÉ .
 SaveNewBikePriceQuoteSMSToDealer
ÉÉ 0
(
ÉÉ0 1
pqId
ÉÉ1 5
,
ÉÉ5 6
dealerMobile
ÉÉ7 C
,
ÉÉC D
customerName
ÉÉE Q
,
ÉÉQ R
customerMobile
ÉÉS a
,
ÉÉa b
bikeName
ÉÉc k
,
ÉÉk l
areaName
ÉÉm u
,
ÉÉu v
cityName
ÉÉw 
,ÉÉ Ä
HttpContextÉÉÅ å
.ÉÉå ç
CurrentÉÉç î
.ÉÉî ï
RequestÉÉï ú
.ÉÉú ù
ServerVariablesÉÉù ¨
[ÉÉ¨ ≠
$strÉÉ≠ ≤
]ÉÉ≤ ≥
.ÉÉ≥ ¥
ToStringÉÉ¥ º
(ÉÉº Ω
)ÉÉΩ æ
,ÉÉæ ø

dealerAreaÉÉ¿  
)ÉÉ  À
;ÉÉÀ Ã
}
áá 	
public
ââ 
static
ââ 
void
ââ 
SaveSMSToCustomer
ââ ,
(
ââ, -
uint
ââ- 1
pqId
ââ2 6
,
ââ6 7#
PQ_DealerDetailEntity
ââ8 M
dealerEntity
ââN Z
,
ââZ [
string
ââ\ b
customerMobile
ââc q
,
ââq r
string
ââs y
customerNameââz Ü
,ââÜ á
stringââà é
BikeNameââè ó
,ââó ò
stringââô ü

dealerNameââ† ™
,ââ™ ´
stringââ¨ ≤
dealerContactNoââ≥ ¬
,ââ¬ √
stringââƒ  
dealerAddressââÀ ÿ
,ââÿ Ÿ
uintââ⁄ ﬁ
bookingAmountââﬂ Ï
,ââÏ Ì
uintââÓ Ú
insuranceAmountââÛ Ç
=ââÉ Ñ
$numââÖ Ü
,ââÜ á
boolââà å$
hasBumperDealerOfferââç °
=ââ¢ £
falseââ§ ©
)ââ© ™
{
ää 	
Bikewale
ãã 
.
ãã 
Notifications
ãã "
.
ãã" #
SMSTypes
ãã# +
obj
ãã, /
=
ãã0 1
new
ãã2 5
Bikewale
ãã6 >
.
ãã> ?
Notifications
ãã? L
.
ããL M
SMSTypes
ããM U
(
ããU V
)
ããV W
;
ããW X
obj
çç 
.
çç 0
"SaveNewBikePriceQuoteSMSToCustomer
çç 2
(
çç2 3
pqId
çç3 7
,
çç7 8
dealerEntity
çç9 E
,
ççE F
customerMobile
ççG U
,
ççU V
customerName
ççW c
,
ççc d
BikeName
ççe m
,
ççm n

dealerName
çço y
,
ççy z
dealerContactNoçç{ ä
,ççä ã
dealerAddressççå ô
,ççô ö
HttpContextççõ ¶
.çç¶ ß
Currentççß Æ
.ççÆ Ø
RequestççØ ∂
.çç∂ ∑
ServerVariablesçç∑ ∆
[çç∆ «
$strçç« Ã
]ççÃ Õ
.ççÕ Œ
ToStringççŒ ÷
(çç÷ ◊
)çç◊ ÿ
,ççÿ Ÿ
bookingAmountçç⁄ Á
,ççÁ Ë
insuranceAmountççÈ ¯
,çç¯ ˘$
hasBumperDealerOfferçç˙ é
)ççé è
;ççè ê
}
éé 	
public
ûû 
static
ûû 
void
ûû 
SendSMSToCustomer
ûû ,
(
ûû, -
uint
ûû- 1
pqId
ûû2 6
,
ûû6 7
string
ûû8 >

requestUrl
ûû? I
,
ûûI J
DPQSmsEntity
ûûK W
objDPQSmsEntity
ûûX g
,
ûûg h
DPQTypes
ûûi q
DPQType
ûûr y
)
ûûy z
{
üü 	
string
†† 
message
†† 
=
†† 
String
†† #
.
††# $
Empty
††$ )
;
††) *
try
¢¢ 
{
££ 
SMSTypes
§§ 
obj
§§ 
=
§§ 
new
§§ "
SMSTypes
§§# +
(
§§+ ,
)
§§, -
;
§§- .
switch
•• 
(
•• 
DPQType
•• 
)
••  
{
¶¶ 
case
ßß 
DPQTypes
ßß !
.
ßß! "
NoOfferNoBooking
ßß" 2
:
ßß2 3
message
®® 
=
®®  !
String
®®" (
.
®®( )
Format
®®) /
(
®®/ 0
$str®®0 í
,®®í ì
objDPQSmsEntity®®î £
.®®£ §

DealerName®®§ Æ
,®®Æ Ø
objDPQSmsEntity®®∞ ø
.®®ø ¿
Locality®®¿ »
,®®» …
objDPQSmsEntity®®  Ÿ
.®®Ÿ ⁄
DealerMobile®®⁄ Ê
,®®Ê Á
objDPQSmsEntity®®Ë ˜
.®®˜ ¯#
LandingPageShortUrl®®¯ ã
)®®ã å
;®®å ç
break
©© 
;
©© 
case
™™ 
DPQTypes
™™ !
.
™™! ""
NoOfferOnlineBooking
™™" 6
:
™™6 7
message
´´ 
=
´´  !
String
´´" (
.
´´( )
Format
´´) /
(
´´/ 0
$str´´0 ∆
,´´∆ «
objDPQSmsEntity´´» ◊
.´´◊ ÿ
BikeName´´ÿ ‡
,´´‡ ·
objDPQSmsEntity´´‚ Ò
.´´Ò Ú
BookingAmount´´Ú ˇ
,´´ˇ Ä
objDPQSmsEntity´´Å ê
.´´ê ë#
LandingPageShortUrl´´ë §
)´´§ •
;´´• ¶
break
¨¨ 
;
¨¨ 
case
≠≠ 
DPQTypes
≠≠ !
.
≠≠! "
OfferNoBooking
≠≠" 0
:
≠≠0 1
message
ÆÆ 
=
ÆÆ  !
String
ÆÆ" (
.
ÆÆ( )
Format
ÆÆ) /
(
ÆÆ/ 0
$strÆÆ0 ≤
,ÆÆ≤ ≥
objDPQSmsEntityÆÆ¥ √
.ÆÆ√ ƒ
BikeNameÆÆƒ Ã
,ÆÆÃ Õ
objDPQSmsEntityÆÆŒ ›
.ÆÆ› ﬁ

DealerNameÆÆﬁ Ë
,ÆÆË È
objDPQSmsEntityÆÆÍ ˘
.ÆÆ˘ ˙
LocalityÆÆ˙ Ç
,ÆÆÇ É
objDPQSmsEntityÆÆÑ ì
.ÆÆì î#
LandingPageShortUrlÆÆî ß
)ÆÆß ®
;ÆÆ® ©
break
ØØ 
;
ØØ 
case
∞∞ 
DPQTypes
∞∞ !
.
∞∞! "
OfferAndBooking
∞∞" 1
:
∞∞1 2
message
±± 
=
±±  !
String
±±" (
.
±±( )
Format
±±) /
(
±±/ 0
$str±±0 ∑
,±±∑ ∏
objDPQSmsEntity±±π »
.±±» …
BikeName±±… —
,±±— “
objDPQSmsEntity±±” ‚
.±±‚ „#
LandingPageShortUrl±±„ ˆ
)±±ˆ ˜
;±±˜ ¯
break
≤≤ 
;
≤≤ 
case
≥≥ 
DPQTypes
≥≥ !
.
≥≥! "(
AndroidAppNoOfferNoBooking
≥≥" <
:
≥≥< =
case
¥¥ 
DPQTypes
¥¥ !
.
¥¥! "&
AndroidAppOfferNoBooking
¥¥" :
:
¥¥: ;
message
µµ 
=
µµ  !
String
µµ" (
.
µµ( )
Format
µµ) /
(
µµ/ 0
$strµµ0 ò
,µµò ô
objDPQSmsEntityµµö ©
.µµ© ™
CustomerNameµµ™ ∂
,µµ∂ ∑
objDPQSmsEntityµµ∏ «
.µµ« »

DealerNameµµ» “
,µµ“ ”
objDPQSmsEntityµµ‘ „
.µµ„ ‰
Localityµµ‰ Ï
,µµÏ Ì
objDPQSmsEntityµµÓ ˝
.µµ˝ ˛
DealerMobileµµ˛ ä
)µµä ã
;µµã å
break
∂∂ 
;
∂∂ 
case
∑∑ 
DPQTypes
∑∑ !
.
∑∑! "
SubscriptionModel
∑∑" 3
:
∑∑3 4
message
∏∏ 
=
∏∏  !
String
∏∏" (
.
∏∏( )
Format
∏∏) /
(
∏∏/ 0
$str
∏∏0 v
,
∏∏v w
objDPQSmsEntity∏∏x á
.∏∏á à 
OrganisationName∏∏à ò
,∏∏ò ô
objDPQSmsEntity∏∏ö ©
.∏∏© ™
DealerMobile∏∏™ ∂
,∏∏∂ ∑
objDPQSmsEntity∏∏∏ «
.∏∏« »
	DealerAdd∏∏» —
,∏∏— “
objDPQSmsEntity∏∏” ‚
.∏∏‚ „

DealerArea∏∏„ Ì
,∏∏Ì Ó
objDPQSmsEntity∏∏Ô ˛
.∏∏˛ ˇ

DealerCity∏∏ˇ â
)∏∏â ä
;∏∏ä ã
break
ππ 
;
ππ 
}
ªª 
if
ºº 
(
ºº 
objDPQSmsEntity
ºº #
!=
ºº$ &
null
ºº' +
&&
ºº, .
!
ºº/ 0
String
ºº0 6
.
ºº6 7
IsNullOrEmpty
ºº7 D
(
ººD E
objDPQSmsEntity
ººE T
.
ººT U
CustomerMobile
ººU c
)
ººc d
&&
ººe g
!
ººh i
String
ººi o
.
ººo p
IsNullOrEmpty
ººp }
(
ºº} ~
messageºº~ Ö
)ººÖ Ü
&&ººá â
pqIdººä é
>ººè ê
$numººë í
)ººí ì
{
ΩΩ 
	SMSCommon
ææ 
sc
ææ  
=
ææ! "
new
ææ# &
	SMSCommon
ææ' 0
(
ææ0 1
)
ææ1 2
;
ææ2 3
sc
øø 
.
øø 

ProcessSMS
øø !
(
øø! "
objDPQSmsEntity
øø" 1
.
øø1 2
CustomerMobile
øø2 @
,
øø@ A
message
øøB I
,
øøI J 
EnumSMSServiceType
øøK ]
.
øø] ^,
NewBikePriceQuoteSMSToCustomer
øø^ |
,
øø| }

requestUrløø~ à
)øøà â
;øøâ ä
}
¿¿ 
}
¡¡ 
catch
¬¬ 
(
¬¬ 
	Exception
¬¬ 
ex
¬¬ 
)
¬¬  
{
√√ 

ErrorClass
ƒƒ 
objErr
ƒƒ !
=
ƒƒ" #
new
ƒƒ$ '

ErrorClass
ƒƒ( 2
(
ƒƒ2 3
ex
ƒƒ3 5
,
ƒƒ5 6
$str
ƒƒ7 ~
)
ƒƒ~ 
;ƒƒ Ä
objErr
≈≈ 
.
≈≈ 
SendMail
≈≈ 
(
≈≈  
)
≈≈  !
;
≈≈! "
}
∆∆ 
}
«« 	
public
’’ 
static
’’ 
void
’’ /
!UsedBikePhotoRequestEmailToDealer
’’ <
(
’’< =
string
’’= C
dealerEmail
’’D O
,
’’O P
string
’’Q W

sellerName
’’X b
,
’’b c
string
’’d j
	buyerName
’’k t
,
’’t u
string
’’v |
buyerContact’’} â
,’’â ä
string’’ã ë
bikeName’’í ö
,’’ö õ
string’’ú ¢
	profileId’’£ ¨
)’’¨ ≠
{
÷÷ 	
ComposeEmailBase
◊◊ 
objEmail
◊◊ %
=
◊◊& '
new
◊◊( +5
'PhotoRequestEmailToDealerSellerTemplate
◊◊, S
(
◊◊S T

sellerName
◊◊T ^
,
◊◊^ _
	buyerName
◊◊` i
,
◊◊i j
buyerContact
◊◊k w
,
◊◊w x
bikeName◊◊y Å
,◊◊Å Ç
	profileId◊◊É å
)◊◊å ç
;◊◊ç é
objEmail
ÿÿ 
.
ÿÿ 
Send
ÿÿ 
(
ÿÿ 
dealerEmail
ÿÿ %
,
ÿÿ% &
$str
ÿÿ' ;
)
ÿÿ; <
;
ÿÿ< =
}
ŸŸ 	
public
‰‰ 
static
‰‰ 
void
‰‰ 3
%UsedBikePhotoRequestEmailForThreeDays
‰‰ @
(
‰‰@ A
string
‰‰A G
CustomerEmail
‰‰H U
,
‰‰U V
string
‰‰W ]
CustomerName
‰‰^ j
,
‰‰j k
string
‰‰l r
Make
‰‰s w
,
‰‰w x
string
‰‰y 
Model‰‰Ä Ö
,‰‰Ö Ü
string‰‰á ç
	profileId‰‰é ó
)‰‰ó ò
{
ÂÂ 	
ComposeEmailBase
ÊÊ 
objEmail
ÊÊ %
=
ÊÊ& '
new
ÊÊ( +0
"PhotoRequestToCustomerForThreeDays
ÊÊ, N
(
ÊÊN O
CustomerName
ÊÊO [
,
ÊÊ[ \
Make
ÊÊ] a
,
ÊÊa b
Model
ÊÊc h
,
ÊÊh i
	profileId
ÊÊj s
)
ÊÊs t
;
ÊÊt u
objEmail
ÁÁ 
.
ÁÁ 
Send
ÁÁ 
(
ÁÁ 
CustomerEmail
ÁÁ '
,
ÁÁ' (
$str
ÁÁ) X
)
ÁÁX Y
;
ÁÁY Z
}
ËË 	
public
ÛÛ 
static
ÛÛ 
void
ÛÛ 3
%UsedBikePhotoRequestEmailForSevenDays
ÛÛ @
(
ÛÛ@ A
string
ÛÛA G
CustomerEmail
ÛÛH U
,
ÛÛU V
string
ÛÛW ]
CustomerName
ÛÛ^ j
,
ÛÛj k
string
ÛÛl r
Make
ÛÛs w
,
ÛÛw x
string
ÛÛy 
ModelÛÛÄ Ö
,ÛÛÖ Ü
stringÛÛá ç
	profileIdÛÛé ó
)ÛÛó ò
{
ÙÙ 	
ComposeEmailBase
ıı 
objEmail
ıı %
=
ıı& '
new
ıı( +0
"PhotoRequestToCustomerForSevenDays
ıı, N
(
ııN O
CustomerName
ııO [
,
ıı[ \
Make
ıı] a
,
ııa b
Model
ııc h
,
ııh i
	profileId
ııj s
)
ııs t
;
ııt u
objEmail
ˆˆ 
.
ˆˆ 
Send
ˆˆ 
(
ˆˆ 
CustomerEmail
ˆˆ '
,
ˆˆ' (
$str
ˆˆ) X
)
ˆˆX Y
;
ˆˆY Z
}
˜˜ 	
public
ÉÉ 
static
ÉÉ 
void
ÉÉ 3
%UsedBikePhotoRequestEmailToIndividual
ÉÉ @
(
ÉÉ@ A 
CustomerEntityBase
ÉÉA S
seller
ÉÉT Z
,
ÉÉZ [ 
CustomerEntityBase
ÉÉ\ n
buyer
ÉÉo t
,
ÉÉt u
string
ÉÉv |
bikeNameÉÉ} Ö
,ÉÉÖ Ü
stringÉÉá ç

listingUrlÉÉé ò
)ÉÉò ô
{
ÑÑ 	
ComposeEmailBase
ÖÖ 
objEmail
ÖÖ %
=
ÖÖ& '
new
ÖÖ( +9
+PhotoRequestEmailToIndividualSellerTemplate
ÖÖ, W
(
ÖÖW X
seller
ÖÖX ^
.
ÖÖ^ _
CustomerName
ÖÖ_ k
,
ÖÖk l
buyer
ÖÖm r
.
ÖÖr s
CustomerName
ÖÖs 
,ÖÖ Ä
buyerÖÖÅ Ü
.ÖÖÜ á
CustomerMobileÖÖá ï
,ÖÖï ñ
bikeNameÖÖó ü
,ÖÖü †

listingUrlÖÖ° ´
)ÖÖ´ ¨
;ÖÖ¨ ≠
objEmail
ÜÜ 
.
ÜÜ 
Send
ÜÜ 
(
ÜÜ 
seller
ÜÜ  
.
ÜÜ  !
CustomerEmail
ÜÜ! .
,
ÜÜ. /
$str
ÜÜ0 D
)
ÜÜD E
;
ÜÜE F
}
áá 	
public
óó 
static
óó 
void
óó 1
#UsedBikePurchaseInquiryEmailToBuyer
óó >
(
óó> ? 
CustomerEntityBase
óó? Q
seller
óóR X
,
óóX Y 
CustomerEntityBase
óóZ l
buyer
óóm r
,
óór s
string
óót z
sellerAddressóó{ à
,óóà â
stringóóä ê
	profileIdóóë ö
,óóö õ
stringóóú ¢
bikeNameóó£ ´
,óó´ ¨
stringóó≠ ≥

kilometersóó¥ æ
,óóæ ø
stringóó¿ ∆
makeYearóó« œ
,óóœ –
stringóó— ◊
formattedPriceóóÿ Ê
,óóÊ Á
stringóóË Ó

listingUrlóóÔ ˘
)óó˘ ˙
{
òò 	
ComposeEmailBase
ôô 
objEmail
ôô %
=
ôô& '
new
ôô( +1
#PurchaseInquiryEmailToBuyerTemplate
ôô, O
(
ôôO P
seller
öö 
.
öö 
CustomerEmail
öö $
,
öö$ %
seller
õõ 
.
õõ 
CustomerName
õõ #
,
õõ# $
seller
úú 
.
úú 
CustomerMobile
úú %
,
úú% &
sellerAddress
ùù 
,
ùù 
	profileId
ûû 
,
ûû 
buyer
üü 
.
üü 

CustomerId
üü  
.
üü  !
ToString
üü! )
(
üü) *
)
üü* +
,
üü+ ,
bikeName
†† 
,
†† 

kilometers
°° 
,
°° 
makeYear
¢¢ 
,
¢¢ 
formattedPrice
££ 
,
££ 
buyer
§§ 
.
§§ 
CustomerName
§§ "
,
•• 

listingUrl
•• 
)
¶¶ 
;
¶¶ 
objEmail
ßß 
.
ßß 
Send
ßß 
(
ßß 
buyer
ßß 
.
ßß  
CustomerEmail
ßß  -
,
ßß- .
String
ßß/ 5
.
ßß5 6
Format
ßß6 <
(
ßß< =
$str
ßß= Z
,
ßßZ [
	profileId
ßß\ e
)
ßße f
)
ßßf g
;
ßßg h
}
®® 	
public
µµ 
static
µµ 
void
µµ 6
(UsedBikePurchaseInquiryEmailToIndividual
µµ C
(
µµC D 
CustomerEntityBase
µµD V
seller
µµW ]
,
µµ] ^ 
CustomerEntityBase
µµ_ q
buyer
µµr w
,
µµw x
string
µµy 
	profileIdµµÄ â
,µµâ ä
stringµµã ë
bikeNameµµí ö
,µµö õ
stringµµú ¢
formattedPriceµµ£ ±
)µµ± ≤
{
∂∂ 	
ComposeEmailBase
∑∑ 
objEmail
∑∑ %
=
∑∑& '
new
∑∑( +<
.PurchaseInquiryEmailToIndividualSellerTemplate
∑∑, Z
(
∑∑Z [
seller
∑∑[ a
.
∑∑a b
CustomerEmail
∑∑b o
,
∑∑o p
seller
∑∑q w
.
∑∑w x
CustomerName∑∑x Ñ
,∑∑Ñ Ö
buyer∑∑Ü ã
.∑∑ã å
CustomerName∑∑å ò
,∑∑ò ô
buyer∑∑ö ü
.∑∑ü †
CustomerEmail∑∑† ≠
,∑∑≠ Æ
buyer∑∑Ø ¥
.∑∑¥ µ
CustomerMobile∑∑µ √
,∑∑√ ƒ
	profileId∑∑≈ Œ
,∑∑Œ œ
bikeName∑∑– ÿ
,∑∑ÿ Ÿ
formattedPrice∑∑⁄ Ë
)∑∑Ë È
;∑∑È Í
objEmail
∏∏ 
.
∏∏ 
Send
∏∏ 
(
∏∏ 
seller
∏∏  
.
∏∏  !
CustomerEmail
∏∏! .
,
∏∏. /
$str
∏∏0 U
)
∏∏U V
;
∏∏V W
}
ππ 	
public
ƒƒ 
static
ƒƒ 
void
ƒƒ /
!UsedBikeApprovalEmailToIndividual
ƒƒ <
(
ƒƒ< = 
CustomerEntityBase
ƒƒ= O
seller
ƒƒP V
,
ƒƒV W
string
ƒƒX ^
	profileId
ƒƒ_ h
,
ƒƒh i
string
ƒƒj p
bikeName
ƒƒq y
,
ƒƒy z
DateTimeƒƒ{ É
makeYearƒƒÑ å
,ƒƒå ç
stringƒƒé î
ownerƒƒï ö
,ƒƒö õ
stringƒƒú ¢
distanceƒƒ£ ´
,ƒƒ´ ¨
stringƒƒ¨ ≤
cityƒƒ≥ ∑
,ƒƒ∑ ∏
stringƒƒπ ø
imgPathƒƒ¿ «
,ƒƒ« »
intƒƒ» À
	inquiryIdƒƒÃ ’
,ƒƒ’ ÷
stringƒƒ÷ ‹
	bwHostUrlƒƒ› Ê
,ƒƒÊ Á
uintƒƒÁ Î
modelIdƒƒÏ Û
,ƒƒÛ Ù
stringƒƒı ˚
qEncodedƒƒ¸ Ñ
)ƒƒÑ Ö
{
≈≈ 	
ComposeEmailBase
∆∆ 
objEmail
∆∆ %
=
∆∆& '
new
∆∆( +*
ListingApprovalEmailToSeller
∆∆, H
(
∆∆H I
seller
∆∆I O
.
∆∆O P
CustomerName
∆∆P \
,
∆∆\ ]
	profileId
∆∆^ g
,
∆∆g h
bikeName
∆∆i q
,
∆∆q r
makeYear
∆∆r z
,
∆∆z {
owner∆∆{ Ä
,∆∆Ä Å
distance∆∆Å â
,∆∆â ä
city∆∆ä é
,∆∆é è
imgPath∆∆è ñ
,∆∆ñ ó
	inquiryId∆∆ó †
,∆∆† °
	bwHostUrl∆∆° ™
,∆∆™ ´
modelId∆∆´ ≤
,∆∆≤ ≥
qEncoded∆∆≥ ª
)∆∆ª º
;∆∆º Ω
objEmail
«« 
.
«« 
Send
«« 
(
«« 
seller
««  
.
««  !
CustomerEmail
««! .
,
««. /
String
««0 6
.
««6 7
Format
««7 =
(
««= >
$str
««> t
,
««t u
bikeName
««v ~
)
««~ 
)«« Ä
;««Ä Å
}
»» 	
public
”” 
static
”” 
void
”” ,
UsedBikeRejectionEmailToSeller
”” 9
(
””9 : 
CustomerEntityBase
””: L
seller
””M S
,
””S T
string
””U [
	profileId
””\ e
,
””e f
string
””g m
bikeName
””n v
)
””v w
{
‘‘ 	
ComposeEmailBase
’’ 
objEmail
’’ %
=
’’& '
new
’’( ++
ListingRejectionEmailToSeller
’’, I
(
’’I J
seller
’’J P
.
’’P Q
CustomerName
’’Q ]
,
’’] ^
	profileId
’’_ h
,
’’h i
bikeName
’’j r
)
’’r s
;
’’s t
objEmail
÷÷ 
.
÷÷ 
Send
÷÷ 
(
÷÷ 
seller
÷÷  
.
÷÷  !
CustomerEmail
÷÷! .
,
÷÷. /
String
÷÷0 6
.
÷÷6 7
Format
÷÷7 =
(
÷÷= >
$str
÷÷> r
,
÷÷r s
bikeName
÷÷t |
)
÷÷| }
)
÷÷} ~
;
÷÷~ 
}
◊◊ 	
public
„„ 
static
„„ 
void
„„ 1
#UsedBikeEditedApprovalEmailToSeller
„„ >
(
„„> ? 
CustomerEntityBase
„„? Q
seller
„„R X
,
„„X Y
string
„„Z `
	profileId
„„a j
,
„„j k
string
„„l r
bikeName
„„s {
,
„„{ |
string„„} É

modelImage„„Ñ é
,„„é è
string„„ê ñ
kms„„ó ö
,„„ö õ
string„„ú ¢
writeReviewLink„„£ ≤
)„„≤ ≥
{
‰‰ 	
ComposeEmailBase
ÂÂ 
objEmail
ÂÂ %
=
ÂÂ& '
new
ÂÂ( +0
"EditedListingApprovalEmailToSeller
ÂÂ, N
(
ÂÂN O
seller
ÂÂO U
.
ÂÂU V
CustomerName
ÂÂV b
,
ÂÂb c
	profileId
ÂÂd m
,
ÂÂm n
bikeName
ÂÂo w
,
ÂÂw x

modelImageÂÂy É
,ÂÂÉ Ñ
kmsÂÂÖ à
,ÂÂà â
writeReviewLinkÂÂä ô
)ÂÂô ö
;ÂÂö õ
objEmail
ÊÊ 
.
ÊÊ 
Send
ÊÊ 
(
ÊÊ 
seller
ÊÊ  
.
ÊÊ  !
CustomerEmail
ÊÊ! .
,
ÊÊ. /
String
ÊÊ0 6
.
ÊÊ6 7
Format
ÊÊ7 =
(
ÊÊ= >
$strÊÊ> Ä
,ÊÊÄ Å
bikeNameÊÊÇ ä
)ÊÊä ã
)ÊÊã å
;ÊÊå ç
}
ÁÁ 	
public
ÓÓ 
static
ÓÓ 
void
ÓÓ '
CustomerRegistrationEmail
ÓÓ 4
(
ÓÓ4 5
string
ÓÓ5 ;
customerEmail
ÓÓ< I
,
ÓÓI J
string
ÓÓK Q
customerName
ÓÓR ^
,
ÓÓ^ _
string
ÓÓ` f
password
ÓÓg o
)
ÓÓo p
{
ÔÔ 	
ComposeEmailBase
 
objEmail
 %
=
& '
new
( +.
 CustomerRegistrationMailTemplate
, L
(
L M
customerEmail
M Z
,
Z [
customerName
\ h
,
h i
password
j r
)
r s
;
s t
objEmail
ÒÒ 
.
ÒÒ 
Send
ÒÒ 
(
ÒÒ 
customerEmail
ÒÒ '
,
ÒÒ' (
$str
ÒÒ) A
)
ÒÒA B
;
ÒÒB C
}
ÚÚ 	
public
˝˝ 
static
˝˝ 
void
˝˝ 2
$UsedBikeEditedRejectionEmailToSeller
˝˝ ?
(
˝˝? @ 
CustomerEntityBase
˝˝@ R
seller
˝˝S Y
,
˝˝Y Z
string
˝˝[ a
	profileId
˝˝b k
,
˝˝k l
string
˝˝m s
bikeName
˝˝t |
,
˝˝| }
string˝˝~ Ñ

modelImage˝˝Ö è
,˝˝è ê
string˝˝ë ó
kms˝˝ò õ
)˝˝õ ú
{
˛˛ 	
ComposeEmailBase
ˇˇ 
objEmail
ˇˇ %
=
ˇˇ& '
new
ˇˇ( +1
#EditedListingRejectionEmailToSeller
ˇˇ, O
(
ˇˇO P
seller
ˇˇP V
.
ˇˇV W
CustomerName
ˇˇW c
,
ˇˇc d
	profileId
ˇˇe n
,
ˇˇn o
bikeName
ˇˇp x
,
ˇˇx y

modelImageˇˇz Ñ
,ˇˇÑ Ö
kmsˇˇÜ â
)ˇˇâ ä
;ˇˇä ã
objEmail
ÄÄ 
.
ÄÄ 
Send
ÄÄ 
(
ÄÄ 
seller
ÄÄ  
.
ÄÄ  !
CustomerEmail
ÄÄ! .
,
ÄÄ. /
String
ÄÄ0 6
.
ÄÄ6 7
Format
ÄÄ7 =
(
ÄÄ= >
$strÄÄ> Å
,ÄÄÅ Ç
bikeNameÄÄÉ ã
)ÄÄã å
)ÄÄå ç
;ÄÄç é
}
ÅÅ 	
public
åå 
static
åå 
void
åå )
UsedBikeAdEmailToIndividual
åå 6
(
åå6 7 
CustomerEntityBase
åå7 I
seller
ååJ P
,
ååP Q
string
ååR X
	profileId
ååY b
,
ååb c
string
ååd j
bikeName
ååk s
,
åås t
string
ååu {
formattedPriceåå| ä
,ååä ã
stringååå í
modelImageUrlååì †
,åå† °
stringåå¢ ®
kmsåå© ¨
,åå¨ ≠
stringååÆ ¥

reviewLinkååµ ø
)ååø ¿
{
çç 	
ComposeEmailBase
éé 
objEmail
éé %
=
éé& '
new
éé( +.
 ListingEmailtoIndividualTemplate
éé, L
(
ééL M
seller
ééM S
.
ééS T
CustomerEmail
ééT a
,
ééa b
seller
ééc i
.
ééi j
CustomerName
ééj v
,
éév w
	profileIdééx Å
,ééÅ Ç
bikeNameééÉ ã
,ééã å
formattedPriceééç õ
,ééõ ú
modelImageUrlééù ™
,éé™ ´
kmséé¨ Ø
,ééØ ∞

reviewLinkéé± ª
)ééª º
;ééº Ω
objEmail
èè 
.
èè 
Send
èè 
(
èè 
seller
èè  
.
èè  !
CustomerEmail
èè! .
,
èè. /
String
èè0 6
.
èè6 7
Format
èè7 =
(
èè= >
$str
èè> w
,
èèw x
bikeNameèèy Å
)èèÅ Ç
)èèÇ É
;èèÉ Ñ
}
êê 	
}
ëë 
}íí åB
7D:\work\bikewaleweb\Bikewale.Notifications\SendMails.cs
	namespace 	
Bikewale
 
. 
Notifications  
{ 
public 

class 
	SendMails 
{ 

SmtpClient 
client 
= 
null  
;  !
MailAddress 
from 
= 
null 
;  
public 
string 
	localMail 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
ReplyTo 
{ 
get  #
;# $
set% (
;( )
}* +
public 
	SendMails 
( 
) 
{ 	'
ConfigureCommonMailSettings '
(' (
)( )
;) *
} 	
public%% 
void%% 
SendMail%% 
(%% 
string%% #
email%%$ )
,%%) *
string%%+ 1
subject%%2 9
,%%9 :
string%%; A
body%%B F
)%%F G
{&& 	!
ConfigureMailSettings'' !
(''! "
email''" '
,''' (
subject'') 0
,''0 1
body''2 6
,''6 7
ReplyTo''8 ?
,''? @
null''A E
,''E F
null''G K
)''K L
;''L M
})) 	
public44 
void44 
SendMail44 
(44 
string44 #
email44$ )
,44) *
string44+ 1
subject442 9
,449 :
string44; A
body44B F
,44F G
string44H N
replyTo44O V
)44V W
{55 	!
ConfigureMailSettings66 !
(66! "
email66" '
,66' (
subject66) 0
,660 1
body662 6
,666 7
replyTo668 ?
,66? @
null66A E
,66E F
null66G K
)66K L
;66L M
}88 	
publicEE 
voidEE 
SendMailEE 
(EE 
stringEE #
emailEE$ )
,EE) *
stringEE+ 1
subjectEE2 9
,EE9 :
stringEE; A
bodyEEB F
,EEF G
stringEEH N
replyToEEO V
,EEV W
stringEEX ^
[EE^ _
]EE_ `
ccEEa c
,EEc d
stringEEe k
[EEk l
]EEl m
bccEEn q
)EEq r
{FF 	!
ConfigureMailSettingsGG !
(GG! "
emailGG" '
,GG' (
subjectGG) 0
,GG0 1
bodyGG2 6
,GG6 7
replyToGG8 ?
,GG? @
ccGGA C
,GGC D
bccGGE H
)GGH I
;GGI J
}II 	
privatePP 
voidPP '
ConfigureCommonMailSettingsPP 0
(PP0 1
)PP1 2
{QQ 	
tryRR 
{SS 
clientUU 
=UU 
newUU 

SmtpClientUU '
(UU' (
MailConfigurationUU( 9
.UU9 :

SMTPSERVERUU: D
)UUD E
;UUE F
	localMailXX 
=XX 
MailConfigurationXX -
.XX- .
	LOCALMAILXX. 7
;XX7 8
from[[ 
=[[ 
new[[ 
MailAddress[[ &
([[& '
	localMail[[' 0
,[[0 1
MailConfiguration[[2 C
.[[C D
MAILFROM[[D L
)[[L M
;[[M N
ReplyTo]] 
=]] 
MailConfiguration]] +
.]]+ ,
REPLYTO]], 3
;]]3 4
}^^ 
catch__ 
(__ 
	Exception__ 
err__  
)__  !
{`` 

ErrorClassaa 
objErraa !
=aa" #
newaa$ '

ErrorClassaa( 2
(aa2 3
erraa3 6
,aa6 7
$straa8 o
)aao p
;aap q
}bb 
}cc 	
privatepp 
voidpp !
ConfigureMailSettingspp *
(pp* +
stringpp+ 1
emailpp2 7
,pp7 8
stringpp9 ?
subjectpp@ G
,ppG H
stringppI O
bodyppP T
,ppT U
stringppV \
replyTopp] d
,ppd e
stringppf l
[ppl m
]ppm n
ccppo q
,ppq r
stringpps y
[ppy z
]ppz {
bccpp| 
)	pp Ä
{qq 	
tryss 
{tt 
MailAddressvv 
tovv 
=vv  
newvv! $
MailAddressvv% 0
(vv0 1
emailvv1 6
)vv6 7
;vv7 8
MailMessageyy 
msgyy 
=yy  !
newyy" %
MailMessageyy& 1
(yy1 2
fromyy2 6
,yy6 7
toyy8 :
)yy: ;
;yy; <
msg|| 
.|| 
Headers|| 
.|| 
Add|| 
(||  
$str||  *
,||* +
String||, 2
.||2 3
IsNullOrEmpty||3 @
(||@ A
replyTo||A H
)||H I
?||J K
ReplyTo||L S
:||T U
replyTo||V ]
)||] ^
;||^ _
if 
( 
cc 
!= 
null 
&& !
cc" $
.$ %
Length% +
>, -
$num. /
)/ 0
{
ÄÄ 
MailAddress
ÅÅ 
addCC
ÅÅ  %
=
ÅÅ& '
null
ÅÅ( ,
;
ÅÅ, -
for
ÉÉ 
(
ÉÉ 
int
ÉÉ 
iTmp
ÉÉ !
=
ÉÉ" #
$num
ÉÉ$ %
;
ÉÉ% &
iTmp
ÉÉ' +
<
ÉÉ, -
cc
ÉÉ. 0
.
ÉÉ0 1
Length
ÉÉ1 7
;
ÉÉ7 8
iTmp
ÉÉ9 =
++
ÉÉ= ?
)
ÉÉ? @
{
ÑÑ 
addCC
ÖÖ 
=
ÖÖ 
new
ÖÖ  #
MailAddress
ÖÖ$ /
(
ÖÖ/ 0
cc
ÖÖ0 2
[
ÖÖ2 3
iTmp
ÖÖ3 7
]
ÖÖ7 8
)
ÖÖ8 9
;
ÖÖ9 :
msg
ÜÜ 
.
ÜÜ 
CC
ÜÜ 
.
ÜÜ 
Add
ÜÜ "
(
ÜÜ" #
addCC
ÜÜ# (
)
ÜÜ( )
;
ÜÜ) *
}
áá 
}
àà 
if
ãã 
(
ãã 
bcc
ãã 
!=
ãã 
null
ãã 
&&
ãã  "
bcc
ãã# &
.
ãã& '
Length
ãã' -
>
ãã. /
$num
ãã0 1
)
ãã1 2
{
åå 
MailAddress
çç 
addBCC
çç  &
=
çç' (
null
çç) -
;
çç- .
for
èè 
(
èè 
int
èè 
iTmp
èè !
=
èè" #
$num
èè$ %
;
èè% &
iTmp
èè' +
<
èè, -
bcc
èè. 1
.
èè1 2
Length
èè2 8
;
èè8 9
iTmp
èè: >
++
èè> @
)
èè@ A
{
êê 
addBCC
ëë 
=
ëë  
new
ëë! $
MailAddress
ëë% 0
(
ëë0 1
bcc
ëë1 4
[
ëë4 5
iTmp
ëë5 9
]
ëë9 :
)
ëë: ;
;
ëë; <
msg
íí 
.
íí 
Bcc
íí 
.
íí  
Add
íí  #
(
íí# $
addBCC
íí$ *
)
íí* +
;
íí+ ,
}
ìì 
}
îî 
msg
óó 
.
óó 

IsBodyHtml
óó 
=
óó  
true
óó! %
;
óó% &
msg
òò 
.
òò 
Priority
òò 
=
òò 
MailPriority
òò +
.
òò+ ,
High
òò, 0
;
òò0 1
msg
õõ 
.
õõ 
Subject
õõ 
=
õõ 
subject
õõ %
;
õõ% &
msg
ûû 
.
ûû 
Body
ûû 
=
ûû 
body
ûû 
;
ûû  
client
°° 
.
°° 
Send
°° 
(
°° 
msg
°° 
)
°°  
;
°°  !
}
££ 
catch
§§ 
(
§§ 
	Exception
§§ 
err
§§  
)
§§  !
{
•• 

ErrorClass
¶¶ 
objErr
¶¶ !
=
¶¶" #
new
¶¶$ '

ErrorClass
¶¶( 2
(
¶¶2 3
err
¶¶3 6
,
¶¶6 7
string
¶¶8 >
.
¶¶> ?
Format
¶¶? E
(
¶¶E F
$str
¶¶F z
,
¶¶z {
email¶¶| Å
)¶¶Å Ç
)¶¶Ç É
;¶¶É Ñ
}
ßß 
}
®® 	
}
™™ 
}´´ Ûù
;D:\work\bikewaleweb\Bikewale.Notifications\SMS\SMSCommon.cs
	namespace 	
Bikewale
 
. 
Notifications  
{ 
public 

enum 
EnumSMSServiceType "
{ /
#UsedPurchaseInquiryIndividualSeller +
=, -
$num. /
,/ 0+
UsedPurchaseInquiryDealerSeller '
=( )
$num* +
,+ ,.
"UsedPurchaseInquiryIndividualBuyer *
=+ ,
$num- .
,. /*
UsedPurchaseInquiryDealerBuyer &
=' (
$num) *
,* +
NewBikeSellOpr 
= 
$num 
, 
NewBikeBuyOpr 
= 
$num 
, 
AccountDetails 
= 
$num 
,  
CustomerRegistration 
= 
$num  
,  ! 
DealerAddressRequest 
= 
$num  
,  !
SellInquiryReminder 
= 
$num  
,  !
PromotionalOffer 
= 
$num 
, 
NewBikeQuote 
= 
$num 
, 
	CustomSMS 
= 
$num 
, 
CustomerPassword   
=   
$num   
,   
MobileVerification!! 
=!! 
$num!! 
,!!  
PaidRenewalAlert"" 
="" 
$num"" 
,"" 
PaidRenewal## 
=## 
$num## 
,## 
MyGarageSMS$$ 
=$$ 
$num$$ 
,$$ 
MyGarageAlerts%% 
=%% 
$num%% 
,%% 
InsuranceRenewal&& 
=&& 
$num&& 
,&& (
NewBikePriceQuoteSMSToDealer'' $
=''% &
$num''' )
,'') **
NewBikePriceQuoteSMSToCustomer(( &
=((' (
$num(() +
,((+ ,#
BikeBookedSMSToCustomer)) 
=))  !
$num))" $
,))$ %!
BikeBookedSMSToDealer** 
=** 
$num**  "
,**" #
RSAFreeHelmetSMS++ 
=++ 
$num++ 
,++ "
LimitedBikeBookedOffer,, 
=,,  
$num,,! #
,,,# $
ClaimedOffer-- 
=-- 
$num-- 
,-- "
BookingCancellationOTP.. 
=..  
$num..! #
,..# $)
BookingCancellationToCustomer// %
=//& '
$num//( *
,//* +-
!SuccessfulUsedSelllistingToSeller00 )
=00* +
$num00, .
,00. /+
ApprovalUsedSellListingToSeller11 '
=11( )
$num11* ,
,11, -,
 RejectionUsedSellListingToSeller22 (
=22) *
$num22+ -
,22- .1
%ApprovalEditedUsedBikeListingToSeller33 -
=33. /
$num330 2
,332 32
&RejectionEditedUsedBikeListingToSeller44 .
=44/ 0
$num441 3
,443 4-
!ServiceCenterDetailsSMSToCustomer55 )
=55* +
$num55, .
,55. /0
$BikeListingExpirySevenDaySMSToSeller66 ,
=66- .
$num66/ 1
,661 2.
"BikeListingExpiryOneDaySMSToSeller77 *
=77+ ,
$num77- /
,77/ 0$
SMSForPhotoUploadTwoDays88  
=88! "
$num88# %
}99 
public;; 

class;; 
	SMSCommon;; 
{<< 
publicEE 
voidEE 

ProcessSMSEE 
(EE 
stringEE %
numbersEE& -
,EE- .
stringEE/ 5
messageEE6 =
,EE= >
EnumSMSServiceTypeEE? Q
esmsEER V
,EEV W
stringEEX ^
pageUrlEE_ f
)EEf g
{FF 	
ifGG 
(GG 
!GG 
StringGG 
.GG 
IsNullOrEmptyGG %
(GG% &
numbersGG& -
)GG- .
)GG. /
{HH 
stringII 
[II 
]II 
arrMobileNosII %
=II& '
numbersII( /
.II/ 0
SplitII0 5
(II5 6
$charII6 9
)II9 :
;II: ;
foreachKK 
(KK 
stringKK 
numberKK  &
inKK' )
arrMobileNosKK* 6
)KK6 7
{LL 

ProcessSMSMM 
(MM 
numberMM %
,MM% &
messageMM' .
,MM. /
esmsMM0 4
,MM4 5
pageUrlMM6 =
,MM= >
trueMM? C
)MMC D
;MMD E
}NN 
}OO 
}PP 	
publicRR 
voidRR 

ProcessSMSRR 
(RR 
stringRR %
numberRR& ,
,RR, -
stringRR. 4
messageRR5 <
,RR< =
EnumSMSServiceTypeRR> P
esmsRRQ U
,RRU V
stringRRW ]
pageUrlRR^ e
,RRe f
boolRRg k
isDNDRRl q
)RRq r
{SS 	
stringXX 
mobileXX 
=XX 
ParseMobileNumberXX -
(XX- .
numberXX. 4
)XX4 5
;XX5 6
ifZZ 
(ZZ 
mobileZZ 
!=ZZ 
$strZZ 
)ZZ 
{[[ 
if\\ 
(\\ 
isDND\\ 
==\\ 
true\\ !
)\\! "
mobile]] 
=]] 
$str]] !
+]]" #
mobile]]$ *
;]]* +
string__ 
retMsg__ 
=__ 
$str__  "
;__" #
string`` 
ctId`` 
=`` 
$str`` "
;``" #
boolaa 
statusaa 
=aa 
falseaa #
;aa# $
boolbb 
isMSMQbb 
=bb 
falsebb #
;bb# $
ifdd 
(dd 
!dd 
Stringdd 
.dd 
IsNullOrEmptydd )
(dd) * 
ConfigurationManagerdd* >
.dd> ?
AppSettingsdd? J
[ddJ K
$strddK S
]ddS T
)ddT U
)ddU V
{ee 
isMSMQff 
=ff 
Convertff $
.ff$ %
	ToBooleanff% .
(ff. / 
ConfigurationManagerff/ C
.ffC D
AppSettingsffD O
[ffO P
$strffP X
]ffX Y
.ffY Z
ToStringffZ b
(ffb c
)ffc d
)ffd e
;ffe f
}gg 
ctIdii 
=ii 
SaveSMSSentDataii &
(ii& '
mobileii' -
,ii- .
messageii/ 6
,ii6 7
esmsii8 <
,ii< =
statusii> D
,iiD E
retMsgiiF L
,iiL M
pageUrliiN U
)iiU V
;iiV W
NameValueCollectionkk #
nvckk$ '
=kk( )
newkk* -
NameValueCollectionkk. A
(kkA B
)kkB C
;kkC D
nvcll 
.ll 
Addll 
(ll 
$strll 
,ll 
ctIdll "
)ll" #
;ll# $
nvcmm 
.mm 
Addmm 
(mm 
$strmm !
,mm! "
messagemm# *
)mm* +
;mm+ ,
nvcnn 
.nn 
Addnn 
(nn 
$strnn "
,nn" #
mobilenn$ *
)nn* +
;nn+ ,
nvcoo 
.oo 
Addoo 
(oo 
$stroo  
,oo  !
$stroo" &
)oo& '
;oo' (
nvcpp 
.pp 
Addpp 
(pp 
$strpp "
,pp" #
$strpp$ &
)pp& '
;pp' (
RabbitMqPublishrr 
publishrr  '
=rr( )
newrr* -
RabbitMqPublishrr. =
(rr= >
)rr> ?
;rr? @
publishss 
.ss 
PublishToQueuess &
(ss& '
BWConfigurationss' 6
.ss6 7
Instancess7 ?
.ss? @

BWSmsQueuess@ J
,ssJ K
nvcssL O
)ssO P
;ssP Q
}tt 
}uu 	
public
ÅÅ 
void
ÅÅ  
ProcessPrioritySMS
ÅÅ &
(
ÅÅ& '
string
ÅÅ' -
number
ÅÅ. 4
,
ÅÅ4 5
string
ÅÅ6 <
message
ÅÅ= D
,
ÅÅD E 
EnumSMSServiceType
ÅÅF X
esms
ÅÅY ]
,
ÅÅ] ^
string
ÅÅ_ e
pageUrl
ÅÅf m
,
ÅÅm n
bool
ÅÅo s
isDND
ÅÅt y
)
ÅÅy z
{
ÇÇ 	
string
áá 
mobile
áá 
=
áá 
ParseMobileNumber
áá -
(
áá- .
number
áá. 4
)
áá4 5
;
áá5 6
if
ââ 
(
ââ 
!
ââ 
String
ââ 
.
ââ 
IsNullOrEmpty
ââ %
(
ââ% &
mobile
ââ& ,
)
ââ, -
)
ââ- .
{
ää 
if
ãã 
(
ãã 
isDND
ãã 
==
ãã 
true
ãã !
)
ãã! "
mobile
åå 
=
åå 
$str
åå !
+
åå" #
mobile
åå$ *
;
åå* +
string
éé 
retMsg
éé 
=
éé 
string
éé  &
.
éé& '
Empty
éé' ,
;
éé, -
string
èè 
ctId
èè 
=
èè 
$str
èè "
;
èè" #
bool
êê 
status
êê 
=
êê 
false
êê #
;
êê# $
bool
ëë 
isMSMQ
ëë 
=
ëë 
false
ëë #
;
ëë# $
if
ìì 
(
ìì 
!
ìì 
String
ìì 
.
ìì 
IsNullOrEmpty
ìì )
(
ìì) *
BWConfiguration
ìì* 9
.
ìì9 :
Instance
ìì: B
.
ììB C
IsMSMQ
ììC I
)
ììI J
)
ììJ K
{
îî 
isMSMQ
ïï 
=
ïï 
Convert
ïï $
.
ïï$ %
	ToBoolean
ïï% .
(
ïï. /
BWConfiguration
ïï/ >
.
ïï> ?
Instance
ïï? G
.
ïïG H
IsMSMQ
ïïH N
)
ïïN O
;
ïïO P
}
ññ 
ctId
òò 
=
òò 
SaveSMSSentData
òò &
(
òò& '
mobile
òò' -
,
òò- .
message
òò/ 6
,
òò6 7
esms
òò8 <
,
òò< =
status
òò> D
,
òòD E
retMsg
òòF L
,
òòL M
pageUrl
òòN U
)
òòU V
;
òòV W!
NameValueCollection
öö #
nvc
öö$ '
=
öö( )
new
öö* -!
NameValueCollection
öö. A
(
ööA B
)
ööB C
;
ööC D
nvc
õõ 
.
õõ 
Add
õõ 
(
õõ 
$str
õõ 
,
õõ 
ctId
õõ "
)
õõ" #
;
õõ# $
nvc
úú 
.
úú 
Add
úú 
(
úú 
$str
úú !
,
úú! "
message
úú# *
)
úú* +
;
úú+ ,
nvc
ùù 
.
ùù 
Add
ùù 
(
ùù 
$str
ùù "
,
ùù" #
mobile
ùù$ *
)
ùù* +
;
ùù+ ,
nvc
ûû 
.
ûû 
Add
ûû 
(
ûû 
$str
ûû  
,
ûû  !
$str
ûû" &
)
ûû& '
;
ûû' (
nvc
üü 
.
üü 
Add
üü 
(
üü 
$str
üü "
,
üü" #
$str
üü$ &
)
üü& '
;
üü' (
RabbitMqPublish
°° 
publish
°°  '
=
°°( )
new
°°* -
RabbitMqPublish
°°. =
(
°°= >
)
°°> ?
;
°°? @
publish
¢¢ 
.
¢¢ 
PublishToQueue
¢¢ &
(
¢¢& '
BWConfiguration
¢¢' 6
.
¢¢6 7
Instance
¢¢7 ?
.
¢¢? @ 
BWPrioritySmsQueue
¢¢@ R
,
¢¢R S
nvc
¢¢T W
)
¢¢W X
;
¢¢X Y
}
££ 
}
§§ 	
private
ÆÆ 
void
ÆÆ 
UpdateSMSSentData
ÆÆ &
(
ÆÆ& '
string
ÆÆ' -
	currentId
ÆÆ. 7
,
ÆÆ7 8
string
ÆÆ9 ?
retMsg
ÆÆ@ F
)
ÆÆF G
{
ØØ 	
if
±± 
(
±± 
!
±± 
String
±± 
.
±± 
IsNullOrEmpty
±± %
(
±±% &
	currentId
±±& /
)
±±/ 0
)
±±0 1
{
≤≤ 
string
≥≥ 
sql
≥≥ 
=
≥≥ 
$str
≥≥ ]
;
≥≥] ^
try
¥¥ 
{
µµ 
using
∂∂ 
(
∂∂ 
	DbCommand
∂∂ $
cmd
∂∂% (
=
∂∂) *
	DbFactory
∂∂+ 4
.
∂∂4 5
GetDBCommand
∂∂5 A
(
∂∂A B
sql
∂∂B E
)
∂∂E F
)
∂∂F G
{
∑∑ 
cmd
∏∏ 
.
∏∏ 

Parameters
∏∏ &
.
∏∏& '
Add
∏∏' *
(
∏∏* +
	DbFactory
∏∏+ 4
.
∏∏4 5

GetDbParam
∏∏5 ?
(
∏∏? @
$str
∏∏@ L
,
∏∏L M
DbType
∏∏N T
.
∏∏T U
Int32
∏∏U Z
,
∏∏Z [
Convert
∏∏\ c
.
∏∏c d
ToInt32
∏∏d k
(
∏∏k l
	currentId
∏∏l u
)
∏∏u v
)
∏∏v w
)
∏∏w x
;
∏∏x y
cmd
ππ 
.
ππ 

Parameters
ππ &
.
ππ& '
Add
ππ' *
(
ππ* +
	DbFactory
ππ+ 4
.
ππ4 5

GetDbParam
ππ5 ?
(
ππ? @
$str
ππ@ I
,
ππI J
DbType
ππK Q
.
ππQ R
String
ππR X
,
ππX Y
retMsg
ππZ `
)
ππ` a
)
ππa b
;
ππb c
cmd
ºº 
.
ºº 
ExecuteNonQuery
ºº +
(
ºº+ ,
)
ºº, -
;
ºº- .
}
ΩΩ 
}
ææ 
catch
øø 
(
øø 
	Exception
øø  
err
øø! $
)
øø$ %
{
¿¿ 

ErrorClass
¡¡ 
objErr
¡¡ %
=
¡¡& '
new
¡¡( +

ErrorClass
¡¡, 6
(
¡¡6 7
err
¡¡7 :
,
¡¡: ;
$str
¡¡< ^
)
¡¡^ _
;
¡¡_ `
objErr
¬¬ 
.
¬¬ 
SendMail
¬¬ #
(
¬¬# $
)
¬¬$ %
;
¬¬% &
}
√√ 
}
ƒƒ 
}
≈≈ 	
string
«« 
SaveSMSSentData
«« 
(
«« 
string
«« %
number
««& ,
,
««, -
string
««. 4
message
««5 <
,
««< = 
EnumSMSServiceType
««> P
esms
««Q U
,
««U V
bool
««W [
status
««\ b
,
««b c
string
««d j
retMsg
««k q
,
««q r
string
««s y
pageUrl««z Å
)««Å Ç
{
»» 	
string
…… 
	currentId
…… 
=
…… 
string
…… %
.
……% &
Empty
……& +
;
……+ ,
try
   
{
ÀÀ 
using
ÕÕ 
(
ÕÕ 
	DbCommand
ÕÕ  
cmd
ÕÕ! $
=
ÕÕ% &
	DbFactory
ÕÕ' 0
.
ÕÕ0 1
GetDBCommand
ÕÕ1 =
(
ÕÕ= >
$str
ÕÕ> M
)
ÕÕM N
)
ÕÕN O
{
ŒŒ 
cmd
œœ 
.
œœ 
CommandType
œœ #
=
œœ$ %
CommandType
œœ& 1
.
œœ1 2
StoredProcedure
œœ2 A
;
œœA B
cmd
—— 
.
—— 

Parameters
—— "
.
——" #
Add
——# &
(
——& '
	DbFactory
——' 0
.
——0 1

GetDbParam
——1 ;
(
——; <
$str
——< H
,
——H I
DbType
——J P
.
——P Q
String
——Q W
,
——W X
$num
——Y [
,
——[ \
number
——] c
)
——c d
)
——d e
;
——e f
cmd
““ 
.
““ 

Parameters
““ "
.
““" #
Add
““# &
(
““& '
	DbFactory
““' 0
.
““0 1

GetDbParam
““1 ;
(
““; <
$str
““< I
,
““I J
DbType
““K Q
.
““Q R
String
““R X
,
““X Y
$num
““Z ]
,
““] ^
message
““_ f
)
““f g
)
““g h
;
““h i
cmd
”” 
.
”” 

Parameters
”” "
.
””" #
Add
””# &
(
””& '
	DbFactory
””' 0
.
””0 1

GetDbParam
””1 ;
(
””; <
$str
””< M
,
””M N
DbType
””O U
.
””U V
Int32
””V [
,
””[ \
esms
””] a
)
””a b
)
””b c
;
””c d
cmd
‘‘ 
.
‘‘ 

Parameters
‘‘ "
.
‘‘" #
Add
‘‘# &
(
‘‘& '
	DbFactory
‘‘' 0
.
‘‘0 1

GetDbParam
‘‘1 ;
(
‘‘; <
$str
‘‘< Q
,
‘‘Q R
DbType
‘‘S Y
.
‘‘Y Z
DateTime
‘‘Z b
,
‘‘b c
DateTime
‘‘d l
.
‘‘l m
Now
‘‘m p
)
‘‘p q
)
‘‘q r
;
‘‘r s
cmd
’’ 
.
’’ 

Parameters
’’ "
.
’’" #
Add
’’# &
(
’’& '
	DbFactory
’’' 0
.
’’0 1

GetDbParam
’’1 ;
(
’’; <
$str
’’< M
,
’’M N
DbType
’’O U
.
’’U V
Boolean
’’V ]
,
’’] ^
status
’’_ e
)
’’e f
)
’’f g
;
’’g h
cmd
÷÷ 
.
÷÷ 

Parameters
÷÷ "
.
÷÷" #
Add
÷÷# &
(
÷÷& '
	DbFactory
÷÷' 0
.
÷÷0 1

GetDbParam
÷÷1 ;
(
÷÷; <
$str
÷÷< M
,
÷÷M N
DbType
÷÷O U
.
÷÷U V
String
÷÷V \
,
÷÷\ ]
$num
÷÷^ a
,
÷÷a b
retMsg
÷÷c i
)
÷÷i j
)
÷÷j k
;
÷÷k l
cmd
◊◊ 
.
◊◊ 

Parameters
◊◊ "
.
◊◊" #
Add
◊◊# &
(
◊◊& '
	DbFactory
◊◊' 0
.
◊◊0 1

GetDbParam
◊◊1 ;
(
◊◊; <
$str
◊◊< L
,
◊◊L M
DbType
◊◊N T
.
◊◊T U
String
◊◊U [
,
◊◊[ \
$num
◊◊] `
,
◊◊` a
pageUrl
◊◊b i
)
◊◊i j
)
◊◊j k
;
◊◊k l
	currentId
ŸŸ 
=
ŸŸ 
Convert
ŸŸ  '
.
ŸŸ' (
ToString
ŸŸ( 0
(
ŸŸ0 1
MySqlDatabase
ŸŸ1 >
.
ŸŸ> ?
ExecuteScalar
ŸŸ? L
(
ŸŸL M
cmd
ŸŸM P
,
ŸŸP Q
ConnectionType
ŸŸR `
.
ŸŸ` a
MasterDatabase
ŸŸa o
)
ŸŸo p
)
ŸŸp q
;
ŸŸq r
}
⁄⁄ 
}
€€ 
catch
‹‹ 
(
‹‹ 
SqlException
‹‹ 
err
‹‹  #
)
‹‹# $
{
›› 

ErrorClass
ﬁﬁ 
objErr
ﬁﬁ !
=
ﬁﬁ" #
new
ﬁﬁ$ '

ErrorClass
ﬁﬁ( 2
(
ﬁﬁ2 3
err
ﬁﬁ3 6
,
ﬁﬁ6 7
$str
ﬁﬁ8 Z
)
ﬁﬁZ [
;
ﬁﬁ[ \
objErr
ﬂﬂ 
.
ﬂﬂ 
SendMail
ﬂﬂ 
(
ﬂﬂ  
)
ﬂﬂ  !
;
ﬂﬂ! "
}
‡‡ 
catch
·· 
(
·· 
	Exception
·· 
err
··  
)
··  !
{
‚‚ 

ErrorClass
„„ 
objErr
„„ !
=
„„" #
new
„„$ '

ErrorClass
„„( 2
(
„„2 3
err
„„3 6
,
„„6 7
$str
„„8 Z
)
„„Z [
;
„„[ \
objErr
‰‰ 
.
‰‰ 
SendMail
‰‰ 
(
‰‰  
)
‰‰  !
;
‰‰! "
}
ÂÂ 
return
ÊÊ 
	currentId
ÊÊ 
;
ÊÊ 
}
ÁÁ 	
string
ÈÈ 
ParseMobileNumber
ÈÈ  
(
ÈÈ  !
string
ÈÈ! '
input
ÈÈ( -
)
ÈÈ- .
{
ÍÍ 	
return
ÎÎ 
Bikewale
ÎÎ 
.
ÎÎ 
Utility
ÎÎ #
.
ÎÎ# $
CommonValidators
ÎÎ$ 4
.
ÎÎ4 5
ParseMobileNumber
ÎÎ5 F
(
ÎÎF G
input
ÎÎG L
)
ÎÎL M
;
ÎÎM N
}
ÏÏ 	
}
ÌÌ 
}ÓÓ ¡ÿ
:D:\work\bikewaleweb\Bikewale.Notifications\SMS\SMSTypes.cs
	namespace 	
Bikewale
 
. 
Notifications  
{ 
public 

class 
SMSTypes 
{ 
public 
void 
RegistrationSMS #
(# $
string$ *
name+ /
,/ 0
string1 7
password8 @
,@ A
stringB H
numberI O
,O P
stringQ W
eMailX ]
,] ^
string_ e
pageUrlf m
)m n
{ 	
string 
message 
= 
String #
.# $
Empty$ )
;) *
try 
{ 
message 
= 
String  
.  !
Format! '
(' (
$str	( §
,
§ •
name
¶ ™
,
™ ´
password
¨ ¥
)
¥ µ
;
µ ∂
EnumSMSServiceType "
esms# '
=( )
EnumSMSServiceType* <
.< = 
CustomerRegistration= Q
;Q R
	SMSCommon 
sc 
= 
new "
	SMSCommon# ,
(, -
)- .
;. /
sc 
. 

ProcessSMS 
( 
number $
,$ %
message& -
,- .
esms/ 3
,3 4
pageUrl5 <
)< =
;= >
} 
catch 
( 
	Exception 
err  
)  !
{ 
HttpContext 
. 
Current #
.# $
Trace$ )
.) *
Warn* .
(. /
$str/ Q
+R S
errT W
.W X
MessageX _
)_ `
;` a

ErrorClass 
objErr !
=" #
new$ '

ErrorClass( 2
(2 3
err3 6
,6 7
$str8 W
)W X
;X Y
objErr 
. 
SendMail 
(  
)  !
;! "
} 
}   	
public## 
void## 
SMSDealerAddress## $
(##$ %
string##% +
number##, 2
,##2 3
string##4 :
address##; B
,##B C
string##D J
pageUrl##K R
)##R S
{$$ 	
try%% 
{&& 
EnumSMSServiceType'' "
esms''# '
=''( )
EnumSMSServiceType''* <
.''< = 
DealerAddressRequest''= Q
;''Q R
	SMSCommon)) 
sc)) 
=)) 
new)) "
	SMSCommon))# ,
()), -
)))- .
;)). /
sc** 
.** 

ProcessSMS** 
(** 
number** $
,**$ %
address**& -
,**- .
esms**/ 3
,**3 4
pageUrl**5 <
)**< =
;**= >
}++ 
catch,, 
(,, 
	Exception,, 
err,,  
),,  !
{-- 
HttpContext.. 
... 
Current.. #
...# $
Trace..$ )
...) *
Warn..* .
(... /
$str../ R
+..S T
err..U X
...X Y
Message..Y `
)..` a
;..a b

ErrorClass// 
objErr// !
=//" #
new//$ '

ErrorClass//( 2
(//2 3
err//3 6
,//6 7
$str//8 X
)//X Y
;//Y Z
objErr00 
.00 
SendMail00 
(00  
)00  !
;00! "
}11 
}22 	
public55 
void55 
SMSNewBikeQuote55 #
(55# $
string55$ *
number55+ 1
,551 2
string553 9
cityName55: B
,55B C
string55D J
bike55K O
,55O P
string55Q W
onRoad55X ^
,55^ _
string55` f

exShowRoom55g q
,55q r
string66( .
rto66/ 2
,662 3
string664 :
	insurance66; D
,66D E
string66F L
pageUrl66M T
)66T U
{77 	
try88 
{99 
EnumSMSServiceType:: "
esms::# '
=::( )
EnumSMSServiceType::* <
.::< =
NewBikeQuote::= I
;::I J
string<< 
message<< 
=<<  
$str<<! #
;<<# $
message>> 
=>> 
$str>> 1
+>>2 3
bike>>4 8
+>>9 :
$str>>; A
+>>B C
cityName>>D L
+>>M N
$str>>O T
+?? 
$str?? )
+??* +

exShowRoom??, 6
+??7 8
$str??9 >
+@@ 
$str@@ '
+@@( )
	insurance@@* 3
+@@4 5
$str@@6 ;
+AA 
$strAA !
+AA" #
rtoAA$ '
+AA( )
$strAA* /
+BB 
$strBB $
+BB% &
onRoadBB' -
;BB- .
	SMSCommonDD 
scDD 
=DD 
newDD "
	SMSCommonDD# ,
(DD, -
)DD- .
;DD. /
scEE 
.EE 

ProcessSMSEE 
(EE 
numberEE $
,EE$ %
messageEE& -
,EE- .
esmsEE/ 3
,EE3 4
pageUrlEE5 <
)EE< =
;EE= >
}FF 
catchGG 
(GG 
	ExceptionGG 
errGG  
)GG  !
{HH 
HttpContextII 
.II 
CurrentII #
.II# $
TraceII$ )
.II) *
WarnII* .
(II. /
$strII/ Q
+IIR S
errIIT W
.IIW X
MessageIIX _
)II_ `
;II` a

ErrorClassJJ 
objErrJJ !
=JJ" #
newJJ$ '

ErrorClassJJ( 2
(JJ2 3
errJJ3 6
,JJ6 7
$strJJ8 W
)JJW X
;JJX Y
objErrKK 
.KK 
SendMailKK 
(KK  
)KK  !
;KK! "
}LL 
}MM 	
publicWW 
voidWW !
SMSMobileVerificationWW )
(WW) *
stringWW* 0
numberWW1 7
,WW7 8
stringWW9 ?
nameWW@ D
,WWD E
stringWWF L
codeWWM Q
,WWQ R
stringWWS Y
pageUrlWWZ a
)WWa b
{XX 	
tryYY 
{ZZ 
EnumSMSServiceType[[ "
esms[[# '
=[[( )
EnumSMSServiceType[[* <
.[[< =
MobileVerification[[= O
;[[O P
string]] 
message]] 
=]]  
string]]! '
.]]' (
Empty]]( -
;]]- .
message__ 
=__ 
code__ 
+__  
$str	__! Ö
;
__Ö Ü
	SMSCommonaa 
scaa 
=aa 
newaa "
	SMSCommonaa# ,
(aa, -
)aa- .
;aa. /
scbb 
.bb 
ProcessPrioritySMSbb %
(bb% &
numberbb& ,
,bb, -
messagebb. 5
,bb5 6
esmsbb7 ;
,bb; <
pageUrlbb= D
,bbD E
truebbF J
)bbJ K
;bbK L
}cc 
catchdd 
(dd 
	Exceptiondd 
errdd  
)dd  !
{ee 
HttpContextff 
.ff 
Currentff #
.ff# $
Traceff$ )
.ff) *
Warnff* .
(ff. /
$strff/ Q
+ffR S
errffT W
.ffW X
MessageffX _
)ff_ `
;ff` a

ErrorClassgg 
objErrgg !
=gg" #
newgg$ '

ErrorClassgg( 2
(gg2 3
errgg3 6
,gg6 7
$strgg8 W
)ggW X
;ggX Y
objErrhh 
.hh 
SendMailhh 
(hh  
)hh  !
;hh! "
}ii 
}jj 	
publicyy 
voidyy (
NewBikePriceQuoteSMSToDealeryy 0
(yy0 1
stringyy1 7
dealerMobileNoyy8 F
,yyF G
stringyyH N
customerNameyyO [
,yy[ \
stringyy] c
customerMobileyyd r
,yyr s
stringyyt z
BikeName	yy{ É
,
yyÉ Ñ
string
yyÖ ã
areaName
yyå î
,
yyî ï
string
yyñ ú
cityName
yyù •
,
yy• ¶
string
yyß ≠
pageUrl
yyÆ µ
,
yyµ ∂
string
yy∑ Ω

dealerArea
yyæ »
)
yy» …
{zz 	
try{{ 
{|| 
EnumSMSServiceType}} "
esms}}# '
=}}( )
EnumSMSServiceType}}* <
.}}< =(
NewBikePriceQuoteSMSToDealer}}= Y
;}}Y Z
string 
message 
=  &
NewBikePQDealerSMSTemplate! ;
(; <
customerName< H
,H I
customerMobileJ X
,X Y
BikeNameZ b
,b c
areaNamed l
,l m
cityNamen v
,v w

dealerArea	x Ç
)
Ç É
;
É Ñ
	SMSCommon
ÅÅ 
sc
ÅÅ 
=
ÅÅ 
new
ÅÅ "
	SMSCommon
ÅÅ# ,
(
ÅÅ, -
)
ÅÅ- .
;
ÅÅ. /
sc
ÇÇ 
.
ÇÇ 

ProcessSMS
ÇÇ 
(
ÇÇ 
dealerMobileNo
ÇÇ ,
,
ÇÇ, -
message
ÇÇ. 5
,
ÇÇ5 6
esms
ÇÇ7 ;
,
ÇÇ; <
pageUrl
ÇÇ= D
)
ÇÇD E
;
ÇÇE F
}
ÉÉ 
catch
ÑÑ 
(
ÑÑ 
	Exception
ÑÑ 
err
ÑÑ  
)
ÑÑ  !
{
ÖÖ 
HttpContext
ÜÜ 
.
ÜÜ 
Current
ÜÜ #
.
ÜÜ# $
Trace
ÜÜ$ )
.
ÜÜ) *
Warn
ÜÜ* .
(
ÜÜ. /
$str
ÜÜ/ ^
+
ÜÜ_ `
err
ÜÜa d
.
ÜÜd e
Message
ÜÜe l
)
ÜÜl m
;
ÜÜm n

ErrorClass
áá 
objErr
áá !
=
áá" #
new
áá$ '

ErrorClass
áá( 2
(
áá2 3
err
áá3 6
,
áá6 7
$str
áá8 d
)
áád e
;
ááe f
objErr
àà 
.
àà 
SendMail
àà 
(
àà  
)
àà  !
;
àà! "
}
ââ 
}
ää 	
public
óó 
void
óó ,
NewBikePriceQuoteSMSToCustomer
óó 2
(
óó2 3#
PQ_DealerDetailEntity
óó3 H
dealerEntity
óóI U
,
óóU V
string
óóW ]
customerMobile
óó^ l
,
óól m
string
óón t
customerNameóóu Å
,óóÅ Ç
stringóóÉ â
BikeNameóóä í
,óóí ì
stringóóî ö

dealerNameóóõ •
,óó• ¶
stringóóß ≠
dealerContactNoóóÆ Ω
,óóΩ æ
stringóóø ≈
dealerAddressóó∆ ”
,óó” ‘
stringóó’ €
pageUrlóó‹ „
,óó„ ‰
uintóóÂ È
bookingAmountóóÍ ˜
,óó˜ ¯
uintóó˘ ˝
insuranceAmountóó˛ ç
=óóé è
$numóóê ë
,óóë í
boolóóì ó$
hasBumperDealerOfferóóò ¨
=óó≠ Æ
falseóóØ ¥
)óó¥ µ
{
òò 	
try
ôô 
{
öö 
bool
úú 
isFlipkartOffer
úú $
=
úú% &
false
úú' ,
;
úú, -
bool
ùù 
isAccessories
ùù "
=
ùù# $
false
ùù% *
;
ùù* +
if
üü 
(
üü 
dealerEntity
üü  
.
üü  !
	objOffers
üü! *
!=
üü+ -
null
üü. 2
&&
üü3 5
dealerEntity
üü6 B
.
üüB C
	objOffers
üüC L
.
üüL M
Count
üüM R
>
üüS T
$num
üüU V
)
üüV W
{
†† 
foreach
°° 
(
°° 
var
°°  
offer
°°! &
in
°°' )
dealerEntity
°°* 6
.
°°6 7
	objOffers
°°7 @
)
°°@ A
{
¢¢ 
if
££ 
(
££ 
offer
££ !
.
££! "
	OfferText
££" +
.
££+ ,
ToLower
££, 3
(
££3 4
)
££4 5
.
££5 6
Contains
££6 >
(
££> ?
$str
££? I
)
££I J
)
££J K
{
§§ 
isFlipkartOffer
•• +
=
••, -
true
••. 2
;
••2 3
break
¶¶ !
;
¶¶! "
}
ßß 
else
®® 
if
®® 
(
®®  !
offer
®®! &
.
®®& '
	OfferText
®®' 0
.
®®0 1
ToLower
®®1 8
(
®®8 9
)
®®9 :
.
®®: ;
Contains
®®; C
(
®®C D
$str
®®D Q
)
®®Q R
)
®®R S
{
©© 
isAccessories
™™ )
=
™™* +
true
™™, 0
;
™™0 1
break
´´ !
;
´´! "
}
¨¨ 
}
≠≠ 
}
ÆÆ  
EnumSMSServiceType
ØØ "
esms
ØØ# '
=
ØØ( ) 
EnumSMSServiceType
ØØ* <
.
ØØ< =,
NewBikePriceQuoteSMSToCustomer
ØØ= [
;
ØØ[ \
string
±± 
message
±± 
=
±±  *
NewBikePQCustomerSMSTemplate
±±! =
(
±±= >
BikeName
±±> F
,
±±F G

dealerName
±±H R
,
±±R S
dealerContactNo
±±T c
,
±±c d
dealerAddress
±±e r
,
±±r s
bookingAmount±±t Å
,±±Å Ç
insuranceAmount±±É í
,±±í ì$
hasBumperDealerOffer±±î ®
,±±® ©
isFlipkartOffer±±™ π
,±±π ∫
isAccessories±±ª »
)±±» …
;±±…  
	SMSCommon
≥≥ 
sc
≥≥ 
=
≥≥ 
new
≥≥ "
	SMSCommon
≥≥# ,
(
≥≥, -
)
≥≥- .
;
≥≥. /
sc
¥¥ 
.
¥¥ 

ProcessSMS
¥¥ 
(
¥¥ 
customerMobile
¥¥ ,
,
¥¥, -
message
¥¥. 5
,
¥¥5 6
esms
¥¥7 ;
,
¥¥; <
pageUrl
¥¥= D
)
¥¥D E
;
¥¥E F
}
µµ 
catch
∂∂ 
(
∂∂ 
	Exception
∂∂ 
err
∂∂  
)
∂∂  !
{
∑∑ 
HttpContext
∏∏ 
.
∏∏ 
Current
∏∏ #
.
∏∏# $
Trace
∏∏$ )
.
∏∏) *
Warn
∏∏* .
(
∏∏. /
$str
∏∏/ `
+
∏∏a b
err
∏∏c f
.
∏∏f g
Message
∏∏g n
)
∏∏n o
;
∏∏o p

ErrorClass
ππ 
objErr
ππ !
=
ππ" #
new
ππ$ '

ErrorClass
ππ( 2
(
ππ2 3
err
ππ3 6
,
ππ6 7
$str
ππ8 f
)
ππf g
;
ππg h
objErr
∫∫ 
.
∫∫ 
SendMail
∫∫ 
(
∫∫  
)
∫∫  !
;
∫∫! "
}
ªª 
}
ºº 	
public
…… 
void
…… &
BikeBookingSMSToCustomer
…… ,
(
……, -
string
……- 3
customerMobile
……4 B
,
……B C
string
……D J
customerName
……K W
,
……W X
string
……Y _
BikeName
……` h
,
……h i
string
……j p

dealerName
……q {
,
……{ |
string……} É
dealerContactNo……Ñ ì
,……ì î
string……ï õ
dealerAddress……ú ©
,……© ™
string……´ ±
pageUrl……≤ π
,……π ∫
string……ª ¡
bookingRefNum……¬ œ
,……œ –
uint……— ’
insuranceAmount……÷ Â
=……Ê Á
$num……Ë È
)……È Í
{
   	
bool
ÀÀ 
isOfferAvailable
ÀÀ !
=
ÀÀ" #
false
ÀÀ$ )
;
ÀÀ) *
try
ÃÃ 
{
ÕÕ  
EnumSMSServiceType
ŒŒ "
esms
ŒŒ# '
=
ŒŒ( ) 
EnumSMSServiceType
ŒŒ* <
.
ŒŒ< =%
BikeBookedSMSToCustomer
ŒŒ= T
;
ŒŒT U
string
–– 
message
–– 
=
––  
$str
––! #
;
––# $
isOfferAvailable
——  
=
——! "
Convert
——# *
.
——* +
	ToBoolean
——+ 4
(
——4 5"
ConfigurationManager
——5 I
.
——I J
AppSettings
——J U
[
——U V
$str
——V h
]
——h i
)
——i j
;
——j k
if
”” 
(
”” 
insuranceAmount
”” #
==
””$ &
$num
””' (
)
””( )
{
‘‘ 
message
’’ 
=
’’ 
$str
’’ A
+
’’B C
BikeName
’’D L
+
’’M N
$str
’’O `
+
’’a b
bookingRefNum
’’c p
+
’’q r
$str
’’s 
+’’Ä Å

dealerName’’Ç å
+’’ç é
$str’’è ì
+’’î ï
dealerContactNo’’ñ •
+’’¶ ß
$str’’® ¨
+’’≠ Æ
dealerAddress’’Ø º
;’’º Ω
}
÷÷ 
else
◊◊ 
{
ÿÿ 
message
ŸŸ 
=
ŸŸ 
String
ŸŸ $
.
ŸŸ$ %
Format
ŸŸ% +
(
ŸŸ+ ,
$strŸŸ, í
,ŸŸí ì
BikeNameŸŸî ú
,ŸŸú ù
bookingRefNumŸŸû ´
,ŸŸ´ ¨

dealerNameŸŸ≠ ∑
,ŸŸ∑ ∏
dealerContactNoŸŸπ »
,ŸŸ» …
dealerAddressŸŸ  ◊
)ŸŸ◊ ÿ
;ŸŸÿ Ÿ
}
⁄⁄ 
	SMSCommon
‹‹ 
sc
‹‹ 
=
‹‹ 
new
‹‹ "
	SMSCommon
‹‹# ,
(
‹‹, -
)
‹‹- .
;
‹‹. /
if
›› 
(
›› 
isOfferAvailable
›› $
&&
››% '
insuranceAmount
››( 7
==
››8 :
$num
››; <
)
››< =
{
ﬁﬁ 
message
ﬂﬂ 
+=
ﬂﬂ 
String
ﬂﬂ %
.
ﬂﬂ% &
Format
ﬂﬂ& ,
(
ﬂﬂ, -
$str
ﬂﬂ- v
,
ﬂﬂv w#
ConfigurationManagerﬂﬂx å
.ﬂﬂå ç
AppSettingsﬂﬂç ò
[ﬂﬂò ô
$strﬂﬂô ∂
]ﬂﬂ∂ ∑
)ﬂﬂ∑ ∏
;ﬂﬂ∏ π
}
‡‡ 
sc
·· 
.
·· 

ProcessSMS
·· 
(
·· 
customerMobile
·· ,
,
··, -
message
··. 5
,
··5 6
esms
··7 ;
,
··; <
pageUrl
··= D
)
··D E
;
··E F
}
‚‚ 
catch
„„ 
(
„„ 
	Exception
„„ 
err
„„  
)
„„  !
{
‰‰ 
HttpContext
ÂÂ 
.
ÂÂ 
Current
ÂÂ #
.
ÂÂ# $
Trace
ÂÂ$ )
.
ÂÂ) *
Warn
ÂÂ* .
(
ÂÂ. /
$str
ÂÂ/ Z
+
ÂÂ[ \
err
ÂÂ] `
.
ÂÂ` a
Message
ÂÂa h
)
ÂÂh i
;
ÂÂi j

ErrorClass
ÊÊ 
objErr
ÊÊ !
=
ÊÊ" #
new
ÊÊ$ '

ErrorClass
ÊÊ( 2
(
ÊÊ2 3
err
ÊÊ3 6
,
ÊÊ6 7
$str
ÊÊ8 `
)
ÊÊ` a
;
ÊÊa b
objErr
ÁÁ 
.
ÁÁ 
SendMail
ÁÁ 
(
ÁÁ  
)
ÁÁ  !
;
ÁÁ! "
}
ËË 
}
ÈÈ 	
public
˘˘ 
void
˘˘ $
BikeBookingSMSToDealer
˘˘ *
(
˘˘* +
string
˘˘+ 1
dealerMobileNo
˘˘2 @
,
˘˘@ A
string
˘˘B H
customerName
˘˘I U
,
˘˘U V
string
˘˘W ]

dealerName
˘˘^ h
,
˘˘h i
string
˘˘j p
customerMobile
˘˘q 
,˘˘ Ä
string˘˘Å á
BikeName˘˘à ê
,˘˘ê ë
string˘˘í ò
pageUrl˘˘ô †
,˘˘† °
UInt32˘˘¢ ®

bookingAmt˘˘© ≥
,˘˘≥ ¥
string˘˘µ ª
bookingRefNum˘˘º …
,˘˘…  
uint˘˘À œ
insuranceAmount˘˘– ﬂ
=˘˘‡ ·
$num˘˘‚ „
)˘˘„ ‰
{
˙˙ 	
try
˚˚ 
{
¸¸  
EnumSMSServiceType
˝˝ "
esms
˝˝# '
=
˝˝( ) 
EnumSMSServiceType
˝˝* <
.
˝˝< =#
BikeBookedSMSToDealer
˝˝= R
;
˝˝R S
string
ˇˇ 
message
ˇˇ 
=
ˇˇ  
$str
ˇˇ! #
;
ˇˇ# $
if
ÄÄ 
(
ÄÄ 
insuranceAmount
ÄÄ #
==
ÄÄ$ &
$num
ÄÄ' (
)
ÄÄ( )
{
ÅÅ 
message
ÇÇ 
=
ÇÇ 
$str
ÇÇ 0
+
ÇÇ1 2
customerName
ÇÇ3 ?
+
ÇÇ@ A
$str
ÇÇB M
+
ÇÇN O

bookingAmt
ÇÇP Z
+
ÇÇ[ \
$str
ÇÇ] d
+
ÇÇe f
BikeName
ÇÇg o
+
ÇÇp q
$str
ÇÇr y
+
ÇÇz {
bookingRefNumÇÇ| â
+ÇÇä ã
$strÇÇå °
+ÇÇ¢ £
customerMobileÇÇ§ ≤
+ÇÇ≥ ¥
$strÇÇµ  
;ÇÇ  À
}
ÉÉ 
else
ÑÑ 
{
ÖÖ 
message
ÜÜ 
=
ÜÜ 
String
ÜÜ $
.
ÜÜ$ %
Format
ÜÜ% +
(
ÜÜ+ ,
$strÜÜ, µ
,ÜÜµ ∂
customerNameÜÜ∑ √
,ÜÜ√ ƒ

bookingAmtÜÜ≈ œ
,ÜÜœ –
BikeNameÜÜ— Ÿ
,ÜÜŸ ⁄
bookingRefNumÜÜ€ Ë
,ÜÜË È
customerMobileÜÜÍ ¯
)ÜÜ¯ ˘
;ÜÜ˘ ˙
}
áá 
	SMSCommon
ââ 
sc
ââ 
=
ââ 
new
ââ "
	SMSCommon
ââ# ,
(
ââ, -
)
ââ- .
;
ââ. /
sc
ää 
.
ää 

ProcessSMS
ää 
(
ää 
dealerMobileNo
ää ,
,
ää, -
message
ää. 5
,
ää5 6
esms
ää7 ;
,
ää; <
pageUrl
ää= D
)
ääD E
;
ääE F
}
ãã 
catch
åå 
(
åå 
	Exception
åå 
err
åå  
)
åå  !
{
çç 
HttpContext
éé 
.
éé 
Current
éé #
.
éé# $
Trace
éé$ )
.
éé) *
Warn
éé* .
(
éé. /
$str
éé/ ^
+
éé_ `
err
ééa d
.
ééd e
Message
éée l
)
éél m
;
éém n

ErrorClass
èè 
objErr
èè !
=
èè" #
new
èè$ '

ErrorClass
èè( 2
(
èè2 3
err
èè3 6
,
èè6 7
$str
èè8 d
)
èèd e
;
èèe f
objErr
êê 
.
êê 
SendMail
êê 
(
êê  
)
êê  !
;
êê! "
}
ëë 
}
íí 	
public
îî 
void
îî '
ClaimedOfferSMSToCustomer
îî -
(
îî- .
string
îî. 4
customerMobile
îî5 C
,
îîC D
string
îîE K
pageUrl
îîL S
)
îîS T
{
ïï 	
try
ññ 
{
óó  
EnumSMSServiceType
òò "
esms
òò# '
=
òò( ) 
EnumSMSServiceType
òò* <
.
òò< =
ClaimedOffer
òò= I
;
òòI J
string
ôô 
message
ôô 
=
ôô  
$strôô! √
;ôô√ ƒ
	SMSCommon
öö 
sc
öö 
=
öö 
new
öö "
	SMSCommon
öö# ,
(
öö, -
)
öö- .
;
öö. /
sc
õõ 
.
õõ 

ProcessSMS
õõ 
(
õõ 
customerMobile
õõ ,
,
õõ, -
message
õõ. 5
,
õõ5 6
esms
õõ7 ;
,
õõ; <
pageUrl
õõ= D
)
õõD E
;
õõE F
}
úú 
catch
ùù 
(
ùù 
	Exception
ùù 
err
ùù  
)
ùù  !
{
ûû 

ErrorClass
üü 
objErr
üü !
=
üü" #
new
üü$ '

ErrorClass
üü( 2
(
üü2 3
err
üü3 6
,
üü6 7
$str
üü8 a
)
üüa b
;
üüb c
objErr
†† 
.
†† 
SendMail
†† 
(
††  
)
††  !
;
††! "
}
°° 
}
¢¢ 	
public
±± 
void
±± .
 SaveNewBikePriceQuoteSMSToDealer
±± 4
(
±±4 5
uint
±±5 9
pqId
±±: >
,
±±> ?
string
±±@ F
dealerMobileNo
±±G U
,
±±U V
string
±±W ]
customerName
±±^ j
,
±±j k
string
±±l r
customerMobile±±s Å
,±±Å Ç
string±±É â
BikeName±±ä í
,±±í ì
string±±î ö
areaName±±õ £
,±±£ §
string±±• ´
cityName±±¨ ¥
,±±¥ µ
string±±∂ º
pageUrl±±Ω ƒ
,±±ƒ ≈
string±±∆ Ã

dealerArea±±Õ ◊
)±±◊ ÿ
{
≤≤ 	
try
≥≥ 
{
¥¥  
EnumSMSServiceType
µµ "
esms
µµ# '
=
µµ( ) 
EnumSMSServiceType
µµ* <
.
µµ< =*
NewBikePriceQuoteSMSToDealer
µµ= Y
;
µµY Z
string
∑∑ 
message
∑∑ 
=
∑∑  (
NewBikePQDealerSMSTemplate
∑∑! ;
(
∑∑; <
customerName
∑∑< H
,
∑∑H I
customerMobile
∑∑J X
,
∑∑X Y
BikeName
∑∑Z b
,
∑∑b c
areaName
∑∑d l
,
∑∑l m
cityName
∑∑n v
,
∑∑v w

dealerArea∑∑x Ç
)∑∑Ç É
;∑∑É Ñ 
SavePQNotification
ππ "
obj
ππ# &
=
ππ' (
new
ππ) , 
SavePQNotification
ππ- ?
(
ππ? @
)
ππ@ A
;
ππA B
obj
∫∫ 
.
∫∫ %
SaveDealerPQSMSTemplate
∫∫ +
(
∫∫+ ,
pqId
∫∫, 0
,
∫∫0 1
message
∫∫2 9
,
∫∫9 :
(
∫∫; <
int
∫∫< ?
)
∫∫? @
esms
∫∫@ D
,
∫∫D E
dealerMobileNo
∫∫F T
,
∫∫T U
pageUrl
∫∫V ]
)
∫∫] ^
;
∫∫^ _
}
ªª 
catch
ºº 
(
ºº 
	Exception
ºº 
err
ºº  
)
ºº  !
{
ΩΩ 
HttpContext
ææ 
.
ææ 
Current
ææ #
.
ææ# $
Trace
ææ$ )
.
ææ) *
Warn
ææ* .
(
ææ. /
$str
ææ/ ^
+
ææ_ `
err
ææa d
.
ææd e
Message
ææe l
)
ææl m
;
ææm n

ErrorClass
øø 
objErr
øø !
=
øø" #
new
øø$ '

ErrorClass
øø( 2
(
øø2 3
err
øø3 6
,
øø6 7
$str
øø8 d
)
øød e
;
øøe f
objErr
¿¿ 
.
¿¿ 
SendMail
¿¿ 
(
¿¿  
)
¿¿  !
;
¿¿! "
}
¡¡ 
}
¬¬ 	
public
œœ 
void
œœ 0
"SaveNewBikePriceQuoteSMSToCustomer
œœ 6
(
œœ6 7
uint
œœ7 ;
pqId
œœ< @
,
œœ@ A#
PQ_DealerDetailEntity
œœB W
dealerEntity
œœX d
,
œœd e
string
œœf l
customerMobile
œœm {
,
œœ{ |
stringœœ} É
customerNameœœÑ ê
,œœê ë
stringœœí ò
BikeNameœœô °
,œœ° ¢
stringœœ£ ©

dealerNameœœ™ ¥
,œœ¥ µ
stringœœ∂ º
dealerContactNoœœΩ Ã
,œœÃ Õ
stringœœŒ ‘
dealerAddressœœ’ ‚
,œœ‚ „
stringœœ‰ Í
pageUrlœœÎ Ú
,œœÚ Û
uintœœÙ ¯
bookingAmountœœ˘ Ü
,œœÜ á
uintœœà å
insuranceAmountœœç ú
=œœù û
$numœœü †
,œœ† °
boolœœ¢ ¶$
hasBumperDealerOfferœœß ª
=œœº Ω
falseœœæ √
)œœ√ ƒ
{
–– 	
try
—— 
{
““ 
bool
‘‘ 
isFlipkartOffer
‘‘ $
=
‘‘% &
false
‘‘' ,
;
‘‘, -
bool
’’ 
isAccessories
’’ "
=
’’# $
false
’’% *
;
’’* +
if
◊◊ 
(
◊◊ 
dealerEntity
◊◊  
.
◊◊  !
	objOffers
◊◊! *
!=
◊◊+ -
null
◊◊. 2
&&
◊◊3 5
dealerEntity
◊◊6 B
.
◊◊B C
	objOffers
◊◊C L
.
◊◊L M
Count
◊◊M R
>
◊◊S T
$num
◊◊U V
)
◊◊V W
{
ÿÿ 
foreach
ŸŸ 
(
ŸŸ 
var
ŸŸ  
offer
ŸŸ! &
in
ŸŸ' )
dealerEntity
ŸŸ* 6
.
ŸŸ6 7
	objOffers
ŸŸ7 @
)
ŸŸ@ A
{
⁄⁄ 
if
€€ 
(
€€ 
offer
€€ !
.
€€! "
	OfferText
€€" +
.
€€+ ,
ToLower
€€, 3
(
€€3 4
)
€€4 5
.
€€5 6
Contains
€€6 >
(
€€> ?
$str
€€? I
)
€€I J
)
€€J K
{
‹‹ 
isFlipkartOffer
›› +
=
››, -
true
››. 2
;
››2 3
break
ﬁﬁ !
;
ﬁﬁ! "
}
ﬂﬂ 
else
‡‡ 
if
‡‡ 
(
‡‡  !
offer
‡‡! &
.
‡‡& '
	OfferText
‡‡' 0
.
‡‡0 1
ToLower
‡‡1 8
(
‡‡8 9
)
‡‡9 :
.
‡‡: ;
Contains
‡‡; C
(
‡‡C D
$str
‡‡D Q
)
‡‡Q R
)
‡‡R S
{
·· 
isAccessories
‚‚ )
=
‚‚* +
true
‚‚, 0
;
‚‚0 1
break
„„ !
;
„„! "
}
‰‰ 
}
ÂÂ 
}
ÊÊ  
EnumSMSServiceType
ÁÁ "
esms
ÁÁ# '
=
ÁÁ( ) 
EnumSMSServiceType
ÁÁ* <
.
ÁÁ< =,
NewBikePriceQuoteSMSToCustomer
ÁÁ= [
;
ÁÁ[ \
string
ÈÈ 
message
ÈÈ 
=
ÈÈ  *
NewBikePQCustomerSMSTemplate
ÈÈ! =
(
ÈÈ= >
BikeName
ÈÈ> F
,
ÈÈF G

dealerName
ÈÈH R
,
ÈÈR S
dealerContactNo
ÈÈT c
,
ÈÈc d
dealerAddress
ÈÈe r
,
ÈÈr s
bookingAmountÈÈt Å
,ÈÈÅ Ç
insuranceAmountÈÈÉ í
,ÈÈí ì$
hasBumperDealerOfferÈÈî ®
,ÈÈ® ©
isFlipkartOfferÈÈ™ π
,ÈÈπ ∫
isAccessoriesÈÈª »
)ÈÈ» …
;ÈÈ…   
SavePQNotification
ÎÎ "
obj
ÎÎ# &
=
ÎÎ' (
new
ÎÎ) , 
SavePQNotification
ÎÎ- ?
(
ÎÎ? @
)
ÎÎ@ A
;
ÎÎA B
obj
ÏÏ 
.
ÏÏ '
SaveCustomerPQSMSTemplate
ÏÏ -
(
ÏÏ- .
pqId
ÏÏ. 2
,
ÏÏ2 3
message
ÏÏ4 ;
,
ÏÏ; <
(
ÏÏ= >
int
ÏÏ> A
)
ÏÏA B
esms
ÏÏB F
,
ÏÏF G
customerMobile
ÏÏH V
,
ÏÏV W
pageUrl
ÏÏX _
)
ÏÏ_ `
;
ÏÏ` a
}
ÌÌ 
catch
ÓÓ 
(
ÓÓ 
	Exception
ÓÓ 
err
ÓÓ  
)
ÓÓ  !
{
ÔÔ 
HttpContext
 
.
 
Current
 #
.
# $
Trace
$ )
.
) *
Warn
* .
(
. /
$str
/ d
+
e f
err
g j
.
j k
Message
k r
)
r s
;
s t

ErrorClass
ÒÒ 
objErr
ÒÒ !
=
ÒÒ" #
new
ÒÒ$ '

ErrorClass
ÒÒ( 2
(
ÒÒ2 3
err
ÒÒ3 6
,
ÒÒ6 7
$str
ÒÒ8 f
)
ÒÒf g
;
ÒÒg h
objErr
ÚÚ 
.
ÚÚ 
SendMail
ÚÚ 
(
ÚÚ  
)
ÚÚ  !
;
ÚÚ! "
}
ÛÛ 
}
ÙÙ 	
private
ÉÉ 
static
ÉÉ 
string
ÉÉ *
NewBikePQCustomerSMSTemplate
ÉÉ :
(
ÉÉ: ;
string
ÉÉ; A
BikeName
ÉÉB J
,
ÉÉJ K
string
ÉÉL R

dealerName
ÉÉS ]
,
ÉÉ] ^
string
ÉÉ_ e
dealerContactNo
ÉÉf u
,
ÉÉu v
string
ÉÉw }
dealerAddressÉÉ~ ã
,ÉÉã å
uintÉÉç ë
bookingAmountÉÉí ü
,ÉÉü †
uintÉÉ° •
insuranceAmountÉÉ¶ µ
,ÉÉµ ∂
boolÉÉ∑ ª$
hasBumperDealerOfferÉÉº –
,ÉÉ– —
boolÉÉ“ ÷
isFlipkartOfferÉÉ◊ Ê
,ÉÉÊ Á
boolÉÉË Ï
isAccessoriesÉÉÌ ˙
)ÉÉ˙ ˚
{
ÑÑ 	
string
ÖÖ 
message
ÖÖ 
=
ÖÖ 
$str
ÖÖ 
;
ÖÖ  
if
áá 
(
áá 
!
áá "
hasBumperDealerOffer
áá %
)
áá% &
{
àà 
if
ââ 
(
ââ 
insuranceAmount
ââ #
==
ââ$ &
$num
ââ' (
)
ââ( )
{
ää 
if
ãã 
(
ãã 
isFlipkartOffer
ãã '
)
ãã' (
{
åå 
message
éé 
=
éé  !
String
éé" (
.
éé( )
Format
éé) /
(
éé/ 0
$stréé0 ≠
,éé≠ Æ
bookingAmountééØ º
,ééº Ω

dealerNameééæ »
,éé» …
dealerAddresséé  ◊
,éé◊ ÿ
dealerContactNoééŸ Ë
)ééË È
;ééÈ Í
}
èè 
else
êê 
if
êê 
(
êê 
isAccessories
êê *
)
êê* +
{
ëë 
message
íí 
=
íí  !
String
íí" (
.
íí( )
Format
íí) /
(
íí/ 0
$stríí0 Ø
,ííØ ∞
bookingAmountíí± æ
,ííæ ø

dealerNameíí¿  
,íí  À
dealerAddressííÃ Ÿ
,ííŸ ⁄
dealerContactNoíí€ Í
)ííÍ Î
;ííÎ Ï
}
ìì 
else
îî 
{
ïï 
message
òò 
=
òò  !
String
òò" (
.
òò( )
Format
òò) /
(
òò/ 0
$stròò0 µ
,òòµ ∂
bookingAmountòò∑ ƒ
,òòƒ ≈

dealerNameòò∆ –
,òò– —
dealerAddressòò“ ﬂ
,òòﬂ ‡
dealerContactNoòò· 
)òò Ò
;òòÒ Ú
}
ôô 
}
öö 
else
õõ 
{
úú 
message
ùù 
=
ùù 
String
ùù $
.
ùù$ %
Format
ùù% +
(
ùù+ ,
$strùù, é
,ùùé è
BikeNameùùê ò
,ùùò ô

dealerNameùùö §
,ùù§ •
dealerContactNoùù¶ µ
,ùùµ ∂
bookingAmountùù∑ ƒ
)ùùƒ ≈
;ùù≈ ∆
}
ûû 
}
üü 
else
†† 
{
°° 
message
¢¢ 
=
¢¢ 
String
¢¢  
.
¢¢  !
Format
¢¢! '
(
¢¢' (
$str¢¢( ¬
,¢¢¬ √
bookingAmount¢¢ƒ —
,¢¢— “
BikeName¢¢” €
)¢¢€ ‹
;¢¢‹ ›
}
££ 
return
§§ 
message
§§ 
;
§§ 
}
•• 	
private
≥≥ 
static
≥≥ 
string
≥≥ (
NewBikePQDealerSMSTemplate
≥≥ 8
(
≥≥8 9
string
≥≥9 ?
customerName
≥≥@ L
,
≥≥L M
string
≥≥N T
customerMobile
≥≥U c
,
≥≥c d
string
≥≥e k
BikeName
≥≥l t
,
≥≥t u
string
≥≥v |
areaName≥≥} Ö
,≥≥Ö Ü
string≥≥á ç
cityName≥≥é ñ
,≥≥ñ ó
string≥≥ò û

dealerArea≥≥ü ©
)≥≥© ™
{
¥¥ 	
string
µµ 
message
µµ 
=
µµ 
$str
µµ 
;
µµ  
if
∂∂ 
(
∂∂ 
!
∂∂ 
string
∂∂ 
.
∂∂ 
IsNullOrEmpty
∂∂ %
(
∂∂% &
areaName
∂∂& .
)
∂∂. /
)
∂∂/ 0
{
∑∑ 
areaName
∏∏ 
=
∏∏ 
$str
∏∏ 
+
∏∏  !
areaName
∏∏" *
;
∏∏* +
}
ππ 
message
∫∫ 
=
∫∫ 
string
∫∫ 
.
∫∫ 
Format
∫∫ #
(
∫∫# $
$str∫∫$ ï
,∫∫ï ñ

dealerArea∫∫ó °
,∫∫° ¢
customerName∫∫£ Ø
,∫∫Ø ∞
areaName∫∫± π
,∫∫π ∫
cityName∫∫ª √
,∫∫√ ƒ
customerMobile∫∫≈ ”
,∫∫” ‘
BikeName∫∫’ ›
)∫∫› ﬁ
;∫∫ﬁ ﬂ
return
ªª 
message
ªª 
;
ªª 
}
ºº 	
public
ææ 
void
ææ 0
"SaveNewBikePriceQuoteSMSToCustomer
ææ 6
(
ææ6 7
uint
ææ7 ;
pqId
ææ< @
,
ææ@ A
string
ææB H
message
ææI P
,
ææP Q
string
ææR X
customerMobile
ææY g
,
ææg h
string
ææi o

requestUrl
ææp z
)
ææz {
{
øø 	
try
¿¿ 
{
¡¡  
EnumSMSServiceType
¬¬ "
esms
¬¬# '
=
¬¬( ) 
EnumSMSServiceType
¬¬* <
.
¬¬< =,
NewBikePriceQuoteSMSToCustomer
¬¬= [
;
¬¬[ \ 
SavePQNotification
√√ "
obj
√√# &
=
√√' (
new
√√) , 
SavePQNotification
√√- ?
(
√√? @
)
√√@ A
;
√√A B
obj
ƒƒ 
.
ƒƒ '
SaveCustomerPQSMSTemplate
ƒƒ -
(
ƒƒ- .
pqId
ƒƒ. 2
,
ƒƒ2 3
message
ƒƒ4 ;
,
ƒƒ; <
(
ƒƒ= >
int
ƒƒ> A
)
ƒƒA B
esms
ƒƒB F
,
ƒƒF G
customerMobile
ƒƒH V
,
ƒƒV W

requestUrl
ƒƒX b
)
ƒƒb c
;
ƒƒc d
}
≈≈ 
catch
∆∆ 
(
∆∆ 
	Exception
∆∆ 
ex
∆∆ 
)
∆∆  
{
«« 

ErrorClass
»» 
objErr
»» !
=
»»" #
new
»»$ '

ErrorClass
»»( 2
(
»»2 3
ex
»»3 5
,
»»5 6
$str
»»7 {
)
»»{ |
;
»»| }
objErr
…… 
.
…… 
SendMail
…… 
(
……  
)
……  !
;
……! "
}
   
}
ÀÀ 	
public
—— 
void
—— #
SMSMobileVerification
—— )
(
——) *
string
——* 0
number
——1 7
,
——7 8
string
——9 ?
otp
——@ C
,
——C D
string
——E K
pageUrl
——L S
)
——S T
{
““ 	
try
”” 
{
‘‘  
EnumSMSServiceType
’’ "
smsEnum
’’# *
=
’’+ , 
EnumSMSServiceType
’’- ?
.
’’? @$
BookingCancellationOTP
’’@ V
;
’’V W
string
÷÷ 
message
÷÷ 
=
÷÷  
string
÷÷! '
.
÷÷' (
Empty
÷÷( -
;
÷÷- .
message
◊◊ 
=
◊◊ 
string
◊◊  
.
◊◊  !
Format
◊◊! '
(
◊◊' (
$str◊◊( ±
,◊◊± ≤
otp◊◊≥ ∂
)◊◊∂ ∑
;◊◊∑ ∏
	SMSCommon
ŸŸ 
sc
ŸŸ 
=
ŸŸ 
new
ŸŸ "
	SMSCommon
ŸŸ# ,
(
ŸŸ, -
)
ŸŸ- .
;
ŸŸ. /
sc
⁄⁄ 
.
⁄⁄  
ProcessPrioritySMS
⁄⁄ %
(
⁄⁄% &
number
⁄⁄& ,
,
⁄⁄, -
message
⁄⁄. 5
,
⁄⁄5 6
smsEnum
⁄⁄7 >
,
⁄⁄> ?
pageUrl
⁄⁄@ G
,
⁄⁄G H
true
⁄⁄I M
)
⁄⁄M N
;
⁄⁄N O
}
€€ 
catch
‹‹ 
(
‹‹ 
	Exception
‹‹ 
err
‹‹  
)
‹‹  !
{
›› 
HttpContext
ﬁﬁ 
.
ﬁﬁ 
Current
ﬁﬁ #
.
ﬁﬁ# $
Trace
ﬁﬁ$ )
.
ﬁﬁ) *
Warn
ﬁﬁ* .
(
ﬁﬁ. /
$str
ﬁﬁ/ \
+
ﬁﬁ] ^
err
ﬁﬁ_ b
.
ﬁﬁb c
Message
ﬁﬁc j
)
ﬁﬁj k
;
ﬁﬁk l

ErrorClass
ﬂﬂ 
objErr
ﬂﬂ !
=
ﬂﬂ" #
new
ﬂﬂ$ '

ErrorClass
ﬂﬂ( 2
(
ﬂﬂ2 3
err
ﬂﬂ3 6
,
ﬂﬂ6 7
$str
ﬂﬂ8 b
)
ﬂﬂb c
;
ﬂﬂc d
objErr
‡‡ 
.
‡‡ 
SendMail
‡‡ 
(
‡‡  
)
‡‡  !
;
‡‡! "
}
·· 
}
‚‚ 	
public
ÏÏ 
void
ÏÏ &
SMSForPhotoUploadTwoDays
ÏÏ ,
(
ÏÏ, -
string
ÏÏ- 3
customerName
ÏÏ4 @
,
ÏÏ@ A
string
ÏÏB H
customerNumber
ÏÏI W
,
ÏÏW X
string
ÏÏY _
make
ÏÏ` d
,
ÏÏd e
string
ÏÏf l
model
ÏÏm r
,
ÏÏr s
string
ÏÏt z
	profileIdÏÏ{ Ñ
,ÏÏÑ Ö
stringÏÏÜ å
editUrlÏÏç î
)ÏÏî ï
{
ÌÌ 	
try
ÓÓ 
{
ÔÔ  
EnumSMSServiceType
 "
smsEnum
# *
=
+ , 
EnumSMSServiceType
- ?
.
? @&
SMSForPhotoUploadTwoDays
@ X
;
X Y
string
ÒÒ 
message
ÒÒ 
=
ÒÒ  
string
ÒÒ! '
.
ÒÒ' (
Empty
ÒÒ( -
;
ÒÒ- .
message
ÛÛ 
=
ÛÛ 
string
ÛÛ  
.
ÛÛ  !
Format
ÛÛ! '
(
ÛÛ' (
$strÛÛ( ∞
,ÛÛ∞ ±
makeÛÛ≤ ∂
,ÛÛ∂ ∑
modelÛÛ∏ Ω
,ÛÛΩ æ
editUrlÛÛø ∆
)ÛÛ∆ «
;ÛÛ« »
	SMSCommon
ıı 
sc
ıı 
=
ıı 
new
ıı "
	SMSCommon
ıı# ,
(
ıı, -
)
ıı- .
;
ıı. /
sc
ˆˆ 
.
ˆˆ  
ProcessPrioritySMS
ˆˆ %
(
ˆˆ% &
customerNumber
ˆˆ& 4
,
ˆˆ4 5
message
ˆˆ6 =
,
ˆˆ= >
smsEnum
ˆˆ? F
,
ˆˆF G
$str
ˆˆH k
,
ˆˆk l
true
ˆˆm q
)
ˆˆq r
;
ˆˆr s
}
˜˜ 
catch
¯¯ 
(
¯¯ 
	Exception
¯¯ 
err
¯¯  
)
¯¯  !
{
˘˘ 

ErrorClass
˙˙ 
objErr
˙˙ !
=
˙˙" #
new
˙˙$ '

ErrorClass
˙˙( 2
(
˙˙2 3
err
˙˙3 6
,
˙˙6 7
$str
˙˙8 [
)
˙˙[ \
;
˙˙\ ]
objErr
˚˚ 
.
˚˚ 
SendMail
˚˚ 
(
˚˚  
)
˚˚  !
;
˚˚! "
}
¸¸ 
}
˛˛ 	
public
ÅÅ 
void
ÅÅ *
BookingCancallationSMSToUser
ÅÅ 0
(
ÅÅ0 1
string
ÅÅ1 7
number
ÅÅ8 >
,
ÅÅ> ?
string
ÅÅ@ F
customerName
ÅÅG S
,
ÅÅS T
string
ÅÅU [
pageUrl
ÅÅ\ c
)
ÅÅc d
{
ÇÇ 	
try
ÉÉ 
{
ÑÑ  
EnumSMSServiceType
ÖÖ "
smsEnum
ÖÖ# *
=
ÖÖ+ , 
EnumSMSServiceType
ÖÖ- ?
.
ÖÖ? @+
BookingCancellationToCustomer
ÖÖ@ ]
;
ÖÖ] ^
string
ÜÜ 
message
ÜÜ 
=
ÜÜ  
string
ÜÜ! '
.
ÜÜ' (
Empty
ÜÜ( -
;
ÜÜ- .
message
áá 
=
áá 
string
áá  
.
áá  !
Format
áá! '
(
áá' (
$stráá( ∏
,áá∏ π
customerNameáá∫ ∆
)áá∆ «
;áá« »
	SMSCommon
ââ 
sc
ââ 
=
ââ 
new
ââ "
	SMSCommon
ââ# ,
(
ââ, -
)
ââ- .
;
ââ. /
sc
ää 
.
ää 

ProcessSMS
ää 
(
ää 
number
ää $
,
ää$ %
message
ää& -
,
ää- .
smsEnum
ää/ 6
,
ää6 7
pageUrl
ää8 ?
,
ää? @
true
ääA E
)
ääE F
;
ääF G
}
ãã 
catch
åå 
(
åå 
	Exception
åå 
err
åå  
)
åå  !
{
çç 
HttpContext
éé 
.
éé 
Current
éé #
.
éé# $
Trace
éé$ )
.
éé) *
Warn
éé* .
(
éé. /
$str
éé/ \
+
éé] ^
err
éé_ b
.
ééb c
Message
ééc j
)
ééj k
;
éék l

ErrorClass
èè 
objErr
èè !
=
èè" #
new
èè$ '

ErrorClass
èè( 2
(
èè2 3
err
èè3 6
,
èè6 7
$str
èè8 b
)
èèb c
;
èèc d
objErr
êê 
.
êê 
SendMail
êê 
(
êê  
)
êê  !
;
êê! "
}
ëë 
}
íí 	
public
úú 
void
úú $
UsedPurchaseInquirySMS
úú *
(
úú* + 
EnumSMSServiceType
úú+ =
smsType
úú> E
,
úúE F
string
úúG M
number
úúN T
,
úúT U
string
úúV \
message
úú] d
,
úúd e
string
úúf l
pageurl
úúm t
)
úút u
{
ùù 	
try
ûû 
{
üü 
	SMSCommon
†† 
sc
†† 
=
†† 
new
†† "
	SMSCommon
††# ,
(
††, -
)
††- .
;
††. /
sc
°° 
.
°° 

ProcessSMS
°° 
(
°° 
number
°° $
,
°°$ %
message
°°& -
,
°°- .
smsType
°°/ 6
,
°°6 7
pageurl
°°8 ?
)
°°? @
;
°°@ A
}
¢¢ 
catch
££ 
(
££ 
	Exception
££ 
ex
££ 
)
££  
{
§§ 

ErrorClass
•• 
objErr
•• !
=
••" #
new
••$ '

ErrorClass
••( 2
(
••2 3
ex
••3 5
,
••5 6
String
••7 =
.
••= >
Format
••> D
(
••D E
$str••E Ä
,••Ä Å
number••Ç à
,••à â
message••ä ë
,••ë í
pageurl••ì ö
)••ö õ
)••õ ú
;••ú ù
objErr
¶¶ 
.
¶¶ 
SendMail
¶¶ 
(
¶¶  
)
¶¶  !
;
¶¶! "
}
ßß 
}
®® 	
public
≤≤ 
void
≤≤ (
ApprovalUsedSellListingSMS
≤≤ .
(
≤≤. / 
EnumSMSServiceType
≤≤/ A
smsType
≤≤B I
,
≤≤I J
string
≤≤K Q
number
≤≤R X
,
≤≤X Y
string
≤≤Z `
	profileId
≤≤a j
,
≤≤j k
string
≤≤l r
customerName
≤≤s 
,≤≤ Ä
string≤≤Å á
pageurl≤≤à è
)≤≤è ê
{
≥≥ 	
string
¥¥ 
message
¥¥ 
=
¥¥ 
String
¥¥ #
.
¥¥# $
Format
¥¥$ *
(
¥¥* +
$str¥¥+ Æ
,¥¥Æ Ø
customerName¥¥∞ º
,¥¥º Ω
	profileId¥¥æ «
.¥¥« »
ToUpper¥¥» œ
(¥¥œ –
)¥¥– —
)¥¥— “
;¥¥“ ”
try
µµ 
{
∂∂ 
	SMSCommon
∑∑ 
sc
∑∑ 
=
∑∑ 
new
∑∑ "
	SMSCommon
∑∑# ,
(
∑∑, -
)
∑∑- .
;
∑∑. /
sc
ππ 
.
ππ 

ProcessSMS
ππ 
(
ππ 
number
ππ $
,
ππ$ %
message
ππ& -
,
ππ- .
smsType
ππ/ 6
,
ππ6 7
pageurl
ππ8 ?
)
ππ? @
;
ππ@ A
}
∫∫ 
catch
ªª 
(
ªª 
	Exception
ªª 
ex
ªª 
)
ªª  
{
ºº 

ErrorClass
ΩΩ 
objErr
ΩΩ !
=
ΩΩ" #
new
ΩΩ$ '

ErrorClass
ΩΩ( 2
(
ΩΩ2 3
ex
ΩΩ3 5
,
ΩΩ5 6
String
ΩΩ7 =
.
ΩΩ= >
Format
ΩΩ> D
(
ΩΩD E
$strΩΩE à
,ΩΩà â
numberΩΩä ê
,ΩΩê ë
messageΩΩí ô
,ΩΩô ö
pageurlΩΩõ ¢
,ΩΩ¢ £
	profileIdΩΩ§ ≠
)ΩΩ≠ Æ
)ΩΩÆ Ø
;ΩΩØ ∞
objErr
ææ 
.
ææ 
SendMail
ææ 
(
ææ  
)
ææ  !
;
ææ! "
}
øø 
}
¿¿ 	
public
≈≈ 
void
≈≈ )
RejectionUsedSellListingSMS
≈≈ /
(
≈≈/ 0 
EnumSMSServiceType
≈≈0 B
smsType
≈≈C J
,
≈≈J K
string
≈≈L R
number
≈≈S Y
,
≈≈Y Z
string
≈≈[ a
	profileId
≈≈b k
,
≈≈k l
string
≈≈m s
pageurl
≈≈t {
)
≈≈{ |
{
∆∆ 	
string
«« 
message
«« 
=
«« 
String
«« #
.
««# $
Format
««$ *
(
««* +
$str««+ ∆
,««∆ «
	profileId««» —
.««— “
ToUpper««“ Ÿ
(««Ÿ ⁄
)««⁄ €
)««€ ‹
;««‹ ›
try
»» 
{
…… 
	SMSCommon
   
sc
   
=
   
new
   "
	SMSCommon
  # ,
(
  , -
)
  - .
;
  . /
sc
ÃÃ 
.
ÃÃ 

ProcessSMS
ÃÃ 
(
ÃÃ 
number
ÃÃ $
,
ÃÃ$ %
message
ÃÃ& -
,
ÃÃ- .
smsType
ÃÃ/ 6
,
ÃÃ6 7
pageurl
ÃÃ8 ?
)
ÃÃ? @
;
ÃÃ@ A
}
ÕÕ 
catch
ŒŒ 
(
ŒŒ 
	Exception
ŒŒ 
ex
ŒŒ 
)
ŒŒ  
{
œœ 

ErrorClass
–– 
objErr
–– !
=
––" #
new
––$ '

ErrorClass
––( 2
(
––2 3
ex
––3 5
,
––5 6
String
––7 =
.
––= >
Format
––> D
(
––D E
$str––E Å
,––Å Ç
number––É â
,––â ä
message––ã í
,––í ì
pageurl––î õ
,––õ ú
	profileId––ù ¶
)––¶ ß
)––ß ®
;––® ©
objErr
—— 
.
—— 
SendMail
—— 
(
——  
)
——  !
;
——! "
}
““ 
}
”” 	
public
÷÷ 
void
÷÷ *
UsedSellSuccessfulListingSMS
÷÷ 0
(
÷÷0 1 
EnumSMSServiceType
÷÷1 C
smsType
÷÷D K
,
÷÷K L
string
÷÷M S
number
÷÷T Z
,
÷÷Z [
string
÷÷\ b
	profileid
÷÷c l
,
÷÷l m
string
÷÷n t
pageurl
÷÷u |
)
÷÷| }
{
◊◊ 	
string
ÿÿ 
message
ÿÿ 
=
ÿÿ 
String
ÿÿ #
.
ÿÿ# $
Format
ÿÿ$ *
(
ÿÿ* +
$strÿÿ+ …
,ÿÿ…  
	profileidÿÿÀ ‘
.ÿÿ‘ ’
ToUpperÿÿ’ ‹
(ÿÿ‹ ›
)ÿÿ› ﬁ
)ÿÿﬁ ﬂ
;ÿÿﬂ ‡
try
ŸŸ 
{
⁄⁄ 
	SMSCommon
€€ 
sc
€€ 
=
€€ 
new
€€ "
	SMSCommon
€€# ,
(
€€, -
)
€€- .
;
€€. /
sc
‹‹ 
.
‹‹ 

ProcessSMS
‹‹ 
(
‹‹ 
number
‹‹ $
,
‹‹$ %
message
‹‹& -
,
‹‹- .
smsType
‹‹/ 6
,
‹‹6 7
pageurl
‹‹8 ?
)
‹‹? @
;
‹‹@ A
}
›› 
catch
ﬁﬁ 
(
ﬁﬁ 
	Exception
ﬁﬁ 
ex
ﬁﬁ 
)
ﬁﬁ  
{
ﬂﬂ 

ErrorClass
‡‡ 
objErr
‡‡ !
=
‡‡" #
new
‡‡$ '

ErrorClass
‡‡( 2
(
‡‡2 3
ex
‡‡3 5
,
‡‡5 6
String
‡‡7 =
.
‡‡= >
Format
‡‡> D
(
‡‡D E
$str
‡‡E ~
,
‡‡~ 
number‡‡Ä Ü
,‡‡Ü á
message‡‡à è
,‡‡è ê
pageurl‡‡ë ò
)‡‡ò ô
)‡‡ô ö
;‡‡ö õ
objErr
·· 
.
·· 
SendMail
·· 
(
··  
)
··  !
;
··! "
}
‚‚ 
}
„„ 	
public
ËË 
void
ËË .
 ApprovalEditedUsedSellListingSMS
ËË 4
(
ËË4 5 
EnumSMSServiceType
ËË5 G
smsType
ËËH O
,
ËËO P
string
ËËQ W
number
ËËX ^
,
ËË^ _
string
ËË` f
	profileId
ËËg p
,
ËËp q
string
ËËr x
customerNameËËy Ö
,ËËÖ Ü
stringËËá ç
pageurlËËé ï
)ËËï ñ
{
ÈÈ 	
string
ÍÍ 
message
ÍÍ 
=
ÍÍ 
String
ÍÍ #
.
ÍÍ# $
Format
ÍÍ$ *
(
ÍÍ* +
$strÍÍ+ Ω
,ÍÍΩ æ
customerNameÍÍø À
,ÍÍÀ Ã
	profileIdÍÍÕ ÷
.ÍÍ÷ ◊
ToUpperÍÍ◊ ﬁ
(ÍÍﬁ ﬂ
)ÍÍﬂ ‡
)ÍÍ‡ ·
;ÍÍ· ‚
try
ÎÎ 
{
ÏÏ 
	SMSCommon
ÌÌ 
sc
ÌÌ 
=
ÌÌ 
new
ÌÌ "
	SMSCommon
ÌÌ# ,
(
ÌÌ, -
)
ÌÌ- .
;
ÌÌ. /
sc
ÔÔ 
.
ÔÔ 

ProcessSMS
ÔÔ 
(
ÔÔ 
number
ÔÔ $
,
ÔÔ$ %
message
ÔÔ& -
,
ÔÔ- .
smsType
ÔÔ/ 6
,
ÔÔ6 7
pageurl
ÔÔ8 ?
)
ÔÔ? @
;
ÔÔ@ A
}
 
catch
ÒÒ 
(
ÒÒ 
	Exception
ÒÒ 
ex
ÒÒ 
)
ÒÒ  
{
ÚÚ 

ErrorClass
ÛÛ 
objErr
ÛÛ !
=
ÛÛ" #
new
ÛÛ$ '

ErrorClass
ÛÛ( 2
(
ÛÛ2 3
ex
ÛÛ3 5
,
ÛÛ5 6
String
ÛÛ7 =
.
ÛÛ= >
Format
ÛÛ> D
(
ÛÛD E
$strÛÛE Ü
,ÛÛÜ á
numberÛÛà é
,ÛÛé è
messageÛÛê ó
,ÛÛó ò
pageurlÛÛô †
,ÛÛ† °
	profileIdÛÛ¢ ´
)ÛÛ´ ¨
)ÛÛ¨ ≠
;ÛÛ≠ Æ
objErr
ÙÙ 
.
ÙÙ 
SendMail
ÙÙ 
(
ÙÙ  
)
ÙÙ  !
;
ÙÙ! "
}
ıı 
}
ˆˆ 	
public
¸¸ 
void
¸¸ /
!RejectionEditedUsedSellListingSMS
¸¸ 5
(
¸¸5 6 
EnumSMSServiceType
¸¸6 H
smsType
¸¸I P
,
¸¸P Q
string
¸¸R X
number
¸¸Y _
,
¸¸_ `
string
¸¸a g
	profileId
¸¸h q
,
¸¸q r
string
¸¸s y
customerName¸¸z Ü
,¸¸Ü á
string¸¸à é
pageurl¸¸è ñ
)¸¸ñ ó
{
˝˝ 	
string
˛˛ 
message
˛˛ 
=
˛˛ 
String
˛˛ #
.
˛˛# $
Format
˛˛$ *
(
˛˛* +
$str˛˛+ ⁄
,˛˛⁄ €
customerName˛˛‹ Ë
,˛˛Ë È
	profileId˛˛Í Û
.˛˛Û Ù
ToUpper˛˛Ù ˚
(˛˛˚ ¸
)˛˛¸ ˝
)˛˛˝ ˛
;˛˛˛ ˇ
try
ˇˇ 
{
ÄÄ 
	SMSCommon
ÅÅ 
sc
ÅÅ 
=
ÅÅ 
new
ÅÅ "
	SMSCommon
ÅÅ# ,
(
ÅÅ, -
)
ÅÅ- .
;
ÅÅ. /
sc
ÉÉ 
.
ÉÉ 

ProcessSMS
ÉÉ 
(
ÉÉ 
number
ÉÉ $
,
ÉÉ$ %
message
ÉÉ& -
,
ÉÉ- .
smsType
ÉÉ/ 6
,
ÉÉ6 7
pageurl
ÉÉ8 ?
)
ÉÉ? @
;
ÉÉ@ A
}
ÑÑ 
catch
ÖÖ 
(
ÖÖ 
	Exception
ÖÖ 
ex
ÖÖ 
)
ÖÖ  
{
ÜÜ 

ErrorClass
áá 
objErr
áá !
=
áá" #
new
áá$ '

ErrorClass
áá( 2
(
áá2 3
ex
áá3 5
,
áá5 6
String
áá7 =
.
áá= >
Format
áá> D
(
ááD E
$strááE á
,ááá à
numberááâ è
,ááè ê
messageááë ò
,ááò ô
pageurlááö °
,áá° ¢
	profileIdáá£ ¨
)áá¨ ≠
)áá≠ Æ
;ááÆ Ø
objErr
àà 
.
àà 
SendMail
àà 
(
àà  
)
àà  !
;
àà! "
}
ââ 
}
ää 	
public
êê 
void
êê %
ServiceCenterDetailsSMS
êê +
(
êê+ ,
string
êê, 2
number
êê3 9
,
êê9 :
string
êê; A
name
êêB F
,
êêF G
string
êêH N
address
êêO V
,
êêV W
string
êêX ^
phone
êê_ d
,
êêd e
string
êêf l
city
êêm q
,
êêq r
string
êês y
pageUrlêêz Å
)êêÅ Ç
{
ëë 	
try
íí 
{
ìì 
string
îî 
message
îî 
=
îî  
String
îî! '
.
îî' (
Format
îî( .
(
îî. /
$strîî/ ñ
,îîñ ó
Environmentîîò £
.îî£ §
NewLineîî§ ´
,îî´ ¨
nameîî≠ ±
,îî± ≤
addressîî≥ ∫
,îî∫ ª
cityîîº ¿
,îî¿ ¡
phoneîî¬ «
)îî« »
;îî» … 
EnumSMSServiceType
ññ "
esms
ññ# '
=
ññ( ) 
EnumSMSServiceType
ññ* <
.
ññ< =/
!ServiceCenterDetailsSMSToCustomer
ññ= ^
;
ññ^ _
	SMSCommon
óó 
sc
óó 
=
óó 
new
óó "
	SMSCommon
óó# ,
(
óó, -
)
óó- .
;
óó. /
sc
òò 
.
òò 

ProcessSMS
òò 
(
òò 
number
òò $
,
òò$ %
message
òò& -
,
òò- .
esms
òò/ 3
,
òò3 4
pageUrl
òò5 <
)
òò< =
;
òò= >
}
ôô 
catch
öö 
(
öö 
	Exception
öö 
ex
öö 
)
öö  
{
õõ 

ErrorClass
úú 
objErr
úú !
=
úú" #
new
úú$ '

ErrorClass
úú( 2
(
úú2 3
ex
úú3 5
,
úú5 6
String
úú7 =
.
úú= >
Format
úú> D
(
úúD E
$strúúE Ö
,úúÖ Ü
numberúúá ç
,úúç é
nameúúè ì
,úúì î
addressúúï ú
,úúú ù
phoneúúû £
,úú£ §
cityúú• ©
,úú© ™
pageUrlúú´ ≤
)úú≤ ≥
)úú≥ ¥
;úú¥ µ
objErr
ùù 
.
ùù 
SendMail
ùù 
(
ùù  
)
ùù  !
;
ùù! "
}
ûû 
}
üü 	
public
¶¶ 
void
¶¶ (
ExpiringListingReminderSMS
¶¶ .
(
¶¶. /
string
¶¶/ 5
number
¶¶6 <
,
¶¶< =
string
¶¶> D
pageUrl
¶¶E L
,
¶¶L M 
EnumSMSServiceType
¶¶N `
esms
¶¶a e
,
¶¶e f
string
¶¶g m
message
¶¶n u
)
¶¶u v
{
ßß 	
try
®® 
{
©© 
	SMSCommon
™™ 
sc
™™ 
=
™™ 
new
™™ "
	SMSCommon
™™# ,
(
™™, -
)
™™- .
;
™™. /
sc
´´ 
.
´´ 

ProcessSMS
´´ 
(
´´ 
number
´´ $
,
´´$ %
message
´´& -
,
´´- .
esms
´´/ 3
,
´´3 4
pageUrl
´´5 <
)
´´< =
;
´´= >
}
¨¨ 
catch
≠≠ 
(
≠≠ 
	Exception
≠≠ 
ex
≠≠ 
)
≠≠  
{
ÆÆ 

ErrorClass
ØØ 
objErr
ØØ !
=
ØØ" #
new
ØØ$ '

ErrorClass
ØØ( 2
(
ØØ2 3
ex
ØØ3 5
,
ØØ5 6
String
ØØ7 =
.
ØØ= >
Format
ØØ> D
(
ØØD E
$strØØE Ä
,ØØÄ Å
numberØØÇ à
,ØØà â
pageUrlØØä ë
,ØØë í
esmsØØì ó
,ØØó ò
messageØØô †
)ØØ† °
)ØØ° ¢
;ØØ¢ £
objErr
∞∞ 
.
∞∞ 
SendMail
∞∞ 
(
∞∞  
)
∞∞  !
;
∞∞! "
}
±± 
}
≤≤ 	
}
≥≥ 
}¥¥ ‰
DD:\work\bikewaleweb\Bikewale.Notifications\SendEmailOnModelChange.cs
	namespace 	
Bikewale
 
. 
Notifications  
{ 
public 

class "
SendEmailOnModelChange '
{ 
public 
static 
void &
SendModelDiscontinuedEmail 5
(5 6
string6 <
	userEmail= F
,F G
stringH N
makeNameO W
,W X
stringY _
	modelName` i
,i j
DateTimej r
dates w
)w x
{ 	
ComposeEmailBase 
objEmail %
=& '
new( +!
ModelDiscontinuedMail, A
(A B
makeNameB J
,J K
	modelNameL U
,U V
dateW [
)[ \
;\ ]
objEmail 
. 
Send 
( 
	userEmail #
,# $
string% +
.+ ,
Format, 2
(2 3
$str3 Q
,Q R
makeNameS [
,[ \
	modelName] f
)f g
)g h
;h i
} 	
public 
static 
void *
SendModelMaskingNameChangeMail 9
(9 :
string: @
	userEmailA J
,J K
stringL R
makeNameS [
,[ \
string] c
	modelNamed m
,m n
stringn t
oldUrlu {
,{ |
string	| Ç
newUrl
É â
)
â ä
{ 	
ComposeEmailBase 
objEmail %
=& '
new( +'
ModelMaskingNameChangedMail, G
(G H
makeNameH P
,P Q
	modelNameR [
,[ \
oldUrl] c
,c d
newUrle k
)k l
;l m
objEmail 
. 
Send 
( 
	userEmail #
,# $
string% +
.+ ,
Format, 2
(2 3
$str3 O
,O P
makeNameQ Y
,Y Z
	modelName[ d
)d e
)e f
;f g
} 	
public## 
static## 
void## )
SendMakeMaskingNameChangeMail## 8
(##8 9
string##9 ?
	userEmail##@ I
,##I J
string##K Q
makeName##R Z
,##Z [
IEnumerable##[ f
<##f g
BikeModelMailEntity##g z
>##z {
models	##| Ç
)
##É Ñ
{$$ 	
ComposeEmailBase%% 
objEmail%% %
=%%& '
new%%( +&
MakeMaskingNameChangedMail%%, F
(%%F G
makeName%%G O
,%%O P
models%%Q W
)%%W X
;%%X Y
objEmail&& 
.&& 
Send&& 
(&& 
	userEmail&& #
,&&# $
string&&% +
.&&+ ,
Format&&, 2
(&&2 3
$str&&3 S
,&&S T
makeName&&U ]
)&&] ^
)&&^ _
;&&_ `
}'' 	
}(( 
})) Ø&
?D:\work\bikewaleweb\Bikewale.Notifications\UserReviewsEmails.cs
	namespace 	
Bikewale
 
. 
Notifications  
{ 
public		 

class		 
UserReviewsEmails		 "
{

 
public 
static 
void %
SendRatingSubmissionEmail 4
(4 5
string5 ;
userName< D
,D E
stringF L
	userEmailM V
,V W
stringX ^
makeName_ g
,g h
stringi o
	modelNamep y
,y z
string	z Ä

reviewLink
Å ã
)
ã å
{ 	
ComposeEmailBase 
objEmail %
=& '
new( +!
RatingSubmissionEmail, A
(A B
userNameB J
,J K
makeNameL T
,T U
	modelNameV _
,_ `

reviewLink` j
)j k
;k l
objEmail 
. 
Send 
( 
	userEmail #
,# $
string% +
.+ ,
Format, 2
(2 3
$str3 Q
,Q R
makeNameS [
,[ \
	modelName] f
)f g
)g h
;h i
} 	
public 
static 
void %
SendReviewSubmissionEmail 4
(4 5
string5 ;
userName< D
,D E
stringF L
	userEmailM V
,V W
stringX ^
makeName_ g
,g h
stringi o
	modelNamep y
)y z
{ 	
ComposeEmailBase 
objEmail %
=& '
new( +!
ReviewSubmissionEmail, A
(A B
userNameB J
,J K
makeNameL T
,T U
	modelNameV _
)_ `
;` a
objEmail 
. 
Send 
( 
	userEmail #
,# $
string% +
.+ ,
Format, 2
(2 3
$str3 e
,e f
makeNameg o
,o p
	modelNameq z
)z {
){ |
;| }
} 	
public   
static   
void   #
SendReviewReminderEmail   2
(  2 3
string  3 9
userName  : B
,  B C
string  D J
	userEmail  K T
,  T U
string  V \

reviewLink  ] g
,  g h
string  i o

ratingDate  p z
,  z {
string	  | Ç
makeName
  É ã
,
  ã å
string
  ç ì
	modelName
  î ù
)
  ù û
{!! 	
ComposeEmailBase"" 
objEmail"" %
=""& '
new""( +
ReviewReminderEmail"", ?
(""? @
userName""@ H
,""H I

reviewLink""J T
,""T U

ratingDate""U _
,""_ `
makeName""a i
,""i j
	modelName""k t
)""t u
;""u v
objEmail## 
.## 
Send## 
(## 
	userEmail## #
,### $
string##% +
.##+ ,
Format##, 2
(##2 3
$str##3 V
,##V W
makeName##X `
,##` a
	modelName##b k
)##k l
)##l m
;##m n
}$$ 	
public)) 
static)) 
void)) #
SendReviewApprovalEmail)) 2
())2 3
string))3 9
userName)): B
,))B C
string))D J
	userEmail))K T
,))T U
string))V \

reviewLink))] g
,))g h
string))i o
	modelName))p y
)))y z
{** 	
ComposeEmailBase++ 
objEmail++ %
=++& '
new++( +
ReviewApprovalEmail++, ?
(++? @
userName++@ H
,++H I

reviewLink++J T
,++T U
	modelName++V _
)++_ `
;++` a
objEmail,, 
.,, 
Send,, 
(,, 
	userEmail,, #
,,,# $
$str,,% V
),,V W
;,,W X
}-- 	
public22 
static22 
void22 $
SendReviewRejectionEmail22 3
(223 4
string224 :
userName22; C
,22C D
string22E K
	userEmail22L U
,22U V
string22W ]
	modelName22^ g
)22g h
{33 	
ComposeEmailBase44 
objEmail44 %
=44& '
new44( + 
ReviewRejectionEmail44, @
(44@ A
userName44A I
,44I J
	modelName44K T
)44T U
;44U V
objEmail55 
.55 
Send55 
(55 
	userEmail55 #
,55# $
$str55% W
)55W X
;55X Y
}66 	
}88 
}99 