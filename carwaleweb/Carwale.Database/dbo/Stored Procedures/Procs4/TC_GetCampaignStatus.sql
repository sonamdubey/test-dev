IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCampaignStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCampaignStatus]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <7 Oct 2015>
-- Description:	<Get Campaigns status>
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCampaignStatus]
	@CampaignId	INT,
	@ContractBehaviour SMALLINT
AS
BEGIN
	IF @ContractBehaviour = 1 -- LEAD BASED
		BEGIN
			SELECT
			SUM(CASE WHEN 
			(ContractBehaviour= 1 AND TotalGoal <> TotalDelivered AND (TotalDelivered > 0 OR TotalDelivered IS NOT NULL))
			THEN 1 END) AS PartiallyinComplete,

			SUM(CASE WHEN (ContractBehaviour= 1  AND (TotalDelivered = 0 OR TotalDelivered IS NULL)) 
			THEN 1 END) AS InComplete 

			FROM TC_ContractCampaignMapping WITH(NOLOCK)
			WHERE CampaignId = @CampaignId
			AND ContractStatus = 2
		END
	ELSE IF @ContractBehaviour = 2
		BEGIN
			SELECT DATEDIFF(DAY,DS.PausedDate,DS.EndDate) PendingDays
			FROM PQ_DealerSponsored DS WITH(NOLOCK) 
			INNER JOIN TC_ContractCampaignMapping CC WITH(NOLOCK)  ON CC.CampaignId = DS.Id
			WHERE DS.Id = @CampaignId AND DS.IsActive = 0 
			AND ContractStatus = 2
		END
END
