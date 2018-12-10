IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PostThanksSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PostThanksSave]
GO

	
CREATE PROCEDURE [dbo].[PostThanksSave]    
@CustomerID NUMERIC(18,0),    
@PostID NUMERIC(18,0),    
@IsSaved BIT OUTPUT    
AS     
BEGIN    
     
 DECLARE @ID NUMERIC(18,0)    
    
 SELECT @ID = ID FROM PostThanks WHERE CustomerID = @CustomerID AND PostID = @PostID    
 SET @IsSaved = 0    
     
 IF @ID IS NULL    
 BEGIN    
     
  INSERT INTO PostThanks    
  (CustomerID, PostID, CreatedDateTime)    
  VALUES    
  (@CustomerID, @PostID, GETDATE())    
     
  SET @IsSaved = 1
  
  DECLARE @UserProfileID NUMERIC(18,0)
  
  SELECT	@UserProfileID = UP.ID
  FROM		ForumThreads FT, UserProfile UP     
  WHERE		FT.ID = @PostID 
			AND FT.CustomerId = UP.UserId
  
  IF @UserProfileID IS NOT NULL			
  BEGIN
	UPDATE	UserProfile
	SET		ThanksReceived = ISNULL(ThanksReceived,0) + 1
	WHERE	ID = @UserProfileID			
  END
  
     
 END    
    
END
