IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdatePEAlertCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdatePEAlertCall]
GO

	
CREATE PROCEDURE [dbo].[CRM_UpdatePEAlertCall]

	@leadId		Numeric,
	@TeamId		Numeric
			
 AS
		DECLARE @CallId Numeric
		
BEGIN

	SELECT @CallId = CallID FROM CRM_CallActiveList WITH(NOLOCK) 
	WHERE LeadId = @leadId AND CallerId = @TeamId AND IsTeam = 1 
			AND CallType = 3
			
	IF @@ROWCOUNT <> 0
		BEGIN
			UPDATE CRM_Calls SET AlertId = 2 WHERE Id = @CallId
			UPDATE CRM_CallActiveList SET AlertId = 2 WHERE CallId = @CallId
		END
END














