CREATE TABLE [dbo].[Subscription] (
    [SubscriptionID]       INT           IDENTITY (1, 1) NOT NULL,
    [EmailAddress]         VARCHAR (255) NOT NULL,
    [SubscriptionCategory] TINYINT       NULL,
    [SubscriptionType]     BIGINT        NULL,
    [Frequency]            VARCHAR (255) NULL,
    [SubscriptionDate]     DATE          NULL,
    PRIMARY KEY CLUSTERED ([SubscriptionID] ASC)
);

