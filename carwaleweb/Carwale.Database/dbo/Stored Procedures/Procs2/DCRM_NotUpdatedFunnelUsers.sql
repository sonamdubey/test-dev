IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_NotUpdatedFunnelUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_NotUpdatedFunnelUsers]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(10th July 2015)
-- Description	:	Get users those are not updated their funnel more than 3 days.
--					If not updated since last 7 days then mail to reporting manager
--					and if more than 3 days then send mail to field user.
-- execute [dbo].[DCRM_NotUpdatedFunnelUsers]
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_NotUpdatedFunnelUsers]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT 
		DISTINCT OU.Id,
		OU.UserName,
		OU.PhoneNo AS UserMobileNo,
		(	SELECT OU.UserName FROM DCRM_ADM_MappedUsers MU(NOLOCK) INNER JOIN OprUsers OU(NOLOCK) ON MU.OprUserId = OU.Id
			WHERE MU.NodeRec = (SELECT NodeRec.GetAncestor(1) FROM DCRM_ADM_MappedUsers MU WHERE MU.OprUserId = DAU.Userid)) AS ReprotingUser,
		(	SELECT OU.LoginId FROM DCRM_ADM_MappedUsers MU(NOLOCK) INNER JOIN OprUsers OU(NOLOCK) ON MU.OprUserId = OU.Id
			WHERE MU.NodeRec = (SELECT NodeRec.GetAncestor(1) FROM DCRM_ADM_MappedUsers MU WHERE MU.OprUserId = DAU.Userid)) AS ReprotingUserLoginId,
		ISNULL	([dbo].[DCRM_GetNumberOFWorkDays] 
						(
							(SELECT TOP 1 ActionTakenOn FROM DCRM_SalesMeeting WHERE ActionTakenBy = DAU.Userid ORDER BY ActionTakenOn DESC),
							GETDATE()
						),-1
				) AS NoOfDaysSinceLastUpdate
		
	FROM 
		DCRM_ADM_UserDealers DAU(NOLOCK) 
		INNER JOIN OprUsers OU(NOLOCK) ON DAU.UserId = OU.Id AND OU.IsActive = 1
	

UNION

	SELECT 
		DISTINCT OU.Id,
		OU.UserName,
		OU.PhoneNo AS UserMobileNo,
		(	SELECT OU.UserName FROM DCRM_ADM_MappedUsers MU(NOLOCK) INNER JOIN OprUsers OU(NOLOCK) ON MU.OprUserId = OU.Id
			WHERE MU.NodeRec = (SELECT NodeRec.GetAncestor(1) FROM DCRM_ADM_MappedUsers MU WHERE MU.OprUserId = DAU.Userid)) AS ReprotingUser,
		(	SELECT OU.LoginId FROM DCRM_ADM_MappedUsers MU(NOLOCK) INNER JOIN OprUsers OU(NOLOCK) ON MU.OprUserId = OU.Id
			WHERE MU.NodeRec = (SELECT NodeRec.GetAncestor(1) FROM DCRM_ADM_MappedUsers MU WHERE MU.OprUserId = DAU.Userid)) AS ReprotingUserLoginId,
		ISNULL	([dbo].[DCRM_GetNumberOFWorkDays] 
						(
							(SELECT TOP 1 UpdatedOn FROM DCRM_SalesDealer WHERE UpdatedBy = DAU.Userid ORDER BY UpdatedOn DESC),
							GETDATE()
						),-1
				) AS NoOfDaysSinceLastUpdate
	FROM 
		DCRM_ADM_UserDealers DAU(NOLOCK) 
		INNER JOIN OprUsers OU(NOLOCK) ON DAU.UserId = OU.Id AND OU.IsActive = 1
END

