USE [CarWale]
GO
/****** Object:  StoredProcedure [dbo].[GetCampaignTemplateGroups]    Script Date: 19-10-2016 14:08:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================      
-- Author:  <Chetan Thambad>      
-- Create date: <17/10/2016>      
-- Description: <Get template id from a group against ABCookie value>  
-- exec [GetCampaignTemplateGroups] 1,4682
--==============================================
ALTER PROCEDURE [dbo].[GetCampaignTemplateGroups] @PlatformId INT
	,@CampaignId INT
AS
BEGIN

CREATE TABLE #GroupTemplateMaster (Id INT, CookieValue INT, TemplateId INT)

INSERT INTO #GroupTemplateMaster (Id, CookieValue, TemplateId) SELECT TGABCM.Id, TGABCM.ABCookieValue
		,TGM.TemplateId
	FROM TemplateGroupABCookieMapping TGABCM WITH (NOLOCK)
	INNER JOIN TemplateGroupMapping TGM WITH (NOLOCK) ON TGABCM.TemplateGroupMappingId = TGM.Id
	INNER JOIN PQ_DealerAd_Template_Platform_Maping PDTPM WITH (NOLOCK) ON TGM.GroupId = PDTPM.AssignedGroupId
	INNER JOIN TemplateGroup TG WITH (NOLOCK) ON PDTPM.AssignedGroupId = TG.Id
	WHERE PDTPM.CampaignId = @CampaignId
		AND PDTPM.PlatformId = @PlatformId
		AND TG.PlatformId = @PlatformId

		SELECT a.CookieValue, a.TemplateId
        FROM #GroupTemplateMaster a
        INNER JOIN  (SELECT CookieValue,MIN(Id) as id FROM #GroupTemplateMaster GROUP BY CookieValue ) AS b 
		ON a.CookieValue = b.CookieValue AND a.id = b.id;

DROP Table #GroupTemplateMaster

END
