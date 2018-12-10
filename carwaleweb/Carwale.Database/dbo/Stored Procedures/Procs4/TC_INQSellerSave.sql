IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQSellerSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQSellerSave]
GO

	
-- Created By:	Tejashree Patil
-- Create date: 7th Jan 2013
-- Description:	Adding Seller Inquiry Details
-- Modified By: Tejashree Patil on 15 Feb 2013, Passed NULL to TC_INQLeadSave instead @Comments
-- Modified By : Surendra on 05 march 2013 modify insert exception statement
-- Modified By : Tejashree Patil on 5th April 2013 at 3 pm, Added @ExcelInquiryId,@TC_InquiryOtherSourceId, @FollowupComments=@Comments for imported seller inquiries. 
-- Modified By : Tejashree 17 April 2013, Changed Update query after @ExcelInquiryId IS NULL condition
-- Modified By : Tejashree 22 April 2013, Set @TC_SellerInquiryId=-1.
-- Modified by : Tejashree Patil on 10 May 2013, Automaticaaly assign all inquiries to single user instead of scheduling.
-- Modified by : Manish Chourasiya,remove begin,commit ,rollback  transaction statement for handling exeception
-- Modified By: Vivek Gupta on 20th May , 2013 Added Three OUtput Parameters @LeadOwnId,@CustomerId,@LeadIdOutput
-- Modified by : Tejashree Patil on 17 May 2013, Updated lead owner id in TC_ImportSellerInquiries Table.
-- Modified by : Tejashree Patil on 24 July 2013, Added IsDealer=0 in WHERE clause.
-- Modified by : Nilesh Utture on 18th Dec, 2013, Push all inquiries to user even if he is having any sales exec. role	
-- Modified by : Nilesh Utture on 18th Feb, 2014, added DISTINCT UserId condition while assigning inquireis to single user dealer	
-- Modified By: Vicky Gupta on 03-08-2015, added parameter LatestVersionId for sp TC_INQLeadSave
-- Modified By Vivek Gupta on 13-08-2015, added original imagepath
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQSellerSave] (
	@TC_SellerInquiryId BIGINT
	,@BranchId BIGINT
	,@VersionId INT
	,@AutoVerified BIT
	,@LeadOwnerId BIGINT
	,
	-- CustomerDetails
	@CustomerName VARCHAR(100)
	,@Email VARCHAR(100)
	,@Mobile VARCHAR(15)
	,@Location VARCHAR(50)
	,@Buytime VARCHAR(20)
	,@Eagerness SMALLINT
	,-- Renamed
	@InquirySource SMALLINT
	,@TC_UserId BIGINT
	,
	-- Car Details
	@MakeYear DATETIME
	,@Price BIGINT
	,@Kilometers INT
	,@Color VARCHAR(100)
	,@AdditionalFuel VARCHAR(50)
	,@RegNo VARCHAR(40)
	,@RegistrationPlace VARCHAR(40)
	,@Insurance VARCHAR(40)
	,@InsuranceExpiry DATETIME
	,@Owners VARCHAR(20)
	,@CarDriven VARCHAR(20)
	,@Tax VARCHAR(20)
	,@Mileage VARCHAR(20)
	,@Accidental BIT
	,@FloodAffected BIT
	,@InteriorColor VARCHAR(100)
	,@CWSellInquiryId BIGINT
	,@SafetyFeatures VARCHAR(500)
	,@ComfortFeatures VARCHAR(500)
	,@OtherFeatures VARCHAR(500)
	,
	-- vehicle condition --            
	@AirConditioning VARCHAR(50)
	,@Brakes VARCHAR(50)
	,@Battery VARCHAR(50)
	,@Electricals VARCHAR(50)
	,@Engine VARCHAR(50)
	,@Exterior VARCHAR(50)
	,@Seats VARCHAR(50)
	,@Suspensions VARCHAR(50)
	,@Tyres VARCHAR(50)
	,@Interior VARCHAR(50)
	,@Overall VARCHAR(50)
	,@Comments VARCHAR(500)
	,@Modifications VARCHAR(500)
	,@Warranties VARCHAR(500)
	,@CW_CustomerId BIGINT = NULL
	,@Status SMALLINT OUTPUT
	,@LeadDivertedTo VARCHAR(100) OUTPUT
	,@ExcelInquiryId BIGINT
	,--Imported from excel
	@InquiryOtherSourceId TINYINT = NULL
	,@LeadOwnId BIGINT = NULL OUTPUT
	,@CustomerId BIGINT = NULL OUTPUT
	,@LeadIdOutput BIGINT = NULL OUTPUT
	,@INQLeadIdOutput BIGINT = NULL OUTPUT
	,@EntryDate DATETIME = NULL
	)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION ProcessBuyerInquiries

		SET @EntryDate = ISNULL(@EntryDate, GETDATE())

		DECLARE @CarDetails VARCHAR(MAX)

		SET @Status = 0

		-- New seller inquiry
		IF (@TC_SellerInquiryId IS NULL)
		BEGIN
			--Step 1: Registering Customer for new inquiry
			--DECLARE @CustomerId BIGINT
			DECLARE @CustStatus SMALLINT
			DECLARE @ActiveLeadId BIGINT

			EXEC TC_CustomerDetailSave @BranchId = @BranchId
				,@CustomerEmail = @Email
				,@CustomerName = @CustomerName
				,@CustomerMobile = @mobile
				,@Location = @Location
				,@Buytime = @Buytime
				,@Comments = NULL
				,@CreatedBy = @TC_UserId
				,@Address = NULL
				,@SourceId = @InquirySource
				,@CustomerId = @CustomerId OUTPUT
				,@Status = @CustStatus OUTPUT
				,@ActiveLeadId = @ActiveLeadId OUTPUT
				,@CW_CustomerId = @CW_CustomerId

			IF (@CustStatus = 1) --Customer Is Fake
			BEGIN
				SET @Status = 99
				SET @TC_SellerInquiryId = - 1
			END
			ELSE
			BEGIN
				--DECLARE @LeadIdOutput BIGINT,
				--DECLARE	@INQLeadIdOutput BIGINT
				DECLARE @INQDate DATETIME = @EntryDate --GETDATE()

				SELECT @CarDetails = V.Make + ' ' + V.Model + ' ' + V.Version + ' '
				FROM vwMMV V
				WHERE V.VersionId = @VersionId

				EXECUTE TC_INQLeadSave @AutoVerified = @AutoVerified
					,@BranchId = @BranchId
					,@CustomerId = @CustomerId
					,@LeadOwnerId = @LeadOwnerId
					,@Eagerness = @Eagerness
					,@CreatedBy = @TC_UserId
					,@InquirySource = @InquirySource
					,@LeadId = @ActiveLeadId
					,@INQDate = @INQDate
					,@LeadInqTypeId = 2
					,@CarDetails = @CarDetails
					,@LeadStage = NULL
					,@LeadIdOutput = @LeadIdOutput OUTPUT
					,@INQLeadIdOutput = @INQLeadIdOutput OUTPUT
					,@NextFollowupDate = NULL
					,@FollowupComments = @Comments
					,@ReturnStatus = @Status OUTPUT
					,@LeadDivertedTo = @LeadDivertedTo OUTPUT
					,@LeadOwnId = @LeadOwnId OUTPUT
					,@LatestVersionId = @VersionId

				-- Step2: Inserting new record of Seller Inquiry
				IF (@INQLeadIdOutput IS NOT NULL)
				BEGIN
					--IF NOT EXISTS( SELECT TOP 1 TC_SellerInquiriesId FROM TC_SellerInquiries WHERE TC_InquiriesLeadId=@INQLeadIdOutput)
					--BEGIN
					INSERT INTO TC_SellerInquiries (
						TC_InquiriesLeadId
						,Price
						,Kms
						,MakeYear
						,Colour
						,RegNo
						,Comments
						,RegistrationPlace
						,Insurance
						,InsuranceExpiry
						,Owners
						,CarDriven
						,Tax
						,CityMileage
						,AdditionalFuel
						,Accidental
						,FloodAffected
						,InteriorColor
						,CWInquiryId
						,Warranties
						,Modifications
						,ACCondition
						,BatteryCondition
						,BrakesCondition
						,ElectricalsCondition
						,EngineCondition
						,ExteriorCondition
						,InteriorCondition
						,SeatsCondition
						,SuspensionsCondition
						,TyresCondition
						,OverallCondition
						,Features_SafetySecurity
						,Features_Comfort
						,Features_Others
						,LastUpdatedDate
						,CarVersionId
						,TC_InquirySourceId
						,CreatedOn
						,CreatedBy /*,TC_InquiryOtherSourceId*/
						)
					VALUES (
						@INQLeadIdOutput
						,@Price
						,@Kilometers
						,@MakeYear
						,@Color
						,@RegNo
						,@Comments
						,@RegistrationPlace
						,@Insurance
						,@InsuranceExpiry
						,@Owners
						,@CarDriven
						,@Tax
						,@Mileage
						,@AdditionalFuel
						,@Accidental
						,@FloodAffected
						,@InteriorColor
						,@CWSellInquiryId
						,@Warranties
						,@Modifications
						,@AirConditioning
						,@Battery
						,@Brakes
						,@Electricals
						,@Engine
						,@Exterior
						,@Interior
						,@Seats
						,@Suspensions
						,@Tyres
						,@Overall
						,@SafetyFeatures
						,@ComfortFeatures
						,@OtherFeatures
						,@INQDate
						,@VersionId
						,@InquirySource
						,/*GETDATE()*/ @EntryDate
						,@TC_UserId /*,@InquiryOtherSourceId*/
						)

					--SET @Status=1
					SET @TC_SellerInquiryId = SCOPE_IDENTITY()

					--IF (
					--		@CWSellInquiryId IS NOT NULL
					--		AND @CW_CustomerId IS NOT NULL
					--		)
					--BEGIN
					--	IF EXISTS (
					--			SELECT TOP 1 C.Id
					--			FROM CarPhotos C WITH (NOLOCK)
					--			INNER JOIN CustomerSellInquiries SI WITH (NOLOCK) ON SI.Id = C.InquiryId
					--			WHERE C.InquiryId = @CWSellInquiryId
					--				AND C.IsDealer = 0
					--			)
					--	BEGIN
					--		INSERT INTO TC_SellCarPhotos (
					--			TC_SellerInquiriesId
					--			,ImageUrlFull
					--			,ImageUrlThumb
					--			,ImageUrlThumbSmall
					--			,DirectoryPath
					--			,IsMain
					--			,IsActive
					--			,HostUrl
					--			,IsReplicated
					--			,EntryDate
					--			,OriginalImgPath
					--			)
					--		SELECT @TC_SellerInquiryId
					--			,ImageUrlFull
					--			,ImageUrlThumb
					--			,ImageUrlThumbSmall
					--			,DirectoryPath
					--			,IsMain
					--			,IsActive
					--			,HostUrl
					--			,IsReplicated
					--			,GETDATE()
					--			,OriginalImgPath
					--		FROM CarPhotos WITH (NOLOCK)
					--		WHERE InquiryId = @CWSellInquiryId
					--			AND IsDealer = 0 -- Modified by : Tejashree Patil on 24 July 2013
					--	END
					--END
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
						UPDATE SI
						SET TC_SellerInquiriesId = @TC_SellerInquiryId
							,IsNew = @IsNewImportedInq
							,IsDeleted = 1
							,LeadOwnerId = @LeadOwnerId
							,ModifiedDate = GETDATE()
						FROM TC_ImportSellerInquiries SI
						INNER JOIN TC_Users U WITH (NOLOCK) ON U.Id = SI.UserId
						WHERE TC_ImportSellerInquiriesId = @ExcelInquiryId
							AND SI.BranchId = @BranchId
							AND IsValid = 1
					END
				END
			END
		END
				-- Update existing seller inquiry record
		ELSE
		BEGIN
			IF EXISTS (
					SELECT TOP 1 TC_SellerInquiriesId
					FROM TC_SellerInquiries S WITH (NOLOCK)
					INNER JOIN TC_InquiriesLead IL WITH (NOLOCK) ON S.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
					WHERE S.TC_SellerInquiriesId = @TC_SellerInquiryId
						AND IL.BranchId = @BranchId
						AND IsPurchased = 0
					)
			BEGIN
				SELECT @CarDetails = Make + ' ' + Model + ' ' + Version
				FROM vwMMV
				WHERE VersionId = @VersionId

				UPDATE INQL
				SET CarDetails = @CarDetails
				FROM TC_InquiriesLead INQL
				INNER JOIN TC_SellerInquiries SI WITH (NOLOCK) ON INQL.TC_InquiriesLeadId = SI.TC_InquiriesLeadId
				WHERE SI.TC_SellerInquiriesId = @TC_SellerInquiryId

				UPDATE TC_SellerInquiries
				SET Price = @Price
					,Kms = @Kilometers
					,MakeYear = @MakeYear
					,Colour = @Color
					,RegNo = @RegNo
					,Comments = @Comments
					,RegistrationPlace = @RegistrationPlace
					,Insurance = @Insurance
					,InsuranceExpiry = @InsuranceExpiry
					,Owners = @Owners
					,CarDriven = @CarDriven
					,Tax = @Tax
					,CityMileage = @Mileage
					,AdditionalFuel = @AdditionalFuel
					,Accidental = @Accidental
					,FloodAffected = @FloodAffected
					,InteriorColor = @InteriorColor
					,Warranties = @Warranties
					,Modifications = @Modifications
					,ACCondition = @AirConditioning
					,BatteryCondition = @Battery
					,BrakesCondition = @Brakes
					,ElectricalsCondition = @Electricals
					,EngineCondition = @Engine
					,ExteriorCondition = @Exterior
					,InteriorCondition = @Interior
					,SeatsCondition = @Seats
					,SuspensionsCondition = @Suspensions
					,TyresCondition = @Tyres
					,OverallCondition = @Overall
					,Features_SafetySecurity = @SafetyFeatures
					,Features_Comfort = @ComfortFeatures
					,Features_Others = @OtherFeatures
					,LastUpdatedDate = @INQDate
					,ModifiedBy = @TC_UserId
					,CarVersionId = @VersionId
					,TC_InquirySourceId = @InquirySource
				WHERE TC_SellerInquiriesId = @TC_SellerInquiryId
					--SET @Status=1
			END
		END

		DECLARE @TC_UsersId BIGINT

		SELECT DISTINCT @TC_UsersId = U.Id
		FROM TC_Users U
		INNER JOIN TC_UsersRole R ON R.UserId = U.Id
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

		COMMIT TRANSACTION ProcessBuyerInquiries

		RETURN @TC_SellerInquiryId
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION ProcessBuyerInquiries

		INSERT INTO TC_Exceptions (
			Programme_Name
			,TC_Exception
			,TC_Exception_Date
			,InputParameters
			)
		VALUES (
			'TC_INQSellerSave'
			,(ERROR_MESSAGE() + 'ERROR_NUMBER(): ' + CONVERT(VARCHAR, ERROR_NUMBER()))
			,GETDATE()
			,' @TC_SellerInquiryId:' + CAST(@TC_SellerInquiryId AS VARCHAR(50)) + ' @BranchId:' + CAST(@BranchId AS VARCHAR(50)) + ' @VersionId:' + CAST(@VersionId AS VARCHAR(50)) + ' @AutoVerified:' + CAST(@AutoVerified AS VARCHAR(5)) + ' @LeadOwnerId:' + CAST(@LeadOwnerId AS VARCHAR(50)) + ' @CustomerName:' + @CustomerName + ' @Email:' + @Email + ' @Mobile:' + @Mobile + ' @Location:' + @Location + ' @Buytime:' + @Buytime + ' @Eagerness:' + CAST(@Eagerness AS VARCHAR(50)) + ' @InquirySource:' + CAST(@InquirySource AS VARCHAR(50)) + ' @TC_UserId:' + CAST(@TC_UserId AS VARCHAR(50)) + ' @MakeYear:' + CAST(@MakeYear AS VARCHAR(50)) + ' @Price:' + CAST(@Price AS VARCHAR(50)) + ' @Kilometers:' + CAST(@Kilometers AS VARCHAR(50)) + ' @Color:' + @Color + ' @AdditionalFuel:' + @AdditionalFuel + ' @RegNo:' + @RegNo + ' @RegistrationPlace:' + @RegistrationPlace + ' @Insurance:' + @Insurance + ' @InsuranceExpiry:' + CAST(@InsuranceExpiry AS VARCHAR(50)) + ' @Owners:' + @Owners + ' @CarDriven:' + @CarDriven + ' @Tax:' + @Tax + ' @Mileage:' + @Mileage + ' @Accidental:' + CAST(@Accidental AS VARCHAR(50)) + ' @FloodAffected:' + CAST(
				@FloodAffected AS VARCHAR(50)) + ' @InteriorColor:' + CAST(@InteriorColor AS VARCHAR(50)) + ' @CWSellInquiryId:' + CAST(@CWSellInquiryId AS VARCHAR(50)) + ' @SafetyFeatures:' + @SafetyFeatures + ' @ComfortFeatures:' + @ComfortFeatures + ' @OtherFeatures:' + @OtherFeatures + ' @AirConditioning:' + @AirConditioning + ' @Brakes:' + @Brakes + '  @Battery:' + @Battery + ' @Electricals:' + @Electricals + ' @Engine:' + @Engine + ' @Exterior:' + @Exterior + ' @Seats:' + @Seats + ' @Suspensions:' + @Suspensions + ' @Tyres:' + @Tyres + '  @Interior:' + @Interior + ' @Overall:' + @Overall + '  @Comments:' + @Comments + ' @Modifications: ' + @Modifications + ' @Warranties : ' + @Warranties + '  @CW_CustomerId : ' + CAST(@CW_CustomerId AS VARCHAR(50))
			)
			--SELECT ERROR_NUMBER() AS ErrorNumber;
	END CATCH;
END

/****** Object:  StoredProcedure [dbo].[TC_SellCarPhotosInsert]    Script Date: 8/14/2015 11:50:57 AM ******/
SET ANSI_NULLS ON
