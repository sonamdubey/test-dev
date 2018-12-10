IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAuthorsList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAuthorsList]
GO

	
-- =============================================
-- Author:		Shalini
-- Create date: 18/07/14
-- Description:	Gets the authors list 
-- =============================================
CREATE PROCEDURE [dbo].[GetAuthorsList]     -- EXEC GetAuthorsList
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT OU.UserName AS AuthorName
		, CEA.BriefProfile 
		,CEA.Designation
		,CEA.ProfileImgUrl
		,CEA.MaskingName
		,CEA.HostURL
		,CEA.ImageName
	FROM Con_EditCms_Author CEA WITH (NOLOCK)
	JOIN OprUsers OU ON CEA.Authorid=OU.Id
	AND CEA.IsActive=1 
	order by OU.UserName
END

