IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelPhotos_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelPhotos_V15]
GO

	-- =============================================    
-- Author:  Vikas    
-- Create date: 19-11-2012    
-- Description: Get the model Images from Photo Gallery Section in EditCms    
-- Modified By : Akansha on 7.2.2014
-- Description : Added Masking Name column
-- exec [dbo].[GetModelPhotos]     10,353
-- Modified by : Supriya Khartode to add applicationId filter
-- =============================================    
CREATE PROCEDURE [dbo].[GetModelPhotos_V15.8.1]     
 -- Add the parameters for the stored procedure here    
 @MakeId NUMERIC,     
 @ModelId NUMERIC,
 @ApplicationId Int    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    -- Insert statements for procedure here    
 with CTE
as
(
SELECT   
      -- ROW_NUMBER() OVER( ORDER BY IsMainImage DESC ,CEI.Id) AS Row,  
	     ROW_NUMBER() OVER( ORDER BY IsMainImage DESC ,CEI.Sequence) AS Row, 

  CEI.Id,  
  CEI.HostUrl,     
  ImageName,        
  IsMainImage,  
  Cma.Name As MakeName,  
  Cmo.Name As ModelName ,
  Cmo.MaskingName,
  CEI.Sequence,
  CEI.OriginalImgPath     
    FROM     
  Con_EditCms_Images CEI    
  INNER JOIN Con_EditCms_Basic CB ON CB.Id = CEI.BasicId   
  INNER JOIN CarModels Cmo On Cmo.ID = CEI.ModelId  
  INNER JOIN CarMakes Cma On Cma.ID = CEI.MakeId    
    WHERE     
  CB.CategoryId = 10 -- Category Id for "Photo Gallery" in EditCms   
  AND CB.IsPublished = 1    
  AND CEI.MakeId = @MakeId     
  AND CEI.ModelId = @ModelId  
  AND CEI.IsActive = 1     
  AND CB.ApplicationID = @ApplicationId
  --ORDER BY     
  --CEI.IsMainImage DESC,     
  --CEI.Id  
  )   
  SELECT Row,Id,HostUrl,ImageName,IsMainImage,MakeName,ModelName,Sequence,MaskingName,OriginalImgPath
  FROM CTE
  WHERE ismainimage=1
END


