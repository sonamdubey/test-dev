IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetThirdPartyLeadQueue]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetThirdPartyLeadQueue]
GO

	-- =============================================
-- Author:		Vinayak
-- Create date: 1/03/2016
-- Description:	Fetches Third Party Q Ids
-- Modified By: Shalini Nair, 24/10/2016, considering the leads sent, lead volume and campaign dates to fetch the campaign
-- =============================================
CREATE PROCEDURE [dbo].[GetThirdPartyLeadQueue] 
	 @ModelId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ThirdPartyLeadSettingId AS ThirdPartyLeadId 
	FROM ThirdPartyLeadSettings tpSet WITH (NOLOCK)
	INNER JOIN HTTPRequestTypes reqType WITH (NOLOCK) ON tpSet.HttpRequestType = reqType.Id
	WHERE (
			ModelId = @ModelId
			OR (
				tpSet.ModelId = - 1
				AND MakeId = (
					SELECT CarMakeId
					FROM CarModels WITH (NOLOCK)
					WHERE ID = @ModelId
					)
				)
			)
		AND IsActive = 1
		AND LeadVolume > LeadsSent
		AND GETDATE() >= CampaignStartDate
		AND GETDATE() <= CampaignEndDate
END

