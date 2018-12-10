IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[AutoExpo_GetMakeDropDownData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[AutoExpo_GetMakeDropDownData]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <7/1/2014>      
-- Description: <Returns list for Car Makes To populate the drop down.> 
-- =============================================      
CREATE procedure [cw].[AutoExpo_GetMakeDropDownData]      -- execute cw.AutoExpo_GetMakeDropDownData
 -- Add the parameters for the stored procedure here      
       
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;  
     
select DISTINCT CM.Name , CM.ID
from CarMakes AS CM WITH(NOLOCK) 
INNER JOIN Con_EditCms_Cars AS CES WITH(NOLOCK) ON CES.MakeId = CM.ID
INNER JOIN Con_EditCms_Basic AS CEB WITH(NOLOCK) ON CEB.Id = CES.BasicId
WHERE CEB.CategoryId = 9 AND CEB.IsActive = 1 AND CEB.IsPublished = 1 AND YEAR(CEB.PublishedDate)>=2013
 			

END

