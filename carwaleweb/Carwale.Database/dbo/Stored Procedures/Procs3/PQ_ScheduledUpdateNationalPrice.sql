IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_ScheduledUpdateNationalPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_ScheduledUpdateNationalPrice]
GO

	-- =============================================
-- Author:		Chetan Thambad
-- Create date: 13/07/2016
-- Description:	This will select version Ids whose price updated on last day and will into update into Con_NewCarNationalPrices table 
-- =============================================
CREATE PROCEDURE [dbo].[PQ_ScheduledUpdateNationalPrice]
AS
BEGIN
	MERGE Con_NewCarNationalPrices AS NNP
	USING (
		SELECT CarVersionId
			,ROUND(Convert(NUMERIC(18, 2), AVG(Price)), 0) AS AveragePrice
			,ROUND(MIN(PRICE), 0) AS MinimumPrice
			,ROUND(MAX(PRICE), 0) AS MaximumPrice
			,COUNT(CityId) AS CityCount
		FROM NewCarShowroomPrices WITH (NOLOCK)
		WHERE CarVersionId IN (
				SELECT DISTINCT CarVersionId
				FROM NewCarShowroomPrices WITH (NOLOCK)
				WHERE LastUpdated > GETDATE() - 1
				)
		GROUP BY CarVersionId
		) AS NCP
		ON NNP.versionid = NCP.carversionid
	WHEN MATCHED
		THEN
			UPDATE
			SET AvgPrice = NCP.AveragePrice
				,MinPrice = NCP.MinimumPrice
				,MaxPrice = NCP.MaximumPrice
				,CityCount = NCP.CityCount
				,LastUpdatedBy = 14
				,LastUpdatedOn = getdate()
	WHEN NOT MATCHED BY TARGET
		THEN
			INSERT (
				VersionId
				,AvgPrice
				,MinPrice
				,MaxPrice
				,CityCount
				,LastUpdatedBy
				,LastUpdatedOn
				)
			VALUES (
				NCP.CarVersionId
				,NCP.AveragePrice
				,NCP.MinimumPrice
				,NCP.MaximumPrice
				,NCP.CityCount
				,14
				,GETDATE()
				);
END

