CREATE TABLE [dbo].[TemplateGroup] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (30) NOT NULL,
    [PlatformId]    INT          NOT NULL,
    [IsActive]      BIT          NOT NULL,
    [LastUpdatedOn] DATETIME     NOT NULL,
    [LastupdatedBy] INT          NOT NULL,
    CONSTRAINT [PK_TemplateGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

