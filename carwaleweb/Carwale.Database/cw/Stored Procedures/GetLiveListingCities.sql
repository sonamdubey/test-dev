IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetLiveListingCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetLiveListingCities]
GO

	

-- =============================================      
-- Author:  <Amit Verma>      
-- Create date: <18 Jan 2013>
-- Description: <Returns live listing cities>
-- [cw].[GetLiveListingCities]
-- No lock condition added
-- edited by <Pawan kumar>
-- =============================================  
CREATE PROCEDURE [cw].[GetLiveListingCities]     
AS      
BEGIN      
  -- SET NOCOUNT ON added to prevent extra result sets from      
  -- interfering with SELECT statements.      
 SET NOCOUNT ON;  
	SELECT CityName AS Name, CityId AS Id FROM LL_Cities WITH(NOLOCK) ORDER BY Name
END
  
