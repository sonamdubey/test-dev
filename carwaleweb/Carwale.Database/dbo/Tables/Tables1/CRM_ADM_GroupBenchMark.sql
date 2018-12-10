CREATE TABLE [dbo].[CRM_ADM_GroupBenchMark] (
    [Id]        NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [GroupId]   INT             NULL,
    [BenchMark] NUMERIC (18, 2) NULL,
    [CreatedBy] NUMERIC (18)    NULL,
    [CreatedOn] DATETIME        NULL,
    [MulFactor] AS              (CONVERT([decimal](10,2),(100)/[Benchmark],0)),
    [IsActive]  BIT             CONSTRAINT [DF__CRM_ADM_G__IsAct__7400825D] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_CRM_ADM_GroupBenchMark] PRIMARY KEY CLUSTERED ([Id] ASC)
);

