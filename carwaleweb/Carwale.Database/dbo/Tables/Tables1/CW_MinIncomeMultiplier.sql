CREATE TABLE [dbo].[CW_MinIncomeMultiplier] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CW_IncomeTypeId] NUMERIC (18) NOT NULL,
    [CW_AgeGroupId]   NUMERIC (18) NULL,
    [CW_CarTierId]    NUMERIC (18) NULL,
    [MinIncome]       FLOAT (53)   NOT NULL,
    [Multiplier]      FLOAT (53)   NOT NULL,
    [IsActive]        BIT          NOT NULL,
    [ResidenceTypeId] INT          NULL,
    CONSTRAINT [PK_CW_MinIncomeMultiplier] PRIMARY KEY CLUSTERED ([Id] ASC)
);

