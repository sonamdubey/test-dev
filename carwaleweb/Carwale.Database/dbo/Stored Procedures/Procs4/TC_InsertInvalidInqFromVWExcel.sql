IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertInvalidInqFromVWExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertInvalidInqFromVWExcel]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 4 July 2013
-- Description:	To insert imported invalid excel inquiry for VW.
-- Modified By : Tejashree Patil on 29 July 2013, Added @BookingAmount parameter and updated BookingAmount.
-- Modified By : Tejashree Patil on 4 Sept 2013, Added query for invalid inquiry, but after validating it but unassigned to user.
-- Modified By : Tejashree Patil on 10 Feb 2014, Added parametrers and related changes done for IsSpecialUser,DealerCode, LastName, Salutation for excel imported by NSC .
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsertInvalidInqFromVWExcel] 
	@Name VARCHAR(100)= NULL,
	@Email VARCHAR(100)= NULL,
	@Mobile VARCHAR(50)= NULL,
	@Location VARCHAR(50)= NULL,
	@Make VARCHAR(50)= NULL,	
	@Model VARCHAR(50)= NULL,
	@Version VARCHAR(50)= NULL,
	@Color VARCHAR(150)= NULL,
	@CarDetails VARCHAR(50)= NULL,
	@Eagerness VARCHAR(50)= NULL,
	@SalesConsultant VARCHAR(50)= NULL,
	@InquirySource VARCHAR(50)= NULL,
	@InquiryDate DATETIME= NULL,
	@CompanyName VARCHAR(150)= NULL,
	@AlternateModel VARCHAR(50)= NULL,
	@AlternateVersion VARCHAR(50)= NULL,
	@AlternateVersionColour VARCHAR(50)= NULL,
	@TestDriveDate VARCHAR(50)= NULL,
	@TDStatus VARCHAR(50)= NULL,
	@NextFollowUpDate VARCHAR(50)= NULL,
	@IsValid BIT= NULL,
	@SalesConsultantId BIGINT= NULL,
	@SourceId INT= NULL,
	@CityId INT= NULL,
	@PrefVersionId INT= NULL,
	@AlternateVersionId INT= NULL,
	@PrefColorIds VARCHAR(150)= NULL,
	@AlternateColorIds VARCHAR(150)= NULL,
	@LostDispositionId SMALLINT= NULL,
	@TDStatusId SMALLINT= NULL,
	@TC_FollowUpDetailsFromExcel TC_FollowUpDetailsFromExcel READONLY,
	@IsCompleteTD BIT= NULL,
	@BranchId BIGINT= NULL, 
	@UserId BIGINT,
	@InquiryType TINYINT = NULL,
	@LostReason VARCHAR(50) = NULL,
	@DeliveryDate VARCHAR(50) = NULL,
	@BookingDate VARCHAR(50) = NULL,
	@ExcelInquiryId BIGINT = NULL,
	@BookingAmount VARCHAR(50) = NULL,-- Modified By : Tejashree Patil on 29 July 2013,
	-- Modified By : Tejashree Patil on 10 Feb 2014, Added parametrers and related changes done for IsSpecialUser,DealerCode, LastName, Salutation for excel imported by NSC .
	@IsSpecialUser BIT = 0,
	@DealerCode VARCHAR(50) = NULL,
	@LastName VARCHAR(100) = NULL,
	@Salutation VARCHAR(10) = NULL
	
