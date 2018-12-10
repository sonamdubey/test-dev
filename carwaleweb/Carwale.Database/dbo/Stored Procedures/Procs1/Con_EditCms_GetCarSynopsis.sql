IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_GetCarSynopsis]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_GetCarSynopsis]
GO

	-- =============================================
-- Author:		amit verma
-- Create date: 18/12/2013
-- Description:	Get Car Synopsis by ModelId
/*
			EXEC Con_EditCms_GetCarSynopsis @Modelid=481,@Priority=NULL
*/
-- =============================================
CREATE PROCEDURE [dbo].[Con_EditCms_GetCarSynopsis]
	-- Add the parameters for the stored procedure here
	@ModelID INT,
	@Priority INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CP.Priority,CP.PageName,CPC.Data
	FROM Con_EditCms_Cars CC WITH(NOLOCK)
	LEFT JOIN Con_EditCms_Basic CB WITH(NOLOCK) ON CC.BasicId = CB.Id
	LEFT JOIN Con_EditCms_Pages CP WITH(NOLOCK) ON CC.BasicId = CP.BasicId
	LEFT JOIN Con_EditCms_PageContent CPC WITH(NOLOCK) ON CP.Id = CPC.PageId
	WHERE CC.ModelId = @ModelID
	AND CB.CategoryId = 14 AND CB.IsActive = 1 AND CB.IsPublished = 1
	AND (@Priority IS NULL OR CP.Priority = @Priority)
END

