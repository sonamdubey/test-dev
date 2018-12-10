IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_LoadSellWarrantyDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_LoadSellWarrantyDetails]
GO

	-- =============================================
-- Modified By : Ashwini Dhamankar on Oct 28,2015  (Called Function Absure_GetMasterCarId only if car is duplicate)
-- Modified By : Chetan Navin on Feb 26,2016 (Added to fetch details of stock for cartrade certified cars)
-- =============================================

CREATE PROCEDURE [dbo].[AbSure_LoadSellWarrantyDetails] 
@AbSure_CarDetailsId INT = NULL,
@StockId INT = NULL
AS
BEGIN
	DECLARE @RegNumber VARCHAR(50),@Status INT,@CancelReason VARCHAR(250),@CancelledReason VARCHAR(250)
	SELECT @CancelledReason = Reason FROM AbSure_ReqCancellationReason WITH(NOLOCK) WHERE Id = 7;

	SELECT	@RegNumber = RegNumber,
			@Status = ACD.Status, 
			@CancelReason = ACD.CancelReason
	FROM	AbSure_CarDetails ACD WITH(NOLOCK)
	WHERE	ACD.ID = @AbSure_CarDetailsId
	
	IF(@Status = 3 AND @CancelReason = @CancelledReason)
	BEGIN
		SELECT @AbSure_CarDetailsId = dbo.Absure_GetMasterCarId(@RegNumber,@AbSure_CarDetailsId) 
	END 

	IF(@StockId IS NULL)
	BEGIN
		SELECT	CD.Model,CONVERT(VARCHAR(15),CD.MakeYear, 106) AS MakeYear,
				CONVERT(VARCHAR(15),CD.RegistrationDate,106) AS RegistrationDate,CD.RegNumber,CD.Kilometer,CD.FinalWarrantyTypeId, W.Warranty
		FROM	AbSure_CarDetails CD WITH(NOLOCK)
				INNER JOIN AbSure_WarrantyTypes W WITH(NOLOCK) ON W.AbSure_WarrantyTypesId = CD.FinalWarrantyTypeId
		WHERE	Id = @AbSure_CarDetailsId
	END
	ELSE
	BEGIN
		SELECT	Model,CONVERT(VARCHAR(5),MfgMonth) + ' ' + CONVERT(VARCHAR(5),MfgYear) AS MakeYear,
				CONVERT(VARCHAR(15),RegDate,106) AS RegistrationDate,RegNum RegNumber,Mileage Kilometer,'1' FinalWarrantyTypeId,'' Warranty
		FROM	TC_CarTradeCertificationData TC WITH(NOLOCK)
		INNER JOIN TC_CarTradeCertificationLiveListing TL WITH(NOLOCK) ON TL.ListingId = TC.ListingId
		WHERE	TC.ListingId = @StockId
	END
END
-------------------------------------------------------------------------------------------------------------------
