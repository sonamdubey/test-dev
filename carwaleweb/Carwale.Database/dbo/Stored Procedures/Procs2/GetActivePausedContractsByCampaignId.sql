IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetActivePausedContractsByCampaignId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetActivePausedContractsByCampaignId]
GO

	-- =============================================
-- Author:		Shalini Nair	
-- Create date: 22/02/2016
-- Description:	Get all active and paused contracts mapped to a campaign
-- exec [dbo].[GetActivePausedContractsByCampaignId] 4631
-- Modified: Vicky Lund, 01/04/2016, Used applicationId column of TC_ContractCampaignMapping
-- =============================================
CREATE PROCEDURE [dbo].[GetActivePausedContractsByCampaignId]
	-- Add the parameters for the stored procedure here
	@CampaignId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT TC.StartDate AS ContractStartDate
		,TC.EndDate AS ContractEndDate
		,TC.ContractId
	FROM TC_ContractCampaignMapping TC WITH (NOLOCK)
	JOIN PQ_DealerSponsored PDS WITH (NOLOCK) ON TC.CampaignId = PDS.Id
		AND TC.ApplicationID = 1
	WHERE TC.CampaignId = @CampaignId
		AND TC.ContractStatus IN (1, 2) -- ACTIVE AND PAUSED CONTRACTS
END
