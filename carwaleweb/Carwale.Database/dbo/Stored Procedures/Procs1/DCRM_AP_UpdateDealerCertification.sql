IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AP_UpdateDealerCertification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AP_UpdateDealerCertification]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <23/04/2015>
-- Description:	<Update cerification for activated package>
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_AP_UpdateDealerCertification]
AS
BEGIN
	DECLARE @TmpActivatedPackagesTbl TABLE (RowID INT IDENTITY(1,1) not null, ActivatedPkgId INT , StockId int)
	DECLARE @RowCnt		INT = 0
	DECLARE @StockId	INT

	INSERT INTO @TmpActivatedPackagesTbl (ActivatedPkgId,StockId)
	SELECT DISTINCT AP.Id , SI.TC_StockId 
	FROM DCRM_ActivatedPackages AP with(NOLOCK) 
	INNER JOIN SellInquiries SI WITH(NOLOCK) ON  AP.DealerId = SI.DealerId 
	INNER JOIN livelistings LL WITH(NOLOCK) ON SI.ID = LL.Inquiryid AND LL.SellerType = 1
	WHERE AP.IsAutobizSynced = 0
	
	SET @RowCnt = @@ROWCOUNT
	DECLARE @RowId	INT = 1

	--pass stockId to update certification
	WHILE (@RowId <= @RowCnt )
	BEGIN
		SELECT @StockId = StockId FROM @TmpActivatedPackagesTbl WHERE RowID = @RowId
		EXEC [dbo].[AbSure_ChangeCertification] @StockId
		SET @RowId = @RowId + 1
	END
	-- UPDATE IsAutobizSynced = 1 
	IF @RowCnt > 0 
	BEGIN
		UPDATE AP SET IsAutobizSynced = 1
		FROM DCRM_ActivatedPackages AP
		INNER JOIN (SELECT DISTINCT ActivatedPkgId FROM @TmpActivatedPackagesTbl) TMP ON TMP.ActivatedPkgId = AP.Id
	END
END



