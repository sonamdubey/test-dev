CREATE TABLE [dbo].[LA_Status] (
    [Id]          SMALLINT      NOT NULL,
    [Value]       VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_LA_Status] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

