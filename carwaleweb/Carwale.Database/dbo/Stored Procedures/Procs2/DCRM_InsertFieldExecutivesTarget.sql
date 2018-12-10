IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_InsertFieldExecutivesTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_InsertFieldExecutivesTarget]
GO

	-- =============================================
-- Author	:	Sachin Bharti(18th May 2015)
-- Description	:	Insert field executives target
-- Modifier : Kartik Rathod on 2 Mar 2016
-- Desc : in case of UCD,set target for each user in bottom to Up manner ie. L3 to L1
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_InsertFieldExecutivesTarget]
	
	@DCRM_TblFieldExecutivesTarget DCRM_TblFieldExecutivesTarget READONLY,
	@BusinessUnitId	SMALLINT,
	@MetricId	SMALLINT,
	@TargetYear VARCHAR(4),
	@QuarterId	SMALLINT,
	@AddedBy INT,
	@IsInserted	BIT OUTPUT

AS
BEGIN
	
	SET @IsInserted = 0

	--SET @TargetYear = (	CASE WHEN GETDATE() BETWEEN CONVERT(DATETIME, '4/1/'+CAST(YEAR(GETDATE()) AS VARCHAR), 101) 
	--										AND	CONVERT(DATETIME, '12/31/'+CAST((YEAR(GETDATE())) AS VARCHAR), 101)
	--					THEN YEAR(GETDATE()) END)

	--create temp table to store values of data table coming as a parameter
	DECLARE @UsersTarget TABLE (Id INT IDENTITY(1,1) , UserId INT , MonthId SMALLINT, UserTarget INT);
	DECLARE	@ExecutiveId	INT,
			@TargetMonthId	SMALLINT,
			@ExecTarget	INT,
			@TotalUsers	SMALLINT,
			@RowNo	SMALLINT,
			@NoOfRow SMALLINT

	--store data into the local table
	INSERT INTO @UsersTarget (UserId,MonthId,UserTarget)
				SELECT OprUserId,Month,Target  FROM @DCRM_TblFieldExecutivesTarget
	SET @TotalUsers = @@ROWCOUNT
	SET @RowNo = 1

	WHILE(@RowNo  <= @TotalUsers)
		BEGIN
			--read data from temp table
			SELECT 
				@ExecutiveId = UserId,
				@TargetMonthId = MonthId,
				@ExecTarget = UserTarget
			FROM	
				@UsersTarget
			WHERE
				Id = @RowNo

			--check if target is already set for the executive
			SELECT Id FROM DCRM_FieldExecutivesTarget WITH(NOLOCK)
			WHERE OprUserId = @ExecutiveId AND TargetMonth = @TargetMonthId AND TargetYear = @TargetYear AND MetricId = @MetricId AND BusinessUnitId = @BusinessUnitId
			SET @NoOfRow = @@ROWCOUNT
			--If not then make new entry
			IF @NoOfRow = 0
				BEGIN
					INSERT INTO DCRM_FieldExecutivesTarget
								(	OprUserId,
									BusinessUnitId,
									MetricId,
									UserTarget,
									TargetMonth,
									TargetYear,
									QuarterId,
									AddedBy,
									AddedOn
								)
								VALUES
								(	@ExecutiveId,
									@BusinessUnitId,
									@MetricId,
									@ExecTarget,
									@TargetMonthId,
									@TargetYear,
									@QuarterId,
									@AddedBy,
									GETDATE()
								)
					SET @IsInserted = 1
				END
			--if target already added then update the entry
			ELSE IF @NoOfRow = 1
				BEGIN
					UPDATE 
						DCRM_FieldExecutivesTarget SET	UserTarget = @ExecTarget,
														UpdatedBy = @AddedBy,
														UpdatedOn = GETDATE()
					WHERE
						OprUserId = @ExecutiveId
						AND TargetMonth = @TargetMonthId
						AND TargetYear = @TargetYear
						AND MetricId = @MetricId
						AND BusinessUnitId = @BusinessUnitId
					SET @IsInserted = 1
				END
			SET @RowNo += 1
		END

		IF @BusinessUnitId = 1	AND @IsInserted = 1	--added by kartik rathod in case of UCD it will generate the new set target for users
		BEGIN 
			EXEC [dbo].[DCRM_SetUCDTarget] @DCRM_TblFieldExecutivesTarget,@TargetYear,@MetricId,@AddedBy,@QuarterId
		END 
END
--------------------





