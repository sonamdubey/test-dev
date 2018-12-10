IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertUnassignedInqFromVWExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertUnassignedInqFromVWExcel]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 14 July 2013
-- Description:	To insert unassigned imported vw format of excel inquiry.
--DECLARE @LeadIdOutput BIGINT 
--EXEC [TC_InsertUnassignedInqFromVWExcel] 1,5,72,229,@LeadIdOutput OUTPUT
--SELECT @LeadIdOutput
-- Modified By : Tejashre Patil on 24 Jun 2013, added UserId in seller and new car select query.
-- Modified By : Tejashre Patil on 6 Aug 2013, @Eagerness is fetched by SELECT clause.
-- Modified By : Tejashre Patil on 25 Feb 2014, NSC excel related changes done.
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsertUnassignedInqFromVWExcel]
	@InquiryType TINYINT,
	@BranchId BIGINT,
	@ExcelInquiryId BIGINT,
	@LeadOwnerId BIGINT,
	@TC_FollowUpDetailsFromExcel TC_FollowUpDetailsFromExcel READONLY,
	@IsTDCompleted BIT
AS
BEGIN
			
	DECLARE @TC_CustomerId BIGINT, @AutoVerified BIT = 1, @TC_InquiryId BIGINT,@LeadIdOutput BIGINT
	DECLARE @InqStatus SMALLINT , @LeadDivertedTo VARCHAR(100),	@LeadOwnId BIGINT,@CustomerId BIGINT
	
	DECLARE  @Name VARCHAR(100),@Email VARCHAR(100),@Mobile VARCHAR(50), @Location VARCHAR(50), @Address VARCHAR(100), @Make VARCHAR(50),  
			 @Model VARCHAR(50),@Version VARCHAR(50), @Color VARCHAR(150), @CarYear VARCHAR(50), @BuyingTime VARCHAR(50), @CarDetails VARCHAR(50),
			 @Eagerness VARCHAR(50),@Comments VARCHAR(50), @SalesConsultant VARCHAR(50), @InquirySource VARCHAR(50), @InquiryDate VARCHAR(50), 
			 @AlternateNo VARCHAR(50), @Area VARCHAR(50),@CompanyName VARCHAR(150), @IsEligibleForCorporate BIT, @AlternateModel VARCHAR(50), 
			 @AlternateVersion VARCHAR(50), @AlternateVersionColour VARCHAR(150),@FirstCarModelOwned VARCHAR(50), @FirstCarVersionOwned VARCHAR(50), 
			 @YearOfPurchase1 VARCHAR(50), @SecondCarModelOwned VARCHAR(50), @SecondCarVersionOwned VARCHAR(50), @YearOfPurchase2 VARCHAR(50),  
			 @IsWillingnessToExchange BIT, @IsFinanceRequired BIT, @IsAccessoriesRequired BIT, @IsSchemesOffered BIT, @IsTestDriveRequested BIT,
			 @TestDriveDate VARCHAR(50), @TDStatus VARCHAR(50), @NextFollowUpDate VARCHAR(50),  @IsValid BIT, @SalesConsultantId BIGINT, 
			 @TC_InquirySourceId TINYINT,@CityId INT, @AreaId INT,@PrefVersionId INT, @AlternateVersionId INT, @PrefColorIds VARCHAR(150), 
			 @AlternateColorIds VARCHAR(150),@RecentCommentDate VARCHAR(50), @RecentComment VARCHAR(MAX), @LostDispositionId SMALLINT, 
			 @TDStatusId SMALLINT, @ActivityFeed VARCHAR(MAX),/*@IsCompleteTD BIT,*/ @ExcelSheetId BIGINT, @UserId BIGINT,  @IsLeadLost BIT, 
			 @LostReason VARCHAR(50), @IsCustomerSuspended BIT,  @IsCarBooked BIT, @PanNo VARCHAR(25), @BookingAmount INT,
			 @BookingReceiptNo VARCHAR(50), @TentativeDeliveryDate VARCHAR(50), @IsAvailedFinance BIT, @IsBoughtAccessories BIT, @IsCarDelivered BIT,
			 @DeliveryDate VARCHAR(50), @IsNewInq BIT , @IsUnassigned BIT=0, @IsDiverted BIT,@INQLeadId BIGINT=NULL, @OldLeadOwnerId BIGINT ,
			 @BookingDate VARCHAR(50),
			 -- Modified By : Tejashre Patil on 25 Feb 2014, NSC excel related changes done.
			 @LastName VARCHAR(100), @Salutation VARCHAR(10), @IsSpecialUser BIT = 0 , @DealerCode VARCHAR(10)
	
	BEGIN
		IF (@InquiryType=3)
		BEGIN
			
			IF(@ExcelInquiryId IS NOT NULL)
			BEGIN
				SELECT	@Name=Name,@Email=Email,@Mobile=Mobile,@Location=City,@Make=CarMake,@Model=CarModel,@Isvalid=Isvalid,@UserId=UserId,@BranchId=BranchId,
						@TC_InquirySourceId=TC_InquirySourceId,@OldLeadOwnerId=LeadOwnerId,@ExcelSheetId=ExcelSheetId,@CityId=CityId,
						@PrefVersionId=VersionId,@Comments=Comments,@Version=CarVersion,@AlternateVersion=AlternateCarVersion,
						@AlternateVersionId=AlternateVersionId,@SalesConsultant=SalesConsultant,@SalesConsultantId =SalesConsultantId ,
						@InquirySource=InquirySource,@InquiryDate=InquiryDate,@AlternateNo=AlternateNo,@CompanyName=CompanyName,@Area=Area,
						@AreaId=AreaId,@Color=PreferedVersionColour,@PrefColorIds=PreferedVersionColourIds,
						@AlternateModel=AlternateModel,@AlternateVersionColour=AlternateVersionColour,
						@AlternateColorIds=AlternateVersionColourIds,@IsEligibleForCorporate=ISNULL(IsEligibleForCorporate,0),
						@IsWillingnessToExchange=IsWillingnessToExchange,@IsFinanceRequired=IsFinanceRequired,
						@IsAccessoriesRequired=IsAccessoriesRequired,@IsSchemesOffered=IsSchemesOffered,@IsTestDriveRequested=IsTestDriveRequested,
						@TestDriveDate=TestDriveDate,@NextFollowUpDate=NextFollowUpDate,@RecentComment =RecentComment ,
						@RecentCommentDate=RecentCommentDate,@ActivityFeed=ActivityFeed,@LostDispositionId=LostDispositionId,@TDStatusId=TDStatusId,
						@IsLeadLost =IsLeadLost ,@LostReason=LeadLostReason,@IsCustomerSuspended=IsCustomerSuspended,@IsCarBooked =IsCarBooked ,
						@PanNo =PanNo ,@BookingAmount =BookingAmount ,@BookingReceiptNo =BookingReceiptNo ,@TentativeDeliveryDate =TentativeDeliveryDate ,
						@IsAvailedFinance =IsAvailedFinance ,@IsBoughtAccessories=IsBoughtAccessories,@IsCarDelivered =IsCarDelivered ,
						@DeliveryDate =DeliveryDate , @BookingDate=BookingDate, @Eagerness=Eagerness,-- Modified By : Tejashre Patil on 6 Aug 2013
						-- Modified By : Tejashre Patil on 25 Feb 2014, NSC excel related changes done.
						@LastName = E.LastName,@Salutation = E.Salutation, @UserId = CASE WHEN E.IsSpecialUser=1 THEN @LeadOwnerId ELSE E.UserId END
				FROM	TC_ExcelInquiries E
				WHERE	BranchId=@BranchId AND IsDeleted=0
						AND Id =@ExcelInquiryId	

			END
			
			SET		@SalesConsultantId=@LeadOwnerId
			
			EXEC TC_INQFromExcelInsertForVW
				@Name ,@Email ,@Mobile ,@Location ,@Address ,@Make ,@Model ,@Version ,@Color ,@CarYear ,@BuyingTime ,@CarDetails ,
				@Eagerness ,@Comments ,@SalesConsultant ,@InquirySource ,@InquiryDate ,@AlternateNo ,@Area ,@CompanyName ,@IsEligibleForCorporate ,
				@AlternateModel ,@AlternateVersion ,@AlternateVersionColour ,@FirstCarModelOwned ,@FirstCarVersionOwned ,@YearOfPurchase1 ,
				@SecondCarModelOwned ,@SecondCarVersionOwned ,@YearOfPurchase2 ,@IsWillingnessToExchange,@IsFinanceRequired,@IsAccessoriesRequired,
				@IsSchemesOffered,@IsTestDriveRequested,@TestDriveDate ,@TDStatus ,@NextFollowUpDate ,@IsValid,@SalesConsultantId ,@TC_InquirySourceId ,
				@CityId ,@AreaId ,@PrefVersionId ,@AlternateVersionId ,@PrefColorIds ,@AlternateColorIds ,@RecentCommentDate ,@RecentComment ,
				@LostDispositionId ,@TDStatusId ,@ActivityFeed , @TC_FollowUpDetailsFromExcel ,@IsTDCompleted,@ExcelSheetId ,@BranchId , @UserId ,
				@InquiryType ,@IsLeadLost, @LostReason ,@IsCustomerSuspended,@IsCarBooked,@BookingDate,@PanNo ,@BookingAmount ,@BookingReceiptNo ,
				@TentativeDeliveryDate ,@IsAvailedFinance, @IsBoughtAccessories,@IsCarDelivered,@DeliveryDate ,@ExcelInquiryId ,@IsNewInq OUTPUT,
				@IsUnassigned OUTPUT,@IsDiverted OUTPUT, @INQLeadId OUTPUT,
				-- Modified By : Tejashree Patil on 10 Feb 2014, Added parametrers IsSpecialUser,DealerCode for excel imported by NSC .
				@IsSpecialUser, @DealerCode, @LastName, @Salutation
		END	
		
		
	END
						
END


SET ANSI_NULLS ON
