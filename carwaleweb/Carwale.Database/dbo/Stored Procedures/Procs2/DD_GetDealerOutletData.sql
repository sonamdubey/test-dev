IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_GetDealerOutletData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_GetDealerOutletData]
GO

	



-------------------------------------------------------------------------------------------


-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <21/10/2014>
-- Description:	<Get Dealer OutletData>
-- =============================================
CREATE PROCEDURE [dbo].[DD_GetDealerOutletData]
	@DD_DealerNamesId	INT,
	@OutletId			INT
AS
BEGIN
	SELECT DO.Id AS OutletId , OutletName , OutletType , CM.Name AS Make , EMailId ,DayType, ContactHours ,Day, OU.UserName , DO.CreatedOn , DO.Website 
	, A.Address + CASE WHEN AR.Name IS NULL THEN '' ELSE ',' +AR.NAME END  +','+ C.Name  +'-'+ A.Pincode as Address
	FROM DD_DealerOutlets DO
	INNER JOIN CarMakes CM WITH(NOLOCK) ON CM.ID = DO.MakeId 
	INNER JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = DO.CreatedBy
	LEFT JOIN DD_DealerOutletAddress OA WITH(NOLOCK) ON OA.DD_DealerOutletsId = DO.id
	LEFT JOIN DD_Addresses A WITH(NOLOCK) ON A.Id = OA.DD_AddressesId
	LEFT JOIN Areas AR WITH(NOLOCK) ON AR.ID = A.AreaId
	LEFT JOIN Cities C WITH(NOLOCK) ON A.CityId = C.ID
	WHERE DO.DD_DealerNamesId = @DD_DealerNamesId AND DO.Id = CASE WHEN @OutletId = -1 THEN DO.Id ELSE @OutletId END
	ORDER BY DO.Id DESC
END

