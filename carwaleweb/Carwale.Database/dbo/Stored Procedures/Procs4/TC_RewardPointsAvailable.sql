IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RewardPointsAvailable]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RewardPointsAvailable]
GO

	-- =============================================
-- Author:		<Vivek,Gupta>
-- Create date: <13-05-2015,>
-- Description:	<Returns Total Availbale Reward Points Of a dealer,>
---Modified By Vivek Gupta on 28-09-2015, added conditions to make points invalid for free dealers
-- Modified By Vivek GUpta, 0n 09-10-2015, commented check of non paid dealers
-- Modified By Vivek Gupta on 06-11-2015, added @Userid parameter to calculater points user wise(not dealer wise)
-- =============================================
CREATE PROCEDURE [dbo].[TC_RewardPointsAvailable]
@BranchId INT,
@UserId INT,
@TotalRewards VARCHAR(10) = 0 OUTPUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
		DECLARE @DealerTypeId TINYINT
		SET @DealerTypeId = (SELECT TC_DealerTypeId FROM Dealers WITH(NOLOCK) WHERE Id = @BranchId)

		--IF (@DealerTypeId = 1)
		--BEGIN
		--	IF NOT EXISTS 
		--		(    SELECT ConsumerId 
		--			 FROM ConsumerCreditPoints WITH(NOLOCK) 
		--			 WHERE ConsumerType = 1 AND PackageType <> 28 
		--			    AND CONVERT(DATE,ExpiryDate) >= CONVERT(DATE,GETDATE())
		--			    AND ConsumerId = @BranchId					 
		--		)  
		--		BEGIN
		--			UPDATE TC_DealerDailyRewardPoints SET IsValid = 0 WHERE DealerId = @BranchId AND CONVERT(DATE,EntryDate) < CONVERT(DATE,GETDATE() - 30) AND ISNULL(IsValid,1) = 1
		--		END
		--END
		--ELSE IF @DealerTypeId = 2
		--BEGIN
		--	IF NOT EXISTS
		--	(
		--		SELECT DealerId 
		--		FROM RVN_DealerPackageFeatures WITH(NOLOCK)
		--		WHERE CONVERT(DATE,PackageEndDate) >= CONVERT(DATE,GETDATE()) 
		--		AND DealerId = @BranchId
		--		UNION
		--		SELECT Id AS DealerId FROm Dealers WITH(NOLOCK)
		--		WHERE Id = @BranchId AND ISNULL(PaidDealer,0) = 1 AND ISNULL(ApplicationId,1) = 2
		--	)
		--	BEGIN
		--		UPDATE TC_DealerDailyRewardPoints SET IsValid = 0 WHERE DealerId = @BranchId AND CONVERT(DATE,EntryDate) < CONVERT(DATE,GETDATE() - 30) AND ISNULL(IsValid,1) = 1
		--	END
		--END		

		--ELSE IF @DealerTypeId = 3
		--BEGIN
		--	IF NOT EXISTS 
		--	(
		--			SELECT ConsumerId 
		--			FROM ConsumerCreditPoints  WITH(NOLOCK)
		--			WHERE ConsumerType = 1 AND PackageType <> 28 
		--			    AND CONVERT(DATE,ExpiryDate) >= CONVERT(DATE,GETDATE())
		--			    AND ConsumerId = @BranchId
		--			UNION
		--			SELECT DealerId AS ConsumerId
		--			FROM RVN_DealerPackageFeatures WITH(NOLOCK)
		--			WHERE CONVERT(DATE,PackageEndDate) >= CONVERT(DATE,GETDATE()) 
		--			AND DealerId = @BranchId
		--	)
		--	BEGIN
		--		UPDATE TC_DealerDailyRewardPoints SET IsValid = 0 WHERE DealerId = @BranchId AND CONVERT(DATE,EntryDate) < CONVERT(DATE,GETDATE() - 30) AND ISNULL(IsValid,1) = 1
		--	END
		--END

		--IF EXISTS (SELECT Id FROM Dealers WITH(NOLOCK) WHERE Id = @BranchId AND ISNULL(PaidDealer,0) = 0)
		--BEGIN	
		--	UPDATE TC_DealerDailyRewardPoints SET IsValid = 0 WHERE DealerId = @BranchId AND CONVERT(DATE,EntryDate) < CONVERT(DATE,GETDATE() - 30) AND ISNULL(IsValid,1) = 1
		--END




		DECLARE @TotalRewardPoints NUMERIC = 0
		DECLARE @TotalRedeemedPoints NUMERIC = 0

		SET @TotalRewardPoints =	
			(SELECT SUM(ISNULL(DRP.TotalRewardsFromWeb,0)) + SUM(ISNULL(DRP.TotalRewardsFromApp,0)) + SUM(ISNULL(DRP.TotalRewardsToSM,0))
			 FROM TC_DealerDailyRewardPoints DRP WITH(NOLOCK)			
			 WHERE DRP.DealerId = @BranchId  --AND ISNULL(IsValid,1) = 1
			 AND  DRP.UserId = @UserId
			 )

		SET @TotalRedeemedPoints =
			(SELECT SUM(ISNULL(RP.RedeemedPoints,0))
			 FROM TC_RedeemedPoints RP WITH(NOLOCK)
			 WHERE DealerId = @BranchId
			 AND RP.UserId = @UserId)
			
		SET @TotalRewards = ISNULL(@TotalRewardPoints,0) - ISNULL(@TotalRedeemedPoints,0)

		SET @TotalRewards = replace(convert(varchar,cast(@TotalRewards as money),1), '.00','')
END
--------------------------------------------------------------------------------------------------------

