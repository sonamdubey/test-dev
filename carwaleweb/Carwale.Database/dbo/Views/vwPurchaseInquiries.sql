IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwPurchaseInquiries' AND
     DROP VIEW dbo.vwPurchaseInquiries
GO

	
CREATE VIEW dbo.vwPurchaseInquiries
AS
SELECT     TOP 100 PERCENT Pi.ID, Ma.Name + ' ' + Mo.Name AS CarMake, 
	Pi.CarModelId AS CarModel, Pi.CarVersionId AS CarVersion, Ve.Name, 
                      Pd.StartBudget, Pd.EndBudget,
		Pd.StartYear, Pd.EndYear,  Mo.CarMakeId,
		Pd.StartMileage, Pd.EndMileage,Pi.DealerId, Pi.Comments,
		TmpYear = (CASE WHEN Pd.EndYear=0 THEN '9999' ELSE Pd.EndYear END) ,
		TmpBudget = (CASE WHEN Pd.EndBudget=0 THEN '999999999' ELSE Pd.EndBudget END) ,
		TmpMileage = (CASE WHEN Pd.EndMileage=0 THEN '9999999' ELSE Pd.EndMileage END) 
FROM         dbo.PurchaseInquiries Pi LEFT OUTER JOIN
                      dbo.PurchaseInquiriesDetails Pd ON Pi.ID = Pd.PurchaseInquiryId LEFT OUTER JOIN
                      dbo.CarModels Mo ON Pi.CarModelId = Mo.ID LEFT OUTER JOIN
                      dbo.CarMakes Ma ON Ma.ID = Mo.CarMakeId LEFT OUTER JOIN
                      dbo.CarVersions Ve ON Pi.CarVersionId = Ve.ID
ORDER BY CarMake

