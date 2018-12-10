IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetSubCampaigns]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetSubCampaigns]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 8th oct,2013
-- Description:	Get Sub Campaigns
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetSubCampaigns] 
	@MainCampaignId INT 
AS
BEGIN	
	SET NOCOUNT ON;
    
	SELECT '' as ID , SubMainCampaignName AS Text,TC_SubMainCampaignID as SUB
	FROM TC_SubMainCampaign WITH(NOLOCK)
	WHERE TC_MainCampaignId = @MainCampaignId AND IsActive = 1
	
	UNION

	SELECT TC_SubCampaignId as ID,SubCampaignName AS Text,SMC.TC_SubMainCampaignId as SUB
	FROM TC_SubCampaign SC
	INNER JOIN TC_SubMainCampaign SMC ON SMC.TC_SubMainCampaignId=SC.TC_SubMainCampaignId
	WHERE SMC.TC_MainCampaignId =@MainCampaignId AND SMC.IsActive = 1
	ORDER BY SUB,ID
END
