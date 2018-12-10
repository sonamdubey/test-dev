CREATE TABLE [dbo].[DealerWebsite_SkodaNavigationLinkMaster] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [ModelId]         INT            NULL,
    [NavigationText]  NVARCHAR (500) NOT NULL,
    [NavigationLink]  NVARCHAR (500) NOT NULL,
    [NavigationOrder] INT            NULL,
    [isActive]        BIT            NULL,
    [NavigationId]    INT            NULL
);

