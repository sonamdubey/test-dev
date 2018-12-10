CREATE TABLE [dbo].[CRM_SkodaClosedLead] (
    [Id]                   NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [TokenNo]              VARCHAR (50)  NULL,
    [AgencyId]             VARCHAR (50)  NULL,
    [AgencyLeadId]         VARCHAR (50)  NULL,
    [TokenRecProcessOwner] VARCHAR (200) NULL,
    [CompanyId]            VARCHAR (50)  NULL,
    [DMSLeadId]            VARCHAR (50)  NULL,
    [DMSClosureDate]       VARCHAR (50)  NULL,
    [DMSClosureType]       VARCHAR (50)  NULL,
    [OrderNo]              VARCHAR (50)  NULL,
    [CreatedOn]            DATETIME      CONSTRAINT [DF_CRM_SkodaClosedLead_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CRM_SkodaClosedLead] PRIMARY KEY CLUSTERED ([Id] ASC)
);

