IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateQuestionHelpful]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateQuestionHelpful]
GO

	
--THIS PROCEDURE is for updating the count of the Question helpful and the disliked field

CREATE PROCEDURE [dbo].[UpdateQuestionHelpful]
	@QuestionId		NUMERIC, 
	@Helpful		BIT
 AS
	--DECLARE
		--@TempCount	AS NUMERIC
BEGIN
	
	IF @Helpful = 0		--disliked
		UPDATE AskUsQuestions
		SET	
			Disliked = IsNull(Disliked, 0) + 1
		WHERE
			ID = @QuestionId
	ELSE
		UPDATE AskUsQuestions
		SET	
			Liked = IsNull(Liked, 0) + 1
		WHERE
			ID = @QuestionId
			
END
