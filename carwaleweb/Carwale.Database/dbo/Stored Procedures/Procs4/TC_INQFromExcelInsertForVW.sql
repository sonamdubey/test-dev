IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQFromExcelInsertForVW]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQFromExcelInsertForVW]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 4 July 2013
-- Description:	To insert imported excel inquiry for VW.
-- Modified by: Tejashree Patil on 24 July 2013, Inserted @LeadOwnerId as parameter in called sps instead of @SalesConsultantId.
-- Modified by: Tejashree Patil on 26 July 2013, Added booking date.--TC_CallScheduling
-- Modified by: Tejashree Patil on 31 July 2013, Called TC_CallScheduling sp if @TC_FollowUpDetailsFromExcel is Null.
-- Modified By : Tejashree Patil on 10 Jan 2014, removed IS NOT NULL, And called TC_INQNewCarBookingSave SP Instead of TC_INQNewCarBookingSaveTest.
-- Modified By : Tejashree Patil on 10 Feb 2014, Added parametrers and related changes done for IsSpecialUser,DealerCode, LastName, Salutation for excel imported by NSC .
-- Modified By : Tejashree Patil on 11 March 2014, check condition for @IsEligibleForCorporate is null
-- Modified by : Tehashree patil on 21-03-2014 for resolving bug related to insertion of special user in calls table
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQFromExcelInsertForVW] 
	@Name VARCHAR(100),
	@Email VARCHAR(100),
	@Mobile VARCHAR(50),
	@Location VARCHAR(50),
	@Address VARCHAR(100),
	@Make VARCHAR(50),	
	@Model VARCHAR(50),
	@Version VARCHAR(50),
	@Color VARCHAR(150),
	@CarYear VARCHAR(50),
	@BuyingTime VARCHAR(50),
	@CarDetails VARCHAR(50),
	@Eagerness VARCHAR(50),
	@Comments VARCHAR(50),
	@SalesConsultant VARCHAR(50),
	@InquirySource VARCHAR(50),
	@InquiryDate VARCHAR(50),
	@AlternateNo VARCHAR(50),
	@Area VARCHAR(50),
	@CompanyName VARCHAR(150),
	@IsEligibleForCorporate BIT,
	@AlternateModel VARCHAR(50),
	@AlternateVersion VARCHAR(50),
	@AlternateVersionColour VARCHAR(150),
	@FirstCarModelOwned VARCHAR(50),
	@FirstCarVersionOwned VARCHAR(50),
	@YearOfPurchase1 VARCHAR(50),
	@SecondCarModelOwned VARCHAR(50),
	@SecondCarVersionOwned VARCHAR(50),
	@YearOfPurchase2 VARCHAR(50),	
	@IsWillingnessToExchange BIT,
	@IsFinanceRequired BIT,
	@IsAccessoriesRequired BIT,
	@IsSchemesOffered BIT,
	-----TestDrive Related-------------
	@IsTestDriveRequested BIT,
	@TestDriveDate VARCHAR(50),
	@TDStatus VARCHAR(50),
	
	@NextFollowUpDate VARCHAR(50),	
	@IsValid BIT,
	@SalesConsultantId BIGINT,
	@SourceId INT,
	@CityId INT,
	@AreaId INT,
	@PrefVersionId INT,
	@AlternateVersionId INT,
	@PrefColorIds VARCHAR(150),
	@AlternateColorIds VARCHAR(150),
	@RecentCommentDate VARCHAR(50),--DATETIME,
	@RecentComment VARCHAR(MAX),
	@LostDispositionId SMALLINT,
	@TDStatusId SMALLINT,
	
	@ActivityFeed VARCHAR(MAX),
	------ FollowUp Details ------
	@TC_FollowUpDetailsFromExcel TC_FollowUpDetailsFromExcel READONLY,
	
	@IsCompleteTD BIT,
	@ExcelSheetId BIGINT,
	@BranchId BIGINT, 
	@UserId BIGINT,
	@InquiryType TINYINT,
	-----Disposition related-------------
	@IsLeadLost BIT,
	@LostReason VARCHAR(50),
	@IsCustomerSuspended BIT,	
	-----Booking related-------------
	@IsCarBooked BIT,
	@BookingDate VARCHAR(50),
	@PanNo VARCHAR(25),
	@BookingAmount VARCHAR(50),
	@BookingReceiptNo VARCHAR(50),
	@TentativeDeliveryDate VARCHAR(50),--DATETIME,
	@IsAvailedFinance BIT,
	@IsBoughtAccessories BIT,
	@IsCarDelivered BIT,
	@DeliveryDate VARCHAR(50),
	@ExcelInquiryId BIGINT,
	@IsNewInq BIT OUTPUT,
	@IsUnassigned BIT=0 OUTPUT,
	@IsDiverted BIT OUTPUT,
	@INQLeadId BIGINT=NULL OUTPUT,
	-- Modified By : Tejashree Patil on 10 Feb 2014, Added parametrers IsSpecialUser,DealerCode for excel imported by NSC .
	@IsSpecialUser BIT = 0,
	@DealerCode VARCHAR(50) = NULL,
	@LastName VARCHAR(100) = NULL,
	@Salutation VARCHAR(10) = NULL
	
