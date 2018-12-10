CREATE TABLE [dbo].[CRM_MobDealerLeads] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (100) NOT NULL,
    [Mobile]    VARCHAR (15)  NOT NULL,
    [Email]     VARCHAR (100) NOT NULL,
    [versionid] NUMERIC (18)  NOT NULL,
    [CityId]    NUMERIC (18)  NOT NULL,
    [Source]    INT           NOT NULL,
    [DealerId]  NUMERIC (18)  NOT NULL,
    [UserId]    INT           NOT NULL,
    [CreatedOn] DATETIME      CONSTRAINT [DF_CRM_MobDealerLeads_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CRM_MobDealerLeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

