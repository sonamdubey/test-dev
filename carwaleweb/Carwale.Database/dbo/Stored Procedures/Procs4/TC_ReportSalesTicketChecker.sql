IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportSalesTicketChecker]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportSalesTicketChecker]
GO

	-- Author : Sachin Bharti(20th August 2013)
-- Modifier : Sachin Bharti(22nd Aug 2013)Add BrandZone constraint
-- Modified By: Manish on 02-09-2013 Used TIL.TC_LeadDispositionId in place on TCN.TC_LeadDispositionId
-- Modified By: Manish use convert(date,TCN.CarDeliveryDate) in place of   CONVERT(VARCHAR(10), TCN.CarDeliveryDate, 103)
-- Modified By: Sachin Bharti(20-09-2013) Commented query for Delivery Counts ans added Query for Retail leads
-- Modified By: Sachin Bharti(20-09-2013) UnCommented query for Delivery Counts
-- Modified By: Sachin Bharti(10-01-2013) Added parameter for VersionId 
CREATE PROCEDURE [dbo].[TC_ReportSalesTicketChecker] 
@RMId		NUMERIC(18,0)= NULL,
@AMId		NUMERIC(18,0)= NULL,
@FromDate	DateTime = NULL,
@ToDate		DateTime = NULL,
@ModelId	INT = NULL,
@MakeId		INT = NULL,
@VersionId	NUMERIC(18,0) = NULL -- Added by Sachin Bharti(10-01-2013)

AS 
BEGIN
		SELECT 
		COUNT(DISTINCT (CASE WHEN  TIL.TC_LeadDispositionId = 4 AND TCN.BookingStatus=32 AND TCN.BookingDate BETWEEN @FromDate AND @ToDate THEN TCN.TC_NewCarInquiriesId END )) AS BookedLead, 
		COUNT(DISTINCT (CASE WHEN  TIL.TC_LeadDispositionId = 4 AND TCN.BookingStatus=32 AND CONVERT(date, TCN.BookingDate) = CONVERT(date, GETDATE()) THEN TCN.TC_NewCarInquiriesId END )) AS TodayBookedLead, 
		COUNT(DISTINCT (CASE WHEN  TCN.CarDeliveryStatus = 77 AND TCN.CarDeliveryDate  BETWEEN @FromDate AND @ToDate THEN TCN.TC_NewCarInquiriesId END )) AS DeliveredLead,
		COUNT(DISTINCT (CASE WHEN  TCN.CarDeliveryStatus = 77 AND CONVERT(DATE, TCN.CarDeliveryDate) = CONVERT(DATE, GETDATE()) THEN TCN.TC_NewCarInquiriesId END )) AS TodayDeliveredLead,
		COUNT(DISTINCT (CASE WHEN TB.Invoicedate  IS NOT NULL AND TB.InvoiceDate BETWEEN @FromDate AND @ToDate THEN TB.TC_NewCarInquiriesId END )) AS RetailLead,
		COUNT(DISTINCT (CASE WHEN TB.Invoicedate  IS NOT NULL AND CONVERT(DATE,TB.InvoiceDate)= CONVERT(DATE,GETDATE())  THEN TB.TC_NewCarInquiriesId END )) AS TodayRetailLead
		FROM TC_NewCarInquiries AS TCN WITH (NOLOCK) 
		INNER JOIN TC_InquiriesLead AS TIL WITH (NOLOCK)  ON TIL.TC_InquiriesLeadId = TCN.TC_InquiriesLeadId
		INNER JOIN vwMMV VM(NOLOCK) ON VM.VersionId = TCN.VersionId AND VM.MakeId = @MakeId
		INNER JOIN Dealers D (NOLOCK) ON D.ID = TIL.BranchId AND D.TC_BrandZoneId IS NOT NULL
		LEFT  JOIN TC_NewCarBooking TB(NOLOCK) ON TB.TC_NewCarInquiriesId = TCN.TC_NewCarInquiriesId 
		WHERE (VM.ModelId = @ModelId OR @ModelId IS NULL)
		AND (D.TC_RMID = @RMId OR @RMId IS NULL)
		AND (D.TC_AMId = @AMId OR @AMId IS NULL)
		AND (VM.VersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(10-01-2013)
END


