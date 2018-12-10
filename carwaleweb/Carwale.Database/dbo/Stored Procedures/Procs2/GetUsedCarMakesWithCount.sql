IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUsedCarMakesWithCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUsedCarMakesWithCount]
GO

	
CREATE PROCEDURE [dbo].[GetUsedCarMakesWithCount]
AS
BEGIN
	SELECT MakeName
		,count(ProfileId) as MakeCount
	FROM LiveListings
	GROUP BY MakeName
	HAVING count(profileid) > 5
	ORDER BY MakeName 
END
