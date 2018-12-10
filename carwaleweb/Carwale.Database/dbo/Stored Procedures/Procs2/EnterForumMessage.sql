IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EnterForumMessage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EnterForumMessage]
GO

	--THIS PROCEDURE IS FOR entering message into the forumThreads table



--ForumThreads ID, ForumId, CustomerId, Message, MsgDateTime, IsActive, IsApproved


-- Modified By : Ravi Koshal
--MOdified Date: 6/27/2013
--Description :  Message Size Increased.

CREATE  PROCEDURE [dbo].[EnterForumMessage]

	@ForumId		NUMERIC, 

	@CustomerId		NUMERIC, 

	@Message		VARCHAR(MAX), 

	@MsgDateTime		DATETIME,

	@AlertType		INT, -- if 100 no action will be taken

	@ClientIPRemote    VARCHAR(50) = NULL,
	
	@ClientIP  VARCHAR(50) = NULL,

	@PostId		NUMERIC = NULL OUTPUT,
	
	@IsModerated INT

 AS

	

BEGIN

	

	--IT IS FOR THE INSERT

	INSERT INTO ForumThreads

		(

			ForumId, 	CustomerId, 	Message, 	MsgDateTime, 		IsActive, 	IsApproved, ClientIPRemote, ClientIP , IsModerated

		)

		VALUES

		(	

			@ForumId, 	@CustomerId, 	@Message, 	@MsgDateTime, 	1, 		0, @ClientIPRemote , @ClientIP, @IsModerated

		)



	SET @PostId = SCOPE_IDENTITY()	



	-- now insert alerts

	-- if customer is not logged in, no alert can be set. 

	-- Also AlertType=100 means no action to be taken. 

	IF @CustomerId <> -1 AND @AlertType <> 100

	BEGIN

		-- delete existing alert, if any 

		DELETE FROM ForumSubscriptions WHERE CustomerId=@CustomerId AND ForumThreadId=@ForumId

		

		-- create new alert	

		IF @AlertType <> 0

		BEGIN

			INSERT INTO ForumSubscriptions (CustomerId, ForumThreadId, EmailSubscriptionId)

			VALUES (@CustomerId, @ForumId, @AlertType)

		END

	END	



	

END

