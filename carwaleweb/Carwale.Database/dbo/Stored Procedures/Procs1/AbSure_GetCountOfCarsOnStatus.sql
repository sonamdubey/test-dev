IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCountOfCarsOnStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCountOfCarsOnStatus]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: August 11,2015
-- Description:	To get Count for different statuses for PanIndia,State and City
-- Exec AbSure_GetCountOfCarsOnStatus   13,20074,7,'2015-09-1','2015-09-30'
-- Modified By : Nilima More on 25th sept 2015.
-- Description : To get Count for Approval on Hold.
-- Modified By : Nilima More on 12th Oct 2015.
-- Description : To get count for Expired Certificate.
-- Modified by : Ashwini Dhamankar on Oct 29,2015 (Handled if car is not in dubtful state)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetCountOfCarsOnStatus] --1,20074,8,'2015-01-12','2015-10-12'
	-- Add the parameters for the stored procedure here
	@CityId          INT,
	@LoggedInUser	 INT,
	@PendingStatus	 INT,
	@StartDate       DATETIME,                    --Oldest Entry Date
	@EndDate         DATETIME                     --Latest Entry Date  
AS
BEGIN
	DECLARE 
	@StateId INT = NULL,
	@IsAxaAgency BIT

	SELECT @StateId =  C.StateId FROM Cities C WITH(NOLOCK) WHERE ID = @CityId

	IF (SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @LoggedInUser AND RoleId = 15) = 15
	BEGIN	
		SET @IsAxaAgency = 1
		DECLARE @TblTempUsers TABLE (Id INT)
		INSERT INTO @TblTempUsers(Id) VALUES (@LoggedInUser)
		INSERT INTO @TblTempUsers(Id) EXEC TC_GetImmediateChild @LoggedInUser
	END

	;WITH CTE AS
	(
		SELECT ACD.Id, C.StateId,ACD.OwnerCityId,C.Name AS CityName
		FROM AbSure_CarDetails ACD WITH(NOLOCK)
		LEFT JOIN AbSure_CarSurveyorMapping CSMP WITH(NOLOCK) ON ACD.Id= CSMP.AbSure_CarDetailsId 
		LEFT JOIN Cities C WITH(NOLOCK) ON ACD.OwnerCityId = C.ID
		LEFT JOIN States S WITH(NOLOCK) ON C.StateId = S.ID 
		WHERE
		(
				((CSMP.TC_UserId IN(SELECT ID FROM @TblTempUsers))
				OR
				((SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @LoggedInUser AND RoleId IN(9, 14))IN(9,14) 
				AND ISNULL(CSMP.TC_UserId,0) = ISNULL(CSMP.TC_UserId,0)
				))
			AND
				((@PendingStatus = 1 AND CSMP.TC_UserId IS NULL AND ISNULL(ACD.Status,0) <> 3 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Agency assignment pending
						OR (@PendingStatus = 2 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WITH(NOLOCK) WHERE IsAgency = 1) AND ISNULL(ACD.Status,0) <> 3 AND ISNULL(ACD.IsSurveyDone,0) = 0 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Surveyor assignment pending
						OR (@PendingStatus = 3 AND CSMP.TC_UserId IN (SELECT ID FROM TC_Users WITH(NOLOCK) WHERE IsAgency <> 1 AND ISNULL(ACD.Status,0) <> 3) AND ISNULL(ACD.IsSurveyDone,0) = 0 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Inspection pending
						OR (@PendingStatus = 4 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.Status,0) <> 3 AND  ISNULL(ACD.Status,0) <> 9 AND (ACD.IsRejected = 0 OR  ACD.IsRejected is NULL) AND CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE) AND (ISNULL(RCImagePending,0) = 0)) -- Approval Pending 
						OR (@PendingStatus = 5 AND ISNULL(ACD.IsSurveyDone,0) = 1 AND (ACD.FinalWarrantyTypeId IS NOT NULL OR ISNULL(ACD.IsRejected,0) = 1) AND ISNULL(ACD.Status,0) <> 3 AND CAST(ACD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Approval Done
						OR (@PendingStatus = 6 AND ACD.Status = 3 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) -- Cancelled
						OR (@PendingStatus = 7 AND ACD.Status = 9 AND  ISNULL(ACD.IsSurveyDone,0) = 1 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE) ) -- OnHold
						OR (@PendingStatus = 8 AND CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) --Expired Certificate
				)
		)
		)

		SELECT *--, DENSE_RANK() OVER (ORDER BY AreaId) NumberForPaging
        INTO   #TblTemp 
        FROM   CTE

		SELECT COUNT(Id) PanIndiaCount
		FROM #TblTemp;
			
		SELECT COUNT(Id) StateCount
		FROM #TblTemp
		WHERE StateId = @StateId

		SELECT COUNT(Id) CityCount
		FROM #TblTemp
		WHERE OwnerCityId = @CityId

		DROP TABLE #TblTemp

		SELECT C.Name CityName,S.Name AS StateName 
		FROM Cities C WITH(NOLOCK)
		INNER JOIN States S ON C.StateId = S.Id
		WHERE C.ID = @CityId
END
