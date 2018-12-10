CREATE TABLE [dbo].[SummaryHead] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SummaryHead]     VARCHAR (100) NOT NULL,
    [SummaryCategory] TINYINT       NULL,
    [isActive]        BIT           CONSTRAINT [DF_SummaryHead_isActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_SummaryHead] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

