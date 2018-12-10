IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_GetHandleName]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_GetHandleName]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Get a user's Handle Name.'> 
-- =============================================      
CREATE procedure [cw].[Forums_GetHandleName]      -- execute cw.Forums_GetHandleName 1278
 -- Add the parameters for the stored procedure here      
 @UserId NUMERIC(18,0)
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
 SELECT HandleName, IsUpdated FROM UserProfile WHERE UserID = @UserId
 
END 
            

