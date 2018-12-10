CREATE TABLE [dbo].[TC_NewCarPurchaseLead] (
    [CustomerName]     VARCHAR (100) NULL,
    [Mobile]           VARCHAR (20)  NULL,
    [EMail]            VARCHAR (100) NULL,
    [CarVersion]       VARCHAR (50)  NULL,
    [CarModel]         VARCHAR (50)  NULL,
    [CarMake]          VARCHAR (50)  NULL,
    [BuyTime]          VARCHAR (50)  NULL,
    [ReqDate]          DATETIME      NULL,
    [MakeId]           INT           NULL,
    [ModelId]          INT           NULL,
    [VersionId]        INT           NULL,
    [CityId]           BIGINT        NULL,
    [NewCarPurchaseId] BIGINT        NULL,
    [CWCustomerId]     BIGINT        NULL,
    [RowNo]            INT           NULL
);

