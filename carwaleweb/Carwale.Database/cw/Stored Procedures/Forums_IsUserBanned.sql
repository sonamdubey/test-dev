IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_IsUserBanned]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_IsUserBanned]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Checks If a customer is banned.> 
-- =============================================      
CREATE procedure [cw].[Forums_IsUserBanned]      -- execute cw.Forums_IsUserBanned 777,1
 -- Add the parameters for the stored procedure here      
 @CustomerId NUMERIC(18,0)
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
 Select CustomerId From Forum_BannedList Where CustomerId = @CustomerId
 
END 

