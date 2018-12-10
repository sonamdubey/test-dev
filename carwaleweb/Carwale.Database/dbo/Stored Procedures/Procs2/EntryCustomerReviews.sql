IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryCustomerReviews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryCustomerReviews]
GO

	
--THIS PROCEDURE IS FOR entry of customer reviews        
--CustomerReviews        
--CustomerId, MakeId, ModelId, VersionId, StyleR, ComfortR, PerformanceR, ValueR, FuelEconomyR, OverallR, Pros, Cons, Comments, Title,         
--EntryDateTime, IsNew, ReportAbused, Liked, Disliked        
        
CREATE PROCEDURE [dbo].[EntryCustomerReviews]        
 @CustomerId  NUMERIC,         
 @MakeId  NUMERIC,         
 @ModelId  NUMERIC,         
 @VersionId  NUMERIC,         
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
 @EntryDateTime DATETIME,        
 @ID   NUMERIC OUTPUT,      
 @IsOwned BIT,      
 @IsNewlyPurchased BIT,      
 @Familiarity INT,      
 @Mileage FLOAT      
         
 AS        
         
BEGIN        
         
 --IT IS FOR THE INSERT        
         
 INSERT INTO CustomerReviews        
  (        
   CustomerId,   MakeId,   ModelId,   VersionId,   StyleR,         
   ComfortR,   PerformanceR,   ValueR,   FuelEconomyR,   OverallR,         
   Pros,    Cons,    Comments,   Title,    EntryDateTime,         
   IsVerified,   ReportAbused,   Liked,    Disliked,  Viewed,      
   IsOwned,  IsNewlyPurchased, Familiarity, Mileage      
  )        
  VALUES        
  (         
   @CustomerId,   @MakeId,   @ModelId,   @VersionId,   @StyleR,         
   @ComfortR,   @PerformanceR,  @ValueR,   @FuelEconomyR,  @OverallR,         
   @Pros,    @Cons,   @Comments,   @Title,    @EntryDateTime,         
   0,    0,    0,    0,   0,      
   @IsOwned,  @IsNewlyPurchased, @Familiarity,  @Mileage       
  )        
         
 SET @ID = SCOPE_IDENTITY()        
          
END 
