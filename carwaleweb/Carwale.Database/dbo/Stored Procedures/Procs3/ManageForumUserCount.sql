IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ManageForumUserCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ManageForumUserCount]
GO

	CREATE PROCEDURE [dbo].[ManageForumUserCount]  
 @Result VARCHAR(100) OUTPUT  
AS  
BEGIN  
  
 DECLARE @MostUsers NUMERIC(18,0)  
 DECLARE @MostUsersDate DATETIME  
 
 SELECT @MostUsers = NoOfUsers, @MostUsersDate = ModifiedDateTime FROM ForumMostUsersOnline Where IsFinal = 1 
  
 DECLARE @Total NUMERIC(18,0)  
 DECLARE @Guests NUMERIC(18,0)  
 DECLARE @Members NUMERIC(18,0)  
   
 SELECT   
 @Total = (SELECT COUNT(SessionID) FROM ForumUserTracking ft WHERE DATEDIFF(MINUTE, ft.ActivityDateTime, getdate()) < 60),  
 @Members = (SELECT COUNT(DISTINCT UserId) FROM ForumUserTracking ft1 WHERE DATEDIFF(MINUTE, ft1.ActivityDateTime, getdate()) < 60 AND UserId <> -1)  
  
 SET @Guests = @Total - @Members  
 
   
 IF @MostUsers IS NULL  
 BEGIN  
    
  SET @MostUsers = @Total  
  SET @MostUsersDate = GETDATE()  
   
  INSERT INTO ForumMostUsersOnline  
  (NoOfUsers, ModifiedDateTime, IsFinal)  
  VALUES  
  (@MostUsers, @MostUsersDate, 1)  
    
 END  
 ELSE IF @MostUsers <= @Total  
 BEGIN  
   
  SET @MostUsers = @Total  
  SET @MostUsersDate = GETDATE()   
   
  UPDATE ForumMostUsersOnline  
  SET IsFinal = 0 Where IsFinal = 1
  
  INSERT INTO ForumMostUsersOnline  
  (NoOfUsers, ModifiedDateTime, IsFinal)  
  VALUES  
  (@MostUsers, @MostUsersDate, 1)  
     
 END  
   
   
 SET @Result = CONVERT(VARCHAR, @Total) + '|' + CONVERT(VARCHAR, @Guests) + '|' + CONVERT(VARCHAR, @Members) + '|' + CONVERT(VARCHAR, @MostUsers) + '|' + CONVERT(VARCHAR, @MostUsersDate)  
END