IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetModelPriceQuoteDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetModelPriceQuoteDetails]
GO

	-- =============================================      
-- Author:  Vikas J      
-- Create date: 10/5/2013,       
-- Description: SP to Getting Price Quote Details for particular City of all the versions avialable for a dealer.
-- =============================================      
      
CREATE PROCEDURE [dbo].[Microsite_GetModelPriceQuoteDetails]        
(        
 @CityId INT,  
 @ModelId INT,
 @DealerId INT    
 
)   
AS          
 --  fetching price details        
 BEGIN         
   SELECT VE.ID, VE.Name, 
	NCS.ExShowroomPrice AS Price, NCS.Insurance , NCS.RTO , NCS.CRTMCharges,  
	NCS.DriveAssure , NCS.withOctroi , NCS.ExShowroomPrice+NCS.CRTMCharges+NCS.DriveAssure+NCS.Insurance+NCS.RTO as OnRoadPrice	
   FROM (CarVersions Ve LEFT JOIN DealerWebsite_ExShowRoomPrices NCS ON VE.ID = NCS.CarVersionId AND NCS.CityId=@CityId AND DealerId=@DealerId  ) 
   WHERE VE.IsDeleted=0 AND Ve.Futuristic=0 AND VE.New=1 AND Ve.CarModelId=@ModelId  
 END
