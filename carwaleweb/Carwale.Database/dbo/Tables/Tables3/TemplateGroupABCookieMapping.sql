CREATE TABLE [dbo].[TemplateGroupABCookieMapping] (
    [Id]                     INT IDENTITY (1, 1) NOT NULL,
    [TemplateGroupMappingId] INT NOT NULL,
    [ABCookieValue]          INT NOT NULL,
    CONSTRAINT [PK_Template_ABCookieMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

