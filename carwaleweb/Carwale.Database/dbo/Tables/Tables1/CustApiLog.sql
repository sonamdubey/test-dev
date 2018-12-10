CREATE TABLE [dbo].[CustApiLog] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [InquiryId]          INT           NOT NULL,
    [ActionType]         TINYINT       NOT NULL,
    [IsSuccessful]       BIT           NOT NULL,
    [LastUpdateDateTime] DATETIME      NOT NULL,
    [Errors]             VARCHAR (100) NULL,
    [IsStockOrImage]     BIT           NULL,
    [StatusCode]         INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

