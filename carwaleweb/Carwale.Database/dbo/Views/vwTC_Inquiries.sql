IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwTC_Inquiries' AND
     DROP VIEW dbo.vwTC_Inquiries
GO

	

 
-- Author:  Surendra               
-- Create date: 3rd Feb,2012               
-- Description: To Club all type of inquiries to display in Worksheet an Inquiries 
-- Modified by Binu 25-06-2012 to add  IL.LastFollowUpComment             
-- =============================================             
CREATE VIEW [dbo].[vwTC_Inquiries] AS         
        
-- Getting All Inquiries for Seller                   
SELECT CD.CustomerName,      
       CD.Mobile,         
       CD.Email,         
       INQ.ModelId,         
       INQ.MakeId,         
       IL.TC_InquiriesLeadId,         
       IL.TC_CustomerId,         
       INQ.InquiryType as TC_InquiryTypeId,         
       IL.BranchId,         
       IL.tc_inquirystatusid,         
       IL.tc_inquiriesfollowupactionid,         
       IL.TC_UserId,         
       IL.IsActionTaken,         
       IL.InquiryCount,         
       IL.NextFollowUpDate,         
       CarName,         
       INQ.CreatedDate  AS inquirydate,         
       U.UserName,         
       ST.Status as Status,         
       INQ.SourceId,         
		--       it.InquiryType,       
		LT.LeadType,    
		IL.InqTypeDesc,    
		TCB.StockId AS stockid,             
       --NULL as SOURCE,         
       --NULL AS regno,         
       --NULL AS makeyear,         
       --NULL As colour,         
       --NULL AS price,         
       --NULL AS stockid,         
       --NULL AS profileid,         
       FA.ActionName,
       IL.LastFollowUpComment      
FROM   TC_Inquiries INQ WITH(NOLOCK)       
       LEFT OUTER JOIN  TC_BuyerInquiries TCB WITH(NOLOCK)  ON TCB.TC_InquiriesId=INQ.TC_InquiriesId and INQ.InquiryType=1      
       JOIN TC_InquiriesLead IL WITH(NOLOCK)  ON (INQ.TC_CustomerId = IL.TC_CustomerId  AND INQ.TC_LeadTypeId=IL.TC_LeadTypeId)      
       JOIN TC_CustomerDetails AS CD WITH(NOLOCK) ON CD.id = IL.tc_customerid        
       LEFT OUTER JOIN TC_Users U WITH(NOLOCK) ON IL.TC_UserId=U.Id          
       --JOIN TC_InquiryType IT WITH(NOLOCK) ON INQ.InquiryType=IT.TC_InquiryTypeId    
       JOIN TC_LeadType LT WITH (NOLOCK) ON IL.TC_LeadTypeId=LT.TC_LeadTypeId    
       LEFT OUTER JOIN  TC_InquiryStatus ST ON IL.TC_InquiryStatusId=ST.TC_InquiryStatusId      
       LEFT OUTER JOIN TC_InquiriesFollowupAction FA  WITH(NOLOCK) ON IL.TC_InquiriesFollowupActionId=FA.TC_InquiriesFollowupActionId   
WHERE IL.IsActive=1  

