IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_GetDealerOutletAddrData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_GetDealerOutletAddrData]
GO

	


-------------------------------------------------------------------------------------------


-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <29/10/2014>
-- Description:	<Get Dealer OutletData>
-- =============================================
CREATE PROCEDURE [dbo].[DD_GetDealerOutletAddrData]
	@DD_DealerNamesId	INT
AS
BEGIN
	SELECT DA.Id AS AddrId, DA.Address , S.Name AS State , CT.Name AS City, ISNULL(A.NAME , '-') AS Area,A.ID AS AreaId , DA.Pincode , DA.Longitude 
	, DA.Latitude , OU.UserName , DA.CreatedOn , D.OutletName
	FROM DD_Addresses DA
	INNER JOIN Cities CT WITH (NOLOCK) ON CT.ID = DA.CityId
	LEFT JOIN Areas A WITH(NOLOCK) ON A.ID = DA.AreaId
	INNER JOIN States S WITH(NOLOCK) ON S.ID = CT.StateId	
	INNER JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = DA.CreatedBy
	LEFT JOIN DD_DealerOutletAddress DO WITH(NOLOCK) ON DO.DD_AddressesId = DA.Id
	LEFT JOIN DD_DealerOutlets D WITH(NOLOCK) ON D.Id = DO.DD_DealerOutletsId
	WHERE DA.DD_DealerNamesId = @DD_DealerNamesId
	ORDER BY DA.Id DESC
END

