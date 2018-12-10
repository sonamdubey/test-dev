IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUsedCarsCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUsedCarsCount]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 07/11/14
-- Description:	Fetches the Count of Used Cars listed of Car Models  
-- =============================================
CREATE PROCEDURE [dbo].[GetUsedCarsCount]
	-- Add the parameters for the stored procedure here
	@MakeId SMALLINT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
SELECT Mo.NAME as ModelName
		,Mo.ID AS Value
		,Ma.NAME AS CarMake
		,Mo.RootId as rootId
		,NULL AS MinLiveListingPrice
		,NULL AS LiveListingCount
		
	INTO #tempModel
	FROM CarModels Mo WITH (NOLOCK)
	INNER JOIN CarMakes Ma WITH(NOLOCK) ON Ma.ID = Mo.CarMakeId
	WHERE (
			Mo.CarMakeId = @MakeId
			OR @MakeId = 0
			)
		AND Mo.IsDeleted = 0

	
	UPDATE #tempModel
	SET MinLiveListingPrice = LLData.Price
		,LiveListingCount = LLData.CNT
	FROM (
		SELECT RootId
			,MIN(Price) AS Price
			,COUNT(ProfileId) CNT
		FROM LiveListings WITH (NOLOCK)
		GROUP BY RootId
		) AS LLData
	WHERE LLData.RootId =#tempModel.rootId

	

	SELECT *
	FROM #tempModel

	DROP TABLE #tempModel


END

