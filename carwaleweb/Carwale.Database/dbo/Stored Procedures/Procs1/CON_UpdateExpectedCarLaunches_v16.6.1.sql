IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CON_UpdateExpectedCarLaunches_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CON_UpdateExpectedCarLaunches_v16]
GO

	--Modified by:Prashant vishe    On 29 aug 2013 for adding priority related query
--Modified by:Rakesh Yadav On 1 June 2016, made modelId as output parameter and set it if it's not coming as input
create PROCEDURE [dbo].[CON_UpdateExpectedCarLaunches_v16.6.1]        
 @Id    NUMERIC,        
 @ExpectedLaunch VARCHAR(250),
 @UpdatePhotoDateTime VARCHAR(50),        
 @LaunchDate  DATETIME,        
 @EstimatedPriceMin DECIMAL(18,2),        
 @EstimatedPriceMax DECIMAL(18,2),        
 @Url    VARCHAR(100),        
 @IsLaunched  BIT,        
 @CWConfidence  TINYINT,        
 @ModelId   VARCHAR(10) = Null OUTPUT,        
 @IsDeleted         BIT ,  
 @Priority int       
 AS                
                 
BEGIN       
      
DECLARE @IsNew BIT;      
DECLARE @IsFuturistic BIT;      
SET @IsNew= (@IsLaunched);       
      
  IF @IsLaunched = 0      
   begin      
  SET @IsFuturistic=1      
   end      
  else      
   begin       
  SET @IsFuturistic=0      
   end      
            
  UPDATE ExpectedCarLaunches SET LaunchDate = @LaunchDate, ExpectedLaunch = @ExpectedLaunch,                 
  EstimatedPriceMin = @EstimatedPriceMin, EstimatedPriceMax = @EstimatedPriceMax,        
  EstimatedPrice = 'Rs.' + REPLACE(Convert(Varchar,@EstimatedPriceMin, 18), '.00', '') + '-' + REPLACE(Convert(Varchar,@EstimatedPriceMax,18), '.00', '') + ' Lakh',        
  IsLaunched = @IsLaunched, CWConfidence = @CWConfidence, UpdatedDate = GETDATE(),IsDeleted =@IsDeleted,Priority=@Priority        
  WHERE Id = @Id      
         
   if(@IsNew=1)    
    begin     
   UPDATE CarModels SET New=@IsNew,Futuristic=@IsFuturistic WHERE Id=(SELECT CarModelId FROM  ExpectedCarLaunches WITH(NOLOCK) WHERE Id=@Id)     
  end     
 else     
  begin    
   UPDATE CarModels SET Futuristic=@IsFuturistic WHERE Id=(SELECT CarModelId FROM  ExpectedCarLaunches WITH(NOLOCK) WHERE Id=@Id)     
  end     
          
          
  --PRINT 'common'         
                
  IF @ModelId IS NOT NULL AND @ModelId != ''            
   BEGIN              
       -- PRINT 'a'        
   UPDATE CarModels SET OriginalImgPath = '/cw/expLaunchesCars/' + @ModelId+'.jpg?v='+ @UpdatePhotoDateTime, HostURL = @Url ,
   New= @IsNew,Futuristic=@IsFuturistic, IsReplicated = 0 WHERE ID = @ModelId         
   END
   ELSE
   BEGIN
	SET @ModelId= (SELECT CarModelId FROM  ExpectedCarLaunches WITH(NOLOCK) WHERE Id=@Id)	
   END             
END
