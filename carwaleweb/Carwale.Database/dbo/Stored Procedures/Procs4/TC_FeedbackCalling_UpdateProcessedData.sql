IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FeedbackCalling_UpdateProcessedData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FeedbackCalling_UpdateProcessedData]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 03 Oct, 2016
-- Description : To Update processed data from feedback calling queue
-- =============================================
CREATE PROCEDURE [dbo].[TC_FeedbackCalling_UpdateProcessedData]
	-- Add the parameters for the stored procedure here
	@TC_FeedBackCalling_PushedDataId INT
AS
BEGIN

	UPDATE TC_FeedBackCalling_PushedData 
	SET IsProcessed = 1,ModifiedDate = GETDATE()
	WHERE Id = @TC_FeedBackCalling_PushedDataId;
	
END
