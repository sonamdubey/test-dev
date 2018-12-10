IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_DealerSubscription]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_DealerSubscription]
GO
	CREATE PROCEDURE [dbo].[WF_DealerSubscription] --106,107,1
(
	@NodeId  AS NUMERIC,
	@RevenueNodeId  AS NUMERIC,
	@ValueType AS SMALLINT,
	@Day AS BIGINT,
	@Month AS BIGINT,
	@Year AS BIGINT,
	@SyncDate AS DATETIME  
)
AS
BEGIN

DECLARE  @Number AS DECIMAL(18,0)
DECLARE  @Revenue AS DECIMAL(18,2)
 
	
	SELECT @Number = ISNULL(COUNT(CEL.ItemId),0)   
	FROM CRM_EventLogs AS CEL, CRM_CarBasicData AS CBD, CRM_Leads AS CL
	WHERE 
		CEL.ItemId = CBD.Id
		AND CBD.LeadId = CL.Id
		AND CL.Owner IN ( SELECT DISTINCT TeamId FROM CRM_ADM_QueueTeams WHERE QueueId IN(6,10,12) )
		AND DAY(CEL.EventOn) = @Day
		AND MONTH(CEL.EventOn) = @Month
		AND YEAR(CEL.EventOn) = @Year
		AND CEL.EventType = 16
		
	SELECT @Revenue = ISNULL(UnitValue,0)   
	FROM WF_NodeUnitValues 
	WHERE NodeId = @NodeId
		

	IF @Number <> 0 AND @Number <> 0.00  
	BEGIN
		SET  @Revenue = ( @Number * @Revenue ) 

		INSERT INTO WF_ActualValues( NodeId, ValueDate, Value, ValueType)  
							 VALUES( @NodeId, @SyncDate, @Number, @ValueType)  

		INSERT INTO WF_ActualValues( NodeId, ValueDate, Value, ValueType)  
							 VALUES( @RevenueNodeId, @SyncDate, @Revenue, @ValueType)  
							 
	END  


END
