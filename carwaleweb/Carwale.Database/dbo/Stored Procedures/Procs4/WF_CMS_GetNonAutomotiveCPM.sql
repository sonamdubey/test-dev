IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_CMS_GetNonAutomotiveCPM]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_CMS_GetNonAutomotiveCPM]
GO
	CREATE PROCEDURE [dbo].[WF_CMS_GetNonAutomotiveCPM]
	
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

	SELECT @Value = ISNULL(SUM(BookedAmount),0) FROM CMS_Campaigns WHERE  CampaignCategory = 7
	AND DAY(EntryDate) = @Day AND MONTH(EntryDate) = @Month
	AND YEAR(EntryDate) = @Year

	IF @Value <> 0 AND @Value <> 0.00
		BEGIN
			INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
			VALUES(@NodeId, @SyncDate, @Value, @ValueType)
		END
		
END
