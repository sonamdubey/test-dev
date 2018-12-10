IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertAskUsReplies]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertAskUsReplies]
GO

	
--THIS PROCEDURE IS FOR POSTING QUESTION ON CUSTOMER BEHALF

CREATE PROCEDURE [InsertAskUsReplies]
	@QuestionId		NUMERIC,
	@Post			VARCHAR(2000), 
	@PostDateTime		DATETIME,
	@Poster		NUMERIC,
	@IsQuestioner		BIT 
 AS

BEGIN
	
	IF @QuestionId IS NOT NULL AND @QuestionId > 0 
	BEGIN	
	INSERT INTO AskUsReplies(QuestionId, Post, PostDateTime, PostedBy, IsQuestioner)
	VALUES (@QuestionId, @Post, @PostDateTime, @Poster, @IsQuestioner)
	END	
END
