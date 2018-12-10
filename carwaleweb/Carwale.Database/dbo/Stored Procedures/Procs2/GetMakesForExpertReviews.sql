IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMakesForExpertReviews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMakesForExpertReviews]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 19.11.2013
-- Description:	Get list of makes for which expert reviews exist
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- exec GetMakesForExpertReviews

-- =============================================
CREATE PROCEDURE [dbo].[GetMakesForExpertReviews]
@ApplicationId int
AS
BEGIN
	SELECT DISTINCT CM.ID AS Value
		,CM.NAME AS TEXT
	FROM Con_EditCms_Basic CB
	INNER JOIN Con_EditCms_Cars CC ON CB.id = CC.BasicId
	INNER JOIN CarMakes CM ON CC.MakeId = CM.ID
	WHERE CB.CategoryId IN (
			8
			,2
			)
			AND CB.ApplicationID = @ApplicationId
		AND CB.IsPublished = 1
		AND CB.IsActive = 1
	ORDER BY CM.NAME
END


