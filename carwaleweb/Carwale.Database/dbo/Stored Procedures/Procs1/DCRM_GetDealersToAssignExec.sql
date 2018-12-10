IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetDealersToAssignExec]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetDealersToAssignExec]
GO
	
-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	26th Nov 2013
-- Description	:	Get Dealers list on basis of roles to assign 
--					to Sales Field ,Service Field and Back office executives
-- Modifier		:	Sachin Bharti(3rd Nov 2013)
-- Purpose		:	Get all dealers for BackOffice executives except 
--					deleted Dealers
-- Modifier	:	Sachin Bharti(12th Jan 2015)
-- Purpose	:	Added filter for stateId hence apply new paramter for that
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetDealersToAssignExec] 
	@RoleId		TINYINT,
	@RegionId	INT = NULL,
	@StateId	INT	= NULL,
	@CityID		INT	= NULL,
	@AreaId		INT	= NULL	
AS
BEGIN
	--For Sales field executives
	IF @RoleId	=	3
		BEGIN
			SELECT DISTINCT D.ID ,D.Organization ,DAR.Name AS Region,ST.Name AS State,CT.Name AS City, A.Name AS Area, 
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 5) AS ServiceField, 
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 3) AS SalesField, 
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 4) AS BO,  
			CASE ISNULL(D.Status,0) WHEN '0' THEN 'ACTIVE' WHEN '1' THEN 'INACTIVE' END AS Status ,D.IsDealerActive,
			CONVERT(CHAR,D.lastupdatedon,113) AS Lastupdatedon FROM Dealers D WITH(NOLOCK)  
			LEFT JOIN DCRM_ADM_RegionCities DARC WITH (NOLOCK) ON D.CityId = DARC.CityId LEFT JOIN DCRM_ADM_Regions DAR WITH(NOLOCK) ON DAR.Id = DARC.RegionId 
			LEFT JOIN States ST WITH (NOLOCK) ON ST.ID = D.StateId LEFT JOIN Cities CT WITH (NOLOCK) ON D.CityId = CT.ID 
			LEFT JOIN Areas A WITH (NOLOCK) ON  A.ID = D.AreaId  
			WHERE D.TC_DealerTypeId IN(1,3) AND D.Status = 1 AND ISNULL(D.IsDealerDeleted,0) <> 1 
			AND D.ID NOT IN(SELECT DealerId FROM DCRM_SalesDealer WHERE  LeadStatus = 1 AND DealerType IN(2,3,5))
			AND	(@RegionId IS NULL OR DAR.Id = @RegionId)
			AND (@StateId IS NULL OR D.StateId = @StateId)
			AND (@CityId IS NULL OR D.CityId = @CityId) 
			AND (@AreaId  IS NULL OR D.AreaId = @AreaId)
			ORDER BY IsDealerActive DESC
		END

	--For Back office executives
	IF @RoleId	=	4
		BEGIN
			SELECT DISTINCT D.ID ,D.Organization ,DAR.Name AS Region,ST.Name AS State,CT.Name AS City, A.Name AS Area,  
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 5) AS ServiceField,  
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 3) AS SalesField,  
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 4) AS BO,  D.TC_DealerTypeId,  
			CASE ISNULL(D.Status,0) WHEN '0' THEN 'ACTIVE' WHEN '1' THEN 'INACTIVE' END AS Status ,D.IsDealerActive,
			CONVERT(CHAR,D.lastupdatedon,113) AS Lastupdatedon   
			FROM Dealers D WITH(NOLOCK)   
			LEFT JOIN DCRM_ADM_RegionCities DARC WITH (NOLOCK) ON D.CityId = DARC.CityId  
			LEFT JOIN DCRM_ADM_Regions DAR WITH(NOLOCK) ON DAR.Id = DARC.RegionId  
			LEFT JOIN States ST WITH (NOLOCK) ON ST.ID = D.StateId  
			LEFT JOIN Cities CT WITH (NOLOCK) ON D.CityId = CT.ID  
			LEFT JOIN Areas A WITH (NOLOCK) ON  A.ID = D.AreaId   
			WHERE ISNULL(D.IsDealerDeleted,0) <> 1   -- D.Status = 0 AND ISNULL(D.IsDealerActive,0) = 1 AND 
			AND	(@RegionId IS NULL OR DAR.Id = @RegionId)
			AND (@StateId IS NULL OR D.StateId = @StateId)
			AND (@CityId IS NULL OR D.CityId = @CityId) 
			AND (@AreaId  IS NULL OR D.AreaId = @AreaId)
			ORDER BY IsDealerActive DESC
		END

	--For Service field executives
	IF @RoleId	=	5
		BEGIN
			SELECT DISTINCT D.ID ,D.Organization ,DAR.Name AS Region,ST.Name AS State,CT.Name AS City, A.Name AS Area, 
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 5) AS ServiceField, 
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 3) AS SalesField, 
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 4) AS BO,  
			CASE ISNULL(D.Status,0) WHEN '0' THEN 'ACTIVE' WHEN '1' THEN 'INACTIVE' END AS Status ,D.IsDealerActive,
			CONVERT(CHAR,D.lastupdatedon,113) AS Lastupdatedon 
			FROM Dealers D WITH(NOLOCK)  
			LEFT JOIN DCRM_ADM_RegionCities DARC WITH (NOLOCK) ON D.CityId = DARC.CityId 
			LEFT JOIN DCRM_ADM_Regions DAR WITH(NOLOCK) ON DAR.Id = DARC.RegionId 
			LEFT JOIN States ST WITH (NOLOCK) ON ST.ID = D.StateId 
			LEFT JOIN Cities CT WITH (NOLOCK) ON D.CityId = CT.ID 
			LEFT JOIN Areas A WITH (NOLOCK) ON  A.ID = D.AreaId  
			WHERE D.TC_DealerTypeId IN(1,3) AND D.Status = 0 
			AND	(@RegionId IS NULL OR DAR.Id = @RegionId)
			AND (@StateId IS NULL OR D.StateId = @StateId)
			AND (@CityId IS NULL OR D.CityId = @CityId) 
			AND (@AreaId  IS NULL OR D.AreaId = @AreaId)
			UNION  
			SELECT DISTINCT D.ID ,D.Organization ,DAR.Name AS Region,ST.Name AS State,CT.Name AS City, A.Name AS Area, 
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 5) AS ServiceField, 
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 3) AS SalesField,  
			(SELECT TOP 1 UserName FROM OprUsers, DCRM_ADM_UserDealers WHERE DealerId = D.id AND UserId = Id AND RoleId = 4) AS BO,   
			CASE ISNULL(D.Status,0) WHEN '0' THEN 'ACTIVE' WHEN '1' THEN 'INACTIVE' END AS Status ,D.IsDealerActive,
			CONVERT(CHAR,D.lastupdatedon,113) AS Lastupdatedon 
			FROM Dealers D WITH(NOLOCK)  
			LEFT JOIN DCRM_ADM_RegionCities DARC WITH (NOLOCK) ON D.CityId = DARC.CityId 
			LEFT JOIN DCRM_ADM_Regions DAR WITH(NOLOCK) ON DAR.Id = DARC.RegionId 
			LEFT JOIN States ST WITH (NOLOCK) ON ST.ID = D.StateId 
			LEFT JOIN Cities CT WITH (NOLOCK) ON D.CityId = CT.ID 
			LEFT JOIN Areas A WITH (NOLOCK) ON  A.ID = D.AreaId  
			WHERE D.TC_DealerTypeId IN(1,3) AND D.Status = 1 
			AND D.ID IN(SELECT DealerId FROM DCRM_SalesDealer  WHERE  LeadStatus = 1 AND DealerType IN(2,3,5)) 
			AND	(@RegionId IS NULL OR DAR.Id = @RegionId)
			AND (@StateId IS NULL OR D.StateId = @StateId)
			AND (@CityId IS NULL OR D.CityId = @CityId) 
			AND (@AreaId  IS NULL OR D.AreaId = @AreaId)
			ORDER BY IsDealerActive DESC
		END
END


