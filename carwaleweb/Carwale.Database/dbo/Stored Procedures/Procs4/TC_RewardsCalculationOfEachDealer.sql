IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RewardsCalculationOfEachDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RewardsCalculationOfEachDealer]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 28-04-2015
-- Description:	Extracting Dealers And Calculating their reward points daily
-- Modified By Vivek Gupta on 17-06-2015, removed condition of paid dealers, coz need to calculate points for every dealer
-- Modified By Vivek Gupta on 06-11-2015, added @Userid parameter to calculater points user wise(not dealer wise)
--EXEC [TC_RewardsCalculationOfEachDealer]
--Modified on 18th Feb 2016 by Deepak Tripathi
--No Points for Used Car Dealers
--New Car Dealer Will have points for TD and Booking 
--Modified by : Ashwini Dhamankar on Aug 25,2016		
-- =============================================
CREATE PROCEDURE [dbo].[TC_RewardsCalculationOfEachDealer]
--@Date DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Date DATETIME = NULL

	IF @Date IS NULL
	SET @Date = GETDATE()-1

	DECLARE @LPId   SMALLINT = 1,--@LoginId
			@COLPId SMALLINT = 2, --@CallsOnLeadId
			@CWPPId SMALLINT = 3, --@CarsWithPhotoId
			@WWPPId SMALLINT = 4, --@WarrantiesPurchasedId
			@WAPId  SMALLINT = 5, --@WarrantiesActivatedId
			@CSPId  SMALLINT = 6, --@CarSoldId
			@WUPId  SMALLINT = 7, --@WebsiteUsedId
		    @DSPId  SMALLINT = 8,--@DigitalServicesId 
			@PUPPId SMALLINT = 9, --@PackageUpgradePremiumId
			@PUMPId SMALLINT = 10, --@PackageUpgradeMaximiserId
			@PUOPId SMALLINT = 11, --@PackageUpgradeOptimiserId
			@PUSPId SMALLINT = 12, --@PackageUpgradeStarterId


			@PQPId   SMALLINT = 13, --@PQIn24HoursId
			@TDCPId  SMALLINT = 14, --@TDCompletionId
			@ALSPId  SMALLINT = 15,--@ActiveLeadsInSystemId
			@CBTCPId SMALLINT = 16, --@carsBookedThroughCarwaleId
			@DOCPId  SMALLINT = 17, --@DuratonOfContractId
			@WNPId   SMALLINT = 18, --@WebsiteNewId
			@SEMPId  SMALLINT = 19, --@SEMId
			@CBPId   SMALLINT = 20 --@ContextualBrandingId

	--DECLARE @FPBId SMALLINT = 21,--Follow Up Points Id
	--	    @BBPBId SMALLINT = 22, --Bike Booking Points Id Bikewale
	--		@PCPBId SMALLINT = 23 --Package Continuation Points Id Bikewale

	DECLARE
	@LP   NUMERIC = 5,--@Login 
	@COLP NUMERIC = 100, --@CallsOnLead
	@CWPP NUMERIC = 90, --@CarsWithPhoto
	@WWPP NUMERIC = 50, --@WarrantiesPurchased
	@WAP  NUMERIC = 100, --@WarrantiesActivated
	@CSP  NUMERIC = 10, --@CarSold
	@WUP  NUMERIC = 500, --@WebsiteUsed
	@DSP  NUMERIC = 500,--@DigitalServices 
	@PUPP NUMERIC = 1000, --@PackageUpgradePremium
	@PUMP NUMERIC = 600, --@PackageUpgradeMaximiser
	@PUOP NUMERIC = 360, --@PackageUpgradeOptimiser
	@PUSP NUMERIC = 120, --@PackageUpgradeStarter

	

	@PhotoCount NUMERIC = 10

	DECLARE
	--@LP NUMERIC = 5,--@Login 
	@PQP    NUMERIC = 100, --@PQIn24Hours
	@TDCP   NUMERIC = 25, --@TDCompletion
	@ALSP   NUMERIC = 10,--@ActiveLeadsInSystem
	@CBTCP  NUMERIC = 50, --@carsBookedThroughCarwale
	@DOCP   NUMERIC = 500, --@DuratonOfContract, is the sum of points earned through booking, td completion and pq calls
	@WNP    NUMERIC = 500, --@WebsiteNew
	@SEMP   NUMERIC = 500, --@SEM
	@CBP    NUMERIC = 500, --@ContextualBranding
	@DOCPSM NUMERIC = 500

	--DECLARE @FPB NUMERIC = 5,--Follow Up Points Id
	--	    @BBPB NUMERIC = 25, --Bike Booking Points Id Bikewale
	--		@PCPB NUMERIC = 100 --Package Continuation Points Id Bikewale


	-- Fetching Dynamic Reward points
	SELECT @LP    = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@LPId   
	SELECT @COLP  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@COLPId 
	SELECT @CWPP  = Points, @PhotoCount = DependencyCount FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@CWPPId 
	SELECT @WWPP  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@WWPPId 
	SELECT @WAP   = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@WAPId  
	SELECT @CSP   = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@CSPId  
	SELECT @WUP   = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@WUPId  
	SELECT @DSP   = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@DSPId  
	SELECT @PUPP  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@PUPPId 
	SELECT @PUMP  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@PUMPId 
	SELECT @PUOP  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@PUOPId 
	SELECT @PUSP  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@PUSPId 
	

	SELECT  @PQP  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@PQPId  
	SELECT	@TDCP = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@TDCPId 
	SELECT  @ALSP = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@ALSPId 
	SELECT  @CBTCP= Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@CBTCPId
	SELECT  @DOCP = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@DOCPId 
	SELECT  @WNP  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@WNPId  
	SELECT  @SEMP = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@SEMPId 
	SELECT  @CBP  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@CBPId  
	--SELECT  @DOCPSM  = Points FROM TC_RewardPoints WHERE Id=
	--SELECT  @FPB  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@FPBId  
	--SELECT  @BBPB  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@BBPBId  
	--SELECT  @PCPB  = Points FROM TC_RewardPoints WITH(NOLOCK) WHERE Id=@PCPBId  


    -- Insert statements for procedure here
	DECLARE @TempDealers TABLE (DealerId INT, TC_DealerTypeId INT, UserId INT)
	DECLARE @DealerId INT, @TC_DealerTypeId INT, @UserId INT  
	
	--Only New Car Dealers
	INSERT INTO @TempDealers
	SELECT D.Id, D.TC_DealerTypeId, U.Id FROM Dealers D WITH(NOLOCK) 
	JOIN TC_Users U WITH(NOLOCK) ON D.ID = U.BranchId AND U.IsActive = 1 AND ISNULL(U.IsCarwaleUser,0) = 0
	WHERE D.IsDealerActive = 1 AND D.ApplicationId = 1 AND D.TC_DealerTypeId IN(2,3) 
	--AND ID IN(SELECT ConsumerId FROM ConsumerCreditPoints  WHERE ConsumerType = 1 AND PackageType <> 28 
	--			AND CONVERT(DATE,ExpiryDate) >= CONVERT(DATE,@Date))
	AND D.ID NOT IN(SELECT DealerId FROM TC_MFCDealers WITH(NOLOCK))
	
	--New Car Dealers
	--INSERT INTO @TempDealers
	--SELECT D.Id, D.TC_DealerTypeId,U.Id FROM Dealers D WITH(NOLOCK) 
	--JOIN TC_Users U WITH(NOLOCK) ON D.ID = U.BranchId AND U.IsActive = 1 AND ISNULL(U.IsCarwaleUser,0) = 0
	--WHERE D.IsDealerActive = 1 AND D.ApplicationId = 1
	--	--AND ID IN(SELECT DealerId FROM RVN_DealerPackageFeatures WHERE CONVERT(DATE,PackageEndDate) >= CONVERT(DATE,@Date)) 
	--	AND D.ID NOT IN(SELECT DealerId FROM @TempDealers) 
		 

	WHILE EXISTS (SELECT TOP 1 DealerId FROM @TempDealers)
	BEGIN
		SET @DealerId = (SELECT TOP 1 DealerId FROM @TempDealers)
		SET @TC_DealerTypeId = (SELECT TOP 1 TC_DealerTypeId FROM @TempDealers WHERE DealerId = @DealerId)
		SET @UserId = (SELECT TOP 1 UserId FROM @TempDealers WHERE DealerId = @DealerId)
		
		--Commented on 18th Feb 2016 by Deepak Tripathi
		--No Points for Used Car Dealers
		--New Car Dealer Will have points for TD and Booking 
		
		EXEC TC_RewardsForNewCarActions @DealerId, @TC_DealerTypeId,@Date, @TDCP, @CBTCP, @LP,@PQP ,@ALSP,@DOCP ,@WNP ,@SEMP ,@CBP ,@DOCPSM ,@UserId  
		
		--IF(@TC_DealerTypeId = 1)
		--	BEGIN
		--	   EXEC TC_RewardsForUsedCarActions @DealerId, @TC_DealerTypeId,@Date, @LP ,@COLP, @CWPP, @WWPP, @WAP, @CSP, @WUP, @DSP, @PUPP, @PUMP, @PUOP, @PUSP, @PhotoCount, @UserId
		--	END																	   
		--ELSE IF(@TC_DealerTypeId = 2)											   
		--	BEGIN																   
		--	   EXEC TC_RewardsForNewCarActions @DealerId, @TC_DealerTypeId,@Date,@LP,@PQP ,@TDCP ,@ALSP ,@CBTCP,@DOCP ,@WNP ,@SEMP ,@CBP ,@DOCPSM ,@UserId   
		--	END																	    
		--ELSE IF(@TC_DealerTypeId = 3)
		--	BEGIN																    
		--		EXEC TC_RewardsForUsedCarActions @DealerId, @TC_DealerTypeId,@Date, @LP ,@COLP, @CWPP, @WWPP, @WAP, @CSP, @WUP, @DSP, @PUPP, @PUMP, @PUOP, @PUSP, @PhotoCount,@UserId   
		--		EXEC TC_RewardsForNewCarActions  @DealerId, @TC_DealerTypeId,@Date,@LP,@PQP ,@TDCP ,@ALSP ,@CBTCP,@DOCP ,@WNP ,@SEMP ,@CBP ,@DOCPSM ,@UserId   
		--	END																	   
																				   
		DELETE FROM @TempDealers WHERE DealerId = @DealerId AND UserId = @UserId

	END

	---BikeWale Reward Program Points Calculation---
	--Stopped on 22nd Feb 2016 by Puneet
	--Uncommented By : Ashwini Dhamankar on Aug 25,2016
	DECLARE @TempBikewaleDealers TABLE (DealerId INT, TC_DealerTypeId INT, UserId INT)
	INSERT INTO @TempBikewaleDealers
	SELECT D.Id, D.TC_DealerTypeId,U.Id FROM Dealers D WITH(NOLOCK) 
	JOIN TC_Users U WITH(NOLOCK) ON D.ID = U.BranchId AND U.IsActive = 1 AND ISNULL(U.IsCarwaleUser,0) = 0
	WHERE D.ApplicationId = 2 AND D.IsDealerActive = 1

	WHILE EXISTS (SELECT TOP 1 DealerId FROM @TempBikewaleDealers)
	BEGIN
		SET @DealerId = (SELECT TOP 1 DealerId FROM @TempBikewaleDealers)
		SET @UserId = (SELECT TOP 1 UserId FROM @TempBikewaleDealers WHERE DealerId = @DealerId)
		SET @TC_DealerTypeId = 2
															   
		EXEC TC_RewardsForNewCarActionsBikeWale @DealerId, @TC_DealerTypeId,@Date,@UserId
		
		DELETE FROM @TempBikewaleDealers WHERE DealerId = @DealerId AND UserId = @UserId

	END

END


