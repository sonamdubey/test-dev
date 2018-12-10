IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_NewCarCWOfferInquirySave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_NewCarCWOfferInquirySave]
GO

	-- Created By:	Tejashree Patil
-- Create date: 25 NOV 2014
-- Description:	Adding New Car Buyer Inquiry with CW Offer from carwale PQ page.
-- Author:	Tejashree Patil on 5 Dec 2014 , Assigned all inquiries of offer to Dealer Principle.
-- =============================================
CREATE  PROCEDURE [dbo].[TC_NewCarCWOfferInquirySave] 
(
	@CustomerName VARCHAR(50)=NULL,
	@CustomerEmail VARCHAR(50)=NULL,
	@CustomerMobile VARCHAR(10)=NULL,
	@VersionId INT,
	@CityId INT,   
	@Buytime VARCHAR(20)=NULL,
	@InquirySource TINYINT=NULL,
	@Eagerness SMALLINT=NULL,
	@AutoVerified BIT,
	@BranchId BIGINT,
	@LeadOwnerId BIGINT,
	@CreatedBy BIGINT,
	@Status SMALLINT OUTPUT,
	@PQReqDate DATETIME=NULL,-- to identify request came for price quote
	@TDReqDate DATETIME=NULL,-- to identify request came for Test drive
	@ModelId INT =NULL,
	@FuelType SMALLINT = NULL,
	@Transmission TINYINT = NULL,
	@CWInquiryId BIGINT = NULL,
	@TC_NewCarInquiryId BIGINT OUTPUT,
	@LeadDivertedTo VARCHAR(100) OUTPUT	,
	@IsNewInq BIT = NULL OUTPUT,
	@LeadOwnId BIGINT = NULL OUTPUT,
	@CustomerId BIGINT = -1 OUTPUT,
	@LeadIdOutput BIGINT = NULL OUTPUT,
	@INQLeadIdOutput BIGINT = NULL OUTPUT,
	@Comments VARCHAR(5000) = NULL,
    @Address VARCHAR(200) =NULL,
	@Salutation VARCHAR(15)=NULL,
	@LastName VARCHAR(100)=NULL,
	@RequestType SMALLINT = NULL,
	@ApplicationId TINYINT = NULL,
	@CwOfferId INT = NULL,
	@CouponCode VARCHAR(10) = NULL
)
AS           

