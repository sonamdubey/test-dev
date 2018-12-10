IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveDealerSurveyorMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveDealerSurveyorMapping]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 26th Oct 2015
-- Description:	to save dealer surveyor mapping
-- EXEC AbSure_SaveDealerSurveyorMapping 5,'8956214703',6
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveDealerSurveyorMapping]
	@DealerId INT,
	@AssignedSurveyor VARCHAR(15),
	@UserId INT
AS
BEGIN
	DECLARE @SurveyorId INT = NULL
	IF @AssignedSurveyor IS NOT NULL
		SELECT @SurveyorId = ID FROM TC_Users WITH (NOLOCK) WHERE Mobile = @AssignedSurveyor
	
	SELECT DealerId FROM TC_DealerSurveyorMapping WITH (NOLOCK) WHERE DealerId = @DealerId
	IF @@ROWCOUNT > 0
	BEGIN
		INSERT INTO TC_DealerSurveyorMappingLog(DealerId,SurveyorMobileNo,SurveyorId,CreatedOn,CreatedBy,EntryDateLog,IsActive,ModifiedBy,ModifiedDate)
		SELECT DealerId,SurveyorMobileNo,SurveyorId,GETDATE(),CreatedBy,EntryDate,IsActive,ModifiedBy,ModifiedDate FROM TC_DealerSurveyorMapping WITH (NOLOCK) WHERE DealerId = @DealerId

		IF @AssignedSurveyor IS NOT NULL
			UPDATE TC_DealerSurveyorMapping SET SurveyorMobileNo = @AssignedSurveyor,SurveyorId = @SurveyorId,ModifiedDate = GETDATE(),IsActive = 1,ModifiedBy = @UserId WHERE DealerId = @DealerId
		ELSE
			UPDATE TC_DealerSurveyorMapping SET SurveyorMobileNo = @AssignedSurveyor,SurveyorId = @SurveyorId,ModifiedDate = GETDATE(),IsActive = 0,ModifiedBy = @UserId WHERE DealerId = @DealerId
	END
	ELSE 
		INSERT INTO TC_DealerSurveyorMapping(DealerId,SurveyorMobileNo,SurveyorId,CreatedBy)
		VALUES (@DealerId,@AssignedSurveyor,@SurveyorId,@UserId)
END