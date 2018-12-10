CREATE TABLE [dbo].[OLM_SCMappedVersions] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [VersionId]       INT          NOT NULL,
    [MappedVersionId] INT          NOT NULL,
    CONSTRAINT [PK_OLM_SCMappedVersions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

