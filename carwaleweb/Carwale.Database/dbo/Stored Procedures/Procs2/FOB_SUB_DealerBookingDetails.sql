IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FOB_SUB_DealerBookingDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FOB_SUB_DealerBookingDetails]
GO

	
--CREATED ON 10 SEP 2009 BY SENTIL    
--PROCEDURE TO RECORD DEALER BOOKING DETAILS     
    
CREATE PROCEDURE [dbo].[FOB_SUB_DealerBookingDetails]    
(    
	@BRId AS NUMERIC(18,0) = 0,     
	@StockId AS NUMERIC(18,0) = 0,    
	@DealerId AS NUMERIC(18,0) = 0,    
	@Amount AS NUMERIC(18,2) = 0.00,
	@PaymentMode AS SMALLINT = 0,    
	@EntryDateTime AS DATETIME,    
	@PaymentDetails AS  VARCHAR(150) = NULL,    
	@DealerOrganizationID AS  NUMERIC(18,0) = 0,    
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
	  IF NOT EXISTS( SELECT BRId FROM FOB_DealerBookingDetails WHERE BRId = @BRId )
		  BEGIN	
			INSERT INTO FOB_DealerBookingDetails    
						( BRId, StockId, DealerId, Amount , PaymentMode, EntryDateTime, PaymentDetails, DealerOrganizationID )    
				  VALUES( @BRId, @StockId, @DealerId, @Amount , @PaymentMode, @EntryDateTime, @PaymentDetails, @DealerOrganizationID )     

			SET @ID = SCOPE_IDENTITY()    

			SET @YEAR = CONVERT(VARCHAR,YEAR(GETDATE()))    
			SET @MONTH = CONVERT(VARCHAR,MONTH(GETDATE()))    
			SET @DAY = CONVERT(VARCHAR,DAY(GETDATE()))    

			SET @BookingCode = 'CWFOBD' + @YEAR + @MONTH + @DAY + CONVERT(VARCHAR,@ID)     

			--To UPDATE BOOKINCODE WITH THE GENERATED ONE    
			UPDATE FOB_DealerBookingDetails SET BookingCode = @BookingCode WHERE ID = @ID    
			
			--To Collect BookingRequest ID and StockID
			SELECT @BRDID = BRId FROM FOB_DealerBookingDetails WHERE ID = @ID

			--TO UPDATE Booking Request Table with IsBooked Status True
			UPDATE FOB_BookingRequestData SET IsBooked = 1 WHERE Id = @BRDID

			--To Update Stocks Table For Availability
			UPDATE FOB_Stocks SET StockCount = StockCount - 1, UpdatedOn = GETDATE() 
			WHERE Id = @StockId				
		  END 
	  ELSE
		  BEGIN	
				SET @ID = -1
		  END	
    
--SELECT *  FROM FOB_DealerBookingDetails    
--TRUNCATE TABLE FOB_DealerBookingDetails     
    
END


