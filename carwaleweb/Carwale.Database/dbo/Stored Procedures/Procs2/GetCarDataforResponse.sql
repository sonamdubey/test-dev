IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarDataforResponse]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarDataforResponse]
GO

	
-- =============================================
-- Author:		Afrose
-- Create date: 19th Sept 2016
-- Description:	Get additional fields from profile id and seller type
-- exec GetCarDataforResponse 9673, 0
-- Modified on 30-09-2016, added join with CustomerSellInquiries,CustomerSellInquiryDetails,SellInquiries,SellInquiriesDetails to get additional fields
-- =============================================
CREATE PROCEDURE [dbo].[GetCarDataforResponse]
@ID INT,
@SellerType INT
AS
BEGIN

SET NOCOUNT ON;
IF(@SellerType=0)
BEGIN
		SELECT
		LL.CityName City,
		LL.MakeName Make,
		LL.ModelName Model,
		LL.VersionName [Version],
		DATEPART(YEAR,LL.MakeYear) [MakeYear],
		LL.Color,
		CFT.FuelType [Fuel],
		LL.Price [Price],
		SD.CityMileage Mileage,	
		SI.CarRegNo RegNo,
		LL.OwnerTypeId Owners,
		LL.ProfileId
		FROM LiveListings LL WITH(NOLOCK)	
		INNER JOIN CarVersions CV WITH(NOLOCK) ON LL.VersionId = CV.ID
		INNER JOIN CarFuelType CFT WITH(NOLOCK) ON CFT.FuelTypeId = CV.CarFuelType
		INNER JOIN CustomerSellInquiries SI WITH(NOLOCK) ON LL.Inquiryid = SI.ID
		INNER JOIN CustomerSellInquiryDetails SD WITH(NOLOCK) ON SI.ID = SD.InquiryId
		WHERE LL.Inquiryid=@ID and SellerType=2 -- for individuals
END
ELSE
BEGIN
		SELECT 
		LL.CityName City,
		LL.MakeName Make,
		LL.ModelName Model,
		LL.VersionName [Version],
		DATEPART(YEAR,LL.MakeYear) [MakeYear],
		LL.Color,
		CFT.FuelType [Fuel],
		LL.Price [Price],
		SD.CityMileage Mileage,	
		SI.CarRegNo RegNo,
		LL.OwnerTypeId Owners,
		LL.ProfileId,
		CASE WHEN SI.SourceId=1 THEN SI.TC_StockId END AS CTInventoryId		
		FROM LiveListings LL WITH(NOLOCK)		
		INNER JOIN CarVersions CV WITH(NOLOCK) ON LL.VersionId = CV.ID
		INNER JOIN CarFuelType CFT WITH(NOLOCK) ON CFT.FuelTypeId = CV.CarFuelType
		INNER JOIN SellInquiries SI WITH(NOLOCK) ON LL.Inquiryid = SI.ID
		INNER JOIN SellInquiriesDetails SD WITH(NOLOCK) ON SI.ID = SD.SellInquiryId
		WHERE LL.Inquiryid=@ID and SellerType=1 --for dealers
END

END
 


