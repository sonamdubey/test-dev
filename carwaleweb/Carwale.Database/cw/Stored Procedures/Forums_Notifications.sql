IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_Notifications]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_Notifications]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Get the name and email of people who have subscribed to this thread> 
-- =============================================      
CREATE procedure [cw].[Forums_Notifications]      -- execute cw.Forums_Notifications 777,1
 -- Add the parameters for the stored procedure here      
 @ForumThreadId NUMERIC(18,0),      
 @CustomerId NUMERIC(18,0)
    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
 SELECT C.Name, C.Email 
 FROM Customers AS C 
 WHERE C.IsFake=0 AND C.ID IN (SELECT CustomerId FROM ForumSubscriptions WHERE EmailSubscriptionId=2 AND ForumThreadId = @ForumThreadId)
 AND C.Id <> @CustomerId
END 



