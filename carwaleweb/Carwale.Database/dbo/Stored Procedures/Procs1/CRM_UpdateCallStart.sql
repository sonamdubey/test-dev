IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateCallStart]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateCallStart]
GO

	

CREATE PROCEDURE [dbo].[CRM_UpdateCallStart]	
	@CallId  NUMERIC
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN		
		
		UPDATE CRM_Calls SET CallStartedOn = GETDATE() WHERE Id = @CallId AND IsActionTaken = 0
		
	END
	
END

