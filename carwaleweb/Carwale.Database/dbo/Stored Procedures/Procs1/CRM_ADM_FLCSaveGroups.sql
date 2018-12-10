IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_FLCSaveGroups]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_FLCSaveGroups]
GO

	
CREATE PROCEDURE [dbo].[CRM_ADM_FLCSaveGroups]
	@Id					INT,
	@Name				VARCHAR(250),
	@GroupType			SMALLINT,
	@LeadScoreLimit     DECIMAL(18,2),
	@Status             SMALLINT OUTPUT
	 
AS
	BEGIN
		IF(@Id != -1)
			BEGIN
			    SELECT CAF.Id FROM CRM_ADM_FLCGroups AS CAF WITH(NOLOCK) WHERE CAF.GroupType=@GroupType AND CAF.Name=@Name AND CAF.Id <> @Id
			   IF @@ROWCOUNT <> 0
					BEGIN
						SET @Status = 0
					END
                ELSE
				BEGIN
					--UPDATE STATMENT
					UPDATE CRM_ADM_FLCGroups SET 
							Name = @Name, GroupType = @GroupType, LeadScoreLimit=@LeadScoreLimit
					WHERE Id = @Id

					SET @Status=1	
				END			
			END
		ELSE
			BEGIN
			 SELECT CAF.Id FROM CRM_ADM_FLCGroups AS CAF WITH(NOLOCK) WHERE CAF.GroupType=@GroupType AND CAF.Name=@Name
			   IF @@ROWCOUNT <> 0
					BEGIN
					   SET @Status = 0
					END
			    ELSE
					BEGIN				
						--INSERT STATMENT
						INSERT INTO CRM_ADM_FLCGroups(Name, GroupType,LeadScoreLimit)
						VALUES(@Name, @GroupType,@LeadScoreLimit)
						SET @Status=1				
					END
				
			 END
    END




