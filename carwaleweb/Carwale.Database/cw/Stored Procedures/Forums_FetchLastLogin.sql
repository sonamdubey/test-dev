IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_FetchLastLogin]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_FetchLastLogin]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/17/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_FetchLastLogin]      -- execute cw.Forums_FetchLastLogin 1278,775
 -- Add the parameters for the stored procedure here      
 @UserId NUMERIC(18,0),
 @LastLoginTime datetime OUTPUT
 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
SELECT @LastLoginTime=ForumLastLogin FROM UserProfile WHERE UserId= @UserId

UPDATE UserProfile SET ForumLastLogin= GETDATE() WHERE UserId= @UserId
END 
       



