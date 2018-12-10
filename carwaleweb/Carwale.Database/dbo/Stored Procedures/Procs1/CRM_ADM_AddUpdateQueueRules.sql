IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_AddUpdateQueueRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_AddUpdateQueueRules]
GO

	CREATE Procedure [dbo].[CRM_ADM_AddUpdateQueueRules]

@QueueId NUMERIC,
@MakeId VARCHAR(5000),
@CityId VARCHAR(5000),
@Status int OUTPUT

AS
	
BEGIN
		if EXISTS (select QueueId from CRM_ADMQueueRules
			where QueueId=@QueueId)
				BEGIN
					SET @Status = 0
					Update CRM_ADMQueueRules 
					Set MakeId = @MakeId , CityId = @CityId
					Where QueueId = @QueueId
					SET @Status = 1
					
				END
				ELSE
				BEGIN
					SET @Status = 0

					INSERT INTO CRM_ADMQueueRules (QueueId,MakeId,CityId) 
					VALUES (@QueueId,@MakeId,@CityId)
					SET @Status = 1
				END
END
