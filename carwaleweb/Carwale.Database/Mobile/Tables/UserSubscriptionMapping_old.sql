CREATE TABLE [Mobile].[UserSubscriptionMapping_old] (
    [Id]            INT      IDENTITY (1, 1) NOT NULL,
    [MobileUserId]  INT      NOT NULL,
    [SubsMasterId]  INT      NOT NULL,
    [IsActive]      BIT      CONSTRAINT [DF_UserSubscriptionMapping_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedOn]     DATETIME NULL,
    [LastUpdatedOn] DATETIME CONSTRAINT [DF_UserSubscriptionMapping_LastUpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_UserSubscriptionMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_Mobile_SubsMasterId]
    ON [Mobile].[UserSubscriptionMapping_old]([SubsMasterId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserSubscriptionMapping_MobileUserId_SubsMasterId]
    ON [Mobile].[UserSubscriptionMapping_old]([MobileUserId] ASC, [SubsMasterId] ASC);

