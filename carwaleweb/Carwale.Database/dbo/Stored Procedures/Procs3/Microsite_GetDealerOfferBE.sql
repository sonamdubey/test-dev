IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDealerOfferBE]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDealerOfferBE]
GO

	
CREATE PROCEDURE [dbo].[Microsite_GetDealerOfferBE]
@DealerId INT
,@ModelId INT=NULL
,@CityId INT=NULL
,@VersionId INT= NULL
,@IsFeatured BIT=NULL
,@IsDealerData bit=0
AS
--Author: Rakesh Yadav	
--Date Created: 30 march 2015
--Desc: Fetch offer for dealer
--modified by Rakesh Yadav on 11 May 2015 added condition for featured deals
--modified by rakesh yadav on 22 jun 2015 modified entire SP to get offers when dealer model and versions are passed 
--(in this case @IsDealerData should be set to 1 else modenid and versionid will considered of CarModels and CarVersions id)
--Modified By : Kritika Choudhary on 23rd July 2015, added SlugImage , TillStockLast in select query and changed the Join conditions
--Modified By : Kritika Choudhary on 28th July 2015, added JOIN with CW_NewCarShowroomPrices and PQ_CategoryItems and order by price
--Modified By : Kritika Choudhary on 30th July 2015, added CityName and CityID
--Modified By : Rakesh Yadav On 05 Aug 2015, Added originalImgPath
--Modified By : Kritika Choudhary on 6th August 2015, removed slug img from select query
--Modified By : Kritika Choudhary on 20th August 2015, added StateId
--Modified By : Kritika Choudhary on 7th Sept 2015, added second Select query for make, model and version name
--Modified By : Kritika Choudhary on 16th Sept 2015, added SlugOriginalImgPath, SlugIsActive
BEGIN
	create table #temp
	(
	Id numeric(18,0) IDENTITY(1,1) ,ModelId Numeric(18,0),OfferId int,MakeName varchar(30),ModelName varchar(30),OfferContent varchar(max),OfferDetails varchar(max),
	OfferStartDate date,OfferEndDate date,OfferTermsContdition varchar(max),HostUrl varchar(100),LargePic varchar(200),OriginalImgPath varchar(300),MinPrice int,
	OfferTitle varchar(500),VersionId numeric(18,0),IsFeatured bit,VersionName varchar(50),Price int,CityName varchar(20), CityId Numeric(18,0), StateId Numeric(18,0)
	)

	INSERT INTO #temp(ModelId,OfferId,MakeName,ModelName,OfferContent,OfferDetails,OfferStartDate,OfferEndDate,OfferTermsContdition,HostUrl,LargePic,OriginalImgPath,
	MinPrice,OfferTitle,VersionId,IsFeatured,VersionName, Price, CityName, CityId, StateId)
	(SELECT distinct VM.ModelId, MDO.Id,VM.Make,VM.Model, MDO.OfferContent,MDO.OfferDetails,MDO.OfferStartDate,MDO.OfferEndDate,MDO.OfferTermsCondition,VM.HostUrl,VM.LargePic
	,VM.OriginalImgPath,VM.ModelMinPrice,
	MDO.OfferTitle , VM.VersionId,MDO.IsFeatured,Vm.Version AS VersionName, NCP.PQ_CategoryItemValue AS Price, C.Name, C.Id, C.StateId
	FROM
	Microsite_DealerOffers MDO WITH(NOLOCK) 
	LEFT JOIN Microsite_OfferModels MOM WITH(NOLOCK) ON MDO.ModelId=MOM.ModelId AND MDO.Id=MOM.OfferId
	LEFT JOIN Microsite_OfferCities MOC WITH(NOLOCK) ON MDO.Id=MOC.OfferId
	LEFT JOIN Microsite_OfferVersions MOV WITH(NOLOCK) ON MDO.Id=MOV.OfferId
	LEFT JOIN vwMicrositeMMV VM WITH(NOLOCK) ON VM.VersionId=MOV.VersionId
	LEFT JOIN Cities C WITH(NOLOCK) ON C.ID=MOC.CityId
	LEFT JOIN CW_NewCarShowroomPrices NCP WITH(NOLOCK) ON NCP.CarVersionId = MOV.VersionId AND NCP.CityId = MOC.CityId 
    LEFT JOIN PQ_CategoryItems CI WITH(NOLOCK) ON NCP.PQ_CategoryItem = CI.Id
	WHERE MDO.IsActive=1 AND MDO.IsDeleted=0 
	AND VM.ModelId IS NOT NULL
	AND ((MDO.OfferStartDate<=GETDATE() AND MDO.OfferEndDate>= GETDATE()) OR MDO.TillStockLast=1)
	AND MDO.DealerId =@DealerId 
	AND (@CityId IS NULL OR C.ID=@CityId)
	AND (@IsFeatured IS NULL OR MDO.IsFeatured=@IsFeatured)
	AND CI.CategoryId=3)
	

	IF @IsDealerData=0	
	BEGIN
		SELECT T.ModelId,T.OfferId,T.MakeName,T.ModelName,T.OfferContent,T.OfferDetails,T.OfferStartDate,T.OfferEndDate,
		T.OfferTermsContdition,T.HostUrl,T.LargePic,T.OriginalImgPath,
		T.MinPrice,T.OfferTitle,T.VersionId,T.IsFeatured,T.VersionName,
		DO.TillStockLast, T.Price, T.CityId, T.CityName,T.StateId, MDI.HostUrl AS SlugHostUrl, MDI.OriginalImgPath AS SlugOriginalImgPath,
		MDI.IsActive AS SlugIsActive 
		FROM #temp T WITH(NOLOCK)
		LEFT JOIN Microsite_DealerOffers DO WITH(NOLOCK) ON DO.Id = T.OfferId 
		LEFT JOIN Microsite_OfferImages MDI WITH(NOLOCK) ON MDI.OfferId = DO.Id AND MDI.IsActive=1
		WHERE (@ModelId IS NULL OR T.ModelId=@ModelId) 
		AND (@VersionId IS NULL OR T.VersionId=@VersionId)
		
		ORDER BY T.Price
	
	END
	ELSE
	BEGIN
		SELECT 
		TDM.Id As ModelId, T.OfferId,T.MakeName,TDM.DWModelName AS ModelName,T.OfferContent,T.OfferDetails,T.OfferStartDate,
		T.OfferEndDate,T.HostUrl,T.LargePic,T.OriginalImgPath,
		T.MinPrice,T.OfferTitle,TDV.ID AS VersionId,T.IsFeatured,
		TDV.DWVersionName AS VersionName,DO.HostUrl AS DealerHostUrl, 
		DO.TillStockLast,T.OfferTermsContdition, T.Price, T.CityId, T.CityName, T.StateId, MDI.HostUrl AS SlugHostUrl, MDI.OriginalImgPath AS SlugOriginalImgPath,
		MDI.IsActive AS SlugIsActive 
		FROM 
		#temp T WITH(NOLOCK)
		LEFT JOIN Microsite_DealerOffers DO WITH(NOLOCK) ON DO.Id = T.OfferId 
		LEFT JOIN Microsite_OfferImages MDI WITH(NOLOCK) ON MDI.OfferId = DO.Id AND MDI.IsActive=1
		JOIN TC_DealerModels TDM WITH(NOLOCK) ON T.ModelId=TDM.CWModelId
		JOIN TC_DealerVersions TDV WITH(NOLOCK) ON T.VersionId=TDV.CWVersionId AND TDM.ID=TDV.DWModelId  --AND TDM.DealerId=TDV.DealerId(Removed by Kritika Choudhary)
		WHERE 
		TDM.IsDeleted=0 AND TDV.IsDeleted=0
		AND (@ModelId IS NULL OR TDM.Id=@ModelId) 
		AND (@VersionId IS NULL OR TDV.Id=@VersionId)
		AND TDM.DealerId=@DealerId 
	    ORDER BY T.Price
	
	END

	drop table #temp
END
