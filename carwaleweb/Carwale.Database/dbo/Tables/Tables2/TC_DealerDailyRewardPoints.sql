CREATE TABLE [dbo].[TC_DealerDailyRewardPoints] (
    [Id]                  INT          IDENTITY (1, 1) NOT NULL,
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


GO
CREATE NONCLUSTERED INDEX [IX_TC_DealerDailyRewardPoints_DealerId]
    ON [dbo].[TC_DealerDailyRewardPoints]([DealerId] ASC, [UserId] ASC)
    INCLUDE([TC_DealerTypeId], [TotalRewardsFromWeb], [TotalRewardsFromApp], [TotalRewardsToSM]);

