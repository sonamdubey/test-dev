IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQBuyerSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQBuyerSave]
GO

	-- Created By:	Surendra
-- Create date: 4th Jan 2013
-- Description:	Adding Buyer's Inquiry with Stock
-- Modified By : Tejashree Patil on 22 Feb 2013 at 6 pm, Changed position of TC_StockAnalysis Insert statement 
-- Modified By : Surendra on 05 march 2013 modify insert exception statement
-- Modified By : Tejashree 19 march 2013 Added output parameter @TC_BuyerInquiryId, @ImportInqCarDetails and @TC_ExcelInquiriesId 
-- Modified By : Tejashree 8 April 2013 Set IsDeleted=1 for ImportedInquiry
-- Modified By : Tejashree 17 April 2013, Changed Update query after @ExcelInquiryId IS NULL condition
-- Modified By : Tejashree 22 April 2013, Set @TC_BuyerInquiryId=-1.
-- Modified by : Tejashree Patil on 10 May 2013, constraint for Name updation when source is MobileMasking, Assign all inquiries to single user.
-- Modified by : Manish Chourasiya,Commit transaction statement in if and else separately and single user scheduling sp is out of the transaction for handling exeception
-- Modified By: Vivek Gupta on 20th May , 2013 Added Three OUtput Parameters @LeadOwnId,@CustomerId,@LeadIdOutput
-- Modified By : Tejashree Patil on 3 Jun 2013, Change source of 60(CarWale Knowlarity Leads) to 1(carwale).
-- Modified By : Tejashree Patil on 07-06-2013
-- Modified by : Nilesh Utture on 18th Dec, 2013, Push all inquiries to user even if he is having any sales exec. role	
-- Modified By : Vivek Gupta on 18-12-2013. Added Parameter @RequestType to save TDRequested or Booking requested for inq coming from carwale.
-- Modified by : Nilesh Utture on 18th Feb, 2014, added DISTINCT UserId condition while assigning inquireis to single user dealer
-- Modified by : Manish Chourasiya on 15-04-2014 added with (nolock) keyword wherever not found.
-- Modified by : Manish Chourasiya on 21-04-2014 removed explicit transaction for avoiding deadlock and long running queries.
-- Modified By: Ashwini Dhamankar on Dec 11,2014 , Inserted BuyerInquiryComment into TC_BuyerInquiryComments table
-- Modified By: ViCKY Gupta on 03-08-2015, Pass the parameter LatestVersionId to SP TC_INQLeadSave
-- Modified By Vivek Gupta on 29-09-2015 ,  making latest inquiry as most interested inquiry
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQBuyerSave] (
	@AutoVerified BIT
	,--1 if inquiry ias added from trading cars
	@BranchId INT
	,@StockId INT
	,
	-- TC_CustomerDetails's related param
	@CustomerName VARCHAR(100)
	,@Email VARCHAR(100)
	,@Mobile VARCHAR(15)
	,@Location VARCHAR(50)
	,@Buytime VARCHAR(20)
	,
	--@CustomerComments VARCHAR(400), Removed by surendra
	--- Followup related Param
	@Comments VARCHAR(500)
	,@Eagerness SMALLINT
	,-- Renamed
	@NextFollowup DATETIME
	,@InquirySource SMALLINT
	,
	----- Other Param
	@LeadOwnerId INT
	,@CreatedBy INT
	,
	-- Loose Inquiery related
	@MinPrice INT
	,@MaxPrice INT
	,@FromMakeYear SMALLINT
	,@ToMakeYear SMALLINT
	,@ModelIds VARCHAR(400)
	,@ModelNames VARCHAR(400)
	,@BodyTypeIds VARCHAR(200)
	,@FuelTypeIds VARCHAR(200)
	,@BodyTypeNames VARCHAR(200)
	,@FuelTypeNames VARCHAR(200)
	,@UsedCarPurchaseInquiryId INT = NULL
	,@Status SMALLINT OUTPUT
	,@CW_CustomerId INT = NULL
	,@SellInquiryId INT = NULL
	,--id FROM SELLERINQUIRIES
	@LeadDivertedTo VARCHAR(100) OUTPUT
	,@TC_BuyerInquiryId INT OUTPUT
	,@ImportInqCarDetails VARCHAR(1000)
	,@ExcelInquiryId INT
	,--Imported from excel
	@InquiryOtherSourceId TINYINT = NULL
	,@LeadOwnId INT = NULL OUTPUT
	,@CustomerId INT = NULL OUTPUT
	,@LeadIdOutput INT = NULL OUTPUT
	,@INQLeadIdOutput INT = NULL OUTPUT
	,@RequestType SMALLINT = NULL
	)
