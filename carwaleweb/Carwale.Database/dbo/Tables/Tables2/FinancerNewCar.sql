CREATE TABLE [dbo].[FinancerNewCar] (
    [Id]           INT          NOT NULL,
    [FinancerId]   INT          NOT NULL,
    [ModelId]      INT          NOT NULL,
    [NotOlderThen] FLOAT (53)   NULL,
    [MaxAmountPer] FLOAT (53)   NULL,
    [MaxAmountRs]  DECIMAL (18) NULL,
    [isActive]     BIT          NOT NULL,
    CONSTRAINT [PK_FinancerNewCar] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

