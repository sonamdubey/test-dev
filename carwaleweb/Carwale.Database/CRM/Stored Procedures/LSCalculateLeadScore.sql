IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[LSCalculateLeadScore]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[LSCalculateLeadScore]
GO

	



-- Description	:	Update/Calculate the score of the lead as per the request
-- Author		:	Deepak Tripathi. 25-May-2012

CREATE PROCEDURE [CRM].[LSCalculateLeadScore]	
	@LeadId	NUMERIC,
	@LSCategory  INT,
	@SubCategoryId INT,
	@CategoryLeadScore FLOAT
AS
BEGIN
	DECLARE @Intercept FLOAT
	DECLARE @ActualIntercept FLOAT
	DECLARE @NewLeadScore FLOAT
	DECLARE @PrevLeadScore FLOAT
	DECLARE @SumAllCategoryScore FLOAT
	
	SET NOCOUNT ON;
	
	BEGIN		
		
		-- Get the Intercept value for lead scoring
		SELECT @Intercept = ConstantValue FROM CRM.LSConstantValues WITH (NOLOCK) WHERE ConstantFieldId = 1
			
		--Update Insert this value for against this lead in this category
		UPDATE  CRM.LSLeadCategoryScore SET LeadScore = @CategoryLeadScore, UpdatedOn = GETDATE()
		WHERE LeadId = @LeadId AND CategoryId = @LSCategory
									
		--Insert Data if no record exist previciously
		IF @@ROWCOUNT = 0
			BEGIN
				INSERT INTO CRM.LSLeadCategoryScore(CategoryId, LeadId, LeadScore, CreatedOn, UpdatedOn)
				VALUES(@LSCategory, @LeadId, @CategoryLeadScore, GETDATE(), GETDATE())
			END
									
		--Make Log 
		INSERT INTO CRM.LSLeadCategoryScoreLog(CategoryId, SubCategoryId, LeadId, LeadScore, CreatedOn)
		VALUES(@LSCategory, @SubCategoryId, @LeadId, @CategoryLeadScore, GETDATE())
																	
		--Get New the sum of all scores in each category against lead
		SELECT @SumAllCategoryScore = SUM(LeadScore) FROM CRM.LSLeadCategoryScore WITH (NOLOCK) WHERE LeadId = @LeadId
		
		--Calculate Overall Score against this lead
		SET @ActualIntercept = @SumAllCategoryScore + @Intercept
		SET @NewLeadScore = (EXP(@ActualIntercept)/(1+EXP(@ActualIntercept)))*100
					
		--Select Existing LeadScore
		SELECT @PrevLeadScore = LeadScore FROM CRM.LSLeadScore WITH (NOLOCK) WHERE LeadId = @LeadId
									 
		 IF @@ROWCOUNT = 0
			BEGIN
				SET @PrevLeadScore = 0
				--Insert lead score
				INSERT INTO CRM.LSLeadScore(LeadId, CreatedOn, UpdatedOn, LeadScore)
				VALUES(@LeadId, GETDATE(), GETDATE(), @NewLeadScore)
			END
		ELSE
			BEGIN
				--Update new score with the existing one
				UPDATE CRM.LSLeadScore SET LeadScore = @NewLeadScore, UpdatedOn = GETDATE() 
				WHERE leadId = @LeadId
			END
									
		--Maintain Log
		INSERT INTO CRM.LSScoreLog(LeadId, PreviousScore, NewScore, SubCategoryId, CreatedOn)
		VALUES(@LeadId, @PrevLeadScore, @NewLeadScore, @SubCategoryId, GETDATE())
	END		
END

