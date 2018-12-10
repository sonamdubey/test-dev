IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ServiceBooking_View]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ServiceBooking_View]
GO

	
-------------------------------------------------
  
-- ============================================= [Microsite_ServiceBooking_View] 5      
-- Author:  Kritika Choudhary     
-- Create date : 12th Oct 2015     
-- Description : To view Service booking details

-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_ServiceBooking_View]      
(        
 @Id INT
)          
AS      
BEGIN      
 SET NOCOUNT ON;      
 
   SELECT MC.Name, MC.EmailId, MC.MobileNum, MSB.PreferredDateTime,CM.Name AS ModelName, CMK.Name AS MakeName,MSB.AutobizInqId,SC.Address
   FROM Microsite_Customers MC WITH(NOLOCK)
   JOIN Microsite_ServiceBooking MSB WITH(NOLOCK) ON MC.Id = MSB.Microsite_CustomerId 
   LEFT JOIN TC_ServiceCenter SC  WITH(NOLOCK) ON MSB.ServiceCenterId=SC.TC_ServiceCenterId and SC.IsActive=1
   JOIN CarModels CM WITH(NOLOCK) ON CM.ID = MSB.ModelId
   JOIN CarMakes CMK WITH(NOLOCK) ON CMK.ID = CM.CarMakeId
   WHERE MSB.Id=@Id
         
END 

