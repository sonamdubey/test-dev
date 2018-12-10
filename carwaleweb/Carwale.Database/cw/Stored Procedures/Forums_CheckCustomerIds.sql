IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_CheckCustomerIds]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_CheckCustomerIds]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_CheckCustomerIds]      -- execute cw.Forums_CheckCustomerIds '3597,9265,9266'
 -- Add the parameters for the stored procedure here      
 @PostIds VARCHAR (MAX)
 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
 
 
 SELECT DISTINCT CustomerId 
 FROM ForumThreads as F with (nolock), 
  [dbo].[fnSplitCSV](@PostIds)
 WHERE ID =ListMember
 
END 



