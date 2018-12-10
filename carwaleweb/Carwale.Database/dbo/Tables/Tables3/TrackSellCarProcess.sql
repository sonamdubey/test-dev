CREATE TABLE [dbo].[TrackSellCarProcess] (
    [ID]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SessionId]     VARCHAR (500) NULL,
    [PageName]      VARCHAR (500) NULL,
    [CustomerId]    NUMERIC (18)  NOT NULL,
    [EntryDateTime] DATETIME      NOT NULL,
    CONSTRAINT [PK_TrackSellCarProcess] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

