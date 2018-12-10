IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UserDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UserDealer]
GO

	
--Created By	:	Sachin Bharti(24th April 2013)
--Purpose		:	To get all Dealers assigned to that field executive
--Modifier      :   Vinay kumar prajapati (4th Oct 2013)
--purpose       :   Don't show  deleted dealer
CREATE PROCEDURE [dbo].[DCRM_UserDealer]
@UserID		NUMERIC(18,0)
AS
BEGIN
	
	SELECT  DISTINCT D.ID AS DealerId ,D.Organization ,D.MobileNo,D.ContactPerson,C.Name AS City,
	CASE ISNULL(D.Status,0) WHEN 'False' THEN 1 ELSE 0 END AS IsDealerActive ,DAR.Name As RoleName,
    DC.ScheduleDate,OU.UserName AS ScheduledBy,ISNULL(DC.Id,0)AS CallId,DT.DealerType,
    CASE ISNULL(D.IsDealerDeleted,0) WHEN 'True' THEN 1 ELSE 0 END AS IsDealerDeleted
    FROM DCRM_ADM_UserDealers DAU WITH (NOLOCK) 
    INNER	JOIN Dealers D WITH (NOLOCK) ON DAU.DealerId = D.ID 
    LEFT	JOIN DCRM_Calls DC WITH (NOLOCK) ON DC.DealerId = DAU.DealerId  AND ActionTakenId = 2 AND DC.UserId = @UserID
    INNER   JOIN Cities AS C WITH (NOLOCK) ON C.ID = D.CityId 
    LEFT    JOIN OprUsers AS OU WITH (NOLOCK) ON OU.Id = DC.ScheduledBy
    LEFT	JOIN DCRM_ADM_Roles DAR WITH(NOLOCK) ON DAR.Id = DAU.RoleId
    LEFT	JOIN TC_DealerType DT WITH(NOLOCK) ON DT.TC_DealerTypeId = D.TC_DealerTypeId
    
    WHERE   DAU.UserId = @UserID AND DAU.RoleId IN(SELECT DAU.RoleId FROM DCRM_ADM_UserRoles DAU WITH(NOLOCK) WHERE DAU.UserId = @UserID) 
	AND ( D.IsDealerDeleted = 0 OR D.IsDealerDeleted IS NULL)
	--AND D.Status = 0  --show all non deleted dealer
    ORDER	BY DC.ScheduleDate DESC 
    
END
