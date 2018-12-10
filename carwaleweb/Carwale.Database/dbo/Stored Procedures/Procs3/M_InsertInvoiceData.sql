IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_InsertInvoiceData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_InsertInvoiceData]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(5th Feb 2015)
-- Description	:	Insert invoice date
-- Modifier	:	Sachin Bharti(13th Aril 2015)
-- Purpose	:	Add status to invoice generation . At first invoice is added in pending
--				status then either it will be get approved or rejected .
-- Modifier	:	Sachin Bharti(29th May 2015)
-- Purpose	:	Added comments column and update the same when approving and rejecting the generated invoice
-- Modifier :	Amit Yadav(22nd July 2015)
-- Purpose	:	Added a column and a parameter to store text to be printed.
-- Modifier	:	Sachin Bharti(23rd July 2015)
-- Purpose	:	Added @InvoiceDate parameter to get invoice date in when using rejected invoice
-- Execute [dbo].[M_InsertInvoiceData] null,null,null,null,null,null,'15-16',3,null,null,2,1037
-- Modifier	:	Amit Yadav(2nd Nov 2015)
-- Purpose	:	Added parameter @ShowSummary.To print contract summary on Invoice.
-- Modifier : Vaibhav K 22-Dec-2015
-- Added UpdatedOn to be updated with the current datetime in case of Invoice approval
-- Updated By Vinay Kumar prajapati : 29th desc 2015 , Log cleanIndiaMission(@IsCleanMission) cess  Data  
-- Modifier : Vaibhav K 4-1-2015
-- Else condition added for invoice number series for > 4 (Nos. 1000)
-- Modified By:Komal Manjare(13-January-2016)-to include sourceId
--@SourceId=1(desktop)
-- @SourceId=2(mobileApp)
-- Modified by : Amit Yadav 22nd Jan 2016 
-- Purpose: Added a parameter @SalesDealerIds to a save multiple SalesId for multiple products in one transaction.
-- Modified By : Komal Manjare(02-june-2016) Add New Parameter @IsKrishiKalyanTax update during approving invoice.
-- EXEC M_InsertInvoiceData 11263,'A',23715,'52,52','1000,22715',52,'',0,1,12,1,52,'','10,155', 1,'','', 1, 1,1 ,'13240,13241'
-- Modified By : Sunil M. Yadav On 02 Auguest 2016 , Check for old status and approval status before approving invoice to avoid overriding of data.
-- =============================================
CREATE PROCEDURE [dbo].[M_InsertInvoiceData]
	
	@TransactionId	INT = NULL,
	@InvoiceName	VARCHAR(250) = NULL,
	@InvoiceAmount	NUMERIC(18,2)= NULL,
	@ProductIds		VARCHAR(50) = NULL,
	@ProductInvoiceAmounts	VARCHAR(250) = NULL,
	@InvoiceSeriesPackageId	INT = NULL,
	@FinanceYearFormat	VARCHAR(10) = NULL,
	@UpdatedBy		INT = NULL,
	@IsLogged		BIT = NULL OUTPUT ,
	@ServiceTax		FLOAT = NULL,
	@Status			SMALLINT = NULL,
	@InvoiceId		INT = NULL,
	@Comments		VARCHAR(250) = NULL,
	@ProductQuantities	VARCHAR(100) = NULL,
	@RejectedInvNumId	INT = NULL, --invoice id which invoice number we will use for new
	@TextToBePrinted VARCHAR(250) = NULL,
	@InvoiceDate	DATETIME = NULL,
	@ShowSummary BIT = NULL, --To print the contract summary.
	@IsCleanMission BIT = 0 ,
	@SourceId INT=1,-- if sourceId=1 then desktop and if 2 then mobileapp
	@SalesDealerIds VARCHAR(50)=NULL--Save salesDealerIds of those products
	,@IsKrishiKalyanTax BIT=0 --for krishi kalyan tax
