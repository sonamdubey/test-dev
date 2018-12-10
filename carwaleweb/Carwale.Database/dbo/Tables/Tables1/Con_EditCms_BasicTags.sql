CREATE TABLE [dbo].[Con_EditCms_BasicTags] (
    [BasicId]      NUMERIC (18) NOT NULL,
    [TagId]        NUMERIC (18) NOT NULL,
    [BWOldBasicId] INT          NULL,
    [BWOldTagId]   INT          NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_Con_EditCms_BasicTags__TagId]
    ON [dbo].[Con_EditCms_BasicTags]([TagId] ASC)
    INCLUDE([BasicId]);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_BasicTags]
    ON [dbo].[Con_EditCms_BasicTags]([BasicId] ASC, [TagId] ASC);

