IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Comments_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Comments_SP]
GO

	
CREATE PROCEDURE [dbo].[Comments_SP]
 @Id   NUMERIC,  
 @UserName  VARCHAR(100),
 @Email  VARCHAR(150),  
 @Comment  VARCHAR(500),   
 @CommentCategoryId  NUMERIC,
 @CategorySourceId NUMERIC,
 @CommentDateTime DATETIME,
 @LoginSource VARCHAR(50),
 @Status  INT OUTPUT    
AS  
  
BEGIN  
  
 SET @Status = 0  
  
 IF @Id = -1  
  
  BEGIN    
  
   INSERT INTO Comments( UserName, Email, Comment, CommentCategoryId, CategorySourceId, CommentDateTime, LoginSource )   
   VALUES( @UserName, @Email, @Comment, @CommentCategoryId, @CategorySourceId, @CommentDateTime, @LoginSource )  
  
   SET @Status = 1        
     
  END  
  
 ELSE  
  
  BEGIN  
     
   UPDATE Comments SET ReportAbuse = 1 WHERE Id = @Id  
     
  END  
END  
