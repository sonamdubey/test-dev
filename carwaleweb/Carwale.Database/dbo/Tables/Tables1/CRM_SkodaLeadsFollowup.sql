CREATE TABLE [dbo].[CRM_SkodaLeadsFollowup] (
    [Id]                   NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [TokenNo]              VARCHAR (50)  NULL,
    [AgencyId]             VARCHAR (50)  NULL,
    [AgencyLeadId]         VARCHAR (50)  NULL,
    [CompanyId]            VARCHAR (50)  NULL,
    [DMSLeadId]            VARCHAR (50)  NULL,
    [DMSLeadOpenedOn]      VARCHAR (50)  NULL,
    [TokenRecProcessOwner] VARCHAR (200) NULL,
    [Action]               VARCHAR (50)  NULL,
    [ActionDate]           VARCHAR (50)  NULL,
    [ActionTime]           VARCHAR (50)  NULL,
    [ActionComment]        VARCHAR (500) NULL,
    [ActionClosureDate]    VARCHAR (50)  NULL,
    [ActionClosureTime]    VARCHAR (50)  NULL,
    [ActionClosureComment] VARCHAR (500) NULL,
    [CreatedOn]            DATETIME      CONSTRAINT [DF_CRM_CRM_SkodaLeadsFollowup_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CRM_SkodaLeadsFollowup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

