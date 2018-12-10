IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerData]
GO

	-- =============================================
-- Created by:		Vicky Lund
-- Creation date:	30-October-2015
-- Description:		
-- Updated by:       Sourav Roy on 16-12-2015
-- Modified: Vicky Lund, 11/07/2016, Added column for Landline STD Code
-- Modified By : Mihir A Chheda [21-09-2016] to add bike wale dealers new paramenter ApplicationId added 
--               while insert / update 
-- Modified By : Chetan A Thambad on 22-09-2016 fetching more data from DealerExclusion table
-- =============================================
CREATE PROCEDURE [dbo].[GetNewCarDealerData] @DealerId INT
AS
BEGIN
	SELECT D.EmailId
		,D.Organization
		,D.Address1
		,D.AreaId
		,D.CityId
		,D.StateId
		,D.Pincode
		,D.PhoneNo
		,D.LandlineCode
		,D.FaxNo
		,D.MobileNo
		,D.WebsiteUrl
		,D.ContactPerson
		,D.ContactEmail
		,D.LogoUrl
		,D.Lattitude
		,D.Longitude
		,CASE 
			WHEN EXISTS (
					SELECT DLC.DealerLocatorConfigurationId
					FROM DealerLocatorConfiguration DLC WITH (NOLOCK)
					WHERE DLC.DealerId = D.ID
						AND DLC.IsLocatorActive = 1
					)
				THEN CONVERT(BIT, 1)
			ELSE CONVERT(BIT, 0)
			END ShowInDealerLocator
		,D.IsDealerActive
		,D.ShowroomStartTime
		,D.ShowroomEndTime
		,(
			SELECT STUFF(-- Added By Sourav Roy
					(
						SELECT ',' + CAST(TM.MakeId AS VARCHAR(50))
						FROM TC_DealerMakes AS TM WITH (NOLOCK)
						WHERE TM.DealerId = @DealerId
						FOR XML PATH('')
						), 1, 1, '')
			) AS MakeId
	,ApplicationId --Mihir A Chheda [21-09-2016]
	,DX.ExclusionFromDate
	,DX.ExclusionReason
	FROM Dealers D WITH (NOLOCK) LEFT OUTER JOIN DealerExclusion DX WITH(NOLOCK) ON D.ID = DX.DealerId
	WHERE D.ID = @DealerId
END

