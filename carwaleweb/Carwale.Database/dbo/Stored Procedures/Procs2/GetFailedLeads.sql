IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFailedLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFailedLeads]
GO

	
-- =============================================
-- Author:		Sanjay Soni
-- Create date: 23/06/2016
-- Description:	Get failed Lead Data
-- Modified BY : Sanjay Soni, on 01/10/2016 added zone id in select query 
-- =============================================
CREATE PROCEDURE [dbo].[GetFailedLeads] 
     @StartDate DATE
	,@EndDate DATE
	,@LeadType varchar(10)
AS
BEGIN
    DECLARE @RequestStartDate datetime , @RequestEndDate datetime 

    set @RequestStartDate = convert(datetime,convert(varchar(10),@StartDate,120)+ ' 00:00:00')	
	set @RequestEndDate = convert(datetime,convert(varchar(10),@EndDate,120)+ ' 23:59:59');

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
		,V.Version
		,PDL.Name
		,PDL.Email
		,PDL.Mobile
		,C.Name AS City
		,C.Id AS CityId
		,PDL.ZoneId
		,CZ.ZoneName AS Zone
		,PDL.AssignedDealerID 
		,PDL.RequestDateTime
		,ND.Name AS AssignedDealerName
		,PDL.Id as LeadId
		,V.ModelId
		,V.VersionId
		,PDL.PlatformId
		,PDL.PQId
	FROM PQDEALERADLEADS PDL WITH (NOLOCK)
	INNER JOIN PQ_DEALERSPONSORED PDS WITH (NOLOCK) ON PDL.CAMPAIGNID = PDS.ID AND PDL.DealerLeadBusinessType = 0
	LEFT JOIN DEALERS D WITH (NOLOCK) ON D.ID = PDS.DEALERID
	LEFT JOIN VWMMV V WITH (NOLOCK) ON V.VERSIONID = PDL.VERSIONID
	LEFT JOIN NCS_DEALERS ND WITH (NOLOCK) ON ND.ID = PDL.ASSIGNEDDEALERID
	LEFT JOIN Cities C WITH (NOLOCK) ON C.Id = PDL.CityId
	LEFT JOIN CityZones CZ WITH (NOLOCK) ON CZ.id = PDL.ZoneId
	WHERE ((@LeadType = 'Failed' AND 
				(PDL.PushStatus is null OR PDL.PushStatus = -1)) 
				OR (@LeadType = 'Regular'))
				AND PDL.REQUESTDATETIME BETWEEN @RequestStartDate and @RequestEndDate
				AND PDL.Id NOT IN (SELECT PqdealerAdLeadId FROM DiscardLeads WITH (NOLOCK))
				Order BY PDL.Id desc
END


