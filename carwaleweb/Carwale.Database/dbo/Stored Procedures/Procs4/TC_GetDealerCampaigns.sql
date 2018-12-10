IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerCampaigns]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerCampaigns]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <05 Oct 15>
-- Description:	<GET ALL CAMPAIGNS FOR DEALER>
-- GetDealerCampaigns 5,2
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealerCampaigns]
	@DealerId	INT,
	@ContractBehaviour	SMALLINT -- 1 FOR LEAD BASED 2 FOR DATE BASED
AS
BEGIN
	SELECT PDS.Id AS Id,PDS.DealerId,PDS.DealerEmailId,PDS.StartDate,PDS.EndDate
	,PDS.TotalCount,PDS.DailyCount,PDS.TotalGoal,PDS.DailyGoal,
	CASE PDS.LeadPanel WHEN 1 THEN 'Normal CRM' WHEN 2 THEN 'Autobiz' WHEN 3 THEN 'CRM Autoassigned' END AS LeadPanel,SM.MaskingNumber,
	CASE WHEN PDS.IsActive = 0 THEN 'Paused' ELSE
	CASE WHEN ( @ContractBehaviour = 1 AND (PDS.TotalGoal IS NOT NULL AND PDS.TotalGoal <> 0) AND PDS.TotalGoal = PDS.TotalCount)
	OR(@ContractBehaviour = 2 AND PDS.EndDate IS NOT NULL AND (PDS.TotalGoal IS NULL OR PDS.TotalGoal = 0) AND PDS.EndDate < GETDATE() )
	THEN 'Completed' ELSE 'Running' END END AS Status
	
	FROM PQ_DealerSponsored PDS WITH (NOLOCK) 	
	LEFT JOIN MM_SellerMobileMasking SM WITH (NOLOCK) ON SM.LeadCampaignId = PDS.Id
	WHERE PDS.DealerId = @DealerId 
	AND 
	(	
		(
			@ContractBehaviour = 1 AND PDS.EndDate IS NULL  
			-- (PDS.TotalGoal IS NOT NULL AND PDS.TotalGoal <> 0)
		) 
		OR 
		(
			@ContractBehaviour = 2 AND PDS.EndDate IS NOT NULL  -- DATE BASED
		)
	) 
	--AND PDS.Type = @ContractBehaviour 
	ORDER BY PDS.Id DESC


	SELECT Organization FROM Dealers WITH(NOLOCK) WHERE ID=@DealerId
END

