�
ND:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\BikeData\BikeDataMapper.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
AutoMappers )
.) *
BikeData* 2
{ 
public 

class 
BikeDataMapper 
{ 
internal 
static 
SynopsisDataDto '
Convert( /
(/ 0
SynopsisData0 <
objSynopsis= H
)H I
{ 	
Mapper 
. 
	CreateMap 
< 
SynopsisData )
,) *
SynopsisDataDto+ :
>: ;
(; <
)< =
;= >
return 
Mapper 
. 
Map 
< 
SynopsisData *
,* +
SynopsisDataDto, ;
>; <
(< =
objSynopsis= H
)H I
;I J
} 	
internal 
static 
SynopsisData $
Convert% ,
(, -
SynopsisDataDto- <
objSynopsis= H
)H I
{   	
Mapper!! 
.!! 
	CreateMap!! 
<!! 
SynopsisDataDto!! ,
,!!, -
SynopsisData!!. :
>!!: ;
(!!; <
)!!< =
;!!= >
return"" 
Mapper"" 
."" 
Map"" 
<"" 
SynopsisDataDto"" -
,""- .
SynopsisData""/ ;
>""; <
(""< =
objSynopsis""= H
)""H I
;""I J
}## 	
}$$ 
}%% �
PD:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\BikeData\BikeModelsMapper.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
AutoMappers )
.) *
BikeData* 2
{ 
public 

class 
BikeModelsMapper !
{ 
internal 
static 
IEnumerable #
<# $
	ModelBase$ -
>- .
Convert/ 6
(6 7
IEnumerable7 B
<B C
BikeModelEntityBaseC V
>V W
	objModelsX a
)a b
{ 	
Mapper 
. 
	CreateMap 
< 
BikeModelEntityBase 0
,0 1
	ModelBase2 ;
>; <
(< =
)= >
;> ?
return 
Mapper 
. 
Map 
< 
IEnumerable )
<) *
BikeModelEntityBase* =
>= >
,> ?
IEnumerable@ K
<K L
	ModelBaseL U
>U V
>V W
(W X
	objModelsX a
)a b
;b c
} 	
internal 
static 
IEnumerable #
<# $
BikeModelDTO$ 0
>0 1
	ConvertV22 ;
(; <
IEnumerable< G
<G H
BikeModelEntityH W
>W X
	objModelsY b
)b c
{ 	
Mapper 
. 
	CreateMap 
< 
BikeModelEntity ,
,, -
BikeModelDTO. :
>; <
(< =
)= >
;> ?
return 
Mapper 
. 
Map 
< 
IEnumerable )
<) *
BikeModelEntity* 9
>9 :
,: ;
IEnumerable< G
<G H
BikeModelDTOH T
>T U
>U V
(V W
	objModelsW `
)` a
;a b
} 	
} 
} �	
RD:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\BikeData\BikeVersionsMapper.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
AutoMappers )
.) *
BikeData* 2
{ 
public 

class 
BikeVersionsMapper #
{ 
internal 
static 
IEnumerable #
<# $
VersionBase$ /
>/ 0
Convert1 8
(8 9
IEnumerable9 D
<D E!
BikeVersionEntityBaseE Z
>Z [
objVersions\ g
)g h
{ 	
Mapper 
. 
	CreateMap 
< !
BikeVersionEntityBase 2
,2 3
VersionBase4 ?
>? @
(@ A
)A B
;B C
return 
Mapper 
. 
Map 
< 
IEnumerable )
<) *!
BikeVersionEntityBase* ?
>? @
,@ A
IEnumerableB M
<M N
VersionBaseN Y
>Y Z
>Z [
([ \
objVersions\ g
)g h
;h i
} 	
} 
} �
UD:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\Comparison\SponsoredComparison.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
AutoMappers )
{ 
public 

class %
SponsoredComparisonMapper *
{ 
internal 
static 
IEnumerable #
<# $"
SponsoredComparisonDTO$ :
>: ;
Convert< C
(C D
IEnumerableD O
<O P
SponsoredComparisonP c
>c d
objSponsorede q
)q r
{ 	
Mapper 
. 
	CreateMap 
< 
SponsoredComparison 0
,0 1"
SponsoredComparisonDTO2 H
>H I
(I J
)J K
;K L
return 
Mapper 
. 
Map 
< 
IEnumerable )
<) *
SponsoredComparison* =
>= >
,> ?
IEnumerable@ K
<K L"
SponsoredComparisonDTOL b
>b c
>c d
(d e
objSponsorede q
)q r
;r s
} 	
internal 
static %
TargetSponsoredMappingDTO 1
Convert2 9
(9 :"
TargetSponsoredMapping: P
objTargetSponsoredQ c
)c d
{ 	
Mapper 
. 
	CreateMap 
< 
	BikeModel &
,& '
BikeModelDTO( 4
>4 5
(5 6
)6 7
;7 8
Mapper 
. 
	CreateMap 
< #
BikeModelVersionMapping 4
,4 5&
BikeModelVersionMappingDTO6 P
>P Q
(Q R
)R S
;S T
Mapper 
. 
	CreateMap 
< "
TargetSponsoredMapping 3
,3 4%
TargetSponsoredMappingDTO5 N
>N O
(O P
)P Q
;Q R
return 
Mapper 
. 
Map 
< "
TargetSponsoredMapping 4
,4 5%
TargetSponsoredMappingDTO6 O
>O P
(P Q
objTargetSponsoredQ c
)c d
;d e
}   	
}!! 
}"" �

RD:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\Dealer\DealerCampaignMapper.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
AutoMappers )
.) *
Dealer* 0
{ 
public 

class  
DealerCampaignMapper %
{		 
internal 
static 
IEnumerable #
<# $
DTO$ '
.' (
Make( ,
., -
MakeBase- 5
>5 6
Convert7 >
(> ?
IEnumerable? J
<J K
EntitiesK S
.S T
BikeMakeEntityBaseT f
>f g
makesh m
)m n
{ 	
Mapper 
. 
	CreateMap 
< 
BikeMakeEntityBase /
,/ 0
MakeBase1 9
>9 :
(: ;
); <
;< =
return 
Mapper 
. 
Map 
< 
IEnumerable )
<) *
BikeMakeEntityBase* <
>< =
,= >
IEnumerable? J
<J K
MakeBaseK S
>S T
>T U
(U V
makesV [
)[ \
;\ ]
} 	
} 
} �
RD:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\Dealer\DealerFacilityMapper.cs
	namespace		 	
BikewaleOpr		
 
.		 
Service		 
.		 
AutoMappers		 )
.		) *
Dealer		* 0
{

 
public 

class  
DealerFacilityMapper %
{ 
internal 
static 
FacilityEntity &
Convert' .
(. /
DealerFacilityDTO/ @
objMakesA I
)I J
{ 	
Mapper 
. 
	CreateMap 
< 
DealerFacilityDTO .
,. /
FacilityEntity0 >
>> ?
(? @
)@ A
;A B
return 
Mapper 
. 
Map 
< 
DealerFacilityDTO /
,/ 0
FacilityEntity1 ?
>? @
(@ A
objMakesA I
)I J
;J K
} 	
} 
} � 
ND:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\Dealer\DealerListMapper.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
AutoMappers )
.) *
Dealer* 0
{		 
public 

class 
DealerListMapper !
{ 
internal 
static 
IEnumerable #
<# $
BikeMakeBase$ 0
>0 1
Convert2 9
(9 :
IEnumerable: E
<E F
BikeMakeEntityBaseF X
>X Y
objMakesZ b
)b c
{ 	
Mapper 
. 
	CreateMap 
< 
BikeMakeEntityBase /
,/ 0
BikeMakeBase1 =
>= >
(> ?
)? @
;@ A
return 
Mapper 
. 
Map 
< 
IEnumerable )
<) *
BikeMakeEntityBase* <
>< =
,= >
IEnumerable? J
<J K
BikeMakeBaseK W
>W X
>X Y
(Y Z
objMakesZ b
)b c
;c d
} 	
internal"" 
static"" 
IEnumerable"" #
<""# $

DealerBase""$ .
>"". /
Convert""0 7
(""7 8
IEnumerable""8 C
<""C D
DealerEntityBase""D T
>""T U

objDealers""V `
)""` a
{## 	
Mapper$$ 
.$$ 
	CreateMap$$ 
<$$ 
DealerEntityBase$$ -
,$$- .

DealerBase$$/ 9
>$$9 :
($$: ;
)$$; <
;$$< =
return%% 
Mapper%% 
.%% 
Map%% 
<%% 
IEnumerable%% )
<%%) *
DealerEntityBase%%* :
>%%: ;
,%%; <
IEnumerable%%= H
<%%H I

