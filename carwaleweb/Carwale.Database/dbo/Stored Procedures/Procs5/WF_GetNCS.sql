IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_GetNCS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_GetNCS]
GO

	CREATE PROCEDURE [dbo].[WF_GetNCS] -- 89,1
	
	@NodeId		AS NUMERIC,
	@ValueType	AS SMALLINT,
	@Day AS BIGINT,
	@Month AS BIGINT,
	@Year AS BIGINT,
	@SyncDate AS DATETIME	

AS
BEGIN

DECLARE @Value  AS DECIMAL(18,2)	

--To Get NCS Leads
	SELECT @Value = ISNULL(COUNT(ID),0) FROM CRM_Leads 
	WHERE OWNER IN ( SELECT TeamId FROM CRM_ADM_QueueTeams WHERE QueueId in (6,10,12) ) AND 
		DAY(CREATEDON) = @Day AND 
		MONTH(CREATEDON) = @Month AND
		YEAR(CREATEDON) = @Year

	IF @Value <> 0 AND @Value <> 0.00
		BEGIN
			INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
					  			 VALUES(@NodeId, @SyncDate, @Value, @ValueType)
		END


END
