IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetESSurveyQuestions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetESSurveyQuestions]

	GO

-- ======================================================================
-- Author:		Supreksha Singh
-- Create date: 4th Oct 2016
-- Description:	to get to list of questions together with the answers
-- ======================================================================
CREATE PROCEDURE [dbo].[CW_GetESSurveyQuestions] 
	@CampaignId INT = NULL
AS
BEGIN
	SELECT Q.Id, Q.QuestionNumber, Q.Question,Q.ImageUrl
	FROM
	CW_ESSurveyQuestion Q WITH(NOLOCK)
	INNER JOIN CW_ESSurveyCampaigns C WITH(NOLOCK) ON Q.ESSurveyCampaignId=C.Id
	WHERE C.Id = @CampaignId 
	AND Q.IsActive = 1 
	AND GETDATE() BETWEEN c.StartDate AND c.EndDate

	SELECT O.Id, O.QuestionId, O.OptionValue, O.OptionNumber
	FROM
	CW_ESSurveyCampaigns C WITH(NOLOCK)
	INNER JOIN CW_ESSurveyQuestion Q WITH(NOLOCK) ON C.Id=Q.ESSurveyCampaignId
	INNER JOIN CW_ESSurveyOptions O WITH(NOLOCK) ON Q.Id=O.QuestionId
	WHERE O.IsActive = 1 
	AND C.Id = @CampaignId 
	AND Q.IsActive=1
	AND GETDATE() BETWEEN C.StartDate AND C.EndDate

	SELECT C.EndDate FROM CW_ESSurveyCampaigns C WITH(NOLOCK)
	WHERE  C.Id = @CampaignId
END
