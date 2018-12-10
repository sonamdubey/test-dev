IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_CL_RevenuePerListing]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_CL_RevenuePerListing]
GO
	CREATE PROCEDURE [dbo].[WF_CL_RevenuePerListing]
	
	@NodeId		AS NUMERIC,
	@ValueType	AS SMALLINT,
	@Day AS BIGINT,
	@Month AS BIGINT,
	@Year AS BIGINT,
	@SyncDate AS DATETIME	

 AS

	DECLARE 
		@Value      AS DECIMAL(18,2),
		@LCount		AS NUMERIC,
		@TRevenue	AS DECIMAL(18,2)

BEGIN

	SELECT @Value = ISNULL(SUM(CPR.ActualAmount),0) 
	FROM ConsumerPackageRequests AS CPR, Packages AS P
	WHERE	P.Id = CPR.PackageId
			AND IsApproved = 1 AND ConsumerType = 2 AND PackageId = 1
			AND DAY(EntryDate) = @Day 
			AND MONTH(EntryDate) = @Month
			AND YEAR(EntryDate) = @Year

	SELECT @LCount = ISNULL(COUNT(Id),0) FROM CustomerSellInquiries 
	WHERE DAY(EntryDate) = @Day AND MONTH(EntryDate) = @Month
	AND YEAR(EntryDate) = @Year
	
	PRINT @Value
	PRINT @LCount
	IF @LCount <> 0 
		BEGIN
			SET @TRevenue = @Value/@LCount

			IF @TRevenue <> 0 AND @TRevenue <> 0.00
				BEGIN
					INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
					VALUES(@NodeId, @SyncDate, @TRevenue, @ValueType)
				END
		END
	ELSE
		BEGIN
			IF @Value <> 0 AND @Value <> 0.00
				BEGIN
					INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
					VALUES(@NodeId, @SyncDate, @Value, @ValueType)
				END
		END
		
END
