CREATE TABLE [dbo].[ComponentProviders] (
    [Id]           NUMERIC (18) NOT NULL,
    [FirstName]    CHAR (10)    NULL,
    [LastName]     CHAR (10)    NULL,
    [Organization] CHAR (10)    NULL,
    CONSTRAINT [PK_ComponentProviders] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

