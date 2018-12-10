IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[SC].[SetPQDailyTracker]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [SC].[SetPQDailyTracker]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 11-09-2013
-- Description:	Set Daily Tracker of Unique Buyers for the day
-- =============================================
CREATE PROCEDURE [SC].[SetPQDailyTracker]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @Date DATE=CONVERT(varchar(11),GETDATE()-1,113)

    -- Insert statements for procedure here
	INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)		
	SELECT 'Unique Buyers' TrackerType,sum(CNT) ,@Date
	FROM PQMatrixUniquePerMonth P 
	WHERE CityId = -2 AND P.Month = MONTH(@Date) AND P.Year = YEAR(@Date) AND P.ForwardedLead = 1
	--AND CNT>1000
END
