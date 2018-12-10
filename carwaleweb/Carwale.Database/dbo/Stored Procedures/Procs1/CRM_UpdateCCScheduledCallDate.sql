IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateCCScheduledCallDate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateCCScheduledCallDate]
GO

	CREATE PROCEDURE [dbo].[CRM_UpdateCCScheduledCallDate]

	@LeadId				NUMERIC,
	@ScheduleDate		DATETIME	
 AS
	
BEGIN
		DECLARE @CallId NUMERIC
		
		SELECT @CallId = CallId FROM CRM_CallActiveList WITH(NOLOCK) WHERE LeadId = @LeadId AND CallType = 3
		IF @@RowCount = 1
			BEGIN
				UPDATE CRM_Calls SET ScheduledOn = @ScheduleDate WHERE Id = @CallId
				UPDATE CRM_CallActiveList SET ScheduledOn = @ScheduleDate WHERE CallId = @CallId
			END
	
END


