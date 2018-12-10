IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetBookingTransaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetBookingTransaction]
GO

	-- Author:		Vaibhav K
-- Create date: 30 Mar 2015
-- Description:	Get booking transaction details
-- Modifier:	Vaibhav K 7 Apr 2015 Added more tables & fields
--Modified By: Rakesh Yadav on 05 Aug 2015 to get OriginalImgPath
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_GetBookingTransaction]
	-- Add the parameters for the stored procedure here
	@BookingTransactionId		NUMERIC(18, 0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	
	--Vaibhav K 7 Apr 2015
	--Modified by Rakesh Yadav on 8 April addded join with CarFuelTypes and CarTransmission
	SELECT TOP 1 MBT.Id BookingTransactionId
	, CMK.Id MakeId, CMK.Name MakeName, MBT.ModelId, CMO.Name ModelName, MBT.VersionId, CV.Name VersionName 
	, CV.CarFuelType FuelTypeId, CF.Descr FuelTypeName, CV.CarTransmission TransmissionTypeId, CT.Descr TransmissionTypeName
	, CMO.HostURL, CMO.SmallPic, CMO.LargePic,CMO.OriginalImgPath,CMO.MinPrice AS ExShowroomDelhi
	, DO.OfferDetails, dbo.Microsite_Fun_GetOnRoadPrice(MBT.VersionId, MBT.CustomerCityId) OnRoadPrice, MBT.Color,MBT.CustomerName,MBT.CustomerMobile
	, MBT.CustomerEmail,MBT.CustomerStateId,S.Name AS CustomerStateName,MBT.CustomerCityId ,C.Name AS CustomerCityName,MBT.VersionPrice
	, MBT.PaymentType,MBT.PaymentMode,MBT.PGTransactionId,MBT.PaymentDate,MBT.DealerId
	, D.ID OutletId, D.Organization OutletName, D.Address1+', '+D.Address2 +', '+A.Name+',' +DC.Name+', PIN - '+D.Pincode OutletAddress, D.WebsiteContactMobile OutletContactInfo
	, MBT.AutoBizInqId,MBT.BookingAmount,MBT.PickUpDate,MBT.AutoBizResponse,MBT.CustomerAddress
	, MDT.DeliveryTime, MDT.BookingAmount DealerBookingAmt
	FROM Microsite_BookingTransaction MBT WITH (NOLOCK)
	JOIN Carmodels CMO WITH (NOLOCK) ON MBT.ModelId = CMO.Id
	JOIN CarMakes CMK WITH (NOLOCK) ON CMO.CarMakeId = CMK.ID
	LEFT JOIN CarVersions CV WITH (NOLOCK) ON MBT.VersionId = CV.ID
	LEFT JOIN CarFuelTypes CF WITH (NOLOCK) ON CV.CarFuelType = CF.CarFuelTypeId
	LEFT JOIN CarTransmission CT WITH (NOLOCK) ON CV.CarTransmission = CT.Id
	LEFT JOIN Microsite_OfferModels DOM WITH (NOLOCK) ON MBT.DealerId = DOM.DealerId AND MBT.ModelId = DOM.ModelId
	LEFT JOIN Microsite_DealerOffers DO WITH (NOLOCK) ON DOM.OfferId = DO.Id AND DO.IsDeleted = 1
	LEFT JOIN States S WITH (NOLOCK) ON MBT.CustomerStateId=S.ID
	LEFT JOIN Cities C WITH (NOLOCK) ON C.StateId=S.ID AND C.ID=MBT.CustomerCityId
	LEFT JOIN Dealers D WITH (NOLOCK) ON MBT.OutletId = D.ID	
	LEFT JOIN Areas A WITH(NOLOCK) ON  A.ID=D.AreaId
	LEFT JOIN Cities DC WITH(NOLOCK) ON DC.ID=D.CityId
	LEFT JOIN Microsite_DeliveryTime MDT ON MDT.VersionId = MBT.VersionId AND MBT.DealerId = MDT.DealerId
	WHERE MBT.Id = @BookingTransactionId
	AND ((MBT.VersionId IS NOT NULL AND MBT.VersionId = CV.ID) OR MBT.VersionId IS NULL)


	--Vaibhav K 09 Apr 2015 Get offer selcted foe the current transaction	
	SELECT MDO.Id OfferId, MDO.OfferTitle, MDO.OfferDetails, MDO.OfferContent
	, MDO.OfferStartDate, MDO.OfferEndDate, MDO.OfferTermsCondition 
	FROM Microsite_BookingTransactionOffers BTO WITH (NOLOCK)
	JOIN Microsite_BookingTransaction MBT WITH (NOLOCK) ON BTO.Microsite_BookingTransactionId = MBT.Id
	JOIN Microsite_DealerOffers MDO WITH (NOLOCK) ON BTO.Microsite_DealerOffersId = MDO.Id
	WHERE MBT.Id = @BookingTransactionId AND MDO.IsActive = 1

	/*
	SELECT TOP 1 NCSP.Price, MBT.VersionId, VW.* ,
	MBT.Id BookingTransactionId, VW.MakeId, VW.Make MakeName, VW.ModelId, VW.Model ModelName, VW.VersionId, VW.Version VersionName, 
	CMO.HostURL, CMO.SmallPic, CMO.LargePic,
	NCSP.Price
	FROM Microsite_BookingTransaction MBT
	JOIN vwMMV VW ON MBT.ModelId = VW.ModelId
	JOIN Carmodels CMO ON VW.ModelId = CMO.Id
	JOIN NewCarShowroomPrices NCSP ON VW.ModelId = NCSP.CarModelId
	WHERE MBT.Id = @BookingTransactionId
	--AND ((MBT.VersionId IS NOT NULL AND MBT.VersionId = VW.VersionId) OR MBT.VersionId IS NULL)
	*/
END
