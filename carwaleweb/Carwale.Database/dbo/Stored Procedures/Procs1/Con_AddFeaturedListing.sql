IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_AddFeaturedListing]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_AddFeaturedListing]
GO

	--Modified By:Prashant Vishe On 21 May 2013
--modification:removed Isertion/Updation of column IsVisible
CREATE PROCEDURE [dbo].[Con_AddFeaturedListing]                    
    @Id           NUMERIC,                    
    @CarId       NUMERIC,                    
    @Description   VARCHAR(1000),     
    @IsActive           BIT,       
    @EntryDateTime   DATETIME,                    
    @Url      VARCHAR(100) = NULL,                   
    @LastSavedId     NUMERIC OUTPUT,         
    @DirectoryPath varchar(50)                   
 AS                    
                       
BEGIN                    
     IF @Id = -1                    
      BEGIN                    
     INSERT INTO Con_FeaturedListings                    
       ( CarId, Description,IsModel,                     
    IsActive,EntryDateTime, HostUrl, DirectoryPath                     
    )                    
      VALUES                    
       ( @CarId, @Description,1,                     
    @IsActive,@EntryDateTime, @Url,@DirectoryPath                     
    )                    
                      
     SET @LastSavedId = SCOPE_IDENTITY()                    
                               
        END                    
    ELSE                    
        BEGIN                    
     UPDATE Con_FeaturedListings SET                     
      Description = @Description,    
      IsActive = @IsActive, HostUrl = @Url, DirectoryPath = @DirectoryPath                
     WHERE CarId = @CarId                   
                                    
     SET @LastSavedId = @Id                    
        END                    
END 


   
      