IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CV].[GetUsedCarValuation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CV].[GetUsedCarValuation]
GO

	-- =============================================    
-- Author:  Umesh Ojha    
-- Create date: 13/2/2013    
-- Description: Fetching data for car valuation    
-- =============================================    
CREATE PROCEDURE [CV].[GetUsedCarValuation]     
 -- Add the parameters for the stored procedure here    
 @ValuationId BIGINT 
    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;  
    
  SELECT ValueExcellentDealer,ValueFairDealer,ValueGoodDealer,ValueExcellent,ValueFair,ValueGood 
  FROM CarValuations where Id = @ValuationId   
 
END
