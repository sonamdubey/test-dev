IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDReportSendExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDReportSendExcel]
GO

	-- ================================================
-- Author:		Vivek Gupta
-- Create date: 8th March,2013 
-- Modified by: Afrose on 11th November, 2015 added case in where clause
-- Description:	Send To Excel The TDReport on TDCalender
-- TC_TDReportSendExcel 5,NULL,NULL,NULL,NULL,NULL,NULL
-- ================================================
CREATE PROCEDURE [dbo].[TC_TDReportSendExcel]
@BranchId INT,
@TDsDate DATETIME,
@TDeDate DATETIME,
@TC_UsersId VARCHAR(20),
@TDStatus VARCHAR(10),
@TC_TDCarsId VARCHAR(20),
@TDAreaId VARCHAR(20)
AS
BEGIN

	SELECT
	        C.TDDate,
			CD.CustomerName,
			S.Source,
			CONVERT(VARCHAR(10),
			C.TDStartTime,100) + ' to ' + CONVERT(VARCHAR(10),
			C.TDEndTime,100) as 'Time' , 
            AreaName,
            U.UserName,
            C.TDCarDetails,
            Status= CASE C.TDStatus WHEN 39 THEN 'Confirmed' WHEN 28 THEN 'Completed' WHEN 27 THEN 'Cancelled' WHEN 29 THEN 'Rescheduled' END 
                    
					FROM		        
					                    TC_TDCalendar C WITH(NOLOCK)  
							 INNER JOIN TC_InquirySource S WITH (NOLOCK) ON C.TC_SourceId=S.Id 
							 INNER JOIN TC_CustomerDetails CD WITH (NOLOCK) ON C.TC_CustomerId=CD.Id
							 INNER JOIN TC_Users U WITH (NOLOCK) ON C.TC_UsersId=U.Id
							 INNER JOIN TC_TDCars TC WITH (NOLOCK) ON TC.TC_TDCarsId=C.TC_TDCarsId
							 LEFT JOIN  TC_NewCarInquiries NC WITH (NOLOCK) ON NC.TC_TDCalendarId = C.TC_TDCalendarId
					WHERE   
					         C.BranchId = @BranchId 
					         AND C.TDStatus <>0
					         AND ((@TDsDate IS NULL)    OR (C.TDDate >= @TDsDate)) 
					         AND ((@TDeDate IS NULL)    OR (C.TDDate <= @TDeDate))
					         AND ((@TC_UsersId IS NULL) OR (U.Id = @TC_UsersId))
					         AND ((@TDStatus IS NULL)      OR(C.TDStatus IN (SELECT ListMember FROM fnSplitCSV(CASE @TDStatus WHEN 1 THEN '29,39' ELSE @TDStatus END))))
					         AND ((@TC_TDCarsId IS NULL)   OR(C.TC_TDCarsId = @TC_TDCarsId))
					         AND ((@TDAreaId IS NULL)      OR(C.ArealId = @TDAreaId))
					         AND TC.IsActive=1
							 ORDER BY C.TDDate,TDStartTime DESC
					         
END
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

