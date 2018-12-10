IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AllHourlyReportDataJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AllHourlyReportDataJob]
GO

	

-- =============================================
-- Author:		Manish
-- Create date: 29-06-2013
-- Description: SP for inserting all the volkswagen report related data to respective table
-- Modified By Manish on 11-07-2013 commented the sp execution " TC_DealerWiseDailyLeadCountJob" since not used any where
-- Modified By Manish on 19-08-2013 changing the order of the execution of sp after optimising volkswagen hourly report
-- =============================================
CREATE PROCEDURE  [dbo].[TC_AllHourlyReportDataJob]
AS
BEGIN

EXEC TC_ReportZoneHourlyDataJob
EXEC TC_VolksWagenEveryHourReportDataJob  --- execution of the sp order interchanged
--EXEC TC_DealerWiseDailyLeadCountJob  

END

