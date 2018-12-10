IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_SaveIHData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_SaveIHData]
GO

	CREATE Procedure [dbo].[Con_SaveIHData]
	@ID					NUMERIC,
	@WeekName			VARCHAR(150),
	@IH					VARCHAR(5000),
	@LastMonthVisits	VARCHAR(50),
	@LastWeekVisits		VARCHAR(50),
	@PQCount			VARCHAR(50),
	@MostResearchedCar	VARCHAR(50),
	@UsedCarsCount		VARCHAR(50),
	@ForumActivityCount	VARCHAR(50),
	@RoadTestCars		VARCHAR(500),
	@CreatedOn			DATETIME,
	@Status				BIGINT OUTPUT

AS
	
BEGIN
	SET @Status = 0
	IF @ID = -1
		BEGIN
			INSERT INTO Con_IHData
			(
				WeekName, IH, LastMonthVisits, LastWeekVisits, PQCount,
				MostResearchedCar, UsedCarsCount, ForumActivityCount,
				RoadTestCars, CreatedOn
			) 
			VALUES 
			(
				@WeekName, @IH, @LastMonthVisits, @LastWeekVisits, @PQCount,
				@MostResearchedCar, @UsedCarsCount, @ForumActivityCount,
				@RoadTestCars, @CreatedOn
			)
		
			SET @Status = SCOPE_IDENTITY() 
		END

	ELSE

		BEGIN
			UPDATE Con_IHData
			SET IH = @IH, LastMonthVisits = @LastMonthVisits, LastWeekVisits = @LastWeekVisits, 
				PQCount = @PQCount, MostResearchedCar = @MostResearchedCar, 
				UsedCarsCount = @UsedCarsCount, ForumActivityCount = @ForumActivityCount,
				RoadTestCars = @RoadTestCars
			WHERE ID = @ID

			SET @Status = @ID 
		END
END


