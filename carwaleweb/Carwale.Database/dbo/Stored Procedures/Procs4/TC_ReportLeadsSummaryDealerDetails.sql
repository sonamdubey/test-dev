IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportLeadsSummaryDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportLeadsSummaryDealerDetails]
GO

	-- Author	:	Sachin Bharti(14th August 2013)
-- Purpose	:	To get employee wise leads count
-- Modifier :	Sachin Bharti(30th Sep 2013)
-- Purpose	:	Added constraint for versionId
CREATE PROCEDURE [dbo].[TC_ReportLeadsSummaryDealerDetails]
@DealerId	NUMERIC(18,0) ,
@FromDate	DateTime ,
@ToDate		DateTime ,
@ReportingUsersList VARCHAR(MAX) = '-1' ,
@ModelId	INT = NULL,
@VersionId	NUMERIC(18,0) = NULL	--Added by Sachin Bharti(30th Sep 2013)
AS 
BEGIN 
			SELECT U.UserName,U.lvl,U.Id, DAY(TLS.CreatedDate) AS Day, MONTH(TLS.CreatedDate) AS Month, U.NodeCode,
			COUNT(DISTINCT (CASE WHEN TLS.CreatedDate BETWEEN @FromDate AND @ToDate THEN TLS.TC_LeadId END)) AS TotalLead, 
			COUNT(DISTINCT (CASE WHEN TLS.TC_LeadStageId<>3 AND ISNULL(TLS.TC_LeadDispositionID,0)<>4  AND TLS.EagernessId = 1 AND TLS.CreatedDate BETWEEN @FromDate AND @ToDate THEN TLS.TC_LeadId END )) AS ActiveHotLead,
			COUNT(DISTINCT (CASE WHEN TLS.TC_LeadStageId<>3 AND ISNULL(TLS.TC_LeadDispositionID,0)<>4  AND TLS.EagernessId= 2 AND TLS.CreatedDate BETWEEN @FromDate AND @ToDate THEN TLS.TC_LeadId END )) AS ActiveWarmLead,
			COUNT(DISTINCT (CASE WHEN TLS.TC_LeadStageId<>3 AND ISNULL(TLS.TC_LeadDispositionID,0)<>4  AND TLS.EagernessId = 3 AND TLS.CreatedDate BETWEEN @FromDate AND @ToDate THEN TLS.TC_LeadId END )) AS ActiveNormalLead,
			COUNT(DISTINCT (CASE WHEN TLS.TC_LeadStageId<>3 AND ISNULL(TLS.TC_LeadDispositionID,0)<>4  AND ISNULL(TLS.EagernessId,0) = 0 AND TLS.CreatedDate BETWEEN @FromDate AND @ToDate THEN TLS.TC_LeadId END )) AS ActiveNotSet,
			COUNT(DISTINCT (CASE WHEN TLS.TestDriveStatus = 28 AND TLS.TestDriveDate BETWEEN @FromDate AND @ToDate THEN TLS.TC_NewCarInquiriesId END )) AS TDCompleted,
			COUNT(DISTINCT (CASE WHEN  TLS.TC_LeadDispositionId = 4 AND TLS.BookingStatus=32 AND TLS.BookingDate BETWEEN @FromDate AND @ToDate THEN TLS.TC_NewCarInquiriesId END )) AS BookedLead,
			COUNT(DISTINCT (CASE WHEN  TLS.CarDeliveryStatus = 77 AND TLS.CarDeliveryDate BETWEEN @FromDate AND @ToDate THEN TLS.TC_NewCarInquiriesId END )) AS DeliveredLead,
			COUNT(DISTINCT (CASE WHEN (TLS.TC_LeadStageId=3 AND  TLS.TC_LeadDispositionID<>4)AND TLS.LeadClosedDate BETWEEN @FromDate AND @ToDate THEN TLS.TC_LeadId END )) AS Lost,
			COUNT(DISTINCT (CASE WHEN (TLS.ScheduledOn<GETDATE()) AND TLS.CreatedDate BETWEEN @FromDate AND @ToDate THEN TLS.TC_LeadId END )) AS PendingFollowUp,
			COUNT(DISTINCT (CASE WHEN CONVERT(DATE,TLS.TestDriveDate) < CONVERT(DATE,GETDATE()) AND TLS.TestDriveStatus <> 27 AND TLS.TestDriveStatus <> 28 AND TLS.CreatedDate BETWEEN @FromDate AND @ToDate THEN TLS.TC_NewCarInquiriesId END)) AS PendingTestDrive 
			FROM TC_LeadBasedSummary AS TLS WITH (NOLOCK)
			INNER JOIN TC_Users U WITH (NOLOCK) ON U.ID=TLS.TC_UsersId AND TLS.DealerId = @DealerID AND U.lvl <> 0 AND U.IsActive = 1 AND (TLS.TC_UsersId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL)
			WHERE  (TLS.CarModelId = @ModelID OR @ModelId IS NULL)
			AND (TLS.CarVersionId = @VersionId OR @VersionId IS NULL) --Added by Sachin Bharti(30th Sep 2013)
			GROUP BY U.UserName,U.Id, U.lvl, DAY(TLS.CreatedDate), MONTH(TLS.CreatedDate),U.NodeCode
			ORDER BY U.NodeCode ASC
END