IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[QPR_GetDashboardDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[QPR_GetDashboardDetails]
GO

	
-- =============================================
-- Author:		Afrose 
-- Create date: 7th Jan 2016
-- Description:	to check if user has filled up the form, and check if users under manager have filled up the same. 
--EXEC QPR_GetDashboardDetails 6
-- =============================================
CREATE PROCEDURE [dbo].[QPR_GetDashboardDetails] 
@UserId INT
AS
BEGIN

	SELECT TOP 1 * 
	FROM Qpr_RatingData WITH(NOLOCK) 
	WHERE UserId =@UserId
	ORDER BY CreatedOn DESC

	SELECT A.id AS [EmpId], A.UserName AS [EmployeeName],A.Designation,B.UserName AS [ReportingManager],B.Id AS [ManagerId]
	INTO #tmp
	FROM OprUsers A WITH(NOLOCK)
	JOIN OprUsers B WITH(NOLOCK) ON B.id=A.ReportingManagerId
    WHERE B.Id=@UserId --to find all those employees reporting under me
	
	SELECT EmpId, EmployeeName, #tmp.ManagerId, ReportingManager, IsSubmitted 
	FROM #tmp WITH(NOLOCK) 
	left join Qpr_RatingData WITH(NOLOCK) ON #tmp.EmpId=Qpr_RatingData.UserId
	--and Type=1--check if reporting employees have filled up rating 
	
	DROP TABLE #tmp
END