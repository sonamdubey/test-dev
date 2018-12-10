IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveUpdateRole]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveUpdateRole]
GO

	
CREATE PROCEDURE [dbo].[DCRM_SaveUpdateRole]
	@UserId			NUMERIC,
	@RoleId		    VARCHAR(30),
	@UpdatedOn		DATETIME,
	@UpdatedBy		NUMERIC,
	@Status			INT OUTPUT 
AS

BEGIN
		INSERT INTO DCRM_ADM_UserRoles
		(
			UserId, RoleId, IsActive, UpdatedOn, UpdatedBy
		) 
		VALUES
		( 
			@UserId, @RoleId, 1, @UpdatedOn, @UpdatedBy
		)

		SET @Status = 1 
			

END