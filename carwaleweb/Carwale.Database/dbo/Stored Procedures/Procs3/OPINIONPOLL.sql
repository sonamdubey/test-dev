IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OPINIONPOLL]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OPINIONPOLL]
GO

	
CREATE PROCEDURE [dbo].[OPINIONPOLL]
	@Question	 VARCHAR(500),	
	@ID		NUMERIC,	--ID. IF IT IS -1 THEN IT IS FOR INSERT, ELSE IT IS FOR UPDATE FOR THE ID 		
	@CategoryId NUMERIC,
	@Ans1		VARCHAR(500),        	
	@Ans2		VARCHAR(500),
	@StartDate	DATETIME,
	@STATUS 	INTEGER OUTPUT	--return value, 0 FOR SUCCESSFULL  ATTEMPT, AND -1 FOR DUPLICATE ENTRY
 AS
	DECLARE @tempQuesId AS NUMERIC	

BEGIN
	SET @STATUS = 0	
	
	IF @ID = -1
	BEGIN
		--IT IS FOR THE INSERT
		INSERT INTO OpinionPollQues( Question , StartDate, CategoryId  ) VALUES( @Question , @StartDate, @CategoryId )

		SET @tempQuesId = SCOPE_IDENTITY()

		INSERT INTO OpinionPollAnswer( QuestionID , Answer  ) VALUES( @tempQuesId , @Ans1 )

		INSERT INTO OpinionPollAnswer( QuestionID , Answer  ) VALUES( @tempQuesId , @Ans2 )

		SET @STATUS = @tempQuesId	
		
	END
	ELSE
	BEGIN
		--IT IS FOR THE UPDATE
		UPDATE OpinionPollQues SET Question = @Question, startDate = @StartDate, CategoryId = @CategoryId  WHERE ID = @ID
			IF @Ans1 <> ''
			BEGIN
				INSERT INTO OpinionPollAnswer( QuestionID , Answer  ) VALUES( @ID , @Ans1 )
			END
			
			IF @Ans2 <> ''
			BEGIN
				INSERT INTO OpinionPollAnswer( QuestionID , Answer  ) VALUES( @ID , @Ans2 )
			END
		SET @STATUS = 1	
	END
	
END
