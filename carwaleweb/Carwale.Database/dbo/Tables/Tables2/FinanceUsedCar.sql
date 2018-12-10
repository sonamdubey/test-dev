CREATE TABLE [dbo].[FinanceUsedCar] (
    [Id]                   INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FinancerId]           INT          NOT NULL,
    [ModelId]              INT          NOT NULL,
    [UserType]             INT          NOT NULL,
    [NotOlderThen]         FLOAT (53)   NULL,
    [MaxAmountPer]         FLOAT (53)   NULL,
    [MaxAmountRs]          NUMERIC (18) NULL,
    [ProcessingChargesPer] FLOAT (53)   NULL,
    [ProcessingChargesMin] INT          NULL,
    [ProcessingChargesMax] INT          NULL,
    [isActive]             BIT          NOT NULL,
    CONSTRAINT [PK_NewCarFinancer] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