AS
BEGIN

	DECLARE @InvoiceSeriesId SMALLINT, @InvoiceSeries	VARCHAR(100) = NULL,@InvoiceNumbers	VARCHAR(100) = NULL
	DECLARE @OldInvoiceStatus INT = -1; -- Sunil M. Yadav On 02 Auguest 2016
	
	--set zero by default
	SET @IsLogged = 0

	--product is first added in pending status that is why there
	--is no invoice number generated in this first step
	IF @Status = 1
		BEGIN
			--to store packageIds for which invoice is generating
			DECLARE @TempProductTable TABLE(RowId INT IDENTITY(1,1), ProductId INT)

			--to store package invoice amount 
			DECLARE @TempProductAmntTable TABLE(RowId INT IDENTITY(1,1), Amount VARCHAR(10))

			--to store salesDealerId 
			DECLARE @TempSalesDealerId TABLE(RowId INT IDENTITY(1,1), SalesDealerId INT)

			--to store package quantities for which invoice is generating
			--that is number of days , leads or RSA/Warranty quantity
			DECLARE @TempPrdQuantitiesTable TABLE(RowId INT IDENTITY(1,1), Quantity INT)
	
			DECLARE @ProductId	INT ,@ProductCount INT, @AmountCount INT,@QuantityCount INT,@RowCount INT,@SalesDealerId INT,@SalesIdCount INT

			DECLARE	@ProductInvoiceAmount NUMERIC(18,2),@ProductQuantity SMALLINT, @PostTaxInvoiceAmount NUMERIC(18,2)
			
			--inquiry point id which series number is going to follow 
			--for invoice generation
			DECLARE @InquiryPointId INT 

			--Insert total products into temp table for which invoice is generating
			INSERT INTO @TempProductTable SELECT *FROM SplitText(@ProductIds,',')
			SET @ProductCount = @@ROWCOUNT

			--Insert the salesDealerId into M_GeneratedInvoiceDetails for each product
			INSERT INTO @TempSalesDealerId SELECT *FROM SplitText(@SalesDealerIds,',')
			SET @SalesIdCount = @@ROWCOUNT

			--Insert each product invoice amount into temp table 
			INSERT INTO @TempProductAmntTable SELECT *FROM fnSplitCSVToChar(@ProductInvoiceAmounts)
			SET @AmountCount = @@ROWCOUNT

			--Insert total product quantities into temp table for which invoice is generating
			INSERT INTO @TempPrdQuantitiesTable SELECT *FROM SplitText(@ProductQuantities,',')
			SET @QuantityCount = @@ROWCOUNT

			--when added procuct counts and getting invoice amount for each product is equal
			IF @ProductCount = @AmountCount AND @QuantityCount = @AmountCount AND @ProductCount = @QuantityCount --AND @SalesIdCount = @ProductCount
				BEGIN
	
					--get inquiry point for which invoice series will follow
					SELECT @InquiryPointId = PK.InqPtCategoryId FROM Packages PK(NOLOCK) WHERE PK.Id = @InvoiceSeriesPackageId		

					--get invoice series for that product only
					SELECT 
						@InvoiceSeries = MI.InvoiceNoSeriesPattern,
						@InvoiceSeriesId = MI.Id
					FROM 
						M_InvoiceNumberSeries MI(NOLOCK)
					WHERE	
						MI.InquiryPointId = @InquiryPointId

					--calculate post tax product invoice amount
					SET  @PostTaxInvoiceAmount = @InvoiceAmount + ((@InvoiceAmount * @ServiceTax)/100)

					--Now insert data into M_GeneratedInvoice table
					INSERT INTO M_GeneratedInvoice 
								(	
									TransactionId,
									InvoiceAmount,
									EntryDate,
									GeneratedBy,
									InvoiceName,
									InvoiceSeries,
									PostTaxInvoiceAmount,
									Status,
									InvoiceSeriesId,
									SourceId
								)
						VALUES
								(
									@TransactionId,
									@InvoiceAmount,
									GETDATE(),
									@UpdatedBy,
									@InvoiceName,
									@InvoiceSeries,
									@PostTaxInvoiceAmount,
									@Status,
									@InvoiceSeriesId,
									@SourceId
								)
					SET @InvoiceId = SCOPE_IDENTITY()
					SET @RowCount = 1
					--Now insert invoice date into M_GeneratedInvoice table
					--Start looping to get all the salesId
					WHILE(@RowCount <= @ProductCount)
						BEGIN
							--Get invoice productId from temp table
							SELECT  @ProductId = ProductId FROM @TempProductTable WHERE RowId = @RowCount
							
							--Get saleDealerId from temp table 
							SELECT @SalesDealerId = SalesDealerId FROM @TempSalesDealerId WHERE RowId = @RowCount

							--Get product invoice amount from temp table
							SELECT  @ProductInvoiceAmount = Amount FROM @TempProductAmntTable WHERE RowId = @RowCount

							--Get product invoice quantity from temp table
							SELECT  @ProductQuantity = Quantity FROM @TempPrdQuantitiesTable WHERE RowId = @RowCount
							
							INSERT INTO M_GeneratedInvoiceDetail
										(
											InvoiceId,
											PackageId,
											ProductInvoiceAmount,
											Quantity,
											SalesDealerId
										)
										VALUES
										(
											@InvoiceId,
											@ProductId,
											@ProductInvoiceAmount,
											@ProductQuantity,
											@SalesDealerId
										)
							IF @@ROWCOUNT > 0 
								SET @IsLogged = 1
							SET @RowCount = @RowCount + 1
						END
				END
		END
	--Approving pending invoices . Invoice number is generating.
	ELSE 
	BEGIN

	SELECT @OldInvoiceStatus = Status FROM M_GeneratedInvoice WITH(NOLOCK) WHERE Id = @InvoiceId

	IF (@Status = 2 AND @OldInvoiceStatus <> 2)
		BEGIN
				
			DECLARE @TempCurrentInvoiceNumber TABLE( CurrentInvoiceNumber INT)
			--GroupId for each invoice number series
			DECLARE @GroupId INT
	
			DECLARE @CurrentNumber INT, @InvoiceSeriesNumber VARCHAR(4), @InvoiceSeriesNumberLen INT
			
			--store generating invoice number
			DECLARE @InvoiceNumber VARCHAR(100)
		
			IF @RejectedInvNumId IS NULL OR @RejectedInvNumId = -1
				BEGIN
					SELECT @InvoiceSeriesId = InvoiceSeriesId FROM M_GeneratedInvoice  WHERE Id = @InvoiceId
				
					--get invoice series for that product only
					SELECT 
						@InvoiceSeries = MI.InvoiceNoSeriesPattern, @GroupId = GroupId
					FROM 
						M_InvoiceNumberSeries MI(NOLOCK)
					WHERE	
						MI.Id = @InvoiceSeriesId

					--update current invoice number in invoice series table
					UPDATE M_InvoiceNumberSeries
						SET	
							CurrentInvoiceSeriesNumber = ISNULL((	SELECT SUM(CurrentInvoiceNumber) FROM M_InvoiceNumberSeries WHERE 
																	GroupId = @GroupId),0) + 1,CurrentInvoiceNumber = ISNULL(CurrentInvoiceNumber, 0) + 1
					--Get the last updated number
					OUTPUT INSERTED.CurrentInvoiceSeriesNumber INTO @TempCurrentInvoiceNumber
					WHERE
						Id = @InvoiceSeriesId

					--read current invoice number from temp table
					SELECT 
						@CurrentNumber = MI.CurrentInvoiceNumber,@InvoiceSeriesNumberLen = LEN(MI.CurrentInvoiceNumber)
					FROM 
						@TempCurrentInvoiceNumber MI

					--format invoice number into four digits
					IF @InvoiceSeriesNumberLen = 1
						SET @InvoiceSeriesNumber = '000'+CONVERT(VARCHAR(4),@CurrentNumber)
					ELSE IF @InvoiceSeriesNumberLen = 2
						SET @InvoiceSeriesNumber = '00'+CONVERT(VARCHAR(4),@CurrentNumber)
					ELSE IF @InvoiceSeriesNumberLen = 3
						SET @InvoiceSeriesNumber = '0'+CONVERT(VARCHAR(4),@CurrentNumber)
					ELSE
						SET @InvoiceSeriesNumber = CONVERT(VARCHAR, @CurrentNumber)	--Vaibhav K 4-1-15 for invoice nos after 1000

					--generate invoice number 
					SET @InvoiceNumber = @InvoiceSeries+'-'+@InvoiceSeriesNumber+'/'+@FinanceYearFormat

					PRINT @InvoiceNumber
					PRINT @InvoiceSeriesNumber
					
					--check invoice date
					IF @InvoiceDate IS NULL
						SET @InvoiceDate = GETDATE()

					--update approved invoice
					UPDATE M_GeneratedInvoice SET 
						InvoiceNumber = @InvoiceNumber,
						InvoiceSeriesNo = @InvoiceSeriesNumber,
						Status = @Status,
						InvoiceDate = @InvoiceDate,
						UpdatedBy = @UpdatedBy,
						UpdatedOn = GETDATE(), --Vaibhav K 22-12-2015 UpdatedOn with current date
						Comments = @Comments,
						TextToBePrinted = @TextToBePrinted,
						ShowContractSummary = @ShowSummary,
						IsCleanMissionManual=@IsCleanMission --- Vinay kumar prajapati 29thn desc 2015 
						,IsKrishiKalyanTaxManual=@IsKrishiKalyanTax  --Komal Manjare on 02-06-2016
					WHERE
						Id = @InvoiceId

					IF @@ROWCOUNT > 0
						BEGIN
							PRINT 'Islogged'
							SET @IsLogged = 1
						END
				END
			ELSE IF @RejectedInvNumId IS NOT NULL AND @RejectedInvNumId > 0
				BEGIN
					--read rejected invoice details
					SELECT 
						@InvoiceSeriesNumber = InvoiceSeriesNo,
						@InvoiceNumber = InvoiceNumber,
						@InvoiceDate = InvoiceDate
					FROM 
						M_GeneratedInvoice WITH(NOLOCK)  WHERE Id = @RejectedInvNumId
					IF @@ROWCOUNT = 1
						BEGIN
							--update approved invoice
							UPDATE M_GeneratedInvoice SET 
								InvoiceNumber = @InvoiceNumber,
								InvoiceSeriesNo = @InvoiceSeriesNumber,
								UsedInvoiceId = @RejectedInvNumId,
								Status = @Status,
								InvoiceDate = @InvoiceDate,
								UpdatedOn = GETDATE(),
								UpdatedBy = @UpdatedBy,
								Comments = @Comments,
								TextToBePrinted = @TextToBePrinted,
								ShowContractSummary = @ShowSummary
							WHERE
								Id = @InvoiceId
							
							IF @@ROWCOUNT > 0
								BEGIN
									UPDATE M_GeneratedInvoice SET Status = 5 WHERE ID = @RejectedInvNumId
									SET @IsLogged = 1
								END
						END
					
				ENd
		END
	--Make added invoice rejected
	ELSE IF (@Status = 3 AND @OldInvoiceStatus <> 3)
		BEGIN
			--Change status of invoice status from pending to rejected
			UPDATE	M_GeneratedInvoice 
				SET Status = @Status,
					UpdatedOn = GETDATE(),
					UpdatedBy = @UpdatedBy,
					Comments = @Comments
				WHERE
					Id = @InvoiceId
			IF @@ROWCOUNT > 0
				SET @IsLogged = 1
		END
	--Reject Approved Invoice
	ELSE IF (@Status = 4 AND @OldInvoiceStatus <> 4)
		BEGIN
			SELECT Id FROM M_GeneratedInvoice MGI(NOLOCK) WHERE MGI.Id = @InvoiceId
			--if record exist
			IF @@ROWCOUNT = 1
				BEGIN							
					--update invoice details 
					UPDATE	
						M_GeneratedInvoice	SET	Status =@Status,--approved rejected
											Comments = @Comments,
											UpdatedBy = @UpdatedBy,
											RejectedDate = GETDATE()
											
					WHERE	
						Id = @InvoiceId
					IF @@ROWCOUNT = 1
						BEGIN
							PRINT 'Rejected'
							SET @IsLogged = 1
						END
				END
		END

	END
END


