CREATE TABLE [AC].[SRC_KeywordType] (
    [Id]     TINYINT      IDENTITY (1, 1) NOT NULL,
    [Type]   VARCHAR (50) NULL,
    [TypeId] TINYINT      NULL,
    CONSTRAINT [PK_SRC_KeywordType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

