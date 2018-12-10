CREATE TABLE [dbo].[NCS_TCDealerLog] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [TCDealerId] NUMERIC (18) NOT NULL,
    [NCDId]      NUMERIC (18) NOT NULL,
    [UpdatedBy]  INT          NULL,
    [UpdatedOn]  DATETIME     NULL,
    CONSTRAINT [PK_NCS_TCDealerLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

