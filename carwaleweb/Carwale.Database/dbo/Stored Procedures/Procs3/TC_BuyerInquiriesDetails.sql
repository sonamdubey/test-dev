IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BuyerInquiriesDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BuyerInquiriesDetails]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 12 Feb 2013 at 3 pm
-- Description:	Details of buyer inquiries from stock id
-- TC_BuyerInquiriesDetails 3896,5,1
-- Modified By : Tejashree Patil on 20 Feb 2013,Fetched Inquiry status details.
-- Modified By: Nilesh Utture on 8th May, 2013 Removed Source Id From SELECT clause
-- =============================================
CREATE PROCEDURE [dbo].[TC_BuyerInquiriesDetails]
(
@StockId INT,
@BranchId INT,
@InqSourceId SMALLINT
)
AS
BEGIN
	SELECT	C.Id , C.CustomerName, C.Email, C.Mobile, B.CreatedOn AS EntryDate, ISNULL(LD.Name, '--') AS InquiryStatus, ISNULL(U.UserName, '--') AS UserName
	FROM	TC_CustomerDetails C WITH(NOLOCK)  
			INNER JOIN  TC_Lead L WITH(NOLOCK)ON L.TC_CustomerId= C.Id 
			INNER JOIN  TC_InquiriesLead IL WITH(NOLOCK)ON IL.TC_LeadId= L.TC_LeadId
			INNER JOIN  TC_BuyerInquiries B WITH(NOLOCK)ON B.TC_InquiriesLeadId=IL.TC_InquiriesLeadId 
			INNER JOIN  TC_InquirySource S WITH(NOLOCK)ON S.Id=B.TC_InquirySourceId AND B.TC_InquirySourceId = @InqSourceId
			LEFT JOIN  TC_LeadDisposition LD WITH(NOLOCK)ON LD.TC_LeadDispositionId=ISNULL(B.BookingStatus,B.TC_LeadDispositionId) 
			LEFT JOIN TC_Users U WITH(NOLOCK) ON U.Id = IL.TC_UserId
	WHERE	L.BranchId	= @BranchId 
			AND StockId = @StockId
	ORDER BY EntryDate DESC
END







/****** Object:  StoredProcedure [dbo].[TC_Login_SP]    Script Date: 02/20/2013 15:53:37 ******/
SET ANSI_NULLS ON
