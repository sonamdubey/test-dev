IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PostForumThread]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PostForumThread]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PostForumThread 

@ForumId		NUMERIC,
@CustomerId		NUMERIC,
@Message		VARCHAR(4000),
@MsgDateTime		DATETIME,
@AskUsQuestionId	NUMERIC,
@IsActive		BIT,
@IsApproved		BIT,
@ModeratorRecommendedPost	BIT,
@LastUpdatedTime	DATETIME,
@UpdatedBy		NUMERIC,
@ThreadId 		NUMERIC OUTPUT

AS
BEGIN
	INSERT INTO ForumThreads(ForumId, CustomerId, Message, MsgDateTime, IsActive,
		IsApproved,ModeratorRecommendedPost,LastUpdatedTime,UpdatedBy)
	VALUES (@ForumId, @CustomerId, @Message, @MsgDateTime, @IsActive,
		@IsApproved, @ModeratorRecommendedPost, @LastUpdatedTime, @UpdatedBy)

	SET @ThreadId = SCOPE_IDENTITY()
END

