IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSimilarCarsByVersionIds]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSimilarCarsByVersionIds]
GO

	-- =============================================
-- Author:		amit verma					EXEC GetSimilarCarsByVersionIds '2522,2197'
-- Create date: 16/10/2013
-- Description:	Get Similar Cars By VersionIds
-- Modeification: by amit verma : added isnew column to return new cars only 17 feb
-- =============================================
CREATE PROCEDURE [dbo].[GetSimilarCarsByVersionIds]
	-- Add the parameters for the stored procedure here
	@VersionIds VARCHAR(MAX),
	@Count TINYINT = 3
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @RowCount INT
	DECLARE @Versions TABLE	(ID INT IDENTITY,VersionId INT,isNew INT, isModelNew INT, ModelId INT)	--added isnew column to return new cars only : 17 feb

	INSERT INTO @Versions (VersionId,isNew) (SELECT items,0 FROM dbo.SplitText(@VersionIds,','))	--added isnew column to return new cars only : 17 feb

	UPDATE V
	SET V.isNew = VW.IsVerionNew,V.isModelNew = VW.IsModelNew,V.ModelId = VW.ModelId
	FROM @Versions V
		LEFT JOIN vwMMV VW ON V.VersionId = VW.VersionId
	
	SET @RowCount = @@ROWCOUNT

	DECLARE @Counter INT = 1
	DECLARE @SimilarVersions VARCHAR(MAX) = ''
	WHILE (@Counter <= @RowCount)
	BEGIN
		SET @SimilarVersions = ''
		SELECT @SimilarVersions = SC.SimilarVersions FROM SimilarCars SC WITH(NOLOCK) WHERE SC.IsActive = 1 AND SC.VersionId = 
		(SELECT VersionId FROM @Versions WHERE ID = @Counter AND isNew = 1 AND isModelNew = 1)	--check isnew column to return new cars only :17 feb
		--SELECT @SimilarVersions

		SELECT TOP(@Count) VW.MakeId,VW.Make,VW.ModelId,VW.Model,VW.VersionId,VW.Version,VW.SmallPic,VW.MaskingName FROM dbo.SplitText(@SimilarVersions,',') SV
		INNER JOIN vwMMV VW ON SV.items = VW.VersionId
		WHERE VW.IsVerionNew = 1 AND VW.VersionId NOT IN (SELECT VersionId FROM @Versions) AND VW.ModelId NOT IN (SELECT ModelId FROM @Versions)

		SET @Counter += 1
	END
END
