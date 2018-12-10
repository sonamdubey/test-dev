CREATE TABLE [dbo].[CH_FollowupActionTime] (
    [Id]                           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FollowupActionTimeCategoryId] NUMERIC (18) NOT NULL,
    [FollowupActionTime]           VARCHAR (50) NOT NULL,
    [isActive]                     BIT          NOT NULL,
    CONSTRAINT [PK_CH_FollowupActionTime] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

