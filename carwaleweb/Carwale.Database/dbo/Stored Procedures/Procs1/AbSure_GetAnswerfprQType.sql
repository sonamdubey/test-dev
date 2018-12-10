IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetAnswerfprQType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetAnswerfprQType]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 5th Mar 2015
-- Description:	Get AnswerValues for each question type
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetAnswerfprQType]
AS
BEGIN
	SELECT QuestionType,AnswerValue,Description,IsActive
	FROM Absure_AnswerTypes
END


-------------------------------------------------------------------


