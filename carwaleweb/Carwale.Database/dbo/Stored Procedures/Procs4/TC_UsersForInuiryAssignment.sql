IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UsersForInuiryAssignment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UsersForInuiryAssignment]
GO

	-- =============================================
-- Author:		Surendra
-- Create date: 12 Jan 2012
-- Description:	This procedure is user list for Inquiry module to assig user to particular lead
-- Modified By : Tejashree Patil on 28 Jun 2013, Added AND R.RoleId in(4,5,6) condition in where clause.
-- Modified By : Tejashree Patil on 25 July 2013, Only required parameters fetched.
-- =============================================
CREATE PROCEDURE [dbo].[TC_UsersForInuiryAssignment]
(
	@BranchId BIGINT
)
AS
BEGIN	
	SELECT	DISTINCT U.Id, U.UserName, U.Email
	FROM	TC_Users U 
			INNER JOIN TC_UsersRole R WITH(NOLOCK)ON U.Id=R.UserId
	WHERE	U.IsActive=1 AND U.IsCarwaleUser=0
			AND R.RoleId in(4,5,6)
			AND U.BranchId=@BranchId
END

