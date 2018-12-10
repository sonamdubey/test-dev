CREATE TABLE [dbo].[OprPageTracker] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LoginId]   NUMERIC (18)  NULL,
    [LoginTime] DATETIME      NULL,
    [Page]      VARCHAR (200) NULL,
    CONSTRAINT [PK_OprPageTracker] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

