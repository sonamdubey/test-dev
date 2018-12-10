IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UCRGetRating]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UCRGetRating]
GO

	-- =============================================
-- Author:		Vishal Srivastava
-- Create date: 3 Febuary 2014 1125 HRS IST
-- Description:	stock rating calculation.
-- EXEC TC_UCRGetRating '4277,4276,4271,4270,4264,4263,4262,4261,4260,4259,4258,4257,4253,4248,4247,4246,4245,4242,4241,4240,'
-- Modified By Vishal Srivastava AE1830 on 08-05-2014 1915 HRS IST:changed All multiplication of weight and attribute to addition
-- Modified By Vivek Gupta on 17/06/2014, Changed photocount check from > to >=
-- =============================================
CREATE PROCEDURE [dbo].[TC_UCRGetRating]
@stockId VARCHAR(MAX)
	-- Add the parameters for the stored procedure here
AS
BEGIN
	DECLARE @ratingTable TABLE (StockId INT, Rating Float)
	DECLARE @Separator VARCHAR(1)=','
	DECLARE @Separator_position INT -- This is used to locate each separator character  
	DECLARE @array_value INT -- this holds each array value as it is returned 
	DECLARE @finalRating FLOAT

  
	CREATE TABLE #Table(StockId INT,KiloMeters INT,CarAge INT,PhotoCount INT,CityRating FLOAT,ModelRating FLOAT)
	INSERT INTO #Table
	-- Select statements for rating of city and model, photo count, age , and kms
		SELECT 
			T.Id AS StockId,
			T.Kms AS KiloMeters,
			DATEDIFF(YEAR, T.MakeYear, GETDATE()) AS CarAge,
			CP.PhotoCount AS PhotoCount,
			C.UsedCarRating AS CityRating,
			CM.UsedCarRating AS ModelRating
		FROM TC_Stock AS T WITH(NOLOCK)
		INNER JOIN Dealers AS D WITH(NOLOCK)
				ON D.ID=T.BranchId
		INNER JOIN CarVersions AS CV WITH(NOLOCK)
				ON CV.ID=T.VersionId
		INNER JOIN CarModels AS CM WITH(NOLOCK)
				ON CM.ID=CV.CarModelId
		INNER JOIN Cities AS C WITH(NOLOCK)
				ON C.ID=D.CityId
		LEFT OUTER JOIN  
				(SELECT stockid,count(id)  PhotoCount FROM TC_CarPhotos  WITH(NOLOCK) WHERE stockid IN (SELECT * FROM fnSplitCSV(@stockId)) GROUP BY stockid  ) AS CP 
		               ON T.Id=CP.StockId
		WHERE T.Id IN(SELECT * FROM fnSplitCSV(@stockId))

		
	DECLARE @cityRating FLOAT, @modelRating FLOAT, @ageRating FLOAT, @KMRating FLOAT, @photoRating FLOAT
	    DECLARE @KM INT, @age INT, @photo INT
	WHILE PATINDEX('%' + @Separator + '%', @stockId) <> 0 
	BEGIN 
		SELECT  @Separator_position = PATINDEX('%' + @Separator + '%',@stockId)  
        SELECT  @array_value = CONVERT(INT,LEFT(@stockId, @Separator_position - 1))
		
		
		SELECT @modelRating=ModelRating, @cityRating=CityRating, @KM=KiloMeters, @age=CarAge, @photo=ISNULL(PhotoCount,0) from #Table
		WHERE StockId = @array_value

		

		IF(@KM=0)
			SET  @KMRating = 0
		ELSE IF(@KM>(SELECT MaxKilometer FROM TC_UCRKilometerRating WHERE TC_UCRKilometerRatingId=5))
			SET  @KMRating =  0.1113
		ELSE
			BEGIN
				SELECT @KMRating = Rating FROM TC_UCRKilometerRating WHERE MaxKilometer >= @KM AND MinKilometer < @KM 
			END

		
	
		IF(@age=0)
			SET  @ageRating = 0.3171342025
		ELSE
			BEGIN
				SELECT @ageRating = Rating FROM TC_UCRAgeRating WHERE MaxAge >= @age AND MinAge < @age 
			END

		

		IF(@photo=0)
			SET  @photoRating = 0.2082
		ELSE
			BEGIN
				SELECT @photoRating = Rating FROM TC_UCRPhotoRating WHERE MaxPhoto >= @photo AND MinPhoto < @photo 
			END

		

		-- Modified By Vishal Srivastava AE1830 on 08-05-2014 1915 HRS IST:changed All multiplication of weight and attribute to addition
		SET @finalRating=ISNULL(@ageRating,0)*0.2+ISNULL(@photoRating,0)*0.3+ISNULL(@KMRating,0)*0.3+ISNULL(@cityRating,0)*0.1+ISNULL(@modelRating,0)*0.1

		INSERT INTO @ratingTable (StockId, Rating)VALUES(@array_value,ISNULL(@finalRating, 0))
		EXEC TC_UCRInsertRating @array_value,@finalRating
		-- This replaces what we just processed with and empty string  
        SELECT  @stockId = STUFF(@stockId, 1, @Separator_position, '')  
	END
	SELECT StockId AS Id,Rating AS Rating FROM @ratingTable
	DROP TABLE #Table
END

