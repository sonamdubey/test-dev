IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetAssociationType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetAssociationType]
GO

	


-- =============================================
-- Author	:	Kritika Choudhary
-- Create date	:	13th Jan 2016
-- Description	:	Bind campaignId from DCRM_CampaignType
-- =============================================

CREATE PROCEDURE [dbo].[DCRM_GetAssociationType]
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT DCT.Id AS VALUE , DCT.CampaignType AS TEXT
	FROM DCRM_CampaignType DCT(NOLOCK)
	WHERE DCT.IsActive=1
	ORDER BY TEXT 
	

END

