IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_GetCustomerForPost]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_GetCustomerForPost]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_GetCustomerForPost]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
@PostId NUMERIC(18,0),
@CustomerId NUMERIC(18,0) OUTPUT

AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
SELECT @CustomerId=CustomerId FROM ForumThreads FT  WHERE FT.Id= @PostId
END 
       
