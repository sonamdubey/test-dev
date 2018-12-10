CREATE TABLE [dbo].[TrilogyCars] (
    [VersionId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_TrilogyCars] PRIMARY KEY CLUSTERED ([VersionId] ASC) WITH (FILLFACTOR = 90)
);

