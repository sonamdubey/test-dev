CREATE TABLE [dbo].[TemplateGroupMapping_Log] (
    [TemplateGroupMappingId] INT      NOT NULL,
    [GroupId]                INT      NOT NULL,
    [TemplateId]             INT      NOT NULL,
    [UpdatedOn]              DATETIME NOT NULL,
    [UpdatedBy]              INT      NOT NULL,
    [UpdateType]             CHAR (1) NOT NULL
);

