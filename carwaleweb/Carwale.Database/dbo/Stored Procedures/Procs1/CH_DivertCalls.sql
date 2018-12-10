IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_DivertCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_DivertCalls]
GO

	

CREATE PROCEDURE [dbo].[CH_DivertCalls]
	
	@TCId			    AS NUMERIC,
	@CallId			    AS NUMERIC,
	@PrevTCId           AS NUMERIC  
	
AS

BEGIN
	
	BEGIN TRAN
		
		UPDATE CH_ScheduledCalls SET  TCID =  @TCId  WHERE CallId = @CallId
		
		UPDATE CH_Calls Set TCID = @TCId WHERE Id = @CallId
		
		UPDATE CH_TeleCallers Set ScheduledCalls = ScheduledCalls + 1 WHERE TcId = @TCId
		
		UPDATE CH_TeleCallers Set ScheduledCalls = ScheduledCalls - 1  WHERE ScheduledCalls >= 1 AND TcId = @PrevTCId  
	 
		UPDATE CH_TeleCallers SET IsNew = 1 WHERE TCID = @PrevTCId
		
	 COMMIT TRAN
END

