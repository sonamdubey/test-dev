CREATE TABLE [dbo].[ServiceProviderUsedCarChains] (
    [ServiceProviderUsedCarID] NUMERIC (18) NOT NULL,
    [UsedCarChainId]           NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ServiceProviderUsedCarChains] PRIMARY KEY CLUSTERED ([ServiceProviderUsedCarID] ASC, [UsedCarChainId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ServiceProviderUsedCarChains_ServiceProviderUsedCars] FOREIGN KEY ([ServiceProviderUsedCarID]) REFERENCES [dbo].[ServiceProviderUsedCars] ([ID]),
    CONSTRAINT [FK_ServiceProviderUsedCarChains_UsedCarChains] FOREIGN KEY ([UsedCarChainId]) REFERENCES [dbo].[UsedCarChains] ([ID])
);

