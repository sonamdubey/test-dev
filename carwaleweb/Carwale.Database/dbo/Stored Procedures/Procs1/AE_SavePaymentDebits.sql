IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_SavePaymentDebits]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_SavePaymentDebits]
GO

	

CREATE PROCEDURE [dbo].[AE_SavePaymentDebits]
(
	@Id AS BIGINT = 0,
	@BidderId AS NUMERIC(18,0) = NULL,
	@AuctionCarId AS NUMERIC(18,0) = NULL,
	@Type AS SMALLINT = NULL,
	@Amount AS NUMERIC(18,2) = NULL,
	@Comment AS VARCHAR(250) = NULL,
	@EntryDate AS DATETIME = NULL,
	@UpdatedOn AS DATETIME = NULL,
	@UpdatedBy	AS BIGINT = NULL,
	@retID AS BIGINT OUT
)
AS
BEGIN
	
	IF(@Id = -1)
		BEGIN
			INSERT INTO 
			AE_PaymentDebits
			(
				BidderId, AuctionCarId, Type, Amount, Comment, EntryDate, UpdatedBy				
			)
			VALUES
			(
				@BidderId, @AuctionCarId, @Type, @Amount, @Comment, @EntryDate, @UpdatedBy								
			)
		
		END
	ELSE
		BEGIN

			UPDATE AE_PaymentDebits
			SET 
				BidderId = @BidderId, AuctionCarId = @AuctionCarId, Type = @Type, 
				Amount = @Amount, Comment = @Comment, UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy				
			WHERE ID = @Id				
		END
		
	SET @retID = SCOPE_IDENTITY()
		
END

