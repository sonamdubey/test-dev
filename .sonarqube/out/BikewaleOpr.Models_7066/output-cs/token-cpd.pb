¾
PD:\work\bikewaleweb\BikewaleOpr.Models\Comparison\ManageSponsoredComparisonVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 

Comparison '
{		 
public

 

class

 '
ManageSponsoredComparisonVM

 ,
{ 
public 
IEnumerable 
< 
SponsoredComparison .
>. / 
Sponsoredcomparisons0 D
{E F
getG J
;J K
setL O
;O P
}Q R
} 
} º
=D:\work\bikewaleweb\BikewaleOpr.Models\Banner\BannerListVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
Banner #
{		 
public

 

class

 
BannerListVM

 
{ 
public 
IEnumerable 
< 
BannerProperty )
>) *

BannerList+ 5
;5 6
} 
} û
9D:\work\bikewaleweb\BikewaleOpr.Models\Banner\BannerVM.cs
	namespace		 	
BikewaleOpr		
 
.		 
Models		 
{

 
public 
class 
BannerVM 
{ 
public 
BannerDetails  
DesktopBannerDetails 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
public 
BannerDetails 
MobileBannerDetails 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
[ 	
JsonProperty	 
( 
$str !
)! "
]" #
public 
DateTime 
	StartDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
JsonProperty	 
( 
$str 
)  
]  !
public 
DateTime 
EndDate 
{  !
get" %
;% &
set' *
;* +
}, -
[ 	
JsonProperty	 
( 
$str )
)) *
]* +
public 
string 
BannerDescription '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
[ 	
JsonProperty	 
( 
$str "
)" #
]# $
public 
uint 

CampaignId 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	
JsonProperty	 
( 
$str 
) 
]  
public 
string 
UserId 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} ß
VD:\work\bikewaleweb\BikewaleOpr.Models\ConfigurePageMetas\ConfigurePageMetaSearchVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
ConfigurePageMetas /
{		 
public 

class %
ConfigurePageMetaSearchVM *
{ 
public 
IEnumerable 
< 
PageMetaEntity )
>) *
PageMetaList+ 7
{8 9
get: =
;= >
set? B
;B C
}D E
public 
ushort 
PageMetaStatus $
{% &
get' *
;* +
set, /
;/ 0
}1 2
} 
} Ï

