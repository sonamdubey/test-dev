IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_FirstConnectAvg]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_FirstConnectAvg]
GO
	CREATE PROCEDURE [dbo].[WF_FirstConnectAvg] -- 91,1

	@NodeId		AS NUMERIC,
	@ValueType	AS SMALLINT,
	@Day AS BIGINT,
	@Month AS BIGINT,
	@Year AS BIGINT,
	@SyncDate AS DATETIME	

AS
BEGIN

DECLARE @Value  AS DECIMAL(18,2)	
DECLARE @ByVAL  AS DECIMAL(18,2)	

--To Get First Connect Diffrence
	SELECT @Value = ISNULL(AVG(AvgFirstConnect),0) FROM  
	(
		SELECT CC.LeadId , CC.ActionTakenOn, 
		       ROW_NUMBER() OVER(PARTITION BY CC.LEADID ORDER BY CC.ActionTakenOn ASC) AS MAXActionDate,
			   DATEDIFF(MINUTE, CL.CREATEDON, CC.ActionTakenOn) AS AvgFirstConnect
		FROM 
			CRM_CALLS CC
			LEFT JOIN CRM_LEADS CL ON CL.ID = CC.LeadId 
		WHERE IsActionTaken = 1 AND 
			  DAY(CL.CREATEDON) = @Day AND 
			  MONTH(CL.CREATEDON) = @Month AND
			  YEAR(CL.CREATEDON) = @Year AND 
			  
			  (DatePart(HH,CL.CREATEDON) BETWEEN 9 AND 19)
		GROUP BY CC.LeadId ,CC.ActionTakenOn, CL.CREATEDON	  
	)tbl	  
	WHERE MAXActionDate = 1

	IF @Value <> 0 AND @Value <> 0.00
		BEGIN

			INSERT INTO 
			WF_ActualValues(NodeId, ValueDate, Value, ValueType)
			VALUES(@NodeId, @SyncDate, @Value, @ValueType)
		END


END
