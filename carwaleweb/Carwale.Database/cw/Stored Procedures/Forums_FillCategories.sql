IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_FillCategories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_FillCategories]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_FillCategories]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
SELECT FC.Name,FSC.Name Category, FSC.Id 
FROM ForumCategories FC, ForumSubCategories FSC 
WHERE FC.Id=FSC.ForumCategoryId AND FSC.IsActive=1 AND FC.IsActive=1 
ORDER BY Category 
END 
