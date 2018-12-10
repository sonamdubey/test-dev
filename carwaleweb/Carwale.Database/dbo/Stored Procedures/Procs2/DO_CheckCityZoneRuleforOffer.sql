IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DO_CheckCityZoneRuleforOffer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DO_CheckCityZoneRuleforOffer]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DO_CheckCityZoneRuleforOffer] 
	@OfferId		NUMERIC(18, 0),
	@CityId			VARCHAR(250) = NULL,
	@ZoneId			VARCHAR(250) = NULL,
	@MakeId			INT = NULL,
	@ModelId		VARCHAR(250) = NULL,
	@VersionId		VARCHAR(250) = NULL,
	@StartDate		DATETIME = NULL,
	@EndDate		DATETIME = NULL,
	@Status			BIT OUTPUT
AS
BEGIN
	DECLARE @TempCity INT, @IndxCity INT,@TempZone INT, @IndxZone INT
	SET @Status = 1

	WHILE @CityId <> '' AND @Status = 1
		BEGIN
			SET @IndxCity = CHARINDEX(',', @CityId)

			IF @IndxCity > 0
				BEGIN
					SET @TempCity = LEFT(@CityId, @IndxCity - 1)
					SET @CityId = RIGHT(@CityId, LEN(@CityId) - @IndxCity)
				END
			ELSE
				BEGIN
					SET @TempCity = @CityId
					SET @CityId = ''
				END

				EXEC DO_CheckRuleForOffer 
				@OfferId = @OfferId,
				@CityId	= @TempCity,
				@ZoneId	= NULL,
				@MakeId	= @MakeId,
				@ModelId = @ModelId,
				@VersionId = @VersionId,
				@StartDate = @StartDate,
				@EndDate = @EndDate,
				@IsNewOffer = @Status OUTPUT 
		END
	
	WHILE @ZoneId <> '' AND @Status = 1
		BEGIN
			SET @IndxZone = CHARINDEX(',', @ZoneId)

			IF @IndxZone > 0
				BEGIN
					SET @TempZone = LEFT(@ZoneId, @IndxZone - 1)
					SET @ZoneId = RIGHT(@ZoneId, LEN(@ZoneId) - @IndxZone)
				END
			ELSE
				BEGIN
					SET @TempZone = @ZoneId
					SET @ZoneId = ''
				END

				EXEC DO_CheckRuleForOffer 
				@OfferId = @OfferId,
				@CityId	= NULL,
				@ZoneId	= @TempZone,
				@MakeId	= @MakeId,
				@ModelId = @ModelId,
				@VersionId = @VersionId,
				@StartDate = @StartDate,
				@EndDate = @EndDate,
				@IsNewOffer = @Status OUTPUT 
		END		
END
