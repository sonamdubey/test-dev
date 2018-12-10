IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNearbyCityDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNearbyCityDealers]
GO

	
-- =============================================
-- Author:		<Ashwini Todkar>
-- Create date: <25/8/2014>
-- Description:	<Gets the Nearby cities dealers for a particular city>
-- =============================================
CREATE PROCEDURE [dbo].[GetNearbyCityDealers] --[dbo].[GetNearbyCityDealers] 1,'11658,11658,11658,9350,15851,7803,15729'
	@CityId INT
	,@DealerIds VARCHAR(50)
AS
BEGIN
	DECLARE @UserLatitude NUMERIC
		,@UserLongitude NUMERIC
		,@NearestCity INT = NULL
		,@ConstLt FLOAT = 0.030694236 -- // constant (km per 1 unit)
		,@ConstLg FLOAT = 0.028870889 -- // constant (km per 1 unit)

	SELECT @UserLatitude = c.Lattitude
		,@UserLongitude = c.Longitude
	FROM Cities c WITH (NOLOCK)
	WHERE id = @CityId

	DECLARE @Latt DECIMAL = ABS(@UserLatitude)
		,@Long DECIMAL = ABS(@UserLongitude);

	WITH CTE
	AS (
		SELECT TOP 1 CT.ID AS CityId
			,Sqrt(Power(((ISNULL(@Latt, 0) - CT.Lattitude) * @ConstLt), 2) + Power(((ISNULL(@Long, 0) - CT.Longitude) * @ConstLg), 2)) AS Distance
		FROM Cities CT WITH (NOLOCK)
		JOIN Dealers d WITH (NOLOCK) ON CT.ID = d.CityId
			AND D.IsDealerActive = 1
		WHERE d.id IN (
				SELECT ListMember
				FROM fnSplitCSV(@DealerIds)
				)
			AND CT.IsDeleted = 0
		ORDER BY Distance
		)
	SELECT @NearestCity = CTE.CityId
	FROM CTE

	SELECT D.ID AS DealerId
		,isnull(D.Organization, '') AS NAME
		,isnull(D.Address1, '') + ' ' + isnull(D.Address2, '') AS Address
		,D.Pincode
		,D.EMailId
		,D.WebsiteUrl AS WebSite
		,D.ShowroomStartTime
		,D.ShowroomEndTime
		,D.ContactHours AS WorkingHours
		,D.Lattitude AS Latitude
		,D.Longitude AS Longitude
		,ci.NAME AS CityName
		,s.NAME AS StateName
		,D.MobileNo
		,D.PhoneNo AS LandLineNo
		,S.ID AS StateId
		,d.FaxNo
		,d.ProfilePhotoHostUrl
		,d.ProfilePhotoUrl
	FROM Dealers D WITH (NOLOCK)
	LEFT JOIN cities ci WITH (NOLOCK) ON ci.id = D.CityId
	LEFT JOIN states s WITH (NOLOCK) ON ci.StateId = s.ID
	WHERE D.IsDealerActive = 1
		AND D.CityId = @NearestCity
		AND D.Id IN (
			SELECT ListMember
			FROM fnSplitCSV(@DealerIds)
			)
END

