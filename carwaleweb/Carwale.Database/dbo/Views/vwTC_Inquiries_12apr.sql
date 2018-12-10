IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwTC_Inquiries_12apr' AND
     DROP VIEW dbo.vwTC_Inquiries_12apr
GO

	


 
-- Author:  Surendra         
-- Create date: 3rd Feb,2012         
-- Description: To Club all type of inquiries to display in Worksheet an Inquiries         
-- =============================================      r   
CREATE VIEW [dbo].[vwTC_Inquiries_12apr] AS   
  
-- Getting All Inquiries for Seller             
SELECT CD.CustomerName,
       CD.Mobile,   
       CD.Email,   
       INQ.ModelId,   
       INQ.MakeId,   
       IL.TC_InquiriesLeadId,   
       IL.TC_CustomerId,   
       IL.TC_InquiryTypeId,   
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
       it.InquiryType, 
       TCB.StockId AS stockid,    
       --NULL as SOURCE,   
       --NULL AS regno,   
       --NULL AS makeyear,   
       --NULL As colour,   
       --NULL AS price,   
       --NULL AS stockid,   
       --NULL AS profileid,   
       FA.ActionName
FROM   TC_Inquiries INQ WITH(NOLOCK) 
       LEFT OUTER JOIN  TC_BuyerInquiries TCB WITH(NOLOCK)  ON TCB.TC_InquiriesId=INQ.TC_InquiriesId and INQ.InquiryType=1
       JOIN TC_InquiriesLead IL WITH(NOLOCK)  ON (INQ.TC_CustomerId = IL.TC_CustomerId  AND INQ.InquiryType=IL.TC_InquiryTypeId)
       JOIN TC_CustomerDetails AS CD WITH(NOLOCK) ON CD.id = IL.tc_customerid  
       LEFT OUTER JOIN TC_Users U WITH(NOLOCK) ON IL.TC_UserId=U.Id    
       JOIN TC_InquiryType IT WITH(NOLOCK) ON INQ.InquiryType=IT.TC_InquiryTypeId 
       LEFT OUTER JOIN  TC_InquiryStatus ST ON IL.TC_InquiryStatusId=ST.TC_InquiryStatusId
       LEFT OUTER JOIN TC_InquiriesFollowupAction FA  WITH(NOLOCK) ON IL.TC_InquiriesFollowupActionId=FA.TC_InquiriesFollowupActionId       
       
       



