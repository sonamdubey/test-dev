IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ValuationFeedback_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ValuationFeedback_Insert]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 20/01/2011
-- Description:	Inserting Data in visitor feedback for carvaluation about how was their experience. 
 -- Modified by Shikhar on Aug 11, 2014. Changed size from 200 to 2000.
-- =============================================
CREATE PROCEDURE [dbo].[ValuationFeedback_Insert] 
	-- Add the parameters for the stored procedure here
	@UserName varchar(50),
	@Email varchar(50),
	@Feedback varchar(2000), -- Modified by Shikhar on Aug 11, 2014. Changed size from 200 to 2000.
	@UserIP varchar(50), 
	@FeedbackDateTime datetime,
	@FBSource nvarchar(200) = null,
	@FeedBackRating tinyint = null,
	@CarInfo nvarchar(200) = null,
	@FBSourceID tinyint = null,
	@ReportID numeric(18,0) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO VisitorFeedbacks(Name, Email, Feedback, UserIP, FeedbackDateTime, FBSource, FeedBackRating, CarInfo,FBSourceID,ReportID)
	VALUES(@UserName, @Email, @Feedback, @UserIP, @FeedbackDateTime, @FBSource, @FeedBackRating, @CarInfo,@FBSourceID,@ReportID)
END

