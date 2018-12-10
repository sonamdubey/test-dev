IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PM_ComposeMessage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PM_ComposeMessage]
GO

	--Created By Sentil on 02 Dec 2009 for Private Messaging
--Used to make an entry for messaging

CREATE PROCEDURE [dbo].[PM_ComposeMessage] 
(
	@ConversationId AS NUMERIC(18,0),
	@SenderId AS NUMERIC(18,0),
	@ReceiverId AS NUMERIC(18,0),
	@Subject AS VARCHAR(100),
	@Message AS VARCHAR(2000),
	@IsDraft AS BIT,
	@Status AS VARCHAR(5),
	@CreatedBy AS NUMERIC(18,0),
	@CreatedDate AS DATETIME,
	@RetInboxID AS NUMERIC(18,0) OUT
)
AS
BEGIN

DECLARE @ConvID AS NUMERIC(18,0)
DECLARE @MessageId AS NUMERIC(18,0)
	
	--IF the entry is new one then insert it into PM_Conversations table 
	IF(@Status = 'N')
		BEGIN
			INSERT INTO PM_Conversations ( Subject, IsDraft, NoOfMessages, CreatedBy ) 
						VALUES		     ( @Subject, @IsDraft, 1, @CreatedBy )
			
			SET @ConvID = SCOPE_IDENTITY()

			INSERT INTO PM_ConversationDetails (ConversationId, UserId, UpdatedDate)
						VALUES				   ( @ConvID, @SenderId, @CreatedDate)
				
			INSERT INTO PM_ConversationDetails (ConversationId, UserId, UpdatedDate)
						VALUES				   ( @ConvID, @ReceiverId, @CreatedDate)
			
		END				
	ELSE
		BEGIN
			IF(@IsDraft = 0)
				BEGIN
					UPDATE PM_Conversations 
					SET  NoOfMessages += 1, 
						 LastUpdatedBy = @CreatedBy, 
						 LastUpdatedOn = @CreatedDate,
						 CreatedDate = @CreatedDate WHERE Id = @ConversationId  
				END	 
			IF(@IsDraft = 1)
				BEGIN
					UPDATE PM_Conversations 
					SET	 LastUpdatedBy = @CreatedBy, 
						 LastUpdatedOn = @CreatedDate
					WHERE Id = @ConversationId 

				END		
			SET @ConvID = @ConversationId
		END	
			
	INSERT INTO PM_Messages ( ConversationId, Message, IsDraft,CreatedBy )
				VALUES		( @ConvID, @Message, @IsDraft,@CreatedBy )	

	SET @MessageId = SCOPE_IDENTITY()
		
	INSERT INTO PM_Sent ( MessageId, SenderId, CreatedBy )
				VALUES	( @MessageId, @SenderId, @CreatedBy	)	
	
	INSERT INTO PM_Inbox ( MessageId, ReceiverId, CreatedBy	)
				VALUES   ( @MessageId, @ReceiverId, @CreatedBy )	

	UPDATE PM_ConversationDetails 
	SET 
		IsActive = 1, 
		IsRead = 0,
		UpdatedDate = @CreatedDate
	WHERE ConversationId = @ConvID AND UserId = @ReceiverId
	
	SET @RetInboxID = SCOPE_IDENTITY()
END
