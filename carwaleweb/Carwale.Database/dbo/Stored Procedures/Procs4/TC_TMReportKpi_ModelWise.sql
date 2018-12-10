IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMReportKpi_ModelWise]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMReportKpi_ModelWise]
GO

	--	Author		:	Vivek Singh(18th November 2013)

--	Description :-To get ModelWise KPI report of all the Dealers under the Logged in user(Hierarchy Wise)
--  Copied from procedure [TC_ReportLeadPerformance1](Target part and Leads)

CREATE Procedure [dbo].[TC_TMReportKpi_ModelWise]
@TempTable TC_TempTableSpclUser READONLY,
--@LoggedInUser NUMERIC(20,0),
@TargetType       INT,
@FromDate	DATETIME = NULL,
@ToDate		DATETIME = NULL,
--@MakeId     NUMERIC(18,0),
@ModelId	NUMERIC(18,0) = NULL,
@VersionId	NUMERIC(18,0) = NULL,
@DealerID	NUMERIC(18,0) = NULL,
@SourceId	NUMERIC(18,0) = NULL

AS
BEGIN


	SELECT * FROM @TempTable ORDER BY  ZoneName;
	--Total Target Of the all the target types (WHEN NSC user Is logged in
		SELECT	[dbo].[f_TC_TMTargetMapping](SUM(TDT.TARGET),@TargetType)AS LeadTarget,V.Model AS ModelName,V.Version AS Version
		FROM TC_DealersTarget TDT(NOLOCK) 
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TDT.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TDT.DealerId AND IsDealer=1
		WHERE (TDT.[MONTH]>=MONTH(@FromDate) AND TDT.[MONTH]<=MONTH(@ToDate)) 
		      AND (TDT.[Year]>=YEAR(@FromDate) AND  TDT.[Year]<=YEAR(@ToDate)) 
			  AND (TDT.CarVersionId = @VersionId OR @VersionId IS NULL)
			  AND (V.ModelId = @ModelId OR @ModelId IS NULL)
		   	  AND (TDT.TC_TargetTypeId=4)
			  AND (TDT.DealerId = @DealerID OR @DealerID IS NULL) 
		
		GROUP BY V.Model,v.Version

		--Retail acheived
		IF @TargetType = 4 
		BEGIN
			SELECT Achieved AS Achieved,A.ModelName,A.Version
				 FROM
				 (
					 SELECT TSU1.TC_SpecialUsersId AS DealerId,TBS.CarModel AS ModelName,V.Version AS Version,
					COUNT  (DISTINCT  TBS.TC_NewCarInquiriesId ) AS Achieved
				FROM 
					TC_LeadBasedSummary TBS WITH (NOLOCK)
					INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
					INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
					INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	                INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
					 WHERE (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
					AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
					AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate
					AND TBS.Invoicedate  IS NOT NULL 
					AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
					GROUP BY TSU1.TC_SpecialUsersId,TBS.CarModel,V.Version
				)	AS A
				INNER JOIN Dealers AS D ON D.ID=A.DealerId
		END


		--Test Drive Acheived
		IF @TargetType = 2 
		 BEGIN
			 SELECT TBS.CarModel AS ModelName , COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved,V.Version AS Version
			  FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			 WHERE TBS.TestDriveStatus = 28 AND TBS.TestDriveDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
				AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
				AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
				AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
			 GROUP BY TBS.CarModel,V.Version
		END
			--Total Leads Acheived
		IF @TargetType = 1 
		BEGIN
			SELECT TBS.CarModel AS ModelName,V.Version AS Version,
				COUNT(DISTINCT(CASE WHEN  TBS.CreatedDate BETWEEN @FromDate AND @ToDate THEN  TBS.TC_LeadId END)) AS Achieved
			FROM 
                TC_LeadBasedSummary TBS WITH (NOLOCK) 
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			WHERE  (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
			AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
			AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
			AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
			GROUP BY TBS.CarModel,V.Version
		END

		--Booking Acheived
		IF @TargetType = 3
		BEGIN
			SELECT TBS.CarModel AS ModelName, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved,V.Version AS Version
			 FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
                INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
			WHERE TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4
				AND TBS.BookingDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
				AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
				AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
				AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
			GROUP BY  TBS.CarModel,V.Version
		END


		IF @TargetType = 5
		BEGIN
		SELECT TBS.CarModel AS ModelName, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved,V.Version AS Version
         FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
	        INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
                INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
          WHERE TBS.CarDeliveryStatus = 77 AND TBS.CarDeliveryDate BETWEEN @FromDate AND @ToDate AND 
		  (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		  AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
          GROUP BY TBS.CarModel,V.Version
		END
		
	END
