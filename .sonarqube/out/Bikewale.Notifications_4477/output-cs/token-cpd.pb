�
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
}@@ �
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
} �
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
} �:
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
$str	TTq �
]
TT� �
)
TT� �
;
TT� �
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
$str	``~ �
)
``� �
;
``� �
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
�� 	
}
�� 
}�� �
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
$str	/ �
;
� �
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
$str	((p �
]
((� �
,
((� �
	ServerVar
((� �
[
((� �
$str
((� �
]
((� �
,
((� �
_error
((� �
.
((� �
	ErrorType
((� �
,
((� �
_error
((� �
.
((� �
Message
((� �
,
((� �
	ServerVar
((� �
[
((� �
$str
((� �
]
((� �
,
((� �
_error
((� �
.
((� �

SourceFile
((� �
,
((� �
_error
((� �
.
((� �
LineNo
((� �
,
((� �
_error
((� �
.
((� �
Trace
((� �
)
((� �
;
((� �
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
}22 �.
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
customerMobile	~ �
,
� �
string
� �
bookingDate
� �
,
� �
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
QQ� �
,
QQ� �
CityName
QQ� �
)
QQ� �
;
QQ� �
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
}\\ �
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
$str	( �
,
� �
_bwId
� �
,
� �
_feedbackText
� �
)
� �
)
� �
;
� �
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
}%% �
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
$str	% �
)
� �
;
� �
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
$str	''% �
)
''� �
;
''� �
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
$str	--% �
)
--� �
;
--� �
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
}88 �
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
$str	: �
;
� �
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
$str	))p �
]
))� �
,
))� �
	ServerVar
))� �
[
))� �
$str
))� �
]
))� �
,
))� �
	ServerVar
))� �
[
))� �
$str
))� �
]
))� �
,
))� �
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
}44 �
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
$str	+ �
;
� �
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
}11 �
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
$str	## �
)
##� �
;
##� �
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
}33 �
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
})) �
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
}'' �
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
Bikewale	| �
.
� �
Utility
� �
.
� � 
BWOprConfiguration
� �
.
� �
Instance
� �
.
� �
BwOprHostUrlForJs
� �
)
� �
;
� �!
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
$str	f �
)
� �
;
� �
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
}&& �:
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
string	$$} �
	imagePath
$$� �
)
$$� �
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
$str	**" �
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
$str	00I �
,
00� �
customerEmail
00� �
)
00� �
:
00� �
$str
00� �
)
00� �
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
$str	::* �
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
$str	FF2 �
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
$str	MM& �
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
$str	RR$ �
)
RR� �
;
RR� �
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
$str	UU. �
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
$str	\\" �
)
\\� �
;
\\� �
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
$str	``e �
)
``� �
;
``� �
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
}qq �]
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
customerName	v �
,
� �
string
� �
customerEmail
� �
,
� �
string
� �
customerMobile
� �
,
� �
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
date	~ �
,
� �
string
� �
	imagePath
� �
,
� �
uint
� �
insuranceAmount
� �
=
� �
$num
� �
)
� �
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
$str	 �
+ 
$str R
+ 
$str L
+  !
$str	" �
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
$str	-- �
+.. 
$str	.. �
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
$str	DD �
+EE 
$strEE u
+FF  !
$str	FF" �
+GG$ %
$strGG& f
+HH( )
$strHH* t
+II$ %
$strII& .
+JJ  !
$strJJ" *
+KK  !
$str	KK" �
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
,	LL �
customerMobile
LL� �
,
LL� �
areaName
LL� �
+
LL� �
$str
LL� �
+
LL� �
cityName
LL� �
,
LL� �
	imagePath
LL� �
)
LL� �
;
LL� �
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
$str	UU" �
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
$str	__" �
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
$str	ii& �
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
$str	rr �
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
�� 
(
�� 
	offerList
�� 
!=
��  
null
��! %
&&
��& (
	offerList
��) 2
.
��2 3
Count
��3 8
>
��9 :
$num
��; <
)
��< =
{
�� 
sb
�� 
.
�� 
AppendFormat
�� #
(
��# $
$str
�� }
+
�� 
$str�� �
+
�� 
$str
�� K
)
�� 
;
�� 
foreach
�� 
(
�� 
var
��  
offer
��! &
in
��' )
	offerList
��* 3
)
��3 4
{
�� 
sb
�� 
.
�� 
AppendFormat
�� '
(
��' (
$str
��( U
,
��U V
offer
��W \
.
��\ ]
	OfferText
��] f
)
��f g
;
��g h
}
�� 
sb
�� 
.
�� 
AppendFormat
�� #
(
��# $
$str
�� #
+
�� 
$str
�� ?
)
�� 
;
�� 
}
�� 
sb
�� 
.
�� 
AppendFormat
�� 
(
��  
$str�� �
+
�� 
$str�� �
+
�� 
$str
�� v
+
��  !
$str��" �
+
��$ %
$str
��& K
+
��( )
$str��* �
+
��$ %
$str
��& .
+
��$ %
$str
��& e
+
��  !
$str
��" *
+
��  !
$str��" �
+
��$ %
$str
��& K
+
��' (
$str��) �
+
��$ %
$str
��& .
+
��$ %
$str
��& `
+
��  !
$str
��" *
+
��  
$str��! �
+
��$ %
$str
��& K
+
��( )
$str��* �
+
��$ %
$str
��& .
+
��$ %
$str
��& e
+
��  !
$str
��" *
+
�� 
$str
�� &
+
�� 
$str
�� A
+
�� 
$str�� �
+
�� 
$str�� �
+
�� 
$str
�� d
+
�� 
$str
�� "
+
�� 
$str
�� a
+
�� 
$str�� �
+
�� 
$str�� �
+
�� 
$str
�� "
+
�� 
$str
�� 
)
�� 
;
�� 
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
;��� �
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
�� 
MailHTML
�� 
=
�� 
sb
�� 
.
�� 
ToString
�� "
(
��" #
)
��# $
;
��$ %
}
�� 	
public
�� 
override
�� 
string
�� 
ComposeBody
�� *
(
��* +
)
��+ ,
{
�� 	
return
�� 
MailHTML
�� 
;
�� 
}
�� 	
	protected
�� 
UInt32
�� "
TotalDiscountedPrice
�� -
(
��- .
List
��. 2
<
��2 3
PQ_Price
��3 ;
>
��; <
discountList
��= I
)
��I J
{
�� 	
UInt32
�� 

totalPrice
�� 
=
�� 
$num
��  !
;
��! "
if
�� 
(
�� 
discountList
�� 
!=
�� 
null
��  $
&&
��% '
discountList
��( 4
.
��4 5
Count
��5 :
>
��; <
$num
��= >
)
��> ?
{
�� 
foreach
�� 
(
�� 
var
�� 
priceListObj
�� )
in
��* ,
discountList
��- 9
)
��9 :
{
�� 

totalPrice
�� 
+=
�� !
priceListObj
��" .
.
��. /
Price
��/ 4
;
��4 5
}
�� 
}
�� 
return
�� 

totalPrice
�� 
;
�� 
}
�� 	
}
�� 
}�� �8
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
� �
dealerMobileNo
� �
,
� �
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
$str	 �
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
$str	""& �
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
$str	*** �
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
$str	.." �
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
$str	33 �
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
$str	;; �
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
$str	@@ �
)
@@� �
;
@@� �
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
$str	CC& �
,
CC� �
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
$str	HH �
)
HH� �
;
HH� �
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
}PP �+
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
}55 �'
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
MakeName	z �
+
� �
$str
� �
+
� �
objMetas
� �
.
� �
	ModelName
� �
+
� �
$str
� �
)
� �
;
� �
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
MakeName	##z �
+
##� �
$str
##� �
)
##� �
;
##� �
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
;	;; �
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
}EE �
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
$str	 �
)
� �
;
� �
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
$str	22f �
)
22� �
;
22� �
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
}<< �\
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
customerEmail	z �
,
� �
uint
� �

totalPrice
� �
,
� �
uint
� �
bookingAmount
� �
,
� �
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

dealerName	 �
,
� �
List
� �
<
� �
OfferEntity
� �
>
� �
	offerList
� �
,
� �
string
� �
	imagePath
� �
,
� �
string
� �
versionName
� �
,
� �
uint
� �
insuranceAmount
� �
=
� �
$num
� �
)
� �
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
$str	  �
+ 
$str	 �
+ 
$str =
+ 
$str 5
+   
$str   X
+!! 
$str!! i
+"" 
$str	"" �
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
$str	22 �
+33 
$str33 h
+44 
$str44 g
+55 
$str55 m
+66 
$str	66 �
+77 
$str	77 �
+88 
$str88 M
+99 
$str99 *
+:: 
$str:: &
+;; 
$str	;; �
+<< 
$str<< i
+== 
$str== `
+>> 
$str	>> �
+?? 
$str?? *
+@@ 
$str@@ Z
+AA 
$strAA g
+BB 
$str	BB �
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
$str	RR �
+SS 
$strSS p
+TT 
$str	TT �
+UU 
$str	UU �
+VV 
$str	VV �
+WW 
$str	WW �
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
$str	dd �
+ee 
$stree J
+ff 
$strff x
+gg 
$strgg 
+hh 
$strhh s
+ii 
$str	ii �
+jj 
$str	jj �
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
$str	ww �
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
�� 
$str�� �
+
�� 
$str
�� ?
+
�� 
$str
�� !
,
�� 
Format
�� 
.
�� 
FormatPrice
�� (
(
��( )
bookingAmount
��) 6
.
��6 7
ToString
��7 ?
(
��? @
)
��@ A
)
��A B
)
�� 
;
�� 
sb
�� 
.
�� 
AppendFormat
�� 
(
��  
$str
��  F
+
�� 
$str
�� u
+
�� 
$str�� �
+
�� 
$str
�� I
+
�� 
$str
�� 7
,
�� 
Format
�� 
.
�� 
FormatPrice
�� $
(
��$ %
(
��% &
balanceAmount
��& 3
-
��4 5"
TotalDiscountedPrice
��6 J
(
��J K
discountList
��K W
)
��W X
)
��X Y
.
��Y Z
ToString
��Z b
(
��b c
)
��c d
)
��d e
)
�� 
;
�� 
if
�� 
(
�� 
	offerList
�� 
!=
��  
null
��! %
&&
��& (
	offerList
��) 2
.
��2 3
Count
��3 8
>
��9 :
$num
��; <
)
��< =
{
�� 
sb
�� 
.
�� 
AppendFormat
�� #
(
��# $
$str
�� Z
+
�� 
$str�� �
+
�� 
$str
�� e
)
��e f
;
��f g
foreach
�� 
(
�� 
var
��  
offer
��! &
in
��' )
	offerList
��* 3
)
��3 4
{
�� 
sb
�� 
.
�� 
AppendFormat
�� '
(
��' (
$str
�� |
+
��  !
$str��" �
+
��  !
$str��" �
+
��  !
$str
��" E
+
�� 
$str
�� "
,
�� 
offer
�� 
.
��  
OfferCategoryId
��  /
,
�� 
offer
�� 
.
��  
	OfferText
��  )
)
�� 
;
�� 
}
�� 
sb
�� 
.
�� 
AppendFormat
�� #
(
��# $
$str
��$ 2
)
��2 3
;
��3 4
}
�� 
sb
�� 
.
�� 
AppendFormat
�� 
(
��  
$str
��  |
+
�� 
$str�� �
+
�� 
$str�� �
+
�� 
$str
�� s
+
�� 
$str
�� 
+
�� 
$str
�� "
+
�� 
$str
�� 
+
�� 
$str�� �
+
�� 
$str
�� q
+
�� 
$str�� �
+
�� 
$str
�� ~
+
�� 
$str�� �
+
�� 
$str
�� *
+
�� 
$str
�� &
+
�� 
$str
�� h
+
�� 
$str
�� %
+
�� 
$str�� �
+
�� 
$str
�� *
+
�� 
$str
�� &
+
�� 
$str
�� B
+
�� 
$str
�� "
+
�� 
$str
�� 
+
�� 
$str
�� g
+
�� 
$str
�� 
+
�� 
$str
�� 
+
�� 
$str
�� 
)
�� 
;
�� 
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
;��� �
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
�� 
MailHTML
�� 
=
�� 
sb
�� 
.
�� 
ToString
�� "
(
��" #
)
��# $
;
��$ %
}
�� 	
public
�� 
override
�� 
string
�� 
ComposeBody
�� *
(
��* +
)
��+ ,
{
�� 	
return
�� 
MailHTML
�� 
;
�� 
}
�� 	
	protected
�� 
UInt32
�� "
TotalDiscountedPrice
�� -
(
��- .
List
��. 2
<
��2 3
PQ_Price
��3 ;
>
��; <
discountList
��= I
)
��I J
{
�� 	
UInt32
�� 

totalPrice
�� 
=
�� 
$num
��  !
;
��! "
if
�� 
(
�� 
discountList
�� 
!=
�� 
null
��  $
&&
��% '
discountList
��( 4
.
��4 5
Count
��5 :
>
��; <
$num
��= >
)
��> ?
{
�� 
foreach
�� 
(
�� 
var
�� 
priceListObj
�� )
in
��* ,
discountList
��- 9
)
��9 :
{
�� 

totalPrice
�� 
+=
�� !
priceListObj
��" .
.
��. /
Price
��/ 4
;
��4 5
}
�� 
}
�� 
return
�� 

totalPrice
�� 
;
�� 
}
�� 	
}
�� 
}�� ��
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
customerEmail	!!~ �
,
!!� �
uint
!!� �

totalPrice
!!� �
,
!!� �
uint
!!� �
bookingAmount
!!� �
,
!!� �
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

dealerName	"" �
,
""� �
List
""� �
<
""� �
OfferEntity
""� �
>
""� �
	offerList
""� �
,
""� �
uint
""� �
insuranceAmount
""� �
=
""� �
$num
""� �
)
""� �
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
$str	== �
)
==� �
;
==� �
sb>> 
.>> 
Append>> 
(>> 
$str	>> �
)
>>� �
;
>>� �
sb?? 
.?? 
Append?? 
(?? 
$str	?? �
)
??� �
;
??� �
sb@@ 
.@@ 
Append@@ 
(@@ 
$str	@@ �
+
@@� �
BikeName
@@� �
+
@@� �
(
@@� �
!
@@� �
String
@@� �
.
@@� �
IsNullOrEmpty
@@� �
(
@@� �
	BikeColor
@@� �
)
@@� �
?
@@� �
$str
@@� �
+
@@� �
	BikeColor
@@� �
+
@@� �
$str
@@� �
:
@@� �
$str
@@� �
)
@@� �
+
@@� �
$str
@@� �
+
@@� �
DateTime
@@� �
.
@@� �
Now
@@� �
.
@@� �
ToString
@@� �
(
@@� �
$str
@@� �
)
@@� �
+
@@� �
$str
@@� �
)
@@� �
;
@@� �
sbAA 
.AA 
AppendAA 
(AA 
$str	AA �
)
AA� �
;
AA� �
sbBB 
.BB 
AppendBB 
(BB 
$strBB i
+BBj k

DealerNameBBl v
+BBw x
$str	BBy �
)
BB� �
;
BB� �
sbCC 
.CC 
AppendCC 
(CC 
$strCC s
+CCt u
CustomerName	CCv �
+
CC� �
$str
CC� �
)
CC� �
;
CC� �
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
$str	DDp �
+
DD� �
BikeName
DD� �
+
DD� �
$str
DD� �
+
DD� � 
BookingReferenceNo
DD� �
+
DD� �
$str
DD� �
)
DD� �
;
DD� �
sbEE 
.EE 
AppendEE 
(EE 
$str	EE �
)
EE� �
;
EE� �
sbFF 
.FF 
AppendFF 
(FF 
$str	FF �
)
FF� �
;
FF� �
sbGG 
.GG 
AppendGG 
(GG 
$str	GG �
+
GG� �
CustomerName
GG� �
+
GG� �
$str
GG� �
)
GG� �
;
GG� �
sbHH 
.HH 
AppendHH 
(HH 
$str	HH �
+
HH� �
CustomerMobile
HH� �
+
HH� �
$str
HH� �
)
HH� �
;
HH� �
sbII 
.II 
AppendII 
(II 
$str	II �
+
II� �
CustomerEmail
II� �
+
II� �
$str
II� �
)
II� �
;
II� �
sbJJ 
.JJ 
AppendJJ 
(JJ 
$str	JJ �
+
JJ� �
CustomerArea
JJ� �
+
JJ� �
$str
JJ� �
)
JJ� �
;
JJ� �
sbKK 
.KK 
AppendKK 
(KK 
$str	KK �
+
KK� �
BikeName
KK� �
+
KK� �
$str
KK� �
+
KK� �
	BikeColor
KK� �
+
KK� �
$str
KK� �
)
KK� �
;
KK� �
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
$str	PP �
)
PP� �
;
PP� �
sbQQ 
.QQ 
AppendQQ 
(QQ 
$str	QQ �
)
QQ� �
;
QQ� �
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
$str	XX& �
+
XX� �
Format
XX� �
.
XX� �
FormatPrice
XX� �
(
XX� �
items
XX� �
.
XX� �
Price
XX� �
.
XX� �
ToString
XX� �
(
XX� �
)
XX� �
)
XX� �
+
XX� �
$str
XX� �
)
XX� �
;
XX� �
}YY 
elseZZ 
{[[ 
sb\\ 
.\\ 
Append\\ %
(\\% &
$str	\\& �
+
\\� �
Format
\\� �
.
\\� �
FormatPrice
\\� �
(
\\� �
items
\\� �
.
\\� �
Price
\\� �
.
\\� �
ToString
\\� �
(
\\� �
)
\\� �
)
\\� �
+
\\� �
$str
\\� �
)
\\� �
;
\\� �
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
$str	bb" �
)
bb� �
;
bb� �
sbcc 
.cc 
Appendcc !
(cc! "
$str	cc" �
+
cc� �
Format
cc� �
.
cc� �
FormatPrice
cc� �
(
cc� �

TotalPrice
cc� �
.
cc� �
ToString
cc� �
(
cc� �
)
cc� �
)
cc� �
+
cc� �
$str
cc� �
)
cc� �
;
cc� �
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
$str	ffy �
+
ff� �
Format
ff� �
.
ff� �
FormatPrice
ff� �
(
ff� �
list
ff� �
.
ff� �
Price
ff� �
.
ff� �
ToString
ff� �
(
ff� �
)
ff� �
)
ff� �
+
ff� �
$str
ff� �
)
ff� �
;
ff� �
}gg 
sbii 
.ii 
Appendii !
(ii! "
$str	ii" �
)
ii� �
;
ii� �
sbjj 
.jj 
Appendjj !
(jj! "
$str	jj" �
+
jj� �
Format
jj� �
.
jj� �
FormatPrice
jj� �
(
jj� �
(
jj� �

TotalPrice
jj� �
-
jj� �"
TotalDiscountedPrice
jj� �
(
jj� �
)
jj� �
)
jj� �
.
jj� �
ToString
jj� �
(
jj� �
)
jj� �
)
jj� �
+
jj� �
$str
jj� �
)
jj� �
;
jj� �
}kk 
elsell 
{mm 
sbnn 
.nn 
Appendnn !
(nn! "
$str	nn" �
)
nn� �
;
nn� �
sboo 
.oo 
Appendoo !
(oo! "
$str	oo" �
+
oo� �
Format
oo� �
.
oo� �
FormatPrice
oo� �
(
oo� �

TotalPrice
oo� �
.
oo� �
ToString
oo� �
(
oo� �
)
oo� �
)
oo� �
+
oo� �
$str
oo� �
)
oo� �
;
oo� �
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
$str	ss �
)
ss� �
;
ss� �
sbtt 
.tt 
Appendtt 
(tt 
$str	tt �
+
tt� �
Format
tt� �
.
tt� �
FormatPrice
tt� �
(
tt� �
BookingAmount
tt� �
.
tt� �
ToString
tt� �
(
tt� �
)
tt� �
)
tt� �
+
tt� �
$str
tt� �
)
tt� �
;
tt� �
sbuu 
.uu 
Appenduu 
(uu 
$str	uu �
)
uu� �
;
uu� �
sbvv 
.vv 
Appendvv 
(vv 
$str	vv �
+
vv� �
Format
vv� �
.
vv� �
FormatPrice
vv� �
(
vv� �
(
vv� �
BalanceAmount
vv� �
-
vv� �"
TotalDiscountedPrice
vv� �
(
vv� �
)
vv� �
)
vv� �
.
vv� �
ToString
vv� �
(
vv� �
)
vv� �
)
vv� �
+
vv� �
$str
vv� �
)
vv� �
;
vv� �
sbww 
.ww 
Appendww 
(ww 
$str	ww �
)
ww� �
;
ww� �
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
$str	{{ �
)
{{� �
;
{{� �
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
�� 
sb
�� 
.
�� 
Append
�� %
(
��% &
$str��& �
+��� �
items��� �
.��� �
	OfferText��� �
+��� �
$str��� �
)��� �
;��� �
}
�� 
}
�� 
else
�� 
{
�� 
sb
�� 
.
�� 
AppendFormat
�� '
(
��' (
$str��( �
,��� �
InsuranceAmount��� �
)��� �
;��� �
}
�� 
sb
�� 
.
�� 
Append
�� 
(
�� 
$str�� �
)��� �
;��� �
}
�� 
sb
�� 
.
�� 
Append
�� 
(
�� 
$str�� �
)��� �
;��� �
sb
�� 
.
�� 
Append
�� 
(
�� 
$str�� �
)��� �
;��� �
sb
�� 
.
�� 
Append
�� 
(
�� 
$str�� �
)��� �
;��� �
sb
�� 
.
�� 
Append
�� 
(
�� 
$str�� �
)��� �
;��� �
sb
�� 
.
�� 
Append
�� 
(
�� 
$str�� �
)��� �
;��� �
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
;��� �
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
�� 
sb
�� 
.
�� 
ToString
�� 
(
�� 
)
��  
;
��  !
}
�� 	
	protected
�� 
UInt32
�� "
TotalDiscountedPrice
�� -
(
��- .
)
��. /
{
�� 	
UInt32
�� 

totalPrice
�� 
=
�� 
$num
��  !
;
��! "
if
�� 
(
�� 
DiscountList
�� 
!=
�� 
null
��  $
&&
��% '
DiscountList
��( 4
.
��4 5
Count
��5 :
>
��; <
$num
��= >
)
��> ?
{
�� 
foreach
�� 
(
�� 
var
�� 
priceListObj
�� )
in
��* ,
DiscountList
��- 9
)
��9 :
{
�� 

totalPrice
�� 
+=
�� !
priceListObj
��" .
.
��. /
Price
��/ 4
;
��4 5
}
�� 
}
�� 
return
�� 

totalPrice
�� 
;
�� 
}
�� 	
}
�� 
}
�� �7
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
double	z �
dealerLatitude
� �
,
� �
double
� �
dealerLongitude
� �
)
� �
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
$str	" �!
,
�! �!
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
$str	00* �
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
$str	44" �	
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
$str	::" �	
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
dealerLongitude	;;q �
)
;;� �
;
;;� �
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
$str	??& �
)
??� �
;
??� �
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
$str	BB* �
,
BB� �
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
$str	GG �
)
GG� �
;
GG� �
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
}ZZ �
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
$str	 �
)
� �
;
� �
message 
. 
AppendFormat $
($ %
$str% |
,| }
Bikewale	~ �
.
� �
Utility
� �
.
� � 
BWOprConfiguration
� �
.
� �
Instance
� �
.
� �
BwOprHostUrlForJs
� �
)
� �
;
� �!
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
$str	f �
)
� �
;
� �
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
}"" �
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
string	{ �
kms
� �
,
� �
string
� �
writeReviewLink
� �
)
� �
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
$str	..* �	
,
..�	 �	
DateTime
..�	 �

