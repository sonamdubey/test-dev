CREATE TABLE [dbo].[UCS_SearchResult] (
    [ID]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SCId]         NUMERIC (18)  NULL,
    [SessionId]    VARCHAR (100) NULL,
    [CustomerId]   NUMERIC (18)  NULL,
    [SearchResult] VARCHAR (MAX) NULL,
    [SearchedDate] DATETIME      NULL,
    CONSTRAINT [PK_NCS_SearchResult] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

