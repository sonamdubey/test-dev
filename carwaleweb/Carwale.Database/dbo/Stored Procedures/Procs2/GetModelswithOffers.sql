IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelswithOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelswithOffers]
GO

	-- =============================================        
-- Author:  Raghu        
-- Create date: 22-5-2013        
-- Description: Gets models for particular make        
-- Modified by: Raghu on 25-07-2013 added condition isdeleted=0  
-- Modified by : Raghu on 2-8-2013 ordered Models based on price   
-- Modified by : Raghu on 12-02-2014 gettin min and max price from carmodels instead of NewCarShowroopPrices  
-- =============================================        
CREATE PROCEDURE [dbo].[GetModelswithOffers]  --EXEC [GetModelswithOffers] 15,801        
(        
--@MakeId int        
@DealerId int        
)        
AS        
BEGIN        
          
   SELECT DISTINCT CM.ID as ModelId,CMA.Name as MakeName, CM.Name as ModelName,CM.HostURL, CM.LargePic,      
   MD.OfferTitle,MD.OfferDetails ,       
   MINPrice MinPrice, MaxPrice MaxPrice      
   FROM CarModels CM  WITH (NOLOCK)      
    INNER JOIN CarMakes CMA WITH (NOLOCK) ON CMA.ID = CM.CarMakeId      
    INNER JOIN Dealer_NewCar DN WITH (NOLOCK) ON DN.MakeId = CMA.ID      
    INNER JOIN Cities C WITH (NOLOCK) ON C.ID = DN.CityId      
    LEFT JOIN Microsite_DealerOffers MD WITH (NOLOCK) ON MD.ModelId = CM.ID AND MD.IsActive =1 and Md.IsDeleted =0       
    AND MD.CityId = C.ID AND MD.DealerId = DN.TcDealerId      
   WHERE  
   DN.TcDealerId = @DealerId AND   
    CM.New = 1 AND CM.Futuristic=0 AND CM.IsDeleted = 0    
   ORDER BY MinPrice   
         
END        
        