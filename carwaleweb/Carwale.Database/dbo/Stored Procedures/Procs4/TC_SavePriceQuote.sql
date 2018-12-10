IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SavePriceQuote]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SavePriceQuote]
GO

	 
-- =================================================================================================================
-- Author		: Suresh Prajapati
-- Created On	: 04th Feb, 2016
-- Description	: To Save PQ Request Details
-- Steps		: 1. If multiple PQ request exists for same VersionId then update data in TC_PriceQuoteRequests table
--				  2. Delete/Insert data from/in TC_PQRequestComponent  
--				  3. Insert data into TC_DispositionLog table
--				  4. Update PQDate,PQStatus,NC.PQRequestedDate in table TC_NewCarInquiries
-- Modified By : Suresh Prajapati on 03rd Feb, 2016
-- Description : Added Dealers's CityId from Adding PQ details
-- Modified By : Ashwini Dhamankar on March 14,2016 (added EMI parameters and inserted into TC_PQRequestEMI and TC_PQRequestEMILog tables)
-- =================================================================================================================
CREATE PROCEDURE [dbo].[TC_SavePriceQuote] @InquiryId INT
	,@BranchId INT = NULL
	,@InqStatus SMALLINT = NULL
	,@VersionId INT = NULL
	,@CustomerId INT = NULL
	,@CustomerName VARCHAR(50) = NULL
	,@CustomerMobile VARCHAR(15) = NULL
	,@CustomerEmail VARCHAR(50) = NULL
	,@CityId INT = NULL
	,@UserId INT = NULL
	,@TC_InquirySourceId INT = 4
	,@ExShowroomPrice INT = NULL
	,@RTO INT = NULL
	,@Insurance INT = NULL
	,@OnRoadPrice INT = NULL
	,@PQComponents TC_PQ_Components READONLY
	,@Tenure INT = NULL
	,@LoanAmount VARCHAR(100) = NULL
	,@RateOfInterest VARCHAR(50) = NULL
	,@DownPayment VARCHAR(100) = NULL
	,@EMIAmount INT = NULL
	,@EMICalculatedOn TinyINT = NULL
