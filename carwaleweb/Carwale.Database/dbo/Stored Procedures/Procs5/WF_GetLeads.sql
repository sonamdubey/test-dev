IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_GetLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_GetLeads]
GO

	CREATE PROCEDURE [dbo].[WF_GetLeads] -- 87,1
	
	@NodeId		AS NUMERIC,
	@ValueType	AS SMALLINT,
	@Day AS BIGINT,
	@Month AS BIGINT,
	@Year AS BIGINT,
	@SyncDate AS DATETIME	

AS
BEGIN

DECLARE @Value  AS DECIMAL(18,2)	

	SELECT @Value = ISNULL(COUNT(ID),0) FROM CRM_Leads 
	WHERE 
		DAY(CREATEDON) = @Day AND 
		MONTH(CREATEDON) = @Month AND
		YEAR(CREATEDON) = @Year

	IF @Value <> 0 AND @Value <> 0.00
		BEGIN
			INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
					  			 VALUES(@NodeId, @SyncDate, @Value, @ValueType)
		END


END
