CREATE TABLE [dbo].[PQ_DealerSponsoredAd_ViewLogs] (
    [PQId]       NUMERIC (18) NOT NULL,
    [CampaignId] NUMERIC (18) NOT NULL,
    [PlatformId] INT          NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_PQ_DealerSponsoredAd_ViewLogs_0616]
    ON [dbo].[PQ_DealerSponsoredAd_ViewLogs]([PQId] ASC);

