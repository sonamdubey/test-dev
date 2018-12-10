IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_SaveAssignedTasks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_SaveAssignedTasks]
GO

	
-- PROCEDURE TO SAVE ASSIGNED TASKS TO THE TALECALLERS IN TABLE 'CH_TCAssignedTasks'

CREATE PROCEDURE [dbo].[CH_SaveAssignedTasks]
	@TCId			AS  	NUMERIC,
	@TBCType		AS 	SMALLINT,
	@CallType		AS 	SMALLINT
	
AS
	
BEGIN
	SELECT TCID FROM CH_TCAssignedTasks WHERE TcId = @TCId AND TBCType = @TBCType AND CallType = @CallType
	
	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO CH_TCAssignedTasks(TCID, TBCType, CallType) VALUES(@TCId, @TBCType, @CallType)
		END
END
