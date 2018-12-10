CREATE TABLE [dbo].[FOB_VersionColours] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarVersionId] NUMERIC (18) NULL,
    [ColourId]     NUMERIC (18) NULL,
    [IsActive]     BIT          CONSTRAINT [DF_FOB_VersionColours_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_FOB_VersionColours] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

