IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[SC].[SetDailyTracker_Bkp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [SC].[SetDailyTracker_Bkp]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 19-08-2013
-- Description:	Set Daily Tracker for the day
--EXEC [sc].[SetDailyTracker]
--Modified by Reshma Shetty 20/08/2013 Added Livelistings and UC Buyers trackers 
-- =============================================
CREATE PROCEDURE [SC].[SetDailyTracker_Bkp]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @Date DATE=CONVERT(varchar(11),GETDATE()-1,113)
	
    INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)
    SELECT 'UCD' AS TrackerType,COUNT(*) UCDCnt,@Date AS TrackerDate
    FROM ConsumerCreditPoints as CP WITH(NOLOCK)
	JOIN Dealers as D on CP.ConsumerId=D.Id and D.TC_DealerTypeId in (1,3)
	WHERE ConsumerType = 1
	AND CONVERT(varchar(11),CP.ExpiryDate,113) >= @Date
	AND D.Id Not IN(SELECT CarwaleDealerId From AutofriendDealerMap WHERE isActive = 1)
	AND D.Status=0
	
	
	
	
	--INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)
 --   SELECT 'NCD' AS TrackerType,COUNT(*) UCDCnt,@Date AS TrackerDate
 --   FROM NCS_Dealers WITH(NOLOCK)
 --   WHERE IsActive = 1 
 --   AND IsNCDDealer = 1
    
    
    INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)
    SELECT 'Livelistings' AS TrackerType,COUNT(ProfileId) LL_Cnt,@Date AS TrackerDate
    FROM livelistings WITH(NOLOCK)
    
    INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)
    SELECT 'UC Buyers' AS TrackerType,COUNT(DISTINCT CustomerID) UsedCarBuyersCount ,@Date AS TrackerDate
	FROM(SELECT CustomerID
	FROM UsedCarPurchaseInquiries with(nolock)
	WHERE CONVERT(varchar(11),RequestDateTime,113) =@Date
	UNION ALL
	SELECT CustomerID
	FROM ClassifiedRequests with(nolock)
	WHERE CONVERT(varchar(11),RequestDateTime,113) =@Date)AS Tab	
	
END
