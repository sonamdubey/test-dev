CREATE TABLE [dbo].[CW_NewCarShowroomPricesRevert23052016] (
    [Id]                   NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CarVersionId]         NUMERIC (18) NOT NULL,
    [CityId]               NUMERIC (18) NOT NULL,
    [PQ_CategoryItem]      NUMERIC (18) NOT NULL,
    [PQ_CategoryItemValue] FLOAT (53)   NULL,
    [LastUpdated]          DATETIME     NULL,
    [OnRoadPriceInd]       BIT          DEFAULT ((1)) NOT NULL,
    [isMetallic]           BIT          DEFAULT ((0)) NOT NULL
);


GO
CREATE CLUSTERED INDEX [IX_CW_NewCarShowroomPrices_Id]
    ON [dbo].[CW_NewCarShowroomPricesRevert23052016]([Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PQ_CategoryItem]
    ON [dbo].[CW_NewCarShowroomPricesRevert23052016]([PQ_CategoryItem] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CW_NewCarShowroomPricesCVCityId]
    ON [dbo].[CW_NewCarShowroomPricesRevert23052016]([CarVersionId] ASC, [CityId] ASC);

