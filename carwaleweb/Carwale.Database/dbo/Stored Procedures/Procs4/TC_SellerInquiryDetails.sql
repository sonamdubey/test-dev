IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SellerInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SellerInquiryDetails]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 8 Oct 2012
-- Description:	Get details of Seller Inquiry
-- EXEC TC_SellerInquiryDetails 5, 77
-- Modified By : Tejashree Patil on 18 Jan 2013,Fetched sourceId from SellerInquiry table, and join with TC_InquiriesLead table 
-- =============================================
CREATE  Procedure [dbo].[TC_SellerInquiryDetails]
	@BranchId BIGINT,
	@TC_SellCarInquiryId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		SELECT SI.*, CD.CustomerName, CD.Email, CD.Location, CD.Mobile, CD.CW_CustomerId,
			   SI.TC_InquirySourceId,-- Modified By : Tejashree Patil on 18 Jan 2013,
			   Ma.ID MakeId, Mo.ID ModelId, Ve.ID VersionId
		FROM   TC_SellerInquiries SI WITH (NOLOCK)
			   --LEFT JOIN TC_Inquiries INQ WITH(NOLOCK)
					 -- ON INQ.TC_InquiriesId = SI.TC_InquiriesId AND INQ.BranchId=@BranchId
			   LEFT JOIN TC_InquiriesLead INQ WITH(NOLOCK)
					  ON INQ.TC_InquiriesLeadId = SI.TC_InquiriesLeadId AND INQ.BranchId=@BranchId	-- Modified By : Tejashree Patil on 18 Jan 2013,				 
			   LEFT JOIN CarVersions Ve WITH(NOLOCK)
					  ON Ve.ID = SI.CarVersionId -- Modified By : Tejashree Patil on 18 Jan 2013,
			   LEFT JOIN CarModels Mo WITH(NOLOCK)
					  ON Mo.ID = Ve.CarModelId
			   LEFT JOIN CarMakes Ma WITH(NOLOCK)
					  ON Ma.id = Mo.CarMakeId
			   LEFT JOIN TC_CustomerDetails CD WITH(NOLOCK)
					  ON CD.Id = INQ.TC_CustomerId AND CD.BranchId=@BranchId 
		WHERE  SI.TC_SellerInquiriesId= @TC_SellCarInquiryId
			   AND @BranchId=INQ.BranchId		
	
END
