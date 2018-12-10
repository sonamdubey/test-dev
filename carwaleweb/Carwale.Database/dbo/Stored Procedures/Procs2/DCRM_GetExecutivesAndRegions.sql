IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetExecutivesAndRegions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetExecutivesAndRegions]
GO

	


-- =============================================
-- Author	:	Sachin Bharti(6th Aug 2015)
-- Description	:	Get field executives and region
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetExecutivesAndRegions]
	
AS
BEGIN
	
	-- get only sales and service field executives
	SELECT 
		DISTINCT DAU.UserId,
		OU.UserName
	FROM
		DCRM_ADM_UserDealers DAU(NOLOCK)
		INNER JOIN OprUsers OU(NOLOCK) ON DAU.UserId = Ou.Id AND OU.IsActive = 1 AND DAU.RoleId IN (3,5) --role id for sales and service field
	ORDER BY
		OU.UserName


	--get regions
	SELECT 
		DAR.Id ,
		DAR.Name AS Region
	FROM	
		DCRM_ADM_Regions DAR(NOLOCK)
	WHERE
		DAR.IsActive = 1
END

