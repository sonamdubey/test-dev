IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCompareCarsWidget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCompareCarsWidget]
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
CREATE PROCEDURE [dbo].[GetCompareCarsWidget] -- exec GetCompareCarsWidget 4
  -- Add the parameters for the stored procedure here      
  @DisplayCount SMALLINT
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from      
      -- interfering with SELECT statements.      
      SET nocount ON;

      SELECT TOP (@DisplayCount) CCL.versionid1              AS Car1,
                                 CCL.versionid2              AS Car2,
                                 CMA.name + ' ' + CMO.name   AS CarName1,
                                 CMAK.name + ' ' + CMOD.name AS CarName2,
                                 CMA.name                    MakeName1,
                                 CMO.name                    ModelName1,
                                 CMAK.name + ' ' + CMOD.name AS CarName2,
                                 CMAK.name                   MakeName2,
                                 CMOD.name                   ModelName2
                                 --,NP1.AvgPrice Price1      
                                 --,NP2.AvgPrice Price2      
                                 ,
                                 SP1.price                   Price1,
                                 SP2.price                   Price2,
                                 CMO.reviewrate              ReviewRate1,
                                 CMOD.reviewrate             ReviewRate2,
                                 CMO.reviewcount             ReviewCount1,
                                 CMOD.reviewcount            ReviewCount2,
                                 CCL.hosturl,
                                 ( CCL.hosturl + '/' + CCL.imagepath + '/'
                                   + CCL.imagename )         AS ImgPath
                                 --added by Prashant Vishe  
                                 ,
                                 CCL.displaypriority,
                                 Row_number()
                                   OVER(
                                     ORDER BY CASE WHEN CCL.displaypriority IS
                                   NULL
                                   THEN 100000 ELSE
                                   CCL.displaypriority END)  RowNo,
								   CMO.MaskingName as MaskingName1,
								   CMOD.MaskingName as MaskingName2
      FROM   con_carcomparisonlist CCL
             INNER JOIN carversions CV
                     ON CCL.versionid1 = CV.id
             INNER JOIN carmodels CMO
                     ON CMO.id = CV.carmodelid
             INNER JOIN carmakes CMA
                     ON CMO.carmakeid = CMA.id
             INNER JOIN carversions CVE
                     ON CCL.versionid2 = CVE.id
             INNER JOIN carmodels CMOD
                     ON CMOD.id = CVE.carmodelid
             INNER JOIN carmakes CMAK
                     ON CMOD.carmakeid = CMAK.id             
             INNER JOIN newcarshowroomprices SP1
                     ON SP1.carversionid = CCL.versionid1--CMO.CarVersionID_Top //25-03-2015 Rohan Sapkal, Price for the versions being compared
                        AND SP1.cityid = 10
             INNER JOIN newcarshowroomprices SP2
                     ON SP2.carversionid = CCL.versionid2--CMOD.CarVersionID_Top //25-03-2015 Rohan Sapkal, Price for the versions being compared
                        AND SP2.cityid = 10      
      WHERE  CCL.isactive = 1
             AND isarchived = 0
      --  and IsArchived = 0  added by prashant Vishe  
      ORDER  BY rowno

  END  
