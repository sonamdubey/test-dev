IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_SaveBidderDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_SaveBidderDetails]
GO

	

CREATE PROCEDURE [dbo].[AE_SaveBidderDetails]
(
	@Id AS NUMERIC(18,0),
	@ConsumerId	NUMERIC,
	@BidderType AS SMALLINT,
	@Name AS VARCHAR(50),
	@EMail AS VARCHAR(50),
	@Mobile AS VARCHAR(15),
	@Landline AS VARCHAR(15),
	@OtherContacts AS VARCHAR(50),
	@CityId AS BIGINT,
	@Address AS VARCHAR(250),
	@PanNumber AS VARCHAR(10),
	@ContactPerson AS VARCHAR(50),
	@UpdatedOn AS DATETIME = null,
	@UpdateBy AS NUMERIC(18,0) = null,
	@TokenQuantity AS INT = NULL,
	@TokenRequestId	NUMERIC OUTPUT 
)
AS
BEGIN
	IF @Id <> -1
		BEGIN
			UPDATE AE_BidderDetails SET
			BidderType = @BidderType, Name = @Name, 
			EMail = @EMail, Mobile = @Mobile, Landline = @Landline, OtherContacts = @OtherContacts, 
			CityId = @CityId, Address = @Address, PanNumber = @PanNumber, ContactPerson = @ContactPerson, 
			UpdatedOn = @UpdatedOn , UpdatedBy = @UpdateBy
			WHERE Id = @Id 						
		END
	ELSE
		BEGIN
			IF NOT EXISTS(SELECT Id FROM AE_BidderDetails WHERE ConsumerId = @ConsumerId AND BidderType = @BidderType)
				BEGIN
					INSERT INTO AE_BidderDetails(
						ConsumerId, BidderType, Name, Email, Mobile, Landline, 
						OtherContacts, CityId, Address, PanNumber, ContactPerson, EntryDate)
					VALUES(@ConsumerId, @BidderType, @Name, @EMail, @Mobile, @Landline, @OtherContacts, 
					@CityId, @Address, @PanNumber, @ContactPerson, GETDATE())
					
					DECLARE @BidderId NUMERIC = SCOPE_IDENTITY()
					
					/* Ececute Procedure to save Token Request */
					INSERT INTO AE_TokenRequests(BidderId, NoOfTokens, EntryDate, Status)
					VALUES(@BidderId, @TokenQuantity, GETDATE(), '0')
					
					SET @TokenRequestId = SCOPE_IDENTITY()
				END			
		END
END	

