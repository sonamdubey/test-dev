IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PCheck]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PCheck]
GO

	

-- =============================================
-- Author:		Deepak
-- Create date: 14-05-2015
-- Description:	Absure Tracker
-- =============================================
CREATE PROCEDURE [dbo].[PCheck]
	@SqlQuery AS VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @StartTime datetime,@EndTime datetime   
	SELECT @StartTime=GETDATE() 
	EXEC (@SqlQuery)
	SELECT @EndTime=GETDATE()   
	SELECT DATEDIFF(ms,@StartTime,@EndTime) AS [Duration in milliseconds]

END


