IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwMatchedPurchaseInquiries' AND
     DROP VIEW dbo.vwMatchedPurchaseInquiries
GO

	
CREATE VIEW vwMatchedPurchaseInquiries AS
SELECT DISTINCT 
	Pi.Id,
	Pi.CarMake,
	IsNull(vw.Matches,0) AS Match , 
	IsNull(Pi.Name,'Any') AS Name, 
	Pi.StartYear,
	Pi.EndYear,
	Pi.StartBudget,
	Pi.EndBudget,
	Pi.StartMileage,
	Pi.EndMileage,
	Pi.DealerId
FROM 
	vwPurchaseInquiries Pi 
	LEFT JOIN vwPurchaseMatches vw ON Pi.Id=Vw.Id







