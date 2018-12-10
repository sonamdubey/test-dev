IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDealerState]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDealerState]
GO

	
CREATE PROCEDURE [dbo].[Microsite_GetDealerState]

@DealerId int

AS 

--Author:Rakesh Yadav

--Date Created: 24 March 2015

--Desc: Get states in which dealer operates



BEGIN

	

	SELECT St.Name AS Text,St.ID AS Value FROM States St WITH(NOLOCK)

	INNER JOIN Cities ct WITH(NOLOCK) ON ct.StateId=St.ID

	INNER JOIN TC_DealerCities TD WITH(NOLOCK) ON TD.CityId=ct.ID

	WHERE TD.DealerId=@DealerId AND TD.IsActive=1
	GROUP BY St.Name,St.ID
	ORDER BY St.Name



END
