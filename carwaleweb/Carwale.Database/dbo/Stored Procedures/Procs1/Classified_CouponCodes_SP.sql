IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_CouponCodes_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_CouponCodes_SP]
GO

	CREATE Procedure Classified_CouponCodes_SP  

@CouponCode VarChar(50),  
@DiscountAmt Numeric Output  

As  

Begin  
	
	SET @DiscountAmt = 0
	
	-- Get discounted amout against provided coupen code
	select @DiscountAmt = Pof.DiscountAmount FROM PromotionalOffers Pof
	WHERE OfferCode = @CouponCode AND ConsumerType=2 AND IsBlocked = 0 AND IsOfferUsed = 0 AND CONVERT(DATE,Pof.OfferValidity) >= Convert(date,Getdate())
		
	-- Update number of number of times a code tried by customer
	update PromotionalOffers set NoOfTrials = IsNull(NoOfTrials, 0) + 1
	WHERE OfferCode = @CouponCode AND ConsumerType=2 AND IsBlocked = 0 AND IsOfferUsed = 0 AND CONVERT(DATE, OfferValidity) >= Getdate()

End







