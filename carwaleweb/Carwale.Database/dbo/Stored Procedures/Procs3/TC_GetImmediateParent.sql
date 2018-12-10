IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetImmediateParent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetImmediateParent]
GO
	
-- Created By: Manish Chourasiya
-- Created Date:18-06-2013
-- Description:-- Description: For getting Immediate parents of the node used in user hierarchy
  --=============================================
CREATE  PROCEDURE  [dbo].[TC_GetImmediateParent]
 @UserId int,
 @TC_ReportingTo INT OUTPUT
 AS
 BEGIN
 SELECT  @TC_ReportingTo=T1.Id 
 FROM  (SELECT ID,lvl,HierId,BranchId,IsActive FROM TC_Users
         WHERE id = @Userid) T2,
                 TC_Users T1
 WHERE
    (T1.HierId = T2.HierId OR  T2.HierId.IsDescendantOf(T1.HierId) = 1)
     and T1.BranchId=T2.BranchId
     and T1.Id<>@Userid
     AND T1.lvl=T2.lvl-1
     AND T1.IsActive=1
  END
