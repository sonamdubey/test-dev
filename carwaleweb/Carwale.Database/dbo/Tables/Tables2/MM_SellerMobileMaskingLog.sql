CREATE TABLE [dbo].[MM_SellerMobileMaskingLog] (
    [MM_SellerMobileMaskingId] INT           NULL,
    [ConsumerId]               NUMERIC (18)  NULL,
    [ConsumerType]             TINYINT       NULL,
    [MaskingNumber]            VARCHAR (20)  NULL,
    [Mobile]                   VARCHAR (35)  NULL,
    [ProductTypeId]            SMALLINT      NULL,
    [NCDBrandId]               INT           NULL,
    [ActionTakenOn]            DATETIME      NULL,
    [ActionTakenBy]            INT           NULL,
    [Remarks]                  VARCHAR (200) NULL,
    [DealerType]               INT           NULL,
    [ServiceProvider]          INT           DEFAULT ((1)) NULL
);

