IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SkodaSaveHotLeadData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SkodaSaveHotLeadData]
GO

	



CREATE PROCEDURE [dbo].[CRM_SkodaSaveHotLeadData]

	@CBDId			NUMERIC,
	@LeadId			Numeric,
	@TokenId		VarChar(50),
	@ErrorCode		VarChar(50),
	@ErrorMessage	VarChar(500),
	@IncomingXML	VarChar(MAX)= null,
	@OutGoingXML	VarChar(MAX) = null,
	@LeadStatus		VarChar(50)
	
 AS
	DECLARE @Id AS Numeric
	
BEGIN
	
	UPDATE CRM_SkodaLeadhotStatus SET TokenId = @TokenId, ErrorCode = @ErrorCode,
			ErrorMessage = @ErrorMessage, LeadStatus = @LeadStatus, UpdatedOn = GETDATE() , 
			IncomingXMLListing = @IncomingXML , OutgoingXMLListing = @OutGoingXML
		    WHERE LeadId = @LeadId AND CBDId = @CBDId
			
	
	IF @@ROWCOUNT =0
		BEGIN
			INSERT INTO CRM_SkodaLeadhotStatus
			(
				LeadId, CBDId, TokenId, ErrorCode, ErrorMessage, LeadStatus ,IncomingXMLListing, OutgoingXMLListing
			)
			VALUES
			(
				@LeadId, @CBDId, @TokenId, @ErrorCode, @ErrorMessage, @LeadStatus,@IncomingXML, @OutGoingXML
			)
			SET @Id = SCOPE_IDENTITY()
		END
END
















