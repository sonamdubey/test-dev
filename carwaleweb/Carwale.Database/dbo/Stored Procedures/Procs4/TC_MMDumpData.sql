IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMDumpData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMDumpData]
GO

	-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date, 30-12-2013>
-- Description:	<Description, This Sp will run as part of Job on daily basis which will dump data for MixAndMatch>
-- =============================================
CREATE PROCEDURE [dbo].[TC_MMDumpData]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	TRUNCATE TABLE TC_MMvwUsedCarInquiries
 
	INSERT INTO [dbo].[TC_MMvwUsedCarInquiries] 
	(CustomerId,
		Name,
		CustomerMobile,
		CustomerEmail,
		CustomerResponseDate,
		InquiryId,
		SellInquiryId,
		SellerType,
		CarModelId,
		CarVersionId,
		FuelTypeId,
		Price,
		Kms,
		MakeYear,
		CityId,
		AreaName,
		DealerId)
	SELECT 
		UP.CustomerID AS CustomerId,
		CD.Name AS Name,
		CD.Mobile AS CustomerMobile,
		CD.email AS CustomerEmail,
		UP.RequestDateTime AS CustomerResponseDate,
		UP.Id AS InquiryId, 
		UP.SellInquiryId, 
		1 SellerType, 
		CV.CarModelId AS CarModelId,
		Si.CarVersionId AS CarVersionId,
		CV.CarFuelType AS FuelTypeId, 
		SI.Price AS Price,
		Si.Kilometers AS Kms, 
		CONVERT(CHAR(3),DATENAME(MONTH, Si.MakeYear)) + '-' + CONVERT(CHAR(4),YEAR(Si.MakeYear)) AS MakeYear, 
		D.CityId AS CityId,
		A.Name as AreaName,
		D.ID AS DealerId
	FROM UsedcarPurchaseInquiries UP WITH (NOLOCK)
	INNER JOIN SellInquiries Si WITH (NOLOCK) ON Si.Id = UP.SellInquiryId and DATEDIFF(DAY,UP.RequestDateTime,GETDATE()) BETWEEN 3 AND 15
	INNER JOIN CarVersions as CV WITH (NOLOCK) ON Si.CarVersionId = CV.ID
	INNER JOIN Dealers D WITH (NOLOCK) ON D.Id = Si.DealerId
	INNER JOIN Customers CD WITH (NOLOCK) ON UP.CustomerId = CD.Id
	LEFT JOIN Areas A WITH (NOLOCK) ON D.AreaId = A.ID


	UNION ALL
	SELECT 
		CR.CustomerId AS CustomerId,
		CD.Name AS Name,
		CD.Mobile AS CustomerMobile,
		CD.email AS CustomerEmail,
		CR.RequestDateTime AS CustomerResponseDate, 
		CR.Id AS InquiryId, 
		CR.SellInquiryId, 
		2 SellerType , 
		CV.CarModelId AS CarModelId,
		CI.CarVersionId AS CarVersionId,
		CV.CarFuelType AS FuelTypeId,
		CI.Price AS Price,
		CI.Kilometers AS Kms ,
		CONVERT(CHAR(3),DATENAME(MONTH, CI.MakeYear)) + '-' + CONVERT(CHAR(4),YEAR(CI.MakeYear)) AS MakeYear,  
		CI.CityId AS CityId,
		A.Name as AreaName,
		null as DealerId
	FROM ClassifiedRequests CR WITH (NOLOCK)
	INNER JOIN CustomerSellInquiries CI  WITH (NOLOCK) ON CI.Id = CR.SellInquiryId and DATEDIFF(DAY,CR.RequestDateTime,GETDATE()) BETWEEN 3 AND 15
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CI.CarVersionId = CV.ID
	INNER JOIN Customers CD WITH (NOLOCK) ON CR.CustomerId = CD.Id
	LEFT JOIN Areas A WITH (NOLOCK) ON CD.AreaId = A.ID 

END


