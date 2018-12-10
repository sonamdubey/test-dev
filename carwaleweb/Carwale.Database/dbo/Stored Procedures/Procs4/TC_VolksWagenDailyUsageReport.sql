IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_VolksWagenDailyUsageReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_VolksWagenDailyUsageReport]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 19-06-2013
-- Description: Report for VW AutoBiz panel Usage 
-- Modified by Manish on 28-06-2013 for adding Test Drive columns
-- Modified By	:	Sachin Bharti(3rd July 2013)
-- Modified By Manish on 19-08-2013  for optimization of query by changing the data fetching process
-- Modified By: Manish Chourasiya on 13-11-2014 changed to add dealer id condition now this houly reports will use for Shaman Used Cars.
-- =============================================
CREATE PROCEDURE [dbo].[TC_VolksWagenDailyUsageReport]-- 3
@RMId				NUMERIC(18,0) = NULL,
@AMId				NUMERIC(18,0) = NULL
AS
BEGIN
--Subject :VW AutoBiz panel Usage 19-06-2013 3 PM

SELECT 
	TVW.Organization AS [Organization] ,
	TotalUsers AS TotalUsers ,
	TotalNoOfLoginsToday AS [TotalNoofLoginsToday],
	TotalNoOfLogins AS [TotalNoofLogins],
	TotalInquiryAddedToday,
	TotalInquiryAdded,
	TotalUniqueInquiryAdded,
	TotalFollowupsToday,
	TotalFollowups,
	TotalUniqueFollowups,
	TotalPendingFollowups,
	TotalUniquePendingFollowups,
	TotalTodaysBookedCar,
	TotalBookedCar,
	TDCompleted ,
	TDCancelled,
	TDBooked
FROM TC_VolksWagenHourlyReportData TVW(NOLOCK) 
INNER JOIN Dealers D(NOLOCK) ON D.ID = TVW.DealerID

WHERE-- ( @RMId IS NULL  OR D.TC_RMId = @RMId ) AND ( @AmId IS NULL  OR D.TC_AMId = @AmId )
 D.ID=50 AND D.IsDealerActive=1 --condition added by manish on 13-11-2014

ORDER BY Organization

END