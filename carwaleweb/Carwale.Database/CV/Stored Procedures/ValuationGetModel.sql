IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CV].[ValuationGetModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CV].[ValuationGetModel]
GO

	-- =============================================  
-- Author: Umesh Ojha  
-- Create date:21 feb 2013
-- Details: Fetching Valuation models 
-- =============================================  
CREATE PROCEDURE [CV].[ValuationGetModel] 
@CarYear AS SMALLINT,
@MakeId INT                                            
AS
BEGIN
SET NOCOUNT ON
SELECT DISTINCT Mo.ID AS Value, Mo.Name AS Text  
        FROM CarModels Mo WITH(NOLOCK)              
             JOIN CarVersions  AS Ve WITH(NOLOCK) ON Mo.Id=Ve.CarModelId 
             JOIN CarValues    AS CV WITH(NOLOCK) ON Ve.ID=CV.CarVersionId 
        WHERE 
              Ve.IsDeleted = 0
         AND  CV.CarYear=@CarYear
         AND  MO.CarMakeId=@MakeId
         ORDER BY Mo.Name
END
