IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwTC_InquiriesForExcel_12apr' AND
     DROP VIEW dbo.vwTC_InquiriesForExcel_12apr
GO

	

-- Modified by:  Binumon George             
-- Modified date: 16 Feb,2012             
-- Description: retriving makeyear, kms for excel
-- ==================================================================================== 
-- Author:  Surendra             
-- Create date: 3rd Feb,2012             
-- Description: To Club all type of inquiries to display in Worksheet an Inquiries             
-- =============================================      r       
CREATE VIEW [dbo].[vwTC_InquiriesForExcel_12apr] AS       
      
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
       SI.RegistrationPlace,   
       COALESCE(STO.RegNo,SI.RegNo)AS RegNo,  
       COALESCE(STO.MakeYear,SI.MakeYear) AS makeyear,
       COALESCE(STO.Kms,SI.Kms) AS Kms,     
       COALESCE(STO.Colour,SI.Colour) AS colour,    
       COALESCE(STO.Price,SI.Price) AS price,   
       STO.Id AS stockid,       
      COALESCE(SINQ.Id,SI.CWInquiryId) AS profileid,      
       SRC.Source  as SOURCE,       
       --NULL AS regno,       
       --NULL AS makeyear,       
       --NULL As colour,       
       --NULL AS price,       
       --NULL AS stockid,       
       --NULL AS profileid,       
       FA.ActionName    
FROM   TC_Inquiries INQ WITH(NOLOCK)       
       JOIN TC_InquiriesLead IL WITH(NOLOCK)  ON (INQ.TC_CustomerId = IL.TC_CustomerId  AND INQ.InquiryType=IL.TC_InquiryTypeId)    
       JOIN TC_CustomerDetails AS CD WITH(NOLOCK) ON CD.id = IL.TC_CustomerId      
       LEFT OUTER JOIN TC_Users U WITH(NOLOCK) ON IL.TC_UserId=U.Id        
       JOIN TC_InquiryType IT WITH(NOLOCK) ON INQ.InquiryType=IT.TC_InquiryTypeId     
       LEFT OUTER JOIN  TC_InquiryStatus ST ON IL.TC_InquiryStatusId=ST.TC_InquiryStatusId and ST.IsActive = 1
       LEFT OUTER JOIN TC_InquiriesFollowupAction FA  WITH(NOLOCK) ON IL.TC_InquiriesFollowupActionId=FA.TC_InquiriesFollowupActionId     
       LEFT OUTER JOIN TC_BuyerInquiries BI WITH(NOLOCK)  ON INQ.TC_InquiriesId= BI.TC_InquiriesId    
       LEFT OUTER JOIN TC_Stock STO WITH(NOLOCK)  ON  STO.Id=BI.StockId    
       LEFT OUTER JOIN SellInquiries SINQ WITH(NOLOCK)  ON  SINQ.TC_StockId=STO.Id    
       LEFT OUTER JOIN TC_SellerInquiries SI WITH(NOLOCK)  ON SI.TC_InquiriesId=INQ.TC_InquiriesId
       INNER JOIN TC_InquirySource SRC WITH(NOLOCK)  ON  INQ.SourceId=SRC.Id

