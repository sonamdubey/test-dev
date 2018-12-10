CREATE TABLE [dbo].[FinanceInterestRates] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarModelId]     NUMERIC (18) NOT NULL,
    [FinancerId]     INT          NOT NULL,
    [UserType]       INT          NOT NULL,
    [InterestRate]   FLOAT (53)   NOT NULL,
    [IsActive]       BIT          CONSTRAINT [DF_FinanceInterestRates_IsActive] DEFAULT (1) NOT NULL,
    [Tenure]         INT          NOT NULL,
    [CustomerPayout] FLOAT (53)   NOT NULL,
    [isUsed]         BIT          NOT NULL,
    CONSTRAINT [PK_FinanceInterestRates] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

