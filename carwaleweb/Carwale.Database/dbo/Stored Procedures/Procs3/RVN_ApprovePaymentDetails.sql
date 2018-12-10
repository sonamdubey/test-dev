IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_ApprovePaymentDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_ApprovePaymentDetails]
GO

	-- =============================================
--	Author	:	Sachin Bharti(7th Oct 2014)
--	Purpose	:	Approve dealer payments and then update or make new entry
--				in RVN_DealerPackageFeatures for that campaign
--	Modifier:	Sachin Bharti(22nd Dec 2014)
--	Purpose	:	Insert transaction id in RVN_DealerPackageFeatures table
-- =============================================
CREATE PROCEDURE [dbo].[RVN_ApprovePaymentDetails]
	
	@DealerId				INT,
	@TransactionId			INT,

	--old transaction amounts
	@OldTotalClosingAmount	NUMERIC(18,2),
	@OldDiscountAmount		NUMERIC(18,2),
	@OldProductAmount		NUMERIC(18,2),

	--new transaction amounts
	@NewTotalClosingAmount	NUMERIC(18,2),
	@NewDiscountAmount		NUMERIC(18,2),
	@NewProductAmount		NUMERIC(18,2),
	
	@IsTransactionDataChanged	BIT,
	@ApprovedPaymentIDs		VARCHAR(100),
	@Comments				VARCHAR(1000)	=	NULL,
	@UpdatedBy				INT,
	@IsActive				TINYINT,
	@PackageStatus			TINYINT,
	@IsSaved				BIT		OUTPUT

AS
	BEGIN

		SET @IsSaved = 0
		DECLARE @TempTableProduts TABLE(RowId INT IDENTITY(1,1),SalesDealerId INT,PackageId INT,ClosingAmount INT ,PackageQuantity INT,PercentageSlab FLOAT)
		DECLARE @ProductRowCount	INT
		DECLARE	@TotalProducts	INT
		DECLARE	@ProductSalesDealerId	INT
		DECLARE	@PackageId	INT
		DECLARE	@ClosingAmount	INT
		DECLARE	@RSAPackageQuantity	INT
		DECLARE @PercentageSlab	FLOAT
		
		--First check is entry already is done or not for that Transaction Id
		SELECT 
			RVN.DealerPackageFeatureID 
		FROM 
			RVN_DealerPackageFeatures RVN(NOLOCK) 
		WHERE 
			RVN.TransactionId = @TransactionId 
			AND RVN.DealerId = @DealerId
		
		--If no entry exist for that TransactionId
		--Then made new entries in RVN_DealerPackageFeatures table
		IF @@ROWCOUNT = 0
		BEGIN
			--Store all the products into temporary table
			INSERT INTO 
				@TempTableProduts (SalesDealerId , PackageId ,ClosingAmount ,PackageQuantity ,PercentageSlab ) 
			SELECT 
				DSD.Id,DSD.PitchingProduct,DSD.ClosingAmount,DSD.Quantity,DSD.PercentageSlab
				FROM DCRM_SalesDealer DSD WHERE DSD.TransactionId = @TransactionId AND DSD.DealerId = @DealerId
			
			SET @TotalProducts = @@ROWCOUNT

			--Start looping to get all the products for that transaction
			SET @ProductRowCount = 1
			WHILE(@ProductRowCount <= @TotalProducts)
			BEGIN
				
				--Select product,amount,packagequantity from temporary table
				SELECT  
					@ProductSalesDealerId=SalesDealerId,
					@PackageId = PackageId,
					@ClosingAmount = ClosingAmount,
					@RSAPackageQuantity = PackageQuantity,
					@PercentageSlab=PercentageSlab
				FROM 
					@TempTableProduts WHERE RowId = @ProductRowCount
				
				IF @PackageId = 39
					SET @PackageStatus = 1

				INSERT INTO RVN_DealerPackageFeatures
					(
						DealerId,PackageId,EntryDate,ClosingAmount,IsActive,PackageStatus,PackageStatusDate,Comments,
						PackageQuantity,ProductSalesDealerId,TransactionId,PercentageSlab,UpdatedBy,UpdatedOn
					)
				VALUES
					(
						@DealerId,@PackageId,GETDATE(),@ClosingAmount,@IsActive,@PackageStatus,GETDATE(),@Comments,
						@RSAPackageQuantity,@ProductSalesDealerId,@TransactionId,@PercentageSlab,@UpdatedBy,GETDATE()
					)
		
				SET @ProductRowCount = @ProductRowCount + 1
			END
		END

		--Approve payments
		IF	@ApprovedPaymentIDs IS NOT NULL AND @ApprovedPaymentIDs <> '-1'
		BEGIN
			--Declare temp table to store paymentIds that we have to approve
			DECLARE @TempTable TABLE( RowID INT Identity(1,1) ,PaymentId INT )
			DECLARE @RowCount	INT
			DECLARE @TotalRows	INT
			DECLARE @PaymentId	INT
				
			INSERT INTO @TempTable SELECT *FROM SplitText(@ApprovedPaymentIDs,',')
			SET @TotalRows = @@ROWCOUNT

			IF @TotalRows  <> 0
			BEGIN
				--First DisApprove all payments
				IF @TransactionId IS NOT NULL AND @TransactionId <> -1
				BEGIN

					--disApprove all the payments
					UPDATE 
						DCRM_PaymentDetails 
					SET 
						IsApproved = 0 ,
						ApprovedBy = null,
						ApprovedOn=null,
						UpdatedBy = @UpdatedBy,
						UpdatedOn = GETDATE()
					WHERE 
						TransactionId = @TransactionId
				END

				--Approving actual payments
				SET @RowCount = 1
				WHILE @RowCount <= @TotalRows
				BEGIN
					SELECT  @PaymentId = PaymentId FROM  @TempTable WHERE RowID = @RowCount
					UPDATE 
						DCRM_PaymentDetails 
					SET		
						IsApproved = 1 , 
						ApprovedBy = @UpdatedBy , 
						ApprovedOn = GETDATE() 
					WHERE	
						ID = @PaymentId AND ISNULL(IsApproved,0) = 0
					SET @RowCount += 1
					SET @IsSaved = 1
				END
					
			END
		END
		ELSE 
			SET @IsSaved = 0


		IF @IsSaved = 1 AND @IsTransactionDataChanged = 1
		BEGIN
			INSERT INTO DCRM_PaymentTransactionlog (TransactionId,
													OldTotalClosingAmount,NewTotalClosingAmount,
													OldDiscountAmount,NewDiscountAmount,
													OldProductAmount,NewProductAmount,
													OldFinalAmount,NewFinalAmount,
													UpdatedBy,UpdatedOn)
											VALUES(@TransactionId,
													@OldTotalClosingAmount,@NewTotalClosingAmount,
													@OldDiscountAmount,@NewDiscountAmount,
													@OldProductAmount,@NewProductAmount,
													@OldProductAmount,@NewProductAmount,
													@UpdatedBy,GETDATE())


			UPDATE	DCRM_PaymentTransaction SET	
												TotalClosingAmount	= @NewTotalClosingAmount,
												DiscountAmount		= @NewDiscountAmount,
												ProductAmount		= @NewProductAmount,
												FinalAmount	=	@NewProductAmount,
												UpdatedBy	=	@UpdatedBy,
												UpdatedOn	=	Getdate()
					WHERE
						TransactionId = @TransactionId

		END
		
	END
