IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveFeedbackQuestion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveFeedbackQuestion]
GO

	
CREATE PROCEDURE [dbo].[CRM_SaveFeedbackQuestion]  
--Name of SP/Function                    : CRM_SaveFeedbackQuestion
--Applications using SP                  : CRM
--Modules using the SP                   : AddFeedbackQuestion.cs
--Technical department                   : Database
--Summary                                : Add question to CRM_FeedbackQuestions
--Author                                 : AMIT Kumar 06-June-2013
--Modification history                   : 1
@SaveType				INT,		-- 1 = Question, 2= Option
@Question				VARCHAR(500) = null,  
@FeedbackType			INT = null,   
@Alias					VARCHAR(500) = null,  
@IsActive				BIT = null,  
@IsCommentType			BIT = null,  
@AnswerType				INT = null,  
@IsDependent			BIT = null,  
@IsMandatory			BIT = null,  
@QuestionOrder			INT = null,
@CreatedBy				NUMERIC(18,0) = null,
@UpdatedBy				NUMERIC(18,0) = null,
@UpdatedOn				DATETIME = null,
@QuestionId				NUMERIC(18,0) = null,
@Answer					VARCHAR(300) = null,
@Rating					VARCHAR(7)= '#63BE7B',
@NewQuestionId			NUMERIC(18,0) OUT
  
AS   
BEGIN 
	IF(@SaveType= 1) 
		BEGIN
			INSERT INTO   
				CRM_FeedbackQuestions   
				(Question,FeedbackType,Alias,IsActive,IsCommentType,AnswerType,IsDependent,QuestionOrder,IsMandatory,CreatedBy,UpdatedBy,UpdatedOn)  
			VALUES
				(@Question,@FeedbackType,@Alias,@IsActive,@IsCommentType,@AnswerType,@IsDependent,@QuestionOrder,@IsMandatory,@CreatedBy,@UpdatedBy,@UpdatedOn)  
			SET @NewQuestionId  = SCOPE_IDENTITY()
		END
		
	IF(@SaveType=2)
		BEGIN
			INSERT INTO   
				CRM_FeedbackAnswers   
				(QuestionId,Answer,Alias,Rating,IsActive,CreatedBy,UpdatedBy,UpdatedOn)  
			VALUES
				(@QuestionId,@Answer,@Alias,@Rating,@IsActive,@CreatedBy,@UpdatedBy,GETDATE())  
			SET @NewQuestionId  = -1	
		END
END