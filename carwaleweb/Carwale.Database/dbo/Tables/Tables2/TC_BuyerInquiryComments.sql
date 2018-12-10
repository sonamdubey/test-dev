CREATE TABLE [dbo].[TC_BuyerInquiryComments] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [TC_BuyerInquiryId] INT           NULL,
    [EntryDate]         DATETIME      NULL,
    [Comments]          VARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

