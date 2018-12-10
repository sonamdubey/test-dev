IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOfferPGDetailsByCouponCode]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOfferPGDetailsByCouponCode]
GO

	-- =============================================
-- Author:		Supriya Khartode
-- Create date: 1/12/2014
-- Description:	Fetch all the details needed for payment gateway transaction according to coupon code passed.
--			  : This will be used in case when user comes from android to mobile page for online payment. 
-- =============================================
CREATE PROCEDURE [dbo].[GetOfferPGDetailsByCouponCode]
	@ResponseId INT
	,@CityId INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT DL.Name,DL.Email,DL.Mobile,DL.PQId	
	FROM  PQDealerAdLeads DL with (nolock) WHERE Id= @ResponseId
	SELECT Name FROM Cities WHERE id=@CityId
	
END




/****** Object:  StoredProcedure [dbo].[GetPaymentInquiryDetails]    Script Date: 12/11/2014 6:02:36 PM ******/
SET ANSI_NULLS ON
