CREATE TABLE [dbo].[DealersFinancePartner] (
    [Id]         INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]   INT NOT NULL,
    [FinancerId] INT NOT NULL,
    CONSTRAINT [PK_DealerFinancerData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

