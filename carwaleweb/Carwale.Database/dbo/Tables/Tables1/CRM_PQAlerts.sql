CREATE TABLE [dbo].[CRM_PQAlerts] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [LeadId]    NUMERIC (18) NOT NULL,
    [CBDId]     NUMERIC (18) NOT NULL,
    [VersionId] NUMERIC (18) NOT NULL,
    [AlertDate] DATETIME     CONSTRAINT [DF_CRM_PQAlerts_AlertDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CRM_PQAlerts] PRIMARY KEY CLUSTERED ([Id] ASC)
);

