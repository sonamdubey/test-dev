IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_CL_NotVerified]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_CL_NotVerified]
GO
	CREATE PROCEDURE [dbo].[WF_CL_NotVerified]
	
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

	Select @Value = COUNT(Id) from CustomerSellInquiries WHere IsApproved = 0 AND IsFake = 0

	PRINT @Value
	IF @Value <> 0 AND @Value <> 0.00
		BEGIN
			INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
			VALUES(@NodeId, @SyncDate, @Value, @ValueType)
		END
		
END
