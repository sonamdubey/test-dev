CREATE TABLE [dbo].[TemplateGroupMapping] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [GroupId]    INT NOT NULL,
    [TemplateId] INT NOT NULL,
    CONSTRAINT [PK_Group_TemplateMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

