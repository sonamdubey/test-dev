IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportLostData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportLostData]
GO

	


--	Author		:	Deepak Tripathi(25th July 2013)
--	Modified	:	1.Sachin Bharti(25th July 2013) 
--	Modified	:	2.Sachin Bharti(25th July 2013) 
--					Purpose : Add modelid constraint for filter the data
--	============================================================

CREATE Procedure [dbo].[TC_ReportLostData]  
@FromDate	DateTime = NULL,
@ToDate		DateTime = NULL,
@MakeId		NUMERIC(18,0),
@ModelName	VARCHAR(50) = NULL,
@ModelId	NUMERIC(18,0) = NULL,
@RMId		NUMERIC(18,0) = NULL,
@AMId		NUMERIC(18,0) = NULL

AS
BEGIN
	
	IF ISNULL(@RMId,0) <= 0
		SET @RMId = NULL
		
	IF ISNULL(@AMId,0) <= 0
		SET @AMId = NULL
		
	--Complete Data	
	SELECT TBZ.ZoneName, TSU.UserName AS RM, TSU.TC_SpecialUsersId AS RMId,
		 TSU1.UserName AS AM,  TSU1.TC_SpecialUsersId AS AMId,
		D.Organization AS Dealer, D.ID AS DealerId,
		COUNT(DISTINCT TBS.TC_LeadId) AS LostLead,
		ISNULL(TBS.Source, 'NA') AS Source, ISNULL(TBS.Eagerness, 'Not Yet Set') AS Eagerness
		   				  
	FROM DEALERS as D WITH (NOLOCK)
		LEFT JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId AND TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID <> 4 AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate 
		--TC_LeadBasedSummary TBS WITH (NOLOCK)
		--INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		INNER JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
		INNER JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
	WHERE  (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL)
			--AND TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID <> 4 
			--AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate 
			AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	GROUP BY TBZ.ZoneName, TSU.UserName, TSU1.UserName, D.Organization, TSU.TC_SpecialUsersId, TSU1.TC_SpecialUsersId, D.ID, TBS.Source, TBS.Eagerness			
	

	
	--Model Wise Data
	SELECT TBS.CarModel	,COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
			INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
			INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
	WHERE (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate AND
		TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID <> 4 AND TBS.CarModel IS NOT NULL AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	GROUP BY TBS.CarModel
	
	--Lost Reasons
	SELECT COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount, TLD.Name AS LostReason, CMO.Name AS LostModel, TSD.SubDispositionName, TBS.InquiryDispositionId
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
			INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
			INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
			INNER JOIN TC_LeadDisposition TLD ON TBS.InquiryDispositionId = TLD.TC_LeadDispositionId
			LEFT JOIN TC_LeadSubDisposition TSD ON TBS.InquirySubDispositionId = TSD.TC_LeadSubDispositionId
			LEFT JOIN CarVersions CV ON TBS.LostVersionId = CV.ID 
			LEFT JOIN CarModels CMO ON CV.CarModelId = CMO.ID
	WHERE (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate AND
		TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID <> 4 AND TBS.CarModel = @ModelName
	GROUP BY  TLD.Name, CMO.Name, TSD.SubDispositionName, TBS.InquiryDispositionId
			
END







