IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUserInfoCarDetails_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUserInfoCarDetails_v15]
GO

	-- =============================================
-- Author:		Vinayak Mishra
-- Create date: 12/09/2014
-- Description:	Get user and car information
-- Modified By Ashish V on 02-01-2015 for replce dealerid in where condition
-- Modified By Anchal Gupta on 05-11-2015 for implementation of migration of data of dealer_newCar table
-- =============================================
CREATE PROCEDURE [dbo].[GetUserInfoCarDetails_v15.11.2]
	-- Add the parameters for the stored procedure here
	@ResponseId INT
	,@VersionId INT
	,@DealerId INT
	,@OfferId INT
	,--added offerId input paremeter by Vinayak on 12/03/2014
	@Name VARCHAR(100) OUTPUT
	,@Email VARCHAR(100) OUTPUT
	,@Mobile VARCHAR(100) OUTPUT
	,@CarName VARCHAR(112) OUTPUT
	,
	--@TcDealerId INT OUTPUT, -- ADDED BY ASHISH
	@DealerName VARCHAR(30) OUTPUT
	,@DealerMobile VARCHAR(50) OUTPUT
	,@DealerEmail VARCHAR(50) OUTPUT
	,@DealerAddress VARCHAR(1000) OUTPUT
	,@DealerPhone VARCHAR(15) OUTPUT
	,@DealerArea VARCHAR(100) OUTPUT
	,@OfferTitle VARCHAR(100) OUTPUT
	,@OfferDesc VARCHAR(200) OUTPUT
AS
BEGIN
	SELECT @Name = NAME
		,@Email = Email
		,@Mobile = Mobile
	FROM PQDealerAdLeads WITH (NOLOCK)
	WHERE Id = @ResponseId

	SELECT @CarName = Car
	FROM vwMMV M WITH (NOLOCK)
	WHERE VersionId = @VersionId

	SELECT TOP 1 @DealerName = D.Organization
		,@DealerEmail = DO.CouponEmailIds
		,@DealerMobile = DO.CouponMobile
		,@DealerAddress = D.Address1
		,@DealerPhone = ISNULL(D.MobileNo, D.PhoneNo)
		,@DealerArea = ISNULL(A.NAME, '')
		,@OfferTitle = DO.OfferTitle
		,@OfferDesc = DO.OfferDescription
	FROM DealerOffersDealers DOD WITH (NOLOCK)
	INNER JOIN Dealers D WITH (NOLOCK) ON DOD.DealerId = D.ID
	INNER JOIN DealerOffers DO WITH (NOLOCK) ON DO.ID = DOD.OfferId
	LEFT OUTER JOIN Areas A WITH (NOLOCK) ON D.AreaId = A.ID
		AND A.IsDeleted = 0

	WHERE DOD.DealerId = @DealerId
		AND DO.ID = @OfferId -- Modified By Ashish V on 02-01-2015 for replce dealerid in where condition
		AND DO.IsActive = 1
		AND D.TC_DealerTypeId = 2
END

