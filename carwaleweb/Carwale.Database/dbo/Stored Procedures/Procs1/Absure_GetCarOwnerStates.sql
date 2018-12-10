IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetCarOwnerStates]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetCarOwnerStates]
GO

	---- =============================================
---- Author:	  KARTIK RATHOD
---- Create date: 7 Aug 2015
---- Description: To load States on the basis of the status selected
---- Modified By : Nilima More on 25th sept 2015
---- Description: load states for Doubtfull cases.
---- =============================================

CREATE PROCEDURE [dbo].[Absure_GetCarOwnerStates]
@Status INT,
@AgencyId INT
	
AS
 BEGIN
	 IF @AgencyId IS NULL --For AXA and Finance AXA
	BEGIN
		SELECT		DISTINCT S.Id Value,S.Name Text
		FROM		Cities C WITH(NOLOCK)
		INNER JOIN	AbSure_CarDetails ACD WITH(NOLOCK) ON ACD.OwnerCityId = C.Id
		INNER JOIN  States S WITH(NOLOCK) ON C.StateId = S.Id
		WHERE		S.IsDeleted = 0 
					AND 
					(
						(@Status = 1 AND ACD.Status IS NULL)
						OR (@Status = 2 AND ACD.Status=6)
						OR (@Status = 3 AND ACD.Status=5)
						OR (@Status = 4 AND ACD.Status=1)
						OR (@Status = 5 AND (ACD.Status=4 or ACD.status = 2))
						OR (@Status = 6 AND ACD.Status=3)
						OR (@Status = 7 AND ACD.Status=9 )
					)
		ORDER BY	S.Name
	END
	ELSE --For Agency
	BEGIN
		DECLARE @TblTempUsers TABLE (Id INT)
		INSERT INTO @TblTempUsers(Id) VALUES (@AgencyId)
		INSERT INTO @TblTempUsers(Id) EXEC TC_GetImmediateChild @AgencyId
		
		SELECT		DISTINCT S.Id Value,S.Name Text
		FROM		Cities C WITH(NOLOCK)
		INNER JOIN	AbSure_CarDetails ACD WITH(NOLOCK) ON ACD.OwnerCityId = C.Id
		INNER JOIN  States S WITH(NOLOCK) ON C.StateId = S.Id
		INNER JOIN	AbSure_CarSurveyorMapping CSM WITH (NOLOCK) ON CSM.AbSure_CarDetailsId = ACD.Id
		INNER JOIN	TC_Users U WITH(NOLOCK) ON U.Id = CSM.TC_UserId 
		WHERE		S.IsDeleted = 0 
					AND 
					(
						(@Status = 1 AND ACD.Status IS NULL)
						OR (@Status = 2 AND ACD.Status=6)
						OR (@Status = 3 AND ACD.Status=5)
						OR (@Status = 4 AND ACD.Status=1)
						OR (@Status = 5 AND (ACD.Status=4 or ACD.status = 2))
						OR (@Status = 6 AND ACD.Status=3)
						OR (@Status = 7 AND ACD.Status=9 )
					)
					AND U.ID IN(SELECT ID FROM @TblTempUsers)
		ORDER BY	S.Name
	END
END



--------------------------------------------------------------------------------------------------------------------




