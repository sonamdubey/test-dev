IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMReportKpi_KeySaleInformation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMReportKpi_KeySaleInformation]
GO

	-- Created By Vivek singh on 25-11-2013 

CREATE Procedure [dbo].[TC_TMReportKpi_KeySaleInformation] 
@TempTable TC_TempTableSpclUser READONLY,
--@LoggedInUser NUMERIC(20,0),
@FromDate	DATETIME = NULL,
@ToDate		DATETIME = NULL,
--@MakeId     NUMERIC(18,0),
@ModelId	NUMERIC(18,0) = NULL,
@VersionId	NUMERIC(18,0) = NULL,
@DealerId	NUMERIC(18,0) = NULL


AS
BEGIN



	
	--TotalLead (Inquiries)
	SELECT 'Inquiries' Inquiries, SUM(Inquiries.LeadCount) AS InquiriesCount
	FROM (
	SELECT 
		(COUNT(DISTINCT(CASE WHEN  TBS.CreatedDate BETWEEN @FromDate AND @ToDate THEN  TBS.TC_LeadId END))) AS LeadCount		
	FROM 
	 TC_LeadBasedSummary TBS WITH (NOLOCK) 
		INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
	WHERE (TBS.DealerId=@DealerId OR @DealerId IS NULL)
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) 
	GROUP BY  TBS.DealerId ) AS Inquiries


	---TD_Completed Data(Test Drive)
	SELECT 'TestDrive' TestDrive, SUM(TD.TD_Completed)  AS TestDriveCount
	FROM 
	(SELECT COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS TD_Completed	  
	FROM  TC_LeadBasedSummary TBS WITH (NOLOCK)
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
	WHERE TBS.TestDriveStatus = 28 AND TBS.TestDriveDate BETWEEN @FromDate AND @ToDate 
		AND (TBS.DealerId=@DealerId OR @DealerId IS NULL)AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) 
	GROUP BY TBS.DealerId) AS TD
		
	--Total Booking Data
	SELECT 'Booking' Booking,  SUM(Booking.BookedLead) AS BookingCount
	FROM(
	SELECT  COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS BookedLead
	FROM  TC_LeadBasedSummary TBS WITH (NOLOCK)
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
	WHERE TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4  
		AND TBS.BookingDate BETWEEN @FromDate AND @ToDate AND (TBS.DealerId=@DealerId OR @DealerId IS NULL)
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) 
	GROUP BY  TBS.DealerId) AS Booking
	
	
	
	-------- Retail
	SELECT 'Retail' Retail, SUM( A.RetailLead) AS RetailCount
	 FROM  
	 (
		 SELECT TBS.DealerId AS DealerId,
		COUNT  (DISTINCT  TBS.TC_NewCarInquiriesId ) AS RetailLead
	FROM 
		 TC_LeadBasedSummary TBS WITH (NOLOCK)
	 INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
		 WHERE (TBS.DealerId=@DealerId OR @DealerId IS NULL) 
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
		AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate
		AND TBS.Invoicedate  IS NOT NULL 
        GROUP BY TBS.DealerId
	)	AS A
	INNER JOIN  Dealers AS D ON D.ID = A.DealerId 	

	--Booking Delivered Data(Total Delivery)
	
	SELECT  'Delivered' Delivered,COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS DeliveredCount	  
	FROM  TC_LeadBasedSummary TBS WITH (NOLOCK)
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
	WHERE TBS.CarDeliveryStatus = 77 AND TBS.CarDeliveryDate BETWEEN @FromDate AND @ToDate AND 
		(TBS.DealerId=@DealerId OR @DealerId IS NULL) AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) 


END
