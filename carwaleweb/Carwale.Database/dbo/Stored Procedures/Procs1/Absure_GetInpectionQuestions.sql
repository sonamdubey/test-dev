IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetInpectionQuestions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetInpectionQuestions]
GO

	-- =============================================
-- Author:        Ruchira Patil
-- Create date: 12th Mar 2015
-- Description:    To fetch all the questions related to car inspection
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetInpectionQuestions]
AS
BEGIN
    SELECT AQ.AbSure_QuestionsId AS QuestionId,AQ.Question,AQ.AbSure_QCategoryId AS CategoryId,AQ.AbSure_QPositionId AS PositionId,AQ.Type,AQ.Weightage
    FROM AbSure_Questions AQ WITH(NOLOCK)
    WHERE AQ.IsActive=1
END
------------------------------------------------------------------------------------------------------------------------------------------------------
