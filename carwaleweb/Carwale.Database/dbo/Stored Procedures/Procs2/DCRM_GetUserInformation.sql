IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetUserInformation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetUserInformation]
GO

	-- =============================================
-- Author	:	Sachin Bharti(3rd Jan 2013)
-- Description	:	Get user information based on user
-- Modifier      : Ajay Singh (7 jan 2016)
-- Description   : get more information related to user
--exec DCRM_GetUserInformation 93
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetUserInformation]
	
	@UserId		NUMERIC(18,0)
AS
BEGIN
	
	        SELECT OU1.Id AS UserID , OU1.UserName ,OU1.LoginId,OU1.Address,OU1.PhoneNo,OU1.RoleIds,OU1.IsOutsideAccess,OU2.LoginId AS ReportingManager,OU2.Id AS ReportingManagerId,HDM.DepartmentName,HDM.HR_DepartmentId AS DepartmentnameId,HDG.Designation,HDG.Id AS DesignationId,OU1.EmployeeCode AS EmployeeCode
	        FROM OPRUSERS  OU1 WITH(NOLOCK)
	        LEFT JOIN OprUsers OU2 WITH(NOLOCK) ON OU2.Id=OU1.ReportingManagerId
	        LEFT JOIN HR_Department HDM WITH(NOLOCK) ON HDM.HR_DepartmentId=OU1.UserDeptId
	        LEFT JOIN HR_Designation HDG WITH(NOLOCK) ON HDG.Id = OU1.Designation
	        WHERE OU1.Id = @UserId
	
END

