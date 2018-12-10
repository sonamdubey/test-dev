CREATE TABLE [dbo].[InterestRates] (
    [ID]             INT        NOT NULL,
    [CarModelId]     INT        NULL,
    [FinancerId]     INT        NULL,
    [InterestRate]   FLOAT (53) NULL,
    [IsActive]       BIT        NOT NULL,
    [Tenure]         INT        NULL,
    [CustomerPayout] FLOAT (53) NULL,
    CONSTRAINT [PK_InterestRates] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

