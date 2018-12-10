CREATE TABLE [dbo].[TC_DealerFinancer] (
    [Id]         BIGINT       IDENTITY (1, 1) NOT NULL,
    [DealerId]   NUMERIC (18) NOT NULL,
    [FinancerId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_TC_DealerFinancer] PRIMARY KEY CLUSTERED ([Id] ASC)
);

