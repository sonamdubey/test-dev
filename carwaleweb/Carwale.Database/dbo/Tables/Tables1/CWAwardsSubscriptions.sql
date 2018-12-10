CREATE TABLE [dbo].[CWAwardsSubscriptions] (
    [Id]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [Email]     VARCHAR (50) NULL,
    [CreatedOn] DATETIME     CONSTRAINT [DF_CWAwardsSubscriptions_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CWAwardsSubscriptions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

