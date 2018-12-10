IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCitiesHavingDealerShowroom_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCitiesHavingDealerShowroom_v16]
GO

	-- Author:Akansha
-- Created on:30.05.2013
-- Description: Get all the cities having dealer showrooms
-- Exec GetCitiesHavingDealerShowroom 
-- Modified by : Raghu on 17/12/2013 Added SellInquiries list for Joining
-- Modified by : Satish Sharma on 20-may-2014 -- Commented like "AND CCP.PackageType = 19" in where clause
-- Modified by : Manish on 08-07-2014 added with (nolock) keyword in the tables.
-- Modified by : Pawan kumar on 09-08-2016 with addition of new table CT_AddOnPackages

CREATE PROCEDURE [dbo].[GetCitiesHavingDealerShowroom_v16.8.3]
AS
BEGIN
SELECT DISTINCT Ct.Id
	,Ct.NAME AS City
FROM Dealers AS D WITH (NOLOCK)
	INNER JOIN Cities AS Ct WITH (NOLOCK) ON D.CityId=Ct.ID
	INNER JOIN CT_AddOnPackages AS AD WITH (NOLOCK) ON D.ID=AD.CWDealerId
	INNER JOIN SellInquiries Si WITH (NOLOCK) ON Si.DealerId=D.ID
WHERE 		
	--AND CCP.PackageType = 19
	AD.EndDate>= GetDate()
	AND AD.IsActive = 1
	AND AD.AddOnPackageId = 100
	AND D.STATUS = 0	
ORDER BY City

END