QD:\work\bikewaleweb\BikewaleOpr.Models\ConfigurePageMetas\ConfigurePageMetasVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
ConfigurePageMetas /
{ 
public 

class  
ConfigurePageMetasVM %
{		 
public

 
IEnumerable

 
<

 
BikeMakeEntityBase

 -
>

- .
Makes

/ 4
{

5 6
get

7 :
;

: ;
set

< ?
;

? @
}

A B
public 
string 
DesktopPages "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
MobilePages !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
CurrentUserId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
uint 

PageMetaId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
PageMetasEntity 
	PageMetas (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
} 
} Ô
SD:\work\bikewaleweb\BikewaleOpr.Models\DealerBookingAmount\DealerBookingAmountVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerBookingAmount 0
{ 
public 

class !
DealerBookingAmountVM &
{ 
public 
string 
	PageTitle 
{  !
get" %
;% &
set' *
;* +
}, -
public		 #
ManageBookingAmountData		 &#
DealerBookingAmountData		' >
{		? @
get		A D
;		D E
set		F I
;		I J
}		K L
public

 $
DealerOperationPricingVM

 '!
DealerOperationParams

( =
{

> ?
get

@ C
;

C D
set

E H
;

H I
}

J K
public 
string 

DealerName  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
MakeId 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} ¯
OD:\work\bikewaleweb\BikewaleOpr.Models\DealerCampaign\CampaignServingAreasVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerCampaign +
{ 
public 

class "
CampaignServingAreasVM '
{ 
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 

DealerName  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
uint 

CampaignId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
IEnumerable 
< 
CityArea #
># $
MappedAreas% 0
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
public 
IEnumerable 
< 
CityArea #
># $#
AdditionallyMappedAreas% <
{= >
get? B
;B C
setD G
;G H
}I J
public 
IEnumerable 
< 
City 
>  
Cities! '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
string 
AdditionalAreaJson (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
IEnumerable 
< 
StateEntityBase *
>* +
States, 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
} 
} È
?D:\work\bikewaleweb\BikewaleOpr.Models\DealerEMI\DealerEMIVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
	DealerEMI &
{ 
public

 

class

 
DealerEMIVM

 
{ 
public 
string 
	PageTitle 
{  !
get" %
;% &
set' *
;* +
}, -
public $
DealerOperationPricingVM '!
DealerOperationParams( =
{> ?
get@ C
;C D
setE H
;H I
}J K
public 
EMI 
dealerEmiFormInfo $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
string 

DealerName  
{! "
get# &
;& '
set( +
;+ ,
}- .
} 
} ˜
ED:\work\bikewaleweb\BikewaleOpr.Models\DealerPricing\AddCategoryVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerPricing *
{ 
public

 

class

 
AddCategoryVM

 
{ 
public 
IEnumerable 
< 
PQ_Price #
># $
PriceCategories% 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
} 
} ÷
ID:\work\bikewaleweb\BikewaleOpr.Models\DealerPricing\CityCopyPricingVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerPricing *
{ 
public 

class 
CityCopyPricingVM "
{ 
public 
IEnumerable 
< 
StateEntityBase *
>* +
States, 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
IEnumerable 
< 
CityNameEntity )
>) *
Cities+ 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
} 
} ¹
HD:\work\bikewaleweb\BikewaleOpr.Models\ManagePrices\PriceMonitoringVM.cs
	namespace

 	
BikewaleOpr


 
.

 
Models

 
.

 
ManagePrices

 )
{ 
public 

class 
PriceMonitoringVM "
{ 
public 
IEnumerable 
< 
Entities #
.# $
MfgCityEntity$ 1
>1 2
CityList3 ;
{< =
get> A
;A B
setC F
;F G
}H I
public 
IEnumerable 
< !
PriceLastUpdateEntity 0
>0 1 
PriceLastUpdatedList2 F
{G H
getI L
;L M
setN Q
;Q R
}S T
public 
IEnumerable 
< !
BikeVersionEntityBase 0
>0 1
BikeVersionList2 A
{B C
getD G
;G H
setI L
;L M
}N O
public 
IEnumerable 
< 
	ModelBase $
>$ %
BikeModelList& 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
	BikeMakes/ 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public 
IEnumerable 
< 
Entities #
.# $
StateEntityBase$ 3
>3 4
States5 ;
{< =
get> A
;A B
setC F
;F G
}H I
public 
uint 
StateId 
{ 
get !
;! "
set# &
;& '
}( )
public 
uint 
MakeId 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
ModelId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	PageTitle 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} ¡
OD:\work\bikewaleweb\BikewaleOpr.Models\DealerFacility\ManageDealerFacilityVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerFacility +
{ 
public 

class "
ManageDealerFacilityVM '
{ 
public 
string 
	PageTitle 
{  !
get" %
;% &
set' *
;* +
}, -
public 
IEnumerable 
< 
FacilityEntity )
>) *
FacilityList+ 7
{8 9
get: =
;= >
set? B
;B C
}D E
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
public $
DealerOperationPricingVM '!
DealerOperationParams( =
{> ?
get@ C
;C D
setE H
;H I
}J K
} 
} ý
KD:\work\bikewaleweb\BikewaleOpr.Models\DealerPricing\DealerCopyPricingVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerPricing *
{ 
public 

class 
DealerCopyPricingVM $
{ 
public 
IEnumerable 
< 
CityNameEntity )
>) *
Cities+ 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
public 
IEnumerable 
< 
DealerMakeEntity +
>+ ,
Dealers- 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
} 
} Â
PD:\work\bikewaleweb\BikewaleOpr.Models\DealerPricing\DealerOperationPricingVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerPricing *
{ 
public 

class $
DealerOperationPricingVM )
:* +
DealerOperationVM, =
{ 
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
Makes/ 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public 
IEnumerable 
< 
DealerEntityBase +
>+ ,
Dealers- 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
MakeId 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
DealerId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
MakesString !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
DealersString #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} 
} »
ID:\work\bikewaleweb\BikewaleOpr.Models\DealerPricing\DealerOperationVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerPricing *
{ 
public

 

class

 
DealerOperationVM

 "
{ 
public 
IEnumerable 
< 
CityNameEntity )
>) *
DealerCities+ 7
{8 9
get: =
;= >
set? B
;B C
}D E
public 
bool 
IsOpen 
{ 
get  
;  !
set" %
;% &
}' (
} 
} ¸
JD:\work\bikewaleweb\BikewaleOpr.Models\DealerPricing\DealerPriceSheetVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerPricing *
{ 
public

 

class

 
DealerPriceSheetVM

 #
{ 
public 
IEnumerable 
< $
DealerVersionPriceEntity 3
>3 4!
dealerVersionPricings5 J
{K L
getM P
;P Q
setR U
;U V
}W X
} 
} ã

