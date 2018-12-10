IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UCDFeedBackSaveData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UCDFeedBackSaveData]
GO

	
CREATE PROCEDURE [dbo].[DCRM_UCDFeedBackSaveData]( @CRM_FeedBackSave DCRM_UCDFeedBackSave READONLY, @customerId bigint, @dealerId bigint)
--Created By Amit Kumar to save feedbackanswer for CRM_UCDFeedback on 16th march 2013
-- changed name of the table from CRM_UCDFeedback to DCRM_UCDFeedback
-- Modifier : 1 Amit kumar on 16th may 2013(put constrain on blank ccomments)

AS
BEGIN
	DECLARE @UpdatedBy  NUMERIC(18,0)
	DECLARE @FBScheduledDate DATETIME
	DECLARE @FBSubmitDate DATETIME
	SELECT Top 1 @UpdatedBy = UpdatedBy, @FBScheduledDate = FBScheduledDate,  @FBSubmitDate = FBSubmitDate FROM  @CRM_FeedBackSave

	IF EXISTS (SELECT  DealerId from DCRM_UCDFeedback WHERE CustomerId = @customerId AND DealerId = @dealerId AND CONVERT(VarChar, FBScheduledDate,101)  = CONVERT(VarChar, @FBScheduledDate,101)   AND CONVERT(VarChar, FBSubmitDate,101) = CONVERT(VarChar, @FBSubmitDate,101))
		BEGIN
			DELETE FROM DCRM_UCDFeedback WHERE CustomerId = @customerId AND DealerId = @dealerId AND CONVERT(VarChar, FBScheduledDate,101)  = CONVERT(VarChar, @FBScheduledDate,101)   AND CONVERT(VarChar, FBSubmitDate,101) = CONVERT(VarChar, @FBSubmitDate,101)
			INSERT INTO DCRM_UCDFeedback
					(CustomerId,
					 AnswerId,                        
					 FBSubmitDate,
					 UpdatedOn,
					 UpdatedBy,
					 Comments,
					 QuestionId,
					 DealerId,
					 FBScheduledDate,
					 CustomerCallingId
					 )
			SELECT CP.CustomerId,
				   CP.AnswerId,
				   CP.FBSubmitDate,
				   CP.UpdatedOn,
				   CP.UpdatedBy,
				   CP.Comments,
				   CP.QuestionId,
				   CP.DealerId,
				   CP.FBScheduledDate,
				   CP.CustomerCallingId
		   FROM @CRM_FeedBackSave CP WHERE CP.AnswerId <> -1 AND CP.AnswerId <> 0 AND CP.Comments <> '-1'
		END 
	ELSE
	BEGIN
		INSERT INTO DCRM_UCDFeedback
					(CustomerId,
					 AnswerId,                        
					 FBSubmitDate,
					 UpdatedOn,
					 UpdatedBy,
					 Comments,
					 QuestionId,
					 DealerId,
					 FBScheduledDate,
					 CustomerCallingId
					 )
			SELECT CP.CustomerId,
				   CP.AnswerId,
				   CP.FBSubmitDate,
				   CP.UpdatedOn,
				   CP.UpdatedBy,
				   CP.Comments,
				   CP.QuestionId,
				   CP.DealerId,
				   CP.FBScheduledDate,
				   CP.CustomerCallingId
		   FROM @CRM_FeedBackSave CP WHERE CP.AnswerId <> -1 AND CP.AnswerId <> 0 AND CP.Comments <> '-1'
   END
   UPDATE DCRM_CustomerCalling  SET ActionID = 1, ActionTakenBy = @UpdatedBy , ActionTakenOn = GETDATE() WHERE CustomerId = @CustomerId --AND DAY(InquiryDate) = DAY(@InquiryDate) AND MONTH(InquiryDate) = MONTH(@InquiryDate) AND YEAR(InquiryDate) = YEAR(@InquiryDate) 
 END
