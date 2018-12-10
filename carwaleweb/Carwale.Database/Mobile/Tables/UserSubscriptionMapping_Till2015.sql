CREATE TABLE [Mobile].[UserSubscriptionMapping_Till2015] (
    [Id]            INT      NOT NULL,
    [MobileUserId]  INT      NOT NULL,
    [SubsMasterId]  INT      NOT NULL,
    [IsActive]      BIT      NOT NULL,
    [CreatedOn]     DATETIME NULL,
    [LastUpdatedOn] DATETIME NOT NULL
);

