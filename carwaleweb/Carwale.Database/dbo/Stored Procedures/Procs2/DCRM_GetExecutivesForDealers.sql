IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetExecutivesForDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetExecutivesForDealers]
GO

	-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	(25th Nov 2013)
-- Description	:	Get executives for assign dealers based on
--					roleId and region
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetExecutivesForDealers]	
	@RegionId	INT = NULL,
	@RoleId		INT = NULL
AS
	BEGIN
	
		SELECT DISTINCT  DAU.UserId AS Value, OU.UserName AS Text  
		FROM DCRM_ADM_UserRoles   DAU WITH(NOLOCK) 
		INNER JOIN OprUsers OU WITH(NOLOCK)ON DAU.UserId = OU.Id  
		INNER JOIN DCRM_ADM_UserRegions DAR WITH(NOLOCK) ON DAR.UserId = OU.Id 
		WHERE ( DAU.RoleId =@RoleID ) AND OU.IsActive = 1  
		AND (DAR.RegionId = @RegionId)  ORDER BY OU.UserName
	END
