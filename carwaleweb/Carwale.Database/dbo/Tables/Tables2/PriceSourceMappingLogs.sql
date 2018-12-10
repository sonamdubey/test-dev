CREATE TABLE [dbo].[PriceSourceMappingLogs] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [PriceSourceMappingId] INT           NOT NULL,
    [PriceSourceId]        INT           NOT NULL,
    [MakeId]               INT           NOT NULL,
    [StateId]              INT           NOT NULL,
    [CityId]               INT           NOT NULL,
    [DealerId]             INT           NULL,
    [Remarks]              VARCHAR (200) NOT NULL,
    [UpdatedBy]            INT           NOT NULL,
    [UpdatedOn]            DATETIME      NOT NULL,
    CONSTRAINT [PK_PriceSourceMappingLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

