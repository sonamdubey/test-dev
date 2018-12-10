IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_SaveNewCarNationalPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_SaveNewCarNationalPrices]
GO

	


--Created ON 13 April 2010
--This Procedure is Used for Inserting/Updating Cars Showroom prices 
--With Details like Avgerage, Minimum, Maximum Price 
--EXEC NPrice_Cars 6, 1, '2010-03-05'
--Modified by  : Sachin Bharti on 5th July 2016
--Purpose : Remove the check of isactive condition from Con_NewCarNationalPrices table
--Modified by: Rakesh Yadav on 25, To calculate EMI on price change (EMI will be calculated on 85% of avg price, with tenure of 60 months and roi of 10.5%)
CREATE PROCEDURE [dbo].[Con_SaveNewCarNationalPrices]
(
	@VersionId	AS NUMERIC(18,0),
	@UserId		AS BIGINT,
	@UpdateOn	AS DATETIME 
)
AS
BEGIN
	
	IF NOT EXISTS(SELECT Id FROM Con_NewCarNationalPrices with(nolock) WHERE VersionId = @VersionId)--modified by Sachin Bharti on 5th July 2016
		BEGIN 
			--INSERT data if the versionId is new to NationalPricing_Cars
			INSERT INTO Con_NewCarNationalPrices(VersionId, AvgPrice, MinPrice, MaxPrice, CityCount, LastUpdatedBy, LastUpdatedOn)
			SELECT 
				   CarVersionId, ROUND(Convert(NUMERIC(18,2),AVG(Price)),0) AS AveragePrice, 
				   ROUND(MIN(PRICE),0) AS MinimumPrice, ROUND(MAX(PRICE),0) AS MaximumPrice, COUNT(CityId) AS CityCount, @UserId, @UpdateOn
			FROM NewCarShowroomPrices(NOLOCK)
			WHERE  CarVersionId = @VersionId
			GROUP BY CarVersionId
		END	
	ELSE
		BEGIN
			--Updating Data for VersionId already inserted with new data. 
			UPDATE Con_NewCarNationalPrices 
			SET AvgPrice = tbl.AveragePrice, 
				MinPrice = tbl.MinimumPrice, 
				MaxPrice = tbl.MaximumPrice, 
				CityCount = tbl.CityCount, 
				LastUpdatedBy = @UserId,
				LastUpdatedOn = @UpdateOn,
				EMI=dbo.CalculateEMI_NewVersion(tbl.AveragePrice*0.85,default,default)--calculate emi on 85% of avg price
			FROM 
				(
					SELECT 
						  ROUND(Convert(NUMERIC(18,2),AVG(Price)),0) AS AveragePrice, 
						  ROUND(MIN(PRICE), 0) AS MinimumPrice, 
						  ROUND(MAX(PRICE), 0) AS MaximumPrice, 
						  COUNT(CityId) AS CityCount
					FROM NewCarShowroomPrices(NOLOCK)
					WHERE  CarVersionId = @VersionId
					GROUP BY CarVersionId
				)tbl
			WHERE VersionId = @VersionId
		END	
END


