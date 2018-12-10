CREATE TABLE [dbo].[ChatRepository] (
    [ChatRepositoryId]  INT           IDENTITY (1, 1) NOT NULL,
    [SenderQuickBloxId] INT           NOT NULL,
    [DealerQuickBloxId] INT           NOT NULL,
    [DeviceId]          VARCHAR (100) NOT NULL,
    [ChatId]            VARCHAR (100) NOT NULL,
    [InquiryId]         VARCHAR (10)  NOT NULL,
    [SellerType]        BIT           NOT NULL,
    [SenderName]        VARCHAR (50)  NULL,
    [SenderMobile]      VARCHAR (20)  NULL,
    [SenderEmail]       VARCHAR (100) NULL,
    [CreatedOn]         DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([ChatRepositoryId] ASC)
);

