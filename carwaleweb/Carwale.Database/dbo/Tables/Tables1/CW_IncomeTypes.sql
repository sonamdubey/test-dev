CREATE TABLE [dbo].[CW_IncomeTypes] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50) NOT NULL,
    [IsActive]      BIT          CONSTRAINT [DF_CW_IncomeTypes_IsActive] DEFAULT ((1)) NOT NULL,
    [MinExperience] INT          NULL,
    CONSTRAINT [PK_CW_IncomeTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

