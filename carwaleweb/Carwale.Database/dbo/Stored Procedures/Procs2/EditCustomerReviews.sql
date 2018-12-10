IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EditCustomerReviews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EditCustomerReviews]
GO

	
--THIS PROCEDURE IS FOR Edit customer reviews          
--CustomerReviews          
--CustomerId, MakeId, ModelId, VersionId, StyleR, ComfortR, PerformanceR, ValueR, FuelEconomyR, OverallR, Pros, Cons, Comments, Title,           
--EntryDateTime, IsNew, ReportAbused, Liked, Disliked          
          
CREATE PROCEDURE [dbo].[EditCustomerReviews]          
 @Id   BIGINT,           
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
 @LastUpdatedOn            DATETIME,        
 @LastUpdatedBy BIGINT = NULL,      
 @IsOwned BIT,              
 @IsNewlyPurchased BIT,              
 @Familiarity INT,              
 @Mileage FLOAT,           
 @IsVerified BIT,             
 @Status  NUMERIC OUTPUT          
AS            
BEGIN          
           
UPDATE  CustomerReviews            
SET           
 StyleR   = @StyleR,          
 ComfortR  = @ComfortR,             
 PerformanceR  = @PerformanceR,             
 ValueR   = @ValueR,          
 FuelEconomyR  = @FuelEconomyR,           
 OverallR  = @OverallR,           
 Pros   = @Pros,            
 Cons   = @Cons,            
 Comments  = @Comments,             
 Title   = @Title,          
 LastUpdatedOn   = @LastUpdatedOn,       
 LastUpdatedBy = @LastUpdatedBy,         
 IsVerified  = @IsVerified,        
 IsOwned = @IsOwned,        
 IsNewlyPurchased = @IsNewlyPurchased,        
 Familiarity = @Familiarity,        
 Mileage = @Mileage           
WHERE           
ID = @Id           
            
SET @Status = 1          
END 
