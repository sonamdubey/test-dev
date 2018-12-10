IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAllChildAbsure]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAllChildAbsure]
GO
	-- Created By: Vinay Kumar
-- Created Date:22nd April 2015
-- Description: For getting all child of any parent for maintaining user hierarchy
  --=============================================
CREATE PROCEDURE  [dbo].[TC_GetAllChildAbsure]
 @UserId INT 
AS

BEGIN
        DECLARE @HierId HIERARCHYID, @LVL SMALLINT, @BranchId SMALLINT, @ChildrenId VARCHAR(MAX)

		DECLARE @TempAxaTable TABLE
		(
			Id INT IDENTITY(1,1),
			UserId INT, --- this is use for get all id under given userId
			ChildId INT,
			IsAgency BIT,
			HierId HierarchyId
		)
		
		SELECT	@HierId =hierid, @LVL=LVL,@BranchId=BranchId 
		FROM	TC_Users WITH(NOLOCK)
		WHERE	id=@UserId

		INSERT INTO @TempAxaTable
		SELECT	@UserId,ID,IsAgency,HierId 
		FROM	TC_Users WITH(NOLOCK)
		WHERE	Hierid.IsDescendantOf(@HierId)= 1
				AND lvl=@lvl+1
				AND BranchId=@BranchId
				AND IsActive=1
		ORDER BY ID
		

		SELECT	U.Id AS UserId, ISNULL(U.IsAgency,0) IsAgency,U.UserName,U.lvl,U.HierId,DENSE_RANK() OVER(ORDER BY T.Id) HierarchyRank
		FROM	TC_Users U WITH(NOLOCK)
				LEFT JOIN @TempAxaTable T ON U.HierId.IsDescendantOf(T.HierId) = 1
		WHERE	IsActive=1
				AND BranchId=@BranchId
		ORDER BY U.HierId

END