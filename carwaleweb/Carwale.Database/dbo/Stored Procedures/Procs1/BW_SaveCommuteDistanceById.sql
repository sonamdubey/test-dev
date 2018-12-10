IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_SaveCommuteDistanceById]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_SaveCommuteDistanceById]
GO

	
-- =============================================
-- Author:		Sumit Kate
-- Create date: 09 May 2016
-- Description:	Updates Commute Distance by Id(dealer/area) specified by IdType and Id
--	@IdType			:	1 - Dealer, 2 - Area
--	@Id				:	Id
--	@DistanceMatrix	:	id-distance delimited values
-- e.g. EXEC [dbo].[BW_SaveCommuteDistanceById] 1, 36, '18990:4.9,18991:9.1,18992:8.1'
--	select * from [dbo].[SplitTextByTwoDelimiters]  ( '18990:51,18991:89,18992:79',',',':') dealerDistance 
-- =============================================
CREATE PROCEDURE [dbo].[BW_SaveCommuteDistanceById] 
	@IdType INT
	,@Id INT
	,@DistanceMatrix VARCHAR(MAX)
AS
BEGIN
	IF((@IdType IS NOT NULL) AND (@IdType BETWEEN 1 AND 2 ))
	BEGIN

		SELECT
			CONVERT(INT,CASE WHEN @IdType = 2 THEN AD.Val1 ELSE @id END) AS Dealerid,
 			CONVERT(INT,CASE WHEN @IdType = 1 THEN AD.Val1 ELSE @id END) AS AreaId,
			CONVERT(FLOAT,AD.Val2) AS Distance,
			GETDATE() AS EntryDate,
 			CONVERT(DATETIME,NULL) AS ModifiedDate,
			CONVERT(BIT,1) AS IsActive
			INTO #tmpDealerDistance
			FROM [dbo].[SplitTextByTwoDelimiters](@DistanceMatrix, ',', ':') AD

		IF EXISTS( select 1 from #tmpDealerDistance)
		BEGIN
			SELECT DISTINCT t.* INTO #tmpDAUpdate from #tmpDealerDistance t
			INNER JOIN DealerAreaCommuteDistance da WITH(NOLOCK) 
			ON t.Dealerid = da.Dealerid AND t.AreaId = da.AreaId

			SELECT DISTINCT t.* INTO #tmpDAInsert from #tmpDealerDistance t
			LEFT OUTER JOIN DealerAreaCommuteDistance da WITH(NOLOCK) 
			ON t.Dealerid = da.Dealerid AND t.AreaId = da.AreaId
			WHERE da.Dealerid IS NULL	
	
			IF EXISTS (SELECT 1 FROM #tmpDAUpdate)
			BEGIN
				UPDATE da
				SET da.Distance = uda.Distance, da.ModifiedDate = GETDATE()
				FROM DealerAreaCommuteDistance AS da
				INNER JOIN #tmpDAUpdate uda ON da.Dealerid = uda.Dealerid AND da.AreaId = uda.AreaId
			END

			IF EXISTS (SELECT 1 FROM #tmpDAInsert)
			BEGIN
				INSERT INTO DealerAreaCommuteDistance(Dealerid,AreaId,Distance,EntryDate,IsActive)
				SELECT Dealerid,AreaId,Distance,GETDATE(),1 from #tmpDAInsert
			END
			DROP TABLE #tmpDAUpdate
			DROP TABLE #tmpDAInsert
		END
		DROP TABLE #tmpDealerDistance 
	END
END
