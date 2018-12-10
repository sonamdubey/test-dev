IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[RoadTestsAll]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[RoadTestsAll]
GO

	      
-- =============================================      
-- Author:  <Prashant Vishe>      
-- Create date: <15 Oct 2012>      
-- Description: <To view Contents of all road test pages... >   
-- Modified By: Akansha on 10.4.2014
-- Description : Added Masking Name Column
   -- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
CREATE PROCEDURE [cw].[RoadTestsAll]      
 -- Add the parameters for the stored procedure here      
 @BasicId Numeric,    
 @ApplicationId INT
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
      
Select Distinct(B.AuthorName), B.Title, B.Url, B.DisplayDate, CMA.Name As Make, CMO.Name As Model,CMO.MaskingName, CV.Name As Version,   
C.ModelId, C.VersionId,   
Case When C.VersionId = -1 Then CMA.Name + ' ' + CMO.Name  Else CMA.Name + ' ' + CMO.Name + ' ' + CV.Name End As Car,   
CA.Name As Category,CF.ValueType,( Cast( P.Priority As VarChar(10) ) + '. ' + P.PageName ) As PageNameForDDL, P.PageName, P.Priority, PC.Data, SC.Name As SubCategory   
From Con_EditCms_Basic B  WITH(NOLOCK)  
 Left Join Con_EditCms_BasicSubCategories BSC WITH(NOLOCK)  On BSC.BasicId = B.Id   
 Left Join Con_EditCms_SubCategories SC WITH(NOLOCK)  On SC.Id = BSC.SubCategoryId And SC.IsActive = 1   
 Left Join Con_EditCms_Cars C WITH(NOLOCK)  On C.BasicId = B.Id And C.IsActive = 1   
 Left Join Con_EditCms_OtherInfo OI WITH(NOLOCK)  On OI.BasicId = B.Id   
 Left Join Con_EditCms_CategoryFields CF WITH(NOLOCK)  On CF.Id = OI.CategoryFieldId And B.CategoryId = CF.CategoryId   
 Left Join Con_EditCms_Category Ca WITH(NOLOCK)  On Ca.Id = B.CategoryId   
 Left Join Con_EditCms_Pages P WITH(NOLOCK) On P.BasicId = B.Id   
 Left Join Con_EditCms_PageContent PC WITH(NOLOCK)  On PC.PageId = P.Id   
 Left JOIN CarMakes CMA WITH(NOLOCK)  On C.MakeId = CMA.ID   
 Left JOIN CarModels CMO WITH(NOLOCK) On C.ModelId = CMO.ID   
 Left JOIN CarVersions CV WITH(NOLOCK)  On C.VersionId = CV.ID   
Where B.ID = @BasicId AND B.IsActive = 1 AND B.IsPublished=1 AND B.ApplicationID = @ApplicationId
Order By P.Priority ;  
  
Select ShowGallery From Con_EditCms_Basic WITH(NOLOCK)  Where Id = @BasicId  
        
END 

