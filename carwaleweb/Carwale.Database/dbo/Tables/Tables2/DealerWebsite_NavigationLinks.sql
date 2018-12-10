CREATE TABLE [dbo].[DealerWebsite_NavigationLinks] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [ModelId]         INT            NULL,
    [NavigationText]  NVARCHAR (500) NOT NULL,
    [NavigationLink]  NVARCHAR (500) NOT NULL,
    [isActive]        BIT            NULL,
    [entryDateTime]   DATETIME       NULL,
    [DealerId]        INT            NULL,
    [NavigationOrder] INT            NULL,
    [NavigationId]    INT            NULL,
    [TargetType]      VARCHAR (30)   DEFAULT ('_self') NOT NULL,
    CONSTRAINT [PK_SkodaNavigationLinks] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_NavigationLinks_NaviagtionPagesMap] FOREIGN KEY ([NavigationId]) REFERENCES [dbo].[DealerWebsite_NaviagtionPagesMap] ([ID])
);

