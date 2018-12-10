IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_SaveFeaturedCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_SaveFeaturedCars]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 22nd Jan 2014
-- Description:	To Save PQ Featured cars 
-- =============================================
CREATE PROCEDURE [dbo].[PQ_SaveFeaturedCars]
	@CampaignId		INT,
	@StateId		VARCHAR(MAX),
	@CityId			VARCHAR(MAX),
	@FeaturedCars	[dbo].[PQ_FeaturedCars] READONLY,
	@Status			BIT OUTPUT
AS
BEGIN
	DECLARE		@NumberOfCars		INT = 0,
				@i					TINYINT,
				@TempFeaturedCar	INT,
				@TempTargetCar		INT

	SELECT @NumberOfCars = COUNT(*) FROM @FeaturedCars
	SET @i = 1

	WHILE @NumberOfCars > 0
	BEGIN
		SELECT @TempTargetCar=TargetVersion, @TempFeaturedCar=FeaturedVersion FROM @FeaturedCars WHERE Id = @i

		DECLARE @TempState INT,@IndxState INT

		DECLARE @States VARCHAR(250)
		SET @States = @StateId

		IF @States IS NOT NULL
			AND @States <> ''
		BEGIN
			WHILE @States <> ''
																																																																																							BEGIN
			SET @IndxState = CHARINDEX(',', @States)

				IF @IndxState > 0
		BEGIN
			SET @TempState = LEFT(@States, @IndxState - 1)
			SET @States = RIGHT(@States, LEN(@States) - @IndxState)
		END
		ELSE
		BEGIN
			SET @TempState = @States
			SET @States = ''
		END

				--current State in variable @TempState	
				DECLARE @Cities VARCHAR(250)

				SET @Cities = @CityId

				DECLARE @TempCity INT,@IndxCity INT

				WHILE @Cities <> ''
				BEGIN
				SET @IndxCity = CHARINDEX(',', @Cities)

				IF @IndxCity > 0
				BEGIN
					SET @TempCity = LEFT(@Cities, @IndxCity - 1)
					SET @Cities = RIGHT(@Cities, LEN(@Cities) - @IndxCity)
				END
				ELSE
				BEGIN
					SET @TempCity = @Cities
					SET @Cities = ''
				END

				SELECT Id FROM PQ_FeaturedCampaignRules 
				WHERE FeaturedCampaignId = @CampaignId 
				AND StateId = @TempState 
				AND CityId = @TempCity
				AND TargetVersion = @TempTargetCar
				AND FeaturedVersion = @TempFeaturedCar

				IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO PQ_FeaturedCampaignRules(FeaturedCampaignId,StateId,CityId,TargetVersion,FeaturedVersion)
					VALUES (@CampaignId,@TempState,@TempCity,@TempTargetCar,@TempFeaturedCar)
				END
			END --end of City loop
			END --end of State loop
		END

		SET @i = @i + 1
		SET @NumberOfCars = @NumberOfCars - 1
	END 
	SET @Status = 1 
END

