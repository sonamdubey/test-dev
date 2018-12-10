CREATE TABLE [dbo].[CRM_JDLeads] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [LeadId]     VARCHAR (20)  NULL,
    [LeadType]   VARCHAR (15)  NULL,
    [Prefix]     VARCHAR (5)   NULL,
    [Name]       VARCHAR (100) NULL,
    [Mobile]     VARCHAR (15)  NULL,
    [Phone]      VARCHAR (15)  NULL,
    [Email]      VARCHAR (50)  NULL,
    [Date]       VARCHAR (30)  NULL,
    [Category]   VARCHAR (100) NULL,
    [City]       VARCHAR (50)  NULL,
    [Area]       VARCHAR (50)  NULL,
    [BranchArea] VARCHAR (100) NULL,
    [DncMobile]  VARCHAR (5)   NULL,
    [DncPhone]   VARCHAR (5)   NULL,
    [Company]    VARCHAR (100) NULL,
    [Type]       VARCHAR (30)  NULL,
    [EntryDate]  DATETIME      CONSTRAINT [DF_CRM_JDLeads_EntryDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CRM_JDLeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

