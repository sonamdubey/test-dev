CREATE TABLE [dbo].[PriceLog_ItemValues] (
    [ID]                   NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [VersionId]            NUMERIC (18) NOT NULL,
    [CityId]               NUMERIC (18) NOT NULL,
    [PQ_CategoryItem]      INT          NULL,
    [PQ_CategoryItemValue] NUMERIC (18) NULL,
    [UpdatedOn]            DATETIME     NOT NULL,
    [IsMetallic]           BIT          DEFAULT ((0)) NULL,
    [UpdatedBy]            INT          NULL,
    [SetID]                VARCHAR (50) NULL,
    [IsModified]           BIT          DEFAULT ((0)) NULL,
    CONSTRAINT [PK_PriceLog_ItemValues] PRIMARY KEY CLUSTERED ([ID] ASC)
);

