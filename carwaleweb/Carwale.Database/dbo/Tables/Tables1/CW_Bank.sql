CREATE TABLE [dbo].[CW_Bank] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [BankType] VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_CW_Bank_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CW_Bank] PRIMARY KEY CLUSTERED ([Id] ASC)
);

