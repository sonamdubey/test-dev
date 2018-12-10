IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertRewardPoints]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertRewardPoints]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 28-04-2015
-- Description:	Inserting Reward points into the table TC_DealerDailyRewardPoints
-- Modified By Vivek Gupta on 24-08-2015, adedd isValid
-- Modified By Vivek Gupta on 06-11-2015, added @Userid parameter to calculater points user wise(not dealer wise)
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsertRewardPoints]
@DealerId INT,
@EntryDate DATETIME,
@TC_DealerTypeId INT,
@TC_RewardPointsId INT,
@RewardPoints NUMERIC,
@TotalRewardsFromWeb NUMERIC,
@TotalRewardsFromApp NUMERIC,
@TotalRewardsToSM NUMERIC = 0,
@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@TotalRewardsFromWeb <> 0 OR @TotalRewardsFromApp <> 0 OR @TotalRewardsToSM <> 0)
	BEGIN
			
			
			IF NOT EXISTS (SELECT Id FROM TC_DealerDailyRewardPoints WITH(NOLOCK) 
							WHERE CONVERT(date,Entrydate) = CONVERT(DATE,@EntryDate) 
							AND DealerId = @DealerId AND UserId = @UserId
							AND TC_RewardPointsId = @TC_RewardPointsId)
			BEGIN
					
					INSERT INTO TC_DealerDailyRewardPoints
						(
						DealerId,
						EntryDate,
						TC_DealerTypeId,
						TC_RewardPointsId,
						RewardPoints,
						TotalRewardsFromWeb,
						TotalRewardsFromApp,
						TotalRewardsToSM,	
						IsValid,
						UserId
						)
					VALUES
						(
						@DealerId,
						@EntryDate,
						@TC_DealerTypeId,
						@TC_RewardPointsId,
						@RewardPoints,
						@TotalRewardsFromWeb,
						@TotalRewardsFromApp,
						@TotalRewardsToSM,						
						1,
						@UserId
						)
			END
    END
END



-------------------------------------------------------------------------------------------------
