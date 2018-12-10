IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_FAAddGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_FAAddGroup]
GO

	
CREATE PROCEDURE [dbo].[NCS_FAAddGroup]
	@Id					NUMERIC,
	@FAId				NUMERIC,
	@GroupName			VARCHAR(100),
	@Status				BIT OUTPUT
 AS
	
BEGIN
	IF @Id = -1 --Insertion
		BEGIN
			SELECT ID FROM NCS_FAGroups 
			WHERE FAId = @FAId AND GroupName = @GroupName

			IF @@ROWCOUNT = 0

				BEGIN

					INSERT INTO NCS_FAGroups
					(	FAId, GroupName, IsActive
					)	
				
					Values
					(	@FAId, @GroupName, 1
					)	

					SET @Status = 1

				END	

			ELSE
				
				SET @Status = 0
		END

	ELSE
		BEGIN
			
			UPDATE NCS_FAGroups
			SET GroupName = @GroupName
			WHERE Id = @Id
				
			SET @Status = 1
		END	
END




