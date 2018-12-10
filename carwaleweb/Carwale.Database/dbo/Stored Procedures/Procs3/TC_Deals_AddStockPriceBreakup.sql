IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_AddStockPriceBreakup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_AddStockPriceBreakup]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 11th May 2016
-- Description:	To add stock prices break up
-- Modified By : Nilima More On 17th May 2016, 
-- Description: if ActualOnroadPrice and @OnRoadPrice are different then make SKU in unapproved stage,and log respective data.
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_AddStockPriceBreakup]
	@StockId INT = NULL,
	@PQComponents [TC_PQ_Components] READONLY,
	@CityId INT,
	@UserId INT,
	@OnRoadPrice INT,
	@TC_Deals_StockPricesBreakupId VARCHAR(500) = NULL OUTPUT
	
AS
BEGIN
	DECLARE @ActualOnroadPrice INT = NULL
	IF(@StockId IS NULL) -- Insert
	BEGIN
		DECLARE @IDs TABLE(ID INT)
		INSERT INTO TC_Deals_StockPricesBreakup(CityId,TC_PQComponentId,ComponentPrice,CreatedBy)
		OUTPUT inserted.TC_Deals_StockPricesBreakupId INTO @IDs(ID)
		SELECT @CityId,ComponentId,ComponentPrice,@UserId 
		FROM @PQComponents

		SELECT @TC_Deals_StockPricesBreakupId =  COALESCE(@TC_Deals_StockPricesBreakupId + ',' , '') + CAST (ID AS VARCHAR) 
		FROM @IDs

	END
	ELSE -- Update
	BEGIN

		-- Modified By : Nilima More On 17th May 2016, if ActualOnroadPrice and @OnRoadPrice are different then make SKU in unapproved stage,and log respective data.
		SELECT @ActualOnroadPrice = ActualOnroadPrice FROM TC_Deals_StockPrices WITH (NOLOCK) WHERE TC_Deals_StockId = @StockId
		IF @ActualOnroadPrice <> @OnRoadPrice
		BEGIN
			-- Update VIN status to unapproved and Send SKU in unapproved stage
			EXEC TC_Deals_ChangeVINStatus @StockId,NULL,1,@UserId,NULL

			--Log the vinstatus
			INSERT INTO TC_Deals_StockVINlog (TC_Deals_StockVINId,VINNo, TC_Deals_StockStatusId, ModifiedOn, ModifiedBy)
							SELECT TC_DealsStockVINId,UPPER(VINNo),1,GETDATE(),@UserId FROM TC_Deals_StockVIN SV WITH (NOLOCK) 
							WHERE VINNo IN (SELECT VINNo FROM TC_Deals_StockVIN WITH (NOLOCK) WHERE TC_Deals_StockId = @StockId)
			-- Log the SKU Data
			INSERT INTO TC_Deals_StockLog (TC_Deals_StockId, BranchId, CarVersionId, MakeYear, VersionColorId, InteriorColor, 
					LastUpdatedOn, LastUpdatedBy, Offers, isApproved)
			SELECT @StockId,BranchId,CarVersionId,MakeYear,VersionColorId,InteriorColor,GETDATE(),@UserId,Offers,0 FROM TC_Deals_Stock WITH (NOLOCK) WHERE ID = @StockId
		END

		--MOodified By Ruchira Patil on 19th MAY 2016(Called SP to delete and log data)
		DECLARE @City VARCHAR(500) = CONVERT(VARCHAR(500),@CityId)
		EXEC TC_Deals_DeleteStockPriceBreakUp @StockId,@City

		INSERT INTO TC_Deals_StockPricesBreakup(CityId,StockId,TC_PQComponentId,ComponentPrice,CreatedBy)		
		OUTPUT inserted.TC_Deals_StockPricesBreakupId INTO @IDs(ID)
		SELECT @CityId,@StockId,ComponentId,ComponentPrice,@UserId 
		FROM @PQComponents

		SELECT @TC_Deals_StockPricesBreakupId =  COALESCE(@TC_Deals_StockPricesBreakupId + ',' , '') + CAST (ID AS VARCHAR) 
		FROM @IDs

	END
END
---------------------------------------------------------------------------------------------------------------------------------------
