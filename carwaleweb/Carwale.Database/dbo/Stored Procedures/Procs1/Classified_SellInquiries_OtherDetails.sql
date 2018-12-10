IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_SellInquiries_OtherDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_SellInquiries_OtherDetails]
GO

	 
-- =============================================  
-- Author:  <kush kumar>  
-- Create date: <9/10/2012>  
-- Description: <FOR SAVING OTHER DETAILS WHICH WERE LEFT IN SP.[Classified_SellInquiries_partial]> 
-- Modified By: Vikas -- Modified On: 11/01/2012 -- Added a new Parameter ReasonForSelling to be saved to CustomerSellInquiries
-- Avishkar Added 17/5/2013 to  trigger LL_UpdateListing
--Modified By : Manish on 11-06-2013 for inserting car as soon as user completes the car detail page information.
--Modified By : amit verma on 10/31/2013 added logic to upgrade package for free listing (amount = 0)
-- =============================================  
  
CREATE PROCEDURE [dbo].[Classified_SellInquiries_OtherDetails]  


   
 @SellInquiryId  NUMERIC,  
 @RegPlace  VARCHAR(50),  
 @RegNo  VARCHAR(50),                 
 @Color   VARCHAR(50),          
 @ColorCode VARCHAR(6),       
 @Comments  VARCHAR(500),  
 @Insurance  VARCHAR(50),          
 @InsuranceExpiry DATETIME,  
 @Owners  NUMERIC,
 @ReasonForSelling VARCHAR(500)   
  
 AS  

  BEGIN  
  /* UPDATE CustomerSellInquiries  
   SET   
   CarRegNo = @RegNo,   
   Color = @Color,   
   ColorCode = @ColorCode,   
   Comments = @Comments,  
   Progress = 70,    -- Vikas : Added to track the progress of the Sell Car Process  
   ReasonForSelling = @ReasonForSelling
   WHERE ID = @SellInquiryId  
  END */  --This update commented by Manish on 11-06-2013 for inserting car into livelisting when cardetail has been completed.
    
 IF EXISTS(SELECT * FROM CustomerSellInquiryDetails WHERE InquiryId = @SellInquiryId)   
  BEGIN  
   UPDATE CustomerSellInquiryDetails  
   SET  
   RegistrationPlace = @RegPlace,  
   Insurance = @Insurance,  
   InsuranceExpiry = @InsuranceExpiry,  
   Owners = @Owners  
   WHERE InquiryId = @SellInquiryId  
   
   --------------------------Update Added by Manish on 11-06-2013---------------------
   UPDATE CustomerSellInquiries  
   SET   
   CarRegNo = @RegNo,   
   Color = @Color,   
   ColorCode = @ColorCode,   
   Comments = @Comments,  
   Progress = 70,    -- Vikas : Added to track the progress of the Sell Car Process  
   ReasonForSelling = @ReasonForSelling
   WHERE ID = @SellInquiryId  
   -----------------------------------------------------------------------------------
   
  END  
 ELSE  
  BEGIN  
   INSERT INTO CustomerSellInquiryDetails(InquiryId, RegistrationPlace, Insurance, InsuranceExpiry, Owners)          
   VALUES ( @SellInquiryId, @RegPlace, @Insurance, @InsuranceExpiry, @Owners) 
   	
  -- Avishkar Added 17/5/2013 to  trigger LL_UpdateListing
   UPDATE CustomerSellInquiries  
   SET   
   CarRegNo = @RegNo,   
   Color = @Color,   
   ColorCode = @ColorCode,   
   Comments = @Comments,  
   Progress = 70,    -- Vikas : Added to track the progress of the Sell Car Process  
   ReasonForSelling = @ReasonForSelling
   WHERE ID = @SellInquiryId
    
  END
  
  --------amit verma on 10/31/2013 added logic to upgrade package for free listing (amount = 0)
	--IF((SELECT P.Amount FROM CustomerSellInquiries CS
	--	LEFT JOIN Packages P ON CS.PackageId = P.Id
	--	WHERE CS.ID = @SellInquiryId) = 0)
	--BEGIN
	--	EXEC [dbo].[UpgradePackageTypeToListingType] 2,@SellInquiryId,-1,0
	--END
END


