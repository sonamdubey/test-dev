IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMatchingTyres_TyreGuide]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMatchingTyres_TyreGuide]
GO

	
-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <20/02/2013>
-- Description:	<Fetches corresponding items(tyres) for a particular version>EXEC GetMatchingTyres_TyreGuide 2209
-- =============================================
CREATE PROCEDURE [dbo].[GetMatchingTyres_TyreGuide] 
	-- Add the parameters for the stored procedure here
	@Version INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here


	DECLARE @FrontWheelDiameter INT;
	DECLARE @FrontWheelSectionWidth INT;
	DECLARE @FrontTyreProfile INT;

	SELECT @FrontWheelDiameter = FrontWheelDiameter
		,@FrontWheelSectionWidth = FrontWheelSectionWidth
		,@FrontTyreProfile = FrontTyreProfile
	FROM Carwale_com..NewCarSpecifications
	WHERE CarVersionId = @Version;

	WITH CTE
	AS (
		SELECT DISTINCT ItemId
			,COUNT(FeatureId) CNT
		FROM Carwale_com..Acc_ItemsFeatures
		WHERE FeatureId IN (
				130
				,131
				,132
				)
			AND NumericValue IN (
				@FrontWheelDiameter
				,@FrontWheelSectionWidth
				,@FrontTyreProfile
				)
		GROUP BY ItemId
		HAVING COUNT(FeatureId) = 3
		)
	SELECT AB.BrandName,AI.*
	FROM Carwale_com..Acc_Items AI
	INNER JOIN CTE ON CTE.ItemId = AI.Id 
	INNER JOIN Carwale_com..Acc_Brands AB ON AB.Id=AI.BrandId
	WHERE BrandId=99
	
	END


