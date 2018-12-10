IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_SaveQueues]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_SaveQueues]
GO

	CREATE PROCEDURE [dbo].[CRM_ADM_SaveQueues]

	@Id				NUMERIC,
	@Name			VARCHAR(150),
	@Rank			INT,
	@IsActive		BIT,
	@AcceptNewLead	BIT,
	@UpdatedBy		NUMERIC,
	@Status			INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	IF @Id = -1
		BEGIN

			SELECT ID FROM CRM_ADM_Queues WITH(NOLOCK) WHERE Name = @Name
			
			IF @@RowCount = 0
				BEGIN
					INSERT INTO CRM_ADM_Queues
					(
						Name, Rank, IsActive, AcceptNewLead, CreatedOn, UpdatedOn, UpdatedBy
					) 
					VALUES
					( 
						@Name, @Rank, @IsActive, @AcceptNewLead, GETDATE(), GETDATE(), @UpdatedBy
					)
		
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
	ELSE
		BEGIN

			SELECT ID FROM CRM_ADM_Queues WITH(NOLOCK) WHERE Name = @Name AND  ID <> @Id
			
			IF @@RowCount = 0

				BEGIN

					UPDATE CRM_ADM_Queues SET 
						Name = @Name, IsActive = @IsActive,
						AcceptNewLead = @AcceptNewLead, Rank = @Rank,
						UpdatedOn = GETDATE(), UpdatedBy = @UpdatedBy
					 WHERE ID = @Id
				
					SET @Status = 1 

				END
			ELSE
				SET @Status = 0
		END
END


