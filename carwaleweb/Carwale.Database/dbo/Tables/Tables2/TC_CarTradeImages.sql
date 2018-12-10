CREATE TABLE [dbo].[TC_CarTradeImages] (
    [TC_CarTradeImageId]             INT           IDENTITY (1, 1) NOT NULL,
    [TC_CarTradeImageResponseId]     INT           NULL,
    [TC_CarTradeCertificationDataId] INT           NULL,
    [ImageName]                      VARCHAR (250) NULL,
    [ImageSerialNum]                 INT           NULL,
    [ImageTag]                       VARCHAR (50)  NULL,
    [ThumbnailImage]                 VARCHAR (250) NULL,
    CONSTRAINT [PK_TC_CarTradeImages] PRIMARY KEY CLUSTERED ([TC_CarTradeImageId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CarTradeImages_TC_CarTradeCertificationDataId]
    ON [dbo].[TC_CarTradeImages]([TC_CarTradeCertificationDataId] ASC);