AS
BEGIN
	
	DECLARE @TC_CustomerId BIGINT,	@AutoVerified BIT = 1,			@TC_InquiryId BIGINT,				@LeadOwnerId BIGINT,
			@InqStatus SMALLINT ,	@LeadDivertedTo VARCHAR(100),	@LeadOwnId BIGINT,					@LeadIdOutput BIGINT,
			@TC_TDCalendarId BIGINT,@OldExcelInqId BIGINT,			@TC_AlternateInquiryId BIGINT=NULL, 
			@TDDate DATE=NULL
	
	SET		@IsNewInq=1 
	SET		@IsDiverted=0
	SET		@LeadOwnerId =@SalesConsultantId
	SET		@IsUnassigned=1	

	--------------blow condition added by Tejashree on 11-03-2014
	IF(@IsEligibleForCorporate IS NULL)
	BEGIN 
	SET @IsEligibleForCorporate=0
	END 
------------------------------------------------------------------------------


		
	IF(@Location IS NULL)
	BEGIN
		SELECT  @Location=Ct.Name,@CityId=Ct.ID
		FROM	Dealers DB
				LEFT JOIN Cities Ct WITH (NOLOCK) ON DB.CityId=Ct.ID
		WHERE	DB.IsTCDealer = 1 AND DB.ID=@BranchId
	END
	
	IF(@IsValid=1)
	BEGIN
		SET @TDDate = CONVERT(DATE,@TestDriveDate)	
			
		IF EXISTS(	SELECT 1 
					FROM	TC_InquiriesLead AS TCIL
							JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON C.Id=TCIL.TC_CustomerId
					WHERE	TCIL.TC_LeadInquiryTypeId=@InquiryType 
							AND TCIL.TC_LeadStageId<>3 	AND TCIL.TC_UserId IS NOT NULL	AND C.IsleadActive=1 AND C.Mobile=@Mobile AND C.BranchId=@BranchId)
						
		BEGIN
		
			--If exists then get LeadOwnerId	
		SELECT	@LeadOwnerId=TCIL.TC_UserId 
		FROM	TC_InquiriesLead AS TCIL
				JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON C.Id=TCIL.TC_CustomerId
		WHERE	TCIL.TC_LeadInquiryTypeId=@InquiryType 
				AND TCIL.TC_LeadStageId<>3 
				AND TCIL.TC_UserId IS NOT NULL
				AND C.IsleadActive=1
				AND C.Mobile=@Mobile
				AND C.BranchId=@BranchId
			
			SET @IsNewInq=0
			--SET @AutoVerified=1
		END
		
		IF (@LeadOwnerId != @SalesConsultantId)
		BEGIN
			SET @IsDiverted=1
		END
		
		SET	@IsUnassigned=1	
	END
		
	BEGIN
		IF (@InquiryType=3)
		BEGIN
			SET @OldExcelInqId = @ExcelInquiryId
			IF(@ExcelInquiryId IS NULL)
			BEGIN
				
				--1) Insert into dump table
				INSERT INTO TC_ExcelInquiries(Name,Email,Mobile,City,CarMake,CarModel,Isvalid,UserId,BranchId,TC_InquirySourceId,EntryDate, LeadOwnerId,
							ExcelSheetId,CityId,VersionId,Comments,	CarVersion, AlternateCarVersion, AlternateVersionId, SalesConsultant, SalesConsultantId, 
							InquirySource, InquiryDate, AlternateNo, CompanyName, Area, AreaId, /*CompanyId,*/ PreferedVersionColour, PreferedVersionColourIds, 
							AlternateModel, AlternateVersionColour, AlternateVersionColourIds, IsEligibleForCorporate, IsWillingnessToExchange, 
							IsFinanceRequired, IsAccessoriesRequired, IsSchemesOffered, IsTestDriveRequested, TestDriveDate, NextFollowUpDate, RecentComment, 
							RecentCommentDate, ActivityFeed,LostDispositionId,TDStatusId,IsLeadLost ,LeadLostReason  ,IsCustomerSuspended,IsCarBooked ,PanNo ,
							BookingAmount ,BookingReceiptNo ,TentativeDeliveryDate ,IsAvailedFinance ,IsBoughtAccessories,IsCarDelivered ,DeliveryDate,
							TDStatus, Eagerness,BuyingTime,BookingDate,IsSpecialUser,DealerCode,Salutation,LastName )-- Modified By : Tejashree Patil on 10 Feb 2014
				
				SELECT		@Name,@Email,@Mobile,@Location,@Make,@Model,@IsValid,@UserId,@BranchId,@SourceId,GETDATE(),@LeadOwnerId,
							@ExcelSheetId,@CityId,@PrefVersionId,@Comments,@Version, @AlternateVersion, @AlternateVersionId, @SalesConsultant, 
							@SalesConsultantId, @InquirySource, @InquiryDate, @AlternateNo, @CompanyName, @Area, @AreaId, /*@CompanyId,*/ @Color, 
							@PrefColorIds, @AlternateModel, @AlternateVersionColour, @AlternateColorIds, @IsEligibleForCorporate, @IsWillingnessToExchange, 
							@IsFinanceRequired, @IsAccessoriesRequired, @IsSchemesOffered, @IsTestDriveRequested, @TestDriveDate, 
							@NextFollowUpDate, @RecentComment, @RecentCommentDate, @ActivityFeed,@LostDispositionId ,@TDStatusId ,@IsLeadLost ,@LostReason ,
							@IsCustomerSuspended ,@IsCarBooked ,@PanNo ,@BookingAmount ,@BookingReceiptNo ,@TentativeDeliveryDate ,@IsAvailedFinance ,
							@IsBoughtAccessories,@IsCarDelivered ,@DeliveryDate ,@TDStatus,@Eagerness,@BuyingTime,@BookingDate,
							@IsSpecialUser,@DealerCode,@Salutation,@LastName-- Modified By : Tejashree Patil on 10 Feb 2014
						  
				WHERE NOT EXISTS (	SELECT	1 
									FROM	TC_ExcelInquiries 
									WHERE	Email =@Email AND Mobile =@Mobile AND (VersionId=@PrefVersionId AND AlternateVersionId=@AlternateVersionId) AND
											BranchId=@BranchId AND IsDeleted=0 AND TC_NewCarInquiriesId IS NULL)							
								
				SET @ExcelInquiryId = SCOPE_IDENTITY()
			
				--2) add other details in new table
				IF(@FirstCarModelOwned IS NOT NULL)
				BEGIN
					INSERT INTO TC_ExcelOtherDetails (TC_LeadId,Make,Model,Version,EntryDate,UserId,BranchId,PurchaseYear,TC_ExcelInquiryId)
					VALUES		(@LeadIdOutput,@Make,@FirstCarModelOwned,@FirstCarVersionOwned,GETDATE(),@UserId,@BranchId,@YearOfPurchase1,@ExcelInquiryId)
					
				END
				
				IF(@SecondCarModelOwned IS NOT NULL)
				BEGIN
					INSERT INTO TC_ExcelOtherDetails (TC_LeadId,Make,Model,Version,EntryDate,UserId,BranchId,PurchaseYear,TC_ExcelInquiryId)
					VALUES		(@LeadIdOutput,@Make,@SecondCarModelOwned,@SecondCarVersionOwned,GETDATE(),@UserId,@BranchId,@YearOfPurchase2,@ExcelInquiryId)					
				END
			END
			
			SET @ExcelInquiryId = ISNULL(@ExcelInquiryId,@OldExcelInqId)--in case of unassigned Scope_identity()=null
			--SET @InquiryDate=CONVERT(DATETIME,@InquiryDate)
			--SET @TestDriveDate=CONVERT(DATETIME,@TestDriveDate)
			--SET @DeliveryDate=CONVERT(DATETIME,@DeliveryDate)
			--SET @TentativeDeliveryDate=CONVERT(DATETIME,@TentativeDeliveryDate)

			--3)Insert into new car table with recent comment
			IF(@IsValid=1 AND @LeadOwnerId IS NOT NULL AND @LeadOwnerId <> -1)
			BEGIN	

			            -- Below condition added by Tehashree patil on 21-03-2014 for resolving bug related to insertion of special user in calls table
						IF (@IsSpecialUser=1)
						BEGIN

						SET @UserId=@LeadOwnerId

						END
						--------------------------------------------------------------------------------------------------------------------------------------


				--1) Insert inquiries in new car Inquiry table if it is valid
				DECLARE @IsNew BIT				
				IF(@PrefVersionId IS NOT NULL AND @PrefVersionId > 0)
				BEGIN
					EXEC TC_INQNewCarBuyerSave @Name,@Email,@Mobile,@PrefVersionId,@CityId ,@BuyingTime ,@SourceId ,@Eagerness ,NULL ,@AutoVerified ,
					@BranchId ,@LeadOwnerId ,@UserId ,@InqStatus  OUTPUT, NULL ,NULL ,NULL  ,NULL  ,NULL  ,NULL ,NULL ,@TC_InquiryId  OUTPUT,
					@LeadDivertedTo OUTPUT,	@IsNew OUTPUT,@SourceId,@ExcelInquiryId ,@LeadOwnId OUTPUT,@TC_CustomerId OUTPUT,
					@LeadIdOutput OUTPUT,@INQLeadId OUTPUT,@Comments,@IsEligibleForCorporate,@CompanyName,@Address,@PrefColorIds,@InquiryDate,
					@IsWillingnessToExchange,	@IsFinanceRequired,	@IsAccessoriesRequired,	@IsSchemesOffered,@Salutation,
					@LastName , NULL, NULL, NULL, NULL, NULL, NULL-- Modified By : Tejashree Patil on 10 Feb 2014
				
					--3) If @AlternateVersionId is not null then Insert new inquiry
					IF(@AlternateVersionId IS NOT NULL AND @AlternateVersionId > 0)
					BEGIN
						EXEC TC_INQNewCarBuyerSave @Name,@Email,@Mobile,@AlternateVersionId,@CityId ,@BuyingTime ,@SourceId ,@Eagerness ,NULL ,@AutoVerified ,
						@BranchId ,@LeadOwnerId ,@UserId ,@InqStatus  OUTPUT, NULL ,NULL ,NULL  ,NULL  ,NULL  ,NULL ,NULL ,@TC_AlternateInquiryId  OUTPUT,
						@LeadDivertedTo OUTPUT,	@IsNew OUTPUT,@SourceId,@ExcelInquiryId ,@LeadOwnId OUTPUT,@TC_CustomerId OUTPUT,
						@LeadIdOutput OUTPUT,@INQLeadId OUTPUT,@Comments,@IsEligibleForCorporate,@CompanyName,@Address,
						@AlternateColorIds,	@InquiryDate, @IsWillingnessToExchange,	@IsFinanceRequired,	@IsAccessoriesRequired,	@IsSchemesOffered,@Salutation,
						@LastName , NULL, NULL, NULL, NULL, NULL, NULL-- Modified By : Tejashree Patil on 10 Feb 2014
					END					
					
					SET @IsUnassigned=0
				END
				----------------------------------------------------------------------------------------------------------------------------------------
				--2) add td if requested
					
				IF(@IsTestDriveRequested = 1)
				BEGIN
					DECLARE @TDStartTime TIME = NULL,	@TDEndTime TIME = NULL,	@TDCarDetails VARCHAR(100)= @Make +' '+@Model+' '+@Version,
							@Status TINYINT, @EventCreatedOn DATE=@InquiryDate
									
					IF(@TDStatusId=-1 OR @TDStatusId IS NULL)
						SET @TDStatusId = 39
					
					SET @TDStartTime ='07:30:00'
					SET @TDEndTime ='08:00:00'
						
					IF(@IsCompleteTD=1)
					BEGIN
						SET @EventCreatedOn=@TDDate
					END
										
					EXEC TC_TDBookingsSave	@BranchId, @TC_CustomerId, @TC_InquiryId, @Address, @Area, @AreaId, @PrefVersionId, @TDCarDetails , 
											@TDDate, @TDStartTime, @TDEndTime, @LeadOwnerId,	@LeadOwnerId,	-1, @Status OUTPUT,  
											@TDStatusId , @SourceId, @TC_TDCalendarId OUTPUT, @IsCompleteTD, 1, @EventCreatedOn
					
				END
				----------------------------------------------------------------------------------------------------------------------------------------
				--3)Inquiry booking if requested
				IF(@IsCarBooked = 1)-- Modified By : Tejashree Patil on 10 Jan 2014
				BEGIN
					/*								 
					EXEC TC_INQNewCarBookingSaveTEST @TC_InquiryId ,NULL ,@BookingAmount ,NULL ,@TC_CustomerId ,@IsFinanceRequired ,NULL ,@UserId , @BranchId ,
													 @PanNo,@BookingAmount , @BookingReceiptNo, NULL, NULL,  NULL ,@BookingDate,  @Address,  @Name ,  
													 NULL,  @Mobile,  @Email, NULL , NULL,  NULL, @AlternateNo , @DeliveryDate ,  @TentativeDeliveryDate

					*/
					EXEC	TC_INQNewCarBookingSave @TC_InquiryId , @Address, @BookingAmount ,  @BookingAmount,  0 ,@TC_CustomerId ,  0 ,  
							NULL,  @UserId,   @BranchId ,@BookingDate, @Name, @Mobile, @DeliveryDate,NULL , NULL, NULL,NULL 
							    
					UPDATE		TC_NewCarBooking 
					SET         PanNo=@PanNo, Email=@Email,	PrefDeliveryDate=@TentativeDeliveryDate		
					WHERE		TC_NewCarInquiriesId=@TC_InquiryId
		
					DECLARE @TC_NewCarBookingId INT = NULL

					SELECT	@TC_NewCarBookingId = TC_NewCarBookingId 
					FROM	TC_NewCarBooking WITH(NOLOCK)
					WHERE	TC_NewCarInquiriesId = @TC_InquiryId
		 
					UPDATE       TC_NewCarPaymentDetails
					SET          BookingAmount=@BookingAmount,
								 ReceiptNo=@BookingReceiptNo,PaymentDate=@BookingDate
					WHERE        TC_NewCarBookingId=@TC_NewCarBookingId
		
	 
				END
				
				----------------------------------------------------------------------------------------------------------------------------------------
				--4)Inquiry disposition
				IF(@IsLeadLost = 1)
				BEGIN
					EXEC TC_changeInquiryDisposition @TC_InquiryId , @LostDispositionId , 3 , @UserId , @NextFollowUpDate
					IF(@TC_AlternateInquiryId IS NULL)--Modified By : Tejashree Patil on 10 Jan 2014 removed IS NOT NULL
					BEGIN
						EXEC TC_changeInquiryDisposition @TC_AlternateInquiryId , @LostDispositionId , 3 , @UserId ,@NextFollowUpDate
					END
				END
				----------------------------------------------------------------------------------------------------------------------------------------
				--5)Followup
				
				DECLARE		@WhileLoopControl INT=1
				DECLARE		@WhileLoopCount INT
				SELECT		@WhileLoopCount=COUNT(Id) FROM @TC_FollowUpDetailsFromExcel
				DECLARE		@ActivityComments VARCHAR(500)=NULL,	@Date DATETIME=@InquiryDate
				
				IF(@WhileLoopCount>1)
				BEGIN
				
					WHILE (@WhileLoopControl<=@WhileLoopCount)
					BEGIN
						SELECT	@ActivityComments=Comment,	
								@Date=ActionTakenOnDate 
						FROM	@TC_FollowUpDetailsFromExcel 
						WHERE	Id=@WhileLoopControl
						
						EXEC	TC_CallScheduling	@LeadIdOutput,@UserId, NULL ,@ActivityComments, @NextFollowUpDate, NULL ,@LeadOwnerId, @Date
													
						SET		@WhileLoopControl=@WhileLoopControl+1

					END
					--EXEC	TC_CallScheduling	@LeadIdOutput,@UserId, NULL ,NULL, @NextFollowUpDate, NULL ,@@LeadOwnerId, @Date
				END
				ELSE
				BEGIN
					EXEC	TC_CallScheduling	@LeadIdOutput,@UserId, NULL ,@RecentComment, @NextFollowUpDate, NULL ,@LeadOwnerId, @Date
				END
				
				---------------------------------------------------------------------------------------------------------------------------------------
				--6)Update InquiryDate,TC_LeadId in TC_ExcelOtherDetails
				UPDATE	TC_ExcelOtherDetails
				SET		InquiryDate=@InquiryDate,TC_LeadId=@LeadIdOutput
				WHERE	TC_ExcelInquiryId=@ExcelInquiryId
				
				---------------------------------------------------------------------------------------------------------------------------------------
				--7)
				IF(	@TDStatusId = 27)
				BEGIN	
					EXEC TC_TDStatusChange @BranchId,@TC_TDCalendarId,27,@InquiryDate 
				END	
				----------------------------------------------------------------------------------------------------------------------------------------
				IF(@IsCarDelivered = 1)
				BEGIN
					EXEC TC_InsertDeliveryDetails	@BranchId ,@UserId ,@LeadOwnerId ,@TC_InquiryId,3, @PanNo,NULL,NULL,NULL,NULL,@DeliveryDate
				END
			END			
		END
	END	
	
END


