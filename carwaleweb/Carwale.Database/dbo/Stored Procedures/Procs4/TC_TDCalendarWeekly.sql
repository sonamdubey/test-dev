IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDCalendarWeekly]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDCalendarWeekly]
GO

	-- =============================================  06/28/2013,76,203
-- Author:		Surendra
-- Create date: 20 June 2012
-- Description:	this will return all record(test drive booking) for given date
-- EXECUTE TC_TDCalendarWeekly '2012-06-28' ,7,5
-- Modified by Nilesh Utture On 2nd of January, 2013, Added AreaName in SELECT and Removed Source as per req.
-- Modified by Umesh Ojha on 28 June 2013 added one more parameter @TC_UsersId
-- Modified by Umesh Ojha on 3 july 2013 for showing completed TD also
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDCalendarWeekly]
@TDDate DATE,
@TdCarId BIGINT,
@BranchId BIGINT,
@TC_UsersId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT C.TC_TDCarsId,CD.CustomerName,U.UserName,C.AreaName, -- Modified by Nilesh Utture On 2nd of January, 2013
		DATEPART(hh,C.TDStartTime) AS StartHr,DATEPART(MI,C.TDStartTime) As StartMin,
		DATEPART(hh,C.TDEndTime) AS EndHr ,DATEPART(MI,C.TDEndTime) AS EndMin,
		CASE DATEPART(WEEKDAY,TDDate)  
		WHEN 1 THEN 'sun' 
		WHEN 2 THEN 'mon' 
		WHEN 3 THEN 'tue' 
		WHEN 4 THEN 'wed' 
		WHEN 5 THEN 'thu' 
		WHEN 6 THEN 'fri' 
		WHEN 7 THEN 'sat' 
    END AS WDay,
    CASE WHEN C.TC_UsersId = @TC_UsersId
    THEN '1' ELSE '0' END AS UserAvailable,
    CASE WHEN C.TC_TDCarsId = @TdCarId
    THEN '1' ELSE '0' END AS CarAvailable    
	FROM TC_TDCalendar C WITH(NOLOCK)
	INNER JOIN TC_CustomerDetails CD WITH(NOLOCK) ON C.TC_CustomerId=CD.Id
	INNER JOIN TC_Users U ON C.TC_UsersId=U.Id
	WHERE 
		C.BranchId=@BranchId AND 
		(C.TC_TDCarsId=@TdCarId OR 
		C.TC_UsersId=@TC_UsersId )AND
		C.TDDate BETWEEN @TDDate AND DATEADD(dd,6,@TDDate) AND TDStatus IN(28,29,39)
	
	ORDER BY TDDate ,TDStartTime ASC

END
