CREATE TABLE [dbo].[CRM_BlockList] (
    [CRM_BlockListId] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [VersionId]       NUMERIC (18) NOT NULL,
    [BlockedDate]     DATETIME     NULL,
    [BlockedBy]       INT          NULL,
    [CityId]          INT          CONSTRAINT [DF_CRM_BlockList_CityId] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_CRM_BlockList] PRIMARY KEY CLUSTERED ([CRM_BlockListId] ASC)
);

