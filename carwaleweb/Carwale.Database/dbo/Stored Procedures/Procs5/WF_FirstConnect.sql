IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_FirstConnect]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_FirstConnect]
GO

	CREATE PROCEDURE [dbo].[WF_FirstConnect] -- 90,1

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

--To Get First Connect Count
	SELECT @Value = COUNT(DISTINCT CC.LeadId)  FROM 
		CRM_CALLS CC
		LEFT JOIN CRM_LEADS CL ON CL.ID = CC.LeadId
	WHERE IsActionTaken = 1 AND 
		  DAY(CL.CREATEDON) = @Day AND 
		  MONTH(CL.CREATEDON) = @Month AND
		  YEAR(CL.CREATEDON) = @Year AND 
		  (DatePart(HH,CL.CREATEDON) BETWEEN 9 AND 19)

--To Get Total Leads
	SELECT @ByVAL = COUNT(ID) 
	FROM CRM_Leads 
	WHERE DAY(CREATEDON) = @Day AND 
		  MONTH(CREATEDON) = @Month AND
		  YEAR(CREATEDON) = @Year AND
		  (DatePart(HH,CREATEDON) BETWEEN 9 AND 19)

	IF @ByVAL <> 0 AND @ByVAL <> 0.00
		BEGIN
			SELECT @VALUE = (@VALUE/@ByVAL) * 100
			
			INSERT INTO WF_ActualValues( NodeId, ValueDate, Value, ValueType)
					  			 VALUES( @NodeId, @SyncDate, @Value, @ValueType)
		END


END
