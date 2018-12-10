IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_SaveBooingAmount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_SaveBooingAmount]
GO

	
-- =============================================
-- Author:		Ashwini Todkar
-- Create date: 16th Dec 2014
-- Description:	To add booking amount for bike
-- =============================================
CREATE PROCEDURE [dbo].[BW_SaveBooingAmount] @DealerId INT
	,@BikeModelId INT
	,@BikeVersionId INT = NULL
	,@Amount INT
AS
BEGIN

	IF @BikeVersionId IS NULL
	BEGIN
		DECLARE @Temp_tbl TABLE (
			VersionId INT
			,DealerId INT
			)

		-- get all versions of a model (new only)
		INSERT INTO @Temp_tbl
		SELECT ID, @DealerId
		FROM BikeVersions WITH (NOLOCK)
		WHERE BikeModelId = @BikeModelId
			AND New = 1;

		-- set booking amount for all version a model
		MERGE BW_DealerBikeBookingAmounts AS BA
		USING (
			SELECT VersionId
				,DealerId
			FROM @Temp_tbl
			) AS BW_BA
			ON BA.DealerId = BW_BA.DealerId
				AND BA.VersionId = BW_BA.VersionId
		WHEN MATCHED
			THEN
				UPDATE
				SET BA.Amount = @Amount
		WHEN NOT MATCHED
			THEN
				INSERT (
					DealerId
					,VersionId
					,Amount
					)
				VALUES (
					BW_BA.DealerId
					,BW_BA.VersionId
					,@Amount
					);
	END
	ELSE
	BEGIN
		DECLARE @Temp_tbl_new TABLE (
			VersionId INT
			,DealerId INT
			)

		INSERT INTO @Temp_tbl_new
		VALUES (
			@BikeVersionId
			,@DealerId
			);

		-- set booking amount for a version
		MERGE BW_DealerBikeBookingAmounts AS BA
		USING (
			SELECT VersionId
				,DealerId
			FROM @Temp_tbl_new
			) AS BW_BA_New
			ON BA.DealerId = BW_BA_New.DealerId
				AND BA.VersionId = BW_BA_New.VersionId
		WHEN MATCHED
			THEN
				UPDATE
				SET BA.Amount = @Amount
		WHEN NOT MATCHED
			THEN
				INSERT (
					DealerId
					,VersionId
					,Amount
					)
				VALUES (
					BW_BA_New.DealerId
					,BW_BA_New.VersionId
					,@Amount
					);
	END
END


