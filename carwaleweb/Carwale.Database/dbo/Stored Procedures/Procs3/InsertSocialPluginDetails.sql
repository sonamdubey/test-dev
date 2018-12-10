IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertSocialPluginDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertSocialPluginDetails]
GO

	-- =============================================
-- Author:		<Ravi Koshal>
-- Create date: <08/13/2013>
-- Description:	<Inserts the facebook comment(s) and like count for a news url>
-- =============================================
CREATE procedure [dbo].[InsertSocialPluginDetails] -- EXEC InsertNewsPluginDetails (1 2 3) 
	@BasicId int,
	@LikeCount int,
	@CommentCount int,
	@TypeCategoryId int
AS
BEGIN
	SET NOCOUNT ON;
	IF(EXISTS(SELECT Id FROM SocialPluginsCount WHERE TypeId = @BasicId AND TypeCategoryId = @TypeCategoryId))
	UPDATE SocialPluginsCount SET FacebookLikecount = @LikeCount , FacebookCommentCount = @CommentCount WHERE TypeId = @BasicId AND TypeCategoryId = @TypeCategoryId
	ELSE
	Insert INTO SocialPluginsCount (TypeId,FacebookLikecount,FacebookCommentCount,TypeCategoryId) 
	VALUES (@BasicId,@LikeCount,@CommentCount,@TypeCategoryId)--Type Category Id added by Ravi 20/08/2013
	
END
