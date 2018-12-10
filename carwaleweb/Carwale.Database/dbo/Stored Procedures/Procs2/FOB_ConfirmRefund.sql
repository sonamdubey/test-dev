IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FOB_ConfirmRefund]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FOB_ConfirmRefund]
GO

	
--CREATED ON 11 SEP 2009 BY SENTIL    
--PROCEDURE FOR CONFIRM REFUNDED   
    
CREATE PROCEDURE [dbo].[FOB_ConfirmRefund]
(    
	@ID AS BIGINT ,
	@RefundDate AS DATETIME , 
	@StockID AS BIGINT = 0,
	@IsRefunded AS BIT = 0
)    
AS    
BEGIN    
    
DECLARE @BRDID INT
DECLARE @StID INT

	 UPDATE FOB_BookingRefund 
			SET  RefundDate = @RefundDate , IsRefunded = @IsRefunded
	 WHERE ID = @ID  	

	 --To Collect BookingRequest ID 
	 SELECT @BRDID = BRId FROM FOB_BookingRefund WHERE ID = @ID
	 
	 --TO UPDATE Booking Request Table with IsBooked Status True
	 UPDATE FOB_BookingRequestData SET IsRefunded = 1 WHERE Id = @BRDID

	 --To Update Stocks Table For Availability
	 UPDATE FOB_Stocks SET StockCount = StockCount + 1, UpdatedOn = GETDATE() WHERE Id = @StockID	
		
		    
--SELECT * FROM FOB_BookingCancellation    
--TRUNCATE TABLE FOB_BookingCancellation     
    
END     