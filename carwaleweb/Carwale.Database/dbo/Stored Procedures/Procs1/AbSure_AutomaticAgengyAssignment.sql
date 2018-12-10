IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_AutomaticAgengyAssignment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_AutomaticAgengyAssignment]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 1st June 2015
-- Description:	Automatic agengy assignment based on the owner car and city
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_AutomaticAgengyAssignment]
	@AbSure_CarId INT
AS
BEGIN
	DECLARE @OwnerCityId BIGINT,
			@OwnerAreaId BIGINT,
			@TC_UserId BIGINT

	SELECT @OwnerCityId = OwnerCityId,@OwnerAreaId = OwnerAreaId FROM AbSure_CarDetails WHERE ID = @AbSure_CarId

	SELECT @TC_UserId = Id
	FROM TC_USERS 
	WHERE IsAgency=1 AND IsActive=1 AND lvl=2
	AND CityId=@OwnerCityId

	IF @@ROWCOUNT > 1
	BEGIN
		SELECT @TC_UserId = Id
		FROM TC_USERS 
		WHERE IsAgency=1 AND IsActive=1 AND lvl=2 
		AND (AreaId=@OwnerAreaId)

		IF @@ROWCOUNT = 1
		BEGIN
			EXEC Absure_SaveCarSurveyorMapping @TC_UserId,@AbSure_CarId,NULL,NULL,NULL,NULL,NULL
		END
	END
	ELSE
	BEGIN
		EXEC Absure_SaveCarSurveyorMapping @TC_UserId,@AbSure_CarId,NULL,NULL,NULL,NULL,NULL
	END
END
