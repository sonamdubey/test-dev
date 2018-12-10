IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_CL_PaidListing]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_CL_PaidListing]
GO
	CREATE PROCEDURE [dbo].[WF_CL_PaidListing]
	
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

	SELECT @Value = ISNULL(COUNT(Id),0) FROM ConsumerPackageRequests 
	WHERE IsApproved = 1 AND ConsumerType = 2 AND PackageId = 1
			AND DAY(EntryDate) = @Day
			AND MONTH(EntryDate) = @Month
			AND YEAR(EntryDate) = @Year
			AND ActualAmount > 0
	
	PRINT @Value
	IF @Value <> 0 AND @Value <> 0.00
		BEGIN
			INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
			VALUES(@NodeId, @SyncDate, @Value, @ValueType)
		END
		
END
