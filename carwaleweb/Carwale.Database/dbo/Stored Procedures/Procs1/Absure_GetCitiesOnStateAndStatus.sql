IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetCitiesOnStateAndStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetCitiesOnStateAndStatus]
GO

	---- =============================================
---- Author:	  KARTIK RATHOD
---- Create date: 10 Aug 2015
---- Description: to load Cities on the basis of State and Status
---- =============================================


CREATE PROCEDURE [dbo].[Absure_GetCitiesOnStateAndStatus]
@Status INT,
@AgencyId INT,
@StateId INT
	
AS
BEGIN
	IF @AgencyId IS NULL --For AXA and Finance AXA
	BEGIN
		SELECT		DISTINCT C.Id Value,C.Name Text
		FROM		Cities C WITH(NOLOCK)
		INNER JOIN	AbSure_CarDetails ACD WITH(NOLOCK) ON ACD.OwnerCityId = C.Id
		WHERE		C.IsDeleted = 0 
					AND C.StateId =@StateId
					AND 
					(
						(@Status = 1 AND ACD.Status IS NULL)
						OR (@Status = 2 AND ACD.Status=6)
						OR (@Status = 3 AND ACD.Status=5)
						OR (@Status = 4 AND ACD.Status=1)
						OR (@Status = 5 AND ACD.Status=4)
						OR (@Status = 6 AND ACD.Status=3)
					)
		ORDER BY	C.Name
	END
	ELSE --For Agency
	BEGIN
		DECLARE @TblTempUsers TABLE (Id INT)
		INSERT INTO @TblTempUsers(Id) VALUES (@AgencyId)
		INSERT INTO @TblTempUsers(Id) EXEC TC_GetImmediateChild @AgencyId
		
		SELECT		DISTINCT C.Id Value,C.Name Text
		FROM		Cities C WITH(NOLOCK)
		INNER JOIN	AbSure_CarDetails ACD WITH(NOLOCK) ON ACD.OwnerCityId = C.Id
		INNER JOIN	AbSure_CarSurveyorMapping CSM WITH (NOLOCK) ON CSM.AbSure_CarDetailsId = ACD.Id
		INNER JOIN	TC_Users U WITH(NOLOCK) ON U.Id = CSM.TC_UserId --AND IsAgency=1
		WHERE		C.IsDeleted = 0 
					AND C.StateId =@StateId
					AND 
					(
						(@Status = 1 AND ACD.Status IS NULL)
						OR (@Status = 2 AND ACD.Status=6)
						OR (@Status = 3 AND ACD.Status=5)
						OR (@Status = 4 AND ACD.Status=1)
						OR (@Status = 5 AND ACD.Status=4)
						OR (@Status = 6 AND ACD.Status=3)
					)
					AND U.ID IN(SELECT ID FROM @TblTempUsers)
		ORDER BY	C.Name
	END
END
