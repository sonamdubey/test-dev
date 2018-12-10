IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BuyerOnStockDetailsForAPI]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BuyerOnStockDetailsForAPI]
GO

	   

-- =======================================================================
-- Created By: Nilesh Utture
-- Created On: 30th April, 2013
-- Description:This SP will give buyer Inquiry on stock details for API 
-- EXEC TC_BuyerOnStockDetailsForAPI 890,5,1
-- Modified By: Nilesh Utture on 1st September, 2013 Added COLUMN NS_ShowClose
-- Modified By Vivek Gupta on 30-09-2015, Added MostInterested for inquiries
-- Modified By :Khushaboo Patil on 17th March , Added columns InquiriesLeadId,LeadInquiryTypeId
-- =======================================================================
CREATE PROCEDURE [dbo].[TC_BuyerOnStockDetailsForAPI] @TC_LeadId INT,@BranchId INT,@TC_UserId INT
AS
BEGIN 
  
  SELECT TC_InquiriesLeadId,
		BranchId,
		TC_CustomerId,
		TC_UserId,
		IsActive,
		TC_LeadId,
		TC_LeadInquiryTypeId,
		TC_LeadDispositionID
	  INTO #TC_InquiriesLead	
	  FROM TC_InquiriesLead as L1 WITH (NOLOCK) 
	  WHERE L1.BranchId=@BranchId ANd L1.TC_LeadId=@TC_LeadId
	  
	   SELECT B.TC_BuyerInquiriesId as Id,
			  CONVERT(VARCHAR, S.Id) as StockId,
			  (CASE L.TC_LeadInquiryTypeId WHEN 1 THEN 'Buyer' END) AS Type, 
			  V.Car, 
			  B.CreatedOn,
			  SRC.Source AS Source,
			  REPLACE(RIGHT(CONVERT(VARCHAR(9), S.MakeYear, 6), 6), ' ', '-') AS Year, 
			  S.Colour AS Color, 
			  S.Price AS Price, 
			  S.Kms AS Distance,
			  S.RegNo + '(' + CC.RegistrationPlace + ')' AS Registration, 
			  (CASE CC.Owners 
				WHEN 1 THEN 'First' 
				WHEN 2 THEN 'Second' 
				WHEN 3 THEN 'Third' 
				WHEN 4 THEN 'Fourth' 
				WHEN 5 THEN 'More than four' END) 
				AS Owner,
				(CASE B.BookingStatus WHEN 34 THEN 'false' ELSE 'true' END) AS NS_ShowClose,
				ISNULL(B.MostInterested,0) AS MostInterested,
				L.TC_InquiriesLeadId AS InquiriesLeadId,L.TC_LeadInquiryTypeId AS LeadInquiryTypeId
					 FROM        #TC_InquiriesLead L 
					 INNER JOIN  TC_BuyerInquiries B WITH (NOLOCK)  ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId 
					 INNER JOIN  TC_Stock          S WITH (NOLOCK)  ON B.StockId = S.Id  
					 INNER JOIN TC_CarCondition CC WITH (NOLOCK) ON CC.StockId = S.Id
					 INNER JOIN  vwMMV             V WITH (NOLOCK)  ON V.VersionId = S.VersionId   
					 INNER JOIN TC_InquirySource SRC WITH (NOLOCK) ON SRC.Id = B.TC_InquirySourceId 
					 WHERE L.BranchId=@BranchId 
					   AND L.TC_LeadId=@TC_LeadId 
					   AND L.TC_UserId = @TC_UserId 
					   AND (B.TC_LeadDispositionID IS NULL OR B.TC_LeadDispositionId = 4)
				
	DROP TABLE #TC_InquiriesLead	

END			

