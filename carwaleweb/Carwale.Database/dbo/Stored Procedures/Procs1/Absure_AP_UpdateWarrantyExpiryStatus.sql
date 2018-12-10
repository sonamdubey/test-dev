IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_AP_UpdateWarrantyExpiryStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_AP_UpdateWarrantyExpiryStatus]
GO

	
-- =============================================
-- Author      : Deepak Tripathi
-- Create date : 13th May 2015
-- Description : To update status of warranty where its expired
-- =============================================
CREATE PROCEDURE [dbo].[Absure_AP_UpdateWarrantyExpiryStatus] 
	
AS
BEGIN
		DECLARE @NumberRecords AS INT
		DECLARE @RowCount AS INT
		DECLARE @CarId AS NUMERIC
		DECLARE @DealerStockId AS NUMERIC
		DECLARE @ProfileId AS NUMERIC
		DECLARE @CertificationId AS NUMERIC
		DECLARE @CertificationURL AS VARCHAR(200)
		DECLARE @TempData Table(RowID INT IDENTITY(1, 1), CarId NUMERIC, StockId NUMERIC)
		DECLARE @InspectionCertificationId AS SMALLINT = 454
		DECLARE @CarScore INT
		DECLARE @WarrantyId SMALLINT
	
		INSERT INTO @TempData								
		SELECT  ACD.Id AS CarId, ACD.StockId 
		FROM	AbSure_CarDetails ACD WITH (NOLOCK) 
		WHERE	SurveyDate IS NOT NULL 
				AND DATEADD(dd,30,SurveyDate) < GETDATE()
				AND Status IN(1,2,4) AND ACD.IsActive = 1 -- Survey Done, Rejected, Accepted
				AND StockId IN  (
									SELECT	TC_StockId 
									FROM	SellInquiries SI WITH(NOLOCK), LiveListings LI WITH(NOLOCK)
									WHERE	SI.ID = LI.Inquiryid 
											AND LI.SellerType = 1 
											AND LI.AbsureScore IS NOT NULL
											AND LI.CertificationId = 417
								)
				
		
		SET @NumberRecords = @@ROWCOUNT
		SET @RowCount = 1

		SELECT	@CertificationURL =  HostURL + '' + DirectoryPath + '' + LogoURL,	
				@CertificationId = Id
		FROM	Classified_CertifiedOrg WITH(NOLOCK) 
		WHERE	Id = @InspectionCertificationId

		IF(@NumberRecords >= 1)
		BEGIN
			WHILE @RowCount <= @NumberRecords
			BEGIN
				SELECT	@CarId = CarId, @DealerStockId = StockId 
				FROM	@TempData 
				WHERE	RowID = @RowCount
						
				--Update CarWale Live Listing Score Data
				SELECT	@ProfileId = SI.Id
				FROM	SellInquiries SI WITH (NOLOCK) , LiveListings LL WITH (NOLOCK), Dealers D  WITH (NOLOCK)
				WHERE	SI.TC_StockId = @DealerStockId 
						AND SI.ID = LL.Inquiryid 
						AND LL.SellerType = 1 
						AND SI.DealerId = D.ID
				IF @@ROWCOUNT > 0
				BEGIN
					/*Expired then certifcationId = 454 accept for gold warranaty activated.*/	
					--Update SellInquiries
					UPDATE	SellInquiries 
					SET		CertificationId = @CertificationId, CertifiedLogoUrl = @CertificationURL
					WHERE	ID = @ProfileId		

					INSERT INTO AbSure_LiveListingUpdateLog (CertifiedLogoUrl,CertificationId,AbSure_CarDetailsId,AbSure_Score,TC_StockId, EntryDate, AbSure_WarrantyTypeId)
					VALUES (@CertificationURL,@CertificationId,@CarId,@CarScore,@DealerStockId, GETDATE(),@WarrantyId)	
				
				END
				SET @RowCount = @RowCount + 1
			END
		END

END

