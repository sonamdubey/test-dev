IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllOtherAuthors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllOtherAuthors]
GO

	
-- =============================================
-- Author:		Shalini
-- Create date: 30/07/14
-- Description:	Gets the list of all other authors based on AuthorId passed 
--Modified by: Rakesh Yadav on 3 march 2016, resolved Designation with Con_EditCms_Author which is present in both tables
-- =============================================
CREATE PROCEDURE [dbo].[GetAllOtherAuthors] --exec GetAllOtherAuthors 2
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
		,ProfileImgUrl
		,MaskingName
		,ImageName
		,CEA.HostURL
	FROM Con_EditCms_Author CEA WITH (NOLOCK)
	JOIN OprUsers OU WITH (NOLOCK) ON CEA.Authorid = OU.Id
		AND CEA.Authorid ! = @AuthorId
		AND CEA.IsActive = 1
		order by OU.UserName
END
