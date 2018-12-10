CREATE TABLE [dbo].[DCRM_SalesLog] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]   NUMERIC (18) NOT NULL,
    [BOExec]     NUMERIC (18) NULL,
    [SFieldExec] NUMERIC (18) NOT NULL,
    [UpdatedBy]  NUMERIC (18) NULL,
    [UpdatedOn]  DATETIME     NULL,
    CONSTRAINT [PK_DCRM_SalesLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

