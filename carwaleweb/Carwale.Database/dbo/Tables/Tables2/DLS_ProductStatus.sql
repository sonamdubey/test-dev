CREATE TABLE [dbo].[DLS_ProductStatus] (
    [Id]       NUMERIC (18) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsClosed] BIT          NOT NULL,
    CONSTRAINT [PK_DLS_ProductStatus] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

