IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SellerInquiryDetailsForAPI]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SellerInquiryDetailsForAPI]
GO

	-- =======================================================================
-- Created By: Nilesh Utture
-- Created On: 30th April, 2013
-- Description:This SP will give buyer Inquiry on stock details for API 
-- EXEC TC_SellerInquiryDetailsForAPI 6937,5,1
-- Modified By: Nilesh Utture on 1st September, 2013 Added COLUMN NS_ShowClose
-- Modified By :Khushaboo Patil on 17th March , Added columns InquiriesLeadId,LeadInquiryTypeId
-- =======================================================================
CREATE PROCEDURE [dbo].[TC_SellerInquiryDetailsForAPI] @TC_LeadId INT,@BranchId INT,@TC_UserId INT
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
	  
	    SELECT S.TC_SellerInquiriesId as Id,
		       (CASE L.TC_LeadInquiryTypeId WHEN 2 THEN 'Seller' END) AS Type, 
		       V.Car, 
		       S.CreatedOn,
		       SRC.Source AS Source, 
		       ISNULL(CONVERT(VARCHAR,CWInquiryId),'') AS ProfileId,
		       REPLACE(RIGHT(CONVERT(VARCHAR(9), S.MakeYear, 6), 6), ' ', '-') AS Year, 
		       S.Colour AS Color, 
		       S.Price AS Price, 
			   S.Kms AS Distance, 
			   S.RegNo + ' (' + S.RegistrationPlace + ')' AS Registration, 
			(CASE Owners 
			WHEN 1 THEN 'First' 
			WHEN 2 THEN 'Second' 
			WHEN 3 THEN 'Third' 
			WHEN 4 THEN 'Fourth' 
			WHEN 5 THEN 'More than four' END) AS Owners,
			(CASE PurchasedStatus 
			WHEN 33 THEN 'false'
			ELSE 'true' END) AS NS_ShowClose,
			L.TC_InquiriesLeadId AS InquiriesLeadId,L.TC_LeadInquiryTypeId AS LeadInquiryTypeId
		                   FROM            #TC_InquiriesLead  L  
		                   INNER JOIN      TC_SellerInquiries S WITH (NOLOCK) ON L.TC_InquiriesLeadId=S.TC_InquiriesLeadId   
						   INNER JOIN      vwMMV              V                ON S.CarVersionId = V.VersionId  
						   INNER JOIN TC_InquirySource SRC WITH (NOLOCK) ON SRC.Id = S.TC_InquirySourceId
		                WHERE  L.BranchId=@BranchId 
		                   AND L.TC_LeadId=@TC_LeadId 
		                   AND L.TC_UserId = @TC_UserId  
		                   AND (S.TC_LeadDispositionID IS NULL OR S.TC_LeadDispositionID = 4)
			 
	DROP TABLE #TC_InquiriesLead	

END			


