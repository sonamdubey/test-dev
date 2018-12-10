IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_InactivateOPRUserMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_InactivateOPRUserMapping]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(12th May 2015)
-- Description	:	Used to inactivate OPR User Mapping
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_InactivateOPRUserMapping]
	
	@UserIds	VARCHAR(100),
	@SuspendedBy	INT,
	@IsSuspended	BIT OUTPUT

AS
BEGIN
	
	SET @IsSuspended = 0

	UPDATE 
		DCRM_ADM_MappedUsers 
	SET
		IsActive = 0,
		SuspendedBy = @SuspendedBy,
		SuspendedOn = GETDATE()
	WHERE
		Id IN (SELECT *FROM fnSplitCSV(@UserIds))
	IF @@ROWCOUNT > 0
		SET @IsSuspended = 1 

END

