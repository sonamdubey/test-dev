IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_BeginTransaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_BeginTransaction]
GO

	
-- =============================================  
-- Author:  Ashish G. Kamble  
-- Create date: 24/7/2013  
-- Description: Proc to begin the customer transaction.   
--    Customer will be registered and a new transaction will be started for the customer.  
-- =============================================  
CREATE PROCEDURE [dbo].[OLM_AudiBE_BeginTransaction]  
 @TransactionId BIGINT OUT,  
 @CustomerId BIGINT OUT,  
 @CustName VARCHAR(100),  
 @CustMobile VARCHAR(50),  
 @stateId Numeric,  
 @cityId Numeric,  
 @CustEmail VARCHAR(50),  
 @sourceId INT,  
 @ModelId INT  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    DECLARE @TranDate DATETIME = GETDATE() 
    
    -- Register Customer
    EXEC OLM_AudiBE_InsertCustomers @CustomerId OUTPUT, @CustName, @CustMobile, @stateId, @cityId, @CustEmail, @sourceId, @TranDate  
      
    -- Insert data into OLM_AudiBE_Transactions table
    
    IF @TransactionId <> '-1'
		BEGIN
			EXEC OLM_AudiBE_UpdateTransaction @TransactionId, @cityId, @sourceId, @ModelId
		END		
    ELSE
		BEGIN
			INSERT INTO OLM_AudiBE_Transactions  
				(CustomerId, ModelId, CityId, SourceId, TransactionDate)  
			VALUES 
				(@CustomerId, @ModelId, @CityId, @sourceId, @TranDate)
		END
		
    SET @TransactionId = SCOPE_IDENTITY()  
END
