IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDScheduleDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDScheduleDetails]
GO

	-- ======================================================================= 
-- Created By : Nilesh Utture
-- Created On : 12th August, 2013
-- Description: Will fetch all the td details for active test drives 
-- Modified By : Ashwini Dhamankar on Oct 18,2016 (commented C.Address and fetched TD.TDAddress )
-- EXEC TC_TDScheduleDetails 5, '04/04/2016', '06/04/2016', '6/12/2013 11:59:59 PM'
-- ======================================================================= 
CREATE PROCEDURE [dbo].[TC_TDScheduleDetails]
	-- Add the parameters for the stored procedure here  
	@BranchId BIGINT
	,@FromDate DATE = NULL
	,@ToDate DATE = NULL
	,@TimeStamp DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from  
	-- interfering with SELECT statements.  
	SET NOCOUNT ON;

	SELECT TD.TC_TDCalendarId
		,C.Id AS CustomerId
		,C.CustomerName
		,C.Mobile
		,TD.TC_TDCarsId AS CarId
		,TD.TC_UsersId AS ConsultantId
		,TD.ArealId AS AreaId
		,CONVERT(VARCHAR, TD.TDDate, 112) AS TDDate
		,SUBSTRING(REPLACE(TD.TDStartTime, ':', ''), PATINDEX('%[^0 ]%', REPLACE(TD.TDStartTime, ':', '') + ' '), 6) AS TDStartTime
		,SUBSTRING(REPLACE(TD.TDEndTime, ':', ''), PATINDEX('%[^0 ]%', REPLACE(TD.TDEndTime, ':', '') + ' '), 6) AS TDEndTime
		,TD.TDDriverId AS DriverId
		,TD.TDStatus
		,NI.TC_NewCarInquiriesId AS InquiryId
		,TD.TDCarDetails AS CarName
		,TD.AreaName
		--,C.Address
		,TD.TDAddress AS Address   --Modified By : Ashwini Dhamankar on Oct 14,2016 (commented C.Address and fetched TD.TDAddress )
		,A.CityId
		,NI.TC_InquirySourceId AS SourceId
	FROM TC_TDCalendar TD
	JOIN TC_NewCarInquiries NI ON NI.TC_TDCalendarId = TD.TC_TDCalendarId
	JOIN TC_CustomerDetails C ON C.Id = TD.TC_CustomerId
	JOIN Areas A ON A.Id = TD.ArealId
	WHERE TD.TDDate BETWEEN @FromDate
			AND @ToDate
		AND (
			(TD.ModifiedDate > @TimeStamp)
			OR (
				@TimeStamp IS NULL
				AND TD.TDStatus IN (
					28
					,29
					,39
					)
				)
			)
		AND TD.BranchId = @BranchId
END
