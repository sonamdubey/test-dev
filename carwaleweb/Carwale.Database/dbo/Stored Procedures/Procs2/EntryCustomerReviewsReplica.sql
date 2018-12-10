IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryCustomerReviewsReplica]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryCustomerReviewsReplica]
GO

	
--THIS PROCEDURE IS FOR entry of customer reviews          
--CustomerReviews          
--CustomerId, MakeId, ModelId, VersionId, StyleR, ComfortR, PerformanceR, ValueR, FuelEconomyR, OverallR, Pros, Cons, Comments, Title,           
--EntryDateTime, IsNew, ReportAbused, Liked, Disliked          
          
CREATE PROCEDURE [dbo].[EntryCustomerReviewsReplica]          
 @ReviewId  NUMERIC,                   
 @StyleR  SMALLINT,           
 @ComfortR  SMALLINT,           
 @PerformanceR  SMALLINT,           
 @ValueR  SMALLINT,           
 @FuelEconomyR SMALLINT,           
 @OverallR  FLOAT,           
 @Pros   VARCHAR(100),           
 @Cons   VARCHAR(100),           
 @Comments  VARCHAR(8000),          
 @Title   VARCHAR(100),                        
 @IsOwned BIT,        
 @IsNewlyPurchased BIT,        
 @Familiarity INT,        
 @Mileage FLOAT,       
 @ID   NUMERIC OUTPUT       
AS          
           
BEGIN          
   
 SET @ID = (SELECT ID FROM CustomerReviewsReplica WHERE ReviewId  = @ReviewId)  
   
 IF @ID IS NULL   
 BEGIN  
           
   INSERT INTO CustomerReviewsReplica          
   (          
     ReviewId,    
     StyleR, ComfortR,   PerformanceR,   ValueR,   FuelEconomyR,   OverallR,           
     Pros,    Cons,    Comments,   Title,         
     IsOwned,  IsNewlyPurchased, Familiarity, Mileage,      
     IsVerified, LastUpdatedOn    
   )          
   VALUES          
   (           
     @ReviewId,   
     @StyleR, @ComfortR,   @PerformanceR,  @ValueR,   @FuelEconomyR,  @OverallR,           
     @Pros,    @Cons,   @Comments,   @Title,            
     @IsOwned,  @IsNewlyPurchased, @Familiarity,  @Mileage,         
     0,  GETDATE()   
   )          
            
   SET @ID = SCOPE_IDENTITY()       
    
 END     
   
 IF @ID IS NOT NULL  
 BEGIN  
   
 UPDATE CustomerReviewsReplica          
    SET     
    StyleR = @StyleR,            
    ComfortR = @ComfortR,     
    PerformanceR = @PerformanceR,     
    ValueR = @ValueR,     
    FuelEconomyR = @FuelEconomyR,     
    OverallR = @OverallR,           
    Pros = @Pros,      
    Cons = @Cons,      
    Comments = @Comments,     
    Title = @Title,                
    IsOwned = @IsOwned,    
    IsNewlyPurchased = @IsNewlyPurchased,   
    Familiarity = @Familiarity,   
    Mileage = @Mileage,  
    IsVerified = 0,   
    LastUpdatedOn = GETDATE()        
 WHERE   
  ID = @ID   
   
 END  
            
END 
