IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetLeadDetailsOnLeadId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetLeadDetailsOnLeadId]
GO

	
-- =============================================
-- Author:		Sanjay
-- Create date: 23/06/2016
-- Description:	Get Lead Data
-- =============================================
CREATE PROCEDURE [dbo].[GetLeadDetailsOnLeadId] 
	@LeadId NUMERIC
AS
BEGIN
	select P.Id as PQDealerAdLeadId
	,p.CityId as CityId
	,p.ZoneId as ZoneId
	,p.VersionId as VersionId
	,p.CampaignId as DealerId
	,p.Email as Email
	,p.Name as Name
	,p.Mobile as Mobile
	,'1 week' as BuyTimeText
	,7 as BuyTimeValue
	,1 as RequestType
	,'34' as InquirySourceId
	,p.DealerLeadBusinessType as LeadBussinessType
	,p.PQId as PQId
	,p.LeadClickSource as LeadClickSource
	,p.PlatformId as PlatformSourceId
	,p.AssignedDealerID as AssignedDealerId
	,case when p.AssignedDealerID > 0 then 'true' else 'false' end as IsAutoApproved
	,V.Model as ModelName 
	,V.ModelId as ModelId 
	from pqdealeradleads P with(nolock) 
    inner join vwMMV V with(nolock) on P.VersionId=V.VersionId 
    where P.Id =@LeadId
END
