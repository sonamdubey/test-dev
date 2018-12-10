CREATE TABLE [dbo].[CRM_SkodaResearchLeads] (
    [Id]           BIGINT       IDENTITY (1, 1) NOT NULL,
    [MakeId]       BIGINT       NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    [Email]        VARCHAR (50) NOT NULL,
    [Mobile]       VARCHAR (50) NOT NULL,
    [CityId]       INT          NOT NULL,
    [VersionId]    BIGINT       NOT NULL,
    [CarName]      VARCHAR (50) NOT NULL,
    [ModelId]      BIGINT       NOT NULL,
    [CWCustomerId] NUMERIC (18) NOT NULL,
    [CRM_LeadId]   BIGINT       NULL,
    [IsPushed]     BIT          CONSTRAINT [DF_CRM_SkodaResearchLeads_IsPushed] DEFAULT ((0)) NULL,
    [CreatedOn]    DATETIME     CONSTRAINT [DF_CRM_SkodaResearchLeads_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CRM_SkodaResearchLeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