.
..�
 �

Now
..�
 �

.
..�
 �

ToString
..�
 �

(
..�
 �

$str
..�
 �

)
..�
 �

,
..�
 �

$str
..�
 �

)
..�
 �

)
..�
 �

;
..�
 �
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
$str	00 �
,
00� �
bikeName
00� �
,
00� �
	profileNo
00� �
)
00� �
;
00� �
sb11 
.11 
AppendFormat11 
(11 
$str	11 �
)
11� �
;
11� �
sb22 
.22 
AppendFormat22 
(22 
string22 "
.22" #
Format22# )
(22) *
$str	22* �
,
22� �
bikeName
22� �
,
22� �
kms
22� �
,
22� �
writeReviewLink
22� �
,
22� �

modelImage
22� �
)
22� �
)
22� �
;
22� �
return33 
sb33 
.33 
ToString33 
(33 
)33  
;33  !
}44 	
}55 
}66 �
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
string	| �
kms
� �
)
� �
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
$str	--* �	
,
--�	 �	
DateTime
--�	 �

.
--�
 �

Now
--�
 �

.
--�
 �

ToString
--�
 �

(
--�
 �

$str
--�
 �

)
--�
 �

,
--�
 �

$str
--�
 �

)
--�
 �

)
--�
 �

;
--�
 �
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
bikeName	//~ �
,
//� �
	profileNo
//� �
)
//� �
;
//� �
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
$str	11 �
)
11� �
;
11� �
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
$str	33* �
,
33� �
bikeName
33� �
,
33� �
kms
33� �
,
33� �
writeReviewLink
33� �
,
33� �

modelImage
33� �
)
33� �
)
33� �
;
33� �
return44 
sb44 
.44 
ToString44 
(44 
)44  
;44  !
}55 	
}66 
}77 �(
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
remainingDays	u �
,
� �
string
� �
	repostUrl
� �
,
� �
string
� �
emailSubject
� �
,
� �
string
� �
imgPath
� �
,
� �
string
� �
distance
� �
,
� �
string
� �
qEncoded
� �
,
� �
string
� �
	bwHostUrl
� �
,
� �
uint
� �
modelId
� �
)
� �
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
$str	// �
,
//� �
DateTime
//� �
.
//� �
Now
//� �
.
//� �
ToString
//� �
(
//� �
$str
//� �
)
//� �
,
//� �
_emailSubject
//� �
)
//� �
;
//� �
sb22 
.22 
AppendFormat22 
(22 
$str	22 �
,
22� �
_sellerName
22� �
,
22� �
	_makeName
22� �
,
22� �

_modelName
22� �
,
22� �
_remainingTime
22� �
,
22� �

_repostUrl
22� �
)
22� �
;
22� �
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
$str	44  �
)
44� �
;
44� �
sb55 
.55 
AppendFormat55 
(55 
$str	55 �
,
55� �
_imgPath
55� �
,
55� �
	_makeName
55� �
,
55� �

_modelName
55� �
,
55� �
	_distance
55� �
,
55� �

_bwHostUrl
55� �
,
55� �
_modelId
55� �
,
55� �
	_qEncoded
55� �
)
55� �
;
55� �
sb77 
.77 
AppendFormat77 
(77 
$str	77 �
)
77� �
;
77� �
sb88 
.88 
AppendFormat88 
(88 
$str	88 �	
)
88�	 �	
;
88�	 �	
return:: 
sb:: 
.:: 
ToString:: 
(:: 
)::  
;::  !
};; 	
}<< 
}== �(
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
owner	%%| �
,
%%� �
string
%%� �
distance
%%� �
,
%%� �
string
%%� �
city
%%� �
,
%%� �
string
%%� �
imgPath
%%� �
,
%%� �
int
%%� �
	inquiryId
%%� �
,
%%� �
string
%%� �
	bwHostUrl
%%� �
,
%%� �
uint
%%� �
modelId
%%� �
,
%%� �
string
%%� �
qEncoded
%%� �
)
%%� �
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
$str	?? �
,
??� �
DateTime
??� �
.
??� �
Now
??� �
.
??� �
ToString
??� �
(
??� �
$str
??� �
)
??� �
)
??� �
;
??� �
sbBB 
.BB 
AppendFormatBB 
(BB 
$str	BB �
,
BB� �

sellerName
BB� �
,
BB� �
bikeName
BB� �
,
BB� �
	profileNo
BB� �
)
BB� �
;
BB� �
sbCC 
.CC 
AppendFormatCC 
(CC 
$str	CC �
,
CC� �
bikeName
CC� �
,
CC� �
city
CC� �
,
CC� �
makeYear
CC� �
.
CC� �
Year
CC� �
,
CC� �
owner
CC� �
,
CC� �
distance
CC� �
,
CC� �
	inquiryId
CC� �
,
CC� �
	bwHostUrl
CC� �
)
CC� �
;
CC� �
sbDD 
.DD 
AppendFormatDD 
(DD 
$str	DD �
,
DD� �
imgPath
DD� �
,
DD� �
bikeName
DD� �
,
DD� �
distance
DD� �
,
DD� �
	bwHostUrl
DD� �
,
DD� �
modelId
DD� �
,
DD� �
qEncoded
DD� �
)
DD� �
;
DD� �
sbGG 
.GG 
AppendFormatGG 
(GG 
$str	GG �
)
GG� �
;
GG� �
sbHH 
.HH 
AppendFormatHH 
(HH 
$str	HH �	
)
HH�	 �	
;
HH�	 �	
returnKK 
sbKK 
.KK 
ToStringKK 
(KK 
)KK  
;KK  !
}LL 	
}MM 
}NN �!
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
$str	00* �

,
00�
 �

DateTime
00�
 �

.
00�
 �

