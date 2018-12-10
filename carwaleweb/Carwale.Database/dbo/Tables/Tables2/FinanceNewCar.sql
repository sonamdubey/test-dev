CREATE TABLE [dbo].[FinanceNewCar] (
    [Id]                   NUMERIC (10) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FinancerId]           INT          NOT NULL,
    [ModelId]              INT          NOT NULL,
    [UserType]             INT          NULL,
    [MaxAmountRs]          NUMERIC (18) NULL,
    [MaxAmountPer]         FLOAT (53)   NULL,
    [ProcessingChargesPer] FLOAT (53)   NULL,
    [ProcessingChargesMin] INT          NULL,
    [ProcessingChargesMax] INT          NULL,
    [isActive]             BIT          NOT NULL,
    CONSTRAINT [PK_FinanceUsedCar] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

