IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetUsersList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetUsersList]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 18 Feb 2015
-- Description:	To Get Users List of Axa panel
-- Updated By : Vinay  kumar Prajapati  added new column "UnAvailableDate"
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetUsersList]
	@UserId INT,
	@OrderBy  VARCHAR(100)=NULL,
	@FromIndex       INT, 
	@ToIndex         INT
   
AS
BEGIN
	DECLARE @HierId HIERARCHYID
	DECLARE @LVL SMALLINT
	DECLARE @BranchId SMALLINT

	SELECT  @HierId =hierid ,@LVL=LVL,@BranchId=BranchId FROM TC_Users WHERE id= @UserId; 

	WITH Cte1
	AS (
		SELECT Users.Id,(SELECT TOP 1  ISNULL(DATEDIFF(day,GETDATE(),ISNULL(SUD.UnavailableDate,GETDATE()-1)),-1) FROM AbSure_SurveyorUnavailabilityDetails AS SUD WITH(NOLOCK) WHERE SUD.SurveyorId = Users.Id AND ISNULL(SUD.UnavailableDate,GETDATE()-1) > GETDATE()-1 ORDER BY SUD.UnavailableDate) UnAvailableDate, Users.BranchId, Users.UserName, Users.Email, Users.Password, Users.Mobile, Users.EntryDate, Users.Id AS Value,Users.UserName AS Text,
		Users.DOB, Users.DOJ, Users.Sex,Users.Address,UR.RoleId
		               
		FROM TC_Users Users
		JOIN TC_UsersRole UR ON UR.UserId = Users.Id
    	WHERE Hierid.IsDescendantOf(@HierId)= 1 and Hierid.GetAncestor(1) = @HierId
		AND lvl<>@lvl
		AND BranchId=@BranchId
		AND IsActive=1
	)

    SELECT *, ROW_NUMBER() OVER (ORDER BY 
                    CASE WHEN @OrderBy ='UserName~1' THEN UserName END,
	                CASE WHEN @OrderBy ='UserName~2' THEN UserName END DESC,
	                CASE WHEN @OrderBy ='UpcomingLeaves~1' THEN UnAvailableDate  END,
					CASE WHEN @OrderBy ='UpcomingLeaves~2' THEN UnAvailableDate END DESC,
					CASE WHEN @OrderBy ='Mobile~1' THEN  Mobile END,
					CASE WHEN @OrderBy ='Mobile~2' THEN  Mobile END DESC
	
	) NumberForPaging INTO   #TblTemp FROM   Cte1 
	SELECT * FROM #TblTemp WHERE (@FromIndex IS NULL AND @ToIndex IS NULL) OR (NumberForPaging  BETWEEN @FromIndex AND @ToIndex )
	
	SELECT COUNT(*) AS RecordCount 
	FROM #TblTemp 

    DROP TABLE #TblTemp 
END

