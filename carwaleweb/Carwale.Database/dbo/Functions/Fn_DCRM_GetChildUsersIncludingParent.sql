IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Fn_DCRM_GetChildUsersIncludingParent]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[Fn_DCRM_GetChildUsersIncludingParent]
GO

	
CREATE FUNCTION [dbo].[Fn_DCRM_GetChildUsersIncludingParent] (@UserId INT)
RETURNS @Childrens TABLE
	(MappingId INT,UsersId INT,UserName VARCHAR(100), NodeRec HIERARCHYID)
AS
BEGIN
	DECLARE @MaxNodeLevel SMALLINT 
	DECLARE	@LoopCount INT = 1
	
	--get max user level in the current hierarchy
	SELECT @MaxNodeLevel = max(UserLevel) from DCRM_ADM_MappedUsers

	--inserting all the child nodes
	WHILE @LoopCount <= @MaxNodeLevel
		BEGIN
			INSERT INTO @Childrens(MappingId, UsersId, UserName, NodeRec)
				SELECT 
					DAM.Id, OU.Id, OU.UserName, DAM.NodeRec
				FROM
					DCRM_ADM_MappedUsers DAM(NOLOCK)
					INNER JOIN OprUsers OU(NOLOCK) ON DAM.OprUserId = Ou.Id AND DAM.IsActive = 1
				WHERE
					DAM.NodeRec.GetAncestor(@LoopCount) = (SELECT NodeRec FROM DCRM_ADM_MappedUsers WHERE OprUserId = @UserId)
			SET @LoopCount = @LoopCount + 1
		END
	--now inserting user also
		INSERT INTO @Childrens(MappingId, UsersId, UserName, NodeRec)
				SELECT 
					DAM.Id, OU.Id, OU.UserName, DAM.NodeRec
				FROM
					DCRM_ADM_MappedUsers DAM(NOLOCK)
					INNER JOIN OprUsers OU(NOLOCK) ON DAM.OprUserId = Ou.Id
				WHERE
					DAM.OprUserId = @UserId
					AND DAM.IsActive = 1
	RETURN
END

