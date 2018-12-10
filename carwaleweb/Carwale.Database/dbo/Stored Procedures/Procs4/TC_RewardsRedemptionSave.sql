IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RewardsRedemptionSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RewardsRedemptionSave]
GO

	
-- =============================================
-- Author:		<Vivek,,Gupta>
-- Create date: <Create Date,14-05-2015,>
-- Description:	 save reward Point Redemption  
-- Modified  By : Vinay kumar prajapati  Impliment PayUMoney  ( Added @RedemptionId  which is  Tc_EwalletId) 
-- Modified By: Tejashree Patil on 18 Feb 2016 at 5.22 pm to save RedeemedAmount and points.
-- Modified By: Tejashree Patil on 17 March 2016 at 5.22 pm to save Redeemedpoints.
-- =============================================
CREATE PROCEDURE [dbo].[TC_RewardsRedemptionSave]
	@DealerId INT,
	@Denomination INT,
	@Quantity SMALLINT,
	@Description VARCHAR(50),
	@EmailTo VARCHAR(50),
	@Status TINYINT = 0 OUTPUT,
	@UserId INT,
	@RedemptionId Int = NULL, -- 2 for PayUMoney , 1 for FlipKartVooucher
	@RedemptionAmount INT =NULL, -- For PayUMoney 
	@RedemptionPoint NUMERIC(18,0) =NULL,
	@RewardPointFactor FLOAT = 0.70
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @TotalPoints VARCHAR(10) = 0
	DECLARE @RedeemedPoints NUMERIC= 0
	DECLARE @TotalAvailablePoints NUMERIC

	IF EXISTS (SELECT Id FROM Dealers WITH(NOLOCK) WHERE Id=@DealerId AND ISNULL(PaidDealer,0) <> 1)
	BEGIN
		SET @Status = 2 -- Un paid dealer can not reddem points
		return;
	END

	EXEC TC_RewardPointsAvailable @BranchId = @DealerId, @UserId = @UserId,@TotalRewards = @TotalPoints OUTPUT
	
	IF(@Denomination IS NULL)
	SET @RedemptionId = ISNULL(@RedemptionId,2)

	IF @RedemptionId = 2  -- For PayUMoney
	BEGIN
		SET @RedeemedPoints = @RedemptionAmount
	END
	ELSE
	BEGIN
		SET @RedeemedPoints = ISNULL(@Denomination,0) * ISNULL(@Quantity,0)
	END

	IF(@RedemptionAmount IS NULL)
	BEGIN
		IF(@RedeemedPoints IS NULL)
			SET @RedeemedPoints = ISNULL(@Denomination,0) * ISNULL(@Quantity,0)
		SET @RedemptionAmount = ROUND(@RedeemedPoints / @RewardPointFactor, 0)
	END

	-- Modified By: Tejashree Patil on 17 March 2016 at 5.22 pm to save Redeemedpoints.
	IF(@RedemptionPoint IS NULL)
	BEGIN
		IF(@RedeemedPoints IS NULL)
			SET @RedeemedPoints = ISNULL(@Denomination,0) * ISNULL(@Quantity,0)
		SET @RedemptionPoint = ROUND(@RedeemedPoints / @RewardPointFactor, 0)
	END

	SET @Status = 0
	SET @TotalAvailablePoints = REPLACE(@TotalPoints,',','')

	IF(((ISNULL(@TotalAvailablePoints,0) - ISNULL(@RedeemedPoints,1)) > = 0) AND (ISNULL(@RedeemedPoints,0) >= 0 AND ISNULL(@RedemptionAmount,0) >= 0 ))
		BEGIN        
		
			INSERT INTO TC_RedeemedPoints
			(
				DealerId,
				RedeemDate,
				Denomination,
				Quantity,
				RedeemedPoints,
				Description,
				EmailSentOn,
				UserId,
				TC_EWalletsId,
				RedeemedAmount
			)

			VALUES

			(
				@DealerId,
				GETDATE(),
				@Denomination,
				@Quantity,
				@RedemptionPoint,
				@Description,
				@EmailTo,
				@UserId,
				@RedemptionId,
				@RedeemedPoints
			)

			IF @@ROWCOUNT > 0
				SET @Status = 1

		END
		
		IF(@RedemptionPoint IS NULL OR @RedeemedPoints IS NULL)
		INSERT INTO TC_Exceptions (Programme_Name,TC_Exception,TC_Exception_Date, InputParameters)
			VALUES('TC_RewardsRedemptionSave', ('ERROR_NUMBER(): NULL'), GETDATE(),		  
		' @DealerId:' + CAST(ISNULL(@DealerId,'NULL') AS VARCHAR(50)) + 
		' @Denomination:' + CAST(ISNULL(@Denomination,'NULL') AS VARCHAR(50)) +
		' @Description:' + CAST(ISNULL(@Description,'NULL') AS VARCHAR(50)) +
		' @EmailTo:' + CAST(ISNULL(@EmailTo,'NULL') AS VARCHAR(50)) +
		' @Status:' + CAST(ISNULL(@Status,'NULL') AS VARCHAR(50)) +
		' @UserId:' + CAST(ISNULL(@UserId,'NULL') AS VARCHAR(50)) +
		' @RedemptionId:' + CAST(ISNULL(@RedemptionId,'NULL') AS VARCHAR(50)) +
		' @RedemptionAmount:' + CAST(ISNULL(@RedemptionAmount,'NULL') AS VARCHAR(50)) +
		' @RedemptionPoint:' + CAST(ISNULL(CONVERT(VARCHAR,@RedemptionPoint),'NULL') AS VARCHAR(50)) +
		' @RedeemedPoints:' + CAST(ISNULL(CONVERT(VARCHAR,@RedeemedPoints),'NULL') AS VARCHAR(50)) +
		' @RewardPointFactor:' + CAST(ISNULL(CONVERT(VARCHAR,@RewardPointFactor),'NULL') AS VARCHAR(50)))
		
END


