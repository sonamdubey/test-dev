IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPremiumDealerStock_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPremiumDealerStock_v15]
GO

	
-- =============================================
-- Author:		Purohith Guguloth
-- Create date: 04/06/2015
-- Description:	This Sp is used to get the dealer stock listings
     --for the Premium Dealer Showroom (currently BBT)
-- Modified By: Prachi Phalak on 05/08/2015
-- Modified by: Shubham Agarwal on 19/09/2016 -- Fetching Car Image URL from livelistings, Removed Carphotos
-- =============================================
CREATE PROCEDURE [dbo].[GetPremiumDealerStock_v15.8.1] 
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
    ,LL.HostURL
	,LL.OriginalImgPath
    ,CMO.MaskingName 
    FROM LiveListings AS LL WITH(NOLOCK)
    INNER JOIN CarModels CMO WITH(NOLOCK) ON LL.ModelId = CMO.ID AND LL.SellerType = 1 
    WHERE LL.DealerId = @dealerId
    ORDER BY LL.Inquiryid DESC
END

