IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_GetTeamLeaders]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_GetTeamLeaders]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 2nd Nov 2016
-- Description:	To get list of TLs of a dealer
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_GetTeamLeaders] 
	@DealerId INT
AS
BEGIN
		SELECT Id,UserName FROM TC_Users U WITH (NOLOCK)
		join TC_UsersRole UR WITH (NOLOCK) on UR.UserId=U.Id
    	WHERE BranchId=@DealerId
		AND IsActive=1
		AND UR.RoleId = 21 -- team leader
		ORDER BY Id 
END