DealerBase%%I S
>%%S T
>%%T U
(%%U V

objDealers%%V `
)%%` a
;%%a b
}&& 	
internal-- 
static-- 
IEnumerable-- #
<--# $!
DealerVersionPriceDTO--$ 9
>--9 :
Convert--; B
(--B C
IEnumerable--C N
<--N O$
DealerVersionPriceEntity--O g
>--g h
dealerVersionPrices--i |
)--| }
{.. 	
Mapper// 
.// 
	CreateMap// 
<// 
VersionPriceEntity// /
,/// 0
VersionPriceDTO//1 @
>//@ A
(//A B
)//B C
;//C D
Mapper00 
.00 
	CreateMap00 
<00 $
DealerVersionPriceEntity00 5
,005 6!
DealerVersionPriceDTO007 L
>00L M
(00M N
)00N O
;00O P
return11 
Mapper11 
.11 
Map11 
<11 
IEnumerable11 )
<11) *$
DealerVersionPriceEntity11* B
>11B C
,11C D
IEnumerable11E P
<11P Q!
DealerVersionPriceDTO11Q f
>11f g
>11g h
(11h i
dealerVersionPrices11i |
)11| }
;11} ~
}22 	
internal99 
static99 
IEnumerable99 #
<99# $
DealerMakeDTO99$ 1
>991 2
Convert993 :
(99: ;
IEnumerable99; F
<99F G
DealerMakeEntity99G W
>99W X
dealerMakeEntities99Y k
)99k l
{:: 	
Mapper;; 
.;; 
	CreateMap;; 
<;; 
DealerMakeEntity;; -
,;;- .
DealerMakeDTO;;/ <
>;;< =
(;;= >
);;> ?
;;;? @
return<< 
Mapper<< 
.<< 
Map<< 
<<< 
IEnumerable<< )
<<<) *
DealerMakeEntity<<* :
><<: ;
,<<; <
IEnumerable<<= H
<<<H I
DealerMakeDTO<<I V
><<V W
><<W X
(<<X Y
dealerMakeEntities<<Y k
)<<k l
;<<l m
}== 	
}>> 
}?? �
]D:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\Dealer\SaveDealerPricingResponseMapper.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
AutoMappers )
.) *
Dealer* 0
{ 
public 

class +
SaveDealerPricingResponseMapper 0
{ 
internal 
static (
SaveDealerPricingResponseDTO 4
Convert5 <
(< =,
 UpdatePricingRulesResponseEntity= ]
savePricesResponse^ p
,p q
boolr v 
isAvailabilitySaved	w �
)
� �
{ 	
Mapper 
. 
	CreateMap 
< ,
 UpdatePricingRulesResponseEntity =
,= >(
SaveDealerPricingResponseDTO? [
>[ \
(\ ]
)] ^
.^ _
	ForMember_ h
(h i
desti m
=>n p
destq u
.u v
IsPriceSaved	v �
,
� �
opt
� �
=>
� �
opt
� �
.
� �
MapFrom
� �
(
� �
s
� �
=>
� �
s
� �
.
� �
IsPriceSaved
� �
)
� �
)
� �
;
� �
Mapper 
. 
	CreateMap 
< ,
 UpdatePricingRulesResponseEntity =
,= >(
SaveDealerPricingResponseDTO? [
>[ \
(\ ]
)] ^
.^ _
	ForMember_ h
(h i
desti m
=>n p
destq u
.u v#
RulesUpdatedModelNames	v �
,
� �
opt
� �
=>
� �
opt
� �
.
� �
MapFrom
� �
(
� �
s
� �
=>
� �
s
� �
.
� �$
RulesUpdatedModelNames
� �
)
� �
)
� �
;
� �
var 
apiResponseData 
=  !
Mapper" (
.( )
Map) ,
<, -,
 UpdatePricingRulesResponseEntity- M
,M N(
SaveDealerPricingResponseDTOO k
>k l
(l m
savePricesResponsem 
)	 �
;
� �
apiResponseData 
. 
IsAvailabilitySaved /
=0 1
isAvailabilitySaved2 E
;E F
return 
apiResponseData "
;" #
} 	
} 
} �
ID:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\Images\ImageMapper.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
AutoMappers )
.) *
Images* 0
{ 
public 

class 
ImageMapper 
{ 
internal 
static 
Entities  
.  !
Images! '
.' (
Image( -
Convert. 5
(5 6
ImageDTO6 >
objImage? G
)G H
{ 	

AutoMapper 
. 
Mapper 
. 
	CreateMap '
<' (
ImageDTO( 0
,0 1
Entities2 :
.: ;
Images; A
.A B
ImageB G
>G H
(H I
)I J
;J K
return 

AutoMapper 
. 
Mapper $
.$ %
Map% (
<( )
ImageDTO) 1
,1 2
Entities3 ;
.; <
Images< B
.B C
ImageC H
>H I
(I J
objImageJ R
)R S
;S T
} 	
internal 
static 
ImageTokenDTO %
Convert& -
(- .
Entities. 6
.6 7
Images7 =
.= >

ImageToken> H
tokenI N
)N O
{ 	

AutoMapper 
. 
Mapper 
. 
	CreateMap '
<' (
Entities( 0
.0 1
Images1 7
.7 8

ImageToken8 B
,B C
ImageTokenDTOD Q
>Q R
(R S
)S T
;T U

AutoMapper 
. 
Mapper 
. 
	CreateMap '
<' (
Entities( 0
.0 1
AWS1 4
.4 5
Token5 :
,: ;
Token< A
>A B
(B C
)C D
;D E
return 

AutoMapper 
. 
Mapper $
.$ %
Map% (
<( )
Entities) 1
.1 2
Images2 8
.8 9

ImageToken9 C
,C D
ImageTokenDTOE R
>R S
(S T
tokenT Y
)Y Z
;Z [
} 	
internal 
static 
Entities  
.  !
Images! '
.' (

ImageToken( 2
Convert3 :
(: ;
ImageTokenDTO; H
dtoI L
)L M
{ 	

AutoMapper 
. 
Mapper 
. 
	CreateMap '
<' (
ImageTokenDTO( 5
,5 6
Entities7 ?
.? @
Images@ F
.F G

ImageTokenG Q
>Q R
(R S
)S T
;T U

AutoMapper 
. 
Mapper 
. 
	CreateMap '
<' (
Token( -
,- .
Entities/ 7
.7 8
AWS8 ;
.; <
Token< A
>A B
(B C
)C D
;D E
return 

AutoMapper 
. 
Mapper $
.$ %
Map% (
<( )
ImageTokenDTO) 6
,6 7
Entities8 @
.@ A
ImagesA G
.G H

ImageTokenH R
>R S
(S T
dtoT W
)W X
;X Y
} 	
}   
}!! �

XD:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\ManufacturerCampaign\SearchMapper.cs
	namespace

 	
BikewaleOpr


 
.

 
Service

 
.

 
AutoMappers

 )
.

) * 
ManufacturerCampaign

* >
{ 
public 

class 
SearchMapper 
{ 
internal 
static 
IEnumerable #
<# $*
ManufacturerCampaignDetailsDTO$ B
>B C
ConvertD K
(K L
IEnumerableL W
<W X+
ManufacturerCampaignDetailsListX w
>w x
_objMfgList	y �
)
� �
{ 	
Mapper 
. 
	CreateMap 
< +
ManufacturerCampaignDetailsList <
,< =*
ManufacturerCampaignDetailsDTO> \
>\ ]
(] ^
)^ _
;_ `
return 
Mapper 
. 
Map 
< 
IEnumerable )
<) *+
ManufacturerCampaignDetailsList* I
>I J
,J K
IEnumerableL W
<W X*
ManufacturerCampaignDetailsDTOX v
>v w
>w x
(x y
_objMfgList	y �
)
� �
;
� �
} 	
} 
} �
JD:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\Location\CityMapper.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
AutoMappers )
.) *
Location* 2
{ 
public 

class 

CityMapper 
{ 
internal 
static 
IEnumerable #
<# $
CityDTO$ +
>+ ,
Convert- 4
(4 5
IEnumerable5 @
<@ A

CityEntityA K
>K L
	objModelsM V
)V W
{ 	
Mapper 
. 
	CreateMap 
< 

CityEntity '
,' (
CityDTO) 0
>0 1
(1 2
)2 3
;3 4
return 
Mapper 
. 
Map 
< 
IEnumerable )
<) *

CityEntity* 4
>4 5
,5 6
IEnumerable7 B
<B C
CityDTOC J
>J K
>K L
(L M
	objModelsM V
)V W
;W X
} 	
internal   
static   
IEnumerable   #
<  # $
CityNameDTO  $ /
>  / 0
Convert  1 8
(  8 9
IEnumerable  9 D
<  D E
CityNameEntity  E S
>  S T
	objModels  U ^
)  ^ _
{!! 	
Mapper"" 
."" 
	CreateMap"" 
<"" 
CityNameEntity"" +
,""+ ,
CityNameDTO""- 8
>""8 9
(""9 :
)"": ;
;""; <
return## 
Mapper## 
.## 
Map## 
<## 
IEnumerable## )
<##) *
CityNameEntity##* 8
>##8 9
,##9 :
IEnumerable##; F
<##F G
CityNameDTO##G R
>##R S
>##S T
(##T U
	objModels##U ^
)##^ _
;##_ `
}$$ 	
}%% 
}&& �
XD:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\ServiceCenter\ServiceCenterMapper.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
AutoMappers )
.) *
ServiceCenter* 7
{ 
public 

class 
ServiceCenterMapper $
{ 
internal 
static 
IEnumerable #
<# $
CityBase$ ,
>, -
Convert. 5
(5 6
IEnumerable6 A
<A B
CityEntityBaseB P
>P Q
objCityListR ]
)] ^
{ 	
Mapper 
. 
	CreateMap 
< 
CityEntityBase +
,+ ,
CityBase- 5
>5 6
(6 7
)7 8
;8 9
return 
Mapper 
. 
Map 
< 
IEnumerable )
<) *
CityEntityBase* 8
>8 9
,9 :
IEnumerable; F
<F G
CityBaseG O
>O P
>P Q
(Q R
objCityListR ]
)] ^
;^ _
} 	
internal%% 
static%% 
IEnumerable%% #
<%%# $ 
ServiceCenterBaseDTO%%$ 8
>%%8 9
Convert%%: A
(%%A B
IEnumerable%%B M
<%%M N 
ServiceCenterDetails%%N b
>%%b c
serviceCenterList%%d u
)%%u v
{&& 	
Mapper'' 
.'' 
	CreateMap'' 
<''  
ServiceCenterDetails'' 1
,''1 2 
ServiceCenterBaseDTO''3 G
>''G H
(''H I
)''I J
;''J K
return(( 
Mapper(( 
.(( 
Map(( 
<(( 
IEnumerable(( )
<(() * 
ServiceCenterDetails((* >
>((> ?
,((? @
IEnumerable((A L
<((L M 
ServiceCenterBaseDTO((M a
>((a b
>((b c
(((c d
serviceCenterList((d u
)((u v
;((v w
}** 	
},, 
}-- �
TD:\work\bikewaleweb\BikewaleOpr.Service\AutoMappers\UserReviews\UserReviewsMapper.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
AutoMappers )
.) *
UserReviews* 5
{		 
public

 

class

 
UserReviewsMapper

 "
{ 
internal 
static  
UserReviewSummaryDto ,
Convert- 4
(4 5
UserReviewSummary5 F
objUserReviewG T
)T U
{ 	
Mapper 
. 
	CreateMap 
< 
BikeModelEntityBase 0
,0 1
	ModelBase2 ;
>; <
(< =
)= >
;> ?
Mapper 
. 
	CreateMap 
< 
BikeMakeEntityBase /
,/ 0
MakeBase1 9
>9 :
(: ;
); <
;< =
Mapper 
. 
	CreateMap 
< 
UserReviewRating -
,- .
UserReviewRatingDto/ B
>B C
(C D
)D E
;E F
Mapper 
. 
	CreateMap 
< 
UserReviewSummary .
,. / 
UserReviewSummaryDto0 D
>D E
(E F
)F G
;G H
Mapper 
. 
	CreateMap 
< 
UserReviewQuestion /
,/ 0!
UserReviewQuestionDto1 F
>F G
(G H
)H I
;I J
Mapper 
. 
	CreateMap 
< #
UserReviewOverallRating 4
,4 5&
UserReviewOverallRatingDto6 P
>P Q
(Q R
)R S
;S T
return 
Mapper 
. 
Map 
< 
UserReviewSummary /
,/ 0 
UserReviewSummaryDto1 E
>E F
(F G
objUserReviewG T
)T U
;U V
} 	
} 
} �-
ND:\work\bikewaleweb\BikewaleOpr.Service\Controllers\Banner\BannerController.cs
	namespace		 	
BikewaleOpr		
 
.		 
Service		 
.		 
Controllers		 )
{

 
public 

class 
BannerController !
:" #
ApiController$ 1
{ 
private 
readonly 
IBannerRepository *!
_objBannerRespository+ @
=A B
nullC G
;G H
public 
BannerController 
(  
IBannerRepository  1 
objBannerRespository2 F
)F G
{ 	!
_objBannerRespository !
=" # 
objBannerRespository$ 8
;8 9
} 	
[ 	
HttpPost	 
, 
Route 
( 
$str 0
)0 1
]1 2
public 
IHttpActionResult  "
SaveBannerBasicDetails! 7
(7 8
[8 9
FromBody9 A
]A B
BannerVMC K
	objBannerL U
)U V
{ 	
uint 

campaignid 
= 
$num 
;  
try   
{!! 

campaignid"" 
="" !
_objBannerRespository"" 2
.""2 3"
SaveBannerBasicDetails""3 I
(""I J
	objBanner""J S
)""S T
;""T U
return## 
Ok## 
(## 

campaignid## $
)##$ %
;##% &
}$$ 
catch%% 
(%% 
	Exception%% 
ex%% 
)%%  
{&& 

ErrorClass(( 
objErr(( !
=((" #
new(($ '

ErrorClass((( 2
(((2 3
ex((3 5
,((5 6
$str((7 o
)((o p
;((p q
return** 
InternalServerError** *
(*** +
)**+ ,
;**, -
}++ 
},, 	
[.. 	
HttpPost..	 
,.. 
Route.. 
(.. 
$str.. L
)..L M
]..M N
public// 
IHttpActionResult//  
ChangeBannerStatus//! 3
(//3 4
uint//4 8
reviewId//9 A
,//A B
UInt16//C I
bannerStatus//J V
)//V W
{00 	
bool11 
status11 
=11 
false11 
;11  
try22 
{33 
status44 
=44 !
_objBannerRespository44 .
.44. /
ChangeBannerStatus44/ A
(44A B
reviewId44B J
,44J K
bannerStatus44L X
)44X Y
;44Y Z
MemCachedUtil55 
.55 
Remove55 $
(55$ %
$str55% E
)55E F
;55F G
MemCachedUtil66 
.66 
Remove66 $
(66$ %
$str66% E
)66E F
;66F G
return77 
Ok77 
(77 
status77  
)77  !
;77! "
}88 
catch99 
(99 
	Exception99 
ex99 
)99  
{:: 

ErrorClass<< 
objErr<< !
=<<" #
new<<$ '

ErrorClass<<( 2
(<<2 3
ex<<3 5
,<<5 6
$str<<7 c
)<<c d
;<<d e
return== 
InternalServerError== *
(==* +
)==+ ,
;==, -
}>> 
}?? 	
[EE 	
HttpPostEE	 
,EE 
RouteEE 
(EE 
$strEE B
)EEB C
]EEC D
publicFF 
IHttpActionResultFF  

SaveBannerFF! +
(FF+ ,
[FF, -
FromBodyFF- 5
]FF5 6
BannerVMFF7 ?
	objBannerFF@ I
,FFJ K
uintFFK O

platformIdFFP Z
)FFZ [
{GG 	
ifHH 
(HH 

platformIdHH 
>HH 
$numHH 
)HH 
{II 
tryJJ 
{KK 
BannerDetailsLL !
objBannerDetailsLL" 2
=LL3 4
(LL5 6

platformIdLL6 @
==LLA C
$numLLD E
)LLE F
?LLG H
	objBannerLLI R
.LLR S 
DesktopBannerDetailsLLS g
:LLh i
	objBannerLLj s
.LLs t 
MobileBannerDetails	LLt �
;
LL� �
boolMM 
successMM  
=MM! "!
_objBannerRespositoryMM# 8
.MM8 9 
SaveBannerPropertiesMM9 M
(MMM N
objBannerDetailsMMN ^
,MM^ _

platformIdMM` j
,MMj k
	objBannerMMl u
.MMu v

CampaignId	MMv �
)
MM� �
;
MM� �
ifNN 
(NN 
successNN 
)NN 
MemCachedUtilOO !
.OO! "
RemoveOO" (
(OO( )
stringOO) /
.OO/ 0
FormatOO0 6
(OO6 7
$strOO7 Y
,OOY Z

platformIdOO[ e
)OOe f
)OOf g
;OOg h
returnPP 
OkPP 
(PP 
successPP %
)PP% &
;PP& '
}QQ 
catchRR 
(RR 
	ExceptionRR  
exRR! #
)RR# $
{SS 

ErrorClassTT 
objErrTT %
=TT& '
newTT( +

ErrorClassTT, 6
(TT6 7
exTT7 9
,TT9 :
$strTT; g
)TTg h
;TTh i
returnVV 
InternalServerErrorVV .
(VV. /
)VV/ 0
;VV0 1
}WW 
}XX 
elseYY 
{ZZ 
return[[ 

BadRequest[[ !
([[! "
$str[[" 2
)[[2 3
;[[3 4
}\\ 
}]] 	
}^^ 
}__ �$
aD:\work\bikewaleweb\BikewaleOpr.Service\Controllers\BikeColorImages\ColorImagesBikesController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
.) *
BikeColorImages* 9
{		 
public 

class &
ColorImagesBikesController +
:, -
ApiController. ;
{ 
private 
readonly &
IColorImagesBikeRepository 3 
_objColorImagesBikes4 H
=I J
nullK O
;O P
public &
ColorImagesBikesController )
() *&
IColorImagesBikeRepository* D
objColorImagesBikesE X
)X Y
{ 	 
_objColorImagesBikes  
=! "
objColorImagesBikes# 6
;6 7
} 	
[ 	
HttpPost	 
, 
Route 
( 
$str 2
)2 3
]3 4
public 
IHttpActionResult  
FetchPhotoId! -
(- .
[. /
FromBody/ 7
]7 8#
ColorImagesBikeEntities8 O
objBikeColorDetailsP c
)c d
{ 	
try 
{   
if!! 
(!! 
objBikeColorDetails!! '
!=!!( *
null!!+ /
&&!!0 2

ModelState!!3 =
.!!= >
IsValid!!> E
)!!E F
{"" 
MemCachedUtil$$ !
.$$! "
Remove$$" (
($$( )
string$$) /
.$$/ 0
Format$$0 6
($$6 7
$str$$7 T
,$$T U
objBikeColorDetails$$V i
.$$i j
Modelid$$j q
)$$q r
)$$r s
;$$s t
MemCachedUtil%% !
.%%! "
Remove%%" (
(%%( )
string%%) /
.%%/ 0
Format%%0 6
(%%6 7
$str%%7 J
,%%J K
objBikeColorDetails%%L _
.%%_ `
Modelid%%` g
)%%g h
)%%h i
;%%i j
return&& 
Ok&& 
(&&  
_objColorImagesBikes&& 2
.&&2 3
FetchPhotoId&&3 ?
(&&? @
objBikeColorDetails&&@ S
)&&S T
)&&T U
;&&U V
}'' 
else(( 
{)) 
return** 

BadRequest** %
(**% &
)**& '
;**' (
}++ 
},, 
catch-- 
(-- 
	Exception-- 
ex-- 
)--  
{.. 

ErrorClass// 
objErr// !
=//" #
new//$ '

ErrorClass//( 2
(//2 3
ex//3 5
,//5 6
$str//7 h
)//h i
;//i j
return00 
InternalServerError00 *
(00* +
)00+ ,
;00, -
}11 
}22 	
[99 	
HttpPost99	 
,99 
Route99 
(99 
$str99 4
)994 5
]995 6
public:: 
IHttpActionResult::  "
DeleteBikeColorDetails::! 7
(::7 8
uint::8 <
photoId::= D
,::D E
uint::E I
modelid::J Q
)::Q R
{;; 	
try== 
{>> 
if?? 
(?? 
photoId?? 
>?? 
$num?? 
)??  
{@@ 
MemCachedUtilAA !
.AA! "
RemoveAA" (
(AA( )
stringAA) /
.AA/ 0
FormatAA0 6
(AA6 7
$strAA7 T
,AAT U
modelidAAV ]
)AA] ^
)AA^ _
;AA_ `
MemCachedUtilBB !
.BB! "
RemoveBB" (
(BB( )
stringBB) /
.BB/ 0
FormatBB0 6
(BB6 7
$strBB7 J
,BBJ K
modelidBBL S
)BBS T
)BBT U
;BBU V
returnCC 
OkCC 
(CC  
_objColorImagesBikesCC 2
.CC2 3"
DeleteBikeColorDetailsCC3 I
(CCI J
photoIdCCJ Q
)CCQ R
)CCR S
;CCS T
}DD 
elseEE 
{FF 
returnGG 

BadRequestGG %
(GG% &
)GG& '
;GG' (
}HH 
}II 
catchJJ 
(JJ 
	ExceptionJJ 
exJJ 
)JJ  
{KK 

ErrorClassLL 
objErrLL !
=LL" #
newLL$ '

ErrorClassLL( 2
(LL2 3
exLL3 5
,LL5 6
$strLL7 j
)LLj k
;LLk l
returnMM 
InternalServerErrorMM *
(MM* +
)MM+ ,
;MM, -
}NN 
}OO 	
}PP 
}QQ �s
_D:\work\bikewaleweb\BikewaleOpr.Service\Controllers\Comparison\SponsoredComparisonController.cs
	namespace

 	
BikewaleOpr


 
.

 
Service

 
.

 
Controllers

 )
.

) *

Comparison

* 4
{ 
public 

class )
SponsoredComparisonController .
:/ 0
ApiController1 >
{ 
private 
readonly *
ISponsoredComparisonRepository 7
_objSponsoredRepo8 I
=J K
nullL P
;P Q
private 
readonly /
#ISponsoredComparisonCacheRepository <(
_objSponsoredComparisonCache= Y
=Z [
null\ `
;` a
public )
SponsoredComparisonController ,
(, -*
ISponsoredComparisonRepository- K
objSponsoredRepoL \
,\ ]0
#ISponsoredComparisonCacheRepository	^ �)
objSponsoredComparisonCache
� �
)
� �
{ 	
_objSponsoredRepo 
= 
objSponsoredRepo  0
;0 1(
_objSponsoredComparisonCache (
=) *'
objSponsoredComparisonCache+ F
;F G
} 	
["" 	
HttpGet""	 
,"" 
Route"" 
("" 
$str"" ;
)""; <
]""< =
public## 
IHttpActionResult##  #
GetSponsoredComparisons##! 8
(##8 9
string##9 ?
statuses##@ H
)##H I
{$$ 	
IEnumerable%% 
<%% 
SponsoredComparison%% +
>%%+ ,"
objSponsoredComparison%%- C
=%%D E
null%%F J
;%%J K
IEnumerable&& 
<&& "
SponsoredComparisonDTO&& .
>&&. /%
objSponsoredComparisonDTO&&0 I
=&&J K
null&&L P
;&&P Q
try'' 
{(( "
objSponsoredComparison)) &
=))' (
_objSponsoredRepo))) :
.)): ;#
GetSponsoredComparisons)); R
())R S
statuses))S [
)))[ \
;))\ ]
if** 
(** "
objSponsoredComparison** *
!=**+ -
null**. 2
)**2 3
{++ %
objSponsoredComparisonDTO-- -
=--. /%
SponsoredComparisonMapper--0 I
.--I J
Convert--J Q
(--Q R"
objSponsoredComparison--R h
)--h i
;--i j
return.. 
Ok.. 
(.. %
objSponsoredComparisonDTO.. 7
)..7 8
;..8 9
}// 
else00 
{11 
return22 
InternalServerError22 .
(22. /
)22/ 0
;220 1
}33 
}44 
catch55 
(55 
	Exception55 
ex55 
)55  
{66 

ErrorClass77 
objErr77 !
=77" #
new77$ '

ErrorClass77( 2
(772 3
ex773 5
,775 6
string777 =
.77= >
Format77> D
(77D E
$str	77E �
,
77� �
statuses
77� �
)
77� �
)
77� �
;
77� �
return88 
InternalServerError88 *
(88* +
)88+ ,
;88, -
}99 
}:: 	
[== 	
HttpGet==	 
,== 
Route== 
(== 
$str== g
)==g h
]==h i
public>> 
IHttpActionResult>>  &
GetSponsoredVersionMapping>>! ;
(>>; <
uint>>< @
id>>A C
,>>C D
uint>>E I
targetModelId>>J W
,>>W X
uint>>Y ]
sponsoredModelId>>^ n
)>>n o
{?? 	"
TargetSponsoredMapping@@ "
objVersionMapping@@# 4
=@@5 6
null@@7 ;
;@@; <%
TargetSponsoredMappingDTOAA % 
objVersionMappingDTOAA& :
=AA; <
nullAA= A
;AAA B
tryBB 
{CC 
objVersionMappingDD !
=DD" #
_objSponsoredRepoDD$ 5
.DD5 60
$GetSponsoredComparisonVersionMappingDD6 Z
(DDZ [
idDD[ ]
,DD] ^
targetModelIdDD_ l
,DDl m
sponsoredModelIdDDn ~
)DD~ 
;	DD �
ifEE 
(EE 
objVersionMappingEE %
!=EE& (
nullEE) -
)EE- .
{FF  
objVersionMappingDTOGG (
=GG) *%
SponsoredComparisonMapperGG+ D
.GGD E
ConvertGGE L
(GGL M
objVersionMappingGGM ^
)GG^ _
;GG_ `
returnHH 
OkHH 
(HH  
objVersionMappingDTOHH 2
)HH2 3
;HH3 4
}II 
elseJJ 
{KK 
returnLL 
InternalServerErrorLL .
(LL. /
)LL/ 0
;LL0 1
}MM 
}OO 
catchPP 
(PP 
	ExceptionPP 
exPP 
)PP  
{QQ 

ErrorClassRR 
objErrRR !
=RR" #
newRR$ '

ErrorClassRR( 2
(RR2 3
exRR3 5
,RR5 6
stringRR7 =
.RR= >
FormatRR> D
(RRD E
$str	RRE �
,
RR� �
id
RR� �
)
RR� �
)
RR� �
;
RR� �
returnSS 
InternalServerErrorSS *
(SS* +
)SS+ ,
;SS, -
}TT 
}UU 	
[]] 	
HttpPost]]	 
,]] 
Route]] 
(]] 
$str]] 6
)]]6 7
]]]7 8
public^^ 
IHttpActionResult^^  #
SaveSponsoredComparison^^! 8
(^^8 9
[^^9 :
FromBody^^: B
]^^B C
SponsoredComparison^^C V

comparison^^W a
)^^a b
{__ 	
uint`` 
comparisonId`` 
=`` 
$num``  !
;``! "
tryaa 
{bb 
comparisonIdcc 
=cc 
_objSponsoredRepocc 0
.cc0 1#
SaveSponsoredComparisoncc1 H
(ccH I

comparisonccI S
)ccS T
;ccT U
ifdd 
(dd 
comparisonIddd  
>dd! "
$numdd# $
)dd$ %
{ee (
_objSponsoredComparisonCacheff 0
.ff0 1-
!RefreshSpsonsoredComparisonsCacheff1 R
(ffR S
)ffS T
;ffT U
}gg 
}hh 
catchii 
(ii 
	Exceptionii 
exii 
)ii  
{jj 

ErrorClasskk 
objErrkk !
=kk" #
newkk$ '

ErrorClasskk( 2
(kk2 3
exkk3 5
,kk5 6
$strkk7 y
)kky z
;kkz {
returnll 
InternalServerErrorll *
(ll* +
)ll+ ,
;ll, -
}mm 
returnnn 
Oknn 
(nn 
comparisonIdnn "
)nn" #
;nn# $
}oo 	
[xx 	
HttpPostxx	 
,xx 
Routexx 
(xx 
$strxx X
)xxX Y
]xxY Z
publicyy 
IHttpActionResultyy  +
UpdateSponsoredComparisonStatusyy! @
(yy@ A
uintyyA E
comparisonIdyyF R
,yyR S
ushortyyT Z
statusIdyy[ c
)yyc d
{zz 	
bool{{ 
isSaved{{ 
={{ 
false{{  
;{{  !
try|| 
{}} 
isSaved~~ 
=~~ 
_objSponsoredRepo~~ +
.~~+ ,+
ChangeSponsoredComparisonStatus~~, K
(~~K L
comparisonId~~L X
,~~X Y
statusId~~Z b
)~~b c
;~~c d(
_objSponsoredComparisonCache ,
., --
!RefreshSpsonsoredComparisonsCache- N
(N O
)O P
;P Q
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
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
return
�� 
Ok
�� 
(
�� 
isSaved
�� 
)
�� 
;
�� 
}
�� 	
[
�� 	
HttpPost
��	 
,
�� 
Route
�� 
(
�� 
$str
�� <
)
��< =
]
��= >
public
�� 
IHttpActionResult
��  /
!SaveSponsoredComparisonsBikeRules
��! B
(
��B C
[
��C D
FromBody
��D L
]
��L M"
VersionTargetMapping
��M a
ruleObj
��b i
)
��i j
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
�� 
	isSuccess
�� 
=
�� 
_objSponsoredRepo
�� -
.
��- ..
 SaveSponsoredComparisonBikeRules
��. N
(
��N O
ruleObj
��O V
)
��V W
;
��W X*
_objSponsoredComparisonCache
�� ,
.
��, -/
!RefreshSpsonsoredComparisonsCache
��- N
(
��N O
)
��O P
;
��P Q
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
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
return
�� 
Ok
�� 
(
�� 
	isSuccess
�� 
)
��  
;
��  !
}
�� 	
[
�� 	
HttpPost
��	 
,
�� 
Route
�� 
(
�� 
$str
�� G
)
��G H
]
��H I
public
�� 
IHttpActionResult
��  ,
DeleteSponsoredComparisonRules
��! ?
(
��? @
uint
��@ D
comparisonId
��E Q
)
��Q R
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
�� 
	isSuccess
�� 
=
�� 
_objSponsoredRepo
�� -
.
��- .3
%DeleteSponsoredComparisonBikeAllRules
��. S
(
��S T
comparisonId
��T `
)
��` a
;
��a b*
_objSponsoredComparisonCache
�� ,
.
��, -/
!RefreshSpsonsoredComparisonsCache
��- N
(
��N O
)
��O P
;
��P Q
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
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
return
�� 
Ok
�� 
(
�� 
	isSuccess
�� 
)
��  
;
��  !
}
�� 	
[
�� 	
HttpPost
��	 
,
�� 
Route
�� 
(
�� 
$str
�� f
)
��f g
]
��g h
public
�� 
IHttpActionResult
��  1
#DeleteSponsoredComparisonModelRules
��! D
(
��D E
uint
��E I
comparisonId
��J V
,
��V W
uint
��X \
sponsoredModelId
��] m
)
��m n
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
�� 
	isSuccess
�� 
=
�� 
_objSponsoredRepo
�� -
.
��- .>
0DeleteSponsoredComparisonBikeSponsoredModelRules
��. ^
(
��^ _
comparisonId
��_ k
,
��k l
sponsoredModelId
��m }
)
��} ~
;
��~ *
_objSponsoredComparisonCache
�� ,
.
��, -/
!RefreshSpsonsoredComparisonsCache
��- N
(
��N O
)
��O P
;
��P Q
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
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
return
�� 
Ok
�� 
(
�� 
	isSuccess
�� 
)
��  
;
��  !
}
�� 	
[
�� 	
HttpPost
��	 
,
�� 
Route
�� 
(
�� 
$str
�� m
)
��m n
]
��n o
public
�� 
IHttpActionResult
��  .
 DeleteTargetComparisonModelRules
��! A
(
��A B
uint
��B F
comparisonId
��G S
,
��S T
uint
��U Y
targetversionId
��Z i
)
��i j
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
�� 
	isSuccess
�� 
=
�� 
_objSponsoredRepo
�� -
.
��- .=
/DeleteSponsoredComparisonBikeTargetVersionRules
��. ]
(
��] ^
comparisonId
��^ j
,
��j k
targetversionId
��l {
)
��{ |
;
��| }*
_objSponsoredComparisonCache
�� ,
.
��, -/
!RefreshSpsonsoredComparisonsCache
��- N
(
��N O
)
��O P
;
��P Q
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
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
return
�� 
Ok
�� 
(
�� 
	isSuccess
�� 
)
��  
;
��  !
}
�� 	
}
�� 
}�� �4
ND:\work\bikewaleweb\BikewaleOpr.Service\Controllers\Content\MakesController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
.) *
Content* 1
{ 
[ 
	Authorize 
] 
public 

class 
MakesController  
:! "
ApiController# 0
{ 
private 
readonly  
IBikeMakesRepository -

_makesRepo. 8
=9 :
null; ?
;? @
private 
readonly 

IBikeMakes #

_bikeMakes$ .
=/ 0
null1 5
;5 6
public 
MakesController 
(  
IBikeMakesRepository 3
makeRepo4 <
,< =

IBikeMakes> H
	bikeMakesI R
)R S
{ 	

_makesRepo 
= 
makeRepo !
;! "

_bikeMakes 
= 
	bikeMakes "
;" #
} 	
[$$ 	
HttpGet$$	 
,$$ 
Route$$ 
($$ 
$str$$ 6
)$$6 7
]$$7 8
public%% 
IHttpActionResult%%  
GetSynopsis%%! ,
(%%, -
int%%- 0
makeId%%1 7
)%%7 8
{&& 	
if'' 
('' 
makeId'' 
>'' 
$num'' 
)'' 
{(( 
SynopsisData)) 
objSynopsis)) (
=))) *
null))+ /
;))/ 0
try++ 
{,, 
objSynopsis-- 
=--  !

_makesRepo--" ,
.--, -
Getsynopsis--- 8
(--8 9
makeId--9 ?
)--? @
;--@ A
}.. 
catch// 
(// 
	Exception//  
ex//! #
)//# $
{00 

ErrorClass11 
objErr11 %
=11& '
new11( +

ErrorClass11, 6
(116 7
ex117 9
,119 :
$str11; H
)11H I
;11I J
return33 
InternalServerError33 .
(33. /
)33/ 0
;330 1
}44 
if66 
(66 
objSynopsis66 
!=66  "
null66# '
)66' (
return77 
Ok77 
(77 
BikeDataMapper77 ,
.77, -
Convert77- 4
(774 5
objSynopsis775 @
)77@ A
)77A B
;77B C
else88 
return99 
NotFound99 #
(99# $
)99$ %
;99% &
}:: 
else;; 
return<< 

BadRequest<< !
(<<! "
)<<" #
;<<# $
}== 	
[HH 	
HttpPostHH	 
,HH 
RouteHH 
(HH 
$strHH 7
)HH7 8
]HH8 9
publicII 
IHttpActionResultII  
SaveSynopsisII! -
(II- .
intII. 1
makeIdII2 8
,II8 9
[II: ;
FromBodyII; C
]IIC D
SynopsisDataDtoIIE T
objSynopsisDtoIIU c
)IIc d
{JJ 	
SynopsisDataKK 
objSynopsisKK $
=KK% &
BikeDataMapperKK' 5
.KK5 6
ConvertKK6 =
(KK= >
objSynopsisDtoKK> L
)KKL M
;KKM N
ifMM 
(MM 
makeIdMM 
>MM 
$numMM 
)MM 
{NN 
tryOO 
{PP 
intQQ 
userIdQQ 
=QQ  
$numQQ! "
;QQ" #
intRR 
.RR 
TryParseRR  
(RR  !
BikewaleRR! )
.RR) *
UtilityRR* 1
.RR1 2
OprUserRR2 9
.RR9 :
IdRR: <
,RR< =
outRR> A
userIdRRB H
)RRH I
;RRI J

_makesRepoTT 
.TT 
UpdateSynopsisTT -
(TT- .
makeIdTT. 4
,TT4 5
userIdTT6 <
,TT< =
objSynopsisTT> I
)TTI J
;TTJ K
}UU 
catchVV 
(VV 
	ExceptionVV  
exVV! #
)VV# $
{WW 

ErrorClassXX 
objErrXX %
=XX& '
newXX( +

ErrorClassXX, 6
(XX6 7
exXX7 9
,XX9 :
$strXX; I
)XXI J
;XXJ K
returnZZ 
InternalServerErrorZZ .
(ZZ. /
)ZZ/ 0
;ZZ0 1
}[[ 
}\\ 
else]] 
return^^ 

BadRequest^^ !
(^^! "
$str^^" 2
)^^2 3
;^^3 4
return`` 
Ok`` 
(`` 
)`` 
;`` 
}aa 	
[ii 	
HttpGetii	 
,ii 
Routeii 
(ii 
$strii B
)iiB C
]iiC D
publicjj 
IHttpActionResultjj  
	GetModelsjj! *
(jj* +
EnumBikeTypejj+ 7
requestTypejj8 C
,jjC D
uintjjE I
makeIdjjJ P
)jjP Q
{kk 	
IEnumerablell 
<ll 
	ModelBasell !
>ll! "
objBikeModelBasell# 3
=ll4 5
nullll6 :
;ll: ;
ifmm 
(mm 
makeIdmm 
>mm 
$nummm 
)mm 
{nn 
tryoo 
{pp 
IEnumerableqq 
<qq  
BikeModelEntityBaseqq  3
>qq3 4"
objBikeModelEntityBaseqq5 K
=qqL M

_bikeMakesqqN X
.qqX Y
GetModelsByMakeqqY h
(qqh i
requestTypeqqi t
,qqt u
makeIdqqv |
)qq| }
;qq} ~
objBikeModelBaserr $
=rr% &
BikeModelsMapperrr' 7
.rr7 8
Convertrr8 ?
(rr? @"
objBikeModelEntityBaserr@ V
)rrV W
;rrW X
}ss 
catchtt 
(tt 
	Exceptiontt  
extt! #
)tt# $
{uu 

ErrorClassvv 
objErrvv %
=vv& '
newvv( +

ErrorClassvv, 6
(vv6 7
exvv7 9
,vv9 :
$strvv; ~
)vv~ 
;	vv �
returnww 
InternalServerErrorww .
(ww. /
)ww/ 0
;ww0 1
}xx 
}yy 
elsezz 
{{{ 
return|| 

BadRequest|| !
(||! "
$str||" 2
)||2 3
;||3 4
}}} 
return~~ 
Ok~~ 
(~~ 
objBikeModelBase~~ &
)~~& '
;~~' (
} 	
}
�� 
}�� �&
OD:\work\bikewaleweb\BikewaleOpr.Service\Controllers\Content\ModelsController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
.) *
Content* 1
{ 
public 

class 
ModelsController !
:" #
ApiController$ 1
{ 
private 
readonly !
IBikeModelsRepository .
_modelsRepo/ :
=; <
null= A
;A B
public 
ModelsController 
(  !
IBikeModelsRepository  5

modelsRepo6 @
)@ A
{ 	
_modelsRepo 
= 

modelsRepo $
;$ %
} 	
['' 	
HttpGet''	 
,'' 
Route'' 
('' 
$str'' N
)''N O
]''O P
public(( 
IHttpActionResult((  
	GetModels((! *
(((* +
uint((+ /
makeId((0 6
,((6 7
ushort((8 >
requestType((? J
)((J K
{)) 	
try** 
{++ 
if,, 
(,, 
makeId,, 
>,, 
$num,, 
&&,, !
requestType,," -
>,,. /
$num,,0 1
),,1 2
{-- 
IEnumerable.. 
<..  
BikeModelEntityBase..  3
>..3 4
	objModels..5 >
=..? @
_modelsRepo..A L
...L M
	GetModels..M V
(..V W
makeId..W ]
,..] ^
requestType.._ j
)..j k
;..k l
if00 
(00 
	objModels00 !
!=00" $
null00% )
&&00* ,
	objModels00- 6
.006 7
Count007 <
(00< =
)00= >
>00? @
$num00A B
)00B C
{11 
IEnumerable22 #
<22# $
	ModelBase22$ -
>22- .
objModelsDTO22/ ;
=22< =
BikeModelsMapper22> N
.22N O
Convert22O V
(22V W
	objModels22W `
)22` a
;22a b
return44 
Ok44 !
(44! "
objModelsDTO44" .
)44. /
;44/ 0
}55 
else66 
{66 
return77 
Ok77 !
(77! "
)77" #
;77# $
}88 
}99 
else:: 
{;; 
return<< 

BadRequest<< %
(<<% &
)<<& '
;<<' (
}== 
}>> 
catch?? 
(?? 
	Exception?? 
ex?? 
)??  
{@@ 

ErrorClassAA 
objErrAA !
=AA" #
newAA$ '

ErrorClassAA( 2
(AA2 3
exAA3 5
,AA5 6
$strAA7 j
)AAj k
;AAk l
returnBB 
InternalServerErrorBB *
(BB* +
)BB+ ,
;BB, -
}CC 
}DD 	
[LL 	
HttpGetLL	 
,LL 
RouteLL 
(LL 
$strLL F
)LLF G
]LLG H
publicMM 
IHttpActionResultMM  
GetVersionsMM! ,
(MM, -
EnumBikeTypeMM- 9
requestTypeMM: E
,MME F
uintMMG K
modelIdMML S
)MMS T
{NN 	
IEnumerableOO 
<OO 
VersionBaseOO #
>OO# $"
objBikeVersionBaseListOO% ;
=OO< =
nullOO> B
;OOB C
ifPP 
(PP 
modelIdPP 
>PP 
$numPP 
)PP 
{QQ 
tryRR 
{SS 
IEnumerableTT 
<TT  !
BikeVersionEntityBaseTT  5
>TT5 6(
objBikeVersionEntityBaseListTT7 S
=TTT U
_modelsRepoTTV a
.TTa b
GetVersionsByModelTTb t
(TTt u
requestType	TTu �
,
TT� �
modelId
TT� �
)
TT� �
;
TT� �"
objBikeVersionBaseListUU *
=UU+ ,
BikeVersionsMapperUU- ?
.UU? @
ConvertUU@ G
(UUG H(
objBikeVersionEntityBaseListUUH d
)UUd e
;UUe f
}VV 
catchWW 
(WW 
	ExceptionWW  
exWW! #
)WW# $
{XX 

ErrorClassYY 
objErrYY %
=YY& '
newYY( +

ErrorClassYY, 6
(YY6 7
exYY7 9
,YY9 :
$str	YY; �
)
YY� �
;
YY� �
returnZZ 
InternalServerErrorZZ .
(ZZ. /
)ZZ/ 0
;ZZ0 1
}[[ 
}\\ 
else]] 
{^^ 
return__ 

BadRequest__ !
(__! "
$str__" 4
)__4 5
;__5 6
}`` 
returnaa 
Okaa 
(aa "
objBikeVersionBaseListaa ,
)aa, -
;aa- .
}bb 	
}cc 
}dd �:
OD:\work\bikewaleweb\BikewaleOpr.Service\Controllers\DealerCampaignController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
{ 
public 

class $
DealerCampaignController )
:* +
ApiController, 9
{ 
private 
readonly %
IDealerCampaignRepository 2(
_objDealerCampaignRepository3 O
;O P
public $
DealerCampaignController '
(' (%
IDealerCampaignRepository( A'
objDealerCampaignRepositoryB ]
)] ^
{ 	(
_objDealerCampaignRepository (
=) *'
objDealerCampaignRepository+ F
;F G
} 	
[ 	
HttpGet	 
, 
Route 
( 
$str 8
)8 9
]9 :
public   
IHttpActionResult    
MakesByDealerCity  ! 2
(  2 3
uint  3 7
cityId  8 >
)  > ?
{!! 	
IEnumerable"" 
<"" 
MakeBase""  
>""  !
objDTOMakes""" -
="". /
null""0 4
;""4 5
try## 
{$$ 
if%% 
(%% 
cityId%% 
>%% 
$num%% 
)%% 
{&& 
IEnumerable'' 
<''  
BikeMakeEntityBase''  2
>''2 3
makes''4 9
='': ;
null''< @
;''@ A
makes(( 
=(( (
_objDealerCampaignRepository(( 8
.((8 9
MakesByDealerCity((9 J
(((J K
cityId((K Q
)((Q R
;((R S
if++ 
(++ 
makes++ 
!=++  
null++! %
)++% &
{,, 
objDTOMakes.. #
=..$ %
new..& )
List..* .
<... /
MakeBase../ 7
>..7 8
(..8 9
)..9 :
;..: ;
objDTOMakes// #
=//$ % 
DealerCampaignMapper//& :
.//: ;
Convert//; B
(//B C
makes//C H
)//H I
;//I J
return11 
Ok11 !
(11! "
objDTOMakes11" -
)11- .
;11. /
}22 
else33 
{44 
return55 
NotFound55 '
(55' (
)55( )
;55) *
}66 
}77 
else88 
{99 
return:: 

BadRequest:: %
(::% &
)::& '
;::' (
};; 
}<< 
catch== 
(== 
	Exception== 
ex== 
)==  
{>> 

ErrorClass?? 
objErr?? !
=??" #
new??$ '

ErrorClass??( 2
(??2 3
ex??3 5
,??5 6
String??7 =
.??= >
Format??> D
(??D E
$str??E v
,??v w
cityId??x ~
)??~ 
)	?? �
;
??� �
return@@ 
InternalServerError@@ *
(@@* +
)@@+ ,
;@@, -
}AA 
}BB 	
[LL 	
HttpGetLL	 
,LL 
RouteLL 
(LL 
$strLL B
)LLB C
]LLC D
publicMM 
IHttpActionResultMM  
DealersByMakeCityMM! 2
(MM2 3
uintMM3 7
cityIdMM8 >
,MM> ?
uintMM@ D
makeIdMME K
,MMK L
boolMMM Q
activecontractMMR `
=MMa b
falseMMc h
)MMh i
{NN 	
tryOO 
{PP 
ifQQ 
(QQ 
cityIdQQ 
>QQ 
$numQQ 
&&QQ !
makeIdQQ" (
>QQ) *
$numQQ+ ,
)QQ, -
{RR 
IEnumerableSS 
<SS  
DealerEntityBaseSS  0
>SS0 1
dealersSS2 9
=SS: ;
nullSS< @
;SS@ A
dealersTT 
=TT (
_objDealerCampaignRepositoryTT :
.TT: ;
DealersByMakeCityTT; L
(TTL M
cityIdTTM S
,TTS T
makeIdTTU [
,TT[ \
activecontractTT] k
)TTk l
;TTl m
ifUU 
(UU 
dealersUU 
!=UU  "
nullUU# '
)UU' (
{VV 
returnWW 
OkWW !
(WW! "
dealersWW" )
)WW) *
;WW* +
}XX 
elseYY 
{YY 
returnYY !
NotFoundYY" *
(YY* +
)YY+ ,
;YY, -
}YY. /
}ZZ 
else[[ 
{\\ 
return]] 

BadRequest]] %
(]]% &
)]]& '
;]]' (
}^^ 
}__ 
catch`` 
(`` 
	Exception`` 
ex`` 
)``  
{aa 

ErrorClassbb 
objErrbb !
=bb" #
newbb$ '

ErrorClassbb( 2
(bb2 3
exbb3 5
,bb5 6
Stringbb7 =
.bb= >
Formatbb> D
(bbD E
$strbbE ~
,bb~ 
cityId
bb� �
,
bb� �
makeId
bb� �
,
bb� �
activecontract
bb� �
)
bb� �
)
bb� �
;
bb� �
returncc 
InternalServerErrorcc *
(cc* +
)cc+ ,
;cc, -
}dd 
}ee 	
[nn 	
HttpGetnn	 
,nn 
Routenn 
(nn 
$strnn @
)nn@ A
]nnA B
publicoo 
IHttpActionResultoo  
DealerCampaignsoo! 0
(oo0 1
uintoo1 5
dealerIdoo6 >
,oo> ?
uintoo@ D
cityIdooE K
,ooK L
uintooM Q
makeIdooR X
,ooX Y
boolooZ ^
activecontractoo_ m
=oon o
falseoop u
)oou v
{pp 	
tryqq 
{rr 
IEnumerabless 
<ss '
DealerCampaignDetailsEntityss 7
>ss7 8
	campaignsss9 B
=ssC D
nullssE I
;ssI J
	campaignstt 
=tt (
_objDealerCampaignRepositorytt 8
.tt8 9
DealerCampaignstt9 H
(ttH I
dealerIdttI Q
,ttQ R
cityIdttS Y
,ttY Z
makeIdtt[ a
,tta b
activecontractttc q
)ttq r
;ttr s
ifuu 
(uu 
	campaignsuu 
!=uu  
nulluu! %
)uu% &
{vv 
returnww 
Okww 
(ww 
	campaignsww '
)ww' (
;ww( )
}xx 
elseyy 
{zz 
return{{ 
NotFound{{ #
({{# $
){{$ %
;{{% &
}|| 
}}} 
catch~~ 
(~~ 
	Exception~~ 
ex~~ 
)~~  
{ 

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
��5 6
String
��7 =
.
��= >
Format
��> D
(
��D E
$str
��E x
,
��x y
dealerId��z �
,��� �
activecontract��� �
)��� �
)��� �
;��� �
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
}
�� 	
}
�� 
}�� �+
^D:\work\bikewaleweb\BikewaleOpr.Service\Controllers\DealerFacility\DealerFacilityController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
.) *
DealerFacility* 8
{ 
public 

class $
DealerFacilityController )
:* +
ApiController, 9
{ 
private 
readonly 
IDealers !
_dealerRepo" -
;- .
public $
DealerFacilityController '
(' (
IDealers( 0
dealer1 7
)7 8
{ 	
_dealerRepo 
= 
dealer  
;  !
} 	
["" 	
HttpPost""	 
,"" 
Route"" 
("" 
$str"" =
)""= >
]""> ?
public## 
IHttpActionResult##  
AddDealerFacility##! 2
(##2 3
[##3 4
FromBody##4 <
]##< =
DealerFacilityDTO##> O
objDTO##P V
)##V W
{$$ 	
FacilityEntity%% 
	objEntity%% $
=%%% &
null%%' +
;%%+ ,
uint'' 
newId'' 
='' 
$num'' 
;'' 
if(( 
((( 
objDTO(( 
.(( 
Id(( 
>(( 
$num(( 
)(( 
{)) 
try** 
{++ 
	objEntity,, 
=,,  
DealerFacilityMapper,,  4
.,,4 5
Convert,,5 <
(,,< =
objDTO,,= C
),,C D
;,,D E
if-- 
(-- 
	objEntity-- !
!=--" $
null--% )
)--) *
{.. 
newId// 
=// 
_dealerRepo//  +
.//+ ,
SaveDealerFacility//, >
(//> ?
	objEntity//? H
)//H I
;//I J
if00 
(00 
newId00  
>00! "
$num00# $
)00$ %
{11 
return22 "
Ok22# %
(22% &
newId22& +
)22+ ,
;22, -
}33 
else44 
{55 
return66 "
InternalServerError66# 6
(666 7
)667 8
;668 9
}77 
}88 
}99 
catch:: 
(:: 
	Exception::  
ex::! #
)::# $
{;; 

ErrorClass<< 
objErr<< %
=<<& '
new<<( +

ErrorClass<<, 6
(<<6 7
ex<<7 9
,<<9 :
string<<; A
.<<A B
Format<<B H
(<<H I
$str	<<I �
,
<<� �
newId
<<� �
)
<<� �
)
<<� �
;
<<� �
return== 
InternalServerError== .
(==. /
)==/ 0
;==0 1
}>> 
}?? 
else@@ 
{AA 
returnBB 

BadRequestBB !
(BB! "
)BB" #
;BB# $
}CC 
returnDD 
NotFoundDD 
(DD 
)DD 
;DD 
}EE 	
[NN 	
HttpPostNN	 
,NN 
RouteNN 
(NN 
$strNN I
)NNI J
]NNJ K
publicOO 
IHttpActionResultOO   
UpdateDealerFacilityOO! 5
(OO5 6
[OO6 7
FromBodyOO7 ?
]OO? @
DealerFacilityDTOOOA R
objDTOOOS Y
)OOY Z
{PP 	
FacilityEntityQQ 
	objEntityQQ $
=QQ% &
nullQQ' +
;QQ+ ,
boolSS 
statusSS 
=SS 
falseSS 
;SS  
ifTT 
(TT 
objDTOTT 
.TT 

FacilityIdTT !
>TT" #
$numTT$ %
)TT% &
{UU 
tryVV 
{WW 
	objEntityXX 
=XX  
DealerFacilityMapperXX  4
.XX4 5
ConvertXX5 <
(XX< =
objDTOXX= C
)XXC D
;XXD E
if[[ 
([[ 
	objEntity[[ !
!=[[" $
null[[% )
)[[) *
{\\ 
status]] 
=]]  
_dealerRepo]]! ,
.]], - 
UpdateDealerFacility]]- A
(]]A B
	objEntity]]B K
)]]K L
;]]L M
if^^ 
(^^ 
status^^ "
)^^" #
{__ 
return`` "
Ok``# %
(``% &
)``& '
;``' (
}bb 
}ee 
}gg 
catchhh 
(hh 
	Exceptionhh  
exhh! #
)hh# $
{ii 

ErrorClassjj 
objErrjj %
=jj& '
newjj( +

ErrorClassjj, 6
(jj6 7
exjj7 9
,jj9 :
stringjj; A
.jjA B
FormatjjB H
(jjH I
$str	jjI �
,
jj� �
objDTO
jj� �
.
jj� �

FacilityId
jj� �
,
jj� �
objDTO
jj� �
.
jj� �
Facility
jj� �
,
jj� �
objDTO
jj� �
.
jj� �
IsActive
jj� �
)
jj� �
)
jj� �
;
jj� �
returnkk 
InternalServerErrorkk .
(kk. /
)kk/ 0
;kk0 1
}ll 
}mm 
elsenn 
{oo 
returnpp 

BadRequestpp !
(pp! "
)pp" #
;pp# $
}qq 
returnrr 
NotFoundrr 
(rr 
)rr 
;rr 
}ss 	
}tt 
}uu �G
QD:\work\bikewaleweb\BikewaleOpr.Service\Controllers\DealerPriceQuoteController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
{ 
public 

class &
DealerPriceQuoteController +
:, -
ApiController. ;
{ 
[ 	
HttpPost	 
] 
public 
HttpResponseMessage "
UnmapDealerWithArea# 6
(6 7
uint7 ;
dealerId< D
,D E
stringF L

areaIdListM W
)W X
{ 	
if 
( 
dealerId 
> 
$num 
) 
{ 
bool   
	isSuccess   
=    
false  ! &
;  & '
try!! 
{"" 
using## 
(## 
IUnityContainer## *
	container##+ 4
=##5 6
new##7 :
UnityContainer##; I
(##I J
)##J K
)##K L
{$$ 
	container%% !
.%%! "
RegisterType%%" .
<%%. /
IDealerPriceQuote%%/ @
,%%@ A&
DealerPriceQuoteRepository%%B \
>%%\ ]
(%%] ^
)%%^ _
;%%_ `
IDealerPriceQuote&& )
objPriceQuote&&* 7
=&&8 9
	container&&: C
.&&C D
Resolve&&D K
<&&K L&
DealerPriceQuoteRepository&&L f
>&&f g
(&&g h
)&&h i
;&&i j
	isSuccess'' !
=''" #
objPriceQuote''$ 1
.''1 2
UnmapDealer''2 =
(''= >
dealerId''> F
,''F G

areaIdList''H R
)''R S
;''S T
}(( 
})) 
catch** 
(** 
	Exception**  
ex**! #
)**# $
{++ 
HttpContext,, 
.,,  
Current,,  '
.,,' (
Trace,,( -
.,,- .
Warn,,. 2
(,,2 3
$str,,3 N
+,,O P
ex,,Q S
.,,S T
Message,,T [
+,,\ ]
ex,,^ `
.,,` a
Source,,a g
),,g h
;,,h i

ErrorClass-- 
objErr-- %
=--& '
new--( +

ErrorClass--, 6
(--6 7
ex--7 9
,--9 :
HttpContext--; F
.--F G
Current--G N
.--N O
Request--O V
.--V W
ServerVariables--W f
[--f g
$str--g l
]--l m
)--m n
;--n o
objErr.. 
... 
SendMail.. #
(..# $
)..$ %
;..% &
return// 
Request// "
.//" #
CreateErrorResponse//# 6
(//6 7
HttpStatusCode//7 E
.//E F
InternalServerError//F Y
,//Y Z
$str//[ r
)//r s
;//s t
}00 
if11 
(11 
	isSuccess11 
)11 
return33 
Request33 "
.33" #
CreateResponse33# 1
<331 2
bool332 6
>336 7
(337 8
HttpStatusCode338 F
.33F G
OK33G I
,33I J
	isSuccess33K T
)33T U
;33U V
else44 
return55 
Request55 "
.55" #
CreateResponse55# 1
(551 2
HttpStatusCode552 @
.55@ A
	NoContent55A J
,55J K
$str55L _
)55_ `
;55` a
}66 
else77 
return88 
Request88 
.88 
CreateErrorResponse88 2
(882 3
HttpStatusCode883 A
.88A B

BadRequest88B L
,88L M
$str88N [
)88[ \
;88\ ]
}99 	
[EE 	
HttpGetEE	 
]EE 
publicFF 
HttpResponseMessageFF "#
GetAllDealerPriceQuotesFF# :
(FF: ;
uintFF; ?
	versionIdFF@ I
,FFI J
uintFFK O
cityIdFFP V
,FFV W
uintFFX \
areaIdFF] c
)FFc d
{GG 	
ifHH 
(HH 
cityIdHH 
>HH 
$numHH 
&&HH 
	versionIdHH '
>HH( )
$numHH* +
&&HH, .
areaIdHH/ 5
>HH6 7
$numHH8 9
)HH9 :
{II 
IEnumerableJJ 
<JJ $
DealerPriceQuoteDetailedJJ 4
>JJ4 5
dealerPriceQuotesJJ6 G
=JJH I
nullJJJ N
;JJN O
IEnumerableKK 
<KK 
uintKK  
>KK  !
dealerIdListKK" .
=KK/ 0
nullKK1 5
;KK5 6
stringLL 
	dealerIdsLL  
=LL! "
stringLL# )
.LL) *
EmptyLL* /
;LL/ 0 
DealerPriceQuoteListMM $
objDealersDetailsMM% 6
=MM7 8
newMM9 < 
DealerPriceQuoteListMM= Q
(MMQ R
)MMR S
;MMS T
tryNN 
{OO 
usingPP 
(PP 
IUnityContainerPP *
	containerPP+ 4
=PP5 6
newPP7 :
UnityContainerPP; I
(PPI J
)PPJ K
)PPK L
{QQ 
	containerRR !
.RR! "
RegisterTypeRR" .
<RR. /
IDealerRR/ 6
,RR6 7
DealersRR8 ?
>RR? @
(RR@ A
)RRA B
;RRB C
	containerSS !
.SS! "
RegisterTypeSS" .
<SS. /
IDealerPriceQuoteSS/ @
,SS@ A&
DealerPriceQuoteRepositorySSB \
>SS\ ]
(SS] ^
)SS^ _
;SS_ `
IDealerTT 
	objDealerTT  )
=TT* +
	containerTT, 5
.TT5 6
ResolveTT6 =
<TT= >
IDealerTT> E
>TTE F
(TTF G
)TTG H
;TTH I
dealerIdListUU $
=UU% &
	objDealerUU' 0
.UU0 1!
GetAllAvailableDealerUU1 F
(UUF G
(UUG H
	versionIdUUH Q
)UUQ R
,UUR S
(UUT U
areaIdUUU [
)UU[ \
)UU\ ]
;UU] ^
foreachVV 
(VV  !
uintVV! %
dealerIdVV& .
inVV/ 1
dealerIdListVV2 >
)VV> ?
{WW 
	dealerIdsXX %
+=XX& (
dealerIdXX) 1
.XX1 2
ToStringXX2 :
(XX: ;
)XX; <
+XX= >
$strXX? B
;XXB C
}YY 
	dealerIdsZZ !
=ZZ" #
	dealerIdsZZ$ -
.ZZ- .
	SubstringZZ. 7
(ZZ7 8
$numZZ8 9
,ZZ9 :
	dealerIdsZZ; D
.ZZD E
LengthZZE K
-ZZL M
$numZZN O
)ZZO P
;ZZP Q
dealerPriceQuotes[[ )
=[[* +
	objDealer[[, 5
.[[5 6%
GetDealerPriceQuoteDetail[[6 O
([[O P
	versionId[[P Y
,[[Y Z
cityId[[[ a
,[[a b
	dealerIds[[c l
)[[l m
;[[m n
}\\ 
}]] 
catch^^ 
(^^ 
	Exception^^  
ex^^! #
)^^# $
{__ 
HttpContext`` 
.``  
Current``  '
.``' (
Trace``( -
.``- .
Warn``. 2
(``2 3
$str``3 R
+``S T
ex``U W
.``W X
Message``X _
+``` a
ex``b d
.``d e
Source``e k
)``k l
;``l m

ErrorClassaa 
objErraa %
=aa& '
newaa( +

ErrorClassaa, 6
(aa6 7
exaa7 9
,aa9 :
HttpContextaa; F
.aaF G
CurrentaaG N
.aaN O
RequestaaO V
.aaV W
ServerVariablesaaW f
[aaf g
$straag l
]aal m
)aam n
;aan o
objErrbb 
.bb 
SendMailbb #
(bb# $
)bb$ %
;bb% &
}cc 
ifdd 
(dd 
dealerPriceQuotesdd %
!=dd& (
nulldd) -
)dd- .
{ee 
objDealersDetailsff %
.ff% &
DealersDetailsff& 4
=ff5 6
dealerPriceQuotesff7 H
;ffH I
returngg 
Requestgg "
.gg" #
CreateResponsegg# 1
<gg1 2 
DealerPriceQuoteListgg2 F
>ggF G
(ggG H
HttpStatusCodeggH V
.ggV W
OKggW Y
,ggY Z
objDealersDetailsgg[ l
)ggl m
;ggm n
}hh 
elseii 
returnjj 
Requestjj "
.jj" #
CreateResponsejj# 1
(jj1 2
HttpStatusCodejj2 @
.jj@ A
	NoContentjjA J
,jjJ K
$strjjL _
)jj_ `
;jj` a
}kk 
elsell 
returnmm 
Requestmm 
.mm 
CreateErrorResponsemm 2
(mm2 3
HttpStatusCodemm3 A
.mmA B

BadRequestmmB L
,mmL M
$strmmN [
)mm[ \
;mm\ ]
}nn 	
}oo 
}pp ��
HD:\work\bikewaleweb\BikewaleOpr.Service\Controllers\DealersController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
{ 
public 

class 
DealersController "
:# $
ApiController% 2
{ 
private 
readonly "
IDealerPriceRepository /!
dealerPriceRepository0 E
=F G
nullH L
;L M
private 
readonly 
IDealerPrice %
dealerPrice& 1
=2 3
null4 8
;8 9
private 
readonly 
IDealers !
dealersRepository" 3
=4 5
null6 :
;: ;
private 
readonly  
IVersionAvailability -
versionAvailability. A
=B C
nullD H
;H I
public 
DealersController  
(  !"
IDealerPriceRepository! 7'
dealerPriceRepositoryObject8 S
,S T
IDealerPriceU a
dealerPriceObjectb s
,s t
IDealers   #
dealersRepositoryObject   ,
,  , - 
IVersionAvailability  . B%
versionAvailabilityObject  C \
)  \ ]
{!! 	!
dealerPriceRepository"" !
=""" #'
dealerPriceRepositoryObject""$ ?
;""? @
dealerPrice## 
=## 
dealerPriceObject## +
;##+ ,
dealersRepository$$ 
=$$ #
dealersRepositoryObject$$  7
;$$7 8
versionAvailability%% 
=%%  !%
versionAvailabilityObject%%" ;
;%%; <
}'' 	
[// 	
HttpGet//	 
,// 
ResponseType// 
(// 
typeof// %
(//% &
BikeMakeBase//& 2
)//2 3
)//3 4
,//4 5
Route//6 ;
(//; <
$str//< V
)//V W
]//W X
public00 
IHttpActionResult00   
GetDealerMakesByCity00! 5
(005 6
int006 9
cityId00: @
)00@ A
{11 	
if22 
(22 
cityId22 
>22 
$num22 
)22 
{33 
IEnumerable44 
<44 
BikewaleOpr44 '
.44' (
Entities44( 0
.440 1
BikeData441 9
.449 :
BikeMakeEntityBase44: L
>44L M
objMakes44N V
=44W X
null44Y ]
;44] ^
IEnumerable55 
<55 
BikeMakeBase55 (
>55( )
objMakesDTO55* 5
=556 7
null558 <
;55< =
try66 
{77 
using88 
(88 
IUnityContainer88 *
	container88+ 4
=885 6
new887 :
UnityContainer88; I
(88I J
)88J K
)88K L
{99 
	container:: !
.::! "
RegisterType::" .
<::. /
IDealers::/ 7
,::7 8
DealersRepository::9 J
>::J K
(::K L
)::L M
;::M N
IDealers;;  
objAllDealer;;! -
=;;. /
	container;;0 9
.;;9 :
Resolve;;: A
<;;A B
DealersRepository;;B S
>;;S T
(;;T U
);;U V
;;;V W
objMakes<<  
=<<! "
objAllDealer<<# /
.<</ 0 
GetDealerMakesByCity<<0 D
(<<D E
cityId<<E K
)<<K L
;<<L M
}== 
if?? 
(?? 
objMakes??  
!=??! #
null??$ (
&&??) +
objMakes??, 4
.??4 5
Count??5 :
(??: ;
)??; <
>??= >
$num??? @
)??@ A
{@@ 
objMakesDTOAA #
=AA$ %
newAA& )

CollectionAA* 4
<AA4 5
BikeMakeBaseAA5 A
>AAA B
(AAB C
)AAC D
;AAD E
objMakesDTOBB #
=BB$ %
DealerListMapperBB& 6
.BB6 7
ConvertBB7 >
(BB> ?
objMakesBB? G
)BBG H
;BBH I
objMakesDD  
=DD! "
nullDD# '
;DD' (
returnFF 
OkFF !
(FF! "
objMakesDTOFF" -
)FF- .
;FF. /
}GG 
elseHH 
{II 
returnJJ 
NotFoundJJ '
(JJ' (
)JJ( )
;JJ) *
}KK 
}LL 
catchMM 
(MM 
	ExceptionMM  
exMM! #
)MM# $
{NN 

ErrorClassOO 
objErrOO %
=OO& '
newOO( +

ErrorClassOO, 6
(OO6 7
exOO7 9
,OO9 :
stringOO; A
.OOA B
FormatOOB H
(OOH I
$str	OOI �
,
OO� �
cityId
OO� �
)
OO� �
)
OO� �
;
OO� �
returnPP 
InternalServerErrorPP .
(PP. /
)PP/ 0
;PP0 1
}QQ 
}SS 
elseTT 
{UU 
returnVV 

BadRequestVV !
(VV! "
)VV" #
;VV# $
}WW 
}YY 	
[`` 	
HttpGet``	 
,`` 
ResponseType`` 
(`` 
typeof`` %
(``% &

DealerBase``& 0
)``0 1
)``1 2
,``2 3
Route``4 9
(``9 :
$str``: d
)``d e
]``e f
publicaa 
IHttpActionResultaa  
GetDealersByMakeaa! 1
(aa1 2
uintaa2 6
makeIdaa7 =
,aa= >
uintaa? C
cityIdaaD J
)aaJ K
{bb 	
ifcc 
(cc 
cityIdcc 
>cc 
$numcc 
&&cc 
makeIdcc $
>cc% &
$numcc' (
)cc( )
{dd 
IEnumerableee 
<ee 
DealerEntityBaseee ,
>ee, -

objDealersee. 8
=ee9 :
nullee; ?
;ee? @
IEnumerableff 
<ff 

DealerBaseff &
>ff& '
objDealersDTOff( 5
=ff6 7
nullff8 <
;ff< =
trygg 
{hh 
usingii 
(ii 
IUnityContainerii *
	containerii+ 4
=ii5 6
newii7 :
UnityContainerii; I
(iiI J
)iiJ K
)iiK L
{jj 
	containerkk !
.kk! "
RegisterTypekk" .
<kk. /
IDealerskk/ 7
,kk7 8
DealersRepositorykk9 J
>kkJ K
(kkK L
)kkL M
;kkM N
IDealersll  
objAllDealerll! -
=ll. /
	containerll0 9
.ll9 :
Resolvell: A
<llA B
DealersRepositoryllB S
>llS T
(llT U
)llU V
;llV W

objDealersmm "
=mm# $
objAllDealermm% 1
.mm1 2
GetDealersByMakemm2 B
(mmB C
makeIdmmC I
,mmI J
cityIdmmK Q
)mmQ R
;mmR S
}nn 
ifpp 
(pp 

objDealerspp "
!=pp# %
nullpp& *
&&pp+ -

objDealerspp. 8
.pp8 9
Countpp9 >
(pp> ?
)pp? @
>ppA B
$numppC D
)ppD E
{qq 
objDealersDTOrr %
=rr& '
newrr( +

Collectionrr, 6
<rr6 7

DealerBaserr7 A
>rrA B
(rrB C
)rrC D
;rrD E
objDealersDTOss %
=ss& '
DealerListMapperss( 8
.ss8 9
Convertss9 @
(ss@ A

objDealersssA K
)ssK L
;ssL M

objDealersuu "
=uu# $
nulluu% )
;uu) *
returnww 
Okww !
(ww! "
objDealersDTOww" /
)ww/ 0
;ww0 1
}xx 
elseyy 
{zz 
return{{ 
NotFound{{ '
({{' (
){{( )
;{{) *
}|| 
}}} 
catch~~ 
(~~ 
	Exception~~  
ex~~! #
)~~# $
{ 

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
��6 7
ex
��7 9
,
��9 :
string
��; A
.
��A B
Format
��B H
(
��H I
$str��I �
,��� �
makeId��� �
,��� �
cityId��� �
)��� �
)��� �
;��� �
return
�� !
InternalServerError
�� .
(
��. /
)
��/ 0
;
��0 1
}
�� 
}
�� 
else
�� 
{
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 
}
�� 	
[
�� 	
HttpGet
��	 
,
�� 
ResponseType
�� 
(
�� 
typeof
�� %
(
��% &
DealerMakeDTO
��& 3
)
��3 4
)
��4 5
,
��5 6
Route
��7 <
(
��< =
$str
��= Y
)
��Y Z
]
��Z [
public
�� 
IHttpActionResult
��  
GetDealersByCity
��! 1
(
��1 2
UInt32
��2 8
cityId
��9 ?
)
��? @
{
�� 	
if
�� 
(
�� 
cityId
�� 
>
�� 
$num
�� 
)
�� 
{
�� 
IEnumerable
�� 
<
�� 
DealerMakeEntity
�� ,
>
��, -
dealerEntities
��. <
=
��= >
null
��? C
;
��C D
IEnumerable
�� 
<
�� 
DealerMakeDTO
�� )
>
��) *

dealerDtos
��+ 5
=
��6 7
null
��8 <
;
��< =
try
�� 
{
�� 
dealerEntities
�� "
=
��# $
dealersRepository
��% 6
.
��6 7
GetDealersByCity
��7 G
(
��G H
cityId
��H N
)
��N O
;
��O P

dealerDtos
�� 
=
��  
DealerListMapper
��! 1
.
��1 2
Convert
��2 9
(
��9 :
dealerEntities
��: H
)
��H I
;
��I J
}
�� 
catch
�� 
(
�� 
	Exception
��  
ex
��! #
)
��# $
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
��6 7
ex
��7 9
,
��9 :
HttpContext
��; F
.
��F G
Current
��G N
.
��N O
Request
��O V
.
��V W
ServerVariables
��W f
[
��f g
$str
��g l
]
��l m
)
��m n
;
��n o
}
�� 
if
�� 
(
�� 

dealerDtos
�� 
!=
�� !
null
��" &
)
��& '
return
�� 
Ok
�� 
(
�� 

dealerDtos
�� (
)
��( )
;
��) *
else
�� 
return
�� 
NotFound
�� #
(
��# $
)
��$ %
;
��% &
}
�� 
else
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 	
[
�� 	
HttpGet
��	 
]
�� 
public
�� 
IHttpActionResult
��  
DeleteDealerOffer
��! 2
(
��2 3
string
��3 9
offerId
��: A
)
��A B
{
�� 	
bool
�� 
isdeleteSucess
�� 
=
��  !
false
��" '
;
��' (
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
��% &
offerId
��& -
)
��- .
)
��. /
{
�� 
try
�� 
{
�� 
using
�� 
(
�� 
IUnityContainer
�� *
	container
��+ 4
=
��5 6
new
��7 :
UnityContainer
��; I
(
��I J
)
��J K
)
��K L
{
�� 
	container
�� !
.
��! "
RegisterType
��" .
<
��. /
IDealers
��/ 7
,
��7 8
DealersRepository
��9 J
>
��J K
(
��K L
)
��L M
;
��M N
IDealers
��  
objCity
��! (
=
��) *
	container
��+ 4
.
��4 5
Resolve
��5 <
<
��< =
DealersRepository
��= N
>
��N O
(
��O P
)
��P Q
;
��Q R
isdeleteSucess
�� &
=
��' (
objCity
��) 0
.
��0 1
DeleteDealerOffer
��1 B
(
��B C
offerId
��C J
)
��J K
;
��K L
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
��  
ex
��! #
)
��# $
{
�� 
HttpContext
�� 
.
��  
Current
��  '
.
��' (
Trace
��( -
.
��- .
Warn
��. 2
(
��2 3
$str
��3 L
+
��M N
ex
��O Q
.
��Q R
Message
��R Y
+
��Z [
ex
��\ ^
.
��^ _
Source
��_ e
)
��e f
;
��f g

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
��6 7
ex
��7 9
,
��9 :
HttpContext
��; F
.
��F G
Current
��G N
.
��N O
Request
��O V
.
��V W
ServerVariables
��W f
[
��f g
$str
��g l
]
��l m
)
��m n
;
��n o
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
�� 
if
�� 
(
�� 
isdeleteSucess
�� "
)
��" #
return
�� 
Ok
�� 
(
�� 
isdeleteSucess
�� ,
)
��, -
;
��- .
else
�� 
return
�� 
NotFound
�� #
(
��# $
)
��$ %
;
��% &
}
�� 
else
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 	
[
�� 	
HttpPost
��	 
]
�� 
public
�� 
IHttpActionResult
��  $
UpdateDealerBikeOffers
��! 7
(
��7 8 
DealerOffersEntity
��8 J
dealerOffer
��K V
)
��V W
{
�� 	
try
�� 
{
�� 
bool
�� 
	isUpdated
�� 
=
��  
false
��! &
;
��& '
using
�� 
(
�� 
IUnityContainer
�� &
	container
��' 0
=
��1 2
new
��3 6
UnityContainer
��7 E
(
��E F
)
��F G
)
��G H
{
�� 
	container
�� 
.
�� 
RegisterType
�� *
<
��* +
IDealers
��+ 3
,
��3 4
DealersRepository
��5 F
>
��F G
(
��G H
)
��H I
;
��I J
IDealers
�� 
	objDealer
�� &
=
��' (
	container
��) 2
.
��2 3
Resolve
��3 :
<
��: ;
DealersRepository
��; L
>
��L M
(
��M N
)
��N O
;
��O P!
TermsHtmlFormatting
�� '

htmlFormat
��( 2
=
��3 4
new
��5 8!
TermsHtmlFormatting
��9 L
(
��L M
)
��M N
;
��N O
dealerOffer
�� 
.
��  
Terms
��  %
=
��& '

htmlFormat
��( 2
.
��2 3
MakeHtmlList
��3 ?
(
��? @
dealerOffer
��@ K
.
��K L
Terms
��L Q
)
��Q R
;
��R S
	isUpdated
�� 
=
�� 
	objDealer
��  )
.
��) *$
UpdateDealerBikeOffers
��* @
(
��@ A
dealerOffer
��A L
)
��L M
;
��M N
if
�� 
(
�� 
	isUpdated
�� !
)
��! "
{
�� 
return
�� 
Ok
�� !
(
��! "
$str
��" ?
)
��? @
;
��@ A
}
�� 
else
�� 
{
�� 
return
�� !
InternalServerError
�� 2
(
��2 3
)
��3 4
;
��4 5
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
��5 6
HttpContext
��7 B
.
��B C
Current
��C J
.
��J K
Request
��K R
.
��R S
ServerVariables
��S b
[
��b c
$str
��c h
]
��h i
)
��i j
;
��j k
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
��! "
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
}
�� 	
[
�� 	
HttpGet
��	 
]
�� 
public
�� 
IHttpActionResult
��  "
EditAvailabilityDays
��! 5
(
��5 6
int
��6 9
availabilityId
��: H
,
��H I
int
��J M
days
��N R
)
��R S
{
�� 	
bool
�� 
iseditSuccess
�� 
=
��  
false
��! &
;
��& '
try
�� 
{
�� 
using
�� 
(
�� 
IUnityContainer
�� &
	container
��' 0
=
��1 2
new
��3 6
UnityContainer
��7 E
(
��E F
)
��F G
)
��G H
{
�� 
	container
�� 
.
�� 
RegisterType
�� *
<
��* +
IDealers
��+ 3
,
��3 4
DealersRepository
��5 F
>
��F G
(
��G H
)
��H I
;
��I J
IDealers
�� 
objCity
�� $
=
��% &
	container
��' 0
.
��0 1
Resolve
��1 8
<
��8 9
DealersRepository
��9 J
>
��J K
(
��K L
)
��L M
;
��M N
iseditSuccess
�� !
=
��" #
objCity
��$ +
.
��+ ,"
EditAvailabilityDays
��, @
(
��@ A
Convert
��A H
.
��H I
ToInt32
��I P
(
��P Q
availabilityId
��Q _
)
��_ `
,
��` a
Convert
��b i
.
��i j
ToInt32
��j q
(
��q r
days
��r v
)
��v w
)
��w x
;
��x y
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
��5 6
HttpContext
��7 B
.
��B C
Current
��C J
.
��J K
Request
��K R
.
��R S
ServerVariables
��S b
[
��b c
$str
��c h
]
��h i
)
��i j
;
��j k
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
�� 
if
�� 
(
�� 
iseditSuccess
�� 
)
�� 
return
�� 
Ok
�� 
(
�� 
iseditSuccess
�� '
)
��' (
;
��( )
else
�� 
return
�� 
NotFound
�� 
(
��  
)
��  !
;
��! "
}
�� 	
[
�� 	
HttpPost
��	 
]
�� 
public
�� 
IHttpActionResult
��  $
DeleteDealerDisclaimer
��! 7
(
��7 8
uint
��8 <
disclaimerId
��= I
)
��I J
{
�� 	
bool
�� 
isDeleteSuccess
��  
=
��! "
false
��# (
;
��( )
try
�� 
{
�� 
using
�� 
(
�� 
IUnityContainer
�� &
	container
��' 0
=
��1 2
new
��3 6
UnityContainer
��7 E
(
��E F
)
��F G
)
��G H
{
�� 
	container
�� 
.
�� 
RegisterType
�� *
<
��* +
IDealers
��+ 3
,
��3 4
DealersRepository
��5 F
>
��F G
(
��G H
)
��H I
;
��I J
IDealers
�� 
objDisclaimer
�� *
=
��+ ,
	container
��- 6
.
��6 7
Resolve
��7 >
<
��> ?
DealersRepository
��? P
>
��P Q
(
��Q R
)
��R S
;
��S T
isDeleteSuccess
�� #
=
��$ %
objDisclaimer
��& 3
.
��3 4$
DeleteDealerDisclaimer
��4 J
(
��J K
disclaimerId
��K W
)
��W X
;
��X Y
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
��5 6
HttpContext
��7 B
.
��B C
Current
��C J
.
��J K
Request
��K R
.
��R S
ServerVariables
��S b
[
��b c
$str
��c h
]
��h i
)
��i j
;
��j k
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
�� 
if
�� 
(
�� 
isDeleteSuccess
�� 
)
��  
return
�� 
Ok
�� 
(
�� 
$str
�� 9
)
��9 :
;
��: ;
else
�� 
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 	
[
�� 	
HttpPost
��	 
]
�� 
public
�� 
IHttpActionResult
��  
EditDisclaimer
��! /
(
��/ 0
uint
��0 4
disclaimerId
��5 A
,
��A B
string
��C I
newDisclaimerText
��J [
)
��[ \
{
�� 	
bool
�� 
iseditSuccess
�� 
=
��  
false
��! &
;
��& '
try
�� 
{
�� 
using
�� 
(
�� 
IUnityContainer
�� &
	container
��' 0
=
��1 2
new
��3 6
UnityContainer
��7 E
(
��E F
)
��F G
)
��G H
{
�� 
	container
�� 
.
�� 
RegisterType
�� *
<
��* +
IDealers
��+ 3
,
��3 4
DealersRepository
��5 F
>
��F G
(
��G H
)
��H I
;
��I J
IDealers
�� 
objCity
�� $
=
��% &
	container
��' 0
.
��0 1
Resolve
��1 8
<
��8 9
DealersRepository
��9 J
>
��J K
(
��K L
)
��L M
;
��M N
iseditSuccess
�� !
=
��" #
objCity
��$ +
.
��+ ,
EditDisclaimer
��, :
(
��: ;
disclaimerId
��; G
,
��G H
newDisclaimerText
��I Z
)
��Z [
;
��[ \
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
��5 6
HttpContext
��7 B
.
��B C
Current
��C J
.
��J K
Request
��K R
.
��R S
ServerVariables
��S b
[
��b c
$str
��c h
]
��h i
)
��i j
;
��j k
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
�� 
if
�� 
(
�� 
iseditSuccess
�� 
)
�� 
return
�� 
Ok
�� 
(
�� 
$str
�� ,
)
��, -
;
��- .
else
�� 
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 	
[
�� 	
HttpGet
��	 
]
�� 
public
�� 
IHttpActionResult
��  !
UpdateBookingAmount
��! 4
(
��4 5
uint
��5 9
	bookingId
��: C
,
��C D
uint
��E I
amount
��J P
)
��P Q
{
�� 	
bool
�� 
	isSuccess
�� 
=
�� 
false
�� "
;
��" #
try
�� 
{
�� 
using
�� 
(
�� 
IUnityContainer
�� &
	container
��' 0
=
��1 2
new
��3 6
UnityContainer
��7 E
(
��E F
)
��F G
)
��G H
{
�� 
	container
�� 
.
�� 
RegisterType
�� *
<
��* +
IDealers
��+ 3
,
��3 4
DealersRepository
��5 F
>
��F G
(
��G H
)
��H I
;
��I J
IDealers
�� 
objBookingAmt
�� *
=
��+ ,
	container
��- 6
.
��6 7
Resolve
��7 >
<
��> ?
DealersRepository
��? P
>
��P Q
(
��Q R
)
��R S
;
��S T%
BookingAmountEntityBase
�� +
objBookingAmtBase
��, =
=
��> ?
new
��@ C%
BookingAmountEntityBase
��D [
(
��[ \
)
��\ ]
{
��^ _
Amount
��` f
=
��g h
amount
��i o
,
��o p
Id
��q s
=
��t u
	bookingId
��v 
}��� �
;��� �
	isSuccess
�� 
=
�� 
objBookingAmt
��  -
.
��- .!
UpdateBookingAmount
��. A
(
��A B
objBookingAmtBase
��B S
)
��S T
;
��T U
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
��5 6
HttpContext
��7 B
.
��B C
Current
��C J
.
��J K
Request
��K R
.
��R S
ServerVariables
��S b
[
��b c
$str
��c h
]
��h i
)
��i j
;
��j k
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
�� 
if
�� 
(
�� 
	isSuccess
�� 
)
�� 
return
�� 
Ok
�� 
(
�� 
	isSuccess
�� #
)
��# $
;
��$ %
else
�� 
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 	
[
�� 	
HttpGet
��	 
]
�� 
public
�� 
IHttpActionResult
��  !
DeleteBookingAmount
��! 4
(
��4 5
uint
��5 9
	bookingId
��: C
)
��C D
{
�� 	
bool
�� 
isDeleteSuccess
��  
=
��! "
false
��# (
;
��( )
try
�� 
{
�� 
using
�� 
(
�� 
IUnityContainer
�� &
	container
��' 0
=
��1 2
new
��3 6
UnityContainer
��7 E
(
��E F
)
��F G
)
��G H
{
�� 
	container
�� 
.
�� 
RegisterType
�� *
<
��* +
IDealers
��+ 3
,
��3 4
DealersRepository
��5 F
>
��F G
(
��G H
)
��H I
;
��I J
IDealers
�� 

objBooking
�� '
=
��( )
	container
��* 3
.
��3 4
Resolve
��4 ;
<
��; <
DealersRepository
��< M
>
��M N
(
��N O
)
��O P
;
��P Q
isDeleteSuccess
�� #
=
��$ %

objBooking
��& 0
.
��0 1!
DeleteBookingAmount
��1 D
(
��D E
	bookingId
��E N
)
��N O
;
��O P
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
��5 6
HttpContext
��7 B
.
��B C
Current
��C J
.
��J K
Request
��K R
.
��R S
ServerVariables
��S b
[
��b c
$str
��c h
]
��h i
)
��i j
;
��j k
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
�� 
if
�� 
(
�� 
isDeleteSuccess
�� 
)
��  
return
�� 
Ok
�� 
(
�� 
$str
�� 9
)
��9 :
;
��: ;
else
�� 
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 	
[
�� 	
HttpPost
��	 
]
�� 
public
�� 
IHttpActionResult
��  
SaveDealerBenefit
��! 2
(
��2 3
uint
��3 7
dealerId
��8 @
,
��@ A
uint
��B F
cityId
��G M
,
��M N
uint
��O S
catId
��T Y
,
��Y Z
string
��[ a
benefitText
��b m
,
��m n
uint
��o s
userId
��t z
,
��z {
uint��| �
	benefitId��� �
=��� �
$num��� �
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
��" #
if
�� 
(
�� 
dealerId
�� 
>
�� 
$num
�� 
&&
�� 
cityId
��  &
>
��' (
$num
��) *
&&
��+ -
catId
��. 3
>
��4 5
$num
��6 7
&&
��8 :
userId
��; A
>
��B C
$num
��D E
&&
��F H
	benefitId
��I R
>
��S T
$num
��U V
&&
��W Y
!
��Z [
String
��[ a
.
��a b
IsNullOrEmpty
��b o
(
��o p
benefitText
��p {
)
��{ |
)
��| }
{
�� 
try
�� 
{
�� 
using
�� 
(
�� 
IUnityContainer
�� *
	container
��+ 4
=
��5 6
new
��7 :
UnityContainer
��; I
(
��I J
)
��J K
)
��K L
{
�� 
	container
�� !
.
��! "
RegisterType
��" .
<
��. /
IDealers
��/ 7
,
��7 8
DealersRepository
��9 J
>
��J K
(
��K L
)
��L M
;
��M N
IDealers
��  
objCity
��! (
=
��) *
	container
��+ 4
.
��4 5
Resolve
��5 <
<
��< =
DealersRepository
��= N
>
��N O
(
��O P
)
��P Q
;
��Q R
	isSuccess
�� !
=
��" #
objCity
��$ +
.
��+ ,
SaveDealerBenefit
��, =
(
��= >
dealerId
��> F
,
��F G
cityId
��H N
,
��N O
catId
��P U
,
��U V
benefitText
��W b
,
��b c
userId
��d j
,
��j k
	benefitId
��l u
)
��u v
;
��v w
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
��  
ex
��! #
)
��# $
{
�� 

ErrorClass
�� 
objErr
�� %
=
��& '
new
��( +

ErrorClass
��, 6
(
��6 7
ex
��7 9
,
��9 :
$str
��; N
)
��N O
;
��O P
objErr
�� 
.
�� 
SendMail
�� #
(
��# $
)
��$ %
;
��% &
return
�� !
InternalServerError
�� .
(
��. /
)
��/ 0
;
��0 1
}
�� 
if
�� 
(
�� 
	isSuccess
�� 
)
�� 
return
�� 
Ok
�� 
(
�� 
$str
�� )
)
��) *
;
��* +
else
�� 
return
�� !
InternalServerError
�� .
(
��. /
)
��/ 0
;
��0 1
}
�� 
else
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 	
[
�� 	
HttpGet
��	 
]
�� 
public
�� 
IHttpActionResult
��  "
DeleteDealerBenefits
��! 5
(
��5 6
string
��6 <

benefitIds
��= G
)
��G H
{
�� 	
bool
�� 
isDeleteSuccess
��  
=
��! "
false
��# (
;
��( )
try
�� 
{
�� 
using
�� 
(
�� 
IUnityContainer
�� &
	container
��' 0
=
��1 2
new
��3 6
UnityContainer
��7 E
(
��E F
)
��F G
)
��G H
{
�� 
	container
�� 
.
�� 
RegisterType
�� *
<
��* +
IDealers
��+ 3
,
��3 4
DealersRepository
��5 F
>
��F G
(
��G H
)
��H I
;
��I J
IDealers
�� 
objDisclaimer
�� *
=
��+ ,
	container
��- 6
.
��6 7
Resolve
��7 >
<
��> ?
DealersRepository
��? P
>
��P Q
(
��Q R
)
��R S
;
��S T
isDeleteSuccess
�� #
=
��$ %
objDisclaimer
��& 3
.
��3 4"
DeleteDealerBenefits
��4 H
(
��H I

benefitIds
��I S
)
��S T
;
��T U
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
��7 M
)
��M N
;
��N O
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
�� 
if
�� 
(
�� 
isDeleteSuccess
�� 
)
��  
return
�� 
Ok
�� 
(
�� 
$str
�� 9
)
��9 :
;
��: ;
else
�� 
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 	
[
�� 	
HttpGet
��	 
]
�� 
public
�� 
IHttpActionResult
��  
DeleteDealerEMI
��! 0
(
��0 1
uint
��1 5
id
��6 8
)
��8 9
{
�� 	
bool
�� 
isDeleteSuccess
��  
=
��! "
false
��# (
;
��( )
try
�� 
{
�� 
using
�� 
(
�� 
IUnityContainer
�� &
	container
��' 0
=
��1 2
new
��3 6
UnityContainer
��7 E
(
��E F
)
��F G
)
��G H
{
�� 
	container
�� 
.
�� 
RegisterType
�� *
<
��* +
IDealers
��+ 3
,
��3 4
DealersRepository
��5 F
>
��F G
(
��G H
)
��H I
;
��I J
IDealers
�� 
	objDealer
�� &
=
��' (
	container
��) 2
.
��2 3
Resolve
��3 :
<
��: ;
DealersRepository
��; L
>
��L M
(
��M N
)
��N O
;
��O P
isDeleteSuccess
�� #
=
��$ %
	objDealer
��& /
.
��/ 0
DeleteDealerEMI
��0 ?
(
��? @
id
��@ B
)
��B C
;
��C D
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
��5 6
$str
��7 H
)
��H I
;
��I J
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
�� 
if
�� 
(
�� 
isDeleteSuccess
�� 
)
��  
return
�� 
Ok
�� 
(
�� 
$str
�� 9
)
��9 :
;
��: ;
else
�� 
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 	
[
�� 	
HttpGet
��	 
]
�� 
public
�� 
IHttpActionResult
��  
GetDealerPrices
��! 0
(
��0 1
uint
��1 5
cityId
��6 <
,
��< =
uint
��> B
makeId
��C I
,
��I J
uint
��K O
dealerId
��P X
)
��X Y
{
�� 	
IEnumerable
�� 
<
�� #
DealerVersionPriceDTO
�� -
>
��- .
dealerPricesDtos
��/ ?
=
��@ A
null
��B F
;
��F G
IEnumerable
�� 
<
�� &
DealerVersionPriceEntity
�� 0
>
��0 1"
dealerPricesEntities
��2 F
=
��G H
null
��I M
;
��M N
if
�� 
(
�� 
cityId
�� 
>
�� 
$num
�� 
&&
�� 
makeId
�� $
>
��% &
$num
��' (
&&
��) +
dealerId
��, 4
>
��5 6
$num
��7 8
)
��8 9
{
�� 
try
�� 
{
�� "
dealerPricesEntities
�� (
=
��) *
dealerPrice
��+ 6
.
��6 7"
GetDealerPriceQuotes
��7 K
(
��K L
cityId
��L R
,
��R S
makeId
��T Z
,
��Z [
dealerId
��\ d
)
��d e
;
��e f
dealerPricesDtos
�� $
=
��% &
DealerListMapper
��' 7
.
��7 8
Convert
��8 ?
(
��? @"
dealerPricesEntities
��@ T
)
��T U
;
��U V
}
�� 
catch
�� 
(
�� 
	Exception
��  
ex
��! #
)
��# $
{
�� 

ErrorClass
�� 
objErr
�� %
=
��& '
new
��( +

ErrorClass
��, 6
(
��6 7
ex
��7 9
,
��9 :
string
��; A
.
��A B
Format
��B H
(
��H I
$str
�� L
,
��L M
cityId
��N T
,
��T U
makeId
��V \
,
��\ ]
dealerId
��^ f
)
��f g
)
��g h
;
��h i
objErr
�� 
.
�� 
SendMail
�� #
(
��# $
)
��$ %
;
��% &
}
�� 
if
�� 
(
�� 
dealerPricesDtos
�� $
!=
��% '
null
��( ,
)
��, -
return
�� 
Ok
�� 
(
�� 
dealerPricesDtos
�� .
)
��. /
;
��/ 0
else
�� 
return
�� 
NotFound
�� #
(
��# $
)
��$ %
;
��% &
}
�� 
else
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 	
[
�� 	
HttpPost
��	 
,
�� 
Route
�� 
(
�� 
$str
�� A
)
��A B
]
��B C
public
�� 
IHttpActionResult
��  -
SaveDealerPricesAndAvailability
��! @
(
��@ A0
"DealerVersionPricesAvailabilityDTO
��A c)
dealerPricesAvaialabilities
��d 
)�� �
{
�� 	*
SaveDealerPricingResponseDTO
�� (
apiResponse
��) 4
=
��5 6
new
��7 :*
SaveDealerPricingResponseDTO
��; W
(
��W X
)
��X Y
;
��Y Z.
 UpdatePricingRulesResponseEntity
�� ,
savePriceResponse
��- >
=
��? @
new
��A D.
 UpdatePricingRulesResponseEntity
��E e
(
��e f
)
��f g
;
��g h
apiResponse
�� 
.
�� 
IsPriceSaved
�� $
=
��% &
false
��' ,
;
��, -
apiResponse
�� 
.
�� !
IsAvailabilitySaved
�� +
=
��, -
false
��. 3
;
��3 4
bool
�� !
isAvailabilitySaved
�� $
=
��% &
false
��' ,
;
��, -
if
�� 
(
�� )
dealerPricesAvaialabilities
�� +
!=
��, .
null
��/ 3
)
��3 4
{
�� 
try
�� 
{
�� 
if
�� 
(
�� )
dealerPricesAvaialabilities
�� 3
.
��3 4!
DealerVersionPrices
��4 G
.
��G H
ItemIds
��H O
!=
��P R
null
��S W
&&
��X Z)
dealerPricesAvaialabilities
��[ v
.
��v w"
DealerVersionPrices��w �
.��� �
ItemIds��� �
.��� �
Count��� �
(��� �
)��� �
>��� �
$num��� �
&&��� �+
dealerPricesAvaialabilities��� �
.��� �#
DealerVersionPrices��� �
.��� �

ItemValues��� �
!=��� �
null��� �
&&��� �+
dealerPricesAvaialabilities��� �
.��� �#
DealerVersionPrices��� �
.��� �

ItemValues��� �
.��� �
Count��� �
(��� �
)��� �
>��� �
$num��� �
&&��� �+
dealerPricesAvaialabilities��� �
.��� �#
DealerVersionPrices��� �
.��� �

VersionIds��� �
!=��� �
null��� �
&&��� �+
dealerPricesAvaialabilities��� �
.��� �#
DealerVersionPrices��� �
.��� �

VersionIds��� �
.��� �
Count��� �
(��� �
)��� �
>��� �
$num��� �
&&��� �+
dealerPricesAvaialabilities��� �
.��� �#
DealerVersionPrices��� �
.��� �
CityIds��� �
!=��� �
null��� �
&&��� �+
dealerPricesAvaialabilities��� �
.��� �#
DealerVersionPrices��� �
.��� �
CityIds��� �
.��� �
Count��� �
(��� �
)��� �
>��� �
$num��� �
&&��� �+
dealerPricesAvaialabilities��� �
.��� �#
DealerVersionPrices��� �
.��� �
	DealerIds��� �
.��� �
Count��� �
(��� �
)��� �
>��� �
$num��� �
)��� �
savePriceResponse
�� )
=
��* +
dealerPrice
��, 7
.
��7 8$
SaveVersionPriceQuotes
��8 N
(
��N O)
dealerPricesAvaialabilities
�� 7
.
��7 8!
DealerVersionPrices
��8 K
.
��K L
	DealerIds
��L U
,
��U V)
dealerPricesAvaialabilities
�� 7
.
��7 8!
DealerVersionPrices
��8 K
.
��K L
CityIds
��L S
,
��S T)
dealerPricesAvaialabilities
�� 7
.
��7 8!
DealerVersionPrices
��8 K
.
��K L

VersionIds
��L V
,
��V W)
dealerPricesAvaialabilities
�� 7
.
��7 8!
DealerVersionPrices
��8 K
.
��K L
ItemIds
��L S
,
��S T)
dealerPricesAvaialabilities
�� 7
.
��7 8!
DealerVersionPrices
��8 K
.
��K L

ItemValues
��L V
,
��V W)
dealerPricesAvaialabilities
�� 7
.
��7 8
BikeModelIds
��8 D
,
��D E)
dealerPricesAvaialabilities
�� 7
.
��7 8
BikeModelNames
��8 F
,
��F G)
dealerPricesAvaialabilities
�� 7
.
��7 8!
DealerVersionPrices
��8 K
.
��K L
	EnteredBy
��L U
,
��U V)
dealerPricesAvaialabilities
�� 7
.
��7 8
MakeId
��8 >
)
�� 
;
�� 
if
�� 
(
�� )
dealerPricesAvaialabilities
�� 3
.
��3 4)
DealerVersionAvailabilities
��4 O
.
��O P
DealerId
��P X
>
��Y Z
$num
��[ \
&&
��] _)
dealerPricesAvaialabilities
��` {
.
��{ |*
DealerVersionAvailabilities��| �
.��� �
BikeVersionIds��� �
!=��� �
null��� �
&&��� �+
dealerPricesAvaialabilities��� �
.��� �+
DealerVersionAvailabilities��� �
.��� �
BikeVersionIds��� �
.��� �
Count��� �
(��� �
)��� �
>��� �
$num��� �
&&��� �+
dealerPricesAvaialabilities��� �
.��� �+
DealerVersionAvailabilities��� �
.��� �
NumberOfDays��� �
!=��� �
null��� �
&&��� �+
dealerPricesAvaialabilities��� �
.��� �+
DealerVersionAvailabilities��� �
.��� �
NumberOfDays��� �
.��� �
Count��� �
(��� �
)��� �
>��� �
$num��� �
)��� �!
isAvailabilitySaved
�� +
=
��, -!
versionAvailability
��. A
.
��A B%
SaveVersionAvailability
��B Y
(
��Y Z)
dealerPricesAvaialabilities
�� 7
.
��7 8)
DealerVersionAvailabilities
��8 S
.
��S T
DealerId
��T \
,
��\ ])
dealerPricesAvaialabilities
�� 7
.
��7 8)
DealerVersionAvailabilities
��8 S
.
��S T
BikeVersionIds
��T b
,
��b c)
dealerPricesAvaialabilities
�� 7
.
��7 8)
DealerVersionAvailabilities
��8 S
.
��S T
NumberOfDays
��T `
)
�� 
;
�� 
apiResponse
�� 
=
��  !-
SaveDealerPricingResponseMapper
��" A
.
��A B
Convert
��B I
(
��I J
savePriceResponse
��J [
,
��[ \!
isAvailabilitySaved
��] p
)
��p q
;
��q r
}
�� 
catch
�� 
(
�� 
	Exception
��  
ex
��! #
)
��# $
{
�� 

ErrorClass
�� 
objErr
�� %
=
��& '
new
��( +

ErrorClass
��, 6
(
��6 7
ex
��7 9
,
��9 :
$str
��; L
)
��L M
;
��M N
}
�� 
if
�� 
(
�� 
apiResponse
�� 
.
��  
IsPriceSaved
��  ,
||
��- /
apiResponse
��0 ;
.
��; <!
IsAvailabilitySaved
��< O
)
��O P
return
�� 
Ok
�� 
(
�� 
apiResponse
�� )
)
��) *
;
��* +
else
�� 
return
�� 
NotFound
�� #
(
��# $
)
��$ %
;
��% &
}
�� 
else
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 	
[
�� 	
HttpPost
��	 
,
�� 
Route
�� 
(
�� 
$str
�� 2
)
��2 3
]
��3 4
public
�� 
IHttpActionResult
��  
SaveDealerPrices
��! 1
(
��1 2 
DealerPriceListDTO
��2 D
dealerPrices
��E Q
)
��Q R
{
�� 	
bool
�� 
isSaved
�� 
=
�� 
false
��  
;
��  !
if
�� 
(
�� 
dealerPrices
�� 
!=
�� 
null
��  $
&&
��% '
dealerPrices
��( 4
.
��4 5

VersionIds
��5 ?
!=
��@ B
null
��C G
&&
��H J
dealerPrices
��K W
.
��W X

VersionIds
��X b
.
��b c
Count
��c h
(
��h i
)
��i j
>
��k l
$num
��m n
&&
��o q
dealerPrices
��r ~
.
��~ 
CityIds�� �
!=��� �
null��� �
&&��� �
dealerPrices��� �
.��� �
	DealerIds��� �
!=��� �
null��� �
&&��� �
dealerPrices��� �
.��� �
CityIds��� �
.��� �
Count��� �
(��� �
)��� �
>��� �
$num��� �
&&��� �
dealerPrices��� �
.��� �
	DealerIds��� �
.��� �
Count��� �
(��� �
)��� �
>��� �
$num��� �
)��� �
{
�� 
try
�� 
{
�� 
isSaved
�� 
=
�� 
dealerPrice
�� )
.
��) *$
SaveVersionPriceQuotes
��* @
(
��@ A
dealerPrices
��A M
.
��M N
	DealerIds
��N W
,
��W X
dealerPrices
��Y e
.
��e f
CityIds
��f m
,
��m n
dealerPrices
��o {
.
��{ |

VersionIds��| �
,��� �
dealerPrices
�� $
.
��$ %
ItemIds
��% ,
,
��, -
dealerPrices
��. :
.
��: ;

ItemValues
��; E
,
��E F
dealerPrices
��G S
.
��S T
	EnteredBy
��T ]
)
��] ^
;
��^ _
}
�� 
catch
�� 
(
�� 
	Exception
��  
ex
��! #
)
��# $
{
�� 

ErrorClass
�� 
objErr
�� %
=
��& '
new
��( +

ErrorClass
��, 6
(
��6 7
ex
��7 9
,
��9 :
$str
��; M
)
��M N
;
��N O
}
�� 
if
�� 
(
�� 
isSaved
�� 
)
�� 
return
�� 
Ok
�� 
(
�� 
isSaved
�� %
)
��% &
;
��& '
else
�� 
return
�� 
NotFound
�� #
(
��# $
)
��$ %
;
��% &
}
�� 
else
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 	
[
�� 	
HttpPost
��	 
,
�� 
Route
�� 
(
�� 
$str
�� C
)
��C D
]
��D E
public
�� 
IHttpActionResult
��  /
!DeleteDealerPricesAndAvailability
��! B
(
��B C#
DealerCityVersionsDTO
��C X 
dealerCityVersions
��Y k
)
��k l
{
�� 	,
DeleteDealerPricingResponseDTO
�� *
apiResponse
��+ 6
=
��7 8
new
��9 <,
DeleteDealerPricingResponseDTO
��= [
(
��[ \
)
��\ ]
;
��] ^
apiResponse
�� 
.
�� 
IsPriceDeleted
�� &
=
��' (
false
��) .
;
��. /
apiResponse
�� 
.
�� #
IsAvailabilityDeleted
�� -
=
��. /
false
��0 5
;
��5 6
if
�� 
(
��  
dealerCityVersions
�� "
.
��" #
CityId
��# )
>
��* +
$num
��, -
&&
��. 0 
dealerCityVersions
��1 C
.
��C D
DealerId
��D L
>
��M N
$num
��O P
)
��P Q
{
�� 
try
�� 
{
�� 
apiResponse
�� 
.
��  
IsPriceDeleted
��  .
=
��/ 0
dealerPrice
��1 <
.
��< =&
DeleteVersionPriceQuotes
��= U
(
��U V 
dealerCityVersions
�� *
.
��* +
DealerId
��+ 3
,
��3 4 
dealerCityVersions
�� *
.
��* +
CityId
��+ 1
,
��1 2 
dealerCityVersions
�� *
.
��* +
BikeVersionIds
��+ 9
)
�� 
;
�� 
apiResponse
�� 
.
��  #
IsAvailabilityDeleted
��  5
=
��6 7!
versionAvailability
��8 K
.
��K L'
DeleteVersionAvailability
��L e
(
��e f 
dealerCityVersions
��f x
.
��x y
DealerId��y �
,��� �"
dealerCityVersions��� �
.��� �
BikeVersionIds��� �
)��� �
;��� �
}
�� 
catch
�� 
(
�� 
	Exception
��  
ex
��! #
)
��# $
{
�� 

ErrorClass
�� 
objErr
�� %
=
��& '
new
��( +

ErrorClass
��, 6
(
��6 7
ex
��7 9
,
��9 :
$str
��; ^
)
��^ _
;
��_ `
}
�� 
if
�� 
(
�� 
apiResponse
�� 
.
��  
IsPriceDeleted
��  .
||
��/ 1
apiResponse
��2 =
.
��= >#
IsAvailabilityDeleted
��> S
)
��S T
return
�� 
Ok
�� 
(
�� 
apiResponse
�� )
)
��) *
;
��* +
else
�� 
return
�� 
NotFound
�� #
(
��# $
)
��$ %
;
��% &
}
�� 
else
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 	
[
�� 	
HttpPost
��	 
]
�� 
public
�� 
IHttpActionResult
��  %
SaveVersionAvailability
��! 8
(
��8 9
VersionDaysDTO
��9 G
versionDays
��H S
)
��S T
{
�� 	
bool
�� 
isSaved
�� 
=
�� 
false
��  
;
��  !
if
�� 
(
�� 
versionDays
�� 
.
�� 
DealerId
�� $
>
��% &
$num
��' (
)
��( )
{
�� 
try
�� 
{
�� 
isSaved
�� 
=
�� !
versionAvailability
�� 1
.
��1 2%
SaveVersionAvailability
��2 I
(
��I J
versionDays
��J U
.
��U V
DealerId
��V ^
,
��^ _
versionDays
��` k
.
��k l
BikeVersionIds
��l z
,
��z {
versionDays��| �
.��� �
NumberOfDays��� �
)��� �
;��� �
}
�� 
catch
�� 
(
�� 
	Exception
��  
ex
��! #
)
��# $
{
�� 

ErrorClass
�� 
objErr
�� %
=
��& '
new
��( +

ErrorClass
��, 6
(
��6 7
ex
��7 9
,
��9 :
$str
��; T
)
��T U
;
��U V
}
�� 
if
�� 
(
�� 
isSaved
�� 
)
�� 
return
�� 
Ok
�� 
(
�� 
isSaved
�� %
)
��% &
;
��& '
else
�� 
return
�� 
NotFound
�� #
(
��# $
)
��$ %
;
��% &
}
�� 
else
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 	
[
�� 	
HttpPost
��	 
]
�� 
public
�� 
IHttpActionResult
��  '
DeleteVersionAvailability
��! :
(
��: ;#
DealerCityVersionsDTO
��; P
dealerVersions
��Q _
)
��_ `
{
�� 	
bool
�� 
	isDeleted
�� 
=
�� 
false
�� "
;
��" #
if
�� 
(
�� 
dealerVersions
�� 
.
�� 
DealerId
�� '
>
��( )
$num
��* +
)
��+ ,
{
�� 
try
�� 
{
�� 
	isDeleted
�� 
=
�� !
versionAvailability
��  3
.
��3 4'
DeleteVersionAvailability
��4 M
(
��M N
dealerVersions
��N \
.
��\ ]
DealerId
��] e
,
��e f
dealerVersions
��g u
.
��u v
BikeVersionIds��v �
)��� �
;��� �
}
�� 
catch
�� 
(
�� 
	Exception
��  
ex
��! #
)
��# $
{
�� 

ErrorClass
�� 
objErr
�� %
=
��& '
new
��( +

ErrorClass
��, 6
(
��6 7
ex
��7 9
,
��9 :
$str
��; V
)
��V W
;
��W X
}
�� 
if
�� 
(
�� 
	isDeleted
�� 
)
�� 
return
�� 
Ok
�� 
(
�� 
	isDeleted
�� '
)
��' (
;
��( )
else
�� 
return
�� 
NotFound
�� #
(
��# $
)
��$ %
;
��% &
}
�� 
else
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 	
}
�� 
}�� �*
FD:\work\bikewaleweb\BikewaleOpr.Service\Controllers\ImageController.cs
	namespace

 	
BikewaleOpr


 
.

 
Service

 
.

 
Controllers

 )
{ 
public 

class 
ImageController  
:! "
ApiController# 0
{ 
private 
readonly 
IImage 
_objImageBL  +
=, -
null. 2
;2 3
public 
ImageController 
( 
IImage %

objImageBL& 0
)0 1
{ 	
_objImageBL 
= 

objImageBL $
;$ %
} 	
[ 	
HttpPost	 
, 
Route 
( 
$str -
)- .
,. /
ResponseType0 <
(< =
typeof= C
(C D
ImageTokenDTOD Q
)Q R
)R S
]S T
public 
IHttpActionResult  
ProcessRequest! /
(/ 0
[0 1
FromBody1 9
]9 :
ImageDTO: B
objImageC K
)K L
{   	
try!! 
{"" 
if## 
(## 

ModelState## 
.## 
IsValid## &
)##& '
{$$ 
Image%% 
objImageEntity%% (
=%%) *
ImageMapper%%+ 6
.%%6 7
Convert%%7 >
(%%> ?
objImage%%? G
)%%G H
;%%H I

ImageToken&& 
token&& $
=&&% &
_objImageBL&&' 2
.&&2 3$
GenerateImageUploadToken&&3 K
(&&K L
objImageEntity&&L Z
)&&Z [
;&&[ \
ImageTokenDTO'' !
dto''" %
=''& '
ImageMapper''( 3
.''3 4
Convert''4 ;
(''; <
token''< A
)''A B
;''B C
if(( 
((( 
dto(( 
!=(( 
null(( #
&&(($ &
dto((' *
.((* +
Status((+ 1
)((1 2
{)) 
return** 
Ok** !
(**! "
dto**" %
)**% &
;**& '
}++ 
else,, 
{-- 
return.. 
InternalServerError.. 2
(..2 3
)..3 4
;..4 5
}// 
}00 
else11 
{22 
return33 

BadRequest33 %
(33% &
)33& '
;33' (
}44 
}55 
catch66 
(66 
System66 
.66 
	Exception66 #
ex66$ &
)66& '
{77 

ErrorClass88 
objErr88 !
=88" #
new88$ '

ErrorClass88( 2
(882 3
ex883 5
,885 6
String887 =
.88= >
Format88> D
(88D E
$str88E X
,88X Y

Newtonsoft88Z d
.88d e
Json88e i
.88i j
JsonConvert88j u
.88u v
SerializeObject	88v �
(
88� �
objImage
88� �
)
88� �
)
88� �
)
88� �
;
88� �
return99 
InternalServerError99 *
(99* +
)99+ ,
;99, -
}:: 
};; 	
[CC 	
RouteCC	 
(CC 
$strCC $
)CC$ %
,CC% &
HttpPostCC' /
,CC/ 0
ResponseTypeCC1 =
(CC= >
typeofCC> D
(CCD E
boolCCE I
)CCI J
)CCJ K
]CCK L
publicDD 
IHttpActionResultDD  
PostDD! %
(DD% &
[DD& '
FromBodyDD' /
]DD/ 0
ImageTokenDTODD0 =
dtoDD> A
)DDA B
{EE 	
ifFF 
(FF 

ModelStateFF 
.FF 
IsValidFF "
)FF" #
{GG 

ImageTokenHH 
entityHH !
=HH" #
ImageMapperHH$ /
.HH/ 0
ConvertHH0 7
(HH7 8
dtoHH8 ;
)HH; <
;HH< =
ifII 
(II 
entityII 
!=II 
nullII "
)II" #
{JJ 
entityKK 
=KK 
_objImageBLKK (
.KK( )
ProcessImageUploadKK) ;
(KK; <
entityKK< B
)KKB C
;KKC D
ifLL 
(LL 
entityLL 
.LL 
StatusLL %
)LL% &
{MM 
returnNN 
OkNN !
(NN! "
trueNN" &
)NN& '
;NN' (
}OO 
elsePP 
ifPP 
(PP 
entityPP #
.PP# $
ServerErrorPP$ /
)PP/ 0
{QQ 
returnRR 
InternalServerErrorRR 2
(RR2 3
)RR3 4
;RR4 5
}SS 
elseTT 
{UU 
returnVV 

BadRequestVV )
(VV) *
$strVV* F
)VVF G
;VVG H
}WW 
}XX 
elseYY 
{ZZ 
return[[ 

BadRequest[[ %
([[% &
$str[[& B
)[[B C
;[[C D
}\\ 
}]] 
else^^ 
{__ 
return`` 

BadRequest`` !
(``! "
)``" #
;``# $
}aa 
}bb 	
}cc 
}dd �
PD:\work\bikewaleweb\BikewaleOpr.Service\Controllers\Location\CitiesController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
.) *
Location* 2
{ 
public 

class 
CitiesController !
:" #
ApiController$ 1
{ 
private 
readonly 
	ILocation "
location# +
=, -
null. 2
;2 3
public 
CitiesController 
(  
)  !
{ 	
location 
= 
new 
LocationRepository -
(- .
). /
;/ 0
} 	
[ 	
HttpGet	 
, 
ResponseType 
( 
typeof %
(% &
CityNameDTO& 1
)1 2
)2 3
,3 4
Route5 :
(: ;
$str; X
)X Y
]Y Z
public   
IHttpActionResult    
GetCitiesByState  ! 1
(  1 2
UInt32  2 8
stateId  9 @
)  @ A
{!! 	
if"" 
("" 
stateId"" 
>"" 
$num"" 
)"" 
{## 
IEnumerable$$ 
<$$ 
CityNameEntity$$ *
>$$* +
cities$$, 2
=$$3 4
null$$5 9
;$$9 :
IEnumerable%% 
<%% 
CityNameDTO%% '
>%%' (
cityDtos%%) 1
=%%2 3
null%%4 8
;%%8 9
try'' 
{(( 
cities)) 
=)) 
location)) %
.))% &
GetCitiesByState))& 6
())6 7
stateId))7 >
)))> ?
;))? @
cityDtos** 
=** 

CityMapper** )
.**) *
Convert*** 1
(**1 2
cities**2 8
)**8 9
;**9 :
}++ 
catch,, 
(,, 
	Exception,,  
ex,,! #
),,# $
{-- 

ErrorClass.. 
objErr.. %
=..& '
new..( +

ErrorClass.., 6
(..6 7
ex..7 9
,..9 :
string..; A
...A B
Format..B H
(..H I
$str..I g
,..g h
stateId..i p
)..p q
)..q r
;..r s
}// 
if00 
(00 
cityDtos00 
!=00 
null00  $
)00$ %
return11 
Ok11 
(11 
cityDtos11 &
)11& '
;11' (
else22 
return33 
NotFound33 #
(33# $
)33$ %
;33% &
}44 
else55 
return66 

BadRequest66 !
(66! "
)66" #
;66# $
}77 	
}99 
}:: ��
jD:\work\bikewaleweb\BikewaleOpr.Service\Controllers\ManufacturerCampaign\ManufacturerCampaignController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
.) *"
ManufacturerCamapaigns* @
{ 
public 

class *
ManufacturerCampaignController /
:0 1
ApiController2 ?
{ 
private 
readonly 
	Interface "
." # 
ManufacturerCampaign# 7
.7 8+
IManufacturerCampaignRepository8 W$
_objManufacturerCampaignX p
=q r
nulls w
;w x
private 
readonly -
!IManufacturerReleaseMaskingNumber :0
$_objManufacturerReleaseMaskingNumber; _
=` a
nullb f
;f g
private 
readonly 
Bikewale !
.! " 
ManufacturerCampaign" 6
.6 7
	Interface7 @
.@ A+
IManufacturerCampaignRepositoryA `
_objMfgCampaigna p
=q r
nulls w
;w x
public *
ManufacturerCampaignController -
(- .
	Interface. 7
.7 8 
ManufacturerCampaign8 L
.L M+
IManufacturerCampaignRepositoryM l$
objManufacturerCampaign	m �
,
� �/
!IManufacturerReleaseMaskingNumber
� �1
#objManufacturerReleaseMaskingNumber
� �
,
� �
Bikewale
� �
.
� �"
ManufacturerCampaign
� �
.
� �
	Interface
� �
.
� �-
IManufacturerCampaignRepository
� �
objMfgCampaign
� �
)
� �
{ 	$
_objManufacturerCampaign $
=% &#
objManufacturerCampaign' >
;> ?0
$_objManufacturerReleaseMaskingNumber   0
=  1 2/
#objManufacturerReleaseMaskingNumber  3 V
;  V W
_objMfgCampaign!! 
=!! 
objMfgCampaign!! ,
;!!, -
}"" 	
[** 	
HttpGet**	 
,** 
Route** 
(** 
$str** O
)**O P
]**P Q
public++ 
IHttpActionResult++  
GetCampaigns++! -
(++- .
uint++. 2
dealerId++3 ;
)++; <
{,, 	
IEnumerable-- 
<-- %
ManufactureDealerCampaign-- 1
>--1 2
_objMfgList--3 >
=--? @
null--A E
;--E F
try.. 
{// 
if00 
(00 
dealerId00 
>00 
$num00  
)00  !
{11 
_objMfgList22 
=22  !$
_objManufacturerCampaign22" :
.22: ;&
SearchManufactureCampaigns22; U
(22U V
dealerId22V ^
)22^ _
;22_ `
return44 
Ok44 
(44 
_objMfgList44 )
)44) *
;44* +
}55 
else66 
{77 
return88 

BadRequest88 %
(88% &
)88& '
;88' (
}99 
}:: 
catch;; 
(;; 
	Exception;; 
ex;; 
);;  
{<< 

ErrorClass== 
objErr== !
===" #
new==$ '

ErrorClass==( 2
(==2 3
ex==3 5
,==5 6
$str==7 d
)==d e
;==e f
objErr>> 
.>> 
SendMail>> 
(>>  
)>>  !
;>>! "
return?? 
InternalServerError?? *
(??* +
)??+ ,
;??, -
}@@ 
}AA 	
[HH 	
ResponseTypeHH	 
(HH 
typeofHH 
(HH 
IEnumerableHH (
<HH( )+
ManufacturerCampaignDetailsListHH) H
>HHH I
)HHI J
)HHJ K
,HHK L
RouteHHM R
(HHR S
$str	HHS �
)
HH� �
]
HH� �
publicII 
IHttpActionResultII  
GetCampaignsV2II! /
(II/ 0
uintII0 4
dealerIdII5 =
,II= >
uintII? C
allActiveCampaignIID U
)IIU V
{JJ 	
IEnumerableKK 
<KK +
ManufacturerCampaignDetailsListKK 7
>KK7 8
_objMfgListKK9 D
=KKE F
nullKKG K
;KKK L
tryLL 
{MM 
ifNN 
(NN 
dealerIdNN 
>NN 
$numNN  
)NN  !
{OO 
_objMfgListPP 
=PP  !$
_objManufacturerCampaignPP" :
.PP: ;#
GetManufactureCampaignsPP; R
(PPR S
dealerIdPPS [
,PP[ \
allActiveCampaignPP] n
)PPn o
;PPo p
returnRR 
OkRR 
(RR 
SearchMapperRR *
.RR* +
ConvertRR+ 2
(RR2 3
_objMfgListRR3 >
)RR> ?
)RR? @
;RR@ A
}SS 
elseTT 
{UU 
returnVV 

BadRequestVV %
(VV% &
)VV& '
;VV' (
}WW 
}XX 
catchYY 
(YY 
	ExceptionYY 
exYY 
)YY  
{ZZ 

ErrorClass[[ 
objErr[[ !
=[[" #
new[[$ '

ErrorClass[[( 2
([[2 3
ex[[3 5
,[[5 6
string[[7 =
.[[= >
Format[[> D
([[D E
$str	[[E �
,
[[� �
dealerId
[[� �
)
[[� �
)
[[� �
;
[[� �
return]] 
InternalServerError]] *
(]]* +
)]]+ ,
;]], -
}^^ 
}__ 	
[ff 	
HttpPostff	 
,ff 
Routeff 
(ff 
$strff t
)fft u
]ffu v
publicgg 
IHttpActionResultgg   
UpdateCampaignStatusgg! 5
(gg5 6
uintgg6 :

campaignIdgg; E
,ggE F
boolggG K
isactiveggL T
)ggT U
{hh 	
boolii 
	isSuccessii 
=ii 
falseii "
;ii" #
tryjj 
{kk 
	isSuccessll 
=ll $
_objManufacturerCampaignll 4
.ll4 5 
UpdateCampaignStatusll5 I
(llI J

campaignIdllJ T
,llT U
isactivellV ^
)ll^ _
;ll_ `
}mm 
catchnn 
(nn 
	Exceptionnn 
exnn 
)nn  
{oo 

ErrorClasspp 
objErrpp !
=pp" #
newpp$ '

ErrorClasspp( 2
(pp2 3
expp3 5
,pp5 6
$strpp7 l
)ppl m
;ppm n
objErrqq 
.qq 
SendMailqq 
(qq  
)qq  !
;qq! "
returnrr 
InternalServerErrorrr *
(rr* +
)rr+ ,
;rr, -
}ss 
returntt 
Oktt 
(tt 
	isSuccesstt 
)tt  
;tt  !
}uu 	
[ww 	
HttpPostww	 
,ww 
Routeww 
(ww 
$strww u
)wwu v
]wwv w
publicxx 
IHttpActionResultxx  "
UpdateCampaignStatusV2xx! 7
(xx7 8
uintxx8 <

campaignIdxx= G
,xxG H
uintxxI M
statusxxN T
)xxT U
{yy 	
boolzz 
	isSuccesszz 
=zz 
falsezz "
;zz" #
try{{ 
{|| 
	isSuccess}} 
=}} $
_objManufacturerCampaign}} 4
.}}4 5 
UpdateCampaignStatus}}5 I
(}}I J

campaignId}}J T
,}}T U
status}}V \
)}}\ ]
;}}] ^
}~~ 
catch 
( 
	Exception 
ex 
)  
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
��5 6
string
��7 =
.
��= >
Format
��> D
(
��D E
$str��E �
,��� �

campaignId��� �
,��� �
status��� �
)��� �
)��� �
;��� �
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
return
�� 
Ok
�� 
(
�� 
	isSuccess
�� 
)
��  
;
��  !
}
�� 	
[
�� 	
HttpGet
��	 
]
�� 
public
�� 
IHttpActionResult
��  %
GetDealerMaskingNumbers
��! 8
(
��8 9
uint
��9 =
dealerId
��> F
)
��F G
{
�� 	
try
�� 
{
�� 
IEnumerable
�� 
<
�� 
MaskingNumber
�� )
>
��) *
numbersList
��+ 6
=
��7 8
null
��9 =
;
��= >
using
�� 
(
�� 
IUnityContainer
�� &
	container
��' 0
=
��1 2
new
��3 6
UnityContainer
��7 E
(
��E F
)
��F G
)
��G H
{
�� 
	container
�� 
.
�� 
RegisterType
�� *
<
��* +
IContractCampaign
��+ <
,
��< =
ContractCampaign
��> N
>
��N O
(
��O P
)
��P Q
;
��Q R
IContractCampaign
�� %
objCC
��& +
=
��, -
	container
��. 7
.
��7 8
Resolve
��8 ?
<
��? @
IContractCampaign
��@ Q
>
��Q R
(
��R S
)
��S T
;
��T U
numbersList
�� 
=
��  !
objCC
��" '
.
��' ("
GetAllMaskingNumbers
��( <
(
��< =
dealerId
��= E
)
��E F
;
��F G
if
�� 
(
�� 
numbersList
�� #
!=
��$ &
null
��' +
&&
��, .
numbersList
��/ :
.
��: ;
Count
��; @
(
��@ A
)
��A B
>
��C D
$num
��E F
)
��F G
{
�� 
return
�� 
Ok
�� !
(
��! "
numbersList
��" -
)
��- .
;
��. /
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
��5 6
$str
��7 g
)
��g h
;
��h i
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
�� 
null
�� 
;
�� 
}
�� 	
[
�� 	
HttpPost
��	 
]
�� 
public
�� 
IHttpActionResult
��  
ReleaseNumber
��! .
(
��. /
uint
��/ 3
dealerId
��4 <
,
��< =
int
��> A

campaignId
��B L
,
��L M
string
��N T
maskingNumber
��U b
,
��b c
int
��d g
userId
��h n
)
��n o
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
�� 
	isSuccess
�� 
=
�� 2
$_objManufacturerReleaseMaskingNumber
�� @
.
��@ A
ReleaseNumber
��A N
(
��N O
dealerId
��O W
,
��W X

campaignId
��Y c
,
��c d
maskingNumber
��e r
,
��r s
userId
��t z
)
��z {
;
��{ |
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
�� 
	isSuccess
�� 
=
�� 
false
�� !
;
��! "

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
��5 6
$str
��7 [
)
��[ \
;
��\ ]
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
Ok
�� 
(
�� 
	isSuccess
�� 
)
��  
;
��  !
}
�� 	
[
�� 	
HttpGet
��	 
,
�� 
Route
�� 
(
�� 
$str
�� K
)
��K L
]
��L M
public
�� 
IHttpActionResult
��  
GetBikeModels
��! .
(
��. /
uint
��/ 3
makeId
��4 :
)
��: ;
{
�� 	
IEnumerable
�� 
<
�� 
BikeModelEntity
�� '
>
��' (
_models
��) 0
=
��1 2
null
��3 7
;
��7 8
try
�� 
{
�� 
if
�� 
(
�� 
makeId
�� 
>
�� 
$num
�� 
)
�� 
{
�� 
_models
�� 
=
�� 
_objMfgCampaign
�� -
.
��- .
GetBikeModels
��. ;
(
��; <
makeId
��< B
)
��B C
;
��C D
if
�� 
(
�� 
_models
�� 
!=
��  "
null
��# '
&&
��( *
_models
��+ 2
.
��2 3
Count
��3 8
(
��8 9
)
��9 :
>
��; <
$num
��= >
)
��> ?
{
�� 
IEnumerable
�� #
<
��# $
BikeModelDTO
��$ 0
>
��0 1
objModelsDTO
��2 >
=
��? @
BikeModelsMapper
��A Q
.
��Q R
	ConvertV2
��R [
(
��[ \
_models
��\ c
)
��c d
;
��d e
return
�� 
Ok
�� !
(
��! "
objModelsDTO
��" .
)
��. /
;
��/ 0
}
�� 
else
�� 
{
�� 
return
�� 
Ok
�� !
(
��! "
)
��" #
;
��# $
}
�� 
}
�� 
else
�� 
{
�� 
return
�� 

BadRequest
�� %
(
��% &
)
��& '
;
��' (
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
��5 6
string
��7 =
.
��= >
Format
��> D
(
��D E
$str��E �
,��� �
makeId��� �
)��� �
)��� �
;��� �
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
}
�� 	
[
�� 	
HttpGet
��	 
,
�� 
Route
�� 
(
�� 
$str
�� M
)
��M N
]
��N O
public
�� 
IHttpActionResult
��  
GetCitiesByState
��! 1
(
��1 2
uint
��2 6
stateId
��7 >
)
��> ?
{
�� 	
IEnumerable
�� 
<
�� 

CityEntity
�� "
>
��" #
_cities
��$ +
=
��, -
null
��. 2
;
��2 3
try
�� 
{
�� 
if
�� 
(
�� 
stateId
�� 
>
�� 
$num
�� 
)
��  
{
�� 
_cities
�� 
=
�� 
_objMfgCampaign
�� -
.
��- .
GetCitiesByState
��. >
(
��> ?
stateId
��? F
)
��F G
;
��G H
if
�� 
(
�� 
_cities
�� 
!=
��  "
null
��# '
&&
��( *
_cities
��+ 2
.
��2 3
Count
��3 8
(
��8 9
)
��9 :
>
��; <
$num
��= >
)
��> ?
{
�� 
IEnumerable
�� #
<
��# $
CityDTO
��$ +
>
��+ ,
objCitiesDTO
��- 9
=
��: ;

CityMapper
��< F
.
��F G
Convert
��G N
(
��N O
_cities
��O V
)
��V W
;
��W X
return
�� 
Ok
�� !
(
��! "
objCitiesDTO
��" .
)
��. /
;
��/ 0
}
�� 
else
�� 
{
�� 
return
�� 
Ok
�� !
(
��! "
)
��" #
;
��# $
}
�� 
}
�� 
else
�� 
{
�� 
return
�� 

BadRequest
�� %
(
��% &
)
��& '
;
��' (
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
��5 6
string
��7 =
.
��= >
Format
��> D
(
��D E
$str��E �
,��� �
stateId��� �
)��� �
)��� �
;��� �
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
}
�� 	
[
�� 	
HttpPost
��	 
,
�� 
Route
�� 
(
�� 
$str
�� A
)
��A B
]
��B C
public
�� 
IHttpActionResult
��  

DeleteRule
��! +
(
��+ ,
[
��, -
FromBody
��- 5
]
��5 6'
ManufacturerRuleEntityDTO
��6 O

ruleEntity
��P Z
)
��Z [
{
�� 	
bool
�� 
	isSuccess
�� 
=
�� 
false
�� "
;
��" #
try
�� 
{
�� 
if
�� 
(
�� 

ruleEntity
�� 
!=
�� !
null
��" &
)
��& '
	isSuccess
�� 
=
�� 
_objMfgCampaign
��  /
.
��/ 0-
DeleteManufacturerCampaignRules
��0 O
(
��O P

ruleEntity
��P Z
.
��Z [

CampaignId
��[ e
,
��e f

ruleEntity
��g q
.
��q r
ModelId
��r y
,
��y z

ruleEntity��{ �
.��� �
StateId��� �
,��� �

ruleEntity��� �
.��� �
CityId��� �
,��� �

ruleEntity��� �
.��� �
UserId��� �
,��� �

ruleEntity��� �
.��� �

IsAllIndia��� �
)��� �
;��� �
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
��7 b
)
��b c
;
��c d
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
return
�� 
Ok
�� 
(
�� 
	isSuccess
�� 
)
��  
;
��  !
}
�� 	
[
�� 	
HttpPost
��	 
,
�� 
Route
�� 
(
�� 
$str
�� T
)
��T U
]
��U V
public
�� 
IHttpActionResult
��  *
ResetTotalLeadDeliveredCount
��! =
(
��= >
uint
��> B

campaignId
��C M
,
��M N
uint
��O S
userId
��T Z
)
��Z [
{
�� 	
bool
�� 
	isSuccess
�� 
=
�� 
false
�� "
;
��" #
try
�� 
{
�� 
if
�� 
(
�� 

campaignId
�� 
>
��  
$num
��! "
&&
��# %
userId
��& ,
>
��- .
$num
��/ 0
)
��0 1
{
�� 
	isSuccess
�� 
=
�� 
_objMfgCampaign
��  /
.
��/ 0%
ResetTotalLeadDelivered
��0 G
(
��G H

campaignId
��H R
,
��R S
userId
��T Z
)
��Z [
;
��[ \
}
�� 
else
�� 
{
�� 
return
�� 

BadRequest
�� %
(
��% &
$str
��& 5
)
��5 6
;
��6 7
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
�� 
err
�� 
=
��  
new
��! $

ErrorClass
��% /
(
��/ 0
ex
��0 2
,
��2 3
String
��4 :
.
��: ;
Format
��; A
(
��A B
$str��B �
,��� �

campaignId��� �
,��� �
userId��� �
)��� �
)��� �
;��� �
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
return
�� 
Ok
�� 
(
�� 
	isSuccess
�� 
)
��  
;
��  !
}
�� 	
}
�� 
}�� �
TD:\work\bikewaleweb\BikewaleOpr.Service\Controllers\PageMetas\PageMetasController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
.) *
	PageMetas* 3
{ 
public 

class 
PageMetasController $
:% &
ApiController' 4
{ 
private 
readonly 

IPageMetas #

_pageMetas$ .
=/ 0
null1 5
;5 6
public 
PageMetasController "
(" #

IPageMetas# -
	pageMetas. 7
)7 8
{ 	

_pageMetas 
= 
	pageMetas "
;" #
} 	
[$$ 	
HttpPost$$	 
,$$ 
Route$$ 
($$ 
$str$$ Y
)$$Y Z
]$$Z [
public%% 
IHttpActionResult%%   
UpdatePageMetaStatus%%! 5
(%%5 6
uint%%6 :

pageMetaId%%; E
,%%E F
ushort%%G M
status%%N T
,%%T U
uint%%V Z
modelId%%[ b
,%%b c
uint%%d h
makeId%%i o
)%%o p
{&& 	
try'' 
{(( 
bool)) 
result)) 
=)) 

_pageMetas)) (
.))( ) 
UpdatePageMetaStatus))) =
())= >

pageMetaId))> H
,))H I
status))J P
)))P Q
;))Q R
if** 
(** 
result** 
)** 
{++ 
if,, 
(,, 
modelId,, 
>,,  !
$num,," #
),,# $
MemCachedUtil-- %
.--% &
Remove--& ,
(--, -
$str--- >
+--? @
modelId--A H
)--H I
;--I J
MemCachedUtil// !
.//! "
Remove//" (
(//( )
$str//) :
+//; <
makeId//= C
)//C D
;//D E
return11 
Ok11 
(11 
true11 "
)11" #
;11# $
}22 
else33 
{44 
return55 
NotFound55 #
(55# $
)55$ %
;55% &
}66 
}77 
catch88 
(88 
	Exception88 
ex88 
)88  
{99 

ErrorClass:: 
objErr:: !
=::" #
new::$ '

ErrorClass::( 2
(::2 3
ex::3 5
,::5 6
String::7 =
.::= >
Format::> D
(::D E
$str::E x
,::x y

pageMetaId	::z �
,
::� �
status
::� �
)
::� �
)
::� �
;
::� �
return;; 
InternalServerError;; *
(;;* +
);;+ ,
;;;, -
}<< 
}== 	
}>> 
}?? �?
\D:\work\bikewaleweb\BikewaleOpr.Service\Controllers\ServiceCenter\ServiceCenterController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
.) *
ServiceCenter* 7
{ 
public 

class #
ServiceCenterController (
:) *
ApiController+ 8
{ 
private 
readonly 
IServiceCenter '
_IServiceCenter( 7
=8 9
null: >
;> ?
public #
ServiceCenterController &
(& '
IServiceCenter' 5
serviceCenter6 C
)C D
{ 	
_IServiceCenter 
= 
serviceCenter +
;+ ,
} 	
['' 	
HttpGet''	 
,'' 
Route'' 
('' 
$str'' A
)''A B
,''B C
ResponseType''D P
(''P Q
typeof''Q W
(''W X
IEnumerable''X c
<''c d
CityBase''d l
>''l m
)''m n
)''n o
]''o p
public(( 
IHttpActionResult((  
Get((! $
((($ %
uint((% )
makeId((* 0
)((0 1
{)) 	
IEnumerable** 
<** 
CityEntityBase** &
>**& '
objcityList**( 3
=**4 5
null**6 :
;**: ;
if++ 
(++ 
makeId++ 
>++ 
$num++ 
)++ 
{,, 
try-- 
{.. 
objcityList// 
=//  !
_IServiceCenter//" 1
.//1 2"
GetServiceCenterCities//2 H
(//H I
makeId//I O
)//O P
;//P Q
if00 
(00 
objcityList00 #
!=00$ &
null00' +
)00+ ,
{11 
IEnumerable22 #
<22# $
CityBase22$ ,
>22, -
cityList22. 6
=227 8
ServiceCenterMapper229 L
.22L M
Convert22M T
(22T U
objcityList22U `
)22` a
;22a b
return33 
Ok33 !
(33! "
cityList33" *
)33* +
;33+ ,
}44 
}66 
catch77 
(77 
	Exception77  
ex77! #
)77# $
{88 
Bikewale99 
.99 
Notifications99 *
.99* +

ErrorClass99+ 5
objErr996 <
=99= >
new99? B

ErrorClass99C M
(99M N
ex99N P
,99P Q
$str	99R �
)
99� �
;
99� �
return:: 
InternalServerError:: .
(::. /
)::/ 0
;::0 1
};; 
}<< 
else== 
{>> 
return?? 

BadRequest?? !
(??! "
)??" #
;??# $
}AA 
returnBB 
NotFoundBB 
(BB 
)BB 
;BB 
}CC 	
[NN 	
HttpGetNN	 
,NN 
RouteNN 
(NN 
$strNN ^
)NN^ _
,NN_ `
ResponseTypeNNa m
(NNm n
typeofNNn t
(NNt u
IEnumerable	NNu �
<
NN� �"
ServiceCenterBaseDTO
NN� �
>
NN� �
)
NN� �
)
NN� �
]
NN� �
publicOO 
IHttpActionResultOO  #
GetServiceCenterDetailsOO! 8
(OO8 9
uintOO9 =
cityIdOO> D
,OOD E
uintOOF J
makeIdOOK Q
,OOQ R
sbyteOOS X
activeStatusOOY e
)OOe f
{PP 	
ServiceCenterDataQQ 
objServiceCenterQQ .
=QQ/ 0
nullQQ1 5
;QQ5 6
IEnumerableRR 
<RR  
ServiceCenterDetailsRR ,
>RR, - 
objServiceCenterListRR. B
=RRC D
nullRRE I
;RRI J
IEnumerableSS 
<SS  
ServiceCenterBaseDTOSS ,
>SS, -
serviceCenterListSS. ?
=SS@ A
nullSSB F
;SSF G
ifUU 
(UU 
cityIdUU 
>UU 
$numUU 
&&UU 
makeIdUU $
>UU% &
$numUU' (
)UU( )
{VV 
tryWW 
{XX 
objServiceCenterYY $
=YY% &
_IServiceCenterYY' 6
.YY6 7'
GetServiceCentersByCityMakeYY7 R
(YYR S
cityIdYYS Y
,YYY Z
makeIdYY[ a
,YYa b
activeStatusYYc o
)YYo p
;YYp q
if[[ 
([[ 
objServiceCenter[[ (
.[[( )
ServiceCenters[[) 7
!=[[8 :
null[[; ?
)[[? @
{\\  
objServiceCenterList]] ,
=]]- .
objServiceCenter]]/ ?
.]]? @
ServiceCenters]]@ N
;]]N O
if__ 
(__  
objServiceCenterList__ 0
!=__1 3
null__4 8
)__8 9
{`` 
serviceCenterListaa -
=aa. /
ServiceCenterMapperaa0 C
.aaC D
ConvertaaD K
(aaK L 
objServiceCenterListaaL `
)aa` a
;aaa b
returnbb "
Okbb# %
(bb% &
serviceCenterListbb& 7
)bb7 8
;bb8 9
}cc 
}ff 
}hh 
catchii 
(ii 
	Exceptionii  
exii! #
)ii# $
{jj 

ErrorClasskk 
objErrkk %
=kk& '
newkk( +

ErrorClasskk, 6
(kk6 7
exkk7 9
,kk9 :
$str	kk; �
)
kk� �
;
kk� �
returnll 
InternalServerErrorll .
(ll. /
)ll/ 0
;ll0 1
}mm 
}nn 
elseoo 
{pp 
returnqq 

BadRequestqq !
(qq! "
)qq" #
;qq# $
}rr 
returnss 
NotFoundss 
(ss 
)ss 
;ss 
}tt 	
[}} 	
HttpGet}}	 
,}} 
Route}} 
(}} 
$str}} g
)}}g h
]}}h i
public~~ 
IHttpActionResult~~  %
UpdateServiceCenterStatus~~! :
(~~: ;
uint~~; ?
cityId~~@ F
,~~F G
uint~~H L
makeId~~M S
,~~S T
uint~~U Y
serviceCenterId~~Z i
,~~i j
string~~k q
currentUserId~~r 
)	~~ �
{ 	
bool
�� 
status
�� 
=
�� 
false
�� 
;
��  
if
�� 
(
�� 
serviceCenterId
�� 
>
��  
$num
��  !
)
��! "
{
�� 
try
�� 
{
�� 
status
�� 
=
�� 
_IServiceCenter
�� ,
.
��, -'
UpdateServiceCenterStatus
��- F
(
��F G
cityId
��G M
,
��M N
makeId
��O U
,
��U V
serviceCenterId
��V e
,
��e f
currentUserId
��g t
)
��t u
;
��u v
if
�� 
(
�� 
status
�� 
)
�� 
{
�� 
return
�� 
Ok
�� !
(
��! "
status
��" (
)
��( )
;
��) *
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
��  
ex
��! #
)
��# $
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
��6 7
ex
��7 9
,
��9 :
$str��; �
)��� �
;��� �
return
�� !
InternalServerError
�� .
(
��. /
)
��/ 0
;
��0 1
}
�� 
}
�� 
else
�� 
{
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 
return
�� 
NotFound
�� 
(
�� 
)
�� 
;
�� 
}
�� 	
}
�� 
}�� �;
OD:\work\bikewaleweb\BikewaleOpr.Service\Controllers\Used\UsedBikesController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
.) *
Used* .
{		 
public 

class 
UsedBikesController $
:% &
ApiController' 4
{ 
private 
readonly 

ISellBikes #
_objSellBikes$ 1
=2 3
null4 8
;8 9
private 
readonly !
IBikeModelsRepository .
_objBikeModels/ =
=> ?
null@ D
;D E
public 
UsedBikesController "
(" #

ISellBikes# -
objSellBikes. :
,: ;!
IBikeModelsRepository< Q
objBikeModelsR _
)_ `
{ 	
_objSellBikes 
= 
objSellBikes (
;( )
_objBikeModels 
= 
objBikeModels *
;* +
} 	
[!! 	
HttpPost!!	 
,!! 
Route!! 
(!! 
$str!! F
)!!F G
]!!G H
public"" 
IHttpActionResult""  
SaveEditedInquiry""! 2
(""2 3
uint""3 7
	inquiryId""8 A
,""A B
short""C H

isApproved""I S
,""S T
int""U X

approvedBy""Y c
,""c d
string""e k
	profileId""l u
,""u v
string""w }
bikeName	""~ �
,
""� �
uint
""� �
modelId
""� �
)
""� �
{## 	
bool$$ 
	isSuccess$$ 
=$$ 
false$$ "
;$$" #
try%% 
{&& 
	isSuccess'' 
='' 
_objSellBikes'' )
.'') *
SaveEditedInquiry''* ;
(''; <
	inquiryId''< E
,''E F

isApproved''G Q
,''Q R

approvedBy''S ]
,''] ^
	profileId''_ h
,''h i
bikeName''j r
,''r s
modelId''t {
)''{ |
;''| }
}(( 
catch)) 
()) 
	Exception)) 
ex)) 
)))  
{** 

ErrorClass++ 
objErr++ !
=++" #
new++$ '

ErrorClass++( 2
(++2 3
ex++3 5
,++5 6
String++7 =
.++= >
Format++> D
(++D E
$str	++E �
,
++� �
	inquiryId
++� �
,
++� �

isApproved
++� �
,
++� �

approvedBy
++� �
,
++� �
	profileId
++� �
)
++� �
)
++� �
;
++� �
objErr,, 
.,, 
SendMail,, 
(,,  
),,  !
;,,! "
return-- 
InternalServerError-- *
(--* +
)--+ ,
;--, -
}.. 
return// 
Ok// 
(// 
	isSuccess// 
)//  
;//  !
}00 	
[88 	
HttpPost88	 
,88 
Route88 
(88 
$str88 B
)88B C
]88C D
public99 
IHttpActionResult99  
FetchPhotoId99! -
(99- .
[99. /
FromBody99/ 7
]997 8$
UsedBikeModelImageEntity998 P
objModelImageEntity99Q d
)99d e
{:: 	
try;; 
{<< 
if== 
(== 
objModelImageEntity== '
!===( *
null==+ /
&&==0 2

ModelState==3 =
.=== >
IsValid==> E
)==E F
{>> 
return?? 
Ok?? 
(?? 
_objBikeModels?? ,
.??, -
FetchPhotoId??- 9
(??9 :
objModelImageEntity??: M
)??M N
)??N O
;??O P
}@@ 
elseAA 
{BB 
returnCC 

BadRequestCC %
(CC% &
)CC& '
;CC' (
}DD 
}EE 
catchFF 
(FF 
	ExceptionFF 
exFF 
)FF  
{GG 

ErrorClassHH 
objErrHH !
=HH" #
newHH$ '

ErrorClassHH( 2
(HH2 3
exHH3 5
,HH5 6
stringHH7 =
.HH= >
FormatHH> D
(HHD E
$str	HHE �
,
HH� �!
objModelImageEntity
HH� �
)
HH� �
)
HH� �
;
HH� �
returnII 
InternalServerErrorII *
(II* +
)II+ ,
;II, -
}JJ 
}KK 	
[SS 	
HttpPostSS	 
,SS 
RouteSS 
(SS 
$strSS W
)SSW X
]SSX Y
publicTT 
IHttpActionResultTT  $
DeleteUsedBikeModelImageTT! 9
(TT9 :
uintTT: >
modelIdTT? F
)TTF G
{UU 	
tryVV 
{WW 
ifXX 
(XX 
modelIdXX 
>XX 
$numXX 
)XX  
{YY 
returnZZ 
OkZZ 
(ZZ 
_objBikeModelsZZ ,
.ZZ, -$
DeleteUsedBikeModelImageZZ- E
(ZZE F
modelIdZZF M
)ZZM N
)ZZN O
;ZZO P
}[[ 
else\\ 
{]] 
return^^ 

BadRequest^^ %
(^^% &
)^^& '
;^^' (
}__ 
}`` 
catchaa 
(aa 
	Exceptionaa 
exaa 
)aa  
{bb 

ErrorClasscc 
objErrcc !
=cc" #
newcc$ '

ErrorClasscc( 2
(cc2 3
excc3 5
,cc5 6
stringcc7 =
.cc= >
Formatcc> D
(ccD E
$strccE 
,	cc �
modelId
cc� �
)
cc� �
)
cc� �
;
cc� �
returndd 
InternalServerErrordd *
(dd* +
)dd+ ,
;dd, -
}ee 
}ff 	
[oo 	
HttpPostoo	 
,oo 
Routeoo 
(oo 
$stroo C
)ooC D
]ooD E
publicpp 
IHttpActionResultpp  
UpdateInquiryAsSoldpp! 4
(pp4 5
uintpp5 9
	inquiryIdpp: C
)ppC D
{qq 	
ifss 
(ss 
	inquiryIdss 
>ss 
$numss  !
)ss! "
{tt 
tryuu 
{vv 
boolww 
successww $
=ww% &
falseww' ,
;ww, -
successxx 
=xx  !
_objBikeModelsxx" 0
.xx0 1
UpdateInquiryAsSoldxx1 D
(xxD E
	inquiryIdxxE N
)xxN O
;xxO P
returnyy 
Okyy !
(yy! "
successyy" )
)yy) *
;yy* +
}zz 
catch{{ 
({{ 
	Exception{{ $
ex{{% '
){{' (
{|| 

ErrorClass}} "
objErr}}# )
=}}* +
new}}, /

ErrorClass}}0 :
(}}: ;
ex}}; =
,}}= >
string}}? E
.}}E F
Format}}F L
(}}L M
$str	}}M �
,
}}� �
	inquiryId
}}� �
)
}}� �
)
}}� �
;
}}� �
return~~ 
InternalServerError~~ 2
(~~2 3
)~~3 4
;~~4 5
} 
}
�� 
else
�� 
{
�� 
return
�� 

BadRequest
�� !
(
��! "
)
��" #
;
��# $
}
�� 
}
�� 	
}
�� 
}�� �|
XD:\work\bikewaleweb\BikewaleOpr.Service\Controllers\UserReviews\UserReviewsController.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
Controllers )
.) *
UserReviews* 5
{ 
public 

class !
UserReviewsController &
:' (
ApiController) 6
{ 
private 
readonly "
IUserReviewsRepository /
_userReviewsRepo0 @
=A B
nullC G
;G H
public !
UserReviewsController $
($ %"
IUserReviewsRepository% ;
userReviewsRepo< K
)K L
{ 	
_userReviewsRepo 
= 
userReviewsRepo .
;. /
} 	
[++ 	
HttpPost++	 
,++ 
Route++ 
(++ 
$str++ F
)++F G
]++G H
public,, 
IHttpActionResult,,  #
UpdateUserReviewsStatus,,! 8
(,,8 9$
UpdateReviewsInputEntity,,9 Q
inputs,,R X
),,X Y
{-- 	
try.. 
{// 
if00 
(00 
inputs00 
!=00 
null00 "
&&00# %
inputs00& ,
.00, -
ReviewId00- 5
>006 7
$num008 9
)009 :
{11 
uint22 
oldTableReviewId22 )
=22* +
_userReviewsRepo22, <
.22< =#
UpdateUserReviewsStatus22= T
(22T U
inputs22U [
.22[ \
ReviewId22\ d
,22d e
inputs22f l
.22l m
ReviewStatus22m y
,22y z
inputs	22{ �
.
22� �
ModeratorId
22� �
,
22� �
inputs
22� �
.
22� �!
DisapprovalReasonId
22� �
,
22� �
inputs
22� �
.
22� �
Review
22� �
,
22� �
inputs
22� �
.
22� �
ReviewTitle
22� �
,
22� �
inputs
22� �
.
22� �

ReviewTips
22� �
,
22� �
inputs
22� �
.
22� �
IsShortListed
22� �
)
22� �
;
22� �
if55 
(55 
inputs55 
.55 
ReviewStatus55 +
.55+ ,
Equals55, 2
(552 3
ReviewsStatus553 @
.55@ A
Approved55A I
)55I J
)55J K
{66 
string77 
	reviewUrl77 (
=77) *
string77+ 1
.771 2
Format772 8
(778 9
$str779 Y
,77Y Z
BWConfiguration77[ j
.77j k
Instance77k s
.77s t
	BwHostUrl77t }
,77} ~
inputs	77 �
.
77� �
MakeMaskingName
77� �
,
77� �
inputs
77� �
.
77� �
ModelMaskingName
77� �
,
77� �
inputs
77� �
.
77� �
ReviewId
77� �
)
77� �
;
77� �
ComposeEmailBase99 (
objBase99) 0
=991 2
new993 6
ReviewApprovalEmail997 J
(99J K
inputs99K Q
.99Q R
CustomerName99R ^
,99^ _
	reviewUrl99` i
,99i j
inputs99k q
.99q r
BikeName99r z
)99z {
;99{ |
objBase:: 
.::  
Send::  $
(::$ %
inputs::% +
.::+ ,
CustomerEmail::, 9
,::9 :
$str::; l
)::l m
;::m n
};; 
else<< 
if<< 
(<< 
inputs<< #
.<<# $
ReviewStatus<<$ 0
.<<0 1
Equals<<1 7
(<<7 8
ReviewsStatus<<8 E
.<<E F
	Discarded<<F O
)<<O P
)<<P Q
{== 
ComposeEmailBase>> (
objBase>>) 0
=>>1 2
new>>3 6 
ReviewRejectionEmail>>7 K
(>>K L
inputs>>L R
.>>R S
CustomerName>>S _
,>>_ `
inputs>>a g
.>>g h
BikeName>>h p
)>>p q
;>>q r
objBase?? 
.??  
Send??  $
(??$ %
inputs??% +
.??+ ,
CustomerEmail??, 9
,??9 :
$str??; m
)??m n
;??n o
}@@ 
ifBB 
(BB 
inputsBB 
.BB 
ModelIdBB &
>BB' (
$numBB) *
&&BB+ -
inputsBB. 4
.BB4 5
ReviewStatusBB5 A
!=BBB D
nullBBE I
)BBI J
{CC 
MemCachedUtilEE %
.EE% &
RemoveEE& ,
(EE, -
stringEE- 3
.EE3 4
FormatEE4 :
(EE: ;
$strEE; f
,EEf g
inputsEEh n
.EEn o
ModelIdEEo v
)EEv w
)EEw x
;EEx y
MemCachedUtilFF %
.FF% &
RemoveFF& ,
(FF, -
stringFF- 3
.FF3 4
FormatFF4 :
(FF: ;
$strFF; f
,FFf g
inputsFFh n
.FFn o
ModelIdFFo v
)FFv w
)FFw x
;FFx y
MemCachedUtilGG %
.GG% &
RemoveGG& ,
(GG, -
stringGG- 3
.GG3 4
FormatGG4 :
(GG: ;
$strGG; f
,GGf g
inputsGGh n
.GGn o
ModelIdGGo v
)GGv w
)GGw x
;GGx y
MemCachedUtilHH %
.HH% &
RemoveHH& ,
(HH, -
stringHH- 3
.HH3 4
FormatHH4 :
(HH: ;
$strHH; f
,HHf g
inputsHHh n
.HHn o
ModelIdHHo v
)HHv w
)HHw x
;HHx y
MemCachedUtilII %
.II% &
RemoveII& ,
(II, -
stringII- 3
.II3 4
FormatII4 :
(II: ;
$strII; f
,IIf g
inputsIIh n
.IIn o
ModelIdIIo v
)IIv w
)IIw x
;IIx y
MemCachedUtilJJ %
.JJ% &
RemoveJJ& ,
(JJ, -
stringJJ- 3
.JJ3 4
FormatJJ4 :
(JJ: ;
$strJJ; V
,JJV W
inputsJJX ^
.JJ^ _
ModelIdJJ_ f
)JJf g
)JJg h
;JJh i
MemCachedUtilKK %
.KK% &
RemoveKK& ,
(KK, -
stringKK- 3
.KK3 4
FormatKK4 :
(KK: ;
$strKK; `
,KK` a
inputsKKb h
.KKh i
ModelIdKKi p
)KKp q
)KKq r
;KKr s
MemCachedUtilLL %
.LL% &
RemoveLL& ,
(LL, -
stringLL- 3
.LL3 4
FormatLL4 :
(LL: ;
$strLL; O
,LLO P
inputsLLQ W
.LLW X
ModelIdLLX _
)LL_ `
)LL` a
;LLa b
MemCachedUtilMM %
.MM% &
RemoveMM& ,
(MM, -
stringMM- 3
.MM3 4
FormatMM4 :
(MM: ;
$strMM; S
,MMS T
inputsMMU [
.MM[ \
ModelIdMM\ c
)MMc d
)MMd e
;MMe f
MemCachedUtilNN %
.NN% &
RemoveNN& ,
(NN, -
stringNN- 3
.NN3 4
FormatNN4 :
(NN: ;
$strNN; X
,NNX Y
inputsNNZ `
.NN` a
ModelIdNNa h
)NNh i
)NNi j
;NNj k
MemCachedUtilOO %
.OO% &
RemoveOO& ,
(OO, -
$strOO- ?
)OO? @
;OO@ A
MemCachedUtilPP %
.PP% &
RemovePP& ,
(PP, -
$strPP- E
)PPE F
;PPF G
}QQ 
}SS 
elseTT 
{UU 
returnVV 

BadRequestVV %
(VV% &
)VV& '
;VV' (
}WW 
}XX 
catchYY 
(YY 
	ExceptionYY 
exYY 
)YY  
{ZZ 

ErrorClass[[ 
objErr[[ !
=[[" #
new[[$ '

ErrorClass[[( 2
([[2 3
ex[[3 5
,[[5 6
$str[[7 |
)[[| }
;[[} ~
return]] 
InternalServerError]] *
(]]* +
)]]+ ,
;]], -
}^^ 
return`` 
Ok`` 
(`` 
true`` 
)`` 
;`` 
}bb 	
[kk 	
HttpGetkk	 
,kk 
Routekk 
(kk 
$strkk @
)kk@ A
]kkA B
publicll 
IHttpActionResultll   
GetUserReviewSummaryll! 5
(ll5 6
uintll6 :
reviewIdll; C
)llC D
{mm 	
UserReviewSummarynn 
objUserReviewnn +
=nn, -
nullnn. 2
;nn2 3 
UserReviewSummaryDtooo  
objDTOUserReviewoo! 1
=oo2 3
nulloo4 8
;oo8 9
trypp 
{qq 
objUserReviewrr 
=rr 
_userReviewsReporr  0
.rr0 1 
GetUserReviewSummaryrr1 E
(rrE F
reviewIdrrF N
)rrN O
;rrO P
iftt 
(tt 
objUserReviewtt !
!=tt" $
nulltt% )
)tt) *
{uu 
objDTOUserReviewww $
=ww% &
newww' * 
UserReviewSummaryDtoww+ ?
(ww? @
)ww@ A
;wwA B
objDTOUserReviewxx $
=xx% &
UserReviewsMapperxx' 8
.xx8 9
Convertxx9 @
(xx@ A
objUserReviewxxA N
)xxN O
;xxO P
returnzz 
Okzz 
(zz 
objDTOUserReviewzz .
)zz. /
;zz/ 0
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
,5 6
$str7 w
)w x
;x y
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
return
�� 
NotFound
�� 
(
�� 
)
�� 
;
�� 
}
�� 	
[
�� 	
HttpPost
��	 
,
�� 
Route
�� 
(
�� 
$str
�� J
)
��J K
]
��K L
public
�� 
IHttpActionResult
��   
UpdateRatingStatus
��! 3
(
��3 4
string
��4 :
	reviewIds
��; D
,
��D E
ReviewsStatus
��F S
status
��T Z
,
��Z [
uint
��\ `
moderatedId
��a l
)
��l m
{
�� 	
bool
�� 
updateStatus
�� 
=
�� 
false
��  %
;
��% &
try
�� 
{
�� 
updateStatus
�� 
=
�� 
_userReviewsRepo
�� /
.
��/ 0+
UpdateUserReviewRatingsStatus
��0 M
(
��M N
	reviewIds
��N W
,
��W X
status
��Y _
,
��_ `
moderatedId
��a l
,
��l m
$num
��n o
)
��o p
;
��p q
if
�� 
(
�� 
status
�� 
.
�� 
Equals
�� !
(
��! "
ReviewsStatus
��" /
.
��/ 0
Approved
��0 8
)
��8 9
)
��9 :
{
�� 
IEnumerable
�� 
<
��  %
BikeRatingApproveEntity
��  7
>
��7 8
objReviewDetails
��9 I
=
��J K
_userReviewsRepo
��L \
.
��\ ]"
GetUserReviewDetails
��] q
(
��q r
	reviewIds
��r {
)
��{ |
;
��| }
foreach
�� 
(
�� 
var
�� 
obj
��  #
in
��$ &
objReviewDetails
��' 7
)
��7 8
{
�� 
MemCachedUtil
�� %
.
��% &
Remove
��& ,
(
��, -
string
��- 3
.
��3 4
Format
��4 :
(
��: ;
$str
��; V
,
��V W
obj
��X [
.
��[ \
ModelId
��\ c
)
��c d
)
��d e
;
��e f
MemCachedUtil
�� %
.
��% &
Remove
��& ,
(
��, -
string
��- 3
.
��3 4
Format
��4 :
(
��: ;
$str
��; `
,
��` a
obj
��b e
.
��e f
ModelId
��f m
)
��m n
)
��n o
;
��o p
MemCachedUtil
�� %
.
��% &
Remove
��& ,
(
��, -
string
��- 3
.
��3 4
Format
��4 :
(
��: ;
$str
��; O
,
��O P
obj
��Q T
.
��T U
ModelId
��U \
)
��\ ]
)
��] ^
;
��^ _
MemCachedUtil
�� %
.
��% &
Remove
��& ,
(
��, -
string
��- 3
.
��3 4
Format
��4 :
(
��: ;
$str
��; S
,
��S T
obj
��U X
.
��X Y
ModelId
��Y `
)
��` a
)
��a b
;
��b c
MemCachedUtil
�� %
.
��% &
Remove
��& ,
(
��, -
string
��- 3
.
��3 4
Format
��4 :
(
��: ;
$str
��; X
,
��X Y
obj
��Z ]
.
��] ^
ModelId
��^ e
)
��e f
)
��f g
;
��g h
}
�� 
MemCachedUtil
�� !
.
��! "
Remove
��" (
(
��( )
$str
��) A
)
��A B
;
��B C
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
�� 
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
��5 6
$str
��7 w
)
��w x
;
��x y
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
return
�� 
Ok
�� 
(
�� 
updateStatus
�� "
)
��" #
;
��# $
}
�� 	
[
�� 	
HttpPost
��	 
,
�� 
Route
�� 
(
�� 
$str
�� Q
)
��Q R
]
��R S
public
�� 
IHttpActionResult
��  "
SaveUserReviewWinner
��! 5
(
��5 6
uint
��6 :
reviewId
��; C
,
��C D
uint
��E I
moderatedId
��J U
)
��U V
{
�� 	
bool
�� 
updateStatus
�� 
=
�� 
false
��  %
;
��% &
try
�� 
{
�� 
updateStatus
�� 
=
�� 
_userReviewsRepo
�� /
.
��/ 0"
SaveUserReviewWinner
��0 D
(
��D E
reviewId
��E M
,
��M N
moderatedId
��O Z
)
��Z [
;
��[ \
if
�� 
(
�� 
updateStatus
��  
)
��  !
{
�� 
MemCachedUtil
�� !
.
��! "
Remove
��" (
(
��( )
$str
��) @
)
��@ A
;
��A B
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
��5 6
$str
��7 w
)
��w x
;
��x y
return
�� !
InternalServerError
�� *
(
��* +
)
��+ ,
;
��, -
}
�� 
return
�� 
Ok
�� 
(
�� 
updateStatus
�� "
)
��" #
;
��# $
}
�� 	
}
�� 
}�� �
6D:\work\bikewaleweb\BikewaleOpr.Service\Global.asax.cs
	namespace 	
BikewaleOpr
 
. 
Service 
{ 
public 

class 
MvcApplication 
:  !
System" (
.( )
Web) ,
., -
HttpApplication- <
{ 
	protected		 
void		 
Application_Start		 (
(		( )
)		) *
{

 	
WebApiConfig 
. 
Register !
(! "
GlobalConfiguration" 5
.5 6
Configuration6 C
)C D
;D E
GlobalConfiguration 
.  
Configuration  -
.- .
EnsureInitialized. ?
(? @
)@ A
;A B
} 	
} 
} �
BD:\work\bikewaleweb\BikewaleOpr.Service\Properties\AssemblyInfo.cs
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
["" 
assembly"" 	
:""	 

AssemblyVersion"" 
("" 
$str"" $
)""$ %
]""% &
[## 
assembly## 	
:##	 

AssemblyFileVersion## 
(## 
$str## (
)##( )
]##) *�;
ND:\work\bikewaleweb\BikewaleOpr.Service\UnityBootstrapper\UnityBootstrapper.cs
	namespace** 	
BikewaleOpr**
 
.** 
Service** 
.** 
UnityConfiguration** 0
{++ 
public99 

static99 
class99 
UnityBootstrapper99 )
{:: 
public>> 
static>> 
IUnityContainer>> %

Initialize>>& 0
(>>0 1
)>>1 2
{?? 	
IUnityContainer@@ 
	container@@ %
=@@& '
new@@( +
UnityContainer@@, :
(@@: ;
)@@; <
;@@< =
	containerBB 
.BB 
RegisterTypeBB "
<BB" #+
IManufacturerCampaignRepositoryBB# B
,BBB C 
ManufacturerCampaignBBD X
>BBX Y
(BBY Z
)BBZ [
;BB[ \
	containerCC 
.CC 
RegisterTypeCC "
<CC" #
IContractCampaignCC# 4
,CC4 5
ContractCampaignCC6 F
>CCF G
(CCG H
)CCH I
;CCI J
	containerDD 
.DD 
RegisterTypeDD "
<DD" #-
!IManufacturerReleaseMaskingNumberDD# D
,DDD E,
 ManufacturerReleaseMaskingNumberDDF f
>DDf g
(DDg h
)DDh i
;DDi j
	containerEE 
.EE 
RegisterTypeEE "
<EE" #
ISellerRepositoryEE# 4
,EE4 5
SellerRepositoryEE6 F
>EEF G
(EEG H
)EEH I
;EEI J
	containerFF 
.FF 
RegisterTypeFF "
<FF" #

ISellBikesFF# -
,FF- .
	SellBikesFF/ 8
>FF8 9
(FF9 :
)FF: ;
;FF; <
	containerGG 
.GG 
RegisterTypeGG "
<GG" #&
IColorImagesBikeRepositoryGG# =
,GG= >%
ColorImagesBikeRepositoryGG? X
>GGX Y
(GGY Z
)GGZ [
;GG[ \
	containerHH 
.HH 
RegisterTypeHH "
<HH" #%
IDealerCampaignRepositoryHH# <
,HH< =$
DealerCampaignRepositoryHH> V
>HHV W
(HHW X
)HHX Y
;HHY Z
	containerII 
.II 
RegisterTypeII "
<II" # 
IBikeMakesRepositoryII# 7
,II7 8
BikeMakesRepositoryII9 L
>IIL M
(IIM N
)IIN O
;IIO P
	containerJJ 
.JJ 
RegisterTypeJJ "
<JJ" #!
IBikeModelsRepositoryJJ# 8
,JJ8 9 
BikeModelsRepositoryJJ: N
>JJN O
(JJO P
)JJP Q
;JJQ R
	containerKK 
.KK 
RegisterTypeKK "
<KK" #
IImageKK# )
,KK) *
ImageBLKK+ 2
>KK2 3
(KK3 4
)KK4 5
;KK5 6
	containerLL 
.LL 
RegisterTypeLL "
<LL" #
IImageRepositoryLL# 3
,LL3 4
ImageRepositoryLL5 D
>LLD E
(LLE F
)LLF G
;LLG H
	containerMM 
.MM 
RegisterTypeMM "
<MM" #
	ISecurityMM# ,
,MM, -

SecurityBLMM. 8
>MM8 9
(MM9 :
)MM: ;
;MM; <
	containerNN 
.NN 
RegisterTypeNN "
<NN" #
IBannerRepositoryNN# 4
,NN4 5
BannerRepositoryNN6 F
>NNF G
(NNG H
)NNH I
;NNI J
	containerOO 
.OO 
RegisterTypeOO "
<OO" #"
IUserReviewsRepositoryOO# 9
,OO9 :!
UserReviewsRepositoryOO; P
>OOP Q
(OOQ R
)OOR S
;OOS T
	containerPP 
.PP 
RegisterTypePP "
<PP" #*
ISponsoredComparisonRepositoryPP# A
,PPA B)
SponsoredComparisonRepositoryPPC `
>PP` a
(PPa b
)PPb c
;PPc d
	containerQQ 
.QQ 
RegisterTypeQQ "
<QQ" #
ICacheManagerQQ# 0
,QQ0 1
MemcacheManagerQQ2 A
>QQA B
(QQB C
)QQC D
;QQD E
	containerRR 
.RR 
RegisterTypeRR "
<RR" #/
#ISponsoredComparisonCacheRepositoryRR# F
,RRF G.
"SponsoredComparisonCacheRepositoryRRH j
>RRj k
(RRk l
)RRl m
;RRm n
	containerSS 
.SS 
RegisterTypeSS "
<SS" # 
ISponsoredComparisonSS# 7
,SS7 8
SponsoredComparisonSS9 L
>SSL M
(SSM N
)SSN O
;SSO P
	containerUU 
.UU 
RegisterTypeUU "
<UU" #
BikewaleUU# +
.UU+ , 
ManufacturerCampaignUU, @
.UU@ A
	InterfaceUUA J
.UUJ K+
IManufacturerCampaignRepositoryUUK j
,UUj k
BikewaleUUl t
.UUt u!
ManufacturerCampaign	UUu �
.
UU� �
DAL
UU� �
.
UU� �,
ManufacturerCampaignRepository
UU� �
>
UU� �
(
UU� �
)
UU� �
;
UU� �
	containerVV 
.VV 
RegisterTypeVV "
<VV" #"
IDealerPriceRepositoryVV# 9
,VV9 :!
DealerPriceRepositoryVV; P
>VVP Q
(VVQ R
)VVR S
;VVS T
	containerWW 
.WW 
RegisterTypeWW "
<WW" #
IDealerPriceQuoteWW# 4
,WW4 5&
DealerPriceQuoteRepositoryWW6 P
>WWP Q
(WWQ R
)WWR S
;WWS T
	containerXX 
.XX 
RegisterTypeXX "
<XX" #
IDealerPriceXX# /
,XX/ 0
DealerPriceXX1 <
>XX< =
(XX= >
)XX> ?
;XX? @
	containerYY 
.YY 
RegisterTypeYY "
<YY" #
IDealersYY# +
,YY+ ,
DealersRepositoryYY- >
>YY> ?
(YY? @
)YY@ A
;YYA B
	containerZZ 
.ZZ 
RegisterTypeZZ "
<ZZ" # 
IVersionAvailabilityZZ# 7
,ZZ7 8
VersionAvailabilityZZ9 L
>ZZL M
(ZZM N
)ZZN O
;ZZO P
	container[[ 
.[[ 
RegisterType[[ "
<[[" #%
IShowroomPricesRepository[[# <
,[[< =
BikeShowroomPrices[[> P
>[[P Q
([[Q R
)[[R S
;[[S T
	container\\ 
.\\ 
RegisterType\\ "
<\\" #

IBikeMakes\\# -
,\\- .
	BikeMakes\\/ 8
>\\8 9
(\\9 :
)\\: ;
;\\; <
	container]] 
.]] 
RegisterType]] "
<]]" #
IServiceCenter]]# 1
,]]1 2
ServiceCenter]]3 @
>]]@ A
(]]A B
)]]B C
;]]C D
	container^^ 
.^^ 
RegisterType^^ "
<^^" #$
IServiceCenterRepository^^# ;
,^^; <#
ServiceCenterRepository^^= T
>^^T U
(^^U V
)^^V W
;^^W X
	containeraa 
.aa 
RegisterTypeaa "
<aa" # 
IPageMetasRepositoryaa# 7
,aa7 8
PageMetasRepositoryaa9 L
>aaL M
(aaM N
)aaN O
;aaO P
	containerbb 
.bb 
RegisterTypebb "
<bb" #

IPageMetasbb# -
,bb- .
	PageMetasbb/ 8
>bb8 9
(bb9 :
)bb: ;
;bb; <
returndd 
	containerdd 
;dd 
}ee 	
}ff 
}gg �
JD:\work\bikewaleweb\BikewaleOpr.Service\UnityBootstrapper\UnityResolver.cs
	namespace 	
BikewaleOpr
 
. 
Service 
. 
UnityConfiguration 0
{ 
public 

class 
UnityResolver 
:  
IDependencyResolver! 4
{ 
	protected 
IUnityContainer !
	container" +
;+ ,
public 
UnityResolver 
( 
IUnityContainer ,
	container- 6
)6 7
{ 	
if 
( 
	container 
== 
null !
)! "
{ 
throw 
new !
ArgumentNullException /
(/ 0
$str0 ;
); <
;< =
} 
this 
. 
	container 
= 
	container &
;& '
} 	
public!! 
object!! 

GetService!!  
(!!  !
Type!!! %
serviceType!!& 1
)!!1 2
{"" 	
try## 
{$$ 
if%% 
(%% 
(%% 
serviceType%%  
.%%  !
IsInterface%%! ,
||%%- /
serviceType%%0 ;
.%%; <

IsAbstract%%< F
)%%F G
&&%%H J
!%%K L
	container%%L U
.%%U V
IsRegistered%%V b
(%%b c
serviceType%%c n
)%%n o
)%%o p
return&& 
null&& 
;&&  
else'' 
return(( 
	container(( $
.(($ %
Resolve((% ,
(((, -
serviceType((- 8
)((8 9
;((9 :
}** 
catch++ 
(++ %
ResolutionFailedException++ ,
)++, -
{,, 
return-- 
null-- 
;-- 
}.. 
}// 	
public66 
IEnumerable66 
<66 
object66 !
>66! "
GetServices66# .
(66. /
Type66/ 3
serviceType664 ?
)66? @
{77 	
try88 
{99 
return:: 
	container::  
.::  !

ResolveAll::! +
(::+ ,
serviceType::, 7
)::7 8
;::8 9
};; 
catch<< 
(<< %
ResolutionFailedException<< ,
)<<, -
{== 
return>> 
new>> 
List>> 
<>>  
object>>  &
>>>& '
(>>' (
)>>( )
;>>) *
}?? 
}@@ 	
publicFF 
IDependencyScopeFF 

BeginScopeFF  *
(FF* +
)FF+ ,
{GG 	
varHH 
childHH 
=HH 
	containerHH !
.HH! " 
CreateChildContainerHH" 6
(HH6 7
)HH7 8
;HH8 9
returnII 
newII 
UnityResolverII $
(II$ %
childII% *
)II* +
;II+ ,
}JJ 	
publicLL 
voidLL 
DisposeLL 
(LL 
)LL 
{MM 	
	containerNN 
.NN 
DisposeNN 
(NN 
)NN 
;NN  
}OO 	
}PP 
}QQ �
AD:\work\bikewaleweb\BikewaleOpr.Service\App_Start\WebApiConfig.cs
	namespace 	
BikewaleOpr
 
. 
Service 
{ 
public 

static 
class 
WebApiConfig $
{ 
public		 
static		 
void		 
Register		 #
(		# $
HttpConfiguration		$ 5
config		6 <
)		< =
{

 	
config 
. 
DependencyResolver %
=& '
new( +
UnityResolver, 9
(9 :
UnityBootstrapper: K
.K L

InitializeL V
(V W
)W X
)X Y
;Y Z
var 
cors 
= 
new 
EnableCorsAttribute .
(. /
origins/ 6
:6 7
$str8 ;
,; <
headers= D
:D E
$strF I
,I J
methodsK R
:R S
$strT W
)W X
;X Y
config 
. 

EnableCors 
( 
cors "
)" #
;# $
config 
. "
MapHttpAttributeRoutes )
() *
)* +
;+ ,
config 
. 
Routes 
. 
MapHttpRoute &
(& '
name 
: 
$str %
,% &
routeTemplate 
: 
$str ?
,? @
defaults 
: 
new 
{ 
id  "
=# $
RouteParameter% 3
.3 4
Optional4 <
,< =
action> D
=E F
RouteParameterG U
.U V
OptionalV ^
}_ `
) 
; 
} 	
} 
} 