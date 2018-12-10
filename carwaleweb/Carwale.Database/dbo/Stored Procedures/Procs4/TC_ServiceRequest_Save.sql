IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ServiceRequest_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ServiceRequest_Save]
GO

	-- =============================================
-- Author:		Chetan Kane
-- Create date: 06/03/2012
-- Description:	SP to save price quote taken from dealers Microsite.
-- There are two steps to save a price qoute. 
-- 1. Register user to TC_CustomersDeyails table and save the car details in the TC_Inquiries using TC_AddTCInquiries
-- 2. Register this ServiceRequest as a inquiry to TC_ServiceRequests Table
-- =============================================
CREATE PROCEDURE [dbo].[TC_ServiceRequest_Save] 
	@DealerId NUMERIC, 
	@VersionId INT,
	@RegNo VARCHAR,
	@PreferedDate DATE,
	@TypeOfService SMALLINT,
	@Comment VARCHAR(250),
	
	@CustomerEmail VarChar(100),
	@CustomerName VarChar(100),
	@CustomerMobile VARCHAR(15),
	@ReqType TINYINT,
	@InquirySource TINYINT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	BEGIN
		-- 1st step
		DECLARE @TC_InquiriesId BIGINT
                   
		EXECUTE TC_AddTCInquiries @CustomerName = @CustomerName, @Email = @CustomerEmail, 
		@Mobile = @CustomerMobile, @Location = NULL, @Buytime = NULL, @CustomerComments = NULL, 
		@BranchId = @DealerId, @VersionId = @VersionId, @Comments = @Comment, @InquiryStatus = NULL, @NextFollowup = NULL, @AssignedTo = NULL, @InquiryType = @ReqType, 
		@InquirySource = @InquirySource, @TC_LeadTypeId = 2,
		@UserId = NULL, @TC_InquiriesId = @TC_InquiriesId OUTPUT
		-- 2nd step
		IF @TC_InquiriesId IS NOT NULL		
		BEGIN 
			INSERT INTO TC_ServiceRequests(TC_InquiriesId,RegNo,PreferredDate,TypeOfService,Comments) VALUES(@TC_InquiriesId, @RegNo, @PreferedDate, @TypeOfService, @Comment)
		END
	END
END
