IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAuthorDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAuthorDetails]
GO

	
-- =============================================
-- Author:		Shalini
-- Create date: 17/07/14
-- Description:	Gets the author details
-- =============================================
CREATE PROCEDURE [dbo].[GetAuthorDetails]		--EXEC GetAuthorDetails 2
	-- Add the parameters for the stored procedure here
	@AuthorId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT OU.UserName AS AuthorName
		,CEA.Designation
		,CEA.ProfileImgUrl
		,CEA.BriefProfile
		,CEA.FullProfile
		,CEA.EmailId
		,CEA.FacebookProfile
		,CEA.GooglePlusProfile
		,CEA.LinkedInProfile
		,CEA.TwitterProfile
		,CEA.MaskingName
		,CEA.ImageName
		,CEA.HostURL
	FROM Con_EditCms_Author CEA WITH (NOLOCK) JOIN OprUsers OU
	ON CEA.Authorid=OU.Id
	AND  CEA.Authorid = @AuthorId
		--AND CEA.IsActive = 1 commented by natesh on 5/1/15 as isactive flag is used for checking if author is present in carwale 
END
