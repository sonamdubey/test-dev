IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetBikeDealerCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetBikeDealerCities]
GO

	

-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 29th Oct 2014
-- Description:	To Get list of Cities where bike dealers are available.
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetBikeDealerCities]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT D.CityId AS Value, C.Name AS Text
	FROM Cities AS C WITH(NOLOCK)
	INNER JOIN  Dealers AS D WITH(NOLOCK) ON C.ID = D.CityId
	WHERE D.ApplicationId=2 AND D.IsDealerActive=1 AND D.IsDealerDeleted=0 AND C.IsDeleted=0
	ORDER BY C.Name
END


