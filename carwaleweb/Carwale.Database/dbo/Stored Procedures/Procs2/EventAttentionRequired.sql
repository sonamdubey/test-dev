IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EventAttentionRequired]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EventAttentionRequired]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE EventAttentionRequired
	-- Add the parameters for the stored procedure here
	@FromDate datetime,@ToDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

declare @StartDate datetime,@endDate datetime
set @StartDate =convert(datetime,convert(varchar(10),@FromDate,120)+ ' 00:00:00')
set @endDate=convert(datetime,convert(varchar(10),@ToDate,120)+ ' 23:59:59')


select distinct convert(date,EventRaisedOn) RaisedOn
,o.UserName RaisedBy
,make
,c.FirstName 
,c.Mobile
,c.Email
,DealerComment
,ct.Name City
,RM.Name cr
,rm.Designation
,d.Name Dealer
,o1.UserName DealerDC
,convert(date,EventCompletedOn) CompletedOn
,o2.username Completedby
,convert(date,ApprovedOn) ApprovedOn
,o3.username approvedBy
from CRM_CustomerCallRqstLog r
left join OprUsers o on r.EventRaisedBy = o.Id
left join NCS_Dealers d on d.ID=r.DealerId
left join CRM_CarBasicData b on b.ID=r.CBDId
left join CRM_Leads l on b.LeadId=l.ID
left join CRM_Customers c on l.CNS_CustId=c.ID
left join Analytics..vwMMV v on b.VersionId=v.id
left join Cities ct on ct.ID=c.CityId
left join NCS_RMDealers rd on rd.DealerId=D.ID
left join NCS_RManagers rm on RM.Id=rd.RMId
left join CRM_ADM_DCDealers dc on dc.DealerId=d.ID
left join OprUsers o1 on dc.DCID=o1.id
left join OprUsers o2 on  o2.Id=r.EventCompletedBy
left join OprUsers o3 on  o3.Id=r.ApprovedOn
where EventRaisedOn between @StartDate and @endDate

END