PD:\work\bikewaleweb\BikewaleOpr.Models\DealerPricing\DealerPricingIndexPageVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerPricing *
{ 
public 

class $
DealerPricingIndexPageVM )
{ 
public		 
string		 
	PageTitle		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		, -
public

 $
DealerOperationPricingVM

 '!
DealerOperationParams

( =
{

> ?
get

@ C
;

C D
set

E H
;

H I
}

J K
public 
ShowPricingVM 
ShowPricingCities .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
AddCategoryVM 
AddCategoryType ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
CityCopyPricingVM  
CopyPricingCities! 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
DealerCopyPricingVM "
CopyPricingDealers# 5
{6 7
get8 ;
;; <
set= @
;@ A
}B C
} 
} Ñ
PD:\work\bikewaleweb\BikewaleOpr.Models\DealerPricing\DealerPricingSheetPageVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerPricing *
{ 
public

 

class

 $
DealerPricingSheetPageVM

 )
{ 
public 
string 
	PageTitle 
{  !
get" %
;% &
set' *
;* +
}, -
public 
uint 
OtherCityId 
{  !
get" %
;% &
set' *
;* +
}, -
public 
uint 
CityId 
{ 
get  
;  !
set" %
;% &
}' (
public 
uint 
CurrentCityId !
{" #
get$ '
{( )
return* 0
(1 2
OtherCityId2 =
>> ?
$num@ A
?B C
OtherCityIdD O
:P Q
CityIdR X
)X Y
;Y Z
}[ \
}] ^
public 
uint 
	EnteredBy 
{ 
get  #
;# $
set% (
;( )
}* +
public 
IEnumerable 
< 
VersionPriceEntity -
>- .#
DealerVersionCategories/ F
{G H
getI L
;L M
setN Q
;Q R
}S T
public 
string $
SelectedCategoriesString .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
DealerPriceSheetVM !
DealerPriceSheet" 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public $
DealerOperationPricingVM '!
DealerOperationParams( =
{> ?
get@ C
;C D
setE H
;H I
}J K
public 
ShowPricingVM 
ShowPricingCities .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
AddCategoryVM 
AddCategoryType ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
CityCopyPricingVM  
CopyPricingCities! 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
DealerCopyPricingVM "
CopyPricingDealers# 5
{6 7
get8 ;
;; <
set= @
;@ A
}B C
} 
} •
ED:\work\bikewaleweb\BikewaleOpr.Models\DealerPricing\ShowPricingVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
DealerPricing *
{ 
public

 

class

 
ShowPricingVM

 
{ 
public 
IEnumerable 
< 
CityNameEntity )
>) *
Cities+ 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
} 
} ¨
SD:\work\bikewaleweb\BikewaleOpr.Models\ManufacturerCampaign\CampaignPropertiesVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
.  
ManufacturerCampaign 1
{		 
public 

class  
CampaignPropertiesVM %
{ 
public 
uint 

CampaignId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
bool 
HasEmiProperties $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
string 
EmiButtonTextMobile )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
string !
EmiPropertyTextMobile +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
string  
EmiButtonTextDesktop *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
string "
EmiPropertyTextDesktop ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
string 
EmiPriority !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
bool 
HasLeadProperties %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string  
LeadButtonTextMobile *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
string "
LeadPropertyTextMobile ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
string !
LeadButtonTextDesktop +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
string #
LeadPropertyTextDesktop -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
[ 	
	AllowHtml	 
] 
public 
string 
LeadHtmlMobile $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
	AllowHtml	 
] 
public   
string   
LeadHtmlDesktop   %
{  & '
get  ( +
;  + ,
set  - 0
;  0 1
}  2 3
[!! 	
	AllowHtml!!	 
]!! 
public"" 
string""  
FormattedHtmlDesktop"" *
{""+ ,
get""- 0
;""0 1
set""2 5
;""5 6
}""7 8
[## 	
	AllowHtml##	 
]## 
public$$ 
string$$ 
FormattedHtmlMobile$$ )
{$$* +
get$$, /
;$$/ 0
set$$1 4
;$$4 5
}$$6 7
public%% 
string%% 
LeadPriority%% "
{%%# $
get%%% (
;%%( )
set%%* -
;%%- .
}%%/ 0
}&& 
}'' à
cD:\work\bikewaleweb\BikewaleOpr.Models\ManufacturerCampaign\ManufacturerCampaignInformationModel.cs
	namespace		 	
