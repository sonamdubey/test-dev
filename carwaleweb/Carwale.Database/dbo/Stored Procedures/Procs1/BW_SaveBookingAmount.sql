IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_SaveBookingAmount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_SaveBookingAmount]
GO

	
-- =============================================
-- Author:		Ashwini Todkar
-- Create date: 16th Dec 2014
-- Description:	To add booking amount for bike
-- Modified By : Suresh Prajapati on 09th, Jan 2015
-- Summary : Made Previous Inactive record Active  if added record already exist
-- exec BW_SaveBookingAmount 11,99,0,111
-- =============================================
CREATE PROCEDURE [dbo].[BW_SaveBookingAmount] @DealerId INT
	,@BikeModelId INT
	,@BikeVersionId INT = NULL
	,@Amount INT
AS
BEGIN
	BEGIN
		IF (
				@BikeVersionId IS NULL
				OR @BikeVersionId = 0
				)
		BEGIN
			DECLARE @Temp_tbl TABLE (
				VersionId INT
				,DealerId INT
				)

			-- get all versions of a model (new only)
			INSERT INTO @Temp_tbl
			SELECT ID
				,@DealerId
			FROM BikeVersions
			WHERE BikeModelId = @BikeModelId
				AND New = 1
				AND IsDeleted = 0;

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
						,BA.IsActive = 1
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
					,BA.IsActive=1
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
END

