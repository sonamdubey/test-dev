IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DCLogCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DCLogCall]
GO

	

CREATE PROCEDURE [dbo].[CRM_DCLogCall]

	@CallId				NUMERIC,
	@DealerId			NUMERIC,
	@ActionComments		VARCHAR(5000),
	@ActionTakenId		NUMERIC,
	@UpdatedBy			NUMERIC
 AS
	
BEGIN
				
		--Update existing DC Call
		UPDATE CRM_DCCalls SET
			IsActionTaken = 1,	ActionTakenId = @ActionTakenId, ActionTakenOn = GETDATE(), 
			ActionComments = @ActionComments, 
			ActionTakenBy = @UpdatedBy
		WHERE Id = @CallId AND IsActionTaken = 0
				
		IF @@ROWCOUNT <> 0
		BEGIN
			DELETE FROM CRM_DCActiveCalls
			WHERE DealerId = @DealerId
		END	
END




