IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarTrade_GetStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarTrade_GetStatus]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 19th Feb, 2015
-- Description : To get status of cartrade certification/warranty
-- Modified By : Chetan Navin on 8th Mar, 2016 (Added condition to check dealer's balance in case if warranty is available)
-- =============================================
CREATE PROCEDURE [dbo].[TC_CarTrade_GetStatus]
	@StockId INT,
	@Status TINYINT = 0 OUTPUT 
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 1 @Status = CertificationStatus 
	FROM TC_CarTradeCertificationRequests WITH(NOLOCK) 
	WHERE  ListingId = @StockId
	ORDER BY TC_CarTradeCertificationRequestId DESC
	--Refer Table TC_CartradeCertificationStatus

	IF(@Status = 1)
	BEGIN
		--Status 5  : Set to indicate that warranty is available
		--Status 9  : Set to indicate certification expired
		--Status 10 : Set to indicate that dealer's account balance is in negative
		SELECT @Status = CASE WHEN TR.CertificationStatus = 1 AND ISNULL(TC.IsWarranty,0) = 1 AND AC.ClosingBalance > 0 AND DATEDIFF(DAY,TC.InvCertifiedDate,GETDATE()) <= 45 THEN 5
							  WHEN  DATEDIFF(DAY,TC.InvCertifiedDate,GETDATE()) > 45 THEN 9 
							  WHEN TR.CertificationStatus = 1 AND ISNULL(TC.IsWarranty,0) = 1 AND DATEDIFF(DAY,TC.InvCertifiedDate,GETDATE()) <= 45 AND AC.ClosingBalance < 0 THEN 10
							  ELSE @Status END
		FROM TC_CarTradeCertificationData TC WITH(NOLOCK) 
		INNER JOIN TC_CarTradeCertificationLiveListing TL WITH(NOLOCK) ON TL.ListingId = TC.ListingId
		INNER JOIN TC_CarTradeCertificationRequests TR WITH(NOLOCK) ON TR.TC_CarTradeCertificationRequestId = TL.TC_CarTradeCertificationRequestId
		INNER JOIN AbSure_Trans_ClosingBalance AC WITH(NOLOCK) ON AC.DealerId = TR.DealerId
		WHERE TC.ListingId = @StockId
	END
END

