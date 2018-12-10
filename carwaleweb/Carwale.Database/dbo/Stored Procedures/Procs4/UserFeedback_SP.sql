IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UserFeedback_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UserFeedback_SP]
GO

	-- PROCEDUTRE TO SAVE USER FEEDBACK TO THE TABLE "UserFeedback"

CREATE PROCEDURE [dbo].[UserFeedback_SP]
	@CustomerId			NUMERIC,
	@Feedback		 	VARCHAR(20),
	@Comments			VARCHAR(200),
	@SourceURL			VARCHAR(200),
	@ClientIP			VARCHAR(50),
	@ClientBrowserType		VARCHAR(150),
	@EntryDate			DATETIME,
	@Status			INTEGER OUTPUT
 AS
	
BEGIN
	INSERT INTO UserFeedback(CustomerId, Feedback, Comments, SourceURL, ClientIP, ClientBrowserType, EntryDate )
	VALUES (@CustomerId, @Feedback, @Comments, @SourceURL, @ClientIP, @ClientBrowserType,@EntryDate)	
	
	IF SCOPE_IDENTITY()  > 0
		SET @Status = 1
	ELSE
		SET @Status = 0
	
END