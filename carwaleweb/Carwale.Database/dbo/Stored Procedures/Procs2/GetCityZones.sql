IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCityZones]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCityZones]
GO

	
-- =============================================        
-- Author:  Vinayak        
-- Create date: 27 nov 2014      
-- Description: Fetching all zones
--exec [GetCityZones] 1
-- =============================================        
CREATE PROCEDURE [dbo].[GetCityZones] 
@CityId SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from        
	-- interfering with SELECT statements.        
	SET NOCOUNT ON;
	SELECT DISTINCT
                       cz.ZoneName
                       ,CZ.ID AS ZoneId
			FROM CityZones CZ WITH(NOLOCK)
			   WHERE         
					CZ.IsActive =1
                    AND CZ.CityId=@CityId
END
