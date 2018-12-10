CREATE TABLE [dbo].[CRM_ADM_LogGroupBenchMark] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [GroupId]   INT             NULL,
    [BenchMark] NUMERIC (18, 2) NULL,
    [DeletedBy] INT             NULL,
    [DeletedOn] DATETIME        NULL,
    CONSTRAINT [PK_CRM_ADM_LogGroupBenchMark] PRIMARY KEY CLUSTERED ([Id] ASC)
);

