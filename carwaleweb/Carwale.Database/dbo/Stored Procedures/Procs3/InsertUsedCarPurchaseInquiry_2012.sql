IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertUsedCarPurchaseInquiry_2012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertUsedCarPurchaseInquiry_2012]
GO

	
-- Modified by - Binumon George  
-- Modified Date -16-02-2012  
-- Description - Add new purchase inquiry to TC from Carwale  
--  Avishkar Modified 23 May 2012 to populate UsedCarPurchaseInquiryId  

-- Modified By - Ashish Ambokar
-- Modified Date - 2/7/2012
--Description - SourceId column value is added in UsedCarPurchaseInquiries table
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE  
  
CREATE PROCEDURE [dbo].[InsertUsedCarPurchaseInquiry_2012]  
 @SellInquiryId  NUMERIC, -- Sell Inquiry Id  
 @CarModelIds  VARCHAR(100) = Null, -- Model IDs  
 @CarModelNames VARCHAR(500) = NULL, -- Model Names  
 @CustomerId  NUMERIC, -- Customer Id  
 @YearFrom  NUMERIC = NULL, -- Car Year From  
 @YearTo  NUMERIC = NULL, -- Car Year To  
 @BudgetFrom  NUMERIC = NULL, -- Budget From  
 @BudgetTo  NUMERIC = NULL, -- Budget To  
 @MileageFrom  NUMERIC = NULL, -- Mileage From  
 @MileageTo  NUMERIC = NULL, -- Mileage To  
 @NoOfCars  INT = 1,  -- How Many cars customer intend to buy  
 @BuyTime  VARCHAR(20) = NULL, -- When customer intend to buy? i time-frame  
 @RequestDateTime DATETIME, -- Entry Date  
 @Comments   VARCHAR(2000) = NULL,  
 @IsApproved  BIT = 1,  --- Whether Inquiry is verified or not. If admin submits the query this value should be 1, 0 otherwise.   
 @SourceId SMALLINT = 1,
 @InquiryId  NUMERIC OUTPUT --id of the inquiry just submitted  
 AS  
 BEGIN  
   
       
          
  -- Check if user is already shown interest  
  If NOT EXISTS(SELECT ID FROM UsedCarPurchaseInquiries WHERE CustomerID = @CustomerId AND SellInquiryId = @SellInquiryId)  
  BEGIN    
    
      BEGIN TRY  
        
    INSERT INTO UsedCarPurchaseInquiries(CustomerId, SellInquiryId,   
     YearFrom, YearTo, KmFrom, KmTo, PriceFrom, PriceTo,  
     NoOfCars, BuyTime, CarModelIds, CarModelNames, Comments, RequestDateTime, IsApproved, SourceId)  
    VALUES( @CustomerId, @SellInquiryId,   
     @YearFrom, @YearTo, @MileageFrom, @MileageTo, @BudgetFrom, @BudgetTo,  
     @NoOfCars, @BuyTime, @CarModelIds, @CarModelNames,  @Comments,  @RequestDateTime,@IsApproved, @SourceId)  
  
    SET @InquiryId =  SCOPE_IDENTITY()  
      
    -- Check if this car uploaded through Trading Cars application  
    declare @StockId numeric, @BranchId int, @dealerid numeric  
      
    select @StockId = si.TC_StockId, @BranchId = st.BranchId, @dealerid = si.DealerId   
    from SellInquiries si, TC_Stock st where si.TC_StockId = st.Id and si.ID = @SellInquiryId  
      
    if @StockId is not null  
    BEGIN  
     declare @customer varchar(50), @email varchar(100), @mobile varchar(12), @current_time datetime = getdate(), @id numeric  
       
     -- If yes, send this inquiry back to thet dealer  
     select @customer=cs.Name, @mobile=cs.Mobile, @email=cs.email from Customers cs where Id = @CustomerId  
       
     -- Push this inquiry to trading car software by calling common procedure TC_AddBuyerInquiries        
       
         
     EXECUTE  TC_AddBuyerInquiries @BranchId =@dealerid,@StockId=@StockId,@CustomerName=@customer,@Email=@email,@Mobile =@mobile,  
            @Location=NULL,@Buytime=NULL,@CustomerComments=NULL,@Comments =NULL,  
            @InquiryStatus =NUll,@NextFollowup =NULL,@AssignedTo =NULL,@InquirySource =1,@UserId =NULL,@UsedCarPurchaseInquiryId=@InquiryId  
            ----  Avishkar Modified 23 May 2012 to populate UsedCarPurchaseInquiryId  
              
         
           
    END  
  END TRY    
     
   BEGIN CATCH     
     INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date)    
     VALUES('InsertUsedCarPurchaseInquiry',ERROR_MESSAGE(),GETDATE())    
    --SELECT ERROR_NUMBER() AS ErrorNumber;    
   END CATCH;    
    
  END   
    
    
      
 END  
  
