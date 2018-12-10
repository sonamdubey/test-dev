IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOfferDealerDetails_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOfferDealerDetails_V14]
GO

	-- =============================================
-- Author:		Ashish Verma
-- Create date: 19/11/2014
-- Description:	Get Carwale Offer Dealer Details

-- input paramrter: DealerId and OfferType
-- output parameters:,DealerName,DealerMobile,DealerEmail,DealerAddress,DealerPhone
--added offerId input paremeter by Ashish verma on 12/03/2014
---Added isnull check for all columns in select
-- =============================================
CREATE PROCEDURE [dbo].[GetOfferDealerDetails_V14.11.3.1] 
	-- Add the parameters for the stored procedure here
	@DealerId INT,
	@OfferId INT --added offerId input paremeter by Ashish verma on 12/03/2014
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

	SELECT TOP 1 @DealerName = ISNULL(DNC.Name,''),
	@DealerEmail = ISNULL(DO.CouponEmailIds,''),---Added isnull check for all columns in select
	@DealerMobile = ISNULL(DO.CouponMobile,''),
	@DealerAddress = ISNULL(DNC.Address,''),
	@DealerPhone = ISNULL( DNC.PrimaryMobileNo,DNC.LandLineNo) ,
	@DealerArea = ISNULL(DNC.DealerArea,''),
	@TcDealerId = DNC.TcDealerId
	FROM DealerOffersDealers DOD WITH (NOLOCK)
	INNER JOIN Dealer_NewCar DNC WITH (NOLOCK) ON DOD.DealerId = DNC.Id
	INNER JOIN DealerOffers DO WITH (NOLOCK) ON DO.ID = DOD.OfferId
	WHERE DOD.DealerId = @DealerId AND DO.ID = @OfferId
	AND DO.IsActive = 1

END
