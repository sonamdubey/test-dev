IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_SaveCarSurveyorMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_SaveCarSurveyorMapping]
GO

	-- =============================================
-- Author      : Chetan Navin		
-- Create date : 22 Dec 2015 
-- Description : To save absure carmapping data.
-- Modifier	   : 1 . Ruchira Patil (26th Feb 2015) - made @AbSure_CarDetailsId parameter as a comma seperated value
-- Modifier	   : 2 . Ruchira Patil (27th Feb 2015) - called Absure_GetCarsforAreaDealers SP to fetch car ids based on areid and dealerid
-- Modifier	   : 3 . Chetan Navin (10th Mar 2015) - made @AbSure_CarDetailsId parameter to handle null values on appending
-- EXEC Absure_SaveCarSurveyorMappingTest 13240,NULL,36,NULL,13175,0,2
-- =============================================
CREATE PROCEDURE [dbo].[Absure_SaveCarSurveyorMapping] 
	@TC_UserId				BIGINT,
	@AbSure_CarDetailsId	VARCHAR(MAX) = NULL, -- Selected car ids
	@AreaId					VARCHAR(MAX) = NULL, -- Selected area ids
	@DealerId				VARCHAR(MAX) = NULL,  -- Selected dealer ids
	@UpdatedBy				INT = NULL,
	@IsSurveyDone			BIT = NULL,
	@PendingStatus			INT = NULL
AS

BEGIN
		DECLARE @TblTempCars TABLE (Id INT IDENTITY(1,1),CarId INT)
		DECLARE @BranchId BIGINT,@StockId BIGINT, @CarIds VARCHAR(MAX)

		--CASE 1: Area/s are selected (i/p: AreaId)
		IF @AreaId IS NOT NULL AND @DealerId IS NULL AND @AbSure_CarDetailsId IS NULL
		BEGIN
			EXEC Absure_GetCarsforAreaDealers NULL,@AreaId ,@IsSurveyDone, @UpdatedBy ,@PendingStatus, @CarIds = @CarIds OUTPUT
		END
		
		--CASE 2: Dealer/s are selected (i/p: AreaId, DealerId)
		IF @DealerId IS NOT NULL AND @AbSure_CarDetailsId IS NULL
		BEGIN
			EXEC Absure_GetCarsforAreaDealers @DealerId,@AreaId ,@IsSurveyDone, @UpdatedBy ,@PendingStatus, @CarIds = @CarIds OUTPUT
		END
		
		--CASE 3: Car/s are selected (i/p: AreaId, DealerId, CarId) + CASE : 2 + CASE 1
		INSERT INTO @TblTempCars (CarId) SELECT ListMember FROM fnSplitCSV(ISNULL(@AbSure_CarDetailsId,'') + ','+ ISNULL(@CarIds,''))

		DECLARE @TempCar	INT,
				@i			TINYINT = 1,
				@CarCounter	INT

		SELECT @CarCounter = COUNT(CarId) FROM @TblTempCars

		WHILE @CarCounter > 0
		BEGIN
			SELECT @TempCar = CarId FROM @TblTempCars WHERE ID = @i
			SET @i = @i + 1
			SET @CarCounter = @CarCounter - 1

			SELECT	@BranchId = ISNULL(DealerId,@BranchId),@StockId = StockId,
					@UpdatedBy = ISNULL(@UpdatedBy,@TC_UserId)
			FROM	AbSure_CarDetails WITH(NOLOCK) 
			WHERE	Id = @TempCar

			IF NOT EXISTS(SELECT AbSure_CarDetailsId FROM AbSure_CarSurveyorMapping  WITH(NOLOCK) WHERE AbSure_CarDetailsId = @TempCar)
				BEGIN
					INSERT INTO AbSure_CarSurveyorMapping(BranchId,TC_UserId,AbSure_CarDetailsId,TC_StockId) 
					VALUES (@BranchId,@TC_UserId,@TempCar,@StockId)
				END
			ELSE
				BEGIN
					UPDATE AbSure_CarSurveyorMapping SET TC_UserId = @TC_UserId,UpdatedBy = @UpdatedBy,UpdatedOn = GETDATE()
					WHERE AbSure_CarDetailsId = @TempCar
				END

			IF (SELECT ISNULL(IsAgency,0) FROM TC_Users  WITH(NOLOCK) WHERE Id=@TC_UserId) = 1 --Agency Assigned
				BEGIN
					EXEC	[AbSure_UpdateStatus] 
							@AbSure_CarDetailsId	= @TempCar,
							@Status					= 6,
							@ModifiedBy				= -1

				END
			ELSE	--Surveyor Assigned
				BEGIN		
					EXEC	[AbSure_UpdateStatus] 
							@AbSure_CarDetailsId	= @TempCar,
							@Status					= 5,
							@ModifiedBy				= -1
				END
		END

	
END
