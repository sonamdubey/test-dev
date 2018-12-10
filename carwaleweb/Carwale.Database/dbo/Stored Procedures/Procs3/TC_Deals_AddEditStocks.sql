IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_AddEditStocks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_AddEditStocks]
GO

	-- =============================================
-- Author		: Nilima More
-- Created Date : 5th Jan 2016
-- Description  : Add  and Edit Deals Stock.
--				 1.While Addindg Status =1
--				 2.Status will be Change To 1 When Editing In Price And Offer.
-- Modifier     : Khushaboo Patil changed edit conditions 
-- Modifier     : Khushaboo Patil added IsDealerDealActive condition on 27 Jan 16
-- Modifier		: Khushaboo Patil insert modelid in DCRM_Deals_ModelStatus on stock add
-- Modifier		: Ruchira Patil on 12th May 2016 (Added parameter @DealsStockPricesBreakupIds and updated stockid for the @DealsStockPricesBreakupIds in TC_Deals_StockPricesBreakup)
-- Modifier     : Harshil on 29July ,2016 (changed from @OnRoadPrices to remove duplicated entries in TC_Deals_StockPriceslog)
--Modifier      : Anchal on 26th july, 2016 removed Insert into VIN log to remove duplicated entries in TC_Deals_StockVINLog
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_AddEditStocks] 
	@BranchId INT, 
	@CarVersionId INT, 
	@MakeYear DATETIME, 
	@VersionColorId INT,
	@InteriorColor VARCHAR(50), 
	@Offers VARCHAR(500), 
	@UserId INT, 
	@DealsStockId INT = NULL, 
	@VIN [TC_Deals_VIN] READONLY, 
	@OnRoadPrices [TC_Deals_CarStockPrice] READONLY, 
	@DealsStockPricesBreakupIds VARCHAR(500) = NULL,
	@TermsAndConditions VARCHAR(500) = NULL,
	@RetVal INT OUTPUT
