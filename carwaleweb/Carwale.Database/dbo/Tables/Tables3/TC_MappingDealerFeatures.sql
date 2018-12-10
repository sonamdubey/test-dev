CREATE TABLE [dbo].[TC_MappingDealerFeatures] (
    [ID]                 INT IDENTITY (1, 1) NOT NULL,
    [BranchId]           INT NULL,
    [HasOffer]           BIT NULL,
    [HasYouTube]         BIT NULL,
    [TC_DealerFeatureId] INT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_TC_MappingDealerFeatures_BranchId]
    ON [dbo].[TC_MappingDealerFeatures]([BranchId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_MappingDealerFeatures_TC_DealerFeatureId]
    ON [dbo].[TC_MappingDealerFeatures]([BranchId] ASC, [TC_DealerFeatureId] ASC);

