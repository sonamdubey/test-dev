IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_SaveTeamMembers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_SaveTeamMembers]
GO

	CREATE PROCEDURE [dbo].[CRM_ADM_SaveTeamMembers]
	
	@IsSave		Bit,
	@TeamId		Numeric,
	@RoleId		Numeric,
	@UserId		Numeric,
	@CreatedOn	DateTime,
	@UpdatedOn	DateTime,
	@UpdatedBy	Numeric,
	@Status		Bit OutPut		
 AS
	
BEGIN
	SET @Status = 0

	SELECT Id FROM CRM_ADM_TeamMembers WHERE TeamId = @TeamId AND RoleId = @RoleId AND UserId = @UserId
			
	IF @@RowCount = 0

		BEGIN
			IF @IsSave = 1

				BEGIN
					INSERT INTO CRM_ADM_TeamMembers
					(
						TeamId, RoleId, UserId, CreatedOn, UpdatedOn, UpdatedBy
					) 
					VALUES
					( 
						@TeamId, @RoleId, @UserId, @CreatedOn, @UpdatedOn, @UpdatedBy
					)
	
					SET @Status = 1 
				END

			ELSE
				
				BEGIN
					SET @Status = 0 
				END
		END

	ELSE

		BEGIN
			IF @IsSave = 1

				BEGIN
					UPDATE CRM_ADM_TeamMembers SET UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy
					WHERE TeamId = @TeamId AND RoleId = @RoleId AND UserId = @UserId
					
					SET @Status = 1
				END

			ELSE

				BEGIN
					DELETE FROM CRM_ADM_TeamMembers 
					WHERE TeamId = @TeamId AND RoleId = @RoleId AND UserId = @UserId
					
					SET @Status = 1
				END
		END
END



