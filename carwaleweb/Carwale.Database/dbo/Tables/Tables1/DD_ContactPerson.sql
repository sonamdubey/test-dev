CREATE TABLE [dbo].[DD_ContactPerson] (
    [Id]                NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DD_OutletId]       INT          NOT NULL,
    [Salutation]        VARCHAR (10) NOT NULL,
    [FirstName]         VARCHAR (50) NOT NULL,
    [LastName]          VARCHAR (50) NOT NULL,
    [DD_DesignationsId] INT          NULL,
    [EmailId]           VARCHAR (50) NULL,
    [CreatedBy]         NUMERIC (18) NOT NULL,
    [CreatedOn]         DATETIME     NOT NULL,
    CONSTRAINT [PK_DD_ContactPerson] PRIMARY KEY CLUSTERED ([Id] ASC)
);

