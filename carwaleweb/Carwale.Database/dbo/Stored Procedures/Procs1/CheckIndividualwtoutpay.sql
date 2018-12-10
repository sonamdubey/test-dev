IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckIndividualwtoutpay]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckIndividualwtoutpay]
GO

	
  
-- =============================================    
-- Author:  Manish   
-- Create date: 28-12-2012    
-- Description: Alert if any individual car listed with no transaction detail.    
-- =============================================    
CREATE PROCEDURE [dbo].[CheckIndividualwtoutpay]     
AS    
BEGIN    
select count(distinct l.Inquiryid)  AS [Total Individual Car Listed with no payment detail] 
from CustomerSellInquiries as csi WITH (NOLOCK)
join LiveListings          as l WITH (NOLOCK) on l.Inquiryid=csi.ID and l.SellerType=2  
where 
l.Inquiryid not in   
(select pgt.CarId from PGTransactions as pgt WITH (NOLOCK) where ProcessCompleted=1 and TransactionCompleted=1)  
AND  
l.Inquiryid  not in (select pgt.CarId from CDTransactions as pgt WITH (NOLOCK))  
end
