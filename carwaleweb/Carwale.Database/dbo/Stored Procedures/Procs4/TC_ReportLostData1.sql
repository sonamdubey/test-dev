IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportLostData1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportLostData1]
GO

	


--	Author		:   Vivek Singh(21st October 2013)
--	Description :-	To get the Lost Summary report of all the Dealers under the Logged in user(Hierarchy Wise)

--					--  Copied from procedure [TC_ReportLostData]
--	============================================================

CREATE Procedure [dbo].[TC_ReportLostData1]  
@TempTable TC_TempTableSpclUser READONLY,
@FromDate	DATETIME = NULL,
@ToDate		DATETIME = NULL,
@MakeId     NUMERIC(18,0),	
@ModelId	NUMERIC(18,0) = NULL,
@ModelName	VARCHAR(50) = NULL

AS
BEGIN

SELECT * FROM @TempTable ORDER BY  ZoneName;

	--Complete Data	
	SELECT TBZ.ZoneName,
		D.Organization AS Dealer, D.ID AS DealerId,
		COUNT(DISTINCT TBS.TC_LeadId) AS LostLead,
		ISNULL(TBS.Source, 'NA') AS Source, ISNULL(TBS.Eagerness, 'Not Yet Set') AS Eagerness
		   				  
	FROM DEALERS as D WITH (NOLOCK)
		LEFT JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId AND TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID <> 4 AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate 
		--TC_LeadBasedSummary TBS WITH (NOLOCK)
		--INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
		INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
	WHERE 	--AND TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID <> 4 
			--AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate 
			(TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	GROUP BY TBZ.ZoneName, D.Organization, D.ID, TBS.Source, TBS.Eagerness			
	

	
	--Model Wise Data
	SELECT TBS.CarModel	,COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
			INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
			INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
			INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
	WHERE  TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate AND
		TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID <> 4 AND TBS.CarModel IS NOT NULL AND (TBS.CarModelId = @ModelId OR @ModelId IS NULL)
	GROUP BY TBS.CarModel
	
	--Lost Reasons
	SELECT COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount, TLD.Name AS LostReason, CMO.Name AS LostModel, TSD.SubDispositionName, TBS.InquiryDispositionId
	FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
			INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
			INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
			INNER JOIN TC_LeadDisposition TLD ON TBS.InquiryDispositionId = TLD.TC_LeadDispositionId
			INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
			LEFT JOIN TC_LeadSubDisposition TSD ON TBS.InquirySubDispositionId = TSD.TC_LeadSubDispositionId
			LEFT JOIN CarVersions CV ON TBS.LostVersionId = CV.ID 
			LEFT JOIN CarModels CMO ON CV.CarModelId = CMO.ID
	WHERE TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate AND
		TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID <> 4 AND TBS.CarModel = @ModelName
	GROUP BY  TLD.Name, CMO.Name, TSD.SubDispositionName, TBS.InquiryDispositionId


SELECT MAX(lvl) AS MAXLEVEL,MIN(lvl) AS MINLEVEL FROM @TempTable


		
END
