IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CV].[InsertValuation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CV].[InsertValuation]
GO

	-- =============================================    
-- Author:  Umesh Ojha    
-- Create date: 12/2/2013    
-- Description: Inserting New request for carvaluation  
-- =============================================    
CREATE PROCEDURE [CV].[InsertValuation]  
 -- Add the parameters for the stored procedure here    
 @CarVersionId  INT, -- Car Version Id    
 @CarYear  DATETIME, -- Car Year    
 @CarKms  INT, -- Car Mileage    
 @CustomerId  INT, -- CustomerId    
 @City   VARCHAR(100), -- City    
 @CityId   INT, -- CityId    
 @ActualCityId  INT, -- ActualCityId    
 @RequestDateTime DATETIME, -- Entry Date    
 @RemoteHost  VARCHAR(100),    
 @RequestSource INT,  -- 1 or 2   
 @ValueExcellent NUMERIC,  
 @ValueGood NUMERIC,  
 @ValueFair NUMERIC,  
 @ValuePoor NUMERIC,  
 @ValueExcellentDealer NUMERIC,  
 @ValueGoodDealer NUMERIC,  
 @ValueFairDealer NUMERIC,  
 @ValuePoorDealer NUMERIC,  
 @ValuationId  NUMERIC OUTPUT -- Valuation Id   
    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;  
    
INSERT INTO CarValuations   
	(CarVersionId, CarYear, CustomerId, City, RequestDateTime,   
	RemoteHost, RequestSource, Kms, CityId, ActualCityId ,  
	ValueExcellent,ValueGood,ValueFair,ValuePoor,ValueExcellentDealer,  
	ValueGoodDealer,ValueFairDealer,ValuePoorDealer)    
VALUES (@CarVersionId, @CarYear, @CustomerId, @City, @RequestDateTime,  
	@RemoteHost, @RequestSource, @CarKms, @CityId, @ActualCityId,  
	@ValueExcellent,@ValueGood,@ValueFair,@ValuePoor,@ValueExcellentDealer,  
	@ValueGoodDealer,@ValueFairDealer,@ValuePoorDealer)   
	SET @ValuationId = SCOPE_IDENTITY()    
END
