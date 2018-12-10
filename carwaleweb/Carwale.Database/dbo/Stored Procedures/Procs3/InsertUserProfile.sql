IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertUserProfile]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertUserProfile]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR UsersProfile TABLE  
  
CREATE PROCEDURE [dbo].[InsertUserProfile]  
 @UserId  NUMERIC,  
 @AboutMe  VARCHAR(1000),  
 @Signature  VARCHAR(500),  
 @AvtarPhoto  VARCHAR(50),  
 @ThumbNailPhoto VARCHAR(50),  
 @RealPhoto  VARCHAR(50),  
 @HostUrl VARCHAR(100),
 @Status  INTEGER OUTPUT


 AS  
    
BEGIN   
   
   DECLARE 
  @SmallUrl VARCHAR(100),
 @ThumbNailUrl VARCHAR(100),
 @MediumUrl VARCHAR(100),
 @DirectoryPath VARCHAR(50)

 SET @Status = 0 
 SET @SmallUrl = @RealPhoto +'_75.jpg'
 SET @ThumbNailUrl = @RealPhoto +'_160.jpg' 
 SET @MediumUrl = @RealPhoto +'_400.jpg' 
 SET @DirectoryPath = '/c/up/r/'

 SELECT Id FROM UserProfile WHERE UserId = @UserId    
  
 IF @@RowCount = 0  
  BEGIN  
   INSERT INTO UserProfile(UserId, AboutMe, Signature, AvtarPhoto, ThumbNail, RealPhoto, HostURL, SmallUrl, ThumbNailUrl, MediumUrl, DirectoryPath )  
   VALUES(@UserId, @AboutMe, @Signature, @AvtarPhoto, @ThumbNailPhoto, @RealPhoto, @HostUrl, @SmallUrl, @ThumbNailUrl, @MediumUrl, @DirectoryPath  )   
  
   SET @Status = 1  
  END  
 ELSE  
  BEGIN  
   UPDATE  UserProfile SET AboutMe = @AboutMe, Signature = @Signature, AvtarPhoto = @AvtarPhoto, ThumbNail = @ThumbNailPhoto, RealPhoto = @RealPhoto, HostURL = @HostUrl, IsReplicated = 0,
    SmallUrl = @SmallUrl, ThumbNailUrl = @ThumbNailUrl, MediumUrl = @MediumUrl, DirectoryPath = @DirectoryPath,StatusId =1
   WHERE UserID = @UserId  
     
   SET @Status = 1  
  END  
    
END  
  
  