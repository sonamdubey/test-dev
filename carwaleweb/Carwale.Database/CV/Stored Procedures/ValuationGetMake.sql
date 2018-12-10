IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CV].[ValuationGetMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CV].[ValuationGetMake]
GO

	-- =============================================  
-- Author:   
-- Create date:
-- Details: 
-- =============================================  
CREATE PROCEDURE [CV].[ValuationGetMake] @CarYear AS SMALLINT                                            
AS
BEGIN
SET NOCOUNT ON
SELECT  DISTINCT Ma.ID AS Value, 
                 Ma.Name AS Text 
        FROM CarMakes  AS Ma 
             JOIN CarModels    AS Mo ON Ma.Id=Mo.CarMakeId 
             JOIN CarVersions  AS Ve ON Mo.Id=Ve.CarModelId 
             JOIN CarValues    AS CV ON Ve.ID=CV.CarVersionId 
        WHERE 
              Ve.IsDeleted = 0
         AND  CV.CarYear=@CarYear
         ORDER BY Ma.Name
END
