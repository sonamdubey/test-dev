IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetTestDriveCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetTestDriveCount]
GO

	
-- Author:		Afrose
-- Create date: 06th November 2015
-- Description:	To get test drive cars of past weeks and current week
-- Modified By : Upendra Kumar
--EXEC TC_GetTestDriveCount 5,'2015-11-01','2015-12-30'   SELECT TDCompletedCount,TDPendingCount FROM ( exec TC_GetTestDriveCount 5,'2015-11-01','2015-12-30') as temp
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetTestDriveCount]
@BranchId INT,
@FromDate DATE = NULL,
@ToDate DATE = NULL

AS
SET NOCOUNT ON

BEGIN

 SELECT  COUNT(CASE WHEN C.TDStatus = 28 AND CONVERT(DATE,C.TDDate) BETWEEN @FromDate and @ToDate THEN C.TC_TDCarsId END) AS TDCompletedCount,       --Completed count
         COUNT(CASE WHEN ( C.TDStatus = 29 OR  C.TDStatus =  39) AND CONVERT(DATE,C.TDDate) < GETDATE() THEN C.TC_TDCarsId END) AS TDPendingCount		--Pending count
   FROM TC_TDCalendar C WITH(NOLOCK)
     INNER JOIN TC_InquirySource S WITH (NOLOCK) ON C.TC_SourceId=S.Id 
     INNER JOIN TC_CustomerDetails CD WITH (NOLOCK) ON C.TC_CustomerId=CD.Id 
     INNER JOIN TC_Users U WITH (NOLOCK) ON C.TC_UsersId=U.Id
     LEFT JOIN TC_TDCars TC WITH (NOLOCK) ON TC.TC_TDCarsId=C.TC_TDCarsId
     LEFT JOIN TC_NewCarInquiries NC WITH (NOLOCK) ON NC.TC_TDCalendarId = C.TC_TDCalendarId
  WHERE C.BranchId=@BranchId 
     	
 
END