Now
00�
 �

.
00�
 �

ToString
00�
 �

(
00�
 �

$str
00�
 �

)
00�
 �

)
00�
 �

)
00�
 �

;
00�
 �
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
$str	22 �
,
22� �
bikeName
22� �
,
22� �
	profileNo
22� �
)
22� �
;
22� �
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
$str	44 �
)
44� �
;
44� �
sb55 
.55 
AppendFormat55 
(55 
$str	55 �
)
55� �
;
55� �
sb66 
.66 
AppendFormat66 
(66 
string66 "
.66" #
Format66# )
(66) *
$str	66* �
,
66� �
bikeName
66� �
,
66� �
kms
66� �
,
66� �

reviewLink
66� �
,
66� �
modelImageUrl
66� �
)
66� �
)
66� �
;
66� �
return77 
sb77 
.77 
ToString77 
(77 
)77  
;77  !
}88 	
}99 
}:: �
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
$str	++ �
,
++� �
DateTime
++� �
.
++� �
Now
++� �
.
++� �
ToString
++� �
(
++� �
$str
++� �	
)
++�	 �	
)
++�	 �	
;
++�	 �	
sb-- 
.-- 
AppendFormat-- 
(-- 
$str	-- �
,
--� �

sellerName
--� �
,
--� �
bikeName
--� �
,
--� �
	profileNo
--� �
)
--� �
;
--� �
sb.. 
... 
AppendFormat.. 
(.. 
$str	.. �
)
..� �
;
..� �
sb00 
.00 
AppendFormat00 
(00 
$str	00 �
)
00� �
;
00� �
sb11 
.11 
AppendFormat11 
(11 
$str	11 �	
)
11�	 �	
;
11�	 �	
return22 
sb22 
.22 
ToString22 
(22 
)22  
;22  !
}33 	
}44 
}55 �
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
bikeName	x �
,
� �
string
� �
	profileId
� �
)
� �
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
$str	  �
,
� �
	buyerName
� �
,
� �
buyerContact
� �
,
� �
bikeName
� �
,
� �
	profileId
� �
)
� �
;
� �
sb 
. 
Append 
( 
$str	 �
)
� �
;
� �
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
}%% �
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
bikeName	| �
,
� �
string
� �

listingUrl
� �
)
� �
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
$str	  �
,
� �
	buyerName
� �
,
� �
buyerContact
� �
,
� �
bikeName
� �
)
� �
;
� �
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
$str	 �
)
� �
;
� �
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
}&& �
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
string	z �
.
� �
IsNullOrEmpty
� �
(
� �

_profileId
� �
)
� �
)
� �
{ 
sb 
. 
AppendFormat 
(  
$str	   �
,
  � �
_make
  � �
,
  � �
_model
  � �
,
  � �

_profileId
  � �
,
  � �
_customerName
  � �
,
  � �
Bikewale
  � �
.
  � �
Utility
  � �
.
  � �
BWConfiguration
  � �
.
  � �
Instance
  � �
.
  � �
	BwHostUrl
  � �
)
  � �
;
  � �
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
}'' �
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
string	z �
.
� �
IsNullOrEmpty
� �
(
� �

_profileId
� �
)
� �
)
� �
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
BWConfiguration	!!t �
.
!!� �
Instance
!!� �
.
!!� �
	BwHostUrl
!!� �
)
!!� �
;
!!� �
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
}&& �'
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
bikeYear	z �
,
� �
	bikePrice
� �
,
� �
	buyerName
� �
,
� �

listingUrl
� �
;
� �
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
bikeName	::| �
,
::� �
	profileNo
::� �
,
::� �

kilometers
::� �
,
::� �
	bikePrice
::� �
)
::� �
;
::� �
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
;	== �
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
}DD �'
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

buyerEmail	~ �
,
� �
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
$str	66 �
,
66� �
	buyerName
66� �
,
66� �
bikeName
66� �
,
66� �
	profileNo
66� �
,
66� �
	bikePrice
66� �
)
66� �
;
66� �
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
bikeName	??z �
)
??� �
;
??� �
sb@@ 
.@@ 
AppendFormat@@ 
(@@ 
$str	@@ �
)
@@� �
;
@@� �
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
}GG �
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
}(( �
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
$str	$$ �
)
$$� �
;
$$� �
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
}** �
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
)	(( �
;
((� �
sb)) 
.)) 
AppendFormat)) 
()) 
$str)) s
,))s t
	modelName))u ~
)))~ 
;	)) �
sb** 
.** 
AppendFormat** 
(** 
$str	** �
)
**� �
;
**� �
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
}00 �
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
	modelName	##} �
,
##� �

ratingDate
##� �
)
##� �
;
##� �
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
}.. �
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
$str	!! �
)
!!� �
;
!!� �
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
;	%% �
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
}++ �
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
.	// �
ToString
//� �
(
//� �
)
//� �
)
//� �
)
//� �
;
//� �

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
}ZZ �m
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
$str	227 �
)
22� �
;
22� �
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
)	SS �
;
SS� �
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
$str	]]7 �
)
]]� �
;
]]� �
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
)	uu �
)
uu� �
;
uu� �
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
$str	7 �
)
� �
;
� �
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
�� 
}
�� 	
internal
�� 
void
�� )
SaveCustomerPQEmailTemplate
�� 1
(
��1 2
uint
��2 6
pqId
��7 ;
,
��; <
string
��= C
	emailBody
��D M
,
��M N
string
��O U
emailSubject
��V b
,
��b c
string
��d j
customerEmail
��k x
)
��x y
{
�� 	
try
�� 
{
�� 
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
�� 
if
�� 
(
�� 
pqId
�� 
>
�� 
$num
��  
)
��  !
{
�� 
cmd
�� 
.
�� 
CommandText
�� '
=
��( )
$str
��* E
;
��E F
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
��X Y
pqId
��Z ^
)
��^ _
)
��_ `
;
��` a
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
��@ `
,
��` a
DbType
��b h
.
��h i
String
��i o
,
��o p
	emailBody
��q z
)
��z {
)
��{ |
;
��| }
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
��@ \
,
��\ ]
DbType
��^ d
.
��d e
String
��e k
,
��k l
$num
��m p
,
��p q
emailSubject
��r ~
)
��~ 
)�� �
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
��@ \
,
��\ ]
DbType
��^ d
.
��d e
String
��e k
,
��k l
$num
��m p
,
��p q
customerEmail
��r 
)�� �
)��� �
;��� �
MySqlDatabase
�� %
.
��% &
ExecuteNonQuery
��& 5
(
��5 6
cmd
��6 9
,
��9 :
ConnectionType
��; I
.
��I J
MasterDatabase
��J X
)
��X Y
;
��Y Z
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
��5 6
$str��7 �
)��� �
;��� �
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
�� 
}
�� 	
}
�� 
}�� �
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
]$$) *��
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
dealerEmail	$$~ �
,
$$� �
string
$$� �
customerName
$$� �
,
$$� �
string
$$� �
customerEmail
$$� �
,
$$� �
string
$$� �
customerMobile
$$� �
,
$$� �
string
$$� �
areaName
$$� �
,
$$� �
string
$$� �
cityName
$$� �
,
$$� �
List
$$� �
<
$$� �
PQ_Price
$$� �
>
$$� �
	priceList
$$� �
,
$$� �
int
$$� �

totalPrice
$$� �
,
$$� �
List
$$� �
<
$$� �
OfferEntity
$$� �
>
$$� �
	offerList
$$� �
,
$$� �
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
versionName	--v �
,
--� �

dealerName
--� �
,
--� �
customerName
--� �
,
--� �
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
customerEmail	00z �
)
00� �
;
00� �
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
LL� �
,
LL� �
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
OfferEntity	MM{ �
>
MM� �
	offerList
MM� �
,
MM� �
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
dealerMobileNo	QQ �
,
QQ� �
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
RR� �
,
RR� �
workingHours
RR� �
)
RR� �
;
RR� �
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
aa� �
,
aa� �
string
aa� �
cityName
aa� �
,
aa� �
string
aa� �

dealerArea
aa� �
)
aa� �
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
HttpContext	ddw �
.
dd� �
Current
dd� �
.
dd� �
Request
dd� �
.
dd� �
ServerVariables
dd� �
[
dd� �
$str
dd� �
]
dd� �
.
dd� �
ToString
dd� �
(
dd� �
)
dd� �
,
dd� �

dealerArea
dd� �
)
dd� �
;
dd� �
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
gg� �
,
gg� �
string
gg� �

dealerName
gg� �
,
gg� �
string
gg� �
dealerContactNo
gg� �
,
gg� �
string
gg� �
dealerAddress
gg� �
,
gg� �
uint
gg� �
bookingAmount
gg� �
,
gg� �
uint
gg� �
insuranceAmount
gg� �
=
gg� �
$num
gg� �
,
gg� �
bool
gg� �"
hasBumperDealerOffer
gg� �
=
gg� �
false
gg� �
)
gg� �
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
dealerContactNo	jjq �
,
jj� �
dealerAddress
jj� �
,
jj� �
HttpContext
jj� �
.
jj� �
Current
jj� �
.
jj� �
Request
jj� �
.
jj� �
ServerVariables
jj� �
[
jj� �
$str
jj� �
]
jj� �
.
jj� �
ToString
jj� �
(
jj� �
)
jj� �
,
jj� �
bookingAmount
jj� �
,
jj� �
insuranceAmount
jj� �
,
jj� �"
hasBumperDealerOffer
jj� �
)
jj� �
;
jj� �
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
mm� �
dealerContactNo
mm� �
,
mm� �
string
mm� �
dealerAddress
mm� �
,
mm� �
string
mm� �
bookingRefNum
mm� �
,
mm� �
uint
mm� �
insuranceAmount
mm� �
=
mm� �
$num
mm� �
)
mm� �
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
HttpContext	pp} �
.
pp� �
Current
pp� �
.
pp� �
Request
pp� �
.
pp� �
ServerVariables
pp� �
[
pp� �
$str
pp� �
]
pp� �
.
pp� �
ToString
pp� �
(
pp� �
)
pp� �
,
pp� �
bookingRefNum
pp� �
,
pp� �
insuranceAmount
pp� �
)
pp� �
;
pp� �
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
string	ss~ �
dealerContactNo
ss� �
,
ss� �
string
ss� �
dealerAddress
ss� �
,
ss� �
string
ss� �
bookingRefNum
ss� �
,
ss� �
UInt32
ss� �

bookingAmt
ss� �
,
ss� �
uint
ss� �
insuranceAmount
ss� �
=
ss� �
$num
ss� �
)
ss� �
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
.	vv �
Request
vv� �
.
vv� �
ServerVariables
vv� �
[
vv� �
$str
vv� �
]
vv� �
.
vv� �
ToString
vv� �
(
vv� �
)
vv� �
,
vv� �

