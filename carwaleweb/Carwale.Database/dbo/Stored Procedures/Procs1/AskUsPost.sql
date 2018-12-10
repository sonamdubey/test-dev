IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AskUsPost]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AskUsPost]
GO

	
--THIS PROCEDURE IS FOR POSTING QUESTION ON CUSTOMER BEHALF

CREATE PROCEDURE [AskUsPost]
	@QuestionId		NUMERIC,
	@CategoryId		NUMERIC, 
	@PosterEmail		VARCHAR(50),
	@Title			VARCHAR(200), 
	@RequestDateTime	DATETIME,
	@Post			VARCHAR(2000),
	@IsQuestioner		BIT
 AS
	DECLARE @CustomerId  AS	NUMERIC
BEGIN
	SELECT @CustomerId= Id FROM Customers WHERE Email=@PosterEmail

	IF @CustomerId IS NOT NULL
	BEGIN
		IF @QuestionId = -1
		BEGIN	
		INSERT INTO AskUsQuestions(CategoryId, CustomerId, Title, RequestDateTime)
		VALUES(@CategoryId, @CustomerId, @Title, @RequestDateTime)
		
		SET @QuestionId = SCOPE_IDENTITY() 
	
		END
		ELSE
		UPDATE AskUsQuestions SET CategoryId=@CategoryId, Title=@Title WHERE Id=@QuestionId
			
		-- Now Execute the Post Insertion Procedure
		EXEC InsertAskUsReplies @QuestionId,@Post,@RequestDateTime,@CustomerId,@IsQuestioner
	END	
	
	
END
