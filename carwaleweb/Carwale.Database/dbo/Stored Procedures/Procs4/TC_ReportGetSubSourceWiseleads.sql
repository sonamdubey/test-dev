IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportGetSubSourceWiseleads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportGetSubSourceWiseleads]
GO

	
--	Author		:	Vivek Singh(9th October 2013)

--	Description :-To get Sub-Source Wise Leads under the Logged in user In a particular source group
--	===============================================================================================

CREATE Procedure [dbo].[TC_ReportGetSubSourceWiseleads] 
@TempTable TC_TempTableSpclUser READONLY,
@FromDate	DATETIME = NULL,
@ToDate		DATETIME = NULL,	
@MakeId     NUMERIC(18,0),
@ModelId	NUMERIC(18,0) = NULL,
@VersionId	NUMERIC(18,0) = NULL,
@GroupSourceName VARCHAR(50)

AS
BEGIN

SELECT COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount, ISNULL(TBS.Source, 'NA') AS Source  
FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
	INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
	INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
	INNER JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id=TBS.SourceId  
	INNER JOIN TC_InquiryGroupSource TIGS WITH (NOLOCK) ON TIGS.TC_InquiryGroupSourceId=TIS.TC_InquiryGroupSourceId AND TIGS.GroupSourceName=@GroupSourceName
WHERE TBS.CreatedDate BETWEEN @FromDate AND @ToDate 
	AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	AND (TBS.CarVersionId = @VersionId OR @VersionId IS NULL)
GROUP BY TBS.Source, TBS.Eagerness,TIGS.GroupSourceName  


END