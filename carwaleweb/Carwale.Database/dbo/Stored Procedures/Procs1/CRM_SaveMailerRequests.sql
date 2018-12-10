IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveMailerRequests]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveMailerRequests]
GO

	

CREATE PROCEDURE [dbo].[CRM_SaveMailerRequests]
	@LeadId				NUMERIC,
	@NewName			VARCHAR(100) = NULL,
	@NewMobile			VARCHAR(15) = NULL,
	@IsCallback			BIT = 0,
	@IsTestDrive		BIT = 0,
	@IsQuotation		BIT = 0,
	@CityId				NUMERIC = NULL,
	@DealerId			NUMERIC = NULL
AS
	BEGIN
		UPDATE CRM_MailerRequests SET 
				NewName = @NewName, NewMobile = @NewMobile,
				IsCallback = @IsCallback, UpdatedOn = GETDATE()
		WHERE LeadId = @LeadId
		
		IF @@ROWCOUNT = 0
			BEGIN
			
				INSERT INTO CRM_MailerRequests
				(LeadId, NewName, NewMobile, IsCallback, UpdatedOn)
				VALUES(@LeadId, @NewName, @NewMobile, @IsCallback, GETDATE())
				
			END

END




