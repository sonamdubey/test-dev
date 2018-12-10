IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_SaveCallTypes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_SaveCallTypes]
GO

	
-- PROCEDURE TO SAVE ASSIGNED TASKS TO THE TALECALLERS IN TABLE 'CH_TCAssignedTasks'

CREATE PROCEDURE [dbo].[CH_SaveCallTypes]
	@Id			AS 	NUMERIC,
	@TBCType		AS  	SMALLINT,
	@CallTypeName	AS 	VARCHAR(50),
	@Priority		AS 	SMALLINT
	
AS
	
	DECLARE @CallTypeId  SMALLINT
BEGIN
	
	SELECT @CallTypeId = IsNULL(Max(CallId) , 0) + 1  FROM CH_CallTypes WHERE TBCType = @TBCType 
	
	IF @Id = -1
		BEGIN
			INSERT INTO CH_CallTypes(TBCType, CallId, Name, Priority) VALUES(@TBCType, @CallTypeId, @CallTypeName, @Priority)
		END
	ELSE
		BEGIN
			UPDATE CH_CallTypes SET TBCType = @TBCType, CallId = @CallTypeId,  Name =  @CallTypeName, Priority = @Priority
			WHERE Id = @Id
		END
END
