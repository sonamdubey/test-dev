IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertHandle]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertHandle]
GO

	CREATE PROCEDURE [dbo].[InsertHandle]  
(  
 @UserId AS NUMERIC(18,0),  
 @HandleName AS  VARCHAR(50),  
 @IsUpdated AS BIT,
 @Status AS BIT OUT  
)  
AS  
BEGIN  
  
 IF NOT EXISTS(SELECT HandleName FROM UserProfile WHERE REPLACE( HandleName, '.', '') = REPLACE( @HandleName, '.', '')  )
	 BEGIN	
		 IF EXISTS(SELECT Id FROM UserProfile WHERE UserId = @UserId)  
			BEGIN  
			   UPDATE UserProfile   
			   SET HandleName = @HandleName, IsUpdated = @IsUpdated  
			   WHERE UserID = @UserId 
			   
			   SET @Status = 'True'
			END  
		 ELSE  
			BEGIN 
				INSERT INTO UserProfile(UserId, HandleName, IsUpdated)  
				VALUES(@UserId, @HandleName, @IsUpdated)   
				
				SET @Status = 'True'
			END 
	 END
 ELSE
	 BEGIN
			SET @Status = 'False'	
	 END      
	 
END

