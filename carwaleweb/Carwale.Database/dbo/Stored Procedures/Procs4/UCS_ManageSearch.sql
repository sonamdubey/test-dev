IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UCS_ManageSearch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UCS_ManageSearch]
GO

	
CREATE PROCEDURE [dbo].[UCS_ManageSearch]        
@Action VARCHAR(10),      
@SessionId VARCHAR(100) = '',        
@CustomerId numeric(18, 0) = -1,        
@Model VARCHAR(1000) = '',        
@Make numeric(18, 0) = 0,        
@PriceFrom numeric(18, 0) = 0,        
@PriceTo numeric(18, 0) = 0,   
@PriceToMax numeric(18, 0) = 0,        
@YearFrom numeric(18, 0) = 0,        
@YearTo numeric(18, 0) = 0,        
@KmFrom numeric(18, 0) = 0,        
@KmTo numeric(18, 0) = 0,        
@KmToMax numeric(18, 0) = 0,        
@City numeric(18, 0) = 0,        
@Dist numeric(18, 0) = 0,        
@St numeric(18, 0) = 0,        
@Li numeric(18, 0) = 0,      
@ListedFrom datetime = NULL,     
@SearchCriteriaProfileIds NVARCHAR(3000) = '' ,   
@Lattitude float = 0,  
@Longitude float = 0  
AS        
BEGIN      
       
       
 IF @Action = 'Insert'      
 BEGIN      
        
  DECLARE @CurrentDate DateTime        
  SET @CurrentDate = getdate()        
  DECLARE @SCId numeric(18, 0)  
         
  INSERT INTO UCS_SearchCriteria        
  (SessionId,CustomerId,Model,Make,PriceFrom,PriceTo,YearFrom,YearTo,        
   KmFrom,KmTo,City,Dist,St,Li,SearchedDate        
  )        
  VALUES        
  (@SessionId,@CustomerId,@Model,@Make,@PriceFrom,@PriceTo,@YearFrom,@YearTo,        
   @KmFrom,@KmTo,@City,@Dist,@St,@Li,@CurrentDate        
  )        
          
  SET @SCId = SCOPE_IDENTITY()    
  
  PRINT @SCId
        
  DECLARE @SearchResult VARCHAR(MAX)    
    
  DECLARE @SearchQuery NVARCHAR(4000)   
  SET @SearchQuery =   'Select   
      @SearchResult = (Select ( ProfileId + '','' )   
      From   
      (LiveListings AS LL LEFT JOIN CarPhotos CP ON CP.InquiryId = LL.InquiryId AND CP.IsDealer = (CASE SellerType WHEN 1 THEN 1 WHEN 2 THEN 0 END) AND CP.IsActive = 1 AND CP.IsMain = 1 AND CP.IsApproved = 1), LL_Cities AS LC   
      Where   
      LC.CityId = @ParamCity   
      AND LL.Lattitude BETWEEN LC.Lattitude - @ParamLattitude AND LC.Lattitude + @ParamLattitude   
      AND LL.Longitude BETWEEN LC.Longitude - @ParamLongitude AND LC.Longitude + @ParamLongitude '   
      
  SET @SearchQuery = @SearchQuery + @SearchCriteriaProfileIds   
  
  
  IF @Model <> ''   
  BEGIN  
    
 SET @SearchQuery =  @SearchQuery + ' AND ModelId IN (' + @Model + ')'   
    
  END  
    
  SET @SearchQuery = @SearchQuery + ' FOR XML PATH(''''))'  
    
  PRINT @SearchQuery  
    
  EXEC sp_executesql     
        @query = @SearchQuery,     
        @params =  N'  
      @ParamCity NUMERIC,  
      @ParamLattitude FLOAT,  
      @ParamLongitude FLOAT,  
      @ParamYearFrom NUMERIC,  
      @ParamYearTo NUMERIC,  
      @ParamPriceFrom NUMERIC,  
      @ParamPriceToMax NUMERIC,  
      @ParamPriceTo NUMERIC,  
      @ParamKmFrom NUMERIC,  
      @ParamKmToMax NUMERIC,  
      @ParamKmTo NUMERIC,  
      @ParamSt NUMERIC,  
      @ParamListedFrom DATETIME,  
      @SearchResult VARCHAR(MAX) OUTPUT  
     ',    
  @ParamCity = @City,   
  @ParamLattitude = @Lattitude,    
  @ParamLongitude = @Longitude,  
  @ParamYearFrom = @YearFrom,  
  @ParamYearTo = @YearTo,  
  @ParamPriceFrom = @PriceFrom,  
  @ParamPriceToMax = @PriceToMax,  
  @ParamPriceTo = @PriceTo,  
  @ParamKmFrom = @KmFrom,  
  @ParamKmToMax = @KmToMax,  
  @ParamKmTo = @KmTo,  
  @ParamSt = @St,  
  @ParamListedFrom = @ListedFrom,  
        @SearchResult = @SearchResult OUTPUT    
  
  Print  @SearchResult
   
  INSERT INTO UCS_SearchResult        
  (SCId,SessionId,CustomerId,SearchResult,SearchedDate)        
  VALUES        
  (@SCId,@SessionId,@CustomerId,@SearchResult,@CurrentDate)        
   
   PRINT 'Search Criteria ID : '  
   PRINT @SCId       
 END      
       
 IF @Action = 'Update'      
 BEGIN      
      
 UPDATE UCS_SearchCriteria      
 SET CustomerId = @CustomerId      
 WHERE SessionId = @SessionId AND CustomerId = -1       
       
 UPDATE UCS_SearchResult      
 SET CustomerId = @CustomerId      
 WHERE SessionId = @SessionId AND CustomerId = -1        
       
 END      
        
END 
