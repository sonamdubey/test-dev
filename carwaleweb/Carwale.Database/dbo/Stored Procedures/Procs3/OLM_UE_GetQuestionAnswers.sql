IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_UE_GetQuestionAnswers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_UE_GetQuestionAnswers]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 19-july-13
-- Description:	Fetch Questions along with answers for Unofficial Expert 2013
--EXEC OLM_UE_GetQuestionAnswers
-- =============================================
CREATE PROCEDURE [dbo].[OLM_UE_GetQuestionAnswers]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
    DECLARE @TempTable	TABLE (QuestionId INT, Question VARCHAR(500))
    
	--fetch random top 10 questions
	INSERT INTO @TempTable (QuestionId, Question)	
	SELECT TOP 8 Id AS QuestionId, Name AS Question FROM OLM_UE_Questions ORDER BY NEWID()
	
	SELECT QuestionId, Question FROM @TempTable
	
	SELECT UEAns.Id AS AnswerId, UEAns.QuestionId, UEAns.Answer
	FROM OLM_UE_Answers	UEAns
	JOIN @TempTable TT ON UEAns.QuestionId = TT.QuestionId
	ORDER BY NEWID()
END
