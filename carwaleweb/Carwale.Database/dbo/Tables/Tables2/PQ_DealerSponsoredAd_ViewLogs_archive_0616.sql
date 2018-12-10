CREATE TABLE [dbo].[PQ_DealerSponsoredAd_ViewLogs_archive_0616] (
    [PQId]       NUMERIC (18) NOT NULL,
    [CampaignId] NUMERIC (18) NOT NULL,
    [PlatformId] INT          NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_PQ_DealerSponsoredAd_ViewLogs]
    ON [dbo].[PQ_DealerSponsoredAd_ViewLogs_archive_0616]([PQId] ASC);

