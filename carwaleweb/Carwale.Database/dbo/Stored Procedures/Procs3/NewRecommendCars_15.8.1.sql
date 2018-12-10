IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NewRecommendCars_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NewRecommendCars_15]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: <26-08-2013>
-- Description:	<getting recommend cars>
-- Modification: 09/10/2013 by amit verma  (changed filter logic for ABS)
-- Modification: 11/10/2013 by amit verma  (added logic to elimintate deleted version)
-- Modification: 20/05/2014 by amit verma  (return only active cars)
-- Modification: 04/06/2015 by Satish Sharma, Removed hard coded "cars" from image model image url
-- =============================================
/*
DECLARE @CarCount INT = 0
DECLARE @MakeidsOut NVARCHAR(MAX)
EXEC [dbo].[NewRecommendCars] 1,100,10,670000,30,'',1200,9,4,1,8,5,3,6,2,7,'',0,0,0,0,0,0,1,@CarCount OUTPUT,@MakeidsOut OUTPUT
SELECT @CarCount,@MakeidsOut
*/
CREATE PROCEDURE [dbo].[NewRecommendCars_15.8.1]
	-- Add the parameters for the stored procedure here
	@Index INT = 1,
	@PageSize INT = 10,
	@ModelCount INT = 10,
	@Budget INT = NULL,
	@Range Numeric(4,2) = 30,
	@MakeIDs VARCHAR(100) = NULL,
	@MonthlyUsage INT = NULL,
	@FuelEconomy TINYINT = NULL,
	@Performance TINYINT = NULL,
	@Aesthetics TINYINT = NULL,
	@Comfort TINYINT = NULL,
	@DimensionAndSpace TINYINT = NULL,
	@Convenience TINYINT = NULL,
	@Entertainment TINYINT = NULL,
	@Safety TINYINT = NULL,
	@SalesAndSupport TINYINT = NULL,
	@FuelType VARCHAR(100) = NULL,
	@TransMissonType TINYINT = NULL,
	@ChildSafety TINYINT = NULL,
	@Powerwindows TINYINT = NULL,
	@ABS TINYINT = NULL,
	@CentralLocking TINYINT = NULL,
	@AirBags TINYINT = NULL,
	@Preset TINYINT = 1,
	@CarCount INT = NULL OUTPUT,
	@MakeidsOut NVARCHAR(MAX) = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--IF (@MonthlyUsage >= 1000 AND (@FuelType IS NULL OR @FuelType = ''))
	--	SET @FuelType = 2
	DECLARE @Limit tinyint = 7	
	DECLARE @High tinyint = 3
	DECLARE @Low tinyint = 2
	DECLARE @CutOffScore FLOAT
	--SET @Range = 99.9
	SELECT RC.*,CF.FuelType,CS.Name SegmentType
		--,CB.Name BodyStyle
		,CT.Descr TransmissonType,NCP.Price NCPPrice,CV.SpecsSummary
		,(RC.DimensionAndSpace * (SELECT POWER( @DimensionAndSpace,(CASE WHEN @DimensionAndSpace > @Limit THEN @High ELSE 0.5 END)))
			+RC.Comfort *  (SELECT POWER( @Comfort,(CASE WHEN @Comfort > @Limit THEN @High ELSE @Low END)))
			+RC.Performance *  (SELECT POWER( @Performance,(CASE WHEN @Performance > @Limit THEN @High ELSE @Low END)))
			+RC.Convenience *  (SELECT POWER( @Convenience,(CASE WHEN @Convenience > @Limit THEN @High ELSE @Low END)))
			+RC.Safety *  (SELECT POWER( @Safety,(CASE WHEN @Safety > @Limit THEN @High ELSE @Low END)))
			+RC.Entertainment *  (SELECT POWER( @Entertainment,(CASE WHEN @Entertainment > @Limit THEN @High ELSE @Low END)))
			+RC.Aesthetics *  (SELECT POWER( @Aesthetics,(CASE WHEN @Aesthetics > @Limit THEN @High ELSE @Low END)))
			+RC.SalesAndSupport *  (SELECT POWER( @SalesAndSupport,(CASE WHEN @SalesAndSupport > 6 THEN @High ELSE @Low END)))
			+RC.FuelEconomy *  (SELECT POWER( @FuelEconomy,(CASE WHEN @FuelEconomy > @Limit THEN @High ELSE @Low END)))
		)AS SortScore
		,COUNT(ModelID) OVER(PARTITION BY ModelID) VCount
	INTO #CarData
	FROM RecommendCars RC WITH(NOLOCK)
		LEFT JOIN CarVersions CV WITH(NOLOCK) ON RC.Versionid = CV.ID
		LEFT JOIN CarFuelType CF WITH(NOLOCK) ON CV.CarFuelType = CF.FuelTypeId
		LEFT JOIN CarSegments CS WITH(NOLOCK) ON CV.SegmentId = CS.ID
		LEFT JOIN CarTransmission CT WITH(NOLOCK) ON CV.CarTransmission = CT.Id
		LEFT JOIN CarModels CM WITH(NOLOCK) ON RC.Modelid = CM.ID
		--LEFT JOIN CarBodyStyles CB ON CV.BodyStyleId = CB.ID
		LEFT JOIN NewCarShowroomPrices NCP WITH(NOLOCK) ON RC.Versionid = NCP.CarVersionId
	WHERE (NCP.Price BETWEEN @Budget*(1 - @Range/100) AND @Budget)
		AND CM.New = 1
		AND CV.New = 1
		AND CV.IsDeleted = 0		-- Modification: 11/10/2013 by amit verma  (added logic to elimintate deleted version)
		AND NCP.Price IS NOT NULL
		AND NCP.CityId = 10
		AND	(RC.Makeid IN (SELECT items FROM DBO.SplitText(@MakeIDs, ',')) OR @MakeIDs IS NULL OR @MakeIDs = '')
		AND	(CF.FuelTypeId IN (SELECT items FROM DBO.SplitText(@FuelType, ',')) OR @FuelType IS NULL OR @FuelType = '')
		AND	(CT.Id = @TransMissonType OR @TransMissonType IS NULL OR @TransMissonType = 0)
		--AND	(RC.ABS = @ABS OR @ABS IS NULL OR @ABS = 0)		
		AND	((RC.ABS IS NOT NULL AND @ABS = 1 AND RC.ABS != 'No') OR @ABS = 0 OR @ABS IS NULL) --10/09/2013 by amit verma  (changed filter logic for ABS)
		AND	((RC.Powerwindows IS NOT NULL AND @Powerwindows = 1 AND RC.Powerwindows != 'No') OR @Powerwindows = 0 OR @Powerwindows IS NULL)
		AND	((RC.CentralLocking IS NOT NULL AND @CentralLocking = 1 AND RC.CentralLocking != 'No') OR @CentralLocking = 0 OR @CentralLocking IS NULL)
		AND	((RC.AirBags IS NOT NULL AND @AirBags = 1 AND RC.AirBags != 'No') OR @AirBags = 0 OR @AirBags IS NULL)
		AND RC.IsActive = 1
	
	SELECT @CutOffScore = 0.9 * MIN(score) from (SELECT DISTINCT ModelID,MAX(SortScore) OVER(PARTITION BY ModelID) Score FROM #CarData) as t
	
	IF(@Index = 1)
		SELECT @CarCount = (SELECT COUNT(DISTINCT ModelID) FROM #CarData)
	ELSE
		SET @CarCount = 0
		
	DECLARE @Models TABLE
	(
		ID INT IDENTITY
		,ModelID INT
		,PriceMax INT
		,PriceMin INT
		,Score FLOAT
		,VCount INT
		,BodyStyleId INT
	)	
	
	INSERT INTO @Models
	SELECT T3.ModelID,T3.PriceMax,T3.PriceMin,T3.Score,T3.VCount,BodyStyle FROM (
	SELECT ROW_NUMBER() OVER (ORDER BY BodyStyle , T2.Score desc) as RowNum,T2.* FROM(
	SELECT DISTINCT 
		T1.ModelID ModelID
		,CM.MaxPrice PriceMax
		,CM.MinPrice PriceMin
		,MAX(SortScore) OVER(PARTITION BY T1.ModelID) Score
		--,T1.VCount VCount
		,COUNT(T1.Versionid) OVER(PARTITION BY T1.ModelID) VCount
		,CASE @Preset
			--WHEN 1 THEN					--Daily Travel
			--	CASE CV.BodyStyleId
			--	WHEN 3 THEN 0			--Hatchback
			--	WHEN 1 THEN 0			--Sedan
			--	WHEN 8 THEN 1			--Station Wagon
			--	WHEN 4 THEN 2			--Minivan/Van
			--	WHEN 6 THEN 2			--SUV/MUV
			--	ELSE 10
			--	END
			--WHEN 2 THEN					--Weekend Trips
			--	CASE CV.BodyStyleId
			--	WHEN 1 THEN 0			--Sedan
			--	WHEN 8 THEN 1			--Station Wagon
			--	WHEN 4 THEN 2			--Minivan/Van
			--	WHEN 6 THEN 2			--SUV/MUV
			--	ELSE 10
			--	END
			--WHEN 3 THEN					--High Performance
			--	CASE CV.BodyStyleId
			--	WHEN 2 THEN 0			--Coupe
			--	WHEN 5 THEN 0			--Convertible
			--	WHEN 3 THEN 1			--Hatchback
			--	WHEN 1 THEN 1			--Sedan
			--	ELSE 10
			--	END
			WHEN 4 THEN					--Utility
				CASE CV.BodyStyleId
				WHEN 6 THEN 0			--SUV/MUV
				WHEN 4 THEN 0			--Minivan/Van
				WHEN 7 THEN 0			--Truck
				WHEN 8 THEN 0			--Station Wagon
				ELSE 10
				END
		
			/*
			3 & 1, 8, 4& 6

			1 , 8, 4 & 6

			2 & 5, 3 & 1
*/		
			--WHEN CV.BodyStyleId = 6 AND @Preset = 1 THEN 0
			--WHEN CV.BodyStyleId = 6 AND @Preset = 1 THEN 0
			--WHEN CV.BodyStyleId = 6 AND @Preset = 1 THEN 0
			--WHEN CV.BodyStyleId = 6 AND @Preset = 1 THEN 0
			
			--WHEN CV.BodyStyleId = 3 AND @Preset = 1 THEN 0
			--WHEN CV.BodyStyleId = 6 AND @Preset = 1 THEN 0
			--WHEN CV.BodyStyleId = 6 AND @Preset = 1 THEN 0
			--WHEN CV.BodyStyleId = 6 AND @Preset = 2 THEN 0
			
			--WHEN CV.BodyStyleId = 6 AND @Preset = 1 THEN 0
			--WHEN CV.BodyStyleId = 6 AND @Preset = 1 THEN 0
			--WHEN CV.BodyStyleId = 6 AND @Preset = 1 THEN 0
			--WHEN CV.BodyStyleId = 6 AND @Preset = 3 THEN 0
			
			--WHEN CV.BodyStyleId = 6 AND @Preset = 4 THEN 0
			--WHEN CV.BodyStyleId = 4 AND @Preset = 4 THEN 0
			--WHEN CV.BodyStyleId = 7 AND @Preset = 4 THEN 0
			--WHEN CV.BodyStyleId = 8 AND @Preset = 4 THEN 0
			ELSE 10
			END  AS BodyStyle
	FROM #CarData T1
	LEFT JOIN CarModels CM WITH(NOLOCK) ON T1.ModelID = CM.ID
	LEFT JOIN CarVersions CV WITH(NOLOCK) ON T1.Versionid = CV.ID AND CV.New = 1 AND CV.IsDeleted = 0	-- Modification: 11/10/2013 by amit verma  (added logic to elimintate deleted version)
	WHERE T1.SortScore >= @CutOffScore
	--LEFT JOIN NewCarShowroomPrices NCP ON CV.ID = NCP.CarVersionId
	--WHERE  NCP.CityId = 10 --T1.SortScore >= @CutOffScore
	--AND CV.New = 1
	) T2) T3 WHERE T3.RowNum BETWEEN ( ((@Index - 1) * @PageSize )+ 1) AND (@Index*@PageSize)
	--ORDER BY Score desc)

	SELECT T.ModelID, CMA.Name MkName,CMO.Name MoName,CMO.HostURL,CMO.OriginalImgPath
	,CMO.Summary
	,VCount
	,Score
	,PriceMax
	,PriceMin
	,BodyStyleId
	,CMO.MaskingName
	FROM @Models T
	LEFT JOIN CarModels CMO WITH(NOLOCK) ON T.modelid = CMO.ID
	LEFT JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID

	SELECT --T1.*
	T1.Modelid,T1.Versionid,CM.Name Makename,CMO.Name Modelname,CV.Name Versionname
			,T1.NCPPrice,T1.SpecsSummary,T1.SortScore,T1.FinalScore,CMO.MaskingName
	FROM @Models T2
	LEFT JOIN #CarData T1 ON  T2.ModelID = T1.ModelID
	LEFT JOIN CarMakes CM WITH(NOLOCK) ON T1.Makeid = CM.ID
	LEFT JOIN CarModels CMO WITH(NOLOCK) ON T1.Modelid = CMO.ID
	LEFT JOIN CarVersions CV WITH(NOLOCK) ON T1.Versionid = CV.ID
	WHERE T1.SortScore >= @CutOffScore
	ORDER BY T1.SortScore DESC,T1.NCPPrice DESC
	
	IF (@Index = 1)
		SELECT @MakeidsOut = COALESCE(@MakeidsOut+',','') + CONVERT(VARCHAR,Makeid) FROM
		(SELECT DISTINCT Makeid FROM #CarData) t
	DROP Table #CarData
END

