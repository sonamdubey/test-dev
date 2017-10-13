”N
>D:\work\bikewaleweb\NoPhotoListingsCommunication\NoPhotoSMS.cs
	namespace 	
Bikewale
 
. (
NoPhotoListingsCommunication /
{		 
public 

class 

NoPhotoSMS 
{ 
private 
readonly 
NoPhotoSMSDAL &
_objNoPhotoSMSDAL' 8
=9 :
null; ?
;? @
public 

NoPhotoSMS 
( 
NoPhotoSMSDAL '
objNoPhotoSMSDAL( 8
)8 9
{ 	
_objNoPhotoSMSDAL 
= 
objNoPhotoSMSDAL  0
;0 1
} 	
public 
void 
SendSMSNoPhoto "
(" #
)# $
{ 	!
NoPhotoUserListEntity !
objNoPhotoList" 0
=1 2
null3 7
;7 8
try 
{ 
Logs 
. 
WriteInfoLog !
(! "
$str" ^
)^ _
;_ `
objNoPhotoList 
=  
_objNoPhotoSMSDAL! 2
.2 3
SendSMSNoPhoto3 A
(A B
)B C
;C D
Logs   
.   
WriteInfoLog   !
(  ! "
$str  " m
)  m n
;  n o
if!! 
(!! 
objNoPhotoList!! "
!=!!# %
null!!& *
)!!* +
{"" 
Logs## 
.## 
WriteInfoLog## %
(##% &
$str##& t
)##t u
;##u v
if$$ 
($$ 
objNoPhotoList$$ &
.$$& '
objTwoDaySMSList$$' 7
!=$$8 :
null$$; ?
)$$? @
{%% 
SendSMSTwoDays&& &
(&&& '
objNoPhotoList&&' 5
.&&5 6
objTwoDaySMSList&&6 F
)&&F G
;&&G H
}'' 
if(( 
((( 
objNoPhotoList(( &
.((& '
objThreeDayMailList((' :
!=((; =
null((> B
)((B C
{)) 
SendEmailThreeDays** *
(*** +
objNoPhotoList**+ 9
.**9 :
objThreeDayMailList**: M
)**M N
;**N O
}++ 
if,, 
(,, 
objNoPhotoList,, &
.,,& '
objSevenDayMailList,,' :
!=,,; =
null,,> B
),,B C
{-- 
SendEmailSevenDays.. *
(..* +
objNoPhotoList..+ 9
...9 :
objSevenDayMailList..: M
)..M N
;..N O
}// 
}00 
}11 
catch22 
(22 
	Exception22 
ex22 
)22  
{33 
Logs44 
.44 
WriteErrorLog44 "
(44" #
$str44# \
)44\ ]
;44] ^

ErrorClass55 
objErr55 !
=55" #
new55$ '

ErrorClass55( 2
(552 3
ex553 5
,555 6
$str557 R
)55R S
;55S T
objErr66 
.66 
SendMail66 
(66  
)66  !
;66! "
}77 
Logs88 
.88 
WriteInfoLog88 
(88 
$str88 
)	88 Ä
;
88Ä Å
}99 	
private>> 
void>> 
SendSMSTwoDays>> #
(>># $
IEnumerable>>$ /
<>>/ 0
NoPhotoSMSEntity>>0 @
>>>@ A
objTwoDaySMSList>>B R
)>>R S
{?? 	
try@@ 
{AA 
UrlShortnerResponseBB #
responseBB$ ,
=BB- .
nullBB/ 3
;BB3 4
LogsCC 
.CC 
WriteInfoLogCC !
(CC! "
$strCC" A
)CCA B
;CCB C
foreachDD 
(DD 
varDD 
CustomerDetailsDD ,
inDD- /
objTwoDaySMSListDD0 @
)DD@ A
{EE 
stringGG 
editUrlGG "
=GG# $
stringGG% +
.GG+ ,
FormatGG, 2
(GG2 3
$strGG3 [
,GG[ \
UtilityGG] d
.GGd e
BWConfigurationGGe t
.GGt u
InstanceGGu }
.GG} ~
	BwHostUrl	GG~ á
,
GGá à
CustomerDetails
GGâ ò
.
GGò ô
	InquiryId
GGô ¢
)
GG¢ £
;
GG£ §
ifHH 
(HH 
!HH 
StringHH 
.HH  
IsNullOrEmptyHH  -
(HH- .
editUrlHH. 5
)HH5 6
)HH6 7
{II 
responseJJ  
=JJ! "
newJJ# &
UrlShortnerJJ' 2
(JJ2 3
)JJ3 4
.JJ4 5
GetShortUrlJJ5 @
(JJ@ A
editUrlJJA H
)JJH I
;JJI J
}KK 
stringLL 
shortUrlLL #
=LL$ %
responseLL& .
!=LL/ 1
nullLL2 6
?LL7 8
responseLL9 A
.LLA B
ShortUrlLLB J
:LLK L
editUrlLLM T
;LLT U(
SendEmailSMSToDealerCustomerMM 0
.MM0 1#
SMSNoPhotoUploadTwoDaysMM1 H
(MMH I
CustomerDetailsMMI X
.MMX Y
CustomerNameMMY e
,MMe f
CustomerDetailsMMg v
.MMv w
CustomerNumber	MMw Ö
,
MMÖ Ü
CustomerDetails
MMá ñ
.
MMñ ó
Make
MMó õ
,
MMõ ú
CustomerDetails
MMù ¨
.
MM¨ ≠
Model
MM≠ ≤
,
MM≤ ≥
CustomerDetails
MM¥ √
.
MM√ ƒ
	InquiryId
MMƒ Õ
,
MMÕ Œ
shortUrl
MMœ ◊
)
MM◊ ÿ
;
MMÿ Ÿ
}NN 
LogsOO 
.OO 
WriteInfoLogOO !
(OO! "
$strOO" ?
)OO? @
;OO@ A
}PP 
catchQQ 
(QQ 
	ExceptionQQ 
exQQ 
)QQ  
{RR 
LogsSS 
.SS 
WriteErrorLogSS "
(SS" #
$strSS# N
)SSN O
;SSO P

ErrorClassTT 
objErrTT !
=TT" #
newTT$ '

ErrorClassTT( 2
(TT2 3
exTT3 5
,TT5 6
$strTT7 R
)TTR S
;TTS T
objErrUU 
.UU 
SendMailUU 
(UU  
)UU  !
;UU! "
}VV 
}WW 	
private\\ 
void\\ 
SendEmailThreeDays\\ '
(\\' (
IEnumerable\\( 3
<\\3 4
NoPhotoSMSEntity\\4 D
>\\D E
objThreeDayMailList\\F Y
)\\Y Z
{]] 	
try^^ 
{__ 
Logs`` 
.`` 
WriteInfoLog`` !
(``! "
$str``" E
)``E F
;``F G
foreachaa 
(aa 
varaa 
CustomerDetailsaa ,
inaa- /
objThreeDayMailListaa0 C
)aaC D
{bb (
SendEmailSMSToDealerCustomercc 0
.cc0 11
%UsedBikePhotoRequestEmailForThreeDayscc1 V
(ccV W
CustomerDetailsccW f
.ccf g
CustomerEmailccg t
,cct u
CustomerDetails	ccv Ö
.
ccÖ Ü
CustomerName
ccÜ í
,
ccí ì
CustomerDetails
ccî £
.
cc£ §
Make
cc§ ®
,
cc® ©
CustomerDetails
cc™ π
.
ccπ ∫
Model
cc∫ ø
,
ccø ¿
CustomerDetails
cc¡ –
.
cc– —
	InquiryId
cc— ⁄
)
cc⁄ €
;
cc€ ‹
}dd 
Logsee 
.ee 
WriteInfoLogee !
(ee! "
$stree" C
)eeC D
;eeD E
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
(jj" #
$strjj# R
)jjR S
;jjS T

ErrorClasskk 
objErrkk !
=kk" #
newkk$ '

ErrorClasskk( 2
(kk2 3
exkk3 5
,kk5 6
$strkk7 V
)kkV W
;kkW X
objErrll 
.ll 
SendMailll 
(ll  
)ll  !
;ll! "
}mm 
}nn 	
privatess 
voidss 
SendEmailSevenDaysss '
(ss' (
IEnumerabless( 3
<ss3 4
NoPhotoSMSEntityss4 D
>ssD E
objSevenDayMailListssF Y
)ssY Z
{tt 	
tryuu 
{vv 
Logsww 
.ww 
WriteInfoLogww !
(ww! "
$strww" E
)wwE F
;wwF G
foreachxx 
(xx 
varxx 
CustomerDetailsxx ,
inxx- /
objSevenDayMailListxx0 C
)xxC D
{yy (
SendEmailSMSToDealerCustomer{{ 0
.{{0 11
%UsedBikePhotoRequestEmailForSevenDays{{1 V
({{V W
CustomerDetails{{W f
.{{f g
CustomerEmail{{g t
,{{t u
CustomerDetails	{{v Ö
.
{{Ö Ü
CustomerName
{{Ü í
,
{{í ì
CustomerDetails
{{î £
.
{{£ §
Make
{{§ ®
,
{{® ©
CustomerDetails
{{™ π
.
{{π ∫
Model
{{∫ ø
,
{{ø ¿
CustomerDetails
{{¡ –
.
{{– —
	InquiryId
{{— ⁄
)
{{⁄ €
;
{{€ ‹
}}} 
Logs~~ 
.~~ 
WriteInfoLog~~ !
(~~! "
$str~~" C
)~~C D
;~~D E
}
ÄÄ 
catch
ÅÅ 
(
ÅÅ 
	Exception
ÅÅ 
ex
ÅÅ 
)
ÅÅ  
{
ÇÇ 
Logs
ÉÉ 
.
ÉÉ 
WriteErrorLog
ÉÉ "
(
ÉÉ" #
$str
ÉÉ# R
)
ÉÉR S
;
ÉÉS T

ErrorClass
ÑÑ 
objErr
ÑÑ !
=
ÑÑ" #
new
ÑÑ$ '

ErrorClass
ÑÑ( 2
(
ÑÑ2 3
ex
ÑÑ3 5
,
ÑÑ5 6
$str
ÑÑ7 V
)
ÑÑV W
;
ÑÑW X
objErr
ÖÖ 
.
ÖÖ 
SendMail
ÖÖ 
(
ÖÖ  
)
ÖÖ  !
;
ÖÖ! "
}
ÜÜ 
}
áá 	
}
àà 
}ââ Ã?
AD:\work\bikewaleweb\NoPhotoListingsCommunication\NoPhotoSMSDAL.cs
	namespace 	
Bikewale
 
. (
NoPhotoListingsCommunication /
{		 
public 

class 
NoPhotoSMSDAL 
{ 
public !
NoPhotoUserListEntity $
SendSMSNoPhoto% 3
(3 4
)4 5
{ 	
Logs 
. 
WriteInfoLog 
( 
$str 6
)6 7
;7 8!
NoPhotoUserListEntity !
objNoPhotoList" 0
=1 2
null3 7
;7 8
try 
{ 
using 
( 
	DbCommand  
cmd! $
=% &
	DbFactory' 0
.0 1
GetDBCommand1 =
(= >
$str> Z
)Z [
)[ \
{ 
cmd 
. 
CommandType #
=$ %
CommandType& 1
.1 2
StoredProcedure2 A
;A B
using 
( 
IDataReader &
dr' )
=* +
MySqlDatabase, 9
.9 :
SelectQuery: E
(E F
cmdF I
,I J
ConnectionTypeK Y
.Y Z
MasterDatabaseZ h
)h i
)i j
{   
if!! 
(!! 
dr!! 
!=!! !
null!!" &
)!!& '
{"" 
objNoPhotoList## *
=##+ ,
new##- 0!
NoPhotoUserListEntity##1 F
(##F G
)##G H
;##H I
objNoPhotoList$$ *
.$$* +
objTwoDaySMSList$$+ ;
=$$< =
new$$> A
List$$B F
<$$F G
NoPhotoSMSEntity$$G W
>$$W X
($$X Y
)$$Y Z
;$$Z [
while%% !
(%%" #
dr%%# %
.%%% &
Read%%& *
(%%* +
)%%+ ,
)%%, -
{&& 
objNoPhotoList''  .
.''. /
objTwoDaySMSList''/ ?
.''? @
Add''@ C
(''C D
new''D G
NoPhotoSMSEntity''H X
{((  !
CustomerName))$ 0
=))1 2
Convert))3 :
.)): ;
ToString)); C
())C D
dr))D F
[))F G
$str))G U
]))U V
)))V W
,))W X
Make**$ (
=**) *
Convert**+ 2
.**2 3
ToString**3 ;
(**; <
dr**< >
[**> ?
$str**? I
]**I J
)**J K
,**K L
Model++$ )
=++* +
Convert++, 3
.++3 4
ToString++4 <
(++< =
dr++= ?
[++? @
$str++@ K
]++K L
)++L M
,++M N
	InquiryId,,$ -
=,,. /
Convert,,0 7
.,,7 8
ToString,,8 @
(,,@ A
dr,,A C
[,,C D
$str,,D O
],,O P
),,P Q
,,,Q R
CustomerNumber--$ 2
=--3 4
Convert--5 <
.--< =
ToString--= E
(--E F
dr--F H
[--H I
$str--I Y
]--Y Z
)--Z [
,--[ \
}00  !
)00! "
;00" #
}11 
Logs22  
.22  !
WriteInfoLog22! -
(22- .
$str22. Z
)22Z [
;22[ \
}33 
if44 
(44 
dr44 
.44 

NextResult44 )
(44) *
)44* +
)44+ ,
{55 
objNoPhotoList66 *
.66* +
objThreeDayMailList66+ >
=66? @
new66A D
List66E I
<66I J
NoPhotoSMSEntity66J Z
>66Z [
(66[ \
)66\ ]
;66] ^
while77 !
(77" #
dr77# %
.77% &
Read77& *
(77* +
)77+ ,
)77, -
{88 
objNoPhotoList99  .
.99. /
objThreeDayMailList99/ B
.99B C
Add99C F
(99F G
new99G J
NoPhotoSMSEntity99K [
{::  !
CustomerName;;$ 0
=;;1 2
Convert;;3 :
.;;: ;
ToString;;; C
(;;C D
dr;;D F
[;;F G
$str;;G U
];;U V
);;V W
,;;W X
Make<<$ (
=<<) *
Convert<<+ 2
.<<2 3
ToString<<3 ;
(<<; <
dr<<< >
[<<> ?
$str<<? I
]<<I J
)<<J K
,<<K L
Model==$ )
===* +
Convert==, 3
.==3 4
ToString==4 <
(==< =
dr=== ?
[==? @
$str==@ K
]==K L
)==L M
,==M N
	InquiryId>>$ -
=>>. /
Convert>>0 7
.>>7 8
ToString>>8 @
(>>@ A
dr>>A C
[>>C D
$str>>D O
]>>O P
)>>P Q
,>>Q R
CustomerEmail??$ 1
=??2 3
Convert??4 ;
.??; <
ToString??< D
(??D E
dr??E G
[??G H
$str??H W
]??W X
)??X Y
,??Y Z
}AA  !
)AA! "
;AA" #
}BB 
LogsCC  
.CC  !
WriteInfoLogCC! -
(CC- .
$strCC. ^
)CC^ _
;CC_ `
}DD 
ifEE 
(EE 
drEE 
.EE 

NextResultEE )
(EE) *
)EE* +
)EE+ ,
{FF 
objNoPhotoListGG *
.GG* +
objSevenDayMailListGG+ >
=GG? @
newGGA D
ListGGE I
<GGI J
NoPhotoSMSEntityGGJ Z
>GGZ [
(GG[ \
)GG\ ]
;GG] ^
whileHH !
(HH" #
drHH# %
.HH% &
ReadHH& *
(HH* +
)HH+ ,
)HH, -
{II 
objNoPhotoListJJ  .
.JJ. /
objSevenDayMailListJJ/ B
.JJB C
AddJJC F
(JJF G
newJJG J
NoPhotoSMSEntityJJK [
{KK  !
CustomerNameLL$ 0
=LL1 2
ConvertLL3 :
.LL: ;
ToStringLL; C
(LLC D
drLLD F
[LLF G
$strLLG U
]LLU V
)LLV W
,LLW X
MakeMM$ (
=MM) *
ConvertMM+ 2
.MM2 3
ToStringMM3 ;
(MM; <
drMM< >
[MM> ?
$strMM? I
]MMI J
)MMJ K
,MMK L
ModelNN$ )
=NN* +
ConvertNN, 3
.NN3 4
ToStringNN4 <
(NN< =
drNN= ?
[NN? @
$strNN@ K
]NNK L
)NNL M
,NNM N
	InquiryIdOO$ -
=OO. /
ConvertOO0 7
.OO7 8
ToStringOO8 @
(OO@ A
drOOA C
[OOC D
$strOOD O
]OOO P
)OOP Q
,OOQ R
CustomerEmailPP$ 1
=PP2 3
ConvertPP4 ;
.PP; <
ToStringPP< D
(PPD E
drPPE G
[PPG H
$strPPH W
]PPW X
)PPX Y
,PPY Z
}QQ  !
)QQ! "
;QQ" #
}RR 
LogsSS  
.SS  !
WriteInfoLogSS! -
(SS- .
$strSS. ^
)SS^ _
;SS_ `
}TT 
drUU 
.UU 
CloseUU  
(UU  !
)UU! "
;UU" #
}VV 
}WW 
}YY 
catchZZ 
(ZZ 
	ExceptionZZ 
exZZ 
)ZZ  
{[[ 
Logs\\ 
.\\ 
WriteInfoLog\\ !
(\\! "
$str\\" >
)\\> ?
;\\? @

ErrorClass]] 
objErr]] !
=]]" #
new]]$ '

ErrorClass]]( 2
(]]2 3
ex]]3 5
,]]5 6
$str]]7 U
)]]U V
;]]V W
objErr^^ 
.^^ 
SendMail^^ 
(^^  
)^^  !
;^^! "
}__ 
Logs`` 
.`` 
WriteInfoLog`` 
(`` 
$str`` A
)``A B
;``B C
returnaa 
objNoPhotoListaa !
;aa! "
}bb 	
}cc 
}dd ÿ	
DD:\work\bikewaleweb\NoPhotoListingsCommunication\NoPhotoSMSEntity.cs
	namespace 	
Bikewale
 
. (
NoPhotoListingsCommunication /
{ 
public 

class 
NoPhotoSMSEntity !
{		 
public

 
string

 
CustomerName

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

/ 0
public 
string 
Make 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
Model 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
	InquiryId 
{  !
get" %
;% &
set' *
;* +
}, -
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
CustomerNumber $
{% &
get' *
;* +
set, /
;/ 0
}1 2
} 
} Ê
ID:\work\bikewaleweb\NoPhotoListingsCommunication\NoPhotoUserListEntity.cs
	namespace 	
Bikewale
 
. (
NoPhotoListingsCommunication /
{ 
public		 

class		 !
NoPhotoUserListEntity		 &
{

 
public 
ICollection 
< 
NoPhotoSMSEntity +
>+ ,
objTwoDaySMSList- =
{> ?
get@ C
;C D
setE H
;H I
}J K
public 
ICollection 
< 
NoPhotoSMSEntity +
>+ ,
objThreeDayMailList- @
{A B
getC F
;F G
setH K
;K L
}M N
public 
ICollection 
< 
NoPhotoSMSEntity +
>+ ,
objSevenDayMailList- @
{A B
getC F
;F G
setH K
;K L
}M N
} 
} ∏
;D:\work\bikewaleweb\NoPhotoListingsCommunication\Program.cs
	namespace 	
Bikewale
 
. (
NoPhotoListingsCommunication /
{ 
class 	
Program
 
{ 
static 
void 
Main 
( 
string 
[  
]  !
args" &
)& '
{ 	
try 
{ 
Logs 
. 
WriteInfoLog !
(! "
$str" O
)O P
;P Q

NoPhotoSMS 

objNoPhoto %
=& '
new( +

NoPhotoSMS, 6
(6 7
new7 :
NoPhotoSMSDAL; H
(H I
)I J
)J K
;K L
Logs 
. 
WriteInfoLog !
(! "
$str" ^
)^ _
;_ `

objNoPhoto 
. 
SendSMSNoPhoto )
() *
)* +
;+ ,
Logs 
. 
WriteInfoLog !
(! "
$str" \
)\ ]
;] ^
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
$str# ]
)] ^
;^ _

ErrorClass 
objErr !
=" #
new$ '

ErrorClass( 2
(2 3
ex3 5
,5 6
$str7 ]
)] ^
;^ _
objErr 
. 
SendMail 
(  
)  !
;! "
} 
Logs 
. 
WriteInfoLog 
( 
$str Q
)Q R
;R S
} 	
}   
}!! ˆ
KD:\work\bikewaleweb\NoPhotoListingsCommunication\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str 7
)7 8
]8 9
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
$str 9
)9 :
]: ;
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