CREATE TABLE [dbo].[CRM_ADM_FLCGroups] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (250)   NOT NULL,
    [IsActive]       BIT             CONSTRAINT [DF_CRM_ADM_FLCGroups_IsActive] DEFAULT ((1)) NOT NULL,
    [GroupType]      SMALLINT        NULL,
    [LeadScoreLimit] DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_CRM_ADM_FLCGroups] PRIMARY KEY CLUSTERED ([Id] ASC)
);

