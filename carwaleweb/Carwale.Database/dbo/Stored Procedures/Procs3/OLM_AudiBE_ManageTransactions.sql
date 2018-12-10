IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_ManageTransactions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_ManageTransactions]
GO

	-- =============================================  
-- Author:  Ashish G. Kamble  
-- Create date: 24/7/2013  
-- Description: Proc to begin the customer transaction.   
--    Customer will be registered and a new transaction will be started for the customer.  
-- Modifier	: Vaibhav K (5-8-2013)
-- Description : If model is changed by the user then make fueltype as -1 and also update customerId of transaction
-- Description : Handled default values to set to -1 when updating transactions and modelId is changed
-- =============================================  
CREATE PROCEDURE [dbo].[OLM_AudiBE_ManageTransactions]  
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
    DECLARE @IsNewCustomerInserted BIT = 0
    
    -- Register Customer
    EXEC OLM_AudiBE_ManageCustomers @CustomerId OUTPUT, @CustName, @CustMobile, @stateId, @cityId, @CustEmail, @sourceId, @TranDate, @IsNewCustomerInserted OUTPUT
      
    -- Insert data into OLM_AudiBE_Transactions table
    
    IF @IsNewCustomerInserted = 1 OR @TransactionId = '-1'
		BEGIN
			INSERT INTO OLM_AudiBE_Transactions  
				(CustomerId, ModelId, CityId, SourceId, TransactionDate, FuelTypeId)  
			VALUES 
				(@CustomerId, @ModelId, @CityId, @sourceId, @TranDate, -1)
			
			SET @TransactionId = SCOPE_IDENTITY()  
		END		
    ELSE
		BEGIN
			--IF MODEL IS CHANGED BY THE USER THEN MAKE FUELTYPEID AS -1 ELSE OLD VALUE	
			--Also make all other detials to -1 if model is changed by the user		
			UPDATE OLM_AudiBE_Transactions
			SET
				CustomerId = @CustomerId,
				CityId = @cityId,
				FuelTypeId = CASE WHEN ModelId <> @ModelId THEN -1 ELSE FuelTypeId END,
				VersionId = CASE WHEN ModelId <> @ModelId THEN -1 ELSE VersionId END,
				GradeId = CASE WHEN ModelId <> @ModelId THEN -1 ELSE GradeId END,
				ModelColorId = CASE WHEN ModelId <> @ModelId THEN -1 ELSE ModelColorId END,
				UpholestryColorId = CASE WHEN ModelId <> @ModelId THEN -1 ELSE UpholestryColorId END,
				ExShowRoomPrice = CASE WHEN ModelId <> @ModelId THEN 0 ELSE ExShowRoomPrice END,
				VersionPriceId = CASE WHEN ModelId <> @ModelId THEN -1 ELSE VersionPriceId END,
				DealerId = CASE WHEN ModelId <> @ModelId THEN -1 ELSE DealerId END,
				ModelId = @ModelId
			WHERE
				Id = @TransactionId
		END	    
END