BEGIN	
SET NOCOUNT ON;	
	BEGIN TRY
		--BEGIN TRANSACTION ProcessBuyerInquiries
					
			DECLARE @CustStatus SMALLINT
			DECLARE @ActiveLeadId BIGINT

			DECLARE @BookingRequest BIT = NULL
			DECLARE @BookingRequestDate DATETIME = NULL
			DECLARE @AppointmentRequest BIT = NULL
			DECLARE @AppointmentRequestDate DATETIME = NULL
			
			SET @Status=0
			
			IF(@RequestType = 3)
				BEGIN
				  SET @BookingRequest = 1
				  SET @BookingRequestDate = GETDATE()
				END

			IF(@RequestType = 4)
			BEGIN
				SET @AppointmentRequest = 1
				SET @AppointmentRequestDate = GETDATE()
			END

			EXEC TC_CustomerDetailSave @BranchId=@BranchId,@CustomerEmail=@CustomerEmail,@CustomerName=@CustomerName,@CustomerMobile=@CustomerMobile,
				@Location=NULL,@Buytime=@Buytime,@Comments=NULL,@CreatedBy=@CreatedBy,@Address=@Address,@SourceId=@InquirySource,
				@CustomerId=@CustomerId OUTPUT,@Status=@CustStatus OUTPUT,@ActiveLeadId=@ActiveLeadId OUTPUT,@CW_CustomerId=NULL,
				@Salutation =@Salutation,@LastName=@LastName, 
				@TC_CampaignSchedulingId=NULL,
				@CustomerAltMob=NULL
								
			IF(@CustStatus=1) --Customer Is Fake
			BEGIN
				SET @Status=99	
				SET @TC_NewCarInquiryId = -1	
			END
			ELSE
			BEGIN	
			
				IF(@InquirySource=57)--Come from Mobile Masking 
				BEGIN					
					SET @InquirySource=6--make Mobile Masking source = CarWale source					
				END
				
				IF(@InquirySource=60)--Come from Mobile Masking  CarWale Knowlarity Leads
				BEGIN					
					SET @InquirySource=1--make Mobile Masking source = CarWale source					
				END
				
				DECLARE @CarDetails VARCHAR(MAX)
				
				DECLARE @INQDate DATETIME=GETDATE()
				
				IF(@VersionId IS NULL ) -- This inquiry is added form dealer wbesite of TD
				BEGIN						
					SELECT TOP 1 @VersionId=V.ID FROM CarVersions V WITH(NOLOCK) 
					WHERE V.CarModelId=@ModelId AND V.IsDeleted=0 AND V.New=1 
					AND V.Futuristic=0
				END
					
				SELECT	@CarDetails=V.Make + ' ' + V.Model + ' '  + V.Version + ' '  
				FROM	vwAllMMV V
				WHERE	V.VersionId=@VersionId AND V.ApplicationId = ISNULL(@ApplicationId,1)
																		
				EXECUTE TC_INQLeadSave @AutoVerified=@AutoVerified,
						@BranchId =@BranchId,
						@CustomerId =@CustomerId,
						@LeadOwnerId=@LeadOwnerId,
						@Eagerness =@Eagerness,
						@CreatedBy =@CreatedBy,
						@InquirySource=@InquirySource,
						@LeadId =@ActiveLeadId,
						@INQDate=@INQDate,
						@LeadInqTypeId =3,
						@CarDetails =@CarDetails,
						@LeadStage =NULL,
						@LeadIdOutput= @LeadIdOutput OUTPUT,
						@INQLeadIdOutput= @INQLeadIdOutput OUTPUT,
						@NextFollowupDate=NULL,
						@FollowupComments =NULL,
						@ReturnStatus=@Status OUTPUT,
						@LeadDivertedTo=@LeadDivertedTo OUTPUT	,
						@LeadOwnId = @LeadOwnId OUTPUT	,
						@TC_CampaignSchedulingId = NULL
						
				DECLARE @PQStatus TINYINT
				DECLARE @TDStatus TINYINT
				
				
				IF(@PQReqDate IS NOT NULL)
				BEGIN
					SET @PQStatus =23
				END
				
				IF(@TDReqDate IS NOT NULL)
				BEGIN
					SET @TDStatus =26
				END				
				
				-- Inserting record in Buyer Inquiries
				IF NOT EXISTS(	SELECT TOP 1 TC_InquiriesLeadId FROM 	TC_NewCarInquiries WITH(NOLOCK)
								WHERE TC_InquiriesLeadId =@INQLeadIdOutput AND VersionId=@VersionId 
								AND TC_LeadDispositionId IS NULL
								AND (@CouponCode IS NULL OR CouponCode = @CouponCode))--Modified By: Tejashree Patil on 21 Nov 2014
				BEGIN				
					
					INSERT INTO TC_NewCarInquiries( TC_InquiriesLeadId, TC_InquirySourceId, CreatedOn, CreatedBy, 
													VersionId, Buytime, BuyDate, CityId,PQRequestedDate,TDRequestedDate,FuelType,TransmissionType,PQStatus,TDStatus,
													Comments, IsCorporate, CompanyName,IsWillingnessToExchange,
													IsFinanceRequired,	IsAccessoriesRequired ,IsSchemesOffered, ExcelInquiryId,
													IsExchange,TC_CampaignSchedulingId, -- Modified By : Vivek Gupta on 20-09-2013
													TC_NewCarExchangeId,BookingRequest,BookingRequestDate,AppointmentRequest,AppointmentRequestDate, -- Modified By : Vishal  on 07-04-2014
													CWInquiryId,CwOfferId, CouponCode)--Modified By: Tejashree Patil on 21 Nov 2014
					VALUES (@INQLeadIdOutput, @InquirySource, @INQDate, @CreatedBy, @VersionId, @Buytime,
							CAST(DATEADD(DAY,CONVERT(INT,@Buytime),GETDATE())AS DATETIME), @CityId,@PQReqDate,@TDReqDate,@FuelType,@Transmission,@PQStatus,@TDStatus,
							@Comments, 0, NULL, NULL ,	0 ,
							0 ,NULL, NULL, NULL,--  -- Modified By : Vishal  on 07-04-2014 since no use of column IsExchange from now.
							NULL,0,@BookingRequest,@BookingRequestDate,@AppointmentRequest,@AppointmentRequestDate,-- Modified By : Vivek Gupta on 20-09-2013
							@CWInquiryId,@CwOfferId, @CouponCode)--Modified By: Tejashree Patil on 21 Nov 2014
							
					--SET @Status=1
					SET @TC_NewCarInquiryId = SCOPE_IDENTITY()					
					
					IF(@VersionId IS NOT NULL AND EXISTS (SELECT ID FROM Dealers WITH (NOLOCK) WHERE ID = @BranchId AND DealerCode IS NOT NULL AND TC_BrandZoneId IS NOT NULL))
					BEGIN
					   EXEC TC_NSCCampaignSave @CityId,@INQDate,@VersionId,@TC_NewCarInquiryId
					END
				END
				ELSE
				BEGIN
					IF(@TDReqDate IS NOT NULL OR @PQReqDate IS NOT NULL) 
					BEGIN --Update is required to identify request came for PQ or TD only if 
						UPDATE	TC_NewCarInquiries 
						SET		PQRequestedDate =ISNULL(@PQReqDate,PQRequestedDate),PQStatus=ISNULL(@PQStatus,PQStatus),
								TDRequestedDate=ISNULL(@TDReqDate,TDRequestedDate),TDStatus=ISNULL(@TDStatus,TDStatus),
								Comments =@Comments
						WHERE	TC_InquiriesLeadId =@INQLeadIdOutput AND VersionId=@VersionId 
								AND TC_LeadDispositionId IS NULL
					END

					IF(@BookingRequest IS NOT NULL OR @AppointmentRequest IS NOT NULL) 
					BEGIN --Update is required to identify request came for PQ or TD only if 
						UPDATE	TC_NewCarInquiries 
						SET		BookingRequest = @BookingRequest,
								BookingRequestDate = @BookingRequestDate,
								AppointmentRequest=@AppointmentRequest,
								AppointmentRequestDate = @AppointmentRequestDate

						WHERE	TC_InquiriesLeadId =@INQLeadIdOutput AND VersionId=@VersionId 
								AND TC_LeadDispositionId IS NULL
					END
											
					--SET @Status=1
								
					SELECT	@TC_NewCarInquiryId=TC_NewCarInquiriesId 
					FROM	TC_NewCarInquiries WITH(NOLOCK)
					WHERE	TC_InquiriesLeadId =@INQLeadIdOutput AND VersionId=@VersionId 
							AND TC_LeadDispositionId IS NULL
							AND (@CouponCode IS NULL OR CouponCode = @CouponCode)	--Modified By: Tejashree Patil on 21 Nov 2014				
				
				END				
				
				IF(@ActiveLeadId IS NOT NULL)
				BEGIN
					SET @IsNewInq = 0
				END	
				ELSE
				BEGIN
					SET @IsNewInq = 1
				END	
									
			-- Author:	Tejashree Patil on 5 Dec 2014 , Assigned all inquiries of offer to Dealer Principle.
				SELECT	TOP 1 @LeadOwnerId=U.Id 
				FROM	TC_Users U WITH(NOLOCK) 
				WHERE	U.BranchId=@BranchId 
						AND IsActive = 1 
						AND IsCarwaleUser = 0 
				ORDER BY U.Id
				
				EXECUTE TC_INQUnassignedLeadAssignment
												@BranchId		=	@BranchId,
												@UserID			=	@LeadOwnerId, 
												@InqLeadIds		=	@LeadIdOutput,
												@ModifiedBy		=	@LeadOwnerId					
			END	
	--	COMMIT TRANSACTION ProcessBuyerInquiries
	END TRY
	
	BEGIN CATCH
		--ROLLBACK TRANSACTION ProcessBuyerInquiries
		 INSERT INTO TC_Exceptions
                      (Programme_Name,
                       TC_Exception,
                       TC_Exception_Date,
                       InputParameters)
         VALUES('TC_NewCarInqSaveWithCWOffer',
         (ERROR_MESSAGE()+'ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),
         GETDATE(),
          ' @CustomerName:' + ISNULL(@CustomerName,'NULL') + 
         ' @CustomerEmail :'+ ISNULL(@CustomerEmail,'NULL') + 
         ' @CustomerMobile : '+	ISNULL(@CustomerMobile,'NULL')  +
         ' @VersionId : '+ISNULL(CAST( @VersionId AS VARCHAR(50)),'NULL')+
         ' @CityId: ' + ISNULL(CAST( @CityId AS VARCHAR(50)),'NULL')+
         ' @Buytime: '+  ISNULL(@Buytime,'NULL') +
         ' @InquirySource : ' +ISNULL(CAST( @InquirySource AS VARCHAR(50)),'NULL')+
         ' @Eagerness: ' + ISNULL(CAST(@Eagerness AS VARCHAR(50)),'NULL')+
         ' @AutoVerified : ' +ISNULL(CAST( @AutoVerified AS VARCHAR(5)),'NULL')+
         ' @BranchId : '+ISNULL(CAST( @BranchId AS VARCHAR(50)),'NULL')+
         ' @LeadOwnerId: ' + ISNULL(CAST( @LeadOwnerId AS VARCHAR(50)),'NULL')+
         ' @CreatedBy: '+ ISNULL(CAST( @CreatedBy AS VARCHAR(50)),'NULL')+
         ' @PQReqDate: ' + ISNULL(CAST( @PQReqDate AS VARCHAR(50)),'NULL')+	
         ' @TDReqDate: '+ISNULL(CAST( @TDReqDate AS VARCHAR(50)),'NULL')+
         ' @ModelId: ' +ISNULL(CAST( @ModelId AS VARCHAR(50)),'NULL')+
         ' @FuelType : '+ISNULL(CAST( @FuelType AS VARCHAR(50)),'NULL')+	
         ' @Transmission : '+ISNULL(CAST( @Transmission AS VARCHAR(50)),'NULL')+
         ' @CWInquiryId: '+ISNULL(CAST( @CWInquiryId AS VARCHAR(50)),'NULL')+
         ' @Comments: '+ISNULL(@Comments,'NULL')+
         ' @Address: '+ISNULL(CAST( @Address AS VARCHAR(200)),'NULL') +
		  '@RequestType: '+ISNULL(CAST( @RequestType AS VARCHAR(5)),'NULL') +
		  '@CwOfferId: '+ISNULL(CAST( @CwOfferId AS VARCHAR(50)),'NULL') +--Modified By: Tejashree Patil on 21 Nov 2014
		  '@CouponCode: '+ISNULL(@CouponCode,'NULL') --Modified By: Tejashree Patil on 21 Nov 2014
         )
	END CATCH;
END

