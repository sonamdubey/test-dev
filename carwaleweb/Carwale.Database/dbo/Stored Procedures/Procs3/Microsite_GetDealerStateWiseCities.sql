IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDealerStateWiseCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDealerStateWiseCities]
GO

	
CREATE PROCEDURE [dbo].[Microsite_GetDealerStateWiseCities]
@DealerId int,
@StateId int
AS 
--Author:Rakesh Yadav
--Date Created: 24 March 2015
--Desc: Get statewise cities in which dealer operates
BEGIN
	SELECT DISTINCT TD.CityId AS Value, C.Name AS Text 
	FROM	
	TC_DealerCities AS TD WITH (NOLOCK) 
	INNER JOIN Cities AS C  WITH (NOLOCK)  on TD.CityID = C.ID       
	INNER JOIN States ST WITH (NOLOCK) ON ST.ID=C.StateId
	WHERE	TD.DealerId = @DealerId AND TD.IsActive = 1  
	AND ST.ID=@StateId
	Order By C.Name
END
