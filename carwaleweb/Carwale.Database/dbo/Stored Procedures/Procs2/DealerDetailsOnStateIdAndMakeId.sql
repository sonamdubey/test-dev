IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerDetailsOnStateIdAndMakeId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerDetailsOnStateIdAndMakeId]
GO

	

 
-- =============================================          
-- Author:  <Vinayak>          
-- Description: <Get the dealer detail based on StateId and MakeId>
-- =============================================          
CREATE PROCEDURE [dbo].[DealerDetailsOnStateIdAndMakeId] (
	@StateId INT
	,@MakeId INT
	)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		D.ID AS DealerId
		,ci.ID as CityId 
		,isnull(D.Organization,'') as Name
		,isnull(D.Address1,'') + ' ' + isnull(D.Address2, '') as Address
		,D.Pincode
		,D.EMailId
		,D.WebsiteUrl as WebSite
		,D.ShowroomStartTime
		,D.ShowroomEndTime
		,D.ContactHours as WorkingHours
		,D.Lattitude as Latitude
		,D.Longitude as Longitude
		,ci.NAME as CityName
		,s.NAME as StateName
		,D.MobileNo
		,D.PhoneNo as LandLineNo 
		,S.ID as StateId
		,d.FaxNo
	FROM Dealers D WITH (NOLOCK)
	INNER JOIN TC_DealerMakes TC WITH (NOLOCK) ON TC.DealerId=D.ID
	INNER JOIN DealerLocatorConfiguration AS DNC WITH (NOLOCK) ON DNC.DealerId=D.ID
	LEFT JOIN cities ci WITH (NOLOCK) ON ci.id = D.CityId
	LEFT JOIN states s WITH (NOLOCK) ON ci.StateId = s.ID
	WHERE 
		D.IsDealerActive = 1 AND s.Id=@StateId
		AND  TC.MakeId=@MakeId
		AND DNC.IsLocatorActive = 1
END

