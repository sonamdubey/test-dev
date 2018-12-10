IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportZonePerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportZonePerformance]
GO

	--	Author	:	Sachin Bharti(28th June 2013)
--	============================================================

CREATE Procedure [dbo].[TC_ReportZonePerformance]
@FromDate	DateTime = NULL,
@ToDate	DateTime = NULL
AS
BEGIN
	SELECT SUM(TC.TotalLead) AS LeadCount, SUM(TC.ActiveHotLead) AS ActiveHotLead,
	SUM(TC.ActiveWarmLead) AS ActiveWarmLead, SUM(TC.ActiveNormalLead) AS ActiveNormalLead,
	SUM(TC.StatusNotSetActiveLead) AS ActiveNotSet, SUM(TC.BookedLead) AS BookedLead,
	SUM(TC.Lost) AS Lost, SUM(TC.PendingFollowUp) AS PendingFollowUp,
	SUM(TC.PendingTestDrive) AS PendingTestDrive, TBZ.ZoneName
	FROM TC_ReportZoneData TC (NOLOCK)
	INNER JOIN Dealers D (NOLOCK) ON D.ID = TC.DealerId
	INNER JOIN TC_BrandZone AS TBZ ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId
	GROUP BY TBZ.ZoneName
END




