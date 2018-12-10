IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetMappedExecutive]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetMappedExecutive]
GO

	
-- =============================================
-- Author:		Kartik Rathod
-- Create date: 6 June 2016
-- Description:	to get mapped executive to dealer
--  DCRM_GetMappedExecutive 3,15346
-- Modifier : Vaibhav K 14 July 2016
-- Join with Dealers,TC_Applications tables to get the dealer ApplicationId & ApplicationName
-- Modified By : Komal Manjare on 24-August-2016
-- Desc:get L2name and L2Email for the respective L3
-- =============================================
CREATE  PROCEDURE [dbo].[DCRM_GetMappedExecutive] 
@RoleId SMALLINT,
@DealerId BIGINT
AS
BEGIN
	SELECT 
	OPR.LoginId, OPR.Id, OPR.UserName,D.ApplicationId, AP.ApplicationName,
	(SELECT OU.UserName FROM DCRM_ADM_MappedUsers DAM1 (NOLOCK) INNER JOIN OprUsers OU(NOLOCK)  ON OU.Id=DAM1.OprUserId WHERE DAM1.NodeRec=DAM.NodeRec.GetAncestor(1) AND OU.IsActive=1) AS L2Name, --Komal Manjare on 24-August-2016
	(SELECT OU.LoginId FROM DCRM_ADM_MappedUsers DAM1 (NOLOCK) INNER JOIN OprUsers OU(NOLOCK) ON OU.Id=DAM1.OprUserId WHERE DAM1.NodeRec=DAM.NodeRec.GetAncestor(1) AND OU.IsActive=1) AS L2LoginId	
	
	FROM	DCRM_ADM_UserDealers AUD WITH(NOLOCK)
	JOIN	OprUsers OPR WITH(NOLOCK) ON AUD.UserId = OPR.ID
	JOIN	Dealers D with(nolock) on AUD.DealerId = D.ID
	JOIN	TC_Applications AP with(nolock) on D.ApplicationId = AP.ApplicationId
	JOIN	DCRM_ADM_MappedUsers DAM (NOLOCK) ON DAM.OprUserId=OPR.Id -- Komal Manjare on 24-August-2016
		WHERE
			AUD.DealerId = @DealerId AND AUD.RoleId = @RoleId AND OPR.IsActive = 1
END

