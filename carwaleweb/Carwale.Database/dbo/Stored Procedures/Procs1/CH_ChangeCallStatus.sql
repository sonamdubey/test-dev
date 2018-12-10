IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_ChangeCallStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_ChangeCallStatus]
GO

	

CREATE  PROCEDURE [dbo].[CH_ChangeCallStatus]

	@CallId		AS 	NUMERIC,
	@CallType	AS	NUMERIC
AS
	
BEGIN
	UPDATE CH_ScheduledCalls Set CallType = @CallType WHERE CallId = @CallId

	UPDATE CH_Calls SET CallType = @CallType WHERE Id = @CallId
END

