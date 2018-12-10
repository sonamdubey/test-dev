CREATE TABLE [dbo].[DealerGACodes] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]     NUMERIC (18) NOT NULL,
    [Code]         VARCHAR (50) NOT NULL,
    [IsActive]     BIT          NOT NULL,
    [EntryDate]    DATETIME     NULL,
    [ModifiedDate] DATETIME     NULL
);

