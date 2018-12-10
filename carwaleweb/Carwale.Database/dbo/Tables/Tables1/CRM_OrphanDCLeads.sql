CREATE TABLE [dbo].[CRM_OrphanDCLeads] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CBDId]    NUMERIC (18) NOT NULL,
    [DealerId] NUMERIC (18) NOT NULL,
    [DCId]     INT          NULL,
    [Type]     SMALLINT     NOT NULL,
    CONSTRAINT [PK_CRM_OrphanDCLeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

