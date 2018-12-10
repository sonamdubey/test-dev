IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Dealer_CheckPackageValidity_V_16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Dealer_CheckPackageValidity_V_16_11_1]
GO

	--THIS PROCEDURE INSERTS THE VALUES FOR THE Cities
-- Modified by Kirtan on 19-11-2014
-- Modified by Vicky Gupta on 27-07-2015
CREATE PROCEDURE Dealer_CheckPackageValidity_V_16_11_1 @ConsumerType NUMERIC
	,@ConsumerId NUMERIC
	,@PackageType NUMERIC OUTPUT
	,@ExpiryDate DATETIME OUTPUT
	,@Points NUMERIC OUTPUT
	,@CarsListed NUMERIC OUTPUT
	,@Status BIT OUTPUT
	,@ValueOffered FLOAT = NULL OUTPUT
AS
BEGIN
	--Get the package details for the give consumer & consumer type
	SELECT TOP 1 @PackageType = CCP.PackageType
		,@ExpiryDate = CCP.ExpiryDate
		,@Points = CCP.Points
	FROM ConsumerCreditPoints CCP
	WHERE CCP.ConsumerId = @ConsumerId
		AND CCP.ConsumerType = @ConsumerType
	ORDER BY CCP.ExpiryDate DESC
		----If any package exist
		--IF @@ROWCOUNT > 0
		--BEGIN
		--	IF (
		--			@ExpiryDate > GETDATE()
		--			OR (
		--				DAY(@ExpiryDate) = DAY(GETDATE())
		--				AND MONTH(@ExpiryDate) = MONTH(GETDATE())
		--				AND YEAR(@ExpiryDate) = YEAR(GETDATE())
		--				)
		--			) -- if package is not yet expired
		--	BEGIN
		--		-- Get total cars listed against this package by that dealer
		--		SELECT @CarsListed = COUNT(SI.ID)
		--		FROM SellInquiries AS SI
		--			,LiveListings AS LL WITH (NOLOCK) -- NOLOCK added by Kirtan on 19-11-2014
		--		WHERE SI.ID = LL.Inquiryid
		--			AND LL.SellerType = @ConsumerType
		--			AND SI.DealerId = @ConsumerId
		--		IF @@ROWCOUNT > 0 -- If cars listed
		--		BEGIN
		--			IF @CarsListed < @Points --  If listed cars count is less than given Inquiry points
		--				SET @Status = 1 -- Can list cars
		--			ELSE
		--				SET @Status = 0 -- Quota full (Can't list a car)
		--		END
		--		ELSE
		--			SET @Status = 1 -- No cars listed yet (Can list cars)
		--	END
		--	ELSE
		--		SET @Status = 0 -- Existing Package expired (Can't list a car)
		--END
		--ELSE
		--	SET @Status = 0 -- Never took any package(Can't list a car)
		----------------------------------------------
		-----Fetch dealership value
		--SELECT TOP 1 @ValueOffered = ISNULL(ValueOffered, 0)
		--FROM PaidSellerScore WITH (NOLOCK)
		--WHERE DealerId = @ConsumerId
END

