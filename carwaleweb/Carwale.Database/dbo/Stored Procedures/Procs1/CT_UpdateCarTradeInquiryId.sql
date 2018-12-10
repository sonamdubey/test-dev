IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CT_UpdateCarTradeInquiryId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CT_UpdateCarTradeInquiryId]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 26 Apr 2016
-- Description:	Update buyer/seller inquiries with CarTrade Inquiry Id (Type - 1:Buyer 2:Seller)
-- =============================================
CREATE PROCEDURE [dbo].[CT_UpdateCarTradeInquiryId]
	-- Add the parameters for the stored procedure here
	@TypeId			INT,
	@CWInquiryId	BIGINT,
	@CTInquiryId	BIGINT,
	@ApiResponse	VARCHAR(2000) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	IF @TypeId = 1--for buyer inquiries
	BEGIN
		UPDATE TC_BuyerInquiries
		SET 
			cteinquiryid = @CTInquiryId,
			cteapiresponse = @ApiResponse,
			Lastupdateddate = GETDATE()
		WHERE TC_BuyerInquiriesId = @CWInquiryId
	END
	ELSE IF @TypeId = 2--for seller inquiries
	BEGIN
		UPDATE TC_SellerInquiries
		SET 
			cteinquiryid = @CTInquiryId,
			cteapiresponse = @ApiResponse,
			Lastupdateddate = GETDATE()
		WHERE TC_SellerInquiriesId = @CWInquiryId
	END
	
END
