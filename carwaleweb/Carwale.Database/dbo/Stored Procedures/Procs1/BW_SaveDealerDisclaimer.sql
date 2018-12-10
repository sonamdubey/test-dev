IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_SaveDealerDisclaimer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_SaveDealerDisclaimer]
GO

	
--=========================================================
--Author      : Suresh Prajapati
--Create date : 03rd Dec, 2014.
--Description : To Save Dealer Disclaimer for specified DealerId.
--=========================================================
CREATE PROCEDURE [dbo].[BW_SaveDealerDisclaimer]
	-- Add the parameters for the stored procedure here
	@DealerId INT
	,@BikeMakeId INT
	,@BikeModelId INT
	,@BikeVersionId INT
	,@Disclaimer VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

	IF (
			(
				@BikeVersionId IS NULL
				OR @BikeVersionId = 0
				)
			AND (
				@BikeModelId IS NULL
				OR @BikeModelId = 0
				)
			)
	BEGIN
		DECLARE @tbl TABLE (
			VersionId INT
			,DealerId INT
			)

		INSERT INTO @tbl
		SELECT BV.ID
			,@DealerId
		FROM BikeVersions BV
		INNER JOIN BikeModels BMO ON BMO.ID = BV.BikeModelId
		INNER JOIN BikeMakes BMA ON BMA.ID = BMO.BikeMakeId
		WHERE BMO.IsDeleted = 0
			AND BMA.IsDeleted = 0
			AND BV.IsDeleted = 0
			AND BMA.New = 1
			AND BMO.New = 1
			AND BV.New = 1
			AND BMA.Futuristic = 0
			AND BMO.Futuristic = 0
			AND BV.Futuristic = 0
			AND BMA.ID = @BikeMakeId

		INSERT INTO BW_DealerDisclaimer (
			DealerId
			,BikeVersionId
			,Disclaimer
			,IsActive
			)
		SELECT DealerId
			,VersionId
			,@Disclaimer
			,1
		FROM @tbl
	END
	ELSE
		IF @BikeModelId IS NOT NULL
			AND (
				@BikeVersionId IS NULL
				OR @BikeVersionId = 0
				)
		BEGIN
			DECLARE @tbl1 TABLE (
				VersionId INT
				,DealerId INT
				)

			INSERT INTO @tbl1
			SELECT ID
				,@DealerId
			FROM BikeVersions AS BV
			WHERE BikeModelId = @BikeModelId
				AND BV.New = 1
				AND BV.IsDeleted = 0
				AND BV.Futuristic = 0

			INSERT INTO BW_DealerDisclaimer (
				DealerId
				,BikeVersionId
				,Disclaimer
				,IsActive
				)
			SELECT DealerId
				,VersionId
				,@Disclaimer
				,1
			FROM @tbl1
		END
		ELSE
		BEGIN
			DECLARE @TempTbl TABLE (
				VersionId INT
				,DealerId INT
				)

			INSERT INTO @TempTbl
			VALUES (
				@BikeVersionId
				,@DealerId
				)

			INSERT INTO BW_DealerDisclaimer (
				DealerId
				,BikeVersionId
				,Disclaimer
				,IsActive
				)
			SELECT DealerId
				,VersionId
				,@Disclaimer
				,1
			FROM @TempTbl
		END
END

