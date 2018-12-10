IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCMSSubcategories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCMSSubcategories]
GO

	-- =============================================
-- Author	:	<Sachin Bharti>
-- Create date	:	<24/09/15>
-- Description	:	<Get CMS subcategories list based on category>
-- =============================================
CREATE PROCEDURE [cw].[GetCMSSubcategories]
	@CategoryId	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	
		Id
		,Name 
	FROM 
		Con_EditCms_SubCategories WITH (NOLOCK)
	WHERE 
		CategoryId=@CategoryId
		AND IsActive=1 
		AND ID NOT IN (21,22)
END


