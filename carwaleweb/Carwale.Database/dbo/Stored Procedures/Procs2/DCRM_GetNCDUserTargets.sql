IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetNCDUserTargets]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetNCDUserTargets]
GO

	-- =============================================
-- Author:		Amit Yadav
-- Create date: 26th Feb 2016
-- Description:	To get the targets for NCD user and users under them.
-- EXEC [DCRM_GetNCDUserTargets] 9,'1,2,3',6,2015
--Modified By:Komal Manajre(30-march-2015)
--Desc:IsActive condition while getting users
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetNCDUserTargets]
	@UserId INT,
	@MonthIds VARCHAR(10),
	@MetricId SMALLINT,
	@TargetYear VARCHAR(5)
AS
BEGIN
	--Get the userlevel and users hierarchy node to show the data accordingly
	DECLARE @UserLevel INT, @UserNode HIERARCHYID,@UserBusinessId INT
	SELECT @UserLevel = MU.UserLevel,@UserNode=MU.NodeRec ,@UserBusinessId=MU.BusinessUnitId 
	FROM DCRM_ADM_MappedUsers MU WITH(NOLOCK)
	WHERE MU.OprUserId=@UserId AND MU.IsActive=1            -- IsActive condition while getting users

	--To get the my target of the user itself
	SELECT 
		OU.Id AS OprUserId,
		MU.UserLevel,
		ISNULL(ET.TargetMonth,0) AS MyTargetMonth,
		ISNULL(ET.UserTarget,0) AS MyUserTarget,
		OU.UserName,
		@UserLevel AS LoginUserLevel,
		@UserBusinessId As BuisnessUnitId
	FROM 
		DCRM_ADM_MappedUsers MU WITH(NOLOCK) 
		INNER JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = MU.OprUserId AND MU.IsActive=1
		LEFT JOIN DCRM_FieldExecutivesTarget ET WITH(NOLOCK) ON ET.OprUserId=MU.OprUserId
			AND ET.TargetMonth IN (SELECT * FROM fnSplitCSV(@MonthIds))
			AND ET.TargetYear = @TargetYear
			AND ET.MetricId = @MetricId
			AND ET.BusinessUnitId=@UserBusinessId --For NCD
		WHERE 
			MU.OprUserId = @UserId
	
	--To get the users under the current user
	SELECT 
			OU.Id AS OprUserId,
			OU.UserName,
			MU.UserLevel,
			ISNULL(ET.TargetMonth,0) AS TargetMonth,
			ISNULL(ET.UserTarget,0) AS UserTarget
			--@UserLevel AS LoginUserLevel
	FROM 
		DCRM_ADM_MappedUsers MU WITH(NOLOCK) 
		INNER JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = MU.OprUserId AND MU.IsActive=1
		LEFT JOIN DCRM_FieldExecutivesTarget ET WITH(NOLOCK) ON ET.OprUserId=MU.OprUserId
			AND ET.TargetMonth IN (SELECT * FROM fnSplitCSV(@MonthIds))
			AND ET.TargetYear = @TargetYear
			AND ET.MetricId = @MetricId
			--AND ET.OprUserId = @UserId
			AND ET.BusinessUnitId=@UserBusinessId --For NCD
	WHERE 
		MU.NodeRec.IsDescendantOf(@UserNode)=1 
		AND MU.UserLevel = @UserLevel+1 --To get the next user i.e. the user just under the current user.
END



