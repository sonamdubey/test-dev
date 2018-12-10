IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_AutomaticSurveyorAssignment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_AutomaticSurveyorAssignment]
GO

	-- ===========================================================================
-- Author      : Suresh Prajpati
-- Create date : 29th June, 2015
-- Description : Automatic surveyor assignment based on the owner car and area
-- EXEC AbSure_AutomaticSurveyorAssignment 156
-- Modifier 1 : Ruchira Patil (17th July 2015) -- added output parameter(set status = 1 only when lead is assigned to  the surveyor to send notification to him)
-- Modefied By : Vinay Kumar  11 aug 2015: Resolve Bug @AssignedUserId = Top 1 row selected from multiple rows
-- Modified By : Ashwini Dhamankar on Oct 23,2015 (Dealerwise automatic assignment)
-- ===========================================================================
CREATE PROCEDURE [dbo].[AbSure_AutomaticSurveyorAssignment] 
@AbSure_CarId INT,
@SurveyorId INT = NULL OUTPUT 
AS
BEGIN
	DECLARE @AreaId INT, @IsAgency BIT,@DealerId BIGINT

		--Get area and DealerId of requested lead
			SELECT @AreaId = OwnerAreaId, @DealerId = DealerId
			FROM AbSure_CarDetails WITH(NOLOCK)
			WHERE Id = @AbSure_CarId

		--check if any surveyor is mapped with Dealer
			SELECT TOP 1 @SurveyorId = TDSM.SurveyorId
			FROM TC_DealerSurveyorMapping TDSM WITH(NOLOCK)
			WHERE TDSM.DealerId = @DealerId AND TDSM.IsActive = 1

		--if any surveyor is not mapped with dealer
			IF(@SurveyorId IS NULL)
				BEGIN
				-- Check if surveyor exists in that area
					SELECT TOP 1 @SurveyorId = U.Id, @IsAgency = ISNULL(U.IsAgency, 0)
					FROM TC_Users AS U WITH (NOLOCK)
					INNER JOIN TC_UserAreaMapping AS AM WITH (NOLOCK) ON AM.TC_UserId = U.Id
					WHERE U.IsActive = 1
						AND AM.IsActive = 1
						AND AM.IsAssigned = 1
						AND AM.AreaId = @AreaId
					ORDER BY ISNULL(U.IsAgency, 0) ASC
				END
			
		IF(@SurveyorId IS NOT NULL)
		BEGIN
			--If Surveyor exists then assign lead to that surveyor 
			EXEC Absure_SaveCarSurveyorMapping @SurveyorId
				,@AbSure_CarId
				,NULL
				,NULL
				,NULL
				,NULL
				,NULL
		END
		
END

-------------------

