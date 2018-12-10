IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_Push_UnVerifiedLeads_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_Push_UnVerifiedLeads_15]
GO

	

-- =============================================
-- Author:		Prachi Phalak
-- Create date: 10th sep, 2015
-- Description:	To get all the unverified leads from ClassifiedLeads table.
-- modified by Purohith Guguloth on 14th sep, 2015  Added condition of sellerType 
-- Modified by Purohith Guguloth on 21/09/2015 ,Removed condition of IsVerified.
-- =============================================
CREATE PROCEDURE [dbo].[Classified_Push_UnVerifiedLeads_15.9.4]
	-- No parameters
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT CFL.CustEmail,CFL.CustMobile,CFL.InquiryId, CFL.CustName,CFL.IsSentToAutoBiz, CFL.IsVerified, CFL.Id
	FROM  ClassifiedLeads CFL WITH (NOLOCK)
	INNER JOIN SellInquiries SI WITH (NOLOCK) ON CFL.InquiryId = SI.ID and CFL.sellerType = '1'  --sellerType is varchar 
	INNER JOIN TC_MappingDealerFeatures MDF WITH (NOLOCK) ON SI.DealerId = MDF.BranchId 
	WHERE MDF.TC_DealerFeatureId = 4 AND CFL.IsSentToAutoBiz= 0 AND DATEADD(minute, -15, GETDATE()) > EntryDateTime 

END



