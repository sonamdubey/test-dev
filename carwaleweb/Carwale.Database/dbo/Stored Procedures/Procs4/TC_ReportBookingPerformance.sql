IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportBookingPerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportBookingPerformance]
GO

	

--	Author		:	Deepak Tripathi(25th July 2013)
--	Modified	:	Sachin Bharti(25th July 2013) 
--					2.Sachin Bharti(30nd July 2013) 
--					  Purpose:Add ModelId constraint
--	Modifier	:	Sachin Bharti(20th Sep 2013)
--	Purpose		:	Change condition for Live Bookings and add query for 
--					Retail and Pending Deliveries also added constraint for ToDate in live bookings
-- Modified by  :  Manish on 23-09-2013 adding codition "ISNULL(TLS.CarDeliveryStatus,0)<>77" in live booking leads for handling old records also before Retail release 
-- Modified By  : Vivek Singh 21-03-2014 To get total booking data including cancellations 
--	============================================================

CREATE Procedure [dbo].[TC_ReportBookingPerformance] 
@FromDate	DateTime = NULL,
@ToDate		DateTime = NULL,
@MakeId		NUMERIC(18,0),
@ModelId	NUMERIC(18,0) = NULL,
@RMId		NUMERIC(18,0) = NULL,
@AMId		NUMERIC(18,0) = NULL

AS
BEGIN
	
	IF ISNULL(@RMId,0) <= 0
		SET @RMId = NULL
		
	IF ISNULL(@AMId,0) <= 0
		SET @AMId = NULL
		
	--Complete Data	
	SELECT TBZ.ZoneName, TSU.UserName AS RM, TSU.TC_SpecialUsersId AS RMId,
		 TSU1.UserName AS AM,  TSU1.TC_SpecialUsersId AS AMId,
		D.Organization AS Dealer, D.ID AS DealerId,
		DAY(TBS.BookingDate) AS Day, MONTH(TBS.BookingDate) AS Month,
		TBS.Source, ISNULL(TBS.Eagerness, 'Not Set') AS Eagerness,
		COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS BookedLead
		   				  
	FROM 
		DEALERS as D WITH (NOLOCK)
		LEFT JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId AND TBS.BookingDate BETWEEN @FromDate AND @ToDate  AND TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4 
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		INNER JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
		INNER JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
	WHERE (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL)
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	GROUP BY TBZ.ZoneName, TSU.UserName, TSU1.UserName, D.Organization, TSU.TC_SpecialUsersId, TSU1.TC_SpecialUsersId, D.ID, 
		DAY(TBS.BookingDate), MONTH(TBS.BookingDate),TBS.Source, TBS.Eagerness
			
	
	--Model Wise Data
	SELECT COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS LeadCount, TBS.CarModel	  
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
			INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
			INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
			INNER JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1 AND TSU.IsActive = 1
	WHERE (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) AND TBS.BookingDate BETWEEN @FromDate AND @ToDate AND
		TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4 AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	GROUP BY TBS.CarModel
	
	--Booked Delivered Data
	SELECT D.ID AS DealerId, D.TC_RMID AS RMId, D.TC_AMId AS AMId, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Delivered	  
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE TBS.CarDeliveryStatus = 77 AND TBS.CarDeliveryDate BETWEEN @FromDate AND @ToDate AND 
		(D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL)AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	GROUP BY D.ID, D.TC_RMID, D.TC_AMId
	
	--Live Bookings Data
	SELECT D.ID AS DealerId, D.TC_RMID AS RMId, D.TC_AMId AS AMId, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS LiveBookings	  
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE	ISNULL(TBS.BookingStatus,0)=32 --AND ISNULL(TBS.CarDeliveryStatus,0) <> 77 AND --Commented By Sachin Bharti(18th Sept 2013)Condition commented since in live booking taking the booked case but retail is pending.
			and TBS.InvoiceDate IS NULL AND TBS.TC_LeadStageId<>3 AND  --- Condition Added by Sachin on 18-09-2013 for taking only open lead bookings
			(D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL)AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
			AND TBS.BookingDate <= @ToDate--Added By Sachin Bharti(20-09-2013)
			AND  ISNULL(TBS.CarDeliveryStatus,0)<>77 ---- Condition added by manish on 23-09-2013 for handling old records also before Retail release
	GROUP BY D.ID, D.TC_RMID, D.TC_AMId	
	
	--	Added by Sachin bharti(20th Sep 2013)
	--	Retail Leads
	SELECT  ZoneName, TSU.UserName AS RM, TSU.TC_SpecialUsersId AS RMId,TSU1.UserName AS AM,  TSU1.TC_SpecialUsersId AS AMId,
			D.Organization AS Dealer, D.ID AS DealerId, RetailLead
	 FROM
	 (
		SELECT  TBZ.ZoneName,D.ID AS DealerId,COUNT  (DISTINCT  TBS.TC_NewCarInquiriesId ) AS RetailLead
		FROM DEALERS as D WITH (NOLOCK)
		INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId 
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		WHERE (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) 
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate
		AND TBS.Invoicedate  IS NOT NULL 
        GROUP BY D.ID,TBZ.ZoneName
	)	AS A
	INNER JOIN Dealers AS D ON D.ID=A.DealerId
	INNER JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
	INNER JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1 
	
	--	Added by Sachin bharti(20th Sep 2013)	
	--Pending Deliveries
	SELECT  ZoneName, TSU.UserName AS RM, TSU.TC_SpecialUsersId AS RMId,TSU1.UserName AS AM,  TSU1.TC_SpecialUsersId AS AMId,
	D.Organization AS Dealer, D.ID AS DealerId,PendingDeliveries
	 FROM
	 (
		SELECT  TBZ.ZoneName,D.ID AS DealerId,COUNT(DISTINCT TBS.TC_LeadId ) AS PendingDeliveries
		FROM 
		DEALERS as D WITH (NOLOCK)
		INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId  
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		WHERE (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) 
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND TBS.Invoicedate  IS NOT NULL
		AND ISNULL(TBS.CarDeliveryStatus,0)<>77
		AND TBS.InvoiceDate <= @ToDate 
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
	GROUP BY  D.ID, D.TC_RMID, D.TC_AMId
END
