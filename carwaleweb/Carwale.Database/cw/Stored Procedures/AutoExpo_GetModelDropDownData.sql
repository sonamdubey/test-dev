IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[AutoExpo_GetModelDropDownData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[AutoExpo_GetModelDropDownData]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <7/1/2014>      
-- Description: <Returns list for Car Models To populate the drop down.> 
-- =============================================      
CREATE procedure [cw].[AutoExpo_GetModelDropDownData]      -- execute cw.AutoExpo_GetModelDropDownData 22
 -- Add the parameters for the stored procedure here      
  @MakeId INT     
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;  
     
select CM.Name,CM.ID 
from CarModels AS CM WITH(NOLOCK) 
INNER JOIN Con_EditCms_Cars AS CES WITH(NOLOCK) ON CES.ModelId = CM.ID
INNER JOIN Con_EditCms_Basic AS CEB WITH(NOLOCK) ON CEB.Id = CES.BasicId
WHERE CEB.CategoryId = 9 AND CEB.IsActive = 1 AND CEB.IsPublished = 1 AND YEAR(CEB.PublishedDate)>=2013 
AND CM.CarMakeId = @MakeId
 			

END

