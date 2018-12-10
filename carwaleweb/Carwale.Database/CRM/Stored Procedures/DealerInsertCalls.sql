IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[DealerInsertCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[DealerInsertCalls]
GO

	-- Description	:	Insert Status and Comment for Calls
-- Author		:	Dilip V. 20-Apr-2012
-- Modifier		:	
CREATE PROCEDURE [CRM].[DealerInsertCalls]

	@LeadId			NUMERIC,
	@CallType		INT,
	@IsTeam			BIT,
	@CallerId		NUMERIC,
	@Subject		VARCHAR(100),	
	@Comments		VARCHAR(500),
	@ActionTakenBy	NUMERIC,
	@AlertId		INT = NULL,
	@DealerId		NUMERIC
 AS
	
BEGIN
	
	INSERT INTO CRM_Calls
	(
		LeadId, CallType, IsTeam, CallerId,Subject, ScheduledOn,ActionTakenOn, IsActionTaken,ActionComments,ActionTakenBy, CreatedOn,  AlertId, DealerId
	) 
	VALUES
	( 
		@LeadId, @CallType, @IsTeam, @CallerId,@Subject, GETDATE(),GETDATE(), 1,@Comments,@ActionTakenBy, GETDATE(),  @AlertId, @DealerId
	)
				
END