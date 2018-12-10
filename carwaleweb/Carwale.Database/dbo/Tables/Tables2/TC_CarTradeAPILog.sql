CREATE TABLE [dbo].[TC_CarTradeAPILog] (
    [TC_CarTradeAPILogId] INT           IDENTITY (1, 1) NOT NULL,
    [RequestURL]          VARCHAR (MAX) NULL,
    [RequestDate]         DATETIME      NULL,
    [RequestBody]         VARCHAR (MAX) NULL,
    [Response]            VARCHAR (MAX) NULL,
    [Status]              SMALLINT      NULL,
    [ProductId]           TINYINT       NULL,
    [ProductItemId]       INT           NULL,
    CONSTRAINT [PK_TC_CarTradeAPILog] PRIMARY KEY CLUSTERED ([TC_CarTradeAPILogId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 . Certification', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_CarTradeAPILog', @level2type = N'COLUMN', @level2name = N'ProductId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Based on Product Id i.e Ex : if ProductId 1 then ProductLogId will be PK of TC_CarTradeCertificationRequests', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_CarTradeAPILog', @level2type = N'COLUMN', @level2name = N'ProductItemId';

