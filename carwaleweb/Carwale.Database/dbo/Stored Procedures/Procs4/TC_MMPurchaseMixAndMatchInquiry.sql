IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMPurchaseMixAndMatchInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMPurchaseMixAndMatchInquiry]
GO

	-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date, 23-12-2013>
-- Description:	<Description, This SP will do work for purchasing MixNMatch Inquiry>
-- Modified By: Nilesh Utture on 3rd Jan, 2014 for deducting and updateing count from all matched stocks
-- Modified By Manish on 03-03-2014 since started using IsDeleted bit in TC_MMCustomerDetails table.
-- =============================================
CREATE PROCEDURE [dbo].[TC_MMPurchaseMixAndMatchInquiry]
	-- Add the parameters for the stored procedure here
	@BranchId INT,
	@UserId INT,
	@CWCustomerId INT,
	@StockId INT,
	@CWInquiryId BIGINT,
	@SellerType TINYINT,
	@CreditPoints TINYINT,
	@CustomerEmail VARCHAR(150) OUTPUT,
	@CustomerMobile VARCHAR(15) OUTPUT,
	@Status TINYINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE --@DeductionPoints INT = @CreditPoints,
			@CustomerName VARCHAR(100),
			@TC_BuyerInquiryId INT,
			@InqStatus INT,
			@LeadDivertedTo INT
	

	IF EXISTS(SELECT DealerId FROM TC_MMDealersPoint WHERE CurrentPoint >= @CreditPoints AND DealerId = @BranchId)
	BEGIN
		-- Insert statements for procedure here
		SELECT @CustomerName = Name,
			   @CustomerEmail = Email,
			   @CustomerMobile = Mobile
		FROM Customers 
		WHERE Id = @CWCustomerId

		EXEC TC_INQBuyerSave @AutoVerified = 1, @BranchId = @BranchId, @StockId = @StockId, @CustomerName = @customerName,
					 @Email = @CustomerEmail, @Mobile = @CustomerMobile,@Location = NULL, @BuyTime = 7, @Comments = NULL,
					 @Eagerness = 1, @NextFollowup = NULL, @InquirySource = 32, @LeadOwnerId =@UserId, @CreatedBy= @UserId, 
					 @MinPrice = NULL, @MaxPrice = NULL,@FromMakeYear = NULL, @ToMakeYear = NULL, @ModelIds = NULL, 
					 @ModelNames = NULL, @BodyTypeIds = NULL, @FuelTypeIds = NULL, @BodyTypeNames = NULL, @FuelTypeNames = NULL,
					 @Status = @InqStatus OUTPUT, @CW_CustomerId = @CWCustomerId, @LeadDivertedTo = @LeadDivertedTo OUTPUT,
					 @TC_BuyerInquiryId = @TC_BuyerInquiryId OUTPUT, @ImportInqCarDetails = NULL, @ExcelInquiryId = NULL

		IF(@TC_BuyerInquiryId IS NOT NULL AND @TC_BuyerInquiryId <> -1)
		BEGIN
			UPDATE TC_MMDealersPoint  
			SET CurrentPoint = (CurrentPoint - @CreditPoints)  
			WHERE CurrentPoint >= @CreditPoints and DealerId = @BranchId ;
			
			-- Modified By: Nilesh Utture on 3rd Jan, 2014  Deduct points from all matching stocks
			UPDATE TC_MMDealersMatchCount 
			SET MatchViewCount = MatchViewCount - 1 ,
			    LastUpdatedOn=GETDATE()
			WHERE StockId IN (SELECT MatchedStockId 
			                     FROM TC_MMCustomerDetails 
								 WHERE CWCustomersId = @CWCustomerId 
								 AND DealerId = @BranchId
								 AND IsDeleted=0 --- Condition added by manisn on 03-03-2014
							  ) 
			AND DealerId = @BranchId
			
			-- Delete commented by manish on 03-03-2014 using IsDeleted column in place of delete statement
			/*DELETE FROM TC_MMCustomerDetails 
			 WHERE CWCustomersId =  @CWCustomerId 
			 AND DealerId = @BranchId*/

			 ---------below update statement added by manish on 03-03-2014  
			 UPDATE TC_MMCustomerDetails  SET IsDeleted=1
			    WHERE CWCustomersId =  @CWCustomerId 
			        AND DealerId = @BranchId

			

			INSERT INTO [dbo].[TC_MMAudit]
				   ([DealerId]
				   ,[UserId]
				   ,[CustomerId]
				   ,[BuyDate]
				   ,[CWInquiryId]
				   ,[BuyPoints]
				   ,[ABInquiryId]
				   ,[SellerType])
			 VALUES
				   (@BranchId,
					@UserId,
					@CWCustomerId,
					GETDATE(),
					@CWInquiryId,
					@CreditPoints,
					@TC_BuyerInquiryId,
					@SellerType
				   )
		END
		SET @Status = 1;
	END
	ELSE
	BEGIN
		SET @Status = 0;
	END
END



/****** Object:  StoredProcedure [dbo].[TC_MMInsertMatchingInquiryForAllStock]    Script Date: 03/03/2014 20:36:24 ******/
SET ANSI_NULLS ON
