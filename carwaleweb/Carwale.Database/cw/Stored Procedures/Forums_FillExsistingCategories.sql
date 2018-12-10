IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_FillExsistingCategories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_FillExsistingCategories]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_FillExsistingCategories]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
 @ThreadId NUMERIC(18,0)
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
SELECT Topic, ForumSubCategoryId CategoryId
FROM Forums 
WHERE Id = @ThreadId 
END 
