IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_UB_NewCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_UB_NewCars]
GO

	CREATE PROCEDURE [dbo].[WF_UB_NewCars] --103,1
(
	@NodeId  AS NUMERIC,  
	@ValueType AS SMALLINT ,
	@Day AS BIGINT,
	@Month AS BIGINT,
	@Year AS BIGINT,
	@SyncDate AS DATETIME 
)
AS 
BEGIN

DECLARE  @UniqueBuyer AS DECIMAL(18,0)

	SELECT @UniqueBuyer = ISNULL(COUNT(DISTINCT NP.CustomerId),0)
	FROM NewCarPurchaseInquiries AS NP WITH (NOLOCK), Customers AS Cu WITH (NOLOCK)
	WHERE ForwardedLead = 1 AND CU.Id = NP.CustomerId AND Cu.IsFake = 0 
		  AND DAY(RequestDateTime) = @Day  
		  AND MONTH(RequestDateTime) = @Month  
		  AND YEAR(RequestDateTime) = @Year 
		  
	IF @UniqueBuyer <> 0 AND @UniqueBuyer <> 0.00  
	BEGIN

		INSERT INTO WF_ActualValues( NodeId, ValueDate, Value, ValueType)  
							 VALUES( @NodeId, @SyncDate, @UniqueBuyer, @ValueType)  
							 
	END  
		  
END
