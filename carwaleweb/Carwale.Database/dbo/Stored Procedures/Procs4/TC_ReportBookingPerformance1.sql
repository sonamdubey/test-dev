IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportBookingPerformance1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportBookingPerformance1]
GO

	

--	Author		:	Vivek Singh(18th oct 2013)
--	Purpose		:	Change condition for Live Bookings and add query for 
--					Retail and Pending Deliveries also added constraint for ToDate in live bookings all the Dealers under the Logged in user(Hierarchy Wise)
-- Copied from procedure [TC_ReportBookingPerformance]
--	============================================================

CREATE Procedure [dbo].[TC_ReportBookingPerformance1] 
@TempTable TC_TempTableSpclUser READONLY,
@FromDate	DATETIME = NULL,
@ToDate		DATETIME = NULL,	
@MakeId     NUMERIC(18,0),
@ModelId	NUMERIC(18,0) = NULL


AS
SET NOCOUNT ON;
BEGIN

SELECT * FROM @TempTable ORDER BY  ZoneName;
 --Complete Data	
	SELECT D.Organization AS Dealer, D.ID AS DealerId,
		DAY(TBS.BookingDate) AS Day, MONTH(TBS.BookingDate) AS Month,
		TBS.Source, ISNULL(TBS.Eagerness, 'Not Set') AS Eagerness,
		COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS BookedLead
		   				  
	FROM 
		DEALERS as D WITH (NOLOCK)
		LEFT JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId AND TBS.BookingDate BETWEEN @FromDate AND @ToDate  AND TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4 
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
	WHERE
		(TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	GROUP BY D.Organization,D.ID, 
		DAY(TBS.BookingDate), MONTH(TBS.BookingDate),TBS.Source, TBS.Eagerness
			
	
	--Model Wise Data
	SELECT COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS LeadCount, TBS.CarModel	  
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
			INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
			INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
			INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
	WHERE TBS.BookingDate BETWEEN @FromDate AND @ToDate AND
		TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4 AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	GROUP BY TBS.CarModel
	
	--Booked Delivered Data
	SELECT D.ID AS DealerId,COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Delivered	  
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
	WHERE TBS.CarDeliveryStatus = 77 AND TBS.CarDeliveryDate BETWEEN @FromDate AND @ToDate AND 
		(TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	GROUP BY D.ID
	
	--Live Bookings Data
	SELECT D.ID AS DealerId,COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS LiveBookings	  
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
		INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
	WHERE	ISNULL(TBS.BookingStatus,0)=32 --AND ISNULL(TBS.CarDeliveryStatus,0) <> 77 AND --Commented By Sachin Bharti(18th Sept 2013)Condition commented since in live booking taking the booked case but retail is pending.
			and TBS.InvoiceDate IS NULL AND TBS.TC_LeadStageId<>3 AND  --- Condition Added by Sachin on 18-09-2013 for taking only open lead bookings
		    (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
			AND TBS.BookingDate <= @ToDate--Added By Sachin Bharti(20-09-2013)
			AND  ISNULL(TBS.CarDeliveryStatus,0)<>77 ---- Condition added by manish on 23-09-2013 for handling old records also before Retail release
	GROUP BY D.ID
	
	--	Added by Sachin bharti(20th Sep 2013)
	--	Retail Leads
	SELECT  D.Organization AS Dealer, D.ID AS DealerId, RetailLead
	 FROM
	 (
		SELECT  TBZ.ZoneName,D.ID AS DealerId,COUNT  (DISTINCT  TBS.TC_NewCarInquiriesId ) AS RetailLead
		FROM DEALERS as D WITH (NOLOCK)
		INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId 
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
		WHERE  (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate
		AND TBS.Invoicedate  IS NOT NULL 
        GROUP BY D.ID,TBZ.ZoneName
	)	AS A
	INNER JOIN Dealers AS D ON D.ID=A.DealerId
	--INNER JOIN #TEMPDEALERS TSU1 WITH (NOLOCK) ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1 
	
	--	Added by Sachin bharti(20th Sep 2013)	
	--Pending Deliveries
	SELECT D.Organization AS Dealer, D.ID AS DealerId,PendingDeliveries
	 FROM
	 (
		SELECT  TBZ.ZoneName,D.ID AS DealerId,COUNT(DISTINCT TBS.TC_LeadId ) AS PendingDeliveries
		FROM 
		DEALERS as D WITH (NOLOCK)
		INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId  
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		WHERE (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND TBS.Invoicedate  IS NOT NULL
		AND ISNULL(TBS.CarDeliveryStatus,0)<>77
		AND TBS.InvoiceDate <= @ToDate 
		GROUP BY D.ID,TBZ.ZoneName
	)	AS A
	INNER JOIN Dealers AS D ON D.ID=A.DealerId
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
		
SELECT MAX(lvl) AS MAXLEVEL,MIN(lvl) AS MINLEVEL FROM @TempTable
		 
END






