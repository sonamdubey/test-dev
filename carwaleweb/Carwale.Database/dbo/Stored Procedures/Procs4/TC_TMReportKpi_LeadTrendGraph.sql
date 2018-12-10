IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMReportKpi_LeadTrendGraph]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMReportKpi_LeadTrendGraph]
GO

	--	Author		:	Ranjeet Kumar 25-Nov-13

---Copy from TC_ReportLeadPerformance1 
--	============================================================

CREATE Procedure [dbo].[TC_TMReportKpi_LeadTrendGraph] 
@TempTable TC_TempTableSpclUser READONLY,
@FromDate	DATETIME = NULL,
@ToDate		DATETIME = NULL,
--@MakeId     NUMERIC(18,0)=NULL,
@ModelId	NUMERIC(18,0) = NULL,
@VersionId	NUMERIC(18,0) = NULL,
@DealerId   NUMERIC(18,0) = NULL

AS
BEGIN

--Day wise lead data
SELECT  DAY(TBS.CreatedDate) AS Day, MONTH(TBS.CreatedDate) AS Month,
	    COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount
FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =TBS.DealerId  AND IsDealer=1
WHERE TBS.CreatedDate BETWEEN @FromDate AND @ToDate  
	AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
	AND (TBS.DealerId =@DealerId OR @DealerId IS NULL)
GROUP BY DAY(TBS.CreatedDate), MONTH(TBS.CreatedDate)

		 		
END
