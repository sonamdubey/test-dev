IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetValuationMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetValuationMakes]
GO

	-- =============================================
-- Author:		<Kirtan Shetty>
-- Create date: <15/7/2014>
-- Description:	<Get car makes on year selection during valuation>
-- =============================================
CREATE PROCEDURE [dbo].[GetValuationMakes] 
	@CarYear SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT Ma.ID AS Value, Ma.Name AS Text 
	FROM CarMakes Ma WITH (NOLOCK)
	INNER JOIN CarModels Mo  WITH (NOLOCK) ON MO.CarMakeId = Ma.ID
	INNER JOIN CarVersions Ve  WITH (NOLOCK) ON Ve.CarModelId = MO.ID
	INNER JOIN CarValues CV  WITH (NOLOCK) ON CV.CarVersionId =  Ve.ID
	WHERE Ve.IsDeleted = 0 AND Ma.Id=Mo.CarMakeId AND Mo.Id=Ve.CarModelId 
	AND CV.CarYear = @CarYear
	ORDER BY Text
END


