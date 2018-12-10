IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RewardsDescription]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RewardsDescription]
GO

	-- =============================================
-- Author:		<Vivek,,Gupta>
-- Create date: <Create Date,14-05-2015,>
-- Description:	<Description,Returns day,week and month wise rewards details,>
-- Modified By Vivek GUpta , added website and didgital service points to the return table
-- Modified By Vivek Gupta on 06-11-2015, added @Userid parameter to calculater points user wise(not dealer wise)
-- =============================================
CREATE PROCEDURE [dbo].[TC_RewardsDescription]
@DealerId INT,
@DescriptionType TINYINT,
@FromDate DATETIME = NULL,
@ToDate DATETIME = NULL,
@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @TempRewardPoints TABLE(TC_RewardPointsId INT, TotalRewardsFromWeb NUMERIC,TotalRewardsFromApp NUMERIC , TotalRewardsToSM NUMERIC)
	DECLARE @TotalPoints NUMERIC
	DECLARE @TotalLoginPoints NUMERIC
	DECLARE @TotalPhotosPoints NUMERIC
	DECLARE @TotalFollowUpPoints NUMERIC
	
	DECLARE @CarSoldPoints NUMERIC
	DECLARE @WarrantiesActivation NUMERIC
	DECLARE @TDCompletionPoints NUMERIC

	DECLARE @FollowupPointsBikeWale NUMERIC
	DECLARE @BikeBookingPoints NUMERIC
	DECLARE @PackageContiuationPoints NUMERIC

	DECLARE @Website NUMERIC
	DECLARE @DigitalServices NUMERIC

	DECLARE @DateRange VARCHAR(50)

	


	--Daily Reward Points Earned
	IF(@DescriptionType = 1)
	BEGIN
		SET @FromDate = CONVERT(DATE,GETDATE()-1)
		SET @Todate = @FromDate

		INSERT INTO @TempRewardPoints
		SELECT TC_RewardPointsId, TotalRewardsFromWeb,TotalRewardsFromApp,TotalRewardsToSM
		FROM TC_DealerDailyRewardPoints DDR WITH(NOLOCK)
		WHERE CONVERT(DATE,EntryDate) = @Todate
		AND   DealerId = @DealerId
		AND UserId = @UserId

		SET @DateRange = CONVERT(VARCHAR(12),GETDATE()-1,106)
		--CONVERT(VARCHAR, DATENAME(day, GETDATE()-1)) + ' ' +  CONVERT(VARCHAR, DATENAME(Month, GETDATE()-1)) + ', ' + CONVERT(VARCHAR, DATENAME(year, GETDATE()-1))
	END

	--Weekly Reward Points Earned
	IF(@DescriptionType = 2)
	BEGIN

		SET @FromDate = DATEADD(wk, DATEDIFF(wk, 6, GETDATE()), 0) 
		SET @Todate = DATEADD(wk, DATEDIFF(wk, 6, GETDATE()), 6)
		
		INSERT INTO @TempRewardPoints
		SELECT TC_RewardPointsId, TotalRewardsFromWeb,TotalRewardsFromApp,TotalRewardsToSM
		FROM TC_DealerDailyRewardPoints DDR WITH(NOLOCK)
		WHERE EntryDate BETWEEN DATEADD(wk, DATEDIFF(wk, 6, GETDATE()), 0) 
							AND DATEADD(wk, DATEDIFF(wk, 6, GETDATE()), 6)
		AND   DealerId = @DealerId
		AND UserId = @UserId

		SET @DateRange =  CONVERT(VARCHAR(12),@FromDate,106) + ' - ' + CONVERT(VARCHAR(12),@Todate,106)
		
		--CONVERT(VARCHAR, DATENAME(day, @FromDate)) + ' ' +  CONVERT(VARCHAR, DATENAME(Month, @FromDate)) + ', ' + CONVERT(VARCHAR, DATENAME(YY, @FromDate)) + ' - '
						--+ CONVERT(VARCHAR, DATENAME(day, @Todate)) + ' ' +  CONVERT(VARCHAR, DATENAME(Month, @Todate)) + ', ' + CONVERT(VARCHAR, DATENAME(YY, @Todate))

	END
	
	--Monthly Reward Points Earned
	IF(@DescriptionType = 3)
	BEGIN
		
		SET @FromDate = DATEADD(mm, DATEDIFF(mm, 0, GETDATE()) - 1, 0)
		SET @Todate = DATEADD(DAY, -(DAY(GETDATE())), GETDATE())

		INSERT INTO @TempRewardPoints
		SELECT TC_RewardPointsId, TotalRewardsFromWeb,TotalRewardsFromApp,TotalRewardsToSM
		FROM TC_DealerDailyRewardPoints DDR WITH(NOLOCK)
		WHERE 
				DATEPART(m, EntryDate)   =  DATEPART(m, DATEADD(m, -1, getdate()))
		AND		DATEPART(yyyy, EntryDate)=  DATEPART(yyyy, DATEADD(m, -1, getdate()))
		AND   DealerId = @DealerId
		AND   UserId = @UserId

		SET @DateRange =  CONVERT(VARCHAR(12),@FromDate,106) + ' - ' + CONVERT(VARCHAR(12),@Todate,106)
		--CONVERT(VARCHAR, DATENAME(day, @FromDate)) + ' ' +  CONVERT(VARCHAR, DATENAME(Month, @FromDate)) + ', ' + CONVERT(VARCHAR, DATENAME(YY, @FromDate)) + ' - '
				--		+ CONVERT(VARCHAR, DATENAME(day, @Todate)) + ' ' +  CONVERT(VARCHAR, DATENAME(Month, @Todate)) + ', ' + CONVERT(VARCHAR, DATENAME(YY, @Todate))
	END

	--In a Date Range Reward Points Earned
	IF(@DescriptionType = 4)
	BEGIN
		INSERT INTO @TempRewardPoints
		SELECT TC_RewardPointsId, TotalRewardsFromWeb,TotalRewardsFromApp,TotalRewardsToSM
		FROM TC_DealerDailyRewardPoints DDR WITH(NOLOCK)
		WHERE 
			  CONVERT(DATE,EntryDate) BETWEEN CONVERT(DATE,@FromDate) AND CONVERT(DATE,@ToDate)
		AND   DealerId = @DealerId
		AND   UserId = @UserId

		SET @DateRange =  CONVERT(VARCHAR(12),@FromDate,106) + ' - ' + CONVERT(VARCHAR(12),@Todate,106)
		--CONVERT(VARCHAR, DATENAME(day, @FromDate)) + ' ' +  CONVERT(VARCHAR, DATENAME(Month, @FromDate)) + ', ' + CONVERT(VARCHAR, DATENAME(YY, @FromDate)) + ' - '
					--	+ CONVERT(VARCHAR, DATENAME(day, @Todate)) + ' ' +  CONVERT(VARCHAR, DATENAME(Month, @Todate)) + ', ' + CONVERT(VARCHAR, DATENAME(YY, @Todate))

	END

	SET @TotalPoints = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints)

	SET @TotalLoginPoints = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints WHERE TC_RewardPointsId = 1) 

	SET @TotalPhotosPoints = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints WHERE TC_RewardPointsId = 3)

	SET @TotalFollowUpPoints = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints WHERE TC_RewardPointsId IN(2,13))

	SET @CarSoldPoints = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints WHERE TC_RewardPointsId IN(6))
	
	SET @WarrantiesActivation = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints WHERE TC_RewardPointsId IN(5))
	
	SET @TDCompletionPoints = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints WHERE TC_RewardPointsId IN(14))

	SET @FollowupPointsBikeWale = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints WHERE TC_RewardPointsId IN(21))

	SET @BikeBookingPoints = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints WHERE TC_RewardPointsId IN(22))

	SET @PackageContiuationPoints = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints WHERE TC_RewardPointsId IN(23))

	SET @Website = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints WHERE TC_RewardPointsId IN(7,18))

	SET @DigitalServices = (SELECT SUM(TotalRewardsFromWeb) + SUM(TotalRewardsFromApp) + SUM(TotalRewardsToSM) FROM @TempRewardPoints WHERE TC_RewardPointsId IN(8))



	DECLARE @IsUsed BIT = 1

	IF EXISTS (SELECT Id FROM Dealers WITH(NOLOCK) where id=@DealerId AND TC_DealerTypeId <> 1)
	BEGIN
		SET @IsUsed = 0
	END

	DECLARE @ApplicationId INT
	SELECT @ApplicationId = ApplicationId FROM Dealers WITH(NOLOCK) WHERE ID= @DealerId
	DECLARE @FollowUpPoints FLOAT = 0
	IF(@ApplicationId = 1)
	BEGIN
	   SET @FollowUpPoints = @TotalFollowUpPoints
	END
	ELSE IF (@ApplicationId = 2)
	BEGIN
		SET @FollowUpPoints = @FollowupPointsBikeWale
	END
	
	 SELECT ISNULL(@TotalPoints,0) AS TotalPoints,
		    ISNULL(@TotalLoginPoints,0) AS LoginPoints,
		    ISNULL(@TotalPhotosPoints,0) AS PhotoUploadPoints,
		    ISNULL(@FollowUpPoints,0) AS FollowUpPoints,
			ISNULL(@CarSoldPoints ,0) AS CarSoldThroughCarwale,
			ISNULL(@BikeBookingPoints ,0) AS BikeSoldThroughBikewale,
		    @DateRange AS DateRange
			,
			@IsUsed AS IsUsed

     EXEC TC_RewardsRedemptionHistory @DealerId = @DealerId, @FromDate = @FromDate, @ToDate = @ToDate, @UserId = @UserId


	 DECLARE @TempTable TABLE (RewardPoints NUMERIC, RewardsFor VARCHAR(100) , DateRange VARCHAR(50), TempId INT)
	 INSERT INTO @TempTable (RewardPoints, RewardsFor, DateRange, TempId)
	 VALUES
	 (@TotalPoints,'Total Points Earned',@DateRange,1),
	 --(@TotalLoginPoints,'First Login of a day', @DateRange,2),
	 --(@TotalFollowUpPoints,'First Follow-up on the lead',@DateRange,3),
	 --(@TotalPhotosPoints,'Photos on the stock', @DateRange,4),
	 --(@CarSoldPoints,'Car sold through carwale', @DateRange,5),
	 --(@WarrantiesActivation,'Warranties Activated', @DateRange,6),
	 (@TDCompletionPoints,'TD Completion', @DateRange,7),
	 --(@FollowupPointsBikeWale,'First Follow-up on the lead', @DateRange,8),
	 (@BikeBookingPoints,'Bike Booking', @DateRange,9)--,
	 --(@Website,'Website renew',@DateRange,10),
	 --(@DigitalServices,'Digital Services',@DateRange,11)
	 --,
	 --(@PackageContiuationPoints,'Package Continuation', @DateRange,10)
	 

	 IF(@ApplicationId = 1)
	 BEGIN
		SELECT ISNULL(RewardPoints,0) AS RewardPoints,RewardsFor,DateRange  FROM @TempTable WHERE (ISNULL(RewardPoints,0) <> 0 OR TempId IN(1,2,3))
	 END
	 ELSE IF (@ApplicationId = 2)
	 BEGIN
		SELECT ISNULL(RewardPoints,0) AS RewardPoints,RewardsFor,DateRange  FROM @TempTable WHERE TempId IN(8,9)
	 END

	 DELETE FROM @TempRewardPoints
	 DELETE FROM @TempTable
END
