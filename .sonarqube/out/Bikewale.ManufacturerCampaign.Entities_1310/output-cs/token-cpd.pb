ê+
UD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ConfigureCampaignEntity.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public 

class #
ConfigureCampaignEntity (
{ 
public '
ManufacturerCampaignDetails *
DealerDetails+ 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public 
IEnumerable 
< %
ManufacturerCampaignPages 4
>4 5
CampaignPages6 C
{D E
getF I
;I J
setK N
;N O
}P Q
} 
public 

class 
PriorityEntity 
{ 
public 
uint 
Id 
{ 
get 
; 
set !
;! "
}# $
public 
string 
Status 
{ 
get "
;" #
set$ '
;' (
}) *
public 
Boolean 

IsSelected !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
public 

class "
CampaignPropertyEntity '
{ 
public 
uint 

CampaignId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
public &
CampaignLeadPropertyEntity )
Lead* .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public   %
CampaignEMIPropertyEntity   (
EMI  ) ,
{  - .
get  / 2
;  2 3
set  4 7
;  7 8
}  9 :
public!! 
IEnumerable!! 
<!! 
PriorityEntity!! )
>!!) *
EMIPriority!!+ 6
{!!7 8
get!!9 <
;!!< =
set!!> A
;!!A B
}!!C D
public"" 
IEnumerable"" 
<"" 
PriorityEntity"" )
>"") *
LeadPriority""+ 7
{""8 9
get"": =
;""= >
set""? B
;""B C
}""D E
public## "
NavigationWidgetEntity## %
NavigationWidget##& 6
{##7 8
get##9 <
;##< =
set##> A
;##A B
}##C D
}$$ 
public%% 

