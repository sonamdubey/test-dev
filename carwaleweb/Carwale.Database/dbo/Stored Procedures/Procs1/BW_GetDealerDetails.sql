IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerDetails]
GO

	-- =============================================
-- Author:		ASHWINI TODKAR
-- Create date: 27th OCT 2014
-- Description:	PROC TO GET DEALER DETAILS AND ALSO OFFERS
-- EXEC BW_GetDealerDetails 11,61,321
-- Modified By : Sadhana Upadhyay on 12 Nov 2014
-- Summary : to filter expire offers
-- Modified By : Ashwini Todkar on 20 Nov 2014 
-- Modified By : Sadhana Upadhyay on 3rd Dec 2014
-- Summary : Retrieved Loan provider
-- Modified By : Ashwini Todkar on 15 dec 2014
-- Summary : Added select clause to get bike booking amount
-- Modified By : Ashish G. Kamble on 24 June 2015
-- Modified : BW_GetDealerPriceQuote sp modified to get the offers data. Query to get the offers removed from this sp.
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealerDetails]
	-- Add the parameters for the stored procedure here
	@DealerId NUMERIC(18, 0)
	,@VersionId NUMERIC(18, 0)
	,@CityId NUMERIC(18, 0)
AS
BEGIN	
	SET NOCOUNT ON;

	EXEC BW_GetDealerPriceQuote @CityId
		,@VersionId
		,@DealerId
	
	--PRINT @ModelId
	-- SELECT CLAUSE TO GET DEALER INFO
	SELECT D.FirstName
		,D.LastName
		,D.Address1 AS Address
		,D.PhoneNo
		,D.MobileNo
		,ISNULL(D.WeAreOpen, 0) WeAreOpen
		,D.ContactHours
		,A.NAME AS AreaName
		,C.NAME AS CityName
		,S.NAME AS StateName
		,ISNULL(D.Lattitude, 0) Lattitude
		,ISNULL(D.Longitude, 0) Longitude
		,D.AreaId
		,D.Pincode
		,D.EmailId
		,D.WebsiteUrl
		,D.Organization
	FROM Dealers AS D WITH (NOLOCK)
	INNER JOIN Areas AS A WITH (NOLOCK) ON A.ID = D.AreaId
	INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = D.CityId
	INNER JOIN States AS S WITH (NOLOCK) ON S.ID = C.StateId
	--WHERE D.CityId = @CityId --commented by Ashwini Todkar on 20 Nov 2014 
	WHERE D.ID = @DealerId 
		AND D.IsDealerDeleted = 0
	
	--Get Facilities offered by dealer
	SELECT DF.Facility
	FROM BW_DealerFacilities DF WITH (NOLOCK)
	WHERE DF.DealerId = @DealerId
		AND DF.IsActive = 1

	--Get EMI details offered by dealer
	SELECT LA.Id
		,ISNULL(LA.LTV, 0) LTV
		,ISNULL(LA.Tenure, 0) Tenure
		,ISNULL(LA.RateOfInterest, 0) RateOfInterest
		,LA.LoanProvider
	FROM BW_DealerLoanAmounts LA WITH (NOLOCK)
	WHERE LA.DealerId = @DealerId

	--Get Booking Amount of a bike added by Ashwini
	SELECT ISNULL(BA.Amount,0) Amount FROM BW_DealerBikeBookingAmounts BA WITH(NOLOCK)
	INNER JOIN BikeVersions BV WITH(NOLOCK)
	ON BV.ID = BA.VersionId
	WHERE BA.VersionId = @VersionId 
	AND BA.DealerId = @DealerId
	AND BA.IsActive = 1
	
END

