IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CV].[ValuationGetVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CV].[ValuationGetVersion]
GO

	                   
                         
-- =============================================  
-- Author: Umesh Ojha  
-- Create date:21 feb 2013
-- Details: Fetching Valuation Versions 
-- =============================================  
CREATE PROCEDURE [CV].[ValuationGetVersion] 
@CarYear AS SMALLINT,
@ModelId INT                                            
AS
BEGIN
SET NOCOUNT ON
SELECT DISTINCT  Ve.ID AS Value, Ve.Name AS Text  
        FROM CarVersions Ve WITH(NOLOCK)             
             JOIN CarValues CV WITH(NOLOCK) ON Ve.ID=CV.CarVersionId 
        WHERE 
              Ve.IsDeleted = 0
         AND  CV.CarYear=@CarYear
         AND  Ve.CarModelId=@ModelId
         ORDER BY Ve.Name
END         
