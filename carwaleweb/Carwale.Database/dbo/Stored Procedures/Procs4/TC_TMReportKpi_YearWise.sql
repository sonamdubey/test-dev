IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMReportKpi_YearWise]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMReportKpi_YearWise]
GO

	--	Author		:	Vivek Singh(21th November 2013)

--	Description :-To get YearWise KPI report of all the Dealers under the Logged in user(Hierarchy Wise)


CREATE Procedure [dbo].[TC_TMReportKpi_YearWise]
@TempTable TC_TempTableSpclUser READONLY,
--@LoggedInUser NUMERIC(20,0),
@TargetType       INT,
@Year	DATETIME = NULL,
--@MakeId     NUMERIC(18,0),
@ModelId	NUMERIC(18,0) = NULL,
@VersionId	NUMERIC(18,0) = NULL,
@DealerID	NUMERIC(18,0) = NULL,
@SourceId	NUMERIC(18,0) = NULL

AS
BEGIN

	SELECT * FROM @TempTable ORDER BY  ZoneName;


	--Total Target Of the all the target types 

	SELECT	[dbo].[f_TC_TMTargetMapping](SUM(TDT.TARGET),@TargetType)AS LeadTarget,TDT.Month AS NameMonth
	FROM TC_DealersTarget TDT(NOLOCK) 
			INNER JOIN vwMMV AS V  WITH (NOLOCK) ON V.VersionId=TDT.CarVersionId
			INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId = TDT.DealerId AND IsDealer=1
	WHERE (TDT.[Year]=YEAR(@year)) 
			AND (TDT.CarVersionId = @VersionId OR @VersionId IS NULL)
			AND (V.ModelId = @ModelId OR @ModelId IS NULL)
			AND ( TDT.DealerId = @DealerID OR @DealerID IS NULL) 
			AND (TDT.TC_TargetTypeId=4)
	GROUP BY TDT.Month
	ORDER BY NameMonth

	--Retail acheived
	IF @TargetType = 4 
	BEGIN
		SELECT A.NameMonth AS NameMonth ,SUM(A.Achieved) AS Achieved
			 FROM
			 (
				 SELECT  TBS.DealerId AS DealerId,TBS.CarModel AS ModelName,MONTH(TBS.Invoicedate) AS NameMonth,
				COUNT  (DISTINCT  TBS.TC_NewCarInquiriesId ) AS Achieved
			FROM 
				TC_LeadBasedSummary TBS WITH (NOLOCK) 
				INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
				INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	            INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
				 WHERE (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
				AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL) -- Added by Sachin Bharti(26th Sep 2013
				AND YEAR(@year)=YEAR(TBS.Invoicedate)
				AND TBS.Invoicedate  IS NOT NULL 
				AND (TBS.DealerId= @DealerID OR @DealerID IS NULL) 
				AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
				GROUP BY TBS.DealerId,TBS.CarModel,MONTH(TBS.Invoicedate)
			)	AS A
			INNER JOIN Dealers AS D ON D.ID=A.DealerId
			GROUP BY A.NameMonth
			ORDER BY NameMonth
	END


	--Test Drive Acheived
	IF @TargetType = 2 
	 BEGIN
		 SELECT MONTH(TBS.TestDriveDate) AS NameMonth, COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved 
		  FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
			INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
			INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	        INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
		 WHERE TBS.TestDriveStatus = 28 AND (YEAR(TBS.TestDriveDate)=YEAR(@year)) 
			AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
			AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
			AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
			AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
		 GROUP BY MONTH(TBS.TestDriveDate)
		 ORDER BY NameMonth
	END
		--Total Leads Acheived
	IF @TargetType = 1 
	BEGIN
		SELECT MONTH(TBS.CreatedDate) AS NameMonth,
			COUNT(DISTINCT(CASE WHEN  (YEAR(TBS.CreatedDate)=YEAR(@year)) THEN  TBS.TC_LeadId END)) AS Achieved
		FROM 
			TC_LeadBasedSummary TBS WITH (NOLOCK)
			INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
			INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	        INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
		WHERE  (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
		AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
		AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
		AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
		GROUP BY MONTH(TBS.CreatedDate)
		ORDER BY NameMonth
    END

	--Booking Acheived
	IF @TargetType = 3
	BEGIN
		SELECT MONTH(TBS.BookingDate) AS NameMonth,COUNT(DISTINCT TBS.TC_NewCarInquiriesId) AS Achieved
		 FROM TC_LeadBasedSummary TBS WITH (NOLOCK)			
			INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId AND IsDealer=1
			INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	        INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId
		WHERE TBS.BookingStatus=32 AND TBS.TC_LeadDispositionID=4
			AND YEAR(TBS.BookingDate)=YEAR(@year)
			AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
			AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
			AND (TBS.DealerId = @DealerID OR @DealerID IS NULL) 
			AND (TIGS.TC_InquiryGroupSourceId=@SourceId OR @SourceId IS NULL)
		GROUP BY MONTH(TBS.BookingDate)
		ORDER BY NameMonth
	END

END
