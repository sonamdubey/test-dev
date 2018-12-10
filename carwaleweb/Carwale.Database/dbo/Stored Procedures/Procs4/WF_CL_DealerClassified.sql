IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_CL_DealerClassified]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_CL_DealerClassified]
GO
	CREATE PROCEDURE [dbo].[WF_CL_DealerClassified]
	
	@NodeId		AS NUMERIC,
	@ValueType	AS SMALLINT,
	@Day AS BIGINT,
	@Month AS BIGINT,
	@Year AS BIGINT,
	@SyncDate AS DATETIME	

 AS

	DECLARE 
		@Value      AS DECIMAL(18,2)

BEGIN

	SELECT @Value = ISNULL(SUM(ActualAmount),0) 
	FROM ConsumerPackageRequests AS CPR, Packages AS P
	WHERE P.Id = CPR.PackageId
		AND CPR.IsApproved = 1 AND CPR.ConsumerType = 1
		AND DAY(EntryDate) = @Day 
		AND MONTH(EntryDate) = @Month
		AND YEAR(EntryDate) = @Year
	
	PRINT @Value
	IF @Value <> 0 AND @Value <> 0.00
		BEGIN
			INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
			VALUES(@NodeId, @SyncDate, @Value, @ValueType)
		END
		
END
