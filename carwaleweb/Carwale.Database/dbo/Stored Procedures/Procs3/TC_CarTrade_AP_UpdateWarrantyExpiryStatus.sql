IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarTrade_AP_UpdateWarrantyExpiryStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarTrade_AP_UpdateWarrantyExpiryStatus]
GO

	
-- =============================================
-- Author      : Chetan Navin
-- Create date : 11th Feb 2016
-- Description : To update status of inspection/warranty where its expired
-- =============================================
CREATE PROCEDURE [dbo].[TC_CarTrade_AP_UpdateWarrantyExpiryStatus] 

AS
BEGIN
		DECLARE @NumberRecords AS INT
		DECLARE @RowCount AS INT
		DECLARE @TC_CarTradeCertificationDataId AS NUMERIC
		DECLARE @DealerStockId AS NUMERIC
		DECLARE @CertificationId AS NUMERIC
		DECLARE @CertificationURL AS VARCHAR(200)
		DECLARE @TempData Table(RowID INT IDENTITY(1, 1), TC_CarTradeCertificationDataId NUMERIC, StockId NUMERIC)
		DECLARE @InspectionCertificationId AS SMALLINT = 454 --Certification, 417 - Warranty
	
		INSERT INTO @TempData								
		SELECT  TCD.TC_CarTradeCertificationDataId AS TC_CarTradeCertificationDataId, TCD.ListingId AS StockId
		FROM	TC_CarTradeCertificationData TCD WITH (NOLOCK) 
		INNER JOIN TC_CarTradeCertificationLiveListing TL WITH(NOLOCK) ON TL.ListingId = TCD.ListingId
		WHERE   DATEDIFF(DAY,TCD.InvCertifiedDate,GETDATE()) > 45  -- Expired
				AND TCD.ListingId IN  (
									SELECT	TC_StockId 
									FROM	SellInquiries SI WITH(NOLOCK), LiveListings LI WITH(NOLOCK)
									WHERE	SI.ID = LI.Inquiryid 
											AND LI.SellerType = 1 
											AND LI.CertificationId = @InspectionCertificationId
								)
				
		
		SET @NumberRecords = @@ROWCOUNT
		SET @RowCount = 1

		
		

		IF(@NumberRecords >= 1)
		BEGIN
			WHILE @RowCount <= @NumberRecords
			BEGIN
				
					
				SELECT	@TC_CarTradeCertificationDataId = TC_CarTradeCertificationDataId, @DealerStockId = StockId 
				FROM	@TempData 
				WHERE	RowID = @RowCount
				
				SELECT  @CertificationId = CertificationId, @CertificationURL = CertifiedLogoUrl
				FROM	TC_Stock	WITH(NOLOCK) 
				WHERE	Id = @DealerStockId
						
			
				/*Expired then certifcationId = 454 accept for gold warranaty activated.*/	
				--Update SellInquiries
				UPDATE	SellInquiries 
				SET		CertificationId = @CertificationId, CertifiedLogoUrl = @CertificationURL
				WHERE	TC_StockId = @DealerStockId		

				DELETE FROM TC_CarTradeCertificationLiveListing
				WHERE ListingId = @DealerStockId

				INSERT INTO AbSure_LiveListingUpdateLog (CertifiedLogoUrl,CertificationId,AbSure_CarDetailsId,AbSure_Score,TC_StockId, EntryDate, AbSure_WarrantyTypeId)
				VALUES (@CertificationURL,@CertificationId,@TC_CarTradeCertificationDataId,NULL,@DealerStockId, GETDATE(),NULL)	


				SET @RowCount = @RowCount + 1
			END
		END
END




