IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetServiceList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetServiceList]
GO

	
-- Author:  AFROSE & UPENDRA   
-- Created date:   15-10-2015  
-- Description: To get Service Request Inquiries
-- EXEC TC_GetServiceList 5,'2015-10-01','2015-10-27',NULL,NULL,NULL,NULL,2,1
--EXEC TC_GetServiceList 5,NULL,NULL,NULL,NULL,NULL,NULL,4,1
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetServiceList]  
(
 @BranchId INT,
 @StartDate DATETIME=NULL,
 @EndDate DATETIME=NULL,
 @CustomerMobile VARCHAR(10)=NULL,   
 @ServiceStatusId INT=NULL,
 @RegistrationNo VARCHAR(10)=NULL,
 @ServiceCenterId INT=NULL,
 @SortOrder TINYINT=1,
 @PageNumber TINYINT=1,
 --@PageSize TINYINT=NULL,
 @SelectedRows INT=NULL OUTPUT
)
AS
 SET NOCOUNT ON   
BEGIN 
      SELECT
		   TCSI.TC_ServiceInquiriesId,
		   ROW_NUMBER() OVER(ORDER BY
		                       CASE WHEN @SortOrder=1 THEN (TCSI.ServiceDate) END ASC, 
							   CASE WHEN @SortOrder=2 THEN (TCSI.ServiceDate) END DESC,
							   CASE WHEN @SortOrder=3 THEN (TCSI.RequestDate) END ASC,
							   CASE WHEN @SortOrder=4 THEN (TCSI.RequestDate) END DESC	) 
		   AS ROWNUMBER,
		   TCSI.CustomerName,
		   TCSI.CustomerMobile,
		   TCSI.ServiceDate,
		   TCSI.RequestDate,
		   TCSI.RegNo,
		   TCSI.Kms,
		   TCSS.ServiceStatus,
		   TCSC.ServiceCenterName,
		   TCSIS.SourceName 
	  INTO #TableForPaging
	  FROM TC_ServiceInquiries TCSI WITH(NOLOCK) 
		  JOIN TC_ServiceInqSource TCSIS WITH(NOLOCK) ON TCSI.TC_ServiceInqSourceId=TCSIS.TC_ServiceInqSourceId
		  JOIN TC_ServiceCenter TCSC WITH(NOLOCK) ON TCSC.TC_ServiceCenterId=TCSI.TC_ServiceCenterId
		  JOIN TC_ServiceStatus TCSS WITH(NOLOCK) ON TCSS.TC_ServiceStatusId=TCSI.TC_ServiceStatusId
	  WHERE TCSI.BranchId=@BranchId		 
		  AND (@StartDate IS NULL OR @EndDate IS NULL OR CONVERT(DATE,TCSI.ServiceDate) BETWEEN @StartDate AND @EndDate)--TCSI.RequestDate BETWEEN @StartDate AND @EndDate
		  AND (@RegistrationNo IS NULL OR TCSI.RegNo=@RegistrationNo)
		  AND(@CustomerMobile IS NULL OR TCSI.CustomerMobile=@CustomerMobile)
		  AND(@ServiceStatusId IS NULL OR TCSI.TC_ServiceStatusId=@ServiceStatusId)
		  AND(@ServiceCenterId IS NULL OR TCSI.TC_ServiceCenterId=@ServiceCenterId)

    
	DECLARE @FirstRow INT, @LastRow INT ,@PageSize INT--To Calculate paging parameters
	SET @PageSize = 10
	SET @FirstRow = ((@PageNumber - 1) * @PageSize) + 1
    SET @LastRow    = @FirstRow + @PageSize -1
			   
	SELECT @SelectedRows=COUNT(*) FROM #TableForPaging TBLPG
	
	SELECT DISTINCT(
		   TBLPG.TC_ServiceInquiriesId),
		   TBLPG.ROWNUMBER,
		   TBLPG.CustomerName,
		   TBLPG.CustomerMobile,
		   TBLPG.ServiceDate,
		   TBLPG.RequestDate,
		   TBLPG.RegNo,
		   TBLPG.Kms,
		   TBLPG.ServiceStatus,
		   TBLPG.ServiceCenterName,
		   TBLPG.SourceName 
	FROM 
	#TableForPaging TBLPG WITH(NOLOCK) WHERE TBLPG.ROWNUMBER BETWEEN @FirstRow AND @LastRow  
	ORDER BY TBLPG.ROWNUMBER
 	
	DROP TABLE #TableForPaging
 END 