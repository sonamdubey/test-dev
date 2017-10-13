
>D:\work\bikewaleweb\Bikewale.UserReviewsRatingEmail\Program.cs
	namespace 	
Bikewale
 
. "
UserReviewsRatingEmail )
{ 
class 	
Program
 
{ 
static 
void 
Main 
( 
) 
{		 	
Logs

 
.

 
WriteInfoLog

 
(

 
$str

 C
)

C D
;

D E
try 
{ 
Logs 
. 
WriteInfoLog !
(! "
$str" R
)R S
;S T
( 
new $
UserReviewsRatingEmailBL -
(- .
). /
)/ 0
.0 1!
SendRatingEmailToUser1 F
(F G
)G H
;H I
Logs 
. 
WriteInfoLog !
(! "
$str" ]
)] ^
;^ _
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
$str# 9
+: ;
ex< >
.> ?
Message? F
)F G
;G H
} 
Logs	 
. 
WriteInfoLog 
( 
$str >
)> ?
;? @
} 	
} 
} ù
ND:\work\bikewaleweb\Bikewale.UserReviewsRatingEmail\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str :
): ;
]; <
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
$str <
)< =
]= >
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
]$$) *É*
OD:\work\bikewaleweb\Bikewale.UserReviewsRatingEmail\UserReviewsRatingEmailBL.cs
	namespace 	
Bikewale
 
