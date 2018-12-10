CREATE TABLE [dbo].[NCS_LeadCampaignLog] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [NCSDealerId]    NUMERIC (18) NOT NULL,
    [CampaignId]     NUMERIC (18) NOT NULL,
    [DelLeads]       BIGINT       NOT NULL,
    [ProjectedLeads] BIGINT       NOT NULL,
    [CreateDate]     DATETIME     NOT NULL,
    [UpdatedBy]      INT          NOT NULL,
    CONSTRAINT [PK_NCS_LeadCampaignLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

