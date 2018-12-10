IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_InsertAndUpdateSpecialOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_InsertAndUpdateSpecialOffers]
GO

	
CREATE PROCEDURE [dbo].[CW_InsertAndUpdateSpecialOffers]
@CompCategoryId int,
@IsUpdate Bit=0,
@ROI DECIMAL(5,2),
@MinLoanAmount int,
@MaxLoanAmount int,
@IsActive BIT,
@RecordExists bit OUTPUT,
@StartDate DATETIME = NULL,
@EndDate DATETIME = NULL

AS 
--Author:Rakesh Yadav on 1 Aug 2015
--desc: insert and update new car ltv for hdfc
BEGIN
	SET @RecordExists=1
	IF NOT EXISTS(Select Id FROM CW_FinanceSpecialOffers WITH(NOLOCK) WHERE CW_CompanyCategoryId = @CompCategoryId AND MinLoanAmount = @MinLoanAmount AND MaxLoanAmount = @MaxLoanAmount) AND @IsUpdate=0
	BEGIN		
		BEGIN
			INSERT INTO CW_FinanceSpecialOffers(CW_CompanyCategoryId, MinLoanAmount, MaxLoanAmount, ROI, IsCampaignActive, StartDate, EndDate)
			VALUES(@CompCategoryId, @MinLoanAmount, @MaxLoanAmount, @ROI, @IsActive, @StartDate, @EndDate)		
		END		
		 SET @RecordExists=0
		
	END
	ELSE
	BEGIN
		IF @IsUpdate=1 
			BEGIN
			UPDATE CW_FinanceSpecialOffers
			SET CW_CompanyCategoryId = @CompCategoryId,
				IsCampaignActive = @IsActive,
				ROI = @ROI,
				MinLoanAmount = @MinLoanAmount,
				MaxLoanAmount = @MaxLoanAmount,
				StartDate = @StartDate,
				EndDate = @EndDate
			WHERE CW_CompanyCategoryId = @CompCategoryId AND MinLoanAmount = @MinLoanAmount AND MaxLoanAmount = @MaxLoanAmount			
			SET @RecordExists=1
		END
	END
END
