IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetUsersUnderHierarchy]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetUsersUnderHierarchy]
GO

	-- =============================================
-- Author:	Komal Manjare
-- Create date: 18-May-2016
-- Description:	get users under the hierarchy of logged in user
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetUsersUnderHierarchy]  
	@UserId INT
AS
BEGIN
	-- Get userlevel,node and businessUnit Id for current user.
    DECLARE @UserLevel INT, @UserNode HIERARCHYID,@UserBusinessId INT
	SELECT @UserLevel = MU.UserLevel,@UserNode=MU.NodeRec ,@UserBusinessId=MU.BusinessUnitId 
	FROM DCRM_ADM_MappedUsers MU WITH(NOLOCK)
	WHERE MU.OprUserId=@UserId AND MU.IsActive=1   
	
	SELECT OU.Id as UserId,Ou.UserName,OU.LoginId AS LoginName,DAM.UserLevel,DAM.BusinessUnitId
	FROM DCRM_ADM_MappedUsers DAM(NOLOCK) 
	INNER JOIN OprUsers OU(NOLOCK) ON OU.Id=DAM.OprUserId
	WHERE DAM.NodeRec.IsDescendantOf(@UserNode)=1 
	AND DAM.UserLevel = @UserLevel+1 AND DAM.IsActive=1 --To get the users just one level below the current user
END
