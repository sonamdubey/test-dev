IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMGetCustomersDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMGetCustomersDetails]
GO

	-- =============================================
-- Name :        TC_MMGetCustomersDetails
-- Author:		Ranjeet Kumar
-- Create date: 05-11-2013
-- Description:	Get the customer details for MIX N MATCH on click of stock count
-- Tables : TC_MMCustomerDetails,TC_MMvwUsedCarInquiries, vwMMV
-- Modified by: Nilesh Utture on 27-12-2013 Removed YEAR Function from SELECT Clause for "TVW.MakeYear" field and added TVW.SellInquiryId
-- Modified by: Manish on 24-01-2014 adding WITH (NOLOCK)  keyword in all the tables.
-- Modified by: Manish on 03-03-2014 Add condition Isdeleted=0 for TC_MMCustomerDetails since new field introduced in this table.
-- =============================================
CREATE PROCEDURE [dbo].[TC_MMGetCustomersDetails] 
	@MatchStockId int
	AS
BEGIN
	SET NOCOUNT ON;	

	SELECT DISTINCT MMC.CWCustomersId AS ID, TVW.Name AS Name, ('xxxxxxx'+ RIGHT(TVW.CustomerMobile,3)) AS Moblie,
		('xxxx'+ RIGHT(TVW.CustomerEmail,(LEN(TVW.CustomerEmail)-CHARINDEX('@',TVW.CustomerEmail) + 1))) AS Email, 
		TVW.MakeYear as CarYear, TVW.Kms, TVW.Price,TVW.InquiryId AS InquiryId, TVW.SellerType as SellerType, (VM.Model + ' ' + VM.Version) as CarName, 
		TVW.SellInquiryId  as SellInqId, FT.FuelType as FuelType, ISNULL(TVW.AreaName,'') AS AreaNames, COUNT(MA.Id) AS ViewCount,
		CONVERT(VARCHAR,(DATEDIFF(DAY, MMC.CustomerResponseDate, GETDATE()))) + ' days ago' AS ResponseDate, MMC.CustomerResponseDate AS ResDate
	FROM TC_MMCustomerDetails AS MMC WITH (NOLOCK)
		INNER JOIN TC_MMvwUsedCarInquiries TVW  WITH (NOLOCK) ON MMC.CWInquiryId = TVW.InquiryId
		INNER JOIN vwMMV VM WITH (NOLOCK)  ON VM.VersionId = TVW.CarVersionId
		LEFT JOIN CarFuelType FT WITH (NOLOCK)  ON FT.FuelTypeId = TVW.FuelTypeId
		LEFT JOIN TC_MMAudit MA WITH (NOLOCK)  ON TVW.CustomerId = MA.CustomerId
	WHERE MMC.SellerType = TVW.SellerType AND MMC.MatchedStockId = @MatchStockId
	AND MMC.IsDeleted=0   ----Condition Added by Manish on 03-03-2014
	GROUP BY MMC.CWCustomersId, TVW.Name,TVW.CustomerMobile, TVW.CustomerEmail, 
	TVW.MakeYear, TVW.Kms, TVW.Price,TVW.InquiryId, TVW.SellerType,VM.Model, 
	VM.Version, TVW.SellInquiryId, FT.FuelType, TVW.AreaName, MMC.CustomerResponseDate
	ORDER BY ResDate DESC
	
END


