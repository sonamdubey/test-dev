IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[QPR_EmpBasicDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[QPR_EmpBasicDetails]
GO

	
-- =============================================
-- Author:		Afrose 
-- Create date: 7th January 2016
-- Description:	to save employee details
--EXEC QPR_EmpBasicDetails 1
-- =============================================
CREATE PROCEDURE [dbo].[QPR_EmpBasicDetails]
@UserId INT
AS 
BEGIN
	SELECT OprA.UserName AS [Name],OprB.UserName AS [ReportingManager],OprB.Id AS [ManagerId], Desig.Designation AS [Designation], Dept.DepartmentName AS [Department],
			Desig.Id AS  [DesignationId] ,  Dept.HR_DepartmentId AS [DepartmentId], OPrA.EmployeeCode AS [EmployeeId] 
	FROM  OprUsers OPrA WITH(NOLOCK)
	LEFT JOIN OprUsers OprB WITH(NOLOCK) ON	OprB.id=OprA.ReportingManagerId
	LEFT JOIN HR_Designation Desig WITH(NOLOCK) ON OprA.Designation=Desig.Id 
	LEFT JOIN HR_Department Dept WITH(NOLOCK) ON OprA.UserDeptId=Dept.HR_DepartmentId
    WHERE OprA.Id =@UserId
END


