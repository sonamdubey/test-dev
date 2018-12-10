IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_GetDealerDetailsInformation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_GetDealerDetailsInformation]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 14/06/2012
-- Description:	This SP returns all details of the dealer.
-- Modified By : Raghu to get Latitude and Longitude for a Dealer on <19-7-2013>
-- =============================================
CREATE PROCEDURE [dbo].[Classified_GetDealerDetailsInformation] --EXEC [Classified_GetDealerDetailsInformation] 9291
	-- Add the parameters for the stored procedure here
	@DealerId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		SELECT TD.LoginId AS LoginId, Passwd, TD.FirstName, TD.LastName, 
		TD.EmailId AS EmailId, TD.Organization AS Organization, TD.Address1 AS Address1, 
		TD.Address2 AS Address2, A.Id AS AreaId, C.Id AS CityId, S.Id AS StateId, 
		A.Name AS Area, C.Name AS City, S.Name AS State, TD.Pincode AS Pincode, Status, 
		TD.PhoneNo AS PhoneNo, TD.FaxNo AS FaxNo, TD.MobileNo AS MobileNo, TD.JoiningDate AS JoiningDate,
		TD.Longitude AS CityLongitude,TD.Lattitude as CityLattitude,
		TD.ExpiryDate AS ExpiryDate, TD.WebsiteUrl AS WebsiteUrl, TD.ContactPerson AS ContactPerson, 
		TD.ContactHours AS ContactHours, TD.ContactEmail AS ContactEmail, TD.LogoUrl AS LogoUrl, TD.BPMobileNo AS BPMobileNo 
		FROM Dealers AS TD
		INNER JOIN States AS S ON S.ID = TD.StateId
		INNER JOIN Cities AS c ON C.ID = TD.CityId 
		LEFT JOIN Areas AS A ON A.ID = TD.AreaId 		
		WHERE TD.ID = @DealerId
END



/****** Object:  StoredProcedure [dbo].[GetTopSellingCars]    Script Date: 07/25/2013 08:42:57 ******/
SET ANSI_NULLS ON
