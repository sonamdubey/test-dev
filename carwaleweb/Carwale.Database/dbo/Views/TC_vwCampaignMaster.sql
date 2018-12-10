IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'TC_vwCampaignMaster' AND
     DROP VIEW dbo.TC_vwCampaignMaster
GO

	

CREATE View [dbo].[TC_vwCampaignMaster]
AS
SELECT 
			M.MakeId, 
			M.TC_MainCampaignId,
			M.MainCampaignName,
			S.TC_SubMainCampaignId,
			S.SubMainCampaignName,
			SU.TC_SubCampaignId,
			SU.SubCampaignName
FROM TC_MainCampaign  AS M WITH (NOLOCK) 
JOIN TC_SubMainCampaign AS S WITH (NOLOCK) ON M.TC_MainCampaignId=S.TC_MainCampaignId
JOIN TC_SubCampaign AS SU WITH (NOLOCK) ON S.TC_SubMainCampaignId=SU.TC_SubMainCampaignId
WHERE M.IsActive=1
AND S.IsActive=1
AND SU.IsActive=1


