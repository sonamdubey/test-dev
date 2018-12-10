IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_UB_UsedCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_UB_UsedCar]
GO
	CREATE PROCEDURE [dbo].[WF_UB_UsedCar] --104,1
(
	@NodeId  AS NUMERIC,  
	@ValueType AS SMALLINT,
	@Day AS BIGINT,
	@Month AS BIGINT,
	@Year AS BIGINT,
	@SyncDate AS DATETIME 
)
AS 
BEGIN

DECLARE  @UniqueByUC AS DECIMAL(18,0)

	SELECT @UniqueByUC = ISNULL(COUNT(CustomerId),0) FROM
	( 
		SELECT  DISTINCT CustomerId FROM CustomerSellInquiries 
		WHERE
		    DAY(EntryDate) = @Day   
		    AND MONTH(EntryDate) = @Month 
		    AND YEAR(EntryDate) = @Year  
		UNION  
		SELECT DISTINCT CustomerId FROM ClassifiedRequests	
		WHERE
			 DAY(RequestDateTime) = @Day  
			 AND MONTH(RequestDateTime) = @Month 
			 AND YEAR(RequestDateTime) = @Year   
		UNION 
		SELECT DISTINCT CustomerId FROM UsedCarPurchaseInquiries	
		WHERE
		     DAY(RequestDateTime) = @Day   
		     AND MONTH(RequestDateTime) = @Month  
		     AND YEAR(RequestDateTime) = @Year  
	)tbl	
			  
	IF @UniqueByUC <> 0 AND @UniqueByUC <> 0.00  
	BEGIN

		INSERT INTO WF_ActualValues( NodeId, ValueDate, Value, ValueType)  
							 VALUES( @NodeId, @SyncDate, @UniqueByUC, @ValueType)  
			 
	END  
		  
END
