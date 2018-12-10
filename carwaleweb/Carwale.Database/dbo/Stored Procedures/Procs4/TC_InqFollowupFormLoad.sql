IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InqFollowupFormLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InqFollowupFormLoad]
GO

	
--Modified by Binu,Date:25-jul-2012,Add versionId and source id colums to select statement 
--Author: Binumon George
--Date :29-03-2012
--Description: Get all type of inquiry leads in followup page
CREATE PROCEDURE [dbo].[TC_InqFollowupFormLoad]
(
@CustomerId INT,
@BranchId NUMERIC,
@LeadId BIGINT
)
AS
BEGIN

-- table for customer details
SELECT CustomerName,Email,Mobile FROM TC_CustomerDetails WITH(NOLOCK)
WHERE Id=@CustomerId

-- Getting Inquiries with Stock
SELECT DISTINCT INQT.InquiryType 'InquiryType',
INQ.CreatedDate AS InquiryDate,
INQ.CarName,
INQ.TC_InquiriesId, INQ.InquiryType AS InqTypeId, INQ.VersionId AS VersionId, INQ.SourceId AS SourceId
--SRC.Source AS InquirySource,

FROM TC_Inquiries INQ 
INNER JOIN TC_InquiriesLead L WITH(NOLOCK)ON L.TC_CustomerId=INQ.TC_CustomerId and INQ.TC_LeadTypeId=L.TC_LeadTypeId
--INNER JOIN TC_InquirySource SRC ON INQ.SourceId=SRC.Id
INNER JOIN TC_InquiryType INQT WITH(NOLOCK)ON INQ.InquiryType=INQT.TC_InquiryTypeId
WHERE L.TC_InquiriesLeadId=@LeadId and L.BranchId=@BranchId AND INQ.TC_LeadTypeId=L.TC_LeadTypeId
ORDER BY INQ.CreatedDate DESC
END


