IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DailyTracker_Bkp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DailyTracker_Bkp]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 12-12-2012
-- Description:	Return Daily Tracker for the day
--Modified by Reshma Shetty 21/08/2013 Introduced delta calculation for some of the trackers 
-- =============================================
CREATE PROCEDURE [dbo].[DailyTracker_Bkp]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Date DATE = CONVERT(VARCHAR(11), GETDATE() - 3, 113)

	DECLARE @Table TABLE (
		TrackerType VARCHAR(20),
		IsDelta BIT
		)

	INSERT INTO @Table
	VALUES  ('UCD',1),
			--('NCD',1),
			('Livelistings',1),
			('UC Buyers',0)


	SELECT  DT.TrackerType,
		CASE IsDelta
			WHEN 1
				THEN DT.TrackerCount - ISNULL(ST.TrackerCount, 0)
			ELSE DT.TrackerCount
			END TrackerCount,
		DT.TrackerDate
	FROM SC.DailyTracker DT
	INNER JOIN @Table T ON T.TrackerType = DT.TrackerType
	LEFT JOIN SC.DailyTracker ST ON ST.TrackerDate = DATEADD(DAY, - 1, DT.TrackerDate)
									AND ST.TrackerType = DT.TrackerType
									AND MONTH(ST.TrackerDate) = MONTH(DT.TrackerDate)
									--Month is checked here so that on first day of the month the actual count is returned instead of the difference 
	WHERE DT.TrackerDate >= @Date
	ORDER BY TrackerDate DESC

END
