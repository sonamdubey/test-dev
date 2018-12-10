IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetOutletUserDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetOutletUserDetail]
GO
	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 15-06-2015
-- Description:	Get outlet dealer login details
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetOutletUserDetail]
	-- Add the parameters for the stored procedure here
@OutletBranchId BIGINT,
@UserEmail VARCHAR(200) OUTPUT,
@UserPassword VARCHAR(50) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT TOP 1 @UserEmail = TCU.Email, @UserPassword = TCU.Password 
	FROM TC_Users TCU WITH(NOLOCK)
	JOIN TC_UsersRole TUR WITH(NOLOCK)
	ON TCU.Id = TUR.UserId AND TCU.BranchId = @OutletBranchId AND TCU.IsActive= 1 AND TUR.RoleId = 1
	ORDER BY TCU.Id
	
END
