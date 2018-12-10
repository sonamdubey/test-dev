IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_Operations_CST_Avg]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_Operations_CST_Avg]
GO
	CREATE PROCEDURE [dbo].[WF_Operations_CST_Avg]  --95,16,1
	
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

--To Get Average For Particular Leads With Respect to Event Type
	Select @Value = AVG(DATEDIFF(MINUTE,CL.CreatedOn,EL.EventOn))
	From
		CRM_Leads as CL,
		CRM_EventLogs AS EL,
		CRM_CarBasicData AS CBD
	Where
		EL.EventType = @EventType  AND
		DAY(EL.EventOn) = @Day AND
		Month(EL.EventOn) = @Month  AND
		Year(EL.EventOn) = @Year  AND
		CBD.ID = EL.ItemId AND
		CL.ID = CBD.LeadId AND
		CL.ID in 
		(
		  Select UniqueLeadId From 
			(
				Select COUNT(EL1.ItemId) AS Cnt, CBD.LeadID AS UniqueLeadId
				From
					CRM_EventLogs AS EL,
					CRM_CarBasicData AS CBD,
					CRM_CarBasicData AS CBD1, 
					CRM_EventLogs AS EL1
					
				Where
					EL.EventType = @EventType AND 
					DAY(EL.EventOn) = @Day AND
					Month(EL.EventOn) = @Month AND
					Year(EL.EventOn) = @Year AND
					CBD.ID = EL.ItemId AND
					CBD1.LeadId = CBD.LeadId AND
					EL1.ItemId = CBD1.ID AND EL1.EventType = @EventType
				GROUP By CBD.LeadID
				Having COUNT(EL1.ItemId) = 1 
			)tbl
		)

	Print @Value
	IF @Value <> 0 AND @Value <> 0.00
		BEGIN
			
			INSERT INTO 
			WF_ActualValues(NodeId, ValueDate, Value, ValueType)
			VALUES(@NodeId, @SyncDate, @Value, @ValueType)
		END
END
