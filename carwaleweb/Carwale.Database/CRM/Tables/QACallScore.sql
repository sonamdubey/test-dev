CREATE TABLE [CRM].[QACallScore] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [QACallDataId] NUMERIC (18) NOT NULL,
    [SubheadId]    NUMERIC (18) NOT NULL,
    [Score]        FLOAT (53)   NULL,
    CONSTRAINT [PK_QACallScore] PRIMARY KEY CLUSTERED ([Id] ASC)
);

