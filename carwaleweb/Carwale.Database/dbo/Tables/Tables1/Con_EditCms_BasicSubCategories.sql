CREATE TABLE [dbo].[Con_EditCms_BasicSubCategories] (
    [BasicId]            NUMERIC (18) NOT NULL,
    [SubCategoryId]      NUMERIC (18) NOT NULL,
    [BWOldBasicId]       INT          NULL,
    [BWOldSubCategoryId] INT          NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_BasicSubCategories]
    ON [dbo].[Con_EditCms_BasicSubCategories]([BasicId] ASC, [SubCategoryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_BasicSubCategories_BasicId]
    ON [dbo].[Con_EditCms_BasicSubCategories]([BasicId] ASC);

