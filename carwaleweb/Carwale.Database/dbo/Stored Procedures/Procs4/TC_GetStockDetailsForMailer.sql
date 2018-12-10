IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetStockDetailsForMailer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetStockDetailsForMailer]
GO

	-- =============================================
-- Author:		<khushaboo Patil>
-- Create date: <7/07/2015>
-- Description:	<Get Stock Details to send mail>
-- =============================================
create PROCEDURE [dbo].[TC_GetStockDetailsForMailer]
	@StockId	INT
AS
BEGIN
		SELECT DISTINCT st.Id,st.BranchId,'D'+ CONVERT(VARCHAR, I.ID) ProfileId,st.IsSychronizedCW,St.Price,CC.Owners,ISNULL(CC.RegistrationPlace,'-')RegistrationPlace , St.kms, DATEPART(YEAR,st.MakeYear)MakeYear	, 
		Ma.id MakeId, Mo.id ModelId,( Ma.name + ' ' + Mo.name + ' ' + Ve.name )   AS MakeModelVersion , 
		(SELECT TOP 1 CP.HostUrl + CP.DirectoryPath + CP.ImageUrlThumb
		FROM   tc_carphotos Cp 
		WHERE  Cp.stockid = St.id 
		AND Cp.isactive = 1 and cp.IsMain = 1)Photos,CF.FuelTypeName , CT.Descr CarTransmission,D.Organization,D.EmailId , D.MobileNo ,
		ISNULL(D.Address1,'') + CASE WHEN D.Address2 IS NULL THEN '' ELSE ',' END Address1,
		ISNULL(D.Address2,'') Address2 , C.Name AS CityName
		FROM   tc_stock St WITH (nolock) 
		INNER JOIN TC_CarCondition CC WITH(NOLOCK) ON CC.StockId = ST.Id
		INNER JOIN  carversions Ve WITH (NOLOCK) ON Ve.id = St.versionid 
		INNER JOIN carmodels Mo WITH (NOLOCK) ON Mo.id = Ve.carmodelid 
		INNER JOIN carmakes Ma WITH (NOLOCK) ON Ma.id = Mo.carmakeid
		INNER JOIN TC_CarFuelType CF WITH(NOLOCK) ON CF.FuelTypeId = VE.CarFuelType
		INNER JOIN CarTransmission CT WITH(NOLOCK) ON CT.Id = VE.CarTransmission
		LEFT JOIN SellInquiries I WITH(NOLOCK) ON I.TC_StockId = ST.Id
		INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = ST.BranchId
		LEFT JOIN Cities C WITH(NOLOCK) ON C.Id = D.CityId
		WHERE  St.id = @StockId
END

