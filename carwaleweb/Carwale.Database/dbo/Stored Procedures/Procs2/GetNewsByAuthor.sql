IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewsByAuthor]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewsByAuthor]
GO

	
-- =============================================
-- Author:		Shalini
-- Create date:29/07/14
-- Description:	Gets the News List based on AuthorId passed 
-- =============================================
CREATE PROCEDURE [dbo].[GetNewsByAuthor] --EXEC GetNewsByAuthor 6
	-- Add the parameters for the stored procedure here
	@AuthorId INT
	,@ApplicationId int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT TOP 10 Title
		,Url
		,Id AS BasicId
		,DisplayDate
	FROM Con_EditCms_Basic WITH (NOLOCK)
	WHERE CategoryId = 1
		AND IsActive = 1
		AND IsPublished=1
		and ApplicationID=@ApplicationId
		AND AuthorId = @AuthorId
		ORDER BY DisplayDate DESC
END

