IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignDetails]
GO

	-- =============================================
-- Author:		Anchal Gupta
-- Create date: 30/09/2015
-- Description: 
-- Modified by: Shalini Nair on 30/09/2015
-- Modified by: Vicky Lund on 07/10/2015
--Modified by : Shalini Nair on 03/11/2015
-- =============================================
CREATE PROCEDURE [dbo].[GetCampaignDetails]
	-- Add the parameters for the stored procedure here
	@ContractId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ccm.DealerId AS DealerId
		,D.Organization AS DealerName
		,ccm.TotalGoal
		,ccm.TotalDelivered
		,ds.TotalGoal as CampaignTotalGoal
		,ds.TotalCount as CampaignTotalDelivered
		,ds.DailyCount 
		,ds.DailyGoal
		,ds.DealerEmailId AS DealerEmailId
		,ds.EnableDealerEmail
		,ds.EnableDealerSMS
		,ds.EnableUserEmail
		,ds.EnableUserSMS
		,ds.CampaignPriority
		,ds.LeadPanel
		,ccm.CampaignId
		,ds.IsActive
		,ds.Phone AS MaskingNumber
		,MM.Mobile
		,MM.ConsumerType
		,MM.NCDBrandId
		,ccm.StartDate 
		,ccm.EndDate
		,ccm.ContractBehaviour 
		,ds.LeadPanel
	FROM TC_ContractCampaignMapping ccm WITH (NOLOCK)
	LEFT JOIN Dealers D WITH (NOLOCK) ON CCM.DealerId = D.ID
	LEFT JOIN PQ_DealerSponsored ds WITH (NOLOCK) ON ccm.CampaignId = ds.Id
	LEFT JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON ccm.DealerId = MM.ConsumerId
	AND MM.LeadCampaignId = ccm.CampaignId
	WHERE ccm.ContractId = @ContractId;
END
