IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_GetOEM]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_GetOEM]
GO

	CREATE PROCEDURE [dbo].[WF_GetOEM] -- 88,1
	
	@NodeId		AS NUMERIC,
	@ValueType	AS SMALLINT,
	@Day AS BIGINT,
	@Month AS BIGINT,
	@Year AS BIGINT,
	@SyncDate AS DATETIME	

AS
BEGIN

DECLARE @Value  AS DECIMAL(18,2)	

--To Get OEM Leads
	SELECT @Value = ISNULL(COUNT(ID),0) FROM CRM_Leads 
	WHERE 
	ID IN (Select LeadId From CRM_CarBasicData Where VersionId IN 
	(Select ID From CarVersions Where CarModelId IN (Select ID From CarModels Where CarMakeId IN (2,9,15,29))))
	AND   DAY(CREATEDON) = @Day AND 
		  MONTH(CREATEDON) = @Month AND
		  YEAR(CREATEDON) = @Year

	IF @Value <> 0 AND @Value <> 0.00
		BEGIN
			INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
					  			 VALUES(@NodeId, @SyncDate, @Value, @ValueType)
		END


END
