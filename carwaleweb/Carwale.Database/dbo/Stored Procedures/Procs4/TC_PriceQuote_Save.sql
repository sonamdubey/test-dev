IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_PriceQuote_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_PriceQuote_Save]
GO

	-- =============================================
-- Author:		Chetan Kane
-- Create date: 3/5/2012, 11:43 AM
-- Description:	SP to save price quote taken from dealers Microsite.
-- There are two steps to save a price qoute. 
-- 1. Register user to TC_CustomersDeyails table and save the car details in the TC_Inquiries using TC_AddTCInquiries 
-- 2. Insert price quote details as inquerie TC_PriceQuoteRequests Table 
-- =============================================
CREATE PROCEDURE [dbo].[TC_PriceQuote_Save]    
    
@DealerId INT, --Id of a 
@Cityid INT,    
@VersionId INT,   
@BuyPlan VARCHAR(30),   
@ReqType TINYINT,  
@InquirySource TINYINT,

-- Customer's contact details
@CustomerName VARCHAR(50),
@CustomerEmail VARCHAR(80),
@CustomerMobile VARCHAR(10),
@QuoteId BIGINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	BEGIN
		-- 1st Step
		DECLARE @TC_InquiryId NUMERIC
		DECLARE @TC_InquiriesId BIGINT
                   
		EXECUTE TC_AddTCInquiries @CustomerName = @CustomerName, @Email = @CustomerEmail, 
		@Mobile = @CustomerMobile, @Location = NULL, @Buytime = @BuyPlan, @CustomerComments = NULL, 
		@BranchId = @DealerId, @VersionId = @VersionId, @Comments = NULL, @InquiryStatus = NULL, @NextFollowup = NULL, @AssignedTo = NULL, @InquiryType = @ReqType, 
		@InquirySource = @InquirySource, @TC_LeadTypeId = 2,
		@UserId = NULL, @TC_InquiriesId = @TC_InquiriesId OUTPUT
		
		-- 2nd Step
		IF @TC_InquiriesId IS NOT NULL
		BEGIN
			INSERT INTO TC_PriceQuoteRequests (TC_InquiriesId,BuyTime,CityId) VALUES(@TC_InquiriesId, @BuyPlan, @Cityid)
			SET @QuoteId = SCOPE_IDENTITY()
		END		
	END
END