AS
BEGIN
	IF (ISNULL(@CityId, 0) = 0)
	BEGIN
		SELECT @CityId = CityId
		FROM Dealers WITH (NOLOCK)
		WHERE ID = @BranchId
	END

	BEGIN TRY
		BEGIN TRANSACTION TC_SavePriceQuoteTransaction

		DECLARE @PQRequestId INT = NULL
			,@TC_InquiryId INT
			,@LeadDivertedTo VARCHAR(50)
			,@IsNew BIT
			,@LeadOwnId INT
			,@LeadIdOutput INT
			,@PQStatus SMALLINT = 25
			,@TC_PriceQuoteRequestsId INT = NULL

		-- Inquiry and Lead Processing
		IF (@InquiryId IS NULL)
		BEGIN
			IF (
					@CustomerMobile IS NULL
					OR @CustomerId IS NOT NULL
					)
			BEGIN
				SELECT @CustomerMobile = Mobile
					,@CustomerEmail = Email
					,@CustomerName = CD.CustomerName
				FROM TC_CustomerDetails CD WITH (NOLOCK)
				WHERE Id = @CustomerId
			END

			EXEC TC_INQNewCarBuyerSave @CustomerName
				,@CustomerEmail
				,@CustomerMobile
				,@VersionId
				,@CityId
				,NULL --@Buytime
				,@TC_InquirySourceId
				,NULL --@Eagerness
				,NULL --@TC_CustomerId
				,1 --@AutoVerified
				,@BranchId
				,@UserId
				,@UserId
				,@InqStatus OUTPUT --@Status
				,NULL --@PQReqDate
				,NULL --@TDReqDate
				,NULL --@ModelId
				,NULL --@FuelType
				,NULL --@Transmission
				,NULL --@CW_CustomerId
				,NULL --@CWInquiryId
				,@InquiryId OUTPUT
				,@LeadDivertedTo OUTPUT
				,@IsNew OUTPUT
				,@TC_InquirySourceId --@InquiryOtherSourceId
				,NULL --@ExcelInquiryId
				,@LeadOwnId OUTPUT
				,@CustomerId OUTPUT
				,@LeadIdOutput OUTPUT
				--IF NOT EXISTS (
				--		SELECT TOP 1 1
				--		FROM TC_NewCarInquiries WITH (NOLOCK)
				--		WHERE TC_NewCarInquiriesId = @InquiryId
				--			AND VersionId = @VersionId
				--		)
				--BEGIN
				--	-- hence a New Inquiry has to be created
				--END
		END

		SELECT @PQRequestId = Id
		FROM TC_PriceQuoteRequests WITH (NOLOCK)
		WHERE TC_InquiriesId = @InquiryId
			AND VersionId = @VersionId
		ORDER BY Id DESC

		-- Check is PQ already requested for same VersionId and InquiryId
		IF (ISNULL(@PQRequestId, 0) = 0)   --if does not exists 
		BEGIN
			-- i.e New PQ Request
			-- Insert into TC_PriceQuoteRequests Table
			INSERT INTO TC_PriceQuoteRequests (
				TC_InquiriesId
				,VersionId
				,CityId
				,ExShowRoomPrice
				,RTO
				,Insurance
				,OnRoadPrice
				,UserId
				,PQDate
				)
			VALUES (
				@InquiryId
				,@VersionId
				,@CityId
				,@ExShowroomPrice
				,@RTO
				,@Insurance
				,@OnRoadPrice
				,@UserId
				,GETDATE()
				)

			SET @TC_PriceQuoteRequestsId = SCOPE_IDENTITY()

			-- Insert Into TC_PQRequestComponent Table
			INSERT INTO TC_PQRequestComponent (
				TC_PriceQuoteRequestsId
				,TC_PQComponentId
				,Amount
				)
			SELECT @TC_PriceQuoteRequestsId
				,ComponentId
				,ComponentPrice
			FROM @PQComponents

			--Addded by Ashwini Dhamankar on March 14,2016
			IF(@EMIAmount IS NOT NULL)
			BEGIN
				INSERT INTO TC_PQRequestEMI
				(
				TC_PriceQuoteRequestsId
				,Tenure
				,LoanAmount
				,RateOfInterest
				,DownPayment
				,EMIAmount
				,EntryDate
				,EMICalculatedOn
				)
				VALUES
				(
				@TC_PriceQuoteRequestsId
				,@Tenure
				,@LoanAmount
				,@RateOfInterest
				,@DownPayment
				,@EMIAmount
				,GETDATE()
				,@EMICalculatedOn
				)
			END

		END
		ELSE
		BEGIN
			--Insert into log
			INSERT INTO TC_PriceQuoteRequestsLog (
				TC_PriceQuoteRequestsId
				,TC_InquiriesId
				,CityId
				,VersionId
				,ExShowRoomPrice
				,RTO
				,Insurance
				,OnRoadPrice
				,UserId
				,PQDate
				)
			SELECT Id
				,TC_InquiriesId
				,CityId
				,VersionId
				,ExShowRoomPrice
				,RTO
				,Insurance
				,OnRoadPrice
				,UserId
				,PQDate
			FROM TC_PriceQuoteRequests WITH (NOLOCK)
			WHERE Id = @PQRequestId

			-- IF EXISTS HENCE UPDATE DETAILS
			UPDATE TC_PriceQuoteRequests
			SET ExShowRoomPrice = @ExShowroomPrice
				,RTO = @RTO
				,Insurance = @Insurance
				,OnRoadPrice = @OnRoadPrice
				,ModifiedDate = GETDATE()
				,ModifiedBy = @UserId
			WHERE Id = @PQRequestId

			-- Insert into log table and Delete old Components Amount
			INSERT INTO TC_PQRequestComponentLog (
				TC_PriceQuoteRequestsId
				,TC_PQComponentId
				,Amount
				,EntryDate
				)
			SELECT TC_PriceQuoteRequestsId
				,TC_PQComponentId
				,Amount
				,GETDATE()
			FROM TC_PQRequestComponent WITH (NOLOCK)
			WHERE TC_PriceQuoteRequestsId = @PQRequestId

			DELETE
			FROM TC_PQRequestComponent
			WHERE TC_PriceQuoteRequestsId = @PQRequestId

			-- Insert new components Into TC_PQRequestComponent Table
			INSERT INTO TC_PQRequestComponent (
				TC_PriceQuoteRequestsId
				,TC_PQComponentId
				,Amount
				)
			SELECT @PQRequestId
				,ComponentId
				,ComponentPrice
			FROM @PQComponents


			--Added by Ashwini Dhamankar on March 14,2016
			IF(@EMIAmount IS NOT NULL)
			BEGIN
				INSERT INTO TC_PQRequestEMILog
				(
				TC_PQRequestEMIId
				,TC_PriceQuoteRequestsId
				,Tenure
				,LoanAmount
				,RateOfInterest
				,DownPayment
				,EMIAmount
				,EntryDate
				,EMICalculatedOn
				)
				SELECT 
				TC_PQRequestEMIId
				,TC_PriceQuoteRequestsId
				,Tenure
				,LoanAmount
				,RateOfInterest
				,DownPayment
				,EMIAmount
				,GETDATE()
				,EMICalculatedOn
				FROM TC_PQRequestEMI WITH(NOLOCK)
				WHERE TC_PriceQuoteRequestsId = @PQRequestId

				UPDATE TC_PQRequestEMI
				SET Tenure = @Tenure
				,LoanAmount = @LoanAmount
				,RateOfInterest = @RateOfInterest
				,DownPayment = @DownPayment
				,EMIAmount = @EMIAmount
				,EntryDate = GETDATE()
				,EMICalculatedOn = @EMICalculatedOn
				WHERE TC_PriceQuoteRequestsId = @PQRequestId
			END
		END

		--Update PQDate,PQStatus,NC.PQRequestedDate in table TC_NewCarInquiries
		UPDATE TC_NewCarInquiries
		SET PQDate = GETDATE()
			,PQStatus = @PQStatus
			,PQRequestedDate = GETDATE()
		WHERE TC_NewCarInquiriesId = @InquiryId

		--Insert TC_DispositionLog table , call existing SP dbo.TC_DispositionLogInsert
		EXEC TC_DispositionLogInsert @UserId
			,@PQStatus
			,@InquiryId
			,5
			,@LeadIdOutput

		--If multiple price quotes are sent, the recent PQ sent on a particular inquiry should get tagged as the most active inquiry.
		EXEC TC_UpdateMostInterestedInquiry @BranchId
			,@InquiryId
			,3

		COMMIT TRANSACTION TC_SavePriceQuoteTransaction
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION TC_SavePriceQuoteTransaction

		INSERT INTO TC_Exceptions (
			Programme_Name
			,TC_Exception
			,TC_Exception_Date
			,InputParameters
			)
		VALUES (
			'TC_SavePriceQuote'
			,(ERROR_MESSAGE() + 'ERROR_NUMBER(): ' + CONVERT(VARCHAR, ERROR_NUMBER()))
			,GETDATE()
			,+ '@InquiryId : ' + ISNULL(CAST(@InquiryId AS VARCHAR(50)), 'NULL') + '@BranchId:' + ISNULL(CAST(@BranchId AS VARCHAR(50)), 'NULL') + '@InqStatus:' + ISNULL(CAST(@InqStatus AS VARCHAR(50)), 'NULL') + '@VersionId:' + ISNULL(CAST(@VersionId AS VARCHAR(50)), 'NULL') + '@CustomerId:' + ISNULL(CAST(@CustomerId AS VARCHAR(50)), 'NULL') + '@CustomerName:' + ISNULL(@CustomerName, 'NULL') + '@CustomerMobile:' + ISNULL(@CustomerMobile, 'NULL') + '@CustomerEmail:' + ISNULL(@CustomerEmail, 'NULL') + '@CityId:' + ISNULL(CAST(@CityId AS VARCHAR(50)), 'NULL') + '@UserId:' + ISNULL(CAST(@UserId AS VARCHAR(50)), 'NULL') + '@TC_InquirySourceId:' + ISNULL(CAST(@TC_InquirySourceId AS VARCHAR(50)), 'NULL') + '@ExShowroomPrice:' + ISNULL(CAST(@ExShowroomPrice AS VARCHAR(50)), 'NULL') + '@RTO:' + ISNULL(CAST(@RTO AS VARCHAR(50)), 'NULL') + '@Insurance:' + ISNULL(CAST(@Insurance AS VARCHAR(50)), 'NULL') + '@OnRoadPrice:' + ISNULL(CAST(@OnRoadPrice AS VARCHAR(50)), 'NULL')
			)
	END CATCH;
END
