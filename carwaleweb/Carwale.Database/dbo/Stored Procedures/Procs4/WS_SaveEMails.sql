IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WS_SaveEMails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WS_SaveEMails]
GO
	
CREATE PROCEDURE [dbo].[WS_SaveEMails]

	@Type			SMALLINT,
	@EMailId		VARCHAR(100),	
	@CampaignName	VARCHAR(50),
	@IsAvlid		BIT OUTPUT	
				
 AS
	
BEGIN
	SET @IsAvlid = 0
	
	IF @Type = 1 -- CheckStatus

		BEGIN
			SELECT ID FROM Customers WITH (NOLOCK) WHERE Email = @EMailId
			IF @@ROWCOUNT > 0
				SET @IsAvlid = 1
		END

	ELSE IF @Type = 2 -- Save Mail
		
		BEGIN 
			SELECT CampaignName FROM WS_ConfirmedEmails WITH (NOLOCK)
			WHERE EMail = @EMailId
			
			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO WS_ConfirmedEmails VALUES(@EMailId, GETDATE(), @CampaignName)
					SET @IsAvlid = 1
				END
		END
END













