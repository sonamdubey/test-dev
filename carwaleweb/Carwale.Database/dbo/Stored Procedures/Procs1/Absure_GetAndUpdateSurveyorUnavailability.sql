IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetAndUpdateSurveyorUnavailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetAndUpdateSurveyorUnavailability]
GO

	-- =============================================
-- Author		: Yuga Hatolkar
-- Created On	: 18th May, 2015
-- Description	: Get surveyor Unavailable Days and Slots.
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetAndUpdateSurveyorUnavailability] --EXEC Absure_GetAndUpdateSurveyorUnavailability 13240, NULL

	@SurveyorId BIGINT = NULL,
	@Id INT = NULL
	
AS
BEGIN
	SET NOCOUNT OFF;
    
	IF @Id IS NULL
		SELECT Id, CONVERT(CHAR(15), UnavailableDate, 106) AS UnavailableDate, SlotId, UpdatedDate 
		FROM AbSure_SurveyorUnavailabilityDetails 
		WHERE (@SurveyorId IS NULL OR SurveyorId = @SurveyorId) AND ISNULL(IsDenied,0) = 0

	ELSE
		UPDATE	AbSure_SurveyorUnavailabilityDetails 
		SET		IsDenied = 1 
		WHERE	Id = @Id

END