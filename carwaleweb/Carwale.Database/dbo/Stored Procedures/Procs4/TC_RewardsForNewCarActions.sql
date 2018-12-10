IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RewardsForNewCarActions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RewardsForNewCarActions]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 24-04-2015
-- Description:	Reward Calculations
-- Modified By Vivek on 3-7-2015, changed conditions for first call to lead
-- Modified By Vivek Gupta on 29-07-2015, stopped giving points to SM and BookingDate changed to bookingevntdate
-- Modified By : Nilima More on 19th OCT 2015, reward points should be given only on CarWale leads.
-- Modified By Vivek Gupta on 06-11-2015, added @Userid parameter to calculater points user wise(not dealer wise)
--Modified on 18th Feb 2016 by Deepak Tripathi
--No Points for Used Car Dealers
--New Car Dealer Will have points for TD and Booking 
-- =============================================
CREATE PROCEDURE [dbo].[TC_RewardsForNewCarActions]
@BranchId INT,
@TC_DealerTypeId INT,
@Date DATETIME,

@TDCP NUMERIC = 25,--100, --@TDCompletion -- Changed from 100 to 25 on 18th feb 2016 by deepak/Arnab
@CBTCP NUMERIC = 50, --100, --@carsBookedThroughCarwale -- Changed from 100 to 50 on 18th feb 2016 by deepak/Arnab

