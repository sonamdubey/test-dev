CREATE TABLE [dbo].[TC_DealerDailyRewardPointsBW310816_bkp] (
    [Id]                  INT          NOT NULL,
    [DealerId]            INT          NULL,
    [EntryDate]           DATETIME     NULL,
    [TC_DealerTypeId]     SMALLINT     NULL,
    [TC_RewardPointsId]   INT          NULL,
    [RewardPoints]        NUMERIC (18) NULL,
    [TotalRewardsFromWeb] NUMERIC (18) NULL,
    [TotalRewardsFromApp] NUMERIC (18) NULL,
    [TotalRewardsToSM]    NUMERIC (18) NULL,
    [IsValid]             BIT          NULL,
    [UserId]              INT          NULL
);

