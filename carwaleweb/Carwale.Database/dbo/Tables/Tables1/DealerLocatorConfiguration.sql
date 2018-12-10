CREATE TABLE [dbo].[DealerLocatorConfiguration] (
    [DealerLocatorConfigurationId] INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]                     INT      NOT NULL,
    [PQ_DealerSponsoredId]         INT      NULL,
    [IsDealerLocatorPremium]       BIT      NULL,
    [IsLocatorActive]              BIT      CONSTRAINT [DF_dealerlocatorconfiguration_IsLocatorActive] DEFAULT ((1)) NULL,
    [CreatedOn]                    DATETIME CONSTRAINT [DF_dealerlocatorconfiguration_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                    INT      NOT NULL,
    [LastUpdatedOn]                DATETIME CONSTRAINT [DF_dealerlocatorconfiguration_LastUpdatedOn] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedBy]                INT      NOT NULL,
    CONSTRAINT [PK_DealerLocatorConfiguration] PRIMARY KEY CLUSTERED ([DealerLocatorConfigurationId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_DealerLocatorConfiguration_DealerId]
    ON [dbo].[DealerLocatorConfiguration]([DealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DealerLocatorConfiguration_PQ_DealerSponsoredId]
    ON [dbo].[DealerLocatorConfiguration]([PQ_DealerSponsoredId] ASC);

