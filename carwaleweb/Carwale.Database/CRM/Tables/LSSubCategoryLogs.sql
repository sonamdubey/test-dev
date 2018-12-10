CREATE TABLE [CRM].[LSSubCategoryLogs] (
    [LSSubCategoryLog] INT          IDENTITY (1, 1) NOT NULL,
    [SubCategoryId]    INT          NOT NULL,
    [Value]            FLOAT (53)   NOT NULL,
    [UpdatedOn]        DATETIME     NOT NULL,
    [UpdatedBy]        INT          NOT NULL,
    [MakeId]           INT          NULL,
    [PriceFrom]        NUMERIC (18) NULL,
    [PriceTo]          NUMERIC (18) NULL,
    [SourceId]         INT          NULL,
    [TierId]           INT          NULL
);