@LP NUMERIC = 5,--@Login 
@PQP NUMERIC = 100, --@PQIn24Hours
@ALSP NUMERIC = 10,--@ActiveLeadsInSystem
@DOCP NUMERIC = 0, --@DuratonOfContract, is the sum of points earned through booking, td completion and pq calls 
@WNP NUMERIC = 500, --@WebsiteNew
@SEMP NUMERIC = 500, --@SEM
@CBP NUMERIC = 500, --@ContextualBranding
@DOCPSM NUMERIC = 0,
@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--select * from TC_RewardPoints
	--select * from TC_DealerDailyRewardPoints

	--DECLARE @LP NUMERIC = 5,--@Login 
	--	    @PQP NUMERIC = 100, --@PQIn24Hours
	--		@TDCP NUMERIC = 100, --@TDCompletion
	--		@ALSP NUMERIC = 10,--@ActiveLeadsInSystem
	--		@CBTCP NUMERIC = 100, --@carsBookedThroughCarwale
	--		@DOCP NUMERIC = 0, --@DuratonOfContract, is the sum of points earned through booking, td completion and pq calls
	--		@WNP NUMERIC = 500, --@WebsiteNew
	--		@SEMP NUMERIC = 500, --@SEM
	--		@CBP NUMERIC = 500, --@ContextualBranding
	--		@DOCPSM NUMERIC = 0 --is the sum of points earned through booking, td completion and pq calls om points given to SM


    DECLARE @LPId SMALLINT = 1,--@LoginId
		    @PQPId SMALLINT = 13, --@PQIn24HoursId
			@TDCPId SMALLINT = 14, --@TDCompletionId
			@ALSPId SMALLINT = 15,--@ActiveLeadsInSystemId
			@CBTCPId SMALLINT = 16, --@carsBookedThroughCarwaleId
			@DOCPId SMALLINT = 17, --@DuratonOfContractId
			@WNPId SMALLINT = 18, --@WebsiteNewId
			@SEMPId SMALLINT = 19, --@SEMId
			@CBPId SMALLINT = 20 --@ContextualBrandingId


	-- Declaring Different Variables to get different points of the dealer

	DECLARE @LoginPW NUMERIC = 0,
			@PriceQuotePW NUMERIC = 0,
			@TDCompletionPW NUMERIC = 0,
			@ActiveLeadsPW NUMERIC = 0,
			@CarsBookedPW NUMERIC = 0,
			@ContractDurationPW NUMERIC = 0,
			@WebsiteNCPW NUMERIC = 0,
			@SEMPointsW NUMERIC = 0,
			@ContextualBrandingPW NUMERIC = 0
     
    DECLARE @LoginPA NUMERIC = 0,
			@PriceQuotePA NUMERIC = 0,
			@TDCompletionPA NUMERIC = 0,
			@ActiveLeadsPA NUMERIC = 0,
			@CarsBookedPA NUMERIC = 0,
			@ContractDurationPA NUMERIC = 0,
			@WebsiteNCPA NUMERIC = 0,
			@SEMPointsA NUMERIC = 0,
			@ContextualBrandingPA NUMERIC = 0,

			@CarsBookedPSM NUMERIC = 0,
			@ContractDurationPSM NUMERIC = 0,
			@WebsiteNCPSM NUMERIC = 0,
			@SEMPointsSM NUMERIC = 0,
			@ContextualBrandingPSM NUMERIC = 0
     
	DECLARE @EntryDate DATETIME
	SET @EntryDate = CONVERT(DATE,@Date)

		--Calculations for New car Dealers

			
			-- Login Points Calculation
			--1. Each user will be rewarded for their first login each calendar day as per IST.
			--2. Only 1 login per calendar day can be rewarded.
			--3. Max one login per day
			-- Commented on 18th Feb 2016 By Deepak
			
			/*IF EXISTS (SELECT Top 1 Id FROM TC_UsersLog WITH(NOLOCK) WHERE CONVERT(DATE,LoggedTime) = CONVERT(DATE,@Date) AND ISNULL(LoginFrom, 'Web') = 'Android'  AND BranchId = @BranchId  AND UserId = @UserId ORDER BY LoggedTime)
			BEGIN
			  SET @LoginPA = @LP
			END
			ELSE IF EXISTS (SELECT Top 1 Id FROM TC_UsersLog WITH(NOLOCK) WHERE CONVERT(DATE,LoggedTime) = CONVERT(DATE,@Date)  AND BranchId = @BranchId AND UserId = @UserId ORDER BY LoggedTime)
			BEGIN
			  SET @LoginPW = @LP
			END
			-- inserting reward points to reward table
			EXEC TC_InsertRewardPoints  @DealerId = @BranchId,
										@EntryDate = @EntryDate,
										@TC_DealerTypeId = @TC_DealerTypeId,
										@TC_RewardPointsId = @LPId,
										@RewardPoints = @LP,
										@TotalRewardsFromWeb = @LoginPW,
										@TotalRewardsFromApp = @LoginPA,
										@UserId = @UserId*/


			--Calls taken on leads came after 6 pm yesterday and before 6pm today
			--This should just be a per lead called on the same day for leads received till 6pm, 
			--and on next calendar day for all leads received after 6pm. Max once per lead.
			   --Actions from web
			-- Commented on 18th Feb 2016 By Deepak
			/*
			DECLARE @CallLeadCount NUMERIC = 0
			SET @CallLeadCount = 0
			SELECT @CallLeadCount = COUNT(DISTINCT C.TC_LeadId) FROM TC_Lead L WITH(NOLOCK)
			JOIN   TC_Calls C WITH(NOLOCK) ON L.TC_LeadId = C.TC_LeadId AND C.IsActionTaken = 1 AND C.CallType = 1 AND L.LeadType = 3 AND ISNULL(C.TC_UsersId,@UserId) = @UserId AND ISNULL(C.TC_ActionApplicationId,1) = 1
			JOIN TC_InquirySource S  WITH(NOLOCK) ON L.TC_InquirySourceId = S.Id  
			WHERE 
				 L.BranchId = @BranchId
				AND C.ScheduledOn >  DATEADD(day, DATEDIFF(day, 0, @Date - 1), '18:00:00.000')
				--AND L.LeadCreationDate <  DATEADD(day, DATEDIFF(day, 0, GETDATE()), '18:00:00.000')
				AND L.LeadCreationDate BETWEEN  DATEADD(day, DATEDIFF(day, 1, @Date), '18:00:00.000') AND DATEADD(day, DATEDIFF(day, 0, @Date), '18:00:00.000')
				AND C.ActionTakenOn <= DATEADD(day, DATEDIFF(day, 0, @Date), '18:00:00.000')
				AND S.TC_InquiryGroupSourceId = 11
			SET @PriceQuotePW = @PQP * @CallLeadCount
			
			-- Actions from App
			SELECT @CallLeadCount = COUNT(DISTINCT C.TC_LeadId) FROM TC_Lead L WITH(NOLOCK)
			JOIN   TC_Calls C WITH(NOLOCK) ON L.TC_LeadId = C.TC_LeadId AND C.IsActionTaken = 1 AND C.CallType = 1 AND L.LeadType = 3 AND ISNULL(C.TC_UsersId,@UserId) = @UserId AND ISNULL(C.TC_ActionApplicationId,1) = 2
			JOIN TC_InquirySource S  WITH(NOLOCK) ON S.Id = L.TC_InquirySourceId   
			WHERE 
				 L.BranchId = @BranchId
				AND C.ScheduledOn >  DATEADD(day, DATEDIFF(day, 0, @Date - 1), '18:00:00.000')
				--AND L.LeadCreationDate <  DATEADD(day, DATEDIFF(day, 0, GETDATE()), '18:00:00.000')
				AND L.LeadCreationDate BETWEEN  DATEADD(day, DATEDIFF(day, 1, @Date), '18:00:00.000') AND DATEADD(day, DATEDIFF(day, 0, @Date), '18:00:00.000')
				AND C.ActionTakenOn <= DATEADD(day, DATEDIFF(day, 0, @Date), '18:00:00.000')
				AND S.TC_InquiryGroupSourceId = 11
				
			SET @PriceQuotePA = 2 * @PQP * @CallLeadCount

			-- inserting reward points to reward table
			EXEC TC_InsertRewardPoints  @DealerId = @BranchId,
										@EntryDate = @EntryDate,
										@TC_DealerTypeId = @TC_DealerTypeId,
										@TC_RewardPointsId = @PQPId,
										@RewardPoints = @PQP,
										@TotalRewardsFromWeb = @PriceQuotePW,
										@TotalRewardsFromApp = @PriceQuotePA,
										@UserId = @UserId

			*/
		-- TD Completion points calculation
			--Same as above, give x points for TD completed in 3 days, carwale reserves the right to check all points. Only once per lead.
			DECLARE @TDCompletedCount NUMERIC = 0

			  -- Action from web
			SELECT @TDCompletedCount = COUNT(DISTINCT TC_TDCalendarId) 
			FROM TC_TDCalendar C WITH(NOLOCK) 
			JOIN TC_CustomerDetails CD WITH(NOLOCK) ON CD.Id = C.TC_CustomerId AND C.TC_UsersId = @UserId
			JOIN TC_InquirySource S WITH(NOLOCK) ON CD.TC_InquirySourceId = S.Id 
			WHERE TDStatus = 28
			AND C.BranchId = @BranchId
			AND CONVERT(DATE,TDDate) = CONVERT(DATE,@Date-3)
			--AND ISNULL(TC_ActionApplicationId,1) = 1
			AND S.TC_InquiryGroupSourceId = 11
			SET @TDCompletionPW = @TDCP * @TDCompletedCount

			-- action from App
			
			--SELECT @TDCompletedCount = COUNT(DISTINCT TC_TDCalendarId) 
			--FROM TC_TDCalendar C WITH(NOLOCK)
			--JOIN TC_CustomerDetails CD WITH(NOLOCK) ON CD.Id = C.TC_CustomerId AND C.TC_UsersId = @UserId
			--JOIN TC_InquirySource S WITH(NOLOCK) ON CD.TC_InquirySourceId = S.Id 
			--WHERE TDStatus = 28
			--AND C.BranchId = @BranchId
			--AND CONVERT(DATE,TDDate) = CONVERT(DATE,@Date-3)
			--AND ISNULL(TC_ActionApplicationId,1) = 2
			--AND S.TC_InquiryGroupSourceId = 11
			--SET @TDCompletionPA = 2*@TDCP * @TDCompletedCount

			-- inserting reward points to reward table
			EXEC TC_InsertRewardPoints  @DealerId = @BranchId,
										@EntryDate = @EntryDate,
										@TC_DealerTypeId = @TC_DealerTypeId,
										@TC_RewardPointsId = @TDCPId,
										@RewardPoints = @TDCP,
										@TotalRewardsFromWeb = @TDCompletionPW,
										@TotalRewardsFromApp = 0,--@TDCompletionPA,
										@UserId = @UserId


		-- Cars booked through carwale
			--Points per lead marked as having bought a car from me.
				-- Action from Web
			DECLARE @BookingCount NUMERIC = 0
			SELECT @BookingCount = COUNT(DISTINCT NCI.TC_NewCarInquiriesId) 
			FROM TC_InquiriesLead IL WITH(NOLOCK) 
			JOIN TC_NewCarInquiries NCI WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId AND IL.TC_UserId = @UserId
			--JOIN TC_NewCarBooking NCB WITH(NOLOCK) ON NCI.TC_NewCarInquiriesId = NCB.TC_NewCarInquiriesId
			JOIN TC_InquirySource S WITH(NOLOCK) ON NCI.TC_InquirySourceId = S.Id
			WHERE IL.BranchId = @BranchId
			--AND CONVERT(DATE,NCI.BookingDate) = CONVERT(DATE,@Date)
			AND CONVERT(DATE,NCI.BookingEventDate) = CONVERT(DATE,@Date)
			AND NCI.BookingStatus = 32 
			--AND ISNULL(NCI.TC_ActionApplicationId,1) = 1
			AND S.TC_InquiryGroupSourceId = 11

			SET @CarsBookedPW = @CBTCP * @BookingCount

				-- Actions from App
			/*SELECT @BookingCount = COUNT(NCI.TC_NewCarInquiriesId) 
			FROM TC_InquiriesLead IL WITH(NOLOCK) 
			JOIN TC_NewCarInquiries NCI WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId AND IL.TC_UserId = @UserId
			--JOIN TC_NewCarBooking NCB WITH(NOLOCK) ON NCI.TC_NewCarInquiriesId = NCB.TC_NewCarInquiriesId
			JOIN TC_InquirySource S WITH(NOLOCK) ON NCI.TC_InquirySourceId = S.Id
			WHERE IL.BranchId = @BranchId
			--AND CONVERT(DATE,NCI.BookingDate) = CONVERT(DATE,@Date)
			AND CONVERT(DATE,NCI.BookingEventDate) = CONVERT(DATE,@Date)
			AND NCI.BookingStatus = 32 
			AND ISNULL(NCI.TC_ActionApplicationId,1) = 2
			AND S.TC_InquiryGroupSourceId = 11

			SET @CarsBookedPA = 2* @CBTCP * @BookingCount

			SET @CarsBookedPSM = 2*(@CarsBookedPW + @CarsBookedPA)*/
			-- inserting reward points to reward table
			EXEC TC_InsertRewardPoints  @DealerId = @BranchId,
										@EntryDate = @EntryDate,
										@TC_DealerTypeId = @TC_DealerTypeId,
										@TC_RewardPointsId = @CBTCPId,
										@RewardPoints = @CBTCP,
										@TotalRewardsFromWeb = @CarsBookedPW,
										@TotalRewardsFromApp = 0,--@CarsBookedPA,
										@TotalRewardsToSM = 0,--@CarsBookedPSM
										@UserId = @UserId

		--Duration Of contract			 	
			--x/2 multiplier on points statement for 6 month contract, 
			--x multiplier on points statement for 12 month contract.
			-- Only for upfront payments.
			-- Commented on 18th Feb 2016 By Deepak
			
			/*
			DECLARE @Months SMALLINT

			SELECT @Months = DATEDIFF(MONTH,PackageStartDate,PackageEndDate) 
			FROM RVN_DealerPackageFeatures WITH(NOLOCK) 
			WHERE PackageId IN (40,50,51,70,71) 
			AND DealerId = @BranchId 
			AND CONVERT(DATE,PackageStartDate) = CONVERT(DATE,@Date)

			--SET @DOCP= @PriceQuotePA + @PriceQuotePW + @TDCompletionPA + @TDCompletionPW + @CarsBookedPA + @CarsBookedPW
			--SET @DOCPSM= @CarsBookedPSM

			IF(@Months > = 6 AND @Months < 12)
			BEGIN
			   SET @ContractDurationPSM = @DOCPSM/2
			   SET @ContractDurationPW =  @DOCP/2
			END

			ELSE IF(@Months > = 12)
			BEGIN
			   SET @ContractDurationPSM = @DOCPSM
			   SET @ContractDurationPW =  @DOCP
			END


			DECLARE @IsDealerPrinciple BIT = 0

			IF EXISTS (SELECT Id FROM TC_Users U WITH(NOLOCK) JOIN TC_UsersRole UR WITH(NOLOCK) ON U.Id = UR.UserId AND UR.UserId = @UserId AND UR.RoleId = 1  WHERE U.BranchId = @BranchId)
			 BEGIN
			  
			  SET @IsDealerPrinciple = 1

			  EXEC TC_InsertRewardPoints    @DealerId = @BranchId,
											@EntryDate = @EntryDate,
											@TC_DealerTypeId = @TC_DealerTypeId,
											@TC_RewardPointsId = @DOCPId,
											@RewardPoints = @DOCP,
											@TotalRewardsFromWeb = @ContractDurationPW,
											@TotalRewardsFromApp = 0,
											@TotalRewardsToSM = 0,--@ContractDurationPSM
											@UserId = @UserId

		     END
		--Website new car
			--Points are once a year
			IF EXISTS (SELECT DealerPackageFeatureID FROM RVN_DealerPackageFeatures WITH(NOLOCK) WHERE PackageId IN (36) AND DealerId = @BranchId AND CONVERT(DATE,EntryDate) = CONVERT(DATE,@Date))
			BEGIN

			IF @IsDealerPrinciple = 1
			   EXEC TC_InsertRewardPoints   @DealerId = @BranchId,
											@EntryDate = @EntryDate,
											@TC_DealerTypeId = @TC_DealerTypeId,
											@TC_RewardPointsId = @WNPId,
											@RewardPoints = @WNP,
											@TotalRewardsFromWeb = 0,
											@TotalRewardsFromApp = 0,
											@TotalRewardsToSM = @WNP,
											@UserId = @UserId
			END
		--SEM
			--Points are once a year
			IF EXISTS (SELECT DealerPackageFeatureID FROM RVN_DealerPackageFeatures WITH(NOLOCK) WHERE PackageId IN (68,69) AND DealerId = @BranchId AND CONVERT(DATE,EntryDate) = CONVERT(DATE,@Date))
			BEGIN
			IF @IsDealerPrinciple = 1
			   EXEC TC_InsertRewardPoints   @DealerId = @BranchId,
											@EntryDate = @EntryDate,
											@TC_DealerTypeId = @TC_DealerTypeId,
											@TC_RewardPointsId = @SEMPId,
											@RewardPoints = @SEMP,
											@TotalRewardsFromWeb = 0,
											@TotalRewardsFromApp = 0,
											@TotalRewardsToSM = @SEMP,
											@UserId = @UserId
			END

		--Contextual Branding
			--Points per month
			IF EXISTS (SELECT DealerPackageFeatureID FROM RVN_DealerPackageFeatures WITH(NOLOCK) WHERE PackageId IN (57,58) AND DealerId = @BranchId AND CONVERT(DATE,EntryDate) = CONVERT(DATE,@Date))
			BEGIN
			IF @IsDealerPrinciple = 1
			   EXEC TC_InsertRewardPoints   @DealerId = @BranchId,
											@EntryDate = @EntryDate,
											@TC_DealerTypeId = @TC_DealerTypeId,
											@TC_RewardPointsId = @CBPId,
											@RewardPoints = @CBP,
											@TotalRewardsFromWeb = 0,
											@TotalRewardsFromApp = 0,
											@TotalRewardsToSM = @CBP,
											@UserId = @UserId
			END*/

END