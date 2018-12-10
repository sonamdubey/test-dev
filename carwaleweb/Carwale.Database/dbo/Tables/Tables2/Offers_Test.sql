CREATE TABLE [dbo].[Offers_Test] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [TextFileName] NUMERIC (18)  NULL,
    [TextFileData] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Offers_Test] PRIMARY KEY CLUSTERED ([Id] ASC)
);

