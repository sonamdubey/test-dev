IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetIndividualListingsDetails_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetIndividualListingsDetails_v16]
GO

	-----------------------------------------------------------------------------------------------------------------------
-- Created By : Sadhana Upadhyay on 29 May 2015
-- Summary : To get Individual listing details
-- EXEC [GetIndividualListingsDetails] 4573
-- Modified by Navead on 11/06/2015 - Changed the inner joins to left join and moved the seller type filter to join condition
-- Modified by Akansha on 22/06/2015 -- added order by IsMain clause for CarPhotos
-- Modified by Akansha on 22/06/2015 -- added IsPremium Column
-- Modified by Akansha on 01/07/2015 -- Added joins to 2 more tables called CarModels, CarMakes and cities in case of sold out cars
-- Modified by Navead on 19/08/2015 -- Version:15.8.1.1 - Commented the NewCarPrice select query
-- Modified by Sahil on 16-10-2015 -- Version:15.10.3 - Getting Carversions data to find discontinued versions.
-- Modified by prachi phalak on 13-01-2016--Added  BodyStyleId and SubSegmentId for recommended cars.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[GetIndividualListingsDetails_v16.1.2] 
	@InquiryId AS INT
AS
BEGIN
	DECLARE @VersionId AS INT
		,@CityId AS INT

	SELECT LL.ProfileId
		,LL.InquiryId
		,CIT.Name as CityName
		,CIT.Id as CityId
		,LL.AreaName
		,ISNULL(LL.AreaId, 0) AreaId
		,CM.Id as MakeId
		,CM.Name as MakeName
		,CMO.Id as ModelId
		,CMO.Name as ModelName
		,CV.New as IsVersionNew
		,CV.Futuristic as IsVersionFuturistic
		,CV.IsDeleted as IsVersionDeleted
		,CV.Discontinuation_date as VersionDiscontinuationDate
		,LL.VersionId
		,LL.VersionName
		,LL.RootId
		,LL.RootName
		,LL.Price
		,LL.Color
		,LL.Kilometers
		,CD.AdditionalFuel
		,CD.Tax
		,CD.Insurance
		,CD.InsuranceExpiry
		,CSI.ReasonForSelling
		,CSI.CarRegNo
		,LL.Comments
		,LL.MakeYear
		,LL.SellerType
		,LL.Seller
		,CFT.FuelType
		,CFT.FuelTypeId
		,CT.Id AS CarTransmissionId
		,CT.Descr AS CarTransmission
		,LL.Owners
		,LL.SellerContact
		,CSI.CustomerName
		,CD.Warranties
		,CD.Modifications
		,CD.ACCondition
		,CD.BatteryCondition
		,CD.BrakesCondition
		,CD.ElectricalsCondition
		,CD.EngineCondition
		,CD.ExteriorCondition
		,CD.InteriorCondition
		,CD.SeatsCondition
		,CD.SuspensionsCondition
		,CD.TyresCondition
		,CD.OverallCondition
		,CD.Features_Comfort
		,CD.Features_Others
		,CD.Features_SafetySecurity
		,CD.RegistrationPlace
		,CD.InteriorColor
		,cd.CityMileage
		,LL.PhotoCount
		,LL.VideoCount
		,LL.IsPremium
		,CASE WHEN CD.YoutubeVideo IS NOT NULL AND CD.IsYouTubeVideoApproved=1 AND LL.IsPremium = 1 THEN  CD.YoutubeVideo END YoutubeVideo
		,LL.LastUpdated
		,CD.Features_SafetySecurity
		,CD.Features_Comfort
		,CD.Features_Others
		,CMO.MaskingName
		,CV.BodyStyleId
		,CV.SubSegmentId
	FROM CustomerSellInquiries CSI WITH(NOLOCK)
	INNER JOIN CustomerSellInquiryDetails CD WITH(NOLOCK) ON CSI.Id = CD.InquiryId and CSI.ID = @InquiryId 
	INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = CSI.CarVersionId
	INNER JOIN CarModels CMO WITH (NOLOCK) ON CV.CarModelId = CMO.ID
	INNER JOIN Cities CIT WITH (NOLOCK) ON CIT.ID = CSI.CityId 
	INNER JOIN CarMakes CM WITH (NOLOCK) ON CM.ID = CMO.CarMakeId
	INNER JOIN CarFuelType CFT WITH(NOLOCK) ON CFT.FuelTypeId = CV.CarFuelType
	INNER JOIN CarTransmission CT WITH(NOLOCK) ON CT.Id = CV.CarTransmission
	LEFT JOIN livelistings LL WITH(NOLOCK)  ON CSI.ID = LL.Inquiryid AND LL.SellerType = 2
	WHERE CSI.ID = @InquiryId 
	

	SELECT @VersionId = VersionId
		,@CityId = CityId
	FROM LiveListings WITH(NOLOCK)
	WHERE Inquiryid=@InquiryId
	AND SellerType=2

	EXEC CD.GetCarFeaturesByVersionID @VersionId

	EXEC CD.GetCarSpecsByVersionID @VersionId

	---Commented by  Navead on 19/08/2015
	--SELECT Price AS NewCarPrice
	--FROM NewCarShowroomPrices WITH(NOLOCK)
	--WHERE CarVersionId = @VersionId
	--	AND IsActive = 1
	--	AND CityId = @CityId

	SELECT InquiryId
		,HostURL + DirectoryPath + ImageUrlFull as ImageUrlFull
		,IsMain
		,HostURL
		,OriginalImgPath
	FROM CarPhotos WITH (NOLOCK)
	WHERE IsActive = 1
		AND IsApproved = 1
		AND IsDealer = 0
		AND InquiryId = @InquiryId
	ORDER BY IsMain desc
END
