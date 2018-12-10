CREATE TABLE [Mobile].[UserSubscriptionMapping] (
    [Id]            INT      IDENTITY (1, 1) NOT NULL,
    [MobileUserId]  INT      NOT NULL,
    [SubsMasterId]  INT      NOT NULL,
    [IsActive]      BIT      CONSTRAINT [DF_UserSubscriptionMapping_IsActive_1] DEFAULT ((1)) NOT NULL,
    [CreatedOn]     DATETIME NULL,
    [LastUpdatedOn] DATETIME CONSTRAINT [DF_UserSubscriptionMapping_LastUpdatedOn_1] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_UserSubscriptionMapping_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_Mobile_SubsMasterId]
    ON [Mobile].[UserSubscriptionMapping]([SubsMasterId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserSubscriptionMapping_MobileUserId_SubsMasterId]
    ON [Mobile].[UserSubscriptionMapping]([MobileUserId] ASC, [SubsMasterId] ASC);

