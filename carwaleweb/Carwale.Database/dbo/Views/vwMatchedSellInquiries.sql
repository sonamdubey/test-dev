IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwMatchedSellInquiries' AND
     DROP VIEW dbo.vwMatchedSellInquiries
GO

	
CREATE VIEW vwMatchedSellInquiries AS
SELECT cs.*,vw.ID AS PurchaseId
FROM (vwPurchaseInquiries vw LEFT JOIN vwCarsForSale cs ON vw.CarVersion=cs.CarVersionId)
WHERE vw.CarVersion <> 0 AND 
	YEAR(cs.MakeYear) >= Vw.StartYear AND 
	YEAR(cs.MakeYear) <= Vw.TmpYear AND
	cs.Kilometers >= Vw.StartMileage AND 
	cs.Kilometers <= Vw.tmpMileage AND
	cs.Price >= Vw.StartBudget AND 
	cs.Price <= vw.TmpBudget
	
UNION 

SELECT cs.*,vw.ID AS PurchaseId
FROM (((vwPurchaseInquiries vw LEFT JOIN CarModels Cm On vw.CarModel=Cm.Id )
	LEFT JOIN CarVersions Cv ON Cv.CarModelID=vw.CarModel ) 
	LEFT JOIN vwCarsForSale cs ON Cv.Id=cs.CarVersionId)
WHERE vw.CarVersion = 0 AND 
	YEAR(cs.MakeYear) >= Vw.StartYear AND 
	YEAR(cs.MakeYear) <= Vw.TmpYear AND
	cs.Kilometers >= Vw.StartMileage AND 
	cs.Kilometers <= Vw.tmpMileage AND
	cs.Price >= Vw.StartBudget AND 
	cs.Price <= vw.TmpBudget 

