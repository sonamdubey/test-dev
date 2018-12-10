IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UsersUpdateHierarchy]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UsersUpdateHierarchy]
GO

	-- Created By: Manish Chourasiya
-- Created  date: 18/03/2013
-- Description:	Used to update hierarchy of the users within the branch
-- Modified By Manish on 18-06-2013 Adding condition Branch id for optimisation purpose.
-- =============================================
CREATE  PROCEDURE [dbo].[TC_UsersUpdateHierarchy]
@UpdateId  INT,         
@ParentID  INT
AS      
      
BEGIN 
		DECLARE
			@hid     AS HIERARCHYID,
			@mgr_hid  AS HIERARCHYID,
			@last_child_hid AS HIERARCHYID;
			
			DECLARE @BranchId INT 
			
			SELECT @BranchId=BranchID FROM TC_Users WHERE ID=@UpdateId;

			IF @ParentID is NULL
			begin
				SET @hid = HIERARCHYID::GetRoot();
				end
			ELSE
				BEGIN
						SET @mgr_hid = (SELECT HierId FROM TC_Users  WHERE ID = @ParentID);
						SET @last_child_hid =(SELECT MAX(HierId) FROM TC_Users WHERE HierId.GetAncestor(1) = @mgr_hid AND BranchId=@BranchId); --Adding Condition branch id by Manish on 18-06-2013
						SET @hid = @mgr_hid.GetDescendant(@last_child_hid, NULL);
				END
    
  				UPDATE  TC_Users set HierId=@hid where id=@UpdateId     
						
				
 
END
/****** Object:  StoredProcedure [dbo].[TC_GetUserAuthorization]    Script Date: 06/20/2013 15:30:41 ******/
SET ANSI_NULLS ON
