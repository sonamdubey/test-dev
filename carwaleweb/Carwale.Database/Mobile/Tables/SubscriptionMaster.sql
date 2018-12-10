CREATE TABLE [Mobile].[SubscriptionMaster] (
    [SubsMasterId]     INT          IDENTITY (1, 1) NOT NULL,
    [SubscriptionName] VARCHAR (50) NULL,
    [CreatedOn]        DATETIME     CONSTRAINT [DF_SubscriptionMaster_CreatedOn] DEFAULT (getdate()) NULL,
    [LastUpdatedOn]    DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([SubsMasterId] ASC)
);

