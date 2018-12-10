IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DeleteFLCRule]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DeleteFLCRule]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 06 Jan 2014
-- Description:	Maintain a log of the deleted FLC Rules
-- =============================================
CREATE PROCEDURE [dbo].[CRM_DeleteFLCRule]
	@Id VARCHAR(MAX),
	@DeletedBy INT
AS
 BEGIN
		INSERT INTO CRM_ADM_AgencyLeadVerifierLog (UserId,Type,CreatedOn,CreatedBy,DeletedBy) 
		SELECT UserId,Type,CreatedOn,CreatedBy,@DeletedBy FROM CRM_ADM_AgencyLeadVerifier WHERE UserId IN (SELECT LISTMEMBER FROM fnSplitCSV(@Id))

		DELETE FROM CRM_ADM_AgencyLeadVerifier WHERE UserId IN (SELECT LISTMEMBER FROM fnSplitCSV(@Id))
END