AS
BEGIN
	DECLARE @IsDealerDealActive BIT
	SELECT @IsDealerDealActive = IsDealerDealActive FROM TC_Deals_Dealers WITH(NOLOCK) WHERE DealerId = @BranchId
	SELECT TC_UsersRoleId FROM TC_UsersRole  WITH(NOLOCK)  WHERE UserId = @UserId AND RoleId IN (18)

	IF (@IsDealerDealActive = 1 AND @@ROWCOUNT > 0)
	BEGIN
		DECLARE @VINId INT, @ExistingOffer VARCHAR(500), @ExistingInteriorColor VARCHAR(50), @ExistingTermsConditions VARCHAR(500),
				@IsPriceChanged INT, @IsVINChanged INT, @IsSKUApproved BIT, @IsOfferColorChanged BIT;
	
		-- SKU is New/ Fresh stock add
		IF @DealsStockId IS NULL
			BEGIN
			-- Save SKU/ Add Stock
			INSERT INTO TC_Deals_Stock (BranchId, CarVersionId, MakeYear, VersionColorId, InteriorColor, EnteredOn, EnteredBy, 
											LastUpdatedOn, LastUpdatedBy, Offers, TermsConditions, isApproved)
			VALUES (@BranchId, @CarVersionId, @MakeYear, @VersionColorId, @InteriorColor, GETDATE(), @UserId, GETDATE(), @UserId, @Offers,@TermsAndConditions, 0)
			SET @RetVal = SCOPE_IDENTITY()

			INSERT INTO DCRM_Deals_ModelStatus (TC_ModelId) 
			SELECT ModelId FROM vwAllMMV WITH(NOLOCK)
			WHERE VersionId = @CarVersionId AND ApplicationId = 1

			-- Log the stock
			INSERT INTO TC_Deals_StockLog (TC_Deals_StockId, BranchId, CarVersionId, MakeYear, VersionColorId, InteriorColor, 
						LastUpdatedOn, LastUpdatedBy, Offers,TermsConditions, isApproved)
			VALUES (@RetVal, @BranchId, @CarVersionId, @MakeYear, @VersionColorId, @InteriorColor, GETDATE(), @UserId, @Offers,@TermsAndConditions,0)

			-- Insert VIN
			INSERT INTO TC_Deals_StockVIN (TC_Deals_StockId, VINNo, STATUS, LastRefreshedOn, LastRefreshedBy, EnteredOn, EnteredBy)
			SELECT @RetVal, UPPER(VINNo), 1, GETDATE(), @UserId, GETDATE(), @UserId FROM @VIN

			--Insert into VIN log    -- Modified by anchal on 26-08-2016
			--INSERT INTO TC_Deals_StockVINlog (TC_Deals_StockVINId,VINNo, TC_Deals_StockStatusId, ModifiedOn, ModifiedBy)
			--SELECT TC_DealsStockVINId,UPPER(VINNo),1,GETDATE(),@UserId FROM TC_Deals_StockVIN WITH (NOLOCK) WHERE VINNo IN (SELECT VINNo FROM @VIN)
			

			-- INSERT ON ROAD PRICES
			INSERT INTO TC_Deals_StockPrices (TC_Deals_StockId, CityId, DiscountedPrice, ActualOnroadPrice, EnteredOn, EnteredBy)
			SELECT @RetVal, CityId, OfferPrice, OnRoadPrice, GETDATE(), @UserId FROM @OnRoadPrices

			-- INSERT PRICELOG
			INSERT INTO TC_Deals_StockPriceslog (TC_Deals_StockId, CityId, DiscountedPrice, ActualOnroadPrice, LoggedOn, LoggedBy)
			SELECT @RetVal, CityId, OfferPrice, OnRoadPrice, GETDATE(), @UserId FROM @OnRoadPrices

			--Ruchira Patil on 12th May 2016 (update stockid for the @DealsStockPricesBreakupIds in TC_Deals_StockPricesBreakup)
			IF @DealsStockPricesBreakupIds IS NOT NULL
			BEGIN
				UPDATE TC_Deals_StockPricesBreakup 
				SET StockId = @RetVal 
				WHERE TC_Deals_StockPricesBreakupId IN (SELECT ListMember FROM fnSplitCSV(@DealsStockPricesBreakupIds))
			END
		END
		ELSE -- Existing SKU/ Edit Stock
			BEGIN
			-- Check id SKU is in approved or unapproved stage
			SELECT @IsSKUApproved = ISNULL(isApproved,0) FROM TC_Deals_Stock WITH (NOLOCK) WHERE ID = @DealsStockId
							
			-- Check for VIN, Prices, Offer and Interior color change
			-- Check for VIN Changes
			DECLARE @tblVINChanged TABLE( VINId INT,VINNo VARCHAR(20),StatusId TINYINT)
			DECLARE @tblPriceChanged TABLE( CityId INT,OnRoadPrice INT,OfferPrice INT)

			INSERT INTO @tblVINChanged(VINId,VINNo,StatusId)
			SELECT VINId,VINNo,StatusId FROM @VIN EXCEPT SELECT TC_DealsStockVINId, VINNo ,Status FROM TC_Deals_StockVIN WITH (NOLOCK) 
			WHERE TC_Deals_StockId = @DealsStockId
			 
			IF(@@ROWCOUNT > 0) -- VIN Changed
				SET @IsVINChanged  = 1
			
			-- Check for Prices Changes
			INSERT INTO @tblPriceChanged (CityId,OnRoadPrice,OfferPrice)
			SELECT CityId,OnRoadPrice,OfferPrice FROM @OnRoadPrices EXCEPT SELECT CityId, ActualOnroadPrice, DiscountedPrice 
			FROM TC_Deals_StockPrices WITH (NOLOCK) WHERE TC_Deals_StockId = @DealsStockId

			IF(@@ROWCOUNT > 0) -- Prices Changed
				SET @IsPriceChanged  = 1
			ELSE 
				BEGIN
					SELECT CityId, DiscountedPrice, ActualOnroadPrice FROM TC_Deals_StockPrices WITH (NOLOCK) WHERE TC_Deals_StockId = @DealsStockId 
					EXCEPT SELECT CityId, OfferPrice,OnRoadPrice FROM @OnRoadPrices
					IF(@@ROWCOUNT > 0) -- Prices Removed
						SET @IsPriceChanged  = 1
				END
				
			-- Check for Offer and interior color change	
			SELECT @ExistingOffer = Offers, @ExistingInteriorColor = InteriorColor ,@ExistingTermsConditions = TermsConditions FROM TC_Deals_Stock WITH (NOLOCK) WHERE ID = @DealsStockId
			IF @ExistingOffer <> @Offers OR @ExistingInteriorColor <> @InteriorColor OR @ExistingTermsConditions <> @TermsAndConditions
				SET @IsOfferColorChanged = 1	
							
			
			-- Update/Delete Data and change the status accordingly
			-- VIN Data Change
			IF @IsVINChanged = 1
				BEGIN
				
					DECLARE @VINStatus AS TINYINT
					IF @IsSKUApproved = 0
						SET @VINStatus = 1
					ELSE
						SET @VINStatus = 2

					UPDATE DS SET DS.VINNo = UPPER(TBL.VINNo), Status = TBL.StatusId
					FROM TC_Deals_StockVIN DS  WITH(NOLOCK) 
					INNER JOIN @tblVINChanged TBL ON TBL.VINId = DS.TC_DealsStockVINId
					WHERE DS.TC_Deals_StockId = @DealsStockId

					
					DECLARE @tblUnavailableVINS TABLE (RowId INT IDENTITY ,VINId INT)
					DECLARE @tblUnavailableVINSCNT INT = 0
					DECLARE @CurrentRow INT = 1

					INSERT INTO @tblUnavailableVINS (VINId)
					SELECT VINId 
					FROM @tblVINChanged
					WHERE StatusId = 12
					
					SET @tblUnavailableVINSCNT = @@ROWCOUNT					
					DECLARE @UnavailableVINID INT

					WHILE(@CurrentRow <= @tblUnavailableVINSCNT)
						BEGIN
							SELECT @UnavailableVINID = VINId FROM @tblUnavailableVINS WHERE RowId = @CurrentRow
							EXEC TC_Deals_ChangeVINStatus @DealsStockId,@UnavailableVINID,@VINStatus,@UserId,NULL
							
							--INSERT INTO TC_Deals_StockVINlog (TC_Deals_StockVINId,VINNo, TC_Deals_StockStatusId, ModifiedOn, ModifiedBy)
							--SELECT @UnavailableVINID,UPPER(VINNo),@VINStatus,GETDATE(),@UserId FROM @tblVINChanged 
							--WHERE VINId = @UnavailableVINID  -- Modified by anchal on 26-08-2016

							SET @CurrentRow = @CurrentRow + 1
						END	
						
						-- Insert new data where VINId is 0
					INSERT INTO TC_Deals_StockVIN (TC_Deals_StockId, VINNo, Status, LastRefreshedOn, LastRefreshedBy, EnteredOn, EnteredBy)
					SELECT @DealsStockId, UPPER(VINNo), @VINStatus , GETDATE(), @UserId, GETDATE(), @UserId FROM @VIN WHERE VINId = 0
					
					-- Log the VIN Data    -- Modified by anchal on 26-08-2016
					--INSERT INTO TC_Deals_StockVINlog (TC_Deals_StockVINId,VINNo, TC_Deals_StockStatusId, ModifiedOn, ModifiedBy)
					--SELECT TC_DealsStockVINId,UPPER(VINNo),Status,GETDATE(),@UserId FROM TC_Deals_StockVIN SV WITH (NOLOCK) WHERE Status IN(1,2) 
					--AND VINNo IN (SELECT VINNo FROM @VIN)
							
				END
			
			-- Prices Change
			IF @IsPriceChanged  = 1
				BEGIN
					-- Update the existing Price
					UPDATE SP SET SP.ActualOnroadPrice = P.OnRoadPrice , SP.DiscountedPrice = P.OfferPrice
					FROM TC_Deals_StockPrices SP  WITH(NOLOCK)  INNER JOIN @OnRoadPrices P ON SP.CityId = P.CityId
					WHERE SP.TC_Deals_StockId = @DealsStockId
					
					-- Insert new city prices
					INSERT INTO TC_Deals_StockPrices (TC_Deals_StockId, CityId, DiscountedPrice, ActualOnroadPrice, EnteredBy,EnteredOn)
					SELECT DISTINCT @DealsStockId, TBL.CityId,  TBL.OfferPrice,  TBL.OnRoadPrice, @UserId, GETDATE() FROM @OnRoadPrices TBL 
					WHERE CityId NOT IN( SELECT DISTINCT CityId FROM TC_Deals_StockPrices WITH (NOLOCK) WHERE TC_Deals_StockId = @DealsStockId)
					
					-- Delete the pricesd
					DELETE FROM TC_Deals_StockPrices WHERE TC_Deals_StockId = @DealsStockId
					AND CityId NOT IN ( SELECT DISTINCT TBL.CityId FROM @OnRoadPrices TBL)

					INSERT INTO TC_Deals_StockPriceslog (TC_Deals_StockId, CityId, DiscountedPrice, ActualOnroadPrice, LoggedOn, LoggedBy)
					SELECT @DealsStockId, CityId, OfferPrice, OnRoadPrice, GETDATE(), @UserId FROM @tblPriceChanged					
				
				END
			
			-- If SKU is Unapproved, it can be changed
			IF @IsSKUApproved = 0 
				BEGIN
					-- Update SKU Data
					UPDATE TC_Deals_Stock
					SET BranchId = @BranchId, CarVersionId = @CarVersionId, MakeYear = @MakeYear, VersionColorId = @VersionColorId, 
						InteriorColor = @InteriorColor, EnteredBy = @UserId, LastUpdatedOn = GETDATE(), LastUpdatedBy = @UserId, 
						Offers = @Offers , TermsConditions = @TermsAndConditions --, isApproved = 0
					WHERE Id = @DealsStockId

					-- Log the changed Data
					INSERT INTO TC_Deals_StockLog (TC_Deals_StockId, BranchId, CarVersionId, MakeYear, VersionColorId, InteriorColor, 
								LastUpdatedOn, LastUpdatedBy, Offers,TermsConditions, isApproved)
					VALUES (@DealsStockId, @BranchId, @CarVersionId, @MakeYear, @VersionColorId, @InteriorColor, GETDATE(), @UserId, @Offers,@TermsAndConditions,0)
				
					-- Update VIN status to unapproved
					EXEC TC_Deals_ChangeVINStatus @DealsStockId,NULL,1,@UserId,NULL
					
					--INSERT INTO TC_Deals_StockVINlog (TC_Deals_StockVINId,VINNo, TC_Deals_StockStatusId, ModifiedOn, ModifiedBy)
					--SELECT TC_DealsStockVINId,UPPER(VINNo),1,GETDATE(),@UserId FROM TC_Deals_StockVIN SV WITH (NOLOCK) 
					--WHERE VINNo IN (SELECT VINNo FROM @VIN)       -- Modified by anchal on 26-08-2016
										
				END
			ELSE IF @IsSKUApproved = 1 -- If approved can't be changed
				BEGIN
					-- If there is change in prices/offer/interior color
					IF @IsPriceChanged  = 1 OR @IsOfferColorChanged = 1
					BEGIN

						-- UPDATE OFFER DATA
						UPDATE TC_Deals_Stock
						SET InteriorColor = @InteriorColor, EnteredBy = @UserId, LastUpdatedOn = GETDATE(), LastUpdatedBy = @UserId, 
							Offers = @Offers , TermsConditions = @TermsAndConditions
						WHERE Id = @DealsStockId
						
						-- Log the SKU Data
						INSERT INTO TC_Deals_StockLog (TC_Deals_StockId, BranchId, CarVersionId, MakeYear, VersionColorId, InteriorColor, 
								LastUpdatedOn, LastUpdatedBy, Offers, TermsConditions, isApproved)
						VALUES (@DealsStockId, @BranchId, @CarVersionId, @MakeYear, @VersionColorId, @InteriorColor, GETDATE(), @UserId, @Offers,@TermsAndConditions,0)
						
						-- Update VIN status to unapproved and Send SKU in unapproved stage
						EXEC TC_Deals_ChangeVINStatus @DealsStockId,NULL,1,@UserId,NULL
					
						--INSERT INTO TC_Deals_StockVINlog (TC_Deals_StockVINId,VINNo, TC_Deals_StockStatusId, ModifiedOn, ModifiedBy)
						--SELECT TC_DealsStockVINId,UPPER(VINNo),1,GETDATE(),@UserId FROM TC_Deals_StockVIN SV WITH (NOLOCK) 
						--WHERE VINNo IN (SELECT VINNo FROM @VIN)       -- Modified by anchal on 26-08-2016
						
					END
				END	
			SET @RetVal = -1
		END
	END
	ELSE 
		SET @RetVal = -2
END
