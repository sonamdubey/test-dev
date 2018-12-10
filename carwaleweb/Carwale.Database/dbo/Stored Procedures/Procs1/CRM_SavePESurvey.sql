IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SavePESurvey]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SavePESurvey]
GO

	CREATE PROCEDURE [dbo].[CRM_SavePESurvey]
	@Id					NUMERIC (18,0),
	@CBDId				NUMERIC(18,0) = NULL,
	@UserId				NUMERIC(18,0) = NULL,
	@CWExperience		SMALLINT = NULL,
	@CTServive			SMALLINT = NULL,
	@ConsultantAble		SMALLINT = NULL,
	@ConsultantAnswer	SMALLINT = NULL,
	@ConsultantAskedTD	SMALLINT = NULL,
	@CWRecommend		SMALLINT = NULL,
	@Comments			VARCHAR(500) = NULL,
	@IsCompleted		BIT = 0,
	@CurrentId			NUMERIC OUTPUT
--@FBDate datetime 


AS
	BEGIN
		IF(@Id != -1)
			BEGIN
				SET @IsCompleted = 1
				--UPDATE STATMENT
				UPDATE CRM_PESurvey SET 
						CWExperience = @CWExperience, CTServive = @CTServive,
						ConsultantAble = @ConsultantAble, ConsultantAnswer = @ConsultantAnswer,
						ConsultantAskedTD = @ConsultantAskedTD,	CWRecommend = @CWRecommend, Comments = @Comments,
						IsCompleted = @IsCompleted, UpdatedOn = GETDATE()
				WHERE Id = @Id
				
				SET @CurrentId = @Id
			END
		ELSE
			BEGIN
				--INSERT STATMENT
				INSERT INTO CRM_PESurvey(CBDId, UserId)
				VALUES(@CBDId, @UserId)
				
				SET @CurrentId = SCOPE_IDENTITY()
			END

END


