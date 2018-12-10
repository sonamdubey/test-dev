IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetWarrantyInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetWarrantyInquiryDetails]
GO

	-- =============================================
-- Author      : Suresh Prajapati
-- Create date : 25th Feb, 2015
-- Description : Procedure to get
-- Modified By : Suresh Prajapati on 11th Mar, 2015
-- Description : To Get Engine Number and VIN
-- EXEC [dbo].[Absure_GetWarrantyInquiryDetails] 121
-- Modified By : Ashwini Todkar on 12 March 2015
-- Description : retrieved ProductType in select clause 
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetWarrantyInquiryDetails]
	-- Add the parameters for the stored procedure here
	@WarrantyInquiryId INT
AS
BEGIN
	SELECT AWI.WarrantyType
		,ISNULL(AWI.WarrantyPrice, 0) AS WarrantyPrice
		,CMA.NAME AS [Make]
		,CMA.ID AS [MakeId]
		,CMO.NAME AS [Model]
		,CMO.ID AS [ModelId]
		,CV.NAME AS [Version]
		,CV.ID AS [VersionId]
		,RegistrationNo
		,EngineNumber
		,VIN
		,RegistrationDate
		,FuelType
		,CarFittedWith
		,Kilometers
		,CustomerName
		,CustomerEmail
		,CustomerMobile
		,CustomerAddress
		,C.NAME AS CityName
		,ISNULL(C.ID, 0) AS CityId
		,A.NAME AS AreaName
		,ISNULL(A.ID, 0) AS AreaId
		,ISNULL(AbsureCarId, 0) AS AbsureCarId
		,ProductType -- rsa or warranty 
	FROM Absure_WarrantyInquiries AS AWI WITH (NOLOCK)
	INNER JOIN CarVersions AS CV WITH (NOLOCK) ON CV.ID = AWI.VersionId
	INNER JOIN CarModels AS CMO WITH (NOLOCK) ON CMO.ID = CV.CarModelId
	INNER JOIN CarMakes AS CMA WITH (NOLOCK) ON CMA.ID = CMO.CarMakeId
	INNER JOIN AbSure_WarrantyTypes AS AWT WITH (NOLOCK) ON AWT.AbSure_WarrantyTypesId = AWI.WarrantyType
	LEFT JOIN Cities AS C WITH (NOLOCK) ON C.ID = AWI.CityId
	LEFT JOIN Areas AS A WITH (NOLOCK) ON A.ID = AWI.AreaId
	WHERE AWI.Id = @WarrantyInquiryId
END

