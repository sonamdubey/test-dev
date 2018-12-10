IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFailedLeads_v16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFailedLeads_v16_11_1]
GO

	-- =============================================
-- Author:		Sanjay Soni
-- Create date: 23/06/2016
-- Description:	Get failed Lead Data
-- Modified BY : Sanjay Soni, on 01/10/2016 added zone id in select query 
-- Modified BY : Vicky Lund, 17/10/2016, added failed reason column
-- Modified BY : Chetan Thambad, 03/11/2016, Fetching Leads based on Mobile number if available
-- EXEC [GetFailedLeads_v16_11_1] '','','9029743272','Failed'
-- =============================================
CREATE PROCEDURE [dbo].[GetFailedLeads_v16_11_1] @StartDate DATE
	,@EndDate DATE
	,@Mobile VARCHAR(100)
	,@LeadType VARCHAR(10)
AS
BEGIN
	DECLARE @RequestStartDate DATETIME
		,@RequestEndDate DATETIME

	SET @RequestStartDate = convert(DATETIME, convert(VARCHAR(10), @StartDate, 120) + ' 00:00:00')
	SET @RequestEndDate = convert(DATETIME, convert(VARCHAR(10), @EndDate, 120) + ' 23:59:59');

	SELECT PDS.DealerId AS CampaignDealer
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
		,V.[Version]
		,PDL.NAME
		,PDL.Email
		,PDL.Mobile
		,C.NAME AS City
		,C.Id AS CityId
		,PDL.ZoneId
		,CZ.ZoneName AS Zone
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
						ELSE 'Failure from autobiz'
						END
			ELSE '---'
			END FailureReason
	FROM PQDealerAdLeads PDL WITH (NOLOCK)
	INNER JOIN PQ_DealerSponsored PDS WITH (NOLOCK) ON PDL.CAMPAIGNID = PDS.ID
		AND PDL.DealerLeadBusinessType = 0
	LEFT OUTER JOIN PQDealerFailedLeads PQDFL WITH (NOLOCK) ON PQDFL.PqDealerLeadId = PDL.Id
	LEFT OUTER JOIN PQDealerFailedReason PQDFR WITH (NOLOCK) ON PQDFL.Reason = PQDFR.Id
	LEFT JOIN Dealers D WITH (NOLOCK) ON D.ID = PDS.DEALERID
	LEFT JOIN VWMMV V WITH (NOLOCK) ON V.VERSIONID = PDL.VERSIONID
	LEFT JOIN NCS_DEALERS ND WITH (NOLOCK) ON ND.ID = PDL.ASSIGNEDDEALERID
	LEFT JOIN Cities C WITH (NOLOCK) ON C.Id = PDL.CityId
	LEFT JOIN CityZones CZ WITH (NOLOCK) ON CZ.id = PDL.ZoneId
	WHERE (
			(
				@LeadType = 'Failed'
				AND (
					PDL.PushStatus IS NULL
					OR PDL.PushStatus = - 1
					)
				)
			OR (@LeadType = 'Regular')
			)
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
