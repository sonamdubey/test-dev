IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FeedBackNCDInquriySaveData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FeedBackNCDInquriySaveData]
GO

	
CREATE PROCEDURE [dbo].[CRM_FeedBackNCDInquriySaveData]( @CRM_FeedBackSave CRM_FeedBackSave READONLY, @CBDId bigint , @itemType INT,@feedbackType INT)
--Created By Amit Kumar to save feedbackanswer for NCDInquiries on 18th dec 2012
--modified by Amit Kumar on 29th jan 2013 (Added QuestionId)
--modified by Amit Kumar april 2013 (Added @feedbackType INT)
AS
BEGIN
	IF EXISTS (SELECT  CBDId from CRM_Feedback WHERE CBDId = @CBDId AND ItemType = @itemType AND QuestionId IN(SELECT Id FROM CRM_FeedbackQuestions WHERE FeedbackType = @feedbackType))
		BEGIN
			DELETE FROM CRM_Feedback WHERE CBDId = @CBDId AND ItemType = @itemType AND QuestionId IN(SELECT Id FROM CRM_FeedbackQuestions WHERE FeedbackType = @feedbackType)
			INSERT INTO CRM_Feedback
					(CBDId,
					 AnswerId,                        
					 FBDate,
					 UpdatedOn,
					 UpdatedBy,
					 Comments,
					 ItemType,
					 QuestionId
					 )
			SELECT CP.CBDId,
				   CP.AnswerId,
				   CP.FBDate,
				   CP.UpdatedOn,
				   CP.UpdatedBy,
				   CP.Comments,
				   CP.ItemType,
				   CP.QuestionId           
		   FROM @CRM_FeedBackSave CP WHERE  CP.AnswerId <> -1 AND  CP.AnswerId <> 0 
		END 
	ELSE
	BEGIN
		INSERT INTO CRM_Feedback
					(CBDId,
					 AnswerId,                        
					 FBDate,
					 UpdatedOn,
					 UpdatedBy,
					 Comments,
					 ItemType,
					 QuestionId
					 )
		SELECT CP.CBDId,
			   CP.AnswerId,
			   CP.FBDate,
			   CP.UpdatedOn,
			   CP.UpdatedBy,
			   CP.Comments,
			   CP.ItemType,
			   CP.QuestionId           
	   FROM @CRM_FeedBackSave CP WHERE  CP.AnswerId <> -1 AND  CP.AnswerId <> 0 
   END
 END

