IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMReportKPI_Salesoverview]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMReportKPI_Salesoverview]
GO

	
--	Author		:	Vivek Singh(27th November 2013)

--	Description :-To get Lead summary report of all the Dealers under the Logged in user(Hierarchy Wise)
--   Copied from procedure [TC_ReportLeadPerformance]
--  Changed on 30/Dec/2013 to add delivery achieved by vivek singh
--  Modified by Vivek Singh on 07-02-2014 removed the column tc_targettypeid as its not needed as all the other targets are calculated on basis of retail target type
--	============================================================


CREATE Procedure [dbo].[TC_TMReportKPI_Salesoverview] 
@TempTable TC_TempTableSpclUser READONLY,
--@LoggedInUser NUMERIC(20,0),
@FromDate	DATETIME = NULL,
@ToDate		DATETIME = NULL,
@ModelId	NUMERIC(18,0) = NULL,
@VersionId	NUMERIC(18,0) = NULL,
@DealerID	NUMERIC(18,0) = NULL,
@SourceId	NUMERIC(18,0) = NULL


AS
BEGIN


	SELECT * FROM @TempTable ORDER BY  ZoneName;



		--Total Target Count(taget type wise) 		 
	SELECT	[dbo].[f_TC_TMTargetMapping](SUM(TDT.TARGET),TT.TC_TargetTypeId)AS LeadTarget,TT.TargetType AS TargetType
	FROM  TC_DealersTarget TDT(NOLOCK) 
			INNER JOIN  vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TDT.CarVersionId
			INNER JOIN  TC_TargetType TT WITH (NOLOCK) ON TT.TC_TargetTypeId IN(SELECT  DISTINCT(TC_TargetTypeId) FROM TC_TargetType)
			INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TDT.DealerId AND IsDealer=1
	WHERE (TDT.[MONTH]>=MONTH(@FromDate) AND TDT.[MONTH]<=MONTH(@ToDate)) 
			AND (TDT.[Year]>=YEAR(@FromDate) AND  TDT.[Year]<=YEAR(@ToDate)) 
				AND (TDT.CarVersionId = @VersionId OR @VersionId IS NULL)
				AND (V.ModelId = @ModelId OR @ModelId IS NULL)
				AND (TDT.TC_TargetTypeId=4)
			AND (TDT.DealerId = @DealerID OR @DealerID IS NULL) 
	GROUP BY TDT.TC_TargetTypeId,TT.TargetType,TT.TC_TargetTypeId
	ORDER BY TDT.TC_TargetTypeId

	
	--TotalLead 
	SELECT 'Total Leads' AS TargetType,
		COUNT(DISTINCT(CASE WHEN  TBS.CreatedDate BETWEEN @FromDate AND @ToDate THEN  TBS.TC_LeadId END)) AS Achieved
	FROM 
		 TC_LeadBasedSummary TBS WITH (NOLOCK)
		 INNER JOIN  vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
		 INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
		 INNER JOIN  TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	INNER JOIN  TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
	WHERE  (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
	AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
	AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
	UNION

	--TD_Completed Data
	SELECT 'TD' AS TargetType, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved
	FROM  TC_LeadBasedSummary TBS WITH (NOLOCK)
		 INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
		 INNER JOIN  TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	INNER JOIN  TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
	WHERE TBS.TestDriveStatus = 28 AND TBS.TestDriveDate BETWEEN @FromDate AND @ToDate 
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
		AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
		AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)

		UNION
	--Total Booking Data
	SELECT 'Bookings' AS TargetType, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved
	FROM  TC_LeadBasedSummary TBS WITH (NOLOCK)
			INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
			INNER JOIN  TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	        INNER JOIN  TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
	WHERE TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4
		AND TBS.BookingDate BETWEEN @FromDate AND @ToDate 
		AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
		AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
		AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
		UNION
	-------- Retail
	SELECT  'Retails' AS TargetType, SUM(RetailLead) AS Achieved
		FROM
		 (
			 SELECT  TBS.DealerId AS DealerId,
			COUNT  (DISTINCT  TBS.TC_NewCarInquiriesId ) AS RetailLead
		FROM 
			 TC_LeadBasedSummary TBS WITH (NOLOCK) 
			 INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
			 INNER JOIN  TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	         INNER JOIN  TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			 WHERE (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
			AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) 
			AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate
			AND TBS.Invoicedate  IS NOT NULL
			AND (TBS.DealerId = @DealerID OR @DealerID IS NULL)  
			AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
			GROUP BY TBS.DealerId
		)	AS A
		INNER JOIN  Dealers AS D ON D.ID=A.DealerId
		INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1

		UNION
     -----Delivery  
	 -----Changed on 30/Dec/2013 to add delivery achieved by vivek singh

	 SELECT 'Delivery' AS TargetType, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Delivered	  
      FROM  TC_LeadBasedSummary TBS WITH (NOLOCK)
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND TSU1.IsDealer=1
    WHERE TBS.CarDeliveryStatus = 77 AND TBS.CarDeliveryDate BETWEEN @FromDate AND @ToDate AND 
		(TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
END
