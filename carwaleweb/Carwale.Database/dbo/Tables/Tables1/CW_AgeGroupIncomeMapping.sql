CREATE TABLE [dbo].[CW_AgeGroupIncomeMapping] (
    [Id]               INT IDENTITY (1, 1) NOT NULL,
    [CW_IncomeTypesId] INT NOT NULL,
    [CW_AgeGroupId]    INT NOT NULL,
    [IsActive]         BIT CONSTRAINT [DF_CW_AgeGroupIncomeMapping_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CW_AgeGroupIncomeMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

