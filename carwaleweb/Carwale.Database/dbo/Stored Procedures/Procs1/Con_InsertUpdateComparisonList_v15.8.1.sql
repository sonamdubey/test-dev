IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertUpdateComparisonList_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertUpdateComparisonList_v15]
GO

	
---Modified By Prashant vishe On 02 july 2013
---added insertion and updation of ImagePath and ImageName        
CREATE PROCEDURE [dbo].[Con_InsertUpdateComparisonList_v15.8.1]        
        
@CID NUMERIC,        
@VersionId1 NUMERIC,        
@VersionId2 NUMERIC,        
@EntryDate DATETIME,        
@IsActive bit,        
@Url Varchar(250),  
@ImagePath varchar(500),  
@ImageName varchar(500),
@TimeStamp VARCHAR(25),        
@CompId NUMERIC OUTPUT       
        
AS        
         
BEGIN        
	SET @CompId = NULL
 IF @CID = -1        
  BEGIN        
   INSERT INTO Con_CarComparisonList         
   (VersionId1,VersionId2,EntryDate,IsActive,HostURL,OriginalImgPath,ImageName)         
   VALUES         
   (@VersionId1,@VersionId2,@EntryDate,@IsActive,@Url,@ImagePath + @ImageName,@ImageName)  
   SET @CompId = SCOPE_IDENTITY()
  END        
 ELSE        
  BEGIN        
   UPDATE Con_CarComparisonList        
   SET VersionId1=@VersionId1,VersionId2=@VersionId2,IsActive=@IsActive,HostURL = @Url,OriginalImgPath=@ImagePath + @ImageName + '?v=' + @TimeStamp,ImageName=@ImageName        
   WHERE ID = @CID  
   SET @CompId = @CID
  END    
END        

