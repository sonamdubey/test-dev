CREATE TABLE [dbo].[UsedPremiumSlotCount] (
    [ID]     INT          IDENTITY (1, 1) NOT NULL,
    [CityId] NUMERIC (18) NOT NULL,
    [Count]  INT          NULL,
    PRIMARY KEY CLUSTERED ([CityId] ASC)
);

