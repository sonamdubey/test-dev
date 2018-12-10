IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SkodaSaveClosedLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SkodaSaveClosedLead]
GO

	
CREATE PROCEDURE [dbo].[CRM_SkodaSaveClosedLead]

	@TokenNo			    VarChar(50),
	@AgencyId			    VarChar(50),
	@AgencyLeadId		    VarChar(50),
	@TokenRecProcessOwner	VarChar(200),
	@CompanyId				VarChar(50),
	@DMSLeadId				VarChar(50),
	@DMSClosureDate			VarChar(50),
	@DMSClosureType			VarChar(50),
	@OrderNo				VarChar(50),
	@Status					NUMERIC = 0 OutPut	
				
 AS
	
BEGIN
		INSERT INTO CRM_SkodaClosedLead
		(
			TokenNo, AgencyId, AgencyLeadId, TokenRecProcessOwner, CompanyId, DMSLeadId	,
		    DMSClosureDate, DMSClosureType, OrderNo
		)
		VALUES
		(
			@TokenNo, @AgencyId, @AgencyLeadId,@TokenRecProcessOwner, @CompanyId, @DMSLeadId,
			@DMSClosureDate, @DMSClosureType, @OrderNo
		)
			
		SET @Status = SCOPE_IDENTITY()	
END