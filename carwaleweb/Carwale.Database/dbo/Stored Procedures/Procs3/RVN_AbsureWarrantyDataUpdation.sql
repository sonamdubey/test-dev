IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_AbsureWarrantyDataUpdation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_AbsureWarrantyDataUpdation]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(8th Jan 2015)
-- Description	:	Update data in tables for AbSure warranty packages
--					after its approval from back office
-- =============================================
CREATE PROCEDURE [dbo].[RVN_AbsureWarrantyDataUpdation]
	
	@RVNDealerPackageFeatureId	INT,
	@DealerId	INT,
	@NoOfCars	INT,
	@DiscountPercentage	FLOAT,
	@CreditedBy		INT,
	@CreditAmount	NUMERIC(18,2)

AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @CreditTransactionId	INT
	DECLARE	@FinalClosingBalance	NUMERIC(18,2)
	
	DECLARE @MyTableVar table( FinalClosingAmount NUMERIC(18,2))

    --First Make new entry in AbSure_Trans_Credits for each approval
	INSERT INTO AbSure_Trans_Credits 
						(DealerId,CreditAmount,CreditDate,CreditedBy,DiscountPer,NoOfCars,ActivationId)
				VALUES	
						(@DealerId,@CreditAmount,GETDATE(),@CreditedBy,@DiscountPercentage,@NoOfCars,@RVNDealerPackageFeatureId)

	SET @CreditTransactionId = SCOPE_IDENTITY()
	
	--If data inserted successfully then update in AbSure_Trans_ClosingBalance table
	IF @CreditTransactionId > 0
		BEGIN
			--If Record already exists for that dealer update it
			UPDATE AbSure_Trans_ClosingBalance
			SET	ClosingBalance = ISNULL(ClosingBalance,0) + @CreditAmount,
				DiscountPer = @DiscountPercentage,
				LastUpdatedBy	=	@CreditedBy,
				LastUpdatedDate = GETDATE()
				--Get the last updated balance
				OUTPUT INSERTED.ClosingBalance INTO @MyTableVar
			WHERE
				DealerId = @DealerId
			
			SELECT @FinalClosingBalance = FinalClosingAmount FROM @MyTableVar
			IF @@ROWCOUNT = 0
				BEGIN
					--If fresh entry
					INSERT INTO AbSure_Trans_ClosingBalance 
							(DealerId,ClosingBalance,DiscountPer,LastUpdatedDate,LastUpdatedBy)
					VALUES
							(@DealerId,@CreditAmount,@DiscountPercentage,GETDATE(),@CreditedBy)
					SET @FinalClosingBalance = @CreditAmount
				END
		
			--Log the transaction
			INSERT INTO AbSure_Trans_Logs 
				(TransactionType,TransactionId,TransactionAmount,ClosingAmount,LogDate,LoggedBy)
			VALUES
				(1,@CreditTransactionId,@CreditAmount,@FinalClosingBalance,GETDATE(),@CreditedBy) --1 for credit
				
		END
END

