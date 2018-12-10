CREATE TABLE [dbo].[TC_PushLeadLog] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [TC_BuyerInquiryId] INT           NULL,
    [DealerId]          INT           NULL,
    [StockId]           INT           NULL,
    [Name]              VARCHAR (50)  NULL,
    [Email]             VARCHAR (100) NULL,
    [Mobile]            VARCHAR (15)  NULL,
    [IsSuccess]         BIT           NULL,
    [APIResponse]       VARCHAR (250) NULL,
    [CreatedOn]         DATETIME      CONSTRAINT [DF_TC_PushLeadLog_CreatedOn] DEFAULT (getdate()) NULL,
    [MFCSourceId]       INT           NULL,
    [MixMatchLead]      BIT           NULL,
    CONSTRAINT [PK_TC_PushLeadLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

