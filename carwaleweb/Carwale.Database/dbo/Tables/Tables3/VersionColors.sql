CREATE TABLE [dbo].[VersionColors] (
    [ID]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarVersionID] NUMERIC (18) NOT NULL,
    [Color]        VARCHAR (50) NOT NULL,
    [Code]         VARCHAR (50) NULL,
    [HexCode]      VARCHAR (50) NULL,
    [IsActive]     BIT          CONSTRAINT [DF_VersionColors_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_VersionColours] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_VersionColors_IsActive_CarVersionID]
    ON [dbo].[VersionColors]([IsActive] ASC, [CarVersionID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VersionColors_IsActive]
    ON [dbo].[VersionColors]([IsActive] ASC)
    INCLUDE([ID], [CarVersionID], [Color]);

