IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerDetailsOnDealerId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerDetailsOnDealerId]
GO

	
 
-- =============================================          
-- Author:  <Vinayak>          
-- Description: <Get the dealer detail based on dealerId>
-- =============================================          
CREATE  PROCEDURE [dbo].[DealerDetailsOnDealerId] (
	@DealerId INT
	--OUTPUT--
	,@OutDealerId INT = NULL OUTPUT 
	,@CityId INT OUTPUT
	,@Name VARCHAR(200) OUTPUT
	,@Address VARCHAR(400) OUTPUT
	,@Pincode VARCHAR(20) OUTPUT
	,@EMailId VARCHAR(100) OUTPUT
	,@WebSite VARCHAR(100) OUTPUT
	,@ShowroomStartTime VARCHAR(50) OUTPUT
	,@ShowroomEndTime VARCHAR(50) OUTPUT
	,@WorkingHours VARCHAR(50) OUTPUT
	,@Latitude FLOAT OUTPUT
	,@Longitude FLOAT OUTPUT
	,@CityName VARCHAR(50) OUTPUT
	,@StateName VARCHAR(30) OUTPUT
	,@MobileNo VARCHAR(50) OUTPUT
	,@LandLineNo VARCHAR(50) OUTPUT
	,@StateId INT OUTPUT
	,@FaxNo VARCHAR(50) OUTPUT
	)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT  @OutDealerId = D.ID
		 ,@CityId = D.CityId
		,@Name = isnull(D.Organization,'')
		,@Address = isnull(D.Address1,'') + ' ' + isnull(D.Address2, '')
		,@Pincode = D.Pincode
		,@EMailId = D.EMailId
		,@WebSite = D.WebsiteUrl
		,@ShowroomStartTime = D.ShowroomStartTime
		,@ShowroomEndTime = D.ShowroomEndTime
		,@WorkingHours = D.ContactHours
		,@Latitude = D.Lattitude
		,@Longitude = D.Longitude
		,@CityName = ci.NAME
		,@StateName = s.NAME
		,@MobileNo = D.MobileNo
		,@LandLineNo = D.PhoneNo
		,@StateId =S.ID
		,@FaxNo =d.FaxNo
	FROM Dealers D WITH (NOLOCK)
	LEFT JOIN cities ci WITH (NOLOCK) ON ci.id = D.CityId
	LEFT JOIN states s WITH (NOLOCK) ON ci.StateId = s.ID
	WHERE 
		D.IsDealerActive = 1 AND D.id=@DealerId
		--AND  D.TC_DealerTypeId = 2
END


