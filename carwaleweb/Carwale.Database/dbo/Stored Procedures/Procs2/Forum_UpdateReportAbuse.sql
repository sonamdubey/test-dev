IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Forum_UpdateReportAbuse]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Forum_UpdateReportAbuse]
GO

	CREATE Procedure [dbo].[Forum_UpdateReportAbuse]

@postId NUMERIC

AS
	
	BEGIN
		DELETE FROM Forum_ReportAbuse 
		WHERE PostId=@postId

		UPDATE ForumThreads
		SET IsActive = 0 WHERE ID =@postId
	END