bookingAmt
vv� �
,
vv� �
bookingRefNum
vv� �
,
vv� �
insuranceAmount
vv� �
)
vv� �
;
vv� �
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
string	}}z �
	profileId
}}� �
,
}}� �
string
}}� �
editUrl
}}� �
)
}}� �
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
�� 
.
�� &
SMSForPhotoUploadTwoDays
�� (
(
��( )
customerName
��) 5
,
��5 6
customerMobile
��7 E
,
��E F
make
��G K
,
��K L
model
��M R
,
��R S
	profileId
��T ]
,
��] ^
editUrl
��_ f
)
��f g
;
��g h
}
�� 	
public
�� 
static
�� 
void
�� $
BookingEmailToCustomer
�� 1
(
��1 2
string
��2 8
customerEmail
��9 F
,
��F G
string
��H N
customerName
��O [
,
��[ \
List
��] a
<
��a b
PQ_Price
��b j
>
��j k
	priceList
��l u
,
��u v
List
��w {
<
��{ |
OfferEntity��| �
>��� �
	offerList��� �
,��� �
string��� �"
bookingReferenceNo��� �
,��� �
uint��� �
totalAmount��� �
,��� �
uint��� � 
preBookingAmount��� �
,��� �
string��� �
makeModelName��� �
,��� �
string��� �
version��� �
,��� �
string��� �
color��� �
,��� �
string��� �
img��� �
,��� �
string��� �

dealerName��� �
,��� �
string��� �
dealerAddress��� �
,��� �
string��� �
dealerMobile��� �
,��� �
string��� �
dealerEmailId��� �
,��� �
string��� �!
dealerWorkingTime��� �
,��� �
double��� �
dealerLatitude��� �
,��� �
double��� �
dealerLongitude��� �
)��� �
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +.
 PreBookingConfirmationToCustomer
��, L
(
��L M
customerName
��M Y
,
��Y Z
	priceList
��[ d
,
��d e
	offerList
��f o
,
��o p!
bookingReferenceNo��q �
,��� �
totalAmount��� �
,��� � 
preBookingAmount��� �
,��� �
makeModelName��� �
,��� �
version��� �
,��� �
color��� �
,��� �
img��� �
,��� �

dealerName��� �
,��� �
dealerAddress��� �
,��� �
dealerMobile��� �
,��� �
dealerEmailId��� �
,��� �!
dealerWorkingTime��� �
,��� �
dealerLatitude��� �
,��� �
dealerLongitude��� �
)��� �
;��� �
objEmail
�� 
.
�� 
Send
�� 
(
�� 
customerEmail
�� '
,
��' (
$str
��) N
+
��O P
makeModelName
��Q ^
,
��^ _
$str
��` b
)
��b c
;
��c d
}
�� 	
public
�� 
static
�� 
void
�� "
BookingEmailToDealer
�� /
(
��/ 0
string
��0 6
dealerEmail
��7 B
,
��B C
string
��D J
customerName
��K W
,
��W X
string
��Y _
customerMobile
��` n
,
��n o
string
��p v
customerArea��w �
,��� �
string��� �
customerEmail��� �
,��� �
uint��� �

totalPrice��� �
,��� �
uint��� �
bookingAmount��� �
,��� �
uint��� �
balanceAmount��� �
,��� �
List��� �
<��� �
PQ_Price��� �
>��� �
	priceList��� �
,��� �
string��� �"
bookingReferenceNo��� �
,��� �
string��� �
bikeName��� �
,��� �
string��� �
	bikeColor��� �
,��� �
string��� �

dealerName��� �
,��� �
string��� �
	imagePath��� �
,��� �
List��� �
<��� �
OfferEntity��� �
>��� �
	offerList��� �
,��� �
string��� �
versionName��� �
)��� �
{
�� 	
if
�� 
(
�� 
!
�� 
String
�� 
.
�� 
IsNullOrEmpty
�� %
(
��% &
dealerEmail
��& 1
)
��1 2
)
��2 3
{
�� 
string
�� 
[
�� 
]
�� 
arrDealerEmail
�� '
=
��( )
dealerEmail
��* 5
.
��5 6
Split
��6 ;
(
��; <
$char
��< ?
)
��? @
;
��@ A
foreach
�� 
(
�� 
string
�� 
email
��  %
in
��& (
arrDealerEmail
��) 7
)
��7 8
{
�� 
ComposeEmailBase
�� $
objEmail
��% -
=
��. /
new
��0 30
"PreBookingConfirmationMailToDealer
��4 V
(
��V W
customerName
��W c
,
��c d
customerMobile
��e s
,
��s t
customerArea��u �
,��� �
customerEmail��� �
,��� �

totalPrice��� �
,��� �
bookingAmount��� �
,��� �
balanceAmount��� �
,��� �
	priceList��� �
,��� �"
bookingReferenceNo��� �
,��� �
bikeName��� �
,��� �
	bikeColor��� �
,��� �

dealerName��� �
,��� �
	offerList��� �
,��� �
	imagePath��� �
,��� �
versionName��� �
)��� �
;��� �
objEmail
�� 
.
�� 
Send
�� !
(
��! "
email
��" '
,
��' (
$str
��) ;
+
��< =
customerName
��> J
+
��K L
$str
��M Y
+
��Z [
bookingAmount
��\ i
+
��j k
$str
��l s
+
��t u
bikeName
��v ~
+�� �
$str��� �
+��� �
	bikeColor��� �
,��� �
$str��� �
)��� �
;��� �
}
�� 
}
�� 
}
�� 	
public
�� 
static
�� 
void
�� "
BookingEmailToDealer
�� /
(
��/ 0
string
��0 6
dealerEmail
��7 B
,
��B C
string
��D J
bikewaleEmail
��K X
,
��X Y
string
��Z `
customerName
��a m
,
��m n
string
��o u
customerMobile��v �
,��� �
string��� �
customerArea��� �
,��� �
string��� �
customerEmail��� �
,��� �
uint��� �

totalPrice��� �
,��� �
uint��� �
bookingAmount��� �
,��� �
uint��� �
balanceAmount��� �
,��� �
List��� �
<��� �
PQ_Price��� �
>��� �
	priceList��� �
,��� �
string��� �"
bookingReferenceNo��� �
,��� �
string��� �
bikeName��� �
,��� �
string��� �
	bikeColor��� �
,��� �
string��� �

dealerName��� �
,��� �
List��� �
<��� �
OfferEntity��� �
>��� �
	offerList��� �
,��� �
string��� �
	imagePath��� �
,��� �
string��� �
versionName��� �
,��� �
uint��� �
insuranceAmount��� �
=��� �
$num��� �
)��� �
{
�� 	
string
�� 
[
�� 
]
�� 
arrBikeWaleEmail
�� %
=
��& '
null
��( ,
;
��, -
if
�� 
(
�� 
!
�� 
String
�� 
.
�� 
IsNullOrEmpty
�� %
(
��% &
dealerEmail
��& 1
)
��1 2
)
��2 3
{
�� 
string
�� 
[
�� 
]
�� 
arrDealerEmail
�� '
=
��( )
dealerEmail
��* 5
.
��5 6
Split
��6 ;
(
��; <
$char
��< ?
)
��? @
;
��@ A
arrBikeWaleEmail
��  
=
��! "
bikewaleEmail
��# 0
.
��0 1
Split
��1 6
(
��6 7
$char
��7 :
)
��: ;
;
��; <
foreach
�� 
(
�� 
string
�� 
email
��  %
in
��& (
arrDealerEmail
��) 7
)
��7 8
{
�� 
ComposeEmailBase
�� $
objEmail
��% -
=
��. /
new
��0 30
"PreBookingConfirmationMailToDealer
��4 V
(
��V W
customerName
��W c
,
��c d
customerMobile
��e s
,
��s t
customerArea��u �
,��� �
customerEmail��� �
,��� �

totalPrice��� �
,��� �
bookingAmount��� �
,��� �
balanceAmount��� �
,��� �
	priceList��� �
,��� �"
bookingReferenceNo��� �
,��� �
bikeName��� �
,��� �
	bikeColor��� �
,��� �

dealerName��� �
,��� �
	offerList��� �
,��� �
	imagePath��� �
,��� �
versionName��� �
,��� �
insuranceAmount��� �
)��� �
;��� �
objEmail
�� 
.
�� 
Send
�� !
(
��! "
email
��" '
,
��' (
$str
��) ;
+
��< =
customerName
��> J
+
��K L
$str
��M Y
+
��Z [
bookingAmount
��\ i
+
��j k
$str
��l s
+
��t u
bikeName
��v ~
+�� �
$str��� �
+��� �
	bikeColor��� �
,��� �
$str��� �
,��� �
null��� �
,��� � 
arrBikeWaleEmail��� �
)��� �
;��� �
}
�� 
}
�� 
}
�� 	
public
�� 
static
�� 
void
�� !
SaveEmailToCustomer
�� .
(
��. /
uint
��/ 3
pqId
��4 8
,
��8 9
string
��: @
bikeName
��A I
,
��I J
string
��K Q
	bikeImage
��R [
,
��[ \
string
��] c

dealerName
��d n
,
��n o
string
��p v
dealerEmail��w �
,��� �
string��� �
dealerMobileNo��� �
,��� �
string��� �
organization��� �
,��� �
string��� �
address��� �
,��� �
string��� �
customerName��� �
,��� �
string��� �
customerEmail��� �
,��� �
List��� �
<��� �
PQ_Price��� �
>��� �
	priceList��� �
,��� �
List��� �
<��� �
OfferEntity��� �
>��� �
	offerList��� �
,��� �
string��� �
pinCode��� �
,��� �
string��� �
	stateName��� �
,��� �
string��� �
cityName��� �
,��� �
uint��� �

totalPrice��� �
,��� �
string
�� 
versionName
�� 
,
�� 
double
��  &
	dealerLat
��' 0
,
��0 1
double
��2 8

dealerLong
��9 C
,
��C D
string
��E K
workingHours
��L X
)
��X Y
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +1
#NewBikePriceQuoteToCustomerTemplate
��, O
(
��O P
bikeName
��P X
,
��X Y
versionName
��Z e
,
��e f
	bikeImage
��g p
,
��p q
dealerEmail
��r }
,
��} ~
dealerMobileNo�� �
,��� �
organization
�� 
,
�� 
address
�� %
,
��% &
customerName
��' 3
,
��3 4
	priceList
��5 >
,
��> ?
	offerList
��@ I
,
��I J
pinCode
��K R
,
��R S
	stateName
��T ]
,
��] ^
cityName
��_ g
,
��g h

totalPrice
��i s
,
��s t
	dealerLat
��u ~
,
��~ 

dealerLong��� �
,��� �
workingHours��� �
)��� �
;��� �
string
�� 
	emailBody
�� 
=
�� 
objEmail
�� '
.
��' (
ComposeBody
��( 3
(
��3 4
)
��4 5
;
��5 6 
SavePQNotification
�� 
obj
�� "
=
��# $
new
��% ( 
SavePQNotification
��) ;
(
��; <
)
��< =
;
��= >
obj
�� 
.
�� )
SaveCustomerPQEmailTemplate
�� +
(
��+ ,
pqId
��, 0
,
��0 1
	emailBody
��2 ;
,
��; <
$str
��= _
+
��` a
bikeName
��b j
,
��j k
dealerEmail
��l w
.
��w x
Split
��x }
(
��} ~
$char��~ �
)��� �
[��� �
$num��� �
]��� �
)��� �
;��� �
}
�� 	
public
�� 
static
�� 
void
�� 
SaveSMSToDealer
�� *
(
��* +
uint
��+ /
pqId
��0 4
,
��4 5
string
��6 <
dealerMobile
��= I
,
��I J
string
��K Q
customerName
��R ^
,
��^ _
string
��` f
customerMobile
��g u
,
��u v
string
��w }
bikeName��~ �
,��� �
string��� �
areaName��� �
,��� �
string��� �
cityName��� �
,��� �
string��� �

dealerArea��� �
)��� �
{
�� 	
Bikewale
�� 
.
�� 
Notifications
�� "
.
��" #
SMSTypes
��# +
obj
��, /
=
��0 1
new
��2 5
Bikewale
��6 >
.
��> ?
Notifications
��? L
.
��L M
SMSTypes
��M U
(
��U V
)
��V W
;
��W X
obj
�� 
.
�� .
 SaveNewBikePriceQuoteSMSToDealer
�� 0
(
��0 1
pqId
��1 5
,
��5 6
dealerMobile
��7 C
,
��C D
customerName
��E Q
,
��Q R
customerMobile
��S a
,
��a b
bikeName
��c k
,
��k l
areaName
��m u
,
��u v
cityName
��w 
,�� �
HttpContext��� �
.��� �
Current��� �
.��� �
Request��� �
.��� �
ServerVariables��� �
[��� �
$str��� �
]��� �
.��� �
ToString��� �
(��� �
)��� �
,��� �

dealerArea��� �
)��� �
;��� �
}
�� 	
public
�� 
static
�� 
void
�� 
SaveSMSToCustomer
�� ,
(
��, -
uint
��- 1
pqId
��2 6
,
��6 7#
PQ_DealerDetailEntity
��8 M
dealerEntity
��N Z
,
��Z [
string
��\ b
customerMobile
��c q
,
��q r
string
��s y
customerName��z �
,��� �
string��� �
BikeName��� �
,��� �
string��� �

dealerName��� �
,��� �
string��� �
dealerContactNo��� �
,��� �
string��� �
dealerAddress��� �
,��� �
uint��� �
bookingAmount��� �
,��� �
uint��� �
insuranceAmount��� �
=��� �
$num��� �
,��� �
bool��� �$
hasBumperDealerOffer��� �
=��� �
false��� �
)��� �
{
�� 	
Bikewale
�� 
.
�� 
Notifications
�� "
.
��" #
SMSTypes
��# +
obj
��, /
=
��0 1
new
��2 5
Bikewale
��6 >
.
��> ?
Notifications
��? L
.
��L M
SMSTypes
��M U
(
��U V
)
��V W
;
��W X
obj
�� 
.
�� 0
"SaveNewBikePriceQuoteSMSToCustomer
�� 2
(
��2 3
pqId
��3 7
,
��7 8
dealerEntity
��9 E
,
��E F
customerMobile
��G U
,
��U V
customerName
��W c
,
��c d
BikeName
��e m
,
��m n

dealerName
��o y
,
��y z
dealerContactNo��{ �
,��� �
dealerAddress��� �
,��� �
HttpContext��� �
.��� �
Current��� �
.��� �
Request��� �
.��� �
ServerVariables��� �
[��� �
$str��� �
]��� �
.��� �
ToString��� �
(��� �
)��� �
,��� �
bookingAmount��� �
,��� �
insuranceAmount��� �
,��� �$
hasBumperDealerOffer��� �
)��� �
;��� �
}
�� 	
public
�� 
static
�� 
void
�� 
SendSMSToCustomer
�� ,
(
��, -
uint
��- 1
pqId
��2 6
,
��6 7
string
��8 >

requestUrl
��? I
,
��I J
DPQSmsEntity
��K W
objDPQSmsEntity
��X g
,
��g h
DPQTypes
��i q
DPQType
��r y
)
��y z
{
�� 	
string
�� 
message
�� 
=
�� 
String
�� #
.
��# $
Empty
��$ )
;
��) *
try
�� 
{
�� 
SMSTypes
�� 
obj
�� 
=
�� 
new
�� "
SMSTypes
��# +
(
��+ ,
)
��, -
;
��- .
switch
�� 
(
�� 
DPQType
�� 
)
��  
{
�� 
case
�� 
DPQTypes
�� !
.
��! "
NoOfferNoBooking
��" 2
:
��2 3
message
�� 
=
��  !
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str��0 �
,��� �
objDPQSmsEntity��� �
.��� �

DealerName��� �
,��� �
objDPQSmsEntity��� �
.��� �
Locality��� �
,��� �
objDPQSmsEntity��� �
.��� �
DealerMobile��� �
,��� �
objDPQSmsEntity��� �
.��� �#
LandingPageShortUrl��� �
)��� �
;��� �
break
�� 
;
�� 
case
�� 
DPQTypes
�� !
.
��! ""
NoOfferOnlineBooking
��" 6
:
��6 7
message
�� 
=
��  !
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str��0 �
,��� �
objDPQSmsEntity��� �
.��� �
BikeName��� �
,��� �
objDPQSmsEntity��� �
.��� �
BookingAmount��� �
,��� �
objDPQSmsEntity��� �
.��� �#
LandingPageShortUrl��� �
)��� �
;��� �
break
�� 
;
�� 
case
�� 
DPQTypes
�� !
.
��! "
OfferNoBooking
��" 0
:
��0 1
message
�� 
=
��  !
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str��0 �
,��� �
objDPQSmsEntity��� �
.��� �
BikeName��� �
,��� �
objDPQSmsEntity��� �
.��� �

DealerName��� �
,��� �
objDPQSmsEntity��� �
.��� �
Locality��� �
,��� �
objDPQSmsEntity��� �
.��� �#
LandingPageShortUrl��� �
)��� �
;��� �
break
�� 
;
�� 
case
�� 
DPQTypes
�� !
.
��! "
OfferAndBooking
��" 1
:
��1 2
message
�� 
=
��  !
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str��0 �
,��� �
objDPQSmsEntity��� �
.��� �
BikeName��� �
,��� �
objDPQSmsEntity��� �
.��� �#
LandingPageShortUrl��� �
)��� �
;��� �
break
�� 
;
�� 
case
�� 
DPQTypes
�� !
.
��! "(
AndroidAppNoOfferNoBooking
��" <
:
��< =
case
�� 
DPQTypes
�� !
.
��! "&
AndroidAppOfferNoBooking
��" :
:
��: ;
message
�� 
=
��  !
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str��0 �
,��� �
objDPQSmsEntity��� �
.��� �
CustomerName��� �
,��� �
objDPQSmsEntity��� �
.��� �

DealerName��� �
,��� �
objDPQSmsEntity��� �
.��� �
Locality��� �
,��� �
objDPQSmsEntity��� �
.��� �
DealerMobile��� �
)��� �
;��� �
break
�� 
;
�� 
case
�� 
DPQTypes
�� !
.
��! "
SubscriptionModel
��" 3
:
��3 4
message
�� 
=
��  !
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str
��0 v
,
��v w
objDPQSmsEntity��x �
.��� � 
OrganisationName��� �
,��� �
objDPQSmsEntity��� �
.��� �
DealerMobile��� �
,��� �
objDPQSmsEntity��� �
.��� �
	DealerAdd��� �
,��� �
objDPQSmsEntity��� �
.��� �

DealerArea��� �
,��� �
objDPQSmsEntity��� �
.��� �

DealerCity��� �
)��� �
;��� �
break
�� 
;
�� 
}
�� 
if
�� 
(
�� 
objDPQSmsEntity
�� #
!=
��$ &
null
��' +
&&
��, .
!
��/ 0
String
��0 6
.
��6 7
IsNullOrEmpty
��7 D
(
��D E
objDPQSmsEntity
��E T
.
��T U
CustomerMobile
��U c
)
��c d
&&
��e g
!
��h i
String
��i o
.
��o p
IsNullOrEmpty
��p }
(
��} ~
message��~ �
)��� �
&&��� �
pqId��� �
>��� �
$num��� �
)��� �
{
�� 
	SMSCommon
�� 
sc
��  
=
��! "
new
��# &
	SMSCommon
��' 0
(
��0 1
)
��1 2
;
��2 3
sc
�� 
.
�� 

ProcessSMS
�� !
(
��! "
objDPQSmsEntity
��" 1
.
��1 2
CustomerMobile
��2 @
,
��@ A
message
��B I
,
��I J 
EnumSMSServiceType
��K ]
.
��] ^,
NewBikePriceQuoteSMSToCustomer
��^ |
,
��| }

requestUrl��~ �
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
��5 6
$str
��7 ~
)
��~ 
;�� �
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
static
�� 
void
�� /
!UsedBikePhotoRequestEmailToDealer
�� <
(
��< =
string
��= C
dealerEmail
��D O
,
��O P
string
��Q W

sellerName
��X b
,
��b c
string
��d j
	buyerName
��k t
,
��t u
string
��v |
buyerContact��} �
,��� �
string��� �
bikeName��� �
,��� �
string��� �
	profileId��� �
)��� �
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +5
'PhotoRequestEmailToDealerSellerTemplate
��, S
(
��S T

sellerName
��T ^
,
��^ _
	buyerName
��` i
,
��i j
buyerContact
��k w
,
��w x
bikeName��y �
,��� �
	profileId��� �
)��� �
;��� �
objEmail
�� 
.
�� 
Send
�� 
(
�� 
dealerEmail
�� %
,
��% &
$str
��' ;
)
��; <
;
��< =
}
�� 	
public
�� 
static
�� 
void
�� 3
%UsedBikePhotoRequestEmailForThreeDays
�� @
(
��@ A
string
��A G
CustomerEmail
��H U
,
��U V
string
��W ]
CustomerName
��^ j
,
��j k
string
��l r
Make
��s w
,
��w x
string
��y 
Model��� �
,��� �
string��� �
	profileId��� �
)��� �
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +0
"PhotoRequestToCustomerForThreeDays
��, N
(
��N O
CustomerName
��O [
,
��[ \
Make
��] a
,
��a b
Model
��c h
,
��h i
	profileId
��j s
)
��s t
;
��t u
objEmail
�� 
.
�� 
Send
�� 
(
�� 
CustomerEmail
�� '
,
��' (
$str
��) X
)
��X Y
;
��Y Z
}
�� 	
public
�� 
static
�� 
void
�� 3
%UsedBikePhotoRequestEmailForSevenDays
�� @
(
��@ A
string
��A G
CustomerEmail
��H U
,
��U V
string
��W ]
CustomerName
��^ j
,
��j k
string
��l r
Make
��s w
,
��w x
string
��y 
Model��� �
,��� �
string��� �
	profileId��� �
)��� �
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +0
"PhotoRequestToCustomerForSevenDays
��, N
(
��N O
CustomerName
��O [
,
��[ \
Make
��] a
,
��a b
Model
��c h
,
��h i
	profileId
��j s
)
��s t
;
��t u
objEmail
�� 
.
�� 
Send
�� 
(
�� 
CustomerEmail
�� '
,
��' (
$str
��) X
)
��X Y
;
��Y Z
}
�� 	
public
�� 
static
�� 
void
�� 3
%UsedBikePhotoRequestEmailToIndividual
�� @
(
��@ A 
CustomerEntityBase
��A S
seller
��T Z
,
��Z [ 
CustomerEntityBase
��\ n
buyer
��o t
,
��t u
string
��v |
bikeName��} �
,��� �
string��� �

listingUrl��� �
)��� �
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +9
+PhotoRequestEmailToIndividualSellerTemplate
��, W
(
��W X
seller
��X ^
.
��^ _
CustomerName
��_ k
,
��k l
buyer
��m r
.
��r s
CustomerName
��s 
,�� �
buyer��� �
.��� �
CustomerMobile��� �
,��� �
bikeName��� �
,��� �

listingUrl��� �
)��� �
;��� �
objEmail
�� 
.
�� 
Send
�� 
(
�� 
seller
��  
.
��  !
CustomerEmail
��! .
,
��. /
$str
��0 D
)
��D E
;
��E F
}
�� 	
public
�� 
static
�� 
void
�� 1
#UsedBikePurchaseInquiryEmailToBuyer
�� >
(
��> ? 
CustomerEntityBase
��? Q
seller
��R X
,
��X Y 
CustomerEntityBase
��Z l
buyer
��m r
,
��r s
string
��t z
sellerAddress��{ �
,��� �
string��� �
	profileId��� �
,��� �
string��� �
bikeName��� �
,��� �
string��� �

kilometers��� �
,��� �
string��� �
makeYear��� �
,��� �
string��� �
formattedPrice��� �
,��� �
string��� �

listingUrl��� �
)��� �
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +1
#PurchaseInquiryEmailToBuyerTemplate
��, O
(
��O P
seller
�� 
.
�� 
CustomerEmail
�� $
,
��$ %
seller
�� 
.
�� 
CustomerName
�� #
,
��# $
seller
�� 
.
�� 
CustomerMobile
�� %
,
��% &
sellerAddress
�� 
,
�� 
	profileId
�� 
,
�� 
buyer
�� 
.
�� 

CustomerId
��  
.
��  !
ToString
��! )
(
��) *
)
��* +
,
��+ ,
bikeName
�� 
,
�� 

