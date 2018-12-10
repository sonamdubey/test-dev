CREATE TABLE [dbo].[CW_NewCarShowroomPrices] (
    [Id]                   NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CarVersionId]         NUMERIC (18) NOT NULL,
    [CityId]               NUMERIC (18) NOT NULL,
    [PQ_CategoryItem]      NUMERIC (18) NOT NULL,
    [PQ_CategoryItemValue] FLOAT (53)   NULL,
    [LastUpdated]          DATETIME     NULL,
    [OnRoadPriceInd]       BIT          DEFAULT ((1)) NOT NULL,
    [isMetallic]           BIT          DEFAULT ((0)) NOT NULL,
    [UpdatedBy]            INT          NULL
);


GO
CREATE CLUSTERED INDEX [IX_CW_NewCarShowroomPrices_Id]
    ON [dbo].[CW_NewCarShowroomPrices]([Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CW_NewCarShowroomPricesCVCityId]
    ON [dbo].[CW_NewCarShowroomPrices]([CarVersionId] ASC, [CityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PQ_CategoryItem]
    ON [dbo].[CW_NewCarShowroomPrices]([PQ_CategoryItem] ASC);

