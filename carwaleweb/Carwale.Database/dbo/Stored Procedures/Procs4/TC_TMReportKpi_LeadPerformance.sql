IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMReportKpi_LeadPerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMReportKpi_LeadPerformance]
GO

	--	Author		:	Ranjeet Kumar 25-Nov-13

---Copy from TC_ReportLeadPerformance1 
--	============================================================

CREATE Procedure [dbo].[TC_TMReportKpi_LeadPerformance] 
@TempTable TC_TempTableSpclUser READONLY,
--@LoggedInUser NUMERIC(20,0),
@FromDate	DATETIME = NULL,
@ToDate		DATETIME = NULL,
--@MakeId     NUMERIC(18,0),
@ModelId	NUMERIC(18,0) = NULL,
@VersionId	NUMERIC(18,0) = NULL


AS
BEGIN




SELECT * FROM @TempTable ORDER BY  ZoneName;



	
----TotalLead & Followup Data & Retail Lead
SELECT TSU1.UserName AS Dealer, TSU1.TC_SpecialUsersId AS DealerId,
	COUNT(DISTINCT(CASE WHEN  TBS.CreatedDate BETWEEN @FromDate AND @ToDate THEN  TBS.TC_LeadId END)) AS LeadCount, 
	COUNT(DISTINCT (CASE  WHEN TBS.TC_LeadStageId<>3 and  (TBS.ScheduledOn<= @ToDate) THEN TBS.TC_LeadId END )) AS PendingFollowUp, 
	COUNT(DISTINCT (CASE WHEN TBS.TC_LeadStageId<>3 AND TBS.TestDriveDate<=@ToDate AND ((TBS.TestDriveStatus<>27 AND TBS.TestDriveStatus<>28)OR TBS.TestDriveStatus IS NULL)  THEN TBS.TC_NewCarInquiriesId END )) AS PendingTestDrive
FROM 
     TC_LeadBasedSummary TBS WITH (NOLOCK) 
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
WHERE  (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
GROUP BY TSU1.UserName,TSU1.TC_SpecialUsersId
		
--Total Booking Data
SELECT TSU1.TC_SpecialUsersId AS DealerId, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS BookedLead
FROM  TC_LeadBasedSummary TBS WITH (NOLOCK)
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
WHERE TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4
	AND TBS.BookingDate BETWEEN @FromDate AND @ToDate 
	AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
GROUP BY  TSU1.TC_SpecialUsersId
	
--Lost Data
SELECT TSU1.TC_SpecialUsersId AS DealerId, COUNT(DISTINCT TBS.TC_LeadId) AS Lost	  
FROM  TC_LeadBasedSummary TBS WITH (NOLOCK)
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
WHERE TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID<>4 AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate 
	AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
GROUP BY TSU1.TC_SpecialUsersId

--TD_Completed Data
SELECT TSU1.TC_SpecialUsersId AS DealerId, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS TD_Completed	  
FROM  TC_LeadBasedSummary TBS WITH (NOLOCK)
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
WHERE TBS.TestDriveStatus = 28 AND TBS.TestDriveDate BETWEEN @FromDate AND @ToDate 
	AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
GROUP BY TSU1.TC_SpecialUsersId

--Booking Delivered Data
SELECT TSU1.TC_SpecialUsersId AS DealerId, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Delivered	  
FROM  TC_LeadBasedSummary TBS WITH (NOLOCK)
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
WHERE TBS.CarDeliveryStatus = 77 AND TBS.CarDeliveryDate BETWEEN @FromDate AND @ToDate AND 
		(TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
GROUP BY TSU1.TC_SpecialUsersId
	
--Live Bookings Data
SELECT TSU1.TC_SpecialUsersId AS DealerId, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS LiveBookings	  
FROM  TC_LeadBasedSummary TBS WITH (NOLOCK)
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
WHERE ISNULL(TBS.BookingStatus,0)=32 and TBS.InvoiceDate IS NULL 
      AND TBS.TC_LeadStageId<>3
	AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	AND TBS.BookingDate <= @ToDate
	AND  ISNULL(TBS.CarDeliveryStatus,0)<>77
	AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
GROUP BY TSU1.TC_SpecialUsersId

--Target Count		 
SELECT	SUM(TDT.TARGET)AS LeadTarget ,TDT.TC_TargetTypeId ,TSU1.TC_SpecialUsersId AS DealerId
FROM  TC_DealersTarget TDT(NOLOCK) 
		INNER JOIN  vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TDT.CarVersionId
		INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TDT.DealerId AND TSU1.IsDealer=1
WHERE (TDT.[MONTH]>=MONTH(@FromDate) AND TDT.[MONTH]<=MONTH(@ToDate)) 
		AND (TDT.[Year]>=YEAR(@FromDate) AND  TDT.[Year]<=YEAR(@ToDate)) 
			AND (TDT.CarVersionId = @VersionId OR @VersionId IS NULL)
			AND (V.ModelId = @ModelId OR @ModelId IS NULL)
		GROUP BY TDT.TC_TargetTypeId,TSU1.TC_SpecialUsersId

---Retail Leads
	SELECT  TSU1.UserName AS Dealer, TSU1.TC_SpecialUsersId AS DealerId, RetailLead
	 FROM
	 (
		 SELECT TSU1.TC_SpecialUsersId AS DealerId,
	--  Modified By: Manish on 16-09-2013 changing the logic of capturing Retails. Consider retail where invoice date is not null		
		COUNT  (DISTINCT  TBS.TC_NewCarInquiriesId ) AS RetailLead
	FROM 
		  TC_LeadBasedSummary TBS
		INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
		 WHERE (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
		AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate
		AND TBS.Invoicedate  IS NOT NULL 
        GROUP BY TSU1.TC_SpecialUsersId
	)	AS A
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId=A.DealerId
	
		



-------Pending Deliveries
	SELECT 	TSU1.UserName AS Dealer, TSU1.TC_SpecialUsersId AS DealerId,PendingDeliveries
	 FROM
	 (
		 SELECT  TSU1.TC_SpecialUsersId AS DealerId,
		COUNT(TBS.TC_LeadId ) AS PendingDeliveries
	FROM 
	   TC_LeadBasedSummary TBS
		INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
		 WHERE (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND TBS.Invoicedate  IS NOT NULL
		AND ISNULL(TBS.CarDeliveryStatus,0)<>77
		AND TBS.InvoiceDate <= @ToDate -- Added by Sachin Bharti(20-09-2013)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
		GROUP BY TSU1.TC_SpecialUsersId
	)	AS A
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId=A.DealerId
	

SELECT MAX(lvl) AS MAXLEVEL,MIN(lvl) AS MINLEVEL FROM @TempTable


		 		
END
