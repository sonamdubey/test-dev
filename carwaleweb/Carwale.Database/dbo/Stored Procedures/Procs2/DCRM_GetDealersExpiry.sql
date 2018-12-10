IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetDealersExpiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetDealersExpiry]
GO

	
-- =============================================
-- Author:		Kartik Rathod
-- Create date: 7 sept 2016
-- Description:	package expiry date and package type if exists
-- EXEC  DCRM_GetDealersExpiry 4271,1
-- Modified By : Komal Manjare on 21-Sept-2016 
-- Desc : instead of ConsumerCreditPoints Dealers is taken as main table 
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetDealersExpiry] 
	@DealerId BIGINT,
	@ConsumerType SMALLINT
AS
BEGIN
		
    SELECT CCP.ExpiryDate, ICP.Name Package, ICP.Id, ISNULL(MAP.IsMigrated,0) AS IsMigrated,
	MAP.MigrationSuccessDate AS MigrationDate,MAP.CTDealerID,ISNULL(D.IsCarTrade,0) AS IsCarTradeWebsite,
	CASE WHEN MAP.IsMigrated=1 AND D.IsCarTrade=1 
		 THEN 1 ELSE 0 END AS IsCarTradeDealer
    FROM Dealers D WITH(NOLOCK) -- Komal Manjare on 21-Sept-2016 
	LEFT JOIN ConsumerCreditPoints CCP WITH(NOLOCK) ON CCP.ConsumerId=D.ID	AND CCP.ConsumerType = @ConsumerType	
	LEFT JOIN InquiryPointCategory ICP WITH(NOLOCK) ON CCP.PackageType = ICP.Id				
	LEFT JOIN CWCTDealerMapping MAP WITH(NOLOCK) ON D.ID = MAP.CWDealerID AND MAP.IsMigrated = 1
	WHERE D.ID= @DealerId   
END

