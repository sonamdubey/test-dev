IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetServicePayment_Details]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetServicePayment_Details]
GO

	  
-- ============================================= [Microsite_GetServicePayment_Details] 5      
-- Author:  Kritika Choudhary     
-- Create date : 23rd Nov 2015     
-- Description : To get service payment details

-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_GetServicePayment_Details]      
(        
 @BillNum INT
)          
AS      
BEGIN      
 SET NOCOUNT ON;      
 
SELECT SI.CustomerName, SI.CustomerEmail, SI.CustomerMobile,SIB.TotalPrice,CMK.Name +' '+ CM.Name AS Car,SC.ServiceCenterName,SIB.TC_ServiceInquiriesId AS BillNum, ISNULL(AT.IsPaymentSuccess,0) AS IsPaymentSuccess
FROM  TC_ServiceInquiries SI WITH(NOLOCK)
JOIN TC_ServiceInquiriesBill SIB WITH(NOLOCK) ON SI.TC_ServiceInquiriesId = SIB.TC_ServiceInquiriesId
JOIN CarModels CM WITH(NOLOCK) ON CM.ID = SI.CarModelId
JOIN CarMakes CMK WITH(NOLOCK) ON CMK.ID = CM.CarMakeId
JOIN TC_ServiceCenter SC WITH(NOLOCK) ON SC.TC_ServiceCenterId = SI.TC_ServiceCenterId
Join Microsite_AtomPGTransactions AT WITH(NOLOCK) ON AT.CustId=SI.TC_ServiceInquiriesId
WHERE SI.TC_ServiceInquiriesId = @BillNum 
ORDER BY AT.EntryDate DESC
END 

