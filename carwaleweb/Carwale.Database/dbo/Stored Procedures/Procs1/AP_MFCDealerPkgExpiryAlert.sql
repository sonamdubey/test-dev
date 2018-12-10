IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_MFCDealerPkgExpiryAlert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_MFCDealerPkgExpiryAlert]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <8th Apr 2015>
-- Description:	<Send Alert on MFC dealers package expired>
-- =============================================
CREATE PROCEDURE [dbo].[AP_MFCDealerPkgExpiryAlert]
AS
BEGIN
	SELECT DealerId, D.Organization, D.MobileNo, D.EmailId, IP.Name AS Package, CONVERT(VARCHAR(11),CP.ExpiryDate,106)ExpiryDate, DATEDIFF(dd, GETDATE(), CP.ExpiryDate) AS ExpiringIn
	FROM TC_MFCDealers TF WITH (NOLOCK)
	INNER JOIN ConsumerCreditPoints CP WITH (NOLOCK) ON TF.DealerId = CP.ConsumerId AND CP.ConsumerType = 1
	INNER JOIN Dealers D WITH (NOLOCK) ON D.Id = TF.DealerId
	INNER JOIN InquiryPointCategory IP WITH (NOLOCK) ON CP.PackageType = IP.Id
	WHERE DATEDIFF(dd, GETDATE(), CP.ExpiryDate) <=10 AND DATEDIFF(dd, GETDATE(), CP.ExpiryDate) > 0
END
