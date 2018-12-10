IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetSurveyorRatingOnParameter]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetSurveyorRatingOnParameter]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 16th April 2015
-- Description : Fetches dealers feedbacks of surveyor on particular parameter
-- Modifier 1: Vinay Kumar Prajapati 30th July 2015 get All cars which is related to surveyor
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetSurveyorRatingOnParameter]
	@SurveyorId INT
	--,@RatingCategoryId TINYINT
AS
BEGIN		
	SELECT  AR.Absure_RatingCategoryId AS RatingId,AIT.RatingValue,AIF.Comments,TU.UserName AS Surveyor,D.Organization AS Dealer,(ACD.Make +' '+ ACD.Model+ ' ' + ACD.Version) AS Car,AR.CategoryText,CONVERT(VARCHAR(11),ISNULL(AIF.EntryDate,GETDATE()),106) AS EntryDate
	,TU.Email AS LoginId, ISNULL(ACD.Id,-1) AS CarId,AIF.Absure_InspectionFeedbackId
	FROM Absure_InspectionFeedback AIF WITH(NOLOCK)
	INNER JOIN Absure_InspectionRating AIT WITH(NOLOCK) ON AIT.InspectionFeedbackId = AIF.Absure_InspectionFeedbackId
	INNER JOIN Absure_RatingCategory AR WITH(NOLOCK) ON AR.Absure_RatingCategoryId = AIT.RatingCategoryId
	INNER JOIN AbSure_CarDetails ACD WITH(NOLOCK) ON ACD.Id = AIF.AbSure_CarDetailsId
	INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = AIF.BranchId
	LEFT JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = AIF.SurveyorId AND TU.ID=@SurveyorId
	WHERE AIF.SurveyorId = @SurveyorId -- AND AIT.RatingCategoryId = @RatingCategoryId
	ORDER BY EntryDate DESC
END

