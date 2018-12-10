IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveLeadTranferLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveLeadTranferLog]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveLeadTranferLog]
	@LeadId				Numeric,
	@TransferTeamId		Numeric,
	@TransferredTeamId	Numeric,
	@TransferDate		DateTime,
	@TransferredBy		Numeric,
	@Comments			VarChar(500),
	@Status				Bit OutPut	
				
 AS
	
BEGIN
	SET @Status = 0
	
	INSERT INTO CRM_LeadTransferLog
	(
		LeadId, TransferTeamId, TransferredTeamId, TransferDate, TransferredBy, Comments
	)
	VALUES
	(
		@LeadId, @TransferTeamId, @TransferredTeamId, @TransferDate, @TransferredBy, @Comments
	)
	
	SET @Status = 1
END














