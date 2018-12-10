IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQNewCarBuyerSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQNewCarBuyerSave]
GO

	-- Created By:	Tejashree Patil
-- Create date: 9th Jan 2013
-- Description:	Adding New Car Buyer Inquiry
-- Modified By : Surendra on 05 march 2013 modify insert exception statement
-- Modified By : Tejashree 22 April 2013, Set @TC_NewCarInquiryId=-1.
-- Modified by : Tejashree Patil on 10 May 2013, Automaticaaly assign all inquiries to single user instead of scheduling.
-- Modified By : Tejashree 17/5/2013 Added output parameter @TC_ExcelInquiriesId and Update query for TC_ExcelInquiries
-- Modified By: Vivek Gupta on 20th May , 2013 Added Three OUtput Parameters @LeadOwnId,@CustomerId,@LeadIdOutput
-- Modified By : Tejashree Patil on 3 Jun 2013, Added @Comments field specailly used by MobileMasking and change source of 60(CarWale Knowlarity Leads) to 1(carwale).
-- Modified By : Nilesh Utture on 7th Jun 2013, Added parameters @IsCorporate, @CompanyName, @Address, @ColorList
-- Modified By : Tejashree 19 Jun 2013, Added ISNULL() While inserting in TC_Exceptions .
-- Modified By : Tejashree 5 July 2013, Added select condition to update TC_NewCarInquiryId in TC_ExcelInquiries, @CreatedOn,@IsWillingnessToExchange,
-- @IsFinanceRequired ,@IsAccessoriesRequired ,@IsSchemesOffered  parameter.
-- Modified By : Tejashree 14 Aug 2013, @CustomerId is used as InputOutput parameter instead of input only and added @IsDplicacyMerged.
-- Modified By : Tejashree Patil on 27 Aug 2013, Added parameter salutation, lastName of customer while adding inquiry.
-- Modified By : Vivek Gupta on 20-09-2013, Added 5 parameters @IsExchange,@ExchangeVersionId,@MakeYear,@Kilometers,@ExpectedPrice
-- Modified By : Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampaignSchedulingId while adding inquiry.
-- Modified By : Vivek Gupta on 25-10-2013, Changes done for adding NSC campaign automatically while adding new car inquiry
-- Modified by : Nilesh Utture on 18th Dec, 2013, Push all inquiries to user even if he is having any sales exec. role	
-- Modified by : Nilesh Utture on 18th Feb, 2014, added DISTINCT UserId condition while assigning inquireis to single user dealer
-- Modified By : Vishal Srivastava on 28 Mar 2014 , added an option for ALtername mobile number AND EXCHNAGE OPTION
-- Modified By Vivek Gupta on 12-05-2014, Added conditions for adding campaign automaticlly
-- Modified By Vivek Gupta on 30th june, Added parameter @RequestType to determine the type of Lead coming
-- Modified By : Tejashree Patil on 04-07-2014 for capturing RequestType parametre in exceptions.
-- Modified By : Tejashree Patil on 30-07-2014 LEFT JOIN instead of INNER JOIN TC_ExcelInquiries.
-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application.
-- Modified By Vivek Gupta on 7-05-2015, removed new condition if versionid is null
-- Modified By Vicky Gupta on 03-08-2015, pass  a parameter VersionId to TC_INQLeadSave
-- Modified By : Afrose on 24-08-2015, added parameter CampaignId
-- Modified By Vivek Gupta on 29-09-2015 ,  making latest inquiry as most interested inquiry
-- EXEC [TC_INQNewCarBuyerSave] 'ABC',null,9999995895,70,10,null,null,null,null
-- Modified By : Tejashree Patil on 30-11-2015, Fetched default version for make mobile masking inquiry.
-- Modified by : Vicky Gupta on 7/1/15 , handled new car inquiry for aged cars(deals)
-- Modified By : Ashwini Dhamankar on Jan 13,2015 (Fetched color and make year of deals stock)
-- Modified By : Ashwini Dhamankar on May 12,2016 (Added InquirySourceId 147 and 148 for deals)
-- Modified By : Nilima More On 13th May 2016,Save IsPaymentSuccess field in tc_newCarInquiries table.
-- Modified By : Nilima More On 23rd May 2016,Changed booking Amount from 2999 to 999.
-- Modified By : Ashwini Dhamankar on June 1,2016 (update existing inquiry of deals for sources 147 and 148 instead of creating new inq according to ispayment flag)
-- Modified By : Suresh Prajapati on 28th July, 2016
-- Description : Modified lead assignment for Advantage leads
-- Modified By : Mihir A Chheda [01-08-2016] fetch version id if version id is null
-- Modified By : Deepak Tripathi [26-09-2016] made @LeadInqTypeId dynamic.
-- Modified by : Tejashree Patil on 28 sept 2016,Fetched dealers CityId when @CityId is <= 0
-- Modified by : Tejashree Patil on 4 Nov 2016, Added parameter @MaskingNumber and saved in TC_NewCarInquiries table.
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQNewCarBuyerSave] (
	-- Customer's contact details
	@CustomerName VARCHAR(50) = NULL
	,@CustomerEmail VARCHAR(50) = NULL
	,@CustomerMobile VARCHAR(10) = NULL
	,
	--  Other Details
	@VersionId INT
	,@CityId INT
	,@Buytime VARCHAR(20) = NULL
	,@InquirySource TINYINT = NULL
	,@Eagerness SMALLINT = NULL
	,@TC_CustomerId INT = NULL
	,@AutoVerified BIT
	,@BranchId INT
	,@LeadOwnerId INT
	,@CreatedBy INT
	,@Status SMALLINT OUTPUT
	,
	-- these paremeters will be used in case of dealer website
	@PQReqDate DATETIME = NULL
	,-- to identify request came for price quote
	@TDReqDate DATETIME = NULL
	,-- to identify request came for Test drive
	@ModelId INT = NULL
	,@FuelType SMALLINT = NULL
	,@Transmission TINYINT = NULL
	,@CW_CustomerId INT = NULL
	,@CWInquiryId INT = NULL
	,@TC_NewCarInquiryId INT OUTPUT
	,@LeadDivertedTo VARCHAR(100) OUTPUT
	,@IsNewInq BIT = NULL OUTPUT
	,@InquiryOtherSourceId TINYINT = NULL
	,@ExcelInquiryId INT = NULL
	,@LeadOwnId INT = NULL OUTPUT
	,@CustomerId INT = - 1 OUTPUT
	,
	--@CustomerId BIGINT = NULL OUTPUT,
	@LeadIdOutput INT = NULL OUTPUT
	,@INQLeadIdOutput INT = NULL OUTPUT
	,@Comments VARCHAR(5000) = NULL
	,@IsCorporate BIT = 0
	,@CompanyName VARCHAR(200) = NULL
	,@Address VARCHAR(200) = NULL
	,@ColorList VARCHAR(200) = NULL
	,@CreatedOn DATETIME = NULL
	,@IsWillingnessToExchange BIT = NULL
	,@IsFinanceRequired BIT = NULL
	,@IsAccessoriesRequired BIT = NULL
	,@IsSchemesOffered BIT = NULL
	,
	--@IsDuplicacyMerged BIT=0 -- Modified By : Tejashree 14 Aug 2013,
	@Salutation VARCHAR(15) = NULL
	,@LastName VARCHAR(100) = NULL
	,
	-- Modified By : Vivek Gupta on 20-09-2013
	@IsExchange TINYINT = NULL
	,@ExchangeVersionId INT = NULL
	,@MakeYear DATETIME = NULL
	,@Kilometers INT = NULL
	,@ExpectedPrice INT = NULL
	,@TC_CampaignSchedulingId INT = NULL
	,-- Modified By : Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampaignSchedulingId while adding inquiry.
	@CustomerAltMob VARCHAR(15) = NULL
	,-- Vishal Srivastava on 28 Mar 2014 , added an option for ALtername mobile number
	@RequestType SMALLINT = NULL
	,--1=pricequoterequest,2=testdriverequest,3=bookingrequest,4=appointmentrequest
	@ApplicationId TINYINT = NULL
	,--Added @ApplicationId to identify application.
	@CampaignId INT = NULL
	,@DealsStockId INT = NULL
	,@IsPaymentSuccess BIT = NULL
	,@MaskingNumber VARCHAR(15) = NULL
	,@SalesExMobileNo VARCHAR(15) = NULL
	)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @LeadInqTypeId TINYINT = 3
	IF @InquirySource IN (134,140,146,147,148)
		SET @LeadInqTypeId = 5
	
	--Added by Tejashree Patil on 28 sept 2016
	IF(ISNULL(@CityId,0) <= 0)
	BEGIN
		SELECT	@CityId = CityId
		FROM	Dealers WITH(NOLOCK)
		WHERE	Id = @BranchId
	END

	--Mihir A Chheda [01-08-2016] fetch version id if version id is null
    IF(@VersionId IS NULL AND @DealsStockId IS NOT NULL)
	BEGIN
	     SELECT @VersionId=CarVersionId FROM TC_Deals_Stock(NOLOCK) WHERE id=@DealsStockId
	END
	IF ISNULL(@BranchId, 0) <= 0
		RETURN 0

	-- inserting record in main table (TC_inquiries) of inquiries
	BEGIN TRY
		BEGIN TRANSACTION ProcessNewCarBuyerInquiries

		--Registering/Updating Customer
		--DECLARE @CustomerId BIGINT
		DECLARE @IsUpdateCampaignLeadCount BIT = 0
		DECLARE @CustStatus SMALLINT
		DECLARE @ActiveLeadId INT --, @Status SMALLINT
		DECLARE @BookingRequest BIT = NULL
		DECLARE @BookingRequestDate DATETIME = NULL
		DECLARE @AppointmentRequest BIT = NULL
		DECLARE @AppointmentRequestDate DATETIME = NULL
		DECLARE @TC_Deals_StockVINId INT = NULL
		DECLARE @TC_Deals_StockVINNo VARCHAR(20) = NULL
		DECLARE @TC_DealsStockPrice INT = NULL
		DECLARE @DealsMakeYear VARCHAR(10) = NULL
		DECLARE @DealsCarColorId INT = NULL
		DECLARE @NextCallTo INT = NULL --Added by Ashwini Dhamankar on Jan 15,2015
			--DECLARE @BookingAmountDeals INT = 2999 -- Added By Deepak on 18th Jan
		DECLARE @BookingAmountDeals INT = 999 -- Modified By : Nilima More On 23rd May 2016,Changed booking Amount from 2999 to 999.
			-- Modified By Suresh Prajapati on 27th jan, 2016 and changed booking price to 2999

		SET @Status = 0

		IF (@RequestType = 3)
		BEGIN
			SET @BookingRequest = 1
			SET @BookingRequestDate = GETDATE()
		END

		IF (@RequestType = 4)
		BEGIN
			SET @AppointmentRequest = 1
			SET @AppointmentRequestDate = GETDATE()
		END

		--below id condition runs when inquiry comes for deals(aged cars)
		IF (
				@DealsStockId IS NOT NULL
				AND @InquirySource IN (
					134
					,147
					,148
					)
				AND @IsPaymentSuccess = 1
				)
		BEGIN
			SELECT TOP 1 @TC_Deals_StockVINId = TDV.TC_DealsStockVINId
				,@TC_Deals_StockVINNo = TDV.VINNo
				,@DealsMakeYear = YEAR(TDS.MakeYear)
				,@DealsCarColorId = TDS.VersionColorId
			FROM TC_Deals_Stock TDS WITH (NOLOCK)
			INNER JOIN TC_Deals_StockVIN TDV WITH (NOLOCK) ON TDS.Id = TDV.TC_Deals_StockId
				AND TDV.[Status] = 2
				AND ISNULL(TDS.isApproved, 0) = 1
			WHERE TDS.Id = @DealsStockId
			ORDER BY TDV.TC_DealsStockVINId

			SET @NextCallTo = 1

			IF (@TC_Deals_StockVINId IS NULL)
			BEGIN
				ROLLBACK TRANSACTION ProcessNewCarBuyerInquiries

				RETURN 0
			END
					--SET @AutoVerified = 1
		END
		ELSE
			IF (
					@DealsStockId IS NOT NULL
					AND @InquirySource IN (134)
					AND @IsPaymentSuccess = 0
					)
			BEGIN
				ROLLBACK TRANSACTION ProcessNewCarBuyerInquiries

				RETURN 0
			END

		IF (
				@CreatedBy IS NULL
				AND @LeadOwnId IS NULL
				AND @InquirySource IN (
					134
					,140
					,146
					,147
					,148
					)
				)
		BEGIN
			IF EXISTS (
					SELECT 1
					FROM TC_MappingDealerFeatures WITH(NOLOCK)
					WHERE BranchId = @BranchId
						AND TC_DealerFeatureId = 8
					) --Check if Prevent Automatic Lead Flow For Sales Excecutive is enabled
			BEGIN -- if yes then assign lead to DP
				SET @CreatedBy = (
						SELECT TOP 1 U.Id
						FROM TC_UsersRole UR WITH (NOLOCK)
						JOIN TC_Users U WITH (NOLOCK) ON U.Id = UR.UserId
							AND U.IsActive = 1
							AND UR.RoleId = 1 -- DP
							AND U.BranchId = @BranchId
						ORDER BY UR.RoleId DESC
						)
			END
			ELSE
			BEGIN
				-- Else Assign to Sales Exec OR DP
				SET @CreatedBy = (
						SELECT TOP 1 U.Id
						FROM TC_UsersRole UR WITH (NOLOCK)
						JOIN TC_Users U WITH (NOLOCK) ON U.Id = UR.UserId
							AND U.IsActive = 1
							AND UR.RoleId IN (
								1,20
								)
							AND U.BranchId = @BranchId
						ORDER BY UR.RoleId DESC
						)
			END

			SET @LeadOwnerId = @CreatedBy
		END

		-- Modified By : Nilesh Utture on 7th Jun 2013, Passed @Address to this SP
		EXEC TC_CustomerDetailSave @BranchId = @BranchId
			,@CustomerEmail = @CustomerEmail
			,@CustomerName = @CustomerName
			,@CustomerMobile = @CustomerMobile
			,@Location = NULL
			,@Buytime = @Buytime
			,@Comments = NULL
			,@CreatedBy = @CreatedBy
			,@Address = @Address
			,@SourceId = @InquirySource
			,@CustomerId = @CustomerId OUTPUT
			,@Status = @CustStatus OUTPUT
			,@ActiveLeadId = @ActiveLeadId OUTPUT
			,@CW_CustomerId = @CW_CustomerId
			,@Salutation = @Salutation
			,@LastName = @LastName
			,@TC_CampaignSchedulingId = @TC_CampaignSchedulingId
			,-- Modified By : Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampainSchedulingId while adding inquiry.
			@CustomerAltMob = @CustomerAltMob

		--,@IsDuplicacyMerged=@IsDuplicacyMerged -- Modified By : Tejashree 14 Aug 2013,
		--Added By Afrose
		--EXEC TC_AlertNotifications @AssignedTo=@LeadOwnId,@LeadId=@INQLeadIdOutput
		--End added by Afrose
		IF (@CustStatus = 1) --Customer Is Fake
		BEGIN
			SET @Status = 99
			SET @TC_NewCarInquiryId = - 1
		END
		ELSE
		BEGIN
			IF (@InquirySource = 57) --Come from Mobile Masking 
			BEGIN
				SET @InquirySource = 6 --make Mobile Masking source = CarWale source					
			END

			IF (@InquirySource = 60) --Come from Mobile Masking  CarWale Knowlarity Leads
			BEGIN
				SET @InquirySource = 6 --make Mobile Masking source = CarWale source					
			END

			DECLARE @CarDetails VARCHAR(MAX)
			--DECLARE @LeadIdOutput BIGINT,
			--DECLARE @INQLeadIdOutput BIGINT
			DECLARE @INQDate DATETIME = ISNULL(@CreatedOn, GETDATE())

			-- Modified By : Tejashree Patil on 30-11-2015, Fetched default version for make mobile masking inquiry.
			IF (@VersionId IS NULL) -- This inquiry is added form dealer wbesite of TD
			BEGIN
				IF (@InquirySource = 6)
					SELECT TOP 1 @VersionId = V.ID
					FROM CarVersions V WITH (NOLOCK)
					WHERE V.NAME LIKE '%version%'
				ELSE
					SELECT TOP 1 @VersionId = V.ID
					FROM CarVersions V WITH (NOLOCK)
					WHERE V.CarModelId = @ModelId
						AND V.IsDeleted = 0 --AND V.New=1 
						AND V.Futuristic = 0
			END

			-- Modified By: Tejashree Patil on 31 Oct 2014, Fetched Interested vehicle details based on ApllicationId
			SELECT @CarDetails = V.Make + ' ' + V.Model + ' ' + V.Version + ' '
			FROM vwAllMMV V WITH (NOLOCK)
			WHERE V.VersionId = @VersionId
				AND V.ApplicationId = ISNULL(@ApplicationId, 1)

			EXECUTE TC_INQLeadSave @AutoVerified = @AutoVerified
				,@BranchId = @BranchId
				,@CustomerId = @CustomerId
				,@LeadOwnerId = @LeadOwnerId
				,@Eagerness = @Eagerness
				,@CreatedBy = @CreatedBy
				,@InquirySource = @InquirySource
				,@LeadId = @ActiveLeadId
				,@INQDate = @INQDate
				,@LeadInqTypeId = @LeadInqTypeId
				,@CarDetails = @CarDetails
				,@LeadStage = NULL
				,@LeadIdOutput = @LeadIdOutput OUTPUT
				,@INQLeadIdOutput = @INQLeadIdOutput OUTPUT
				,@NextFollowupDate = NULL
				,@FollowupComments = NULL
				,@ReturnStatus = @Status OUTPUT
				,@LeadDivertedTo = @LeadDivertedTo OUTPUT
				,@LeadOwnId = @LeadOwnId OUTPUT
				,@TC_CampaignSchedulingId = @TC_CampaignSchedulingId
				,@LatestVersionId = @VersionId
				,@NextCallTo = @NextCallTo

			-- Modified By : Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampainSchedulingId while adding inquiry.
			DECLARE @PQStatus TINYINT
			DECLARE @TDStatus TINYINT

			IF (@PQReqDate IS NOT NULL)
			BEGIN
				SET @PQStatus = 25
			END

			IF (@TDReqDate IS NOT NULL)
			BEGIN
				SET @TDStatus = 26
			END

			--Check whether its a first inquiry on lead or not
			IF ISNULL(@CampaignId, 0) > 0
			BEGIN
				SELECT TC_NewCarInquiriesId
				FROM TC_NewCarInquiries WITH (NOLOCK)
				WHERE TC_InquiriesLeadId = @INQLeadIdOutput

				IF @@ROWCOUNT = 0
				BEGIN
					SET @IsUpdateCampaignLeadCount = 1
				END
				ELSE
				BEGIN
					--Log Data
					INSERT INTO TC_ContractCampaignDataLog (
						TC_InquiryLeadId
						,CampaignId
						,STATUS
						)
					VALUES (
						@INQLeadIdOutput
						,@CampaignId
						,'Duplicate'
						)
				END
			END

			-- Inserting record in Buyer Inquiries
			DECLARE @IsInquiryDuplicate BIT = 0
			DECLARE @IsInquiryAdd BIT = 0
			DECLARE @Deals_IsAllowToAdd BIT = 0

			--added by : Ashwini Dhamankar on June 1,2016
			IF (
					@InquirySource IN (
						147
						,148
						)
					)
			BEGIN
				IF NOT EXISTS (
						SELECT TC_NewCarInquiriesId
						FROM TC_NewCarInquiries NCI WITH (NOLOCK)
						INNER JOIN TC_InquiriesLead TCIL WITH (NOLOCK) ON NCI.TC_InquiriesLeadId = TCIl.TC_InquiriesLeadId
						WHERE TCIL.TC_CustomerId = @CustomerId
							AND NCI.TC_Deals_StockId = @DealsStockId
							AND NCI.TC_InquirySourceId IN (
								147
								,148
								)
							AND NCI.IsPaymentSuccess = 0
						)
				BEGIN
					SET @Deals_IsAllowToAdd = 1
				END
				ELSE
				BEGIN
					SELECT @TC_NewCarInquiryId = TC_NewCarInquiriesId
					FROM TC_NewCarInquiries NCI WITH (NOLOCK)
					INNER JOIN TC_InquiriesLead TCIL WITH (NOLOCK) ON NCI.TC_InquiriesLeadId = TCIl.TC_InquiriesLeadId
					WHERE TCIL.TC_CustomerId = @CustomerId
						AND NCI.TC_Deals_StockId = @DealsStockId
						AND NCI.TC_InquirySourceId IN (
							147
							,148
							)
						AND NCI.IsPaymentSuccess = 0

					INSERT INTO TC_Deals_NewCarInquiriesLog (
						TC_NewCarInquiriesId
						,TC_InquiriesLeadId
						,TC_InquirySourceId
						,InqCreatedOn
						,TC_Deals_StockId
						,TC_DealsStockVinId
						,IsPaymentSuccess
						,EntryDate
						)
					SELECT TC_NewCarInquiriesId
						,TC_InquiriesLeadId
						,TC_InquirySourceId
						,CreatedOn
						,TC_Deals_StockId
						,TC_DealsStockVinId
						,IsPaymentSuccess
						,GETDATE()
					FROM TC_NewCarInquiries WITH (NOLOCK)
					WHERE TC_NewCarInquiriesId = @TC_NewCarInquiryId

					UPDATE TC_NewCarInquiries
					SET IsPaymentSuccess = @IsPaymentSuccess
						,TC_DealsStockVINId = @TC_Deals_StockVINId
						,TC_InquirySourceId = @InquirySource
					WHERE TC_NewCarInquiriesId = @TC_NewCarInquiryId

					UPDATE TC_InquiriesLead
					SET InqSourceId = @InquirySource
					WHERE TC_InquiriesLeadId = @INQLeadIdOutput

					UPDATE TC_Lead
					SET TC_InquirySourceId = @InquirySource
					WHERE TC_LeadId = @ActiveLeadId

					IF (@IsPaymentSuccess = 1)
					BEGIN
						UPDATE TC_Calls
						SET NextCallTo = @NextCallTo
						WHERE TC_LeadId = @ActiveLeadId

						UPDATE TC_ActiveCalls
						SET NextCallTo = @NextCallTo
						WHERE TC_LeadId = @ActiveLeadId
						
					END
				END
			END

			IF EXISTS (
					SELECT TOP 1 TC_InquiriesLeadId
					FROM TC_NewCarInquiries WITH (NOLOCK)
					WHERE TC_InquiriesLeadId = @INQLeadIdOutput
						AND VersionId = @VersionId
						AND TC_LeadDispositionId IS NULL
					)
				SET @IsInquiryDuplicate = 1

			IF (
					(
						@InquirySource IN (
							134
							,140
							)
						)
					OR (
						@InquirySource IN (
							147
							,148
							)
						AND @Deals_IsAllowToAdd = 1
						) -- modified by:Ashwini Dhamankar on June 1,2016, added @Deals_isAllowToAdd condition
					)
				SET @IsInquiryAdd = 1
			ELSE
				IF @IsInquiryDuplicate = 0
					SET @IsInquiryAdd = 1

			IF @IsInquiryAdd = 1
			BEGIN
				-- Modified By : Nilesh Utture on 7th Jun 2013, Inserted IsCorporate and CompanyName				
				INSERT INTO TC_NewCarInquiries (
					TC_InquiriesLeadId
					,TC_InquirySourceId
					,CreatedOn
					,CreatedBy
					,VersionId
					,Buytime
					,BuyDate
					,CityId
					,PQRequestedDate
					,TDRequestedDate
					,FuelType
					,TransmissionType
					,PQStatus
					,TDStatus
					,
					/*TC_InquiryOtherSourceId,*/ Comments
					,IsCorporate
					,CompanyName
					,IsWillingnessToExchange
					,IsFinanceRequired
					,IsAccessoriesRequired
					,IsSchemesOffered
					,ExcelInquiryId
					,IsExchange
					,TC_CampaignSchedulingId
					,-- Modified By : Vivek Gupta on 20-09-2013
					TC_NewCarExchangeId
					,BookingRequest
					,BookingRequestDate
					,AppointmentRequest
					,AppointmentRequestDate
					,CampaignId
					,TC_DealsStockVINId
					,TC_Deals_StockId --Modified By:Vishal on 07-04-2014, Modified by Afrose on 24-08-2015, param CamapignId
					,IsPaymentSuccess -- Modified By : Nilima More On 13th May 2016,Save IsPaymentSuccess field in tc_newCarInquiries table.
					,MaskingNumber -- Modified by : Tejashree Patil on 4 Nov 2016
					,SalesExMobileNo
					)
				VALUES (
					@INQLeadIdOutput
					,@InquirySource
					,@INQDate
					,@CreatedBy
					,@VersionId
					,@Buytime
					,CAST(DATEADD(DAY, CONVERT(INT, @Buytime), GETDATE()) AS DATETIME)
					,@CityId
					,@PQReqDate
					,@TDReqDate
					,@FuelType
					,@Transmission
					,@PQStatus
					,@TDStatus
					,
					/*@InquiryOtherSourceId,*/ @Comments
					,@IsCorporate
					,@CompanyName
					,@IsWillingnessToExchange
					,@IsFinanceRequired
					,@IsAccessoriesRequired
					,@IsSchemesOffered
					,@ExcelInquiryId
					,NULL
					,--  -- Modified By : Vishal  on 07-04-2014 since no use of column IsExchange from now.
					@TC_CampaignSchedulingId
					,-- Modified By : Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampainSchedulingId while adding inquiry.
					@IsExchange
					,@BookingRequest
					,@BookingRequestDate
					,@AppointmentRequest
					,@AppointmentRequestDate
					,@CampaignId
					,@TC_Deals_StockVINId
					,@DealsStockId -- Modified By : Vivek Gupta on 20-09-2013
					,@IsPaymentSuccess -- Modified By : Nilima More On 13th May 2016,Save IsPaymentSuccess field in tc_newCarInquiries table.
					,@MaskingNumber -- Modified by : Tejashree Patil on 4 Nov 2016
					,@SalesExMobileNo
					)

				--SET @Status=1
				SET @TC_NewCarInquiryId = SCOPE_IDENTITY()

				--Added By Afrose to insert into MappingTable
				--Logic: No duplicate record exists in NewCarInquiries and hence we are reusing it
				IF @IsUpdateCampaignLeadCount = 1
					AND ISNULL(@CampaignId, 0) > 0 -- If this is the first inquiry against that lead
					EXEC TC_ContractsCampaignMapping @CampaignId = @CampaignId
						,@DealerId = @BranchId
						,@INQLeadIdOutput = @INQLeadIdOutput
						,@TC_NewCarInquiryId = @TC_NewCarInquiryId

				-- Modified By : Nilesh Utture on 7th Jun 2013, Added color to resp. table
				IF (
						@ColorList IS NOT NULL
						AND @DealsStockId IS NULL
						)
				BEGIN
					INSERT INTO TC_PrefNewCarColour (
						TC_NewCarInquiriesId
						,VersionColorsId
						)
					SELECT @TC_NewCarInquiryId AS Id
						,ListMember
					FROM dbo.fnSplitCSVValuesWithIdentity(@ColorList)
				END

				-- Modified By : Vivek Gupta on 20-09-2013,ADded exchange car details.
				--IF @IsExchange = 1 -- condition commented by Vishal on 07-04-2014
				IF (
						@IsExchange = 3
						OR @IsExchange = 4
						) -- New condition added by Vishal on 07-04-2014
				BEGIN
					INSERT INTO TC_ExchangeNewCar (
						TC_NewCarInquiriesId
						,CarVersionId
						,MakeYear
						,Kms
						,ExpectedPrice
						)
					VALUES (
						@TC_NewCarInquiryId
						,@ExchangeVersionId
						,@MakeYear
						,@Kilometers
						,@ExpectedPrice
						)
				END

				--BELOW IF Block ADDED By : Vivek Gupta on 25-10-2013
				-- By Vivek Gupta on 12-05-2014 Added (SELECT ID FROM Dealers WHERE ID = @BranchId AND DealerCode IS NOT NULL AND TC_BrandZoneId IS NOT NULL)
				IF (
						@VersionId IS NOT NULL
						AND EXISTS (
							SELECT ID
							FROM Dealers WITH (NOLOCK)
							WHERE ID = @BranchId
								AND DealerCode IS NOT NULL
								AND TC_BrandZoneId IS NOT NULL
							)
						)
				BEGIN
					EXEC TC_NSCCampaignSave @CityId
						,@INQDate
						,@VersionId
						,@TC_NewCarInquiryId
				END
			END
			ELSE
			BEGIN
				IF (
						@TDReqDate IS NOT NULL
						OR @PQReqDate IS NOT NULL
						)
				BEGIN --Update is required to identify request came for PQ or TD only if 
					UPDATE TC_NewCarInquiries
					SET PQRequestedDate = ISNULL(@PQReqDate, PQRequestedDate)
						,PQStatus = ISNULL(@PQStatus, PQStatus)
						,TDRequestedDate = ISNULL(@TDReqDate, TDRequestedDate)
						,TDStatus = ISNULL(@TDStatus, TDStatus)
						,Comments = @Comments
					WHERE TC_InquiriesLeadId = @INQLeadIdOutput
						AND VersionId = @VersionId
						AND TC_LeadDispositionId IS NULL
				END

				IF (
						@BookingRequest IS NOT NULL
						OR @AppointmentRequest IS NOT NULL
						)
				BEGIN --Update is required to identify request came for PQ or TD only if 
					UPDATE TC_NewCarInquiries
					SET BookingRequest = @BookingRequest
						,BookingRequestDate = @BookingRequestDate
						,AppointmentRequest = @AppointmentRequest
						,AppointmentRequestDate = @AppointmentRequestDate
					WHERE TC_InquiriesLeadId = @INQLeadIdOutput
						AND VersionId = @VersionId
						AND TC_LeadDispositionId IS NULL
				END

				--SET @Status=1
				SELECT @TC_NewCarInquiryId = TC_NewCarInquiriesId
				FROM TC_NewCarInquiries WITH (NOLOCK)
				WHERE TC_InquiriesLeadId = @INQLeadIdOutput
					AND VersionId = @VersionId
					AND TC_LeadDispositionId IS NULL
			END

			------------------------------------
			--Modified By : Ashwini Dhamankar on June 1,2016 (Changed position of below logic)
			-- Log aged car inquiry in TC_NewCarBooking 
			IF (
					@TC_Deals_StockVINId IS NOT NULL
					AND @IsPaymentSuccess = 1
					)
			BEGIN
				-- need to call TC_Deals_ChangeVINStatus
				EXEC TC_Deals_ChangeVINStatus NULL
					,@TC_Deals_StockVINId
					,4
					,@LeadOwnerId
					,NULL

				--Get Price
				DECLARE @StockPrice INT = NULL

				SET @StockPrice = (
						SELECT TOP 1 DiscountedPrice
						FROM TC_Deals_StockPrices WITH (NOLOCK)
						WHERE TC_Deals_StockId = @DealsStockId
							AND CityId = @CityId
						)

				IF NOT EXISTS (
						SELECT TOP 1 TC_NewCarInquiriesId
						FROM TC_NewCarBooking WITH (NOLOCK)
						WHERE TC_NewCarInquiriesId = @TC_NewCarInquiryId
						) -- 
				BEGIN
					INSERT INTO TC_NewCarBooking (
						TC_NewCarInquiriesId
						,RequestedDate
						,BookingStatus
						,Price
						,Payment
						,PendingPayment
						,IsLoanRequired
						,TC_UsersId
						,VinNo
						,CustomerName
						,ContactNo
						,Email
						,Salutation
						,LastName
						,TC_Deals_StockVINId
						,TC_Deals_StockId
						,ModelYear
						,CarColorId
						)
					VALUES (
						@TC_NewCarInquiryId
						,GETDATE()
						,96
						,@StockPrice
						,@BookingAmountDeals
						,@StockPrice - @BookingAmountDeals
						,NULL
						,@CreatedBy
						,@TC_Deals_StockVINNo
						,@CustomerName
						,@CustomerMobile
						,@CustomerEmail
						,@Salutation
						,@LastName
						,@TC_Deals_StockVINId
						,@DealsStockId
						,@DealsMakeYear
						,@DealsCarColorId
						)
				END
				ELSE
				BEGIN
					UPDATE TC_NewCarBooking
					SET TC_NewCarInquiriesId = @TC_NewCarInquiryId
						,RequestedDate = GETDATE()
						,BookingStatus = 96
						,Price = @StockPrice
						,Payment = @BookingAmountDeals
						,PendingPayment = @StockPrice - @BookingAmountDeals
						,IsLoanRequired = NULL
						,TC_UsersId = @CreatedBy
						,VinNo = @TC_Deals_StockVINNo
						,CustomerName = @CustomerName
						,ContactNo = @CustomerMobile
						,Email = @CustomerEmail
						,Salutation = @Salutation
						,LastName = @LastName
						,TC_Deals_StockVINId = @TC_Deals_StockVINId
						,TC_Deals_StockId = @DealsStockId
						,ModelYear = @DealsMakeYear
						,CarColorId = @DealsCarColorId
					WHERE TC_NewCarInquiriesId = @TC_NewCarInquiryId
				END

				--Change the status of inquiry also
				UPDATE TC_NewCarInquiries
				SET BookingStatus = 96
				WHERE TC_NewCarInquiriesId = @TC_NewCarInquiryId
			END

			-------------------------------------------
			IF (@ActiveLeadId IS NOT NULL)
			BEGIN
				SET @IsNewInq = 0
			END
			ELSE
			BEGIN
				SET @IsNewInq = 1
			END

			--SELECT @ExcelInquiryId
			IF (@ExcelInquiryId IS NOT NULL)
			BEGIN
				DECLARE @TC_NewCarInquiryIds VARCHAR(50) = NULL -- Modified By : Tejashree 5 July 2013

				SELECT @TC_NewCarInquiryIds = TC_NewCarInquiriesId
				FROM TC_ExcelInquiries EI WITH (NOLOCK)
				WHERE EI.Id = @ExcelInquiryId
					AND EI.BranchId = @BranchId
					AND IsValid = 1

				SET @TC_NewCarInquiryIds = ISNULL(CONVERT(VARCHAR(50), @TC_NewCarInquiryIds) + ',', '') + CONVERT(VARCHAR(50), @TC_NewCarInquiryId)

				SELECT @TC_NewCarInquiryIds AS TC_NewCarInquiryIds
					,@TC_NewCarInquiryIds AS TC_NewCarInquiryIds

				UPDATE EI
				SET TC_NewCarInquiriesId = @TC_NewCarInquiryIds
					,IsNew = @IsNewInq
					,IsDeleted = 1
					,LeadOwnerId = @LeadOwnerId
					,ModifiedDate = GETDATE()
				FROM TC_ExcelInquiries EI WITH (NOLOCK)
				LEFT JOIN TC_Users U WITH (NOLOCK) ON U.Id = EI.UserId -- Modified By : Tejashree Patil on 30-07-2014
				WHERE EI.Id = @ExcelInquiryId
					AND EI.BranchId = @BranchId
					AND IsValid = 1
			END

			-- Modified by : Tejashree Patil on 10 May 2013
			/*DECLARE @CntUserInDealership SMALLINT, @TC_UsersId BIGINT

				SELECT @CntUserInDealership =COUNT(*) FROM TC_Users  WITH(NOLOCK) WHERE BranchId=@BranchId AND IsActive=1 AND IsCarwaleUser=0

				SELECT @TC_UsersId=Id FROM TC_Users WITH(NOLOCK) WHERE BranchId=@BranchId AND IsActive=1 AND IsCarwaleUser=0

				

				IF(@CntUserInDealership=1) -- Single User Dealership assign all to that single user

				BEGIN 

					EXECUTE TC_LeadVerificationSchedulingForSingleUser @TC_UsersId, @BranchId

				END*/
			-- Modified by : Nilesh Utture on 18th Dec, 2013, Push all inquiries to user even if he is having any sales exec. role		
			-- Modified by : Nilesh Utture on 18th Feb, 2014, added DISTINCT UserId condition while assigning inquireis to single user dealer		
			DECLARE @TC_UsersId INT

			SELECT DISTINCT @TC_UsersId = U.Id
			FROM TC_Users U WITH (NOLOCK)
			INNER JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
			WHERE BranchId = @BranchId
				AND IsActive = 1
				AND IsCarwaleUser = 0
				AND R.RoleId IN (
					4
					,5
					,6
					)

			IF (@@ROWCOUNT = 1) -- Single User Dealership assign all to that single user
			BEGIN
				EXECUTE TC_LeadVerificationSchedulingForSingleUser @TC_UsersId
					,@BranchId
			END
		END

		-- making latest inquiry as most interested inquiry
		IF @TC_NewCarInquiryId <> - 1
			AND (
				SELECT COUNT(DISTINCT BI.TC_BuyerInquiriesId) + COUNT(DISTINCT NI.TC_NewCarInquiriesId)
				FROM TC_InquiriesLead TCIL WITH (NOLOCK)
				LEFT JOIN TC_BuyerInquiries BI WITH (NOLOCK) ON TCIL.TC_InquiriesLeadId = BI.TC_InquiriesLeadId
				LEFT JOIN TC_NewCarInquiries NI WITH (NOLOCK) ON TCIL.TC_InquiriesLeadId = NI.TC_InquiriesLeadId
				WHERE TCIL.TC_LeadId = @LeadIdOutput
				) = 1
		BEGIN
			EXEC TC_UpdateMostInterestedInquiry @BranchId
				,@TC_NewCarInquiryId
				,3
		END

		---------------------START--- Update  sourceId in customer table and in TC_INQLeadSave, if source Id = 134------------------
		IF (
				@InquirySource IN (
					134
					,140
					,146
					,147
					,148
					)
				)
		BEGIN
			UPDATE TC_InquiriesLead
			SET InqSourceId = @InquirySource
			WHERE TC_InquiriesLeadId = @LeadIdOutput

			UPDATE TC_CustomerDetails
			SET TC_InquirySourceId = @InquirySource
			WHERE ActiveLeadId = @LeadIdOutput

			UPDATE TC_Lead
			SET TC_InquirySourceId = @InquirySource
			WHERE TC_LeadId = @LeadIdOutput
		END

		-----------------END--------------------------------------------
		COMMIT TRANSACTION ProcessNewCarBuyerInquiries
	END TRY

	BEGIN CATCH
		--print 'rollback'
		ROLLBACK TRANSACTION ProcessNewCarBuyerInquiries

		INSERT INTO TC_Exceptions (
			Programme_Name
			,TC_Exception
			,TC_Exception_Date
			,InputParameters
			)
		VALUES (
			'TC_INQNewCarBuyerSave'
			,(ERROR_MESSAGE() + 'ERROR_NUMBER(): ' + CONVERT(VARCHAR, ERROR_NUMBER()))
			,GETDATE()
			,' @CustomerName:' + ISNULL(@CustomerName, 'NULL') + ' @CustomerEmail :' + ISNULL(@CustomerEmail, 'NULL') + ' @CustomerMobile : ' + ISNULL(@CustomerMobile, 'NULL') + ' @VersionId : ' + ISNULL(CAST(@VersionId AS VARCHAR(50)), 'NULL') + ' @CityId: ' + ISNULL(CAST(@CityId AS VARCHAR(50)), 'NULL') + ' @Buytime: ' + ISNULL(@Buytime, 'NULL') + ' @InquirySource : ' + ISNULL(CAST(@InquirySource AS VARCHAR(50)), 'NULL') + ' @Eagerness: ' + ISNULL(CAST(@Eagerness AS VARCHAR(50)), 'NULL') + ' @TC_CustomerId: ' + ISNULL(CAST(@TC_CustomerId AS VARCHAR(50)), 'NULL') + ' @AutoVerified : ' + ISNULL(CAST(@AutoVerified AS VARCHAR(5)), 'NULL') + ' @BranchId : ' + ISNULL(CAST(@BranchId AS VARCHAR(50)), 'NULL') + ' @LeadOwnerId: ' + ISNULL(CAST(@LeadOwnerId AS VARCHAR(50)), 'NULL') + ' @CreatedBy: ' + ISNULL(CAST(@CreatedBy AS VARCHAR(50)), 'NULL') + ' @PQReqDate: ' + ISNULL(CAST(@PQReqDate AS VARCHAR(50)), 'NULL') + ' @TDReqDate: ' + ISNULL(CAST(@TDReqDate AS VARCHAR(50)), 'NULL') + ' @ModelId: ' + ISNULL(CAST(@ModelId AS VARCHAR(50)), 'NULL') + ' @FuelType : ' + ISNULL(CAST(@FuelType AS VARCHAR(50
					)), 'NULL') + ' @Transmission : ' + ISNULL(CAST(@Transmission AS VARCHAR(50)), 'NULL') + ' @CW_CustomerId: ' + ISNULL(CAST(@CW_CustomerId AS VARCHAR(50)), 'NULL') + ' @CWInquiryId: ' + ISNULL(CAST(@CWInquiryId AS VARCHAR(50)), 'NULL') + ' @InquiryOtherSourceId: ' + ISNULL(CAST(@InquiryOtherSourceId AS VARCHAR(50)), 'NULL') + ' @ExcelInquiryId: ' + ISNULL(CAST(@ExcelInquiryId AS VARCHAR(50)), 'NULL') + ' @Comments: ' + ISNULL(@Comments, 'NULL') + ' @IsCorporate: ' + ISNULL(CAST(@IsCorporate AS VARCHAR(5)), 'NULL') + ' @CompanyName: ' + ISNULL(@CompanyName, 'NULL') + ' @Address: ' + ISNULL(@Address, 'NULL') + ' @ColorList: ' + ISNULL(@ColorList, 'NULL')
			)
			--SELECT ERROR_NUMBER() AS ErrorNumber;
	END CATCH;
END


