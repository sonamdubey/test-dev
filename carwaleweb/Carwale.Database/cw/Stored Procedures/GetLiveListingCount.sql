IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetLiveListingCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetLiveListingCount]
GO

	
-- =============================================      
-- Author:  <Amit Verma>      
-- Create date: <18 Jan 2013>      
-- Description: <Returns live listing count value> 
-- [CW].[GetLiveListingCount]
-- =============================================  
CREATE PROCEDURE [cw].[GetLiveListingCount]     
AS      
BEGIN      
  -- SET NOCOUNT ON added to prevent extra result sets from      
  -- interfering with SELECT statements.      
 SET NOCOUNT ON;  
	Select Count(ProfileId) as LiveStockCount From LiveListings WITH(NOLOCK)
END
  
  
