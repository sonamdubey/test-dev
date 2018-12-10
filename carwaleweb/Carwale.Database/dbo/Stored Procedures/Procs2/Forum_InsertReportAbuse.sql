IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Forum_InsertReportAbuse]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Forum_InsertReportAbuse]
GO

	CREATE procedure [dbo].[Forum_InsertReportAbuse]

@subId		NUMERIC,
@postId NUMERIC,
@customerId		NUMERIC,
@comment Varchar(100),
@createDate datetime

AS
	
	BEGIN
		INSERT INTO Forum_ReportAbuse 
		(ThreadId,PostId,CustomerId,Comment,CreateDate) 
		VALUES 
		(@subId,@postId,@customerId,@comment,@createDate)
	END