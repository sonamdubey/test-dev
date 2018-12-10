IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetMFCDealersToPushLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetMFCDealersToPushLead]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <12/2/2015>
-- Description:	<Returns MFC dealers>
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetMFCDealersToPushLead] 
AS
BEGIN
	--SELECT  D.ID AS DealerId,ISNULL(D.Address1,'') + ' - ' + ISNULL(D.Organization,'') AS DealerName, MD.SendMixMatchLead, 
	--CASE WHEN CP.ExpiryDate >= CONVERT(DATE,GETDATE()) THEN 'Active' ELSE 'Expired' END AS Status,MD.LeadCntPerDay
	--FROM TC_MFCDealers MD 
	--INNER JOIN ConsumerCreditPoints CP ON MD.DealerId = CP.ConsumerId AND CP.ConsumerType = 1   
	--INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = MD.DealerId
	--ORDER BY Status, DealerName

	SELECT tab.DealerId,tab.DealerName,tab.LeadCntperDay,tab.Status,tab.SendMixMatchLead,
	SUM(CASE WHEN MixMatchLead =1 THEN tab.Count END) AS MixMatchInq,
	SUM(CASE WHEN MixMatchLead IS NULL OR MixMatchLead=0 THEN tab.Count END) AS BuyerInq
	FROM
	(
	SELECT  D.ID AS DealerId,ISNULL(D.Address1,'') + ' - ' + ISNULL(D.Organization,'') AS DealerName, MD.SendMixMatchLead, 
	CASE WHEN CP.ExpiryDate >= CONVERT(DATE,GETDATE()) THEN 'Active' ELSE 'Expired' END AS Status,MD.LeadCntPerDay
	,COUNT(TC_BuyerInquiryId) as Count,MixMatchLead
	FROM TC_MFCDealers MD WITH(NOLOCK)
	INNER JOIN ConsumerCreditPoints CP WITH(NOLOCK) ON MD.DealerId = CP.ConsumerId AND CP.ConsumerType = 1   
	INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = MD.DealerId
	left JOIN TC_PushLeadLog PL WITH(NOLOCK) ON MD.DealerId = PL.DealerId
	GROUP BY D.ID,MixMatchLead,Status,LeadCntPerDay,D.Address1,D.Organization,SendMixMatchLead,CP.ExpiryDate
	)TAB  group by tab.DealerId,tab.DealerName,tab.LeadCntperDay,tab.Status,tab.SendMixMatchLead
	ORDER BY TAB.Status,TAB.DealerName
END