AS
BEGIN
	DECLARE  @CarYear VARCHAR(50), @BuyingTime VARCHAR(50), @Comments VARCHAR(50), @AlternateNo VARCHAR(50), @Area VARCHAR(50),
			 @FirstCarModelOwned VARCHAR(50), @FirstCarVersionOwned VARCHAR(50), @Address VARCHAR(100),
			 @YearOfPurchase1 VARCHAR(50), @SecondCarModelOwned VARCHAR(50), @SecondCarVersionOwned VARCHAR(50), @YearOfPurchase2 VARCHAR(50),  
			 @IsWillingnessToExchange BIT, @IsFinanceRequired BIT, @IsAccessoriesRequired BIT, @IsSchemesOffered BIT, @IsTestDriveRequested BIT,			 
			 @AreaId INT, @RecentCommentDate VARCHAR(50), @RecentComment VARCHAR(MAX)=NULL, @ActivityFeed VARCHAR(MAX)=NULL,
			 @IsLeadLost BIT, @IsCustomerSuspended BIT,  @IsCarBooked BIT, @PanNo VARCHAR(25), @IsEligibleForCorporate BIT,
			 @BookingReceiptNo VARCHAR(50), @TentativeDeliveryDate VARCHAR(50), @IsAvailedFinance BIT, @IsBoughtAccessories BIT, @IsCarDelivered BIT,
			 @IsNewInq BIT , @IsUnassigned BIT, @IsDiverted BIT,@INQLeadId BIGINT, @OldLeadOwnerId BIGINT , @ExcelSheetId BIGINT
	
	IF(@IsSpecialUser = 0) -- Modified By : Tejashree Patil on 10 Feb 2014,Added @IsSpecialUser condition.
		SET @IsSpecialUser = NULL
		
	IF(@Location IS NULL)
	BEGIN
		SELECT  @Location=Ct.Name,@CityId=Ct.ID
		FROM	Dealers DB
				LEFT JOIN Cities Ct WITH (NOLOCK) ON DB.CityId=Ct.ID
		WHERE	DB.IsTCDealer = 1 AND DB.ID=@BranchId
	END
	IF (@InquiryType=3)
	BEGIN
		IF(@ExcelInquiryId IS NOT NULL)
		BEGIN
			--1) Insert into dump table
			UPDATE	TC_ExcelInquiries
			SET		Name=@Name,Email=@Email,Mobile=@Mobile,City=@Location,CarMake=@Make,CarModel=@Model,IsValid=@IsValid,
					TC_InquirySourceId=@SourceId,CityId=@CityId,VersionId=@PrefVersionId,CarVersion=@Version,AlternateCarVersion=@AlternateVersion,
					AlternateVersionId=@AlternateVersionId,SalesConsultant=@SalesConsultant,SalesConsultantId =@SalesConsultantId ,
					InquirySource=@InquirySource,InquiryDate=@InquiryDate,AlternateNo=@AlternateNo,CompanyName=@CompanyName,
					PreferedVersionColourIds=@PrefColorIds,AlternateModel=@AlternateModel,BookingAmount=@BookingAmount,
					AlternateVersionColour=@AlternateVersionColour,AlternateVersionColourIds=@AlternateColorIds,TestDriveDate=@TestDriveDate,
					NextFollowUpDate=@NextFollowUpDate,LostDispositionId=@LostDispositionId,TDStatusId=@TDStatusId,
					LeadLostReason=@LostReason,DeliveryDate =@DeliveryDate, Eagerness=@Eagerness, BookingDate=@BookingDate,
					-- Modified By : Tejashree Patil on 10 Feb 2014, Updated parametrers DealerCode, LastName, Salutation.
					BranchId=@BranchId,	LastName=@LastName, DealerCode=@DealerCode, Salutation=@Salutation		  
			WHERE	((@IsSpecialUser IS NULL AND BranchId=@BranchId)  OR (IsSpecialUser=@IsSpecialUser)) -- Modified By : Tejashree Patil on 10 Feb 2014,Added @IsSpecialUser condition.
					AND Id =@ExcelInquiryId
		END
		
		SELECT	@Name=Name,@Email=Email,@Mobile=Mobile,@Location=City,@Make=CarMake,@Model=CarModel,@IsValid=IsValid,@UserId=UserId,
				@BranchId=BranchId,@SourceId=TC_InquirySourceId,@OldLeadOwnerId=LeadOwnerId,@ExcelSheetId=ExcelSheetId,@CityId=CityId,
				@PrefVersionId=VersionId,@Comments=Comments,@Version=CarVersion,@AlternateVersion=AlternateCarVersion,
				@AlternateVersionId=AlternateVersionId,@SalesConsultant=SalesConsultant,@SalesConsultantId =SalesConsultantId ,
				@InquirySource=InquirySource,@InquiryDate=InquiryDate,@AlternateNo=AlternateNo,@CompanyName=CompanyName,@Area=Area,
				@AreaId=AreaId,@Color=PreferedVersionColour,@PrefColorIds=PreferedVersionColourIds,
				@AlternateModel=AlternateModel,@AlternateVersionColour=AlternateVersionColour,
				@AlternateColorIds=AlternateVersionColourIds,@IsEligibleForCorporate=IsEligibleForCorporate,
				@IsWillingnessToExchange=IsWillingnessToExchange,@IsFinanceRequired=IsFinanceRequired,
				@IsAccessoriesRequired=IsAccessoriesRequired,@IsSchemesOffered=IsSchemesOffered,@IsTestDriveRequested=IsTestDriveRequested,
				@TestDriveDate=TestDriveDate,@NextFollowUpDate=NextFollowUpDate,@RecentComment =RecentComment ,
				@RecentCommentDate=RecentCommentDate,@ActivityFeed=ActivityFeed,@LostDispositionId=LostDispositionId,@TDStatusId=TDStatusId,
				@IsLeadLost =IsLeadLost ,@LostReason=LeadLostReason,@IsCustomerSuspended=IsCustomerSuspended,@IsCarBooked =IsCarBooked ,
				@PanNo =PanNo ,@BookingAmount =BookingAmount ,@BookingReceiptNo =BookingReceiptNo ,@TentativeDeliveryDate =TentativeDeliveryDate ,
				@IsAvailedFinance =IsAvailedFinance ,@IsBoughtAccessories=IsBoughtAccessories,@IsCarDelivered =IsCarDelivered ,
				@DeliveryDate =DeliveryDate , @BookingDate=BookingDate, 
				-- Modified By : Tejashree Patil on 10 Feb 2014, Updated parametrers DealerCode, LastName, Salutation.
				@LastName = LastName, @DealerCode=DealerCode
		FROM	TC_ExcelInquiries
		WHERE	((@IsSpecialUser IS NULL AND BranchId=@BranchId)  OR (IsSpecialUser=@IsSpecialUser))-- Modified By : Tejashree Patil on 10 Feb 2014,Added @IsSpecialUser condition.
				AND IsDeleted=0
				AND Id =@ExcelInquiryId	
		
		/************************************ Modified By : Tejashree Patil on 4 Sept 2013******************************/
		--When closed but invalid lead after valid data but unassigned to any use then by default it will assigned to user which is importing excel or logged in user.
		IF (@IsValid=1 AND @SalesConsultantId IS NULL AND (@LostDispositionId IS NOT NULL OR @LostDispositionId <> '-1'))
		BEGIN
			SET @SalesConsultantId = @UserId
		END		
		/****************************************************************************************************************/
		IF (@IsValid=1 AND @SalesConsultantId IS NOT NULL)
		BEGIN
			EXEC TC_INQFromExcelInsertForVW
				@Name ,@Email ,@Mobile ,@Location ,@Address ,@Make ,@Model ,@Version ,@Color ,@CarYear ,@BuyingTime ,@CarDetails ,
				@Eagerness ,@Comments ,@SalesConsultant ,@InquirySource ,@InquiryDate ,@AlternateNo ,@Area ,@CompanyName ,@IsEligibleForCorporate,
				@AlternateModel ,@AlternateVersion ,@AlternateVersionColour ,@FirstCarModelOwned ,@FirstCarVersionOwned ,@YearOfPurchase1 ,
				@SecondCarModelOwned ,@SecondCarVersionOwned ,@YearOfPurchase2 ,@IsWillingnessToExchange,@IsFinanceRequired,@IsAccessoriesRequired,
				@IsSchemesOffered,@IsTestDriveRequested,@TestDriveDate ,@TDStatus ,@NextFollowUpDate ,@IsValid,@SalesConsultantId ,@SourceId ,
				@CityId ,@AreaId ,@PrefVersionId ,@AlternateVersionId ,@PrefColorIds ,@AlternateColorIds ,@RecentCommentDate ,@RecentComment ,
				@LostDispositionId ,@TDStatusId ,@ActivityFeed , @TC_FollowUpDetailsFromExcel ,@IsCompleteTD,@ExcelSheetId ,@BranchId , @UserId ,
				@InquiryType ,@IsLeadLost, @LostReason ,@IsCustomerSuspended,@IsCarBooked,@BookingDate, @PanNo ,@BookingAmount ,@BookingReceiptNo ,
				@TentativeDeliveryDate ,@IsAvailedFinance, @IsBoughtAccessories,@IsCarDelivered,@DeliveryDate ,@ExcelInquiryId ,@IsNewInq OUTPUT,
				@IsUnassigned OUTPUT,@IsDiverted OUTPUT, @INQLeadId OUTPUT, 
				-- Modified By : Tejashree Patil on 10 Feb 2014, Updated parametrers DealerCode, LastName, Salutation.
				@IsSpecialUser,@DealerCode,@LastName,@Salutation
		END	
	END	
END