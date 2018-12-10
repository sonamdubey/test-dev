IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_DealersCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_DealersCities]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil >
-- Create date: <29th Dec 15>
-- Description:	<Fetch Dealers Cities with prices>
--TC_GetAgedCarsDealersCities 5,246
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_DealersCities]
AS
BEGIN
	SELECT DISTINCT DC.CitiesId  AS CityId, C.Name AS CityName
	FROM  TC_Deals_Cities DC WITH (NOLOCK) 
	INNER JOIN Cities AS C  WITH (NOLOCK)  on DC.CitiesId = C.ID
	Order By C.Name 
END
