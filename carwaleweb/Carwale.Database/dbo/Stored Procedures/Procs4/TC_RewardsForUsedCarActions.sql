IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RewardsForUsedCarActions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RewardsForUsedCarActions]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 24-04-2015
-- Description:	Reward Calculations
-- Modified By: Vivek on 15-06-2015, added used car booking points
-- Modified By Vivek Gupta, on 30-06-2015, changed points calculation condition for booking
-- Modified By Vivek on 3-7-2015, changed conditions for first call to lead
-- Modified By Vivek on 9-7-2015, changed activationDate of warranty to entrydate
-- Modified By Vivek Gupta on 21-07-2015, added 5 days old condition to prevent duplicate points given to dealer on stock with 10 photos
-- Modified By Chetan Navin on 21-08-2015, Added different slabs for points on the basis of activated warranty count(monthly)
-- Modified By : Vivek GUpta, 21-08-2015, changed conditions to give points for booking used car and purchaging a car
-- Modified By : Nilima More on 19th OCT 2015, reward points should be given only on CarWale leads.
-- Modified By Vivek Gupta, on 3rd nov 2015, warranty 100 points started again, festive offers changes have been commented
-- Modified By Vivek Gupta on 4th nov, 2015, reward points is now be given user wise not dealer wise
-- Modified By Vivek Gupta on 06-11-2015, added @Userid parameter to calculater points user wise(not dealer wise)
-- Modified By : Suresh Prajapati on 25th Nov, 2015
-- Description : Added @UserId Parameter
--EXEC [TC_RewardsForUsedCarActions] 5,3,'2012-11-18 14:42:37.330', 243
-- =============================================
CREATE PROCEDURE [dbo].[TC_RewardsForUsedCarActions] @BranchId INT
	,@TC_DealerTypeId INT
	,@Date DATETIME
	,@LP NUMERIC = 5
	,--@Login 
	@COLP NUMERIC = 100
	,--@CallsOnLead -- 
	@CWPP NUMERIC = 90
	,--@CarsWithPhoto
	@WWPP NUMERIC = 50
	,--@WarrantiesPurchased
	@WAP NUMERIC = 100
	,--@WarrantiesActivated
	@CSP NUMERIC = 10
	,--@CarSold --
	@WUP NUMERIC = 500
	,--@WebsiteUsed 
	@DSP NUMERIC = 500
	,--@DigitalServices 
	@PUPP NUMERIC = 1000
	,--@PackageUpgradePremium
	@PUMP NUMERIC = 600
	,--@PackageUpgradeMaximiser
	@PUOP NUMERIC = 360
	,--@PackageUpgradeOptimiser
	@PUSP NUMERIC = 120
	,--@PackageUpgradeStarter,
	@PhotoCount NUMERIC = 10
	,@UserId INT -- Added By : Suresh Prajapati
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--select * from TC_RewardPoints
	--select * from TC_DealerDailyRewardPoints
	--DECLARE @LP NUMERIC = 5,--@Login 
	--		@COLP NUMERIC = 100, --@CallsOnLead
	--		@CWPP NUMERIC = 90, --@CarsWithPhoto
	--		@WWPP NUMERIC = 50, --@WarrantiesPurchased
	--		@WAP NUMERIC = 100, --@WarrantiesActivated
	--		@CSP NUMERIC = 10, --@CarSold
	--		@WUP NUMERIC = 500, --@WebsiteUsed
	--	    @DSP NUMERIC = 500,--@DigitalServices 
	--		@PUPP NUMERIC = 1000, --@PackageUpgradePremium
	--		@PUMP NUMERIC = 600, --@PackageUpgradeMaximiser
	--		@PUOP NUMERIC = 360, --@PackageUpgradeOptimiser
	--		@PUSP NUMERIC = 120 --@PackageUpgradeStarter
	DECLARE @LPId SMALLINT = 1
		,--@LoginId 
		@COLPId SMALLINT = 2
		,--@CallsOnLeadId
		@CWPPId SMALLINT = 3
		,--@CarsWithPhotoId
		@WWPPId SMALLINT = 4
		,--@WarrantiesPurchasedId
		@WAPId SMALLINT = 5
		,--@WarrantiesActivatedId
		@CSPId SMALLINT = 6
		,--@CarSoldId
		@WUPId SMALLINT = 7
		,--@WebsiteUsedId
		@DSPId SMALLINT = 8
		,--@DigitalServicesId 
		@PUPPId SMALLINT = 9
		,--@PackageUpgradePremiumId
		@PUMPId SMALLINT = 10
		,--@PackageUpgradeMaximiserId
		@PUOPId SMALLINT = 11
		,--@PackageUpgradeOptimiserId
		@PUSPId SMALLINT = 12 --@PackageUpgradeStarterId
	DECLARE @EntryDate DATETIME

	SET @EntryDate = CONVERT(DATE, @Date)

	-- Declaring Different Variables to get different points of the dealer
	-- Pints Variable decalred for storing points earned from web action
	DECLARE @LoginPW NUMERIC = 0
		,@CallsOnLeadPW NUMERIC = 0
		,@CarsWithPhotosPW NUMERIC = 0
		,@WarrantiesPurchasedPW NUMERIC = 0
		,@WarrantiesActivatedPW NUMERIC = 0
		,@CarSoldPW NUMERIC = 0
		,@WebsiteUCPW NUMERIC = 0
		,@DigitalServicesPW NUMERIC = 0
		,@PackageUpgradePW NUMERIC(18, 3) = 0
	-- Pints Variable decalred for storing points earned from aPP action
	DECLARE @LoginPA NUMERIC = 0
		,@CallsOnLeadPA NUMERIC = 0
		,@CarsWithPhotosPA NUMERIC = 0
		,@WarrantiesPurchasedPA NUMERIC = 0
		,@WarrantiesActivatedPA NUMERIC = 0
		,@CarSoldPA NUMERIC = 0
		,
		--@WebsiteUCPA NUMERIC = NULL,
		@DigitalServicesPA NUMERIC = 0
		,@PackageUpgradePA NUMERIC(18, 3) = 0

	-- Below Calculating each and every points of the dealers
	--Login Point Calculation	
	--1. Each user will be rewarded for their first login each calendar day as per IST.
	--2. Only 1 login per calendar day can be rewarded.
	--3. Max one login per day
	IF EXISTS (
			SELECT TOP 1 Id
			FROM TC_UsersLog WITH (NOLOCK)
			WHERE CONVERT(DATE, LoggedTime) = CONVERT(DATE, @Date)
				AND ISNULL(LoginFrom, 'Web') = 'Android'
				AND BranchId = @BranchId
				AND UserId = @UserId
			ORDER BY LoggedTime
			)
	BEGIN
		SET @LoginPA = @LP
	END
	ELSE
		IF EXISTS (
				SELECT TOP 1 Id
				FROM TC_UsersLog WITH (NOLOCK)
				WHERE CONVERT(DATE, LoggedTime) = CONVERT(DATE, @Date)
					AND BranchId = @BranchId
					AND UserId = @UserId
				)
		BEGIN
			SET @LoginPW = @LP
		END

	EXEC TC_InsertRewardPoints @DealerId = @BranchId
		,@EntryDate = @EntryDate
		,@TC_DealerTypeId = @TC_DealerTypeId
		,@TC_RewardPointsId = @LPId
		,@RewardPoints = @LP
		,@TotalRewardsFromWeb = @LoginPW
		,@TotalRewardsFromApp = @LoginPA
		,@UserId = @UserId

	--Calls taken on leads came after 8 pm yesterday and before 8pm today
	--This should just be a per lead called on the same day for leads received till 8pm, 
	--and on next calendar day for all leads received after 8pm, not as a percentage criteria. 
	--Max once per lead. This also enables daily reporting then.
	--Actions from web
	DECLARE @CallLeadCount NUMERIC = 0

	SELECT @CallLeadCount = COUNT(DISTINCT C.TC_LeadId)
	FROM TC_Lead L WITH (NOLOCK)
	JOIN TC_Calls C WITH (NOLOCK) ON L.TC_LeadId = C.TC_LeadId
		AND C.IsActionTaken = 1
		AND C.CallType = 1
		AND L.LeadType = 1
		AND ISNULL(C.TC_UsersId, @UserId) = @UserId
		AND ISNULL(C.TC_ActionApplicationId, 1) = 1
	JOIN TC_InquirySource S WITH (NOLOCK) ON L.TC_InquirySourceId = S.Id
	WHERE L.BranchId = @BranchId
		AND C.ScheduledOn > DATEADD(day, DATEDIFF(day, 0, @Date - 1), '20:00:00.000')
		AND C.ActionTakenOn <= DATEADD(day, DATEDIFF(day, 0, @Date), '20:00:00.000')
		AND L.LeadCreationDate BETWEEN DATEADD(day, DATEDIFF(day, 1, @Date), '20:00:00.000')
			AND DATEADD(day, DATEDIFF(day, 0, @Date), '20:00:00.000')
		AND S.TC_InquiryGroupSourceId = 11

	SET @CallsOnLeadPW = @COLP * @CallLeadCount

	--Actions from App
	SELECT @CallLeadCount = COUNT(DISTINCT C.TC_LeadId)
	FROM TC_Lead L WITH (NOLOCK)
	JOIN TC_Calls C WITH (NOLOCK) ON L.TC_LeadId = C.TC_LeadId
		AND C.IsActionTaken = 1
		AND C.CallType = 1
		AND L.LeadType = 1
		AND ISNULL(C.TC_UsersId, @UserId) = @UserId
		AND ISNULL(C.TC_ActionApplicationId, 1) = 2
	JOIN TC_InquirySource I WITH (NOLOCK) ON L.TC_InquirySourceId = I.Id
	WHERE L.BranchId = @BranchId
		AND C.ScheduledOn > DATEADD(day, DATEDIFF(day, 0, @Date - 1), '20:00:00.000')
		AND C.ActionTakenOn <= DATEADD(day, DATEDIFF(day, 0, @Date), '20:00:00.000')
		AND L.LeadCreationDate BETWEEN DATEADD(day, DATEDIFF(day, 1, @Date), '20:00:00.000')
			AND DATEADD(day, DATEDIFF(day, 0, @Date), '20:00:00.000')
		AND I.TC_InquiryGroupSourceId = 11

	SET @CallsOnLeadPA = 2 * @COLP * @CallLeadCount

	--inserting points to the table
	EXEC TC_InsertRewardPoints @DealerId = @BranchId
		,@EntryDate = @EntryDate
		,@TC_DealerTypeId = @TC_DealerTypeId
		,@TC_RewardPointsId = @COLPId
		,@RewardPoints = @COLP
		,@TotalRewardsFromWeb = @CallsOnLeadPW
		,@TotalRewardsFromApp = @CallsOnLeadPA
		,@UserId = @UserId

	-- Stocks uploaded with minimum of 10 photos 
	--Each time a car with min 10 photos is uploaded (and approved by our dealer team) then X points given.
	DECLARE @TempTable TABLE (
		PhotoCount NUMERIC
		,StockId INT
		,ApplicationId SMALLINT
		)

	-- stocks uploaded from web		
	--SELECT @StockCount = COUNT(St.StockId) FROM
	INSERT INTO @TempTable
	SELECT COUNT(CP.Id) AS PhotoCount
		,CP.StockId
		,ISNULL(S.TC_ActionApplicationId, 1)
	FROM TC_Stock S WITH (NOLOCK)
	JOIN TC_CarPhotos CP WITH (NOLOCK) ON S.Id = CP.StockId
		AND S.IsActive = 1
		AND ISNULL(S.ISPointsGiven, 0) <> 1
		AND S.StatusId = 1
		AND ISNULL(S.ModifiedBy, @UserId) = @UserId
	--AND ISNULL(S.TC_ActionApplicationId,1) = 1
	WHERE CONVERT(DATE, S.EntryDate) > = '2015-05-01' --We start giving points from this date only
		AND CONVERT(DATE, S.EntryDate) < CONVERT(DATE, @Date - 4) -- car sould be atleast 5 days old then only points will be given
		AND S.BranchId = @BranchId
	GROUP BY CP.StockId
		,ISNULL(S.TC_ActionApplicationId, 1)
	HAVING COUNT(CP.Id) > @PhotoCount - 1

	--SELECT  COUNT(StockId) FROM @TempTable WHERE ApplicationId = 1	
	SET @CarsWithPhotosPW = (
			SELECT COUNT(StockId)
			FROM @TempTable
			WHERE ApplicationId = 1
			) * @CWPP
	-- stocks uploaded from app		
	SET @CarsWithPhotosPA = (
			SELECT COUNT(StockId)
			FROM @TempTable
			WHERE ApplicationId = 2
			) * @CWPP * 2

	-- inserting points into the table
	EXEC TC_InsertRewardPoints @DealerId = @BranchId
		,@EntryDate = @EntryDate
		,@TC_DealerTypeId = @TC_DealerTypeId
		,@TC_RewardPointsId = @CWPPId
		,@RewardPoints = @CWPP
		,@TotalRewardsFromWeb = @CarsWithPhotosPW
		,@TotalRewardsFromApp = @CarsWithPhotosPA
		,@UserId = @UserId

	--Updating stock that points has been given for minimum 10 photos upload
	UPDATE TC_Stock
	SET ISPointsGiven = 1
	WHERE Id IN (
			SELECT StockId
			FROM @TempTable
			)

	DELETE
	FROM @TempTable

	--Warranties Activated Points
	--Require min criteria for activation - e.g. customer name, mobile, email, 
	--CarWale reserves the right to check this, else points will be reversed. 
	--Also activating a warranty at 150 points might be a lot, so you could
	-- make it 100 points with a temporary 50% bonus point clause, gives you 
	--flexibility to pull it back to 100 points when you need to.
	--warranties activated through web
	--below warranty points calculation has been disabled for the time being, changes done on 30-10-2015,
	-- points will be given manually, we will start giving 100 points from next month(01-11-2015)
	--IF(DAY(GETDATE()) = 1)
	--BEGIN
	--	--Coount warranty by checking if its amount is credited in our account		 
	--	DECLARE @WarrantyCount NUMERIC = NULL
	--	SELECT @WarrantyCount = COUNT(Absure_ActivatedWarrantyId)
	--	FROM AbSure_ActivatedWarranty AA WITH(NOLOCK)
	--	JOIN AbSure_Trans_Debits AD WITH(NOLOCK) ON AA.AbSure_CarDetailsId = AD.CarId
	--	JOIN AbSure_Trans_Logs AL WITH(NOLOCK) ON AD.Id = AL.TransactionId AND AL.TransactionType = 2
	--	WHERE AA.DealerId = @BranchId
	--	AND ISNULL(TC_ActionApplicationId,1) = 1
	--	AND AA.EntryDate >= DATEADD(MONTH,-1,DATEADD(MONTH, DATEDIFF(MONTH,0,GETDATE()), 0))  --Added By Chetan Navin on 21 Aug 2015
	--	AND AA.EntryDate < DATEADD(MONTH, DATEDIFF(MONTH,0,GETDATE()),0)
	--	AND AL.ClosingAmount > 0
	--	--AND CONVERT(DATE,EntryDate) = CONVERT(DATE,@Date)							--Commented by Chetan Navin on 21 Aug 2015
	--	SET @WAP = CASE WHEN @WarrantyCount BETWEEN 1 AND 3 THEN 2000
	--					WHEN @WarrantyCount BETWEEN 4 AND 5 THEN 2500
	--					WHEN @WarrantyCount BETWEEN 6 AND 10 THEN 3000
	--					WHEN @WarrantyCount > 10 THEN 4500
	--					ELSE 0 END
	--	SET @WarrantiesActivatedPW = @WarrantyCount * @WAP
	----warranties activated through App
	--	--SELECT @WarrantyCount = COUNT(Absure_ActivatedWarrantyId)
	--	--FROM AbSure_ActivatedWarranty WITH(NOLOCK)
	--	--WHERE DealerId = @BranchId
	--	--AND ISNULL(TC_ActionApplicationId,1) = 2
	--	--AND CONVERT(DATE,EntryDate) = CONVERT(DATE,@Date)
	--	--SET @WarrantiesActivatedPA = 2* @WarrantyCount * @WAP
	--	--inserting points to the table
	--	EXEC TC_InsertRewardPoints		@DealerId = @BranchId,
	--									@EntryDate = @EntryDate,
	--									@TC_DealerTypeId = @TC_DealerTypeId,
	--									@TC_RewardPointsId = @WAPId,
	--									@RewardPoints = @WAP,
	--									@TotalRewardsFromWeb = @WarrantiesActivatedPW,
	--									@TotalRewardsFromApp = @WarrantiesActivatedPA
	--END
	-- Car Sold lead came from carwale  points calculation
	--Suggest: Give x points per carwale lead who bought a car from the dealer. 
	--CarWale reserves the right to ask for min data and confirm this data with the other party. 
	--Put the bonus points as a separate multiplier that can be used tactically when required. 
	--Not required for seller enquiries / leads.
	-- warranty points startred again from 3rd of oct,2015 as we were giving, by vive gupta
	--warranties activated through web
	DECLARE @WarrantyCount NUMERIC = NULL

	SELECT @WarrantyCount = COUNT(Absure_ActivatedWarrantyId)
	FROM AbSure_ActivatedWarranty WITH (NOLOCK)
	WHERE DealerId = @BranchId
		AND ISNULL(TC_ActionApplicationId, 1) = 1
		AND CONVERT(DATE, EntryDate) = CONVERT(DATE, @Date)
		AND UserId = @UserId

	SET @WarrantiesActivatedPW = @WarrantyCount * @WAP

	--warranties activated through App
	SELECT @WarrantyCount = COUNT(Absure_ActivatedWarrantyId)
	FROM AbSure_ActivatedWarranty WITH (NOLOCK)
	WHERE DealerId = @BranchId
		AND ISNULL(TC_ActionApplicationId, 1) = 2
		AND CONVERT(DATE, EntryDate) = CONVERT(DATE, @Date)
		AND UserId = @UserId

	SET @WarrantiesActivatedPA = 2 * @WarrantyCount * @WAP

	--inserting points to the table
	EXEC TC_InsertRewardPoints @DealerId = @BranchId
		,@EntryDate = @EntryDate
		,@TC_DealerTypeId = @TC_DealerTypeId
		,@TC_RewardPointsId = @WAPId
		,@RewardPoints = @WAP
		,@TotalRewardsFromWeb = @WarrantiesActivatedPW
		,@TotalRewardsFromApp = @WarrantiesActivatedPA
		,@UserId = @UserId

	--changed condition to give used buyer booking points only when a stock is marked as sold to the customer who came from carwale.
	--	booked from web
	DECLARE @CarBookedCountWeb NUMERIC = 0

	SELECT @CarBookedCountWeb = COUNT(DISTINCT BI.TC_BuyerInquiriesId)
	FROM TC_BuyerInquiries BI WITH (NOLOCK)
	JOIN TC_InquiriesLead IL WITH (NOLOCK) ON BI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
		AND IL.BranchId = @BranchId
		AND BI.BookingStatus IN (
			34
			,42
			)
		AND CONVERT(DATE, BI.BookingDate) = CONVERT(DATE, @Date)
	JOIN TC_CustomerDetails C WITH (NOLOCK) ON IL.TC_CustomerId = C.Id
	JOIN TC_Stock S WITH (NOLOCK) ON C.Id = S.SoldToCustomerId
		--AND S.ModifiedBy = @UserId
		AND IL.TC_UserId =  @UserId
	JOIN TC_InquirySource I WITH (NOLOCK) ON BI.TC_InquirySourceId = I.Id
	WHERE I.TC_InquiryGroupSourceId = 11

	--WHERE --BI.BookingStatus = 34
	--AND ISNULL(BI.TC_ActionApplicationId,1) = 1
	--//commented by vivek gupta coz app action points are not given.
	--	booked from app
	--DECLARE @CarBookedCountApp NUMERIC = 0 
	--SELECT @CarBookedCountApp = COUNT(BI.TC_BuyerInquiriesId) FROM TC_BuyerInquiries BI WITH (NOLOCK)
	--JOIN TC_InquiriesLead IL WITH (NOLOCK) ON BI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId AND IL.BranchId = @BranchId
	--WHERE --BI.BookingStatus = 34
	--BI.BookingStatus IN (34,42)
	--AND ISNULL(BI.TC_ActionApplicationId,1) = 2
	--AND CONVERT(DATE,BI.BookingDate) = CONVERT(DATE,@Date)
	--DECLARE @StockSoldCount NUMERIC = 0
	----sold from web
	--SELECT @StockSoldCount = COUNT(SI.TC_SellerInquiriesId) FROM TC_SellerInquiries SI WITH (NOLOCK)
	--JOIN TC_InquiriesLead IL WITH (NOLOCK) ON SI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId AND IL.BranchId = @BranchId
	--WHERE SI.PurchasedStatus = 33
	--AND ISNULL(SI.TC_ActionApplicationId,1) = 1
	--AND CONVERT(DATE,SI.PurchasedDate) = CONVERT(DATE,@Date)
	--SET @CarSoldPW = (@CarBookedCountWeb) * @CSP
	--	--sold from app
	--SELECT @StockSoldCount = COUNT(SI.TC_SellerInquiriesId) FROM TC_SellerInquiries SI WITH (NOLOCK)
	--JOIN TC_InquiriesLead IL WITH (NOLOCK) ON SI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId AND IL.BranchId = @BranchId
	--WHERE SI.PurchasedStatus = 33
	--AND ISNULL(SI.TC_ActionApplicationId,1) = 2
	--AND CONVERT(DATE,SI.PurchasedDate) = CONVERT(DATE,@Date)
	--SET @CarSoldPA = 2*(@StockSoldCount + @CarBookedCountApp) * @CSP
	-- Calculating Points for purchasing a car 
	--Points will be given only when dealer enters chassis no of the car while adding that car to the stock
	--by vivek Gupta on 21-08-2015
	DECLARE @PurchaseCount INT = 0

	SELECT @PurchaseCount = COUNT(SI.TC_SellerInquiriesId)
	FROM TC_SellerInquiries SI WITH (NOLOCK)
	JOIN TC_Stock S WITH (NOLOCK) ON SI.TC_SellerInquiriesId = S.TC_SellerInquiriesId
		AND S.BranchId = @BranchId
		AND VIN IS NOT NULL
		AND S.RegNo <> 'NA'
		AND S.RegNo IS NOT NULL
		--AND S.BranchId = @BranchId
		AND S.ModifiedBy = @UserId
	JOIN TC_InquirySource I WITH (NOLOCK) ON SI.TC_InquirySourceId = I.ID
	WHERE CONVERT(DATE, S.EntryDate) = CONVERT(DATE, @Date)
		AND I.TC_InquiryGroupSourceId = 11

	SET @CarSoldPW = (@CarBookedCountWeb + @PurchaseCount) * @CSP

	----inserting points to the table
	EXEC TC_InsertRewardPoints @DealerId = @BranchId
		,@EntryDate = @EntryDate
		,@TC_DealerTypeId = @TC_DealerTypeId
		,@TC_RewardPointsId = @CSPId
		,@RewardPoints = @CSP
		,@TotalRewardsFromWeb = @CarSoldPW
		,@TotalRewardsFromApp = 0
		,@UserId = @UserId

	--PRINT('@DealerId = '+convert(varchar,@BranchId)
	--								+'@EntryDate = '+			convert(varchar,@EntryDate)
	--								+'@TC_DealerTypeId = '+		convert(varchar,@TC_DealerTypeId)
	--								+'@TC_RewardPointsId = '+	convert(varchar,@CSPId)
	--								+'@RewardPoints = '+		convert(varchar,@CSP)
	--								+'@TotalRewardsFromWeb = '+	convert(varchar,@CarSoldPW)
	--								+'@TotalRewardsFromApp = '+	convert(varchar,0)
	--								+'@UserId = '+				convert(varchar,@UserId))
	-- Website Points Calculation
	--Credited once for a first time sign-up, and once per annual renewal
	IF EXISTS (
			SELECT DealerPackageFeatureID
			FROM RVN_DealerPackageFeatures WITH (NOLOCK)
			WHERE PackageId = 36
				AND DealerId = @BranchId
				AND CONVERT(DATE, EntryDate) = CONVERT(DATE, @Date)
			)
	BEGIN
		SET @WebsiteUCPW = @WUP

		IF EXISTS (
				SELECT Id
				FROM TC_Users U WITH (NOLOCK)
				JOIN TC_UsersRole UR WITH (NOLOCK) ON U.Id = UR.UserId
					AND UR.UserId = @UserId
					AND UR.RoleId = 1
				WHERE U.BranchId = @BranchId
				)
			EXEC TC_InsertRewardPoints @DealerId = @BranchId
				,@EntryDate = @EntryDate
				,@TC_DealerTypeId = @TC_DealerTypeId
				,@TC_RewardPointsId = @WUPId
				,@RewardPoints = @WUP
				,@TotalRewardsFromWeb = @WebsiteUCPW
				,@TotalRewardsFromApp = 0
				,@UserId = @UserId
	END

	-- Digital Points Calulation
	--SELECT TOP 10 * FROM RVN_DealerPackageFeatures WHERE PackageId = 36
	-- Package Upgrade Caculation
	--Subscription points should only be given for annual subscriptions, and not month
	DECLARE @PackageId SMALLINT
	DECLARE @SecondaryPackageId SMALLINT

	SELECT @PackageId = PackageType
		,@SecondaryPackageId = (
			SELECT TOP 1 PackageId
			FROM ConsumerPackageRequests WITH (NOLOCK)
			WHERE ConsumerType = 1
				AND ConsumerId = CCP.ConsumerId
				AND isApproved = 1
				AND isActive = 1
			ORDER BY Id DESC
			)
	FROM ConsumerCreditPoints CCP WITH (NOLOCK)
	WHERE ConsumerId = @BranchId
		AND ExpiryDate > = @Date

	DECLARE @PackageTypeReward INT = NULL
		,@PackagePoints NUMERIC = 0

	IF (@PackageId = 29)
	BEGIN
		SET @PackageUpgradePW = @PUPP / 365
		SET @PackageTypeReward = @PUPPId
		SET @PackagePoints = @PUPP
	END
	ELSE
		IF (@PackageId = 19)
		BEGIN
			SET @PackageUpgradePW = @PUMP / 365
			SET @PackageTypeReward = @PUMPId
			SET @PackagePoints = @PUMP
		END
		ELSE
			IF (@PackageId = 18)
			BEGIN
				IF @SecondaryPackageId = 30
				BEGIN
					SET @PackageUpgradePW = @PUOP / 365
					SET @PackageTypeReward = @PUOPId
					SET @PackagePoints = @PUOP
				END
				ELSE
					IF @SecondaryPackageId = 34
					BEGIN
						SET @PackageUpgradePW = @PUSP / 365
						SET @PackageTypeReward = @PUSPId
						SET @PackagePoints = @PUSP
					END
			END

	--inserting points to the table
	IF @PackageTypeReward IS NOT NULL
	BEGIN
		IF EXISTS (
				SELECT Id
				FROM TC_Users U WITH (NOLOCK)
				JOIN TC_UsersRole UR WITH (NOLOCK) ON U.Id = UR.UserId
					AND UR.UserId = @UserId
					AND UR.RoleId = 1
				WHERE U.BranchId = @BranchId
				)
			EXEC TC_InsertRewardPoints @DealerId = @BranchId
				,@EntryDate = @EntryDate
				,@TC_DealerTypeId = @TC_DealerTypeId
				,@TC_RewardPointsId = @PackageTypeReward
				,@RewardPoints = @PackagePoints
				,@TotalRewardsFromWeb = @PackageUpgradePW
				,@TotalRewardsFromApp = 0
				,@UserId = @UserId
	END
END
