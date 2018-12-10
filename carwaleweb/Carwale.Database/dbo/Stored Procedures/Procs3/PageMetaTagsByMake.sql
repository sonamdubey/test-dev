IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PageMetaTagsByMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PageMetaTagsByMake]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 24/11/14
-- Description:	<Gets the MakePage MetaTags based on MakeId passed>
-- =============================================
CREATE PROCEDURE [dbo].[PageMetaTagsByMake]
	-- Add the parameters for the stored procedure here
	@PageId numeric ,
	@MakeId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MakeId,Title,Description,Keywords,Heading,Summary,IsActive FROM PAGEMETATAGS WITH(NOLOCK) WHERE ISACTIVE=1  and PageId=@PageId and MakeId = @MakeId
END

