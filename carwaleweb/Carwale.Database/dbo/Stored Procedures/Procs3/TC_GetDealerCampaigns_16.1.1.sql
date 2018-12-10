IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerCampaigns_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerCampaigns_16]
GO
	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <05 Oct 15>
-- Description:	<GET ALL CAMPAIGNS FOR DEALER>
-- GetDealerCampaigns 5,2
-- Modified By Chetan Thambad <31/12/2015> : Removed StartDate, EndDate, TotalCount, DailyCount, TotalGoal, DailyGoal columns and retrived DealerName
-- Modified By Chetan Thambad <17/02/2016> : To get information based on campaign behavior
-- Modified By : Shalini Nair on 23/02/2016 to get STATUS as ACTIVE or PAUSED 
-- Modified by :Shalini Nair on 02/03/2016 to retrieve only campaigns which have a contract assosciated with it
-- Modified: Vicky Lund, 05/04/2016, Used applicationId column of MM_SellerMobileMasking, TC_ContractCampaignMapping
-- EXEC [TC_GetDealerCampaigns_16.1.1] 3797,1
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealerCampaigns_16.1.1] @DealerId INT
	,@ContractBehaviour SMALLINT -- 1 FOR LEAD BASED 2 FOR DATE BASED
AS
BEGIN
	SELECT DISTINCT PDS.Id AS Id
		,PDS.DealerId
		,PDS.DealerEmailId
		,PDS.DealerName
		,-- Modified By Chetan
		CASE PDS.LeadPanel
			WHEN 1
				THEN 'Normal CRM'
			WHEN 2
				THEN 'Autobiz'
			WHEN 3
				THEN 'CRM Autoassigned'
			END AS LeadPanel
		,CASE 
			WHEN PDS.IsDefaultNumber = 1
				THEN (
						SELECT TOP 1 TollFreeNumber
						FROM CarwaleTollFreeNumber WITH (NOLOCK)
						)
			ELSE SM.MaskingNumber
			END MaskingNumber
		,CASE 
			WHEN PDS.IsActive = 0
				THEN 'Paused'
			WHEN PDS.IsActive = 1
				THEN 'Active'
			END AS STATUS
	FROM PQ_DealerSponsored PDS WITH (NOLOCK)
	INNER JOIN TC_ContractCampaignMapping TC WITH (NOLOCK) ON TC.CampaignId = PDS.ID
		AND TC.ApplicationID = 1
	LEFT JOIN MM_SellerMobileMasking SM WITH (NOLOCK) ON SM.LeadCampaignId = PDS.Id
		AND SM.ApplicationId = 1
	WHERE PDS.DealerId = @DealerId
		AND (
			(
				@ContractBehaviour = 1
				AND PDS.CampaignBehaviour = 1 -- LEAD BASED
				)
			OR (
				@ContractBehaviour = 2
				AND PDS.CampaignBehaviour = 2 -- DATE BASED
				AND TC.StartDate IS NOT NULL
				AND TC.EndDate IS NOT NULL
				)
			)
	ORDER BY PDS.Id DESC

	SELECT Organization
	FROM Dealers WITH (NOLOCK)
	WHERE ID = @DealerId
END
