CREATE TABLE [dbo].[tempCon_EditCms_Category] (
    [Id]                NUMERIC (18) IDENTITY (8, 1) NOT NULL,
    [Name]              VARCHAR (50) NULL,
    [AllowCarSelection] BIT          NULL,
    [MinCarSelection]   SMALLINT     NULL,
    [MaxCarSelection]   SMALLINT     NOT NULL,
    [CreateAlbum]       BIT          NULL,
    [AddSpecification]  BIT          NULL,
    [VersionSelection]  BIT          NOT NULL,
    [IsActive]          BIT          NULL,
    [ForumCategoryId]   INT          NULL,
    [EntryDate]         DATETIME     NULL,
    [LastUpdatedTime]   DATETIME     NULL,
    [LastUpdatedBy]     NUMERIC (18) NULL,
    [IsSinglePage]      BIT          NOT NULL,
    CONSTRAINT [PK_Con_EditCms_Category1] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

