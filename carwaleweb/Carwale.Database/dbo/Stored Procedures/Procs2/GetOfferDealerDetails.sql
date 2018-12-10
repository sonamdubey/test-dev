IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOfferDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOfferDealerDetails]
GO

	-- =============================================
-- Author:		Ashish Verma
-- Create date: 19/11/2014
-- Description:	Get Carwale Offer Dealer Details

-- input paramrter: DealerId and OfferType
-- output parameters:,DealerName,DealerMobile,DealerEmail,DealerAddress,DealerPhone
-- =============================================
CREATE PROCEDURE [dbo].[GetOfferDealerDetails] 
	-- Add the parameters for the stored procedure here
	@DealerId INT
	-- Output Parameters
	,@TcDealerId INT OUTPUT -- ADDED BY ASHISH
	,@DealerName VARCHAR(30) OUTPUT
	,@DealerMobile VARCHAR(50) OUTPUT
	,@DealerEmail VARCHAR(50) OUTPUT
	,@DealerAddress VARCHAR(1000) OUTPUT
	,@DealerPhone VARCHAR(15) OUTPUT
	,@DealerArea VARCHAR(100) OUTPUT
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT TOP 1 @DealerName = DNC.Name,
	@DealerEmail = DO.CouponEmailIds,
	@DealerMobile = DO.CouponMobile,
	@DealerAddress = DNC.Address,
	@DealerPhone = DNC.ContactNo,
	@DealerArea = DNC.DealerArea,
	@TcDealerId = DNC.TcDealerId
	FROM DealerOffersDealers DOD WITH (NOLOCK)
	INNER JOIN Dealer_NewCar DNC WITH (NOLOCK) ON DOD.DealerId = DNC.Id
	INNER JOIN DealerOffers DO WITH (NOLOCK) ON DO.ID = DOD.OfferId
	WHERE DOD.DealerId = @DealerId
	AND DO.IsActive = 1

END
