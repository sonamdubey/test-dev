IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetUserShownOnCarwale]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetUserShownOnCarwale]
GO

	--	============================================================
--	Author	    :	Upendra
--	Created On  :	18/01/2016 
--	Description :	To get user selected to be shown on carwale from UserList.aspx
--	============================================================

CREATE PROCEDURE [dbo].[TC_GetUserShownOnCarwale]
@BranchId INT
AS
BEGIN
 SELECT UserName,ImageUrl FROM TC_Users WITH(NOLOCK) WHERE BranchId=@BranchId AND IsShownCarwale=1 AND IsActive=1
END