class%% %
CampaignEMIPropertyEntity%% *
{&& 
public'' 
bool'' 
HasEmiProperties'' $
{''% &
get''' *
;''* +
set'', /
;''/ 0
}''1 2
public(( 
string(( 
ButtonTextMobile(( &
{((' (
get(() ,
;((, -
set((. 1
;((1 2
}((3 4
public)) 
string)) 
PropertyTextMobile)) (
{))) *
get))+ .
;)). /
set))0 3
;))3 4
}))5 6
public** 
string** 
ButtonTextDesktop** '
{**( )
get*** -
;**- .
set**/ 2
;**2 3
}**4 5
public++ 
string++ 
PropertyTextDesktop++ )
{++* +
get++, /
;++/ 0
set++1 4
;++4 5
}++6 7
public,, 
string,, 
Priority,, 
{,,  
get,,! $
;,,$ %
set,,& )
;,,) *
},,+ ,
public-- 
bool-- 
EnableProperty-- "
{--# $
get--% (
;--( )
set--* -
;--- .
}--/ 0
}.. 
public00 

class00 &
CampaignLeadPropertyEntity00 +
{11 
public22 
bool22 
HasLeadProperties22 %
{22& '
get22( +
;22+ ,
set22- 0
;220 1
}222 3
public33 
string33 
ButtonTextMobile33 &
{33' (
get33) ,
;33, -
set33. 1
;331 2
}333 4
public44 
string44 
PropertyTextMobile44 (
{44) *
get44+ .
;44. /
set440 3
;443 4
}445 6
public55 
string55 
ButtonTextDesktop55 '
{55( )
get55* -
;55- .
set55/ 2
;552 3
}554 5
public66 
string66 
PropertyTextDesktop66 )
{66* +
get66, /
;66/ 0
set661 4
;664 5
}666 7
public77 
string77 

HtmlMobile77  
{77! "
get77# &
;77& '
set77( +
;77+ ,
}77- .
public88 
string88 
HtmlDesktop88 !
{88" #
get88$ '
;88' (
set88) ,
;88, -
}88. /
public99 
string99 
Priority99 
{99  
get99! $
;99$ %
set99& )
;99) *
}99+ ,
public:: 
bool:: 
EnableProperty:: "
{::# $
get::% (
;::( )
set::* -
;::- .
}::/ 0
};; 
}== ê
YD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerCampaignDetails.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public		 

class		 '
ManufacturerCampaignDetails		 ,
{

 
public 
uint 
Id 
{ 
get 
; 
set !
;! "
}# $
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
string 
MaskingNumber #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
DateTime 
CampaignStartDate )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
DateTime 
CampaignEndDate '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
int 
DailyLeadLimit !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
TotalLeadLimit !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
DailyLeadsDelivered &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
int 
TotalLeadsDelivered &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
CampaignStatus $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
bool $
ShowCampaignOnExshowroom ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
string 

DealerName  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
MobileNo 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} ⁄
bD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerCampaignEMIConfiguration.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public 

class 0
$ManufacturerCampaignEMIConfiguration 5
{ 
public		 
uint		 

CampaignId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
public

 
uint

 
DealerId

 
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
string 
Organization "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
PopupHeading "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
PopupDescription &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
PopupSuccessMessage )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
string 
MaskingNumber #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
EMIButtonTextMobile )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
string !
EMIPropertyTextMobile +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
string  
EMIButtonTextDesktop *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
string "
EMIPropertyTextDesktop ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
bool 
PincodeRequired #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
bool 
DealerRequired "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
bool 
EmailRequired !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} ﬁ
XD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerCampaignEntity.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public 

class &
ManufacturerCampaignEntity +
{ 
public		 1
%ManufacturerCampaignLeadConfiguration		 4
LeadCampaign		5 A
{		B C
get		D G
;		G H
set		I L
;		L M
}		N O
public

 0
$ManufacturerCampaignEMIConfiguration

 3
EMICampaign

4 ?
{

@ A
get

B E
;

E F
set

G J
;

J K
}

L M
} 
} Õ
cD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerCampaignLeadConfiguration.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public 

class 1
%ManufacturerCampaignLeadConfiguration 6
{ 
public		 
uint		 
DealerId		 
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
 

CampaignId

 
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
string 
Organization "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
PopupHeading "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
PopupDescription &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
PopupSuccessMessage )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
string 
MaskingNumber #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string !
LeadsButtonTextMobile +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
string #
LeadsPropertyTextMobile -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
string "
LeadsButtonTextDesktop ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
string $
LeadsPropertyTextDesktop .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
string 
LeadsHtmlMobile %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
LeadsHtmlDesktop &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
ShowOnExshowroom $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
bool 
PincodeRequired #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
bool 
DealerRequired "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
bool 
EmailRequired !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} Ø
WD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerCampaignPages.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public 

class %
ManufacturerCampaignPages *
{ 
public 
int 
PageId 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
PageName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
bool 

IsSelected 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} ‰
WD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerCampaignPopup.cs
	namespace 	
Bikewaleopr
 
.  
ManufacturerCampaign *
.* +
Entities+ 3
{ 
public		 	
class		
 %
ManufacturerCampaignPopup		 )
{

 
public 
uint 

CampaignId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
PopupHeading "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
PopupDescription &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
PopupSuccessMessage )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
bool 
EmailRequired !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
bool 
PinCodeRequired #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
bool 
DealerRequired "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} Í
YD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerCampaignPopupVM.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
.0 1
Models1 7
{		 
public

 

class

 '
ManufacturerCampaignPopupVM

 ,
{ 
public %
ManufacturerCampaignPopup (
objPopup) 1
{1 2
get2 5
;5 6
set6 9
;9 :
}: ;
public "
NavigationWidgetEntity %
NavigationWidget& 6
{7 8
get9 <
;< =
set> A
;A B
}C D
} 
} Ñ
^D:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerCampaignRulesWrapper.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public		 

class		 ,
 ManufacturerCampaignRulesWrapper		 1
{

 
public 
IEnumerable 
< "
ManufacturerRuleEntity 1
>1 2%
ManufacturerCampaignRules3 L
{M N
getO R
;R S
setT W
;W X
}Y Z
public 
bool 
ShowOnExShowroom $
{% &
get' *
;* +
set, /
;/ 0
}1 2
} 
} …
^D:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerCampaignServingPages.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public 

enum ,
 ManufacturerCampaignServingPages 0
{ 
Mobile_Model_Page		 
=		 
$num		 
,		 
Desktop_Model_Page

 
=

 
$num

 
,

 #
Mobile_DealerPriceQuote 
=  !
$num" #
,# $$
Desktop_DealerPriceQuote  
=! "
$num# $
,$ %
Mobile_PriceInCity 
= 
$num 
, 
Desktop_PriceInCity 
= 
$num 
} 
} ë
aD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\Models\SearchManufacturerCampaignVM.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
.0 1
Models1 7
{ 
public 

class (
SearchManufacturerCampaignVM -
{ 
public 
string 
UserId 
{ 
get "
;" #
set$ '
;' (
}) *
public		 
IEnumerable		 
<		 
ManufacturerEntity		 -
>		- .
ManufacturerList		/ ?
{		@ A
get		B E
;		E F
set		G J
;		J K
}		L M
}

 
} ¨
TD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\NavigationWidgetEntity.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public		 

class		 "
NavigationWidgetEntity		 '
{

 
public 
uint 

ActivePage 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
uint 

CampaignId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} ˛
LD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\BikeMakeEntity.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public 

class 
BikeMakeEntity 
{ 
public		 
uint		 
MakeId		 
{		 
get		  
;		  !
set		" %
;		% &
}		' (
public

 
string

 
MakeName
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
+ ,
} 
} Ç
MD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\BikeModelEntity.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public 

class 
BikeModelEntity  
{ 
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
 
string

 
	ModelName

 
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
} ˆ
HD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\CityEntity.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public 

class 

CityEntity 
{ 
public		 
uint		 
CityId		 
{		 
get		  
;		  !
set		" %
;		% &
}		' (
public

 
string

 
CityName
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
+ ,
} 
} É
]D:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerCampaignRulesEntity.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public		 

class		 +
ManufacturerCampaignRulesEntity		 0
{

 
public 
BikeMakeEntity 
Make "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
BikeModelEntity 
Model $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
IEnumerable 
< 
StateEntity &
>& '
State( -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
} 
} á
YD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerCampaignRulesVM.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public		 

class		 '
ManufacturerCampaignRulesVM		 ,
{

 
public 
uint 

CampaignId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
IEnumerable 
< +
ManufacturerCampaignRulesEntity :
>: ;
Rules< A
{B C
getD G
;G H
setI L
;L M
}N O
public 
IEnumerable 
< 
BikeMakeEntity )
>) *
Makes+ 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public 
IEnumerable 
< 
StateEntity &
>& '
States( .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public "
NavigationWidgetEntity %
NavigationWidget& 6
{7 8
get9 <
;< =
set> A
;A B
}C D
public 
bool 
ShowOnExShowroom $
{% &
get' *
;* +
set, /
;/ 0
}1 2
} 
} Æ
TD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\ManufacturerRuleEntity.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public 

class "
ManufacturerRuleEntity '
{ 
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
 
uint

 
MakeId
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
public 
uint 
StateId 
{ 
get !
;! "
set# &
;& '
}( )
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
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
MakeName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
	StateName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
CityName 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} Ä
UD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str A
)A B
]B C
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
$str C
)C D
]D E
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
]$$) *å
lD:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\SearchCampaign\ManufacturerCampaignDetailsList.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
.0 1
SearchCampaign1 ?
{ 
public		 	
class		
 +
ManufacturerCampaignDetailsList		 /
{

 
public 
uint 
Id 
{ 
get 
; 
set !
;! "
}# $
public 
string 
Description "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
MaskingNumber #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
CampaignStartDate '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
string 
CampaignEndDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 
DailyLeadLimit !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
TotalLeadLimit !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
DailyLeadsDelivered &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
int 
TotalLeadsDelivered &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
Status 
{ 
get  #
;# $
set% (
;( )
}* +
public 
bool $
ShowCampaignOnExshowroom ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
} 
} œ
ID:\work\bikewaleweb\Bikewale.ManufacturerCampaign.Entities\StateEntity.cs
	namespace 	
Bikewale
 
.  
ManufacturerCampaign '
.' (
Entities( 0
{ 
public		 

class		 
StateEntity		 
{

 
public 
uint 
StateId 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
	StateName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
IEnumerable 
< 

CityEntity %
>% &
Cities' -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
} 
} 