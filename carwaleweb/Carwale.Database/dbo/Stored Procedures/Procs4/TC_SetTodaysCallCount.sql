IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SetTodaysCallCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SetTodaysCallCount]
GO

	
-- =============================================  
-- Author:  Manish  
-- Create date: 09-Jan-13 
-- Details: SP will update the todays call count of the user to zero value and shedule to run every EOD
-- =============================================  
CREATE PROCEDURE [dbo].[TC_SetTodaysCallCount]
AS
BEGIN

UPDATE TC_Users SET TodaysCallCount=0

END

