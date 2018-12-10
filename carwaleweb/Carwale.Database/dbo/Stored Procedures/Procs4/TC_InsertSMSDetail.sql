IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertSMSDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertSMSDetail]
GO

	

-- Created By	: Sachin Bharti(23rd Sep 2013)
-- Purpose		: To insert all the SMS data regarding leads count for today 
--				  and month till date in TC_SMSDetail table of Special Users

CREATE PROCEDURE [dbo].[TC_InsertSMSDetail] 
AS
BEGIN

 TRUNCATE TABLE TC_SMSDetail;

	----Total Inquiries
	INSERT INTO TC_SMSDetail  (DealerId,
							   CurrentDayCount,
							   CurrentMonthCount,
							   TC_TargetTypeId)
	SELECT DealerId,
	COUNT  (DISTINCT (CASE WHEN CONVERT(DATE,TLS.CreatedDate)=CONVERT(DATE,GETDATE()) THEN   TLS.TC_LeadId END) ) AS CurrentDayCount,
	COUNT(DISTINCT TLS.TC_LeadId) CurrentMonthCount,
	1
	FROM TC_LeadBasedSummary AS TLS(NOLOCK)
	WHERE MONTH(TLS.CreatedDate)= MONTH (GETDATE())
	AND YEAR (TLS.CreatedDate)= YEAR(GETDATE())
	GROUP BY TLS.DealerId

	----Total TestDrive
	INSERT INTO TC_SMSDetail  (DealerId,
							   CurrentDayCount,
							   CurrentMonthCount,
							   TC_TargetTypeId)
	SELECT TLS.DealerId,
	COUNT(CASE WHEN CONVERT(DATE,TLS.TestDriveDate)=CONVERT(DATE,GETDATE()) THEN   TLS.TC_NewCarInquiriesId END ) AS CurrentDayCount,
	COUNT(TLS.TC_NewCarInquiriesId) CurrentMonthCount,
	2
	FROM TC_LeadBasedSummary AS TLS(NOLOCK)
	WHERE MONTH(TLS.TestDriveDate)= MONTH (GETDATE())AND YEAR (TLS.TestDriveDate)= YEAR(GETDATE())
	AND TLS.TestDriveStatus = 28 
	GROUP BY TLS.DealerId

	----Total Bookings
	INSERT INTO TC_SMSDetail  (DealerId,
							   CurrentDayCount,
							   CurrentMonthCount,
							   TC_TargetTypeId)
	SELECT TLS.DealerId,
	COUNT(CASE WHEN CONVERT(DATE,TLS.BookingDate)=CONVERT(DATE,GETDATE()) THEN   TLS.TC_NewCarInquiriesId END ) AS CurrentDayCount,
	COUNT(TLS.TC_NewCarInquiriesId) CurrentMonthCount,
	3
	FROM TC_LeadBasedSummary AS TLS(NOLOCK)
	WHERE MONTH(TLS.BookingDate)= MONTH (GETDATE())AND YEAR (TLS.BookingDate)= YEAR(GETDATE())
	AND TLS.BookingStatus=32 AND TLS.TC_LeadDispositionID=4
	GROUP BY TLS.DealerId

	----Total Retails
	INSERT INTO TC_SMSDetail  (DealerId,
							   CurrentDayCount,
							   CurrentMonthCount,
							   TC_TargetTypeId)
	SELECT TLS.DealerId,
	COUNT(CASE WHEN TLS.InvoiceDate IS NOT NULL AND CONVERT(DATE,TLS.InvoiceDate)=CONVERT(DATE,GETDATE()) THEN   TLS.TC_NewCarInquiriesId END ) AS CurrentDayCount,
	COUNT(TLS.TC_NewCarInquiriesId) CurrentMonthCount,
	4
	FROM TC_LeadBasedSummary AS TLS(NOLOCK)
	WHERE TLS.InvoiceDate IS NOT NULL AND
	MONTH(TLS.InvoiceDate)= MONTH (GETDATE())AND YEAR (TLS.InvoiceDate)= YEAR(GETDATE())
	GROUP BY TLS.DealerId

	----Total Delivery
	INSERT INTO TC_SMSDetail  (DealerId,
							   CurrentDayCount,
							   CurrentMonthCount,
							   TC_TargetTypeId)
	SELECT TLS.DealerId,
	COUNT(CASE WHEN CONVERT(DATE,TLS.CarDeliveryDate)=CONVERT(DATE,GETDATE()) THEN   TLS.TC_NewCarInquiriesId END ) AS CurrentDayCount,
	COUNT(TLS.TC_NewCarInquiriesId) CurrentMonthCount,
	5
	FROM TC_LeadBasedSummary AS TLS(NOLOCK)
	WHERE MONTH(TLS.CarDeliveryDate)= MONTH (GETDATE())AND YEAR (TLS.CarDeliveryDate)= YEAR(GETDATE())
	AND TLS.CarDeliveryStatus = 77
	GROUP BY TLS.DealerId

END