kilometers
�� 
,
�� 
makeYear
�� 
,
�� 
formattedPrice
�� 
,
�� 
buyer
�� 
.
�� 
CustomerName
�� "
,
�� 

listingUrl
�� 
)
�� 
;
�� 
objEmail
�� 
.
�� 
Send
�� 
(
�� 
buyer
�� 
.
��  
CustomerEmail
��  -
,
��- .
String
��/ 5
.
��5 6
Format
��6 <
(
��< =
$str
��= Z
,
��Z [
	profileId
��\ e
)
��e f
)
��f g
;
��g h
}
�� 	
public
�� 
static
�� 
void
�� 6
(UsedBikePurchaseInquiryEmailToIndividual
�� C
(
��C D 
CustomerEntityBase
��D V
seller
��W ]
,
��] ^ 
CustomerEntityBase
��_ q
buyer
��r w
,
��w x
string
��y 
	profileId��� �
,��� �
string��� �
bikeName��� �
,��� �
string��� �
formattedPrice��� �
)��� �
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +<
.PurchaseInquiryEmailToIndividualSellerTemplate
��, Z
(
��Z [
seller
��[ a
.
��a b
CustomerEmail
��b o
,
��o p
seller
��q w
.
��w x
CustomerName��x �
,��� �
buyer��� �
.��� �
CustomerName��� �
,��� �
buyer��� �
.��� �
CustomerEmail��� �
,��� �
buyer��� �
.��� �
CustomerMobile��� �
,��� �
	profileId��� �
,��� �
bikeName��� �
,��� �
formattedPrice��� �
)��� �
;��� �
objEmail
�� 
.
�� 
Send
�� 
(
�� 
seller
��  
.
��  !
CustomerEmail
��! .
,
��. /
$str
��0 U
)
��U V
;
��V W
}
�� 	
public
�� 
static
�� 
void
�� /
!UsedBikeApprovalEmailToIndividual
�� <
(
��< = 
CustomerEntityBase
��= O
seller
��P V
,
��V W
string
��X ^
	profileId
��_ h
,
��h i
string
��j p
bikeName
��q y
,
��y z
DateTime��{ �
makeYear��� �
,��� �
string��� �
owner��� �
,��� �
string��� �
distance��� �
,��� �
string��� �
city��� �
,��� �
string��� �
imgPath��� �
,��� �
int��� �
	inquiryId��� �
,��� �
string��� �
	bwHostUrl��� �
,��� �
uint��� �
modelId��� �
,��� �
string��� �
qEncoded��� �
)��� �
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +*
ListingApprovalEmailToSeller
��, H
(
��H I
seller
��I O
.
��O P
CustomerName
��P \
,
��\ ]
	profileId
��^ g
,
��g h
bikeName
��i q
,
��q r
makeYear
��r z
,
��z {
owner��{ �
,��� �
distance��� �
,��� �
city��� �
,��� �
imgPath��� �
,��� �
	inquiryId��� �
,��� �
	bwHostUrl��� �
,��� �
modelId��� �
,��� �
qEncoded��� �
)��� �
;��� �
objEmail
�� 
.
�� 
Send
�� 
(
�� 
seller
��  
.
��  !
CustomerEmail
��! .
,
��. /
String
��0 6
.
��6 7
Format
��7 =
(
��= >
$str
��> t
,
��t u
bikeName
��v ~
)
��~ 
)�� �
;��� �
}
�� 	
public
�� 
static
�� 
void
�� ,
UsedBikeRejectionEmailToSeller
�� 9
(
��9 : 
CustomerEntityBase
��: L
seller
��M S
,
��S T
string
��U [
	profileId
��\ e
,
��e f
string
��g m
bikeName
��n v
)
��v w
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( ++
ListingRejectionEmailToSeller
��, I
(
��I J
seller
��J P
.
��P Q
CustomerName
��Q ]
,
��] ^
	profileId
��_ h
,
��h i
bikeName
��j r
)
��r s
;
��s t
objEmail
�� 
.
�� 
Send
�� 
(
�� 
seller
��  
.
��  !
CustomerEmail
��! .
,
��. /
String
��0 6
.
��6 7
Format
��7 =
(
��= >
$str
��> r
,
��r s
bikeName
��t |
)
��| }
)
��} ~
;
��~ 
}
�� 	
public
�� 
static
�� 
void
�� 1
#UsedBikeEditedApprovalEmailToSeller
�� >
(
��> ? 
CustomerEntityBase
��? Q
seller
��R X
,
��X Y
string
��Z `
	profileId
��a j
,
��j k
string
��l r
bikeName
��s {
,
��{ |
string��} �

modelImage��� �
,��� �
string��� �
kms��� �
,��� �
string��� �
writeReviewLink��� �
)��� �
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +0
"EditedListingApprovalEmailToSeller
��, N
(
��N O
seller
��O U
.
��U V
CustomerName
��V b
,
��b c
	profileId
��d m
,
��m n
bikeName
��o w
,
��w x

modelImage��y �
,��� �
kms��� �
,��� �
writeReviewLink��� �
)��� �
;��� �
objEmail
�� 
.
�� 
Send
�� 
(
�� 
seller
��  
.
��  !
CustomerEmail
��! .
,
��. /
String
��0 6
.
��6 7
Format
��7 =
(
��= >
$str��> �
,��� �
bikeName��� �
)��� �
)��� �
;��� �
}
�� 	
public
�� 
static
�� 
void
�� '
CustomerRegistrationEmail
�� 4
(
��4 5
string
��5 ;
customerEmail
��< I
,
��I J
string
��K Q
customerName
��R ^
,
��^ _
string
��` f
password
��g o
)
��o p
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +.
 CustomerRegistrationMailTemplate
��, L
(
��L M
customerEmail
��M Z
,
��Z [
customerName
��\ h
,
��h i
password
��j r
)
��r s
;
��s t
objEmail
�� 
.
�� 
Send
�� 
(
�� 
customerEmail
�� '
,
��' (
$str
��) A
)
��A B
;
��B C
}
�� 	
public
�� 
static
�� 
void
�� 2
$UsedBikeEditedRejectionEmailToSeller
�� ?
(
��? @ 
CustomerEntityBase
��@ R
seller
��S Y
,
��Y Z
string
��[ a
	profileId
��b k
,
��k l
string
��m s
bikeName
��t |
,
��| }
string��~ �

modelImage��� �
,��� �
string��� �
kms��� �
)��� �
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +1
#EditedListingRejectionEmailToSeller
��, O
(
��O P
seller
��P V
.
��V W
CustomerName
��W c
,
��c d
	profileId
��e n
,
��n o
bikeName
��p x
,
��x y

modelImage��z �
,��� �
kms��� �
)��� �
;��� �
objEmail
�� 
.
�� 
Send
�� 
(
�� 
seller
��  
.
��  !
CustomerEmail
��! .
,
��. /
String
��0 6
.
��6 7
Format
��7 =
(
��= >
$str��> �
,��� �
bikeName��� �
)��� �
)��� �
;��� �
}
�� 	
public
�� 
static
�� 
void
�� )
UsedBikeAdEmailToIndividual
�� 6
(
��6 7 
CustomerEntityBase
��7 I
seller
��J P
,
��P Q
string
��R X
	profileId
��Y b
,
��b c
string
��d j
bikeName
��k s
,
��s t
string
��u {
formattedPrice��| �
,��� �
string��� �
modelImageUrl��� �
,��� �
string��� �
kms��� �
,��� �
string��� �

reviewLink��� �
)��� �
{
�� 	
ComposeEmailBase
�� 
objEmail
�� %
=
��& '
new
��( +.
 ListingEmailtoIndividualTemplate
��, L
(
��L M
seller
��M S
.
��S T
CustomerEmail
��T a
,
��a b
seller
��c i
.
��i j
CustomerName
��j v
,
��v w
	profileId��x �
,��� �
bikeName��� �
,��� �
formattedPrice��� �
,��� �
modelImageUrl��� �
,��� �
kms��� �
,��� �

reviewLink��� �
)��� �
;��� �
objEmail
�� 
.
�� 
Send
�� 
(
�� 
seller
��  
.
��  !
CustomerEmail
��! .
,
��. /
String
��0 6
.
��6 7
Format
��7 =
(
��= >
$str
��> w
,
��w x
bikeName��y �
)��� �
)��� �
;��� �
}
�� 	
}
�� 
}�� �B
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
)	pp �
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
�� 
MailAddress
�� 
addCC
��  %
=
��& '
null
��( ,
;
��, -
for
�� 
(
�� 
int
�� 
iTmp
�� !
=
��" #
$num
��$ %
;
��% &
iTmp
��' +
<
��, -
cc
��. 0
.
��0 1
Length
��1 7
;
��7 8
iTmp
��9 =
++
��= ?
)
��? @
{
�� 
addCC
�� 
=
�� 
new
��  #
MailAddress
��$ /
(
��/ 0
cc
��0 2
[
��2 3
iTmp
��3 7
]
��7 8
)
��8 9
;
��9 :
msg
�� 
.
�� 
CC
�� 
.
�� 
Add
�� "
(
��" #
addCC
��# (
)
��( )
;
��) *
}
�� 
}
�� 
if
�� 
(
�� 
bcc
�� 
!=
�� 
null
�� 
&&
��  "
bcc
��# &
.
��& '
Length
��' -
>
��. /
$num
��0 1
)
��1 2
{
�� 
MailAddress
�� 
addBCC
��  &
=
��' (
null
��) -
;
��- .
for
�� 
(
�� 
int
�� 
iTmp
�� !
=
��" #
$num
��$ %
;
��% &
iTmp
��' +
<
��, -
bcc
��. 1
.
��1 2
Length
��2 8
;
��8 9
iTmp
��: >
++
��> @
)
��@ A
{
�� 
addBCC
�� 
=
��  
new
��! $
MailAddress
��% 0
(
��0 1
bcc
��1 4
[
��4 5
iTmp
��5 9
]
��9 :
)
��: ;
;
��; <
msg
�� 
.
�� 
Bcc
�� 
.
��  
Add
��  #
(
��# $
addBCC
��$ *
)
��* +
;
��+ ,
}
�� 
}
�� 
msg
�� 
.
�� 

