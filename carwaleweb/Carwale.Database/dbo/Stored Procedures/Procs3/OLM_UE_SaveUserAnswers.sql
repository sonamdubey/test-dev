IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_UE_SaveUserAnswers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_UE_SaveUserAnswers]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 20-july-2013
-- Description:	Save answers for the unofficial expert 2013
--				If user already submitted answers then delete previous records and add new
-- =============================================
CREATE PROCEDURE [dbo].[OLM_UE_SaveUserAnswers]
	-- Add the parameters for the stored procedure here
	@UserId			NUMERIC,
	@AnswerIds		VARCHAR(100),
	@LastUpdatedOn	DATETIME,
	@Score			SMALLINT OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
    SET @Score = 0
    
    --Pass only if user id is not -1
    IF @UserId <> -1
		BEGIN
			--check whether record already exist for that userId or not			
			SELECT UserId FROM OLM_UE_UserAnswers WHERE UserId = @UserId
			IF @@ROWCOUNT <> 0
				BEGIN
					--If record exist for that userId then delete all previous records
					DELETE FROM OLM_UE_UserAnswers WHERE UserId = @UserId
				END
			
			--Insert the answers for the userId and lastUpdatedDate
			INSERT INTO OLM_UE_UserAnswers (UserId, AnswerId, LastUpdatedOn)
			SELECT @UserId, LISTMEMBER,@LastUpdatedOn FROM fnSplitCSV(@AnswerIds)
			
			--To get the score of the user(No. of right answers given by the user)
			SELECT @Score = COUNT(Id) FROM OLM_UE_Answers
			WHERE Id IN (SELECT ListMember FROM fnSplitCSV(@AnswerIds)) AND IsCorrect = 1
		END
END
