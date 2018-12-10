IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelsForExpertReviews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelsForExpertReviews]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 19.11.2013
-- Description:	Get list of models for particular make for which expert reviews exist
-- exec GetModelsForExpertReviews 8
-- =============================================
CREATE PROCEDURE [dbo].[GetModelsForExpertReviews]
@MakeId int 
AS
BEGIN
	SELECT DISTINCT CM.id as Value
	,CM.NAME as Text, CM.MaskingName
FROM Con_EditCms_Basic CB
INNER JOIN Con_EditCms_Cars CC ON CB.id = CC.BasicId
INNER JOIN CarModels cm ON CC.ModelId = CM.ID
WHERE CB.CategoryId IN (
		8
		,2
		)
	AND MakeId = @MakeId
	AND CB.IsPublished=1 AND CB.IsActive=1
ORDER BY CM.NAME
END