IsBodyHtml
�� 
=
��  
true
��! %
;
��% &
msg
�� 
.
�� 
Priority
�� 
=
�� 
MailPriority
�� +
.
��+ ,
High
��, 0
;
��0 1
msg
�� 
.
�� 
Subject
�� 
=
�� 
subject
�� %
;
��% &
msg
�� 
.
�� 
Body
�� 
=
�� 
body
�� 
;
��  
client
�� 
.
�� 
Send
�� 
(
�� 
msg
�� 
)
��  
;
��  !
}
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
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
��2 3
err
��3 6
,
��6 7
string
��8 >
.
��> ?
Format
��? E
(
��E F
$str
��F z
,
��z {
email��| �
)��� �
)��� �
;��� �
}
�� 
}
�� 	
}
�� 
}�� �
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
�� 
void
��  
ProcessPrioritySMS
�� &
(
��& '
string
��' -
number
��. 4
,
��4 5
string
��6 <
message
��= D
,
��D E 
EnumSMSServiceType
��F X
esms
��Y ]
,
��] ^
string
��_ e
pageUrl
��f m
,
��m n
bool
��o s
isDND
��t y
)
��y z
{
�� 	
string
�� 
mobile
�� 
=
�� 
ParseMobileNumber
�� -
(
��- .
number
��. 4
)
��4 5
;
��5 6
if
�� 
(
�� 
!
�� 
String
�� 
.
�� 
IsNullOrEmpty
�� %
(
��% &
mobile
��& ,
)
��, -
)
��- .
{
�� 
if
�� 
(
�� 
isDND
�� 
==
�� 
true
�� !
)
��! "
mobile
�� 
=
�� 
$str
�� !
+
��" #
mobile
��$ *
;
��* +
string
�� 
retMsg
�� 
=
�� 
string
��  &
.
��& '
Empty
��' ,
;
��, -
string
�� 
ctId
�� 
=
�� 
$str
�� "
;
��" #
bool
�� 
status
�� 
=
�� 
false
�� #
;
��# $
bool
�� 
isMSMQ
�� 
=
�� 
false
�� #
;
��# $
if
�� 
(
�� 
!
�� 
String
�� 
.
�� 
IsNullOrEmpty
�� )
(
��) *
BWConfiguration
��* 9
.
��9 :
Instance
��: B
.
��B C
IsMSMQ
��C I
)
��I J
)
��J K
{
�� 
isMSMQ
�� 
=
�� 
Convert
�� $
.
��$ %
	ToBoolean
��% .
(
��. /
BWConfiguration
��/ >
.
��> ?
Instance
��? G
.
��G H
IsMSMQ
��H N
)
��N O
;
��O P
}
�� 
ctId
�� 
=
�� 
SaveSMSSentData
�� &
(
��& '
mobile
��' -
,
��- .
message
��/ 6
,
��6 7
esms
��8 <
,
��< =
status
��> D
,
��D E
retMsg
��F L
,
��L M
pageUrl
��N U
)
��U V
;
��V W!
NameValueCollection
�� #
nvc
��$ '
=
��( )
new
��* -!
NameValueCollection
��. A
(
��A B
)
��B C
;
��C D
nvc
�� 
.
�� 
Add
�� 
(
�� 
$str
�� 
,
�� 
ctId
�� "
)
��" #
;
��# $
nvc
�� 
.
�� 
Add
�� 
(
�� 
$str
�� !
,
��! "
message
��# *
)
��* +
;
��+ ,
nvc
�� 
.
�� 
Add
�� 
(
�� 
$str
�� "
,
��" #
mobile
��$ *
)
��* +
;
��+ ,
nvc
�� 
.
�� 
Add
�� 
(
�� 
$str
��  
,
��  !
$str
��" &
)
��& '
;
��' (
nvc
�� 
.
�� 
Add
�� 
(
�� 
$str
�� "
,
��" #
$str
��$ &
)
��& '
;
��' (
RabbitMqPublish
�� 
publish
��  '
=
��( )
new
��* -
RabbitMqPublish
��. =
(
��= >
)
��> ?
;
��? @
publish
�� 
.
�� 
PublishToQueue
�� &
(
��& '
BWConfiguration
��' 6
.
��6 7
Instance
��7 ?
.
��? @ 
BWPrioritySmsQueue
��@ R
,
��R S
nvc
��T W
)
��W X
;
��X Y
}
�� 
}
�� 	
private
�� 
void
�� 
UpdateSMSSentData
�� &
(
��& '
string
��' -
	currentId
��. 7
,
��7 8
string
��9 ?
retMsg
��@ F
)
��F G
{
�� 	
if
�� 
(
�� 
!
�� 
String
�� 
.
�� 
IsNullOrEmpty
�� %
(
��% &
	currentId
��& /
)
��/ 0
)
��0 1
{
�� 
string
�� 
sql
�� 
=
�� 
$str
�� ]
;
��] ^
try
�� 
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
��A B
sql
��B E
)
��E F
)
��F G
{
�� 
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
Convert
��\ c
.
��c d
ToInt32
��d k
(
��k l
	currentId
��l u
)
��u v
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
��@ I
,
��I J
DbType
��K Q
.
��Q R
String
��R X
,
��X Y
retMsg
��Z `
)
��` a
)
��a b
;
��b c
cmd
�� 
.
�� 
ExecuteNonQuery
�� +
(
��+ ,
)
��, -
;
��- .
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
��  
err
��! $
)
��$ %
{
�� 

ErrorClass
�� 
objErr
�� %
=
��& '
new
��( +

ErrorClass
��, 6
(
��6 7
err
��7 :
,
��: ;
$str
��< ^
)
��^ _
;
��_ `
objErr
�� 
.
�� 
SendMail
�� #
(
��# $
)
��$ %
;
��% &
}
�� 
}
�� 
}
�� 	
string
�� 
SaveSMSSentData
�� 
(
�� 
string
�� %
number
��& ,
,
��, -
string
��. 4
message
��5 <
,
��< = 
EnumSMSServiceType
��> P
esms
��Q U
,
��U V
bool
��W [
status
��\ b
,
��b c
string
��d j
retMsg
��k q
,
��q r
string
��s y
pageUrl��z �
)��� �
{
�� 	
string
�� 
	currentId
�� 
=
�� 
string
�� %
.
��% &
Empty
��& +
;
��+ ,
try
�� 
{
�� 
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
��= >
$str
��> M
)
��M N
)
��N O
{
�� 
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
��< H
,
��H I
DbType
��J P
.
��P Q
String
��Q W
,
��W X
$num
��Y [
,
��[ \
number
��] c
)
��c d
)
��d e
;
��e f
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
String
��R X
,
��X Y
$num
��Z ]
,
��] ^
message
��_ f
)
��f g
)
��g h
;
��h i
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
��< M
,
��M N
DbType
��O U
.
��U V
Int32
��V [
,
��[ \
esms
��] a
)
��a b
)
��b c
;
��c d
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
��< Q
,
��Q R
DbType
��S Y
.
��Y Z
DateTime
��Z b
,
��b c
DateTime
��d l
.
��l m
Now
��m p
)
��p q
)
��q r
;
��r s
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
��< M
,
��M N
DbType
��O U
.
��U V
Boolean
��V ]
,
��] ^
status
��_ e
)
��e f
)
��f g
;
��g h
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
��< M
,
��M N
DbType
��O U
.
��U V
String
��V \
,
��\ ]
$num
��^ a
,
��a b
retMsg
��c i
)
��i j
)
��j k
;
��k l
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
��< L
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
��] `
,
��` a
pageUrl
��b i
)
��i j
)
��j k
;
��k l
	currentId
�� 
=
�� 
Convert
��  '
.
��' (
ToString
��( 0
(
��0 1
MySqlDatabase
��1 >
.
��> ?
ExecuteScalar
��? L
(
��L M
cmd
��M P
,
��P Q
ConnectionType
��R `
.
��` a
MasterDatabase
��a o
)
��o p
)
��p q
;
��q r
}
�� 
}
�� 
catch
�� 
(
�� 
SqlException
�� 
err
��  #
)
��# $
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
��2 3
err
��3 6
,
��6 7
$str
��8 Z
)
��Z [
;
��[ \
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
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
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
��2 3
err
��3 6
,
��6 7
$str
��8 Z
)
��Z [
;
��[ \
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
	currentId
�� 
;
�� 
}
�� 	
string
�� 
ParseMobileNumber
��  
(
��  !
string
��! '
input
��( -
)
��- .
{
�� 	
return
�� 
Bikewale
�� 
.
�� 
Utility
�� #
.
��# $
CommonValidators
��$ 4
.
��4 5
ParseMobileNumber
��5 F
(
��F G
input
��G L
)
��L M
;
��M N
}
�� 	
}
�� 
}�� ��
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
$str	( �
,
� �
name
� �
,
� �
password
� �
)
� �
;
� �
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
$str	__! �
;
__� �
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
BikeName	yy{ �
,
yy� �
string
yy� �
areaName
yy� �
,
yy� �
string
yy� �
cityName
yy� �
,
yy� �
string
yy� �
pageUrl
yy� �
,
yy� �
string
yy� �

dealerArea
yy� �
)
yy� �
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

dealerArea	x �
)
� �
;
� �
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
dealerMobileNo
�� ,
,
��, -
message
��. 5
,
��5 6
esms
��7 ;
,
��; <
pageUrl
��= D
)
��D E
;
��E F
}
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
{
�� 
HttpContext
�� 
.
�� 
Current
�� #
.
��# $
Trace
��$ )
.
��) *
Warn
��* .
(
��. /
$str
��/ ^
+
��_ `
err
��a d
.
��d e
Message
��e l
)
��l m
;
��m n

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
��2 3
err
��3 6
,
��6 7
$str
��8 d
)
��d e
;
��e f
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
�� 
}
�� 	
public
�� 
void
�� ,
NewBikePriceQuoteSMSToCustomer
�� 2
(
��2 3#
PQ_DealerDetailEntity
��3 H
dealerEntity
��I U
,
��U V
string
��W ]
customerMobile
��^ l
,
��l m
string
��n t
customerName��u �
,��� �
string��� �
BikeName��� �
,��� �
string��� �

