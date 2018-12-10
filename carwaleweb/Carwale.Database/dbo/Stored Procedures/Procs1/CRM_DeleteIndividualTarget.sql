IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DeleteIndividualTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DeleteIndividualTarget]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 29th July 2014
-- Description:	To update the isdeleted field of CRM_TargetLog AND delete the row from CRM_IndividualTarget for that user
-- =============================================
CREATE PROCEDURE [dbo].[CRM_DeleteIndividualTarget] 
	@Id	VARCHAR(MAX),
	@DeletedBy INT,
	@DeletedOn DATETIME
AS
BEGIN

	UPDATE CRM_TargetLog SET IsDeleted = 1,ActionTakenBy=@DeletedBy,ActionTakenOn=@DeletedOn 
	WHERE UserId IN (SELECT ListMember FROM fnSplitCSV(@Id)) AND Date = CAST(GETDATE() AS DATE)

	DELETE FROM CRM_IndividualTarget WHERE UserId IN (SELECT ListMember FROM fnSplitCSV(@Id))

END

