IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPremiumDealerStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPremiumDealerStock]
GO

	
-- =============================================
-- Author:		Purohith Guguloth
-- Create date: 04/06/2015
-- Description:	This Sp is used to get the dealer stock listings
     --for the Premium Dealer Showroom (currently BBT)
-- =============================================
CREATE PROCEDURE [dbo].[GetPremiumDealerStock] 
	-- Add the parameters for the stored procedure here
	@dealerId INT = 0  --input variable for dealer ID for which we need the stock
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- select statements for the procedure id here
	SELECT  LL.ProfileId
    ,(MakeName + ' ' + ModelName + ' ' + VersionName) Car
    ,CONVERT(VARCHAR, MakeYear, 106) MakeYear
    ,LL.MakeName
    ,LL.ModelName
    ,LL.CityName
    ,LL.Price
    ,LL.Kilometers
    ,LL.CertificationId
    ,ISNULL(CP.DirectoryPath, '') + ISNULL(CP.ImageUrlThumb, '') AS ImageUrlMedium
    ,CP.ImageUrlThumbSmall
    ,CP.HostURL
    ,CP.DirectoryPath
    ,CMO.MaskingName 
    FROM LiveListings AS LL 
		LEFT OUTER JOIN CarPhotos CP 
			ON LL.InquiryId = CP.InquiryId --Left Outer Join to get photos using Inquiry ID 
    AND CP.IsMain = 1
    AND CP.IsActive = 1
    AND CP.IsApproved = 1
    AND CP.IsDealer = 1
    INNER JOIN CarModels CMO ON LL.ModelId = CMO.ID AND LL.SellerType = 1  
    AND LL.InquiryId IN (SELECT ID FROM SellInquiries
        WHERE DealerId = @dealerId AND StatusId = 1) -- Adding the filter for Active Stocks
    ORDER BY LL.Inquiryid DESC
END

