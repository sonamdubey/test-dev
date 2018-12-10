IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CAPostAnswer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CAPostAnswer]
GO

	
--THIS PROCEDURE IS FOR posting a answer in Carwale Answers


CREATE PROCEDURE [dbo].[CAPostAnswer]
	@QuestionId		NUMERIC, 
	@CustomerId		NUMERIC, 
	@Answer		VARCHAR(2000), 
	@AnswerDateTime	DATETIME,
	@AnswerId		NUMERIC OUTPUT
 AS
	
BEGIN
	
	--IT IS FOR THE INSERT
	INSERT INTO CAAnswers ( CAQuestionId, CustomerId, Answer, AnswerDateTime )
	VALUES ( @QuestionId, @CustomerId, @Answer, @AnswerDateTime )

		--get the forum id
	SET @AnswerId = SCOPE_IDENTITY()
		
END
