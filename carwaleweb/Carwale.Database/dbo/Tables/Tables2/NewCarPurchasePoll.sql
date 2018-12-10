CREATE TABLE [dbo].[NewCarPurchasePoll] (
    [ID]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VersionId]  NUMERIC (18)  NULL,
    [Price]      NUMERIC (18)  NULL,
    [MakeYear]   DATETIME      NULL,
    [City]       VARCHAR (100) NULL,
    [IpAddress]  VARCHAR (200) NULL,
    [EntryTime]  DATETIME      NOT NULL,
    [CustomerId] NUMERIC (18)  NULL,
    CONSTRAINT [PK_NewCarPoll] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

