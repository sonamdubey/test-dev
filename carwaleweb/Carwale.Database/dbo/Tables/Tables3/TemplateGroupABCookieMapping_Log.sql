CREATE TABLE [dbo].[TemplateGroupABCookieMapping_Log] (
    [TemplateGroupABCookieMappingId] INT      NOT NULL,
    [TemplateGroupMappingId]         INT      NOT NULL,
    [ABCookieValue]                  INT      NOT NULL,
    [UpdatedOn]                      DATETIME NOT NULL,
    [UpdatedBy]                      INT      NOT NULL,
    [UpdateType]                     CHAR (1) NOT NULL
);

