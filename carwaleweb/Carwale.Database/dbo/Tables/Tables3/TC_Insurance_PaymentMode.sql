CREATE TABLE [dbo].[TC_Insurance_PaymentMode] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [PaymentMode] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_TC_Insurance_PaymentMode] PRIMARY KEY CLUSTERED ([Id] ASC)
);

