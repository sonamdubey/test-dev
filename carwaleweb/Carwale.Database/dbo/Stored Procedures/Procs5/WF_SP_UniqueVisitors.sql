IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_SP_UniqueVisitors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_SP_UniqueVisitors]
GO

	CREATE PROCEDURE [dbo].[WF_SP_UniqueVisitors]
	
	@NodeId		AS NUMERIC,
	@ValueType	AS SMALLINT,
	@Month AS BIGINT,
	@Year AS BIGINT

 AS

	DECLARE 
		@Value      AS DECIMAL(18,2),
		@EntryDate	AS DATETIME,
		@MaxCount	NUMERIC,
		@StartCount	NUMERIC

BEGIN
	DECLARE @TempTbl1 Table(Id INT IDENTITY(1, 1) PRIMARY KEY NOT NULL , Val NUMERIC, EntryDate DATETIME)
	
	INSERT INTO @TempTbl1
	SELECT ISNULL(UniqueVisitors,0) AS Val, EntryDate 
	FROM DailyLogs 
	WHERE MONTH(EntryDate) = @Month
			AND YEAR(EntryDate) = @Year

	SET @StartCount = 1
	SET @MaxCount = 0
	SELECT @MaxCount = COUNT(Id) FROM @TempTbl1

	WHILE @StartCount <= @MaxCount
		BEGIN
			SELECT @Value=Val, @EntryDate =  EntryDate 
			FROM @TempTbl1 WHERE Id = @StartCount
			
			PRINT @Value
			PRINT @EntryDate

			IF @Value <> 0 AND @Value <> 0.00
				BEGIN
					UPDATE WF_ActualValues SET Value = @Value
					WHERE NodeId = @NodeId AND ValueType = @ValueType
							AND DAY(ValueDate) = DAY(@EntryDate)
							AND MONTH(ValueDate) = MONTH(@EntryDate)
							AND YEAR(ValueDate) = YEAR(@EntryDate)

					IF @@ROWCOUNT = 0
						BEGIN 
							INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
							VALUES(@NodeId, @EntryDate, @Value, @ValueType)
						END
				END
			SELECT @StartCount = @StartCount + 1
		END
	
END
