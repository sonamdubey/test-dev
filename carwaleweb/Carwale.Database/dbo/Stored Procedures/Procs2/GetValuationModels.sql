IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetValuationModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetValuationModels]
GO

	
-- =============================================
-- Author:		<Kirtan Shetty>
-- Create date: <15/7/2014>
-- Description:	<Get car models on make selection during valuation>
-- =============================================
CREATE PROCEDURE [dbo].[GetValuationModels] @CarYear SMALLINT
	,@MakeId SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT Mo.ID AS Value
		,Mo.NAME AS TEXT
	FROM CarModels Mo WITH (NOLOCK)
	INNER JOIN CarVersions Ve  WITH (NOLOCK) ON Ve.CarModelId = Mo.ID
	INNER JOIN CarValues Cv  WITH (NOLOCK) ON Cv.CarVersionId = Ve.ID
	WHERE Ve.IsDeleted = 0
		AND Mo.Id = Ve.CarModelId
		AND Cv.CarYear = @CarYear
		AND CarMakeId = @MakeId
	ORDER BY TEXT
END


