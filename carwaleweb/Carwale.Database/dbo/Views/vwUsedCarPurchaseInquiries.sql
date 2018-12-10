IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwUsedCarPurchaseInquiries' AND
     DROP VIEW dbo.vwUsedCarPurchaseInquiries
GO

	
Create VIEW vwUsedCarPurchaseInquiries AS

SELECT  Pi.ID, CarModelNames,CarModelIds, SellInquiryId,
	        Pi.PriceFrom, Pi.PriceTo,
		Pi.YearFrom, Pi.YearTo,
		Pi.KmFrom, Pi.KmTo, Pi.Comments,
		TmpYear = (CASE WHEN Pi.YearTo=0 THEN '9999' ELSE Pi.YearTo END) ,
		TmpPrice = (CASE WHEN Pi.PriceTo=0 THEN '999999999' ELSE Pi.PriceTo END) ,
		TmpKm = (CASE WHEN Pi.KmTo=0 THEN '9999999' ELSE Pi.KmTo END) 
FROM         UsedCarPurchaseInquiries Pi
WHERE SellInquiryId IS NULL



