IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[scb].[SetBWDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [scb].[SetBWDealers]
GO

	CREATE PROCEDURE scb.SetBWDealers
AS
INSERT INTO [scb].[BikeWaleDailyTracker](TrackerType,TrackerDate,TrackerCount)
SELECT 1 AS TrackerType, cast(joiningdate as date) TrackerDate,COUNT(d.ID) TrackerCount
FROM CARWALE_COM..Dealers d with (NoLock)
WHERE d.ApplicationId =2 
and d.isDealerActive =1
GROUP BY CAST(JOININGDATE AS DATE)
ORDER BY CAST(JOININGDATE AS DATE)

