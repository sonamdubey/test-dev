IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportLeadPerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportLeadPerformance]
GO

	
--	Author		:	Deepak Tripathi(2nd July 2013)
--	Modified	:	1.Sachin Bharti(3rd July 2013) 
--					2.Sachin Bharti(30nd July 2013) 
--					  Purpose:Add ModelId constraint
--- Modified By : Manish on 10-09-2013 for taking only open lead bookings added condition tc_leadstageid<>3
-- Modified By: Sachin on 13-09-2013 change in report implementing target
-- Modified By: Manish on 16-09-2013 changing the logic of capturing Retails. Consider retail where invoice date is not null
-- Modified By Sachin Bharti 16th Sep 2013(Added constraint for RMId and AMId)
-- Modified by: Manish on 18-09-2013 changing the query for PendingDeliveries, Retail  only for optimisation purpose
-- Modified by: Manish on 20-09-2013 Separate the query for Pending deliveries and Retail
-- Modified By: Sachin bharti on 20-09-103 Added constraint for ToDate on LiveBookings,PendingDeliveries,PendingTestDrive and PendingFollowUp 
-- Modified by: Manish on 23-09-2013 adding codition "ISNULL(TLS.CarDeliveryStatus,0)<>77" in live booking leads for handling old records also before Retail release 
-- Modified BY: Sachin Bharti(26th Sep 2013) Added a parameter for VersionId 
-- Modified by Vivek singh(14th n0v 2013) Removed distinct clause from the pending deliveries count
-- Modified By  : Vivek Singh 21-03-2014 To get total booking data including cancellations 
--	============================================================

CREATE Procedure [dbo].[TC_ReportLeadPerformance] 
@FromDate	DateTime = NULL,
@ToDate		DateTime = NULL,
@MakeId		NUMERIC(18,0),
@ModelId	NUMERIC(18,0) = NULL,
@RMId		NUMERIC(18,0) = NULL,
@AMId		NUMERIC(18,0) = NULL,
@VersionId	NUMERIC(18,0) = NULL --Sachin Bharti(26th Sep 2013) Added a parameter for VersionId 

