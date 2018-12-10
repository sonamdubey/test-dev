IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetPageLoadDataForMapNewUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetPageLoadDataForMapNewUser]
GO

	-- =============================================
-- Author	:	Sachin Bharti(11th May 2015)
-- Description	:	Used to get page load data for map new user
-- Modifier	:	Sachin Bharti(25th June 2015)
-- Purpose	:	Only to show active mapped users
-- =============================================

CREATE PROCEDURE [dbo].[DCRM_GetPageLoadDataForMapNewUser]
	
AS
BEGIN
	
	--get oprUsers those are not mapped yet
	SELECT 
		DISTINCT OU.Id AS UserId , 
		OU.UserName 
	FROM
		OprUsers OU(NOLOCK) 
		LEFT JOIN DCRM_ADM_MappedUsers AMU(NOLOCK) ON AMU.OprUserId = OU.Id AND AMU.IsActive = 1
	WHERE
		OU.IsActive = 1
		AND AMU.Id IS NULL
	ORDER BY 
		OU.UserName 


	--get business units
	SELECT 
		BU.Id, BU.Name 
	FROM 
		DCRM_BusinessUnit BU(NOLOCK) 
	WHERE 
		BU.IsActive = 1
		AND BU.Id <> 4
	ORDER BY Name  

	--get all mapped users
	SELECT 
		AMU.Id AS MappingId,
		OU.UserName,
		OU.Id AS UserId,
		(SELECT OU.UserName FROM OprUsers OU(NOLOCK) WHERE OU.Id = AMU1.OprUserId) AS ReportsTo,
		AMU1.OprUserId AS ReportsToId,
		BU.Name AS BusinessUnit,
		BU.Id AS BusinessUnitId,
		AMU.IsActive,
		CASE WHEN AMU.IsActive = 0 THEN 'disabled="disabled"' ELSE '' END AS IsDisable,
		CASE WHEN (AMU.UserLevel = 0 OR AMU.IsActive = 0)  THEN 'hide' END AS IsVisible,
		AMU.UserLevel,
		CONVERT(VARCHAR,AMU.MappedOn,106) AS ActivatedOn,
		CONVERT(VARCHAR,AMU.SuspendedOn,106) AS SuspendedOn,
		(SELECT OU.UserName FROM OprUsers OU(NOLOCK) WHERE OU.Id = AMU.MappedBy) AS MappedBy,
		(SELECT OU.UserName FROM OprUsers OU(NOLOCK) WHERE OU.Id = AMU.SuspendedBy) AS SuspendedBy,
		(SELECT COUNT(*) From DCRM_ADM_MappedUsers 
			WHERE NodeRec.GetAncestor(1) = (SELECT TOP 1 NodeRec FROM DCRM_ADM_MappedUsers WHERE OprUserId = AMU.OprUserId and IsActive = 1) and IsActive = 1) AS DirectReportee
	FROM
		DCRM_ADM_MappedUsers AMU(NOLOCK)
		LEFT JOIN DCRM_ADM_MappedUsers AMU1(NOLOCK) ON AMU1.NodeRec = AMU.NodeRec.GetAncestor(1)
		INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = AMU.OprUserId
		INNER JOIN DCRM_BusinessUnit BU(NOLOCK) ON BU.Id = AMU.BusinessUnitId
	WHERE
		AMU.IsActive = 1
	ORDER BY
		AMU.NodeCode , AMU.IsActive DESC
		
END
