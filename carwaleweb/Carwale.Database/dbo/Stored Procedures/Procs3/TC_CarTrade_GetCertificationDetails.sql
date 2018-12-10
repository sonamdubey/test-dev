IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarTrade_GetCertificationDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarTrade_GetCertificationDetails]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 04th Dec 2015
-- Description : To get certified car details
-- Modified By : Chetan Navin (Added ThumbnailImage in the image query)
-- =============================================
CREATE PROCEDURE [dbo].[TC_CarTrade_GetCertificationDetails]
	@ListingId INT
AS
BEGIN
	SET NOCOUNT ON;
    DECLARE @TC_CarTradeCertificationDataId INT

	SELECT TOP 1 @TC_CarTradeCertificationDataId = TC_CarTradeCertificationDataId
	FROM TC_CarTradeCertificationData TC WITH(NOLOCK)
	INNER JOIN TC_CarTradeCertificationLiveListing TL WITH(NOLOCK) ON TL.ListingId = TC.ListingId
	WHERE TC.ListingId = @ListingId
	ORDER BY TC_CarTradeCertificationDataId DESC

	--Fetches car details
	SELECT City,Make,Model,Variant,Color,MfgMonth,MfgYear,Mileage,Owner,RegType,RegDate,RegNum,RegCity,LastRegDate,Insurance,InsExp,Hypo,Transmission,
	WheelBackLeft,WheelBackRight,WheelFrontLeft,WheelFrontRight,WheelSt,InvCertifiedDate AS CertificateDate,DamageImage
	FROM TC_CarTradeCertificationData WITH(NOLOCK)	
	WHERE TC_CarTradeCertificationDataId = @TC_CarTradeCertificationDataId

	--Fetches refurb details
	SELECT Category,SubCategory,SubName 
	FROM TC_CarTradeRefurb WITH(NOLOCK)
	WHERE TC_CarTradeCertificationDataId = @TC_CarTradeCertificationDataId

	--Fetches images details
	SELECT ImageName,ImageTag,TC_CarTradeImageId as ImageId ,ThumbnailImage
	FROM TC_CarTradeImages WITH(NOLOCK) 
	WHERE TC_CarTradeCertificationDataId = @TC_CarTradeCertificationDataId

	--Fetches accessory details
	SELECT AccessoryName
	FROM TC_CarTradeAccessories WITH(NOLOCK) 
	WHERE TC_CarTradeCertificationDataId = @TC_CarTradeCertificationDataId
END
