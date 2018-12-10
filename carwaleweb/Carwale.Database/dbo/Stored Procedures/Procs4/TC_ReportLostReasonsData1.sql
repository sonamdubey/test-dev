IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportLostReasonsData1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportLostReasonsData1]
GO

	



--	Author		:	Vivek Singh(24th October 2013)
--	Description :-To get Lost reasons Data for a model under the Logged in user against specific reason or brand
--	============================================================

CREATE Procedure [dbo].[TC_ReportLostReasonsData1]
@TempTable TC_TempTableSpclUser READONLY,
@Type		TINYINT,
@DispType	INT,
@FromDate	DateTime,
@ToDate		DateTime,
@MakeId     NUMERIC(18,0),	
@ModelName	VARCHAR(50),
@LostModelName	VARCHAR(50) = NULL,
@LostReason	VARCHAR(500) = NULL

AS
BEGIN
	IF @Type = 1
		BEGIN
			--Reason Wise Data
			SELECT COUNT(DISTINCT TBS.TC_LeadId) AS LeadCount, TLD.SubDispositionName AS LostReason
			FROM TC_LeadBasedSummary TBS WITH (NOLOCK)
					INNER JOIN DEALERS as D WITH (NOLOCK) ON D.ID = TBS.DealerId
					INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
					INNER JOIN TC_LeadSubDisposition TLD ON TBS.InquirySubDispositionId = TLD.TC_LeadSubDispositionId
					INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
			WHERE TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate AND
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
					INNER JOIN @TempTable TSU1  ON TSU1.TC_SpecialUsersId =D.ID AND IsDealer=1
			WHERE TBS.LeadClosedDate BETWEEN @FromDate AND @ToDate AND
				TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID <> 4 AND TBS.CarModel = @ModelName 
				AND TBS.InquirySubDispositionId IN(SELECT TC_LeadSubDispositionId FROM TC_LeadSubDisposition WHERE SubDispositionName = @LostReason)
				AND TBS.InquiryDispositionId = @DispType
			GROUP BY  CMO.Name
		END
END