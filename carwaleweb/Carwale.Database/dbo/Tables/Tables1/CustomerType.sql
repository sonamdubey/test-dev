CREATE TABLE [dbo].[CustomerType] (
    [Id]       NUMERIC (18) NOT NULL,
    [TypeName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CustomerType] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

