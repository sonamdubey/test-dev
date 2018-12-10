IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCompareCarsWidget_v16_7_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCompareCarsWidget_v16_7_1]
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
-- Modified By : Ajay Singh on 1-July-2016 fetched MinAvgPrice from carmodels  
CREATE PROCEDURE [dbo].[GetCompareCarsWidget_v16_7_1] -- exec [dbo].[GetCompareCarsWidget_15.8.1] 4
  -- Add the parameters for the stored procedure here      
  @DisplayCount SMALLINT
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from      
      -- interfering with SELECT statements.      
      SET nocount ON;

      SELECT TOP (@DisplayCount)
	     CCL.versionid1 AS VersionId1
		,CCL.versionid1              AS Car1
		,CCL.versionid2 AS VersionId2
		,CCL.versionid2              AS Car2
		,CMA.name + ' ' + CMO.name   AS CarName1
        ,CMAK.name + ' ' + CMOD.name AS CarName2
		,CMA.NAME MakeName1
		,CMO.NAME ModelName1
		,CMO.Id ModelId1
		,CMAK.NAME 
		,CMAK.NAME MakeName2
		,CMOD.NAME ModelName2
		,CMOD.Id ModelId2
		,SP1.AvgPrice AS Price1 --added by ajay singh on 1 july 2017
		,SP2.AvgPrice AS Price2 --added by ajay singh on 1 july 2017
		,CMO.reviewrate ReviewRate1
		,CMOD.reviewrate ReviewRate2
		,CMO.reviewcount ReviewCount1
		,CMOD.reviewcount ReviewCount2
		,CCL.HostURL
		,CCL.ImageName 
		,CCL.OriginalImgPath
		,( CCL.hosturl + '/' + CCL.OriginalImgPath + '/'
                                   + CCL.imagename )         AS ImgPath
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
			 INNER JOIN Con_NewCarNationalPrices SP1 WITH(NOLOCK)
                     ON SP1.VersionId = CCL.versionid1--CMO.CarVersionID_Top //25-03-2015 Rohan Sapkal, Price for the versions being compared added by ajay singh
             INNER JOIN Con_NewCarNationalPrices SP2 WITH(NOLOCK)
                     ON SP2.VersionId = CCL.versionid2--CMOD.CarVersionID_Top //25-03-2015 Rohan Sapkal, Price for the versions being company added by ajay singh                         
					    
      WHERE  CCL.isactive = 1
             AND isarchived = 0
      ORDER  BY rowno

  END    


