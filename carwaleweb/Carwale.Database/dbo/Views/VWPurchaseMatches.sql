IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'VWPurchaseMatches' AND
     DROP VIEW dbo.VWPurchaseMatches
GO

	
CREATE VIEW VWPurchaseMatches AS

SELECT 
	vw.CarMake, 
	vw.Name AS Name , 
	COUNT( Si.Id ) AS Matches, 
	vw.StartYear,
	vw.EndYear,
	vw.StartBudget,
	vw.EndBudget,
	vw.StartMileage,
	vw.EndMileage,
	vw.DealerId,
	vw.Id
	
FROM 
	vwPurchaseInquiries vw 
	LEFT JOIN SellInquiries Si ON vw.CarVersion=Si.CarVersionId
WHERE vw.CarVersion <> 0  AND 
	YEAR(Si.MakeYear) >= Vw.StartYear AND 
	YEAR(Si.MakeYear) <= Vw.TmpYear AND
	Si.Kilometers >= Vw.StartMileage AND 
	Si.Kilometers <= Vw.tmpMileage AND
	Si.Price >= Vw.StartBudget AND 
	Si.Price <= vw.TmpBudget AND
	Si.DealerId NOT IN 
	( 
		SELECT IGNOREDID AS ID FROM DEALERIGNORED WHERE DEALERID=vw.DealerId 
		UNION SELECT DEALERID AS ID FROM DEALERRESTRICTED WHERE RESTRICTEDID=vw.DealerId 
		UNION SELECT Id FROM Dealers WHERE Status = 1 
	)
GROUP BY 
	vw.CarMake, vw.Name,vw.StartYear,vw.EndYear,vw.StartBudget,
	vw.EndBudget,vw.StartMileage,vw.EndMileage,vw.DealerId,vw.Id

UNION 

SELECT 
	vw.CarMake, 
	IsNull(vw.Name,'Any') AS Name , 
	COUNT( Si.Id ) AS Matches, 
	vw.StartYear,
	vw.EndYear,
	vw.StartBudget,
	vw.EndBudget,
	vw.StartMileage,
	vw.EndMileage,
	vw.DealerId,
	vw.Id
FROM 
	((( vwPurchaseInquiries vw 
	LEFT JOIN CarModels Cm On vw.CarModel=Cm.Id )
	LEFT JOIN CarVersions Cv ON Cv.CarModelID=vw.CarModel ) 
	LEFT JOIN SellInquiries Si ON Cv.Id=Si.CarVersionId)
WHERE 
	vw.CarVersion = 0 AND 
	YEAR(Si.MakeYear) >= Vw.StartYear AND
	 YEAR(Si.MakeYear) <= Vw.TmpYear AND
	Si.Kilometers >= Vw.StartMileage AND 
	Si.Kilometers <= Vw.tmpMileage AND
	Si.Price >= Vw.StartBudget AND 
	Si.Price <= vw.TmpBudget AND
	Si.DealerId NOT IN 
	( 
		SELECT IGNOREDID AS ID FROM DEALERIGNORED WHERE DEALERID=vw.DealerId 
		UNION SELECT DEALERID AS ID FROM DEALERRESTRICTED WHERE RESTRICTEDID=vw.DealerId 
		UNION SELECT Id FROM Dealers WHERE Status = 1 
	)
GROUP BY vw.CarMake, vw.Name,vw.StartYear,vw.EndYear,vw.StartBudget,
	vw.EndBudget,vw.StartMileage,vw.EndMileage,vw.DealerId,vw.Id














