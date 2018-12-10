IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_InsertValuationRequest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_InsertValuationRequest]
GO

	 --=============================================
-- Author:		Kumar Vikram
-- Create date: 11.12.2013
-- Description:FOR INSERTING RECORDS
 --=============================================  
CREATE PROCEDURE [dbo].[AxisBank_InsertValuationRequest]  
 @FileReferenceNumber VARCHAR(20), -- File Reference Number	
 @CarVersionId  NUMERIC, -- Car Version Id  
 @CarYear  DATETIME, -- Car Year  
 @CarKms  NUMERIC, -- Car Mileage  
 @RegistrationNumber VARCHAR(50), -- Car Registration Number
 @Asc  NUMERIC(18,0),
 @UserId  NUMERIC(18,0), -- CustomerId  
 @City   VARCHAR(100), -- City  
 @CityId   NUMERIC, -- CityId  
 @ActualCityId  NUMERIC, -- ActualCityId  
 @RequestDateTime DATETIME, -- Entry Date  
 @RemoteHost  VARCHAR(100),  
 @RequestSource INT,  -- 1 or 2
 @CarCondition Varchar(20),  
 @ValuationId  NUMERIC OUTPUT -- Valuation Id  
   
 AS  
  
BEGIN  
 INSERT INTO AxisBank_CarValuations(FileReferenceNumber, CarVersionId, CarYear, RegistrationNumber, ASC_Id, CustomerId, City, RequestDateTime, RemoteHost, RequestSource, CarCondition, Kms, CityId, ActualCityId)  
 VALUES (@FileReferenceNumber, @CarVersionId, @CarYear, @RegistrationNumber, @Asc, @UserId, @City, @RequestDateTime, @RemoteHost, @RequestSource, @CarCondition, @CarKms, @CityId, @ActualCityId)  
  
 SET @ValuationId = SCOPE_IDENTITY()     
    
END  
