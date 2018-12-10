IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_AbuseReviewComment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_AbuseReviewComment]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 3/Jul/2008
-- Description:	SP to abuse the comments on review on accessories items
-- =============================================
CREATE PROCEDURE [dbo].[Acc_AbuseReviewComment] 
	-- Add the parameters for the stored procedure here
	@CommentId	NUMERIC
AS
BEGIN
	UPDATE Acc_ReviewComments SET ReportAbuse = 1 WHERE Id = @CommentId
END

