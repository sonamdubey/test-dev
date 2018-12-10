CREATE TABLE [dbo].[AdImpressionLog] (
    [ID]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AdId]           NUMERIC (18)  NULL,
    [AdName]         VARCHAR (500) NULL,
    [Impression]     NUMERIC (18)  NOT NULL,
    [ImpressionDate] DATETIME      NOT NULL,
    CONSTRAINT [PK_AdImpressionLog] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

