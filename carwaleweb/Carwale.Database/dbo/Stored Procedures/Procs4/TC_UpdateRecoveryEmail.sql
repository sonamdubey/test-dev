IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateRecoveryEmail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateRecoveryEmail]
GO

	-- =============================================
-- Author	:	Sachin Bharti(9th June 2014)
-- Description	:	Update TC_Users recovery Email Id
-- Modifier	:	Sachin Bharti(6th June 2014)
-- Purpose	:	Remove condition for checking email Id
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateRecoveryEmail]
	@BranchId	NUMERIC,
	@RecoveryEmail	VARCHAR(50),
	@Id			NUMERIC,
	@Status		NUMERIC OUTPUT
AS
BEGIN
	SET @Status=0
	UPDATE TC_Users SET PwdRecoveryEmail=@RecoveryEmail WHERE Id=@Id AND BranchId=@BranchId
	IF @@ROWCOUNT <> 0
		SET @Status=1
END
