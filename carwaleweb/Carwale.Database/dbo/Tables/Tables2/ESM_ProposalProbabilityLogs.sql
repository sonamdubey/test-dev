CREATE TABLE [dbo].[ESM_ProposalProbabilityLogs] (
    [Id]                 INT          IDENTITY (1, 1) NOT NULL,
    [ESM_ProposalId]     NUMERIC (18) NULL,
    [UpdatedProbability] NUMERIC (18) NULL,
    [UpdatedBy]          NUMERIC (18) NULL,
    [UpdatedOn]          DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

