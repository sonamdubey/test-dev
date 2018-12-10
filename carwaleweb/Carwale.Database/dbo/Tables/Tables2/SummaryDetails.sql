CREATE TABLE [dbo].[SummaryDetails] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SummaryHeadId]  NUMERIC (18) NOT NULL,
    [SummaryValue]   NUMERIC (18) NOT NULL,
    [MonthYear]      DATETIME     NOT NULL,
    [SummaryOfMonth] DATETIME     NOT NULL,
    [isOriginal]     BIT          CONSTRAINT [DF_SummaryDetails_isOriginal] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_SummaryDetails] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

