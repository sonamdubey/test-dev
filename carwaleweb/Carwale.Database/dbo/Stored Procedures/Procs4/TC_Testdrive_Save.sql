IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Testdrive_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Testdrive_Save]
GO

	-- =============================================
-- Created By: Chetan Kane
-- Create Date: 28/2/2012
-- Description:	SP to push submitted lead to TC Inquiry Management.
-- =============================================
CREATE PROCEDURE [dbo].[TC_Testdrive_Save] 
	-- Add the parameters for the stored procedure here
	@DealerId NUMERIC, -- Id of the dealer to whom this TD request is to be sumbmited
	@ModelId SMALLINT, -- Model choosen by customer for test drive 
	@FuelType TINYINT, -- Fuel type preffered by customer 1.Petrol 2.Diesel 
	@Transmission TINYINT, -- Transmission type preffered by customer 1.Automatic 2.Manual
	
	-- Customer's contact details 
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
		DECLARE @TC_InquiryId NUMERIC
		DECLARE @TC_InquiriesId BIGINT
                   
        -- In case of test drive version id is not available. However version is is required during pusing leads to TC Inquiry Management.
        -- Hence we need to fetch version id from available modelId
		DECLARE @TC_VersionId BIGINT
		Set @TC_VersionId=(SELECT TOP 1 id FROM CarVersions WHERE CarModelId=@ModelId and IsDeleted=0 and New=1)
        
        -- Push Lead to TC Inquiry Management           
		EXECUTE TC_AddTCInquiries @CustomerName = @CustomerName, @Email = @CustomerEmail, 
		@Mobile = @CustomerMobile, @Location = NULL, @Buytime = NULL, @CustomerComments = NULL, 
		@BranchId = @DealerId, @VersionId = @TC_VersionId, @Comments = NULL, @InquiryStatus = NULL, @NextFollowup = NULL, @AssignedTo = NULL, @InquiryType = @ReqType, 
		@InquirySource = @InquirySource, @TC_LeadTypeId = 2,
		@UserId = NULL, @TC_InquiriesId = @TC_InquiriesId OUTPUT
		
		-- Check whether inquiry submitted successfully to IM
		IF @TC_InquiriesId IS NOT NULL		
		BEGIN 
			INSERT INTO TC_TestdriveRequests( TC_InquiriesId, ModelId,FuelType,Transmission) VALUES( @TC_InquiriesId, @ModelId,@FuelType,@Transmission)
		END
	END
END

