IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetUserRole]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetUserRole]
GO

	 

-- =============================================
-- Author	    :	Vicky Gupta(29th Dec 2015)
-- Description	:  To get user's role from  id in string format ex- field-executive
-- TC_GetUserRole 38058
-- Modified By : Vaibhav K 4/5/2016 added distinct keyword 
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetUserRole]
@UserId INT
AS
       BEGIN
	       SELECT DISTINCT RM.RoleName -- Vaibhav K 4/5/2016 added distinct keyword 
		   FROM TC_UsersRole AS UR WITH(NOLOCK) 
		   INNER JOIN TC_RolesMaster AS RM WITH(NOLOCK)
		   ON UR.UserId = @UserId AND UR.RoleId = RM.TC_RolesMasterId
        END