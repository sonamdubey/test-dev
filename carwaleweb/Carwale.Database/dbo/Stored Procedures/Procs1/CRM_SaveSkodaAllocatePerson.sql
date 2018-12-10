IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveSkodaAllocatePerson]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveSkodaAllocatePerson]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 1 April 2013
-- Description : Saves and Updates skoda allocate sales person data 
-- =============================================
CREATE PROCEDURE [dbo].[CRM_SaveSkodaAllocatePerson]
    @CBDId			BIGINT,
	@TokenId		VARCHAR(50),
	@LeadId			BIGINT,
	@ErrorCode		VARCHAR(50),
	@ErrorMessage   VARCHAR (500),
	@CreatedBy INT,
	@IncomingXMLListing VARCHAR(MAX) = null,
	@OutgoingXMLListing VARCHAR(MAX)= null,
	@LeadStatus VARCHAR (50)
	
AS
	DECLARE @Id AS Numeric
	
BEGIN

	SET NOCOUNT ON;
	
	UPDATE CRM_SkodaAllocatePerson SET TokenId = @TokenId, ErrorCode = @ErrorCode,
			ErrorMessage = @ErrorMessage, LeadStatus = @LeadStatus, UpdatedOn = GETDATE() , 
			IncomingXMLListing = @IncomingXMLListing , OutgoingXMLListing = @OutgoingXMLListing
		    WHERE LeadId = @LeadId AND CBDId = @CBDId
	
    IF @@ROWCOUNT =0
		BEGIN
			INSERT INTO CRM_SkodaAllocatePerson
			(
			   CBDId,TokenId,LeadId,ErrorCode,ErrorMessage,LeadStatus,CreatedBy,IncomingXMLListing,OutgoingXMLListing
			)
			VALUES
			(
			  @CBDId,@TokenId,@LeadId,@ErrorCode,@ErrorMessage,@LeadStatus,@CreatedBy,@IncomingXMLListing,@OutgoingXMLListing
			)
			SET @Id = SCOPE_IDENTITY()	
		END
END

