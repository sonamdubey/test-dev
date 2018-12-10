IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MaintainUserHierarchy]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MaintainUserHierarchy]
GO

	
-- Created By: Manish Chourasiya
-- Created Date:21-06-2013
-- Description: Maintain hierarachy of the user after update hierarchy.
  --=============================================
CREATE PROCEDURE [dbo].[TC_MaintainUserHierarchy]
@UpdateId INT,
@NewParentId INT
AS
BEGIN
	DECLARE @TblAllChild TABLE (ChildId INT)
	DECLARE @TblParentChild TABLE (ID INT IDENTITY(1,1),OldParentId INT,ChildId INT,LVL INT)
	DECLARE @WhileLoopCount INT
	DECLARE @WhileLoopControl INT =1
	DECLARE @ChildId INT
	DECLARE @ParentId INT


	INSERT INTO @TblAllChild   EXEC TC_GetALLChild @UpdateId

	INSERT INTO @TblParentChild (OldParentId,ChildId,LVL )
	SELECT  T1.Id  ,T2.Id,T2.lvl
	 FROM  (SELECT ID,lvl,HierId,BranchId,IsActive 
			FROM TC_Users AS U
			JOIN @TblAllChild AS T ON T.ChildId=U.Id
			) T2,
			TC_Users T1
	 WHERE
		(T1.HierId = T2.HierId OR  T2.HierId.IsDescendantOf(T1.HierId) = 1)
		 and T1.BranchId=T2.BranchId
		 and T1.Id<>T2.Id
		 AND T1.lvl=T2.lvl-1
		 AND T1.IsActive=1
		 ORDER BY lvl
	     
	EXEC TC_UsersUpdateHierarchy @UpdateId,@NewParentId

	SELECT @WhileLoopCount=COUNT(*) FROM @TblAllChild

	WHILE (@WhileLoopControl<=@WhileLoopCount)
	BEGIN 
	 
		 SELECT @ChildId=ChildId,@ParentId=OldParentId FROM @TblParentChild WHERE ID=@WhileLoopControl
		 
		 EXEC TC_UsersUpdateHierarchy @ChildId,@ParentId

		 
		 SET @WhileLoopControl=@WhileLoopControl+1
	 
	END 

END