BikewaleOpr		
 
.		 
Models		 
.		  
ManufacturerCampaign		 1
{

 
public 

class 0
$ManufacturerCampaignInformationModel 5
{ 
public #
ConfigureCampaignEntity &
CampaignInformation' :
{; <
get< ?
;? @
setA D
;D E
}E F
public 
uint 

CampaignId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
IEnumerable 
< 
MaskingNumber (
>( )
MaskingNumbers* 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
public "
NavigationWidgetEntity %
NavigationWidget& 6
{7 8
get9 <
;< =
set> A
;A B
}C D
} 
} ì
AD:\work\bikewaleweb\BikewaleOpr.Models\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str -
)- .
]. /
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
$str /
)/ 0
]0 1
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
]$$) *ç
UD:\work\bikewaleweb\BikewaleOpr.Models\Comparison\ManageSponsoredComparisonRulesVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 

Comparison '
{		 
public

 

class

 ,
 ManageSponsoredComparisonRulesVM

 1
{ 
public 
uint 
ComparisonId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
IEnumerable 
< 
SponsoredVersion +
>+ ,$
ComparisonVersionMapping- E
{F G
getG J
;J K
setK N
;N O
}O P
} 
} ¤
SD:\work\bikewaleweb\BikewaleOpr.Models\ServiceCenter\ServiceCenterCompleteDataVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
ServiceCenter *
{		 
public

 

class

 '
ServiceCenterCompleteDataVM

 ,
{ 
public %
ServiceCenterCompleteData '
details( /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
public 
string 
MakeName 
{ 
get  #
;# $
set% (
;( )
}* +
public '
ServiceCenterCompleteDataVM *
(* +
)+ ,
{ 	
details 
= 
new %
ServiceCenterCompleteData 3
(3 4
)4 5
;5 6
} 	
} 
} õ
KD:\work\bikewaleweb\BikewaleOpr.Models\ServiceCenter\ServiceCenterPageVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
ServiceCenter *
{		 
public 

class 
ServiceCenterPageVM $
{ 
public 
IEnumerable 
< 
Entities #
.# $
BikeData$ ,
., -
BikeMakeEntityBase- ?
>? @
MakesA F
{G H
getI L
;L M
setN Q
;Q R
}S T
public 
IEnumerable 
< 
Entities #
.# $
BikeData$ ,
., -
BikeMakeEntityBase- ?
>? @
AllMakesA I
{J K
getL O
;O P
setQ T
;T U
}V W
public 
IEnumerable 
< 
CityEntityBase )
>) *
AllCityList+ 6
{7 8
get9 <
;< =
set> A
;A B
}C D
} 
} Ì
MD:\work\bikewaleweb\BikewaleOpr.Models\UserReviews\ManageUserReviewsPageVM.cs
	namespace 	
BikewaleOpr
 
. 
Models 
. 
UserReviews (
{ 
public 

class #
ManageUserReviewsPageVM (
{ 
public 
IEnumerable 
< 
BikeMakeEntityBase -
>- .
Makes/ 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public 
IEnumerable 
< 

ReviewBase %
>% &
Reviews' .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
ReviewsInputFilters "
selectedFilters# 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
int 
currentUserId  
{! "
get# &
;& '
set( +
;+ ,
}- .
} 
} æ
>D:\work\bikewaleweb\BikewaleOpr.Models\Users\LoginViewModel.cs
	namespace 	
BikeWaleOpr
 
. 
Models 
. 
Users "
{ 
public 

class 
LoginViewModel 
{		 
public

 
string

 
Username
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
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} 