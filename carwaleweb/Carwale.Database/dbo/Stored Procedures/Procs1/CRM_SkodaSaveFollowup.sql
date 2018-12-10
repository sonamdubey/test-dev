IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SkodaSaveFollowup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SkodaSaveFollowup]
GO

	

CREATE PROCEDURE [dbo].[CRM_SkodaSaveFollowup]

	@TokenNo			VarChar(50),
	@AgencyId			VarChar(50),
	@AgencyLeadId		VarChar(50),
	@CompanyId			VarChar(50),
	@DMSLeadId			VarChar(50),
	@DMSLeadOpenedOn	VarChar(50),
	@TokenRecProcessOwner	VarChar(200),
	@Action				VarChar(50),
	@ActionDate			VarChar(50),
	@ActionTime			VarChar(50),
	@ActionComment		VarChar(500),
	@ActionClosureDate	VarChar(50),
	@ActionClosureTime	VarChar(50),
	@ActionClosureComment	VarChar(500),
	@Status					NUMERIC OutPut	
				
 AS
	
BEGIN
		INSERT INTO CRM_SkodaLeadsFollowup
		(
			TokenNo, AgencyId, AgencyLeadId, CompanyId, DMSLeadId,
			DMSLeadOpenedOn, TokenRecProcessOwner, Action, ActionDate,
			ActionTime, ActionComment, ActionClosureDate, ActionClosureTime,
			ActionClosureComment
		)
		VALUES
		(
			@TokenNo, @AgencyId, @AgencyLeadId, @CompanyId, @DMSLeadId,
			@DMSLeadOpenedOn, @TokenRecProcessOwner, @Action, @ActionDate,
			@ActionTime, @ActionComment,@ActionClosureDate, @ActionClosureTime,
			@ActionClosureComment
		)
			
		SET @Status = SCOPE_IDENTITY()	
END














