CREATE TABLE [dbo].[Microsite_TransactionType] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [TransactionType] VARCHAR (10) NULL,
    [IsActive]        BIT          DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