dealerName��� �
,��� �
string��� �
dealerContactNo��� �
,��� �
string��� �
dealerAddress��� �
,��� �
string��� �
pageUrl��� �
,��� �
uint��� �
bookingAmount��� �
,��� �
uint��� �
insuranceAmount��� �
=��� �
$num��� �
,��� �
bool��� �$
hasBumperDealerOffer��� �
=��� �
false��� �
)��� �
{
�� 	
try
�� 
{
�� 
bool
�� 
isFlipkartOffer
�� $
=
��% &
false
��' ,
;
��, -
bool
�� 
isAccessories
�� "
=
��# $
false
��% *
;
��* +
if
�� 
(
�� 
dealerEntity
��  
.
��  !
	objOffers
��! *
!=
��+ -
null
��. 2
&&
��3 5
dealerEntity
��6 B
.
��B C
	objOffers
��C L
.
��L M
Count
��M R
>
��S T
$num
��U V
)
��V W
{
�� 
foreach
�� 
(
�� 
var
��  
offer
��! &
in
��' )
dealerEntity
��* 6
.
��6 7
	objOffers
��7 @
)
��@ A
{
�� 
if
�� 
(
�� 
offer
�� !
.
��! "
	OfferText
��" +
.
��+ ,
ToLower
��, 3
(
��3 4
)
��4 5
.
��5 6
Contains
��6 >
(
��> ?
$str
��? I
)
��I J
)
��J K
{
�� 
isFlipkartOffer
�� +
=
��, -
true
��. 2
;
��2 3
break
�� !
;
��! "
}
�� 
else
�� 
if
�� 
(
��  !
offer
��! &
.
��& '
	OfferText
��' 0
.
��0 1
ToLower
��1 8
(
��8 9
)
��9 :
.
��: ;
Contains
��; C
(
��C D
$str
��D Q
)
��Q R
)
��R S
{
�� 
isAccessories
�� )
=
��* +
true
��, 0
;
��0 1
break
�� !
;
��! "
}
�� 
}
�� 
}
��  
EnumSMSServiceType
�� "
esms
��# '
=
��( ) 
EnumSMSServiceType
��* <
.
��< =,
NewBikePriceQuoteSMSToCustomer
��= [
;
��[ \
string
�� 
message
�� 
=
��  *
NewBikePQCustomerSMSTemplate
��! =
(
��= >
BikeName
��> F
,
��F G

dealerName
��H R
,
��R S
dealerContactNo
��T c
,
��c d
dealerAddress
��e r
,
��r s
bookingAmount��t �
,��� �
insuranceAmount��� �
,��� �$
hasBumperDealerOffer��� �
,��� �
isFlipkartOffer��� �
,��� �
isAccessories��� �
)��� �
;��� �
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
customerMobile
�� ,
,
��, -
message
��. 5
,
��5 6
esms
��7 ;
,
��; <
pageUrl
��= D
)
��D E
;
��E F
}
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
{
�� 
HttpContext
�� 
.
�� 
Current
�� #
.
��# $
Trace
��$ )
.
��) *
Warn
��* .
(
��. /
$str
��/ `
+
��a b
err
��c f
.
��f g
Message
��g n
)
��n o
;
��o p

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
��2 3
err
��3 6
,
��6 7
$str
��8 f
)
��f g
;
��g h
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
�� 
}
�� 	
public
�� 
void
�� &
BikeBookingSMSToCustomer
�� ,
(
��, -
string
��- 3
customerMobile
��4 B
,
��B C
string
��D J
customerName
��K W
,
��W X
string
��Y _
BikeName
��` h
,
��h i
string
��j p

dealerName
��q {
,
��{ |
string��} �
dealerContactNo��� �
,��� �
string��� �
dealerAddress��� �
,��� �
string��� �
pageUrl��� �
,��� �
string��� �
bookingRefNum��� �
,��� �
uint��� �
insuranceAmount��� �
=��� �
$num��� �
)��� �
{
�� 	
bool
�� 
isOfferAvailable
�� !
=
��" #
false
��$ )
;
��) *
try
�� 
{
��  
EnumSMSServiceType
�� "
esms
��# '
=
��( ) 
EnumSMSServiceType
��* <
.
��< =%
BikeBookedSMSToCustomer
��= T
;
��T U
string
�� 
message
�� 
=
��  
$str
��! #
;
��# $
isOfferAvailable
��  
=
��! "
Convert
��# *
.
��* +
	ToBoolean
��+ 4
(
��4 5"
ConfigurationManager
��5 I
.
��I J
AppSettings
��J U
[
��U V
$str
��V h
]
��h i
)
��i j
;
��j k
if
�� 
(
�� 
insuranceAmount
�� #
==
��$ &
$num
��' (
)
��( )
{
�� 
message
�� 
=
�� 
$str
�� A
+
��B C
BikeName
��D L
+
��M N
$str
��O `
+
��a b
bookingRefNum
��c p
+
��q r
$str
��s 
+��� �

dealerName��� �
+��� �
$str��� �
+��� �
dealerContactNo��� �
+��� �
$str��� �
+��� �
dealerAddress��� �
;��� �
}
�� 
else
�� 
{
�� 
message
�� 
=
�� 
String
�� $
.
��$ %
Format
��% +
(
��+ ,
$str��, �
,��� �
BikeName��� �
,��� �
bookingRefNum��� �
,��� �

dealerName��� �
,��� �
dealerContactNo��� �
,��� �
dealerAddress��� �
)��� �
;��� �
}
�� 
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
if
�� 
(
�� 
isOfferAvailable
�� $
&&
��% '
insuranceAmount
��( 7
==
��8 :
$num
��; <
)
��< =
{
�� 
message
�� 
+=
�� 
String
�� %
.
��% &
Format
��& ,
(
��, -
$str
��- v
,
��v w#
ConfigurationManager��x �
.��� �
AppSettings��� �
[��� �
$str��� �
]��� �
)��� �
;��� �
}
�� 
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
customerMobile
�� ,
,
��, -
message
��. 5
,
��5 6
esms
��7 ;
,
��; <
pageUrl
��= D
)
��D E
;
��E F
}
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
{
�� 
HttpContext
�� 
.
�� 
Current
�� #
.
��# $
Trace
��$ )
.
��) *
Warn
��* .
(
��. /
$str
��/ Z
+
��[ \
err
��] `
.
��` a
Message
��a h
)
��h i
;
��i j

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
��2 3
err
��3 6
,
��6 7
$str
��8 `
)
��` a
;
��a b
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
�� 
}
�� 	
public
�� 
void
�� $
BikeBookingSMSToDealer
�� *
(
��* +
string
��+ 1
dealerMobileNo
��2 @
,
��@ A
string
��B H
customerName
��I U
,
��U V
string
��W ]

dealerName
��^ h
,
��h i
string
��j p
customerMobile
��q 
,�� �
string��� �
BikeName��� �
,��� �
string��� �
pageUrl��� �
,��� �
UInt32��� �

bookingAmt��� �
,��� �
string��� �
bookingRefNum��� �
,��� �
uint��� �
insuranceAmount��� �
=��� �
$num��� �
)��� �
{
�� 	
try
�� 
{
��  
EnumSMSServiceType
�� "
esms
��# '
=
��( ) 
EnumSMSServiceType
��* <
.
��< =#
BikeBookedSMSToDealer
��= R
;
��R S
string
�� 
message
�� 
=
��  
$str
��! #
;
��# $
if
�� 
(
�� 
insuranceAmount
�� #
==
��$ &
$num
��' (
)
��( )
{
�� 
message
�� 
=
�� 
$str
�� 0
+
��1 2
customerName
��3 ?
+
��@ A
$str
��B M
+
��N O

bookingAmt
��P Z
+
��[ \
$str
��] d
+
��e f
BikeName
��g o
+
��p q
$str
��r y
+
��z {
bookingRefNum��| �
+��� �
$str��� �
+��� �
customerMobile��� �
+��� �
$str��� �
;��� �
}
�� 
else
�� 
{
�� 
message
�� 
=
�� 
String
�� $
.
��$ %
Format
��% +
(
��+ ,
$str��, �
,��� �
customerName��� �
,��� �

bookingAmt��� �
,��� �
BikeName��� �
,��� �
bookingRefNum��� �
,��� �
customerMobile��� �
)��� �
;��� �
}
�� 
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
dealerMobileNo
�� ,
,
��, -
message
��. 5
,
��5 6
esms
��7 ;
,
��; <
pageUrl
��= D
)
��D E
;
��E F
}
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
{
�� 
HttpContext
�� 
.
�� 
Current
�� #
.
��# $
Trace
��$ )
.
��) *
Warn
��* .
(
��. /
$str
��/ ^
+
��_ `
err
��a d
.
��d e
Message
��e l
)
��l m
;
��m n

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
��2 3
err
��3 6
,
��6 7
$str
��8 d
)
��d e
;
��e f
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� '
ClaimedOfferSMSToCustomer
�� -
(
��- .
string
��. 4
customerMobile
��5 C
,
��C D
string
��E K
pageUrl
��L S
)
��S T
{
�� 	
try
�� 
{
��  
EnumSMSServiceType
�� "
esms
��# '
=
��( ) 
EnumSMSServiceType
��* <
.
��< =
ClaimedOffer
��= I
;
��I J
string
�� 
message
�� 
=
��  
$str��! �
;��� �
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
customerMobile
�� ,
,
��, -
message
��. 5
,
��5 6
esms
��7 ;
,
��; <
pageUrl
��= D
)
��D E
;
��E F
}
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
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
��2 3
err
��3 6
,
��6 7
$str
��8 a
)
��a b
;
��b c
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� .
 SaveNewBikePriceQuoteSMSToDealer
�� 4
(
��4 5
uint
��5 9
pqId
��: >
,
��> ?
string
��@ F
dealerMobileNo
��G U
,
��U V
string
��W ]
customerName
��^ j
,
��j k
string
��l r
customerMobile��s �
,��� �
string��� �
BikeName��� �
,��� �
string��� �
areaName��� �
,��� �
string��� �
cityName��� �
,��� �
string��� �
pageUrl��� �
,��� �
string��� �

