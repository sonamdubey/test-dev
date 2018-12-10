CREATE TABLE [dbo].[DLS_EventLog] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [EventSubId] NUMERIC (18) NOT NULL,
    [CBDId]      NUMERIC (18) NOT NULL,
    [EventOn]    DATETIME     NOT NULL,
    [EventBy]    NUMERIC (18) NOT NULL,
    [IsDealer]   BIT          NOT NULL,
    CONSTRAINT [PK_DLS_EventLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

