CREATE TABLE [dbo].[AffiliateMembersSites] (
    [ID]                 NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AffiliateMembersId] NUMERIC (18)   NOT NULL,
    [SiteName]           VARCHAR (100)  NULL,
    [SiteUrl]            VARCHAR (100)  NULL,
    [SiteDescription]    VARCHAR (1000) NULL,
    [SiteCategory]       VARCHAR (100)  NULL,
    [MembershipCode]     VARCHAR (50)   NULL,
    [IsActive]           BIT            CONSTRAINT [DF_AffiliateMembersSites_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_AffiliateMembersSites] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

