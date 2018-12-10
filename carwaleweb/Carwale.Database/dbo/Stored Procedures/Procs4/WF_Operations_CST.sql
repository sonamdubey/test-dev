IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_Operations_CST]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_Operations_CST]
GO
	CREATE PROCEDURE [dbo].[WF_Operations_CST]  --93,1
	
	@NodeId		AS NUMERIC,
	@EventType		AS NUMERIC,	
	@ValueType	AS SMALLINT,
	@Day AS BIGINT,
	@Month AS BIGINT,
	@Year AS BIGINT,
	@SyncDate AS DATETIME	

AS
BEGIN

DECLARE @Value  AS DECIMAL(18,2)	

--To Get Only Consultation Leads
	SELECT @Value = COUNT(ID) 
	FROM
		CRM_EventLogs
	WHERE EventType = @EventType AND 
		  DAY(EventOn) = @Day AND 
		  MONTH(EventOn) = @Month AND
		  YEAR(EventOn) = @Year

	IF @Value <> 0 AND @Value <> 0.00
		BEGIN
			INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
					  			 VALUES(@NodeId, @SyncDate, @Value, @ValueType)
		END
END