dealerArea��� �
)��� �
{
�� 	
try
�� 
{
��  
EnumSMSServiceType
�� "
esms
��# '
=
��( ) 
EnumSMSServiceType
��* <
.
��< =*
NewBikePriceQuoteSMSToDealer
��= Y
;
��Y Z
string
�� 
message
�� 
=
��  (
NewBikePQDealerSMSTemplate
��! ;
(
��; <
customerName
��< H
,
��H I
customerMobile
��J X
,
��X Y
BikeName
��Z b
,
��b c
areaName
��d l
,
��l m
cityName
��n v
,
��v w

dealerArea��x �
)��� �
;��� � 
SavePQNotification
�� "
obj
��# &
=
��' (
new
��) , 
SavePQNotification
��- ?
(
��? @
)
��@ A
;
��A B
obj
�� 
.
�� %
SaveDealerPQSMSTemplate
�� +
(
��+ ,
pqId
��, 0
,
��0 1
message
��2 9
,
��9 :
(
��; <
int
��< ?
)
��? @
esms
��@ D
,
��D E
dealerMobileNo
��F T
,
��T U
pageUrl
��V ]
)
��] ^
;
��^ _
}
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
{
�� 
HttpContext
�� 
.
�� 
Current
�� #
.
��# $
Trace
��$ )
.
��) *
Warn
��* .
(
��. /
$str
��/ ^
+
��_ `
err
��a d
.
��d e
Message
��e l
)
��l m
;
��m n

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
��2 3
err
��3 6
,
��6 7
$str
��8 d
)
��d e
;
��e f
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� 0
"SaveNewBikePriceQuoteSMSToCustomer
�� 6
(
��6 7
uint
��7 ;
pqId
��< @
,
��@ A#
PQ_DealerDetailEntity
��B W
dealerEntity
��X d
,
��d e
string
��f l
customerMobile
��m {
,
��{ |
string��} �
customerName��� �
,��� �
string��� �
BikeName��� �
,��� �
string��� �

dealerName��� �
,��� �
string��� �
dealerContactNo��� �
,��� �
string��� �
dealerAddress��� �
,��� �
string��� �
pageUrl��� �
,��� �
uint��� �
bookingAmount��� �
,��� �
uint��� �
insuranceAmount��� �
=��� �
$num��� �
,��� �
bool��� �$
hasBumperDealerOffer��� �
=��� �
false��� �
)��� �
{
�� 	
try
�� 
{
�� 
bool
�� 
isFlipkartOffer
�� $
=
��% &
false
��' ,
;
��, -
bool
�� 
isAccessories
�� "
=
��# $
false
��% *
;
��* +
if
�� 
(
�� 
dealerEntity
��  
.
��  !
	objOffers
��! *
!=
��+ -
null
��. 2
&&
��3 5
dealerEntity
��6 B
.
��B C
	objOffers
��C L
.
��L M
Count
��M R
>
��S T
$num
��U V
)
��V W
{
�� 
foreach
�� 
(
�� 
var
��  
offer
��! &
in
��' )
dealerEntity
��* 6
.
��6 7
	objOffers
��7 @
)
��@ A
{
�� 
if
�� 
(
�� 
offer
�� !
.
��! "
	OfferText
��" +
.
��+ ,
ToLower
��, 3
(
��3 4
)
��4 5
.
��5 6
Contains
��6 >
(
��> ?
$str
��? I
)
��I J
)
��J K
{
�� 
isFlipkartOffer
�� +
=
��, -
true
��. 2
;
��2 3
break
�� !
;
��! "
}
�� 
else
�� 
if
�� 
(
��  !
offer
��! &
.
��& '
	OfferText
��' 0
.
��0 1
ToLower
��1 8
(
��8 9
)
��9 :
.
��: ;
Contains
��; C
(
��C D
$str
��D Q
)
��Q R
)
��R S
{
�� 
isAccessories
�� )
=
��* +
true
��, 0
;
��0 1
break
�� !
;
��! "
}
�� 
}
�� 
}
��  
EnumSMSServiceType
�� "
esms
��# '
=
��( ) 
EnumSMSServiceType
��* <
.
��< =,
NewBikePriceQuoteSMSToCustomer
��= [
;
��[ \
string
�� 
message
�� 
=
��  *
NewBikePQCustomerSMSTemplate
��! =
(
��= >
BikeName
��> F
,
��F G

dealerName
��H R
,
��R S
dealerContactNo
��T c
,
��c d
dealerAddress
��e r
,
��r s
bookingAmount��t �
,��� �
insuranceAmount��� �
,��� �$
hasBumperDealerOffer��� �
,��� �
isFlipkartOffer��� �
,��� �
isAccessories��� �
)��� �
;��� � 
SavePQNotification
�� "
obj
��# &
=
��' (
new
��) , 
SavePQNotification
��- ?
(
��? @
)
��@ A
;
��A B
obj
�� 
.
�� '
SaveCustomerPQSMSTemplate
�� -
(
��- .
pqId
��. 2
,
��2 3
message
��4 ;
,
��; <
(
��= >
int
��> A
)
��A B
esms
��B F
,
��F G
customerMobile
��H V
,
��V W
pageUrl
��X _
)
��_ `
;
��` a
}
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
{
�� 
HttpContext
�� 
.
�� 
Current
�� #
.
��# $
Trace
��$ )
.
��) *
Warn
��* .
(
��. /
$str
��/ d
+
��e f
err
��g j
.
��j k
Message
��k r
)
��r s
;
��s t

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
��2 3
err
��3 6
,
��6 7
$str
��8 f
)
��f g
;
��g h
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
private
�� 
static
�� 
string
�� *
NewBikePQCustomerSMSTemplate
�� :
(
��: ;
string
��; A
BikeName
��B J
,
��J K
string
��L R

dealerName
��S ]
,
��] ^
string
��_ e
dealerContactNo
��f u
,
��u v
string
��w }
dealerAddress��~ �
,��� �
uint��� �
bookingAmount��� �
,��� �
uint��� �
insuranceAmount��� �
,��� �
bool��� �$
hasBumperDealerOffer��� �
,��� �
bool��� �
isFlipkartOffer��� �
,��� �
bool��� �
isAccessories��� �
)��� �
{
�� 	
string
�� 
message
�� 
=
�� 
$str
�� 
;
��  
if
�� 
(
�� 
!
�� "
hasBumperDealerOffer
�� %
)
��% &
{
�� 
if
�� 
(
�� 
insuranceAmount
�� #
==
��$ &
$num
��' (
)
��( )
{
�� 
if
�� 
(
�� 
isFlipkartOffer
�� '
)
��' (
{
�� 
message
�� 
=
��  !
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str��0 �
,��� �
bookingAmount��� �
,��� �

dealerName��� �
,��� �
dealerAddress��� �
,��� �
dealerContactNo��� �
)��� �
;��� �
}
�� 
else
�� 
if
�� 
(
�� 
isAccessories
�� *
)
��* +
{
�� 
message
�� 
=
��  !
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str��0 �
,��� �
bookingAmount��� �
,��� �

dealerName��� �
,��� �
dealerAddress��� �
,��� �
dealerContactNo��� �
)��� �
;��� �
}
�� 
else
�� 
{
�� 
message
�� 
=
��  !
String
��" (
.
��( )
Format
��) /
(
��/ 0
$str��0 �
,��� �
bookingAmount��� �
,��� �

dealerName��� �
,��� �
dealerAddress��� �
,��� �
dealerContactNo��� �
)��� �
;��� �
}
�� 
}
�� 
else
�� 
{
�� 
message
�� 
=
�� 
String
�� $
.
��$ %
Format
��% +
(
��+ ,
$str��, �
,��� �
BikeName��� �
,��� �

dealerName��� �
,��� �
dealerContactNo��� �
,��� �
bookingAmount��� �
)��� �
;��� �
}
�� 
}
�� 
else
�� 
{
�� 
message
�� 
=
�� 
String
��  
.
��  !
Format
��! '
(
��' (
$str��( �
,��� �
bookingAmount��� �
,��� �
BikeName��� �
)��� �
;��� �
}
�� 
return
�� 
message
�� 
;
�� 
}
�� 	
private
�� 
static
�� 
string
�� (
NewBikePQDealerSMSTemplate
�� 8
(
��8 9
string
��9 ?
customerName
��@ L
,
��L M
string
��N T
customerMobile
��U c
,
��c d
string
��e k
BikeName
��l t
,
��t u
string
��v |
areaName��} �
,��� �
string��� �
cityName��� �
,��� �
string��� �

dealerArea��� �
)��� �
{
�� 	
string
�� 
message
�� 
=
�� 
$str
�� 
;
��  
if
�� 
(
�� 
!
�� 
string
�� 
.
�� 
IsNullOrEmpty
�� %
(
��% &
areaName
��& .
)
��. /
)
��/ 0
{
�� 
areaName
�� 
=
�� 
$str
�� 
+
��  !
areaName
��" *
;
��* +
}
�� 
message
�� 
=
�� 
string
�� 
.
�� 
Format
�� #
(
��# $
$str��$ �
,��� �

dealerArea��� �
,��� �
customerName��� �
,��� �
areaName��� �
,��� �
cityName��� �
,��� �
customerMobile��� �
,��� �
BikeName��� �
)��� �
;��� �
return
�� 
message
�� 
;
�� 
}
�� 	
public
�� 
void
�� 0
"SaveNewBikePriceQuoteSMSToCustomer
�� 6
(
��6 7
uint
��7 ;
pqId
��< @
,
��@ A
string
��B H
message
��I P
,
��P Q
string
��R X
customerMobile
��Y g
,
��g h
string
��i o

requestUrl
��p z
)
��z {
{
�� 	
try
�� 
{
��  
EnumSMSServiceType
�� "
esms
��# '
=
��( ) 
EnumSMSServiceType
��* <
.
��< =,
NewBikePriceQuoteSMSToCustomer
��= [
;
��[ \ 
SavePQNotification
�� "
obj
��# &
=
��' (
new
��) , 
SavePQNotification
��- ?
(
��? @
)
��@ A
;
��A B
obj
�� 
.
�� '
SaveCustomerPQSMSTemplate
�� -
(
��- .
pqId
��. 2
,
��2 3
message
��4 ;
,
��; <
(
��= >
int
��> A
)
��A B
esms
��B F
,
��F G
customerMobile
��H V
,
��V W

requestUrl
��X b
)
��b c
;
��c d
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
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
$str
��7 {
)
��{ |
;
��| }
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� #
SMSMobileVerification
�� )
(
��) *
string
��* 0
number
��1 7
,
��7 8
string
��9 ?
otp
��@ C
,
��C D
string
��E K
pageUrl
��L S
)
��S T
{
�� 	
try
�� 
{
��  
EnumSMSServiceType
�� "
smsEnum
��# *
=
��+ , 
EnumSMSServiceType
��- ?
.
��? @$
BookingCancellationOTP
��@ V
;
��V W
string
�� 
message
�� 
=
��  
string
��! '
.
��' (
Empty
��( -
;
��- .
message
�� 
=
�� 
string
��  
.
��  !
Format
��! '
(
��' (
$str��( �
,��� �
otp��� �
)��� �
;��� �
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
��  
ProcessPrioritySMS
�� %
(
��% &
number
��& ,
,
��, -
message
��. 5
,
��5 6
smsEnum
��7 >
,
��> ?
pageUrl
��@ G
,
��G H
true
��I M
)
��M N
;
��N O
}
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
{
�� 
HttpContext
�� 
.
�� 
Current
�� #
.
��# $
Trace
��$ )
.
��) *
Warn
��* .
(
��. /
$str
��/ \
+
��] ^
err
��_ b
.
��b c
Message
��c j
)
��j k
;
��k l

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
err
��3 6
,
��6 7
$str
��8 b
)
��b c
;
��c d
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� &
SMSForPhotoUploadTwoDays
�� ,
(
��, -
string
��- 3
customerName
��4 @
,
��@ A
string
��B H
customerNumber
��I W
,
��W X
string
��Y _
make
��` d
,
��d e
string
��f l
model
��m r
,
��r s
string
��t z
	profileId��{ �
,��� �
string��� �
editUrl��� �
)��� �
{
�� 	
try
�� 
{
��  
EnumSMSServiceType
�� "
smsEnum
��# *
=
��+ , 
EnumSMSServiceType
��- ?
.
��? @&
SMSForPhotoUploadTwoDays
��@ X
;
��X Y
string
�� 
message
�� 
=
��  
string
��! '
.
��' (
Empty
��( -
;
��- .
message
�� 
=
�� 
string
��  
.
��  !
Format
��! '
(
��' (
$str��( �
,��� �
make��� �
,��� �
model��� �
,��� �
editUrl��� �
)��� �
;��� �
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
��  
ProcessPrioritySMS
�� %
(
��% &
customerNumber
��& 4
,
��4 5
message
��6 =
,
��= >
smsEnum
��? F
,
��F G
$str
��H k
,
��k l
true
��m q
)
��q r
;
��r s
}
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
{
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
err
��3 6
,
��6 7
$str
��8 [
)
��[ \
;
��\ ]
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� *
BookingCancallationSMSToUser
�� 0
(
��0 1
string
��1 7
number
��8 >
,
��> ?
string
��@ F
customerName
��G S
,
��S T
string
��U [
pageUrl
��\ c
)
��c d
{
�� 	
try
�� 
{
��  
EnumSMSServiceType
�� "
smsEnum
��# *
=
��+ , 
EnumSMSServiceType
��- ?
.
��? @+
BookingCancellationToCustomer
��@ ]
;
��] ^
string
�� 
message
�� 
=
��  
string
��! '
.
��' (
Empty
��( -
;
��- .
message
�� 
=
�� 
string
��  
.
��  !
Format
��! '
(
��' (
$str��( �
,��� �
customerName��� �
)��� �
;��� �
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
number
�� $
,
��$ %
message
��& -
,
��- .
smsEnum
��/ 6
,
��6 7
pageUrl
��8 ?
,
��? @
true
��A E
)
��E F
;
��F G
}
�� 
catch
�� 
(
�� 
	Exception
�� 
err
��  
)
��  !
{
�� 
HttpContext
�� 
.
�� 
Current
�� #
.
��# $
Trace
��$ )
.
��) *
Warn
��* .
(
��. /
$str
��/ \
+
��] ^
err
��_ b
.
��b c
Message
��c j
)
��j k
;
��k l

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
err
��3 6
,
��6 7
$str
��8 b
)
��b c
;
��c d
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� $
UsedPurchaseInquirySMS
�� *
(
��* + 
EnumSMSServiceType
��+ =
smsType
��> E
,
��E F
string
��G M
number
��N T
,
��T U
string
��V \
message
��] d
,
��d e
string
��f l
pageurl
��m t
)
��t u
{
�� 	
try
�� 
{
�� 
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
number
�� $
,
��$ %
message
��& -
,
��- .
smsType
��/ 6
,
��6 7
pageurl
��8 ?
)
��? @
;
��@ A
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
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
String
��7 =
.
��= >
Format
��> D
(
��D E
$str��E �
,��� �
number��� �
,��� �
message��� �
,��� �
pageurl��� �
)��� �
)��� �
;��� �
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� (
ApprovalUsedSellListingSMS
�� .
(
��. / 
EnumSMSServiceType
��/ A
smsType
��B I
,
��I J
string
��K Q
number
��R X
,
��X Y
string
��Z `
	profileId
��a j
,
��j k
string
��l r
customerName
��s 
,�� �
string��� �
pageurl��� �
)��� �
{
�� 	
string
�� 
message
�� 
=
�� 
String
�� #
.
��# $
Format
��$ *
(
��* +
$str��+ �
,��� �
customerName��� �
,��� �
	profileId��� �
.��� �
ToUpper��� �
(��� �
)��� �
)��� �
;��� �
try
�� 
{
�� 
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
number
�� $
,
��$ %
message
��& -
,
��- .
smsType
��/ 6
,
��6 7
pageurl
��8 ?
)
��? @
;
��@ A
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
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
String
��7 =
.
��= >
Format
��> D
(
��D E
$str��E �
,��� �
number��� �
,��� �
message��� �
,��� �
pageurl��� �
,��� �
	profileId��� �
)��� �
)��� �
;��� �
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� )
RejectionUsedSellListingSMS
�� /
(
��/ 0 
EnumSMSServiceType
��0 B
smsType
��C J
,
��J K
string
��L R
number
��S Y
,
��Y Z
string
��[ a
	profileId
��b k
,
��k l
string
��m s
pageurl
��t {
)
��{ |
{
�� 	
string
�� 
message
�� 
=
�� 
String
�� #
.
��# $
Format
��$ *
(
��* +
$str��+ �
,��� �
	profileId��� �
.��� �
ToUpper��� �
(��� �
)��� �
)��� �
;��� �
try
�� 
{
�� 
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
number
�� $
,
��$ %
message
��& -
,
��- .
smsType
��/ 6
,
��6 7
pageurl
��8 ?
)
��? @
;
��@ A
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
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
String
��7 =
.
��= >
Format
��> D
(
��D E
$str��E �
,��� �
number��� �
,��� �
message��� �
,��� �
pageurl��� �
,��� �
	profileId��� �
)��� �
)��� �
;��� �
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� *
UsedSellSuccessfulListingSMS
�� 0
(
��0 1 
EnumSMSServiceType
��1 C
smsType
��D K
,
��K L
string
��M S
number
��T Z
,
��Z [
string
��\ b
	profileid
��c l
,
��l m
string
��n t
pageurl
��u |
)
��| }
{
�� 	
string
�� 
message
�� 
=
�� 
String
�� #
.
��# $
Format
��$ *
(
��* +
$str��+ �
,��� �
	profileid��� �
.��� �
ToUpper��� �
(��� �
)��� �
)��� �
;��� �
try
�� 
{
�� 
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
number
�� $
,
��$ %
message
��& -
,
��- .
smsType
��/ 6
,
��6 7
pageurl
��8 ?
)
��? @
;
��@ A
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
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
String
��7 =
.
��= >
Format
��> D
(
��D E
$str
��E ~
,
��~ 
number��� �
,��� �
message��� �
,��� �
pageurl��� �
)��� �
)��� �
;��� �
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� .
 ApprovalEditedUsedSellListingSMS
�� 4
(
��4 5 
EnumSMSServiceType
��5 G
smsType
��H O
,
��O P
string
��Q W
number
��X ^
,
��^ _
string
��` f
	profileId
��g p
,
��p q
string
��r x
customerName��y �
,��� �
string��� �
pageurl��� �
)��� �
{
�� 	
string
�� 
message
�� 
=
�� 
String
�� #
.
��# $
Format
��$ *
(
��* +
$str��+ �
,��� �
customerName��� �
,��� �
	profileId��� �
.��� �
ToUpper��� �
(��� �
)��� �
)��� �
;��� �
try
�� 
{
�� 
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
number
�� $
,
��$ %
message
��& -
,
��- .
smsType
��/ 6
,
��6 7
pageurl
��8 ?
)
��? @
;
��@ A
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
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
String
��7 =
.
��= >
Format
��> D
(
��D E
$str��E �
,��� �
number��� �
,��� �
message��� �
,��� �
pageurl��� �
,��� �
	profileId��� �
)��� �
)��� �
;��� �
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� /
!RejectionEditedUsedSellListingSMS
�� 5
(
��5 6 
EnumSMSServiceType
��6 H
smsType
��I P
,
��P Q
string
��R X
number
��Y _
,
��_ `
string
��a g
	profileId
��h q
,
��q r
string
��s y
customerName��z �
,��� �
string��� �
pageurl��� �
)��� �
{
�� 	
string
�� 
message
�� 
=
�� 
String
�� #
.
��# $
Format
��$ *
(
��* +
$str��+ �
,��� �
customerName��� �
,��� �
	profileId��� �
.��� �
ToUpper��� �
(��� �
)��� �
)��� �
;��� �
try
�� 
{
�� 
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
number
�� $
,
��$ %
message
��& -
,
��- .
smsType
��/ 6
,
��6 7
pageurl
��8 ?
)
��? @
;
��@ A
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
String
��7 =
.
��= >
Format
��> D
(
��D E
$str��E �
,��� �
number��� �
,��� �
message��� �
,��� �
pageurl��� �
,��� �
	profileId��� �
)��� �
)��� �
;��� �
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� %
ServiceCenterDetailsSMS
�� +
(
��+ ,
string
��, 2
number
��3 9
,
��9 :
string
��; A
name
��B F
,
��F G
string
��H N
address
��O V
,
��V W
string
��X ^
phone
��_ d
,
��d e
string
��f l
city
��m q
,
��q r
string
��s y
pageUrl��z �
)��� �
{
�� 	
try
�� 
{
�� 
string
�� 
message
�� 
=
��  
String
��! '
.
��' (
Format
��( .
(
��. /
$str��/ �
,��� �
Environment��� �
.��� �
NewLine��� �
,��� �
name��� �
,��� �
address��� �
,��� �
city��� �
,��� �
phone��� �
)��� �
;��� � 
EnumSMSServiceType
�� "
esms
��# '
=
��( ) 
EnumSMSServiceType
��* <
.
��< =/
!ServiceCenterDetailsSMSToCustomer
��= ^
;
��^ _
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
number
�� $
,
��$ %
message
��& -
,
��- .
esms
��/ 3
,
��3 4
pageUrl
��5 <
)
��< =
;
��= >
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
String
��7 =
.
��= >
Format
��> D
(
��D E
$str��E �
,��� �
number��� �
,��� �
name��� �
,��� �
address��� �
,��� �
phone��� �
,��� �
city��� �
,��� �
pageUrl��� �
)��� �
)��� �
;��� �
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
public
�� 
void
�� (
ExpiringListingReminderSMS
�� .
(
��. /
string
��/ 5
number
��6 <
,
��< =
string
��> D
pageUrl
��E L
,
��L M 
EnumSMSServiceType
��N `
esms
��a e
,
��e f
string
��g m
message
��n u
)
��u v
{
�� 	
try
�� 
{
�� 
	SMSCommon
�� 
sc
�� 
=
�� 
new
�� "
	SMSCommon
��# ,
(
��, -
)
��- .
;
��. /
sc
�� 
.
�� 

ProcessSMS
�� 
(
�� 
number
�� $
,
��$ %
message
��& -
,
��- .
esms
��/ 3
,
��3 4
pageUrl
��5 <
)
��< =
;
��= >
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 

ErrorClass
�� 
objErr
�� !
=
��" #
new
��$ '

ErrorClass
��( 2
(
��2 3
ex
��3 5
,
��5 6
String
��7 =
.
��= >
Format
��> D
(
��D E
$str��E �
,��� �
number��� �
,��� �
pageUrl��� �
,��� �
esms��� �
,��� �
message��� �
)��� �
)��� �
;��� �
objErr
�� 
.
�� 
SendMail
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
}
�� 
}�� �
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
string	| �
newUrl
� �
)
� �
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
models	##| �
)
##� �
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
})) �&
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
string	z �

reviewLink
� �
)
� �
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
string	  | �
makeName
  � �
,
  � �
string
  � �
	modelName
  � �
)
  � �
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