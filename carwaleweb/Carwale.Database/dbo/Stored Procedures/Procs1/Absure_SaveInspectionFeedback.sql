IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_SaveInspectionFeedback]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_SaveInspectionFeedback]
GO

	--========================================================================
-- Author      : Suresh Prajapati
-- Created On  : 16th April, 2015
-- Summary     : To save Surveyor's inspection feedback given by dealer
--========================================================================
CREATE PROCEDURE [dbo].[Absure_SaveInspectionFeedback]
	-- Add the parameters for the stored procedure here
	@RatingResult dbo.Absure_RatingResult READONLY
	,@Comments VARCHAR(500) = NULL
	,@BranchId BIGINT
	,@Absure_CarDetailsId NUMERIC(18, 0)
	,@TC_UserId INT
	,@InspectionFeedbackId BIGINT = NULL
	,@Success INT = NULL OUTPUT
AS
BEGIN
	DECLARE @SurveyorId INT

	SELECT @SurveyorId = TC_UserId
	FROM AbSure_CarSurveyorMapping AS SM WITH (NOLOCK)
	INNER JOIN AbSure_CarDetails AS CD ON CD.Id = SM.AbSure_CarDetailsId
	WHERE CD.Id = @Absure_CarDetailsId

	INSERT INTO AbSure_InspectionFeedback (
		Comments
		,BranchId
		,Absure_CarDetailsId
		,TC_UserId
		,SurveyorId
		,EntryDate
		)
	VALUES (
		@Comments
		,@BranchId
		,@Absure_CarDetailsId
		,@TC_UserId
		,@SurveyorId
		,GETDATE()
		)

	SET @InspectionFeedbackId = SCOPE_IDENTITY()

	INSERT INTO Absure_InspectionRating (
		InspectionFeedbackId
		,RatingCategoryId
		,RatingValue
		)
	SELECT @InspectionFeedbackId
		,RatingId
		,RatingValue
	FROM @RatingResult

	SET @Success = SCOPE_IDENTITY()
END

