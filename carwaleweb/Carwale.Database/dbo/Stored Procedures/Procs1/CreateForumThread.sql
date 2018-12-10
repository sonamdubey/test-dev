IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CreateForumThread]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CreateForumThread]
GO

	--THIS PROCEDURE IS FOR creating new thread
--Forums ID, ForumSubCategoryId, CustomerId, Topic, StartDateTime, IsActive


--Modified By : Ravi Koshal
--MOdified Date : 6/27/2013
--Description : IsModerated column added to the forums and forumthreads table.
CREATE  PROCEDURE [dbo].[CreateForumThread]
	@ForumSubCategoryId	NUMERIC, 
	@CustomerId		NUMERIC, 
	@Topic			VARCHAR(200), 
	@StartDateTime		DATETIME,
	@Message		VARCHAR(4000),
	@AlertType		INT,
	@IsModerated  INT,
	@Url VARCHAR(500),
	@ClientIPRemote VARCHAR(50) = NULL,
	@ClientIP    VARCHAR(50)=NULL,
	@ThreadId		NUMERIC OUTPUT
 AS
	DECLARE @ForumId AS NUMERIC,
		 @PostId AS NUMERIC
BEGIN
	
	--IT IS FOR THE INSERT
	INSERT INTO Forums
		(
			ForumSubCategoryId, 	CustomerId, 	Topic, 		StartDateTime, 		IsActive,	IsApproved, ClientIPRemote,ClientIP,IsModerated,Url
		)
		VALUES
		(	
			@ForumSubCategoryId, 	@CustomerId, 	@Topic, 	@StartDateTime, 	1,		0,  @ClientIPRemote,@ClientIP,@IsModerated,@Url
		)

		--get the forum id
		SET @ForumId = SCOPE_IDENTITY()
		
		SET @ThreadId=@ForumId
		
		EXEC EnterForumMessage @ForumId, @CustomerId, @Message, @StartDateTime, @AlertType,@ClientIPRemote,@ClientIP, @PostId,@IsModerated
		
END

