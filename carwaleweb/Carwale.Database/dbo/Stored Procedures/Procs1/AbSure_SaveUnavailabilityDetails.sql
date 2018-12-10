IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveUnavailabilityDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveUnavailabilityDetails]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: May 21,2015
-- Description:	To save unavailable date and time slot of surveyor
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveUnavailabilityDetails]
	@MobileNo			VARCHAR(50),
	@UnavailabilityTbl  [dbo].[AbSure_SurveyorUnavailabilityDataTblTyp] READONLY,
	@StatusId           TINYINT  = NULL  OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @SurveyorId BIGINT

	SELECT	@SurveyorId = Id 
	FROM	TC_Users TU WITH(NOLOCK)
	WHERE   TU.Mobile = @MobileNo 

	INSERT INTO		AbSure_SurveyorUnavailabilityDetails (SurveyorId,UnavailableDate,SlotId,UpdatedDate)
	SELECT			@SurveyorId,UnavailableDate,
					CASE TimeSlot WHEN '9am-12pm' 
					THEN 1 
					WHEN '12pm-3pm'
					THEN 2
					WHEN '3pm-6pm' 
					THEN 3
					END,
					GETDATE() 
					FROM @UnavailabilityTbl

	SET @StatusId = 1
END
