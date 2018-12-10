IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCompareCarsWidget-v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCompareCarsWidget-v16]
GO

	
-- =============================================      
-- Author:  Reshma Shetty      
-- Create date: 17/12/2012      
-- Description: Car comparison details.      
-- Modified By Vikas -- Retrieval of Avg Price instead of Min Price. 
--Modified By Prashant Vishe  --     Retrieval of ImgPath and added IsArchived = 0 
-- =============================================     
-- Modified By : Ashish G. Kamble on 17 July 2013
-- Description : Prices are referred from new delhi. Con_NewCarNationalPrices table is ignored.
-- =============================================      
-- =============================================     
-- Modified By : Akansha on 4.2.2014
-- Description : Added 2 new columns for Masking Name of the car model
-- =============================================      
-- Modified By : Ashwini Todkar 22 July 2015 Removed unneccesary select column
-- Modified By : Ajay Singh on  11 may 2016 removed the hard coded city=10 and fetched the city from carmodel for the corrosponding model of the version
CREATE PROCEDURE [dbo].[GetCompareCarsWidget-v16.5.4] -- exec [dbo].[GetCompareCarsWidget-v16.5.4] 4

  -- Add the parameters for the stored procedure here      
  @DisplayCount SMALLINT
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from      
      -- interfering with SELECT statements.      
      SET nocount ON;

      SELECT TOP (@DisplayCount) CCL.versionid1 AS VersionId1
		,CCL.versionid2 AS VersionId2
		--,CMA.NAME --+ ' ' + CMO.NAME AS CarName1
		--,CMAK.NAME --+ ' ' + CMOD.NAME AS CarName2
		,CMA.NAME MakeName1
		,CMO.NAME ModelName1
		,CMO.Id ModelId1
		,CMAK.NAME --+ ' ' + CMOD.NAME AS CarName2
		,CMAK.NAME MakeName2
		,CMOD.NAME ModelName2
		,CMOD.Id ModelId2
		--,NP1.AvgPrice Price1      
		--,NP2.AvgPrice Price2      
		,SP1.price Price1 
	    ,SP2.price Price2
		--,CMOD.MinPrice Price1
		--,CMOD.MinPrice Price2
		,CMO.reviewrate ReviewRate1
		,CMOD.reviewrate ReviewRate2
		,CMO.reviewcount ReviewCount1
		,CMOD.reviewcount ReviewCount2
		,CCL.HostURL
		,CCL.ImageName 
		,CCL.OriginalImgPath
		--,(CCL.hosturl + '/' + CCL.imagepath + '/' + CCL.imagename) AS ImgPath
		--added by Prashant Vishe  
		,CCL.displaypriority
		,Row_number() OVER (
			ORDER BY CASE 
					WHEN CCL.displaypriority IS NULL
						THEN 100000
					ELSE CCL.displaypriority
					END
			) RowNo
		,CMO.MaskingName AS MaskingName1
		,CMOD.MaskingName AS MaskingName2
      FROM   con_carcomparisonlist CCL WITH(NOLOCK)
             INNER JOIN carversions CV WITH(NOLOCK)
                     ON CCL.versionid1 = CV.id
             INNER JOIN carmodels CMO WITH(NOLOCK)
                     ON CMO.id = CV.carmodelid
             INNER JOIN carmakes CMA WITH(NOLOCK)
                     ON CMO.carmakeid = CMA.id
             INNER JOIN carversions CVE WITH(NOLOCK)
                     ON CCL.versionid2 = CVE.id
             INNER JOIN carmodels CMOD WITH(NOLOCK)
                     ON CMOD.id = CVE.carmodelid
             INNER JOIN carmakes CMAK WITH(NOLOCK)
                     ON CMOD.carmakeid = CMAK.id             
            INNER JOIN newcarshowroomprices SP1 WITH(NOLOCK)  
                    ON SP1.carversionid = CCL.versionid1--CMO.CarVersionID_Top //25-03-2015 Rohan Sapkal, Price for the versions being compared
                       AND SP1.cityid = (SELECT PriceCityId from CarModels WITH(NOLOCK) Where ID =CMO.Id)
             INNER JOIN newcarshowroomprices SP2 WITH(NOLOCK)
                    ON SP2.carversionid = CCL.versionid2--CMOD.CarVersionID_Top //25-03-2015 Rohan Sapkal, Price for the versions being compared
                       AND SP2.cityid = (SELECT PriceCityId from CarModels WITH(NOLOCK) Where ID = CMOD.Id)      
      WHERE  CCL.isactive = 1
             AND isarchived = 0
      --  and IsArchived = 0  added by prashant Vishe  
      ORDER  BY rowno

  END    


