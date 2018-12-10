IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_Transaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_Transaction]
GO

	
-- =============================================
-- Author		:	Tejashree Patil on 9 Jan 2015
-- Description	:	Recorded debit warranry amount related transaction 
-- Modifier 1   :	Ruchira Patil on 20th Mar 2015 (To update policy no)
-- Modifier 2   :   Chetan Navin (Commented code related to updation of absure_cardetails table)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_Transaction]
		@DealerId						INT,
		@AbSure_CarId					INT,
		@DebitAmount					DECIMAL(18,2),
		@ServiceTaxValue				DECIMAL(18,2),
		@UserId							INT,
		@GoldSalesCost					DECIMAL(18,2), 
		@SilverSalesCost				DECIMAL(18,2)
AS
BEGIN

	DECLARE @DebitedTranactionId INT,
			@FinalClosingBalance DECIMAL(18,2) = 0,
			@DiscountValue	DECIMAL(18,2) = 0,
			@FinalDebitedAmount	DECIMAL(18,2) = 0,
			@DiscountPercentage DECIMAL(18,2) = 0,
			@PolicyNo	VARCHAR(50) 
			
	DECLARE @ClosingBalance TABLE( ClosingBalance DECIMAL(18,2))

	--Get Discount value and calculate ServiceTaxValue, FinalAmount
	SELECT	@DiscountPercentage = ISNULL(CB.DiscountPer,0)
	FROM	AbSure_Trans_ClosingBalance CB WITH(NOLOCK)
	WHERE	CB.DealerId = @DealerId

	SET		@DiscountValue		= (@DebitAmount * @DiscountPercentage /100.000)
	SET 	@ServiceTaxValue	= (((@DebitAmount - @DiscountValue) * @ServiceTaxValue) /100.000)
	SET		@FinalDebitedAmount = (@DebitAmount - @DiscountValue + @ServiceTaxValue)

	--Record debited value
	INSERT INTO AbSure_Trans_Debits
				(DealerId,CarId,DebitedAmount,DiscountValue,ServiceTaxValue,FinalDebitedAmount,DebitDate,DebitedBy, GoldSalesCost, SilverSalesCost)
		 VALUES 
				(@DealerId, @AbSure_CarId, @DebitAmount, @DiscountValue, @ServiceTaxValue, @FinalDebitedAmount, GETDATE(), @UserId, @GoldSalesCost, @SilverSalesCost)

	SET		@DebitedTranactionId = SCOPE_IDENTITY()

	--Update ClosingBalance
	IF NOT EXISTS( SELECT DealerId FROM AbSure_Trans_ClosingBalance WITH(NOLOCK) WHERE DealerId = @DealerId)
	BEGIN		
		INSERT INTO AbSure_Trans_ClosingBalance 
				(DealerId,ClosingBalance,DiscountPer,LastUpdatedDate,LastUpdatedBy)
		VALUES
				(@DealerId,0,ISNULL(@DiscountPercentage,0),GETDATE(),@UserId)
	END

	UPDATE	AbSure_Trans_ClosingBalance
	SET		ClosingBalance	= ISNULL(ClosingBalance,0) - ISNULL(@FinalDebitedAmount,0),
			DiscountPer		= @DiscountPercentage,
			LastUpdatedBy	= @UserId,
			LastUpdatedDate = GETDATE()
			OUTPUT INSERTED.ClosingBalance INTO @ClosingBalance
	WHERE	DealerId = @DealerId

	SELECT	@FinalClosingBalance = ISNULL(ClosingBalance ,0)
	FROM	@ClosingBalance

	--Log each transaction
	INSERT INTO AbSure_Trans_Logs
			   (TransactionType, TransactionId, TransactionAmount, ClosingAmount, LogDate, LoggedBy)
		 VALUES
			   (2, @DebitedTranactionId, @FinalDebitedAmount, @FinalClosingBalance, GETDATE(), @UserId)	

		/*Commented By : Chetan Navin on 7th Mar 2016
		--	After successfull transaction of specific car warranty then make it soldOut(IsSoldOut =true) 
		--UPDATE	AbSure_CarDetails 
		--SET		IsSoldOut=1 
		--WHERE	Id=@AbSure_CarId	
		
		
		/*start - Modified by   :	Ruchira Patil on 20th Mar 2015 (To update policy no)*/
		SELECT @PolicyNo = [dbo].[Absure_GenerateWarrantyPolicyNo](@AbSure_CarId,1) -- 1 is for dealer
		
		UPDATE AbSure_CarDetails SET PolicyNo = @PolicyNo WHERE Id = @AbSure_CarId
		/*end*/
		*/	
END
-------------------------------------------------------------------------------------------------------------
