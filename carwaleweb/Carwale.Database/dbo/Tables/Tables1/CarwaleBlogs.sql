CREATE TABLE [dbo].[CarwaleBlogs] (
    [ID]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ModelId]   NUMERIC (18)  NULL,
    [VersionId] NUMERIC (18)  NULL,
    [Path]      VARCHAR (100) NULL,
    [IsActive]  BIT           NOT NULL,
    CONSTRAINT [PK_CarwaleBlogs] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

