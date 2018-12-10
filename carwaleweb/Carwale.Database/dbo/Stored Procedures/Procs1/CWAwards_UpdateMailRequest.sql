IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CWAwards_UpdateMailRequest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CWAwards_UpdateMailRequest]
GO

	
-- =============================================
-- Author:		Vaibhav Kale
-- Create date: 25-Jan-2013
-- Description:	To update mail request(subscription) to true for the specific survey
-- =============================================
CREATE PROCEDURE [dbo].[CWAwards_UpdateMailRequest]
	-- Add the parameters for the stored procedure here
	@SurveyId		NUMERIC,
	@Status			SMALLINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
    SET @Status = -1
	
	IF @SurveyId <> -1
		BEGIN
			UPDATE CWAwards
				SET MailRequired = 1
			WHERE SurveyId = @SurveyId
		END
	SET @STatus = 1
END

