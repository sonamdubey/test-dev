IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CV].[GetValuationCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CV].[GetValuationCities]
GO

	-- =============================================    
-- Author:  Umesh Ojha    
-- Create date: 15/2/2013    
-- Description: Fetching CAR Valuation city     
-- =============================================    
CREATE PROCEDURE [CV].[GetValuationCities]      
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;  
    
	SELECT CD.CityId, Ci.Name  
	FROM CarValuesCityDeviation CD  
	INNER JOIN Cities Ci ON Ci.Id = CD.CityId ORDER BY Name  
END
