CREATE TABLE [dbo].[Con_EditCms_SubCategories] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CategoryId]      NUMERIC (18) NOT NULL,
    [Name]            VARCHAR (50) NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_Con_EditCms_SubCategories_IsActive] DEFAULT ((1)) NOT NULL,
    [BWMigratedId]    INT          NULL,
    [BWOldCategoryId] INT          NULL,
    CONSTRAINT [PK_Con_EditCms_SubCategories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

