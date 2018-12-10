CREATE TABLE [dbo].[SkodaNavigationLinksBkp] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [ModelId]        INT            NULL,
    [NavigationText] NVARCHAR (500) NOT NULL,
    [NavigationLink] NVARCHAR (500) NOT NULL,
    [isActive]       BIT            NULL,
    [entryDateTime]  DATETIME       NULL,
    [DealerId]       INT            NULL
);

