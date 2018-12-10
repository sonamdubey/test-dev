CREATE TABLE [dbo].[PaymentModes] (
    [Id]                   NUMERIC (18) NOT NULL,
    [ModeName]             VARCHAR (50) NOT NULL,
    [DefaultEncashedValue] BIT          CONSTRAINT [DF_PaymentModes_DefaultEncashedValue] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_PaymentModes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

