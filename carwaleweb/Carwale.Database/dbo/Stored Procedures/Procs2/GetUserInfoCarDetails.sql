IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUserInfoCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUserInfoCarDetails]
GO

	-- =============================================
-- Author:		Vinayak Mishra
-- Create date: 12/09/2014
-- Description:	Get user and car information
-- Modified By Ashish V on 02-01-2015 for replce dealerid in where condition
-- =============================================
CREATE PROCEDURE [dbo].[GetUserInfoCarDetails]
	-- Add the parameters for the stored procedure here
	@ResponseId INT,
	@VersionId INT,
	@DealerId INT,
	@OfferId INT, --added offerId input paremeter by Vinayak on 12/03/2014
	
	@Name varchar(100) OUTPUT,
	@Email varchar(100) OUTPUT,
	@Mobile varchar(100) OUTPUT,
	@CarName varchar(112) OUTPUT,
	--@TcDealerId INT OUTPUT, -- ADDED BY ASHISH
	@DealerName VARCHAR(30) OUTPUT,
	@DealerMobile VARCHAR(50) OUTPUT,
	@DealerEmail VARCHAR(50) OUTPUT,
	@DealerAddress VARCHAR(1000) OUTPUT,
	@DealerPhone VARCHAR(15) OUTPUT,
	@DealerArea VARCHAR(100) OUTPUT,
	@OfferTitle VARCHAR(100) OUTPUT,
	@OfferDesc VARCHAR(200) OUTPUT
AS
BEGIN
	SELECT @Name=Name,@Email=Email,@Mobile=Mobile FROM PQDealerAdLeads WHERE Id=@ResponseId
	
	SELECT @CarName=Car FROM vwMMV M WHERE VersionId = @VersionId
	
	SELECT TOP 1 @DealerName = DNC.Name,
	@DealerEmail = DO.CouponEmailIds,
	@DealerMobile = DO.CouponMobile,
	@DealerAddress = DNC.Address,
	@DealerPhone = ISNULL( DNC.PrimaryMobileNo,DNC.LandLineNo) ,
	@DealerArea = DNC.DealerArea,
	@OfferTitle = DO.OfferTitle,
	@OfferDesc = DO.OfferDescription
	FROM DealerOffersDealers DOD WITH (NOLOCK)
	INNER JOIN Dealer_NewCar DNC WITH (NOLOCK) ON DOD.DealerId = DNC.Id
	INNER JOIN DealerOffers DO WITH (NOLOCK) ON DO.ID = DOD.OfferId
	WHERE DOD.DealerId = @DealerId AND DO.ID = @OfferId-- Modified By Ashish V on 02-01-2015 for replce dealerid in where condition
	AND DO.IsActive = 1
END






/****** Object:  StoredProcedure [dbo].[GetDealerSponsorshipDeatils]    Script Date: 2/16/2015 5:15:00 PM ******/
-- SET ANSI_NULLS ON
