IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPaymentInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPaymentInquiryDetails]
GO

	-- =============================================
-- Author:		<Author,,Ashish Verma>
-- Create date: <Create Date,12/05/2014,>
-- Description:	<Description, for Geeting PaymentInquiryDetails,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPaymentInquiryDetails]
	-- Add the parameters for the stored procedure here
	@ResponseId Int,
	@CouponId Int,
	--Output parameters
	@DealerId Int Output,
	@OfferId Int Output,
	@CouponCode Varchar(20) Output,
	@InquiryId Int Output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @InquiryId = DAL.PushStatus,@DealerId=DAL.DealerId from PQDealerAdLeads DAL with(nolock)
	where Id = @ResponseId
	SELECT @CouponCode = OCC.CouponCode,@OfferId=OCC.OfferId from OfferCouponCodes OCC with(nolock) 
	where CouponId = @CouponId
END





/****** Object:  StoredProcedure [dbo].[GetOfferPGDetailsForAndroid]    Script Date: 12/11/2014 6:04:04 PM ******/
SET ANSI_NULLS ON
