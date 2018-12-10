IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerModelsCategoryMappingView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerModelsCategoryMappingView]
GO

	-- =============================================
-- Author:		Mihir A Chheda
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_DealerModelsCategoryMappingView]
@DealerId INT, 
@MakeId INT=NULL,
@ModelId INT=NULL,
@CategoryId INT=NULL                 
AS      
BEGIN      
   SET NOCOUNT ON;      
 
   SELECT   mdwm.Id AS Id,CONVERT(varchar,CMK.Name+' '+DM.DWModelName) AS CarName,mmfc.CategoryName,mdwm.IsActive,cmk.ID MakeId,DM.ID ModelId,mdwm.Microsite_ModelFeatureCategoriesId CategoryId
   FROM     Microsite_DWModelFeatureCategories mdwm
   JOIN     Microsite_ModelFeatureCategories mmfc ON mmfc.Id=mdwm.Microsite_ModelFeatureCategoriesId
   JOIN     TC_DealerModels DM ON dm.ID=mdwm.DWModelId
   JOIN     CarModels CM ON CM.ID=DM.CWModelId
   JOIN     CarMakes CMK ON CM.CarMakeId=CMK.ID      
   WHERE    DM.DealerId=@DealerId AND (@MakeId Is NULL OR CMK.ID=@MakeId) AND (@ModelId IS NULL OR DM.ID=@ModelId) AND (@CategoryId IS NULL OR mdwm.Microsite_ModelFeatureCategoriesId=@CategoryId)
   ORDER BY CarName DESC   --AND DM.IsDeleted = 0 //Removed by Mihir
        
END 

