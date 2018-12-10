IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FOB_SUBMIT_BRD]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FOB_SUBMIT_BRD]
GO

	
--CREATED ON 10 SEP 2009 BY SENTIL          
--PROCEDURE FOR SUBMIT BOOKING REQUEST DATA          
          
CREATE PROCEDURE [dbo].[FOB_SUBMIT_BRD]           
(          
 @CustomerId AS NUMERIC(18,0) = 0,          
 @CustomerName AS VARCHAR(100) = NULL,          
 @EmailId AS VARCHAR(100) = NULL,          
 @Mobile AS VARCHAR(50) = NULL,          
 @Landline AS VARCHAR(50) = NULL,          
 @CityId AS NUMERIC(18,0) = 0,          
 @DeliveryCityId AS NUMERIC(18,0) = 0,          
 @DealerId AS NUMERIC(18,0) = 0,          
 @RequestType AS SMALLINT = 0,     
 @VersionID AS NUMERIC(18,0)=0,     
 @TestDrive AS BIT = 0 ,    
 @AvailLoan  AS BIT = 0,       
 @Color AS NUMERIC(18,0)=0,   
 @Amount AS NUMERIC(18,2) =0.00,          
 @EntryDateTime AS DATETIME,          
 @ID AS BIGINT OUT           
)          
AS          
BEGIN      
      
 INSERT INTO FOB_BookingRequestData          
        ( CustomerId, CustomerName, EmailId, Mobile, Landline, CityId, DeliveryCityId, DealerId, RequestType,     
		  TestDrive, AvailLoan, VersionID ,Color, Amount, EntryDateTime)          
  VALUES( @CustomerId, @CustomerName, @EmailId, @Mobile, @Landline, @CityId, @DeliveryCityId, @DealerId, @RequestType,     
		  @TestDrive, @AvailLoan, @VersionID, @Color, @Amount, @EntryDateTime)           
          
 SET @ID = SCOPE_IDENTITY()          
          
--SELECT * FROM FOB_BookingRequestData          
--TRUNCATE TABLE FOB_BookingRequestData           
          
END      
  