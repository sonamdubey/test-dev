IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockStatusChange]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockStatusChange]
GO

	-- Author:  Surendra Chouksey    
-- Create date: 28 Sept,2011    
-- Description: Changing stock status to sols or not available  
-- Modified   : Date-9th Aug,Desc- LastUpdated=GETDATE() in added while updating Sellinquiries  
-- Modified   : Tejashree Patil on Date-25th Sept 2012 ,Desc- Added branchId new parameter  
-- Modified   : Tejashree Patil on Date-16th Oct 2012 ,Changed PATINDEX to CHARINDEX
-- Modified By:  Manish on 30-07-2013 for maintaining log of the removed car
-- Modified by: Manish on 27-02-2014 Removed LastUpdatedDatd update from sellinquiries since when stock status change and again upload to CarWale lastUpdated date should not be changed.
-- Modifie By : Vivek Gupta on 14th July,2014 , Added @CustomerCameFrom
-- =============================================    
CREATE  PROCEDURE [dbo].[TC_StockStatusChange]    
(    
@StockIdChain VARCHAR(1000),    
@Status TINYINT,   
@BranchId BIGINT,
@CustomerCameFrom  VARCHAR(200)  = NULL  
)      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON;
     
	
			 DECLARE @Separator CHAR(1)=','    
			 DECLARE @Separator_position INT -- This is used to locate each separator character      
			 DECLARE @array_value VARCHAR(1000) -- this holds each array value as it is returned      
			  -- For my loop to work I need an extra separator at the end. I always look to the      
			  -- left of the separator character for each array value      
			      
				SET @StockIdChain = @StockIdChain + @Separator      
				 WHILE CHARINDEX(@Separator , @StockIdChain) <> 0       
					BEGIN         
						-- patindex matches the a pattern against a string      
						SELECT  @Separator_position = CHARINDEX(@Separator ,@StockIdChain)      
						SELECT  @array_value = LEFT(@StockIdChain, @Separator_position - 1)    
			   -- Checking whether Stock is availabe in carwale or not    
			   IF EXISTS(SELECT Id FROM SellInquiries WHERE TC_StockId=@array_value AND DealerId=@BranchId)    
			   BEGIN    
			   -- Making same stock not available in SellInquiry table    
			   UPDATE SellInquiries SET StatusId =2, 
										ModifiedDate=GETDATE()
									--	LastUpdated=GETDATE()   --- Commented by Manish on 27-02-2014 since LastUpdatedDate should change only for certain logic
					WHERE TC_StockId=@array_value AND DealerId=@BranchId 
			   
			   
			-------------------------Below insert statement add by Manish on 30-07-2013 for maintaining log of the removed car-----------
							INSERT INTO TC_StockUploadedLog(SellInquiriesId, 
															DealerId,
															IsCarUploaded,
															CreatedOn)
													SELECT  Id,
															DealerId,
															0,
															GETDATE()
													 FROM  SellInquiries WITH (NOLOCK) 
													 WHERE TC_StockId=@array_value 
													 AND DealerId=@BranchId 
			---------------------------------------------------------------------------------------------------------------------------------------------
			   
			   
			     
			   END    
			          
			   -- Updating Tc_stock and making it not availabe    
			   UPDATE TC_Stock SET StatusId =@Status,IsSychronizedCW=0 ,LastUpdatedDate=GETDATE(), SoldToCustomerFrom = @CustomerCameFrom
							   WHERE Id=@array_value  AND BranchId=@BranchId  
				-- This replaces what we just processed with and empty string      
				 SELECT  @StockIdChain = STUFF(@StockIdChain, 1, @Separator_position, '')       
			  END --while End 
			  

END

