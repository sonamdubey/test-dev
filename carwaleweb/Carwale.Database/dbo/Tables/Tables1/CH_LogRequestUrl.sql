CREATE TABLE [dbo].[CH_LogRequestUrl] (
    [CHL_Id]          NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [RequestCallId]   NUMERIC (18)  NOT NULL,
    [RequestUrl]      VARCHAR (100) NOT NULL,
    [RequestDateTime] DATETIME      NOT NULL,
    CONSTRAINT [PK_CH_LogRequestUrl] PRIMARY KEY CLUSTERED ([CHL_Id] ASC) WITH (FILLFACTOR = 90)
);

