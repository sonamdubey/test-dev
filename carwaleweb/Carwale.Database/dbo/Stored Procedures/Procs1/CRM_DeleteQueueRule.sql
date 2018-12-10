IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DeleteQueueRule]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DeleteQueueRule]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 22 Nov 2013
-- Description:	Maintain a log of the deleted Queue Rules
-- =============================================
CREATE PROCEDURE [dbo].[CRM_DeleteQueueRule]
	-- Add the parameters for the stored procedure here
	@Id VARCHAR(MAX),
	@DeletedBy INT,
	@DeletedOn DATETIME
AS
 BEGIN
		INSERT INTO CRM_ADM_QueueRuleParamsLog (ID,QueueId,MakeId,ModelId,CityId,CreatedOn,SourceId,IsResearch,DealerId,DeletedBy,DeletedOn) 
		SELECT ID,QueueId,MakeId,ModelId,CityId,CreatedOn,SourceId,IsResearch,DealerId,@DeletedBy,@DeletedOn FROM CRM_ADM_QueueRuleParams WHERE ID IN (SELECT LISTMEMBER FROM fnSplitCSV(@Id))

		DELETE FROM CRM_ADM_QueueRuleParams WHERE ID IN (SELECT LISTMEMBER FROM fnSplitCSV(@Id))
END
