IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DWModelFeatureView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DWModelFeatureView]
GO

	
-------------------------------------------------------------
-- =============================================
-- Author:		Mihir A Chheda
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Modified by:Komal Manjare on 7 AUGUST 2015
--OriginalImgPath and HostUrl fetched 
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_DWModelFeatureView]
@DealerId INT,
@MakeId INT=NULL,
@ModelId INT=NULL,
@CategoryId INT=NULL               
AS      
BEGIN      
   SET NOCOUNT ON;      
 
   SELECT   mdf.Id AS Id,CONVERT(varchar,CMK.Name+' '+DM.DWModelName) AS CarName,mmfc.CategoryName,mdf.FeatureTitle,mdf.FeatureDescription,mdf.SortOrder,mdf.IsActive,cmk.ID AS MakeId,dm.ID AS DWModeld,mdwm.ID AS DWCategoryId,(mdf.HostUrl+mdf.ImgPath+mdf.ImgName)AS ImgPath,mdf.HostUrl,mdf.OriginalImgPath
   FROM     Microsite_DWModelFeatures mdf 
   JOIN     Microsite_DWModelFeatureCategories mdwm ON mdwm.Id=mdf.Microsite_DWModelFeatureCategoriesId
   JOIN     Microsite_ModelFeatureCategories mmfc ON mmfc.Id=mdwm.Microsite_ModelFeatureCategoriesId
   JOIN     TC_DealerModels DM ON dm.ID=mdwm.DWModelId
   JOIN     CarModels CM ON CM.ID=DM.CWModelId
   JOIN     CarMakes CMK ON CMK.ID=CM.CarMakeId
   WHERE    DM.DealerId=@DealerId AND (@MakeId Is NULL OR CMK.ID=@MakeId) AND (@ModelId IS NULL OR DM.ID=@ModelId) AND (@CategoryId IS NULL OR mdwm.ID=@CategoryId)
   ORDER BY CarName DESC 
        
END 