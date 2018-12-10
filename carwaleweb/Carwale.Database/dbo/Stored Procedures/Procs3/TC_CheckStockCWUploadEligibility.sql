IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckStockCWUploadEligibility]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckStockCWUploadEligibility]
GO

	-- ======================================================================================================================
-- Author		: Suresh Prajapati
-- Created On	: 17th Mar, 2016
-- Description	: This SP verifies the eligibility criteria for uploading stock on Carwale with dealer's current package 
-- exec TC_CheckStockCWUploadEligibility '611183,611184,612001,611615,611150,611143,611138,611185,611186,611187,611190,611191,611192,611193,611201,611230',5
-- exec TC_CheckStockCWUploadEligibility '611183,611184,612001,611615,611150,611143,611138,611185,611186,611187,611190,611191,611192,611193,611201,611230',83
-- Modified By : Tejashree Patil on 14 April 2016, checked eligibility Version wise.
-- ======================================================================================================================
CREATE PROCEDURE [dbo].[TC_CheckStockCWUploadEligibility]
	-- Add the parameters for the stored procedure here
	@StockIds VARCHAR(300) = NULL
	,@DealerId INT = NULL
AS
BEGIN
	DECLARE @IsStockLimitExceeded BIT = 1
		,@IsGroupTypeViolationFalis BIT = 1

	----------------------- GET ALL REQUESTED STOCKIDS WITH MODELID AND CARNAME -----------------------------------
	CREATE TABLE #TmpStockIds (
		StockId INT
		,ModelId INT
		,StockName VARCHAR(200)
		,BranchId INT
		,VersionId INT -- Modified By : Tejashree Patil on 14 April 2016, Added versionid
		)

	INSERT INTO #TmpStockIds (
		StockId
		,ModelId
		,StockName
		,BranchId
		,VersionId -- Modified By : Tejashree Patil on 14 April 2016, Added versionid
		)
	SELECT DISTINCT ListMember
		,MMV.ModelId
		,MMV.Car AS StockName
		,S.BranchId
		,S.VersionId -- Modified By : Tejashree Patil on 14 April 2016, Added versionid
	FROM fnSplitCSVValues(@StockIds) AS FV
	INNER JOIN TC_Stock AS S WITH (NOLOCK) ON S.Id = FV.ListMember
	INNER JOIN vwAllMMV AS MMV WITH (NOLOCK) ON S.VersionId = MMV.VersionId -- Modified By : Tejashree Patil on 14 April 2016, Added versionid
		AND MMV.ApplicationId = 1
	WHERE S.IsActive = 1
		AND S.StatusId IN (
			1 -- Available
			,4 -- Suspended
			)
		AND ISNULL(S.IsSychronizedCW, 0) <> 1
		AND BranchId = @DealerId
	--select * from #TmpStockIds
	------------------------------------------------------------------------------------------------------------------
	DECLARE @StockCount INT = @@ROWCOUNT
	DECLARE @MaxStockCount INT

	SELECT @MaxStockCount = CCP.Points
	FROM ConsumerCreditPoints AS CCP WITH (NOLOCK)
	WHERE ConsumerId = @DealerId
		AND ConsumerType = 1

	SELECT @MaxStockCount AS StockLimit

	--IF (@StockCount <> 0)
	--BEGIN
		-- Check for New UCD Package Dealer
		--IF NOT EXISTS (
		--		SELECT 1
		--		FROM ConsumerCreditPoints WITH (NOLOCK)
		--		WHERE ConsumerType = 1
		--			AND ConsumerId = @DealerId
		--			AND CarGroupType IS NULL
		--		)
		--BEGIN
		-- i.e. New UCD Package Dealer
		-------------------------------------------- STEP  1 : Check for IsStockLimitExceeded -------------------------------------------
		DECLARE @StockLimit INT;
		--DECLARE @MaxStockCount INT
		DECLARE @UploadedStockCount INT

		--SELECT @MaxStockCount = CCP.Points
		--FROM ConsumerCreditPoints AS CCP WITH (NOLOCK)
		--WHERE ConsumerId = @DealerId
		--	AND ConsumerType = 1
		--SELECT @MaxStockCount AS StockLimit
		SELECT @UploadedStockCount = COUNT(DISTINCT S.Id)
		FROM TC_Stock AS S WITH (NOLOCK)
		WHERE S.BranchId = @DealerId
			AND S.StatusId = 1 -- Available
			AND ISNULL(IsSychronizedCW, 0) = 1
			AND S.IsActive = 1

		--SELECT @UploadedStockCount AS S
		SET @StockLimit = @MaxStockCount - @UploadedStockCount
		--SELECT @StockLimit AS StockLimit
		SET @IsStockLimitExceeded = (
				SELECT CASE 
						WHEN @StockCount > @StockLimit
							THEN 1
						ELSE 0
						END
				)

		SELECT @IsStockLimitExceeded AS IsStockLimitExceeded

		DECLARE @IsOldUCDPckgDealer BIT = CASE (
					SELECT 1
					FROM ConsumerCreditPoints WITH (NOLOCK)
					WHERE ConsumerType = 1
						AND ConsumerId = @DealerId
						AND CarGroupType IS NULL
					)
				WHEN 1
					THEN 1
				ELSE 0
				END

		--IF (@IsStockLimitExceeded = 1)
		--	SELECT @MaxStockCount AS StockLimit
		------------------------------------------------------- STEP 1 Ends Here ---------------------------------------------------------
		IF (
				@IsOldUCDPckgDealer <> 1
				AND @IsStockLimitExceeded <> 1
				)
		BEGIN
			-------------------------------------------------- STEP 2 : Group Type Violation Check-------------------------------------------
			--@CarGroupType VARCHAR(100)
			--,@TopUpCarGroup VARCHAR(100)
			DECLARE @CarsGroupType VARCHAR(200)

			--SELECT @CarGroupType = CarGroupType
			--	,@TopUpCarGroup = TopUpCarGroupType
			--FROM ConsumerCreditPoints WITH (NOLOCK)
			--WHERE ConsumerId = @DealerId
			--	AND ConsumerType = 1
			SELECT @CarsGroupType = ISNULL(CarGroupType, '') + ',' + ISNULL(TopUpCarGroupType, '')
			FROM ConsumerCreditPoints WITH (NOLOCK)
			WHERE ConsumerId = @DealerId
				AND ConsumerType = 1 -- Dealer

			--SELECT *
			--FROM #TmpStockIds
			--SELECT @CarsGroupType AS CarsGroupType
			SELECT DISTINCT S.StockId
				,S.ModelId
				,S.StockName
				,S.VersionId
			FROM #TmpStockIds AS S WITH (NOLOCK)
			--INNER JOIN ConsumerCreditPoints AS CC WITH (NOLOCK) ON CC.ConsumerId = S.BranchId
			LEFT JOIN CarGroupTypes AS CG WITH (NOLOCK) ON CG.VersionId = S.VersionId --CG.ModelId = S.ModelId
				-- Modified By : Tejashree Patil on 14 April 2016, Added versionid
				--AND CG.CarGroupTypeId IN (
				--	SELECT CT.ListMember
				--	FROM fnSplitCSVValues(@CarGroupType) AS CT
				--	UNION
				--	SELECT TP.ListMember
				--	FROM fnSplitCSVValues(@TopUpCarGroup) AS TP
				--	)
				AND CarGroupTypeId IN (
					SELECT DISTINCT ListMember
					FROM fnSplitCSVValues(@CarsGroupType)
					)
				AND CG.IsActive = 1
			WHERE CG.VersionId IS NULL

			--CC.ConsumerType = 1 AND 
			--CG.ModelId IS NULL
			--AND CG.IsActive = 1
			SET @IsGroupTypeViolationFalis = (
					CASE 
						WHEN @@ROWCOUNT > 0
							THEN 1
						ELSE 0
						END
					)

			SELECT @IsGroupTypeViolationFalis AS IsGroupTypeViolationFalis
				------------------------------------------------------- STEP 2 Ends Here ---------------------------------------------------------
		END

		-- END
		-- ELSE
		-- BEGIN
		IF (@IsOldUCDPckgDealer = 1)
		BEGIN
			-- i.e. Old UCD Package Dealer and hence suspend Eligibility check for group type
			-- SET @IsStockLimitExceeded = 0
			-- SELECT @IsStockLimitExceeded AS IsStockLimitExceeded
			-- suspend Eligibility check for group type
			SELECT StockId
				,ModelId
				,StockName
			FROM #TmpStockIds
			WHERE 1 <> 1

			SET @IsGroupTypeViolationFalis = 0

			SELECT @IsGroupTypeViolationFalis AS IsGroupTypeViolationFalis
		END
	--END

	DROP TABLE #TmpStockIds
END