IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FOB_ConfirmCancellation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FOB_ConfirmCancellation]
GO

	
--CREATED ON 11 SEP 2009 BY SENTIL      
--PROCEDURE FOR CONFIRM CANCELLATION     
      
CREATE PROCEDURE [dbo].[FOB_ConfirmCancellation]  
(      
	@ID AS BIGINT ,  
	@StockID AS BIGINT ,  
	@IsCancellation AS BIT ,
	@CancelledDate AS DATETIME      
)      
AS      
BEGIN      
      
DECLARE @BRDID INT  

  
  UPDATE FOB_BookingCancellation   
		 SET  CancelledDate = @CancelledDate , IsCancellation = @IsCancellation    
		 WHERE ID = @ID     
  
  --To Collect BookingRequest ID   
  SELECT @BRDID = BRId FROM FOB_BookingCancellation WHERE ID = @ID  
    
  --TO UPDATE Booking Request Table with IsBooked Status True  
  UPDATE FOB_BookingRequestData SET IsCancelled = 1 WHERE Id = @BRDID  
  
  --To Update Stocks Table For Availability  
  UPDATE FOB_Stocks SET StockCount = StockCount + 1, UpdatedOn = GETDATE() WHERE Id = @StockID   
    
        
--SELECT * FROM FOB_BookingCancellation      
--TRUNCATE TABLE FOB_BookingCancellation       
      
END     
      

