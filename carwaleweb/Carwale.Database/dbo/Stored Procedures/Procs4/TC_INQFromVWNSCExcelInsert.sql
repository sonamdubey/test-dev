IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQFromVWNSCExcelInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQFromVWNSCExcelInsert]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 12 Feb 2014
-- Description:	To insert imported nsc excel inquiry.
-- DECLARE @IsNewInq BIT 
-- EXEC [TC_INQFromVWNSCExcelInsert] NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 
-- 1, 5,NULL, 2278,NULL, NULL, 1, NULL, NULL,@IsNewInq OUTPUT
-- SELECT @IsNewInq
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQFromVWNSCExcelInsert] 
	@Salutation VARCHAR(10) ,
	@FirstName VARCHAR(100) ,
	@LastName VARCHAR(100) ,
	@Email VARCHAR(100) ,
	@Mobile VARCHAR(15) ,
	@City VARCHAR(50) ,
	@CityId INT,
	@Make VARCHAR(50) ,
	@Model VARCHAR(50) ,
	@Version VARCHAR(50) ,
	@VersionId INT,
	--@BuyingTime VARCHAR(20) ,
	@CarDetails VARCHAR(1000) ,
	--@Eagerness SMALLINT ,
	@IsValid BIT ,
	@UserId BIGINT, 
	@BranchId BIGINT,
	@DealerCode VARCHAR(10),
	@Source VARCHAR(50),
	@TC_InquirySourceId SMALLINT,
	@ExcelInquiryId BIGINT = NULL,
	@ExcelSheetId BIGINT,
	@IsSpecialUser BIT,
	@LeadIdOutput BIGINT OUTPUT,
	@IsNewInq BIT OUTPUT,
	@IsUnassigned BIT=0 OUTPUT,
	@IsDiverted BIT OUTPUT,
	@INQLeadId BIGINT OUTPUT
AS
BEGIN
	
		DECLARE @TC_FollowUpDetailsFromExcel TC_FollowUpDetailsFromExcel
		EXECUTE TC_INQFromExcelInsertForVW
				@Name = @FirstName,	@Email = @Email,@Mobile = @Mobile,@Location = @City,@Address = NULL,@Make = @Make,@Model = @Model,@Version = @Version,
				@Color = NULL,@CarYear = NULL,@BuyingTime = NULL,@CarDetails = @CarDetails,@Eagerness = NULL,@Comments = NULL,@SalesConsultant = NULL,
				@InquirySource = @Source,@InquiryDate = NULL,@AlternateNo = NULL,@Area = NULL,@CompanyName = NULL,@IsEligibleForCorporate = NULL,@AlternateModel = NULL,
				@AlternateVersion = NULL,@AlternateVersionColour = NULL,@FirstCarModelOwned = NULL,@FirstCarVersionOwned = NULL,@YearOfPurchase1 = NULL,@SecondCarModelOwned = NULL,
				@SecondCarVersionOwned = NULL,@YearOfPurchase2 = NULL,	@IsWillingnessToExchange = NULL,@IsFinanceRequired = NULL,@IsAccessoriesRequired = NULL,
				@IsSchemesOffered = NULL,@IsTestDriveRequested = NULL,@TestDriveDate = NULL,@TDStatus = NULL,@NextFollowUpDate = NULL,	@IsValid = @IsValid,@SalesConsultantId = NULL,
				@SourceId = @TC_InquirySourceId,@CityId = @CityId,@AreaId = NULL,@PrefVersionId = @VersionId,@AlternateVersionId = NULL,@PrefColorIds = NULL,@AlternateColorIds = NULL,
				@RecentCommentDate = NULL,@RecentComment = NULL,@LostDispositionId = NULL,@TDStatusId = NULL,@ActivityFeed = NULL,
				@TC_FollowUpDetailsFromExcel = @TC_FollowUpDetailsFromExcel,@IsCompleteTD = NULL,@ExcelSheetId = @ExcelSheetId,@BranchId = @BranchId,
				@UserId = @UserId,@InquiryType = 3,@IsLeadLost = NULL,@LostReason = NULL,@IsCustomerSuspended = NULL,	
				@IsCarBooked = NULL,@BookingDate = NULL,@PanNo = NULL,@BookingAmount = NULL,@BookingReceiptNo = NULL,@TentativeDeliveryDate = NULL,
				@IsAvailedFinance = NULL,@IsBoughtAccessories = NULL,@IsCarDelivered = NULL,@DeliveryDate = NULL,@ExcelInquiryId = @ExcelInquiryId,
				@IsNewInq = @IsNewInq OUTPUT,@IsUnassigned = @IsUnassigned OUTPUT,@IsDiverted = @IsDiverted OUTPUT,@INQLeadId = @INQLeadId OUTPUT,@IsSpecialUser = @IsSpecialUser,
				@DealerCode = @DealerCode,@LastName = @LastName,@Salutation = @Salutation

END

