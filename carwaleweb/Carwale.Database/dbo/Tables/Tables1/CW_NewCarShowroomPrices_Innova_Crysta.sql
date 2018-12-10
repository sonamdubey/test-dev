CREATE TABLE [dbo].[CW_NewCarShowroomPrices_Innova_Crysta] (
    [Id]                   NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CarVersionId]         NUMERIC (18) NOT NULL,
    [CityId]               NUMERIC (18) NOT NULL,
    [PQ_CategoryItem]      NUMERIC (18) NOT NULL,
    [PQ_CategoryItemValue] FLOAT (53)   NULL,
    [LastUpdated]          DATETIME     NULL,
    [OnRoadPriceInd]       BIT          NOT NULL,
    [isMetallic]           BIT          NOT NULL
);