AS
BEGIN
	
	IF ISNULL(@RMId,0) <= 0
		SET @RMId = NULL
		
	IF ISNULL(@AMId,0) <= 0
		SET @AMId = NULL
	
	--Day wise lead data
	SELECT  DAY(TBS.CreatedDate) AS Day, MONTH(TBS.CreatedDate) AS Month,
		COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE TBS.CreatedDate BETWEEN @FromDate AND @ToDate AND (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) 
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
	GROUP BY DAY(TBS.CreatedDate), MONTH(TBS.CreatedDate)
	
	--Model Wise Data
	SELECT COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount, TBS.CarModel	  
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
			INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
			INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE TBS.CreatedDate BETWEEN @FromDate AND @ToDate AND (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) 
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
	GROUP BY TBS.CarModel
	
	--Source & Eagerness Wise Data
	SELECT COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount, ISNULL(TBS.Source, 'NA') AS Source,ISNULL(TBS.Eagerness, 'Not Yet Set') AS Eagerness
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE TBS.CreatedDate BETWEEN @FromDate AND @ToDate AND (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) 
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
	GROUP BY TBS.Source, TBS.Eagerness
	
	--TotalLead & Followup Data & PendingTestDrive
	SELECT TBZ.ZoneName, TSU.UserName AS RM, TSU.TC_SpecialUsersId AS RMId,
		 TSU1.UserName AS AM,  TSU1.TC_SpecialUsersId AS AMId,
		D.Organization AS Dealer, D.ID AS DealerId,
		COUNT(DISTINCT(CASE WHEN  TBS.CreatedDate BETWEEN @FromDate AND @ToDate THEN  TBS.TC_LeadId END)) AS LeadCount,
		--COUNT(DISTINCT (CASE WHEN (TBS.ScheduledOn<GETDATE() ) THEN TBS.TC_LeadId END )) AS PendingFollowUp, Commented By Sachin Bharti(20-09-2013)
		COUNT(DISTINCT (CASE  WHEN TBS.TC_LeadStageId<>3 and  (TBS.ScheduledOn<= @ToDate) THEN TBS.TC_LeadId END )) AS PendingFollowUp, --Added by Sachin Bharti(20-09-2013)
		--COUNT(DISTINCT (CASE WHEN TBS.TestDriveDate<CONVERT(DATE,GETDATE()) AND ((TBS.TestDriveStatus<>27 AND TBS.TestDriveStatus<>28)OR TBS.TestDriveStatus IS NULL)  THEN TBS.TC_NewCarInquiriesId END )) AS PendingTestDrive	  Commented by Sachin Bharti(20-09-2013)
		COUNT(DISTINCT (CASE WHEN TBS.TC_LeadStageId<>3 AND TBS.TestDriveDate<=@ToDate AND ((TBS.TestDriveStatus<>27 AND TBS.TestDriveStatus<>28)OR TBS.TestDriveStatus IS NULL)  THEN TBS.TC_NewCarInquiriesId END )) AS PendingTestDrive
	FROM 
		DEALERS as D WITH (NOLOCK)
		INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId 
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		INNER JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
		INNER JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1 
	WHERE (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) 
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
	GROUP BY TBZ.ZoneName, TSU.UserName, TSU.TC_SpecialUsersId,
		 TSU1.UserName,  TSU1.TC_SpecialUsersId, D.Organization, D.ID 
		
	--Total Booking Data
	SELECT D.ID AS DealerId, D.TC_RMID AS RMId, D.TC_AMId AS AMId, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS BookedLead
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4  
		AND TBS.BookingDate BETWEEN @FromDate AND @ToDate AND (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL)
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
	GROUP BY  D.ID, D.TC_RMID, D.TC_AMId
	
	--Lost Data
	SELECT D.ID AS DealerId, D.TC_RMID AS RMId, D.TC_AMId AS AMId, COUNT(DISTINCT TBS.TC_LeadId) AS Lost	  
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID<>4 AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate AND (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL)
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
	GROUP BY D.ID, D.TC_RMID, D.TC_AMId
	
	--TD_Completed Data
	SELECT D.ID AS DealerId, D.TC_RMID AS RMId, D.TC_AMId AS AMId, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS TD_Completed	  
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE TBS.TestDriveStatus = 28 AND TBS.TestDriveDate BETWEEN @FromDate AND @ToDate 
		AND (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL)AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
	GROUP BY D.ID, D.TC_RMID, D.TC_AMId
	
	--Booking Delivered Data
	SELECT D.ID AS DealerId, D.TC_RMID AS RMId, D.TC_AMId AS AMId, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Delivered	  
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE TBS.CarDeliveryStatus = 77 AND TBS.CarDeliveryDate BETWEEN @FromDate AND @ToDate AND 
		(D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL)AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
	GROUP BY D.ID, D.TC_RMID, D.TC_AMId
	
	--Live Bookings Data
	SELECT D.ID AS DealerId, D.TC_RMID AS RMId, D.TC_AMId AS AMId, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS LiveBookings	  
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE ISNULL(TBS.BookingStatus,0)=32 and TBS.InvoiceDate IS NULL 
		AND TBS.TC_LeadStageId<>3  --- Condition Added by Manish on 10-09-2013 for taking only open lead bookings
		AND (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND TBS.BookingDate <= @ToDate -- Added by Sachin Bharti(20-09-2013)
		AND  ISNULL(TBS.CarDeliveryStatus,0)<>77 ---- Condition added by manish on 23-09-2013 for handling old records also before Retail release
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
	GROUP BY D.ID, D.TC_RMID, D.TC_AMId	

	--Target Count		 
	SELECT	SUM(TDT.TARGET)AS LeadTarget ,TDT.TC_TargetTypeId ,D.ID AS DealerId,
			D.TC_AMId AS AMId,D.TC_RMId AS RMId
	FROM TC_DealersTarget TDT(NOLOCK) 
			INNER JOIN Dealers AS D WITH (NOLOCK) ON TDT.DealerId=D.Id
			INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TDT.CarVersionId  -- Modified BY: Sachin Bharti(26th Sep 2013) condition for VersionId 
	WHERE (TDT.[MONTH]>=MONTH(@FromDate) AND TDT.[MONTH]<=MONTH(@ToDate)) 
			AND (TDT.[Year]>=YEAR(@FromDate) AND  TDT.[Year]<=YEAR(@ToDate)) 
			AND (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) --Modified By Sachin Bharti 16th Sep 2013(Added constraint for RMId and AMId)
			AND (TDT.CarVersionId = @VersionId OR @VersionId IS NULL)
			AND (V.ModelId = @ModelId OR @ModelId IS NULL)
	GROUP BY TDT.TC_TargetTypeId,D.ID,D.TC_AMId,D.TC_RMId
	
		
	-- Modified by: Manish on 20-09-2013 Separate the query for Pending deliveries and Retail
	
	-------- Retail
	SELECT  ZoneName, TSU.UserName AS RM, TSU.TC_SpecialUsersId AS RMId,
	 TSU1.UserName AS AM,  TSU1.TC_SpecialUsersId AS AMId,
	D.Organization AS Dealer, D.ID AS DealerId, RetailLead
	 FROM
	 (
		 SELECT  TBZ.ZoneName,D.ID AS DealerId,
	--  Modified By: Manish on 16-09-2013 changing the logic of capturing Retails. Consider retail where invoice date is not null		
		COUNT  (DISTINCT  TBS.TC_NewCarInquiriesId ) AS RetailLead
	FROM 
		DEALERS as D WITH (NOLOCK)
		INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId  --AND TBS.CreatedDate  BETWEEN @FromDate AND @ToDate
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		 WHERE (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) 
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
		AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate
		AND TBS.Invoicedate  IS NOT NULL 
        GROUP BY D.ID,TBZ.ZoneName
	)	AS A
	INNER JOIN Dealers AS D ON D.ID=A.DealerId
	INNER JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
	INNER JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1 
		
	-------Pending Deliveries
	SELECT  ZoneName, TSU.UserName AS RM, TSU.TC_SpecialUsersId AS RMId,
	 TSU1.UserName AS AM,  TSU1.TC_SpecialUsersId AS AMId,
	D.Organization AS Dealer, D.ID AS DealerId,PendingDeliveries
	 FROM
	 (
		 SELECT  TBZ.ZoneName,D.ID AS DealerId,
		COUNT(TBS.TC_LeadId ) AS PendingDeliveries  --Modified by Vivek singh(14th n0v 2013) Removed distinct clause from the pending deliveries count
	FROM 
		DEALERS as D WITH (NOLOCK)
		INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId  --AND TBS.CreatedDate  BETWEEN @FromDate AND @ToDate
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		 WHERE (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) 
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND TBS.Invoicedate  IS NOT NULL
		AND ISNULL(TBS.CarDeliveryStatus,0)<>77
		AND TBS.InvoiceDate <= @ToDate -- Added by Sachin Bharti(20-09-2013)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
		GROUP BY D.ID,TBZ.ZoneName
	)	AS A
	INNER JOIN Dealers AS D ON D.ID=A.DealerId
	INNER JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
	INNER JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1 


	 ---Total Booking data(booked and cancelled)	by vivek singh 21-03-2014
    SELECT D.ID AS DealerId, D.TC_RMID AS RMId, D.TC_AMId AS AMId, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS TotalBookedLead
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE (TBS.BookingDate BETWEEN @FromDate AND @ToDate)
		 AND (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL)
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) 
	GROUP BY  D.ID, D.TC_RMID, D.TC_AMId
 
 
END

