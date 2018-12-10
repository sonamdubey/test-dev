IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_CancelDuplicateCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_CancelDuplicateCar]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 7th Sept 2015
-- Description:	cancel the car for which the registration number matches with the existing inspected car
-- Modified By: Tejashree Patil on 2nd Oct 2015, auto linking stock when the car is added through panel
-- Modified By: Ruchira Patil on 19th Oct (passed dealerid to Absure_SaveStockRegNumberMapping)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_CancelDuplicateCar] 
	@CarId	INT,
	@UserId	INT,
	@RegNum VARCHAR(50)
AS
BEGIN
	/*Cancel car*/
	DECLARE @CancelledReason VARCHAR(100)

	SELECT	@CancelledReason = Reason 
	FROM	AbSure_ReqCancellationReason WITH(NOLOCK) 
	WHERE	Id = 7 -- Cancellation reason of duplicate car

	UPDATE	AbSure_CarDetails 
	SET		IsCancelled = 1, Status = 3, CancelledOn = GETDATE(),
			CancelReason = @CancelledReason, CancelledBy = @UserId,
			RegNumber = @RegNum -- update registration number when the car is added through add stock wherein the dealer dosn't enter proper regNum
	WHERE	Id = @CarId

	/*Automatic linking of car with stock*/
	DECLARE @StockId INT, @RegNumber VARCHAR(50),@DealerId INT
	SELECT	@StockId = CD.StockId, @RegNumber = CD.RegNumber,@DealerId = CD.DealerId
	FROM	AbSure_CarDetails CD WITH(NOLOCK) 
	WHERE	Id = @CarId

	IF (@StockId IS NOT NULL)
	BEGIN
		EXECUTE Absure_SaveStockRegNumberMapping @StockId, @RegNumber, @CarId ,@DealerId,1    --- 1 is used to show the Autolinking of car
	END
END
