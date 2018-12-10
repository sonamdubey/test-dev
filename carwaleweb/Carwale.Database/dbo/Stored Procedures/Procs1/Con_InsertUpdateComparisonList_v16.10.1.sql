IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertUpdateComparisonList_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertUpdateComparisonList_v16]
GO

	

---Modified By Prashant vishe On 02 july 2013
---added insertion and updation of ImagePath and ImageName   
---Modified By: Supreksha Singh on 17-10-2016
---Added IsSponsored flag     
CREATE PROCEDURE [dbo].[Con_InsertUpdateComparisonList_v16.10.1]        
        
@CID NUMERIC,        
@VersionId1 NUMERIC,        
@VersionId2 NUMERIC,        
@EntryDate DATETIME,        
@IsActive bit,
@IsSponsored bit,        
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
   (VersionId1,VersionId2,EntryDate,IsActive,HostURL,OriginalImgPath,ImageName,IsSponsored)         
   VALUES         
   (@VersionId1,@VersionId2,@EntryDate,@IsActive,@Url,@ImagePath + @ImageName,@ImageName,@IsSponsored)  
   SET @CompId = SCOPE_IDENTITY()
  END        
 ELSE        
  BEGIN        
   UPDATE Con_CarComparisonList        
   SET VersionId1=@VersionId1,VersionId2=@VersionId2,IsActive=@IsActive,HostURL = @Url,OriginalImgPath=@ImagePath + @ImageName + '?v=' + @TimeStamp,ImageName=@ImageName,IsSponsored=@IsSponsored        
   WHERE ID = @CID  
   SET @CompId = @CID
  END    
END        



