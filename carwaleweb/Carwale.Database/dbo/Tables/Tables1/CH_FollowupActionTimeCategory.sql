CREATE TABLE [dbo].[CH_FollowupActionTimeCategory] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [TimeCategory] VARCHAR (50) NOT NULL,
    [isActive]     BIT          NOT NULL,
    CONSTRAINT [PK_CH_FollowupTimeCategory] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

