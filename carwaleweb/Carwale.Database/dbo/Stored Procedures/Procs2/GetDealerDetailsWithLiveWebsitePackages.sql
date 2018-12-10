IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerDetailsWithLiveWebsitePackages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerDetailsWithLiveWebsitePackages]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 16 June 2016
-- Description:	to get dealerid with other details for which package is running
-- Modified By : Kartik Rathod on 20 Jun 2016,To get details of website which expires in 15 days.
-- 36	Site Development / 37	Site Maintenance
-- Modified By : Vaibhav K 22 June 2016 changed Rvn.AmountPaid to Rvn.ClosingAmount
-- Modified By : Vaibhav K 25 July 2016 added condition for rvn package status (running)
-- Modified By : Mihir Chheda on 25 sept 2016, fetched L2Email,D.EmailId AS DealerEmail,D.ApplicationId
-- Modified by : Kartik removed cast function from RVN.PackageEndDate
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerDetailsWithLiveWebsitePackages] 
	@GetDealersForExpirationAlert BIT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	declare @currentdate DATE = CAST(GETDATE() AS DATE)
	declare @MailAlertDuration VARCHAR(50) = '0,15,30,60,90'

	select	RVN.DealerId,CONVERT(VARCHAR(11),RVN.PackageStartDate,106) AS PackageStartDate,CONVERT(VARCHAR(11),RVN.PackageEndDate,106) AS PackageEndDate,
			D.Organization AS DealerName,RVN.ClosingAmount AS Amount,P.Name as PackageName,D.WebsiteUrl,OPR.LoginId AS L3Email
			,(SELECT OU.LoginId
			  FROM DCRM_ADM_MappedUsers DAM1(NOLOCK)
			  INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = DAM1.OprUserId
			  WHERE DAM1.NodeRec = DAM.NodeRec.GetAncestor(1)
			  AND OU.IsActive = 1) AS L2Email
			,D.EmailId AS DealerEmail,D.ApplicationId
	
	from	rvn_dealerpackagefeatures RVN with(nolock)
	join	Dealers D with(nolock) on RVN.DealerId  = D.Id
	join	Packages P with(nolock) on RVN.PackageId = P.Id
	left join DCRM_ADM_UserDealers AUD WITH(NOLOCK) on RVN.DealerId = AUD.DealerId and AUD.RoleId = 3
	left join	OprUsers OPR WITH(NOLOCK) ON AUD.UserId = OPR.ID AND OPR.IsActive = 1
	LEFT JOIN DCRM_ADM_MappedUsers DAM(NOLOCK) ON DAM.OprUserId = OPR.Id
	where	
		RVN.PackageId in (36,37)	
		and RVN.PackageStatus = 2 --Vaibhav K 25 July 2016 -- package status running
		and CAST(RVN.PackageStartDate AS DATE) <= @currentdate and CAST(RVN.PackageEndDate AS DATE) >= @currentdate
		and (
					@GetDealersForExpirationAlert IS NULL 
				OR	@GetDealersForExpirationAlert = 0 
				OR	datediff(day,@currentdate,RVN.PackageEndDate) in (select ListMember from fnSplitCSV(@MailAlertDuration))
			) 

END


