CREATE TABLE [dbo].[CRM_ContactPointType] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (20) NOT NULL,
    [IsActive] BIT          NOT NULL,
    CONSTRAINT [PK_CRM_ContactPointType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

