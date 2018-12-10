IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDCalendarDatewise]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDCalendarDatewise]
GO

	
-- =============================================
-- Author:		Surendra
-- Create date: 19 June 2012
-- Description:	this will return all record(test drive booking) for given date
-- EXECUTE TC_TDCalendarDatewise null ,5,null
-- Modified by : Tejashree :changed order by descending
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDCalendarDatewise]
@TDDate DATE,
@BranchId BIGINT,
@TDCarId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF(@TDCarId IS NULL AND @TDDate IS NULL )
	BEGIN
		SELECT C.TC_TDCalendarId,C.TDDate, CD.CustomerName,S.Source,CONVERT(VARCHAR(10),C.TDStartTime,100) + ' to ' + CONVERT(VARCHAR(10),C.TDEndTime,100) as 'Time',
			AreaName,U.UserName,C.TDCarDetails,C.TDStatus as StatusId,Status=
			CASE C.TDStatus
			WHEN 1 THEN 'Tentative'
			WHEN 2 THEN 'Confirmed'	
			WHEN 3 THEN 'Complete'	
			WHEN 4 THEN 'Cancelled'			
			END --1=Tentative,2=Confirmed,3=Complete,4=Cancel,Others=0
		FROM TC_TDCalendar C WITH(NOLOCK)
		INNER JOIN TC_InquirySource S WITH(NOLOCK) ON C.TC_SourceId=S.Id
		INNER JOIN TC_CustomerDetails CD WITH(NOLOCK) ON C.TC_CustomerId=CD.Id
		INNER JOIN TC_Users U ON C.TC_UsersId=U.Id
		INNER JOIN TC_TDCars TC ON TC.TC_TDCarsId=C.TC_TDCarsId
		WHERE C.BranchId=@BranchId AND TC.IsActive=1
		ORDER BY TDDate DESC,TDStartTime DESC
	END
	ELSE IF(@TDCarId IS NULL AND @TDDate IS NOT NULL )
		BEGIN
			SELECT C.TC_TDCalendarId,C.TDDate, CD.CustomerName,S.Source,CONVERT(VARCHAR(10),C.TDStartTime,100) + ' to ' + CONVERT(VARCHAR(10),C.TDEndTime,100) as 'Time',
				AreaName,U.UserName,C.TDCarDetails,C.TDStatus as StatusId,Status=
				CASE C.TDStatus
				WHEN 1 THEN 'Tentative'
				WHEN 2 THEN 'Confirmed'
				WHEN 3 THEN 'Complete'	
				WHEN 4 THEN 'Cancelled'				
				END --1=Tentative,2=Confirmed,3=Complete,4=Cancel,Others=0
			FROM TC_TDCalendar C WITH(NOLOCK)
			INNER JOIN TC_InquirySource S WITH(NOLOCK) ON C.TC_SourceId=S.Id
			INNER JOIN TC_CustomerDetails CD WITH(NOLOCK) ON C.TC_CustomerId=CD.Id
			INNER JOIN TC_Users U ON C.TC_UsersId=U.Id
			INNER JOIN TC_TDCars TC ON TC.TC_TDCarsId=C.TC_TDCarsId
			WHERE C.TDDate=@TDDate AND C.BranchId=@BranchId AND TC.IsActive=1
			ORDER BY TDDate DESC,TDStartTime DESC
		END
	ELSE IF(@TDCarId IS NOT NULL AND @TDDate IS NULL )
		BEGIN
			SELECT C.TC_TDCalendarId,C.TDDate, CD.CustomerName,S.Source,CONVERT(VARCHAR(10),C.TDStartTime,100) + ' to ' + CONVERT(VARCHAR(10),C.TDEndTime,100) as 'Time',
				AreaName,U.UserName,C.TDCarDetails,C.TDStatus as StatusId,Status=
				CASE C.TDStatus
				WHEN 1 THEN 'Tentative'
				WHEN 2 THEN 'Confirmed'	
				WHEN 3 THEN 'Complete'	
				WHEN 4 THEN 'Cancelled'			
				END --1=Tentative,2=Confirmed,3=Complete,4=Cancel,Others=0
			FROM TC_TDCalendar C WITH(NOLOCK)
			INNER JOIN TC_InquirySource S WITH(NOLOCK) ON C.TC_SourceId=S.Id
			INNER JOIN TC_CustomerDetails CD WITH(NOLOCK) ON C.TC_CustomerId=CD.Id
			INNER JOIN TC_Users U ON C.TC_UsersId=U.Id
			INNER JOIN TC_TDCars TC ON TC.TC_TDCarsId=C.TC_TDCarsId
			WHERE C.TC_TDCarsId=@TDCarId AND C.BranchId=@BranchId AND TC.IsActive=1
			ORDER BY TDDate DESC,TDStartTime DESC
		END
	ELSE
		BEGIN-- checking here date and car base
			SELECT C.TC_TDCalendarId,C.TDDate, CD.CustomerName,S.Source,CONVERT(VARCHAR(10),C.TDStartTime,100) + ' to ' + CONVERT(VARCHAR(10),C.TDEndTime,100) as 'Time',
				AreaName,U.UserName,C.TDCarDetails,C.TDStatus as StatusId,
				Status=
				CASE C.TDStatus
				WHEN 1 THEN 'Tentative'
				WHEN 2 THEN 'Confirmed'
				WHEN 3 THEN 'Complete'	
				WHEN 4 THEN 'Cancelled'			
				END --1=Tentative,2=Confirmed,3=Complete,4=Cancel,Others=0
				
			FROM TC_TDCalendar C WITH(NOLOCK)
			INNER JOIN TC_InquirySource S WITH(NOLOCK) ON C.TC_SourceId=S.Id
			INNER JOIN TC_CustomerDetails CD WITH(NOLOCK) ON C.TC_CustomerId=CD.Id
			INNER JOIN TC_Users U ON C.TC_UsersId=U.Id
			INNER JOIN TC_TDCars TC ON TC.TC_TDCarsId=C.TC_TDCarsId
			WHERE C.TDDate=@TDDate AND C.BranchId=@BranchId AND C.TC_TDCarsId=@TDCarId AND TC.IsActive=1
			ORDER BY TDDate DESC,TDStartTime DESC
		END

END

