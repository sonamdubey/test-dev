IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CAPostQuestion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CAPostQuestion]
GO

	
--THIS PROCEDURE IS FOR posting a question in Carwale Answers


CREATE PROCEDURE [dbo].[CAPostQuestion]
	@CAQuestionCategoryId	NUMERIC, 
	@CustomerId		NUMERIC, 
	@Question		VARCHAR(500), 
	@QuestionDateTime	DATETIME,
	@Description		VARCHAR(2000),
	@QuestionId		NUMERIC OUTPUT
 AS
	
BEGIN
	
	--IT IS FOR THE INSERT
	INSERT INTO CAQuestions ( CACategoryId, CustomerId, QuestionTitle, QuestionDescription, QuestionDateTime )
	VALUES ( @CAQuestionCategoryId, @CustomerId, @Question, @Description, @QuestionDateTime )

		--get the forum id
	SET @QuestionId = SCOPE_IDENTITY()
		
END
