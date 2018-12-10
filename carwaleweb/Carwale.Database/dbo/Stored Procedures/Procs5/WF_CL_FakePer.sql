IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_CL_FakePer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_CL_FakePer]
GO

	CREATE PROCEDURE [dbo].[WF_CL_FakePer]
	
	@NodeId		AS NUMERIC,
	@ValueType	AS SMALLINT,
	@Day AS BIGINT,
	@Month AS BIGINT,
	@Year AS BIGINT,
	@SyncDate AS DATETIME

 AS

	DECLARE 
		@Value      AS DECIMAL(18,2),
		@FakeVal	AS NUMERIC,
		@VerVal		As NUMERIC	

BEGIN
	DECLARE @TempTbl1 Table(TotalCalls NUMERIC, ActionId NUMERIC)
	
	INSERT INTO @TempTbl1
	SELECT COUNT(DISTINCT CL.CallId) AS TotalCalls, CL.ActionId
	FROM CH_Calls AS CC, CH_Logs AS CL
	WHERE CC.Id = CL.CallId AND CC.CallType = 1 AND CC.TBCType=2
		AND CL.ActionId IN(4,5) AND 
		DAY(CC.EntryDateTime) = @Day AND
		MONTH(CC.EntryDateTime) = @Month 
		AND YEAR(CC.EntryDateTime) = @Year
	GROUP BY CL.ActionId

	SELECT @VerVal=ISNULL(SUM(TotalCalls),0) FROM @TempTbl1
	SELECT @FakeVal=ISNULL(TotalCalls,0) FROM @TempTbl1 WHERE ActionId = 5
	
	PRINT @VerVal
	PRINT @FakeVal
	IF @VerVal <> 0 AND @VerVal <> 0.00
		BEGIN
			SET @Value = (100*@FakeVal)/@VerVal
			INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
			VALUES(@NodeId, @SyncDate, @Value, @ValueType)
		END
	
END

















