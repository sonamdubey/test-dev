IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarTrade_GetCertificationReqParameters]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarTrade_GetCertificationReqParameters]
GO

	-- ====================================================
-- Author		: Suresh Prajapati
-- Created on	: 19th Dec, 2015
-- Description	: To get stock details and dealer details parametrs for CarTrade certification request of a stock
-- Modified by  : Vicky gupta on 9/2/2016, fetched owner count for a given stock
--                Chetan Navin on 3/3/2016 ,Added dealer contact person,email and pincode columns to fetch                 
-- exec TC_CarTrade_GetCertificationReqParameters 612009
-- ====================================================
CREATE PROCEDURE [dbo].[TC_CarTrade_GetCertificationReqParameters]
@StockId INT
AS
BEGIN
	DECLARE @OwnerNo VARCHAR(50)
	SELECT @OwnerNo = Owners FROM TC_CarCondition WITH(NOLOCK)
	WHERE StockId = @StockId

	SELECT S.RegNo
		,S.BranchId
		,D.Organization
		,D.MobileNo
		,ISNULL(D.Address1, '') + ISNULL(D.Address2, '') AS DealerAddress
		,CMA.NAME AS Make
		,CMO.NAME AS Model
		,CV.NAME AS [Version]
		,S.Colour
		,CONVERT(VARCHAR, DATEPART(YEAR, S.MakeYear)) AS ManufacturingYear
		,S.Kms 
		,@OwnerNo AS OwnerNo
		,D.ContactPerson 
		,D.EmailId
		,A.PinCode
		,C.Name AS City
	FROM TC_Stock AS S WITH (NOLOCK)
	INNER JOIN Dealers AS D WITH (NOLOCK) ON D.ID = S.BranchId
	INNER JOIN CarVersions AS CV WITH (NOLOCK) ON CV.ID = S.VersionId
	INNER JOIN CarModels AS CMO WITH (NOLOCK) ON CMO.ID = CV.CarModelId
	INNER JOIN CarMakes AS CMA WITH (NOLOCK) ON CMA.ID = CMO.CarMakeId
	INNER JOIN Cities C WITH(NOLOCK) ON C.ID = D.CityId
	INNER JOIN Areas A WITH(NOLOCK) ON A.ID = D.AreaId
	WHERE S.Id = @StockId
END
-----------------------------------------------------------------------------------------------------
