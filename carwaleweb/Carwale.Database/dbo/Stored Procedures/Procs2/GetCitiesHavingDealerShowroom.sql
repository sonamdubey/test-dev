IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCitiesHavingDealerShowroom]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCitiesHavingDealerShowroom]
GO

	-- Author:Akansha
-- Created on:30.05.2013
-- Description: Get all the cities having dealer showrooms
-- Exec GetCitiesHavingDealerShowroom 
-- Modified by : Raghu on 17/12/2013 Added SellInquiries list for Joining
-- Modified by : Satish Sharma on 20-may-2014 -- Commented like "AND CCP.PackageType = 19" in where clause
-- Modified by : Manish on 08-07-2014 added with (nolock) keyword in the tables.

CREATE PROCEDURE [dbo].[GetCitiesHavingDealerShowroom]
AS
BEGIN
SELECT DISTINCT Ct.Id
	,Ct.NAME AS City
FROM Dealers AS D WITH (NOLOCK)
	,Cities AS Ct WITH (NOLOCK)
	,ActiveDealers AS AD WITH (NOLOCK)
	,ConsumerCreditPoints AS CCP WITH (NOLOCK)
	,SellInquiries Si WITH (NOLOCK)
WHERE D.CityId = Ct.Id
	AND D.Id = AD.DealerId
	AND D.Id = CCP.ConsumerId
	AND CCP.ConsumerType = 1
	--AND CCP.PackageType = 19
	AND CCP.ExpiryDate >= GetDate()
	AND AD.isActive = 1
	AND AD.HasShowroom = 1
	AND D.STATUS = 0
	AND Si.DealerId = D.ID
ORDER BY City
END