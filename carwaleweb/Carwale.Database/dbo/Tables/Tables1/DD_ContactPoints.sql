CREATE TABLE [dbo].[DD_ContactPoints] (
    [Id]                 NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ContactNumber]      VARCHAR (50) NOT NULL,
    [ContactType]        SMALLINT     NOT NULL,
    [IsPrimary]          BIT          NOT NULL,
    [DD_DealerNamesId]   INT          NULL,
    [CreatedBy]          NUMERIC (18) NOT NULL,
    [CreatedOn]          DATETIME     NOT NULL,
    [DD_DealerOutletId]  INT          NULL,
    [DD_ContactPersonId] INT          NULL,
    CONSTRAINT [PK_DD_ContactPoints] PRIMARY KEY CLUSTERED ([Id] ASC)
);

