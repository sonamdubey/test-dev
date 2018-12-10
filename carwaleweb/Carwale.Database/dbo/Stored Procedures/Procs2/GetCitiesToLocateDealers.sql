IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCitiesToLocateDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCitiesToLocateDealers]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 29.08.2013
-- Description:	Gets the distint cities for which new car dealers are present
-- Modified by Raghu : on 3/12/2014 Added with(nolock) constraint
-- =============================================
CREATE PROCEDURE [dbo].[GetCitiesToLocateDealers]
AS
BEGIN
	 SELECT DISTINCT Ci.Id, Ci.Name 
     FROM Cities Ci WITH(NOLOCK), Dealer_NewCar AS DNC WITH(NOLOCK)
     WHERE DNC.CityId = Ci.Id AND DNC.IsActive= 1 AND Ci.IsDeleted = 0
     ORDER BY Ci.Name 
END
