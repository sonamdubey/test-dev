IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveFeedbackData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveFeedbackData]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 30 Nov 20112
-- Description:	Saves Feedback about Skoda from Facebook feedback page into table OLM_FeedbackData
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SaveFeedbackData]
	-- Add the parameters for the stored procedure here
	@CustomerName	VARCHAR(50),
	@Email			varchar(50),
	@Mobile			varchar(15),
	@CarModel		NUMERIC(18,0),
	@CarRegNum		VARCHAR(15),
	@Feedback		VARCHAR(1000),
	@ClientIp		VARCHAR(20)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO OLM_FeedbackData
		(
			CustomerName, Email, Mobile, CarModel, CarRegNum, Feedback, ClientIp
		)
	VALUES
		(
			@CustomerName, @Email, @Mobile, @CarModel, @CarRegNum, @Feedback, @ClientIp			
		)
END

