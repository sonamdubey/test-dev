CREATE TABLE [dbo].[Con_EditCms_Category] (
    [Id]                  NUMERIC (18)  IDENTITY (8, 1) NOT NULL,
    [Name]                VARCHAR (50)  NULL,
    [AllowCarSelection]   BIT           NULL,
    [MinCarSelection]     SMALLINT      NULL,
    [MaxCarSelection]     SMALLINT      CONSTRAINT [DF__Con_EditC__MaxCa__5D784BAE] DEFAULT ((1)) NOT NULL,
    [CreateAlbum]         BIT           NULL,
    [AddSpecification]    BIT           CONSTRAINT [DF_Con_EditCms_Category_AddSpecification] DEFAULT ((0)) NULL,
    [VersionSelection]    BIT           CONSTRAINT [DF__Con_EditC__Versi__5E6C6FE7] DEFAULT ((0)) NOT NULL,
    [IsActive]            BIT           CONSTRAINT [DF_Con_EditCms_Category_IsActive] DEFAULT ((1)) NULL,
    [ForumCategoryId]     INT           NULL,
    [EntryDate]           DATETIME      CONSTRAINT [DF_Con_EditCms_Category_EntryDate] DEFAULT (getdate()) NULL,
    [LastUpdatedTime]     DATETIME      NULL,
    [LastUpdatedBy]       NUMERIC (18)  NULL,
    [IsSinglePage]        BIT           CONSTRAINT [DF_Con_EditCms_Category_IsSinglePage] DEFAULT ((0)) NOT NULL,
    [BWMigratedId]        INT           NULL,
    [CategoryMaskingName] VARCHAR (150) NULL,
    [DisplayName]         VARCHAR (150) NULL,
    CONSTRAINT [PK_Con_EditCms_Category] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

