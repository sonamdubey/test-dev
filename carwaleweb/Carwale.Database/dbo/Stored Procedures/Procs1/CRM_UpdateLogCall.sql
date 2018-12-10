IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateLogCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateLogCall]
GO

	CREATE PROCEDURE [dbo].[CRM_UpdateLogCall]

	@CallId				Numeric,
	@ActionTakenId		Numeric,
	@ActionTakenOn		DateTime,
	@ActionComments		VarChar(5000),
	@ActionTakenBy		Numeric,
	@DispType			SmallInt = -1,	
	@Status				Bit OutPut		
 AS
	
BEGIN
	SET @Status = 0
	
	IF @DispType = -1 OR @DispType = '' OR @DispType IS NULL OR @DispType = 0
		BEGIN
			UPDATE CRM_Calls SET
				IsActionTaken = 1,	ActionTakenId = @ActionTakenId, ActionTakenOn = @ActionTakenOn, 
				ActionComments = @ActionComments, ActionTakenBy = @ActionTakenBy
			WHERE Id = @CallId
		END
	ELSE
		BEGIN
			UPDATE CRM_Calls SET
				IsActionTaken = 1,	ActionTakenId = @ActionTakenId, ActionTakenOn = @ActionTakenOn, 
				ActionComments = @ActionComments, ActionTakenBy = @ActionTakenBy, DispType = @DispType
			WHERE Id = @CallId
		END
	
	IF @@ROWCOUNT <> 0
		BEGIN
			DELETE FROM CRM_CallActiveList WHERE CallId = @CallId
			SET @Status = 1
		END
				
END


