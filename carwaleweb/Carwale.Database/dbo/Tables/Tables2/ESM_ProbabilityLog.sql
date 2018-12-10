CREATE TABLE [dbo].[ESM_ProbabilityLog] (
    [id]                    NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [proposalId]            NUMERIC (18) NOT NULL,
    [UpdatedProbability]    NUMERIC (18) NOT NULL,
    [UpdatedBy]             NUMERIC (18) NOT NULL,
    [UpdatedOn]             DATETIME     NOT NULL,
    [ESM_ProposedProductId] NUMERIC (18) NULL,
    CONSTRAINT [PK_ESM_ProbabilityLog] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

