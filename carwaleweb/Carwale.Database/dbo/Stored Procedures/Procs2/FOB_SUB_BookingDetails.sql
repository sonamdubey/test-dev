IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FOB_SUB_BookingDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FOB_SUB_BookingDetails]
GO

	
--CREATED ON 10 SEP 2009 BY SENTIL    
--PROCEDURE TO RECORD BOOKING DETAILS     
    
CREATE PROCEDURE [dbo].[FOB_SUB_BookingDetails]    
(    
	 @BRId AS NUMERIC(18,0) = 0,     
	 @StockId AS NUMERIC(18,0) = 0,    
	 @DealerId AS NUMERIC(18,0) = 0,    
	 @PaymentMode AS SMALLINT = 0,    
	 @EntryDateTime AS DATETIME,
	 @Amount AS NUMERIC(18,2) = 0.00,	    
	 @PaymentDetails AS  VARCHAR(150) = NULL,    
	 @ID AS INT OUT,    
	 @BookingCode AS VARCHAR(50) OUT    
)    
AS    
BEGIN    
    
--TO GET THE YEAR MONTH DAY TO GENERATE BOOKINGCODE    
DECLARE @YEAR VARCHAR(10)    
DECLARE @MONTH VARCHAR(10)    
DECLARE @DAY VARCHAR(30)    
  
 
DECLARE @BRDID INT  
	  --TO CHECK WHETHER BRID IT EXISTS OR NOT 
	  IF NOT EXISTS( SELECT BRId FROM FOB_CWBookingDetails WHERE BRId = @BRId )
		  BEGIN	
			  INSERT INTO FOB_CWBookingDetails    
					 ( BRId, StockId, DealerId, Amount , PaymentMode, EntryDateTime, PaymentDetails )    
			   VALUES( @BRId, @StockId, @DealerId, @Amount , @PaymentMode, @EntryDateTime, @PaymentDetails )     
			      
			  SET @ID = SCOPE_IDENTITY()    
			     
			  SET @YEAR = CONVERT(VARCHAR,YEAR(GETDATE()))    
			  SET @MONTH = CONVERT(VARCHAR,MONTH(GETDATE()))    
			  SET @DAY = CONVERT(VARCHAR,DAY(GETDATE()))    
			     
			  SET @BookingCode = 'CWFOBC' + @YEAR + @MONTH + @DAY + CONVERT(VARCHAR,@ID)     
			      
			  --To UPDATE BOOKINCODE WITH THE GENERATED ONE    
			  UPDATE FOB_CWBookingDetails SET BookingCode = @BookingCode WHERE ID = @ID    
			     
			  --To Collect BookingRequest ID and StockID  
			  SELECT @BRDID = BRId FROM FOB_CWBookingDetails WHERE ID = @ID  
			    
			  --TO UPDATE Booking Request Table with IsBooked Status True  
			  UPDATE FOB_BookingRequestData SET IsBooked = 1, IsPaid = 1 WHERE Id = @BRDID  
			  
			   --To Update Stocks Table For Availability  
				UPDATE FOB_Stocks SET StockCount = StockCount -1, UpdatedOn = GETDATE() 
				WHERE Id = @StockID 
			   
		  END 
	  ELSE
		  BEGIN	
				SET @ID = -1
		  END	
--SELECT *  FROM FOB_CWBookingDetails    
--TRUNCATE TABLE FOB_CWBookingDetails     

	  	   
END
