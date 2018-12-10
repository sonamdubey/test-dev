IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[NCAlert].[PriceQuoteMailerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [NCAlert].[PriceQuoteMailerData]
GO

	
--Created By: Shikhar on 28-05-2014
--Description: Send the email alert for the customer who have taken Price Quotes.
CREATE PROCEDURE [NCAlert].[PriceQuoteMailerData]
AS
BEGIN
	SELECT DISTINCT 
		CEM.CustomerID, 
		CEM.NewCarPurchaseInquiriesId,
		CEM.CustomerName, 
		CEM.CustomerEmail, 
		CEM.CustomerKey,
		CEM.CityId,
		CEM.CityName, 
		CACD.MakeId, 
		CACD.MakeName, 
		CACD.ModelId, 
		CACD.ModelName,
		CACD.ModelMaskingName,
		CACD.VersionId,
		CACD.VersionName,
		CACD.ImageUrl,
		CEM.ShowroomPrice,
		CEM.RTO,
		CEM.Insurance,
		ISNULL(CEM.DepotCharges, 0) AS DepotCharges,	
		(CEM.ShowroomPrice + CEM.RTO + CEM.Insurance + ISNULL(CACD.DepotCharges, 0)) AS OnRoadPrice,
		(CACD.MT1MakeName + ' ' + CACD.MT1ModelName + ' ' + CACD.MT1VersionName) AS MT1CarName,
		(CEM.MT1ShowroomPrice + ISNULL(CEM.MT1RTO,0) + ISNULL(CEM.MT1Insurance,0) + ISNULL(CEM.MT1DepotCharges, 0)) AS MT1OnRoadPrice,
		CACD.MT1VersionId,
		(CACD.MT2MakeName + ' ' + CACD.MT2ModelName + ' ' + CACD.MT2VersionName) AS MT2CarName,
		(CEM.MT2ShowroomPrice + ISNULL(CEM.MT2RTO,0) + ISNULL(CEM.MT2Insurance,0) + ISNULL(CEM.MT2DepotCharges, 0)) AS MT2OnRoadPrice,
		CACD.MT2VersionId,
		(CACD.MT3MakeName + ' ' + CACD.MT3ModelName + ' ' + CACD.MT3VersionName) AS MT3CarName,
		(CEM.MT3ShowroomPrice + ISNULL(CEM.MT3RTO,0) + ISNULL(CEM.MT3Insurance,0) + ISNULL(CEM.MT3DepotCharges, 0)) AS MT3OnRoadPrice,
		CACD.MT3VersionId,
		CACD.Sm1ImageUrl,
		CACD.Sm1MakeName, 
		CACD.Sm1ModelName,
		CACD.Sm1ModelMaskingName,
		CACD.Sm1ModelId,
		CONVERT(DECIMAL(10,2), CACD.Sm1MinPrice/100000.0) AS Sm1MinPrice,
		CACD.Sm2ImageUrl,
		CACD.Sm2MakeName, 
		CACD.Sm2ModelName,
		CACD.Sm2ModelMaskingName,
		CACD.Sm2ModelId,
		CONVERT(DECIMAL(10,2), CACD.Sm2MinPrice/100000.0) AS Sm2MinPrice,
		CACD.Sm3ImageUrl,
		CACD.Sm3MakeName,
		CACD.Sm3ModelName,
		CACD.Sm3ModelMaskingName,
		CACD.Sm3ModelId,	
		CONVERT(DECIMAL(10,2), CACD.Sm3MinPrice/100000.0) AS Sm3MinPrice,	
		CACD.Sm4ImageUrl,
		CACD.Sm4MakeName, 
		CACD.Sm4ModelName,
		CACD.Sm4ModelMaskingName,
		CACD.Sm4ModelId,
		CONVERT(DECIMAL(10,2), CACD.Sm4MinPrice/100000.0) AS Sm4MinPrice
	FROM NCAlert.CustomerEligibleForMail CEM WITH(NOLOCK)
	INNER JOIN NCAlert.NewCarAlertEmailEntireCarData CACD WITH(NOLOCK)
		ON CEM.PQVersionId = CACD.VersionId
	WHERE
		(CACD.Sm4ModelId IS NOT NULL AND CACD.MT3VersionId IS NOT NULL)
	AND 
		(CEM.ShowroomPrice IS NOT NULL AND CEM.MT1ShowroomPrice IS NOT NULL AND CEM.MT2ShowroomPrice IS NOT NULL AND CEM.MT3ShowroomPrice IS NOT NULL)
    AND CEM.CustomerEmail  NOT LIKE '%@unknown.com'

 END 

