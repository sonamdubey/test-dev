IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ContactRequest_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ContactRequest_Save]
GO

	-- =============================================
-- Author:		Chetan Kane
-- Create date: 15/3/2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TC_ContactRequest_Save] 
	@DealerId NUMERIC,
	@CustomerEmail VarChar(100),
	@CustomerName VarChar(100),
	@CustomerMobile VARCHAR(15),
	@Comments VARCHAR(250),
	@InquirySource TINYINT,
	@ReqType TINYINT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	BEGIN
		-- 1st step
		DECLARE @CustomerId NUMERIC
		DECLARE @TC_InquiryId NUMERIC
		DECLARE @TC_InquiriesId BIGINT
                   
		EXECUTE TC_AddTCInquiries @CustomerName = @CustomerName, @Email = @CustomerEmail, 
		@Mobile = @CustomerMobile, @Location = NULL, @Buytime = NULL, @CustomerComments = NULL, 
		@BranchId = @DealerId, @VersionId = NULL, @Comments = NULL, @InquiryStatus = NULL, @NextFollowup = NULL, @AssignedTo = NULL, @InquiryType = @ReqType, 
		@InquirySource = @InquirySource, @TC_LeadTypeId = 2,
		@UserId = NULL, @TC_InquiriesId = @TC_InquiriesId OUTPUT
		-- 2nd step
		IF @TC_InquiriesId IS NOT NULL		
		BEGIN 
			INSERT INTO TC_OtherRequests ( TC_InquiriesId,Comments) VALUES(@TC_InquiriesId,@Comments)	
		END	
	END
END

