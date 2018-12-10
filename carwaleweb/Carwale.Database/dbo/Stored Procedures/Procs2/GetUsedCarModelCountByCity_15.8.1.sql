IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUsedCarModelCountByCity_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUsedCarModelCountByCity_15]
GO

	
-- =============================================      
-- Author:  <Vikas J>
-- Create date: <3/02/2013>      
-- Description: <Returns the data of all the current models available for the provided makeId.Also gives the modelwise and citiwise count of used car and minimum price>
-- Modified: Modified by Reshma on 12-03-2013 Adding extra column cityid to the result set.
-- Modified By: Akansha on 10.4.2014
-- Description : Added Masking Name Column
-- Modified By : Chetan on 07-08-2015 added "CM.OriginalImgPath"
-- =============================================      
CREATE PROCEDURE [dbo].[GetUsedCarModelCountByCity_15.8.1]   -- exec GetUsedCarModelCountByCity 2,1
	@MakeId INT = 0, --Make Id 
	@CityId INT = 0  --City Id 
AS
BEGIN
	
	DECLARE @Latitude FLOAT, @Longitude FLOAT

	SELECT @Latitude=Lattitude,@Longitude=Longitude FROM Cities WHERE ID=@CityId 

	--Returns the data of all the current models available for the provided makeId.Also gives the modelwise and citiwise count of used car and minimum price 
	SELECT CM.ID AS ModelId
		,CM.CarMakeId AS MakeId
		,CM.NAME AS Model
		,CM.New
		,CM.OriginalImgPath
		,CM.HostURL
		,CM.MaskingName
		,COUNT(DISTINCT Inquiryid) AS TotalUsedCars
		,MIN(LL.Price) OldPrice
		,MAX(ISNULL(NCN.CityId,-1)) as CityId
		
	FROM CarVersions CV 
	INNER JOIN CarModels CM ON CV.CarModelId=CM.ID
	LEFT JOIN NewCarShowroomPrices NCN ON NCN.CarVersionId=CV.ID and CityId=@CityId --added to return only those models which have prices for that city
	LEFT JOIN LiveListings LL WITH (NOLOCK) ON CM.ID = LL.ModelId
		AND (
			Lattitude BETWEEN @Latitude - 50 * 32.57940665
				AND @Latitude + 50 * 32.57940665
			)
		AND (
			Longitude BETWEEN @Longitude - 50 * 34.63696611
				AND @Longitude + 50 * 34.63696611
			)
	WHERE CM.CarMakeId = @MakeId
	AND CM.IsDeleted=0
	GROUP BY CM.ID
		,CM.CarMakeId
		,CM.NAME
		,CM.OriginalImgPath
		,CM.HostURL
		,CM.New
		,CM.MaskingName
		

	
END 