. "
UserReviewsRatingEmail )
{ 
public 

class $
UserReviewsRatingEmailBL )
{ 
private 
readonly %
UserReviewsRatingEmailDAL 2
userRatingRepo3 A
=B C
newD G%
UserReviewsRatingEmailDALH a
(a b
)b c
;c d
private 
readonly 
UrlShortner $
url% (
=) *
new+ .
UrlShortner/ :
(: ;
); <
;< =
public 
void !
SendRatingEmailToUser )
() *
)* +
{ 	
IEnumerable 
< (
UserReviewsRatingEmailEntity 4
>4 5
userList6 >
=? @
GetCustomerMailDataA T
(T U
)U V
;V W
try   
{!! 
if"" 
("" 
userList"" 
!="" 
null""  $
)""$ %
{## 
foreach$$ 
($$ 
var$$  
user$$! %
in$$& (
userList$$) 1
)$$1 2
{%% 
UserReviewsEmails&& )
.&&) *%
SendRatingSubmissionEmail&&* C
(&&C D
user''  
.''  !
CustomerName''! -
,''- .
user((  
.((  !
CustomerEmail((! .
,((. /
user))  
.))  !
MakeName))! )
,))) *
user**  
.**  !
	ModelName**! *
,*** +
user++  
.++  !

ReviewLink++! +
)+++ ,
;++, -
},, 
}-- 
}.. 
catch// 
{00 
Logs11 
.11 
WriteInfoLog11 !
(11! "
$str11" [
)11[ \
;11\ ]
}22 
}33 	
public99 
IEnumerable99 
<99 (
UserReviewsRatingEmailEntity99 7
>997 8
GetCustomerMailData999 L
(99L M
)99M N
{:: 	
IEnumerable;; 
<;; (
UserReviewsRatingEmailEntity;; 4
>;;4 5
customerEmailList;;6 G
=;;H I
null;;J N
;;;N O
try<< 
{== 
customerEmailList>> !
=>>" #
userRatingRepo>>$ 2
.>>2 3
GetUserList>>3 >
(>>> ?
)>>? @
;>>@ A
foreach?? 
(?? 
var?? 
user?? !
in??" $
customerEmailList??% 6
)??6 7
{@@ 
stringAA 
qEncodedAA #
=AA$ % 
GetEncryptedUrlTokenAA& :
(AA: ;
stringAA; A
.AAA B
FormatAAB H
(AAH I
$str	AAI Õ
,BB! "
userBB# '
.BB' (
ReviewIdBB( 0
,BB0 1
userBB2 6
.BB6 7
MakeIdBB7 =
,BB= >
userCC! %
.CC% &
ModelIdCC& -
,CC- .
userCC/ 3
.CC3 4
OverAllRatingCC4 A
,CCA B
userDD! %
.DD% &

CustomerIdDD& 0
,DD0 1
userDD2 6
.DD6 7
PriceRangeIdDD7 C
,DDC D
userEE! %
.EE% &
CustomerNameEE& 2
,EE2 3
userEE4 8
.EE8 9
CustomerEmailEE9 F
,EEF G
userFF! %
.FF% &
PageSourceIdFF& 2
,FF2 3
userFF4 8
.FF8 9
IsFakeFF9 ?
)FF? @
)FF@ A
;FFA B
stringGG 
	reviewUrlGG $
=GG% &
stringGG' -
.GG- .
FormatGG. 4
(GG4 5
$strGG5 P
,GGP Q
BWConfigurationGGR a
.GGa b
InstanceGGb j
.GGj k
	BwHostUrlGGk t
,GGt u
qEncodedGGv ~
)GG~ 
;	GG €
UrlShortnerResponseHH '
shortUrlHH( 0
=HH1 2
urlHH3 6
.HH6 7
GetShortUrlHH7 B
(HHB C
	reviewUrlHHC L
)HHL M
;HHM N
ifII 
(II 
shortUrlII  
!=II! #
nullII$ (
)II( )
userJJ 
.JJ 

ReviewLinkJJ '
=JJ( )
shortUrlJJ* 2
.JJ2 3
ShortUrlJJ3 ;
;JJ; <
elseKK 
userLL 
.LL 

ReviewLinkLL '
=LL( )
	reviewUrlLL* 3
;LL3 4
}MM 
}NN 
catchOO 
{PP 
LogsQQ 
.QQ 
WriteInfoLogQQ !
(QQ! "
$strQQ" Y
)QQY Z
;QQZ [
}RR 
returnSS 
customerEmailListSS $
;SS$ %
}TT 	
privateZZ 
stringZZ  
GetEncryptedUrlTokenZZ +
(ZZ+ ,
stringZZ, 2
valueZZ3 8
)ZZ8 9
{[[ 	
string]] 
token]] 
=]] 
string]] !
.]]! "
Empty]]" '
;]]' (
token__ 
=__ 
Utils__ 
.__ 
Utils__ 
.__  
EncryptTripleDES__  0
(__0 1
value__1 6
)__6 7
;__7 8
returnaa 
tokenaa 
;aa 
}bb 	
}ff 
}gg ½)
PD:\work\bikewaleweb\Bikewale.UserReviewsRatingEmail\UserReviewsRatingEmailDAL.cs
	namespace

 	
Bikewale


 
.

 "
