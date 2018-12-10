IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTPFailedLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTPFailedLeads]
GO

	-- =============================================
-- Author:		Sanjay Soni
-- Create date: 23/06/2016
-- Description:	Get Third Party failed Lead Data
-- Modified BY : Sanjay Soni, on 01/10/2016 added zone id in select query 
-- Modified BY : Vicky Lund, 20/10/2016, added failed reason column
-- exec [dbo].[GetTPFailedLeads]  '01/01/2016','10/10/2016','Failed'
-- =============================================
CREATE PROCEDURE [dbo].[GetTPFailedLeads] @StartDate DATE
	,@EndDate DATE
	,@LeadType VARCHAR(10)
AS
BEGIN
	DECLARE @RequestStartDate DATETIME
		,@RequestEndDate DATETIME

	SET @RequestStartDate = convert(DATETIME, convert(VARCHAR(10), @StartDate, 120) + ' 00:00:00')
	SET @RequestEndDate = convert(DATETIME, convert(VARCHAR(10), @EndDate, 120) + ' 23:59:59');

	SELECT PDL.DealerId AS CampaignDealer
		,TPLQ.IsSuccess AS PushStatus
		,ND.NAME AS Organization
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
		,V.ModelId
		,V.Version
		,V.VersionId
		,TPLQ.CustomerName AS NAME
		,TPLQ.Email
		,TPLQ.Mobile
		,C.NAME AS City
		,C.Id AS CityId
		,PDL.ZoneId
		,CZ.ZoneName AS Zone
		,PDL.AssignedDealerID
		,TPLQ.EntryDate AS RequestDateTime
		,ND.NAME AS AssignedDealerName
		,TPLQ.PQId AS LeadId
		,PDL.PlatformId
		,PDL.PQId
		,TPLQ.PushStatus FailureReason
	FROM ThirdPartyLeadQueue TPLQ WITH (NOLOCK)
	INNER JOIN PQDEALERADLEADS PDL WITH (NOLOCK) ON TPLQ.PQId = PDL.Id
	LEFT JOIN ThirdPartyLeadSettings TPLS WITH (NOLOCK) ON TPLS.ThirdPartyLeadSettingId = TPLQ.ThirdPartyLeadId
		AND TPLS.IsActive = 1
	LEFT JOIN VWMMV V WITH (NOLOCK) ON V.VERSIONID = PDL.VERSIONID
	LEFT JOIN NCS_DEALERS ND WITH (NOLOCK) ON ND.ID = PDL.ASSIGNEDDEALERID
	LEFT JOIN Cities C WITH (NOLOCK) ON C.Id = PDL.CityId
	LEFT JOIN CityZones CZ WITH (NOLOCK) ON CZ.id = PDL.ZoneId
	WHERE (
			(
				@LeadType = 'Failed'
				AND (
					TPLQ.IsSuccess = 0
					OR TPLQ.IsSuccess IS NULL
					)
				)
			OR (@LeadType = 'Regular')
			)
		AND PDL.REQUESTDATETIME BETWEEN @RequestStartDate
			AND @RequestEndDate
		AND PDL.Id NOT IN (
			SELECT PqdealerAdLeadId
			FROM DiscardLeads WITH (NOLOCK)
			)
	ORDER BY PDL.Id DESC
END