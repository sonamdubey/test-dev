IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_SaveQueueTeams]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_SaveQueueTeams]
GO

	CREATE PROCEDURE [dbo].[CRM_ADM_SaveQueueTeams]

	@QueueId	NUMERIC,
	@TeamId		NUMERIC,
	@Status		BIT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0
	
	SELECT QueueId FROM CRM_ADM_QueueTeams WHERE QueueId = @QueueId AND TeamId = @TeamId
			
		IF @@RowCount = 0
			BEGIN
				INSERT INTO CRM_ADM_QueueTeams
				(
					QueueId, TeamId
				) 
				VALUES
				( 
					@QueueId, @TeamId
				)
		
				SET @Status = 1 
			END
		ELSE
			SET @Status = 0
END

