IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerListingDetails_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerListingDetails_v15]
GO

	--------------------------------------------------------------------------------------------------------------
-- Created By : Prachi Phalak on 05/08/2015
-- Summary : To get dealer listing details
-- EXEC [GetDealerListingDetails] 9669

-- Modified by: Navead Kazi on 19/08/2015 Version:15.8.1.1 - Commented the NewCarPrice select query
--------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[GetDealerListingDetails_v15.8.1.1] 
@InquiryId AS INT

AS
BEGIN
	DECLARE @VersionId AS INT
		,@CityId AS INT
		,@DealerId AS INT
		,@StockId AS INT 
		

	SELECT LL.ProfileId
		,LL.Inquiryid
		,CIT.Name as CityName
		,CIT.Id as CityId
		,LL.AreaName
		,LL.AreaId
		,CM.Id as MakeId
		,CM.Name as MakeName
		,CMO.Id as ModelId
		,CMO.Name as ModelName
		,LL.VersionId
		,LL.VersionName
		,LL.RootId
		,LL.RootName
		,LL.Price
		,LL.Color
		,LL.Kilometers
		,LL.OfferStartDate
		,LL.OfferEndDate
		,LL.IsPremium
		,CD.AdditionalFuel
		,CD.Insurance
		,CD.InsuranceExpiry
		,LL.Comments
		,SI.CarRegNo
		,CD.CityMileage
		,LL.MakeYear
		,LL.SellerType
		,LL.Seller
		,ISNULL(LL.CertificationId,0) CertificationId
		,LL.CertifiedLogoUrl
		,CFT.FuelType
		,CFT.FuelTypeId
		,CT.Id AS CarTransmissionId
		,CT.Descr AS CarTransmission
		,LL.Owners
		,D.ActiveMaskingNumber
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
		,CD.InteriorColor
		,LL.EMI
		,LL.PhotoCount
		,LL.VideoCount
		,CASE WHEN CD.YoutubeVideo IS NOT NULL AND CD.IsYouTubeVideoApproved=1 AND LL.IsPremium = 1 THEN  CD.YoutubeVideo END YoutubeVideo
		,D.ActiveMaskingNumber SellerContact
		,LL.LastUpdated
		,CD.RegistrationPlace
		,D.Organization
		,CD.OneTimeTax AS Tax
		,D.Address1 + ' ' + D.Address2 AS Address
		,D.Lattitude
		,D.Longitude
		,D.FirstName + ' ' + D.LastName AS DealerName
		,D.ID AS DealerId
		,SI.TC_StockId
		,ISNULL(CC.HostURL, '') + ISNULL(CC.DirectoryPath, '') + ISNULL(CC.LogoURL, '') AS DealerLogoUrl --Added by Purohith Guguloth to get dealer logo url on 16th june, 2015
		--ISNULL(D.ProfilePhotoHostUrl,'') + ISNULL(D.ProfilePhotoUrl,'') AS DealerProfileImageUrl -- Added by Navead to get dealer profile image
		,ISNULL(D.ProfilePhotoHostUrl,'') AS DealerProfileHostUrl --Modified by Navead on 11/08/2015
		,ISNULL(D.OriginalImgPath,'') AS DealerProfileImagePath --Modified by Navead on 11/08/2015
		,CMO.MaskingName
	FROM SellInquiries SI WITH (NOLOCK)
	INNER JOIN SellInquiriesDetails CD WITH (NOLOCK) ON SI.ID = CD.SellInquiryId AND SI.ID = @InquiryId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = SI.CarVersionId
	INNER JOIN CarModels CMO WITH (NOLOCK) ON CV.CarModelId = CMO.ID
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = SI.DealerId
	INNER JOIN Cities CIT WITH (NOLOCK) ON CIT.ID = D.CityId 
	INNER JOIN CarMakes CM WITH (NOLOCK) ON CM.ID = CMO.CarMakeId
	INNER JOIN CarFuelType CFT WITH (NOLOCK) ON CFT.FuelTypeId = CV.CarFuelType
	INNER JOIN CarTransmission CT WITH (NOLOCK) ON CT.Id = CV.CarTransmission
	LEFT JOIN livelistings LL WITH (NOLOCK) ON LL.Inquiryid = SI.ID AND LL.SellerType = 1
	LEFT JOIN Classified_CertifiedOrg CC WITH(NOLOCK) ON CC.Id = d.CertificationId 
	WHERE SI.ID = @InquiryId
		

	SELECT @VersionId = SI.CarVersionId
		,@CityId = D.CityId
		,@DealerId = D.ID
		,@StockId = SI.TC_StockId
	FROM SellInquiries SI WITH (NOLOCK)
	INNER JOIN Dealers D  WITH (NOLOCK) ON D.ID = SI.DealerId
	WHERE SI.ID = @InquiryId
		
	EXEC CD.GetCarFeaturesByVersionID @VersionId

	EXEC CD.GetCarSpecsByVersionID @VersionId

	--Commented by  Navead Kazi on 19/08/2015
	--SELECT Price AS NewCarPrice
	--FROM NewCarShowroomPrices WITH (NOLOCK)
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
		AND IsDealer = 1
		AND InquiryId = @InquiryId
	ORDER BY IsMain desc

	SELECT IsCarInWarranty
		,WarrantyValidTill
		,WarrantyProvidedBy
		,ThirdPartyWarrantyProviderName
		,WarrantyDetails
		,HasExtendedWarranty
		,ExtendedWarrantyValidFor
		,ExtendedWarrantyProviderName
		,ExtendedWarrantyDetails
		,HasAnyServiceRecords
		,ServiceRecordsAvailableFor
		,HasRSAAvailable
		,RSAValidTill
		,RSAProviderName
		,RSADetails
		,HasFreeRSA
		,FreeRSAValidFor
		,FreeRSAProvidedBy
		,FreeRSADetails
	FROM TC_CarAdditionalInformation WITH (NOLOCK)
	WHERE StockId = @StockId

	SELECT TCST.EMI
		,TCST.LoanAmount
		,TCST.InterestRate
		,TCST.Tenure
		,TCST.OtherCharges
		,TCST.LoanToValue
		,TCST.ShowEMIOnCarwale
	FROM TC_Stock TCST WITH (NOLOCK)
	WHERE TCST.Id = @StockId
	
	EXEC [dbo].[Classified_SellersOffering_15.4.1] @InquiryId

	EXEC [dbo].[GetDescriptionofUsedCarwithOffers] @InquiryId
	
	--EXEC GetFeaturesBySegment @VersionId commented by Purohith Guguloth on 15th june, 2015
END