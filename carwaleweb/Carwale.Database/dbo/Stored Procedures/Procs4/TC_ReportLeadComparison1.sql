IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportLeadComparison1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportLeadComparison1]
GO

	--	Author		:	Vivek Singh(05th November 2013)

--	Description :-To get the leads data for camparison. 
--	============================================================

CREATE Procedure [dbo].[TC_ReportLeadComparison1] 
@TempTable TC_TempTableSpclUser READONLY,
@FromDate	DateTime = NULL,
@ToDate		DateTime = NULL,
@MakeId		NUMERIC(18,0),
@ModelId	NUMERIC(18,0) = NULL,
@DealerID	NUMERIC(18,0) = NULL,
@TYPE		TINYINT		=	NULL
AS
SET NOCOUNT ON;
BEGIN

IF @TYPE = 1
		BEGIN
			--Day wise lead data
			SELECT  DAY(TBS.CreatedDate) AS Day, MONTH(TBS.CreatedDate) AS Month,
				COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount
			FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
				INNER JOIN @TempTable TSU1  ON (TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1) OR TSU1.TC_SpecialUsersId IS NULL
			WHERE TBS.CreatedDate BETWEEN @FromDate AND @ToDate
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL) AND (D.ID = @DealerID OR @DealerID IS NULL) 
			GROUP BY DAY(TBS.CreatedDate), MONTH(TBS.CreatedDate)
		END
	IF @TYPE = 2
		BEGIN
			
			--TD_Completed Data
			SELECT DAY(TBS.TestDriveDate) AS Day, MONTH(TBS.TestDriveDate) AS Month, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS LeadCount	  
			FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
				INNER JOIN @TempTable TSU1  ON (TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1) OR TSU1.TC_SpecialUsersId IS NULL
			WHERE TBS.TestDriveStatus = 28 AND TBS.TestDriveDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
				AND (D.ID = @DealerID OR @DealerID IS NULL) 
			GROUP BY DAY(TBS.TestDriveDate) , MONTH(TBS.TestDriveDate)
		END
	IF @TYPE = 3
		BEGIN
			--Booking Data
			SELECT DAY(TBS.BookingDate) AS Day, MONTH(TBS.BookingDate) AS Month, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS LeadCount 
			FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
				INNER JOIN @TempTable TSU1  ON (TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1) OR TSU1.TC_SpecialUsersId IS NULL
			WHERE TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4  AND TBS.BookingDate BETWEEN @FromDate AND @ToDate
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)AND (D.ID = @DealerID OR @DealerID IS NULL) 
			GROUP BY  DAY(TBS.BookingDate) , MONTH(TBS.BookingDate)
		END
	IF @TYPE = 4
		BEGIN
			--Booking Delivered Data
			SELECT DAY(TBS.CarDeliveryDate) AS Day, MONTH(TBS.CarDeliveryDate) AS Month, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS LeadCount	  
			FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
				INNER JOIN @TempTable TSU1  ON (TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1) OR TSU1.TC_SpecialUsersId IS NULL
			WHERE TBS.CarDeliveryStatus = 77 AND TBS.CarDeliveryDate BETWEEN @FromDate AND @ToDate AND 
				 (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
				AND (D.ID = @DealerID OR @DealerID IS NULL) 
			GROUP BY DAY(TBS.CarDeliveryDate) , MONTH(TBS.CarDeliveryDate)
		END
	IF @TYPE = 5
		BEGIN
			--Lost Data
			SELECT DAY(TBS.LeadClosedDate) AS Day, MONTH(TBS.LeadClosedDate) AS Month, COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount	  
			FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
				INNER JOIN @TempTable TSU1  ON (TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1) OR TSU1.TC_SpecialUsersId IS NULL
			WHERE TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID<>4 AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL) AND (D.ID = @DealerID OR @DealerID IS NULL) 
			GROUP BY DAY(TBS.LeadClosedDate) , MONTH(TBS.LeadClosedDate)
		END
	
	IF @TYPE = 6
		BEGIN
			--Booking Cancelled Data
			--- Modified by Manish on 17-09-2013 for changing booking date to  booking cancelled date
			SELECT DAY(TBS.BookingCancelDate) AS Day, MONTH(TBS.BookingCancelDate) AS Month, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS LeadCount 
			FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
				INNER JOIN @TempTable TSU1  ON (TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1) OR TSU1.TC_SpecialUsersId IS NULL
			WHERE TBS.BookingStatus=31  AND TBS.BookingCancelDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)AND (D.ID = @DealerID OR @DealerID IS NULL) 
			GROUP BY  DAY(TBS.BookingCancelDate) , MONTH(TBS.BookingCancelDate)
		END
	
	IF @TYPE = 7
		BEGIN
			--Retail Data
			--Added By Sachin Bharti(20-09-2013)
			SELECT DAY(TBS.InvoiceDate) AS Day, MONTH(TBS.InvoiceDate) AS Month, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS LeadCount 
			FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
				INNER JOIN @TempTable TSU1  ON (TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1) OR TSU1.TC_SpecialUsersId IS NULL
			WHERE TBS.InvoiceDate IS NOT NULL AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)AND (D.ID = @DealerID OR @DealerID IS NULL) 
			GROUP BY  DAY(TBS.InvoiceDate) , MONTH(TBS.InvoiceDate)
		END
END
