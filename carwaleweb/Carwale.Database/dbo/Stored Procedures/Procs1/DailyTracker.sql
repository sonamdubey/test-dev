IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DailyTracker]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DailyTracker]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 12-12-2012
-- Description:	Return Daily Tracker for the day
--Modified by Reshma Shetty 21/08/2013 Introduced delta calculation for some of the trackers 
--Modified by Reshma Shetty 2/09/2013 Added new trackers in @Table
--Modified by AM Commented 12-09-2013 to hold on inactive trackers
-- =============================================
CREATE PROCEDURE [dbo].[DailyTracker]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--DECLARE @Date DATE = CONVERT(VARCHAR(11), GETDATE() - 2, 113)
	DECLARE @Date DATE = CONVERT(VARCHAR(11), GETDATE()-1 , 113)

	DECLARE @Table TABLE (
		TrackerType VARCHAR(30),
		IsDelta BIT,
		OrderBy SMALLINT
		)

    --Modified by AM Commented 12-09-2013 to hold on inactive trackers
	INSERT INTO @Table
	VALUES  ('Audi Bookings',0,1),
			('Mahindra Bookings',0,1),
			('Tata Bookings',0,1),
			('Skoda Bookings',0,1),
			('UCD',1,2),
			('NCD',1,3),
			('Livelistings-Indiv',1,4),
			('Livelistings-Dealer',1,10),
			('UC Buyers',0,5),
			--('UniquePQ',0,6),
			('Unique Buyers',1,7),
			('Leads Processed',0,8),
			('Mahindra Leads Assigned',0,10),
			('Skoda Leads Assigned',0,11),
			('Leads Assigned',0,9)			
			


	SELECT DT.TrackerType,
		CASE IsDelta
			WHEN 1
				THEN DT.TrackerCount - ISNULL(ST.TrackerCount, 0)
			ELSE DT.TrackerCount
			END as TrackerCount,
		DT.TrackerDate
	FROM SC.DailyTracker DT
	INNER JOIN @Table T ON T.TrackerType = DT.TrackerType
	LEFT JOIN SC.DailyTracker ST ON ST.TrackerDate = DATEADD(DAY, - 1, DT.TrackerDate)
									AND ST.TrackerType = DT.TrackerType
									AND MONTH(ST.TrackerDate) = MONTH(DT.TrackerDate)
									--Month is checked here so that on first day of the month the actual count is returned instead of the difference 
	WHERE DT.TrackerDate = @Date
	ORDER BY OrderBy

END
