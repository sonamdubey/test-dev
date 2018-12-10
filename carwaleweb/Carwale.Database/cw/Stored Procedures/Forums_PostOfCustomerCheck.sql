IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_PostOfCustomerCheck]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_PostOfCustomerCheck]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_PostOfCustomerCheck]      -- execute cw.Forums_CheckStickyThreads 1278
 -- Add the parameters for the stored procedure here      
 @ThreadId NUMERIC(18,0)
 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
UPDATE ForumThreads SET IsActive=0 WHERE ID = @ThreadId

 
END 