AS
BEGIN
	SET NOCOUNT ON;

	-- inserting record in main table (TC_inquiries) of inquiries
	--Below variables are declare by Vivek Gupta on 18-12-2013.
	DECLARE @IsTDRequested BIT = 0
	DECLARE @TDRequestedDate DATETIME = NULL
	DECLARE @BookingRequested BIT = 0
	DECLARE @BookingRequestedDate DATETIME = NULL

	IF (@RequestType = 2)
	BEGIN
		SET @IsTDRequested = 1
		SET @TDRequestedDate = GETDATE()
	END
	ELSE
		IF (@RequestType = 3)
		BEGIN
			SET @BookingRequested = 1
			SET @BookingRequestedDate = GETDATE()
		END

	BEGIN TRY
		--	BEGIN TRANSACTION ProcessBuyerInquiries  line commented by manish on 21-04-2014
		--Registering/Updating Customer
		--DECLARE @CustomerId BIGINT
		DECLARE @CustStatus SMALLINT
		DECLARE @ActiveLeadId INT

		SET @Status = 0

		IF (@SellInquiryId IS NOT NULL) -- this means inquiry come from carwale
		BEGIN
			SELECT @BranchId = DealerId
				,@StockId = TC_StockId
			FROM SellInquiries WITH (NOLOCK)
			WHERE ID = @SellInquiryId
		END

		-- following sp will add or update customer and will return customer staus and active lead
		EXEC TC_CustomerDetailSave @BranchId = @BranchId
			,@CustomerEmail = @Email
			,@CustomerName = @CustomerName
			,@CustomerMobile = @mobile
			,@Location = @Location
			,@Buytime = @Buytime
			,@Comments = NULL
			,@CreatedBy = @CreatedBy
			,@Address = NULL
			,@SourceId = @InquirySource
			,@CustomerId = @CustomerId OUTPUT
			,@Status = @CustStatus OUTPUT
			,@ActiveLeadId = @ActiveLeadId OUTPUT
			,@CW_CustomerId = @CW_CustomerId
			 --@CampaignId = NULL                --Added on 18th Sep 2015

		IF (@CustStatus = 1) --Customer Is Fake
		BEGIN
			SET @Status = 99
			SET @TC_BuyerInquiryId = - 1
				--	COMMIT TRANSACTION ProcessBuyerInquiries	line commented by manish on 21-04-2014
		END
		ELSE --- Customer is not fake hence proceed
		BEGIN
			IF (@InquirySource = 57) --Come from Mobile Masking 
			BEGIN
				--SET @InquirySource=1--make Mobile Masking source = CarWale source					
				SET @InquirySource = 6 --make Mobile Masking source = CarWale source--Modified by Tejashree on 24-05-2013 for Mobile masking source				
			END

			IF (@InquirySource = 60) --Come from Mobile Masking  CarWale Knowlarity Leads
			BEGIN
				SET @InquirySource = 6 --make Mobile Masking source = CarWale source					
			END

			DECLARE @CarDetails VARCHAR(MAX)
			--DECLARE @LeadIdOutput BIGINT,
			--DECLARE @INQLeadIdOutput BIGINT
			DECLARE @INQDate DATETIME = GETDATE()
			DECLARE @LatestVersionId INT

			IF (@StockId IS NOT NULL)
			BEGIN
				SELECT @CarDetails = V.Make + ' ' + V.Model + ' ' + V.Version + ' '
					,@LatestVersionId = V.VersionId --Modified by vicky Gupta on 03-08-2015
				FROM TC_Stock S WITH (NOLOCK)
				INNER JOIN vwMMV V WITH (NOLOCK) ON S.VersionId = V.VersionId
				WHERE S.Id = @StockId
			END
			ELSE
			BEGIN -- loose inquiry
				--SELECT @CarDetails=ISNULL(@ModelIds,'') + ' ' + ISNULL(CONVERT(VARCHAR(10),@MinPrice),'')
				SELECT @CarDetails = ([dbo].TC_CarDetailBuyInq(@MinPrice, @MaxPrice, @FromMakeYear, @ToMakeYear, @ModelNames, @BodyTypeNames, @FuelTypeNames))

				SET @LatestVersionId = NULL
			END

			IF (@ExcelInquiryId IS NOT NULL)
			BEGIN
				SET @CarDetails = @ImportInqCarDetails
			END

			EXECUTE TC_INQLeadSave @AutoVerified = @AutoVerified
				,@BranchId = @BranchId
				,@CustomerId = @CustomerId
				,@LeadOwnerId = @LeadOwnerId
				,@Eagerness = @Eagerness
				,@CreatedBy = @CreatedBy
				,@InquirySource = @InquirySource
				,@LeadId = @ActiveLeadId
				,@INQDate = @INQDate
				,@LeadInqTypeId = 1
				,@CarDetails = @CarDetails
				,@LeadStage = NULL
				,@LeadIdOutput = @LeadIdOutput OUTPUT
				,@INQLeadIdOutput = @INQLeadIdOutput OUTPUT
				,@NextFollowupDate = @NextFollowup
				,@FollowupComments = @Comments
				,@ReturnStatus = @Status OUTPUT
				,@LeadDivertedTo = @LeadDivertedTo OUTPUT
				,@LeadOwnId = @LeadOwnId OUTPUT
				,@LatestVersionId = @LatestVersionId,
				@CampaignId = NULL   --Added on 18th Sep 2015

			-- Inserting record in Buyer Inquiries
			-- Restriction for same buyer inquiry for same stock with not lead disposition	
			IF (@StockId IS NOT NULL)
			BEGIN
				IF NOT EXISTS (
						SELECT TOP 1 TC_BuyerInquiriesId
						FROM TC_BuyerInquiries WITH (NOLOCK)
						WHERE TC_InquiriesLeadId = @INQLeadIdOutput
							AND TC_LeadDispositionId IS NULL
							AND StockId = @StockId
						)
				BEGIN
					--SELECT @ContactVerified = ISNULL(ContactVerified, -1) FROM TC_CustomerDetails WITH(NOLOCK) WHERE Id = (SELECT TC_CustomerId FROM TC_InquiriesLead WITH(NOLOCK) WHERE TC_InquiriesLeadId = @INQLeadIdOutput)					   
					INSERT INTO TC_BuyerInquiries (
						StockId
						,UsedCarPurchaseInquiryId
						,TC_InquiriesLeadId
						,CreatedBy
						,CreatedOn
						,MakeYearFrom
						,MakeYearTo
						,PriceMin
						,PriceMax
						,TC_InquirySourceId
						,BuyDate
						,Comments
						,IsTDRequested
						,TDRequestedDate
						,BookingRequested
						,BookingRequestedDate
						)
					VALUES (
						@StockId
						,@UsedCarPurchaseInquiryId
						,@INQLeadIdOutput
						,@CreatedBy
						,@INQDate
						,@FromMakeYear
						,@ToMakeYear
						,@MinPrice
						,@MaxPrice
						,@InquirySource
						,CAST(DATEADD(DAY, CONVERT(INT, @Buytime), GETDATE()) AS DATETIME)
						,@Comments
						,@IsTDRequested
						,@TDRequestedDate
						,@BookingRequested
						,@BookingRequestedDate
						)

					SET @TC_BuyerInquiryId = SCOPE_IDENTITY();

					-- Modified By : Tejashree Patil on 22 Feb 2013 at 6 pm								
					IF NOT EXISTS (
							SELECT StockId
							FROM TC_StockAnalysis WITH (NOLOCK)
							WHERE StockId = @StockId
							)
					BEGIN
						INSERT INTO TC_StockAnalysis (
							StockId
							,CWResponseCount
							,TCResponseCount
							)
						VALUES (
							@StockId
							,0
							,0
							)
					END

					IF (@InquirySource = 1) -- CarWale as source
					BEGIN
						-- Update CWResponseCount to Table.
						UPDATE TC_StockAnalysis
						SET CWResponseCount = CWResponseCount + 1
						WHERE StockId = @StockId
					END
					ELSE -- Any other source
					BEGIN
						-- Update TCResponseCount to Table.
						UPDATE TC_StockAnalysis
						SET TCResponseCount = TCResponseCount + 1
						WHERE StockId = @StockId
					END

					--Modified By: Ashwini Dhamankar on Dec 11,2014 , Inserted BuyerInquiryComment into TC_BuyerInquiryComments table
					IF (@InquirySource = 91)
					BEGIN
						INSERT INTO TC_BuyerInquiryComments (
							TC_BuyerInquiryId
							,EntryDate
							,Comments
							)
						VALUES (
							@TC_BuyerInquiryId
							,GETDATE()
							,@Comments
							)
					END
				END
						--Below Else Block Added by Vivek Gupta on 19-12-2013.. to update request type for inq coming from carwale source.
				ELSE
				BEGIN
					DECLARE @BuyerInqId INT

					SELECT TOP 1 @BuyerInqId = TC_BuyerInquiriesId
					FROM TC_BuyerInquiries WITH (NOLOCK)
					WHERE TC_InquiriesLeadId = @INQLeadIdOutput
						AND TC_LeadDispositionId IS NULL
						AND StockId = @StockId

					IF (@RequestType = 2)
						UPDATE TC_BuyerInquiries
						SET IsTDRequested = @IsTDRequested
							,TDRequestedDate = @TDRequestedDate
						WHERE TC_BuyerInquiriesId = @BuyerInqId

					IF (@RequestType = 3)
						UPDATE TC_BuyerInquiries
						SET BookingRequested = @BookingRequested
							,BookingRequestedDate = @BookingRequestedDate
						WHERE TC_BuyerInquiriesId = @BuyerInqId

					--Modified By: Ashwini Dhamankar on Dec 11,2014 , Inserted BuyerInquiryComment into TC_BuyerInquiryComments table
					IF (@InquirySource = 91)
					BEGIN
						INSERT INTO TC_BuyerInquiryComments (
							TC_BuyerInquiryId
							,EntryDate
							,Comments
							)
						VALUES (
							@BuyerInqId
							,GETDATE()
							,@Comments
							)
					END
				END
			END
			ELSE
			BEGIN
				INSERT INTO TC_BuyerInquiries (
					StockId
					,UsedCarPurchaseInquiryId
					,TC_InquiriesLeadId
					,CreatedBy
					,CreatedOn
					,MakeYearFrom
					,MakeYearTo
					,PriceMin
					,PriceMax
					,TC_InquirySourceId
					,BuyDate /*,TC_InquiryOtherSourceId*/
					,Comments
					,IsTDRequested
					,TDRequestedDate
					,BookingRequested
					,BookingRequestedDate
					)
				VALUES (
					@StockId
					,@UsedCarPurchaseInquiryId
					,@INQLeadIdOutput
					,@CreatedBy
					,@INQDate
					,@FromMakeYear
					,@ToMakeYear
					,@MinPrice
					,@MaxPrice
					,@InquirySource
					,CAST(DATEADD(DAY, CONVERT(INT, @Buytime), GETDATE()) AS DATETIME) /*, @InquiryOtherSourceId*/
					,@Comments
					,@IsTDRequested
					,@TDRequestedDate
					,@BookingRequested
					,@BookingRequestedDate
					)
			END

			--DECLARE @TC_BuyerInquiries BIGINT
			SET @TC_BuyerInquiryId = SCOPE_IDENTITY();

			-- Adding Model preference for loose inquiry			
			IF (@ModelIds IS NOT NULL)
			BEGIN
				INSERT INTO Tc_PrefModelMake (
					TC_BuyerInquiriesId
					,ModelId
					,CreatedOn
					)
				SELECT @TC_BuyerInquiryId
					,listmember
					,@INQDate
				FROM [dbo].[fnSplitCSV](@ModelIds)
			END

			-- Adding Body Types preferences for loose inquiry				
			IF (@BodyTypeIds IS NOT NULL)
			BEGIN
				INSERT INTO Tc_PrefBodyStyle (
					TC_BuyerInquiriesId
					,BodyType
					,CreatedOn
					)
				SELECT @TC_BuyerInquiryId
					,listmember
					,@INQDate
				FROM [dbo].[fnSplitCSV](@BodyTypeIds)
			END

			-- Adding fueltype preferences for loose inquiry
			IF (@FuelTypeIds IS NOT NULL)
			BEGIN
				INSERT INTO TC_PrefFuelType (
					TC_BuyerInquiriesId
					,FuelType
					,CreatedOn
					)
				SELECT @TC_BuyerInquiryId
					,listmember
					,@INQDate
				FROM [dbo].[fnSplitCSV](@FuelTypeIds)
			END

			--SET @Status=1		
			DECLARE @IsNewImportedInq BIT

			IF (@ActiveLeadId IS NOT NULL)
			BEGIN
				SET @IsNewImportedInq = 0
			END
			ELSE
			BEGIN
				SET @IsNewImportedInq = 1
			END

			IF (@ExcelInquiryId IS NOT NULL)
			BEGIN
				UPDATE BI
				SET TC_BuyerInquiriesId = @TC_BuyerInquiryId
					,IsNew = @IsNewImportedInq
					,IsDeleted = 1
					,LeadOwnerId = @LeadOwnerId
					,ModifiedDate = GETDATE()
				FROM TC_ImportBuyerInquiries BI WITH (NOLOCK)
				INNER JOIN TC_Users U WITH (NOLOCK) ON U.Id = BI.UserId
				WHERE TC_ImportBuyerInquiriesId = @ExcelInquiryId
					AND BI.BranchId = @BranchId
					AND IsValid = 1
			END

			-- Modified by : Tejashree Patil on 10 May 2013
			/*DECLARE @CntUserInDealership SMALLINT, @TC_UsersId BIGINT

				SELECT @CntUserInDealership =COUNT(*) FROM TC_Users WHERE BranchId=@BranchId AND IsActive=1 AND IsCarwaleUser=0

				SELECT @TC_UsersId=Id FROM TC_Users WHERE BranchId=@BranchId AND IsActive=1 AND IsCarwaleUser=0*/
			-- Modified by : Nilesh Utture on 18th Dec, 2013, Push all inquiries to user even if he is having any sales exec. role				
			DECLARE @TC_UsersId INT

			SELECT DISTINCT @TC_UsersId = U.Id -- Modified by : Nilesh Utture on 18th Feb, 2014, added DISTINCT UserId condition while assigning inquireis to single user dealer
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
					--	COMMIT TRANSACTION ProcessBuyerInquiries        line commented by manish on 21-04-2014
					/*IF(@CntUserInDealership=1) -- Single User Dealership assign all to that single user

				BEGIN 

					EXECUTE TC_LeadVerificationSchedulingForSingleUser @TC_Usersid, @BranchId

				END*/
		END

		-- making latest inquiry as most interested inquiry
		IF @TC_BuyerInquiryId <> -1 AND (SELECT COUNT(DISTINCT BI.TC_BuyerInquiriesId) + COUNT(DISTINCT NI.TC_NewCarInquiriesId)  FROM TC_InquiriesLead TCIL WITH(NOLOCK)
														LEFT JOIN TC_BuyerInquiries BI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId = BI.TC_InquiriesLeadId
														LEFT JOIN TC_NewCarInquiries NI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId = NI.TC_InquiriesLeadId
										  WHERE TCIL.TC_LeadId = @LeadIdOutput
											) = 1
         BEGIN
			EXEC TC_UpdateMostInterestedInquiry @BranchId, @TC_BuyerInquiryId, 1
		 END
	END TRY

	BEGIN CATCH
		--ROLLBACK TRANSACTION ProcessBuyerInquiries      line commented by manish on 21-04-2014
		DECLARE @Inputparameters VARCHAR(MAX)

		SET @Inputparameters = '@AutoVerified:' + CAST(@AutoVerified AS VARCHAR(5)) + ',@BranchId:' + CAST(ISNULL(@BranchId, - 1) AS VARCHAR(50)) + ',@StockId:' + CAST(ISNULL(@StockId, - 1) AS VARCHAR(50)) + ',@CustomerName:' + ISNULL(@CustomerName, '') + ',@Email: ' + ISNULL(@Email, '') + ',@Mobile:' + ISNULL(@Mobile, '') + ',@Location:' + ISNULL(@Location, '') + ',@Buytime:' + ISNULL(@Buytime, '') + ',@Comments:' + ISNULL(@Comments, '') + ',@Eagerness:' + CAST(ISNULL(@Eagerness, '') AS VARCHAR(50)) + ',@InquirySource:' + CAST(ISNULL(@InquirySource, 0) AS VARCHAR(50)) + ',@LeadOwnerId:' + CAST(ISNULL(@LeadOwnerId, 0) AS VARCHAR(50)) + ',@CreatedBy:' + CAST(ISNULL(@CreatedBy, 0) AS VARCHAR(50)) + ',@MinPrice:' + CAST(ISNULL(@MinPrice, - 1) AS VARCHAR(50)) + ',@MaxPrice:' + CAST(ISNULL(@MaxPrice, - 1) AS VARCHAR(50)) + ',@FromMakeYear:' + CAST(ISNULL(@FromMakeYear, - 1) AS VARCHAR(50)) + ',@ToMakeYear:' + CAST(ISNULL(@ToMakeYear, - 1) AS VARCHAR(50)) + ',@ModelIds:' + ISNULL(@ModelIds, '') + ',@ModelNames:' + ISNULL(@ModelNames, '') + ',@BodyTypeIds:' + ISNULL(@BodyTypeIds, '') + ',@FuelTypeIds:' + ISNULL(
				@FuelTypeIds, '') + ',@BodyTypeNames:' + ISNULL(@BodyTypeNames, '') + ',@FuelTypeNames:' + ISNULL(@FuelTypeNames, '') + ',@UsedCarPurchaseInquiryId:' + CAST(ISNULL(@UsedCarPurchaseInquiryId, - 1) AS VARCHAR(50)) + ',@CW_CustomerId:' + CAST(ISNULL(@CW_CustomerId, - 1) AS VARCHAR(50)) + ',@SellInquiryId:' + CAST(ISNULL(@SellInquiryId, - 1) AS VARCHAR(50))

		INSERT INTO TC_Exceptions (
			Programme_Name
			,TC_Exception
			,TC_Exception_Date
			,Inputparameters
			)
		VALUES (
			'TC_INQBuyerSave'
			,(ERROR_MESSAGE() + ', ERROR_NUMBER(): ' + CONVERT(VARCHAR, ERROR_NUMBER()))
			,GETDATE()
			,@Inputparameters
			)
			--SELECT ERROR_NUMBER() AS ErrorNumber;
	END CATCH;
END

