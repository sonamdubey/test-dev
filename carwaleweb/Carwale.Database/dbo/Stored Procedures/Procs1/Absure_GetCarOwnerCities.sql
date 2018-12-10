IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetCarOwnerCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetCarOwnerCities]
GO

	-- =============================================
-- Author:		Chetan Navin
-- Create date: 3rd Feb 2015
-- Description:	To fetch dealers with warranty
-- Modified By Ruchira Patil on 3rd July 2015 (to load cities on the basis of the status selected )
-- Dropdown status 1 (Agency Assignment Pending -- null)
-- Dropdown status 2 (Surveyor Assignment Pending -- 6)
-- Dropdown status 3 (Inspection Pending -- 5)
-- Dropdown status 4 (Approval Pending -- 1)
-- Dropdown status 5 (Approval Done -- 4)
-- Dropdown status 6 (Cancelled Request -- 3)
-- Modified By : Nilima More on 15th sept 2015 
-- Dropdown status 7 (Doubtfull -- 9)
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetCarOwnerCities]
@Status INT,
@AgencyId INT
	
AS
BEGIN
	IF @AgencyId IS NULL --For AXA and Finance AXA
	BEGIN
		SELECT		DISTINCT C.Id Value,C.Name Text
		FROM		Cities C WITH(NOLOCK)
		INNER JOIN	AbSure_CarDetails ACD WITH(NOLOCK) ON ACD.OwnerCityId = C.Id
		WHERE		C.IsDeleted = 0 
					AND 
					(
						(@Status = 1 AND ACD.Status IS NULL)
						OR (@Status = 2 AND ACD.Status=6)
						OR (@Status = 3 AND ACD.Status=5)
						OR (@Status = 4 AND ACD.Status=1)
						OR (@Status = 5 AND (ACD.Status=4 or ACD.status = 2))
						OR (@Status = 6 AND ACD.Status=3)
						OR (@Status = 7 AND ACD.Status=9)
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
					AND 
					(
						(@Status = 1 AND ACD.Status IS NULL)
						OR (@Status = 2 AND ACD.Status=6)
						OR (@Status = 3 AND ACD.Status=5)
						OR (@Status = 4 AND ACD.Status=1)
						OR (@Status = 5 AND (ACD.Status=4 or ACD.status = 2))
						OR (@Status = 6 AND ACD.Status=3)
						OR (@Status = 7 AND ACD.Status=9)
					)
					AND U.ID IN(SELECT ID FROM @TblTempUsers)
		ORDER BY	C.Name
	END

 --   -- Insert statements for procedure here
	--SELECT DISTINCT C.Id,C.Name FROM Cities C WITH(NOLOCK)
	--INNER JOIN AbSure_CarDetails ACD WITH(NOLOCK) ON ACD.OwnerCityId = C.Id
	--WHERE C.IsDeleted = 0 
	--ORDER BY C.Name

END


-----------------------------------------------------------------------------------------------------------------------------------------------

