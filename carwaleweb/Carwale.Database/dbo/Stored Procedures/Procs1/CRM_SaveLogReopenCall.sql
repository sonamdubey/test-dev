IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveLogReopenCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveLogReopenCall]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveLogReopenCall]
	
	@CallId			Numeric,
	@CallType		Int,
	@LeadId			Numeric,
	@TeamId			Numeric,
	@ReopenedBy		Numeric,
	@Subject		VarChar(500),
	@ReopenDate		DateTime,
	@CurrentId		Numeric OutPut
				
 AS
	
BEGIN
	SET @currentId = -1
		
	INSERT INTO CRM_CallReopenLog
	(
		CallId, CallType, LeadId, TeamId,
		ReopenedBy, Subject, ReopenDate
	)
	VALUES
	(
		@CallId, @CallType, @LeadId, @TeamId, 
		@ReopenedBy, @Subject, @ReopenDate
	)

	SET @CurrentId = Scope_Identity()

END












