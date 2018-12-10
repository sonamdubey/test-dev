IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetALLParent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetALLParent]
GO
	-- Created By: Manish Chourasiya
-- Created Date:18-06-2013
-- Description: For getting 
  --=============================================
CREATE PROCEDURE  [dbo].[TC_GetALLParent]
 @Userid int
 AS
 BEGIN
 SELECT  T1.Id AS AllParentId   
 FROM  (SELECT ID,lvl,HierId,BranchId,IsActive FROM TC_Users
         WHERE id = @Userid) T2,
                 TC_Users T1
 WHERE
    (T1.HierId = T2.HierId OR  T2.HierId.IsDescendantOf(T1.HierId) = 1)
     AND T1.BranchId=T2.BranchId
     AND T1.Id<>@Userid
     AND T1.IsActive=1
  END