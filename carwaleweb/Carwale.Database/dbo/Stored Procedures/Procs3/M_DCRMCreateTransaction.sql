IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_DCRMCreateTransaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_DCRMCreateTransaction]
GO

	

-- =============================================
-- Author	:	Sachin Bharti(12th Dec 2014)
-- Description	:	Create transaction for pitching products after
--					their convertion or making them 90 percent
-- Modifier	:	Sachin Bharti(28th Jan 2015)
-- Purpose	:	Make @ServiceTAX,@IsTDSGiven,@TDSAmount,@PANNumber,@TANNumber paramters nullabl
-- Updated by : vinay kumar prajapati 28th desc 2015  @Source=1 for web application , 2-Api,3-mobileSite
-- =============================================
CREATE PROCEDURE [dbo].[M_DCRMCreateTransaction] --'12950,',100,0,114,14,0,0,114,NULL,NULL,3,-1
	
	@SalesDealerIds			VARCHAR(50),
	@TotalClosingAmount		NUMERIC(18,2),
	@DiscountAmount			NUMERIC(18,2),
	@ProductAmount			NUMERIC(18,2),
	@ServiceTAX				FLOAT = NULL,
	@IsTDSGiven				BIT = NULL,
	@TDSAmount				FLOAT = NULL,
	@FinalAmount			NUMERIC(18,2),
	@PANNumber				VARCHAR(10) = NULL,
	@TANNumber				VARCHAR(10) = NULL,
	@UpdatedBy				INT,
	@Source                 SMALLINT =1,
	@TransactionId			INT OUTPUT
AS
BEGIN
	
	
	DECLARE @TempTable TABLE(RowId INT IDENTITY(1,1), SalesDealerId INT)
	DECLARE @RowCount	INT
	DECLARE	@TotalSalesId	INT
	DECLARE	@SalesId	INT
	SET @TransactionId = -1
	--Store all sales dealer id into temporary table
	INSERT INTO @TempTable SELECT *FROM SplitText(@SalesDealerIds,',')
	SET @TotalSalesId = @@ROWCOUNT

	--If we have salesId then create transaction
	IF @TotalSalesId > 0 
	BEGIN
		INSERT INTO 
			DCRM_PaymentTransaction 
				( TotalClosingAmount, DiscountAmount,ProductAmount,ServiceTax,IsTDSGiven,TDSAmount,FinalAmount,PANNumber,TANNumber,CreatedBy,CreatedOn,Source)
			VALUES 
				(@TotalClosingAmount,@DiscountAmount,@ProductAmount,@ServiceTAX,@IsTDSGiven,@TDSAmount,@FinalAmount,@PANNumber,@TANNumber,@UpdatedBy,GETDATE(),@Source)
		
		IF @@ROWCOUNT = 1
		BEGIN
			SET @TransactionId = SCOPE_IDENTITY()
			
			SET @RowCount = 1
			
			--Start looping to get all the salesId
			WHILE(@RowCount <= @TotalSalesId)
			BEGIN
				--Get salesId from temp table
				SELECT  @SalesId = SalesDealerId FROM @TempTable WHERE RowId = @RowCount
				
				--Now update DCRM_SalesDealer table with Transaction Id	
				UPDATE DCRM_SalesDealer 
					SET
						TransactionId = @TransactionId,UpdatedOn = GETDATE(),UpdatedBy = @UpdatedBy
					WHERE
						Id = @SalesId

				SET @RowCount = @RowCount + 1
			END
		END
	END
END



