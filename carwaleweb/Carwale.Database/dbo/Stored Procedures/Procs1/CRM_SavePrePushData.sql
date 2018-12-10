IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SavePrePushData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SavePrePushData]
GO

	


CREATE PROCEDURE [dbo].[CRM_SavePrePushData]

	@Id					Numeric,
	@CustomerName		VarChar(250),
	@Mobile				VarChar(100),
	@EMail				VarChar(100),
	@State				VarChar(50),
	@City				VarChar(50),
	@CarName			VarChar(100),
	@LeadId				Numeric,
	@TokenId			VarChar(50),
	@Status				VarChar(150),
	@StartDate			DateTime,
	@EndDate			DateTime,
	@IsMailSent			Bit,
	@ErrorCode			VarChar(20),
	@Result				VarChar(50),
	@IncomingXML		VarChar(MAX) = null,
	@OutGoingXML		VarChar(MAX) = null,
	@NewId				Numeric OutPut	
				
 AS
	
BEGIN
	SET @NewId = -1
	IF @Id = -1

		BEGIN

			INSERT INTO CRM_PrePushData
			(
				CustomerName, Mobile, EMail, State, City, CarName, LeadId, StartDate, IsMailSent ,IncomingXMLListing, OutgoingXMLListing
			)
			VALUES
			(
				@CustomerName, @Mobile, @EMail, @State, @City, @CarName, @LeadId, @StartDate, @IsMailSent ,@IncomingXML, @OutGoingXML
			)
			
			SET @NewId = SCOPE_IDENTITY()

		END

	ELSE
		
		BEGIN 
			UPDATE CRM_PrePushData SET EndDate = @EndDate, TokenId = @TokenId, Status = @Status,
					IsMailSent = @IsMailSent, ErrorCode = @ErrorCode, Result = @Result , IncomingXMLListing = @IncomingXML , OutgoingXMLListing = @OutGoingXML
			WHERE Id = @Id
				
			SET @NewId = @Id
		END
END















