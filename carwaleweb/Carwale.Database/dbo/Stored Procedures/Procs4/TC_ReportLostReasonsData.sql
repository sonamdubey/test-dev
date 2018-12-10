IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportLostReasonsData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportLostReasonsData]
GO

	



--	Author		:	Deepak Tripathi(25th July 2013)
--	Modified	:	Sachin Bharti(25th July 2013) 
--	============================================================

CREATE Procedure [dbo].[TC_ReportLostReasonsData]
@Type		TINYINT,
@DispType	INT,
@FromDate	DateTime,
@ToDate		DateTime,
@MakeId		NUMERIC(18,0),
@ModelName	VARCHAR(50),
@LostModelName	VARCHAR(50) = NULL,
@LostReason	VARCHAR(500) = NULL,
@RMId		NUMERIC(18,0) = NULL,
@AMId		NUMERIC(18,0) = NULL

AS
BEGIN
	
	IF ISNULL(@RMId,0) <= 0
		SET @RMId = NULL
		
	IF ISNULL(@AMId,0) <= 0
		SET @AMId = NULL
		
	IF @Type = 1
		BEGIN
			--Reason Wise Data
			SELECT COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount, TLD.SubDispositionName AS LostReason
			FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
					INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
					INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
					INNER JOIN TC_LeadSubDisposition TLD ON TBS.InquirySubDispositionId = TLD.TC_LeadSubDispositionId
			WHERE (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate AND
				TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID <> 4 AND TBS.CarModel = @ModelName 
				AND TBS.LostVersionId IN(SELECT ID FROM CarVersions WHERE CarModelId IN(SELECT ID FROM CarModels WHERE Name = @LostModelName))
				AND TBS.InquiryDispositionId = @DispType
			GROUP BY  TLD.SubDispositionName
		END
	ELSE IF @Type = 2
		BEGIN
			--Model Wise Data
			SELECT COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount, CMO.Name AS LostModels
			FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
					INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
					INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
					INNER JOIN CarVersions CV ON TBS.LostVersionId = CV.ID
					INNER JOIN CarModels CMO ON CV.CarModelId = CMO.ID
			WHERE (D.TC_RMID = @RMId OR @RMId IS NULL) AND (D.TC_AMId = @AMId OR @AMId IS NULL) AND TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate AND
				TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID <> 4 AND TBS.CarModel = @ModelName 
				AND TBS.InquirySubDispositionId IN(SELECT TC_LeadSubDispositionId FROM TC_LeadSubDisposition WHERE SubDispositionName = @LostReason)
				AND TBS.InquiryDispositionId = @DispType
			GROUP BY  CMO.Name
		END
END








