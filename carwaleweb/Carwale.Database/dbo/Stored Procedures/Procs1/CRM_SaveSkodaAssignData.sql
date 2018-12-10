IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveSkodaAssignData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveSkodaAssignData]
GO

	


CREATE PROCEDURE [dbo].[CRM_SaveSkodaAssignData]

	@Id					Numeric,
	@pushId				Numeric,
	@leadId				Numeric,
	@dealerId			Numeric,
	@tokenId			VarChar(50),
	@pushStatus			VarChar(150),
	@Comments			VarChar(150),
	@StartDate			DateTime,
	@EndDate			DateTime,
	@Status				Bit,
	@IsCityChanged		Bit,
	@ErrorCode			VarChar(20),
	@DMSId				VarChar(15),
	@IncomingXML		VarChar(MAX) = null,
	@OutGoingXML		VarChar(MAX) = null,
	@NewId				Numeric OutPut	
				
 AS
	
BEGIN
	SET @NewId = -1
	IF @Id = -1

		BEGIN

			INSERT INTO CRM_SkodaDealerAssignment
			(
				LeadId, DealerId, StartDate, PushId, TokenId,IncomingXMLListing, OutgoingXMLListing
			)
			VALUES
			(
				@LeadId, @DealerId, @StartDate, @PushId, @TokenId ,@IncomingXML, @OutGoingXML
			)
			
			SET @NewId = SCOPE_IDENTITY()

		END

	ELSE
		
		BEGIN 
			UPDATE CRM_SkodaDealerAssignment SET EndDate = @EndDate, PushStatus = @PushStatus,
					Comments = @Comments, Status = @Status, TokenId = @TokenId,
					IsCityChanged = @IsCityChanged, ErrorCode = @ErrorCode, DMSId = @DMSId  , IncomingXMLListing = @IncomingXML, OutgoingXMLListing = @OutGoingXML
			WHERE Id = @Id
				
			SET @NewId = @Id
		END
END















