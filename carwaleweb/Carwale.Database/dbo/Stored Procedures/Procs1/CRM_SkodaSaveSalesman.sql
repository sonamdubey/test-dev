IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SkodaSaveSalesman]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SkodaSaveSalesman]
GO

	
CREATE PROCEDURE [dbo].[CRM_SkodaSaveSalesman]

	@TokenNo			VarChar(50),
	@AgencyId			VarChar(50),
	@AgencyLeadId		VarChar(50),
	@CompanyId			VarChar(50),
	@DMSLeadId			VarChar(50),
	@DMSLeadOpenedOn	VarChar(50),
	@TokenRecProcessOwner	VarChar(200),
	@SalesMan			VarChar(200),
	@FromDate			VarChar(50),
	@ToDate				VarChar(50),
	@Comments			VarChar(500),
	@Status				NUMERIC OutPut	
				
 AS
	
BEGIN
		INSERT INTO CRM_SkodaLeadsSalesman
		(
			TokenNo, AgencyId, AgencyLeadId, CompanyId, DMSLeadId,
			DMSLeadOpenedOn, TokenRecProcessOwner, SalesMan, FromDate,
			ToDate, Comments
		)
		VALUES
		(
			@TokenNo, @AgencyId, @AgencyLeadId, @CompanyId, @DMSLeadId,
			@DMSLeadOpenedOn, @TokenRecProcessOwner, @SalesMan, @FromDate,
			@ToDate, @Comments
		)
			
		SET @Status = SCOPE_IDENTITY()
END













