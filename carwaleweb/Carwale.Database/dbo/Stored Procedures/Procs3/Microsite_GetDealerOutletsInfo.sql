IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDealerOutletsInfo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDealerOutletsInfo]
GO

	
CREATE PROCEDURE [dbo].[Microsite_GetDealerOutletsInfo]
@AdminDealerId NUmeric(18,0)
/*
Author:Rakesh Yadav
Date Created: 10 April 2015
Desc: fetch all dealers/Outlets and their info under dealer having dealerId equels @AdminDealerId
*/
AS
BEGIN
	--Vaibhav K 15 Apr 2015 changed query
	SELECT DISTINCT DO.ID as DealerId,DO.EmailId,DO.Organization,DO.CityId,DO.Lattitude
	,DO.Longitude,DO.Address1+', '+DO.Address2 +', '+A.Name+',' +C.Name+', PIN - '+DO.Pincode AS DealerAddress,
	DO.WebsiteContactMobile AS MobileNo
	FROM Dealers D
	LEFT JOIN TC_DealerAdmin DAD ON D.ID = DAD.DealerId AND D.IsMultiOutlet = 1
	LEFT JOIN TC_DealerAdminMapping DAMP ON DAD.Id = DAMP.DealerAdminId
	LEFT JOIN Dealers DO ON DAMP.DealerId = DO.ID OR D.ID = DO.ID
	JOIN Areas A WITH(NOLOCK) ON  A.ID=DO.AreaId
	JOIN Cities C WITH(NOLOCK) ON C.ID=DO.CityId	
	WHERE D.ID = @AdminDealerId AND DO.IsDealerDeleted = 0 AND DO.IsDealerActive = 1

	/*
	select D.ID as DealerId,D.EmailId,D.Organization,D.CityId,D.Lattitude
	,D.Longitude,D.Address1+', '+D.Address2 +', '+A.Name+',' +C.Name+', PIN - '+D.Pincode AS DealerAddress,
	D.WebsiteContactMobile AS MobileNo
	from 
	Dealers AS D 
	INNER JOIN TC_DealerAdminMapping AS DAM WITH(NOLOCK) ON D.ID=DAM.DealerId
	INNER JOIN TC_DealerAdmin AS DA WITH(NOLOCK) ON DA.Id= DAM.DealerAdminId
	INNER JOIN Areas A WITH(NOLOCK) ON  A.ID=D.AreaId
	INNER JOIN States S WITH(NOLOCK) ON S.ID=D.StateId
	INNER JOIN Cities C WITH(NOLOCK) ON C.ID=D.CityId AND C.StateId=S.ID
	where DA.DealerId=@AdminDealerId AND D.IsDealerActive=1 
	*/
END
