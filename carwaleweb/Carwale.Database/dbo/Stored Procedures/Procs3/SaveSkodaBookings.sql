IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveSkodaBookings]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveSkodaBookings]
GO

	
CREATE PROCEDURE [dbo].[SaveSkodaBookings]        
@CarId INT,        
@CustomerCity NUMERIC(18, 0),        
@CustomerId NUMERIC(18,0),    
@CustomerName VARCHAR(100),        
@CustomerEmail VARCHAR(100),        
@CustomerMobile VARCHAR(12),        
@ID NUMERIC(18,0) OUTPUT        
AS        
BEGIN        
       
 DECLARE @ExShowroom NUMERIC(18,0)      
 DECLARE @RTO NUMERIC(18,0)      
 DECLARE @Insurance NUMERIC(18,0)   
 DECLARE @BookingAmount NUMERIC(18,0)     
       
 SELECT @ExShowroom = IsNull(Price,0), @RTO = IsNull(RTO, 0), @Insurance = IsNull(Insurance,0)      
 FROM NewCarShowroomPrices      
 WHERE CarVersionId = @CarId AND CityId = @CustomerCity  
   
 SELECT @BookingAmount = ISNULL(BookingAmount, 0)  
 FROM SB_BookingAmount  
 WHERE CarId = @CarId      
        
 INSERT INTO SkodaBookings        
 (CarId,CustomerCity, CustomerId, CustomerName,CustomerEmail, CustomerMobile, ExShowroom, RTO, Insurance, BookingAmount, EntryDateTime)        
 VALUES        
 (@CarId,@CustomerCity, @CustomerId, @CustomerName,@CustomerEmail,@CustomerMobile, @ExShowroom, @RTO, @Insurance, @BookingAmount, GETDATE())        
         
 SET @ID = SCOPE_IDENTITY() 
 
 UPDATE SkodaBookings
 SET	BookingCode = 'CWSOBC'+ CONVERT(varchar,GETDATE(),112) + CONVERT(VARCHAR, @ID)       
 WHERE	ID = @ID 
        
END
