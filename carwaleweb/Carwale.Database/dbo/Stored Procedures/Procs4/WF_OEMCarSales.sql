IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_OEMCarSales]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_OEMCarSales]
GO
	CREATE PROCEDURE [dbo].[WF_OEMCarSales] --108,109,15,1
(
	@NodeId  AS NUMERIC,
	@RevenueNodeId  AS NUMERIC,
	@MakeId  AS NUMERIC,   
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
	FROM CRM_EventLogs AS CEL, CRM_CarBasicData AS CBD, 
			CarVersions AS CV, CarModels AS CMO
	WHERE 
		CEL.ItemId = CBD.Id
		AND CBD.VersionId = CV.Id AND CV.CarModelId = CMO.Id AND CMO.CarMakeId = @MakeId 	
		AND DAY(CEL.EventOn) = @Day
		AND MONTH(CEL.EventOn) = @Month
		AND YEAR(CEL.EventOn) = @Year
		AND CEL.EventType = 16	
		
		
	IF @Number <> 0 AND @Number <> 0.00  
	BEGIN
	
		INSERT INTO WF_ActualValues( NodeId, ValueDate, Value, ValueType)  
		VALUES( @NodeId, @SyncDate, @Number, @ValueType) 
							 
		SELECT @Revenue = ISNULL(UnitValue,0)   
		FROM WF_NodeUnitValues 
		WHERE NodeId = @NodeId
		
		IF @Revenue <> 0 AND @Revenue <> 0.00
		BEGIN
			SET  @Revenue = ( @Number * @Revenue ) 
			INSERT INTO WF_ActualValues( NodeId, ValueDate, Value, ValueType)  
			VALUES( @RevenueNodeId, @SyncDate, @Revenue, @ValueType)  
		END					 
	END  


END
