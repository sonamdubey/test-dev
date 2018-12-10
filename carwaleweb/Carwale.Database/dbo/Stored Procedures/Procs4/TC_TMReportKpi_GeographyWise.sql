IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMReportKpi_GeographyWise]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMReportKpi_GeographyWise]
GO

	--	Author		:	Vivek Singh(18th November 2013)

--	Description :-To get GeographyWise KPI report of all the Dealers under the Logged in user(Hierarchy Wise)
--  Copied from procedure [TC_ReportLeadPerformance1](Target part and Leads)

CREATE Procedure [dbo].[TC_TMReportKpi_GeographyWise]
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


	DECLARE @MinLevel NUMERIC(18,0);
	DECLARE @MaxLevel NUMERIC(18,0);



	SELECT  @MaxLevel=MAX(lvl),@MinLevel=MIN(lvl) FROM @TempTable;
	SELECT * FROM @TempTable ORDER BY  ZoneName;


	--Total Target Of the all the target types (WHEN NSC user Is logged in)
	IF @MinLevel=3
	BEGIN
		SELECT	[dbo].[f_TC_TMTargetMapping](SUM(TDT.TARGET),@TargetType)AS LeadTarget ,TSU1.ZoneName AS ZoneName
		FROM TC_DealersTarget TDT(NOLOCK) 
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TDT.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TDT.DealerId AND IsDealer=1
		WHERE (TDT.[MONTH]>=MONTH(@FromDate) AND TDT.[MONTH]<=MONTH(@ToDate)) 
		AND (TDT.[Year]>=YEAR(@FromDate) AND  TDT.[Year]<=YEAR(@ToDate)) 
					AND (TDT.CarVersionId = @VersionId OR @VersionId IS NULL)
					AND (V.ModelId = @ModelId OR @ModelId IS NULL)
					AND (TDT.TC_TargetTypeId=4)
					AND (TDT.DealerId = @DealerID OR @DealerID IS NULL) 
		
		GROUP BY TSU1.ZoneName

		--Retail acheived
		IF @TargetType = 4 
		BEGIN
			SELECT SUM(Achieved) AS Achieved,A.ZoneName
				 FROM
				 (
					 SELECT  TSU1.ZoneName,TBS.DealerId AS DealerId,
					COUNT  (DISTINCT  TBS.TC_NewCarInquiriesId ) AS Achieved
				FROM 
					TC_LeadBasedSummary TBS WITH (NOLOCK) 
					INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
					INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId  AND IsDealer=1
					INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	                INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
					 WHERE (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
					AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate
					AND TBS.Invoicedate  IS NOT NULL 
					AND (TBS.DealerId  = @DealerID OR @DealerID IS NULL) 
					AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
					GROUP BY TSU1.ZoneName,TBS.DealerId
				)	AS A
				INNER JOIN Dealers AS D ON D.ID=A.DealerId
				 GROUP BY A.ZoneName
		END


		--Test Drive Acheived
		IF @TargetType = 2 
		 BEGIN
			 SELECT TSU1.ZoneName AS ZoneName , COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved
			  FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			 WHERE TBS.TestDriveStatus = 28 AND TBS.TestDriveDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
				AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
				AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
				AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
			 GROUP BY TSU1.ZoneName
		END
			--Total Leads Acheived
		IF @TargetType = 1 
		BEGIN
			SELECT TSU1.ZoneName AS ZoneName,
				COUNT(DISTINCT(CASE WHEN  TBS.CreatedDate BETWEEN @FromDate AND @ToDate THEN  TBS.TC_LeadId END)) AS Achieved
			FROM 
				TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			WHERE  (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
			AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
			AND (TBS.DealerId = @DealerID OR @DealerID IS NULL)
			AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL) 
			GROUP BY TSU1.ZoneName
		END

		--Booking Acheived
		IF @TargetType = 3
		BEGIN
			SELECT TSU1.ZoneName AS ZoneName, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved
			 FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			WHERE TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4
				AND TBS.BookingDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
				AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
				AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
				AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
			GROUP BY  TSU1.ZoneName
		END
	END


	IF @MinLevel=4   --(WHEN Regional Manager target Is fetched)
	BEGIN
		SELECT	[dbo].[f_TC_TMTargetMapping](SUM(TDT.TARGET),@TargetType)AS LeadTarget,TSU2.UserName AS ZoneName
		FROM TC_DealersTarget TDT(NOLOCK) 
				INNER JOIN Dealers AS D WITH (NOLOCK) ON TDT.DealerId=D.Id
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TDT.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
				INNER JOIN @TempTable TSU2  ON TSU2.TC_SpecialUsersId=D.TC_AMId
		WHERE (TDT.[MONTH]>=MONTH(@FromDate) AND TDT.[MONTH]<=MONTH(@ToDate)) 
		      AND (TDT.[Year]>=YEAR(@FromDate) AND  TDT.[Year]<=YEAR(@ToDate)) 
			  AND (TDT.CarVersionId = @VersionId OR @VersionId IS NULL)
			  AND (V.ModelId = @ModelId OR @ModelId IS NULL)
			 AND (TDT.TC_TargetTypeId=4)
			 AND (D.ID = @DealerID OR @DealerID IS NULL) 
		
		GROUP BY TSU2.UserName

		--Retail acheived
		IF @TargetType = 4 
		BEGIN
			SELECT SUM(Achieved) AS Achieved,A.UserName AS ZoneName
				 FROM
				 (
					 SELECT  TSU2.UserName AS UserName,D.Id AS DealerId,
					COUNT  (DISTINCT  TBS.TC_NewCarInquiriesId ) AS Achieved
				FROM 
					DEALERS as D WITH (NOLOCK)
					INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId  --AND TBS.CreatedDate  BETWEEN @FromDate AND @ToDate
					INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
					INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
					INNER JOIN @TempTable TSU2  ON TSU2.TC_SpecialUsersId=D.TC_AMId
					INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	                INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
					 WHERE (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
					AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate
					AND TBS.Invoicedate  IS NOT NULL 
					AND (D.ID = @DealerID OR @DealerID IS NULL) 
					AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
					GROUP BY TSU2.UserName,D.ID
				)	AS A
			INNER JOIN Dealers AS D ON D.ID=A.DealerId
			GROUP BY A.UserName
		END


		--Test Drive Acheived
		IF @TargetType = 2 
		 BEGIN
			 SELECT TSU2.UserName AS ZoneName, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved
			  FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId AND D.IsDealerActive= 1
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
				INNER JOIN @TempTable TSU2  ON TSU2.TC_SpecialUsersId=D.TC_AMId
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			 WHERE TBS.TestDriveStatus = 28 AND TBS.TestDriveDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
				AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
				AND (D.ID = @DealerID OR @DealerID IS NULL) 
				AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
			 GROUP BY TSU2.UserName
		END
			--Total Leads Acheived
		IF @TargetType = 1 
		BEGIN
			SELECT TSU2.UserName AS ZoneName,
				COUNT(DISTINCT(CASE WHEN  TBS.CreatedDate BETWEEN @FromDate AND @ToDate THEN  TBS.TC_LeadId END)) AS Achieved
			FROM 
				DEALERS AS D WITH (NOLOCK)
				INNER JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId  AND  D.IsDealerActive= 1
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
				INNER JOIN @TempTable TSU2  ON TSU2.TC_SpecialUsersId=D.TC_AMId
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			WHERE  (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
			AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
			AND (D.ID = @DealerID OR @DealerID IS NULL) 
			AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
			GROUP BY TSU2.UserName
		END

		--Booking Acheived
		IF @TargetType = 3
		BEGIN
			SELECT TSU2.UserName AS ZoneName,COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved
			 FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
				INNER JOIN @TempTable TSU2  ON TSU2.TC_SpecialUsersId=D.TC_AMId
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			WHERE TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4
				AND TBS.BookingDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
				AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
				AND (D.ID = @DealerID OR @DealerID IS NULL) 
				AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
			GROUP BY TSU2.UserName
		END
	END

	--When target of Area Manager is fetched 
   IF @MinLevel=5   
	BEGIN
		SELECT	[dbo].[f_TC_TMTargetMapping](SUM(TDT.TARGET),@TargetType)AS LeadTarget,TSU1.UserName AS ZoneName
		FROM TC_DealersTarget TDT(NOLOCK) 
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TDT.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TDT.DealerId AND IsDealer=1
		WHERE (TDT.[MONTH]>=MONTH(@FromDate) AND TDT.[MONTH]<=MONTH(@ToDate)) 
		      AND (TDT.[Year]>=YEAR(@FromDate) AND  TDT.[Year]<=YEAR(@ToDate)) 
			  AND (TDT.CarVersionId = @VersionId OR @VersionId IS NULL)
			  AND (V.ModelId = @ModelId OR @ModelId IS NULL)
			  AND (TDT.TC_TargetTypeId=4)
			  AND (TDT.DealerId = @DealerID OR @DealerID IS NULL) 
		
		GROUP BY TSU1.UserName

		--Retail acheived
		IF @TargetType = 4 
		BEGIN
			SELECT A.UserName AS ZoneName, SUM(Achieved) AS Achieved
				 FROM
				 (
					 SELECT  TSU1.UserName, TBS.DealerId AS DealerId,
					COUNT  (DISTINCT  TBS.TC_NewCarInquiriesId ) AS Achieved
				FROM 
					TC_LeadBasedSummary TBS WITH (NOLOCK)
					INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
					INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
					INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	                INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
					 WHERE (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
					AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
					AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate
					AND TBS.Invoicedate  IS NOT NULL 
					AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
					AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
					GROUP BY TBS.DealerId,TSU1.UserName
				)	AS A
			INNER JOIN Dealers AS D ON D.ID=A.DealerId
			GROUP BY A.USERNAME
		END


		--Test Drive Acheived
		IF @TargetType = 2 
		 BEGIN
			 SELECT TSU1.UserName AS ZoneName,COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved
			  FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			 WHERE TBS.TestDriveStatus = 28 AND TBS.TestDriveDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
				AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
				AND (TBS.DealerId = @DealerID OR @DealerID IS NULL)
				AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL) 
			 GROUP BY TSU1.UserName
		END
			--Total Leads Acheived
		IF @TargetType = 1 
		BEGIN
			SELECT   TSU1.UserName AS ZoneName,
				COUNT(DISTINCT(CASE WHEN  TBS.CreatedDate BETWEEN @FromDate AND @ToDate THEN  TBS.TC_LeadId END)) AS Achieved
			FROM 
				TC_LeadBasedSummary TBS WITH (NOLOCK)
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			WHERE  (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
			AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
			AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
			AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
			GROUP BY TSU1.UserName
		END

		--Booking Acheived
		IF @TargetType = 3
		BEGIN
			SELECT  TSU1.UserName AS ZoneName,COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved
			 FROM TC_LeadBasedSummary TBS 
				INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TBS.CarVersionId
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
			WHERE TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4
				AND TBS.BookingDate BETWEEN @FromDate AND @ToDate 
				AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
				AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
				AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
				AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
			GROUP BY   TSU1.UserName
		END
	END

END
