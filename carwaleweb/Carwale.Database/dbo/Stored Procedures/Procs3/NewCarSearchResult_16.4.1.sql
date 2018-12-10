IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NewCarSearchResult_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NewCarSearchResult_16]
GO

	
-- =============================================
--Author:Shalini Nair 
--Create date : 08/12/14
--Description : Fetches the New Cars based on the parameters passed 
--Modified By: Shalini Nair on 26/07/2015 
--Modified By: Rakesh Yadav On 31 Dec 2015,@CarMakeIds length in increased upto 150 char
--Modified By: Sachin Bharti (13/04/16)
--Purpose : Change order by from MaxPrice to Minprice
-- =============================================
CREATE PROCEDURE [dbo].[NewCarSearchResult_16.4.1]
	-- Add the parameters for the stored procedure here
	@CarMakeIds varchar(150) = null 
	,@FuelTypeIds VARCHAR(100) = NULL
	,@TransmissionTypeIds VARCHAR(100) = NULL
	,@BodyStyleIds VARCHAR(100) = NULL
	,@MinPrice INT = NULL
	,@MaxPrice INT = NULL
	,@SortCriteria VARCHAR(100) = NULL
	,@SortOrder int = NULL
	,@StartIndex INT 
	,@LastIndex INT 
	,@ExShowroomCityId int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	 WITH CTE5 AS 
	 (
			SELECT DISTINCT MA.ID AS MakeId
				,MA.NAME AS MakeName
				,MO.ID AS ModelId
				,MA.NAME + ' ' + MO.NAME AS CarModel
				,MO.SmallPic
				,MO.LargePic
				,MO.NAME ModelName
				,MO.HostUrl
				,ISNULL(MO.ReviewRate, 0) MoReviewRate
				,ISNULL(MO.ReviewCount, 0) MoReviewCount
				,Mo.MinPrice as MinPrice
				,Mo.MaxPrice as MaxPrice
				,Mo.OriginalImgPath
				--,MIN(ISNULL(SP.Price, 0)) OVER (PARTITION BY MO.ID) AS MinPrice
				--,MAX(ISNULL(SP.Price, 0)) OVER (PARTITION BY MO.ID) AS MaxPrice
				
			FROM CarMakes AS MA WITH(NOLOCK)
				INNER JOIN CarModels AS MO WITH(NOLOCK) ON MA.ID = MO.CarMakeId
				INNER JOIN (
					CarVersions AS CV WITH(NOLOCK) LEFT JOIN NewCarStandardFeatures CD WITH(NOLOCK) ON CD.CarVersionId = CV.Id
					) ON MO.ID = CV.CarModelId
				INNER JOIN CarBodyStyles AS CB WITH(NOLOCK) ON CB.ID = CV.BodyStyleId
				INNER JOIN NewCarSpecifications AS SD WITH(NOLOCK) ON SD.CarVersionId = CV.ID
				INNER JOIN NewCarShowroomPrices AS SP WITH(NOLOCK) ON SP.CarVersionId = CV.ID
				
			WHERE --MA.ID = MO.CarMakeId
				--AND MO.ID = CV.CarModelId
				MO.New =1 
				AND MO.Futuristic =0 
				AND MO.IsDeleted = 0
				AND CV.New = 1
				--AND SP.CarVersionId = CV.ID
				AND SP.CityId = @ExShowroomCityId
				AND CV.IsDeleted = 0
				--AND CB.ID = CV.BodyStyleId
				--AND SD.CarVersionId = CV.ID
				AND(@CarMakeIds is null or MA.Id in (SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@CarMakeIds)))
				AND (
					@FuelTypeIds IS NULL
					OR Cv.CarFuelType IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@FuelTypeIds)
						)
					)
				AND (
					@TransmissionTypeIds IS NULL
					OR Cv.CarTransmission IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@TransmissionTypeIds)
						)
					)
				AND (
					@BodyStyleIds IS NULL
					OR Cv.BodyStyleId IN (
						SELECT listmember
						FROM fnSplitCSVValuesWithIdentity(@BodyStyleIds)
						)
					)
					and (@MinPrice is null and @MaxPrice is null or SP.Price between (@MinPrice) and (@MaxPrice))
			) 

		, TotalCount as (
			SELECT COUNT(ModelId) AS CarCount FROM CTE5)

	SELECT RESULT.Row
			,RESULT.MakeId
			,RESULT.MakeName
			,RESULT.ModelId
			,RESULT.CarModel
			,RESULT.SmallPic
			,RESULT.LargePic
			,RESULT.ModelName
			,RESULT.HostUrl
			,RESULT.MoReviewRate
			,RESULT.MoReviewCount
			,RESULT.MinPrice
			,RESULT.MaxPrice
			,RESULT.OriginalImgPath
			,TotalCount.CarCount
	FROM (
	
		SELECT ROW_NUMBER() OVER (
				ORDER BY
						CASE when @SortCriteria ='1' and @SortOrder =1 then CarModel END DESC,
						CASE when @SortCriteria ='2' AND @SortOrder= 1 then MaxPrice END ASC ,
						CASE when @SortCriteria='3' AND @SortOrder =1 then MoReviewCount END DESC,
						CASE when @SortCriteria='4' then NewId() end,
						
						CASE when @SortCriteria ='1' then CarModel END,
						CASE when @SortCriteria ='2' then MaxPrice END ASC,
						CASE when @SortCriteria='3' then MoReviewCount END,
						
						MinPrice ASC
				)AS Row
			,CTE5.MakeId
			,CTE5.MakeName
			,CTE5.ModelId
			,CTE5.CarModel
			,CTE5.SmallPic
			,CTE5.LargePic
			,CTE5.ModelName
			,CTE5.HostUrl
			,CTE5.MoReviewRate
			,CTE5.MoReviewCount
			,CTE5.MinPrice
			,CTE5.MaxPrice
			,CTE5.OriginalImgPath
		FROM CTE5
		) AS RESULT , TotalCount WHERE RESULT.Row BETWEEN @StartIndex AND @LastIndex 

	 ORDER BY CASE WHEN @SortCriteria = '4' THEN MinPrice END ASC

	end 