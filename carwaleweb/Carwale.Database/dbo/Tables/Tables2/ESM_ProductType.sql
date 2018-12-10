CREATE TABLE [dbo].[ESM_ProductType] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [ProductType] VARCHAR (50) NULL,
    CONSTRAINT [PK_ESM_ProductType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

