IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AddMicroSiteSellerInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AddMicroSiteSellerInquiry]
GO

	-- =============================================  
-- Created By : Binumon George   
-- Create date: 19 Apr 2012  
-- Description: For Add Micro sites seller inquiries to TC_stock and TC_SellerInquiries tables.  
-- =============================================  
CREATE Procedure [dbo].[TC_AddMicroSiteSellerInquiry]  
@BranchId   BIGINT,  
@VersionId   INT,   
-- TC_CustomerDetails's related param  
@CustomerName VARCHAR(100),  
@Email VARCHAR(100),  
@Mobile VARCHAR(15),  
@Location VARCHAR(50),  
  
--------------------------  
  
@MakeYear   DATETIME,   
@Price    BIGINT,   
@Kilometers   int,   
@Color    VARCHAR(100),   
@RegNo    VARCHAR(40),  
@RegistrationPlace varchar(40),  
@Insurance   varchar(40),  
@InsuranceExpiry datetime,  
@Owners    varchar(20),  
@Tax    varchar(20),  
@Comments   VARCHAR(500),  
@SId BIGINT OUT  
  
AS             
Begin     
  DECLARE @StockId BIGINT  
  DECLARE @SellInqId BIGINT  
  DECLARE @CurrDate datetime  
  SET @CurrDate=GETDATE()  
    
 -- here we inserting value to seller table  
    EXEC @SellInqId =  TC_AddSellerInquiry @BranchId, @VersionId, @CustomerName ,@Email ,@Mobile ,@Location,NULL ,NULL ,  
         @MakeYear, @Price, @Kilometers,@Color,NULL, @RegNo, @RegistrationPlace,@Insurance,  
         @InsuranceExpiry,@Owners, NULL, @Tax,NULL, NULL, NULL, NULL,   
         @Comments, NULL, 1, 2, NULL 
                
 -- here we inserting value to Tc_Stock table 
 DECLARE @existingStockId BIGINT 
 EXEC TC_SaveStockInfo_SP -1,@VersionId, @BranchId, 1,@CurrDate, @MakeYear, @RegNo, @Kilometers, @Price,              
        @Color,@CurrDate, @RegistrationPlace,  @Owners, @Tax, @Insurance, @InsuranceExpiry, @StockId out,0,    
        NUll , NULL, NULL

 -- Bellow line is added by Umesh Ojha on 23/5/2012      
 --- Update Table Tc_Stock for IsApproved = 0 because bydefault this field added by 1 
 -- sp from dealer site first this stock should approved & then this will show in the stock list. 
 UPDATE TC_Stock SET IsApproved=0 WHERE Id=@StockId
            

 -- select @SellInqId,@StockId  
 -- here we inserting combination of new stockid and SellInqId in to TC_StockSellInq  
 INSERT INTO TC_StockSellInq(StockId,SellInqId)VALUES(@StockId,@SellInqId)  
    
 SET @SId = @StockId
 
End 


