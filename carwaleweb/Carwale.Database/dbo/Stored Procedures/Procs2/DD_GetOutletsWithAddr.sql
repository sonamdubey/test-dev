IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_GetOutletsWithAddr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_GetOutletsWithAddr]
GO

	


-------------------------------------------------------------------------------------------


-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <1/12/2014>
-- Description:	<Get All outlets with associted address and dealers>
-- =============================================
CREATE PROCEDURE [dbo].[DD_GetOutletsWithAddr] 
	@SearchText      VARCHAR(50)
AS
BEGIN
	SELECT DO.Id AS OutletId,DN.Id AS DealerId ,DO.OutletName + CASE WHEN A.AreaId <> '0' THEN ','+AR.Name ELSE '' END +CASE WHEN A.CityId IS NOT NULL THEN ','+CT.Name ELSE '' END
	+ CASE WHEN do.DD_DealerNamesId IS NOT NULL THEN ','+DN.Name ELSE '' END  AS Outlet
	FROM DD_DealerOutlets DO 
	INNER JOIN DD_DealerNames DN WITH(NOLOCK) ON DN.Id = DO.DD_DealerNamesId
	LEFT JOIN DD_DealerOutletAddress DA WITH(NOLOCK) ON DA.DD_DealerOutletsId = DO.Id
	LEFT JOIN DD_Addresses A WITH(NOLOCK) ON A.Id = DA.DD_AddressesId
	LEFT JOIN Cities CT WITH(NOLOCK) ON A.CityId = CT.ID
	LEFT JOIN Areas AR WITH(NOLOCK) ON AR.ID = A.AreaId
	WHERE DO.OutletName LIKE @SearchText +'%'
END

