IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_NewCarPurchaseUpdateDealerNAudit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_NewCarPurchaseUpdateDealerNAudit]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 28-Feb-2014
-- Description:	Audit for New Car purchase inquiry bought by Dealer,Update dealer point AND Delete that customer from TC_NewCarPurchaseLead table.
-- =============================================
CREATE PROCEDURE [dbo].[TC_NewCarPurchaseUpdateDealerNAudit] 
	-- Add the parameters for the stored procedure here
	@CreditPoints INT ,
	@BranchId INT= NULL,
	@NewCarPurchaseInquiriesId BIGINT= NULL,
	@LeadOwnerId BIGINT = NULL,
	@CustomerId BIGINT= NULL,
	@CWCustomerId BIGINT= NULL,
	@TC_NewCarInquiryId BIGINT= NULL,
	@VersionId INT = NULL


AS
BEGIN
	--New car Purchase Inquiry Audit 
		INSERT INTO [dbo].[TC_NewCarPurchaseAudit]
				   ([DealerId]
           ,[UserId]
           ,[CustomerId]
           ,[BuyDate]
           ,[NewCarPurchaseInquiriesId]
           ,[BuyPoints]
           ,[VersionId]
           ,[TC_InquiriesLeadId])
			 VALUES
				   (@BranchId,
					@LeadOwnerId,
					@CustomerId,
					GETDATE(),
					@NewCarPurchaseInquiriesId,
					@CreditPoints,
					@VersionId,
					@TC_NewCarInquiryId
					
				   )
		--Update Dealerpoints
			UPDATE TC_NewCarPurchaseDealerPoints
			SET CurrentPoint = (CurrentPoint - @CreditPoints), LastUpdatedOn = GETDATE()
			WHERE CurrentPoint >= @CreditPoints and DealerId = @BranchId ;
			
	
END
