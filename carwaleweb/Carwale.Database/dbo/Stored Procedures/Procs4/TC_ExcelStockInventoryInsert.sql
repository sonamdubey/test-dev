IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ExcelStockInventoryInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ExcelStockInventoryInsert]
GO

	
-- =============================================
-- Author:		Tejashree Patil
-- Create date: 12 Sept,2013
-- Description:	Insert stock inventory details from imported excel.
-- Modified By: Tejashree Patil on 30 Sept 2013, Updated other deatils in TC_StockInventory table.
-- =============================================
CREATE PROCEDURE [dbo].[TC_ExcelStockInventoryInsert]
@BranchId BIGINT,
@UserId BIGINT,
@IsSpecialUser BIT,
@ExcelSheetId BIGINT,
@TC_ExcelStockInventoryId BIGINT,
@ModelCode  VARCHAR(100),
@ColourCode   VARCHAR(100),
@PrCodes   VARCHAR(100),
@Region   VARCHAR(100),
@ChassisNumber   VARCHAR(100),
@DealerCompanyName   VARCHAR(100),
@SellingDealer   VARCHAR(100),
@DealerLocation   VARCHAR(100),
@PaymentDealerInvoiceDate    VARCHAR(100),
@ModelYear   VARCHAR(100),
@CheckpointDate   VARCHAR(100),
@WholesaleDate   VARCHAR(100),
@IsValid  BIT,
@IsExist BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @IsExist = 0
	--New record
	IF(@TC_ExcelStockInventoryId IS NULL)
	BEGIN
		--Insert new records if chassis number do not exists
		IF NOT EXISTS(	SELECT      ChassisNumber
						FROM        TC_ExcelStockInventory
						WHERE       ChassisNumber = @ChassisNumber
									AND IsDeleted=0
						UNION
						SELECT      ChassisNumber
						FROM        TC_NewCarBooking
						WHERE       ChassisNumber = @ChassisNumber

						UNION
						SELECT		ChassisNumber
						FROM		TC_StockInventory
						WHERE		ChassisNumber = @ChassisNumber
					 )
        BEGIN
			INSERT INTO TC_ExcelStockInventory
						(ModelCode,ColourCode,PrCodes,Region,ChassisNumber,DealerCompanyName,SellingDealerCode,DealerLocation,PaymentDealerInvoiceDate,
						 ModelYear, CheckpointDate,WholesaleDate,IsValid,IsDeleted,EntryDate,CreatedBy,BranchId,TC_ExcelSheetStockInventoryId, IsSpecialUser)
			VALUES		(@ModelCode, @ColourCode, @PrCodes, @Region, @ChassisNumber , @DealerCompanyName, @SellingDealer, @DealerLocation, @PaymentDealerInvoiceDate, 
						 @ModelYear, @CheckpointDate, @WholesaleDate, @IsValid, 0, GETDATE(), @UserId, @BranchId, @ExcelSheetId, @IsSpecialUser)
						 
			SET		@TC_ExcelStockInventoryId = SCOPE_IDENTITY()
		END
		ELSE
		BEGIN
			SET @IsExist =1
		END
	END
	ELSE --Update old record inValid records
	BEGIN
		UPDATE	TC_ExcelStockInventory
		SET		ModelCode=@ModelCode,
				ColourCode=@ColourCode,
				ChassisNumber=@ChassisNumber,
				SellingDealerCode=@SellingDealer,
				IsValid=@IsValid,
				ModifiedDate=GETDATE(),
				ModifiedBy=@UserId,
				BranchId=@BranchId
		WHERE	TC_ExcelStockInventoryId=@TC_ExcelStockInventoryId
				--AND BranchId=@BranchId
	END
	
	--Insert valid records in TC_StockInventory where all valid chassis number exists.
	IF(@IsValid = 1)
	BEGIN
		IF NOT EXISTS(	SELECT      ChassisNumber
						FROM        TC_StockInventory
						WHERE       ChassisNumber = @ChassisNumber

						UNION
						SELECT      ChassisNumber
						FROM        TC_NewCarBooking
						WHERE       ChassisNumber = @ChassisNumber
						
						UNION
						SELECT		ChassisNumber
						FROM		TC_StockInventory
						WHERE		ChassisNumber = @ChassisNumber
					 )
        BEGIN
			SELECT	@DealerCompanyName=Organization
			FROM	Dealers 
			WHERE	DealerCode=@SellingDealer
					AND IsDealerActive=1
					AND IsTCDealer=1
			
			SELECT	@DealerLocation=DealerLocation,@PaymentDealerInvoiceDate=PaymentDealerInvoiceDate,
					@ModelYear=ModelYear,@CheckpointDate=CheckpointDate,@WholesaleDate=WholesaleDate
			FROM	TC_ExcelStockInventory
			WHERE	TC_ExcelStockInventoryId=@TC_ExcelStockInventoryId

			--pass @TC_ExcelStockInventoryId for TC_ExcelStockInventoryId and @ExcelSheetId for TC_ExcelSheetStockInventoryId
			INSERT INTO  TC_StockInventory
						(ModelCode,ColourCode,PrCodes,Region,ChassisNumber,DealerCompanyName,SellingDealerCode,DealerLocation,PaymentDealerInvoiceDate,
						 ModelYear, CheckpointDate,WholesaleDate,EntryDate,TC_UserId,BranchId,TC_ExcelStockInventoryId,TC_ExcelSheetStockInventoryId,IsSpecialUser)
			VALUES		(@ModelCode, @ColourCode, @PrCodes, @Region, @ChassisNumber , @DealerCompanyName, @SellingDealer, @DealerLocation, @PaymentDealerInvoiceDate, 
						 @ModelYear, @CheckpointDate, @WholesaleDate, GETDATE(), @UserId, @BranchId, @TC_ExcelStockInventoryId,@ExcelSheetId,@IsSpecialUser)	
			
			----Update IsDeleted=1 for all valid records in TC_ExcelStockInventory.			 
			--UPDATE	TC_ExcelStockInventory
			--SET		IsDeleted=1
			--WHERE	TC_ExcelStockInventoryId=@TC_ExcelStockInventoryId
			--		AND BranchId=@BranchId
		END	 
	END
END


