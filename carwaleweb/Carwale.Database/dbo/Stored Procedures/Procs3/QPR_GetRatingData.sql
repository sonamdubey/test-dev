IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[QPR_GetRatingData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[QPR_GetRatingData]
GO

	
-- =============================================
-- Author:		Afrose 
-- Create date: 7th Jan 2016
-- Description:	to get the questions submitted by the user
-- EXEC QPR_GetRatingData 6
-- =============================================
CREATE PROCEDURE [dbo].[QPR_GetRatingData] 
@UserId INT 
AS
BEGIN
			DECLARE @Qpr_RatingDataId INT

			SELECT OprA.UserName AS [Name],OprB.UserName AS [ReportingManager],OprB.Id AS [ManagerId], Desig.Designation AS [Designation], Dept.DepartmentName AS [Department],
			Desig.Id AS  [DesignationId] ,  Dept.HR_DepartmentId AS [DepartmentId], OPrA.EmployeeCode AS [EmployeeId] , OPrA.HostURL AS HostURL , OPrA.OriginalImgPath AS OriginalImgPath
			FROM  OprUsers OPrA WITH(NOLOCK)
			LEFT JOIN OprUsers OprB WITH(NOLOCK) ON	OprB.id=OprA.ReportingManagerId
			LEFT JOIN HR_Designation Desig WITH(NOLOCK) ON OprA.Designation=Desig.Id 
			LEFT JOIN HR_Department Dept WITH(NOLOCK) ON OprA.UserDeptId=Dept.HR_DepartmentId
			WHERE OprA.Id =@UserId

			SELECT TOP 1 @Qpr_RatingDataId = ID 
			FROM Qpr_RatingData WITH(NOLOCK) 
			WHERE UserId=@UserId
			ORDER BY CreatedOn DESC

			SELECT TOP 1 Mission, IsSubmitted ,EndTime, ExtraAchieved
			FROM Qpr_RatingData WITH(NOLOCK) 
			WHERE UserId=@UserId
			ORDER BY CreatedOn DESC
			 
			SELECT KRA,KPI,Weightage,SelfScore
			FROM Qpr_Outcomes WITH(NOLOCK)  
			WHERE Qpr_RatingDataId = @Qpr_RatingDataId


			SELECT Question_Id,Comments,Qpr_ResponseValuesId 
			FROM Qpr_QuestionsResponse WITH(NOLOCK)  
			WHERE Qpr_RatingDataId = @Qpr_RatingDataId
			Order By Question_Id

			
END


