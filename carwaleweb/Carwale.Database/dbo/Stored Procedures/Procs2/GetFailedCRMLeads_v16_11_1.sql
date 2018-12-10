IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFailedCRMLeads_v16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFailedCRMLeads_v16_11_1]
GO

	-- =============================================
-- Author:		Sanjay Soni
-- Create date: 23/06/2016
-- Description:	Get failed CRM Lead Data Which not present in Tp lead table
-- Modified: Vicky Lund, 26-10-2016, Removed LeadType. Added Failure reason column
-- Modified BY : Chetan Thambad, 03/11/2016, Fetching Leads based on Mobile number if available
-- exec [dbo].[GetFailedCRMLeads_v16_11_1]  '10/26/2016','10/26/2016', ''
-- =============================================
CREATE PROCEDURE [dbo].[GetFailedCRMLeads_v16_11_1] @StartDate DATE
	,@EndDate DATE
	,@Mobile VARCHAR(100)
AS
BEGIN
	DECLARE @RequestStartDate DATETIME
		,@RequestEndDate DATETIME

	SET @RequestStartDate = convert(DATETIME, convert(VARCHAR(10), @StartDate, 120) + ' 00:00:00')
	SET @RequestEndDate = convert(DATETIME, convert(VARCHAR(10), @EndDate, 120) + ' 23:59:59');

	SELECT DISTINCT PDS.DealerId AS CampaignDealer
		,PDL.PushStatus
		,D.Organization
		,PDL.CampaignId
		,PDL.LeadClickSource
		,CampaignType = CASE 
			WHEN PDL.DealerLeadBusinessType = 0
				THEN 'AUTOBIZ'
			ELSE 'CRM'
			END
		,V.Make
		,V.MakeId
		,V.Model
		,V.Version
		,PDL.NAME
		,PDL.Email
		,PDL.Mobile
		,C.NAME AS City
		,C.Id AS CityId
		,CZ.ZoneName AS Zone
		,PDL.ZoneId
		,PDL.AssignedDealerID
		,PDL.RequestDateTime
		,ND.NAME AS AssignedDealerName
		,PDL.Id AS LeadId
		,V.ModelId
		,V.VersionId
		,PDL.PlatformId
		,PDL.PQId
		,CASE 
			WHEN PDL.PushStatus IS NULL
				OR PDL.PushStatus = - 1
				THEN CASE 
						WHEN PQDFR.Reason IS NOT NULL
							THEN PQDFR.Reason
						ELSE 'Campaign not running'
						END
			ELSE '---'
			END FailureReason
	FROM PQDEALERADLEADS PDL WITH (NOLOCK)
	LEFT OUTER JOIN PQDealerFailedLeads PQDFL WITH (NOLOCK) ON PQDFL.PqDealerLeadId = PDL.Id
	LEFT OUTER JOIN PQDealerFailedReason PQDFR WITH (NOLOCK) ON PQDFL.Reason = PQDFR.Id
	LEFT OUTER JOIN ThirdPartyLeadQueue TPLQ WITH (NOLOCK) ON PDL.Id = TPLQ.PQId
	INNER JOIN PQ_DEALERSPONSORED PDS WITH (NOLOCK) ON PDL.CAMPAIGNID = PDS.ID
		AND PDL.DealerLeadBusinessType = 1
	LEFT JOIN DEALERS D WITH (NOLOCK) ON D.ID = PDS.DEALERID
	LEFT JOIN VWMMV V WITH (NOLOCK) ON V.VERSIONID = PDL.VERSIONID
	INNER JOIN ThirdPartyLeadSettings TPLS WITH (NOLOCK) ON TPLS.ModelId = V.ModelId
		AND TPLS.IsActive = 1
	LEFT JOIN NCS_DEALERS ND WITH (NOLOCK) ON ND.ID = PDL.ASSIGNEDDEALERID
	LEFT JOIN Cities C WITH (NOLOCK) ON C.Id = PDL.CityId
	LEFT JOIN CityZones CZ WITH (NOLOCK) ON CZ.id = PDL.ZoneId
	WHERE TPLQ.PQId IS NULL
		AND PDL.Id NOT IN (
			SELECT PqdealerAdLeadId
			FROM DiscardLeads WITH (NOLOCK)
			)
			AND (
			(
				@StartDate != ''
				AND @EndDate != ''
				AND (
					PDL.Mobile = @Mobile
					AND PDL.Mobile != ''
					AND PDL.Mobile IS NOT NULL
					AND PDL.REQUESTDATETIME BETWEEN @RequestStartDate
						AND @RequestEndDate
					)
				) -- yes yes yes
			OR (
				@StartDate = ''
				AND @EndDate = ''
				AND (
					PDL.Mobile = @Mobile
					AND @Mobile != ''
					)
				) -- no no yes
			OR (
				@StartDate != ''
				AND @EndDate != ''
				AND PDL.REQUESTDATETIME BETWEEN @RequestStartDate
					AND @RequestEndDate
				AND (@Mobile = '')
				) -- yes yes no
			)
	ORDER BY PDL.Id DESC
END

