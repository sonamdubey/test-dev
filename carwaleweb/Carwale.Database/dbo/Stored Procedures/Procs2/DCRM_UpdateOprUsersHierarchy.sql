IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateOprUsersHierarchy]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateOprUsersHierarchy]
GO

	
-- Created By	: Sachin Bharti(13th May 2015)
-- Description	:	Used to update hierarchy of opr users within the branch
-- execute DCRM_OprUsersUpdateHierarchy 8,28
-- =============================================
CREATE  PROCEDURE [dbo].[DCRM_UpdateOprUsersHierarchy]
	@UpdateId  INT,         
	@ParentID  INT = NULL
AS      
      
BEGIN 
	DECLARE
		@hid     AS HIERARCHYID,
		@mgr_hid  AS HIERARCHYID,
		@last_child_hid AS HIERARCHYID;

	IF @ParentID is NULL
		BEGIN
			SET @hid = HIERARCHYID::GetRoot();
		END
	ELSE
		BEGIN
				SET @mgr_hid = (SELECT NodeRec FROM DCRM_ADM_MappedUsers  WHERE OprUserId = @ParentID AND IsActive = 1);
				SET @last_child_hid =(SELECT MAX(NodeRec) FROM DCRM_ADM_MappedUsers WHERE NodeRec.GetAncestor(1) = @mgr_hid); --Adding Condition branch id by Manish on 18-06-2013
				SET @hid = @mgr_hid.GetDescendant(@last_child_hid, NULL);
		END
    
	--updating hierarchy
  	UPDATE  DCRM_ADM_MappedUsers set NodeRec=@hid where id=@UpdateId     
 
END

