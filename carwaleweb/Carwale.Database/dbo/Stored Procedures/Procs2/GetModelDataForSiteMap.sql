IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelDataForSiteMap]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelDataForSiteMap]
GO

	-- =============================================
-- Author:		amit verma
-- Create date: 28 mar 2014
-- Description:	return model details from compare cars sitemap generation
-- =============================================
/*
	EXEC GetModelDataForSiteMap 353
*/
CREATE PROCEDURE [dbo].[GetModelDataForSiteMap]
	@ModelId INT = -1
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF(@ModelId != -1)
	BEGIN		
		DECLARE @RelModeLs TABLE (ModelId INT)
		DECLARE @SubSegmentId INT = (SELECT CASE SubSegmentID WHEN NULL THEN -1 ELSE SubSegmentID END FROM CarModels  WITH(NOLOCK) WHERE ID = @ModelId)
		IF(@SubSegmentId != -1)
		BEGIN
			INSERT INTO @RelModeLs
			SELECT ID FROM CarModels WITH(NOLOCK) WHERE SubSegmentID BETWEEN (@SubSegmentID - 1) AND (@SubSegmentId + 1)
			
			SELECT CM.ID ModelId,CMA.Name Make,CM.Name Model,CM.MaskingName,CM.SubSegmentID,CM.CarVersionID_Top
			FROM @RelModeLs M
			INNER JOIN CarModels CM WITH(NOLOCK) ON M.ModelId = CM.ID
			INNER JOIN CarVersions CV WITH(NOLOCK) ON CM.CarVersionID_Top = CV.ID
			INNER JOIN CarMakes CMA WITH(NOLOCK) ON CM.CarMakeId = CMA.ID	
			WHERE CM.NEW = 1 AND CM.IsDeleted = 0 AND CM.CarVersionID_Top IS NOT NULL AND CV.NEW = 1 AND CV.IsDeleted = 0 AND CM.SubSegmentID IS NOT NULL
			AND CM.ID != @ModelId
			
			SELECT CM.ID ModelId,CMA.Name Make,CM.Name Model,CM.MaskingName,CM.SubSegmentID,CM.CarVersionID_Top
			FROM CarModels CM WITH(NOLOCK)
			INNER JOIN CarVersions CV WITH(NOLOCK) ON CM.CarVersionID_Top = CV.ID
			INNER JOIN CarMakes CMA WITH(NOLOCK) ON CM.CarMakeId = CMA.ID	
			WHERE CM.NEW = 1 AND CM.IsDeleted = 0 AND CM.CarVersionID_Top IS NOT NULL AND CV.NEW = 1 AND CV.IsDeleted = 0 AND CM.SubSegmentID IS NOT NULL
			AND CM.ID = @ModelId
		END
	END
	ELSE
	BEGIN
		SELECT CM.ID ModelId,CMA.Name Make,CM.Name Model,CM.MaskingName,CM.SubSegmentID,CM.CarVersionID_Top
		FROM CarModels CM WITH(NOLOCK)
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CM.CarVersionID_Top = CV.ID
		INNER JOIN CarMakes CMA WITH(NOLOCK) ON CM.CarMakeId = CMA.ID	
		WHERE CM.NEW = 1 AND CM.IsDeleted = 0 AND CM.CarVersionID_Top IS NOT NULL AND CV.NEW = 1 AND CV.IsDeleted = 0 AND CM.SubSegmentID IS NOT NULL
	END
END

