IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ValidateOfferCouponCode]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ValidateOfferCouponCode]
GO

	-- =============================================  
-- Author:  Tejashree Patil on 25 Nov 2014.
-- Description: To validate Offer Coupon Code.
-- =============================================  
CREATE PROCEDURE [dbo].[TC_ValidateOfferCouponCode] 
	 @BranchId BIGINT,
	 @CouponCode VARCHAR(25) = NULL,
	 @CwOfferId INT
AS    
BEGIN
	IF NOT EXISTS (SELECT TC_BookingOffersLogId FROM TC_BookingOffersLog WITH(NOLOCK) WHERE CouponCode=@CouponCode)
	BEGIN
		SELECT	DOD.OfferId, CouponCode , DOD.DealerId
		FROM	OfferCouponCodes CC WITH(NOLOCK)
				INNER JOIN DealerOffersDealers DOD WITH(NOLOCK) ON DOD.OfferId = CC.OfferId
				INNER JOIN DealerOffers DO WITH(NOLOCK) ON DO.Id = CC.OfferId
				INNER JOIN Dealer_NewCar DNC WITH(NOLOCK) ON DNC.Id = DOD.DealerId
		WHERE	(DOD.DealerId = -1 OR DNC.TcDealerId=@BranchId ) 
				AND CC.CouponCode = @CouponCode 
				AND ((DO.OfferUnits - DO.ClaimedUnits) > 0)
				AND CC.OfferId=@CwOfferId
	END
END

