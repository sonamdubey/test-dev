IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PM_SaveDraft]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PM_SaveDraft]
GO

	--Created By Sentil on 03 Dec 2009 for Private Messaging
--Used to Save an Drafts for new and replied drafts

CREATE PROCEDURE [dbo].[PM_SaveDraft]
(
	@ConversationId AS NUMERIC(18,0),
	@Subject AS VARCHAR(100),
	@Message AS VARCHAR(2000),
	@ReceiverId AS NUMERIC(18,0), 
	@Operation BIT,
	@CreatedBy AS NUMERIC(18,0),
	@CreatedDate AS DATETIME
)
AS 
BEGIN
	
	--Operation = 1 is used save the drafts and Operation = 2 to convert draft to message(send button click)
	IF(@Operation = 1)
	BEGIN
		IF(@Subject = NULL OR @Subject = '')
			BEGIN
				SELECT @Subject = Subject FROM PM_Conversations WHERE id= @ConversationId
			END
		
		UPDATE PM_Conversations  
		SET 
			Subject = @Subject
		WHERE id = @ConversationId  AND CreatedBy = @CreatedBy
	 
		UPDATE PM_Messages  
		SET 
			Message = @Message, 
			PostedDate = @CreatedDate
		WHERE ConversationId = @ConversationId AND IsDraft = 1 AND CreatedBy = @CreatedBy
		
		UPDATE PM_Inbox
		SET
			ReceiverId = @ReceiverId, 
			CreatedDate = @CreatedDate
		WHERE MessageId IN 
			  (
				SELECT id FROM PM_Messages WHERE ConversationId = @ConversationId AND IsDraft = 1
			  ) 
			  AND CreatedBy = @CreatedBy	
	END
	ELSE IF(@Operation = 0)
		BEGIN
			UPDATE PM_Conversations 
			SET  IsDraft = 0,
				 NoOfMessages += 1, 
				 LastUpdatedBy = @CreatedBy, 
				 LastUpdatedOn = @CreatedDate
			WHERE Id = @ConversationId  		
				 
			UPDATE PM_Messages  
			SET IsDraft = 0,
				PostedDate = @CreatedDate
			WHERE ConversationId = @ConversationId AND IsDraft = 1 AND CreatedBy = @CreatedBy
			
			UPDATE PM_ConversationDetails 
			SET 
				IsActive = 1, 
				IsRead = 0,
				UpdatedDate = @CreatedDate
			WHERE ConversationId = @ConversationId AND UserId = @ReceiverId
			
		END			

END
