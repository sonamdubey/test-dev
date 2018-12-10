IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_CheckStickyThreads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_CheckStickyThreads]
GO

	
-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE PROCEDURE [cw].[Forums_CheckStickyThreads]      -- execute cw.Forums_CheckStickyThreads 1278
 -- Add the parameters for the stored procedure here      
 @CreatedBy NUMERIC(18,0),
 @ThreadId NUMERIC(18,0)

 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
SELECT ID FROM Forum_StickyThreads WHERE CreatedBy = @CreatedBy AND ThreadId = @ThreadId
 
END 
       



