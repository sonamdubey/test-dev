CREATE TABLE [dbo].[PageMetaTags] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [PageId]      NUMERIC (18)  NOT NULL,
    [MakeId]      INT           NULL,
    [ModelId]     INT           NULL,
    [Title]       VARCHAR (200) NULL,
    [Description] VARCHAR (500) NULL,
    [Keywords]    VARCHAR (500) NULL,
    [Heading]     VARCHAR (200) NULL,
    [IsActive]    BIT           NULL,
    [Summary]     VARCHAR (500) NULL
);

