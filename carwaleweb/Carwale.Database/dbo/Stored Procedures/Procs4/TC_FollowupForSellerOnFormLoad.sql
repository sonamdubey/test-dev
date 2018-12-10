IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FollowupForSellerOnFormLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FollowupForSellerOnFormLoad]
GO

	-- Created By:	Surendra
-- Create date: 25 Jan 2012
-- Description:	This Procedure will be used on the form load of Followup for Selller
-- =============================================
CREATE PROCEDURE [dbo].[TC_FollowupForSellerOnFormLoad]
(
@CustomerId INT,
@BranchId NUMERIC
)
as
-- table for customer details
SELECT CustomerName,Email,Mobile FROM TC_CustomerDetails WITH(NOLOCK)
WHERE Id=@CustomerId		
		
-- table for Inquiries with stock
SELECT (VW.Make + ' ' + VW.Model + ' ' + VW.Version) as 'CarName',INQ.CreatedDate AS 'InquiryDate' 
,SR.Source ,TSI.MakeYear,TSI.RegistrationPlace,TSI.AdditionalFuel,TSI.Colour,TSI.Kms,
	TSI.Accidental,TSI.CarDriven,TSI.CityMileage,TSI.Comments,TSI.Price,TSI.FloodAffected,TSI.Insurance,
	TSI.InsuranceExpiry,TSI.InteriorColor,TSI.Owners,TSI.CWInquiryId,TSI.RegNo,TSI.Tax,TSI.CWInquiryId AS 'ProfileId'
	FROM TC_Inquiries INQ WITH(NOLOCK)
	INNER JOIN TC_SellerInquiries TSI WITH(NOLOCK) ON INQ.TC_InquiriesId=TSI.TC_InquiriesId
	--FROM TC_Inquiries INQ INNER JOIN TC_SellerInquiries TSI ON INQ.CustomerId=TSI.TC_CustomerId
	INNER JOIN vwMMV VW ON INQ.VersionId=VW.VersionId		
	INNER JOIN TC_InquirySource SR WITH(NOLOCK) ON INQ.SourceId=SR.Id
	WHERE INQ.TC_CustomerId=@CustomerId AND INQ.InquiryType=2
	