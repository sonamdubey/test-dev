IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_MovePost]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_MovePost]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_MovePost]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
@Topic VARCHAR(200),
@ForumSubCategoryId NUMERIC(18,0),
@ForumId NUMERIC(18,0)

AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
UPDATE Forums SET Topic = @Topic, ForumSubCategoryId = @ForumSubCategoryId WHERE ID = @ForumId
END 
       
