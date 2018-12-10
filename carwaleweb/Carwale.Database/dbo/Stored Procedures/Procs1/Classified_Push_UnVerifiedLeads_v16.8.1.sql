IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_Push_UnVerifiedLeads_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_Push_UnVerifiedLeads_v16]
GO

	
-- =============================================
-- Author:		Prachi Phalak
-- Create date: 10th sep, 2015
-- Description:	To get all the unverified leads from ClassifiedLeads table.
-- modified by Purohith Guguloth on 14th sep, 2015  Added condition of sellerType 
-- Modified by Purohith Guguloth on 21/09/2015 ,Removed condition of IsVerified.
-- Modified by Shubham Agarwal on 08/08/2016: added CT_AddonPackages to check package of dealers, Changed column IsSentToAutoBiz to IsSentToSource and Fetching SourceId, DealerId, TC_StockId
-- Modified by Afrose on 11-10-2016, Added check for Isactive=1 on CT_AddonPackages 
-- =============================================
CREATE PROCEDURE [dbo].[Classified_Push_UnVerifiedLeads_v16.8.1]
	-- No parameters
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT CFL.CustEmail, CFL.CustMobile, CFL.InquiryId, CFL.CustName, CFL.IsSentToSource, SI.SourceId, CFL.IsVerified, CFL.Id, SI.DealerId, SI.TC_StockId
	FROM  ClassifiedLeads CFL WITH (NOLOCK)
	INNER JOIN SellInquiries SI WITH (NOLOCK) ON CFL.InquiryId = SI.ID and CFL.sellerType = '1'  --sellerType is varchar 
	INNER JOIN CT_AddOnPackages CAP WITH (NOLOCK) ON SI.DealerId = CAP.CWDealerId 
	WHERE CAP.AddOnPackageId = 101 AND CFL.IsSentToSource = 0 AND DATEADD(minute, -15, GETDATE()) > EntryDateTime 
	AND CAP.IsActive=1          --Modified By Afrose

END


