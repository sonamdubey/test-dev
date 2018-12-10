IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetLiveListingMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetLiveListingMakes]
GO

	
-- =============================================      
-- Author:  <Pawan kumar>      
-- Create date: <09 Aug 2015>
-- Description: <Returns live listing makes>
-- [cw].[GetLiveListingMakes]
-- =============================================  
CREATE PROCEDURE [cw].[GetLiveListingMakes]     
AS      
BEGIN      
  -- SET NOCOUNT ON added to prevent extra result sets from      
  -- interfering with SELECT statements.      
 SET NOCOUNT ON;  
Select Distinct Lv.MakeId, Lv.MakeName 
                FROM LiveListings Lv WITH(NOLOCK) 
                 Order By Lv.MakeName
END
  
