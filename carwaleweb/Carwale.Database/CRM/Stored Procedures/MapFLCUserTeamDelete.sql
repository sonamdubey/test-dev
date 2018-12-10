IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[MapFLCUserTeamDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[MapFLCUserTeamDelete]
GO

	




--Summary							: Delete Map FLC User Team
--Author							: Dilip V. 03-Oct-2012
--Modification history				: 1.

CREATE PROCEDURE [CRM].[MapFLCUserTeamDelete]
@Id VARCHAR(200),
@UpdatedBy	NUMERIC(18,0)
AS

BEGIN
	SET NOCOUNT ON
	
	UPDATE CRM.MapFLCUserTeam SET IsActive = 0, UpdatedBy = @UpdatedBy,UpdatedOn = GETDATE() WHERE Id IN (SELECT ListMember FROM fnSplitCSV(@Id))

END