UserReviewsRatingEmail

 )
{ 
public 

class %
UserReviewsRatingEmailDAL *
{ 
public 
IEnumerable 
< (
UserReviewsRatingEmailEntity 7
>7 8
GetUserList9 D
(D E
)E F
{ 	
Logs 
. 
WriteInfoLog 
( 
$str A
)A B
;B C
ICollection 
< (
UserReviewsRatingEmailEntity 4
>4 5
userList6 >
=? @
nullA E
;E F
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
CommandType #
=$ %
CommandType& 1
.1 2
StoredProcedure2 A
;A B
cmd 
. 
CommandText #
=$ %
$str& G
;G H
using!! 
(!! 
IDataReader!! &
dr!!' )
=!!* +
MySqlDatabase!!, 9
.!!9 :
SelectQuery!!: E
(!!E F
cmd!!F I
,!!I J
ConnectionType!!K Y
.!!Y Z
ReadOnly!!Z b
)!!b c
)!!c d
{"" 
if## 
(## 
dr## 
!=## !
null##" &
)##& '
{$$ 
userList%% $
=%%% &
new%%' *

Collection%%+ 5
<%%5 6(
UserReviewsRatingEmailEntity%%6 R
>%%R S
(%%S T
)%%T U
;%%U V
while&& !
(&&" #
dr&&# %
.&&% &
Read&&& *
(&&* +
)&&+ ,
)&&, -
{'' 
userList((  (
.((( )
Add(() ,
(((, -
new((- 0(
UserReviewsRatingEmailEntity((1 M
{))  !
ReviewId**$ ,
=**- .
SqlReaderConvertor**/ A
.**A B
ToUInt32**B J
(**J K
dr**K M
[**M N
$str**N X
]**X Y
)**Y Z
,**Z [
MakeName++$ ,
=++- .
Convert++/ 6
.++6 7
ToString++7 ?
(++? @
dr++@ B
[++B C
$str++C M
]++M N
)++N O
,++O P
	ModelName,,$ -
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
,,,Q R

CustomerId--$ .
=--/ 0
SqlReaderConvertor--1 C
.--C D
ToUInt32--D L
(--L M
dr--M O
[--O P
$str--P \
]--\ ]
)--] ^
,--^ _
CustomerName..$ 0
=..1 2
Convert..3 :
...: ;
ToString..; C
(..C D
dr..D F
[..F G
$str..G U
]..U V
)..V W
,..W X
CustomerEmail//$ 1
=//2 3
Convert//4 ;
.//; <
ToString//< D
(//D E
dr//E G
[//G H
$str//H W
]//W X
)//X Y
,//Y Z
MakeId00$ *
=00* +
SqlReaderConvertor00+ =
.00= >
ToUInt3200> F
(00F G
dr00G I
[00I J
$str00J R
]00R S
)00S T
,00T U
ModelId11$ +
=11+ ,
SqlReaderConvertor11, >
.11> ?
ToUInt3211? G
(11G H
dr11H J
[11J K
$str11K T
]11T U
)11U V
,11V W
OverAllRating22$ 1
=221 2
Convert222 9
.229 :
ToString22: B
(22B C
dr22C E
[22E F
$str22F U
]22U V
)22V W
,22W X
PriceRangeId33$ 0
=330 1
SqlReaderConvertor331 C
.33C D
ToUInt3233D L
(33L M
dr33M O
[33O P
$str33P ^
]33^ _
)33_ `
,33` a
PageSourceId44$ 0
=440 1
SqlReaderConvertor441 C
.44C D
ToUInt3244D L
(44L M
dr44M O
[44O P
$str44P Z
]44Z [
)44[ \
,44\ ]
IsFake55$ *
=55* +
SqlReaderConvertor55+ =
.55= >
	ToBoolean55> G
(55G H
dr55H J
[55J K
$str55K S
]55S T
)55T U
}66  !
)66! "
;66" #
}77 
dr88 
.88 
Close88 $
(88$ %
)88% &
;88& '
}99 
}:: 
};; 
}<< 
catch== 
{>> 
Logs?? 
.?? 
WriteInfoLog?? !
(??! "
$str??" J
)??J K
;??K L
}@@ 
LogsAA 
.AA 
WriteInfoLogAA 
(AA 
$strAA O
)AAO P
;AAP Q
returnBB 
userListBB 
;BB 
}CC 	
}EE 
}GG °
SD:\work\bikewaleweb\Bikewale.UserReviewsRatingEmail\UserReviewsRatingEmailEntity.cs
	namespace 	
Bikewale
 
. "
UserReviewsRatingEmail )
{ 
public		 

class		 (
UserReviewsRatingEmailEntity		 -
{

 
public 
uint 
ReviewId 
{ 
get "
;" #
set$ '
;' (
}) *
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
string 
	ModelName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
uint 

CustomerId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
CustomerName "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
CustomerEmail #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 

ReviewLink  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
uint 
MakeId 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
ModelId 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
OverAllRating #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
uint 
PriceRangeId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
uint 
PageSourceId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
bool 
IsFake 
{ 
get  
;  !
set" %
;% &
}' (
} 
} 