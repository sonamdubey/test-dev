IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFailedCRMLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFailedCRMLeads]
GO

	
-- =============================================
-- Author:		Sanjay Soni
-- Create date: 23/06/2016
-- Description:	Get failed CRM Lead Data Which not present in Tp lead table
-- exec [dbo].[GetFailedCRMLeads]  '08/29/2016','10/29/2016','Regular'
-- =============================================
CREATE PROCEDURE [dbo].[GetFailedCRMLeads] 
     @StartDate DATE
	,@EndDate DATE
	,@LeadType varchar(10)
AS
BEGIN
    DECLARE @RequestStartDate datetime , @RequestEndDate datetime 

    set @RequestStartDate = convert(datetime,convert(varchar(10),@StartDate,120)+ ' 00:00:00')	
	set @RequestEndDate = convert(datetime,convert(varchar(10),@EndDate,120)+ ' 23:59:59');

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
		,PDL.Name
		,PDL.Email
		,PDL.Mobile
		,C.Name AS City
		,C.Id AS CityId
		,CZ.ZoneName AS Zone
		,PDL.ZoneId
		,PDL.AssignedDealerID
		,PDL.RequestDateTime
		,ND.Name AS AssignedDealerName
		,PDL.Id as LeadId
		,V.ModelId
		,V.VersionId
		,PDL.PlatformId
		,PDL.PQId
	FROM PQDEALERADLEADS PDL WITH (NOLOCK)
	LEFT OUTER JOIN ThirdPartyLeadQueue TPLQ WITH (NOLOCK) ON PDL.Id = TPLQ.PQId
	INNER JOIN PQ_DEALERSPONSORED PDS WITH (NOLOCK) ON PDL.CAMPAIGNID = PDS.ID AND PDL.DealerLeadBusinessType = 1
	LEFT JOIN DEALERS D WITH (NOLOCK) ON D.ID = PDS.DEALERID
	LEFT JOIN VWMMV V WITH (NOLOCK) ON V.VERSIONID = PDL.VERSIONID
	INNER JOIN ThirdPartyLeadSettings TPLS WITH (NOLOCK) ON TPLS.ModelId = V.ModelId AND TPLS.IsActive = 1
	LEFT JOIN NCS_DEALERS ND WITH (NOLOCK) ON ND.ID = PDL.ASSIGNEDDEALERID
	LEFT JOIN Cities C WITH (NOLOCK) ON C.Id = PDL.CityId
	LEFT JOIN CityZones CZ WITH (NOLOCK) ON CZ.id = PDL.ZoneId
	WHERE TPLQ.PQId IS NULL AND 
	((@LeadType = 'Failed' AND 
				(PDL.PushStatus is null OR PDL.PushStatus = -1)) 
				OR (@LeadType = 'Regular'))
				AND PDL.REQUESTDATETIME BETWEEN @RequestStartDate and @RequestEndDate
				AND PDL.Id NOT IN (SELECT PqdealerAdLeadId FROM DiscardLeads WITH (NOLOCK))
				ORDER BY PDL.Id DESC
END

