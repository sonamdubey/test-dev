IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetVersions_BrowsePage_14_11_3]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetVersions_BrowsePage_14_11_3]
GO

	 --=============================================
-- Author: <Reshma Shetty>
-- Create date: <06/03/2013>
-- Description:        <Get the version details to be displayed on the browse car by versions page> EXEC GetVersions_BrowsePage 386
-- Modified by:Reshma Shetty				Date:9/4/2013			Comment:Added a contraint to restrict futuristic versions
-- Modified By : Ashish G. Kamble on 10 July 2013
-- Modified : showing prices from delhi only.
-- Modified By: Shalini on 19/11/14; Also retrieving BodyStyleId,FuelTypeId and TransmissionTypeId
-- Modified by: Manish on 08-09-2016 added with (nolock) keyword.
-- =============================================
CREATE PROCEDURE [dbo].[GetVersions_BrowsePage_14_11_3]
       -- Add the parameters for the stored procedure here
      @ModelId INT,
      @CityId BIGINT = NULL
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

   -- Insert statements for procedure here
       SELECT CV.ID AS ID, CV.Name AS Version, CV.CarModelId AS ModelId,
       CV.SpecsSummary,cmo.MaskingName,
       --AvgPrice AS MinPrice, CV.New, IsNull(CV.ReviewRate, 0) AS ReviewRate,
       CV.New, IsNull(CV.ReviewRate, 0) AS ReviewRate,
       SP.Price AS MinPrice,
       IsNull(CV.ReviewCount, 0) AS ReviewCount,
	    CV.BodyStyleId,CASE  
                         WHEN SD.FuelType = 'Petrol' THEN 1  WHEN SD.FuelType = 'Diesel' THEN 2  WHEN SD.FuelType = 'CNG' THEN 3  
                         WHEN SD.FuelType= 'Electric' THEN 4  END AS FuelTypeId,  CASE  WHEN SD.TransmissionType = 'Automatic' THEN 1 
                         WHEN SD.TransmissionType = 'Manual' THEN 2  END  AS TransmissionTypeId, 
                         CASE WHEN CV.CarFuelType = 1 THEN 'Petrol'
                         WHEN CV.CarFuelType = 2 THEN 'Diesel' 
                         WHEN CV.CarFuelType = 3 THEN 'CNG' 
                         WHEN CV.CarFuelType = 4 THEN 'LPG' 
                         WHEN CV.CarFuelType = 5 THEN 'Electric' 
                         END  
                          AS CarFuelType
       FROM CarVersions AS CV with (nolock)
	   Inner Join carmodels cmo  with (nolock) on cv.CarModelId=cmo.ID
       --LEFT JOIN Con_NewCarNationalPrices NCN ON NCN.VersionId=CV.ID
       LEFT JOIN NewCarShowroomPrices AS SP with (nolock) ON SP.CarVersionId = CV.ID AND SP.CityId = @CityId ,
	    NewCarSpecifications AS SD with (nolock)
       WHERE CV.IsDeleted=0 AND CV.IsSpecsExist=1
       AND CV.Futuristic=0 
       AND CV.CarModelId=@ModelId AND SD.CarVersionId = CV.ID
       ORDER BY MinPrice ASC

	  
END

