IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetALLChild]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetALLChild]
GO
	
-- Created By: Manish Chourasiya
-- Created Date:18-06-2013
-- Description: For getting all child of any parent for maintaining user hierarchy
  --=============================================
CREATE PROCEDURE  [dbo].[TC_GetALLChild]
@UserId INT
AS
BEGIN

	DECLARE @HierId HIERARCHYID
	DECLARE @LVL SMALLINT
	DECLARE @BranchId SMALLINT

		SELECT  @HierId =hierid ,@LVL=LVL,@BranchId=BranchId FROM TC_Users WHERE id=@UserId
	
		SELECT ID AS AllChildId FROM TC_Users
    	WHERE Hierid.IsDescendantOf(@HierId)= 1
		AND lvl<>@lvl
		AND BranchId=@BranchId
		AND IsActive=1
		ORDER BY ID
END
