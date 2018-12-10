IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetUserManagerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetUserManagerDetails]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 9-Dec-2015
-- Description:	To get opr user's reporting manager details DCRM
-- =============================================
CREATE PROCEDURE  [dbo].[DCRM_GetUserManagerDetails]
	-- Add the parameters for the stored procedure here
	@UserId			INT,
	@BusinessUnitId	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	/*
	SELECT 
		AMU.Id AS MappingId,
		OU.UserName,
		OU.Id AS UserId,
		(SELECT OU.UserName FROM OprUsers OU(NOLOCK) WHERE OU.Id = AMU1.OprUserId) AS ReportsTo,
		(SELECT OU.LoginId FROM OprUsers OU(NOLOCK) WHERE OU.Id = AMU1.OprUserId) AS ReportsToLoginId,
		AMU1.OprUserId AS ReportsToId,		
		BU.Name AS BusinessUnit,
		BU.Id AS BusinessUnitId,
		AMU.IsActive
	FROM
		DCRM_ADM_MappedUsers AMU(NOLOCK)
		LEFT JOIN DCRM_ADM_MappedUsers AMU1(NOLOCK) ON AMU1.NodeRec = AMU.NodeRec.GetAncestor(1)
		INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = AMU.OprUserId
		INNER JOIN DCRM_BusinessUnit BU(NOLOCK) ON BU.Id = AMU.BusinessUnitId
	WHERE 
	AMU.IsActive = 1 
	AND BU.Id = @BusinessUnitId
	AND OU.Id = @UserId
	*/
	--Above query commented by Vaibhav K 15-Feb-2016 and query changed to the below one.
	SELECT TOP 1
		AMU.Id AS MappingId,
		OU.UserName,
		OU.LoginId,
		OU.Id AS UserId,
		--(SELECT OU.UserName FROM OprUsers OU(NOLOCK) WHERE OU.Id = AMU1.OprUserId) AS ReportsTo,
		--(SELECT OU.LoginId FROM OprUsers OU(NOLOCK) WHERE OU.Id = AMU1.OprUserId) AS ReportsToLoginId,
		--AMU1.OprUserId AS ReportsToId,
		OUL2.UserName l2UserName,
		OUL2.LoginId l2LoginId,
		OUL2.Id l2Userid,
		OUL1.UserName L1UserName,
		OUL1.LoginId L1LoginId,
		OUL1.Id L1UserId,
		BU.Name AS BusinessUnit,
		BU.Id AS BusinessUnitId,
		AMU.IsActive
	FROM
		DCRM_ADM_MappedUsers AMU WITH (NOLOCK)
		JOIN DCRM_ADM_MappedUsers AMU1 WITH (NOLOCK) ON AMU1.NodeRec = AMU.NodeRec.GetAncestor(1)
		JOIN OprUsers OU WITH (NOLOCK) ON OU.Id = AMU.OprUserId
		JOIN DCRM_BusinessUnit BU WITH (NOLOCK) ON BU.Id = AMU.BusinessUnitId
		JOIN OprUsers OUL2 WITH (NOLOCK) ON OUL2.Id = AMU1.OprUserId
		LEFT JOIN DCRM_ADM_MappedUsers AMU2 WITH (NOLOCK) ON AMU2.NodeRec = AMU1.NodeRec.GetAncestor(1)
		LEFT JOIN OprUsers OUL1 WITH (NOLOCK) ON AMU2.OprUserId = OUl1.Id
	WHERE 
	AMU.IsActive = 1 
	AND (@BusinessUnitId IS NULL OR BU.Id = @BusinessUnitId)
	AND OU.Id = @UserId
END

------------------------------------------------------------------

------------------------------------------------Amit Yadav SP Changes---------------------------------------------


