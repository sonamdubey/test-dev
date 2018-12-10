IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveLeadReactivationLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveLeadReactivationLog]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveLeadReactivationLog]
	@LeadId				Numeric,
	@AssignedToTeam		Numeric,
	@ReactivatedBy		Numeric,
	@ReactivationDate	DateTime,
	@Comments			VarChar(500),
	@Reason				VarChar(500),
	@Status				Bit OutPut	
				
 AS
	
BEGIN
	SET @Status = 0
	
	INSERT INTO CRM_LeadReactivationLog
	(
		LeadId, AssignedToTeam, ReactivatedBy, ReactivationDate, Comments, Reason
	)
	VALUES
	(
		@LeadId, @AssignedToTeam, @ReactivatedBy, @ReactivationDate, @Comments, @Reason
	)
	
	SET @Status = 1
END
