IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveCustomCallLogs]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveCustomCallLogs]
GO

	


CREATE PROCEDURE [dbo].[DCRM_SaveCustomCallLogs]
	@ProspectId			NUMERIC,
	@CallId				NUMERIC,
	@ContactNumber		VARCHAR(20),
	@SystemType			SMALLINT,
	@CreatedBy			NUMERIC,
	@Status				INT OUTPUT
	
AS
BEGIN
	
	INSERT INTO DCRM_CustomCallLog(ProspectId, ContactNumber, SystemType, CallId, CreatedBy) 
	VALUES(@ProspectId, @ContactNumber, @SystemType, @CallId, @CreatedBy)
	
	SET @Status = 1
END



