IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportLeadTrend]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportLeadTrend]
GO

	--	Author	:	Sachin Bharti(28th June 2013)
--	============================================================

CREATE Procedure [dbo].[TC_ReportLeadTrend]
@FromDate	DateTime = NULL,
@ToDate	DateTime = NULL
AS
BEGIN
	SELECT SUM(TC.TotalLeadCount) AS LeadCount, DAY(LeadCreationDate) AS Day, MONTH(LeadCreationDate) AS Month
	FROM TC_DealerWiseDailyLeadCount TC (NOLOCK)
	WHERE TC.LeadCreationDate BETWEEN @FromDate AND @ToDate 
	GROUP BY DAY(LeadCreationDate), MONTH(LeadCreationDate)
END




