CREATE TABLE [dbo].[PriceLog_ItemValues_Arch_29042016] (
    [ID]                   NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [VersionId]            NUMERIC (18) NOT NULL,
    [CityId]               NUMERIC (18) NOT NULL,
    [PQ_CategoryItem]      INT          NULL,
    [PQ_CategoryItemValue] NUMERIC (18) NULL,
    [UpdatedOn]            DATETIME     NOT NULL
);

